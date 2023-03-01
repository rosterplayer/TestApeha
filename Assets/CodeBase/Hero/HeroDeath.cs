using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroHealth))]
  public class HeroDeath : MonoBehaviour
  {
    public HeroHealth Health;

    public HeroMove Move;
    public HeroAttack Attack;
    public ClickObserver ClickObserver;
    public HeroAnimator Animator;

    public GameObject DeathFx;
    private bool _isDead;
    
    private IWindowService _windowService;

    public void Construct(IWindowService windowService)
    {
      _windowService = windowService;
    }

    private void Start() => 
      Health.HealthChanged += HealthChanged;

    private void OnDestroy() => 
      Health.HealthChanged -= HealthChanged;

    public void OnDeathAnimationEnded()
    {
      _windowService.Open(WindowId.ExitDungeonWindow, "Main");
    }
    
    private void HealthChanged()
    {
      if (!_isDead && Health.Current <= 0f)
        Die();
    }

    private void Die()
    {
      _isDead = true;
      Move.enabled = false;
      Attack.enabled = false;
      ClickObserver.enabled = false;
      Animator.PlayDeath();

      Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
  }
}