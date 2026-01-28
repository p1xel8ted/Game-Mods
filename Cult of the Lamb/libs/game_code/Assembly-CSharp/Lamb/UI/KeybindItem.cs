// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeybindItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using src.Extensions;
using UnityEngine;
using UnityEngine.Serialization;

#nullable disable
namespace Lamb.UI;

public class KeybindItem : MonoBehaviour
{
  [Header("Action")]
  [SerializeField]
  [ActionIdProperty(typeof (RewiredConsts.Category))]
  public int _category;
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  [SerializeField]
  [FormerlySerializedAs("Action")]
  public int _action;
  [SerializeField]
  public Pole _axisContribution;
  [SerializeField]
  [TermsPopup("")]
  public string _bindingTerm;
  [SerializeField]
  public Localize _localize;
  [Header("Bindings")]
  [SerializeField]
  public bool _isRebindable = true;
  [SerializeField]
  public bool _showKeyboardBinding = true;
  [SerializeField]
  public BindingItem _keyboardBinding;
  [SerializeField]
  public bool _showMouseBinding = true;
  [SerializeField]
  public BindingItem _mousebinding;
  [SerializeField]
  public bool _showJoystickBinding = true;
  [SerializeField]
  public BindingItem _joystickBinding;
  [Header("Misc")]
  [SerializeField]
  public KeybindConflictLookup _keybindConflictLookup;
  [SerializeField]
  public GameObject _alertContainer;
  [SerializeField]
  public GameObject _lockContainer;
  [SerializeField]
  public MMButton _button;

  public int Category => this._category;

  public int Action => this._action;

  public Pole AxisContribution => this._axisContribution;

  public bool IsRebindable => this._isRebindable;

  public bool ShowKeyboardBinding => this._showKeyboardBinding;

  public bool ShowMouseBinding => this._showMouseBinding;

  public bool ShowJoystickBinding => this._showJoystickBinding;

  public BindingItem KeyboardBinding => this._keyboardBinding;

  public BindingItem MouseBinding => this._mousebinding;

  public BindingItem JoystickBinding => this._joystickBinding;

  public MMButton Button => this._button;

  public KeybindConflictLookup KeybindConflictLookup => this._keybindConflictLookup;

  public void OnValidate()
  {
    if ((UnityEngine.Object) this._localize != (UnityEngine.Object) null)
      this._localize.Term = this._bindingTerm;
    if ((UnityEngine.Object) this._keyboardBinding != (UnityEngine.Object) null)
    {
      if (this._showKeyboardBinding)
      {
        this._keyboardBinding.Category = this._category;
        this._keyboardBinding.Action = this._action;
        this._keyboardBinding.BindingTerm = this._bindingTerm;
        this._keyboardBinding.AxisContribution = this._axisContribution;
        this._keyboardBinding.ControllerType = ControllerType.Keyboard;
      }
      if (this._keyboardBinding.gameObject.activeSelf != this._showKeyboardBinding)
        this._keyboardBinding.gameObject.SetActive(this._showKeyboardBinding);
    }
    if ((UnityEngine.Object) this._mousebinding != (UnityEngine.Object) null)
    {
      if (this._showMouseBinding)
      {
        this._mousebinding.Category = this._category;
        this._mousebinding.Action = this._action;
        this._mousebinding.BindingTerm = this._bindingTerm;
        this._mousebinding.AxisContribution = this._axisContribution;
        this._mousebinding.ControllerType = ControllerType.Mouse;
      }
      if (this._mousebinding.gameObject.activeSelf != this._showMouseBinding)
        this._mousebinding.gameObject.SetActive(this._showMouseBinding);
    }
    if ((UnityEngine.Object) this._joystickBinding != (UnityEngine.Object) null)
    {
      if (this._showJoystickBinding)
      {
        this._joystickBinding.Category = this._category;
        this._joystickBinding.Action = this._action;
        this._joystickBinding.BindingTerm = this._bindingTerm;
        this._joystickBinding.AxisContribution = this._axisContribution;
        this._joystickBinding.ControllerType = ControllerType.Joystick;
      }
      if (this._joystickBinding.gameObject.activeSelf != this._showJoystickBinding)
        this._joystickBinding.gameObject.SetActive(this._showJoystickBinding);
    }
    if (!((UnityEngine.Object) this._lockContainer != (UnityEngine.Object) null) || this._lockContainer.activeSelf == !this._isRebindable)
      return;
    this._lockContainer.SetActive(!this._isRebindable);
  }

  public void Awake() => this.UpdateBindingWarning();

  public void OnEnable()
  {
    ControlSettingsUtilities.OnBindingReset += new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnRebind += new System.Action<Binding>(this.OnRebind);
    GeneralInputSource.OnBindingsReset += new System.Action(this.UpdateBindingWarning);
  }

  public void OnDisable()
  {
    ControlSettingsUtilities.OnBindingReset -= new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnRebind -= new System.Action<Binding>(this.OnRebind);
    GeneralInputSource.OnBindingsReset -= new System.Action(this.UpdateBindingWarning);
  }

  public void OnRebind(Binding binding) => this.OnBindingReset(binding.Action);

  public void OnBindingReset(int action)
  {
    if (action != this._action)
      return;
    this.UpdateBindingWarning();
  }

  public void UpdateBindingWarning()
  {
    bool flag = false;
    if ((UnityEngine.Object) this._keyboardBinding != (UnityEngine.Object) null && this._showKeyboardBinding)
    {
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(this._category, ControllerType.Keyboard);
      if (controllerMapForCategory != null && controllerMapForCategory.GetActionElementMap(this._action, this._axisContribution) != null)
        flag = true;
    }
    if ((UnityEngine.Object) this._mousebinding != (UnityEngine.Object) null && this._showMouseBinding)
    {
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(this._category, ControllerType.Mouse);
      if (controllerMapForCategory != null && controllerMapForCategory.GetActionElementMap(this._action, this._axisContribution) != null)
        flag = true;
    }
    if ((UnityEngine.Object) this._joystickBinding != (UnityEngine.Object) null && this._showJoystickBinding)
    {
      ControllerMap controllerMapForCategory = InputManager.General.GetControllerMapForCategory(this._category, InputManager.General.GetLastActiveController());
      if (controllerMapForCategory != null && controllerMapForCategory.GetActionElementMap(this._action, this._axisContribution) != null)
        flag = true;
    }
    this._alertContainer.SetActive(!flag);
  }
}
