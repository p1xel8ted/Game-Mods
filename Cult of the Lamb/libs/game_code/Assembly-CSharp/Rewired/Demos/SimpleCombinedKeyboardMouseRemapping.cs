// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.SimpleCombinedKeyboardMouseRemapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Rewired.Demos;

[AddComponentMenu("")]
public class SimpleCombinedKeyboardMouseRemapping : MonoBehaviour
{
  public const string category = "Default";
  public const string layout = "Default";
  public const string uiCategory = "UI";
  public InputMapper inputMapper_keyboard = new InputMapper();
  public InputMapper inputMapper_mouse = new InputMapper();
  public GameObject buttonPrefab;
  public GameObject textPrefab;
  public RectTransform fieldGroupTransform;
  public RectTransform actionGroupTransform;
  public Text controllerNameUIText;
  public Text statusUIText;
  public List<SimpleCombinedKeyboardMouseRemapping.Row> rows = new List<SimpleCombinedKeyboardMouseRemapping.Row>();
  public SimpleCombinedKeyboardMouseRemapping.TargetMapping _replaceTargetMapping;

  public Player player => ReInput.players.GetPlayer(0);

  public void OnEnable()
  {
    if (!ReInput.isReady)
      return;
    this.inputMapper_keyboard.options.timeout = 5f;
    this.inputMapper_mouse.options.timeout = 5f;
    this.inputMapper_mouse.options.ignoreMouseXAxis = true;
    this.inputMapper_mouse.options.ignoreMouseYAxis = true;
    this.inputMapper_keyboard.options.allowButtonsOnFullAxisAssignment = false;
    this.inputMapper_mouse.options.allowButtonsOnFullAxisAssignment = false;
    this.inputMapper_keyboard.InputMappedEvent += (Action<InputMapper.InputMappedEventData>) new Action<InputMapper.InputMappedEventData>(this.OnInputMapped);
    this.inputMapper_keyboard.StoppedEvent += (Action<InputMapper.StoppedEventData>) new Action<InputMapper.StoppedEventData>(this.OnStopped);
    this.inputMapper_mouse.InputMappedEvent += (Action<InputMapper.InputMappedEventData>) new Action<InputMapper.InputMappedEventData>(this.OnInputMapped);
    this.inputMapper_mouse.StoppedEvent += (Action<InputMapper.StoppedEventData>) new Action<InputMapper.StoppedEventData>(this.OnStopped);
    this.InitializeUI();
  }

  public void OnDisable()
  {
    this.inputMapper_keyboard.Stop();
    this.inputMapper_mouse.Stop();
    this.inputMapper_keyboard.RemoveAllEventListeners();
    this.inputMapper_mouse.RemoveAllEventListeners();
  }

  public void RedrawUI()
  {
    this.controllerNameUIText.text = "Keyboard/Mouse";
    for (int index1 = 0; index1 < this.rows.Count; ++index1)
    {
      SimpleCombinedKeyboardMouseRemapping.Row row = this.rows[index1];
      InputAction action = this.rows[index1].action;
      string str = string.Empty;
      int actionElementMapId = -1;
      for (int index2 = 0; index2 < 2; ++index2)
      {
        foreach (ActionElementMap actionElementMap in (IEnumerable<ActionElementMap>) this.player.controllers.maps.GetMap(index2 == 0 ? ControllerType.Keyboard : ControllerType.Mouse, 0, "Default", "Default").ElementMapsWithAction(action.id))
        {
          if (actionElementMap.ShowInField(row.actionRange))
          {
            str = actionElementMap.elementIdentifierName;
            actionElementMapId = actionElementMap.id;
            break;
          }
        }
        if (actionElementMapId >= 0)
          break;
      }
      row.text.text = str;
      row.button.onClick.RemoveAllListeners();
      int index = index1;
      row.button.onClick.AddListener((UnityAction) (() => this.OnInputFieldClicked(index, actionElementMapId)));
    }
  }

  public void ClearUI()
  {
    this.controllerNameUIText.text = string.Empty;
    for (int index = 0; index < this.rows.Count; ++index)
      this.rows[index].text.text = string.Empty;
  }

