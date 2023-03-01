using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.DungeonProgress
{
  public class DungeonProgress : IDungeonProgressService
  {
    private List<GameObject> _enemies;

    public DungeonProgress()
    {
      _enemies = new List<GameObject>();
    }

    public void AddEnemyForObserve(GameObject enemy) => 
      _enemies.Add(enemy);

    public bool IsDungeonCompleted()
    {
      foreach (GameObject enemy in _enemies)
      {
        if (enemy != null)
          return false;
      }

      return true;
    }

    public void Cleanup() => 
      _enemies.Clear();
  }
}