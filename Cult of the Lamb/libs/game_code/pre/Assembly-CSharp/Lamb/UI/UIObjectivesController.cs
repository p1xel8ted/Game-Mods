// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIObjectivesController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIObjectivesController : BaseMonoBehaviour
{
  private const float kShowDuration = 0.5f;
  private const float kHideDuration = 0.5f;
  [Header("Objectives Controller")]
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _contentContainer;
  [SerializeField]
  private CanvasGroup _canvasGroup;
  [Header("Templates")]
  [SerializeField]
  private UIObjectiveGroup _objectiveGroupTemplate;
  private Dictionary<string, UIObjectiveGroup> _objectiveGroups = new Dictionary<string, UIObjectiveGroup>();
  private Vector2 _onScreenPosition;
  private Vector2 _offScreenPosition;
  private bool _initialized;

  public bool Hidden { private set; get; }

  private void Awake()
  {
    this._onScreenPosition = this._offScreenPosition = this._rectTransform.anchoredPosition;
    this._offScreenPosition.x = this._rectTransform.sizeDelta.x + 50f;
  }

  private void Start() => this.OnLoadComplete();

  private void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnLoadComplete);
    ObjectiveManager.OnObjectiveAdded += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveAdded);
    ObjectiveManager.OnObjectiveCompleted += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveCompleted);
    ObjectiveManager.OnObjectiveRemoved += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveRemoved);
    ObjectiveManager.OnObjectiveFailed += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveFailed);
    ObjectiveManager.OnObjectiveTracked += new Action<string>(this.OnObjectiveTracked);
    ObjectiveManager.OnObjectiveUntracked += new Action<string>(this.OnObjectiveUntracked);
  }

  private void OnDisable()
  {
    SaveAndLoad.OnLoadComplete -= new System.Action(this.OnLoadComplete);
    ObjectiveManager.OnObjectiveAdded -= new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveAdded);
    ObjectiveManager.OnObjectiveCompleted -= new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveCompleted);
    ObjectiveManager.OnObjectiveRemoved -= new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveRemoved);
    ObjectiveManager.OnObjectiveFailed -= new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveFailed);
    ObjectiveManager.OnObjectiveTracked -= new Action<string>(this.OnObjectiveTracked);
    ObjectiveManager.OnObjectiveUntracked -= new Action<string>(this.OnObjectiveUntracked);
  }

  public void Show(bool instant = false)
  {
    this._rectTransform.DOKill();
    this.StopAllCoroutines();
    if (instant)
    {
      this.Hidden = false;
      this._rectTransform.anchoredPosition = this._onScreenPosition;
    }
    else
      this.StartCoroutine((IEnumerator) this.DoShow());
  }

  private IEnumerator DoShow()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._onScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.Hidden = false;
  }

  public void Hide(bool instant = false)
  {
    Debug.Log((object) "UIObjectivesController - Hide".Colour(Color.yellow));
    this.Hidden = true;
    this._rectTransform.DOKill();
    this.StopAllCoroutines();
    if (instant)
      this._rectTransform.anchoredPosition = this._offScreenPosition;
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  private IEnumerator DoHide()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._offScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void HideAllExcluding(string groupID)
  {
    foreach (UIObjectiveGroup uiObjectiveGroup in this._objectiveGroups.Values)
    {
      if (uiObjectiveGroup.GroupID != groupID)
        uiObjectiveGroup.HideSoft();
    }
  }

  public void ShowAll()
  {
    foreach (KeyValuePair<string, UIObjectiveGroup> objectiveGroup in this._objectiveGroups)
      objectiveGroup.Value.Show();
  }

  private void OnLoadComplete()
  {
    if (this._initialized || !SaveAndLoad.Loaded)
      return;
    Debug.Log((object) "UIObjectivesController - Load Complete".Colour(Color.yellow));
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (ObjectiveManager.IsTracked(objective.UniqueGroupID))
        this.AddObjectiveInstance(objective);
      objective.Init(false);
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (ObjectiveManager.IsTracked(completedObjective.UniqueGroupID))
        this.AddObjectiveInstance(completedObjective);
    }
    foreach (ObjectivesData failedObjective in DataManager.Instance.FailedObjectives)
    {
      if (ObjectiveManager.IsTracked(failedObjective.UniqueGroupID))
        this.AddObjectiveInstance(failedObjective);
    }
    this._initialized = true;
  }

  private void OnObjectiveTracked(string uniqueGroupID)
  {
    UIObjectiveGroup uiObjectiveGroup;
    if (this._objectiveGroups.TryGetValue(uniqueGroupID, out uiObjectiveGroup))
    {
      uiObjectiveGroup.UpdateTrackingState(true);
    }
    else
    {
      foreach (ObjectivesData objectivesData in ObjectiveManager.GetAllObjectivesOfGroup(uniqueGroupID))
        this.AddObjectiveInstance(objectivesData);
    }
  }

  private void OnObjectiveUntracked(string uniqueGroupID)
  {
    UIObjectiveGroup uiObjectiveGroup;
    if (!this._objectiveGroups.TryGetValue(uniqueGroupID, out uiObjectiveGroup))
      return;
    uiObjectiveGroup.Hide();
    this._objectiveGroups.Remove(uniqueGroupID);
  }

  private void OnObjectiveAdded(ObjectivesData objective)
  {
    if (objective.IsComplete)
      return;
    this.AddObjectiveInstance(objective);
  }

  private void OnObjectiveCompleted(ObjectivesData objective)
  {
    UIObjectiveGroup objectiveGroup;
    if (this._objectiveGroups.TryGetValue(objective.UniqueGroupID, out objectiveGroup))
    {
      this.AddObjectiveInstance(objective, !objectiveGroup.Shown);
    }
    else
    {
      objectiveGroup = this.AddObjectiveInstance(objective, true);
      foreach (ObjectivesData objectivesData in ObjectiveManager.GetAllObjectivesOfGroup(objective.UniqueGroupID))
        objectiveGroup.AddObjective(objectivesData);
    }
    if (!objectiveGroup.Shown)
    {
      if (!objectiveGroup.Showing)
        objectiveGroup.Show();
      objectiveGroup.OnShown += (System.Action) (() => objectiveGroup.CompleteObjective(objective));
    }
    else
      objectiveGroup.CompleteObjective(objective);
  }

  private void OnObjectiveFailed(ObjectivesData objective)
  {
    UIObjectiveGroup objectiveGroup;
    if (this._objectiveGroups.TryGetValue(objective.UniqueGroupID, out objectiveGroup))
    {
      this.AddObjectiveInstance(objective, !objectiveGroup.Shown);
    }
    else
    {
      objectiveGroup = this.AddObjectiveInstance(objective, true);
      foreach (ObjectivesData objectivesData in ObjectiveManager.GetAllObjectivesOfGroup(objective.UniqueGroupID))
        objectiveGroup.AddObjective(objectivesData);
    }
    if (!objectiveGroup.Shown)
    {
      if (!objectiveGroup.Showing)
        objectiveGroup.Show();
      objectiveGroup.OnShown += (System.Action) (() => objectiveGroup.FailObjective(objective));
    }
    else
      objectiveGroup.FailObjective(objective);
  }

  private void OnObjectiveRemoved(ObjectivesData objective)
  {
  }

  private UIObjectiveGroup AddObjectiveInstance(
    ObjectivesData objectivesData,
    bool ignoreStatus = false,
    bool instant = false)
  {
    UIObjectiveGroup objectiveGroup;
    if (this._objectiveGroups.TryGetValue(objectivesData.UniqueGroupID, out objectiveGroup))
    {
      if (!objectiveGroup.HasObjective(objectivesData))
        objectiveGroup.AddObjective(objectivesData, ignoreStatus);
      return objectiveGroup;
    }
    objectiveGroup = this._objectiveGroupTemplate.Instantiate<UIObjectiveGroup>((Transform) this._contentContainer);
    objectiveGroup.transform.SetSiblingIndex(10);
    objectiveGroup.Configure(objectivesData.UniqueGroupID, objectivesData.GroupId);
    objectiveGroup.AddObjective(objectivesData, ignoreStatus);
    objectiveGroup.Show(instant);
    objectiveGroup.OnHidden += (System.Action) (() => this._objectiveGroups.Remove(objectiveGroup.UniqueGroupID));
    this._objectiveGroups.Add(objectivesData.UniqueGroupID, objectiveGroup);
    return objectiveGroup;
  }
}
