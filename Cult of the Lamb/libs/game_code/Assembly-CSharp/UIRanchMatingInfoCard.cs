// Decompiled with JetBrains decompiler
// Type: UIRanchMatingInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI.RanchSelect;
using Spine.Unity;
using TMPro;
using UnityEngine;

#nullable disable
public class UIRanchMatingInfoCard : MonoBehaviour
{
  [SerializeField]
  public CanvasGroup canvasGroup;
  [SerializeField]
  public SkeletonGraphic animalSpine1;
  [SerializeField]
  public SkeletonGraphic animalSpine2;
  [SerializeField]
  public TMP_Text textAnimalName1;
  [SerializeField]
  public TMP_Text textAnimalName2;
  [SerializeField]
  public GameObject unavailableContainer;
  [SerializeField]
  public TMP_Text unavailableText;

  public void Configure(
    Interaction_RanchHutch hutch,
    RanchSelectEntry animal1,
    RanchSelectEntry animal2,
    UIRanchMatingMenu menuController,
    bool fadeAnimal1 = false,
    bool fadeAnimal2 = false)
  {
    if (animal1 != null)
    {
      this.animalSpine1.ConfigureAnimal(animal1.AnimalInfo);
      this.ResizeSkeleton(this.animalSpine1, animal1.AnimalInfo.Type);
      this.textAnimalName1.text = animal1.AnimalInfo.GetName();
      if (!fadeAnimal1)
        this.animalSpine1.color = Color.white;
      else
        this.animalSpine1.color = Color.gray;
      this.SetUnavailableText(animal1.AvailabilityStatus);
    }
    else
    {
      this.animalSpine1.color = Color.black;
      this.textAnimalName1.text = "";
    }
    if (animal2 != null)
    {
      this.animalSpine2.ConfigureAnimal(animal2.AnimalInfo);
      this.ResizeSkeleton(this.animalSpine2, animal2.AnimalInfo.Type);
      this.textAnimalName2.text = animal2.AnimalInfo.GetName();
      if (!fadeAnimal2)
        this.animalSpine2.color = Color.white;
      else
        this.animalSpine2.color = Color.gray;
      if (animal1 == null || animal1.AvailabilityStatus == RanchSelectEntry.Status.Available)
        this.SetUnavailableText(animal2.AvailabilityStatus);
    }
    else
    {
      this.animalSpine2.color = Color.black;
      this.textAnimalName2.text = "";
    }
    this.canvasGroup.transform.DOKill();
    this.canvasGroup.transform.DOScale(1.2f, 0.0f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    this.canvasGroup.transform.DOScale(1f, 0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void ResizeSkeleton(SkeletonGraphic skeleton, InventoryItem.ITEM_TYPE animalType)
  {
    if (animalType == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA || animalType == InventoryItem.ITEM_TYPE.ANIMAL_SNAIL)
      skeleton.transform.localScale = Vector3.one;
    else
      skeleton.transform.localScale = Vector3.one * 1.2785f;
  }

  public void SetUnavailableText(RanchSelectEntry.Status availability)
  {
    if (availability > RanchSelectEntry.Status.Unavailable)
    {
      this.unavailableContainer.SetActive(true);
      this.unavailableText.text = $"{ScriptLocalization.UI_RanchSelect.Unavailable}: {LocalizationManager.GetTranslation($"UI/RanchSelect/{availability}")}";
    }
    else if (availability == RanchSelectEntry.Status.Unavailable)
    {
      this.unavailableContainer.SetActive(true);
      this.unavailableText.text = ScriptLocalization.UI_RanchSelect.Unavailable;
    }
    else
      this.unavailableContainer.SetActive(false);
  }
}
