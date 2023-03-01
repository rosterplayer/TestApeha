using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.DungeonProgress;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.RandomService;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Input;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
  public class BootstrapState : IState
  {
    private const string Initial = "Initial";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
    {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _services = services;
      
      RegisterServices();
    }

    public void Enter()
    {
      _sceneLoader.Load(Initial, onLoaded: EnterLoadLevel);
    }

    public void Exit()
    {
    }

    private void EnterLoadLevel() => 
      _stateMachine.Enter<LoadProgressState>();
    
    private void RegisterServices()
    {
      RegisterStaticData();
      RegisterRandomService();
      _services.RegisterSingle<IDungeonProgressService>(new DungeonProgress());

      _services.RegisterSingle<IGameStateMachine>(_stateMachine);
      _services.RegisterSingle(RegisterInputService());
      RegisterAssetService();
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());


      _services.RegisterSingle<IUIFactory>(new UIFactory(
        _services.Single<IAssetProvider>(), 
        _services.Single<IStaticDataService>(),
        _services.Single<IGameStateMachine>(),
        _services.Single<IDungeonProgressService>()
        ));

      _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));

      _services.RegisterSingle<IGameFactory>(new GameFactory(
        _services.Single<IAssetProvider>(),
        _services.Single<IStaticDataService>(),
        _services.Single<IRandomService>(),
        _services.Single<IPersistentProgressService>(),
        _services.Single<IWindowService>(),
        _services.Single<IDungeonProgressService>()
        ));

      _services.RegisterSingle<ISaveLoadService>(new SaveLoadService(
        _services.Single<IPersistentProgressService>(), 
        _services.Single<IGameFactory>()));
    }

    private void RegisterAssetService()
    {
      var assetProvider = new AssetProvider();
      _services.RegisterSingle<IAssetProvider>(assetProvider);
      assetProvider.Initialize();
    }

    private void RegisterStaticData()
    {
      IStaticDataService staticData = new StaticDataService();
      staticData.LoadMonsters();
      _services.RegisterSingle<IStaticDataService>(staticData);
    }

    private void RegisterRandomService()
    {
      IRandomService randomService = new UnityRandomService();
      _services.RegisterSingle(randomService);
    }

    private static IInputService RegisterInputService()
    {
      if (Application.isMobilePlatform)
        return new MobileInputService();
      else
        return new StandaloneInputService();
    }
  }
}