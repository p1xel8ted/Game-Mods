// Decompiled with JetBrains decompiler
// Type: ChoiceIndicator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.UINavigator;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class ChoiceIndicator : BaseMonoBehaviour
{
  [Header("Offset")]
  public Vector3 Offset;
  [Header("Arrow")]
  public RectTransform _arrowRectTransform;
  [Header("Choices")]
  [SerializeField]
  private ChoiceIndicator.ChoiceOption _choice1;
  [SerializeField]
  private ChoiceIndicator.ChoiceOption _choice2;
  [Header("Objectives Box")]
  [SerializeField]
  private GameObject _informationBox;
  [SerializeField]
  private Localize _informationTitle;
  [SerializeField]
  private TextMeshProUGUI _informationDescription;
  [Header("Prompt")]
  [SerializeField]
  private GameObject _promptBox;
  [SerializeField]
  private Localize _prompt;
  [Header("Misc")]
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private RectTransform _rectTransform;
  private float _yLerpSpeed = 20f;
  private Camera _currentMain;
  private Vector3 _position;
  private ObjectivesData _objectivesData;
  private IMMSelectable _selectable;

  private void OnEnable()
  {
    UIMenuBase.OnFirstMenuShow += new System.Action(this.OnFirstMenuShow);
    UIMenuBase.OnFinalMenuHide += new System.Action(this.OnFinalMenuHide);
    this._choice1.Button.onClick.AddListener((UnityAction) (() => this.OnOptionClicked(this._choice1)));
    this._choice2.Button.onClick.AddListener((UnityAction) (() => this.OnOptionClicked(this._choice2)));
    this._choice1.Button.OnSelected += (System.Action) (() => this.OnOptionSelected(this._choice1.Button));
    this._choice2.Button.OnSelected += (System.Action) (() => this.OnOptionSelected(this._choice2.Button));
    this._choice1.Button.OnDeselected += (System.Action) (() => this.OnOptionDeselected(this._choice1.Button));
    this._choice2.Button.OnDeselected += (System.Action) (() => this.OnOptionDeselected(this._choice2.Button));
  }

  private void OnDisable()
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
  }

  public void Show(
    string choiceTerm1,
    string choiceTerm2,
    System.Action choice1Callback,
    System.Action choice2Callback,
    Vector3 worldPosition)
  {
    this.Show(choiceTerm1, string.Empty, choiceTerm2, string.Empty, choice1Callback, choice2Callback, worldPosition);
  }

  public void Show(
    string choice1Term,
    string choice1SubtitleTerm,
    string choice2Term,
    string choice2SubtitleTerm,
    System.Action choice1Callback,
    System.Action choice2Callback,
    Vector3 worldPosition)
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
    this.Show(worldPosition);
  }

  private void Show(Vector3 worldPosition)
  {
    this._currentMain = Camera.main;
    this._rectTransform.position = this._currentMain.WorldToScreenPoint(worldPosition) - this.Offset;
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

  private void LocalizeObjective()
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

  private void OnOptionSelected(MMButton button)
  {
    AudioManager.Instance.PlayOneShot("event:/ui/change_selection");
    button.transform.DOKill();
    button.transform.DOScale(Vector3.one * 1.1f, 0.15f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    Quaternion endValue = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Clamp(this._arrowRectTransform.position.x - button.transform.position.x, -1f, 1f) * 90f));
    this._arrowRectTransform.DOKill();
    this._arrowRectTransform.DORotateQuaternion(endValue, 0.15f).SetUpdate<TweenerCore<Quaternion, Quaternion, NoOptions>>(true).SetEase<TweenerCore<Quaternion, Quaternion, NoOptions>>(Ease.InOutSine);
    this._selectable = (IMMSelectable) button;
  }

  private void OnOptionDeselected(MMButton button)
  {
    button.transform.DOKill();
    button.transform.DOScale(Vector3.one, 0.15f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
  }

  private void OnOptionClicked(ChoiceIndicator.ChoiceOption choiceOption)
  {
    AudioManager.Instance.PlayOneShot("event:/sermon/select_upgrade");
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    System.Action callback = choiceOption.Callback;
    if (callback != null)
      callback();
    this.Hide();
  }

  public void UpdatePosition(Vector3 worldPosition)
  {
    this._position = this._currentMain.WorldToScreenPoint(worldPosition) - this.Offset;
  }

  private void LateUpdate()
  {
    this._rectTransform.position = Vector3.Lerp(this._rectTransform.position, this._position, Time.unscaledDeltaTime * this._yLerpSpeed);
  }

  private void Update()
  {
    if (!this._canvasGroup.interactable || this._selectable != null || InputManager.General.MouseInputActive)
      return;
    float horizontalAxis = InputManager.UI.GetHorizontalAxis();
    if ((double) Mathf.Abs(horizontalAxis) <= 0.20000000298023224)
      return;
    if ((double) horizontalAxis < 0.0)
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._choice1.Button);
    else
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this._choice2.Button);
  }

  private void Hide() => this.StartCoroutine((IEnumerator) this.DoHide());

  private IEnumerator DoShow()
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

  private IEnumerator DoHide()
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

  private void OnFirstMenuShow() => this.SetActiveStateForMenu(false);

  private void OnFinalMenuHide() => this.SetActiveStateForMenu(true);

  protected virtual void SetActiveStateForMenu(bool state)
  {
    foreach (IMMSelectable componentsInChild in this.gameObject.GetComponentsInChildren<IMMSelectable>())
      componentsInChild.SetInteractionState(state);
    if (this._selectable == null || !state)
      return;
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew(this._selectable);
  }

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
