// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorNew
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Rewired;
using Rewired.Integration.UnityUI;
using System;
using Unify;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
namespace src.UINavigator;

public class UINavigatorNew : MonoSingleton<UINavigatorNew>
{
  public const float kHorizontalAxisThreshold = 0.2f;
  public const float kVerticalAxisThreshold = 0.2f;
  public const float kSelectionDelayMax = 0.2f;
  public const float kSelectionHoldDelayReduction = 0.05f;
  public const float kSelectionHoldDelayReductionThreshold = 2f;
  public const float kButtonDownDelayMax = 0.1f;
  public const float kButtonHoldDelayMax = 0.25f;
  public const float kButtonHoldDelayReduction = 0.05f;
  public const int kButtonHoldDelayReductionThreshold = 2;
  public Action<Selectable, Selectable> OnSelectionChanged;
  public Action<Selectable> OnDefaultSetComplete;
  public System.Action OnCancelDown;
  public System.Action OnPageNavigateLeft;
  public System.Action OnPageNavigateRight;
  public System.Action OnClear;
  public Action<PlayerFarming> OnInputAllowedOnlyFromSpecificPlayerChanged;
  public bool AllowAcceptHold;
  public bool LockNavigation;
  public bool LockInput;
  public PlayerFarming _allowInputOnlyFromPlayer;
  public bool _temporarilyAllowInputOnlyFromAnyPlayer;
  [SerializeField]
  public bool _disableSFX;
  public RewiredStandaloneInputModule _inputModule;
  public Vector3 _recentMoveVector;
  public float _selectionDelay;
  public float _buttonDownDelay;
  public int _navigationHold;
  public int _consecutiveHold;
  public IMMSelectable _currentSelectable;
  public Vector3 _previousMouseInput;
  public const float mouse_movement_threshold = 25f;
  public const float inactiveCursorVisibiliyMaxTime = 1.5f;
  public float inactiveCursorVisilibtyTimer = 1.5f;

  public PlayerFarming AllowInputOnlyFromPlayer
  {
    get => this._allowInputOnlyFromPlayer;
    set
    {
      this._allowInputOnlyFromPlayer = value;
      Action<PlayerFarming> specificPlayerChanged = this.OnInputAllowedOnlyFromSpecificPlayerChanged;
      if (specificPlayerChanged == null)
        return;
      specificPlayerChanged(!this.TemporarilyAllowInputOnlyFromAnyPlayer ? this._allowInputOnlyFromPlayer : (PlayerFarming) null);
    }
  }

  public bool TemporarilyAllowInputOnlyFromAnyPlayer
  {
    get => this._temporarilyAllowInputOnlyFromAnyPlayer;
    set
    {
      this._temporarilyAllowInputOnlyFromAnyPlayer = value;
      Action<PlayerFarming> specificPlayerChanged = this.OnInputAllowedOnlyFromSpecificPlayerChanged;
      if (specificPlayerChanged == null)
        return;
      specificPlayerChanged(!this.TemporarilyAllowInputOnlyFromAnyPlayer ? this._allowInputOnlyFromPlayer : (PlayerFarming) null);
    }
  }

  public IMMSelectable CurrentSelectable => this._currentSelectable;

  public Vector2 RecentMoveVector => (Vector2) this._recentMoveVector;

  public Vector3 PreviousMouseInput => this._previousMouseInput;

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void InitializeUINavigator()
  {
    GameObject gameObject = new GameObject();
    gameObject.AddComponent<UINavigatorNew>();
    gameObject.name = "UINavigator";
  }

