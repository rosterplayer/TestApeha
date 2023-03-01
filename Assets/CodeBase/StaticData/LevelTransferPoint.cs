using System;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData
{
  [Serializable]
  public class LevelTransferPoint
  {
    public Vector3 Position;
    public string TransferTo;
    public WindowId WindowForOpen;

    public LevelTransferPoint(Vector3 at, string transferTo, WindowId windowForOpen)
    {
      Position = at;
      TransferTo = transferTo;
      WindowForOpen = windowForOpen;
    }
  }
}