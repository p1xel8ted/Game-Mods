// Decompiled with JetBrains decompiler
// Type: Interaction_Bed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using MMTools;
using src.Extensions;
using src.UI.Menus.ShareHouseMenu;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_Bed : Interaction
{
  public Structure Structure;
  [SerializeField]
  public interaction_CollapsedBed collapsedBed;
  [SerializeField]
  public GameObject uncollapsedBed;
  [SerializeField]
  public Dwelling dwelling;
  [SerializeField]
  public GameObject leaderDwelling;
  public Structures_Bed _StructureInfo;
  public Vector3 previousPosition;
  public SpriteXPBar XPBar;
  public bool cacheCollapse;
  public bool Activated;
  public FollowerInfo OldFollower;
  public int _lastSoulCount = -1;
  public bool _hasUnlockAvailable;
  public const float SleepThreshold = 80f;
  public Follower follower;
  public bool Activating;

  public virtual Structures_Bed StructureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Bed;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public bool playerCanSleep
  {
    get
    {
      if (TimeManager.CurrentPhase == DayPhase.Night && (double) TimeManager.CurrentPhaseProgress < 0.89999997615814209)
        return true;
      return TimeManager.CurrentPhase == DayPhase.Dusk && (double) TimeManager.CurrentPhaseProgress > 0.89999997615814209;
    }
  }

  public void Collapse()
  {
    this.StructureBrain.Collapse(true, true, false);
    this.UpdateBed();
  }

  public void Awake()
  {
  }

  public void Start() => this.dwelling = this.GetComponent<Dwelling>();

  public override void Update()
  {
    base.Update();
    if (this.StructureBrain == null || this.StructureBrain.Data == null || !this.StructureBrain.Data.ClaimedByPlayer || (double) DataManager.Instance.SurvivalMode_Sleep > 80.0 || this.Interactable)
      return;
    this.HasChanged = true;
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    FollowerBrain.OnDwellingAssigned += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingAssignedAwaitClaim += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared += new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    else
      this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    this.OldFollower = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
    if (this.StructureBrain.Data.Type == global::StructureBrain.TYPES.BED_3)
    {
      this.HasSecondaryInteraction = true;
      this.UpdateBar();
    }
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.OnStructureMoved);
    this.StructureBrain.OnBedCollapsed += new Structures_Bed.BedEvent(this.UpdateBed);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    Structures_Bed structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
    this.UpdateBed();
    this.UpdateFreezing();
    if (this.StructureBrain.Data.ClaimedByPlayer && (UnityEngine.Object) this.leaderDwelling != (UnityEngine.Object) null)
    {
      if ((bool) (UnityEngine.Object) this.uncollapsedBed)
        this.uncollapsedBed.gameObject.SetActive(false);
      if ((bool) (UnityEngine.Object) this.collapsedBed)
        this.collapsedBed.gameObject.SetActive(false);
      if ((bool) (UnityEngine.Object) this.XPBar)
        this.XPBar.gameObject.SetActive(false);
      this.leaderDwelling.gameObject.SetActive(this.StructureBrain.Data.ClaimedByPlayer);
    }
    if (DataManager.Instance.SurvivalModeActive && DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.PlayerSleep))
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.PlayerSleep);
    if (this.StructureBrain.Data.Type != global::StructureBrain.TYPES.LEADER_TENT)
      return;
    this.StructureBrain.Data.ClaimedByPlayer = true;
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XPBar.gameObject.SetActive(true);
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
  }

  public void UpdateFreezing()
  {
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.StructureBrain != null)
      this.StructureBrain.OnBedCollapsed -= new Structures_Bed.BedEvent(this.UpdateBed);
    if (this.StructureBrain != null)
    {
      Structures_Bed structureBrain = this.StructureBrain;
      structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
    }
    FollowerBrain.OnDwellingAssigned -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingAssignedAwaitClaim -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
    FollowerBrain.OnDwellingCleared -= new FollowerBrain.DwellingAssignmentChanged(this.OnDwellingAssignmentChanged);
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.OnStructureMoved);
  }

  public void OnStructureMoved(StructuresData structure)
  {
    if (this._StructureInfo == null || structure == null || structure.ID != this._StructureInfo.Data.ID)
      return;
    this.UpdateFreezing();
  }

  public void UpdateBed()
  {
    if (this.StructureBrain.Data.Type == global::StructureBrain.TYPES.BED_3)
      return;
    if (this.cacheCollapse != this.StructureBrain.IsCollapsed)
    {
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
      AudioManager.Instance.PlayOneShot("event:/building/finished_fabric", this.transform.position);
      this.cacheCollapse = this.StructureBrain.IsCollapsed;
    }
    if ((bool) (UnityEngine.Object) this.collapsedBed)
      this.collapsedBed.gameObject.SetActive(this.StructureBrain.IsCollapsed);
    if ((bool) (UnityEngine.Object) this.uncollapsedBed)
      this.uncollapsedBed.gameObject.SetActive(!this.StructureBrain.IsCollapsed);
    this.enabled = !this.StructureBrain.IsCollapsed;
  }

  public void OnDwellingAssignmentChanged(int followerID, Dwelling.DwellingAndSlot d)
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.Structure.Structure_Info == null || d.ID != this.Structure.Structure_Info.ID)
      return;
    this.OldFollower = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
  }

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
      return;
    if (this.StructureBrain.Data.ClaimedByPlayer)
    {
      this.ThirdLabel = "";
      this.SecondaryLabel = "";
      if ((double) DataManager.Instance.SurvivalMode_Sleep <= 80.0 || this.playerCanSleep)
      {
        this.Interactable = true;
        this.Label = LocalizationManager.GetTranslation("Interactions/SleepTillMorning");
      }
      else
      {
        this.Interactable = false;
        this.Label = LocalizationManager.GetTranslation("Interactions/NotTired");
      }
    }
    else
    {
      if (this.OldFollower != null && this.StructureBrain.Data.FollowerID != -1)
      {
        if (LocalizeIntegration.IsArabic())
          this.Label = " | " + ScriptLocalization.Interactions_Bed.Re_Assign;
        else
          this.Label = ScriptLocalization.Interactions_Bed.Re_Assign + " | ";
        this.SecondaryLabel = " " + ScriptLocalization.Interactions_Bed.LivesHere.Replace("{0}", this.OldFollower.Name);
      }
      else
      {
        if (LocalizeIntegration.IsArabic())
          this.Label = " | " + ScriptLocalization.Interactions_Bed.Assign;
        else
          this.Label = ScriptLocalization.Interactions_Bed.Assign + " | ";
        this.SecondaryLabel = " " + ScriptLocalization.Interactions_Bed.Unoccupied;
      }
      if (this.StructureBrain.Data.Type == global::StructureBrain.TYPES.BED_3)
      {
        if (this.StructureBrain.SoulCount != this._lastSoulCount)
        {
          this._lastSoulCount = this.StructureBrain.SoulCount;
          this._hasUnlockAvailable = GameManager.HasUnlockAvailable();
        }
        this.XPBar.gameObject.SetActive(true);
        if (this.StructureBrain.SoulCount > 0)
        {
          string str = (this._hasUnlockAvailable ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
          string receiveDevotion = ScriptLocalization.Interactions.ReceiveDevotion;
          if (LocalizeIntegration.IsArabic())
            this.SecondaryLabel = $"{receiveDevotion} {str} {LocalizeIntegration.ReverseText(this.StructureBrain.SoulMax.ToString())} / {LocalizeIntegration.ReverseText(this._StructureInfo.SoulCount.ToString())}{StaticColors.GreyColorHex}";
          else
            this.SecondaryLabel = $"{receiveDevotion} {str} {this._StructureInfo.SoulCount.ToString()}{StaticColors.GreyColorHex} / {this.StructureBrain.SoulMax.ToString()}";
        }
      }
      else if (this.StructureBrain.Data.Type == global::StructureBrain.TYPES.SHARED_HOUSE)
      {
        this.Label = LocalizationManager.GetTranslation("Interactions/View");
        this.SecondaryLabel = string.Empty;
      }
      if (!DataManager.Instance.SurvivalModeActive || this.StructureBrain.Data.Type == global::StructureBrain.TYPES.SHARED_HOUSE)
        return;
      bool flag = false;
      foreach (global::StructureBrain structureBrain in StructureManager.GetAllStructuresOfType<Structures_Bed>())
      {
        if (structureBrain.Data.ClaimedByPlayer)
        {
          flag = true;
          break;
        }
      }
      this.ThirdInteractable = true;
      this.HasThirdInteraction = true;
      if (!this.StructureBrain.Data.ClaimedByPlayer && !flag)
      {
        this.ThirdLabel = ScriptLocalization.Interactions.Claim;
      }
      else
      {
        this.ThirdInteractable = false;
        this.HasThirdInteraction = false;
      }
    }
  }

  public void CloseAndSpeak(string ConversationEntryTerm)
  {
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(this.gameObject, "FollowerInteractions/" + ConversationEntryTerm, "idle")
    }, (List<MMTools.Response>) null, (System.Action) null));
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.StructureBrain.Data.ClaimedByPlayer)
    {
      base.OnInteract(state);
      if ((double) DataManager.Instance.SurvivalMode_Sleep <= 80.0 || this.playerCanSleep)
        this.StartCoroutine((IEnumerator) this.SleepTillMorning());
      else
        this.playerFarming.GoToAndStop(this.transform.position + new Vector3(0.0f, -0.5f), this.gameObject, GoToCallback: (System.Action) (() => this.CloseAndSpeak("NotTired")));
    }
    else
    {
      if (this.Activated)
        return;
      this.Activated = true;
      GameManager.GetInstance().OnConversationNew();
      Time.timeScale = 0.0f;
      HUD_Manager.Instance.Hide(false, 0);
      if (this.Structure.Type == global::StructureBrain.TYPES.SHARED_HOUSE)
      {
        UIShareHouseMenuController houseMenuController = MonoSingleton<UIManager>.Instance.ShareHouseMenuTemplate.Instantiate<UIShareHouseMenuController>();
        houseMenuController.Show(this);
        houseMenuController.OnHidden = houseMenuController.OnHidden + (System.Action) (() =>
        {
          this.OnHidden();
          this.HasChanged = true;
        });
      }
      else
      {
        UIFollowerSelectMenuController selectMenuController = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
        selectMenuController.VotingType = TwitchVoting.VotingType.BED;
        selectMenuController.Show(this.GetFollowerSelectEntries(), false, UpgradeSystem.Type.Count, true, true, true, false, true);
        selectMenuController.OnFollowerSelected = selectMenuController.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
        selectMenuController.OnHidden = selectMenuController.OnHidden + (System.Action) (() =>
        {
          this.OnHidden();
          this.HasChanged = true;
        });
      }
    }
  }

  public List<FollowerSelectEntry> GetFollowerSelectEntries()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.IsSnowman)
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
      else if (follower.Traits.Contains(FollowerTrait.TraitType.Mutated))
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
      else if (this.OldFollower != null && follower == this.OldFollower)
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
      else if (this.StructureBrain.Data.MultipleFollowerIDs.Contains(follower.ID))
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.UnavailableAlreadyInBed));
      else if (follower.CursedState == Thought.Child && follower.ID != 100000)
        followerSelectEntries.Add(new FollowerSelectEntry(follower));
      else if (follower.ID == 100000)
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.Unavailable));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower)));
    }
    return followerSelectEntries;
  }

  public void WakeUpFollower()
  {
    this.OldFollower = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
    this.follower = FollowerManager.FindFollowerByID(this.OldFollower.ID);
    if ((UnityEngine.Object) this.follower == (UnityEngine.Object) null)
      return;
    this.follower.Brain.ClearPersonalOverrideTaskProvider();
    if (TimeManager.IsNight)
      this.follower.Brain.AddThought(Thought.SleepInterrupted);
    this.follower.Brain.CompleteCurrentTask();
    this.follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    this.follower.Brain._directInfoAccess.WakeUpDay = TimeManager.CurrentDay;
    CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, this.follower.Brain.Info.ID);
    this.follower.TimedAnimation("tantrum", 3.16666675f, (System.Action) (() => this.follower.Brain.CompleteCurrentTask()));
  }

  public void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    int ID1 = -1;
    int dwellingslot1 = this.dwelling.StructureInfo.MultipleFollowerIDs.Count;
    if (this.StructureBrain.Data.FollowerID == ID1)
      this.StructureBrain.Data.FollowerID = -1;
    if (this.StructureBrain.Data.MultipleFollowerIDs.Count >= this.StructureBrain.SlotCount)
    {
      ID1 = this.StructureBrain.Data.MultipleFollowerIDs[0];
      this.StructureBrain.Data.MultipleFollowerIDs.Remove(ID1);
      if (FollowerInfo.GetInfoByID(ID1) != null)
      {
        Follower followerById = FollowerManager.FindFollowerByID(ID1);
        if ((bool) (UnityEngine.Object) followerById)
        {
          if (followerById.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
            followerById.Brain.CurrentTask.Abort();
          dwellingslot1 = followerById.Brain._directInfoAccess.DwellingSlot;
          followerById.Brain.ClearDwelling();
        }
      }
    }
    int ID2 = Dwelling.NO_HOME;
    int dwellingslot2 = Dwelling.NO_HOME;
    if (followerInfo != null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
      if (followerById.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
        followerById.Brain.CurrentTask.Abort();
      ID2 = followerById.Brain.GetDwellingAndSlot() != null ? followerById.Brain.GetDwellingAndSlot().ID : Dwelling.NO_HOME;
      dwellingslot2 = followerById.Brain.GetDwellingAndSlot() != null ? followerById.Brain.GetDwellingAndSlot().dwellingslot : Dwelling.NO_HOME;
      followerById.Brain.ClearDwelling();
      followerById.Brain.AssignDwelling(new Dwelling.DwellingAndSlot(this.dwelling.StructureInfo.ID, dwellingslot1, 0), followerInfo.ID, false);
      followerById.Brain._directInfoAccess.PreviousDwellingID = Dwelling.NO_HOME;
      followerById.Brain._directInfoAccess.WakeUpDay = -1;
      followerById.Brain.CheckChangeTask();
    }
    if (ID1 != -1 && FollowerInfo.GetInfoByID(ID1) != null)
    {
      Follower followerById = FollowerManager.FindFollowerByID(ID1);
      if ((UnityEngine.Object) followerById == (UnityEngine.Object) null)
      {
        this.HasChanged = true;
        return;
      }
      if (followerById.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
        followerById.Brain.CurrentTask.Abort();
      if (ID2 != Dwelling.NO_HOME)
      {
        followerById.Brain.AssignDwelling(new Dwelling.DwellingAndSlot(ID2, dwellingslot2, 0), followerById.Brain.Info.ID, false);
        StructureManager.GetStructureByID<Structures_Bed>(ID2).ReservedForTask = true;
      }
      else
      {
        Dwelling.DwellingAndSlot freeDwellingAndSlot = StructureManager.GetFreeDwellingAndSlot(FollowerLocation.Base, this.OldFollower);
        if (freeDwellingAndSlot != null)
        {
          followerById.Brain.AssignDwelling(freeDwellingAndSlot, followerById.Brain.Info.ID, false);
          StructureManager.GetStructureByID<Structures_Bed>(freeDwellingAndSlot.ID);
        }
      }
      followerById.Brain._directInfoAccess.PreviousDwellingID = Dwelling.NO_HOME;
      followerById.Brain._directInfoAccess.WakeUpDay = -1;
      followerById.Brain.CheckChangeTask();
    }
    this.HasChanged = true;
  }

  public void OnHidden()
  {
    this.Activated = false;
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    if (this.StructureBrain.SoulCount > 0)
    {
      if (this.Activating)
        return;
      this.Activating = true;
      this.StartCoroutine((IEnumerator) this.GiveReward());
    }
    else
      this.playerFarming.indicator.PlayShake();
  }

  public override void OnThirdInteract(StateMachine state)
  {
    base.OnThirdInteract(state);
    if (this.StructureBrain.Data.ClaimedByPlayer)
      return;
    for (int index = this.StructureBrain.Data.MultipleFollowerIDs.Count - 1; index >= 0; --index)
    {
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureBrain.Data.MultipleFollowerIDs[index]);
      if (infoById != null)
        FollowerBrain.GetOrCreateBrain(infoById)?.ClearDwelling();
    }
    this.Interactable = false;
    this.StructureBrain.Data.ClaimedByPlayer = true;
    this.HasChanged = true;
    this.StartCoroutine((IEnumerator) this.ClaimBedIE());
  }

  public IEnumerator ClaimBedIE()
  {
    Interaction_Bed interactionBed = this;
    foreach (Follower follower in Follower.Followers)
    {
      if (follower.Brain.CurrentTaskType == FollowerTaskType.ClaimDwelling)
      {
        follower.Brain.CurrentTask.Abort();
        follower.Brain.ClearDwelling();
      }
    }
    interactionBed.playerFarming.GoToAndStop(interactionBed.transform.position + Vector3.up / 2f, GoToCallback: new System.Action(interactionBed.\u003CClaimBedIE\u003Eb__44_0));
    yield return (object) new WaitForSeconds(1.5f);
    if ((bool) (UnityEngine.Object) interactionBed.uncollapsedBed)
      interactionBed.uncollapsedBed.gameObject.SetActive(false);
    if ((bool) (UnityEngine.Object) interactionBed.collapsedBed)
      interactionBed.collapsedBed.gameObject.SetActive(false);
    if ((bool) (UnityEngine.Object) interactionBed.XPBar)
      interactionBed.XPBar.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/temple_key/fragment_pickup", interactionBed.gameObject);
    interactionBed.leaderDwelling.gameObject.SetActive(true);
    BiomeConstants.Instance.EmitSmokeInteractionVFX(interactionBed.transform.position - Vector3.forward, Vector3.one * 3f);
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ClaimBed);
    yield return (object) new WaitForEndOfFrame();
    interactionBed.GetLabel();
    interactionBed.HasChanged = true;
  }

  public IEnumerator SleepTillMorning()
  {
    Interaction_Bed interactionBed = this;
    Debug.Log((object) ("Previous Season timestamp is " + SeasonsManager.SEASON_NORMALISED_PROGRESS.ToString()));
    interactionBed.EndIndicateHighlighted(interactionBed.playerFarming);
    Debug.Log((object) "PLAY STING!");
    UIManager.PlayAudio("event:/Stings/don't_starve_sting");
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionBed.playerFarming.gameObject);
    Vector3 SleepingPosition = interactionBed.transform.position + new Vector3(0.09f, 0.211f, -0.02f);
    bool Moving = true;
    interactionBed.playerFarming.GoToAndStop(SleepingPosition, GoToCallback: (System.Action) (() =>
    {
      Moving = false;
      this.playerFarming.transform.position = SleepingPosition;
    }));
    Interaction_Ranchable playerAnimal = interactionBed.playerFarming.GetPlayerAnimal();
    playerAnimal?.Sleep(SleepingPosition + Vector3.left * 2f);
    while (Moving)
      yield return (object) null;
    interactionBed.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionBed.playerFarming.state.facingAngle = 180f;
    interactionBed.playerFarming.simpleSpineAnimator.Animate("sleeping-start", 0, false);
    interactionBed.playerFarming.simpleSpineAnimator.AddAnimate("sleeping", 0, true, 0.0f);
    interactionBed.playerFarming.transform.DOMove(SleepingPosition, 0.5f);
    yield return (object) new WaitForSeconds(1f);
    LightingManager.Instance.inGlobalOverride = true;
    LightingManager.Instance.globalOverrideSettings = LightingManager.Instance.nightSettings;
    LightingManager.Instance.transitionDurationMultiplier = 0.25f;
    LightingManager.Instance.UpdateLighting(false, true);
    yield return (object) new WaitForSeconds(1f);
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      Time.timeScale = 1f;
      SimulationManager.SkipToPhase(DayPhase.Dawn, (System.Action) (() =>
      {
        if ((UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null)
        {
          LightingManager.Instance.globalOverrideSettings = LightingManager.Instance.dawnSettings;
          LightingManager.Instance.transitionDurationMultiplier = 0.5f;
          LightingManager.Instance.UpdateLighting(false, true);
        }
        GameManager.GetInstance().WaitForSecondsRealtime(0.5f, (System.Action) (() =>
        {
          MMTransition.ResumePlay();
          DataManager.Instance.SurvivalMode_Sleep = 100f;
          if ((UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null)
            LightingManager.Instance.inGlobalOverride = false;
          GameManager.GetInstance().OnConversationEnd(false);
          if ((UnityEngine.Object) LightingManager.Instance != (UnityEngine.Object) null)
            LightingManager.Instance.transitionDurationMultiplier = 1f;
          if (FollowerBrainStats.IsBloodMoon)
            LocationManager._Instance.EnableBloodMoon();
          SeasonsManager.DisableCurrentActiveBlizzards();
          interaction_FollowerInteraction.CancelAllLightningStrikes();
          if (!((UnityEngine.Object) AudioManager.Instance != (UnityEngine.Object) null))
            return;
          EventInstance loopedInstance = AudioManager.Instance.CreateLoop("event:/player/lamb_megaphone", this.playerFarming.gameObject, true);
          this.playerFarming.CustomAnimationWithCallback("sleeping-stop", false, (System.Action) (() =>
          {
            playerAnimal?.ResetAnimalState();
            AudioManager.Instance.StopLoop(loopedInstance);
            PlayerFarming.SetStateForAllPlayers();
          }));
        }));
      }));
    }));
  }

  public IEnumerator GiveReward()
  {
    Interaction_Bed interactionBed = this;
    int Souls = interactionBed.StructureBrain.SoulCount;
    for (int i = 0; i < Souls; ++i)
    {
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
      {
        SoulCustomTarget.Create(interactionBed.playerFarming.gameObject, interactionBed.transform.position, Color.white, new System.Action(interactionBed.\u003CGiveReward\u003Eb__46_0));
        --interactionBed.StructureBrain.SoulCount;
        interactionBed.UpdateBar();
      }
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, interactionBed.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      float num = Mathf.Clamp((float) (Souls - i) / (float) interactionBed.StructureBrain.SoulMax, 0.0f, 1f);
      interactionBed.XPBar.UpdateBar(num);
      yield return (object) new WaitForSeconds(0.1f);
    }
    interactionBed.XPBar.UpdateBar(0.0f);
    interactionBed.StructureBrain.SoulCount = 0;
    interactionBed.GetSecondaryLabel();
    interactionBed.HasChanged = true;
    interactionBed.Activating = false;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__35_0() => this.CloseAndSpeak("NotTired");

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__35_1()
  {
    this.OnHidden();
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__35_2()
  {
    this.OnHidden();
    this.HasChanged = true;
  }

  [CompilerGenerated]
  public void \u003CWakeUpFollower\u003Eb__38_0() => this.follower.Brain.CompleteCurrentTask();

  [CompilerGenerated]
  public void \u003CClaimBedIE\u003Eb__44_0()
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(this.playerFarming.gameObject);
    this.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    AudioManager.Instance.PlayOneShot("event:/material/dirt_dig", this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CGiveReward\u003Eb__46_0() => this.playerFarming.GetSoul(1);
}
