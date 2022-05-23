using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Physics;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Area.EnvironmentData
{
    [Serializable]
    public class MagicEnergy
    {
        
        private readonly IMagicEnergyConfiguration _configuration;
        private int _disturbance;
        private int _energy;

        public MagicEnergy()
        {
            _configuration = GameConfigurationManager.Current.Physics.MagicEnergyConfiguration;

            _energy = _configuration.MaxValue;
            _disturbance = 0;
        }

        public int MaxEnergy => _configuration.MaxValue;

        public int Energy
        {
            get => _energy;
            set
            {
                var clearValue = Math.Max(0, value);
                _energy = Math.Min(CurrentMaxEnergy, clearValue);
            }
        }

        public int Disturbance
        {
            get => _disturbance;
            set
            {
                value = Math.Max(0, value);
                _disturbance = Math.Min(_configuration.MaxValue, value);
            }
        }

        private int CurrentMaxEnergy => _configuration.MaxValue - Disturbance;

        public void Normalize()
        {
            if (Energy > _configuration.DisturbanceStartLevel)
            {
                Energy = Energy + _configuration.RegenerationValue;
                return;
            }

            Disturbance = Math.Min(MaxEnergy, Disturbance + _configuration.DisturbanceIncrement);
            Energy = Energy; // Refresh energy value.
        }

        public void Balance(MagicEnergy other)
        {
            if (other.Energy == other.CurrentMaxEnergy && Energy == CurrentMaxEnergy)
                return;

            var thisDiff = CurrentMaxEnergy - Energy;
            var otherDiff = other.CurrentMaxEnergy - other.Energy;

            if (thisDiff >= otherDiff)
            {
                var maxTransferValue = Math.Min(thisDiff, other.Energy);
                var transferValue = Math.Min(_configuration.MaxTransferValue, maxTransferValue);

                Energy += transferValue;
                other.Energy -= transferValue;
                return;
            }

            {
                var maxTransferValue = Math.Min(otherDiff, Energy);
                var transferValue = Math.Min(_configuration.MaxTransferValue, maxTransferValue);
                Energy -= transferValue;
                other.Energy += transferValue;
            }
        }

        public void ApplyMagicEnvironment(IDestroyableObject destroyable, Point position)
        {
            if (Disturbance > _configuration.DisturbanceDamageStartLevel)
            {
                destroyable.Statuses.Add(new ManaDisturbedObjectStatus());
            }
        }
    }
}