  public override void Start()
  {
    base.Start();
    this.transform.SetParent((Transform) null);
    this.EnsureInputModuleUpdated();
    SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.OnActiveSceneChanged);
  }

  public void OnActiveSceneChanged(Scene current, Scene next) => this.EnsureInputModuleUpdated();

  public void EnsureInputModuleUpdated()
  {
    if ((UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null)
      EventSystem.current.transform.SetParent((Transform) null);
    if (!((UnityEngine.Object) MonoSingleton<RewiredEventSystem>.Instance != (UnityEngine.Object) null))
      return;
    this._inputModule = MonoSingleton<RewiredEventSystem>.Instance.GetComponent<RewiredStandaloneInputModule>();
    this._inputModule.allowMouseInput = InputManager.General.MouseInputActive;
    this._previousMouseInput = Input.mousePosition;
  }

  public void Update()
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.None:
      case UnifyManager.Platform.Standalone:
        if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null && (UnityEngine.Object) this._inputModule != (UnityEngine.Object) null)
        {
          Controller activeController1 = InputManager.General.GetLastActiveController(MonoSingleton<UIManager>.Instance.MenusBlocked ? this.AllowInputOnlyFromPlayer : (PlayerFarming) null);
          if (activeController1 != null)
          {
            bool flag1 = (double) Vector3.Distance(Input.mousePosition, this._previousMouseInput) >= 25.0;
            if (MonoSingleton<UIManager>.Instance.MenusBlocked && (UnityEngine.Object) this.AllowInputOnlyFromPlayer != (UnityEngine.Object) null && !this.AllowInputOnlyFromPlayer.canUseKeyboard)
              this._inputModule.allowMouseInput = true;
            bool flag2 = Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2);
            if (flag1 | flag2)
              this.inactiveCursorVisilibtyTimer = 0.0f;
            else if ((activeController1.type == ControllerType.Keyboard || activeController1.type == ControllerType.Mouse) && (double) this.inactiveCursorVisilibtyTimer < 1.5)
              this.inactiveCursorVisilibtyTimer += Time.unscaledDeltaTime;
            if (!this._inputModule.allowMouseInput)
            {
              if (flag1)
              {
                this._inputModule.allowMouseInput = true;
                InputManager.General.MouseInputActive = true;
                Cursor.visible = true;
                this._previousMouseInput = Input.mousePosition;
                break;
              }
              break;
            }
            if (activeController1.type == ControllerType.Joystick && (InputManager.General.GetAnyButton() || InputManager.General.GetAnyAxisHold()))
            {
              if (!flag1 && !flag2)
              {
                this._previousMouseInput = Input.mousePosition;
                this._inputModule.allowMouseInput = false;
                bool flag3 = false;
                if (PlayerFarming.playersCount > 1)
                {
                  for (int index = 0; index < PlayerFarming.players.Count; ++index)
                  {
                    PlayerFarming player = PlayerFarming.players[index];
                    Controller activeController2 = InputManager.General.GetLastActiveController(player);
                    if ((UnityEngine.Object) player != (UnityEngine.Object) null && activeController2 != null && (!MonoSingleton<UIManager>.Instance.MenusBlocked || (UnityEngine.Object) player == (UnityEngine.Object) this.AllowInputOnlyFromPlayer) && player.canUseKeyboard && (activeController2.type == ControllerType.Mouse || activeController2.type == ControllerType.Keyboard))
                      flag3 = true;
                  }
                }
                bool flag4 = flag1 || (double) this.inactiveCursorVisilibtyTimer < 1.5;
                InputManager.General.MouseInputActive = flag3 & flag4;
                Cursor.visible = flag3 & flag4;
                if (this._currentSelectable != null && (UnityEngine.Object) this._currentSelectable.Selectable != (UnityEngine.Object) null)
                {
                  this._currentSelectable.Selectable.OnPointerExit((PointerEventData) null);
                  break;
                }
                break;
              }
              this._previousMouseInput = Input.mousePosition;
              break;
            }
            if (activeController1.type == ControllerType.Keyboard || activeController1.type == ControllerType.Mouse)
            {
              bool flag5 = PlayerFarming.playersCount == 1;
              bool flag6;
              if (activeController1.type == ControllerType.Mouse & flag1)
              {
                flag6 = true;
                InputManager.General.MouseInputActive = true;
              }
              else if (PlayerFarming.playersCount == 0)
              {
                flag1 = activeController1.type == ControllerType.Mouse;
                bool flag7 = flag1 || (double) this.inactiveCursorVisilibtyTimer < 1.5;
                flag6 = flag7;
                InputManager.General.MouseInputActive = flag7;
              }
              else if (flag5)
              {
                if (PlayerFarming.Instance.CurrentWeaponInfo != null && (UnityEngine.Object) PlayerFarming.Instance.CurrentWeaponInfo.WeaponData != (UnityEngine.Object) null && PlayerFarming.Instance.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType == EquipmentType.Blunderbuss)
                  flag1 = activeController1.type == ControllerType.Mouse;
                bool flag8 = flag1 || (double) this.inactiveCursorVisilibtyTimer < 1.5;
                flag6 = flag8;
                InputManager.General.MouseInputActive = flag8;
              }
              else
              {
                PlayerFarming playerFarming = (PlayerFarming) null;
                foreach (PlayerFarming player in PlayerFarming.players)
                {
                  Controller activeController3 = InputManager.General.GetLastActiveController(player);
                  if (!((UnityEngine.Object) player == (UnityEngine.Object) null) && activeController3 != null && (!MonoSingleton<UIManager>.Instance.MenusBlocked || !((UnityEngine.Object) player != (UnityEngine.Object) this.AllowInputOnlyFromPlayer)) && player.canUseKeyboard)
                  {
                    switch (activeController3.type)
                    {
                      case ControllerType.Keyboard:
                      case ControllerType.Mouse:
                        playerFarming = player;
                        goto label_40;
                      default:
                        continue;
                    }
                  }
                }
label_40:
                if ((UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.CurrentWeaponInfo != null && (UnityEngine.Object) playerFarming.CurrentWeaponInfo.WeaponData != (UnityEngine.Object) null && playerFarming.CurrentWeaponInfo.WeaponData.PrimaryEquipmentType == EquipmentType.Blunderbuss)
                  flag1 = activeController1.type == ControllerType.Mouse;
                bool flag9 = flag1 || (double) this.inactiveCursorVisilibtyTimer < 1.5;
                flag6 = (UnityEngine.Object) playerFarming != (UnityEngine.Object) null & flag9;
                InputManager.General.MouseInputActive = flag6;
              }
              Cursor.visible = flag6;
              if (flag1 | flag2)
              {
                this._previousMouseInput = Input.mousePosition;
                break;
              }
              break;
            }
            if (flag1 | flag2)
            {
              this._previousMouseInput = Input.mousePosition;
              break;
            }
            break;
          }
          break;
        }
        break;
    }
    PlayerFarming playerFarming1 = (PlayerFarming) null;
    if ((bool) (UnityEngine.Object) this.AllowInputOnlyFromPlayer && !this.TemporarilyAllowInputOnlyFromAnyPlayer)
      playerFarming1 = this.AllowInputOnlyFromPlayer;
    PlayerFarming playerFarming2 = playerFarming1;
    if (this._currentSelectable != null && (UnityEngine.Object) this._currentSelectable.Selectable != (UnityEngine.Object) null && (bool) (UnityEngine.Object) this._currentSelectable.playerFarming)
      playerFarming2 = this._currentSelectable.playerFarming;
    if (!this.LockInput)
    {
      if ((double) this._buttonDownDelay <= 0.0)
      {
        if (InputManager.UI.GetAcceptButtonDown(playerFarming2) || this.AllowAcceptHold && InputManager.UI.GetAcceptButtonHeld(playerFarming2))
          this.PerformConfirmAction();
        else if (InputManager.UI.GetCancelButtonDown(playerFarming2))
          this.PerformCancelAction();
      }
      else
        this._buttonDownDelay -= Time.unscaledDeltaTime;
      if (InputManager.UI.GetAcceptButtonUp(playerFarming2))
      {
        this._consecutiveHold = 0;
        this._buttonDownDelay = 0.0f;
      }
    }
    if (this.LockNavigation)
      return;
    if ((double) this._selectionDelay <= 0.0)
    {
      if ((UnityEngine.Object) this._currentSelectable != (UnityEngine.Object) null)
      {
        IMMSelectable newSelectable = (IMMSelectable) null;
        if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis(playerFarming2)) > (double) Mathf.Abs(InputManager.UI.GetVerticalAxis(playerFarming2)))
        {
          if ((double) InputManager.UI.GetHorizontalAxis(playerFarming2) > 0.20000000298023224)
            newSelectable = this._currentSelectable.TryNavigateRight();
          else if ((double) InputManager.UI.GetHorizontalAxis(playerFarming2) < -0.20000000298023224)
            newSelectable = this._currentSelectable.TryNavigateLeft();
        }
        else if ((double) InputManager.UI.GetVerticalAxis(playerFarming2) > 0.20000000298023224)
          newSelectable = this._currentSelectable.TryNavigateUp();
        else if ((double) InputManager.UI.GetVerticalAxis(playerFarming2) < -0.20000000298023224)
          newSelectable = this._currentSelectable.TryNavigateDown();
        if (newSelectable != null)
          this.ChangeSelection(newSelectable);
      }
      if (InputManager.UI.GetPageNavigateLeftDown(playerFarming2))
        this.PerformPageNavigationLeft();
      if (InputManager.UI.GetPageNavigateRightDown(playerFarming2))
        this.PerformPageNavigationRight();
    }
    else
      this._selectionDelay -= Time.unscaledDeltaTime;
    if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis(playerFarming2)) >= 0.20000000298023224 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis(playerFarming2)) >= 0.20000000298023224)
      return;
    this._navigationHold = 0;
    this._selectionDelay = 0.0f;
  }

  public void PerformConfirmAction()
  {
    if (this._currentSelectable == null || !((UnityEngine.Object) this._currentSelectable.Selectable != (UnityEngine.Object) null) || !this._currentSelectable.Interactable)
      return;
    this.PerformButtonAction(true);
    this._currentSelectable.TryPerformConfirmAction();
  }

  public void PerformCancelAction()
  {
    this.PerformButtonAction();
    System.Action onCancelDown = this.OnCancelDown;
    if (onCancelDown == null)
      return;
    onCancelDown();
  }

  public void PerformPageNavigationLeft()
  {
    this.PerformNavigationAction();
    System.Action pageNavigateLeft = this.OnPageNavigateLeft;
    if (pageNavigateLeft == null)
      return;
    pageNavigateLeft();
  }

  public void PerformPageNavigationRight()
  {
    this.PerformNavigationAction();
    System.Action pageNavigateRight = this.OnPageNavigateRight;
    if (pageNavigateRight == null)
      return;
    pageNavigateRight();
  }

  public void PerformButtonAction(bool confirmation = false)
  {
    if (this.AllowAcceptHold & confirmation)
    {
      this._buttonDownDelay = 0.25f - Mathf.Min(Mathf.Max(0.05f * (float) (this._consecutiveHold - 2), 0.0f), 0.15f);
      ++this._consecutiveHold;
    }
    else
      this._buttonDownDelay = 0.1f;
  }

  public void ChangeSelection(IMMSelectable newSelectable)
  {
    this.PerformNavigationAction();
    if (this._currentSelectable == newSelectable || !newSelectable.Interactable)
      return;
    this._recentMoveVector = newSelectable.Selectable.transform.position - this._currentSelectable.Selectable.transform.position;
    Action<Selectable, Selectable> selectionChanged = this.OnSelectionChanged;
    if (selectionChanged != null)
      selectionChanged(newSelectable.Selectable, this._currentSelectable.Selectable);
    this.NavigateTo(newSelectable);
  }

  public void NavigateTo(IMMSelectable newSelectable)
  {
    if (this._currentSelectable == newSelectable || !newSelectable.Interactable)
      return;
    if (!this._disableSFX)
      UIManager.PlayAudio("event:/ui/change_selection");
    this._currentSelectable = newSelectable;
    this._currentSelectable.Selectable.Select();
  }

  public void NavigateToNew(IMMSelectable newSelectable)
  {
    if (this._currentSelectable == newSelectable)
      return;
    if (this._currentSelectable != null)
      this._currentSelectable.SetNormalTransitionState();
    this._recentMoveVector.x = this._recentMoveVector.y = 0.0f;
    this.NavigateTo(newSelectable);
    Action<Selectable> defaultSetComplete = this.OnDefaultSetComplete;
    if (defaultSetComplete == null)
      return;
    defaultSetComplete(newSelectable.Selectable);
  }

  public void ForcePerformNavigationAction() => this.PerformNavigationAction();

  public void PerformNavigationAction()
  {
    this._selectionDelay = 0.2f - Mathf.Min(Mathf.Max((float) (0.05000000074505806 * ((double) this._navigationHold - 2.0)), 0.0f), 0.15f);
    ++this._navigationHold;
  }

  public void Clear()
  {
    System.Action onClear = this.OnClear;
    if (onClear != null)
      onClear();
    this._currentSelectable = (IMMSelectable) null;
    this._selectionDelay = 0.0f;
    this._buttonDownDelay = 0.0f;
    this._navigationHold = 0;
    this._consecutiveHold = 0;
    this._recentMoveVector.x = this._recentMoveVector.y = 0.0f;
  }
}
