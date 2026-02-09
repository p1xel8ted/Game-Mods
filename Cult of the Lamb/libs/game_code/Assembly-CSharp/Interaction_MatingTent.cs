// Decompiled with JetBrains decompiler
// Type: Interaction_MatingTent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_MatingTent : Interaction
{
  public static Interaction_MatingTent Instance;
  public const int MATING_DURATION = 30;
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject pos1;
  [SerializeField]
  public GameObject pos2;
  [SerializeField]
  public GameObject sexyPos;
  [SerializeField]
  public GameObject playerPos;
  [SerializeField]
  public GameObject normal;
  [SerializeField]
  public GameObject particles;
  [SerializeField]
  public GameObject evilParticles;
  [SerializeField]
  public GameObject yngyaParticles;
  [SerializeField]
  public GameObject ActivateLighting;
  [SerializeField]
  public GameObject inprogress;
  [SerializeField]
  public SkeletonAnimation tentaclesSpine;
  [SerializeField]
  public SkeletonAnimation lambMatingSpine;
  public List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
  public Structures_MatingTent structureBrain;
  public Follower follower1;
  public Follower follower2;
  public string sCollect;
  public PlayerFarming playerInside;
  public bool bothPlayersInside;
  public bool isLoadingAssets;

  public Structure Structure => this.structure;

  public void Awake()
  {
    Interaction_MatingTent.Instance = this;
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    this.UpdateLocalisation();
  }

  public void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.structureBrain = this.structure.Brain as Structures_MatingTent;
    this.normal.gameObject.SetActive(this.structureBrain.Data.MultipleFollowerIDs.Count <= 0);
    this.inprogress.gameObject.SetActive(this.structureBrain.Data.MultipleFollowerIDs.Count > 0);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sCollect = ScriptLocalization.Interactions.Collect;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = true;
    this.HasSecondaryInteraction = false;
    this.SecondaryLabel = "";
    if ((UnityEngine.Object) this.playerInside != (UnityEngine.Object) null)
    {
      this.SecondaryInteractable = false;
      this.Label = ScriptLocalization.Interactions.JoinDance;
    }
    else if ((double) DataManager.Instance.MatingCompletedTimestamp != -1.0 && (double) TimeManager.TotalElapsedGameTime > (double) DataManager.Instance.MatingCompletedTimestamp && this.structureBrain != null && this.structureBrain.Data != null && this.structureBrain.Data.MultipleFollowerIDs.Count > 0)
    {
      this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
    }
    else
    {
      int num = 0;
      foreach (Follower follower in Follower.Followers)
      {
        if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
          ++num;
      }
      if (num >= 2)
      {
        this.Label = $"{LocalizationManager.GetTranslation("Interactions/InviteFollowers")} {InventoryItem.CapacityString(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)}";
        this.Interactable = true;
      }
      else
      {
        this.Label = DataManager.Instance.Followers.Count <= 0 ? ScriptLocalization.Interactions.NoFollowers : ScriptLocalization.Interactions.RequiresMoreFollowers;
        this.Interactable = false;
      }
      if ((double) DataManager.Instance.MatingCompletedTimestamp != -1.0 && this.structureBrain.Data.MultipleFollowerIDs.Count >= 2)
      {
        this.Interactable = false;
        this.Label = string.Format(LocalizationManager.GetTranslation("Interactions/FollowersMating"), (object) FollowerInfo.GetInfoByID(this.structureBrain.Data.MultipleFollowerIDs[0]).Name, (object) FollowerInfo.GetInfoByID(this.structureBrain.Data.MultipleFollowerIDs[1]).Name);
      }
      this.HasSecondaryInteraction = this.SecondaryInteractable = CoopManager.CoopActive && (UnityEngine.Object) this.playerInside == (UnityEngine.Object) null;
      if (!((UnityEngine.Object) this.playerInside == (UnityEngine.Object) this.playerFarming))
        return;
      this.Label = "";
    }
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    if (this.SecondaryInteractable && !DataManager.Instance.PlayersShagged)
      this.SecondaryLabel = (UnityEngine.Object) this.playerInside != (UnityEngine.Object) null ? ScriptLocalization.Interactions.JoinDance : LocalizationManager.GetTranslation("UI/Enter");
    else
      this.SecondaryLabel = "";
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.normal.gameObject.SetActive(true);
    this.inprogress.gameObject.SetActive(false);
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (DataManager.Instance.PlayersShagged)
      return;
    this._playerFarming = state.GetComponent<PlayerFarming>();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlayersMatingIE());
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((UnityEngine.Object) this.structure != (UnityEngine.Object) null)
      this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (!(bool) (UnityEngine.Object) MonoSingleton<UIManager>.Instance)
      return;
    MonoSingleton<UIManager>.Instance.UnloadMatingMenuAssets();
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.isLoadingAssets)
      return;
    base.OnInteract(state);
    if ((UnityEngine.Object) this.playerInside != (UnityEngine.Object) null)
      this.StartCoroutine((IEnumerator) this.PlayersMatingIE());
    else if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT) < 1)
    {
      this.playerFarming.indicator.PlayShake();
    }
    else
    {
      GameManager.GetInstance().OnConversationNew();
      Time.timeScale = 0.0f;
      HUD_Manager.Instance.Hide(false, 0);
      this.followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (Follower follower in Follower.Followers)
      {
        if (follower.Brain.Info.IsSnowman && !follower.Brain.Info.IsGoodSnowman)
          this.followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerSelectEntry.Status.Unavailable));
        else if (!DataManager.Instance.Followers_Recruit.Contains(follower.Brain._directInfoAccess) && !follower.Brain.Info.SkinName.Contains("Webber"))
          this.followerSelectEntries.Add(new FollowerSelectEntry(follower.Brain._directInfoAccess, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain, true)));
      }
      this.follower1 = (Follower) null;
      this.follower2 = (Follower) null;
      this.isLoadingAssets = true;
      this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadMatingMenuAssets(), (System.Action) (() =>
      {
        this.isLoadingAssets = false;
        UIMatingMenuController matingMenuController = MonoSingleton<UIManager>.Instance.MatingMenuControllerTemplate.Instantiate<UIMatingMenuController>();
        matingMenuController.Show(this, this.followerSelectEntries);
        matingMenuController.OnFollowersChosen += (System.Action<FollowerInfo, FollowerInfo>) ((f1, f2) =>
        {
          this.follower1 = FollowerManager.FindFollowerByID(f1.ID);
          this.follower2 = FollowerManager.FindFollowerByID(f2.ID);
          if (TimeManager.IsNight && (UnityEngine.Object) this.follower1 != (UnityEngine.Object) null && this.follower1.Brain.CurrentTask != null && this.follower1.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.follower1.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower1.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
            CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, f1.ID);
          if (TimeManager.IsNight && (UnityEngine.Object) this.follower2 != (UnityEngine.Object) null && this.follower2.Brain.CurrentTask != null && this.follower2.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.follower2.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower2.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
            CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, f2.ID);
          this.StartCoroutine((IEnumerator) this.SexyTimeIE());
        });
        matingMenuController.OnHidden = matingMenuController.OnHidden + (System.Action) (() =>
        {
          if (!((UnityEngine.Object) this.follower1 == (UnityEngine.Object) null) && !((UnityEngine.Object) this.follower2 == (UnityEngine.Object) null))
            return;
          GameManager.GetInstance().OnConversationEnd();
          Time.timeScale = 1f;
        });
      })));
    }
  }

  public IEnumerator SexyTimeIE()
  {
    Interaction_MatingTent interactionMatingTent = this;
    interactionMatingTent.playerFarming.GoToAndStop(interactionMatingTent.playerPos.transform.position, interactionMatingTent.gameObject);
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT, -1);
    Time.timeScale = 1f;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionMatingTent.gameObject, 6f);
    SimulationManager.Pause();
    interactionMatingTent.follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    interactionMatingTent.follower1.transform.position = interactionMatingTent.pos1.transform.position;
    interactionMatingTent.follower1.FacePosition(interactionMatingTent.sexyPos.transform.position);
    bool bothZombies = interactionMatingTent.follower1.Brain.HasTrait(FollowerTrait.TraitType.Zombie) && interactionMatingTent.follower2.Brain.HasTrait(FollowerTrait.TraitType.Zombie);
    if (interactionMatingTent.follower1.Brain.HasTrait(FollowerTrait.TraitType.Zombie) && !bothZombies)
      interactionMatingTent.follower1.transform.position = interactionMatingTent.pos1.transform.position + Vector3.right / 3f;
    interactionMatingTent.follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    interactionMatingTent.follower2.transform.position = interactionMatingTent.pos2.transform.position;
    interactionMatingTent.follower2.FacePosition(interactionMatingTent.sexyPos.transform.position);
    if (interactionMatingTent.follower2.Brain.HasTrait(FollowerTrait.TraitType.Zombie) && !bothZombies)
      interactionMatingTent.follower2.transform.position = interactionMatingTent.pos2.transform.position + Vector3.left / 3f;
    yield return (object) new WaitForEndOfFrame();
    Interaction_MatingTent.FollowerResponse follower1Response = interactionMatingTent.GetResponse(interactionMatingTent.follower1.Brain, interactionMatingTent.follower2.Brain);
    Interaction_MatingTent.FollowerResponse follower2Response = interactionMatingTent.GetResponse(interactionMatingTent.follower2.Brain, interactionMatingTent.follower1.Brain);
    float chanceForSuccess = Interaction_MatingTent.GetChanceToMate(interactionMatingTent.follower1.Brain, interactionMatingTent.follower2.Brain);
    bool success = (double) UnityEngine.Random.value <= (double) chanceForSuccess || !DataManager.Instance.HadInitialMatingTentInteraction && (double) chanceForSuccess > 0.0;
    if (success)
    {
      if (follower1Response < Interaction_MatingTent.FollowerResponse.Obliged)
        follower1Response = Interaction_MatingTent.FollowerResponse.Obliged;
      if (follower2Response < Interaction_MatingTent.FollowerResponse.Obliged)
        follower2Response = Interaction_MatingTent.FollowerResponse.Obliged;
    }
    else if ((double) UnityEngine.Random.value < 0.5 && follower1Response >= Interaction_MatingTent.FollowerResponse.Obliged)
      follower1Response = Interaction_MatingTent.FollowerResponse.NotInterested;
    else if (follower2Response >= Interaction_MatingTent.FollowerResponse.Obliged)
      follower2Response = Interaction_MatingTent.FollowerResponse.NotInterested;
    Follower leader = follower1Response < Interaction_MatingTent.FollowerResponse.Obliged ? interactionMatingTent.follower1 : interactionMatingTent.follower2;
    Follower other = follower1Response < Interaction_MatingTent.FollowerResponse.Obliged ? interactionMatingTent.follower2 : interactionMatingTent.follower1;
    if (leader.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      Follower follower = leader;
      leader = other;
      other = follower;
    }
    yield return (object) new WaitForEndOfFrame();
    if (leader.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      double num1 = (double) leader.SetBodyAnimation("Zombie/zombie-idle", true);
    }
    else
    {
      double num2 = (double) leader.SetBodyAnimation(interactionMatingTent.GetDecidingAnimation(), true);
    }
    if (other.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      double num3 = (double) other.SetBodyAnimation("Zombie/zombie-idle", true);
    }
    else
    {
      double num4 = (double) other.SetBodyAnimation(interactionMatingTent.GetAwaitingAnimation(interactionMatingTent.follower2.Brain), true);
    }
    if (!bothZombies)
      yield return (object) new WaitForSeconds(3f);
    double num5 = (double) interactionMatingTent.follower1.SetBodyAnimation(interactionMatingTent.GetResponseAnimation(follower1Response), false);
    interactionMatingTent.follower1.AddBodyAnimation(interactionMatingTent.follower1.Brain.CurrentState == null ? "idle" : interactionMatingTent.follower1.Brain.CurrentState.OverrideIdleAnim, true, 0.0f);
    double num6 = (double) interactionMatingTent.follower2.SetBodyAnimation(interactionMatingTent.GetResponseAnimation(follower2Response), false);
    interactionMatingTent.follower2.AddBodyAnimation(interactionMatingTent.follower2.Brain.CurrentState == null ? "idle" : interactionMatingTent.follower2.Brain.CurrentState.OverrideIdleAnim, true, 0.0f);
    if (!success)
      AudioManager.Instance.PlayOneShot("event:/Stings/mating_fail");
    if (!bothZombies)
      yield return (object) new WaitForSeconds(1.5f);
    DataManager.Instance.HadInitialMatingTentInteraction = true;
    Objectives_Mating objectivesMating = (Objectives_Mating) null;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective.Type == Objectives.TYPES.MATING)
        objectivesMating = objective as Objectives_Mating;
    }
    if (objectivesMating != null && objectivesMating.CompleteTerm.Contains("Story") && (objectivesMating.TargetFollower_1 == interactionMatingTent.follower1.Brain.Info.ID && objectivesMating.TargetFollower_2 == interactionMatingTent.follower2.Brain.Info.ID || objectivesMating.TargetFollower_2 == interactionMatingTent.follower1.Brain.Info.ID && objectivesMating.TargetFollower_1 == interactionMatingTent.follower2.Brain.Info.ID))
      success = true;
    if (success)
    {
      if (!interactionMatingTent.follower1.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      {
        double num7 = (double) interactionMatingTent.follower1.SetBodyAnimation("kiss", false);
      }
      if (!interactionMatingTent.follower2.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      {
        double num8 = (double) interactionMatingTent.follower2.SetBodyAnimation("kiss", false);
      }
      if (!bothZombies)
        yield return (object) new WaitForSeconds(4f);
      interactionMatingTent.follower1.GoTo(interactionMatingTent.sexyPos.transform.position, new System.Action(interactionMatingTent.\u003CSexyTimeIE\u003Eb__35_0));
      interactionMatingTent.follower2.GoTo(interactionMatingTent.sexyPos.transform.position, new System.Action(interactionMatingTent.\u003CSexyTimeIE\u003Eb__35_1));
      if (bothZombies)
        yield return (object) new WaitForSeconds(1.5f);
      else
        yield return (object) new WaitForSeconds(0.5f);
      interactionMatingTent.follower1.HideAllFollowerIcons();
      interactionMatingTent.follower2.HideAllFollowerIcons();
      interactionMatingTent.follower1.Spine.gameObject.SetActive(false);
      interactionMatingTent.follower2.Spine.gameObject.SetActive(false);
      DataManager.Instance.MatingCompletedTimestamp = TimeManager.TotalElapsedGameTime + 30f;
      interactionMatingTent.follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Mating());
      interactionMatingTent.follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Mating());
      interactionMatingTent.structureBrain.Data.MultipleFollowerIDs.Add(interactionMatingTent.follower1.Brain.Info.ID);
      interactionMatingTent.structureBrain.Data.MultipleFollowerIDs.Add(interactionMatingTent.follower2.Brain.Info.ID);
      interactionMatingTent.structureBrain.SetEggInfo(interactionMatingTent.follower1.Brain, interactionMatingTent.follower2.Brain, chanceForSuccess);
      interactionMatingTent.normal.gameObject.SetActive(false);
      interactionMatingTent.inprogress.gameObject.SetActive(true);
      interactionMatingTent.particles.gameObject.SetActive(true);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendFollowerToMatingTent, interactionMatingTent.follower1.Brain.Info.ID);
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendFollowerToMatingTent, interactionMatingTent.follower2.Brain.Info.ID);
      if (!interactionMatingTent.structureBrain.Data.MatingFailed && interactionMatingTent.structureBrain.Data.EggInfo != null)
      {
        List<FollowerTrait.TraitType> p1traits = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) interactionMatingTent.follower1.Brain._directInfoAccess.Traits);
        for (int index = p1traits.Count - 1; index >= 0; --index)
        {
          if (FollowerTrait.ExcludedFromMating.Contains(p1traits[index]))
            p1traits.RemoveAt(index);
        }
        List<FollowerTrait.TraitType> p2traits = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) interactionMatingTent.follower2.Brain._directInfoAccess.Traits);
        for (int index = p2traits.Count - 1; index >= 0; --index)
        {
          if (FollowerTrait.ExcludedFromMating.Contains(p2traits[index]))
            p2traits.RemoveAt(index);
        }
        int num9 = 0;
        for (int index = 0; index < p1traits.Count; ++index)
        {
          ++num9;
          if (FollowerTrait.ExclusiveTraits.ContainsKey(p1traits[index]) && !p2traits.Contains(FollowerTrait.ExclusiveTraits[p1traits[index]]))
            ++num9;
        }
        List<FollowerTrait.TraitType> randomTraits = new List<FollowerTrait.TraitType>();
        for (int index = 0; index < 2; ++index)
        {
          FollowerTrait.TraitType startingTrait;
          do
          {
            startingTrait = FollowerTrait.GetStartingTrait();
          }
          while (randomTraits.Contains(startingTrait) || num9 < 2 && FollowerTrait.ExclusiveTraits.ContainsKey(startingTrait) && (p1traits.Contains(FollowerTrait.ExclusiveTraits[startingTrait]) || p2traits.Contains(FollowerTrait.ExclusiveTraits[startingTrait])));
          randomTraits.Add(startingTrait);
        }
        if (interactionMatingTent.follower1.Brain.Info.IsGoodSnowman && interactionMatingTent.follower2.Brain.Info.IsGoodSnowman)
        {
          p1traits.Clear();
          p2traits.Clear();
          randomTraits.Clear();
        }
        UITraitSelector traitSelector = MonoSingleton<UIManager>.Instance.TraitSelectorTemplate.Instantiate<UITraitSelector>();
        traitSelector.Show();
        traitSelector.Configure(interactionMatingTent.follower1.Brain._directInfoAccess, interactionMatingTent.follower2.Brain._directInfoAccess, p1traits, p2traits, randomTraits, interactionMatingTent.structureBrain.Data.EggInfo);
        traitSelector.TraitsChosen += new System.Action<List<FollowerTrait.TraitType>>(interactionMatingTent.\u003CSexyTimeIE\u003Eb__35_4);
        while ((UnityEngine.Object) traitSelector != (UnityEngine.Object) null)
          yield return (object) null;
        traitSelector = (UITraitSelector) null;
      }
      ObjectiveManager.CompleteMatingObjective(interactionMatingTent.follower1.Brain.Info.ID, interactionMatingTent.follower2.Brain.Info.ID);
      interactionMatingTent.FollowersFinishedMating(interactionMatingTent.follower1, interactionMatingTent.follower2);
    }
    else
    {
      Follower f = (Follower) null;
      if (follower1Response >= Interaction_MatingTent.FollowerResponse.Obliged && follower2Response < Interaction_MatingTent.FollowerResponse.Obliged)
      {
        interactionMatingTent.follower2.Brain.CompleteCurrentTask();
        f = interactionMatingTent.follower1;
        interactionMatingTent.follower1.AddThought((Thought) UnityEngine.Random.Range(323, 326));
        interactionMatingTent.follower2.AddThought((Thought) UnityEngine.Random.Range(326, 328));
        interactionMatingTent.follower1.TimedAnimation("Reactions/react-cry", 9.066667f, (System.Action) (() => f.Brain.CompleteCurrentTask()));
      }
      else if (follower2Response >= Interaction_MatingTent.FollowerResponse.Obliged && follower1Response < Interaction_MatingTent.FollowerResponse.Obliged)
      {
        interactionMatingTent.follower1.Brain.CompleteCurrentTask();
        f = interactionMatingTent.follower2;
        interactionMatingTent.follower2.AddThought((Thought) UnityEngine.Random.Range(323, 326));
        interactionMatingTent.follower1.AddThought((Thought) UnityEngine.Random.Range(326, 328));
        interactionMatingTent.follower2.TimedAnimation("Reactions/react-cry", 9.066667f, (System.Action) (() => f.Brain.CompleteCurrentTask()));
      }
      else
      {
        interactionMatingTent.follower1.Brain.CompleteCurrentTask();
        interactionMatingTent.follower2.Brain.CompleteCurrentTask();
      }
      if (objectivesMating != null && (objectivesMating.TargetFollower_1 == interactionMatingTent.follower1.Brain.Info.ID || objectivesMating.TargetFollower_2 == interactionMatingTent.follower1.Brain.Info.ID) && !objectivesMating.CompleteTerm.Contains("Story"))
        objectivesMating.CompleteTerm = "GiveQuest/Mating/CompleteUnsuccessful";
      ObjectiveManager.CompleteMatingObjective(interactionMatingTent.follower1.Brain.Info.ID, interactionMatingTent.follower2.Brain.Info.ID);
      yield return (object) new WaitForSeconds(1f);
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
    }
  }

  public Interaction_MatingTent.FollowerResponse GetResponse(FollowerBrain f1, FollowerBrain f2)
  {
    IDAndRelationship relationship = f1.Info.GetOrCreateRelationship(f2.Info.ID);
    if (f1.HasTrait(FollowerTrait.TraitType.Zombie))
      return Interaction_MatingTent.FollowerResponse.Zombie;
    if (FollowerManager.AreSiblings(f1.Info.ID, f2.Info.ID) || FollowerManager.IsChildOf(f1.Info.ID, f2.Info.ID) || FollowerManager.IsChildOf(f2.Info.ID, f1.Info.ID) || f1.HasTrait(FollowerTrait.TraitType.Celibate))
      return Interaction_MatingTent.FollowerResponse.Disgusted;
    if (f1.HasTrait(FollowerTrait.TraitType.Lustful))
      return Interaction_MatingTent.FollowerResponse.InLove;
    if (f1.CurrentState.Type != FollowerStateType.Drunk && f2.CurrentState.Type == FollowerStateType.Drunk)
      return Interaction_MatingTent.FollowerResponse.Disgusted;
    if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
      return Interaction_MatingTent.FollowerResponse.Happy;
    if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
      return Interaction_MatingTent.FollowerResponse.InLove;
    if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
      return Interaction_MatingTent.FollowerResponse.Enemies;
    return (double) UnityEngine.Random.value >= 0.20000000298023224 ? Interaction_MatingTent.FollowerResponse.Obliged : Interaction_MatingTent.FollowerResponse.NotInterested;
  }

  public string GetResponseAnimation(Interaction_MatingTent.FollowerResponse response)
  {
    switch (response)
    {
      case Interaction_MatingTent.FollowerResponse.NotInterested:
        return "Reactions/react-non-believers";
      case Interaction_MatingTent.FollowerResponse.Enemies:
        return (double) UnityEngine.Random.value < 0.25 ? "Conversation/react-mean1" : "Conversation/react-hate" + UnityEngine.Random.Range(1, 3).ToString();
      case Interaction_MatingTent.FollowerResponse.Disgusted:
        return "Reactions/react-sick";
      case Interaction_MatingTent.FollowerResponse.Happy:
        return "Reactions/react-admire" + UnityEngine.Random.Range(1, 4).ToString();
      case Interaction_MatingTent.FollowerResponse.InLove:
        return "Reactions/react-loved" + ((double) UnityEngine.Random.value < 0.5 ? "2" : "");
      case Interaction_MatingTent.FollowerResponse.Zombie:
        return "Zombie/zombie-idle";
      default:
        if ((double) UnityEngine.Random.value < 0.25)
          return "Conversations/react-love1";
        if ((double) UnityEngine.Random.value < 0.25)
          return "Conversations/react-nice1";
        return (double) UnityEngine.Random.value < 0.25 ? "Conversations/react-nice3" : "Reactions/react-bow";
    }
  }

  public string GetAwaitingAnimation(FollowerBrain brain)
  {
    if (brain.HasTrait(FollowerTrait.TraitType.Argumentative) || brain.HasTrait(FollowerTrait.TraitType.Bastard))
      return "Egg/mating-pose2";
    switch (UnityEngine.Random.Range(0, 3))
    {
      case 0:
        return "Egg/mating-pose";
      case 1:
        return "Egg/mating-pose2";
      default:
        return "Egg/mating-pose3";
    }
  }

  public string GetDecidingAnimation()
  {
    switch (UnityEngine.Random.Range(0, 4))
    {
      case 0:
        return "Egg/mating-consider";
      case 1:
        return "Egg/mating-consider2";
      case 2:
        return "Egg/mating-consider3";
      default:
        return "Egg/mating-consider4";
    }
  }

  public void OnHidden()
  {
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
  }

  public static float GetChanceToMate(int follower1, int follower2)
  {
    FollowerBrain brain1 = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(follower1));
    FollowerBrain brain2 = FollowerBrain.GetOrCreateBrain(FollowerInfo.GetInfoByID(follower2));
    return brain1 != null && brain2 != null ? Interaction_MatingTent.GetChanceToMate(brain1, brain2) : 0.0f;
  }

  public static float GetChanceToMate(FollowerBrain f1, FollowerBrain f2)
  {
    float num = 0.5f;
    IDAndRelationship relationship = f1.Info.GetOrCreateRelationship(f2.Info.ID);
    if (FollowerManager.IsChild(f1.Info.ID) || FollowerManager.IsChild(f2.Info.ID) || FollowerManager.AreSiblings(f1.Info.ID, f2.Info.ID) || FollowerManager.IsChildOf(f1.Info.ID, f2.Info.ID) || FollowerManager.IsChildOf(f2.Info.ID, f1.Info.ID) || f1.CurrentState == null || f2.CurrentState == null || f1.Info.ID == 666 && (f2.Info.ID == 99994 || f2.Info.ID == 99995) || f2.Info.ID == 666 && (f1.Info.ID == 99994 || f1.Info.ID == 99995) || f1.Info.IsSnowman && !f1.Info.IsGoodSnowman || f2.Info.IsSnowman && !f2.Info.IsGoodSnowman)
      return 0.0f;
    if (f1.HasTrait(FollowerTrait.TraitType.Celibate) || f2.HasTrait(FollowerTrait.TraitType.Celibate))
      num -= 0.3f;
    if (f1.HasTrait(FollowerTrait.TraitType.Lustful) || f2.HasTrait(FollowerTrait.TraitType.Lustful))
      num += 0.3f;
    if (f1.HasTrait(FollowerTrait.TraitType.MissionaryExcited) || f2.HasTrait(FollowerTrait.TraitType.MissionaryExcited))
      num += 0.2f;
    if (f1.CurrentState.Type != FollowerStateType.Drunk && f2.CurrentState.Type == FollowerStateType.Drunk)
      num -= 0.25f;
    if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Friends)
      num += 0.25f;
    else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Lovers)
    {
      num += 0.35f;
      if (f1._directInfoAccess.SpouseFollowerID == f2._directInfoAccess.SpouseFollowerID)
        num += 0.2f;
    }
    else if (relationship.CurrentRelationshipState == IDAndRelationship.RelationshipState.Enemies)
    {
      num -= 0.35f;
      if (f1._directInfoAccess.SpouseFollowerID == f2._directInfoAccess.SpouseFollowerID)
        num -= 0.2f;
    }
    if ((double) f1._directInfoAccess.Exhaustion > 0.0)
      num -= 0.4f;
    if ((double) f2._directInfoAccess.Exhaustion > 0.0)
      num -= 0.4f;
    if (f1._directInfoAccess.CursedState == Thought.OldAge)
      num -= 0.2f;
    if (f2._directInfoAccess.CursedState == Thought.OldAge)
      num -= 0.2f;
    if (f1.Info.CursedState == Thought.Ill)
      num -= 0.2f;
    if (f2.Info.CursedState == Thought.Ill)
      num -= 0.2f;
    if (f1.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      num -= 0.25f;
    if (f2.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      num -= 0.25f;
    if (f1.Info.HasTrait(FollowerTrait.TraitType.Zombie) && f2.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      return 1f;
    if (f1.Info.CursedState == Thought.BecomeStarving)
      num -= 0.2f;
    if (f2.Info.CursedState == Thought.BecomeStarving)
      num -= 0.2f;
    if (FollowerBrainStats.IsNudism)
      num += 0.35f;
    if (f1.Info.IsSnowman && f2.Info.IsSnowman)
      num += 0.2f;
    if (f1.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_1) || f1.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_2) || f1.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_3))
      num += 0.2f;
    if (f2.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_1) || f2.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_2) || f2.Info.Traits.Contains(FollowerTrait.TraitType.PureBlood_3))
      num += 0.2f;
    foreach (ObjectivesData objective in DataManager.Instance.Objectives)
    {
      if (objective is Objectives_Mating objectivesMating && objectivesMating.TargetFollower_1 == f1.Info.ID && objectivesMating.TargetFollower_2 == f2.Info.ID && objective.CompleteTerm.Contains("Story"))
        num = 1f;
      else if (objective is Objectives_Custom objectivesCustom && objectivesCustom.CustomQuestType == Objectives.CustomQuestTypes.DepositFollower && (f1.Info.SkinName.Contains("Bug") || f2.Info.SkinName.Contains("Bug")))
        num += 0.2f;
    }
    int maxValue = 15;
    System.Random random = new System.Random(f1.Info.ID + f2.Info.ID + TimeManager.CurrentDay);
    return Mathf.Clamp01(num + (float) random.Next(-maxValue, maxValue) / 100f);
  }

  public void FollowersFinishedMating(Follower f1, Follower f2)
  {
    this.structureBrain.SetEggReady();
    this.structureBrain.Data.MultipleFollowerIDs.Clear();
    DataManager.Instance.MatingCompletedTimestamp = -1f;
    FollowerSpecialType special1 = f1.Brain.Info.ID == 99996 ? f1.Brain.Info.Special : FollowerSpecialType.None;
    FollowerSpecialType special2 = f2.Brain.Info.ID == 99996 ? f2.Brain.Info.Special : FollowerSpecialType.None;
    FollowerBrain.SetFollowerCostume(f1.Spine.Skeleton, f1.Brain.Info.XPLevel, f1.Brain.Info.SkinName, f1.Brain.Info.SkinColour, FollowerOutfitType.Naked, f1.Brain.Info.Hat, f1.Brain.Info.Clothing, f1.Brain.Info.Customisation, special1, f1.Brain.Info.Necklace, f1.Brain.Info.ClothingVariant, f1.Brain._directInfoAccess);
    FollowerBrain.SetFollowerCostume(f2.Spine.Skeleton, f2.Brain.Info.XPLevel, f2.Brain.Info.SkinName, f2.Brain.Info.SkinColour, FollowerOutfitType.Naked, f2.Brain.Info.Hat, f2.Brain.Info.Clothing, f2.Brain.Info.Customisation, special2, f2.Brain.Info.Necklace, f2.Brain.Info.ClothingVariant, f2.Brain._directInfoAccess);
    this.playerFarming.GoToAndStop(this.playerPos, this.gameObject);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.gameObject, 6f);
    if ((UnityEngine.Object) f1 != (UnityEngine.Object) null && (UnityEngine.Object) f2 != (UnityEngine.Object) null)
    {
      this.StartCoroutine((IEnumerator) this.FollowersFinishedMatingIE(f1, f2));
    }
    else
    {
      if (f1.Brain != null)
      {
        f1.Brain.CompleteCurrentTask();
        this.TryToInfectOtherFollower(f1.Brain, f2.Brain);
      }
      if (f2.Brain == null)
        return;
      f2.Brain.CompleteCurrentTask();
      this.TryToInfectOtherFollower(f2.Brain, f1.Brain);
    }
  }

  public IEnumerator FollowersFinishedMatingIE(Follower follower1, Follower follower2)
  {
    Interaction_MatingTent interactionMatingTent = this;
    if (interactionMatingTent.structure.Brain.Data.MatingFailed)
      yield return (object) interactionMatingTent.StartCoroutine((IEnumerator) interactionMatingTent.FailedMatingIE(follower1, follower2));
    else
      yield return (object) interactionMatingTent.StartCoroutine((IEnumerator) interactionMatingTent.SuccessfulMatingIE(follower1, follower2));
    FollowerBrain.SetFollowerCostume(follower1.Spine.Skeleton, follower1.Brain._directInfoAccess, forceUpdate: true);
    FollowerBrain.SetFollowerCostume(follower2.Spine.Skeleton, follower2.Brain._directInfoAccess, forceUpdate: true);
    SimulationManager.UnPause();
    follower1.ShowAllFollowerIcons();
    follower2.ShowAllFollowerIcons();
    interactionMatingTent.TryToInfectOtherFollower(follower1.Brain, follower2.Brain);
    interactionMatingTent.TryToInfectOtherFollower(follower2.Brain, follower1.Brain);
  }

  public IEnumerator SuccessfulMatingIE(Follower follower1, Follower follower2)
  {
    Interaction_MatingTent interactionMatingTent = this;
    if (follower1.Brain._directInfoAccess.IsSnowman && follower2.Brain._directInfoAccess.IsSnowman)
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => this.Structure.Brain.SnowedUnder()));
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().OnConversationNext(interactionMatingTent.gameObject, 6f);
    follower1.Interaction_FollowerInteraction.enabled = false;
    follower2.Interaction_FollowerInteraction.enabled = false;
    follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower1.transform.position = interactionMatingTent.sexyPos.transform.position;
    follower2.transform.position = interactionMatingTent.sexyPos.transform.position;
    Follower followerHoldingEgg = (double) UnityEngine.Random.value < 0.5 ? follower1 : follower2;
    Follower other = (UnityEngine.Object) follower1 == (UnityEngine.Object) followerHoldingEgg ? follower2 : follower1;
    GameObject pos1 = interactionMatingTent.pos1;
    interactionMatingTent.pos1 = (UnityEngine.Object) follower1 == (UnityEngine.Object) followerHoldingEgg ? interactionMatingTent.pos1 : interactionMatingTent.pos2;
    interactionMatingTent.pos2 = (UnityEngine.Object) follower1 == (UnityEngine.Object) followerHoldingEgg ? interactionMatingTent.pos2 : pos1;
    follower1.HideAllFollowerIcons();
    follower2.HideAllFollowerIcons();
    ++DataManager.Instance.EggsProduced;
    if (follower1.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      interactionMatingTent.TurnFollowerIntoZombie(follower2);
    else if (follower2.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      interactionMatingTent.TurnFollowerIntoZombie(follower1);
    if (follower1.Brain.Info.HasTrait(FollowerTrait.TraitType.Mutated) && follower2.Brain.Info.CursedState == Thought.None)
      interactionMatingTent.TurnFollowerIntoMutated(follower2);
    else if (follower2.Brain.Info.HasTrait(FollowerTrait.TraitType.Mutated) && follower1.Brain.Info.CursedState == Thought.None)
      interactionMatingTent.TurnFollowerIntoMutated(follower1);
    yield return (object) new WaitForEndOfFrame();
    interactionMatingTent.normal.gameObject.SetActive(true);
    interactionMatingTent.inprogress.gameObject.SetActive(false);
    interactionMatingTent.transform.localScale = new Vector3(1.05f, 0.95f);
    interactionMatingTent.transform.DOScale(Vector3.one, 0.8f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutElastic, 0.35f, 0.3f);
    other.GoTo(interactionMatingTent.pos2.transform.position, (System.Action) (() => other.FacePosition(followerHoldingEgg.transform.position)));
    bool shocked = (double) UnityEngine.Random.value < 0.10000000149011612;
    if (shocked)
    {
      if (!other.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
      {
        float num = UnityEngine.Random.value;
        other.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, (double) num > 0.5 ? "Egg/egg-walkout-shocked" : "Egg/egg-walkout-incredulous");
        other.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, (double) num > 0.5 ? "Egg/egg-idle-shocked" : "Egg/egg-idle-incredulous");
        other.Brain.CurrentState = (FollowerState) new FollowerState_Slow();
      }
    }
    else
    {
      followerHoldingEgg.AddThought((Thought) UnityEngine.Random.Range(316, 320));
      other.AddThought((Thought) UnityEngine.Random.Range(316, 320));
    }
    if (followerHoldingEgg.Brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, interactionMatingTent.structure.Brain.Data.EggInfo.Golden ? "Egg/zombie-walk-limp-egg-gold" : "Egg/zombie-walk-limp-egg");
      followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, interactionMatingTent.structure.Brain.Data.EggInfo.Golden ? "Egg/zombie-egg-placedown-gold" : "Egg/zombie-egg-placedown");
      followerHoldingEgg.GoTo(interactionMatingTent.pos1.transform.position, (System.Action) (() =>
      {
        followerHoldingEgg.FacePosition(other.transform.position);
        double num = (double) followerHoldingEgg.SetBodyAnimation(this.structure.Brain.Data.EggInfo.Golden ? "Egg/zombie-egg-placedown-gold" : "Egg/zombie-egg-placedown", false);
      }));
    }
    else
    {
      followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, interactionMatingTent.structure.Brain.Data.EggInfo.Golden ? "Egg/run-egg-gold" : "Egg/run-egg");
      followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, interactionMatingTent.structure.Brain.Data.EggInfo.Golden ? "Egg/egg-placedown-gold" : "Egg/egg-placedown");
      followerHoldingEgg.GoTo(interactionMatingTent.pos1.transform.position, (System.Action) (() =>
      {
        followerHoldingEgg.FacePosition(other.transform.position);
        double num = (double) followerHoldingEgg.SetBodyAnimation(this.structure.Brain.Data.EggInfo.Golden ? "Egg/egg-placedown-gold" : "Egg/egg-placedown", false);
      }));
    }
    Skin skin = followerHoldingEgg.Spine.Skeleton.Skin;
    if (DataManager.Instance.PalworldSkins.Contains<string>(interactionMatingTent.structure.Brain.Data.EggInfo.Parent_1_SkinName) && DataManager.Instance.PalworldSkins.Contains<string>(interactionMatingTent.structure.Brain.Data.EggInfo.Parent_2_SkinName) && !DataManager.Instance.FollowerSkinsUnlocked.Contains("PalworldTwo"))
    {
      bool flag = true;
      foreach (Interaction_EggFollower interactionEggFollower in Interaction_EggFollower.Interaction_EggFollowers)
      {
        if (interactionEggFollower.Structure.Brain != null && (interactionEggFollower.Structure.Brain.Data.EggInfo.Parent_1_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_1_ID && interactionEggFollower.Structure.Brain.Data.EggInfo.Parent_2_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_2_ID || interactionEggFollower.Structure.Brain.Data.EggInfo.Parent_1_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_2_ID && interactionEggFollower.Structure.Brain.Data.EggInfo.Parent_2_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_1_ID))
          flag = false;
      }
      foreach (Interaction_Hatchery hatchery in Interaction_Hatchery.Hatcheries)
      {
        if (hatchery.Structure.Brain != null && hatchery.Structure.Brain.Data.EggInfo != null && (hatchery.Structure.Brain.Data.EggInfo.Parent_1_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_1_ID && hatchery.Structure.Brain.Data.EggInfo.Parent_2_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_2_ID || hatchery.Structure.Brain.Data.EggInfo.Parent_1_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_2_ID && hatchery.Structure.Brain.Data.EggInfo.Parent_2_ID == interactionMatingTent.structure.Brain.Data.EggInfo.Parent_1_ID))
          flag = false;
      }
      if (flag)
      {
        skin.AddSkin(followerHoldingEgg.Spine.Skeleton.Data.FindSkin("Eggs/Palworld_Dark"));
        interactionMatingTent.structure.Brain.Data.EggInfo.Special = FollowerSpecialType.Palworld_Dark;
      }
      else
        skin.AddSkin(followerHoldingEgg.Spine.Skeleton.Data.FindSkin(interactionMatingTent.structure.Brain.Data.EggInfo.Golden ? "Eggs/Gold" : "Eggs/Normal"));
    }
    else if (interactionMatingTent.structure.Brain.Data.EggInfo.Rotting)
      skin.AddSkin(followerHoldingEgg.Spine.Skeleton.Data.FindSkin("Eggs/Mutated"));
    else
      skin.AddSkin(followerHoldingEgg.Spine.Skeleton.Data.FindSkin(interactionMatingTent.structure.Brain.Data.EggInfo.Golden ? "Eggs/Gold" : "Eggs/Normal"));
    followerHoldingEgg.Spine.Skeleton.SetSkin(skin);
    yield return (object) new WaitForSeconds(0.75f);
    AudioManager.Instance.PlayOneShot("event:/Stings/mating_success");
    bool waiting = true;
    Spine.AnimationState.TrackEntryEventDelegate OnPlantEgg = (Spine.AnimationState.TrackEntryEventDelegate) null;
    OnPlantEgg = (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
    {
      if (!(e.Data.Name == "plant"))
        return;
      followerHoldingEgg.Spine.AnimationState.Event -= OnPlantEgg;
      this.SpawnEgg(followerHoldingEgg.transform.position + (other.transform.position - followerHoldingEgg.transform.position).normalized * 0.8f);
      waiting = false;
    });
    followerHoldingEgg.Spine.AnimationState.Event += OnPlantEgg;
    while (waiting)
      yield return (object) null;
    if (!shocked && !other.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      double num = (double) other.SetBodyAnimation(interactionMatingTent.GetPostMatingAnim(), false);
      other.AddBodyAnimation("idle", true, 0.0f);
    }
    if (followerHoldingEgg.Brain.HasTrait(FollowerTrait.TraitType.Zombie))
    {
      followerHoldingEgg.Brain.CurrentState.SetStateAnimations(followerHoldingEgg, true);
    }
    else
    {
      followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
      followerHoldingEgg.AddBodyAnimation("idle", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(2.5f);
    followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    if (other.Brain.CurrentState == null)
    {
      other.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
      other.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    }
    other.Brain.CheckChangeState();
    follower1.Brain.CompleteCurrentTask();
    follower2.Brain.CompleteCurrentTask();
    follower1.Interaction_FollowerInteraction.enabled = true;
    follower2.Interaction_FollowerInteraction.enabled = true;
    follower1.OverridingEmotions = false;
    follower2.OverridingEmotions = false;
    follower1.Brain.MakeExhausted();
    follower2.Brain.MakeExhausted();
    if (follower1.Brain.Info.CursedState == Thought.OldAge)
    {
      if ((double) UnityEngine.Random.value < (double) interactionMatingTent.GetElderDeathProbability(follower2))
      {
        follower1.Brain.DiedOfOldAge = true;
        follower1.DieWithAnimation("die", deathNotificationType: NotificationCentre.NotificationType.DiedFromOldAge);
      }
      else
        follower1.Brain.MakeInjured();
    }
    else if (follower1.Brain.Info.CursedState == Thought.Ill)
      follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
    else if (follower1.Brain.Info.CursedState == Thought.Dissenter && (double) UnityEngine.Random.value < 0.5)
      follower2.Brain.MakeDissenter();
    if (follower2.Brain.Info.CursedState == Thought.OldAge)
    {
      if ((double) UnityEngine.Random.value < (double) interactionMatingTent.GetElderDeathProbability(follower1))
      {
        follower2.Brain.DiedOfOldAge = true;
        follower2.DieWithAnimation("die", deathNotificationType: NotificationCentre.NotificationType.DiedFromOldAge);
      }
      else
        follower2.Brain.MakeInjured();
    }
    else if (follower2.Brain.Info.CursedState == Thought.Ill)
      follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
    else if (follower2.Brain.Info.CursedState == Thought.Dissenter && (double) UnityEngine.Random.value < 0.5)
      follower1.Brain.MakeDissenter();
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/MatingSuccessful", follower1.Brain.Info.Name, follower2.Brain.Info.Name);
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator FailedMatingIE(Follower follower1, Follower follower2)
  {
    if (follower1.Brain._directInfoAccess.IsSnowman || follower2.Brain._directInfoAccess.IsSnowman)
      GameManager.GetInstance().WaitForSeconds(3f, (System.Action) (() => this.Structure.Brain.SnowedUnder()));
    yield return (object) new WaitForSeconds(2f);
    this.normal.gameObject.SetActive(true);
    this.inprogress.gameObject.SetActive(false);
    follower1.Interaction_FollowerInteraction.enabled = false;
    follower2.Interaction_FollowerInteraction.enabled = false;
    follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower1.Brain.CurrentState = (FollowerState) new FollowerState_Slow();
    follower2.Brain.CurrentState = (FollowerState) new FollowerState_Slow();
    Follower followerHoldingEgg = (double) UnityEngine.Random.value < 0.5 ? follower1 : follower2;
    Follower other = (UnityEngine.Object) follower1 == (UnityEngine.Object) followerHoldingEgg ? follower2 : follower1;
    other.GoTo(this.pos2.transform.position, (System.Action) (() => other.FacePosition(followerHoldingEgg.transform.position)));
    followerHoldingEgg.GoTo(this.pos1.transform.position, (System.Action) (() => followerHoldingEgg.FacePosition(other.transform.position)));
    follower1.HideAllFollowerIcons();
    follower2.HideAllFollowerIcons();
    yield return (object) new WaitForEndOfFrame();
    followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Egg/mating-failed-walkout-ashamed");
    if (other.Brain.CurrentState == null)
      other.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "Egg/mating-failed-walkout-ashamed");
    AudioManager.Instance.PlayOneShot("event:/Stings/mating_fail");
    yield return (object) new WaitForSeconds(2f);
    follower1.Brain.CompleteCurrentTask();
    follower2.Brain.CompleteCurrentTask();
    follower1.Interaction_FollowerInteraction.enabled = true;
    follower2.Interaction_FollowerInteraction.enabled = true;
    follower1.OverridingEmotions = false;
    follower2.OverridingEmotions = false;
    followerHoldingEgg.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    if (other.Brain.CurrentState == null)
      other.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run");
    follower1.Brain.CheckChangeState();
    follower2.Brain.CheckChangeState();
    follower1.AddThought((Thought) UnityEngine.Random.Range(316, 320));
    follower2.AddThought((Thought) UnityEngine.Random.Range(320, 323));
    follower1.Brain.MakeExhausted();
    follower2.Brain.MakeExhausted();
    if (follower1.Brain.Info.CursedState == Thought.OldAge)
    {
      if ((double) UnityEngine.Random.value < (double) this.GetElderDeathProbability(follower2))
      {
        follower1.Brain.DiedOfOldAge = true;
        follower1.DieWithAnimation("die", deathNotificationType: NotificationCentre.NotificationType.DiedFromOldAge);
      }
      else
        follower1.Brain.MakeInjured();
    }
    else if (follower1.Brain.Info.CursedState == Thought.Ill)
      follower1.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
    else if (follower1.Brain.Info.CursedState == Thought.Dissenter && (double) UnityEngine.Random.value < 0.5)
      follower2.Brain.MakeDissenter();
    if (follower2.Brain.Info.CursedState == Thought.OldAge)
    {
      if ((double) UnityEngine.Random.value < (double) this.GetElderDeathProbability(follower1))
      {
        follower2.Brain.DiedOfOldAge = true;
        follower2.DieWithAnimation("die", deathNotificationType: NotificationCentre.NotificationType.DiedFromOldAge);
      }
      else
        follower2.Brain.MakeInjured();
    }
    else if (follower2.Brain.Info.CursedState == Thought.Ill)
      follower2.Brain.HardSwapToTask((FollowerTask) new FollowerTask_Vomit());
    else if (follower2.Brain.Info.CursedState == Thought.Dissenter && (double) UnityEngine.Random.value < 0.5)
      follower1.Brain.MakeDissenter();
    NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/MatingFailed", follower1.Brain.Info.Name, follower2.Brain.Info.Name);
    GameManager.GetInstance().OnConversationEnd();
  }

  public void TryToInfectOtherFollower(
    FollowerBrain sickFollowerBrain,
    FollowerBrain followerToInfectBrain)
  {
    if (sickFollowerBrain == null || followerToInfectBrain == null || sickFollowerBrain.Info.CursedState != Thought.Ill)
      return;
    followerToInfectBrain.MakeSick();
  }

  public void SpawnEgg(Vector3 spawnPos)
  {
    StructuresData egg = StructuresData.GetInfoByType(StructureBrain.TYPES.EGG_FOLLOWER, 0);
    egg.EggInfo = this.structureBrain.Data.EggInfo;
    if (egg.EggInfo.Golden && egg.EggInfo.Traits != null && egg.EggInfo.Traits.Contains(FollowerTrait.TraitType.ChosenOne))
      egg.CanBecomeRotten = false;
    this.structureBrain.CollectEgg();
    StructureManager.BuildStructure(FollowerLocation.Base, egg, spawnPos, Vector2Int.one, false, (System.Action<GameObject>) (obj =>
    {
      obj.transform.DOPunchScale(obj.transform.localScale * 0.3f, 0.3f);
      obj.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, egg.EggInfo.Rotting, egg.EggInfo.Golden, egg.EggInfo.Special);
      obj.GetComponent<PickUp>().Bounce = false;
      if (!egg.EggInfo.Rotting || !((UnityEngine.Object) PathTileManager.Instance != (UnityEngine.Object) null) || PathTileManager.Instance.GetTileTypeAtPosition(this.transform.position) != StructureBrain.TYPES.NONE)
        return;
      PathTileManager.Instance.SetTile(StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR, this.transform.position);
    }));
  }

  public string GetPostMatingAnim()
  {
    switch (UnityEngine.Random.Range(0, 5))
    {
      case 0:
        return "Egg/egg-afterglow1";
      case 1:
        return "Egg/egg-afterglow2";
      case 2:
        return "Egg/egg-afterglow3";
      case 3:
        return "Egg/egg-afterglow4";
      default:
        return "Egg/egg-afterglow5";
    }
  }

  public float GetElderDeathProbability(Follower anotherFollower)
  {
    return anotherFollower.Brain.Info.CursedState != Thought.Ill ? 0.1f : 0.5f;
  }

  public void TurnFollowerIntoZombie(Follower follower)
  {
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Zombie) || follower.Brain.Info.ID == 99996 && !DataManager.Instance.SozoNoLongerBrainwashed || follower.Brain.Info.CursedState == Thought.Dissenter)
      return;
    follower.Brain.AddTrait(FollowerTrait.TraitType.Zombie, true);
    follower.Brain.CurrentState = (FollowerState) new FollowerState_Zombie();
  }

  public void TurnFollowerIntoMutated(Follower follower)
  {
    if (follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated) || follower.Brain.Info.ID == 99996 && !DataManager.Instance.SozoNoLongerBrainwashed || follower.Brain.Info.CursedState == Thought.Dissenter || FollowerManager.UniqueFollowerIDs.Contains(follower.Brain.Info.ID))
      return;
    follower.Brain.AddTrait(FollowerTrait.TraitType.Mutated, true);
    FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
  }

  public IEnumerator PlayersMatingIE()
  {
    Interaction_MatingTent interactionMatingTent1 = this;
    if ((UnityEngine.Object) interactionMatingTent1.playerInside != (UnityEngine.Object) null)
    {
      interactionMatingTent1.bothPlayersInside = true;
      interactionMatingTent1.Interactable = false;
      interactionMatingTent1.playerFarming.GoToAndStop(interactionMatingTent1.sexyPos.transform.position, GoToCallback: new System.Action(interactionMatingTent1.\u003CPlayersMatingIE\u003Eb__53_0));
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(interactionMatingTent1.sexyPos, 6f);
      AudioManager.Instance.PlayOneShot("event:/player/speak_to_follower_noBookPage");
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.4f, 0.5f);
      yield return (object) new WaitForSeconds(1.5f);
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/just_tentacles");
      interactionMatingTent1.tentaclesSpine.AnimationState.SetAnimation(0, "mating", false);
      interactionMatingTent1.tentaclesSpine.AnimationState.AddAnimation(0, "hidden", true, 0.0f);
      CameraManager.instance.ShakeCameraForDuration(1.4f, 1.4f, 4f);
      interactionMatingTent1.evilParticles.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(0.2f);
      AudioManager.Instance.PlayOneShot("event:/player/goat_player/goat_bleat");
      yield return (object) new WaitForSeconds(3.8f);
      interactionMatingTent1.normal.gameObject.SetActive(true);
      interactionMatingTent1.inprogress.gameObject.SetActive(false);
      foreach (Component player in PlayerFarming.players)
        player.gameObject.SetActive(true);
      interactionMatingTent1.playerFarming.GoToAndStop(interactionMatingTent1.pos1.transform.position, groupAction: true, forcedOtherPosition: new Vector3?(interactionMatingTent1.pos2.transform.position));
      yield return (object) new WaitForSeconds(0.5f);
      foreach (PlayerFarming player in PlayerFarming.players)
        interactionMatingTent1.StartCoroutine((IEnumerator) interactionMatingTent1.Vomit(player));
      yield return (object) new WaitForSeconds(2f);
      DataManager.Instance.PlayersShagged = true;
      interactionMatingTent1.bothPlayersInside = false;
      interactionMatingTent1.playerInside = (PlayerFarming) null;
      interactionMatingTent1.HasChanged = true;
      GameManager.GetInstance().OnConversationEnd();
      yield return (object) new WaitForSeconds(0.5f);
      foreach (PlayerFarming player in PlayerFarming.players)
        player.health.BlackHearts += 2f;
      DataManager.Instance.PLAYER_BLACK_HEARTS += 2f;
      DataManager.Instance.COOP_PLAYER_BLACK_HEARTS += 2f;
    }
    else
    {
      Interaction_MatingTent interactionMatingTent = interactionMatingTent1;
      bool waiting = true;
      interactionMatingTent1.playerInside = interactionMatingTent1.playerFarming;
      interactionMatingTent1.playerFarming.GoToAndStop(interactionMatingTent1.sexyPos.transform.position, IdleOnEnd: true, GoToCallback: (System.Action) (() =>
      {
        interactionMatingTent.playerFarming.gameObject.SetActive(false);
        waiting = false;
        interactionMatingTent.particles.gameObject.SetActive(false);
        interactionMatingTent.evilParticles.gameObject.SetActive(false);
        interactionMatingTent.normal.gameObject.SetActive(false);
        interactionMatingTent.inprogress.gameObject.SetActive(true);
      }));
      while (waiting)
        yield return (object) null;
      PlayerFarming pl = interactionMatingTent1.playerFarming;
      Vector3 lockedPosition = pl.transform.position;
      Vector3 matingTentPosition = interactionMatingTent1.transform.position;
      while (pl.transform.position == lockedPosition)
      {
        if ((!LetterBox.IsPlaying && !MonoSingleton<UIManager>.Instance.MenusBlocked || (UnityEngine.Object) interactionMatingTent1 == (UnityEngine.Object) null) && (InputManager.Gameplay.GetInteract2ButtonDown(pl) || PlayerFarming.Location != FollowerLocation.Base || !CoopManager.CoopActive || (UnityEngine.Object) interactionMatingTent1 == (UnityEngine.Object) null))
        {
          if (InputManager.Gameplay.GetInteract2ButtonDown(pl))
            pl.GoToAndStop(pl.transform.position + Vector3.down, IdleOnEnd: true, GoToCallback: (System.Action) (() =>
            {
              foreach (PlayerFarming player in PlayerFarming.players)
              {
                if ((UnityEngine.Object) player != (UnityEngine.Object) pl && player.state.CURRENT_STATE == StateMachine.State.InActive && UIItemSelectorOverlayController.SelectorOverlays.Count == 0)
                  pl.state.CURRENT_STATE = StateMachine.State.InActive;
              }
            }));
          else if (CoopManager.CoopActive)
            pl.state.CURRENT_STATE = StateMachine.State.Idle;
          if (!((UnityEngine.Object) interactionMatingTent1 == (UnityEngine.Object) null))
          {
            interactionMatingTent1.normal.gameObject.SetActive(true);
            interactionMatingTent1.inprogress.gameObject.SetActive(false);
            break;
          }
          break;
        }
        if (interactionMatingTent1.bothPlayersInside)
        {
          pl.indicator.Deactivate();
          yield break;
        }
        if (matingTentPosition != interactionMatingTent1.transform.position)
        {
          matingTentPosition = interactionMatingTent1.transform.position;
          pl.transform.position = lockedPosition = interactionMatingTent1.sexyPos.transform.position;
        }
        yield return (object) new WaitForEndOfFrame();
        pl.Update();
        if (LetterBox.IsPlaying || MonoSingleton<UIManager>.Instance.MenusBlocked)
        {
          pl.indicator.Deactivate();
        }
        else
        {
          pl.indicator.text.text = "";
          pl.indicator.SecondaryText.text = LocalizationManager.GetTranslation("UI/Exit");
          pl.indicator.Reset();
          pl.indicator.gameObject.SetActive(true);
          pl.indicator.canvasGroup.alpha = 1f;
          pl.indicator.SetActivePosition();
        }
      }
      pl.indicator.gameObject.SetActive(false);
      if (pl.isLamb || CoopManager.CoopActive)
        pl.gameObject.SetActive(true);
      interactionMatingTent1.playerInside = (PlayerFarming) null;
      if ((UnityEngine.Object) interactionMatingTent1 != (UnityEngine.Object) null)
      {
        interactionMatingTent1.normal.gameObject.SetActive(true);
        interactionMatingTent1.inprogress.gameObject.SetActive(false);
      }
    }
  }

  public IEnumerator Vomit(PlayerFarming player)
  {
    Interaction_MatingTent interactionMatingTent = this;
    player.CustomAnimation("eat-react-bad", true);
    yield return (object) new WaitForSeconds(0.233333334f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/hold_back_vom", interactionMatingTent.gameObject);
    yield return (object) new WaitForSeconds(0.733333349f);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/vom", interactionMatingTent.gameObject);
    yield return (object) new WaitForSeconds(1.0333333f);
    player.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__34_0()
  {
    this.isLoadingAssets = false;
    UIMatingMenuController matingMenuController = MonoSingleton<UIManager>.Instance.MatingMenuControllerTemplate.Instantiate<UIMatingMenuController>();
    matingMenuController.Show(this, this.followerSelectEntries);
    matingMenuController.OnFollowersChosen += (System.Action<FollowerInfo, FollowerInfo>) ((f1, f2) =>
    {
      this.follower1 = FollowerManager.FindFollowerByID(f1.ID);
      this.follower2 = FollowerManager.FindFollowerByID(f2.ID);
      if (TimeManager.IsNight && (UnityEngine.Object) this.follower1 != (UnityEngine.Object) null && this.follower1.Brain.CurrentTask != null && this.follower1.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.follower1.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower1.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
        CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, f1.ID);
      if (TimeManager.IsNight && (UnityEngine.Object) this.follower2 != (UnityEngine.Object) null && this.follower2.Brain.CurrentTask != null && this.follower2.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.follower2.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower2.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
        CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, f2.ID);
      this.StartCoroutine((IEnumerator) this.SexyTimeIE());
    });
    matingMenuController.OnHidden = matingMenuController.OnHidden + (System.Action) (() =>
    {
      if (!((UnityEngine.Object) this.follower1 == (UnityEngine.Object) null) && !((UnityEngine.Object) this.follower2 == (UnityEngine.Object) null))
        return;
      GameManager.GetInstance().OnConversationEnd();
      Time.timeScale = 1f;
    });
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__34_1(FollowerInfo f1, FollowerInfo f2)
  {
    this.follower1 = FollowerManager.FindFollowerByID(f1.ID);
    this.follower2 = FollowerManager.FindFollowerByID(f2.ID);
    if (TimeManager.IsNight && (UnityEngine.Object) this.follower1 != (UnityEngine.Object) null && this.follower1.Brain.CurrentTask != null && this.follower1.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.follower1.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower1.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
      CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, f1.ID);
    if (TimeManager.IsNight && (UnityEngine.Object) this.follower2 != (UnityEngine.Object) null && this.follower2.Brain.CurrentTask != null && this.follower2.Brain.CurrentTask.State == FollowerTaskState.Doing && (this.follower2.Brain.CurrentTaskType == FollowerTaskType.Sleep || this.follower2.Brain.CurrentTaskType == FollowerTaskType.SleepBedRest))
      CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, f2.ID);
    this.StartCoroutine((IEnumerator) this.SexyTimeIE());
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__34_2()
  {
    if (!((UnityEngine.Object) this.follower1 == (UnityEngine.Object) null) && !((UnityEngine.Object) this.follower2 == (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().OnConversationEnd();
    Time.timeScale = 1f;
  }

  [CompilerGenerated]
  public void \u003CSexyTimeIE\u003Eb__35_0()
  {
    this.follower1.FacePosition(this.follower2.transform.position);
  }

  [CompilerGenerated]
  public void \u003CSexyTimeIE\u003Eb__35_1()
  {
    this.follower2.FacePosition(this.follower1.transform.position);
  }

  [CompilerGenerated]
  public void \u003CSexyTimeIE\u003Eb__35_4(List<FollowerTrait.TraitType> traits)
  {
    this.structureBrain.Data.EggInfo.Traits = traits;
  }

  [CompilerGenerated]
  public void \u003CPlayersMatingIE\u003Eb__53_0()
  {
    this.playerFarming.gameObject.SetActive(false);
  }

  public enum FollowerResponse
  {
    NotInterested = 0,
    Enemies = 1,
    Disgusted = 2,
    Obliged = 100, // 0x00000064
    Happy = 101, // 0x00000065
    InLove = 102, // 0x00000066
    Other = 200, // 0x000000C8
    Zombie = 201, // 0x000000C9
  }
}
