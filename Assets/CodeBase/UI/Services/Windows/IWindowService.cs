using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Windows
{
  public interface IWindowService : IService
  {
    void Open(WindowId windowId);
    void Open<TPayload>(WindowId windowId, TPayload payload);
  }
}