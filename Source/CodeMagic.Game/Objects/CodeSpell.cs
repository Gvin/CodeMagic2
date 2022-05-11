using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Game.Images;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Spells;
using CodeMagic.Game.Spells.Script;
using Microsoft.Extensions.Logging;

namespace CodeMagic.Game.Objects;

public interface ICodeSpell : ILightObject, IDynamicObject
{

}

[Serializable]
public class CodeSpell : MapObjectBase, ICodeSpell, IWorldImageProvider
{
    private readonly ILogger<CodeSpell> _logger;

    private const string ImageHighMana = "Spell_HighMana";
    private const string ImageMediumMana = "Spell_MediumMana";
    private const string ImageLowMana = "Spell_LowMana";

    private const int HighManaLevel = 100;
    private const int MediumManaLevel = 20;

    public const LightLevel DefaultLightLevel = LightLevel.Dusk1;

    private readonly ISymbolsAnimationsManager _animations;

    public CodeSpell()
    {
        _logger = StaticLoggerFactory.CreateLogger<CodeSpell>();
        _animations = new SymbolsAnimationsManager(
            TimeSpan.FromMilliseconds(500),
            AnimationFrameStrategy.OneByOneStartFromRandom);

        LightPower = DefaultLightLevel;
        RemainingLightTime = null;
        LifeTime = 0;
    }

    public CodeSpell(ICreatureObject caster, string code)
        : this()
    {
        CasterId = caster.Id;
        Code = code;
        CodeExecutor = new SpellCodeExecutor(CasterId, Code);
    }

    public SpellCodeExecutor CodeExecutor { get; set; }

    public string CasterId { get; set; }

    public string Code { get; set; }

    public int? RemainingLightTime { get; set; }

    public int LifeTime { get; set; }

    public override ObjectSize Size => ObjectSize.Huge;

    public UpdateOrder UpdateOrder => UpdateOrder.Early;

    public bool Updated { get; set; }

    public int Mana { get; set; }

    private LightLevel LightPower { get; set; }

    public override ZIndex ZIndex => ZIndex.Spell;

    public ILightSource[] LightSources => new ILightSource[]
    {
        new StaticLightSource(LightPower)
    };

    public void SetEmitLight(LightLevel level, int time)
    {
        LightPower = level;
        RemainingLightTime = time;
    }

    public void Update(Point position)
    {
        var currentPosition = position;
        try
        {
            ProcessLightEmitting();

            var action = CodeExecutor.Execute(position, this, LifeTime);
            LifeTime++;

            if (action.ManaCost <= Mana)
            {
                currentPosition = action.Perform(position);
                Mana -= action.ManaCost;
            }
            else
            {
                Mana = 0;
            }

            if (Mana != 0)
                return;

            CurrentGame.Journal.Write(new SpellOutOfManaMessage(Name));
            CurrentGame.Map.RemoveObject(currentPosition, this);
        }
        catch (SpellException ex)
        {
            _logger.LogDebug(ex, "Spell error");
            CurrentGame.Journal.Write(new SpellErrorMessage(Name, ex.Message));
            CurrentGame.Map.RemoveObject(currentPosition, this);
        }
    }

    private void ProcessLightEmitting()
    {
        if (!RemainingLightTime.HasValue)
            return;

        if (RemainingLightTime.Value < 0)
        {
            LightPower = DefaultLightLevel;
            RemainingLightTime = null;
            return;
        }

        RemainingLightTime = RemainingLightTime.Value - 1;
    }

    public ISymbolsImage GetWorldImage(IImagesStorageService storage)
    {
        if (Mana >= HighManaLevel)
            return _animations.GetImage(storage, ImageHighMana);
        if (Mana >= MediumManaLevel)
            return _animations.GetImage(storage, ImageMediumMana);
        return _animations.GetImage(storage, ImageLowMana);
    }
}