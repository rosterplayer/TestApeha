using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.DungeonProgress;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.LevelTransfer;
using CodeBase.LootContainer;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory
{
  public class GameFactory : IGameFactory
  {
    public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
    public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
    private GameObject HeroGameObject { get; set; }

    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _progressService;
    private readonly IWindowService _windowService;
    private readonly IDungeonProgressService _dungeonProgress;

    public GameFactory(IAssetProvider assets, 
      IStaticDataService staticData, 
      IRandomService randomService, 
      IPersistentProgressService progressService, 
      IWindowService windowService, 
      IDungeonProgressService dungeonProgress
      )
    {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
      _windowService = windowService;
      _dungeonProgress = dungeonProgress;
      _randomService = randomService;
    }

    public async Task WarmUp()
    {
      await _assets.Load<GameObject>(AssetAddress.TreasureChest);
      await _assets.Load<GameObject>(AssetAddress.Spawner);
    }
    
    public async Task<GameObject> CreateHero(Vector3 at)
    {
      HeroGameObject = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);
      HeroGameObject.GetComponent<HeroDeath>().Construct(_windowService);
      return HeroGameObject;
    }

    public async Task<GameObject> CreateHud()
    {
      GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);

      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>()) 
        openWindowButton.Construct(_windowService);

      return hud;
    }

    public async Task<GameObject> CreateMonster(MonsterTypeId typeId, Transform parent)
    {
      MonsterStaticData monsterData = _staticData.ForMonster(typeId);

      GameObject prefab = await _assets.Load<GameObject>(monsterData.PrefabReference);
      
      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
      
      IHealth health = monster.GetComponent<IHealth>();
      health.Current = monsterData.Hp;
      health.Max = monsterData.Hp;

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      Attack attack = monster.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;
      
      monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);
      
      _dungeonProgress.AddEnemyForObserve(monster);

      return monster;
    }

    public async Task CreateLoot(Vector3 at)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.TreasureChest);
      GameObject loot = InstantiateRegistered(prefab, at);
      loot.GetComponent<TreasureChest>().Construct(_randomService);
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterTypeId monsterTypeId)
    {
      GameObject prefab = await _assets.Load<GameObject>(AssetAddress.Spawner);
      SpawnPoint spawner = InstantiateRegistered(prefab, at)
        .GetComponent<SpawnPoint>();

      spawner.Construct(this);
      spawner.Id = spawnerId;
      spawner.MonsterTypeId = monsterTypeId;
    }

    public void CreateLevelTransferTrigger(LevelTransferPoint transferPoint)
    {
      LevelTransferTrigger transferTrigger = Object
        .Instantiate(Resources.Load<GameObject>(AssetAddress.LevelTransferTrigger), transferPoint.Position, Quaternion.identity)
        .GetComponent<LevelTransferTrigger>();
      
      transferTrigger.Construct(_windowService);
      transferTrigger.TransferTo = transferPoint.TransferTo;
      transferTrigger.WindowForOpen = transferPoint.WindowForOpen;
    }

    private void Register(ISavedProgressReader progressReader)
    {
      if (progressReader is ISavedProgress progressWriter) 
        ProgressWriters.Add(progressWriter);

      ProgressReaders.Add(progressReader);
    }

    public void Cleanup()
    {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      
      _assets.CleanUp();
    }

    private GameObject InstantiateRegistered(GameObject prefab, Vector3 position)
    {
      GameObject heroGameObject = Object.Instantiate(prefab, position, Quaternion.identity);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private GameObject InstantiateRegistered(GameObject prefab)
    {
      GameObject heroGameObject = Object.Instantiate(prefab);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 position)
    {
      GameObject heroGameObject = await _assets.Instantiate(prefabPath, position);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
    {
      GameObject heroGameObject = await _assets.Instantiate(prefabPath);
      RegisterProgressWatchers(heroGameObject);
      return heroGameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject)
    {
      foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>()) 
        Register(progressReader);
    }
  }
}