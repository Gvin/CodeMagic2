using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Area.EnvironmentData;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.UI.Images;

namespace CodeMagic.Game.Items.Usable;

[Serializable]
public class AncientScroll : ScrollBase
{
    private const int MagicDamageOnFailedScroll = 1;
    private const int DisturbanceIncrementOnFailedScroll = 5;

    private const string ImageWorld = "ItemsOnGround_Scroll";

    public string InventoryImageName { get; set; }

    public int DamagePercent { get; set; }

    public string DamagedCode { get; set; }

    public override bool Use(IGameCore game)
    {
        var chanceToRead = GetChanceToRead(game.Player);
        if (RandomHelper.CheckChance(chanceToRead))
            return base.Use(game);

        game.Journal.Write(new FailedToUseScrollMessage());
        game.Player.Damage(CurrentGame.PlayerPosition, MagicDamageOnFailedScroll, Element.Magic);
        game.Journal.Write(new EnvironmentDamageMessage(game.Player, MagicDamageOnFailedScroll, Element.Magic));
        var environment = (IGameEnvironment) game.Map.GetCell(game.PlayerPosition).Environment;
        environment.MagicDisturbanceLevel += DisturbanceIncrementOnFailedScroll;
        return false;
    }

    private int GetChanceToRead(IPlayer player)
    {
        return 100 - DamagePercent + player.ScrollReadingBonus;
    }

    public override string GetSpellDisplayCode()
    {
        return DamagedCode;
    }

    public override ISymbolsImage GetWorldImage(IImagesStorage storage)
    {
        return storage.GetImage(ImageWorld);
    }

    public override ISymbolsImage GetInventoryImage(IImagesStorage storage)
    {
        return storage.GetImage(InventoryImageName);
    }

    public override StyledLine[] GetDescription(Player player)
    {
        return new[]
        {
            TextHelper.GetWeightLine(Weight),
            StyledLine.Empty,
            new StyledLine {$"Spell Name: {SpellName}"},
            new StyledLine {"Damaged: ", new StyledString($"{DamagePercent}%", TextHelper.NegativeValueColor)},
            StyledLine.Empty,
            new StyledLine {"This scroll looks old and damaged."},
            new StyledLine {"It's title is written with some unknown language."},
            new StyledLine {"Some runes can barely be red."}
        };
    }
}