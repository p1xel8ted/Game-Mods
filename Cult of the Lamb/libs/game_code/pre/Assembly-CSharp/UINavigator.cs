// Decompiled with JetBrains decompiler
// Type: UINavigator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using System.Collections;
using System.Collections.Generic;
using Unify.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
public class UINavigator : BaseMonoBehaviour
{
  public bool isCardNavigator;
  public CanvasGroup canvasGroup;
  public Scrollbar scrollBar;
  public bool MoveAButton;
  public GameObject buttonToMove;
  public bool CallBackOnChange;
  public UnityEvent Callback;
  public bool Is2DArray;
  public int arrayWidth;
  public string HorizontalNavAxisName = "Horizontal";
  public string VerticalNavAxisName = "Vertical";
  private Vector3 velocity = Vector3.zero;
  private float startingScrollBarValue;
  private float SelectionDelay;
  public int _CurrentSelection;
  public float scrollValue;
  private bool canvasGroupOff;
  private Animator nextButtonAnim;
  private Animator prevButtonAnim;
  public bool updateButtons;
  public List<global::Buttons> Buttons = new List<global::Buttons>();
  public List<global::Buttons> dynamicButtons = new List<global::Buttons>();
  public List<global::Buttons> list = new List<global::Buttons>();
  public Selectable startingItem;
  public ScrollRect scrollRect;
  public Selectable _selectable;
  private Selectable newSelect;
  public bool useUnityNavigation;
  public bool focusOnSelected;
  public RectTransform focusMovementRect;
  public bool ControlsEnabled = true;
  public UINavigator.ChangeSelection OnChangeSelection;
  public UINavigator.ChangeSelectionUnity OnChangeSelectionUnity;
  public UINavigator.Deselect OnDeselect;
  public UINavigator.Close OnClose;
  public System.Action OnSelectDown;
  public int oldSelection = -1;
  private Animator animator;
  public bool released;
  private RectTransform rect;
  private RectTransform thisRect;
  public Vector3 focusOffset = new Vector3(0.0f, 0.0f, 0.0f);
  private float ButtonDownDelay;
  public UINavigator.ButtonDown OnButtonDown;

  private Player player => RewiredInputManager.MainPlayer;

  public Selectable selectable
  {
    get => this._selectable;
    set
    {
      if (!((UnityEngine.Object) this._selectable != (UnityEngine.Object) value))
        return;
      UINavigator.ChangeSelectionUnity changeSelectionUnity = this.OnChangeSelectionUnity;
      if (changeSelectionUnity != null)
        changeSelectionUnity(this._selectable, value);
      this._selectable = value;
      this._selectable.Select();
    }
  }

  public void updateIndex()
  {
    for (int index = 0; index < this.Buttons.Count; ++index)
      this.Buttons[index].Index = index;
  }

