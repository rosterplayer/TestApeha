using System;
using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.LootContainer
{
  public class Explosible : MonoBehaviour
  {
    public ParticleSystem ParticleSystem;
    
    public float ExplosionRadius = 2f;
    public float Damage = 10f;

    private Collider[] _hits = new Collider[1];
    private int _layerMask;

    private void Start()
    {
      _layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    public void Blast()
    {
      ParticleSystem.Play();

      if (Hit(out Collider hit))
      { 
        hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
      }
    }
    
    private bool Hit(out Collider hit)
    {
      int hitsCount = Physics.OverlapSphereNonAlloc(transform.position, ExplosionRadius, _hits, _layerMask);
      
      hit = _hits.FirstOrDefault();
      
      return hitsCount > 0;
    }
  }
}