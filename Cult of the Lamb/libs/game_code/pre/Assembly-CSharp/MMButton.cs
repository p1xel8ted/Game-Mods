// Decompiled with JetBrains decompiler
// Type: MMButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using src.UINavigator;
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
  [SerializeField]
  private bool _disableControlTransfer;
  [SerializeField]
  public string _confirmSFX = "event:/ui/confirm_selection";
  [SerializeField]
  public string _confirmDeniedSFX = "";
  [SerializeField]
  public bool _vibrateOnConfirm = true;
  [SerializeField]
  public bool _vibrateOnDeny = true;

  public Selectable Selectable => (Selectable) this;

  public bool Interactable
  {
    get => this.interactable;
    set => this.interactable = value;
  }

  public bool Confirmable { set; get; } = true;

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

  public override void OnPointerClick(PointerEventData eventData)
  {
    if (eventData.button != PointerEventData.InputButton.Left || !this.IsActive() || !this.IsInteractable())
      return;
    this.TryPerformConfirmAction();
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    if (!this.IsActive() || !this.IsInteractable())
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

  public IMMSelectable TryNavigateLeft() => this.FindSelectableOnLeft() as IMMSelectable;

  public IMMSelectable TryNavigateRight() => this.FindSelectableOnRight() as IMMSelectable;

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
