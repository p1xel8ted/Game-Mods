// Decompiled with JetBrains decompiler
// Type: MMTools.MMControlPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Rewired;
using src.Extensions;
using src.UINavigator;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#nullable disable
namespace MMTools;

public class MMControlPrompt : MonoBehaviour
{
  [Header("Binding")]
  [SerializeField]
  [ActionIdProperty(typeof (RewiredConsts.Category))]
  [FormerlySerializedAs("Category")]
  public int _category;
  [SerializeField]
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  [FormerlySerializedAs("Action")]
  public int _action;
  [ActionIdProperty(typeof (Pole))]
  public int AxisContribution;
  [SerializeField]
  public bool _prioritizeMouse = true;
  [SerializeField]
  public bool _prioritizeMouseFallbackToKeyboard = true;
  [Header("Prompt")]
  [SerializeField]
  public ControlMappings _controlMappings;
  [SerializeField]
  public TextMeshProUGUI _text;
  [SerializeField]
  public TextMeshProUGUI _iconText;
  [SerializeField]
  public TextMeshProUGUI _actionText;
  [SerializeField]
  public Image _icon;
  public CoopIndicatorIcon coopIndicatorIcon;
  [HideInInspector]
  public bool IgnoreControllerChange;
  [HideInInspector]
  public PlayerFarming playerFarming;

  public Image Icon => this._icon;

  public int Category
  {
    get => this._category;
    set
    {
      if (this._category == value)
        return;
      this._category = value;
      this.ForceUpdate();
    }
  }

  public int Action
  {
    get => this._action;
    set
    {
      if (this._action == value)
        return;
      this._action = value;
      this.ForceUpdate();
    }
  }

  public bool PrioritizeMouse
  {
    get => this._prioritizeMouse;
    set
    {
      if (this._prioritizeMouse == value)
        return;
      this._prioritizeMouse = value;
      this.ForceUpdate();
    }
  }

