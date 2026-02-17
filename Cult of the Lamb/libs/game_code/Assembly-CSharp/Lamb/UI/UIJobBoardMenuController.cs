// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIJobBoardMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI;

public class UIJobBoardMenuController : UIMenuBase
{
  [SerializeField]
  public JobBoardMenuItem jobBoardMenuItem;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public UIObjectivesController objectiveController;
  [SerializeField]
  public CanvasGroup noJobs;
  [SerializeField]
  public JobBoardInfoCardController infoCardController;
  [SerializeField]
  public TextMeshProUGUI TitleText;
  [Header("Content Containers")]
  [SerializeField]
  public GameObject contentText;
  [SerializeField]
  public RectTransform content;
  [SerializeField]
  public GameObject contentTextUnavailable;
  [SerializeField]
  public RectTransform contentUnavailable;
  [SerializeField]
  public GameObject contentTextComplete;
  [SerializeField]
  public RectTransform contentComplete;
  [SerializeField]
  public RectTransform contentCompleteAlreadyClaimed;
  [SerializeField]
  public GameObject contentCompleteAlreadyClaimedText;
  public bool didCancel;
  public bool inSequence;
  public List<JobBoardMenuItem> jobItems = new List<JobBoardMenuItem>();
  public List<JobBoardMenuItem> completeJobItems = new List<JobBoardMenuItem>();
  [CompilerGenerated]
  public Interaction_JobBoard \u003CJobBoard\u003Ek__BackingField;
  public System.Action OnAcceptJobQuest;
  public System.Action OnRemoveJobQuest;
  public Action<ObjectivesData> OnItemCompleted;
  public Action<JobBoardMenuItem> OnJobInfoCardClick;

  public Interaction_JobBoard JobBoard
  {
    get => this.\u003CJobBoard\u003Ek__BackingField;
    set => this.\u003CJobBoard\u003Ek__BackingField = value;
  }

  public JobBoardInfoCardController InfoCardController => this.infoCardController;

  public void Show(Interaction_JobBoard jobBoard, bool instant = false)
  {
    this.JobBoard = jobBoard;
    for (int index = 0; index < jobBoard.Objectives.Count; ++index)
    {
      bool flag = index == 0 || DataManager.Instance.JobBoardsClaimedQuests.Contains((int) (this.JobBoard.Host + index - 1));
      if (!DataManager.Instance.JobBoardsClaimedQuests.Contains((int) (this.JobBoard.Host + index)))
      {
        JobBoardMenuItem jobBoardMenuItem = UnityEngine.Object.Instantiate<JobBoardMenuItem>(this.jobBoardMenuItem, this.content.transform);
        bool active = true;
        if (!flag && this.JobBoard.RequiresPreviousObjectiveCompleted)
          active = false;
        else if (jobBoard.Objectives[index].RequiresCondition && !DataManager.Instance.GetVariable(jobBoard.Objectives[index].ConditionalVariable))
          active = false;
        jobBoardMenuItem.Configure(jobBoard.Objectives[index], this, active, index);
        jobBoardMenuItem.OnHighlighted += new Action<JobBoardMenuItem>(this.OnMenuItemHightlighted);
        jobBoardMenuItem.OnSelected += new Action<JobBoardMenuItem>(this.OnMenuItemSelected);
        this.jobItems.Add(jobBoardMenuItem);
      }
      else
      {
        JobBoardMenuItem jobBoardMenuItem = UnityEngine.Object.Instantiate<JobBoardMenuItem>(this.jobBoardMenuItem, this.contentCompleteAlreadyClaimed.transform);
        jobBoardMenuItem.OnHighlighted += new Action<JobBoardMenuItem>(this.OnMenuItemHightlighted);
        jobBoardMenuItem.CompletedClaimedReward = true;
        jobBoardMenuItem.Completed = true;
        jobBoardMenuItem.Configure(jobBoard.Objectives[index], this, true, index);
        this.completeJobItems.Add(jobBoardMenuItem);
      }
    }
    this.Show();
    foreach (JobBoardMenuItem jobItem in this.jobItems)
    {
      if (jobItem.Completed && !jobItem.CompletedClaimedReward)
        jobItem.SetNotCompleted();
    }
  }

  public override void OnShowFinished()
  {
    base.OnShowFinished();
    if (!this.jobItems.Any<JobBoardMenuItem>((Func<JobBoardMenuItem, bool>) (j => j.Completed && !j.CompletedClaimedReward)))
      return;
    Debug.Log((object) "We have a completed task that we havent claimed the reward for");
    this.StartCoroutine((IEnumerator) this.ShowCompletedTask());
  }

