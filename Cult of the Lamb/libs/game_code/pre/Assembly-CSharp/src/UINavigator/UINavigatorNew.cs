// Decompiled with JetBrains decompiler
// Type: src.UINavigator.UINavigatorNew
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private const float kSelectionDelayMax = 0.2f;
  private const float kSelectionHoldDelayReduction = 0.05f;
  private const float kSelectionHoldDelayReductionThreshold = 2f;
  private const float kButtonDownDelayMax = 0.1f;
  private const float kButtonHoldDelayMax = 0.25f;
  private const float kButtonHoldDelayReduction = 0.05f;
  private const int kButtonHoldDelayReductionThreshold = 2;
  public Action<Selectable, Selectable> OnSelectionChanged;
  public Action<Selectable> OnDefaultSetComplete;
  public System.Action OnCancelDown;
  public System.Action OnPageNavigateLeft;
  public System.Action OnPageNavigateRight;
  public System.Action OnClear;
  public bool AllowAcceptHold;
  public bool LockNavigation;
  public bool LockInput;
  [SerializeField]
  private bool _disableSFX;
  private RewiredStandaloneInputModule _inputModule;
  private Vector3 _recentMoveVector;
  private float _selectionDelay;
  private float _buttonDownDelay;
  private int _navigationHold;
  private int _consecutiveHold;
  private IMMSelectable _currentSelectable;
  private Vector3 _previousMouseInput;

  public IMMSelectable CurrentSelectable => this._currentSelectable;

  public Vector2 RecentMoveVector => (Vector2) this._recentMoveVector;

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

  private void OnActiveSceneChanged(Scene current, Scene next) => this.EnsureInputModuleUpdated();

  private void EnsureInputModuleUpdated()
  {
    if ((UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null)
      EventSystem.current.transform.SetParent((Transform) null);
    if (!((UnityEngine.Object) MonoSingleton<RewiredEventSystem>.Instance != (UnityEngine.Object) null))
      return;
    this._inputModule = MonoSingleton<RewiredEventSystem>.Instance.GetComponent<RewiredStandaloneInputModule>();
    this._inputModule.allowMouseInput = InputManager.General.MouseInputActive;
    this._previousMouseInput = Input.mousePosition;
  }

  private void Update()
  {
    switch (UnifyManager.platform)
    {
      case UnifyManager.Platform.None:
      case UnifyManager.Platform.Standalone:
        if ((UnityEngine.Object) this._inputModule != (UnityEngine.Object) null)
        {
          Controller activeController = InputManager.General.GetLastActiveController();
          if (activeController != null)
          {
            if (!this._inputModule.allowMouseInput)
            {
              if (Input.mousePosition != this._previousMouseInput)
              {
                this._inputModule.allowMouseInput = true;
                InputManager.General.MouseInputActive = true;
                break;
              }
              break;
            }
            if (activeController.type != ControllerType.Mouse && InputManager.General.GetAnyButton())
            {
              this._previousMouseInput = Input.mousePosition;
              this._inputModule.allowMouseInput = false;
              InputManager.General.MouseInputActive = false;
              if (this._currentSelectable != null)
              {
                this._currentSelectable.Selectable.OnPointerExit((PointerEventData) null);
                break;
              }
              break;
            }
            break;
          }
          break;
        }
        break;
    }
    if (!this.LockInput)
    {
      if ((double) this._buttonDownDelay <= 0.0)
      {
        if (InputManager.UI.GetAcceptButtonDown() || this.AllowAcceptHold && InputManager.UI.GetAcceptButtonHeld())
          this.PerformConfirmAction();
        else if (InputManager.UI.GetCancelButtonDown())
          this.PerformCancelAction();
      }
      else
        this._buttonDownDelay -= Time.unscaledDeltaTime;
      if (InputManager.UI.GetAcceptButtonUp())
      {
        this._consecutiveHold = 0;
        this._buttonDownDelay = 0.0f;
      }
    }
    if (this.LockNavigation)
      return;
    if ((double) this._selectionDelay <= 0.0)
    {
      if (this._currentSelectable != null)
      {
        IMMSelectable newSelectable = (IMMSelectable) null;
        if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) > (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()))
        {
          if ((double) InputManager.UI.GetHorizontalAxis() > 0.20000000298023224)
            newSelectable = this._currentSelectable.TryNavigateRight();
          else if ((double) InputManager.UI.GetHorizontalAxis() < -0.20000000298023224)
            newSelectable = this._currentSelectable.TryNavigateLeft();
        }
        else if ((double) InputManager.UI.GetVerticalAxis() > 0.20000000298023224)
          newSelectable = this._currentSelectable.TryNavigateUp();
        else if ((double) InputManager.UI.GetVerticalAxis() < -0.20000000298023224)
          newSelectable = this._currentSelectable.TryNavigateDown();
        if (newSelectable != null)
          this.ChangeSelection(newSelectable);
      }
      if (InputManager.UI.GetPageNavigateLeftDown())
        this.PerformPageNavigationLeft();
      if (InputManager.UI.GetPageNavigateRightDown())
        this.PerformPageNavigationRight();
    }
    else
      this._selectionDelay -= Time.unscaledDeltaTime;
    if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) >= 0.20000000298023224 || (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) >= 0.20000000298023224)
      return;
    this._navigationHold = 0;
    this._selectionDelay = 0.0f;
  }

  private void PerformConfirmAction()
  {
    if (this._currentSelectable == null || !this._currentSelectable.Interactable)
      return;
    this.PerformButtonAction(true);
    this._currentSelectable.TryPerformConfirmAction();
  }

  private void PerformCancelAction()
  {
    this.PerformButtonAction();
    System.Action onCancelDown = this.OnCancelDown;
    if (onCancelDown == null)
      return;
    onCancelDown();
  }

  private void PerformPageNavigationLeft()
  {
    this.PerformNavigationAction();
    System.Action pageNavigateLeft = this.OnPageNavigateLeft;
    if (pageNavigateLeft == null)
      return;
    pageNavigateLeft();
  }

  private void PerformPageNavigationRight()
  {
    this.PerformNavigationAction();
    System.Action pageNavigateRight = this.OnPageNavigateRight;
    if (pageNavigateRight == null)
      return;
    pageNavigateRight();
  }

  private void PerformButtonAction(bool confirmation = false)
  {
    if (this.AllowAcceptHold & confirmation)
    {
      this._buttonDownDelay = 0.25f - Mathf.Min(Mathf.Max(0.05f * (float) (this._consecutiveHold - 2), 0.0f), 0.15f);
      ++this._consecutiveHold;
    }
    else
      this._buttonDownDelay = 0.1f;
  }

  private void ChangeSelection(IMMSelectable newSelectable)
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

  private void NavigateTo(IMMSelectable newSelectable)
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

  private void PerformNavigationAction()
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
