using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Services.Input
{
  public interface IInputService : IService
  {
    bool IsMouseClicked();
    Vector3 MousePosition();
  }
}