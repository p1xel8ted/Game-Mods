// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIObjectiveGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIObjectiveGroup : BaseMonoBehaviour
{
  public const float kUntrackedOnScreenDuration = 5f;
  public const float kShowHideDuration = 0.4f;
  public static Action<string> OnObjectiveGroupBeginHide;
  public static Action<string> OnObjectiveGroupHidden;
  public System.Action OnShown;
  public System.Action OnHide;
  public System.Action OnHidden;
  [Header("Objective Group")]
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _objectiveContent;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [SerializeField]
  public TextMeshProUGUI _header;
  [SerializeField]
  public GameObject _trackingContainer;
  [SerializeField]
  public GameObject _winterObjective;
  [SerializeField]
  public GameObject _timerContainer;
  [SerializeField]
  public Image _timerBar;
  [Header("Templates")]
  [SerializeField]
  public UIObjective _objectiveTemplate;
  public Vector2 _onScreenPosition = new Vector2(220f, 0.0f);
  public Vector2 _offScreenPosition = new Vector2(800f, 0.0f);
  public string _uniqueGroupID;
  public string _groupID;
  public string _subtitle;
  public List<UIObjective> _objectiveItems = new List<UIObjective>();
  public List<ObjectivesData> _objectivesData = new List<ObjectivesData>();
  public List<Tweener> _tweeners = new List<Tweener>();
  [CompilerGenerated]
  public bool \u003CShown\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CShowing\u003Ek__BackingField;

  public RectTransform RectTransform => this._rectTransform;

  public string UniqueGroupID => this._uniqueGroupID;

  public string GroupID => this._groupID;

  public string Subtitle => this._subtitle;

  public bool AllCompleted => ObjectiveManager.AllObjectivesComplete(this._objectivesData);

  public bool Shown
  {
    set => this.\u003CShown\u003Ek__BackingField = value;
    get => this.\u003CShown\u003Ek__BackingField;
  }

  public bool Showing
  {
    set => this.\u003CShowing\u003Ek__BackingField = value;
    get => this.\u003CShowing\u003Ek__BackingField;
  }

  public void Configure(
    string uniqueGroupID,
    string groupID,
    bool isWinterObjective = false,
    bool showTimer = false,
    string subtitle = null)
  {
    this._uniqueGroupID = uniqueGroupID;
    this._groupID = groupID;
    this._subtitle = subtitle;
    this._winterObjective.gameObject.SetActive(isWinterObjective);
    this._timerContainer.gameObject.SetActive(showTimer);
    this._canvasGroup.alpha = 0.0f;
    this._onScreenPosition.y = this._offScreenPosition.y = this._rectTransform.anchoredPosition.y;
    this._rectTransform.anchoredPosition = this._offScreenPosition;
    this._trackingContainer.SetActive(false);
    this.Localize();
    if (!showTimer)
      return;
    this.transform.SetAsFirstSibling();
  }

  public void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  public void Localize()
  {
    string str = LocalizationManager.GetTranslation(this._groupID);
    if (this._subtitle != null)
      str = $"{str}\n<size=16><color=#F5EDD54D>{this._subtitle.Localized()}";
    this._header.text = str;
  }

  public void Update()
  {
    if (!ObjectiveManager.IsTracked(this._uniqueGroupID) && InputManager.Gameplay.GetTrackQuestButtonDown())
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
      ObjectiveManager.TrackGroup(this._uniqueGroupID);
    }
    if (!this._timerContainer.gameObject.activeSelf || this._objectivesData.Count <= 0)
      return;
    foreach (ObjectivesData objectivesData in this._objectivesData)
    {
      if ((double) objectivesData.ExpireTimestamp != -1.0)
        this._timerBar.fillAmount = objectivesData.ExpiryTimeNormalized;
    }
  }

  public void Show(bool instant = false, bool useTimeScale = true, System.Action andThen = null)
  {
    this.KillTweens();
    this.StopAllCoroutines();
    if (instant)
    {
      this._rectTransform.anchoredPosition = this._onScreenPosition;
      this._canvasGroup.alpha = 1f;
      System.Action onShown = this.OnShown;
      if (onShown != null)
        onShown();
      this.Shown = true;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow(useTimeScale, andThen));
  }

  public IEnumerator DoShow(bool useTimeScale, System.Action andThen = null)
  {
    this.Showing = true;
    yield return (object) null;
    this._trackingContainer.SetActive(!ObjectiveManager.IsTracked(this._uniqueGroupID) && !this.AllCompleted);
    this._tweeners.Add((Tweener) this._rectTransform.DOAnchorPosX(this._onScreenPosition.x, 0.4f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(!useTimeScale));
    this._tweeners.Add((Tweener) this._canvasGroup.DOFade(1f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale));
    if (useTimeScale)
      yield return (object) new WaitForSeconds(0.5f);
    else
      yield return (object) new WaitForSecondsRealtime(0.5f);
    System.Action action = andThen;
    if (action != null)
      action();
    System.Action onShown = this.OnShown;
    if (onShown != null)
      onShown();
    this.Shown = true;
    this.Showing = false;
    if (!ObjectiveManager.IsTracked(this._uniqueGroupID))
    {
      if (useTimeScale)
        yield return (object) new WaitForSeconds(5f);
      else
        yield return (object) new WaitForSecondsRealtime(5f);
      if (!ObjectiveManager.IsTracked(this._uniqueGroupID))
      {
        System.Action onHide = this.OnHide;
        if (onHide != null)
          onHide();
        yield return (object) this.DoHide(useTimeScale);
      }
    }
  }

  public void Hide(bool instant = false, bool useTimeScale = true, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    if (this.Shown)
    {
      Action<string> objectiveGroupBeginHide = UIObjectiveGroup.OnObjectiveGroupBeginHide;
      if (objectiveGroupBeginHide != null)
        objectiveGroupBeginHide(this._uniqueGroupID);
    }
    this.Shown = false;
    System.Action onHide = this.OnHide;
    if (onHide != null)
      onHide();
    if (instant)
    {
      this._rectTransform.anchoredPosition = this._offScreenPosition;
      this._canvasGroup.alpha = 0.0f;
      System.Action onHidden = this.OnHidden;
      if (onHidden != null)
        onHidden();
      Action<string> objectiveGroupHidden = UIObjectiveGroup.OnObjectiveGroupHidden;
      if (objectiveGroupHidden == null)
        return;
      objectiveGroupHidden(this._uniqueGroupID);
    }
    else
      this.StartCoroutine((IEnumerator) this.DoHide(useTimeScale));
  }

  public IEnumerator DoHide(bool useTimeScale, System.Action andThen = null)
  {
    UIObjectiveGroup uiObjectiveGroup = this;
    uiObjectiveGroup.KillTweens();
    uiObjectiveGroup._tweeners.Add((Tweener) uiObjectiveGroup._rectTransform.DOAnchorPosX(uiObjectiveGroup._offScreenPosition.x, 0.4f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(!useTimeScale));
    uiObjectiveGroup._tweeners.Add((Tweener) uiObjectiveGroup._canvasGroup.DOFade(0.0f, 0.2f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale));
    if (useTimeScale)
      yield return (object) new WaitForSeconds(0.5f);
    else
      yield return (object) new WaitForSecondsRealtime(0.5f);
    System.Action action = andThen;
    if (action != null)
      action();
    System.Action onHidden = uiObjectiveGroup.OnHidden;
    if (onHidden != null)
      onHidden();
    Action<string> objectiveGroupHidden = UIObjectiveGroup.OnObjectiveGroupHidden;
    if (objectiveGroupHidden != null)
      objectiveGroupHidden(uiObjectiveGroup._uniqueGroupID);
    UnityEngine.Object.Destroy((UnityEngine.Object) uiObjectiveGroup.gameObject);
  }

  public void HideSoft(bool useTimeScale = true)
  {
    this.KillTweens();
    this._tweeners.Add((Tweener) this._rectTransform.DOAnchorPosX(this._offScreenPosition.x, 0.4f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(!useTimeScale));
    this._tweeners.Add((Tweener) this._canvasGroup.DOFade(0.0f, 0.2f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(!useTimeScale));
  }

  public void UpdateTrackingState(bool trackingState, bool useTimeScale = true)
  {
    this._trackingContainer.SetActive(!trackingState);
    if (this.Shown)
      return;
    this.KillTweens();
    this.StopAllCoroutines();
    this.Show(useTimeScale: useTimeScale);
  }

  public void AddObjective(ObjectivesData objectivesData, bool ignoreStatus = false)
  {
    if (this._objectivesData.Contains(objectivesData))
      return;
    UIObjective uiObjective = this._objectiveTemplate.Instantiate<UIObjective>((Transform) this._objectiveContent);
    uiObjective.Configure(objectivesData, ignoreStatus);
    this._objectiveItems.Add(uiObjective);
    this._objectivesData.Add(objectivesData);
    LayoutRebuilder.ForceRebuildLayoutImmediate(this._objectiveContent);
  }

  public void CompleteObjective(ObjectivesData objectivesData)
  {
    foreach (UIObjective objectiveItem in this._objectiveItems)
    {
      if (objectivesData == objectiveItem.ObjectivesData)
      {
        objectiveItem.CompleteObjective(new System.Action(this.CheckCompletionStatus));
        break;
      }
    }
  }

  public void FailObjective(ObjectivesData objectivesData)
  {
    foreach (UIObjective objectiveItem in this._objectiveItems)
    {
      if (objectivesData == objectiveItem.ObjectivesData)
      {
        objectiveItem.FailObjective(new System.Action(this.CheckCompletionStatus));
        break;
      }
    }
  }

  public bool HasObjective(ObjectivesData objectivesData)
  {
    if (this._objectivesData.Contains(objectivesData))
      return true;
    foreach (ObjectivesData objectivesData1 in this._objectivesData)
    {
      if (objectivesData1.ID == objectivesData.ID)
        return true;
    }
    return false;
  }

  public void CheckCompletionStatus()
  {
    if (!this.AllCompleted)
      return;
    this.Hide();
  }

  public void KillTweens()
  {
    foreach (Tweener tweener in this._tweeners)
    {
      if (tweener != null && tweener.active)
        tweener.Kill();
    }
    this._tweeners.Clear();
  }
}
