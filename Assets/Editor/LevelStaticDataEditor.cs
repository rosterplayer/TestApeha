using System.Linq;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.LevelTransfer;
using CodeBase.LootContainer;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";

    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData) target;

      if (GUILayout.Button("Collect"))
      {
        levelData.EnemySpawners =
          FindObjectsOfType<EnemySpawnMarker>()
            .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
            .ToList();
        
        levelData.LootSpawners =
          FindObjectsOfType<LootSpawnMarker>()
            .Select(x => new LootSpawnerData(x.GetComponent<UniqueId>().Id, x.LootTypeId, x.transform.position))
            .ToList();

        levelData.LevelKey = SceneManager.GetActiveScene().name;

        levelData.InitialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;

        LevelTransferMarker[] transferPoints = FindObjectsOfType<LevelTransferMarker>();
        levelData.TransferPoints.Clear();
        foreach (LevelTransferMarker levelTransferMarker in transferPoints)
        {
          levelData.TransferPoints.Add(new LevelTransferPoint(levelTransferMarker.transform.position, levelTransferMarker.TransferTo, levelTransferMarker.WindowId));
        }
      }

      EditorUtility.SetDirty(target);
    }
  }
}