// Decompiled with JetBrains decompiler
// Type: UITrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UITrait : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  [SerializeField]
  private Image icon;
  [SerializeField]
  private GameObject selectedBorder;
  [SerializeField]
  private GameObject removeIcon;
  [SerializeField]
  private GameObject descriptionContainer;
  [SerializeField]
  private TMP_Text traitTitle;
  [SerializeField]
  private TMP_Text traitDescription;

  public FollowerTrait.TraitType Trait { get; private set; }

  public void Play(FollowerTrait.TraitType trait)
  {
    this.Trait = trait;
    this.icon.sprite = FollowerTrait.GetIcon(trait);
    this.removeIcon.SetActive(false);
    this.traitTitle.text = FollowerTrait.GetLocalizedTitle(trait);
    this.traitDescription.text = FollowerTrait.GetLocalizedDescription(trait);
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.descriptionContainer.SetActive(true);
    this.selectedBorder.SetActive(true);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.descriptionContainer.SetActive(false);
    this.selectedBorder.SetActive(false);
  }
}
