using System.Threading.Tasks;
using CodeBase.CameraLogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.DungeonProgress;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States
{
  public class LoadLevelState : IPayloadedState<string>
  {
    private readonly GameStateMachine _gameStateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _curtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;
    private readonly IDungeonProgressService _dungeonProgress;

    public LoadLevelState(
      GameStateMachine gameStateMachine, 
      SceneLoader sceneLoader, 
      LoadingCurtain curtain,
      IGameFactory gameFactory, 
      IPersistentProgressService progressService, 
      IStaticDataService staticData, 
      IUIFactory uiFactory, 
      IDungeonProgressService dungeonProgress
      )
    {
      _gameStateMachine = gameStateMachine;
      _sceneLoader = sceneLoader;
      _curtain = curtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
      _uiFactory = uiFactory;
      _dungeonProgress = dungeonProgress;
    }

    public void Enter(string sceneName)
    {
      _curtain.Show();
      _dungeonProgress.Cleanup();
      _gameFactory.Cleanup();
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }
    
    public void Exit() => 
      _curtain.Hide();

    private async void OnLoaded()
    {
      await InitUIRoot();
      await InitGameWorld();
      InformProgressReaders();
      
      _gameStateMachine.Enter<GameLoopState>();
    }

    private async Task InitUIRoot() => 
      await _uiFactory.CreateUIRoot();

    private void InformProgressReaders()
    {
      foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
      {
        progressReader.LoadProgress(_progressService.Progress);
      }
    }

    private async Task InitGameWorld()
    {
      LevelStaticData levelData = LevelStaticData();

      await InitSpawners(levelData);

      await InitLoot(levelData);
      
      InitLevelTransferTrigger(levelData);

      GameObject hero = await InitHero(levelData);

      await InitHud(hero);

      CameraFollow(hero);
    }

    private async Task<GameObject> InitHero(LevelStaticData levelData)
    {
      _progressService.Progress.HeroState.ResetHP();
      return await _gameFactory.CreateHero(levelData.InitialHeroPosition);
    }

    private async Task InitSpawners(LevelStaticData levelData)
    {
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners) 
        await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterTypeId);
    }

    private async Task InitLoot(LevelStaticData levelData)
    {
      foreach (LootSpawnerData lootSpawner in levelData.LootSpawners)
      {
        await _gameFactory.CreateLoot(lootSpawner.Position);
      }
    }

    private async Task InitHud(GameObject hero)
    {
      GameObject hud = await _gameFactory.CreateHud();
      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
    }

    private void CameraFollow(GameObject hero)
    {
      Camera.main
        .GetComponent<CameraFollow>()
        .Follow(hero);
    }

    private void InitLevelTransferTrigger(LevelStaticData levelData)
    {
      foreach (LevelTransferPoint levelDataTransferPoint in levelData.TransferPoints)
      {
        _gameFactory.CreateLevelTransferTrigger(levelDataTransferPoint);
      }
    }

    private LevelStaticData LevelStaticData() => 
      _staticData.ForLevel(SceneManager.GetActiveScene().name);
  }
}