  public void InitializeUI()
  {
    IEnumerator enumerator1 = (IEnumerator) this.actionGroupTransform.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator1.Current).gameObject);
    }
    finally
    {
      if (enumerator1 is IDisposable disposable)
        disposable.Dispose();
    }
    IEnumerator enumerator2 = (IEnumerator) this.fieldGroupTransform.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
        UnityEngine.Object.Destroy((UnityEngine.Object) ((Component) enumerator2.Current).gameObject);
    }
    finally
    {
      if (enumerator2 is IDisposable disposable)
        disposable.Dispose();
    }
    foreach (InputAction action in (IEnumerable<InputAction>) ReInput.mapping.ActionsInCategory("Default"))
    {
      if (action.type == InputActionType.Axis)
      {
        this.CreateUIRow(action, AxisRange.Full, action.descriptiveName);
        this.CreateUIRow(action, AxisRange.Positive, !string.IsNullOrEmpty(action.positiveDescriptiveName) ? action.positiveDescriptiveName : action.descriptiveName + " +");
        this.CreateUIRow(action, AxisRange.Negative, !string.IsNullOrEmpty(action.negativeDescriptiveName) ? action.negativeDescriptiveName : action.descriptiveName + " -");
      }
      else if (action.type == InputActionType.Button)
        this.CreateUIRow(action, AxisRange.Positive, action.descriptiveName);
    }
    this.RedrawUI();
  }

  public void CreateUIRow(InputAction action, AxisRange actionRange, string label)
  {
    GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.textPrefab);
    gameObject1.transform.SetParent((Transform) this.actionGroupTransform);
    gameObject1.transform.SetAsLastSibling();
    gameObject1.GetComponent<Text>().text = label;
    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(this.buttonPrefab);
    gameObject2.transform.SetParent((Transform) this.fieldGroupTransform);
    gameObject2.transform.SetAsLastSibling();
    this.rows.Add(new SimpleCombinedKeyboardMouseRemapping.Row()
    {
      action = action,
      actionRange = actionRange,
      button = gameObject2.GetComponent<UnityEngine.UI.Button>(),
      text = gameObject2.GetComponentInChildren<Text>()
    });
  }

  public void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
  {
    if (index < 0 || index >= this.rows.Count)
      return;
    ControllerMap map1 = this.player.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
    ControllerMap map2 = this.player.controllers.maps.GetMap(ControllerType.Mouse, 0, "Default", "Default");
    ControllerMap controllerMap = !map1.ContainsElementMap(actionElementMapToReplaceId) ? (!map2.ContainsElementMap(actionElementMapToReplaceId) ? (ControllerMap) null : map2) : map1;
    this._replaceTargetMapping = new SimpleCombinedKeyboardMouseRemapping.TargetMapping()
    {
      actionElementMapId = actionElementMapToReplaceId,
      controllerMap = controllerMap
    };
    this.StartCoroutine((IEnumerator) this.StartListeningDelayed(index, map1, map2, actionElementMapToReplaceId));
  }

  public IEnumerator StartListeningDelayed(
    int index,
    ControllerMap keyboardMap,
    ControllerMap mouseMap,
    int actionElementMapToReplaceId)
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.inputMapper_keyboard.Start(new InputMapper.Context()
    {
      actionId = this.rows[index].action.id,
      controllerMap = keyboardMap,
      actionRange = this.rows[index].actionRange,
      actionElementMapToReplace = keyboardMap.GetElementMap(actionElementMapToReplaceId)
    });
    this.inputMapper_mouse.Start(new InputMapper.Context()
    {
      actionId = this.rows[index].action.id,
      controllerMap = mouseMap,
      actionRange = this.rows[index].actionRange,
      actionElementMapToReplace = mouseMap.GetElementMap(actionElementMapToReplaceId)
    });
    this.player.controllers.maps.SetMapsEnabled(false, "UI");
    this.statusUIText.text = "Listening...";
  }

  public void OnInputMapped(InputMapper.InputMappedEventData data)
  {
    this.inputMapper_keyboard.Stop();
    this.inputMapper_mouse.Stop();
    if (this._replaceTargetMapping.controllerMap != null && data.actionElementMap.controllerMap != this._replaceTargetMapping.controllerMap)
      this._replaceTargetMapping.controllerMap.DeleteElementMap(this._replaceTargetMapping.actionElementMapId);
    this.RedrawUI();
  }

  public void OnStopped(InputMapper.StoppedEventData data)
  {
    this.statusUIText.text = string.Empty;
    this.player.controllers.maps.SetMapsEnabled(true, "UI");
  }

  public class Row
  {
    public InputAction action;
    public AxisRange actionRange;
    public UnityEngine.UI.Button button;
    public Text text;
  }

  public struct TargetMapping
  {
    public ControllerMap controllerMap;
    public int actionElementMapId;
  }
}
