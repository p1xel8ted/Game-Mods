// Decompiled with JetBrains decompiler
// Type: ChoiceIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class ChoiceIndicator : BaseMonoBehaviour
{
  public static HashSet<ChoiceIndicator> activeChoiceIndicators = new HashSet<ChoiceIndicator>();
  [Header("Offset")]
  public Vector3 Offset;
  [Header("Arrow")]
  public RectTransform _arrowRectTransform;
  [Header("Choices")]
  [SerializeField]
  public ChoiceIndicator.ChoiceOption _choice1;
  [SerializeField]
  public ChoiceIndicator.ChoiceOption _choice2;
  [Header("Objectives Box")]
  [SerializeField]
  public GameObject _informationBox;
  [SerializeField]
  public Localize _informationTitle;
  [SerializeField]
  public TextMeshProUGUI _informationDescription;
  [Header("Prompt")]
  [SerializeField]
  public GameObject _promptBox;
  [SerializeField]
  public Localize _prompt;
  [Header("Misc")]
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public RectTransform _rectTransform;
  public float _yLerpSpeed = 20f;
  public Camera _currentMain;
  public Vector3 _position;
  public ObjectivesData _objectivesData;
  public bool playSelectSfx = true;
  public bool playConfirmSfx = true;
  public IMMSelectable _selectable;

  public float _offsetResolutionYMultiplier => (float) Screen.height / 1080f;

  public void OnEnable()
  {
    UIMenuBase.OnFirstMenuShow += new System.Action(this.OnFirstMenuShow);
    UIMenuBase.OnFinalMenuHide += new System.Action(this.OnFinalMenuHide);
    this._choice1.Button.onClick.AddListener((UnityAction) (() => this.OnOptionClicked(this._choice1)));
    this._choice2.Button.onClick.AddListener((UnityAction) (() => this.OnOptionClicked(this._choice2)));
    this._choice1.Button.OnSelected += (System.Action) (() => this.OnOptionSelected(this._choice1.Button));
    this._choice2.Button.OnSelected += (System.Action) (() => this.OnOptionSelected(this._choice2.Button));
    this._choice1.Button.OnDeselected += (System.Action) (() => this.OnOptionDeselected(this._choice1.Button));
    this._choice2.Button.OnDeselected += (System.Action) (() => this.OnOptionDeselected(this._choice2.Button));
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    ChoiceIndicator.activeChoiceIndicators.Add(this);
    this.transform.SetAsLastSibling();
  }

  public void OnDisable()
  {
    UIMenuBase.OnFirstMenuShow -= new System.Action(this.OnFirstMenuShow);
    UIMenuBase.OnFinalMenuHide -= new System.Action(this.OnFinalMenuHide);
    this._choice1.Button.onClick.RemoveAllListeners();
    this._choice1.Button.OnSelected = (System.Action) null;
    this._choice1.Button.OnDeselected = (System.Action) null;
    this._choice2.Button.onClick.RemoveAllListeners();
    this._choice2.Button.OnSelected = (System.Action) null;
    this._choice2.Button.OnDeselected = (System.Action) null;
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.LocalizeObjective);
    ChoiceIndicator.activeChoiceIndicators.Remove(this);
    if (ChoiceIndicator.activeChoiceIndicators.Count > 0)
      return;
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
  }

  public void Show(
    string choiceTerm1,
    string choiceTerm2,
    System.Action choice1Callback,
    System.Action choice2Callback,
    Vector3 worldPosition,
    bool forceUpdate = false,
    bool playSelectSfx = true,
    bool playConfirmSfx = true)
  {
    this.Show(choiceTerm1, string.Empty, choiceTerm2, string.Empty, choice1Callback, choice2Callback, worldPosition, forceUpdate, playSelectSfx, playConfirmSfx);
  }

  public void Show(
    string choice1Term,
    string choice1SubtitleTerm,
    string choice2Term,
    string choice2SubtitleTerm,
    System.Action choice1Callback,
    System.Action choice2Callback,
    Vector3 worldPosition,
    bool forceUpdate = false,
    bool playSelectSfx = true,
    bool playConfirmSfx = true)
  {
    this._choice1.TextLocalize.Term = choice1Term;
    if (!string.IsNullOrEmpty(choice1SubtitleTerm))
      this._choice1.SubtitleLocalize.Term = choice1SubtitleTerm;
    else
      this._choice1.SubtitleLocalize.gameObject.SetActive(false);
    this._choice1.Callback = choice1Callback;
    this._choice2.TextLocalize.Term = choice2Term;
    if (!string.IsNullOrEmpty(choice2SubtitleTerm))
      this._choice2.SubtitleLocalize.Term = choice2SubtitleTerm;
    else
      this._choice2.SubtitleLocalize.gameObject.SetActive(false);
    this._choice2.Callback = choice2Callback;
    this.playSelectSfx = playSelectSfx;
    this.playConfirmSfx = playConfirmSfx;
    this.Show(worldPosition, forceUpdate);
  }

  public void Show(Vector3 worldPosition, bool forceUpdate = false)
  {
    this._currentMain = Camera.main;
    if (forceUpdate)
      this.UpdatePosition(worldPosition);
    this._rectTransform.position = this._currentMain.WorldToScreenPoint(worldPosition) - this.Offset * this._offsetResolutionYMultiplier;
    this._informationBox.SetActive(false);
    this._promptBox.SetActive(false);
    this.StartCoroutine((IEnumerator) this.DoShow());
  }

  public void ShowObjectivesBox(ObjectivesData objectivesData)
  {
    this._objectivesData = objectivesData;
    this._informationBox.SetActive(true);
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.LocalizeObjective);
    this.LocalizeObjective();
  }

  public void LocalizeObjective()
  {
    if (!string.IsNullOrEmpty(this._objectivesData.GroupId))
      this._informationTitle.Term = this._objectivesData.GroupId;
    else
      this._informationTitle.gameObject.SetActive(false);
    if (!string.IsNullOrEmpty(this._objectivesData.Text))
      this._informationDescription.text = this._objectivesData.Text;
    else
      this._informationDescription.gameObject.SetActive(false);
  }

  public void ShowPrompt(string prompt)
  {
    this._promptBox.SetActive(true);
    this._prompt.Term = prompt;
  }

  public void OnOptionSelected(MMButton button)
  {
    if (this.playSelectSfx)
      AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
    button.transform.DOKill();
    button.transform.DOScale(Vector3.one * 1.1f, 0.15f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    Quaternion endValue = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Clamp(this._arrowRectTransform.position.x - button.transform.position.x, -1f, 1f) * 90f));
    this._arrowRectTransform.DOKill();
    this._arrowRectTransform.DORotateQuaternion(endValue, 0.15f).SetUpdate<TweenerCore<Quaternion, Quaternion, NoOptions>>(true).SetEase<TweenerCore<Quaternion, Quaternion, NoOptions>>(Ease.InOutSine);
    this._selectable = (IMMSelectable) button;
  }

  public void OnOptionDeselected(MMButton button)
  {
    button.transform.DOKill();
    button.transform.DOScale(Vector3.one, 0.15f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }

  public void OnOptionClicked(ChoiceIndicator.ChoiceOption choiceOption)
  {
    if (this.playConfirmSfx)
      AudioManager.Instance.PlayOneShot("event:/sermon/select_upgrade");
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    System.Action callback = choiceOption.Callback;
    if (callback != null)
      callback();
    this.Hide();
  }

  public void UpdatePosition(Vector3 worldPosition)
  {
    this._position = this._currentMain.WorldToScreenPoint(worldPosition) - this.Offset * this._offsetResolutionYMultiplier;
  }

  public void LateUpdate()
  {
    this._rectTransform.position = Vector3.Lerp(this._rectTransform.position, this._position, Time.unscaledDeltaTime * this._yLerpSpeed);
  }

  public void Update()
  {
    if (!this._canvasGroup.interactable)
      return;
    float f = InputManager.UI.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer);
    if (InputManager.General.MouseInputActive)
      f = (double) InputManager.General.GetMousePosition(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer).x < (double) Screen.width / 2.0 ? -1f : 1f;
    if ((double) Mathf.Abs(f) > 0.20000000298023224)
    {
      if ((double) f < 0.0)
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._choice1.Button);
      else
        MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._choice2.Button);
    }
    if (!InputManager.General.MouseInputActive || !InputManager.Gameplay.GetInteractButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      return;
    if ((double) f < 0.0)
      this._choice1.Button.onClick?.Invoke();
    else
      this._choice2.Button.onClick?.Invoke();
  }

  public void Hide() => this.StartCoroutine((IEnumerator) this.DoHide());

  public IEnumerator DoShow()
  {
    this._canvasGroup.interactable = false;
    float progress = 0.0f;
    float duration = 0.5f;
    while ((double) (progress += Time.unscaledDeltaTime) < (double) duration)
    {
      this._canvasGroup.alpha = progress / duration;
      this._rectTransform.localScale = Vector3.one * Mathf.SmoothStep(1.2f, 1f, progress / duration);
      yield return (object) null;
    }
    this._canvasGroup.alpha = 1f;
    this._rectTransform.localScale = Vector3.one;
    this._canvasGroup.interactable = true;
  }

  public IEnumerator DoHide()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    ChoiceIndicator choiceIndicator = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      UnityEngine.Object.Destroy((UnityEngine.Object) choiceIndicator.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    choiceIndicator._canvasGroup.interactable = false;
    choiceIndicator.SetActiveStateForMenu(false);
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    choiceIndicator._canvasGroup.DOKill();
    choiceIndicator._canvasGroup.DOFade(0.0f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSecondsRealtime(0.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnFirstMenuShow() => this.SetActiveStateForMenu(false);

  public void OnFinalMenuHide() => this.SetActiveStateForMenu(true);

  public virtual void SetActiveStateForMenu(bool state)
  {
    foreach (IMMSelectable componentsInChild in this.gameObject.GetComponentsInChildren<IMMSelectable>())
      componentsInChild.SetInteractionState(state);
    if (this._selectable == null || !state)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(this._selectable);
  }

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__21_0() => this.OnOptionClicked(this._choice1);

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__21_1() => this.OnOptionClicked(this._choice2);

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__21_2() => this.OnOptionSelected(this._choice1.Button);

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__21_3() => this.OnOptionSelected(this._choice2.Button);

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__21_4() => this.OnOptionDeselected(this._choice1.Button);

  [CompilerGenerated]
  public void \u003COnEnable\u003Eb__21_5() => this.OnOptionDeselected(this._choice2.Button);

  [Serializable]
  public struct ChoiceOption
  {
    public MMButton Button;
    public TextMeshProUGUI Text;
    public Localize TextLocalize;
    public TextMeshProUGUI SubtitleText;
    public Localize SubtitleLocalize;
    public System.Action Callback;
  }
}
