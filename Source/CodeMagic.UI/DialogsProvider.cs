using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI
{
    public class DialogsProvider : IDialogsProvider
    {
        private readonly IApplicationController _controller;

        public DialogsProvider(IApplicationController controller)
        {
            _controller = controller;
        }

        public void OpenInventoryDialog(string inventoryName, IInventory inventory)
        {
            _controller.CreatePresenter<CustomInventoryPresenter>().Run(CurrentGame.Game, inventoryName, inventory);
        }

        public void OpenWaitDialog(string message, Action waitAction)
        {
            _controller.CreatePresenter<WaitMessagePresenter>().Run(message, waitAction);
        }
    }
}