  public IEnumerator ShowCompletedTask()
  {
    UIJobBoardMenuController boardMenuController = this;
    boardMenuController.inSequence = true;
    List<JobBoardMenuItem> completedTasksToHighlight = new List<JobBoardMenuItem>();
    foreach (JobBoardMenuItem jobItem in boardMenuController.jobItems)
    {
      if (jobItem.Completed && !jobItem.CompletedClaimedReward)
        completedTasksToHighlight.Add(jobItem);
    }
    boardMenuController._scrollRect.vertical = false;
    boardMenuController.infoCardController.enabled = false;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    boardMenuController.SetActiveStateForMenu(false);
    yield return (object) null;
    JobBoardMenuItem jobBoardMenuItem = (JobBoardMenuItem) null;
    foreach (JobBoardMenuItem menuItem in completedTasksToHighlight)
    {
      AudioManager.Instance.PlayOneShot("event:/dlc/env/woolhaven/jobboard_quest_complete");
      AudioManager.Instance.PlayOneShot("event:/dlc/music/woolhaven/jobboard_quest_complete");
      RectTransform rect = menuItem.GetComponent<RectTransform>();
      yield return (object) boardMenuController._scrollRect.DoScrollTo(rect);
      menuItem.StartCoroutine((IEnumerator) menuItem.UnlockSequenceIE());
      rect.GetChild(0).DOPunchPosition(new Vector3(0.0f, 10f), 0.25f).SetUpdate<Tweener>(true);
      yield return (object) new WaitForSecondsRealtime(0.5f);
      Debug.Log((object) "Polish button");
      boardMenuController.completeJobItems.Add(menuItem);
      jobBoardMenuItem = menuItem;
      rect = (RectTransform) null;
    }
    boardMenuController._scrollRect.vertical = true;
    boardMenuController.infoCardController.enabled = true;
    boardMenuController.SetActiveStateForMenu(true);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) jobBoardMenuItem.MMButton);
    boardMenuController.inSequence = false;
  }

  public void SortJobItems()
  {
    foreach (JobBoardMenuItem jobItem in this.jobItems)
    {
      if (jobItem.Completed && !jobItem.CompletedClaimedReward)
        jobItem.transform.parent = (Transform) this.contentComplete;
      else if (!jobItem.IsDisabled)
        jobItem.transform.parent = (Transform) this.content;
      else if (jobItem.CompletedClaimedReward)
        jobItem.transform.parent = (Transform) this.contentCompleteAlreadyClaimed;
      else
        jobItem.transform.parent = (Transform) this.contentUnavailable;
    }
    if (this.contentComplete.childCount == 0)
      this.contentTextComplete.SetActive(false);
    if (this.contentUnavailable.childCount == 0)
      this.contentTextUnavailable.SetActive(false);
    if (this.content.childCount == 0)
      this.contentText.SetActive(false);
    if (this.contentCompleteAlreadyClaimed.childCount != 0)
      return;
    this.contentCompleteAlreadyClaimedText.SetActive(false);
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    JobBoardMenuItem safeItemSelection = this.jobItems.Count <= 0 ? this.completeJobItems[0] : this.jobItems[0];
    this.TitleText.text = LocalizationManager.GetTranslation(safeItemSelection.Objective.GroupId);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) safeItemSelection.MMButton);
    GameManager.GetInstance().WaitForSecondsRealtime(0.0f, (System.Action) (() =>
    {
      this.objectiveController.HideAllExcluding(safeItemSelection.Objective.GroupId, false);
      this.noJobs.alpha = 0.0f;
      if (this.objectiveController.GetCount() - this.objectiveController.GetCountExcluding(safeItemSelection.Objective.GroupId) <= 0)
        this.noJobs.DOFade(1f, 0.5f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
      if (this.objectiveController.GetCount() - this.objectiveController.GetCountExcluding(safeItemSelection.Objective.GroupId) > 0)
        return;
      foreach (JobBoardMenuItem jobItem in this.jobItems)
      {
        ObjectivesData objective = (ObjectivesData) null;
        if (this.TrackingObjective(jobItem, out objective, true, false))
        {
          DataManager.Instance.Objectives.Remove(objective);
          this.OnMenuItemSelected(jobItem);
        }
      }
    }));
    JobBoardMenuItem config = this.jobItems.FirstOrDefault<JobBoardMenuItem>((Func<JobBoardMenuItem, bool>) (x => x.Completed));
    if ((UnityEngine.Object) config != (UnityEngine.Object) null)
    {
      foreach (JobBoardMenuItem jobItem in this.jobItems)
      {
        if ((UnityEngine.Object) jobItem != (UnityEngine.Object) config)
          jobItem.SetDisabled();
      }
      foreach (JobBoardMenuItem completeJobItem in this.completeJobItems)
      {
        if ((UnityEngine.Object) completeJobItem != (UnityEngine.Object) config)
          completeJobItem.SetDisabled();
      }
      MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) config.MMButton);
      this.infoCardController.Card1?.Show(config, true);
    }
    this.infoCardController.Card1.OnTrackButtonClicked += new UnityAction<JobBoardMenuItem>(this.OnInfoCardClick);
    this.infoCardController.Card2.OnTrackButtonClicked += new UnityAction<JobBoardMenuItem>(this.OnInfoCardClick);
    this.SortJobItems();
  }

  public void OnInfoCardClick(JobBoardMenuItem item)
  {
    MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.TryPerformConfirmAction();
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if (!((UnityEngine.Object) this.jobItems.FirstOrDefault<JobBoardMenuItem>((Func<JobBoardMenuItem, bool>) (x => x.Completed)) == (UnityEngine.Object) null) || this.jobItems.Count == 0 && this.completeJobItems.Count == 0)
      return;
    JobBoardMenuItem config = this.jobItems.Count <= 0 ? this.completeJobItems[0] : this.jobItems[0];
    this.TitleText.text = LocalizationManager.GetTranslation(config.JobData.GroupTitle);
    this.infoCardController.Card1.Show(config, true);
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    this.infoCardController.Card1.OnTrackButtonClicked -= new UnityAction<JobBoardMenuItem>(this.OnMenuItemSelected);
    this.infoCardController.Card2.OnTrackButtonClicked -= new UnityAction<JobBoardMenuItem>(this.OnMenuItemSelected);
  }

  public void OnMenuItemSelected(JobBoardMenuItem item)
  {
    if (item.IsDisabled)
    {
      this.infoCardController.DOComplete();
      this.infoCardController.transform.DOPunchPosition(new Vector3(20f, 0.0f), 0.25f).SetUpdate<Tweener>(true);
      UIManager.PlayAudio("event:/ui/negative_feedback");
      MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
    }
    else
    {
      Debug.Log((object) $"OnMenuItemSelected: {item.Objective.GroupId} - {item.Objective.Type}");
      if (item.Completed)
      {
        DataManager.Instance.JobBoardsClaimedQuests.Add((int) (this.JobBoard.Host + this.JobBoard.Objectives.IndexOf(item.JobData)));
        Action<ObjectivesData> onItemCompleted = this.OnItemCompleted;
        if (onItemCompleted != null)
          onItemCompleted(item.Objective);
        System.Action onRemoveJobQuest = this.OnRemoveJobQuest;
        if (onRemoveJobQuest != null)
          onRemoveJobQuest();
        this.Hide();
      }
      else
      {
        ObjectiveManager.OverrideQueuing = true;
        ObjectivesData objective = (ObjectivesData) null;
        bool flag = this.TrackingObjective(item, out objective, true, true);
        List<ObjectivesData> objectivesOfGroupId = ObjectiveManager.GetAllObjectivesOfGroupID(item.Objective.GroupId);
        if (objectivesOfGroupId.Count > 0)
        {
          for (int index = 0; index < objectivesOfGroupId.Count; ++index)
          {
            ObjectiveManager.UntrackGroup(objectivesOfGroupId[index].UniqueGroupID, true);
            DataManager.Instance.Objectives.Remove(objectivesOfGroupId[index]);
          }
        }
        if (!flag)
        {
          ObjectiveManager.Add(item.Objective, true, true);
          System.Action onAcceptJobQuest = this.OnAcceptJobQuest;
          if (onAcceptJobQuest != null)
            onAcceptJobQuest();
          this.noJobs.DOKill();
          this.noJobs.DOFade(0.0f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
          UIManager.PlayAudio("event:/ui/objective_text_appear");
        }
        else
        {
          System.Action onRemoveJobQuest = this.OnRemoveJobQuest;
          if (onRemoveJobQuest != null)
            onRemoveJobQuest();
          this.noJobs.DOKill();
          this.noJobs.DOFade(1f, 0.33f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true).SetDelay<TweenerCore<float, float, FloatOptions>>(0.4f);
        }
        foreach (JobBoardMenuItem jobItem in this.jobItems)
          jobItem.UpdateTracked();
        ObjectiveManager.OverrideQueuing = false;
      }
    }
  }

  public bool TrackingObjective(
    JobBoardMenuItem item,
    out ObjectivesData objective,
    bool includeActive,
    bool includeCompleted)
  {
    objective = (ObjectivesData) null;
    List<ObjectivesData> customObjectives = ObjectiveManager.GetCustomObjectives(item.Objective.Type, includeActive, includeCompleted);
    if (customObjectives.Count > 0)
    {
      foreach (ObjectivesData objectivesData in customObjectives)
      {
        Debug.Log((object) $"TrackingObjective: obj.GroupId={objectivesData.GroupId}, item.Objective.GroupId={item.Objective.GroupId}");
        if (objectivesData.GroupId == item.Objective.GroupId || objectivesData.GroupId == item.Objective.GroupId.TryLocalize())
        {
          if (objectivesData is Objectives_PlaceStructure objectivesPlaceStructure1 && item.Objective is Objectives_PlaceStructure objective1)
            Debug.Log((object) $"Place Structure Pair is found! {{PlaceStructureA.DecoType={objectivesPlaceStructure1.DecoType}, PlaceStructureB.DecoType={objective1.DecoType}, PlaceStructureA.Target={objectivesPlaceStructure1.Target}, PlaceStructureB.Target={objective1.Target},PlaceStructureB.IsComplete={objective1.IsComplete}}}");
          if (objectivesData is Objectives_GetAnimal objectivesGetAnimal && item.Objective is Objectives_GetAnimal objective2 && objectivesGetAnimal.AnimalType == objective2.AnimalType || objectivesData is Objectives_PlaceStructure objectivesPlaceStructure2 && item.Objective is Objectives_PlaceStructure objective3 && objectivesPlaceStructure2.DecoType == objective3.DecoType && objectivesPlaceStructure2.Target == objective3.Target || objectivesData is Objectives_BuildStructure objectivesBuildStructure && item.Objective is Objectives_BuildStructure objective4 && objectivesBuildStructure.StructureType == objective4.StructureType && objectivesBuildStructure.Target == objective4.Target || objectivesData is Objectives_WinFlockadeBet objectivesWinFlockadeBet && item.Objective is Objectives_WinFlockadeBet objective5 && objectivesWinFlockadeBet.OpponentTermId == objective5.OpponentTermId && objectivesWinFlockadeBet.TargetWoolAmount == objective5.TargetWoolAmount || objectivesData is Objectives_CollectItem objectivesCollectItem && item.Objective is Objectives_CollectItem objective6 && objectivesCollectItem.ItemType == objective6.ItemType || objectivesData is Objectives_ShowFleece objectivesShowFleece && item.Objective is Objectives_ShowFleece objective7 && objectivesShowFleece.FleeceType == objective7.FleeceType || objectivesData is Objectives_LegendaryWeaponRun legendaryWeaponRun && item.Objective is Objectives_LegendaryWeaponRun objective8 && legendaryWeaponRun.LegendaryWeapon == objective8.LegendaryWeapon || objectivesData is Objectives_FindChildren objectivesFindChildren && item.Objective is Objectives_FindChildren objective9 && objectivesFindChildren.Location == objective9.Location)
          {
            objective = objectivesData;
            return true;
          }
        }
      }
    }
    return false;
  }

  public bool CompletedObjective(JobBoardMenuItem item, out ObjectivesDataFinalized objective)
  {
    objective = (ObjectivesDataFinalized) null;
    foreach (ObjectivesDataFinalized objectivesDataFinalized in DataManager.Instance.CompletedObjectivesHistory)
    {
      if ((objectivesDataFinalized.GroupId == item.Objective.GroupId || objectivesDataFinalized.GroupId == item.Objective.GroupId.TryLocalize()) && (objectivesDataFinalized is Objectives_GetAnimal.FinalizedData_GetAnimal finalizedDataGetAnimal && item.Objective is Objectives_GetAnimal objective1 && finalizedDataGetAnimal.AnimalType == objective1.AnimalType || objectivesDataFinalized is Objectives_PlaceStructure.FinalizedData_PlaceStructure dataPlaceStructure && item.Objective is Objectives_PlaceStructure objective2 && dataPlaceStructure.DecoType == objective2.DecoType && dataPlaceStructure.Target == objective2.Target || objectivesDataFinalized is Objectives_BuildStructure.FinalizedData_BuildStructure dataBuildStructure && item.Objective is Objectives_BuildStructure objective3 && dataBuildStructure.StructureType == objective3.StructureType && dataBuildStructure.Target == objective3.Target || objectivesDataFinalized is Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet dataWinFlockadeBet && item.Objective is Objectives_WinFlockadeBet objective4 && dataWinFlockadeBet.OpponentTermId == objective4.OpponentTermId || objectivesDataFinalized is Objectives_CollectItem.FinalizedData_CollectItem finalizedDataCollectItem && item.Objective is Objectives_CollectItem objective5 && finalizedDataCollectItem.ItemType == objective5.ItemType || objectivesDataFinalized is Objectives_ShowFleece.FinalizedData_ShowFleece finalizedDataShowFleece && item.Objective is Objectives_ShowFleece objective6 && finalizedDataShowFleece.FleeceType == objective6.FleeceType || objectivesDataFinalized is Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun legendaryWeaponRun && item.Objective is Objectives_LegendaryWeaponRun objective7 && legendaryWeaponRun.LegendaryWeapon == objective7.LegendaryWeapon || objectivesDataFinalized is Objectives_FindChildren.FinalizedData_FindChildren dataFindChildren && item.Objective is Objectives_FindChildren objective8 && dataFindChildren.Location == objective8.Location))
      {
        objective = objectivesDataFinalized;
        return true;
      }
    }
    return false;
  }

  public bool CompletedObjective(ObjectivesData objective)
  {
    foreach (ObjectivesDataFinalized objectivesDataFinalized in DataManager.Instance.CompletedObjectivesHistory)
    {
      if ((objectivesDataFinalized.GroupId == objective.GroupId || objectivesDataFinalized.GroupId == objective.GroupId.TryLocalize()) && (objectivesDataFinalized is Objectives_GetAnimal.FinalizedData_GetAnimal finalizedDataGetAnimal && objective is Objectives_GetAnimal objectivesGetAnimal && finalizedDataGetAnimal.AnimalType == objectivesGetAnimal.AnimalType || objectivesDataFinalized is Objectives_PlaceStructure.FinalizedData_PlaceStructure dataPlaceStructure && objective is Objectives_PlaceStructure objectivesPlaceStructure && dataPlaceStructure.DecoType == objectivesPlaceStructure.DecoType && dataPlaceStructure.Target == objectivesPlaceStructure.Target || objectivesDataFinalized is Objectives_BuildStructure.FinalizedData_BuildStructure dataBuildStructure && objective is Objectives_BuildStructure objectivesBuildStructure && dataBuildStructure.StructureType == objectivesBuildStructure.StructureType && dataBuildStructure.Target == objectivesBuildStructure.Target || objectivesDataFinalized is Objectives_WinFlockadeBet.FinalizedData_WinFlockadeBet dataWinFlockadeBet && objective is Objectives_WinFlockadeBet objectivesWinFlockadeBet && dataWinFlockadeBet.OpponentTermId == objectivesWinFlockadeBet.OpponentTermId || objectivesDataFinalized is Objectives_CollectItem.FinalizedData_CollectItem finalizedDataCollectItem && objective is Objectives_CollectItem objectivesCollectItem && finalizedDataCollectItem.ItemType == objectivesCollectItem.ItemType || objectivesDataFinalized is Objectives_ShowFleece.FinalizedData_ShowFleece finalizedDataShowFleece && objective is Objectives_ShowFleece objectivesShowFleece && finalizedDataShowFleece.FleeceType == objectivesShowFleece.FleeceType || objectivesDataFinalized is Objectives_LegendaryWeaponRun.FinalizedData_LegendaryWeaponRun legendaryWeaponRun1 && objective is Objectives_LegendaryWeaponRun legendaryWeaponRun2 && legendaryWeaponRun1.LegendaryWeapon == legendaryWeaponRun2.LegendaryWeapon || objectivesDataFinalized is Objectives_FindChildren.FinalizedData_FindChildren dataFindChildren && objective is Objectives_FindChildren objectivesFindChildren && dataFindChildren.Location == objectivesFindChildren.Location))
        return true;
    }
    return false;
  }

  public void OnMenuItemHightlighted(JobBoardMenuItem item)
  {
  }

  public override void OnCancelButtonInput()
  {
    if (this.inSequence)
      return;
    this.didCancel = true;
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    if (this.didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
