// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputBehaviorWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class InputBehaviorWindow : Window
{
  private const float minSensitivity = 0.1f;
  [SerializeField]
  private RectTransform spawnTransform;
  [SerializeField]
  private UnityEngine.UI.Button doneButton;
  [SerializeField]
  private UnityEngine.UI.Button cancelButton;
  [SerializeField]
  private UnityEngine.UI.Button defaultButton;
  [SerializeField]
  private TMP_Text doneButtonLabel;
  [SerializeField]
  private TMP_Text cancelButtonLabel;
  [SerializeField]
  private TMP_Text defaultButtonLabel;
  [SerializeField]
  private GameObject uiControlSetPrefab;
  [SerializeField]
  private GameObject uiSliderControlPrefab;
  private List<InputBehaviorWindow.InputBehaviorInfo> inputBehaviorInfo;
  private Dictionary<int, Action<int>> buttonCallbacks;
  private int playerId;

  public override void Initialize(int id, Func<int, bool> isFocusedCallback)
  {
    if ((UnityEngine.Object) this.spawnTransform == (UnityEngine.Object) null || (UnityEngine.Object) this.doneButton == (UnityEngine.Object) null || (UnityEngine.Object) this.cancelButton == (UnityEngine.Object) null || (UnityEngine.Object) this.defaultButton == (UnityEngine.Object) null || (UnityEngine.Object) this.uiControlSetPrefab == (UnityEngine.Object) null || (UnityEngine.Object) this.uiSliderControlPrefab == (UnityEngine.Object) null || (UnityEngine.Object) this.doneButtonLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.cancelButtonLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.defaultButtonLabel == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired Control Mapper: All inspector values must be assigned!");
    }
    else
    {
      this.inputBehaviorInfo = new List<InputBehaviorWindow.InputBehaviorInfo>();
      this.buttonCallbacks = new Dictionary<int, Action<int>>();
      this.doneButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().done;
      this.cancelButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().cancel;
      this.defaultButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().default_;
      base.Initialize(id, isFocusedCallback);
    }
  }

  public void SetData(int playerId, Rewired.UI.ControlMapper.ControlMapper.InputBehaviorSettings[] data)
  {
    if (!this.initialized)
      return;
    this.playerId = playerId;
    for (int index = 0; index < data.Length; ++index)
    {
      Rewired.UI.ControlMapper.ControlMapper.InputBehaviorSettings behaviorSettings = data[index];
      if (behaviorSettings != null && behaviorSettings.isValid)
      {
        InputBehavior inputBehavior = this.GetInputBehavior(behaviorSettings.inputBehaviorId);
        if (inputBehavior != null)
        {
          UIControlSet controlSet = this.CreateControlSet();
          Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty = new Dictionary<int, InputBehaviorWindow.PropertyType>();
          string customEntry = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetCustomEntry(behaviorSettings.labelLanguageKey);
          if (!string.IsNullOrEmpty(customEntry))
            controlSet.SetTitle(customEntry);
          else
            controlSet.SetTitle(inputBehavior.name);
          if (behaviorSettings.showJoystickAxisSensitivity)
          {
            UISliderControl slider = this.CreateSlider(controlSet, inputBehavior.id, (string) null, Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetCustomEntry(behaviorSettings.joystickAxisSensitivityLabelLanguageKey), behaviorSettings.joystickAxisSensitivityIcon, behaviorSettings.joystickAxisSensitivityMin, behaviorSettings.joystickAxisSensitivityMax, new Action<int, int, float>(this.JoystickAxisSensitivityValueChanged), new Action<int, int>(this.JoystickAxisSensitivityCanceled));
            slider.slider.value = Mathf.Clamp(inputBehavior.joystickAxisSensitivity, behaviorSettings.joystickAxisSensitivityMin, behaviorSettings.joystickAxisSensitivityMax);
            idToProperty.Add(slider.id, InputBehaviorWindow.PropertyType.JoystickAxisSensitivity);
          }
          if (behaviorSettings.showMouseXYAxisSensitivity)
          {
            UISliderControl slider = this.CreateSlider(controlSet, inputBehavior.id, (string) null, Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetCustomEntry(behaviorSettings.mouseXYAxisSensitivityLabelLanguageKey), behaviorSettings.mouseXYAxisSensitivityIcon, behaviorSettings.mouseXYAxisSensitivityMin, behaviorSettings.mouseXYAxisSensitivityMax, new Action<int, int, float>(this.MouseXYAxisSensitivityValueChanged), new Action<int, int>(this.MouseXYAxisSensitivityCanceled));
            slider.slider.value = Mathf.Clamp(inputBehavior.mouseXYAxisSensitivity, behaviorSettings.mouseXYAxisSensitivityMin, behaviorSettings.mouseXYAxisSensitivityMax);
            idToProperty.Add(slider.id, InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity);
          }
          this.inputBehaviorInfo.Add(new InputBehaviorWindow.InputBehaviorInfo(inputBehavior, controlSet, idToProperty));
        }
      }
    }
    this.defaultUIElement = this.doneButton.gameObject;
  }

  public void SetButtonCallback(
    InputBehaviorWindow.ButtonIdentifier buttonIdentifier,
    Action<int> callback)
  {
    if (!this.initialized || callback == null)
      return;
    if (this.buttonCallbacks.ContainsKey((int) buttonIdentifier))
      this.buttonCallbacks[(int) buttonIdentifier] = callback;
    else
      this.buttonCallbacks.Add((int) buttonIdentifier, callback);
  }

  public override void Cancel()
  {
    if (!this.initialized)
      return;
    foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
      inputBehaviorInfo.RestorePreviousData();
    Action<int> action;
    if (!this.buttonCallbacks.TryGetValue(1, out action))
    {
      if (this.cancelCallback == null)
        return;
      this.cancelCallback();
    }
    else
      action(this.id);
  }

  public void OnDone()
  {
    Action<int> action;
    if (!this.initialized || !this.buttonCallbacks.TryGetValue(0, out action))
      return;
    action(this.id);
  }

  public void OnCancel() => this.Cancel();

  public void OnRestoreDefault()
  {
    if (!this.initialized)
      return;
    foreach (InputBehaviorWindow.InputBehaviorInfo inputBehaviorInfo in this.inputBehaviorInfo)
      inputBehaviorInfo.RestoreDefaultData();
  }

  private void JoystickAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
  {
    this.GetInputBehavior(inputBehaviorId).joystickAxisSensitivity = value;
  }

  private void MouseXYAxisSensitivityValueChanged(int inputBehaviorId, int controlId, float value)
  {
    this.GetInputBehavior(inputBehaviorId).mouseXYAxisSensitivity = value;
  }

  private void JoystickAxisSensitivityCanceled(int inputBehaviorId, int controlId)
  {
    this.GetInputBehaviorInfo(inputBehaviorId)?.RestoreData(InputBehaviorWindow.PropertyType.JoystickAxisSensitivity, controlId);
  }

  private void MouseXYAxisSensitivityCanceled(int inputBehaviorId, int controlId)
  {
    this.GetInputBehaviorInfo(inputBehaviorId)?.RestoreData(InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity, controlId);
  }

  public override void TakeInputFocus() => base.TakeInputFocus();

  private UIControlSet CreateControlSet()
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.uiControlSetPrefab);
    gameObject.transform.SetParent((Transform) this.spawnTransform, false);
    return gameObject.GetComponent<UIControlSet>();
  }

  private UISliderControl CreateSlider(
    UIControlSet set,
    int inputBehaviorId,
    string defaultTitle,
    string overrideTitle,
    Sprite icon,
    float minValue,
    float maxValue,
    Action<int, int, float> valueChangedCallback,
    Action<int, int> cancelCallback)
  {
    UISliderControl slider = set.CreateSlider(this.uiSliderControlPrefab, icon, minValue, maxValue, (Action<int, float>) ((cId, value) => valueChangedCallback(inputBehaviorId, cId, value)), (Action<int>) (cId => cancelCallback(inputBehaviorId, cId)));
    string str = string.IsNullOrEmpty(overrideTitle) ? defaultTitle : overrideTitle;
    if (!string.IsNullOrEmpty(str))
    {
      slider.showTitle = true;
      slider.title.text = str;
    }
    else
      slider.showTitle = false;
    slider.showIcon = (UnityEngine.Object) icon != (UnityEngine.Object) null;
    return slider;
  }

  private InputBehavior GetInputBehavior(int id)
  {
    return ReInput.mapping.GetInputBehavior(this.playerId, id);
  }

  private InputBehaviorWindow.InputBehaviorInfo GetInputBehaviorInfo(int inputBehaviorId)
  {
    int count = this.inputBehaviorInfo.Count;
    for (int index = 0; index < count; ++index)
    {
      if (this.inputBehaviorInfo[index].inputBehavior.id == inputBehaviorId)
        return this.inputBehaviorInfo[index];
    }
    return (InputBehaviorWindow.InputBehaviorInfo) null;
  }

  private class InputBehaviorInfo
  {
    private InputBehavior _inputBehavior;
    private UIControlSet _controlSet;
    private Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty;
    private InputBehavior copyOfOriginal;

    public InputBehavior inputBehavior => this._inputBehavior;

    public UIControlSet controlSet => this._controlSet;

    public InputBehaviorInfo(
      InputBehavior inputBehavior,
      UIControlSet controlSet,
      Dictionary<int, InputBehaviorWindow.PropertyType> idToProperty)
    {
      this._inputBehavior = inputBehavior;
      this._controlSet = controlSet;
      this.idToProperty = idToProperty;
      this.copyOfOriginal = new InputBehavior(inputBehavior);
    }

    public void RestorePreviousData() => this._inputBehavior.ImportData(this.copyOfOriginal);

    public void RestoreDefaultData()
    {
      this._inputBehavior.Reset();
      this.RefreshControls();
    }

    public void RestoreData(InputBehaviorWindow.PropertyType propertyType, int controlId)
    {
      switch (propertyType)
      {
        case InputBehaviorWindow.PropertyType.JoystickAxisSensitivity:
          float joystickAxisSensitivity = this.copyOfOriginal.joystickAxisSensitivity;
          this._inputBehavior.joystickAxisSensitivity = joystickAxisSensitivity;
          UISliderControl control1 = this._controlSet.GetControl<UISliderControl>(controlId);
          if (!((UnityEngine.Object) control1 != (UnityEngine.Object) null))
            break;
          control1.slider.value = joystickAxisSensitivity;
          break;
        case InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity:
          float xyAxisSensitivity = this.copyOfOriginal.mouseXYAxisSensitivity;
          this._inputBehavior.mouseXYAxisSensitivity = xyAxisSensitivity;
          UISliderControl control2 = this._controlSet.GetControl<UISliderControl>(controlId);
          if (!((UnityEngine.Object) control2 != (UnityEngine.Object) null))
            break;
          control2.slider.value = xyAxisSensitivity;
          break;
      }
    }

    public void RefreshControls()
    {
      if ((UnityEngine.Object) this._controlSet == (UnityEngine.Object) null || this.idToProperty == null)
        return;
      foreach (KeyValuePair<int, InputBehaviorWindow.PropertyType> keyValuePair in this.idToProperty)
      {
        UISliderControl control = this._controlSet.GetControl<UISliderControl>(keyValuePair.Key);
        if (!((UnityEngine.Object) control == (UnityEngine.Object) null))
        {
          switch (keyValuePair.Value)
          {
            case InputBehaviorWindow.PropertyType.JoystickAxisSensitivity:
              control.slider.value = this._inputBehavior.joystickAxisSensitivity;
              continue;
            case InputBehaviorWindow.PropertyType.MouseXYAxisSensitivity:
              control.slider.value = this._inputBehavior.mouseXYAxisSensitivity;
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  public enum ButtonIdentifier
  {
    Done,
    Cancel,
    Default,
  }

  private enum PropertyType
  {
    JoystickAxisSensitivity,
    MouseXYAxisSensitivity,
  }
}
