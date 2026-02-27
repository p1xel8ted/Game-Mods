// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.CalibrationWindow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired.Integration.UnityUI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class CalibrationWindow : Window
{
  private const float minSensitivityOtherAxes = 0.1f;
  private const float maxDeadzone = 0.8f;
  [SerializeField]
  private RectTransform rightContentContainer;
  [SerializeField]
  private RectTransform valueDisplayGroup;
  [SerializeField]
  private RectTransform calibratedValueMarker;
  [SerializeField]
  private RectTransform rawValueMarker;
  [SerializeField]
  private RectTransform calibratedZeroMarker;
  [SerializeField]
  private RectTransform deadzoneArea;
  [SerializeField]
  private Slider deadzoneSlider;
  [SerializeField]
  private Slider zeroSlider;
  [SerializeField]
  private Slider sensitivitySlider;
  [SerializeField]
  private Toggle invertToggle;
  [SerializeField]
  private RectTransform axisScrollAreaContent;
  [SerializeField]
  private UnityEngine.UI.Button doneButton;
  [SerializeField]
  private UnityEngine.UI.Button calibrateButton;
  [SerializeField]
  private TMP_Text doneButtonLabel;
  [SerializeField]
  private TMP_Text cancelButtonLabel;
  [SerializeField]
  private TMP_Text defaultButtonLabel;
  [SerializeField]
  private TMP_Text deadzoneSliderLabel;
  [SerializeField]
  private TMP_Text zeroSliderLabel;
  [SerializeField]
  private TMP_Text sensitivitySliderLabel;
  [SerializeField]
  private TMP_Text invertToggleLabel;
  [SerializeField]
  private TMP_Text calibrateButtonLabel;
  [SerializeField]
  private GameObject axisButtonPrefab;
  private Joystick joystick;
  private string origCalibrationData;
  private int selectedAxis = -1;
  private AxisCalibrationData origSelectedAxisCalibrationData;
  private float displayAreaWidth;
  private List<UnityEngine.UI.Button> axisButtons;
  private Dictionary<int, Action<int>> buttonCallbacks;
  private int playerId;
  private RewiredStandaloneInputModule rewiredStandaloneInputModule;
  private int menuHorizActionId = -1;
  private int menuVertActionId = -1;
  private float minSensitivity;

  private bool axisSelected
  {
    get
    {
      return this.joystick != null && this.selectedAxis >= 0 && this.selectedAxis < this.joystick.calibrationMap.axisCount;
    }
  }

  private AxisCalibration axisCalibration
  {
    get
    {
      return !this.axisSelected ? (AxisCalibration) null : this.joystick.calibrationMap.GetAxis(this.selectedAxis);
    }
  }

  public override void Initialize(int id, Func<int, bool> isFocusedCallback)
  {
    if ((UnityEngine.Object) this.rightContentContainer == (UnityEngine.Object) null || (UnityEngine.Object) this.valueDisplayGroup == (UnityEngine.Object) null || (UnityEngine.Object) this.calibratedValueMarker == (UnityEngine.Object) null || (UnityEngine.Object) this.rawValueMarker == (UnityEngine.Object) null || (UnityEngine.Object) this.calibratedZeroMarker == (UnityEngine.Object) null || (UnityEngine.Object) this.deadzoneArea == (UnityEngine.Object) null || (UnityEngine.Object) this.deadzoneSlider == (UnityEngine.Object) null || (UnityEngine.Object) this.sensitivitySlider == (UnityEngine.Object) null || (UnityEngine.Object) this.zeroSlider == (UnityEngine.Object) null || (UnityEngine.Object) this.invertToggle == (UnityEngine.Object) null || (UnityEngine.Object) this.axisScrollAreaContent == (UnityEngine.Object) null || (UnityEngine.Object) this.doneButton == (UnityEngine.Object) null || (UnityEngine.Object) this.calibrateButton == (UnityEngine.Object) null || (UnityEngine.Object) this.axisButtonPrefab == (UnityEngine.Object) null || (UnityEngine.Object) this.doneButtonLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.cancelButtonLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.defaultButtonLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.deadzoneSliderLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.zeroSliderLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.sensitivitySliderLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.invertToggleLabel == (UnityEngine.Object) null || (UnityEngine.Object) this.calibrateButtonLabel == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired Control Mapper: All inspector values must be assigned!");
    }
    else
    {
      this.axisButtons = new List<UnityEngine.UI.Button>();
      this.buttonCallbacks = new Dictionary<int, Action<int>>();
      this.doneButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().done;
      this.cancelButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().cancel;
      this.defaultButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().default_;
      this.deadzoneSliderLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_deadZoneSliderLabel;
      this.zeroSliderLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_zeroSliderLabel;
      this.sensitivitySliderLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_sensitivitySliderLabel;
      this.invertToggleLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_invertToggleLabel;
      this.calibrateButtonLabel.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().calibrateWindow_calibrateButtonLabel;
      base.Initialize(id, isFocusedCallback);
    }
  }

  public void SetJoystick(int playerId, Joystick joystick)
  {
    if (!this.initialized)
      return;
    this.playerId = playerId;
    this.joystick = joystick;
    if (joystick == null)
    {
      Debug.LogError((object) "Rewired Control Mapper: Joystick cannot be null!");
    }
    else
    {
      float num = 0.0f;
      for (int index1 = 0; index1 < joystick.axisCount; ++index1)
      {
        int index = index1;
        GameObject gameObject = UITools.InstantiateGUIObject<UnityEngine.UI.Button>(this.axisButtonPrefab, (Transform) this.axisScrollAreaContent, "Axis" + (object) index1);
        UnityEngine.UI.Button button = gameObject.GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener((UnityAction) (() => this.OnAxisSelected(index, button)));
        TMP_Text inSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<TMP_Text>(gameObject);
        if ((UnityEngine.Object) inSelfOrChildren != (UnityEngine.Object) null)
          inSelfOrChildren.text = Rewired.UI.ControlMapper.ControlMapper.GetLanguage().GetElementIdentifierName((Controller) joystick, ((IList<ControllerElementIdentifier>) joystick.AxisElementIdentifiers)[index1].id, AxisRange.Full);
        if ((double) num == 0.0)
          num = UnityTools.GetComponentInSelfOrChildren<LayoutElement>(gameObject).minHeight;
        this.axisButtons.Add(button);
      }
      float spacing = this.axisScrollAreaContent.GetComponent<VerticalLayoutGroup>().spacing;
      this.axisScrollAreaContent.sizeDelta = new Vector2(this.axisScrollAreaContent.sizeDelta.x, Mathf.Max((float) joystick.axisCount * (num + spacing) - spacing, this.axisScrollAreaContent.sizeDelta.y));
      this.origCalibrationData = joystick.calibrationMap.ToXmlString();
      this.displayAreaWidth = this.rightContentContainer.sizeDelta.x;
      this.rewiredStandaloneInputModule = this.gameObject.transform.root.GetComponentInChildren<RewiredStandaloneInputModule>();
      if ((UnityEngine.Object) this.rewiredStandaloneInputModule != (UnityEngine.Object) null)
      {
        this.menuHorizActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.horizontalAxis);
        this.menuVertActionId = ReInput.mapping.GetActionId(this.rewiredStandaloneInputModule.verticalAxis);
      }
      if (joystick.axisCount > 0)
        this.SelectAxis(0);
      this.defaultUIElement = this.doneButton.gameObject;
      this.RefreshControls();
      this.Redraw();
    }
  }

  public void SetButtonCallback(
    CalibrationWindow.ButtonIdentifier buttonIdentifier,
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
    if (this.joystick != null)
      this.joystick.ImportCalibrationMapFromXmlString(this.origCalibrationData);
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

  protected override void Update()
  {
    if (!this.initialized)
      return;
    base.Update();
    this.UpdateDisplay();
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
    if (!this.initialized || this.joystick == null)
      return;
    this.joystick.calibrationMap.Reset();
    this.RefreshControls();
    this.Redraw();
  }

  public void OnCalibrate()
  {
    Action<int> action;
    if (!this.initialized || !this.buttonCallbacks.TryGetValue(3, out action))
      return;
    action(this.selectedAxis);
  }

  public void OnInvert(bool state)
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.axisCalibration.invert = state;
  }

  public void OnZeroValueChange(float value)
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.axisCalibration.calibratedZero = value;
    this.RedrawCalibratedZero();
  }

  public void OnZeroCancel()
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.axisCalibration.calibratedZero = this.origSelectedAxisCalibrationData.zero;
    this.RedrawCalibratedZero();
    this.RefreshControls();
  }

  public void OnDeadzoneValueChange(float value)
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.axisCalibration.deadZone = Mathf.Clamp(value, 0.0f, 0.8f);
    if ((double) value > 0.800000011920929)
      this.deadzoneSlider.value = 0.8f;
    this.RedrawDeadzone();
  }

  public void OnDeadzoneCancel()
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.axisCalibration.deadZone = this.origSelectedAxisCalibrationData.deadZone;
    this.RedrawDeadzone();
    this.RefreshControls();
  }

  public void OnSensitivityValueChange(float value)
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.SetSensitivity(this.axisCalibration, value);
  }

  public void OnSensitivityCancel(float value)
  {
    if (!this.initialized || !this.axisSelected)
      return;
    this.axisCalibration.sensitivity = this.origSelectedAxisCalibrationData.sensitivity;
    this.RefreshControls();
  }

  public void OnAxisScrollRectScroll(Vector2 pos)
  {
    int num = this.initialized ? 1 : 0;
  }

  private void OnAxisSelected(int axisIndex, UnityEngine.UI.Button button)
  {
    if (!this.initialized || this.joystick == null)
      return;
    this.SelectAxis(axisIndex);
    this.RefreshControls();
    this.Redraw();
  }

  private void UpdateDisplay() => this.RedrawValueMarkers();

  private void Redraw()
  {
    this.RedrawCalibratedZero();
    this.RedrawValueMarkers();
  }

  private void RefreshControls()
  {
    if (!this.axisSelected)
    {
      this.deadzoneSlider.value = 0.0f;
      this.zeroSlider.value = 0.0f;
      this.sensitivitySlider.value = 0.0f;
      this.invertToggle.isOn = false;
    }
    else
    {
      this.deadzoneSlider.value = this.axisCalibration.deadZone;
      this.zeroSlider.value = this.axisCalibration.calibratedZero;
      this.sensitivitySlider.value = this.GetSliderSensitivity(this.axisCalibration);
      this.invertToggle.isOn = this.axisCalibration.invert;
    }
  }

  private void RedrawDeadzone()
  {
    if (!this.axisSelected)
      return;
    this.deadzoneArea.sizeDelta = new Vector2(this.displayAreaWidth * this.axisCalibration.deadZone, this.deadzoneArea.sizeDelta.y);
    this.deadzoneArea.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.deadzoneArea.anchoredPosition.y);
  }

  private void RedrawCalibratedZero()
  {
    if (!this.axisSelected)
      return;
    this.calibratedZeroMarker.anchoredPosition = new Vector2(this.axisCalibration.calibratedZero * -this.deadzoneArea.parent.localPosition.x, this.calibratedZeroMarker.anchoredPosition.y);
    this.RedrawDeadzone();
  }

  private void RedrawValueMarkers()
  {
    if (!this.axisSelected)
    {
      this.calibratedValueMarker.anchoredPosition = new Vector2(0.0f, this.calibratedValueMarker.anchoredPosition.y);
      this.rawValueMarker.anchoredPosition = new Vector2(0.0f, this.rawValueMarker.anchoredPosition.y);
    }
    else
    {
      float axis = this.joystick.GetAxis(this.selectedAxis);
      float num = Mathf.Clamp(this.joystick.GetAxisRaw(this.selectedAxis), -1f, 1f);
      this.calibratedValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * axis, this.calibratedValueMarker.anchoredPosition.y);
      this.rawValueMarker.anchoredPosition = new Vector2(this.displayAreaWidth * 0.5f * num, this.rawValueMarker.anchoredPosition.y);
    }
  }

  private void SelectAxis(int index)
  {
    if (index < 0 || index >= this.axisButtons.Count || (UnityEngine.Object) this.axisButtons[index] == (UnityEngine.Object) null)
      return;
    this.axisButtons[index].interactable = false;
    this.axisButtons[index].Select();
    for (int index1 = 0; index1 < this.axisButtons.Count; ++index1)
    {
      if (index1 != index)
        this.axisButtons[index1].interactable = true;
    }
    this.selectedAxis = index;
    this.origSelectedAxisCalibrationData = this.axisCalibration.GetData();
    this.SetMinSensitivity();
  }

  public override void TakeInputFocus()
  {
    base.TakeInputFocus();
    if (this.selectedAxis >= 0)
      this.SelectAxis(this.selectedAxis);
    this.RefreshControls();
    this.Redraw();
  }

  private void SetMinSensitivity()
  {
    if (!this.axisSelected)
      return;
    this.minSensitivity = 0.1f;
    if (!((UnityEngine.Object) this.rewiredStandaloneInputModule != (UnityEngine.Object) null))
      return;
    if (this.IsMenuAxis(this.menuHorizActionId, this.selectedAxis))
    {
      this.GetAxisButtonDeadZone(this.playerId, this.menuHorizActionId, ref this.minSensitivity);
    }
    else
    {
      if (!this.IsMenuAxis(this.menuVertActionId, this.selectedAxis))
        return;
      this.GetAxisButtonDeadZone(this.playerId, this.menuVertActionId, ref this.minSensitivity);
    }
  }

  private bool IsMenuAxis(int actionId, int axisIndex)
  {
    if ((UnityEngine.Object) this.rewiredStandaloneInputModule == (UnityEngine.Object) null)
      return false;
    IList<Player> allPlayers = (IList<Player>) ReInput.players.AllPlayers;
    int count1 = allPlayers.Count;
    for (int index1 = 0; index1 < count1; ++index1)
    {
      IList<JoystickMap> maps = (IList<JoystickMap>) allPlayers[index1].controllers.maps.GetMaps<JoystickMap>(this.joystick.id);
      if (maps != null)
      {
        int count2 = maps.Count;
        for (int index2 = 0; index2 < count2; ++index2)
        {
          IList<ActionElementMap> axisMaps = (IList<ActionElementMap>) maps[index2].AxisMaps;
          if (axisMaps != null)
          {
            int count3 = axisMaps.Count;
            for (int index3 = 0; index3 < count3; ++index3)
            {
              ActionElementMap actionElementMap = axisMaps[index3];
              if (actionElementMap.actionId == actionId && actionElementMap.elementIndex == axisIndex)
                return true;
            }
          }
        }
      }
    }
    return false;
  }

  private void GetAxisButtonDeadZone(int playerId, int actionId, ref float value)
  {
    InputAction action = ReInput.mapping.GetAction(actionId);
    if (action == null)
      return;
    int behaviorId = action.behaviorId;
    InputBehavior inputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId);
    if (inputBehavior == null)
      return;
    value = inputBehavior.buttonDeadZone + 0.1f;
  }

  private float GetSliderSensitivity(AxisCalibration axisCalibration)
  {
    return axisCalibration.sensitivityType == AxisSensitivityType.Multiplier || axisCalibration.sensitivityType != AxisSensitivityType.Power ? axisCalibration.sensitivity : CalibrationWindow.ProcessPowerValue(axisCalibration.sensitivity, 0.0f, this.sensitivitySlider.maxValue);
  }

  public void SetSensitivity(AxisCalibration axisCalibration, float sliderValue)
  {
    if (axisCalibration.sensitivityType == AxisSensitivityType.Multiplier)
    {
      axisCalibration.sensitivity = Mathf.Clamp(sliderValue, this.minSensitivity, float.PositiveInfinity);
      if ((double) sliderValue >= (double) this.minSensitivity)
        return;
      this.sensitivitySlider.value = this.minSensitivity;
    }
    else if (axisCalibration.sensitivityType == AxisSensitivityType.Power)
      axisCalibration.sensitivity = CalibrationWindow.ProcessPowerValue(sliderValue, 0.0f, this.sensitivitySlider.maxValue);
    else
      axisCalibration.sensitivity = sliderValue;
  }

  private static float ProcessPowerValue(float value, float minValue, float maxValue)
  {
    value = Mathf.Clamp(value, minValue, maxValue);
    if ((double) value > 1.0)
      value = MathTools.ValueInNewRange(value, 1f, maxValue, 1f, 0.0f);
    else if ((double) value < 1.0)
      value = MathTools.ValueInNewRange(value, 0.0f, 1f, maxValue, 1f);
    return value;
  }

  public enum ButtonIdentifier
  {
    Done,
    Cancel,
    Default,
    Calibrate,
  }
}
