// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMInputField
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.UINavigator;
using System;
using TMPro;
using UI.Keyboards;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMInputField : TMP_InputField, IMMSelectable, IKeyboardDelegate
{
  public System.Action OnSelected;
  public System.Action OnDeselected;
  public System.Action OnPointerEntered;
  public System.Action OnPointerExited;
  public System.Action OnStartedEditing;
  public Action<string> OnEndedEditing;
  public PlayerFarming _playerFarming;
  [SerializeField]
  public string _confirmSFX = "event:/ui/confirm_selection";
  public ControllerType _controllerType;
  public MMOnScreenKeyboard _onScreenKeyboard;

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

  public override void OnEnable()
  {
    base.OnEnable();
    if (!Application.isPlaying)
      return;
    this.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
    this._controllerType = InputManager.General.GetLastActiveController().type;
    InputManager.General.OnActiveControllerChanged += new Action<Controller>(this.OnActiveControllerChanged);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!Application.isPlaying)
      return;
    this.onEndEdit.RemoveListener(new UnityAction<string>(this.OnEndEdit));
    InputManager.General.OnActiveControllerChanged -= new Action<Controller>(this.OnActiveControllerChanged);
  }

  public void OnActiveControllerChanged(Controller controller)
  {
    this._controllerType = controller.type;
  }

  public void SetNormalTransitionState()
  {
    this.DoStateTransition(Selectable.SelectionState.Normal, true);
  }

  public override void OnPointerEnter(PointerEventData eventData)
  {
    base.OnPointerEnter(eventData);
    if (!this.Interactable || this.isFocused)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this);
    System.Action onPointerEntered = this.OnPointerEntered;
    if (onPointerEntered == null)
      return;
    onPointerEntered();
  }

  public override void OnPointerClick(PointerEventData eventData)
  {
    if (!this.Interactable || eventData.button != PointerEventData.InputButton.Left || this.isFocused)
      return;
    this.TryPerformConfirmAction();
  }

  public override void OnPointerExit(PointerEventData eventData)
  {
    base.OnPointerExit(eventData);
    if (!this.Interactable || this.isFocused)
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
    foreach (Graphic componentsInChild in this.GetComponentsInChildren<Graphic>())
      componentsInChild.raycastTarget = state;
  }

  public virtual bool TryPerformConfirmAction()
  {
    if (this.isFocused)
      return false;
    if (MMOnScreenKeyboard.RequiresOnScreenKeyboard())
    {
      this.Select();
      this.ActivateInputField();
      this._onScreenKeyboard = this.gameObject.AddComponent<MMOnScreenKeyboard>();
      Debug.Log((object) "Send Open Keyboard");
      this._onScreenKeyboard.Show((IKeyboardDelegate) this, this);
    }
    else
      this.ActivateInputField();
    if (!this.isFocused)
    {
      Debug.Log((object) "OnStartedEditing");
      System.Action onStartedEditing = this.OnStartedEditing;
      if (onStartedEditing != null)
        onStartedEditing();
      UIManager.PlayAudio(this._confirmSFX);
      RumbleManager.Instance.Rumble();
    }
    return true;
  }

  public void KeyboardDismissed(string result)
  {
    this.text = result;
    UnityEngine.Object.Destroy((UnityEngine.Object) this._onScreenKeyboard);
    this._onScreenKeyboard = (MMOnScreenKeyboard) null;
  }

  public void Update()
  {
    if (this._controllerType != ControllerType.Joystick || !InputManager.UI.GetCancelButtonDown())
      return;
    this.DeactivateInputField();
  }

  public virtual IMMSelectable TryNavigateLeft()
  {
    return this.isFocused ? (IMMSelectable) null : this.FindSelectableOnLeft() as IMMSelectable;
  }

  public virtual IMMSelectable TryNavigateRight()
  {
    return this.isFocused ? (IMMSelectable) null : this.FindSelectableOnRight() as IMMSelectable;
  }

  public virtual IMMSelectable TryNavigateUp()
  {
    return this.isFocused ? (IMMSelectable) null : this.FindSelectableOnUp() as IMMSelectable;
  }

  public virtual IMMSelectable TryNavigateDown()
  {
    return this.isFocused ? (IMMSelectable) null : this.FindSelectableOnDown() as IMMSelectable;
  }

  public IMMSelectable FindSelectableFromDirection(Vector3 direction)
  {
    return this.Selectable.FindSelectable(direction) as IMMSelectable;
  }

  public override void OnPointerDown(PointerEventData pointerEventData)
  {
    base.OnPointerDown(pointerEventData);
    System.Action onStartedEditing = this.OnStartedEditing;
    if (onStartedEditing == null)
      return;
    onStartedEditing();
  }

  public override void OnSelect(BaseEventData eventData)
  {
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

  public void OnEndEdit(string str)
  {
    Debug.Log((object) "Closed Keyboard OnEndEdit");
    str = str.StripNullTerminator();
    this.m_Text = str.StripHtml();
    Action<string> onEndedEditing = this.OnEndedEditing;
    if (onEndedEditing == null)
      return;
    onEndedEditing(str);
  }
}