  public void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new System.Action<Controller>(this.OnActiveControllerChanged);
    ControlSettingsUtilities.OnGamepadLayoutChanged += new System.Action(this.ForceUpdate);
    ControlSettingsUtilities.OnRebind += new System.Action<Binding>(this.OnRebind);
    ControlSettingsUtilities.OnBindingReset += new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnGamepadPromptsChanged += new System.Action(this.ForceUpdate);
    GeneralInputSource.OnBindingsReset += new System.Action(this.ForceUpdate);
    this.ForceUpdate();
  }

  public void OnDisable()
  {
    InputManager.General.OnActiveControllerChanged -= new System.Action<Controller>(this.OnActiveControllerChanged);
    ControlSettingsUtilities.OnGamepadLayoutChanged -= new System.Action(this.ForceUpdate);
    ControlSettingsUtilities.OnRebind -= new System.Action<Binding>(this.OnRebind);
    ControlSettingsUtilities.OnBindingReset -= new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnGamepadPromptsChanged -= new System.Action(this.ForceUpdate);
    GeneralInputSource.OnBindingsReset -= new System.Action(this.ForceUpdate);
  }

  public void ForceUpdate()
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      this.playerFarming = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null ? MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer : PlayerFarming.Instance;
    if (InputManager.General.GetLastActiveController(this.playerFarming) != null)
    {
      this.OnActiveControllerChanged(InputManager.General.GetLastActiveController(this.playerFarming));
    }
    else
    {
      if (InputManager.General.GetDefaultController(this.playerFarming) == null)
        return;
      this.OnActiveControllerChanged(InputManager.General.GetDefaultController(this.playerFarming));
    }
  }

  public void UpdateTerm(string s) => this._actionText.GetComponent<Localize>().SetTerm(s);

  public void OnRebind(Binding binding)
  {
    if (binding.Action != this.Action)
      return;
    this.ForceUpdate();
  }

  public void OnBindingReset(int action)
  {
    if (action != this.Action)
      return;
    this.ForceUpdate();
  }

  public void OnActiveControllerChanged(Controller controller)
  {
    if (this.IgnoreControllerChange)
      return;
    if ((bool) (UnityEngine.Object) this.playerFarming)
    {
      Rewired.Player rewiredPlayer = this.playerFarming.rewiredPlayer;
      if (rewiredPlayer != null && !rewiredPlayer.controllers.ContainsController(controller))
        return;
      if (PlayerFarming.playersCount > 1 && (UnityEngine.Object) this.coopIndicatorIcon != (UnityEngine.Object) null)
      {
        this.coopIndicatorIcon.gameObject.SetActive(true);
        if (this.playerFarming.isLamb)
          this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Lamb);
        else
          this.coopIndicatorIcon.SetIcon(CoopIndicatorIcon.CoopIcon.Goat);
      }
    }
    if (controller != null)
    {
      if (controller.type != ControllerType.Joystick && this._prioritizeMouse)
      {
        Controller controller1 = InputManager.General.GetController(ControllerType.Mouse, this.playerFarming);
        if (controller1 != null && controller1.isConnected && controller1.enabled)
          controller = controller1;
      }
      else if (controller.type == ControllerType.Mouse)
        controller = InputManager.General.GetController(ControllerType.Keyboard, this.playerFarming);
    }
    if (controller == null || this.AssignPrompt(this.GetActionElementMap(controller), controller) || !this._prioritizeMouse || controller.type != ControllerType.Mouse || !this._prioritizeMouseFallbackToKeyboard)
      return;
    controller = InputManager.General.GetController(ControllerType.Keyboard, this.playerFarming);
    this.AssignPrompt(this.GetActionElementMap(controller), controller);
  }

  public ActionElementMap GetActionElementMap(Controller controller, bool requiresPlayerFarming = true)
  {
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null & requiresPlayerFarming)
    {
      this.playerFarming = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null ? MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer : PlayerFarming.Instance;
      InputManager.General.GetLastActiveController(this.playerFarming);
    }
    ControllerMap controllerMap = (ControllerMap) null;
    if (this.Category == 0)
      controllerMap = InputManager.Gameplay.GetControllerMap(controller, this.playerFarming);
    else if (this.Category == 1)
      controllerMap = InputManager.UI.GetControllerMap(controller, this.playerFarming);
    else if (this.Category == 2)
      controllerMap = InputManager.PhotoMode.GetControllerMap(controller, this.playerFarming);
    if (controllerMap != null)
      return controllerMap.GetActionElementMap(this.Action, (Pole) this.AxisContribution);
    Debug.Log((object) $"Unable to determine Controller Map! - {this.Category}".Colour(Color.red));
    return (ActionElementMap) null;
  }

  public bool AssignPrompt(
    ActionElementMap actionElementMap,
    Controller controller,
    bool requiresPlayerFarming = true)
  {
    if (requiresPlayerFarming && (UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null && (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null)
      this.playerFarming = (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null ? MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer : PlayerFarming.Instance;
    if (controller == null)
      controller = InputManager.General.GetLastActiveController(this.playerFarming);
    if (actionElementMap == null)
      actionElementMap = this.GetActionElementMap(controller, requiresPlayerFarming);
    if (actionElementMap == null)
    {
      this._iconText.font = this._controlMappings.GetFontForPlatform(Platform.PC);
      this._icon.gameObject.SetActive(false);
      this._text.gameObject.SetActive(false);
      this._iconText.gameObject.SetActive(true);
      this._iconText.text = "--";
      this._iconText.verticalAlignment = VerticalAlignmentOptions.Capline;
      return false;
    }
    if (requiresPlayerFarming && (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && InputManager.General.GetLastActiveController(this.playerFarming) == null)
      controller = InputManager.General.GetDefaultController(this.playerFarming);
    if (actionElementMap != null && actionElementMap.controllerMap != null)
    {
      if (actionElementMap.controllerMap.controllerType == ControllerType.Keyboard)
      {
        bool isSpecialCharacter;
        string keyboardCode = ControlMappings.GetKeyboardCode(actionElementMap.keyboardKeyCode, out isSpecialCharacter);
        if (!isSpecialCharacter)
        {
          this._text.text = keyboardCode;
          this._text.gameObject.SetActive(true);
          this._text.fontSize = 30f;
          this._text.fontSizeMin = 10f;
          this._text.fontSizeMax = 30f;
          this._text.enableAutoSizing = true;
          this._iconText.gameObject.SetActive(false);
          this._iconText.fontStyle = FontStyles.Normal;
          this._icon.gameObject.SetActive(true);
        }
        else
          this.SetSpecialPCIcon(keyboardCode);
      }
      else if (actionElementMap.controllerMap.controllerType == ControllerType.Mouse)
        this.SetSpecialPCIcon(ControlMappings.GetMouseCode((MouseInputElement) actionElementMap.elementIdentifierId, (Pole) this.AxisContribution));
      else if (actionElementMap.controllerMap.controllerType == ControllerType.Joystick)
      {
        Platform platformFromInputType = ControlUtilities.GetPlatformFromInputType(ControlUtilities.GetCurrentInputType(controller));
        IGamepadTemplate template = controller.GetTemplate<IGamepadTemplate>();
        List<ControllerTemplateElementTarget> templateElementTargetList = new List<ControllerTemplateElementTarget>();
        ControllerElementTarget target = (ControllerElementTarget) actionElementMap;
        List<ControllerTemplateElementTarget> results = templateElementTargetList;
        template.GetElementTargets(target, (IList<ControllerTemplateElementTarget>) results);
        this._iconText.font = this._controlMappings.GetFontForPlatform(platformFromInputType);
        this._iconText.fontSize = 42f;
        this._iconText.enableAutoSizing = false;
        this._iconText.fontStyle = FontStyles.Normal;
        this._icon.gameObject.SetActive(false);
        this._text.gameObject.SetActive(false);
        this._iconText.gameObject.SetActive(true);
        this._iconText.verticalAlignment = VerticalAlignmentOptions.Geometry;
        this._iconText.text = ControlMappings.GetControllerCodeFromID(templateElementTargetList[0].element.id);
      }
    }
    return true;
  }

  public void SetSpecialPCIcon(string icon)
  {
    this._iconText.font = this._controlMappings.GetFontForPlatform(Platform.PC);
    this._iconText.text = icon;
    this._iconText.fontSize = 70f;
    this._iconText.fontStyle = FontStyles.Bold;
    this._iconText.enableAutoSizing = false;
    this._iconText.gameObject.SetActive(true);
    this._iconText.verticalAlignment = VerticalAlignmentOptions.Geometry;
    this._text.gameObject.SetActive(false);
    this._icon.gameObject.SetActive(false);
  }
}
