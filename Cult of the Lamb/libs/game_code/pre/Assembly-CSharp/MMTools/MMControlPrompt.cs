// Decompiled with JetBrains decompiler
// Type: MMTools.MMControlPrompt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using src.Extensions;
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
  private bool _lockToControllerType;
  [SerializeField]
  private ControllerType _controllerType;
  [SerializeField]
  [ActionIdProperty(typeof (RewiredConsts.Category))]
  [FormerlySerializedAs("Category")]
  private int _category;
  [SerializeField]
  [ActionIdProperty(typeof (RewiredConsts.Action))]
  [FormerlySerializedAs("Action")]
  private int _action;
  [ActionIdProperty(typeof (Pole))]
  public int AxisContribution;
  [SerializeField]
  private bool _prioritizeMouse = true;
  [SerializeField]
  private bool _prioritizeMouseFallbackToKeyboard = true;
  [Header("Prompt")]
  [SerializeField]
  private ControlMappings _controlMappings;
  [SerializeField]
  private TextMeshProUGUI _text;
  [SerializeField]
  private TextMeshProUGUI _iconText;
  [SerializeField]
  private Image _icon;

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

  private void OnEnable()
  {
    InputManager.General.OnActiveControllerChanged += new System.Action<Controller>(this.OnActiveControllerChanged);
    ControlSettingsUtilities.OnGamepadLayoutChanged += new System.Action(this.ForceUpdate);
    ControlSettingsUtilities.OnRebind += new System.Action<Binding>(this.OnRebind);
    ControlSettingsUtilities.OnBindingReset += new System.Action<int>(this.OnBindingReset);
    ControlSettingsUtilities.OnGamepadPromptsChanged += new System.Action(this.ForceUpdate);
    GeneralInputSource.OnBindingsReset += new System.Action(this.ForceUpdate);
    this.ForceUpdate();
  }

  private void OnDisable()
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
    this.OnActiveControllerChanged(InputManager.General.GetLastActiveController());
  }

  private void OnRebind(Binding binding)
  {
    if (binding.Action != this.Action)
      return;
    this.ForceUpdate();
  }

  private void OnBindingReset(int action)
  {
    if (action != this.Action)
      return;
    this.ForceUpdate();
  }

  private void OnActiveControllerChanged(Controller controller)
  {
    if (controller != null)
    {
      if (controller.type != ControllerType.Joystick && this._prioritizeMouse)
      {
        Controller controller1 = InputManager.General.GetController(ControllerType.Mouse);
        if (controller1 != null && controller1.isConnected && controller1.enabled)
          controller = controller1;
      }
      else if (controller.type == ControllerType.Mouse)
        controller = InputManager.General.GetController(ControllerType.Keyboard);
    }
    if (controller == null || this.AssignPrompt(this.GetActionElementMap(controller), controller) || !this._prioritizeMouse || controller.type != ControllerType.Mouse || !this._prioritizeMouseFallbackToKeyboard)
      return;
    controller = InputManager.General.GetController(ControllerType.Keyboard);
    this.AssignPrompt(this.GetActionElementMap(controller), controller);
  }

  private ActionElementMap GetActionElementMap(Controller controller)
  {
    ControllerMap controllerMap = (ControllerMap) null;
    if (this.Category == 0)
      controllerMap = InputManager.Gameplay.GetControllerMap(controller);
    else if (this.Category == 1)
      controllerMap = InputManager.UI.GetControllerMap(controller);
    if (controllerMap != null)
      return controllerMap.GetActionElementMap(this.Action, (Pole) this.AxisContribution);
    Debug.Log((object) $"Unable to determine Controller Map! - {this.Category}".Colour(Color.red));
    return (ActionElementMap) null;
  }

  private bool AssignPrompt(ActionElementMap actionElementMap, Controller controller)
  {
    if (actionElementMap == null)
    {
      this._iconText.font = this._controlMappings.GetFontForPlatform(Platform.PC);
      this._iconText.fontSize = 50f;
      this._iconText.enableAutoSizing = false;
      this._iconText.fontStyle = FontStyles.Normal;
      this._icon.gameObject.SetActive(false);
      this._text.gameObject.SetActive(false);
      this._iconText.gameObject.SetActive(true);
      this._iconText.text = "--";
      this._iconText.verticalAlignment = VerticalAlignmentOptions.Capline;
      return false;
    }
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
      this.SetSpecialPCIcon(ControlMappings.GetMouseCode((MouseInputElement) actionElementMap.elementIdentifierId));
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
    return true;
  }

  private void SetSpecialPCIcon(string icon)
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
