// Decompiled with JetBrains decompiler
// Type: Lamb.UI.IndoctrinationTraitItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class IndoctrinationTraitItem : MonoBehaviour
{
  [SerializeField]
  public MMSelectable _selectable;
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public Image _arrow;
  [Header("Arrows")]
  [SerializeField]
  public Sprite _positiveArrow;
  [SerializeField]
  public Sprite _negativeArrow;
  [SerializeField]
  public GameObject _deactivated;
  [SerializeField]
  public GameObject _randomIcon;
  [SerializeField]
  public GameObject manipulationShuffle;
  [SerializeField]
  public GameObject manipulationAdd;
  [SerializeField]
  public GameObject manipulationMinus;
  public FollowerTrait.TraitType _traitType;
  public Animator animator;

  public MMSelectable Selectable => this._selectable;

  public MMButton Button => this._button;

  public FollowerTrait.TraitType TraitType => this._traitType;

  public void Configure(FollowerTrait.TraitType traitType)
  {
    this._selectable = this.GetComponent<MMSelectable>();
    this._traitType = traitType;
    if (traitType == FollowerTrait.TraitType.None && (UnityEngine.Object) this._randomIcon != (UnityEngine.Object) null)
    {
      this._selectable.interactable = false;
      this._randomIcon.gameObject.SetActive(true);
      this._icon.gameObject.SetActive(false);
      this._arrow.gameObject.SetActive(false);
    }
    else
    {
      this._icon.sprite = FollowerTrait.GetIcon(this._traitType);
      this._arrow.sprite = FollowerTrait.IsPositiveTrait(this._traitType) ? this._positiveArrow : this._negativeArrow;
    }
    if ((UnityEngine.Object) this._button != (UnityEngine.Object) null)
    {
      this.animator = this.GetComponent<Animator>();
      this._button.OnSelected += (System.Action) (() => this.animator.SetTrigger("Highlighted"));
      this._button.OnDeselected += (System.Action) (() => this.animator.SetTrigger("Normal"));
      this._button.onClick.AddListener((UnityAction) (() => this.animator.SetTrigger("Selected")));
    }
    if ((UnityEngine.Object) this.manipulationShuffle != (UnityEngine.Object) null)
      this.manipulationShuffle.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.manipulationAdd != (UnityEngine.Object) null)
      this.manipulationAdd.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.manipulationMinus != (UnityEngine.Object) null))
      return;
    this.manipulationMinus.gameObject.SetActive(false);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) this._button != (UnityEngine.Object) null))
      return;
    this.SetDeactivated(!this._button.Confirmable);
  }

  public void SetState(UITraitManipulatorMenuController.Type type)
  {
    this.manipulationShuffle.gameObject.SetActive(type == UITraitManipulatorMenuController.Type.Shuffle);
    this.manipulationAdd.gameObject.SetActive(type == UITraitManipulatorMenuController.Type.Add);
    this.manipulationMinus.gameObject.SetActive(type == UITraitManipulatorMenuController.Type.Remove);
    this._arrow.gameObject.SetActive(false);
  }

  public void SetDeactivated(bool state)
  {
    if (!((UnityEngine.Object) this._deactivated != (UnityEngine.Object) null))
      return;
    this._deactivated.SetActive(state);
  }

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__19_1() => this.animator.SetTrigger("Highlighted");

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__19_2() => this.animator.SetTrigger("Normal");

  [CompilerGenerated]
  public void \u003CConfigure\u003Eb__19_0() => this.animator.SetTrigger("Selected");
}
