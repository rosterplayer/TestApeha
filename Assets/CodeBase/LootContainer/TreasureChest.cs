using System.Collections;
using CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine;

namespace CodeBase.LootContainer
{
  public class TreasureChest : MonoBehaviour
  {
    public Animator Animator;
    public Explosible Explosible;
    public GameObject PickupFxPrefab;
    public TextMeshPro LootText;
    public GameObject PickupPopup;
    
    private static readonly int IsOpened = Animator.StringToHash("IsOpened");
    private bool _opened;

    private IRandomService _randomService;

    public void Construct(IRandomService randomService)
    {
      _randomService = randomService;
    }

    public void Open()
    {
      if (_opened)
        return;
      
      Animator.SetBool(IsOpened, true);
      if (WillExplode())
      {
        Explosible.Blast();
        StartCoroutine(DestroyTimer());
      }
      else
      {
        PlayPickupFx();
        ShowText();
      }

      _opened = true;
    }
    
    private void PlayPickupFx()
    {
      Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);
    }

    private void ShowText()
    {
      LootText.text = $"{_randomService.Next(10, 50)}";
      PickupPopup.SetActive(true);
    }

    private bool WillExplode() => 
      _randomService.Next(0, 10) < 4;
    
    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(1);
      Destroy(gameObject);
    }
  }
}