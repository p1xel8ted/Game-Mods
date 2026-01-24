// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIObjectivesController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIObjectivesController : BaseMonoBehaviour
{
  public const float kShowDuration = 0.5f;
  public const float kHideDuration = 0.5f;
  [Header("Objectives Controller")]
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _contentContainer;
  [SerializeField]
  public CanvasGroup _canvasGroup;
  [Header("Templates")]
  [SerializeField]
  public UIObjectiveGroup _objectiveGroupTemplate;
  [Header("Additional Settings")]
  [SerializeField]
  public bool useTimeScale = true;
  public Dictionary<string, UIObjectiveGroup> _objectiveGroups = new Dictionary<string, UIObjectiveGroup>();
  [CompilerGenerated]
  public bool \u003CHidden\u003Ek__BackingField;
  public Vector2 _onScreenPosition;
  public Vector2 _offScreenPosition;
  public bool _initialized;

  public bool Hidden
  {
    set => this.\u003CHidden\u003Ek__BackingField = value;
    get => this.\u003CHidden\u003Ek__BackingField;
  }

  public void Awake()
  {
    this._onScreenPosition = this._offScreenPosition = this._rectTransform.anchoredPosition;
    this._offScreenPosition.x = this._rectTransform.sizeDelta.x + 50f;
  }

  public void Start() => this.OnLoadComplete();

  public void OnEnable()
  {
    SaveAndLoad.OnLoadComplete += new System.Action(this.OnLoadComplete);
    ObjectiveManager.OnObjectiveAdded += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveAdded);
    ObjectiveManager.OnObjectiveCompleted += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveCompleted);
    ObjectiveManager.OnObjectiveRemoved += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveRemoved);
    ObjectiveManager.OnObjectiveFailed += new ObjectiveManager.ObjectiveUpdated(this.OnObjectiveFailed);
    ObjectiveManager.OnObjectiveTracked += new Action<string>(this.OnObjectiveTracked);
    ObjectiveManager.OnObjectiveUntracked += new Action<string>(this.OnObjectiveUntracked);
  }

  public void OnDisable()
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

  public IEnumerator DoShow()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._onScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.Hidden = false;
  }

  public void Hide(bool instant = false)
  {
    this.Hidden = true;
    this._rectTransform.DOKill();
    this.StopAllCoroutines();
    if (instant)
      this._rectTransform.anchoredPosition = this._offScreenPosition;
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  public IEnumerator DoHide()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._offScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void HideAllExcluding(string groupID, bool soft = true)
  {
    for (int index = this._objectiveGroups.Count - 1; index >= 0; --index)
    {
      KeyValuePair<string, UIObjectiveGroup> keyValuePair = this._objectiveGroups.ElementAt<KeyValuePair<string, UIObjectiveGroup>>(index);
      if (keyValuePair.Value.GroupID != groupID)
      {
        if (soft)
        {
          keyValuePair = this._objectiveGroups.ElementAt<KeyValuePair<string, UIObjectiveGroup>>(index);
          keyValuePair.Value.HideSoft(this.useTimeScale);
        }
        else
        {
          keyValuePair = this._objectiveGroups.ElementAt<KeyValuePair<string, UIObjectiveGroup>>(index);
          keyValuePair.Value.Hide(useTimeScale: this.useTimeScale);
        }
      }
    }
  }

  public int GetCount()
  {
    int count = 0;
    for (int index = this._objectiveGroups.Count - 1; index >= 0; --index)
      ++count;
    return count;
  }

  public int GetCountExcluding(string groupID)
  {
    int countExcluding = 0;
    for (int index = this._objectiveGroups.Count - 1; index >= 0; --index)
    {
      if (this._objectiveGroups.ElementAt<KeyValuePair<string, UIObjectiveGroup>>(index).Value.GroupID != groupID)
        ++countExcluding;
    }
    return countExcluding;
  }

  public void ShowAll()
  {
    foreach (KeyValuePair<string, UIObjectiveGroup> objectiveGroup in this._objectiveGroups)
      objectiveGroup.Value.Show(useTimeScale: this.useTimeScale);
  }

  public void OnLoadComplete()
  {
    if (this._initialized || !SaveAndLoad.Loaded)
      return;
    Debug.Log((object) "UIObjectivesController - Load Complete".Colour(Color.yellow));
    List<ObjectivesData> objectivesDataList = new List<ObjectivesData>();
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (ObjectiveManager.IsTracked(objective.UniqueGroupID))
        objectivesDataList.Add(objective);
      objective.Init(false);
    }
    foreach (ObjectivesData completedObjective in DataManager.Instance.CompletedObjectives)
    {
      if (ObjectiveManager.IsTracked(completedObjective.UniqueGroupID))
        objectivesDataList.Add(completedObjective);
    }
    foreach (ObjectivesData failedObjective in DataManager.Instance.FailedObjectives)
    {
      if (ObjectiveManager.IsTracked(failedObjective.UniqueGroupID))
        objectivesDataList.Add(failedObjective);
    }
    for (int index1 = 0; index1 < DataManager.Instance.TrackedObjectiveGroupIDs.Count; ++index1)
    {
      for (int index2 = 0; index2 < objectivesDataList.Count; ++index2)
      {
        if (DataManager.Instance.TrackedObjectiveGroupIDs[index1] == objectivesDataList[index2].UniqueGroupID)
          this.AddObjectiveInstance(objectivesDataList[index2]);
      }
    }
    this._initialized = true;
  }

  public void OnObjectiveTracked(string uniqueGroupID)
  {
    UIObjectiveGroup uiObjectiveGroup;
    if (this._objectiveGroups.TryGetValue(uniqueGroupID, out uiObjectiveGroup))
    {
      uiObjectiveGroup.UpdateTrackingState(true, this.useTimeScale);
    }
    else
    {
      foreach (ObjectivesData objectivesData in ObjectiveManager.GetAllObjectivesOfGroup(uniqueGroupID))
        this.AddObjectiveInstance(objectivesData);
    }
  }

  public void OnObjectiveUntracked(string uniqueGroupID)
  {
    UIObjectiveGroup uiObjectiveGroup;
    if (!this._objectiveGroups.TryGetValue(uniqueGroupID, out uiObjectiveGroup))
      return;
    uiObjectiveGroup.Hide(useTimeScale: this.useTimeScale);
    this._objectiveGroups.Remove(uniqueGroupID);
  }

  public void OnObjectiveAdded(ObjectivesData objective)
  {
    if (objective.IsComplete)
      return;
    this.AddObjectiveInstance(objective);
  }

  public void OnObjectiveCompleted(ObjectivesData objective)
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
        objectiveGroup.Show(useTimeScale: this.useTimeScale);
      System.Action onShownObjective = (System.Action) (() => objectiveGroup.CompleteObjective(objective));
      onShownObjective += (System.Action) (() => objectiveGroup.OnShown -= onShownObjective);
      objectiveGroup.OnShown += onShownObjective;
    }
    else
      objectiveGroup.CompleteObjective(objective);
  }

  public void OnObjectiveFailed(ObjectivesData objective)
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
        objectiveGroup.Show(useTimeScale: this.useTimeScale);
      System.Action onShownObjective = (System.Action) (() => objectiveGroup.FailObjective(objective));
      onShownObjective += (System.Action) (() => objectiveGroup.OnShown -= onShownObjective);
      objectiveGroup.OnShown += onShownObjective;
    }
    else
      objectiveGroup.FailObjective(objective);
  }

  public void OnObjectiveRemoved(ObjectivesData objective)
  {
  }

  public UIObjectiveGroup AddObjectiveInstance(
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
    objectiveGroup.Configure(objectivesData.UniqueGroupID, objectivesData.GroupId, objectivesData.IsWinterObjective, objectivesData.TimerType == Objectives.TIMER_TYPE.Large, objectivesData.subtitle);
    objectiveGroup.AddObjective(objectivesData, ignoreStatus);
    objectiveGroup.Show(instant, this.useTimeScale);
    objectiveGroup.OnHide += (System.Action) (() => this._objectiveGroups.Remove(objectiveGroup.UniqueGroupID));
    this._objectiveGroups.Add(objectivesData.UniqueGroupID, objectiveGroup);
    return objectiveGroup;
  }
}
