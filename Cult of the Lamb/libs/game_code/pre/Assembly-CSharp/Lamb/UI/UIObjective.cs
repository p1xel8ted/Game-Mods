// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIObjective
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Spine.Unity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIObjective : BaseMonoBehaviour
{
  protected const string kTickBoxOnAnimation = "on";
  protected const string kTickBoxTurnOnAnimation = "turn-on";
  protected const string kTickBoxOffAnimation = "off";
  protected const string kTickBoxTurnOffFailedAnimation = "turn-on-failed";
  protected const string kTickBoxFailedAnimation = "on-failed";
  [SerializeField]
  private TextMeshProUGUI _text;
  [SerializeField]
  private SkeletonGraphic _tickBox;
  [SerializeField]
  private Image[] _strikethroughs;
  [SerializeField]
  private GameObject _timeContainer;
  [SerializeField]
  private Image _timeWheel;
  private ObjectivesData _objectivesData;
  private Coroutine _deferredStrikethroughUpdate;

  private int NumLines => this._text.textInfo.lineCount;

  public ObjectivesData ObjectivesData => this._objectivesData;

  private void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
    ObjectiveManager.OnObjectiveUpdated += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveUpdated);
    Singleton<AccessibilityManager>.Instance.OnTextScaleChanged += new System.Action(this.OnTextScaleChanged);
  }

  private void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
    ObjectiveManager.OnObjectiveUpdated -= new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveUpdated);
    Singleton<AccessibilityManager>.Instance.OnTextScaleChanged -= new System.Action(this.OnTextScaleChanged);
  }

  private void Localize()
  {
    this._text.text = this._objectivesData.Text;
    this.UpdateStrikethroughs();
  }

  private void OnTextScaleChanged() => this.UpdateStrikethroughs();

  public void Configure(ObjectivesData objectivesData, bool ignoreStatus)
  {
    foreach (Image strikethrough in this._strikethroughs)
      strikethrough.fillAmount = 0.0f;
    this._objectivesData = objectivesData;
    if (!ignoreStatus)
    {
      if (objectivesData.IsComplete)
      {
        this._text.color = StaticColors.GreyColor;
        this._tickBox.SetAnimation("on");
        foreach (Image strikethrough in this._strikethroughs)
          strikethrough.fillAmount = 1f;
      }
      else if (objectivesData.IsFailed)
      {
        this._text.color = StaticColors.RedColor;
        this._tickBox.SetAnimation("on-failed");
        foreach (Image strikethrough in this._strikethroughs)
        {
          strikethrough.fillAmount = 1f;
          strikethrough.color = StaticColors.RedColor;
        }
      }
      else
      {
        this._text.color = StaticColors.OffWhiteColor;
        foreach (Image strikethrough in this._strikethroughs)
          strikethrough.fillAmount = 0.0f;
      }
    }
    this._timeContainer.SetActive(this._objectivesData.HasExpiry && !this.ObjectivesData.IsComplete && !objectivesData.IsFailed);
    this.Localize();
  }

  private void Update()
  {
    if (!this._timeContainer.activeSelf || this._objectivesData == null)
      return;
    this._timeWheel.fillAmount = this._objectivesData.ExpiryTimeNormalized;
  }

  private void UpdateStrikethroughs()
  {
    if (this._deferredStrikethroughUpdate != null)
      this.StopCoroutine(this._deferredStrikethroughUpdate);
    this._deferredStrikethroughUpdate = this.StartCoroutine((IEnumerator) this.DeferredStrikethroughUpdate());
  }

  private IEnumerator DeferredStrikethroughUpdate()
  {
    yield return (object) null;
    bool flag = this._objectivesData.IsFailed || this._objectivesData.IsComplete;
    float num = this._text.rectTransform.rect.height / (float) (this.NumLines * 2);
    for (int index = 0; index < this._strikethroughs.Length; ++index)
    {
      this._strikethroughs[index].gameObject.SetActive(index < this.NumLines);
      if (index < this.NumLines)
      {
        this._strikethroughs[index].rectTransform.anchoredPosition = (Vector2) new Vector3(0.0f, (float) ((double) this._text.rectTransform.rect.height * 0.5 - (double) num - (double) num * 2.0 * (double) index));
        this._strikethroughs[index].fillAmount = flag ? 1f : 0.0f;
      }
      else
        this._strikethroughs[index].fillAmount = 0.0f;
    }
  }

  private void OnObjectiveUpdated(ObjectivesData objectivesData)
  {
    if (objectivesData != this._objectivesData)
      return;
    this.Localize();
  }

  public void CompleteObjective(System.Action andThen = null)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoCompleteObjective(andThen));
  }

  private IEnumerator DoCompleteObjective(System.Action andThen = null)
  {
    UIManager.PlayAudio("event:/ui/objective_complete");
    yield return (object) this._tickBox.YieldForAnimation("turn-on");
    yield return (object) this.DoStrikethrough(StaticColors.OffWhiteColor);
    this._text.color = StaticColors.GreyColor;
    System.Action action = andThen;
    if (action != null)
      action();
  }

  public void FailObjective(System.Action andThen = null)
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoFailedObjective(andThen));
  }

  private IEnumerator DoFailedObjective(System.Action andThen = null)
  {
    UIManager.PlayAudio("event:/ui/objective_failed");
    yield return (object) this._tickBox.YieldForAnimation("turn-on-failed");
    yield return (object) this.DoStrikethrough(StaticColors.RedColor);
    this._text.color = StaticColors.GreyColor;
    System.Action action = andThen;
    if (action != null)
      action();
  }

  private IEnumerator DoStrikethrough(Color color)
  {
    float t = 0.25f / (float) this.NumLines;
    for (int i = 0; i < this.NumLines; ++i)
    {
      this._strikethroughs[i].color = color;
      this._strikethroughs[i].DOFillAmount(1f, t).SetEase<TweenerCore<float, float, FloatOptions>>(i == this.NumLines - 1 ? Ease.OutSine : Ease.Linear);
      yield return (object) new WaitForSecondsRealtime(t);
    }
  }
}
