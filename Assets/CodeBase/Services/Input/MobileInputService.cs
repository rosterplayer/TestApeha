using UnityEngine;

namespace CodeBase.Services.Input
{
  public class MobileInputService : IInputService
  {
    public bool IsMouseClicked()
    {
      if (UnityEngine.Input.touchCount > 0)
      {
        Touch touch = UnityEngine.Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
          return true;
      }

      return false;
    }

    public Vector3 MousePosition()
    {
      if (UnityEngine.Input.touchCount > 0)
      {
        Touch touch = UnityEngine.Input.GetTouch(0);
        return touch.position;
      }

      return Vector3.zero;
    }
  }
}