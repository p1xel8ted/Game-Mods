// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using src.UINavigator;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebSocketSharp;

#nullable disable
namespace Lamb.UI;

public class MMSelectable : Selectable, IMMSelectable, ISelectHandler, IEventSystemHandler
{
  public System.Action OnSelected;
  public System.Action OnConfirm;
  public System.Action OnConfirmDenied;
  public System.Action OnDeselected;
  public System.Action OnPointerEntered;
  public System.Action OnPointerExited;
  public PlayerFarming _playerFarming;
  [CompilerGenerated]
  public bool \u003CConfirmable\u003Ek__BackingField;
  [SerializeField]
  public MaskableGraphic[] _targetGraphics;
  [SerializeField]
  public string _confirmSFX = "event:/ui/confirm_selection";
  [SerializeField]
  public string _confirmDeniedSFX = "";
  [SerializeField]
  public bool _disableControlTransfer;
  public Color _startingColourHighConstrast;

  public Selectable Selectable => (Selectable) this;

  public virtual bool Interactable
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

  public void SetNormalTransitionState()
  {
    this.DoStateTransition(Selectable.SelectionState.Normal, true);
  }

  public override void DoStateTransition(Selectable.SelectionState state, bool instant)
  {
    base.DoStateTransition(state, instant);
    if (this._targetGraphics == null)
      return;
    ColorBlock colors;
    Color color1;
    switch (state)
    {
      case Selectable.SelectionState.Normal:
        colors = this.colors;
        color1 = colors.normalColor;
        break;
      case Selectable.SelectionState.Highlighted:
        colors = this.colors;
        color1 = colors.highlightedColor;
        break;
      case Selectable.SelectionState.Pressed:
        colors = this.colors;
        color1 = colors.pressedColor;
        break;
      case Selectable.SelectionState.Selected:
        colors = this.colors;
        color1 = colors.selectedColor;
        break;
      case Selectable.SelectionState.Disabled:
        colors = this.colors;
        color1 = colors.disabledColor;
        break;
      default:
        color1 = Color.white;
        break;
    }
    Color color2 = color1;
    foreach (MaskableGraphic targetGraphic in this._targetGraphics)
    {
      if ((UnityEngine.Object) targetGraphic != (UnityEngine.Object) null)
      {
        MaskableGraphic maskableGraphic = targetGraphic;
        Color targetColor = color2;
        double duration;
        if (!instant)
        {
          colors = this.colors;
          duration = (double) colors.fadeDuration;
        }
        else
          duration = 0.0;
        maskableGraphic.CrossFadeColor(targetColor, (float) duration, true, true);
      }
    }
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    base.OnPointerEnter(eventData);
    if (!this.Interactable || !InputManager.General.MouseInputEnabled || this._disableControlTransfer)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this);
    System.Action onPointerEntered = this.OnPointerEntered;
    if (onPointerEntered == null)
      return;
    onPointerEntered();
  }

  public override void OnPointerExit(PointerEventData eventData)
  {
    base.OnPointerExit(eventData);
    if (!this.Interactable || !InputManager.General.MouseInputEnabled)
      return;
    System.Action onPointerExited = this.OnPointerExited;
    if (onPointerExited == null)
      return;
    onPointerExited();
  }

  public void SetInteractionState(bool state)
  {
    this.Interactable = state;
    Graphic component = this.GetComponent<Graphic>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.raycastTarget = state;
    foreach (Graphic componentsInChild in this.GetComponentsInChildren<Graphic>(true))
      componentsInChild.raycastTarget = state;
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

  public virtual bool TryPerformConfirmAction()
  {
    if (this.Confirmable)
    {
      System.Action onConfirm = this.OnConfirm;
      if (onConfirm != null)
        onConfirm();
      if (!this._confirmSFX.IsNullOrEmpty())
      {
        UIManager.PlayAudio(this._confirmSFX);
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
      RumbleManager.Instance.Rumble();
    }
    return false;
  }

  public virtual IMMSelectable TryNavigateLeft() => this.FindSelectableOnLeft() as IMMSelectable;

  public virtual IMMSelectable TryNavigateRight() => this.FindSelectableOnRight() as IMMSelectable;

  public virtual IMMSelectable TryNavigateUp() => this.FindSelectableOnUp() as IMMSelectable;

  public virtual IMMSelectable TryNavigateDown() => this.FindSelectableOnDown() as IMMSelectable;

  public IMMSelectable FindSelectableFromDirection(Vector3 direction)
  {
    return this.Selectable.FindSelectable(direction) as IMMSelectable;
  }

  public override void OnSelect(BaseEventData eventData)
  {
    base.OnSelect(eventData);
    System.Action onSelected = this.OnSelected;
    if (onSelected == null)
      return;
    onSelected();
  }
}
