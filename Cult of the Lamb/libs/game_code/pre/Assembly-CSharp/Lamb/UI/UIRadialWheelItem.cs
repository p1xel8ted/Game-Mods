// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRadialWheelItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class UIRadialWheelItem : MonoBehaviour
{
  private const string kSelectedAnimatorState = "NormalToPressed";
  private const string kDeselectedAnimatorState = "PressedToNormal";
  private const string kInactiveAnimatorState = "Inactive";
  private const string kActiveAnimatorState = "Normal";
  private const string kSelectedInactiveAnimatorState = "InactiveToPressed";
  private const string kDeselectedInactiveAnimatorState = "PressedToInactive";
  [SerializeField]
  protected MMButton _button;
  [SerializeField]
  protected Animator _animator;
  [SerializeField]
  protected CanvasGroup _canvasGroup;
  private bool _selected;
  private bool _inactive;

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
