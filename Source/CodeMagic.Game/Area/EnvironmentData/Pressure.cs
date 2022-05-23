using System;
using System.Diagnostics;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Physics;

namespace CodeMagic.Game.Area.EnvironmentData
{
    [DebuggerDisplay("{" + nameof(Value) + "} kPa")]
    [Serializable]
    public class Pressure
    {
        private int _value;

        private readonly IPressureConfiguration _configuration;

        public Pressure()
        {
            _configuration = GameConfigurationManager.Current.Physics.PressureConfiguration;

            _value = _configuration.NormalValue;
            OldValue = _configuration.NormalValue;
        }

        public int Value
        {
            get => _value;
            set
            {
                if (value < _configuration.MinValue)
                {
                    this._value = _configuration.MinValue;
                    return;
                }

                if (value > _configuration.MaxValue)
                {
                    this._value = _configuration.MaxValue;
                    return;
                }

                this._value = value;
            }
        }

        public int OldValue { get; set; }

        public void Normalize()
        {
            if (Value == _configuration.NormalValue)
                return;

            var difference = Math.Abs(_configuration.NormalValue - Value);
            difference = Math.Min(difference, _configuration.NormalizeSpeed);

            if (Value > _configuration.NormalValue)
            {
                Value -= difference;
            }

            if (Value < _configuration.NormalValue)
            {
                Value += difference;
            }
        }

        public void Balance(Pressure other)
        {
            if (Value == other.Value)
                return;

            var medium = (int)Math.Round((Value + other.Value) / 2d);
            Value = medium;
            other.Value = medium;
        }

        public int GetPressureDamage()
        {
            var pureDamage = GetPurePressureDamage();
            var pressureChangeDamage = GetPressureChangeDamage();

            OldValue = Value;

            return pureDamage + pressureChangeDamage;
        }

        private int GetPressureChangeDamage()
        {
            var difference = Math.Abs(Value - OldValue);

            if (difference < _configuration.ChangePressureDamageConfiguration.PressureLevel)
                return 0;

            var differenceValue = _configuration.ChangePressureDamageConfiguration.PressureLevel - difference;
            return (int) Math.Round(differenceValue * _configuration.ChangePressureDamageConfiguration.DamageMultiplier);
        }

        private int GetPurePressureDamage()
        {
            if (_value < _configuration.LowPressureDamageConfiguration.PressureLevel)
            {
                var diff = _configuration.LowPressureDamageConfiguration.PressureLevel - _value;
                return (int)Math.Round(diff * _configuration.LowPressureDamageConfiguration.DamageMultiplier);
            }

            if (_value > _configuration.HighPressureDamageConfiguration.PressureLevel)
            {
                var diff = _value - _configuration.HighPressureDamageConfiguration.PressureLevel;
                return (int)Math.Round(diff * _configuration.HighPressureDamageConfiguration.DamageMultiplier);
            }

            return 0;
        }
    }
}