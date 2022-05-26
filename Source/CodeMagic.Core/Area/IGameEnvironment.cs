using CodeMagic.Core.Game;

namespace CodeMagic.Core.Area;

public interface IGameEnvironment
{
    void Update(Point position, IAreaMapCell cell);

    void Balance(IAreaMapCell cell, IAreaMapCell otherCell);

    int Temperature { get; set; }

    int Pressure { get; set; }

    int MagicEnergyLevel { get; set; }

    int MaxMagicEnergyLevel { get; }

    int MagicDisturbanceLevel { get; set; }
}