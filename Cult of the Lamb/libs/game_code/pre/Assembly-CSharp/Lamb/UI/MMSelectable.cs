// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
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
  [SerializeField]
  public string _confirmSFX = "event:/ui/confirm_selection";
  [SerializeField]
  public string _confirmDeniedSFX = "";

  public Selectable Selectable => (Selectable) this;

  public virtual bool Interactable
  {
    get => this.interactable;
    set => this.interactable = value;
  }

  public bool Confirmable { set; get; }

  public void SetNormalTransitionState()
  {
    this.DoStateTransition(Selectable.SelectionState.Normal, true);
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    base.OnPointerEnter(eventData);
    if (!this.Interactable)
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
    if (!this.Interactable)
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
