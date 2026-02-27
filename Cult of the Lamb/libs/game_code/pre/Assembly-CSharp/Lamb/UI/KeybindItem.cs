// Decompiled with JetBrains decompiler
// Type: Lamb.UI.KeybindItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private int _category;
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  [SerializeField]
  [FormerlySerializedAs("Action")]
  private int _action;
  [SerializeField]
  private Pole _axisContribution;
  [SerializeField]
  [TermsPopup("")]
  private string _bindingTerm;
  [SerializeField]
  private Localize _localize;
  [Header("Bindings")]
  [SerializeField]
  private bool _isRebindable = true;
  [SerializeField]
  private bool _showKeyboardBinding = true;
  [SerializeField]
  private BindingItem _keyboardBinding;
  [SerializeField]
  private bool _showMouseBinding = true;
  [SerializeField]
  private BindingItem _mousebinding;
  [SerializeField]
  private bool _showJoystickBinding = true;
  [SerializeField]
  private BindingItem _joystickBinding;
  [Header("Misc")]
  [SerializeField]
  private KeybindConflictLookup _keybindConflictLookup;
  [SerializeField]
  private GameObject _alertContainer;
  [SerializeField]
  private GameObject _lockContainer;
  [SerializeField]
  private MMButton _button;
  [SerializeField]
  private Animator _animator;

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

  private void OnValidate()
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

  private void Awake()
  {
    if ((UnityEngine.Object) this._keyboardBinding != (UnityEngine.Object) null && this._showKeyboardBinding)
    {
      this._button.OnSelected += new System.Action(this._keyboardBinding.ShowSelected);
      this._button.OnDeselected += new System.Action(this._keyboardBinding.ShowNormal);
    }
    if ((UnityEngine.Object) this._mousebinding != (UnityEngine.Object) null && this._showMouseBinding)
    {
      this._button.OnSelected += new System.Action(this._mousebinding.ShowSelected);
      this._button.OnDeselected += new System.Action(this._mousebinding.ShowNormal);
    }
    if ((UnityEngine.Object) this._joystickBinding != (UnityEngine.Object) null && this._showJoystickBinding)
    {
      this._button.OnSelected += new System.Action(this._joystickBinding.ShowSelected);
      this._button.OnDeselected += new System.Action(this._joystickBinding.ShowNormal);
    }
    this.UpdateBindingWarning();
  }

  private void OnEnable()
  {
    ControlSettingsUtilities.OnBindingReset += new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnRebind += new System.Action<Binding>(this.OnRebind);
    GeneralInputSource.OnBindingsReset += new System.Action(this.UpdateBindingWarning);
  }

  private void OnDisable()
  {
    ControlSettingsUtilities.OnBindingReset -= new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnRebind -= new System.Action<Binding>(this.OnRebind);
    GeneralInputSource.OnBindingsReset -= new System.Action(this.UpdateBindingWarning);
  }

  private void OnRebind(Binding binding) => this.OnBindingReset(binding.Action);

  private void OnBindingReset(int action)
  {
    if (action != this._action)
      return;
    this.UpdateBindingWarning();
  }

  private void UpdateBindingWarning()
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

  public void ShowSelected()
  {
    this._animator.ResetAllTriggers();
    this._animator.SetTrigger("Selected");
  }

  public void ShowNormal()
  {
    this._animator.ResetAllTriggers();
    this._animator.SetTrigger("Normal");
  }
}
