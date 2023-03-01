using CodeBase.UI.Services.Factory;

namespace CodeBase.UI.Services.Windows
{
  public class WindowService : IWindowService
  {
    private readonly IUIFactory _uiFactory;

    public WindowService(IUIFactory uiFactory)
    {
      _uiFactory = uiFactory;
    }

    public void Open(WindowId windowId)
    {
      switch (windowId)
      {
        case WindowId.Unknown:
          break;
      }
    }
    
    public void Open<TPayload>(WindowId windowId, TPayload payload)
    {
      switch (windowId)
      {
        case WindowId.Unknown:
          break;
        case WindowId.EnterDungeonWindow:
          _uiFactory.CreateEnterDungeonWindow(payload.ToString());
          break;
        case WindowId.ExitDungeonWindow:
          _uiFactory.CreateLeaveDungeonWindow(payload.ToString());
          break;
      }
    }
  }
}