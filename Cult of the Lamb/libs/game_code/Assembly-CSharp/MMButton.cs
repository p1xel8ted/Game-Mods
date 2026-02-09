// Decompiled with JetBrains decompiler
// Type: MMButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Rewired.Integration.UnityUI;
using src.UINavigator;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebSocketSharp;

#nullable disable
public class MMButton : Button, IMMSelectable, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  public System.Action OnSelected;
  public System.Action OnDeselected;
  public System.Action OnConfirmDenied;
  public System.Action OnPointerEntered;
  public System.Action OnPointerExited;
  public System.Action OnTryNavigateRight;
  public System.Action OnTryNavigateLeft;
  public bool PreventMouseSelection;
  public PlayerFarming _playerFarming;
  [CompilerGenerated]
  public bool \u003CConfirmable\u003Ek__BackingField = true;
  [SerializeField]
  public MaskableGraphic[] _targetGraphics;
  [SerializeField]
  public bool _disableControlTransfer;
  [SerializeField]
  public string _confirmSFX = "event:/ui/confirm_selection";
  [SerializeField]
  public string _confirmDeniedSFX = "";
  [SerializeField]
  public bool _vibrateOnConfirm = true;
  [SerializeField]
  public bool _vibrateOnDeny = true;
  public bool AlwaysAllowMouseInput;

  public Selectable Selectable => (Selectable) this;

  public bool Interactable
  {
    get => this.interactable;
    set => this.interactable = value;
  }

  public PlayerFarming playerFarming
  {
    get => this._playerFarming;
    set => this._playerFarming = value;
  }

  public bool Confirmable
  {
    set => this.\u003CConfirmable\u003Ek__BackingField = value;
    get => this.\u003CConfirmable\u003Ek__BackingField;
  }

  public MaskableGraphic[] TargetGraphics => this._targetGraphics;

  public IMMSelectable FindSelectableFromDirection(Vector3 direction)
  {
    return this.Selectable.FindSelectable(direction) as IMMSelectable;
  }

  public void SetNormalTransitionState()
  {
    try
    {
      this.enabled = false;
      this.enabled = true;
      this.DoStateTransition(Selectable.SelectionState.Normal, true);
    }
    catch
    {
    }
  }

  public override void DoStateTransition(Selectable.SelectionState state, bool instant)
  {
    base.DoStateTransition(state, instant);
    if (this._targetGraphics == null)
      return;
    Color targetColor = this.GetTargetColor(state, this.colors);
    foreach (MaskableGraphic targetGraphic in this._targetGraphics)
    {
      if ((UnityEngine.Object) targetGraphic != (UnityEngine.Object) null)
      {
        SelectableColourProxy component;
        if (targetGraphic.TryGetComponent<SelectableColourProxy>(out component))
          targetColor = this.GetTargetColor(state, component.Colors);
        targetGraphic.CrossFadeColor(targetColor, instant ? 0.0f : this.colors.fadeDuration, true, true);
      }
    }
  }

  public Color GetTargetColor(Selectable.SelectionState state, ColorBlock colorBlock)
  {
    switch (state)
    {
      case Selectable.SelectionState.Normal:
        return colorBlock.normalColor;
      case Selectable.SelectionState.Highlighted:
        return colorBlock.highlightedColor;
      case Selectable.SelectionState.Pressed:
        return colorBlock.pressedColor;
      case Selectable.SelectionState.Selected:
        return colorBlock.selectedColor;
      case Selectable.SelectionState.Disabled:
        return colorBlock.disabledColor;
      default:
        return Color.white;
    }
  }

  public override void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left)
      return;
    bool flag = !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if (!this.IsActive() || !this.IsInteractable() || !flag)
      return;
    this.TryPerformConfirmAction();
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    bool flag = !this.PreventMouseSelection && !InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if (!this.IsActive() || !this.IsInteractable() || !flag)
      return;
    base.OnPointerEnter(eventData);
    System.Action onPointerEntered = this.OnPointerEntered;
    if (onPointerEntered != null)
      onPointerEntered();
    if (this._disableControlTransfer)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this);
  }

  public override void OnPointerExit(PointerEventData eventData)
  {
    if (!this.IsActive() || !this.IsInteractable())
      return;
    base.OnPointerExit(eventData);
    System.Action onPointerExited = this.OnPointerExited;
    if (onPointerExited == null)
      return;
    onPointerExited();
  }

  public void SetInteractionState(bool state)
  {
    if (!this.enabled)
      return;
    this.Interactable = state;
    Graphic component = this.GetComponent<Graphic>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.raycastTarget = state;
    foreach (Graphic componentsInChild in this.GetComponentsInChildren<Graphic>())
      componentsInChild.raycastTarget = state;
  }

  public override void OnSelect(BaseEventData eventData)
  {
    if (this.PreventMouseSelection && eventData is PlayerPointerEventData && ((PlayerPointerEventData) eventData).mouseSource != null)
      return;
    base.OnSelect(eventData);
    if (!this.Interactable)
      return;
    System.Action onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected();
  }

  public override void OnDeselect(BaseEventData eventData)
  {
    base.OnDeselect(eventData);
    if (!this.Interactable)
      return;
    System.Action onDeselected = this.OnDeselected;
    if (onDeselected == null)
      return;
    onDeselected();
  }

  public IMMSelectable TryNavigateLeft()
  {
    System.Action onTryNavigateLeft = this.OnTryNavigateLeft;
    if (onTryNavigateLeft != null)
      onTryNavigateLeft();
    return this.FindSelectableOnLeft() as IMMSelectable;
  }

  public IMMSelectable TryNavigateRight()
  {
    System.Action tryNavigateRight = this.OnTryNavigateRight;
    if (tryNavigateRight != null)
      tryNavigateRight();
    return this.FindSelectableOnRight() as IMMSelectable;
  }

  public IMMSelectable TryNavigateUp() => this.FindSelectableOnUp() as IMMSelectable;

  public IMMSelectable TryNavigateDown() => this.FindSelectableOnDown() as IMMSelectable;

  public bool TryPerformConfirmAction()
  {
    if (this.Confirmable)
    {
      this.onClick?.Invoke();
      if (!this._confirmSFX.IsNullOrEmpty())
      {
        UIManager.PlayAudio(this._confirmSFX);
        if (this._vibrateOnConfirm)
          RumbleManager.Instance.Rumble();
      }
      return true;
    }
    System.Action onConfirmDenied = this.OnConfirmDenied;
    if (onConfirmDenied != null)
      onConfirmDenied();
    if (!this._confirmDeniedSFX.IsNullOrEmpty())
    {
      UIManager.PlayAudio(this._confirmDeniedSFX);
      if (this._vibrateOnDeny)
        RumbleManager.Instance.Rumble();
    }
    return false;
  }
}
