// Decompiled with JetBrains decompiler
// Type: UITrait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UITrait : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  [SerializeField]
  public Image icon;
  [SerializeField]
  public GameObject selectedBorder;
  [SerializeField]
  public GameObject removeIcon;
  [SerializeField]
  public GameObject descriptionContainer;
  [SerializeField]
  public TMP_Text traitTitle;
  [SerializeField]
  public TMP_Text traitDescription;
  [CompilerGenerated]
  public FollowerTrait.TraitType \u003CTrait\u003Ek__BackingField;

  public FollowerTrait.TraitType Trait
  {
    get => this.\u003CTrait\u003Ek__BackingField;
    set => this.\u003CTrait\u003Ek__BackingField = value;
  }

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
