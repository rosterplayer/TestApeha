using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;

namespace CodeBase.UI.Services.Factory
{
  public interface IUIFactory : IService
  {
    Task CreateUIRoot();
    void CreateEnterDungeonWindow(string transferTo);
    void CreateLeaveDungeonWindow(string transferTo);
  }
}