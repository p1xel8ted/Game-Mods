// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UICoopAssignController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Rewired;
using src.UINavigator;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unify;
using Unify.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UICoopAssignController : UIMenuBase
{
  public static int keyboardInputOptionPreviousSelection = 0;
  public static int[] displayedConnectedGamepadsPreviousSelection = new int[4];
  public CoopAssignInputOption[] displayedConnectedGamepads;
  public CoopAssignInputOption keyboardInputOption;
  public TMP_Text DebugText;
  public bool SELF_SELECTION_ONLY = true;
  public int oldConnectedGamepads;
  public static CoopAssignInputOption SelectedInputOption;
  public CoopAssignInputOption lastSelectedGamepad;
  public Transform GamepadContentContainer;
  public MMButton spawnButton;
  public TextMeshProUGUI spawnWarning;
  public UINavigatorFollowElement navigatorFollowElement;
  public GameObject ConfirmPrompt;
  [HideInInspector]
  public bool mouseToggleDisabled;
  public int lambCount;
  public int goatCount;
  public int maxDetectedGamepads;
  public bool debugModeActive;
  public bool confirmLock;
  public float preventSpawnBufferTime;

  public void OnEnable()
  {
    this.spawnButton.OnPointerEntered += (System.Action) (() => this.mouseToggleDisabled = true);
    this.spawnButton.OnPointerExited += (System.Action) (() => this.mouseToggleDisabled = false);
    if (UnifyManager.platform != UnifyManager.Platform.Standalone && UnifyManager.platform != UnifyManager.Platform.None)
    {
      this.keyboardInputOption.gameObject.SetActive(false);
    }
    else
    {
      this.keyboardInputOption.gameObject.SetActive(true);
      this.keyboardInputOption.SetpadNumDisplay(-1);
    }
    if (UICoopAssignController.keyboardInputOptionPreviousSelection != 0)
    {
      this.keyboardInputOption.SetSelection(UICoopAssignController.keyboardInputOptionPreviousSelection, true);
      this.preventSpawnBufferTime = Time.realtimeSinceStartup;
    }
    else
    {
      this.keyboardInputOption.SetSelection(this.keyboardInputOption.selection, true);
      this.preventSpawnBufferTime = Time.realtimeSinceStartup;
    }
    for (int index = 0; index < this.displayedConnectedGamepads.Length; ++index)
    {
      if (UICoopAssignController.displayedConnectedGamepadsPreviousSelection[index] != 0)
      {
        this.displayedConnectedGamepads[index].SetSelection(UICoopAssignController.displayedConnectedGamepadsPreviousSelection[index], true);
        this.preventSpawnBufferTime = Time.realtimeSinceStartup;
      }
      else
      {
        this.displayedConnectedGamepads[index].SetSelection(this.displayedConnectedGamepads[index].selection, true);
        this.preventSpawnBufferTime = Time.realtimeSinceStartup;
      }
      this.displayedConnectedGamepads[index].gameObject.SetActive(false);
    }
    this.GamepadContentContainer.localPosition = new Vector3(-1000f, 0.0f, 0.0f);
    this.GamepadContentContainer.DOLocalMoveX(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
    this.spawnButton.onClick.AddListener(new UnityAction(this.ConfirmSpawnButtonPress));
    if (UnifyManager.platform != UnifyManager.Platform.Standalone && UnifyManager.platform != UnifyManager.Platform.None)
      this.oldConnectedGamepads = UserHelper.activePlayers;
    else
      this.oldConnectedGamepads = ReInput.controllers.joystickCount;
  }

  public override void OnShowStarted()
  {
    UIManager.PlayAudio("event:/ui/open_menu");
    this.spawnButton.gameObject.SetActive(true);
    this.ConfirmPrompt.SetActive(true);
    MMButton component = this.spawnButton.GetComponent<MMButton>();
    MonoSingleton<UINavigatorNew>.Instance.ChangeSelection((IMMSelectable) component);
    if (this.navigatorFollowElement.isActiveAndEnabled)
      this.navigatorFollowElement.DoMoveButton((Selectable) component);
    this.spawnButton.gameObject.SetActive(false);
    this.ConfirmPrompt.SetActive(false);
    this.spawnWarning.gameObject.SetActive(true);
    base.OnShowStarted();
  }

  public override void OnShowCompleted()
  {
    Engagement.GlobalAllowEngagement = true;
    base.OnShowCompleted();
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable)
      return;
    if (UnityEngine.Input.GetKey(KeyCode.L) && UnityEngine.Input.GetKey(KeyCode.P))
      this.debugModeActive = true;
    this.UpdateDisplayedConnectedGamepads();
  }

  public void UpdateDisplayedConnectedGamepads()
  {
    int num1;
    if (UnifyManager.platform != UnifyManager.Platform.Standalone && UnifyManager.platform != UnifyManager.Platform.None)
    {
      if (ReInput.controllers.joystickCount <= 1)
      {
        if (Engagement.GlobalAllowEngagement)
          Engagement.GlobalAllowEngagement = false;
        num1 = 1;
      }
      else
      {
        if (!Engagement.GlobalAllowEngagement)
          Engagement.GlobalAllowEngagement = true;
        num1 = UserHelper.activePlayers;
      }
    }
    else
      num1 = ReInput.controllers.joystickCount;
    if (num1 == 0)
    {
      if (SceneManager.GetActiveScene().name != "TestUIScene")
        this.Hide();
    }
    else if (num1 != this.oldConnectedGamepads && !this.SELF_SELECTION_ONLY)
    {
      MMButton component = this.displayedConnectedGamepads[0].GetComponent<MMButton>();
      MonoSingleton<UINavigatorNew>.Instance.ChangeSelection((IMMSelectable) component);
      this.navigatorFollowElement.DoMoveButton((Selectable) component);
    }
    this.oldConnectedGamepads = num1;
    if ((bool) (UnityEngine.Object) UICoopAssignController.SelectedInputOption)
    {
      this.lastSelectedGamepad = UICoopAssignController.SelectedInputOption;
      this.navigatorFollowElement.SetAltImage(true);
    }
    else
      this.navigatorFollowElement.SetAltImage();
    if (UnifyManager.platform == UnifyManager.Platform.Standalone || UnifyManager.platform == UnifyManager.Platform.None)
    {
      this.keyboardInputOption.gameObject.SetActive(true);
      this.keyboardInputOption.SetpadNumDisplay(-1);
    }
    if (this.SELF_SELECTION_ONLY)
    {
      if (UnifyManager.platform == UnifyManager.Platform.Standalone || UnifyManager.platform == UnifyManager.Platform.None)
      {
        for (int playerNo = 0; playerNo < ReInput.players.playerCount; ++playerNo)
        {
          Player player = RewiredInputManager.GetPlayer(playerNo);
          if (player.controllers.hasKeyboard)
          {
            Controller activeController = player.controllers.GetLastActiveController();
            if (activeController != null && (activeController.type == ControllerType.Keyboard || activeController.type == ControllerType.Mouse))
            {
              bool flag = UnityEngine.Input.GetMouseButtonDown(0) && !this.mouseToggleDisabled || UnityEngine.Input.GetKey(KeyCode.Space) || UnityEngine.Input.GetKey(KeyCode.KeypadEnter);
              if (this.keyboardInputOption.selection == 1 & flag || (double) player.GetAxis(1) < -0.800000011920929 || UnityEngine.Input.GetKey(KeyCode.LeftArrow) || InputManager.UI.GetPageNavigateLeftDown(player))
              {
                this.keyboardInputOption.SetSelection(-1);
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
              }
              else if (this.keyboardInputOption.selection == -1 & flag || (double) player.GetAxis(1) > 0.800000011920929 || UnityEngine.Input.GetKey(KeyCode.RightArrow) || InputManager.UI.GetPageNavigateRightDown(player))
              {
                this.keyboardInputOption.SetSelection();
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
              }
              else
                this.keyboardInputOption.inputLock = false;
            }
          }
        }
        IList<Joystick> joystickList = (IList<Joystick>) new List<Joystick>((IEnumerable<Joystick>) ReInput.controllers.Joysticks);
        List<Guid> guidList = new List<Guid>();
        if (joystickList.Count > this.maxDetectedGamepads)
          this.maxDetectedGamepads = joystickList.Count;
        int num2 = 0;
        this.debugModeActive = true;
        for (int index = 0; index < this.maxDetectedGamepads; ++index)
        {
          ++num2;
          Joystick joystick = joystickList[index];
          if (num1 > index && joystickList.Count >= index && (!guidList.Contains((Guid) ((Controller) joystick).deviceInstanceGuid) || this.debugModeActive))
          {
            guidList.Add((Guid) ((Controller) joystick).deviceInstanceGuid);
            this.displayedConnectedGamepads[index].gameObject.SetActive(true);
            this.displayedConnectedGamepads[index].SetpadNumDisplay(num2);
            if (joystick != null && joystick.type != ControllerType.Keyboard && joystick.type != ControllerType.Mouse)
            {
              if ((double) joystick.GetAxis(0) < -0.800000011920929 || joystick.GetButtonById(this.GetControllerElementIdByIdentifier((Controller) joystick, "D-Pad Left")) || InputManager.UI.GetPageNavigateLeftDown(joystick))
              {
                this.displayedConnectedGamepads[index].SetSelection(-1);
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
                MMVibrate.RumblePad(joystick, 0, 1, 0.2f);
              }
              else if ((double) joystick.GetAxis(0) > 0.800000011920929 || joystick.GetButtonById(this.GetControllerElementIdByIdentifier((Controller) joystick, "D-Pad Right")) || InputManager.UI.GetPageNavigateRightDown(joystick))
              {
                this.displayedConnectedGamepads[index].SetSelection();
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
                MMVibrate.RumblePad(joystick, 1, 1, 0.2f);
              }
              else
                this.displayedConnectedGamepads[index].inputLock = false;
            }
          }
          else
            this.displayedConnectedGamepads[index].gameObject.SetActive(false);
        }
        if (this.maxDetectedGamepads > joystickList.Count)
        {
          this.spawnButton.gameObject.SetActive(false);
          this.navigatorFollowElement.gameObject.SetActive(false);
          this.maxDetectedGamepads = joystickList.Count;
        }
      }
      else
      {
        if (UserHelper.activePlayers > this.maxDetectedGamepads)
          this.maxDetectedGamepads = UserHelper.activePlayers;
        int num3 = 0;
        this.debugModeActive = true;
        for (int playerNo = 0; playerNo < this.maxDetectedGamepads; ++playerNo)
        {
          ++num3;
          Player player = RewiredInputManager.GetPlayer(playerNo);
          if (num1 > playerNo && UserHelper.activePlayers >= playerNo)
          {
            this.displayedConnectedGamepads[playerNo].gameObject.SetActive(true);
            this.displayedConnectedGamepads[playerNo].SetpadNumDisplay(num3);
            if (player != null)
            {
              if ((double) player.GetAxis(1) < -0.800000011920929 || InputManager.UI.GetPageNavigateLeftDown(player))
              {
                this.displayedConnectedGamepads[playerNo].SetSelection(-1);
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
                MMVibrate.RumblePad(((IList<Joystick>) player.controllers.Joysticks)[0], 0, 1, 0.2f);
              }
              else if ((double) player.GetAxis(1) > 0.800000011920929 || InputManager.UI.GetPageNavigateRightDown(player))
              {
                this.displayedConnectedGamepads[playerNo].SetSelection();
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
                MMVibrate.RumblePad(((IList<Joystick>) player.controllers.Joysticks)[0], 1, 1, 0.2f);
              }
              else
                this.displayedConnectedGamepads[playerNo].inputLock = false;
            }
          }
          else
            this.displayedConnectedGamepads[playerNo].gameObject.SetActive(false);
        }
        if (this.maxDetectedGamepads > UserHelper.activePlayers)
        {
          this.spawnButton.gameObject.SetActive(false);
          this.navigatorFollowElement.gameObject.SetActive(false);
          this.maxDetectedGamepads = UserHelper.activePlayers;
        }
      }
    }
    else
    {
      for (int playerNo = 0; playerNo < this.displayedConnectedGamepads.Length; ++playerNo)
      {
        if (num1 > playerNo)
        {
          this.displayedConnectedGamepads[playerNo].gameObject.SetActive(true);
          if (this.displayedConnectedGamepads[playerNo].gameObject.activeSelf)
          {
            Player player = RewiredInputManager.GetPlayer(playerNo);
            if ((UnityEngine.Object) UICoopAssignController.SelectedInputOption != (UnityEngine.Object) null)
            {
              this.navigatorFollowElement.gameObject.transform.localScale = new Vector3((float) UICoopAssignController.SelectedInputOption.selection, 1f, 1f);
              if ((double) player.GetAxis(1) > 0.800000011920929 || InputManager.UI.GetPageNavigateLeftDown(player))
              {
                UICoopAssignController.SelectedInputOption.SetSelection();
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
                this.lastSelectedGamepad = UICoopAssignController.SelectedInputOption;
                break;
              }
              if ((double) player.GetAxis(1) < -0.800000011920929 || InputManager.UI.GetPageNavigateRightDown(player))
              {
                UICoopAssignController.SelectedInputOption.SetSelection(-1);
                this.preventSpawnBufferTime = Time.realtimeSinceStartup;
                this.lastSelectedGamepad = UICoopAssignController.SelectedInputOption;
                break;
              }
              UICoopAssignController.SelectedInputOption.inputLock = false;
            }
          }
        }
        else
          this.displayedConnectedGamepads[playerNo].gameObject.SetActive(false);
      }
    }
    this.CheckInputsForSpawnButton();
    if (!this.spawnButton.gameObject.activeSelf)
      return;
    for (int playerNo = 0; playerNo < ReInput.players.playerCount; ++playerNo)
    {
      Player player = RewiredInputManager.GetPlayer(playerNo);
      if (player != null && player.GetButtonDown(38))
        this.ConfirmSpawnButtonPress();
    }
  }

  public int GetControllerElementIdByIdentifier(Controller pad, string identifier)
  {
    for (int elementIdentifierId = 0; elementIdentifierId < pad.elementCount; ++elementIdentifierId)
    {
      if (pad.GetElementIdentifierById(elementIdentifierId).name == identifier)
        return elementIdentifierId;
    }
    return -1;
  }

  public void CheckInputsForSpawnButton()
  {
    this.lambCount = 0;
    this.goatCount = 0;
    if (UnifyManager.platform == UnifyManager.Platform.Standalone || UnifyManager.platform == UnifyManager.Platform.None)
    {
      if (this.keyboardInputOption.selection == -1)
        ++this.lambCount;
      else
        ++this.goatCount;
    }
    for (int index = 0; index < this.displayedConnectedGamepads.Length; ++index)
    {
      CoopAssignInputOption connectedGamepad = this.displayedConnectedGamepads[index];
      if (connectedGamepad.gameObject.activeSelf)
      {
        if (connectedGamepad.selection == -1)
          ++this.lambCount;
        else
          ++this.goatCount;
      }
    }
    if (this.lambCount > 0 && this.goatCount > 0)
    {
      this.spawnWarning.gameObject.SetActive(false);
      if (this.spawnButton.gameObject.activeSelf)
        return;
      this.spawnButton.gameObject.SetActive(true);
      this.ConfirmPrompt.SetActive(true);
      DOTween.Kill((object) this.spawnButton.transform);
      this.spawnButton.transform.localScale = Vector3.one * 1.33f;
      this.spawnButton.transform.DOScale(Vector3.one, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        if (!this.SELF_SELECTION_ONLY)
          return;
        this.navigatorFollowElement.gameObject.SetActive(true);
        MMButton component = this.spawnButton.GetComponent<MMButton>();
        MonoSingleton<UINavigatorNew>.Instance.ChangeSelection((IMMSelectable) component);
        this.navigatorFollowElement.DoMoveButton((Selectable) component);
      }));
    }
    else
    {
      if (!this.spawnButton.gameObject.activeSelf)
        return;
      this.navigatorFollowElement.gameObject.SetActive(false);
      this.spawnButton.gameObject.SetActive(false);
      this.ConfirmPrompt.SetActive(false);
      DOTween.Kill((object) this.spawnButton.transform);
      this.spawnWarning.gameObject.SetActive(true);
    }
  }

  public override void OnCancelButtonInput()
  {
    if (this.confirmLock)
      return;
    if (this._canvasGroup.interactable)
      this.Hide(true);
    MonoSingleton<UIManager>.Instance.ShowPauseMenu(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
  }

  public override void OnHideStarted()
  {
    Engagement.GlobalAllowEngagement = false;
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public static void SetInputForSoloPlay()
  {
    if (UnifyManager.platform == UnifyManager.Platform.Standalone || UnifyManager.platform == UnifyManager.Platform.None)
    {
      RewiredInputManager.GetPlayer(1).controllers.hasKeyboard = false;
      RewiredInputManager.GetPlayer(1).controllers.hasMouse = false;
      RewiredInputManager.GetPlayer(0).controllers.hasKeyboard = true;
      RewiredInputManager.GetPlayer(0).controllers.hasMouse = true;
      for (int joystickId = 0; joystickId < ReInput.controllers.joystickCount; ++joystickId)
      {
        Joystick joystick = ReInput.controllers.GetJoystick(joystickId);
        RewiredInputManager.GetPlayer(0).controllers.AddController((Controller) joystick, true);
      }
      Debug.Log((object) "Solo play input set");
    }
    CoopManager.RefreshCoopPlayerRewired();
  }

  public void ConfirmSpawnButtonPress()
  {
    Debug.Log((object) $"PREVENT SPAWN TIME? {((double) Time.realtimeSinceStartup < (double) this.preventSpawnBufferTime + 3.0).ToString()} Time {Time.realtimeSinceStartup.ToString()}/{this.preventSpawnBufferTime.ToString()}");
    if ((double) Time.realtimeSinceStartup < (double) this.preventSpawnBufferTime + 0.25)
      return;
    this.CheckInputsForSpawnButton();
    if (!this.spawnButton.gameObject.activeSelf || this.confirmLock)
      return;
    this.confirmLock = true;
    if (this._canvasGroup.interactable)
      this.Hide();
    if (UnifyManager.platform == UnifyManager.Platform.Standalone || UnifyManager.platform == UnifyManager.Platform.None)
    {
      if (this.keyboardInputOption.selection == -1)
      {
        RewiredInputManager.GetPlayer(0).controllers.hasKeyboard = true;
        RewiredInputManager.GetPlayer(0).controllers.hasMouse = true;
        RewiredInputManager.GetPlayer(1).controllers.hasKeyboard = false;
        RewiredInputManager.GetPlayer(1).controllers.hasMouse = false;
      }
      else if (this.keyboardInputOption.selection == 1)
      {
        RewiredInputManager.GetPlayer(0).controllers.hasKeyboard = false;
        RewiredInputManager.GetPlayer(0).controllers.hasMouse = false;
        RewiredInputManager.GetPlayer(1).controllers.hasKeyboard = true;
        RewiredInputManager.GetPlayer(1).controllers.hasMouse = true;
      }
      UICoopAssignController.keyboardInputOptionPreviousSelection = this.keyboardInputOption.selection;
      for (int joystickId = 0; joystickId < this.displayedConnectedGamepads.Length; ++joystickId)
      {
        Joystick joystick = ReInput.controllers.GetJoystick(joystickId);
        CoopAssignInputOption connectedGamepad = this.displayedConnectedGamepads[joystickId];
        if (connectedGamepad.selection == -1)
        {
          RewiredInputManager.GetPlayer(0).controllers.AddController((Controller) joystick, true);
          RewiredInputManager.GetPlayer(1).controllers.RemoveController((Controller) joystick);
        }
        else if (connectedGamepad.selection == 1)
        {
          RewiredInputManager.GetPlayer(1).controllers.AddController((Controller) joystick, true);
          RewiredInputManager.GetPlayer(0).controllers.RemoveController((Controller) joystick);
        }
        UICoopAssignController.displayedConnectedGamepadsPreviousSelection[joystickId] = this.displayedConnectedGamepads[joystickId].selection;
      }
    }
    else
    {
      for (int playerNo = 0; playerNo < this.displayedConnectedGamepads.Length; ++playerNo)
      {
        CoopAssignInputOption connectedGamepad = this.displayedConnectedGamepads[playerNo];
        if (RewiredInputManager.GetPlayer(playerNo) != null)
        {
          if (connectedGamepad.selection == -1)
            CoopManager.LambController = playerNo;
          else
            CoopManager.GoatController = playerNo;
          UICoopAssignController.displayedConnectedGamepadsPreviousSelection[playerNo] = this.displayedConnectedGamepads[playerNo].selection;
        }
      }
    }
    if (SceneManager.GetActiveScene().name == "TestUIScene")
    {
      CoopManager.CoopActive = true;
      PlayerFarming.RefreshPlayersCount(false);
      System.Action onPlayerJoined = CoopManager.Instance.OnPlayerJoined;
      if (onPlayerJoined == null)
        return;
      onPlayerJoined();
    }
    else
      CoopManager.Instance.SpawnCoopPlayer(1);
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__15_0() => this.mouseToggleDisabled = true;

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__15_1() => this.mouseToggleDisabled = false;

  [CompilerGenerated]
  public void \u003CCheckInputsForSpawnButton\u003Eb__25_0()
  {
    if (!this.SELF_SELECTION_ONLY)
      return;
    this.navigatorFollowElement.gameObject.SetActive(true);
    MMButton component = this.spawnButton.GetComponent<MMButton>();
    MonoSingleton<UINavigatorNew>.Instance.ChangeSelection((IMMSelectable) component);
    this.navigatorFollowElement.DoMoveButton((Selectable) component);
  }
}
