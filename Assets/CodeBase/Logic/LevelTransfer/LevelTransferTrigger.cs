using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Logic.LevelTransfer
{
  public class LevelTransferTrigger : MonoBehaviour
  {
    private const string PlayerTag = "Player";
    
    public string TransferTo;
    public WindowId WindowForOpen;
    
    private bool _triggered;
    
    private IWindowService _windowService;

    public void Construct(IWindowService windowService)
    {
      _windowService = windowService;
    }
    
    private void OnTriggerEnter(Collider other)
    {
      if (_triggered)
        return;
      
      if (other.CompareTag(PlayerTag))
      {
        _windowService.Open(WindowForOpen, TransferTo);
        _triggered = true;
      }
    }

    private void OnTriggerExit(Collider other)
    {
      _triggered = false;
    }
  }
}