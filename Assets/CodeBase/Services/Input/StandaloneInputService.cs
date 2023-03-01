using UnityEngine;

namespace CodeBase.Services.Input
{
  public class StandaloneInputService : IInputService
  {
    public bool IsMouseClicked() => 
      UnityEngine.Input.GetMouseButton(0);

    public Vector3 MousePosition() => 
      UnityEngine.Input.mousePosition;
  }
}