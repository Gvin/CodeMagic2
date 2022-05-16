using System;
using System.Threading.Tasks;

namespace CodeMagic.UI.Presenters;

public interface IWaitMessageView : IView
{
    string Message { set; }
}

public class WaitMessagePresenter : IPresenter
{
    private readonly IWaitMessageView _view;

    public WaitMessagePresenter(IWaitMessageView view)
    {
        _view = view;
    }

    public void Run(string message, Action waitAction)
    {
        _view.Message = message;
        _view.Show();

        Task.Run(() =>
        {
            waitAction();
            _view.Close();
        });
    }
}