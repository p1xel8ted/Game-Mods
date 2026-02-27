// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIObjectiveGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UIObjectiveGroup : BaseMonoBehaviour
{
  private const float kUntrackedOnScreenDuration = 5f;
  private const float kShowHideDuration = 0.4f;
  public static Action<string> OnObjectiveGroupBeginHide;
  public static Action<string> OnObjectiveGroupHidden;
  public System.Action OnShown;
  public System.Action OnHidden;
  [Header("Objective Group")]
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _objectiveContent;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [SerializeField]
  private TextMeshProUGUI _header;
  [SerializeField]
  private GameObject _trackingContainer;
  [Header("Templates")]
  [SerializeField]
  private UIObjective _objectiveTemplate;
  private Vector2 _onScreenPosition = new Vector2(220f, 0.0f);
  private Vector2 _offScreenPosition = new Vector2(800f, 0.0f);
  private string _uniqueGroupID;
  private string _groupID;
  private List<UIObjective> _objectiveItems = new List<UIObjective>();
  private List<ObjectivesData> _objectivesData = new List<ObjectivesData>();
  private List<Tweener> _tweeners = new List<Tweener>();

  public RectTransform RectTransform => this._rectTransform;

  public string UniqueGroupID => this._uniqueGroupID;

  public string GroupID => this._groupID;

  public bool AllCompleted => ObjectiveManager.AllObjectivesComplete(this._objectivesData);

  public bool Shown { private set; get; }

  public bool Showing { private set; get; }

  public void Configure(string uniqueGroupID, string groupID)
  {
    this._uniqueGroupID = uniqueGroupID;
    this._groupID = groupID;
    this._canvasGroup.alpha = 0.0f;
    this._onScreenPosition.y = this._offScreenPosition.y = this._rectTransform.anchoredPosition.y;
    this._rectTransform.anchoredPosition = this._offScreenPosition;
    this._trackingContainer.SetActive(false);
    this.Localize();
  }

  private void OnEnable()
  {
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  private void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.Localize);
  }

  private void Localize() => this._header.text = LocalizationManager.GetTranslation(this._groupID);

  private void Update()
  {
    if (ObjectiveManager.IsTracked(this._uniqueGroupID) || !InputManager.Gameplay.GetTrackQuestButtonDown())
      return;
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive");
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    ObjectiveManager.TrackGroup(this._uniqueGroupID);
  }

  public void Show(bool instant = false, System.Action andThen = null)
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
      this.StartCoroutine((IEnumerator) this.DoShow(andThen));
  }

  private IEnumerator DoShow(System.Action andThen = null)
  {
    this.Showing = true;
    yield return (object) null;
    this._trackingContainer.SetActive(!ObjectiveManager.IsTracked(this._uniqueGroupID) && !this.AllCompleted);
    this._tweeners.Add((Tweener) this._rectTransform.DOAnchorPosX(this._onScreenPosition.x, 0.4f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.OutBack));
    this._tweeners.Add((Tweener) this._canvasGroup.DOFade(1f, 0.2f));
    yield return (object) new WaitForSeconds(0.5f);
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
      yield return (object) new WaitForSeconds(5f);
      if (!ObjectiveManager.IsTracked(this._uniqueGroupID))
        yield return (object) this.DoHide();
    }
  }

  public void Hide(bool instant = false, System.Action andThen = null)
  {
    this.StopAllCoroutines();
    if (this.Shown)
    {
      Action<string> objectiveGroupBeginHide = UIObjectiveGroup.OnObjectiveGroupBeginHide;
      if (objectiveGroupBeginHide != null)
        objectiveGroupBeginHide(this._uniqueGroupID);
    }
    this.Shown = false;
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
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  private IEnumerator DoHide(System.Action andThen = null)
  {
    UIObjectiveGroup uiObjectiveGroup = this;
    uiObjectiveGroup.KillTweens();
    uiObjectiveGroup._tweeners.Add((Tweener) uiObjectiveGroup._rectTransform.DOAnchorPosX(uiObjectiveGroup._offScreenPosition.x, 0.4f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack));
    uiObjectiveGroup._tweeners.Add((Tweener) uiObjectiveGroup._canvasGroup.DOFade(0.0f, 0.2f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.2f));
    yield return (object) new WaitForSeconds(0.5f);
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

  public void HideSoft()
  {
    this.KillTweens();
    this._tweeners.Add((Tweener) this._rectTransform.DOAnchorPosX(this._offScreenPosition.x, 0.4f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InBack));
    this._tweeners.Add((Tweener) this._canvasGroup.DOFade(0.0f, 0.2f).SetDelay<TweenerCore<float, float, FloatOptions>>(0.2f));
  }

  public void UpdateTrackingState(bool trackingState)
  {
    this._trackingContainer.SetActive(!trackingState);
    if (this.Shown)
      return;
    this.KillTweens();
    this.StopAllCoroutines();
    this.Show();
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

  private void CheckCompletionStatus()
  {
    if (!this.AllCompleted)
      return;
    this.Hide();
  }

  private void KillTweens()
  {
    foreach (Tween tweener in this._tweeners)
      tweener.Kill();
    this._tweeners.Clear();
  }
}
