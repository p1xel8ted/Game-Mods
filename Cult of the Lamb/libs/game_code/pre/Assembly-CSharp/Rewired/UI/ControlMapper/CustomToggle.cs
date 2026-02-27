// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CustomToggle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class CustomToggle : Toggle, ICustomSelectable, ICancelHandler, IEventSystemHandler
{
  [SerializeField]
  private Sprite _disabledHighlightedSprite;
  [SerializeField]
  private Color _disabledHighlightedColor;
  [SerializeField]
  private string _disabledHighlightedTrigger;
  [SerializeField]
  private bool _autoNavUp = true;
  [SerializeField]
  private bool _autoNavDown = true;
  [SerializeField]
  private bool _autoNavLeft = true;
  [SerializeField]
  private bool _autoNavRight = true;
  private bool isHighlightDisabled;

  public Sprite disabledHighlightedSprite
  {
    get => this._disabledHighlightedSprite;
    set => this._disabledHighlightedSprite = value;
  }

  public Color disabledHighlightedColor
  {
    get => this._disabledHighlightedColor;
    set => this._disabledHighlightedColor = value;
  }

  public string disabledHighlightedTrigger
  {
    get => this._disabledHighlightedTrigger;
    set => this._disabledHighlightedTrigger = value;
  }

  public bool autoNavUp
  {
    get => this._autoNavUp;
    set => this._autoNavUp = value;
  }

  public bool autoNavDown
  {
    get => this._autoNavDown;
    set => this._autoNavDown = value;
  }

  public bool autoNavLeft
  {
    get => this._autoNavLeft;
    set => this._autoNavLeft = value;
  }

  public bool autoNavRight
  {
    get => this._autoNavRight;
    set => this._autoNavRight = value;
  }

  private bool isDisabled => !this.IsInteractable();

  private event UnityAction _CancelEvent;

  public event UnityAction CancelEvent
  {
    add => this._CancelEvent += value;
    remove => this._CancelEvent -= value;
  }

  public override Selectable FindSelectableOnLeft()
  {
    return (this.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavLeft ? UISelectionUtility.FindNextSelectable((Selectable) this, this.transform, Vector3.left) : base.FindSelectableOnLeft();
  }

  public override Selectable FindSelectableOnRight()
  {
    return (this.navigation.mode & Navigation.Mode.Horizontal) != Navigation.Mode.None || this._autoNavRight ? UISelectionUtility.FindNextSelectable((Selectable) this, this.transform, Vector3.right) : base.FindSelectableOnRight();
  }

  public override Selectable FindSelectableOnUp()
  {
    return (this.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavUp ? UISelectionUtility.FindNextSelectable((Selectable) this, this.transform, Vector3.up) : base.FindSelectableOnUp();
  }

  public override Selectable FindSelectableOnDown()
  {
    return (this.navigation.mode & Navigation.Mode.Vertical) != Navigation.Mode.None || this._autoNavDown ? UISelectionUtility.FindNextSelectable((Selectable) this, this.transform, Vector3.down) : base.FindSelectableOnDown();
  }

  protected override void OnCanvasGroupChanged()
  {
    base.OnCanvasGroupChanged();
    if ((Object) EventSystem.current == (Object) null)
      return;
    this.EvaluateHightlightDisabled((Object) EventSystem.current.currentSelectedGameObject == (Object) this.gameObject);
  }

  protected override void DoStateTransition(Selectable.SelectionState state, bool instant)
  {
    if (this.isHighlightDisabled)
    {
      Color highlightedColor = this._disabledHighlightedColor;
      Sprite highlightedSprite = this._disabledHighlightedSprite;
      string highlightedTrigger = this._disabledHighlightedTrigger;
      if (!this.gameObject.activeInHierarchy)
        return;
      switch (this.transition)
      {
        case Selectable.Transition.ColorTint:
          this.StartColorTween(highlightedColor * this.colors.colorMultiplier, instant);
          break;
        case Selectable.Transition.SpriteSwap:
          this.DoSpriteSwap(highlightedSprite);
          break;
        case Selectable.Transition.Animation:
          this.TriggerAnimation(highlightedTrigger);
          break;
      }
    }
    else
      base.DoStateTransition(state, instant);
  }

  private void StartColorTween(Color targetColor, bool instant)
  {
    if ((Object) this.targetGraphic == (Object) null)
      return;
    this.targetGraphic.CrossFadeColor(targetColor, instant ? 0.0f : this.colors.fadeDuration, true, true);
  }

  private void DoSpriteSwap(Sprite newSprite)
  {
    if ((Object) this.image == (Object) null)
      return;
    this.image.overrideSprite = newSprite;
  }

  private void TriggerAnimation(string triggername)
  {
    if ((Object) this.animator == (Object) null || !this.animator.enabled || !this.animator.isActiveAndEnabled || (Object) this.animator.runtimeAnimatorController == (Object) null || string.IsNullOrEmpty(triggername))
      return;
    this.animator.ResetTrigger(this._disabledHighlightedTrigger);
    this.animator.SetTrigger(triggername);
  }

  public override void OnSelect(BaseEventData eventData)
  {
    base.OnSelect(eventData);
    this.EvaluateHightlightDisabled(true);
  }

  public override void OnDeselect(BaseEventData eventData)
  {
    base.OnDeselect(eventData);
    this.EvaluateHightlightDisabled(false);
  }

  private void EvaluateHightlightDisabled(bool isSelected)
  {
    if (!isSelected)
    {
      if (!this.isHighlightDisabled)
        return;
      this.isHighlightDisabled = false;
      this.DoStateTransition(this.isDisabled ? Selectable.SelectionState.Disabled : this.currentSelectionState, false);
    }
    else
    {
      if (!this.isDisabled)
        return;
      this.isHighlightDisabled = true;
      this.DoStateTransition(Selectable.SelectionState.Disabled, false);
    }
  }

  public void OnCancel(BaseEventData eventData)
  {
    if (this._CancelEvent == null)
      return;
    this._CancelEvent();
  }
}
