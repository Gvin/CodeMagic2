using System.Threading.Tasks;
using CodeMagic.Core.Game;

namespace CodeMagic.Game.GameProcess;

public interface ISaveService
{
    void SaveGame();

    (IGameCore, IGameData) LoadGame();

    Task SaveGameAsync();

    void DeleteSave();
}