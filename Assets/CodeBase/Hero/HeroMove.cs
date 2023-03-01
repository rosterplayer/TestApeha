using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero
{
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    public NavMeshAgent NavMeshAgent;

    public void SetDestinationPoint(Vector3 destination) => 
      NavMeshAgent.SetDestination(destination);
    
    public void FaceTarget(Vector3 target)
    {
      Vector3 direction = (target - transform.position).normalized;
      Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public void UpdateProgress(PlayerProgress progress) => 
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    
    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.WorldData.PositionOnLevel.Level == CurrentLevel())
      {
        Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
        if (savedPosition != null)
        {
          Warp(savedPosition);
        }
      }
    }

    private void Warp(Vector3Data to)
    {
      NavMeshAgent.enabled = false;
      transform.position = to.AsUnityVector().AddY(NavMeshAgent.height);
      NavMeshAgent.enabled = true;
    }

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;
  }
}