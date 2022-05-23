using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.GameProcess;

namespace CodeMagic.UI.Blazor.Services
{
	public class SaveService : ISaveService
	{
		public void DeleteSave()
		{
            // TODO: Implement
		}

		public (IGameCore?, IGameData?) LoadGame()
		{
			return (null, null);
		}

		public void SaveGame()
		{
			// TODO: Implement
		}

		public Task SaveGameAsync()
		{
            // TODO: Implement
			return Task.CompletedTask;
		}
	}
}
