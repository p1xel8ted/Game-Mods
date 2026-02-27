// Decompiled with JetBrains decompiler
// Type: Interaction_JobBoard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Flockade;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class Interaction_JobBoard : Interaction
{
  [Header("Info")]
  [SerializeField]
  public Interaction_JobBoard.HostEnum host;
  [SerializeField]
  public DataManager.Variables activeVariable;
  [SerializeField]
  public DataManager.Variables completedVariable;
  [SerializeField]
  public global::Objectives.CustomQuestTypes customQuestType;
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public bool requiresPreviousObjectiveCompleted;
  [SerializeField]
  public bool periodicallyUpdateGoop;
  [SerializeField]
  public float goopCheckPeriod = 1f;
  [SerializeField]
  public List<Interaction_JobBoard.JobData> objectives = new List<Interaction_JobBoard.JobData>();
  [SerializeField]
  public GameObject dirt;
  [SerializeField]
  public List<GameObject> plants;
  [SerializeField]
  public GameObject goop;
  [SerializeField]
  public GameObject papers;
  [Header("Callbacks")]
  [SerializeField]
  public UnityEvent onJobComplete;
  public int _jobBoardHideLock;
  public bool hasTasksAlreadyCompleted;
  public float goopCheckTimer;
  public global::Objectives.CustomQuestTypes[] returnToBoardQuests = new global::Objectives.CustomQuestTypes[6]
  {
    global::Objectives.CustomQuestTypes.ReturnToRancher,
    global::Objectives.CustomQuestTypes.ReturnToBlacksmithJobBoard,
    global::Objectives.CustomQuestTypes.ReturnToTarotJobBoard,
    global::Objectives.CustomQuestTypes.ReturnToDecoJobBoard,
    global::Objectives.CustomQuestTypes.ReturnToFlockadeJobBoard,
    global::Objectives.CustomQuestTypes.ReturnToPriestJobBoard
  };
  public bool _revealing;

  public Interaction_JobBoard.HostEnum Host => this.host;

  public DataManager.Variables ActiveVariable => this.activeVariable;

  public bool RequiresPreviousObjectiveCompleted => this.requiresPreviousObjectiveCompleted;

  public List<Interaction_JobBoard.JobData> Objectives => this.objectives;

  public event Interaction_JobBoard.JobEvent OnJobCompleted;

  public int JobsCompletedCount
  {
    get
    {
      int jobsCompletedCount = 0;
      for (int index = 0; index < this.objectives.Count; ++index)
      {
        if (DataManager.Instance.JobBoardsClaimedQuests.Contains((int) (this.host + index)))
          ++jobsCompletedCount;
      }
      return jobsCompletedCount;
    }
  }

  public bool BoardCompleted => this.JobsCompletedCount >= this.objectives.Count;

  public void Start()
  {
    this.GetLabel();
    if (DataManager.Instance.GetVariable(this.activeVariable) && !DataManager.Instance.GetVariable(this.completedVariable))
      return;
    this.gameObject.SetActive(false);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.hasTasksAlreadyCompleted = this.CheckObjectives();
  }

  public bool CheckObjectives()
  {
    int num = -1;
    foreach (Interaction_JobBoard.JobData objective1 in this.objectives)
    {
      ++num;
      if (!DataManager.Instance.JobBoardsClaimedQuests.Contains((int) (this.host + num)))
      {
        ObjectivesData objective2 = CreateObjective.GetObjective(objective1.Objective, objective1.GroupTitle);
        if (this.host == Interaction_JobBoard.HostEnum.Rancher && objective2.Type == global::Objectives.TYPES.GET_ANIMAL)
        {
          foreach (Objectives_GetAnimal.FinalizedData_GetAnimal completedObjective in ObjectiveManager.GetCustomCompletedObjectives<Objectives_GetAnimal.FinalizedData_GetAnimal>())
          {
            if (completedObjective != null && completedObjective.GroupId == objective2.GroupId && completedObjective.AnimalType == ((Objectives_GetAnimal) objective2).AnimalType)
              return true;
          }
        }
        else if (this.host == Interaction_JobBoard.HostEnum.Priest && objective2.Type == global::Objectives.TYPES.SHOW_FLEECE)
        {
          foreach (Objectives_ShowFleece.FinalizedData_ShowFleece completedObjective in ObjectiveManager.GetCustomCompletedObjectives<Objectives_ShowFleece.FinalizedData_ShowFleece>())
          {
            if (completedObjective != null && completedObjective.GroupId == objective2.GroupId && completedObjective.FleeceType == ((Objectives_ShowFleece) objective2).FleeceType)
              return true;
          }
        }
        if (objective2 != null && objective2 is IJobBoardObjective jobBoardObjective)
          return jobBoardObjective.CanBeCompleted();
        if (objective2 != null && (ObjectiveManager.HasCompletedJobBoardObjective(objective2) || objective2.TryComplete()))
          return true;
      }
    }
    return false;
  }

  public override void Update()
  {
    base.Update();
    global::Objectives.CustomQuestTypes returnQuestForHost = this.GetReturnQuestForHost();
    if (this.periodicallyUpdateGoop)
    {
      this.goopCheckTimer += Time.deltaTime;
      if ((double) this.goopCheckTimer >= (double) this.goopCheckPeriod)
      {
        this.goopCheckTimer = 0.0f;
        this.hasTasksAlreadyCompleted = this.CheckObjectives();
      }
    }
    bool flag = returnQuestForHost != global::Objectives.CustomQuestTypes.None && ObjectiveManager.HasCustomObjectiveOfType(returnQuestForHost);
    if (!((UnityEngine.Object) this.goop != (UnityEngine.Object) null))
      return;
    this.goop.SetActive(this.hasTasksAlreadyCompleted | flag);
  }

  public global::Objectives.CustomQuestTypes GetReturnQuestForHost()
  {
    switch (this.host)
    {
      case Interaction_JobBoard.HostEnum.Rancher:
        return global::Objectives.CustomQuestTypes.ReturnToRancher;
      case Interaction_JobBoard.HostEnum.Flockade:
        return global::Objectives.CustomQuestTypes.ReturnToFlockadeJobBoard;
      case Interaction_JobBoard.HostEnum.Tarot:
        return global::Objectives.CustomQuestTypes.ReturnToTarotJobBoard;
      case Interaction_JobBoard.HostEnum.Deco:
        return global::Objectives.CustomQuestTypes.ReturnToDecoJobBoard;
      case Interaction_JobBoard.HostEnum.Priest:
        return global::Objectives.CustomQuestTypes.ReturnToPriestJobBoard;
      case Interaction_JobBoard.HostEnum.Blacksmith:
        return global::Objectives.CustomQuestTypes.ReturnToBlacksmithJobBoard;
      default:
        return global::Objectives.CustomQuestTypes.None;
    }
  }

  public override void GetLabel()
  {
    base.GetLabel();
    string str = $"{ScriptLocalization.FollowerInteractions.MakeDemand} | {LocalizationManager.GetTranslation(this.Objectives[0].GroupTitle)}";
    this.Label = this.Interactable ? str : "";
  }

  public void AddHideLock() => ++this._jobBoardHideLock;

  public void RemoveHideLock() => --this._jobBoardHideLock;

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.CheckCompleteFleeceObjective();
    this.StartCoroutine(this.HandleInteraction());
  }

  public IEnumerator HandleInteraction()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_JobBoard interactionJobBoard = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    GameManager.GetInstance().OnConversationNew();
    SimulationManager.Pause();
    interactionJobBoard.playerFarming.GoToAndStop(interactionJobBoard.transform.position + new Vector3(0.0f, -1.5f), interactionJobBoard.gameObject, GoToCallback: new System.Action(interactionJobBoard.DoJobBoardInteraction));
    return false;
  }

  public void DoJobBoardInteraction()
  {
    UIJobBoardMenuController menu = MonoSingleton<UIManager>.Instance.JobBoardMenuTemplate.Instantiate<UIJobBoardMenuController>();
    menu.Show(this);
    MonoSingleton<UIManager>.Instance.SetMenuInstance((UIMenuBase) menu);
    bool jobCompleted = false;
    UIJobBoardMenuController boardMenuController = menu;
    boardMenuController.OnHidden = boardMenuController.OnHidden + (System.Action) (() =>
    {
      this.CheckCompleteFleeceObjective();
      if (!jobCompleted)
        GameManager.GetInstance().OnConversationEnd();
      this.hasTasksAlreadyCompleted = this.CheckObjectives();
      SimulationManager.UnPause();
    });
    menu.OnItemCompleted += (System.Action<ObjectivesData>) (objective =>
    {
      ObjectiveManager.CompleteCustomObjective(this.customQuestType);
      this.onJobComplete?.Invoke();
      Interaction_JobBoard.JobEvent onJobCompleted = this.OnJobCompleted;
      if (onJobCompleted != null)
        onJobCompleted(objective);
      jobCompleted = true;
    });
    if (this.host != Interaction_JobBoard.HostEnum.Rancher)
      return;
    menu.OnAcceptJobQuest += (System.Action) (() =>
    {
      LambTownController.Instance.RanchDropOffItem.SetOnState(true);
      LambTownController.Instance.RanchDropOffItem.SetDropOffState(false, false);
    });
    menu.OnRemoveJobQuest += (System.Action) (() =>
    {
      LambTownController.Instance.RanchDropOffItem.SetOnState(false);
      LambTownController.Instance.RanchDropOffItem.SetDropOffState(false, false);
    });
  }

  public void Reveal()
  {
    if (DataManager.Instance.GetVariable(this.activeVariable))
      return;
    GameManager.GetInstance().StartCoroutine(this.RevealIE());
  }

  public IEnumerator RevealIE()
  {
    Interaction_JobBoard interactionJobBoard = this;
    if (!interactionJobBoard._revealing)
    {
      interactionJobBoard._revealing = true;
      yield return (object) new WaitForEndOfFrame();
      DataManager.Instance.SetVariable(interactionJobBoard.activeVariable, true);
      while (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom || LetterBox.IsPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew(SnapLetterBox: false);
      while (PlayerFarming.AnyPlayerGotoAndStopping())
        yield return (object) null;
      yield return (object) null;
      interactionJobBoard.gameObject.SetActive(true);
      interactionJobBoard.container.transform.localPosition = new Vector3(0.0f, 0.0f, 2.5f);
      interactionJobBoard.papers.gameObject.SetActive(false);
      foreach (GameObject plant in interactionJobBoard.plants)
      {
        if (!((UnityEngine.Object) plant == (UnityEngine.Object) null))
          plant.transform.localScale = Vector3.zero;
      }
      if ((UnityEngine.Object) interactionJobBoard.dirt != (UnityEngine.Object) null)
        interactionJobBoard.dirt.transform.localScale = Vector3.zero;
      float duration = 2f;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionJobBoard.gameObject, 6f);
      yield return (object) new WaitForSeconds(0.5f);
      interactionJobBoard.container.transform.DOMoveZ(0.0f, duration);
      interactionJobBoard.container.transform.DOScale(Vector3.one, duration).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", interactionJobBoard.gameObject);
      BiomeConstants.Instance.EmitSmokeInteractionVFXForDuration(duration, 0.5f, interactionJobBoard.transform.position, Vector3.one);
      interactionJobBoard.PlayBuildingSFXForDuration(duration);
      GameManager.GetInstance().CameraZoom(8f, duration);
      CameraManager.instance.ShakeCameraForDuration(0.01f, 0.1f, duration);
      interactionJobBoard.dirt.transform.DOScale(Vector3.one, duration).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      yield return (object) new WaitForSeconds(duration);
      interactionJobBoard.papers.gameObject.SetActive(true);
      interactionJobBoard.container.transform.DOShakePosition(0.5f, 0.1f);
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_Short", interactionJobBoard.gameObject);
      AudioManager.Instance.PlayOneShot("event:/building/finished_wood", interactionJobBoard.gameObject);
      CameraManager.shakeCamera(3f, 0.0f);
      interactionJobBoard.PlayPlantGrowSequence();
      yield return (object) new WaitForSeconds(1f);
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.JobBoards))
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.JobBoards, callback: (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
      else
        GameManager.GetInstance().OnConversationEnd();
      interactionJobBoard._revealing = false;
    }
  }

  public void PlayPlantGrowSequence() => this.StartCoroutine(this.PlantGrowRoutine());

  public IEnumerator PlantGrowRoutine()
  {
    foreach (GameObject plant in this.plants)
    {
      if (!((UnityEngine.Object) plant == (UnityEngine.Object) null))
      {
        plant.transform.DOScale(Vector3.one, 0.5f).From(0.0f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
        yield return (object) new WaitForSeconds(0.3f);
      }
    }
  }

  public void PlayBuildingSFXForDuration(float duration, float min = 0.1f, float max = 0.5f)
  {
    this.StartCoroutine(this.PlayBuildingSFXForDurationIE(duration, min, max));
  }

  public IEnumerator PlayBuildingSFXForDurationIE(float duration, float min, float max)
  {
    Interaction_JobBoard interactionJobBoard = this;
    for (float time = 0.0f; (double) time < (double) duration; time += UnityEngine.Random.Range(min, max))
    {
      AudioManager.Instance.PlayOneShot("event:/followers/hammering", interactionJobBoard.gameObject);
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(min, max));
    }
  }

  public void Hide() => this.StartCoroutine(this.HideIE());

  public IEnumerator HideIE()
  {
    Interaction_JobBoard interactionJobBoard = this;
    interactionJobBoard.Interactable = false;
    yield return (object) new WaitForEndOfFrame();
    while (PlayerFarming.Location != FollowerLocation.DLC_ShrineRoom || LetterBox.IsPlaying)
      yield return (object) null;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionJobBoard.gameObject, 6f);
    interactionJobBoard.container.transform.DOShakePosition(0.25f, 0.05f, 6);
    CameraManager.instance.ShakeCameraForDuration(0.005f, 0.05f, 0.25f);
    yield return (object) new WaitForSeconds(0.25f);
    interactionJobBoard.StartCoroutine(interactionJobBoard.PlantShrinkRoutine());
    if ((UnityEngine.Object) interactionJobBoard.dirt != (UnityEngine.Object) null)
      interactionJobBoard.dirt.transform.DOScale(Vector3.zero, 1.6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    BiomeConstants.Instance.EmitSmokeInteractionVFXForDuration(2f, 0.5f, interactionJobBoard.transform.position, Vector3.one);
    interactionJobBoard.PlayBuildingSFXForDuration(2f);
    interactionJobBoard.container.transform.DOMoveZ(2.5f, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    interactionJobBoard.container.transform.DOScale(Vector3.zero, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    GameManager.GetInstance().CameraZoom(5f, 2f);
    AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", interactionJobBoard.gameObject);
    yield return (object) new WaitForSeconds(2f);
    ObjectiveManager.CompleteCustomObjective(interactionJobBoard.customQuestType);
    DataManager.Instance.SetVariable(interactionJobBoard.completedVariable, true);
    GameManager.GetInstance().OnConversationEnd();
    if ((UnityEngine.Object) WoolhavenYngyaStatue.Instance != (UnityEngine.Object) null)
    {
      WoolhavenYngyaStatue.Instance.PlayUnlockChain();
      yield return (object) null;
      yield return (object) new WaitWhile((Func<bool>) (() => WoolhavenYngyaStatue.Instance.UnlockingChain));
    }
    interactionJobBoard.StartCoroutine(interactionJobBoard.DeactivateJobBoardIE());
  }

  public IEnumerator DeactivateJobBoardIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    Interaction_JobBoard interactionJobBoard = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      Debug.Log((object) $"Job Board Complete handlers are finished, deactivating job board: lockcount: {interactionJobBoard._jobBoardHideLock}");
      interactionJobBoard.gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    Debug.Log((object) $"Waiting for Job Board Complete handlers to finish up before deactivating job board: lockcount: {interactionJobBoard._jobBoardHideLock}");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitWhile(new Func<bool>(interactionJobBoard.\u003CDeactivateJobBoardIE\u003Eb__56_0));
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public IEnumerator PlantShrinkRoutine()
  {
    for (int i = this.plants.Count - 1; i >= 0; --i)
    {
      GameObject plant = this.plants[i];
      if (!((UnityEngine.Object) plant == (UnityEngine.Object) null))
      {
        plant.transform.DOScale(Vector3.zero, 0.4f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
        yield return (object) new WaitForSeconds(0.2f);
      }
    }
  }

  public void CheckCompleteFleeceObjective()
  {
    foreach (Interaction_JobBoard.JobData objective in this.Objectives)
    {
      if (objective.Objective.Fleece == (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerFleece || objective.Objective.Fleece == (PlayerFleeceManager.FleeceType) DataManager.Instance.PlayerVisualFleece)
        ObjectiveManager.CompleteShowFleeceObjective();
    }
  }

  [CompilerGenerated]
  public bool \u003CDeactivateJobBoardIE\u003Eb__56_0() => this._jobBoardHideLock > 0;

  [Serializable]
  public enum HostEnum
  {
    Rancher = 0,
    Flockade = 100, // 0x00000064
    Tarot = 200, // 0x000000C8
    Deco = 300, // 0x0000012C
    Priest = 400, // 0x00000190
    Blacksmith = 500, // 0x000001F4
  }

  [Serializable]
  public class JobData
  {
    public string GroupTitle;
    public CreateObjective.ObjectiveToGive Objective;
    public bool IsRewardTarot;
    public bool IsRewardDeco;
    public bool IsRewardItem;
    public bool IsRewardFlockade;
    public bool IsRewardWeapon;
    public TarotCards.Card RewardTarot;
    public StructureBrain.TYPES RewardDeco;
    public InventoryItem.ITEM_TYPE RewardItem;
    public FlockadePieceType RewardFlockade;
    public EquipmentType RewardWeapon;
    public bool RequiresCondition;
    public DataManager.Variables ConditionalVariable;
  }

  public delegate void JobEvent(ObjectivesData objective);
}
