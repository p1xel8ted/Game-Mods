// Decompiled with JetBrains decompiler
// Type: Rewired.Demos.SimpleControlRemapping
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
public class SimpleControlRemapping : MonoBehaviour
{
  public const string category = "Default";
  public const string layout = "Default";
  public const string uiCategory = "UI";
  public InputMapper inputMapper = new InputMapper();
  public GameObject buttonPrefab;
  public GameObject textPrefab;
  public RectTransform fieldGroupTransform;
  public RectTransform actionGroupTransform;
  public Text controllerNameUIText;
  public Text statusUIText;
  public ControllerType selectedControllerType;
  public int selectedControllerId;
  public List<SimpleControlRemapping.Row> rows = new List<SimpleControlRemapping.Row>();

  public Player player => ReInput.players.GetPlayer(0);

  public ControllerMap controllerMap
  {
    get
    {
      return this.controller == null ? (ControllerMap) null : this.player.controllers.maps.GetMap(this.controller.type, this.controller.id, "Default", "Default");
    }
  }

  public Controller controller
  {
    get
    {
      return this.player.controllers.GetController(this.selectedControllerType, this.selectedControllerId);
    }
  }

  public void OnEnable()
  {
    if (!ReInput.isReady)
      return;
    this.inputMapper.options.timeout = 5f;
    this.inputMapper.options.ignoreMouseXAxis = true;
    this.inputMapper.options.ignoreMouseYAxis = true;
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged);
    ReInput.ControllerDisconnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged);
    this.inputMapper.InputMappedEvent += (Action<InputMapper.InputMappedEventData>) new Action<InputMapper.InputMappedEventData>(this.OnInputMapped);
    this.inputMapper.StoppedEvent += (Action<InputMapper.StoppedEventData>) new Action<InputMapper.StoppedEventData>(this.OnStopped);
    this.InitializeUI();
  }

  public void OnDisable()
  {
    this.inputMapper.Stop();
    this.inputMapper.RemoveAllEventListeners();
    ReInput.ControllerConnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged);
    ReInput.ControllerDisconnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerChanged);
  }

  public void RedrawUI()
  {
    if (this.controller == null)
    {
      this.ClearUI();
    }
    else
    {
      this.controllerNameUIText.text = this.controller.name;
      for (int index1 = 0; index1 < this.rows.Count; ++index1)
      {
        SimpleControlRemapping.Row row = this.rows[index1];
        InputAction action = this.rows[index1].action;
        string str = string.Empty;
        int actionElementMapId = -1;
        foreach (ActionElementMap actionElementMap in (IEnumerable<ActionElementMap>) this.controllerMap.ElementMapsWithAction(action.id))
        {
          if (actionElementMap.ShowInField(row.actionRange))
          {
            str = actionElementMap.elementIdentifierName;
            actionElementMapId = actionElementMap.id;
            break;
          }
        }
        row.text.text = str;
        row.button.onClick.RemoveAllListeners();
        int index = index1;
        row.button.onClick.AddListener((UnityAction) (() => this.OnInputFieldClicked(index, actionElementMapId)));
      }
    }
  }

  public void ClearUI()
  {
    this.controllerNameUIText.text = this.selectedControllerType != ControllerType.Joystick ? string.Empty : "No joysticks attached";
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
    this.rows.Add(new SimpleControlRemapping.Row()
    {
      action = action,
      actionRange = actionRange,
      button = gameObject2.GetComponent<UnityEngine.UI.Button>(),
      text = gameObject2.GetComponentInChildren<Text>()
    });
  }

  public void SetSelectedController(ControllerType controllerType)
  {
    bool flag = false;
    if (controllerType != this.selectedControllerType)
    {
      this.selectedControllerType = controllerType;
      flag = true;
    }
    int selectedControllerId = this.selectedControllerId;
    this.selectedControllerId = this.selectedControllerType != ControllerType.Joystick ? 0 : (this.player.controllers.joystickCount <= 0 ? -1 : ((IList<Joystick>) this.player.controllers.Joysticks)[0].id);
    if (this.selectedControllerId != selectedControllerId)
      flag = true;
    if (!flag)
      return;
    this.inputMapper.Stop();
    this.RedrawUI();
  }

  public void OnControllerSelected(int controllerType)
  {
    this.SetSelectedController((ControllerType) controllerType);
  }

  public void OnInputFieldClicked(int index, int actionElementMapToReplaceId)
  {
    if (index < 0 || index >= this.rows.Count || this.controller == null)
      return;
    this.StartCoroutine((IEnumerator) this.StartListeningDelayed(index, actionElementMapToReplaceId));
  }

  public IEnumerator StartListeningDelayed(int index, int actionElementMapToReplaceId)
  {
    yield return (object) new WaitForSeconds(0.1f);
    this.inputMapper.Start(new InputMapper.Context()
    {
      actionId = this.rows[index].action.id,
      controllerMap = this.controllerMap,
      actionRange = this.rows[index].actionRange,
      actionElementMapToReplace = this.controllerMap.GetElementMap(actionElementMapToReplaceId)
    });
    this.player.controllers.maps.SetMapsEnabled(false, "UI");
    this.statusUIText.text = "Listening...";
  }

  public void OnControllerChanged(ControllerStatusChangedEventArgs args)
  {
    this.SetSelectedController(this.selectedControllerType);
  }

  public void OnInputMapped(InputMapper.InputMappedEventData data) => this.RedrawUI();

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
}