  private IEnumerator ScrollToTop()
  {
    if ((UnityEngine.Object) this.scrollRect != (UnityEngine.Object) null)
    {
      this.scrollRect.verticalNormalizedPosition = 1f;
      LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform) this.scrollRect.transform);
      yield break;
    }
  }

  private void Start()
  {
    this.released = false;
    this.setDefault();
    this.checkScrollBar();
    if (this.updateButtons)
    {
      this.newList();
      this.list = this.dynamicButtons;
    }
    else
      this.list = this.Buttons;
    this.StartCoroutine((IEnumerator) this.ScrollToTop());
  }

  private void checkScrollBar()
  {
    if (!((UnityEngine.Object) this.scrollBar != (UnityEngine.Object) null))
      return;
    CanvasGroup component = this.scrollBar.GetComponent<CanvasGroup>();
    if ((double) this.scrollBar.size >= 0.95)
    {
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.alpha = 0.0f;
    }
    else
    {
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.alpha = 1f;
    }
  }

  public void setDefault()
  {
    this.checkScrollBar();
    this.canvasGroupOff = false;
    if (!this.useUnityNavigation)
    {
      if (this.list.Count <= 0 || this.list[0] == null)
        return;
      this.oldSelection = -1;
      this._CurrentSelection = -1;
      this.CurrentSelection = 0;
      for (int index = 0; index < this.list.Count - 1 && !this.list[index].Button.activeInHierarchy; ++index)
        ++this.CurrentSelection;
    }
    else
    {
      if (!((UnityEngine.Object) this.startingItem != (UnityEngine.Object) null))
        return;
      this.selectable = this.startingItem;
      this.newSelect = this.startingItem;
      this.unityNavigation();
    }
  }

  public void Update()
  {
    if (this.player == null || !((UnityEngine.Object) this.canvasGroup != (UnityEngine.Object) null))
      return;
    if ((double) this.canvasGroup.alpha == 1.0)
    {
      this.Controls();
    }
    else
    {
      if (this.canvasGroupOff)
        return;
      this.canvasGroupOff = true;
    }
  }

  public void newList()
  {
    this.dynamicButtons.Clear();
    for (int index = 0; index < this.Buttons.Count; ++index)
    {
      if (this.Buttons[index].Button.activeSelf)
        this.dynamicButtons.Add(this.Buttons[index]);
    }
    this.setDefault();
  }

  private void MoveButton()
  {
    if (!this.MoveAButton)
      return;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.MoveButtonRoutine());
  }

  private IEnumerator MoveButtonRoutine()
  {
    if (!this.canvasGroup.interactable)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    Vector3 targetLocalPosition;
    Vector3 currentLocalPosition;
    if (!this.useUnityNavigation)
    {
      targetLocalPosition = this.buttonToMove.transform.parent.InverseTransformPoint(this.list[this._CurrentSelection].Button.transform.position);
      currentLocalPosition = this.buttonToMove.transform.localPosition;
    }
    else
    {
      targetLocalPosition = this.buttonToMove.transform.parent.InverseTransformPoint(this.selectable.transform.position);
      currentLocalPosition = this.buttonToMove.transform.localPosition;
    }
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime * 5f) <= 1.0)
    {
      this.buttonToMove.transform.localPosition = Vector3.SmoothDamp(targetLocalPosition, currentLocalPosition, ref this.velocity, Progress);
      yield return (object) null;
    }
    yield return (object) null;
  }

  private void changeSetting(int i, int valueToIncrease)
  {
  }

  public int CurrentSelection
  {
    get => this._CurrentSelection;
    set
    {
      if (this.Callback != null)
        this.Callback?.Invoke();
      if (this.list.Count <= 0)
        return;
      AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
      if ((UnityEngine.Object) this.selectable != (UnityEngine.Object) null)
      {
        UINavigator.Deselect onDeselect = this.OnDeselect;
        if (onDeselect != null)
          onDeselect(this.list[this._CurrentSelection]);
      }
      this.SelectionDelay = 0.5f;
      this.oldSelection = this._CurrentSelection;
      this._CurrentSelection = value;
      if (this._CurrentSelection < 0)
        this._CurrentSelection = this.list.Count - 1;
      if (this._CurrentSelection > this.list.Count - 1)
        this._CurrentSelection = 0;
      if (this.list[this._CurrentSelection] != null)
      {
        GameObject button = this.list[this._CurrentSelection].Button;
        if ((UnityEngine.Object) button == (UnityEngine.Object) null)
          return;
        this.animator = button.GetComponent<Animator>();
        if ((UnityEngine.Object) this.animator != (UnityEngine.Object) null && this.oldSelection != this._CurrentSelection)
          this.animator.SetTrigger("Selected");
        if (this.oldSelection != -1 && this.oldSelection != this._CurrentSelection)
        {
          this.animator = this.list[this.oldSelection].Button.GetComponent<Animator>();
          if ((UnityEngine.Object) this.animator != (UnityEngine.Object) null)
            this.animator.SetTrigger("Normal");
        }
        this.selectable = this.list[this._CurrentSelection].Button.GetComponent<Selectable>();
        if ((UnityEngine.Object) this.selectable != (UnityEngine.Object) null)
        {
          this.selectable.Select();
        }
        else
        {
          UnityEngine.UI.Button component = this.list[this._CurrentSelection].Button.GetComponent<UnityEngine.UI.Button>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.Select();
        }
      }
      this.MoveButton();
      UINavigator.ChangeSelection onChangeSelection = this.OnChangeSelection;
      if (onChangeSelection == null)
        return;
      onChangeSelection(this.list[this._CurrentSelection]);
    }
  }

  private void updateScrollBar()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.MoveScrollBar());
  }

  private IEnumerator MoveScrollBar()
  {
    if (!this.canvasGroup.interactable)
      yield return (object) null;
    float Progress = 0.0f;
    this.startingScrollBarValue = this.scrollBar.value;
    this.scrollValue = (float) this._CurrentSelection / (float) (this.list.Count - 1);
    --this.scrollValue;
    this.scrollValue *= -1f;
    while ((double) (Progress += Time.unscaledDeltaTime * 5f) <= 1.0)
    {
      this.scrollBar.value = Mathf.SmoothStep(this.startingScrollBarValue, this.scrollValue, Progress);
      yield return (object) null;
    }
  }

  private void updateScrollBarHorizontal()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.MoveScrollBarHorizontal());
  }

  private IEnumerator MoveScrollBarHorizontal()
  {
    if (!this.canvasGroup.interactable)
      yield return (object) null;
    float Progress = 0.0f;
    this.startingScrollBarValue = this.scrollBar.value;
    this.scrollValue = (float) this._CurrentSelection / (float) this.list.Count;
    while ((double) (Progress += Time.unscaledDeltaTime * 5f) <= 1.0)
    {
      this.scrollBar.value = Mathf.SmoothStep(this.startingScrollBarValue, this.scrollValue, Progress);
      yield return (object) null;
    }
  }

  private void unityNavigation()
  {
    if (!((UnityEngine.Object) this.newSelect != (UnityEngine.Object) null) || !this.newSelect.gameObject.activeSelf)
      return;
    this.SelectionDelay = 0.5f;
    this.animator = this.newSelect.gameObject.GetComponent<Animator>();
    if ((UnityEngine.Object) this.animator != (UnityEngine.Object) null)
      this.animator.SetTrigger("Selected");
    this.selectable = this.newSelect;
    this.MoveButton();
    this.selectable.Select();
    if (!((UnityEngine.Object) this.scrollBar != (UnityEngine.Object) null))
      return;
    this.updateScrollBar();
  }

  private void focusOnSelectedObject()
  {
    this.rect = this.selectable.gameObject.GetComponent<RectTransform>();
    if ((UnityEngine.Object) this.focusMovementRect == (UnityEngine.Object) null)
      this.focusMovementRect = this.gameObject.GetComponent<RectTransform>();
    this.focusMovementRect.position = Vector3.Lerp(this.focusMovementRect.position, -this.rect.position + this.focusOffset, 10f * Time.unscaledDeltaTime);
  }

  private void DeselectButton()
  {
    if ((UnityEngine.Object) this.newSelect != (UnityEngine.Object) null)
      this.animator = this.newSelect.gameObject.GetComponent<Animator>();
    if (!((UnityEngine.Object) this.animator != (UnityEngine.Object) null))
      return;
    this.animator.SetTrigger("Normal");
  }

  private void Controls()
  {
    if (this.focusOnSelected)
      this.focusOnSelectedObject();
    if (!this.ControlsEnabled || !this.canvasGroup.interactable || (double) this.canvasGroup.alpha == 0.0)
      return;
    if (this.canvasGroupOff)
      this.setDefault();
    this.SelectionDelay -= Time.unscaledDeltaTime;
    if ((double) this.SelectionDelay < 0.0)
    {
      if (!this.useUnityNavigation)
      {
        if (this.Is2DArray)
        {
          if (this.canvasGroup.interactable)
          {
            if ((UnityEngine.Object) this.scrollBar != (UnityEngine.Object) null)
              this.updateScrollBarHorizontal();
            if ((double) this.player.GetAxis(this.HorizontalNavAxisName) > 0.20000000298023224)
              ++this.CurrentSelection;
            if ((double) this.player.GetAxis(this.HorizontalNavAxisName) < -0.20000000298023224)
              --this.CurrentSelection;
          }
          else
          {
            if ((double) this.player.GetAxis(this.HorizontalNavAxisName) > 0.20000000298023224 && (double) Mathf.Abs(this.player.GetAxis(this.VerticalNavAxisName)) < 0.34999999403953552)
              this.changeSetting(this.CurrentSelection, 1);
            if ((double) this.player.GetAxis(this.HorizontalNavAxisName) < -0.20000000298023224 && (double) Mathf.Abs(this.player.GetAxis(this.VerticalNavAxisName)) < 0.34999999403953552)
              this.changeSetting(this.CurrentSelection, -1);
          }
        }
        else if (this.list.Count > 0 && this.list[0].isSetting)
        {
          if ((double) this.player.GetAxis(this.HorizontalNavAxisName) > 0.20000000298023224 && (double) Mathf.Abs(this.player.GetAxis(this.VerticalNavAxisName)) < 0.34999999403953552)
            this.changeSetting(this.CurrentSelection, 1);
          if ((double) this.player.GetAxis(this.HorizontalNavAxisName) < -0.20000000298023224 && (double) Mathf.Abs(this.player.GetAxis(this.VerticalNavAxisName)) < 0.34999999403953552)
            this.changeSetting(this.CurrentSelection, -1);
        }
      }
      else
      {
        if ((double) this.player.GetAxis(this.HorizontalNavAxisName) > 0.20000000298023224)
        {
          this.DeselectButton();
          this.newSelect = this.selectable.FindSelectableOnRight();
          this.unityNavigation();
        }
        if ((double) this.player.GetAxis(this.HorizontalNavAxisName) < -0.20000000298023224)
        {
          this.DeselectButton();
          this.newSelect = this.selectable.FindSelectableOnLeft();
          this.unityNavigation();
        }
      }
    }
    if ((double) this.SelectionDelay < 0.0 && this.list.Count > 0)
    {
      if (!this.useUnityNavigation)
      {
        if ((double) this.player.GetAxis(this.VerticalNavAxisName) < -0.34999999403953552)
        {
          if (this.Is2DArray)
          {
            this.CurrentSelection += this.arrayWidth;
          }
          else
          {
            for (int index = Mathf.Min(this.CurrentSelection + 1, this.list.Count - 1); index < this.list.Count; ++index)
            {
              if (this.list[index].Button.activeInHierarchy)
              {
                this.CurrentSelection = index;
                break;
              }
            }
          }
          if ((UnityEngine.Object) this.scrollBar != (UnityEngine.Object) null)
            this.updateScrollBar();
        }
        if ((double) this.player.GetAxis(this.VerticalNavAxisName) > 0.34999999403953552)
        {
          if (this.Is2DArray)
          {
            this.CurrentSelection -= this.arrayWidth;
          }
          else
          {
            for (int index = Mathf.Max(this.CurrentSelection - 1, 0); index >= 0; --index)
            {
              if (this.list[index].Button.activeInHierarchy)
              {
                this.CurrentSelection = index;
                break;
              }
            }
          }
          if ((UnityEngine.Object) this.scrollBar != (UnityEngine.Object) null)
            this.updateScrollBar();
        }
      }
      else
      {
        if ((double) this.player.GetAxis(this.VerticalNavAxisName) < -0.34999999403953552)
        {
          this.DeselectButton();
          this.newSelect = this.selectable.FindSelectableOnDown();
          this.unityNavigation();
        }
        if ((double) this.player.GetAxis(this.VerticalNavAxisName) > 0.34999999403953552)
        {
          this.DeselectButton();
          this.newSelect = this.selectable.FindSelectableOnUp();
          this.unityNavigation();
        }
      }
    }
    if ((double) this.SelectionDelay > 0.10000000149011612 && (double) Mathf.Abs(this.player.GetAxis(this.HorizontalNavAxisName)) <= 0.20000000298023224 && (double) Mathf.Abs(this.player.GetAxis(this.VerticalNavAxisName)) <= 0.34999999403953552)
      this.SelectionDelay = 0.1f;
    if ((double) (this.ButtonDownDelay -= Time.deltaTime) < 0.0 && this.released && InputManager.UI.GetAcceptButtonHeld())
    {
      if (!this.useUnityNavigation)
      {
        if (this.list.Count > 0)
        {
          UINavigator.ButtonDown onButtonDown = this.OnButtonDown;
          if (onButtonDown != null)
            onButtonDown(this.list[this.CurrentSelection]);
        }
      }
      else
      {
        this.StopAllCoroutines();
        this.selectable.GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
      }
      this.ButtonDownDelay = 0.2f;
    }
    if (!this.released && !InputManager.UI.GetAcceptButtonHeld())
      this.released = true;
    if (this.useUnityNavigation)
      return;
    if (this.list.Count > 0 && InputManager.UI.GetAcceptButtonDown() && this.canvasGroup.interactable)
    {
      if (buttons.Toggle == this.list[this.CurrentSelection].buttonTypes)
      {
        this.changeSetting(this.CurrentSelection, 1);
      }
      else
      {
        UnityEngine.UI.Button component = this.list[this.CurrentSelection].Button.GetComponent<UnityEngine.UI.Button>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.onClick.Invoke();
      }
    }
    if (InputManager.UI.GetAcceptButtonDown())
    {
      System.Action onSelectDown = this.OnSelectDown;
      if (onSelectDown != null)
        onSelectDown();
    }
    if ((double) this.SelectionDelay > 0.0 || !InputManager.UI.GetCancelButtonUp() || this.isCardNavigator && (!this.isCardNavigator || UIWeaponCardSoul.uIWeaponCardSouls.Count > 0))
      return;
    UINavigator.Close onClose = this.OnClose;
    if (onClose == null)
      return;
    onClose();
  }

  public delegate void ChangeSelection(global::Buttons Button);

  public delegate void ChangeSelectionUnity(Selectable NewSelectable, Selectable PrevSelectable);

  public delegate void Deselect(global::Buttons Button);

  public delegate void Close();

  public delegate void ButtonDown(global::Buttons CurrentButton);
}
