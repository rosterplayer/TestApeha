using UnityEngine;

namespace CodeBase.Infrastructure.Services.DungeonProgress
{
  public interface IDungeonProgressService : IService
  {
    void AddEnemyForObserve(GameObject enemy);
    bool IsDungeonCompleted();
    void Cleanup();
  }
}