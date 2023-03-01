using CodeBase.Infrastructure.States;
using UnityEngine.UI;

namespace CodeBase.UI.Windows
{
  public class EnterDungeonWindow : BaseWindow
  {
    public Button EnterButton;
    public Button LeaveButton;

    private string _transferTo;
    private IGameStateMachine _stateMachine;

    public void Construct(IGameStateMachine stateMachine, string transferTo)
    {
      _stateMachine = stateMachine;
      _transferTo = transferTo;
    }

    protected override void OnAwake()
    {
      base.OnAwake();
      EnterButton.onClick.AddListener( () => _stateMachine.Enter<LoadLevelState, string>(_transferTo));
      LeaveButton.onClick.AddListener( () => Destroy(gameObject));
    }
  }
}