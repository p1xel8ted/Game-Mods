// Decompiled with JetBrains decompiler
// Type: UI_NavigatorSimple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using Unify.Input;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UI_NavigatorSimple : BaseMonoBehaviour
{
  public Selectable startingItem;
  public CanvasGroup canvasGroup;
  public string HorizontalNavAxisName = "Horizontal";
  public string VerticalNavAxisName = "Vertical";
  public float ButtonDownDelay;
  public bool DisableSFX;
  public Selectable prevSelectable;
  public Selectable selectable;
  public float SelectionDelay;
  public bool released;
  public bool cancelReleased;
  public Selectable newSelect;
  public bool canvasOff;
  public UI_NavigatorSimple.ChangeSelection OnChangeSelection;
  public System.Action OnSelectDown;
  public System.Action OnCancelDown;
  public System.Action OnDefaultSetComplete;

  public Player player => RewiredInputManager.MainPlayer;

  public void Start() => this.setDefault();

  public void setDefault()
  {
    if ((UnityEngine.Object) this.startingItem != (UnityEngine.Object) null && this.canvasGroup.interactable)
    {
      this.canvasOff = false;
      this.selectable = this.startingItem;
      this.newSelect = this.startingItem;
      this.prevSelectable = this.startingItem;
      this.unityNavigation();
    }
    System.Action defaultSetComplete = this.OnDefaultSetComplete;
    if (defaultSetComplete == null)
      return;
    defaultSetComplete();
  }

  public void unityNavigation()
  {
    if (!((UnityEngine.Object) this.newSelect != (UnityEngine.Object) null) || !this.newSelect.interactable || !this.newSelect.gameObject.activeSelf)
      return;
    this.SelectionDelay = 2.5f;
    this.selectable = this.newSelect;
    this.selectable.Select();
    if ((UnityEngine.Object) this.prevSelectable != (UnityEngine.Object) this.selectable)
    {
      UI_NavigatorSimple.ChangeSelection onChangeSelection = this.OnChangeSelection;
      if (onChangeSelection != null)
        onChangeSelection(this.selectable, this.prevSelectable);
      if (!this.DisableSFX)
        AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
    }
    this.prevSelectable = this.selectable;
  }

  public void Update()
  {
    if (!this.canvasGroup.interactable || (double) this.canvasGroup.alpha == 0.0)
    {
      this.canvasOff = true;
    }
    else
    {
      if (this.canvasOff)
        this.setDefault();
      this.SelectionDelay -= Time.unscaledDeltaTime;
      if (this.player == null)
        return;
      if (!this.released && !InputManager.UI.GetAcceptButtonHeld())
        this.released = true;
      if ((double) (this.ButtonDownDelay -= Time.unscaledDeltaTime) < 0.0 && this.released && InputManager.UI.GetAcceptButtonDown() && (UnityEngine.Object) this.selectable != (UnityEngine.Object) null)
      {
        System.Action onSelectDown = this.OnSelectDown;
        if (onSelectDown != null)
          onSelectDown();
        this.selectable.GetComponent<UnityEngine.UI.Button>()?.onClick?.Invoke();
        this.ButtonDownDelay = 0.2f;
        RumbleManager.Instance.Rumble();
        if (!this.DisableSFX)
          AudioManager.Instance.PlayOneShot("event:/ui/confirm_selection");
      }
      if (!this.cancelReleased && !InputManager.UI.GetCancelButtonHeld())
        this.cancelReleased = true;
      if ((double) (this.ButtonDownDelay -= Time.unscaledDeltaTime) < 0.0 && this.cancelReleased && InputManager.UI.GetCancelButtonUp())
      {
        System.Action onCancelDown = this.OnCancelDown;
        if (onCancelDown != null)
          onCancelDown();
        this.ButtonDownDelay = 0.2f;
      }
      if ((double) Mathf.Abs(InputManager.UI.GetHorizontalAxis()) <= 0.20000000298023224 && (double) Mathf.Abs(InputManager.UI.GetVerticalAxis()) <= 0.20000000298023224)
        this.SelectionDelay = 0.0f;
      if ((double) this.SelectionDelay >= 0.0 || !((UnityEngine.Object) this.selectable != (UnityEngine.Object) null))
        return;
      if ((double) InputManager.UI.GetHorizontalAxis() > 0.20000000298023224)
      {
        this.newSelect = this.selectable.FindSelectableOnRight();
        this.unityNavigation();
      }
      if ((double) InputManager.UI.GetHorizontalAxis() < -0.20000000298023224)
      {
        this.newSelect = this.selectable.FindSelectableOnLeft();
        this.unityNavigation();
      }
      if ((double) InputManager.UI.GetVerticalAxis() < -0.34999999403953552)
      {
        if ((UnityEngine.Object) this.selectable != (UnityEngine.Object) null && this.selectable is Scrollbar && (double) ((Scrollbar) this.selectable).value > 0.0099999997764825821)
          return;
        this.newSelect = this.selectable.FindSelectableOnDown();
        this.unityNavigation();
      }
      if ((double) InputManager.UI.GetVerticalAxis() <= 0.34999999403953552 || (UnityEngine.Object) this.selectable != (UnityEngine.Object) null && this.selectable is Scrollbar && (double) ((Scrollbar) this.selectable).value < 0.99000000953674316)
        return;
      this.newSelect = this.selectable.FindSelectableOnUp();
      this.unityNavigation();
    }
  }

  public delegate void ChangeSelection(Selectable NewSelectable, Selectable PrevSelectable);
}
