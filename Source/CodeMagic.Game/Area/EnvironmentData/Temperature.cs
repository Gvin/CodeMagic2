using System;
using System.Diagnostics;
using CodeMagic.Core.Game;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Game.Area.EnvironmentData
{
    [DebuggerDisplay("{" + nameof(Value) + "} C")]
    [Serializable]
    public class Temperature
    {
        private int _value;

        private readonly ITemperatureConfiguration _configuration;

        public Temperature()
        {
            _configuration = GameConfigurationManager.Current.Physics.TemperatureConfiguration;
            _value = _configuration.NormalValue;
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value < _configuration.MinValue)
                {
                    _value = _configuration.MinValue;
                    return;
                }

                if (value > _configuration.MaxValue)
                {
                    _value = _configuration.MaxValue;
                    return;
                }

                _value = value;
            }
        }

        public void Normalize()
        {
            var difference = Math.Abs(_configuration.NormalValue - Value);
            difference = Math.Min(difference, _configuration.NormalizeSpeedInside);

            if (Value > _configuration.NormalValue)
            {
                Value -= difference;
            }

            if (Value < _configuration.NormalValue)
            {
                Value += difference;
            }
        }

        public void Balance(Temperature other)
        {
            if (Value == other.Value)
                return;

            var mediana = (int)Math.Round((Value + other.Value) / 2d);
            var difference = Math.Abs(Value - mediana);
            var transferValue = GetTemperatureTransferValue(difference);

            if (Value > other.Value)
            {
                Value -= transferValue;
                other.Value += transferValue;
            }
            else
            {
                Value += transferValue;
                other.Value -= transferValue;
            }
        }

        private int GetTemperatureTransferValue(int difference)
        {
            var result = (int)Math.Round(difference * _configuration.TransferValueToDifferenceMultiplier);
            return Math.Min(result, _configuration.MaxTransferValue);
        }

        public int GetTemperatureDamage(out Element? damageElement)
        {
            return GetTemperatureDamage(_configuration, _value, out damageElement);
        }

        public static int GetTemperatureDamage(ITemperatureConfiguration configuration, int temperature, out Element? damageElement)
        {
            if (temperature < configuration.ColdDamageConfiguration.TemperatureLevel)
            {

                damageElement = Element.Frost;
                var damageValue = configuration.ColdDamageConfiguration.TemperatureLevel - temperature;
                return (int)Math.Round(damageValue * configuration.ColdDamageConfiguration.DamageMultiplier);
            }

            if (temperature > configuration.HeatDamageConfiguration.TemperatureLevel)
            {

                damageElement = Element.Fire;
                var damageValue = temperature - configuration.HeatDamageConfiguration.TemperatureLevel;
                return (int)Math.Round(damageValue * configuration.HeatDamageConfiguration.DamageMultiplier);
            }

            damageElement = null;
            return 0;
        }
    }
}