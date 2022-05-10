using CodeMagic.Core.Game;
using CodeMagic.Game;
using CodeMagic.Game.GameProcess;

namespace CodeMagic.UI.Blazor.Services
{
	public class SaveService : ISaveService
	{
		public void DeleteSave()
		{
			throw new NotImplementedException();
		}

		public (IGameCore?, IGameData?) LoadGame()
		{
			return (null, null);
		}

		public void SaveGame()
		{
			throw new NotImplementedException();
		}

		public Task SaveGameAsync()
		{
			throw new NotImplementedException();
		}
	}
}
