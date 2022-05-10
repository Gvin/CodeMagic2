using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Objects.LiquidObjects;

namespace CodeMagic.Game.Objects.SteamObjects
{
    public interface ISteam : ISpreadingObject, IDynamicObject
    {
    }

    public abstract class AbstractSteam : MapObjectBase, ISteam
    {
        private ILiquidConfiguration _configuration;
        private int _volume;

        protected abstract string LiquidType { get; }

        private ILiquidConfiguration Configuration
        {
            get
            {
                if (string.IsNullOrEmpty(LiquidType))
                {
                    throw new InvalidOperationException("Liquid type not initialized");
                }

                return _configuration ??= ConfigurationManager.GetLiquidConfiguration(LiquidType);
            }
        }

        public override ObjectSize Size => ObjectSize.Huge;

        public UpdateOrder UpdateOrder => UpdateOrder.Medium;

        public override bool BlocksVisibility => Thickness >= 100;

        protected int Thickness => (int)(Volume * Configuration.Steam.ThicknessMultiplier);

        public override ZIndex ZIndex => ZIndex.Air;

        public abstract string Type { get; }

        public int Volume
        {
            get => _volume;
            set => _volume = Math.Max(0, value);
        }

        public int MaxVolumeBeforeSpread => Configuration.Steam.MaxVolumeBeforeSpread;

        public int MaxSpreadVolume => Configuration.Steam.MaxSpreadVolume;

        public abstract ISpreadingObject Separate(int volume);

        protected string GetCustomConfigurationValue(string key)
        {
            var stringValue = Configuration.CustomValues
                .FirstOrDefault(value => string.Equals(value.Key, key))?.Value;
            if (string.IsNullOrEmpty(stringValue))
                throw new ApplicationException($"Custom value {key} not found in the configuration for \"{Type}\".");

            return stringValue;
        }

        public void Update(Point position)
        {
            var cell = CurrentGame.Map.GetCell(position);
            if (Volume == 0)
            {
                CurrentGame.Map.RemoveObject(position, this);
                return;
            }

            if (cell.Temperature() < Configuration.BoilingPoint)
            {
                ProcessCondensation(position, cell);
            }

            UpdateSteam(position);
        }

        protected virtual void UpdateSteam(Point position)
        {
            // Do nothing.
        }

        private void ProcessCondensation(Point position, IAreaMapCell cell)
        {
            var missingTemperature = Configuration.BoilingPoint - cell.Temperature();
            var volumeToRaiseTemp = (int)Math.Floor(missingTemperature * Configuration.CondensationTemperatureMultiplier);
            var volumeToCondense = Math.Min(volumeToRaiseTemp, Volume);
            var heatGain = (int)Math.Floor(volumeToCondense / Configuration.CondensationTemperatureMultiplier);

            cell.Environment.Cast().Temperature += heatGain;
            Volume -= volumeToCondense;

            var liquidVolume = volumeToCondense / Configuration.EvaporationMultiplier;

            CurrentGame.Map.AddObject(position, CreateLiquid(liquidVolume));
        }

        protected abstract ILiquid CreateLiquid(int volume);

        public bool Updated { get; set; }
    }
}