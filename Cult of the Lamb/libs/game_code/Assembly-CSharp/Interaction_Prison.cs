// Decompiled with JetBrains decompiler
// Type: Interaction_Prison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Lamb.UI.PrisonerMenu;
using MMTools;
using src.Extensions;
using src.UI.Menus;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Prison : Interaction
{
  public Structure Structure;
  public Structures_Prison _StructureInfo;
  [SerializeField]
  public GameObject playerPosition;
  public const float ACQUIRE_TRAIT_CHANCE = 0.2f;
  public const int MIN_PRISON_DAYS_FOR_TRAIT = 2;
  public bool isReleasing;
  public EventInstance LoopInstance;
  public float count;
  public UIPrisonerMenuController prisonerMenu;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Prison structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Prison;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public int PrisonerID => this.StructureInfo.FollowerID;

  public void Awake() => this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);

  public void Start()
  {
    this.ActivateDistance = 2f;
    this.UpdateLocalisation();
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(this.StructureInfo.FollowerID))
      return;
    this.StructureInfo.FollowerID = -1;
  }

  public override void GetLabel()
  {
    this.Label = this.StructureInfo == null || FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID) == null && this.StructureInfo.FollowerID != -1 ? "" : ScriptLocalization.Structures.PRISON;
  }

  public override void GetSecondaryLabel()
  {
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
  }

  public IEnumerator ReEducate()
  {
    Interaction_Prison interactionPrison = this;
    yield return (object) new WaitForSeconds(0.25f);
    interactionPrison.count = 0.0f;
    interactionPrison.playerFarming.GoToAndStop(interactionPrison.playerPosition, interactionPrison.gameObject, true);
    while (interactionPrison.playerFarming.GoToAndStopping)
    {
      if ((double) interactionPrison.count > 60.0)
        interactionPrison.playerFarming.EndGoToAndStop();
      ++interactionPrison.count;
      yield return (object) null;
    }
    Follower prisoner = FollowerManager.FindFollowerByID(interactionPrison.StructureInfo.FollowerID);
    if ((UnityEngine.Object) prisoner == (UnityEngine.Object) null)
    {
      GameManager.GetInstance().OnConversationEnd();
      PlayerFarming.SetStateForAllPlayers();
      yield return (object) new WaitForSeconds(0.5f);
    }
    else
    {
      interactionPrison.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      GameManager.GetInstance().OnConversationNext(interactionPrison.gameObject, 8f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_acknowledge", interactionPrison.gameObject);
      AudioManager.Instance.PlayOneShot("event:/sermon/book_pickup", interactionPrison.gameObject);
      interactionPrison.playerFarming.Spine.state.SetAnimation(0, "build", true);
      yield return (object) new WaitForSeconds(0.3f);
      if (interactionPrison._playerFarming.isLamb)
        AudioManager.Instance.PlayOneShot("event:/sermon/sermon_speech_bubble", interactionPrison.playerFarming.gameObject);
      else
        AudioManager.Instance.PlayOneShot("event:/sermon/goat_sermon_speech_bubble", interactionPrison.playerFarming.gameObject);
      interactionPrison.LoopInstance = AudioManager.Instance.CreateLoop("event:/sermon/preach_loop", interactionPrison.gameObject);
      prisoner.Spine.state.SetAnimation(0, "Prison/stocks-reeducate", false);
      double num1 = (double) prisoner.SetBodyAnimation("Prison/stocks-reeducate", false);
      prisoner.AddBodyAnimation("Prison/stocks", false, 0.0f);
      yield return (object) new WaitForSeconds(2f);
      prisoner.SetFaceAnimation("Reactions/react-brainwashed", false);
      AudioManager.Instance.StopLoop(interactionPrison.LoopInstance);
      CameraManager.shakeCamera(1f, (float) UnityEngine.Random.Range(0, 360));
      AudioManager.Instance.PlayOneShot("event:/sermon/end_sermon", interactionPrison.gameObject);
      prisoner.Spine.state.AddAnimation(0, "Prison/stocks", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", interactionPrison.gameObject);
      AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", interactionPrison.gameObject.transform.position);
      BiomeConstants.Instance.EmitHeartPickUpVFX(prisoner.gameObject.transform.position, 0.0f, "black", "burst_big");
      int cursedState = (int) prisoner.Brain.Info.CursedState;
      interactionPrison.structureBrain.Reeducate(prisoner.Brain);
      prisoner.Brain.Stats.ReeducatedAction = true;
      interactionPrison.ReeducatedThoughts(prisoner);
      interactionPrison.ReeducatedThoughts(prisoner);
      yield return (object) new WaitForSeconds(1f);
      yield return (object) new WaitForSeconds(1f);
      prisoner.SetFaceAnimation("Reactions/react-loved", false);
      AudioManager.Instance.PlayOneShot("event:/sermon/book_put_down", interactionPrison.gameObject);
      if (prisoner.Brain.Info.ID == 99996 && prisoner.Brain.Info.CursedState != Thought.Dissenter && !DataManager.Instance.SozoNoLongerBrainwashed)
      {
        prisoner.Brain.Info.Name = LocalizationManager.GetTranslation("NAMES/SozoOld");
        prisoner.HideAllFollowerIcons();
        SimulationManager.Pause();
        List<ConversationEntry> c = new List<ConversationEntry>()
        {
          new ConversationEntry(prisoner.gameObject, "Conversation_NPC/SozoFollower/NoLongerBrainwashed/0"),
          new ConversationEntry(prisoner.gameObject, "Conversation_NPC/SozoFollower/NoLongerBrainwashed/1"),
          new ConversationEntry(prisoner.gameObject, "Conversation_NPC/SozoFollower/NoLongerBrainwashed/2"),
          new ConversationEntry(prisoner.gameObject, "Conversation_NPC/SozoFollower/NoLongerBrainwashed/3")
        };
        foreach (ConversationEntry conversationEntry in c)
        {
          conversationEntry.SetZoom = true;
          conversationEntry.Zoom = 5f;
          conversationEntry.Animation = "Conversations/talk-nice1";
          conversationEntry.DefaultAnimation = "idle";
          conversationEntry.TermName = LocalizationManager.GetTranslation("NAMES/SozoOld");
        }
        FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
        prisoner.Brain.HardSwapToTask((FollowerTask) nextTask);
        bool wait = true;
        nextTask.GoToAndStop(prisoner, interactionPrison.transform.position + Vector3.down / 2f + Vector3.left, (System.Action) (() =>
        {
          wait = false;
          double num2 = (double) prisoner.SetBodyAnimation("idle", true);
          prisoner.FacePosition(this.playerFarming.transform.position);
          this.playerFarming.GoToAndStop(this.transform.position + Vector3.down / 2f + Vector3.right, prisoner.gameObject);
        }));
        while (wait)
          yield return (object) null;
        double num3 = (double) prisoner.SetBodyAnimation("sozo-sober", false);
        prisoner.AddBodyAnimation("idle", true, 0.0f);
        prisoner.Spine.AnimationState.Event += (Spine.AnimationState.TrackEntryEventDelegate) ((trackEntry, e) =>
        {
          if (e.Data.Name == "SozoGetsSober")
          {
            DataManager.Instance.SozoNoLongerBrainwashed = true;
            prisoner.Brain._directInfoAccess.SozoBrainshed = false;
            prisoner.Brain.Info.CursedState = Thought.OldAge;
            prisoner.Brain.AddThought(Thought.OldAge, true);
            DataManager.Instance.Followers_Elderly_IDs.Add(prisoner.Brain.Info.ID);
            prisoner.SetEmotionAnimation();
            FollowerBrain.SetFollowerCostume(prisoner.Spine.Skeleton, prisoner.Brain.Info.XPLevel, prisoner.Brain.Info.SkinName, prisoner.Brain.Info.SkinColour, FollowerOutfitType.Old, prisoner.Brain.Info.Hat, prisoner.Brain.Info.Clothing, prisoner.Brain.Info.Customisation, FollowerSpecialType.SozoOld, prisoner.Brain.Info.Necklace, prisoner.Brain.Info.ClothingVariant);
          }
          else
          {
            if (!(e.Data.Name == "Audio/fol_sozo_lose_mushroom"))
              return;
            AudioManager.Instance.PlayOneShot("event:/dialogue/followers/npc/fol_sozo_lose_mushroom", prisoner.gameObject);
          }
        });
        yield return (object) new WaitForSeconds(8f);
        interactionPrison.playerFarming.LookToObject = prisoner.gameObject;
        foreach (ConversationEntry conversationEntry in c)
          conversationEntry.CharacterName = prisoner.Brain.Info.Name;
        MMConversation.Play(new ConversationObject(c, (List<MMTools.Response>) null, (System.Action) null), false);
        yield return (object) null;
        while (MMConversation.isPlaying)
          yield return (object) null;
        DataManager.Instance.SozoUnlockedMushroomSkin = true;
        wait = true;
        FollowerSkinCustomTarget.Create(prisoner.transform.position - Vector3.back * 0.5f, interactionPrison.playerFarming.transform.position, 1f, "Mushroom", (System.Action) (() => wait = false));
        while (wait)
          yield return (object) null;
        GameManager.GetInstance().OnConversationNew();
        AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("SOZO_QUEST"));
        SimulationManager.UnPause();
        prisoner.Brain.MakeOld();
        prisoner.ShowAllFollowerIcons();
        DataManager.Instance.Followers_Imprisoned_IDs.Remove(prisoner.Brain.Info.ID);
        interactionPrison.StructureInfo.FollowerID = -1;
        prisoner.Brain.CompleteCurrentTask();
        prisoner.Interaction_FollowerInteraction.Interactable = true;
        prisoner.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
        GameManager.GetInstance().OnConversationEnd();
        c = (List<ConversationEntry>) null;
      }
      else
      {
        GameManager.GetInstance().OnConversationEnd();
        PlayerFarming.SetStateForAllPlayers();
        yield return (object) new WaitForSeconds(0.5f);
      }
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    if (this.StructureInfo.FollowerID != -1)
    {
      this.prisonerMenu = MonoSingleton<UIManager>.Instance.PrisonerMenuTemplate.Instantiate<UIPrisonerMenuController>();
      this.prisonerMenu.Show(FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID), this.StructureInfo);
      this.prisonerMenu.OnFollowerReleased += (System.Action<FollowerInfo>) (followerInfo => this.ReleaseFollower());
      this.prisonerMenu.OnReEducate += (System.Action<FollowerInfo>) (followerInfo =>
      {
        Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
        if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null) || followerById.Brain.Stats.ReeducatedAction)
          return;
        this.prisonerMenu.Hide();
        this.StartCoroutine((IEnumerator) this.ReEducate());
      });
      UIPrisonerMenuController prisonerMenu1 = this.prisonerMenu;
      prisonerMenu1.OnHidden = prisonerMenu1.OnHidden + (System.Action) (() => Time.timeScale = 1f);
      UIPrisonerMenuController prisonerMenu2 = this.prisonerMenu;
      prisonerMenu2.OnCancel = prisonerMenu2.OnCancel + (System.Action) (() =>
      {
        if (this.isReleasing)
          return;
        GameManager.GetInstance().OnConversationEnd();
      });
    }
    else
    {
      List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
      {
        if (follower.Brain._directInfoAccess.IsSnowman)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
        else if (follower.Brain._directInfoAccess.CursedState == Thought.OldAge)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerCursedStateAvailability(follower)));
        else
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
      }
      followerSelectEntries.Sort((Comparison<FollowerSelectEntry>) ((a, b) => b.FollowerInfo.HasThought(Thought.Dissenter).CompareTo(a.FollowerInfo.HasThought(Thought.Dissenter))));
      UIPrisonMenuController prisonMenuController = MonoSingleton<UIManager>.Instance.PrisonMenuTemplate.Instantiate<UIPrisonMenuController>();
      prisonMenuController.Show(followerSelectEntries, followerSelectionType: UpgradeSystem.Type.Building_Prison, showDefaultInfoCard: true);
      prisonMenuController.OnFollowerSelected = prisonMenuController.OnFollowerSelected + (System.Action<FollowerInfo>) (followerInfo =>
      {
        this.ImprisonFollower(FollowerBrain.GetOrCreateBrain(followerInfo));
        BiomeConstants.Instance?.EmitSmokeExplosionVFX(this.gameObject.transform.position);
        AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_acknowledge", this.gameObject);
      });
      prisonMenuController.OnHidden = prisonMenuController.OnHidden + (System.Action) (() => Time.timeScale = 1f);
      prisonMenuController.OnCancel = prisonMenuController.OnCancel + (System.Action) (() =>
      {
        if (this.isReleasing)
          return;
        GameManager.GetInstance().OnConversationEnd();
      });
    }
  }

  public void ImprisonFollower(FollowerBrain follower)
  {
    this.StructureInfo.FollowerID = follower.Info.ID;
    this.StructureInfo.FollowerImprisonedTimestamp = TimeManager.TotalElapsedGameTime;
    this.StructureInfo.FollowerImprisonedFaith = follower.Stats.Reeducation;
    this.StartCoroutine((IEnumerator) this.ImprisonFollowerIE(follower));
  }

  public void ReleaseFollower()
  {
    this.isReleasing = true;
    this.StartCoroutine((IEnumerator) this.ReleaseFollowerIE());
  }

  public IEnumerator ReleaseFollowerIE()
  {
    yield return (object) new WaitForEndOfFrame();
    if (this.StructureInfo != null)
    {
      FollowerInfo info = FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID);
      if (info != null)
      {
        FollowerBrain brain = FollowerBrain.GetOrCreateBrain(info);
        if (brain != null)
        {
          Follower f = FollowerManager.FindFollowerByID(this.StructureInfo.FollowerID);
          if (!((UnityEngine.Object) f == (UnityEngine.Object) null))
          {
            GameManager.GetInstance().OnConversationNew();
            GameManager.GetInstance().OnConversationNext(f.gameObject, 5f);
            yield return (object) new WaitForSeconds(1.2f);
            AudioManager.Instance.PlayOneShot("event:/followers/imprison", f.transform.position);
            DataManager.Instance.Followers_Imprisoned_IDs.Remove(brain.Info.ID);
            this.StructureInfo.FollowerID = -1;
            brain.CompleteCurrentTask();
            f.Interaction_FollowerInteraction.Interactable = true;
            f.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
            GameManager.GetInstance().OnConversationEnd();
            this.isReleasing = false;
            yield return (object) new WaitForSeconds(1f);
            if ((double) UnityEngine.Random.value < 0.20000000298023224 && TimeManager.CurrentDay >= info.ImprisonedDay + 2 && !brain.HasTrait(FollowerTrait.TraitType.CriminalEvangelizing) && !brain.HasTrait(FollowerTrait.TraitType.CriminalHardened) && !brain.HasTrait(FollowerTrait.TraitType.CriminalReformed) && !brain.HasTrait(FollowerTrait.TraitType.CriminalScarred))
            {
              float num = UnityEngine.Random.value;
              if ((double) num < 0.33)
                brain.AddTrait(FollowerTrait.TraitType.CriminalHardened, true);
              else if ((double) num < 0.66)
              {
                if (brain.Info.CursedState != Thought.Dissenter)
                {
                  brain.RemoveTrait(FollowerTrait.TraitType.Lazy);
                  brain.AddTrait(FollowerTrait.TraitType.CriminalReformed, true);
                }
              }
              else
              {
                brain.AddTrait(FollowerTrait.TraitType.CriminalScarred, true);
                this.AddDistressedThought(brain);
              }
            }
          }
        }
      }
    }
  }

  public void AddDistressedThought(FollowerBrain followerBrain)
  {
    Thought randomThoughtFromSet = FollowerThoughts.GetRandomThoughtFromSet(Thought.Distressed_0, Thought.Distressed_1, Thought.Distressed_2, Thought.Distressed_3);
    if (followerBrain.HasThought(randomThoughtFromSet))
      return;
    followerBrain.AddThought(randomThoughtFromSet);
  }

  public IEnumerator ImprisonFollowerIE(FollowerBrain follower)
  {
    Interaction_Prison interactionPrison = this;
    yield return (object) new WaitForEndOfFrame();
    Follower f = FollowerManager.FindFollowerByID(follower.Info.ID);
    follower.CompleteCurrentTask();
    follower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    interactionPrison.StructureInfo.FollowerID = follower.Info.ID;
    f.transform.position = interactionPrison.GetComponent<Prison>().PrisonerLocation.position;
    interactionPrison.ImprisonedThoughts(f);
    AudioManager.Instance.PlayOneShot("event:/followers/imprison", f.transform.position);
    yield return (object) new WaitForEndOfFrame();
    follower.CompleteCurrentTask();
    follower.HardSwapToTask((FollowerTask) new FollowerTask_Imprisoned(interactionPrison.StructureInfo.ID));
    int imprisonedDay = follower._directInfoAccess.ImprisonedDay;
    follower._directInfoAccess.ImprisonedDay = TimeManager.CurrentDay;
    if (!DataManager.Instance.Followers_Imprisoned_IDs.Contains(follower.Info.ID))
      DataManager.Instance.Followers_Imprisoned_IDs.Add(follower.Info.ID);
    if (follower.Info.CursedState != Thought.Dissenter)
    {
      bool flag1 = false;
      foreach (ObjectivesData objective in DataManager.Instance.Objectives)
      {
        if (objective.Type == Objectives.TYPES.CUSTOM && ((Objectives_Custom) objective).CustomQuestType == Objectives.CustomQuestTypes.SendFollowerToPrison && ((Objectives_Custom) objective).TargetFollowerID == follower.Info.ID)
        {
          flag1 = true;
          break;
        }
      }
      bool flag2 = true;
      if (follower.HasTrait(FollowerTrait.TraitType.Spy))
      {
        flag2 = false;
      }
      else
      {
        foreach (Follower follower1 in Follower.Followers)
        {
          if (follower1.Brain._directInfoAccess.MurderedBy == follower.Info.ID)
          {
            flag2 = false;
            break;
          }
        }
      }
      if (!flag1 & flag2)
      {
        if (DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Disciplinarian))
          CultFaithManager.AddThought(Thought.Cult_Imprison_Trait);
        else
          CultFaithManager.AddThought(Thought.Cult_Imprison);
      }
    }
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.SendFollowerToPrison, follower.Info.ID);
    GameManager.GetInstance().OnConversationEnd();
    if (follower.Info.ID == 99996 && follower._directInfoAccess.SozoBrainshed && follower.Info.CursedState == Thought.Dissenter && TimeManager.CurrentDay - imprisonedDay >= 1)
    {
      interactionPrison.playerFarming.GoToAndStop(interactionPrison.playerPosition, f.gameObject);
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(interactionPrison.gameObject, "Conversation_NPC/SozoFollower/Imprisoned/0")
      };
      foreach (ConversationEntry conversationEntry in Entries)
      {
        conversationEntry.CharacterName = ScriptLocalization.NAMES.Sozo;
        conversationEntry.DefaultAnimation = "Prison/stocks";
      }
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) (() => f.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Prison/stocks"))));
    }
  }

  public void ImprisonedThoughts(Follower prisoner)
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Location == prisoner.Brain.Location && allBrain != prisoner.Brain)
      {
        if ((double) allBrain.Stats.Reeducation > 0.0)
        {
          if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
            allBrain.AddThought(Thought.DissenterImprisonedSleeping);
          else if (allBrain.HasTrait(FollowerTrait.TraitType.Libertarian))
            allBrain.AddThought(Thought.ImprisonedLibertarian);
          else
            allBrain.AddThought(Thought.DissenterImprisoned);
        }
        else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
          allBrain.AddThought(Thought.InnocentImprisonedSleeping);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.Disciplinarian))
          allBrain.AddThought(Thought.InnocentImprisonedDisciplinarian);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.Libertarian))
          allBrain.AddThought(Thought.ImprisonedLibertarian);
        else
          allBrain.AddThought(Thought.InnocentImprisoned);
      }
    }
  }

  public void ReeducatedThoughts(Follower prisoner)
  {
    if ((double) prisoner.Brain.Stats.Reeducation > 0.0)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != prisoner.Brain)
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.Cynical))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedSleepingCynicalTrait);
            else
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedCynicalTrait);
          }
          else if (allBrain.HasTrait(FollowerTrait.TraitType.Gullible))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedSleepingGullibleTrait);
            else
              allBrain.AddThought(Thought.DissenterCultMemberReeducatedGullibleTrait);
          }
          else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
            allBrain.AddThought(Thought.DissenterCultMemberReeducatedSleeping);
          else
            allBrain.AddThought(Thought.DissenterCultMemberReeducated);
        }
      }
    }
    else
    {
      JudgementMeter.ShowModify(1);
      prisoner.Brain.AddThought(Thought.InnocentReeducated);
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
      {
        if (allBrain != prisoner.Brain)
        {
          if (allBrain.HasTrait(FollowerTrait.TraitType.Cynical))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedCynicalTraitSleeping);
            else
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedCynicalTrait);
          }
          else if (allBrain.HasTrait(FollowerTrait.TraitType.Gullible))
          {
            if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedGullibleTraitSleeping);
            else
              allBrain.AddThought(Thought.InnocentCultMemberReeducatedGullibleTrait);
          }
          else if (allBrain.CurrentTaskType == FollowerTaskType.Sleep)
            allBrain.AddThought(Thought.InnocentCultMemberReeducatedSleeping);
          else
            allBrain.AddThought(Thought.InnocentCultMemberReeducated);
        }
      }
    }
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    AudioManager.Instance.StopLoop(this.LoopInstance);
    this.isReleasing = false;
  }

  public IEnumerator GiveItemsRoutine(InventoryItem.ITEM_TYPE itemType, int quantity)
  {
    Interaction_Prison interactionPrison = this;
    for (int i = 0; i < Mathf.Max(quantity, 10); ++i)
    {
      ResourceCustomTarget.Create(interactionPrison.gameObject, interactionPrison.playerFarming.transform.position, itemType, (System.Action) null);
      yield return (object) new WaitForSeconds(0.15f);
    }
    Inventory.ChangeItemQuantity(itemType, -quantity);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_0(FollowerInfo followerInfo) => this.ReleaseFollower();

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_1(FollowerInfo followerInfo)
  {
    Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
    if (!((UnityEngine.Object) followerById != (UnityEngine.Object) null) || followerById.Brain.Stats.ReeducatedAction)
      return;
    this.prisonerMenu.Hide();
    this.StartCoroutine((IEnumerator) this.ReEducate());
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_3()
  {
    if (this.isReleasing)
      return;
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_5(FollowerInfo followerInfo)
  {
    this.ImprisonFollower(FollowerBrain.GetOrCreateBrain(followerInfo));
    BiomeConstants.Instance?.EmitSmokeExplosionVFX(this.gameObject.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/general_acknowledge", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__23_7()
  {
    if (this.isReleasing)
      return;
    GameManager.GetInstance().OnConversationEnd();
  }
}
