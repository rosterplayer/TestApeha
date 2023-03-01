using CodeBase.Infrastructure.Services.DungeonProgress;
using CodeBase.Infrastructure.States;
using TMPro;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public class LeaveDungeonWindow : BaseWindow
  {
    public Button AwardButton;
    public Button LeaveButton;
    public TextMeshProUGUI Message;

    private string _transferTo;
    private IGameStateMachine _stateMachine;
    private IDungeonProgressService _dungeonProgress;

    public void Construct(IGameStateMachine stateMachine, IDungeonProgressService dungeonProgressService, string transferTo)
    {
      _stateMachine = stateMachine;
      _dungeonProgress = dungeonProgressService;
      _transferTo = transferTo;
    }

    protected override void Initialize()
    {
      if (_dungeonProgress.IsDungeonCompleted())
      {
        Message.text = "";
        AwardButton.onClick.AddListener(() => _stateMachine.Enter<LoadLevelState, string>(_transferTo));
      }
      else
      {
        Message.text = "There are still monsters in the dungeon";
        AwardButton.interactable = false;
      }

      LeaveButton.onClick.AddListener( () => _stateMachine.Enter<LoadLevelState, string>(_transferTo));
    }
  }
}