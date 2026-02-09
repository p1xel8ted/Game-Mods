// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRadialWheelItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class UIRadialWheelItem : MonoBehaviour
{
  public const string kSelectedAnimatorState = "NormalToPressed";
  public const string kDeselectedAnimatorState = "PressedToNormal";
  public const string kInactiveAnimatorState = "Inactive";
  public const string kActiveAnimatorState = "Normal";
  public const string kSelectedInactiveAnimatorState = "InactiveToPressed";
  public const string kDeselectedInactiveAnimatorState = "PressedToInactive";
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public Animator _animator;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  public bool _selected;
  public bool _inactive;

  public MMButton Button => this._button;

  public Vector2 Vector => (Vector2) this.transform.localPosition.normalized;

  public virtual void DoSelected()
  {
    if (this._selected)
      return;
    UIManager.PlayAudio("event:/ui/change_selection");
    if (this._inactive)
      this._animator.Play("InactiveToPressed");
    else
      this._animator.Play("NormalToPressed");
    this._selected = true;
  }

  public virtual void DoDeselected()
  {
    if (!this._selected)
      return;
    if (this._inactive)
      this._animator.Play("PressedToInactive");
    else
      this._animator.Play("PressedToNormal");
    this._selected = false;
  }

  public virtual void DoInactive()
  {
    this._animator.Play("Inactive");
    this._inactive = true;
  }

  public virtual void DoActive()
  {
    this._animator.Rebind();
    this._animator.Play("Normal");
    this._inactive = false;
  }

  public abstract string GetTitle();

  public abstract string GetDescription();

  public abstract bool IsValidOption();

  public abstract bool Visible();
}
