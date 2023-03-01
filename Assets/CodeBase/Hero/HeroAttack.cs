using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroAnimator), typeof(NavMeshAgent))]
  public class HeroAttack : MonoBehaviour, ISavedProgressReader
  {
    public HeroAnimator HeroAnimator;
    public NavMeshAgent NavAgent;

    private static int _layerMask;
    private Collider[] _hits = new Collider[3];
    private Stats _stats;

    public float AttackDistance => _stats.DamageRadius;

    private void Awake()
    {
      _layerMask = 1 << LayerMask.NameToLayer("Hittable");
    }

    public void TryAttack()
    {
      if(!HeroAnimator.IsAttacking)
        HeroAnimator.PlayAttack();
    }

    public void OnAttack()
    {
      for (int i = 0; i < Hit(); i++)
      {
        _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
      }
    }

    public void LoadProgress(PlayerProgress progress) => _stats = progress.HeroStats;

    private int Hit()
    {
      PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 1);
      return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);
    }

    private Vector3 StartPoint()
    {
      Vector3 characterControllerWorldPosition = transform.position;
      return new Vector3(
        characterControllerWorldPosition.x, 
        characterControllerWorldPosition.y + NavAgent.height / 4,
        characterControllerWorldPosition.z);
    }
  }
}