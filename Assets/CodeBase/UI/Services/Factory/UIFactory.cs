using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.DungeonProgress;
using CodeBase.Infrastructure.States;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory
{
  class UIFactory : IUIFactory
  {
    private readonly IAssetProvider _assetsProvider;
    private readonly IStaticDataService _staticData;
    private readonly IGameStateMachine _gameStateMachine;
    private readonly IDungeonProgressService _dungeonProgress;

    private Transform _uiRoot;

    public UIFactory(
      IAssetProvider assetsProvider, 
      IStaticDataService staticData,
      IGameStateMachine gameStateMachine,
      IDungeonProgressService dungeonProgress
      )
    {
      _assetsProvider = assetsProvider;
      _staticData = staticData;
      _gameStateMachine = gameStateMachine;
      _dungeonProgress = dungeonProgress;
    }

    public async Task CreateUIRoot()
    {
      GameObject root = await _assetsProvider.Instantiate(AssetAddress.UIRootAddress);
      _uiRoot = root.transform;
    }
    
    public void CreateEnterDungeonWindow(string transferTo)
    {
      WindowConfig config = _staticData.ForWindow(WindowId.EnterDungeonWindow);
      EnterDungeonWindow window = Object.Instantiate(config.Prefab, _uiRoot) as EnterDungeonWindow;
      window.Construct(_gameStateMachine, transferTo);
    }
    
    public void CreateLeaveDungeonWindow(string transferTo)
    {
      WindowConfig config = _staticData.ForWindow(WindowId.ExitDungeonWindow);
      LeaveDungeonWindow window = Object.Instantiate(config.Prefab, _uiRoot) as LeaveDungeonWindow;
      window.Construct(_gameStateMachine, _dungeonProgress, transferTo);
    }
  }
}