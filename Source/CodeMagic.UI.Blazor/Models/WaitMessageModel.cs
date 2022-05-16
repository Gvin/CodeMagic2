using CodeMagic.UI.Blazor.Services;
using CodeMagic.UI.Presenters;

namespace CodeMagic.UI.Blazor.Models;

public class WaitMessageModel : WindowModelBase, IWaitMessageView
{
    public WaitMessageModel(IWindowService windowService) 
        : base(windowService)
    {
    }

    public string? Message { get; set; }
}