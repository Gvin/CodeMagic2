using CodeMagic.Core.Area;

namespace CodeMagic.Game.Area.EnvironmentData
{
    public static class AreaMapCellExtensions
    {
        public static int Temperature(this IAreaMapCell cell)
        {
            return cell.Environment.Temperature;
        }

        public static int Pressure(this IAreaMapCell cell)
        {
            return cell.Environment.Pressure;
        }

        public static int MagicEnergyLevel(this IAreaMapCell cell)
        {
            return cell.Environment.MagicEnergyLevel;
        }

        public static int MagicDisturbanceLevel(this IAreaMapCell cell)
        {
            return cell.Environment.MagicDisturbanceLevel;
        }

        public static int MaxMagicEnergyLevel(this IAreaMapCell cell)
        {
            return cell.Environment.MaxMagicEnergyLevel;
        }
    }
}