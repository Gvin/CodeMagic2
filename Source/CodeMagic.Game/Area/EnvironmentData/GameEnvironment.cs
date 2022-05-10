using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Saving;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.Area.EnvironmentData
{
    [Serializable]
    public class GameEnvironment : IGameEnvironment
    {
        private const string SaveKeyTemperature = "Temperature";
        private const string SaveKeyPressure = "Pressure";
        private const string SaveKeyMagicEnergy = "MagicEnergy";

        private const double TemperatureToPressureMultiplier = 0.6d;

        public GameEnvironment()
        {
            Temperature = new Temperature();
            Pressure = new Pressure();
            MagicEnergy = new MagicEnergy();
        }

        public Temperature Temperature { get; set; }

        public Pressure Pressure { get; set; }

        public MagicEnergy MagicEnergy { get; set; }

        public SaveDataBuilder GetSaveData()
        {
            return new SaveDataBuilder(GetType(), new Dictionary<string, object>
            {
                {SaveKeyTemperature, Temperature},
                {SaveKeyPressure, Pressure},
                {SaveKeyMagicEnergy, MagicEnergy}
            });
        }

        int IGameEnvironment.Temperature
        {
            get => Temperature.Value;
            set
            {
                var oldValue = Temperature.Value;
                Temperature.Value = value;
                var diff = Temperature.Value - oldValue;
                var pressureDiff = (int) Math.Round(diff * TemperatureToPressureMultiplier);
                This.Pressure += pressureDiff;
            }
        }

        int IGameEnvironment.MagicEnergyLevel
        {
            get => MagicEnergy.Energy;
            set => MagicEnergy.Energy = value;
        }

        int IGameEnvironment.MaxMagicEnergyLevel => MagicEnergy.MaxEnergy;

        int IGameEnvironment.MagicDisturbanceLevel
        {
            get => MagicEnergy.Disturbance;
            set => MagicEnergy.Disturbance = value;
        }

        int IGameEnvironment.Pressure
        {
            get => Pressure.Value;
            set => Pressure.Value = value;
        }

        private IGameEnvironment This => this;

        public void Update(Point position, IAreaMapCell cell)
        {
            CheckFuelObjects(position, cell);

            foreach (var destroyableObject in cell.Objects.OfType<IDestroyableObject>())
            {
                ApplyEnvironment(destroyableObject, position);
            }

            Normalize();

            if (Temperature.Value >= FireObject.SmallFireTemperature && !cell.Objects.OfType<IFireObject>().Any())
            {
                CurrentGame.Map.AddObject(position, FireObject.Create(Temperature.Value));
            }
        }

        private void Normalize()
        {
            Temperature.Normalize();
            Pressure.Normalize();
            MagicEnergy.Normalize();
        }

        private void CheckFuelObjects(Point position, IAreaMapCell cell)
        {
            var fuelObjects = cell.Objects
                .OfType<IFuelObject>()
                .Where(obj => obj.CanIgnite && This.Temperature >= obj.IgnitionTemperature)
                .ToArray();
            if (fuelObjects.Length == 0)
                return;

            var maxTemperature = fuelObjects.Max(obj => obj.BurnTemperature);
            This.Temperature = maxTemperature;
            foreach (var fuelObject in fuelObjects)
            {
                fuelObject.FuelLeft--;
                if (fuelObject.FuelLeft <= 0)
                {
                    CurrentGame.Map.RemoveObject(position, fuelObject);
                }
            }
        }

        private void ApplyEnvironment(IDestroyableObject destroyable, Point position)
        {
            var temperatureDamage = Temperature.GetTemperatureDamage(out var temperDamageElement);
            var pressureDamage = Pressure.GetPressureDamage();

            if (temperatureDamage > 0 && temperDamageElement.HasValue)
            {
                CurrentGame.Journal.Write(new EnvironmentDamageMessage(destroyable, temperatureDamage, temperDamageElement.Value), destroyable);
                destroyable.Damage(position, temperatureDamage, temperDamageElement.Value);
            }

            if (pressureDamage > 0)
            {
                CurrentGame.Journal.Write(new EnvironmentDamageMessage(destroyable, pressureDamage, Element.Blunt), destroyable);
                destroyable.Damage(position, pressureDamage, Element.Blunt);
            }

            MagicEnergy.ApplyMagicEnvironment(destroyable, position);
        }

        public void Balance(IAreaMapCell cell, IAreaMapCell otherCell)
        {
            var otherEnvironment = (GameEnvironment)otherCell.Environment;

            Temperature.Balance(otherEnvironment.Temperature);
            Pressure.Balance(otherEnvironment.Pressure);
            MagicEnergy.Balance(otherEnvironment.MagicEnergy);

            CheckFireSpread(cell, otherCell);
        }

        private void CheckFireSpread(IAreaMapCell cell, IAreaMapCell otherCell)
        {
            var localEnvironment = (IGameEnvironment) cell.Environment;
            var otherEnvironment = (IGameEnvironment) otherCell.Environment;

            var localIgnitable = cell.Objects.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);
            var otherIgnitable = otherCell.Objects.OfType<IFireSpreadingObject>().FirstOrDefault(obj => obj.SpreadsFire);

            if (localIgnitable == null || otherIgnitable == null)
                return;

            if (localIgnitable.GetIsOnFire(cell) && otherEnvironment.Temperature < localIgnitable.BurningTemperature)
            {
                otherEnvironment.Temperature = Math.Max(otherEnvironment.Temperature, localIgnitable.BurningTemperature);
            }

            if (otherIgnitable.GetIsOnFire(otherCell) && localEnvironment.Temperature < otherIgnitable.BurningTemperature)
            {
                localEnvironment.Temperature = Math.Max(localEnvironment.Temperature, otherIgnitable.BurningTemperature);
            }
        }
    }
}