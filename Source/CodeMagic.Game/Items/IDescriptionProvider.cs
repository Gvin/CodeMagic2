using CodeMagic.Core.Objects;

namespace CodeMagic.Game.Items
{
    public interface IDescriptionProvider
    {
        StyledLine[] GetDescription(IPlayer player);
    }
}