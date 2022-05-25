using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Liquids;
using CodeMagic.Game.Drawing;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.Objects.LiquidObjects;

[Serializable]
public class OilLiquid : MapObjectBase, ILiquid, IFireSpreadingObject, IDynamicObject, IWorldImageProvider
{
    private const string ImageSmall = "Oil_Small";
    private const string ImageMedium = "Oil_Medium";
    private const string ImageBig = "Oil_Big";

    private const string CustomValueIgnitionTemperature = "IgnitionTemperature";
    private const string CustomValueBurningTemperature = "BurningTemperature";
    private const string CustomValueBurningRate = "BurningRate";
    private const string CustomValueHeatSpeed = "HeatSpeed";

    public const string LiquidType = "OilLiquid";

    private readonly ILiquidConfiguration _configuration;
    private readonly int _ignitionTemperature;
    private readonly int _heatSpeed;
    private readonly double _burningRate;

    private int _volume;

    public OilLiquid(int volume)
        : this()
    {
        _volume = volume;
    }

    public OilLiquid()
    {
        _configuration = GameConfigurationManager.GetLiquidConfiguration(LiquidType);

        _ignitionTemperature = GetCustomInt(CustomValueIgnitionTemperature);
        BurningTemperature = GetCustomInt(CustomValueBurningTemperature);
        _heatSpeed = GetCustomInt(CustomValueHeatSpeed);
        _burningRate = GetCustomDouble(CustomValueBurningRate);
    }

    public override string Name => "Oil";

    public UpdateOrder UpdateOrder => UpdateOrder.Medium;

    public override ZIndex ZIndex => ZIndex.FloorCover;

    public override ObjectSize Size => ObjectSize.Huge;

    public string Type => LiquidType;

    public void Update(Point position)
    {
        var cell = CurrentGame.Map.GetCell(position);

        if (Volume <= 0)
        {
            CurrentGame.Map.RemoveObject(position, this);
            return;
        }

        if (cell.Temperature() >= _ignitionTemperature)
        {
            ProcessBurning(cell);
        }

        if (Volume >= MinVolumeForEffect)
        {
            ApplyOilyStatus(cell);
        }
    }

    private void ApplyOilyStatus(IAreaMapCell cell)
    {
        var destroyableObjects = cell.Objects.OfType<IDestroyableObject>();
        foreach (var destroyable in destroyableObjects)
        {
            destroyable.Statuses.Add(new OilyObjectStatus());
        }
    }

    private void ProcessBurning(IAreaMapCell cell)
    {
        if (cell.Temperature() < BurningTemperature)
        {
            var temperatureDiff = BurningTemperature - cell.Temperature();
            var temperatureChange = Math.Min(temperatureDiff, _heatSpeed);
            cell.Environment.Cast().Temperature += temperatureChange;
        }

        var burnedVolume = (int)Math.Ceiling(cell.Temperature() * _burningRate);
        Volume -= burnedVolume;
    }

    private int GetCustomInt(string key)
    {
        var stringValue = GetCustomString(key);
        return int.Parse(stringValue);
    }

    private string GetCustomString(string key)
    {
        var stringValue =
            _configuration.CustomValues.FirstOrDefault(value =>
                string.Equals(value.Key, key))?.Value;
        if (string.IsNullOrEmpty(stringValue))
            throw new ArgumentException(
                $"Custom value \"{key}\" not found in configuration for liquid type \"{LiquidType}\".");
        return stringValue;
    }

    private double GetCustomDouble(string key)
    {
        var stringValue = GetCustomString(key);
        return double.Parse(stringValue);
    }

    public int Volume
    {
        get => _volume;
        set => _volume = Math.Max(0, value);
    }

    public int MaxVolumeBeforeSpread => _configuration.MaxVolumeBeforeSpread;

    public int MaxSpreadVolume => _configuration.MaxSpreadVolume;

    public int MinVolumeForEffect => _configuration.MinVolumeForEffect;

    public ISpreadingObject Separate(int separateVolume)
    {
        Volume -= separateVolume;
        return new OilLiquid(separateVolume);
    }

    public bool Updated { get; set; }

    public bool GetIsOnFire(IAreaMapCell cell)
    {
        return cell.Temperature() >= _ignitionTemperature;
    }

    public bool SpreadsFire => true;

    public int BurningTemperature { get; }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        if (Volume >= _configuration.MaxVolumeBeforeSpread)
            return storage.GetImage(ImageBig);

        var halfSpread = _configuration.MaxVolumeBeforeSpread / 2;
        if (Volume >= halfSpread)
            return storage.GetImage(ImageMedium);

        return storage.GetImage(ImageSmall);
    }
}