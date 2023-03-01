using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.LootContainer;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(HeroMove), typeof(HeroAttack))]
  public class ClickObserver : MonoBehaviour
  {
    private const float InteractionDistance = 3f;
    
    public HeroMove HeroMove;
    public HeroAttack HeroAttack;

    private IInputService _inputService;

    private Camera _camera;
    private int _clickableLayerMask;
    private int _groundLayerMask;
    private int _hittableLayerMask;
    private int _pickupLayerMask;


    private void Start()
    {
      _inputService = AllServices.Container.Single<IInputService>();
      _camera = Camera.main;
      
      _groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
      _hittableLayerMask = 1 << LayerMask.NameToLayer("Hittable");
      _pickupLayerMask = 1 << LayerMask.NameToLayer("Pickup");
      _clickableLayerMask = _groundLayerMask |= _hittableLayerMask |= _pickupLayerMask;
    }

    private void Update()
    {
      if (_inputService.IsMouseClicked()) 
        ChooseAction();
    }

    private void ChooseAction()
    {
      Ray ray = _camera.ScreenPointToRay(_inputService.MousePosition());

      if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity, _clickableLayerMask))
      {
        var distance = (transform.position - raycastHit.point).magnitude;
        
        if (raycastHit.collider.GetComponentInParent<EnemyHealth>() != null && distance <= HeroAttack.AttackDistance * 3)
        {
          HeroMove.FaceTarget(raycastHit.point);
          HeroAttack.TryAttack();
        }
        else if (raycastHit.collider.TryGetComponent(out TreasureChest chest) && distance <= InteractionDistance)
        {
          chest.Open();
        }
        else
        {
          HeroMove.SetDestinationPoint(raycastHit.point);
        }
      }
    }
  }
}