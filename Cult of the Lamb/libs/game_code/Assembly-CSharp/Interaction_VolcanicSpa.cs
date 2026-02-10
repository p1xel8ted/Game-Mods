// Decompiled with JetBrains decompiler
// Type: Interaction_VolcanicSpa
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_VolcanicSpa : Interaction
{
  public static List<Interaction_VolcanicSpa> HealingBays = new List<Interaction_VolcanicSpa>();
  public Structure Structure;
  public StructureBrain _StructureInfo;
  public Transform[] FollowerPositions;
  public GameObject EntrancePosition;
  public GameObject EdgePosition;
  public GameObject RewardPosition;
  public Transform playerStandPosition;
  public GameObject splashParticleBig;
  public GameObject splashParticleSmall;
  public ParticleSystem splashParticles;
  public ParticleSystem splash2Particles;
  public SpriteRenderer WaterRenderer;
  public Color ColdWaterColor = Color.white;
  public Color HotWaterColor = Color.red;
  public GameObject heatBubbles;
  public GameObject loveBubbles;
  public float MaxSoakDuration = 30f;
  public float soakTimer;
  [HideInInspector]
  public bool Activated;
  public bool isLockedByCoupleKiss;
  public List<Follower> currentSpaOccupants = new List<Follower>();
  public bool[] spaSlotOccupied;
  public string closeMenuSFX = "event:/ui/close_menu";
  public string boilConfirmFollowerSFX = "event:/dlc/building/spa/boil_confirm";
  public string boilFollowerActionSFX = "event:/dlc/building/spa/boil_action";
  public string defrostFollowerConfirmFollowerSFX = "event:/dlc/building/spa/defrost_confirm";
  public string defrostFollowerActionSFX = "event:/dlc/building/spa/defrost_action";
  public string defrostFollowerCompleteSFX = "event:/dlc/building/spa/defrost_complete";
  public string poolPartyConfirmActionSFX = "event:/dlc/building/spa/poolparty_confirm";
  public string poolPartyActionSFX = "event:/dlc/building/spa/poolparty_action";
  public string poolPartyLoopSFX = "event:/dlc/building/spa/poolparty_action_loop";
  public string poolpartyCompleteSFX = "event:/dlc/building/spa/poolparty_complete";
  public EventInstance spaActiveLoopInstance;
  public EventInstance poolPartyLoopInstance;
  public Dictionary<int, int> _occupantSlots = new Dictionary<int, int>();
  public bool someoneEnteringOrExiting;
  public Coroutine endPoolPartyCoroutine;
  public Coroutine healingRoutineCoroutine;
  public bool confirmBurn;
  public bool makeEggMeal;
  public bool giveSin;
  public Canvas confirmBurnCanvas;
  public bool forceWorkInAllSeasons;
  public bool forceWarmSeasonBehaviour;
  public bool forceSpouseTest;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public void Start()
  {
    if (!Interaction_VolcanicSpa.HealingBays.Contains(this))
      Interaction_VolcanicSpa.HealingBays.Add(this);
    this.spaSlotOccupied = new bool[this.FollowerPositions.Length];
    if (!((UnityEngine.Object) this.WaterRenderer != (UnityEngine.Object) null))
      return;
    this.WaterRenderer.color = this.ColdWaterColor;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.isLockedByCoupleKiss || this.someoneEnteringOrExiting)
      LocalizationManager.GetTranslation("UI/FollowerSelect/Unavailable");
    else
      this.Label = ScriptLocalization.Interactions_Bed.Assign;
  }

  public new void Update()
  {
    base.Update();
    if (PlayerFarming.Location == FollowerLocation.Base && (double) this.soakTimer > 0.0)
    {
      this.soakTimer -= Time.deltaTime;
      if ((double) this.soakTimer <= 0.0)
      {
        this.StopCoroutine(this.healingRoutineCoroutine);
        this.healingRoutineCoroutine = (Coroutine) null;
        if (this.endPoolPartyCoroutine != null)
        {
          this.StopCoroutine(this.endPoolPartyCoroutine);
          this.endPoolPartyCoroutine = (Coroutine) null;
        }
        this.endPoolPartyCoroutine = this.StartCoroutine((IEnumerator) this.EndPoolParty());
      }
    }
    if (this.someoneEnteringOrExiting || this.isLockedByCoupleKiss)
    {
      if (this.Interactable)
      {
        this.Interactable = false;
        this.label = "";
        this.HasChanged = true;
      }
    }
    else if (!this.Interactable)
    {
      this.Interactable = true;
      this.Label = ScriptLocalization.Interactions_Bed.Assign;
      this.HasChanged = true;
    }
    if (this.someoneEnteringOrExiting || this.currentSpaOccupants.Count <= 0)
      return;
    this.KeepOccupantsSnapped();
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.isLockedByCoupleKiss || this.someoneEnteringOrExiting)
      return;
    base.OnInteract(state);
    if (this.Activated || this.currentSpaOccupants.Count >= this.FollowerPositions.Length)
      return;
    this.Activated = true;
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    List<Follower> source = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!this.currentSpaOccupants.Contains(follower) && !FollowerManager.FollowerLocked(follower.Brain.Info.ID, excludeFreezing: true))
        source.Add(follower);
    }
    List<Follower> list = source.OrderByDescending<Follower, float>((Func<Follower, float>) (entry => entry.Brain.Stats.Freezing)).ToList<Follower>();
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in list)
      followerSelectEntries.Add(new FollowerSelectEntry(follower));
    Time.timeScale = 0.0f;
    SimulationManager.Pause();
    UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectMenu.VotingType = TwitchVoting.VotingType.VOLCANIC_SPA;
    followerSelectMenu.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
    selectMenuController2.OnHide = selectMenuController2.OnHide + (System.Action) (() => UIManager.PlayAudio("event:/ui/close_menu"));
    UIFollowerSelectMenuController selectMenuController3 = followerSelectMenu;
    selectMenuController3.OnCancel = selectMenuController3.OnCancel + (System.Action) (() =>
    {
      followerSelectMenu = (UIFollowerSelectMenuController) null;
      this.OnHidden();
      Time.timeScale = 1f;
      SimulationManager.UnPause();
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectMenu;
    selectMenuController4.OnShow = selectMenuController4.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
        followerInfoBox.ShowItemCostValue(this.GetCost(followerInfoBox.followBrain));
    });
    UIFollowerSelectMenuController selectMenuController5 = followerSelectMenu;
    selectMenuController5.OnShownCompleted = selectMenuController5.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
        followerInfoBox.ShowItemCostValue(this.GetCost(followerInfoBox.followBrain));
    });
  }

  public void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    UIManager.PlayAudio(this.defrostFollowerConfirmFollowerSFX);
    bool flag = true;
    List<InventoryItem> cost = this.GetCost(FollowerBrain.GetOrCreateBrain(followerInfo));
    foreach (InventoryItem inventoryItem in cost)
    {
      if (Inventory.GetItemQuantity(inventoryItem.type) < inventoryItem.quantity)
        flag = false;
    }
    if (flag)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
      if ((bool) (UnityEngine.Object) followerById)
      {
        if (followerById.Brain.CurrentTaskType == FollowerTaskType.Sleep)
          CultFaithManager.AddThought(Thought.Cult_WokeUpFollower, followerById.Brain.Info.ID);
        this.RemoveFreezing(followerById.Brain);
        if (this.healingRoutineCoroutine != null)
        {
          this.StopCoroutine(this.healingRoutineCoroutine);
          this.healingRoutineCoroutine = (Coroutine) null;
        }
        this.healingRoutineCoroutine = this.StartCoroutine((IEnumerator) this.HealingRoutine(followerById));
      }
      foreach (InventoryItem inventoryItem in cost)
        Inventory.ChangeItemQuantity(inventoryItem.type, -inventoryItem.quantity);
    }
    else
    {
      this.OnHidden();
      UIManager.PlayAudio("event:/dlc/relic/shared_cantuse");
    }
    this.Activated = false;
  }

  public void RemoveFreezing(FollowerBrain brain)
  {
    if (brain.Info.CursedState == Thought.Freezing)
      NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerFreezing, brain.Info, NotificationFollower.Animation.Happy);
    brain.FrozeToDeath = false;
    brain.Stats.Freezing = 0.0f;
    brain.RemoveCurseState(Thought.Freezing);
    FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
    if (freezingStateChanged == null)
      return;
    freezingStateChanged(brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.spaActiveLoopInstance = AudioManager.Instance.CreateLoop("event:/dlc/building/spa/active_loop", this.gameObject, true);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    AudioManager.Instance.StopLoop(this.spaActiveLoopInstance);
    AudioManager.Instance.StopLoop(this.poolPartyLoopInstance);
  }

  public IEnumerator HealingRoutine(Follower follower)
  {
    Interaction_VolcanicSpa interactionVolcanicSpa = this;
    SimulationManager.Pause();
    interactionVolcanicSpa.someoneEnteringOrExiting = true;
    interactionVolcanicSpa.HasChanged = true;
    Time.timeScale = 1f;
    interactionVolcanicSpa.soakTimer = interactionVolcanicSpa.MaxSoakDuration;
    int slotIndex = interactionVolcanicSpa.GetAvailableSlotIndex();
    bool flag;
    if (!interactionVolcanicSpa.forceWorkInAllSeasons && (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter || interactionVolcanicSpa.forceWarmSeasonBehaviour))
    {
      Debug.Log((object) "BURN call top");
      yield return (object) interactionVolcanicSpa.StartCoroutine((IEnumerator) interactionVolcanicSpa.BurnConfirm());
      if (!interactionVolcanicSpa.confirmBurn)
      {
        if (interactionVolcanicSpa.currentSpaOccupants.Count > 0)
          AudioManager.Instance.PlayOneShot(interactionVolcanicSpa.poolPartyConfirmActionSFX, follower.transform.position);
        GameManager.GetInstance().OnConversationEnd();
        SimulationManager.UnPause();
        interactionVolcanicSpa.someoneEnteringOrExiting = false;
        interactionVolcanicSpa.HasChanged = true;
        flag = false;
        goto label_63;
      }
      slotIndex = 4;
    }
    if (slotIndex < 0)
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      interactionVolcanicSpa.someoneEnteringOrExiting = false;
      interactionVolcanicSpa.HasChanged = true;
      flag = false;
    }
    else if (interactionVolcanicSpa.FollowerPositions == null)
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      interactionVolcanicSpa.someoneEnteringOrExiting = false;
      interactionVolcanicSpa.HasChanged = true;
      flag = false;
    }
    else if (interactionVolcanicSpa.spaSlotOccupied == null)
    {
      SimulationManager.UnPause();
      GameManager.GetInstance().OnConversationEnd();
      interactionVolcanicSpa.someoneEnteringOrExiting = false;
      interactionVolcanicSpa.HasChanged = true;
      flag = false;
    }
    else
    {
      if (slotIndex >= interactionVolcanicSpa.FollowerPositions.Length || slotIndex >= interactionVolcanicSpa.spaSlotOccupied.Length)
      {
        int num = interactionVolcanicSpa.FollowerPositions.Length != 0 ? interactionVolcanicSpa.FollowerPositions.Length - 1 : -1;
        if (num < 0)
        {
          SimulationManager.UnPause();
          GameManager.GetInstance().OnConversationEnd();
          interactionVolcanicSpa.someoneEnteringOrExiting = false;
          interactionVolcanicSpa.HasChanged = true;
          flag = false;
          goto label_63;
        }
        slotIndex = num;
      }
      follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_HardManualControl());
      follower.Brain.Stats.ReceivedBlessing = true;
      follower.FacePosition(interactionVolcanicSpa.transform.position);
      if ((UnityEngine.Object) interactionVolcanicSpa.EntrancePosition != (UnityEngine.Object) null)
        follower.transform.position = interactionVolcanicSpa.EntrancePosition.transform.position;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(follower.gameObject);
      if (slotIndex >= 0 && slotIndex < interactionVolcanicSpa.spaSlotOccupied.Length)
        interactionVolcanicSpa.spaSlotOccupied[slotIndex] = true;
      interactionVolcanicSpa.currentSpaOccupants.Add(follower);
      interactionVolcanicSpa.soakTimer = interactionVolcanicSpa.MaxSoakDuration;
      yield return (object) new WaitForSeconds(0.5f);
      List<InventoryItem> costs = interactionVolcanicSpa.GetCost(follower.Brain);
      int totalParticles = 0;
      foreach (InventoryItem inventoryItem in costs)
        totalParticles += inventoryItem.quantity;
      if ((UnityEngine.Object) interactionVolcanicSpa.WaterRenderer != (UnityEngine.Object) null)
        interactionVolcanicSpa.WaterRenderer.DOColor(interactionVolcanicSpa.HotWaterColor, 1f);
      if ((UnityEngine.Object) interactionVolcanicSpa.heatBubbles != (UnityEngine.Object) null)
        interactionVolcanicSpa.heatBubbles.SetActive(true);
      for (int i = 0; i < totalParticles; ++i)
      {
        ResourceCustomTarget.Create(interactionVolcanicSpa.gameObject, interactionVolcanicSpa.playerFarming.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 0.25f), (InventoryItem.ITEM_TYPE) costs[UnityEngine.Random.Range(0, costs.Count)].type, (System.Action) null);
        yield return (object) new WaitForSeconds(0.1f);
      }
      if (slotIndex == -1)
      {
        SimulationManager.UnPause();
        GameManager.GetInstance().OnConversationEnd();
        interactionVolcanicSpa.someoneEnteringOrExiting = false;
        interactionVolcanicSpa.HasChanged = true;
        flag = false;
      }
      else
      {
        follower.Brain.TemporaryOutfitStore = follower.Brain.Info.Outfit;
        if ((bool) (UnityEngine.Object) follower && (bool) (UnityEngine.Object) follower.Spine && follower.Spine.Skeleton.Data != null)
        {
          if ((double) follower.Brain.Stats.Freezing > 0.0)
          {
            follower.TimedAnimation("Reactions/react-happy1", 1.86666667f);
            follower.AddBodyAnimation("Conversations/talk-nice2", true, 0.0f);
            yield return (object) new WaitForSeconds(0.5f);
          }
          FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, 0, follower.Brain.Info.SkinName, follower.Brain.Info.SkinColour, FollowerOutfitType.Naked, follower.Brain.Info.Hat, follower.Brain.Info.Clothing, follower.Brain.Info.Customisation, follower.Brain.Info.Special, follower.Brain.Info.Necklace, follower.Brain.Info.ClothingVariant, follower.Brain._directInfoAccess);
          yield return (object) new WaitForSeconds(1.5f);
        }
        Transform targetPosition = (Transform) null;
        if (slotIndex >= 0 && interactionVolcanicSpa.FollowerPositions != null && slotIndex < interactionVolcanicSpa.FollowerPositions.Length)
        {
          targetPosition = interactionVolcanicSpa.FollowerPositions[slotIndex];
          bool waiting = true;
          if ((UnityEngine.Object) follower != (UnityEngine.Object) null && follower.Brain != null && follower.Brain.CurrentTask != null && (UnityEngine.Object) interactionVolcanicSpa.EdgePosition != (UnityEngine.Object) null)
            ((FollowerTask_ManualControl) follower.Brain.CurrentTask).GoToAndStop(follower, interactionVolcanicSpa.EdgePosition.transform.position, (System.Action) (() => waiting = false));
          else
            waiting = false;
          while (waiting)
            yield return (object) null;
          AudioManager.Instance.PlayOneShot("event:/dlc/building/spa/follower_jump_in", follower.gameObject);
          AudioManager.Instance.PlayOneShot("event:/dlc/building/spa/defrost_action_follower_vo", follower.gameObject);
          double num1 = (double) follower.SetBodyAnimation("Activities/activity-tub-jump-in-bomb", false);
          yield return (object) new WaitForSeconds(0.5f);
          follower.transform.DOMove(targetPosition.position, 0.6333333f);
          yield return (object) new WaitForSeconds(0.333333343f);
          interactionVolcanicSpa.splashParticleBig.transform.position = new Vector3(follower.transform.position.x, interactionVolcanicSpa.splashParticleBig.transform.position.y, follower.transform.position.z);
          interactionVolcanicSpa.splashParticles.Play();
          yield return (object) new WaitForSeconds(0.333333343f);
          follower.Spine.gameObject.SetActive(false);
          yield return (object) new WaitForSeconds(1f);
          double num2 = (double) follower.SetBodyAnimation("Activities/activity-tub-settle", false);
          follower.Spine.gameObject.SetActive(true);
          interactionVolcanicSpa.splash2Particles.Play();
          interactionVolcanicSpa.splashParticleSmall.transform.position = new Vector3(follower.transform.position.x, interactionVolcanicSpa.splashParticleSmall.transform.position.y, follower.transform.position.z);
          follower.transform.SetParent(interactionVolcanicSpa.transform, true);
          yield return (object) new WaitForSeconds(1f);
          if ((follower.Brain.Info.CursedState == Thought.Freezing || SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter) && !follower.Brain.Info.IsSnowman)
          {
            follower.OverridingEmotions = true;
            follower.SetFaceAnimation("Emotions/emotion-happy", true);
            double num3 = (double) follower.SetBodyAnimation(follower.Brain.Info.CursedState == Thought.Freezing ? "Spa/spa-cured" : "Spa/spa-normal", false);
            follower.AddBodyAnimation("idle", true, 0.0f);
            if (slotIndex == 0 || slotIndex == 3)
              follower.State.facingAngle = 0.0f;
            else if (slotIndex == 1 || slotIndex == 4)
              follower.State.facingAngle = 180f;
            follower.State.LookAngle = follower.State.facingAngle;
            if (follower.Brain.Info.CursedState == Thought.Freezing)
            {
              AudioManager.Instance.PlayOneShot(interactionVolcanicSpa.defrostFollowerActionSFX, follower.transform.position);
              AudioManager.Instance.PlayOneShot("event:/dlc/building/spa/defrost_action_follower_vo", follower.transform.position);
              FollowerBrainStats.StatStateChangedEvent freezingStateChanged = FollowerBrainStats.OnFreezingStateChanged;
              if (freezingStateChanged != null)
                freezingStateChanged(follower.Brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
              NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.NoLongerFreezing, follower.Brain.Info, NotificationFollower.Animation.Happy);
              follower.Brain.Info.CursedState = Thought.None;
              BiomeConstants.Instance.EmitHeartPickUpVFX(follower.gameObject.transform.position, 0.0f, "black", "burst_big");
              follower.Brain.Stats.Freezing = 0.0f;
              follower.Brain.Stats.Exhaustion = 0.0f;
            }
            interactionVolcanicSpa._occupantSlots[follower.Brain.Info.ID] = slotIndex;
            yield return (object) new WaitForSeconds(2f);
            follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_HardManualControl());
            SimulationManager.UnPause();
            GameManager.GetInstance().OnConversationEnd();
            interactionVolcanicSpa.someoneEnteringOrExiting = false;
            interactionVolcanicSpa.HasChanged = true;
            interactionVolcanicSpa.CheckForSpaCouples();
          }
          else
          {
            Debug.Log((object) ("BURN done confirm? " + interactionVolcanicSpa.confirmBurn.ToString()));
            GameManager.GetInstance().OnConversationNew();
            GameManager.GetInstance().OnConversationNext(follower.gameObject, 7f);
            AudioManager.Instance.PlayOneShot("event:/dlc/building/spa/boil_action_follower_vo", follower.transform.position);
            AudioManager.Instance.PlayOneShot(interactionVolcanicSpa.boilFollowerActionSFX);
            double num4 = (double) follower.SetBodyAnimation("Scared/scared-scream", false);
            yield return (object) new WaitForSeconds(0.1f);
            double num5 = (double) follower.SetBodyAnimation("Spa/spa-killed", false);
            DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 3f, 3f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
            yield return (object) new WaitForSeconds(2.9f);
            if (follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated))
              interactionVolcanicSpa.StartCoroutine((IEnumerator) interactionVolcanicSpa.SpawnItem(InventoryItem.ITEM_TYPE.MAGMA_STONE, 10, new Vector2(0.05f, 0.1f), follower.transform.position));
            else if (!follower.Brain.Info.IsSnowman)
              interactionVolcanicSpa.StartCoroutine((IEnumerator) interactionVolcanicSpa.SpawnItem(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 10, new Vector2(0.05f, 0.1f), follower.transform.position));
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("Notifications/Follower/DiedFromOverheating", follower.Brain.Info.Name);
            follower.transform.SetParent(BiomeBaseManager.Instance.Room.CustomTransform.transform, true);
            follower.Die(NotificationCentre.NotificationType.BurntToDeath, false, force: true);
            CameraManager.instance.ShakeCameraForDuration(0.5f, 0.6f, 0.2f);
            yield return (object) new WaitForSeconds(2f);
            SimulationManager.UnPause();
            GameManager.GetInstance().OnConversationEnd();
            if (slotIndex >= 0 && slotIndex < interactionVolcanicSpa.spaSlotOccupied.Length)
              interactionVolcanicSpa.spaSlotOccupied[slotIndex] = false;
            interactionVolcanicSpa.currentSpaOccupants.Remove(follower);
            interactionVolcanicSpa._occupantSlots.Remove(follower.Brain.Info.ID);
            interactionVolcanicSpa.someoneEnteringOrExiting = false;
            interactionVolcanicSpa.HasChanged = true;
          }
          costs = (List<InventoryItem>) null;
          targetPosition = (Transform) null;
          yield break;
        }
        SimulationManager.UnPause();
        GameManager.GetInstance().OnConversationEnd();
        interactionVolcanicSpa.someoneEnteringOrExiting = false;
        interactionVolcanicSpa.HasChanged = true;
        flag = false;
      }
    }
label_63:
    return flag;
  }

  public IEnumerator EndPoolParty()
  {
    Interaction_VolcanicSpa interactionVolcanicSpa1 = this;
    interactionVolcanicSpa1.someoneEnteringOrExiting = true;
    interactionVolcanicSpa1.HasChanged = true;
    try
    {
      if (interactionVolcanicSpa1.currentSpaOccupants.Count > 1)
      {
        AudioManager.Instance.StopLoop(interactionVolcanicSpa1.poolPartyLoopInstance);
        AudioManager.Instance.PlayOneShot(interactionVolcanicSpa1.poolpartyCompleteSFX);
      }
      if (interactionVolcanicSpa1.giveSin)
      {
        foreach (Follower currentSpaOccupant in interactionVolcanicSpa1.currentSpaOccupants)
        {
          if ((UnityEngine.Object) currentSpaOccupant != (UnityEngine.Object) null && currentSpaOccupant.Brain != null)
            currentSpaOccupant.Brain.AddPleasure(FollowerBrain.PleasureActions.GroupSpa);
        }
        interactionVolcanicSpa1.giveSin = false;
      }
      interactionVolcanicSpa1.StartCoroutine((IEnumerator) interactionVolcanicSpa1.StopParticleEffect(interactionVolcanicSpa1.heatBubbles));
      if (interactionVolcanicSpa1.makeEggMeal)
      {
        if ((UnityEngine.Object) interactionVolcanicSpa1.RewardPosition != (UnityEngine.Object) null)
          InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MEAL_EGG, 1, interactionVolcanicSpa1.RewardPosition.transform.position, (float) UnityEngine.Random.Range(9, 11), new System.Action<PickUp>(interactionVolcanicSpa1.\u003CEndPoolParty\u003Eb__56_0));
        interactionVolcanicSpa1.makeEggMeal = false;
      }
      foreach (Follower follower in interactionVolcanicSpa1.currentSpaOccupants)
      {
        follower.Brain.FrozeToDeath = false;
        follower.Interaction_FollowerInteraction.Interactable = false;
        follower.SimpleAnimator.Animate("idle", 1, true, 0.0f);
        double num1 = (double) follower.SetBodyAnimation("Reactions/react-happy1", false);
        follower.SetFaceAnimation("Reactions/react-happy1", false);
        follower.Brain.AddThought(Thought.UsedHotspring);
        BiomeConstants.Instance.EmitHeartPickUpVFX(follower.gameObject.transform.position, 0.0f, "black", "burst_big");
        AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", follower.gameObject);
        yield return (object) new WaitForSeconds(0.5f);
        bool exiting = true;
        follower.transform.SetParent(BiomeBaseManager.Instance.Room.CustomTransform.transform, true);
        if (follower.Brain.CurrentTask is FollowerTask_ManualControl)
          ((FollowerTask_ManualControl) follower.Brain.CurrentTask).GoToAndStop(follower, interactionVolcanicSpa1.EdgePosition.transform.position + Vector3.up, (System.Action) (() => exiting = false));
        else
          exiting = false;
        while (exiting)
          yield return (object) null;
        AudioManager.Instance.PlayOneShot("event:/dlc/building/spa/follower_jump_out", follower.gameObject);
        double num2 = (double) follower.SetBodyAnimation("Activities/activity-tub-jump-out", false);
        yield return (object) new WaitForSeconds(0.5f);
        AudioManager.Instance.PlayOneShot(interactionVolcanicSpa1.defrostFollowerCompleteSFX);
        follower.transform.DOMove(interactionVolcanicSpa1.EntrancePosition.transform.position, 0.6333333f);
        yield return (object) new WaitForSeconds(1f);
        follower.OverridingEmotions = false;
        follower.SimpleAnimator.ResetAnimationsToDefaults();
        follower.Brain.CurrentState.SetStateAnimations(follower, true);
        follower.FacePosition(interactionVolcanicSpa1.state.transform.position);
        follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num3 = (double) follower.SetBodyAnimation("Reactions/react-happy1", false);
        follower.SetFaceAnimation("Reactions/react-happy1", false);
        yield return (object) new WaitForSeconds(1f);
        if ((bool) (UnityEngine.Object) follower && (bool) (UnityEngine.Object) follower.Spine && follower.Spine.Skeleton.Data != null)
          FollowerBrain.SetFollowerCostume(follower.Spine.Skeleton, 0, follower.Brain.Info.SkinName, follower.Brain.Info.SkinColour, follower.Brain.TemporaryOutfitStore, follower.Brain.Info.Hat, follower.Brain.Info.Clothing, follower.Brain.Info.Customisation, follower.Brain.Info.Special, follower.Brain.Info.Necklace, follower.Brain.Info.ClothingVariant, follower.Brain._directInfoAccess);
        follower.Brain.CurrentState = (FollowerState) new FollowerState_Default();
        follower.Brain.CurrentState.SetStateAnimations(follower, true);
        follower.Brain.CompleteCurrentTask();
        follower.SetEmotionAnimation();
        interactionVolcanicSpa1._occupantSlots.Remove(follower.Brain.Info.ID);
        follower.Interaction_FollowerInteraction.Interactable = true;
      }
      interactionVolcanicSpa1.StartCoroutine((IEnumerator) interactionVolcanicSpa1.StopParticleEffect(interactionVolcanicSpa1.loveBubbles));
      AudioManager.Instance.StopLoop(interactionVolcanicSpa1.poolPartyLoopInstance);
      interactionVolcanicSpa1.currentSpaOccupants.Clear();
      for (int index = 0; index < interactionVolcanicSpa1.spaSlotOccupied.Length; ++index)
        interactionVolcanicSpa1.spaSlotOccupied[index] = false;
      if ((UnityEngine.Object) interactionVolcanicSpa1.WaterRenderer != (UnityEngine.Object) null)
        interactionVolcanicSpa1.WaterRenderer.DOColor(interactionVolcanicSpa1.ColdWaterColor, 1f);
      if ((UnityEngine.Object) interactionVolcanicSpa1.heatBubbles != (UnityEngine.Object) null)
        interactionVolcanicSpa1.StartCoroutine((IEnumerator) interactionVolcanicSpa1.StopParticleEffect(interactionVolcanicSpa1.heatBubbles));
      interactionVolcanicSpa1.isLockedByCoupleKiss = false;
      interactionVolcanicSpa1.HasChanged = true;
      interactionVolcanicSpa1.Interactable = true;
    }
    finally
    {
      Interaction_VolcanicSpa interactionVolcanicSpa2 = this;
      interactionVolcanicSpa2._occupantSlots.Clear();
      interactionVolcanicSpa2.someoneEnteringOrExiting = false;
      interactionVolcanicSpa2.HasChanged = true;
      interactionVolcanicSpa2.endPoolPartyCoroutine = (Coroutine) null;
    }
  }

  public IEnumerator BurnConfirm()
  {
    Interaction_VolcanicSpa interactionVolcanicSpa = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionVolcanicSpa.gameObject, 6f);
    yield return (object) new WaitForSeconds(1f);
    Debug.Log((object) "BURN start");
    GameObject g = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/UI/Choice Indicator"), GameObject.FindWithTag("Canvas").transform) as GameObject;
    g.name = "BURN CONFIRM";
    if ((UnityEngine.Object) g != (UnityEngine.Object) null)
    {
      Debug.Log((object) "BURN g not null");
      ChoiceIndicator choice = g.GetComponent<ChoiceIndicator>();
      if ((bool) (UnityEngine.Object) choice)
      {
        Debug.Log((object) "BURN choice not null");
        choice.Offset = new Vector3(0.0f, -300f);
        choice.Show(LocalizationManager.GetTranslation("UI/HotTub/TooHotWarning"), LocalizationManager.GetTranslation("UI/HotTub/TooHotWarningSubtitle"), LocalizationManager.GetTranslation("UI/HotTub/TooHotIgnoreWarning"), LocalizationManager.GetTranslation("UI/HotTub/TooHotIgnoreWarningSubtitle"), (System.Action) (() =>
        {
          this.confirmBurn = false;
          AudioManager.Instance.PlayOneShot("event:/dlc/building/spa/boil_cancel");
          g = (GameObject) null;
          GameManager.GetInstance().OnConversationEnd();
        }), (System.Action) (() =>
        {
          this.confirmBurn = true;
          g = (GameObject) null;
          AudioManager.Instance.PlayOneShot(this.boilConfirmFollowerSFX);
        }), interactionVolcanicSpa.transform.position, true, playConfirmSfx: false);
        while ((UnityEngine.Object) g != (UnityEngine.Object) null)
        {
          choice.UpdatePosition(interactionVolcanicSpa.transform.position);
          yield return (object) null;
        }
      }
      choice = (ChoiceIndicator) null;
    }
  }

  public void CheckForSpaCouples()
  {
    if (this.currentSpaOccupants.Count != 2 && this.currentSpaOccupants.Count != 5)
      return;
    Follower currentSpaOccupant1 = this.currentSpaOccupants[0];
    Follower currentSpaOccupant2 = this.currentSpaOccupants[1];
    if (this.forceSpouseTest || currentSpaOccupant1.Brain._directInfoAccess.SpouseFollowerID == currentSpaOccupant2.Brain.Info.ID)
    {
      double num1 = (double) currentSpaOccupant1.SetBodyAnimation("kiss", false);
      double num2 = (double) currentSpaOccupant2.SetBodyAnimation("kiss", false);
      currentSpaOccupant1.AddBodyAnimation("idle", true, 0.0f);
      currentSpaOccupant2.AddBodyAnimation("idle", true, 0.0f);
      if ((UnityEngine.Object) this.loveBubbles != (UnityEngine.Object) null)
        this.loveBubbles.SetActive(true);
      this.makeEggMeal = true;
      this.isLockedByCoupleKiss = true;
      this.Interactable = false;
      this.HasChanged = true;
    }
    else
    {
      if (this.currentSpaOccupants.Count != 5)
        return;
      AudioManager.Instance.PlayOneShot(this.poolPartyActionSFX, currentSpaOccupant1.transform.position);
      this.poolPartyLoopInstance = AudioManager.Instance.CreateLoop(this.poolPartyLoopSFX, this.gameObject, true);
      this.isLockedByCoupleKiss = true;
      this.Interactable = false;
      this.HasChanged = true;
      if ((UnityEngine.Object) this.loveBubbles != (UnityEngine.Object) null)
        this.loveBubbles.SetActive(true);
      this.giveSin = true;
      DOVirtual.DelayedCall(UnityEngine.Random.value * 2f, (TweenCallback) (() =>
      {
        if (this.currentSpaOccupants.Count <= 0)
          return;
        double num = (double) this.currentSpaOccupants[0].SetBodyAnimation("kiss", true);
      }));
      DOVirtual.DelayedCall(UnityEngine.Random.value * 2f, (TweenCallback) (() =>
      {
        if (this.currentSpaOccupants.Count <= 1)
          return;
        double num = (double) this.currentSpaOccupants[1].SetBodyAnimation("kiss", true);
      }));
      DOVirtual.DelayedCall(UnityEngine.Random.value * 2f, (TweenCallback) (() =>
      {
        if (this.currentSpaOccupants.Count <= 2)
          return;
        double num = (double) this.currentSpaOccupants[2].SetBodyAnimation("kiss", true);
      }));
      DOVirtual.DelayedCall(UnityEngine.Random.value * 2f, (TweenCallback) (() =>
      {
        if (this.currentSpaOccupants.Count <= 3)
          return;
        double num = (double) this.currentSpaOccupants[3].SetBodyAnimation("kiss", true);
      }));
      DOVirtual.DelayedCall(UnityEngine.Random.value * 2f, (TweenCallback) (() =>
      {
        if (this.currentSpaOccupants.Count <= 4)
          return;
        double num = (double) this.currentSpaOccupants[4].SetBodyAnimation("kiss", true);
      }));
    }
  }

  public int GetAvailableSlotIndex()
  {
    for (int availableSlotIndex = 0; availableSlotIndex < this.spaSlotOccupied.Length; ++availableSlotIndex)
    {
      if (!this.spaSlotOccupied[availableSlotIndex])
        return availableSlotIndex;
    }
    return -1;
  }

  public bool IsSpaAlreadyOccupied()
  {
    for (int index = 0; index < this.spaSlotOccupied.Length; ++index)
    {
      if (this.spaSlotOccupied[index])
        return true;
    }
    return false;
  }

  public IEnumerator StopParticleEffect(GameObject bubbleObject)
  {
    if (!((UnityEngine.Object) bubbleObject == (UnityEngine.Object) null))
    {
      ParticleSystem ps = bubbleObject.GetComponent<ParticleSystem>();
      if ((UnityEngine.Object) ps != (UnityEngine.Object) null)
      {
        ps.Stop();
        yield return (object) new WaitUntil((Func<bool>) (() => !ps.IsAlive(true)));
      }
      bubbleObject.SetActive(false);
    }
  }

  public void OnHidden()
  {
    this.Activated = false;
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void OnDestroy()
  {
    Interaction_VolcanicSpa.HealingBays.Remove(this);
    AudioManager.Instance.StopLoop(this.spaActiveLoopInstance);
    AudioManager.Instance.StopLoop(this.poolPartyLoopInstance);
    this._occupantSlots.Clear();
    base.OnDestroy();
  }

  public List<InventoryItem> GetCost(FollowerBrain brain)
  {
    return new List<InventoryItem>()
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 8)
    };
  }

  public IEnumerator SpawnItem(
    InventoryItem.ITEM_TYPE item,
    int amount,
    Vector2 timeBetween,
    Vector3 pos)
  {
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(timeBetween.x, timeBetween.y));
    for (int i = 0; i < amount; ++i)
    {
      InventoryItem.Spawn(item, 1, pos - Vector3.forward).GetComponent<PickUp>().SetInitialSpeedAndDiraction(4f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(timeBetween.x, timeBetween.y));
    }
  }

  public void KeepOccupantsSnapped()
  {
    if (this.FollowerPositions == null || this.FollowerPositions.Length == 0)
      return;
    for (int index1 = 0; index1 < this.currentSpaOccupants.Count; ++index1)
    {
      Follower currentSpaOccupant = this.currentSpaOccupants[index1];
      if (!((UnityEngine.Object) currentSpaOccupant == (UnityEngine.Object) null) && currentSpaOccupant.Brain != null)
      {
        int id = currentSpaOccupant.Brain.Info.ID;
        int index2;
        if (!this._occupantSlots.TryGetValue(id, out index2) || index2 < 0 || index2 >= this.FollowerPositions.Length)
        {
          index2 = Mathf.Clamp(index1, 0, this.FollowerPositions.Length - 1);
          this._occupantSlots[id] = index2;
        }
        Transform followerPosition = this.FollowerPositions[index2];
        if (!((UnityEngine.Object) followerPosition == (UnityEngine.Object) null) && (double) Vector3.Distance(currentSpaOccupant.transform.position, followerPosition.position) > 0.75)
        {
          this.RemoveFreezing(currentSpaOccupant.Brain);
          currentSpaOccupant.transform.position = followerPosition.position;
          if ((UnityEngine.Object) currentSpaOccupant.State != (UnityEngine.Object) null)
          {
            switch (index2)
            {
              case 0:
              case 3:
                currentSpaOccupant.State.facingAngle = 0.0f;
                break;
              case 1:
              case 4:
                currentSpaOccupant.State.facingAngle = 180f;
                break;
            }
            currentSpaOccupant.State.LookAngle = currentSpaOccupant.State.facingAngle;
          }
          Rigidbody2D component = currentSpaOccupant.GetComponent<Rigidbody2D>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.velocity = Vector2.zero;
          currentSpaOccupant.OverridingEmotions = true;
          currentSpaOccupant.SetFaceAnimation("Emotions/emotion-happy", true);
          double num = (double) currentSpaOccupant.SetBodyAnimation("Spa/spa-normal", false);
          currentSpaOccupant.AddBodyAnimation("idle", true, 0.0f);
          Debug.Log((object) ("Needed snap " + Time.time.ToString()));
        }
      }
    }
  }

  [CompilerGenerated]
  public void \u003CEndPoolParty\u003Eb__56_0(PickUp pickUp)
  {
    Meal component = pickUp.GetComponent<Meal>();
    component.CreateStructureLocation = this.StructureInfo.Location;
    component.CreateStructureOnStop = true;
  }

  [CompilerGenerated]
  public void \u003CCheckForSpaCouples\u003Eb__59_0()
  {
    if (this.currentSpaOccupants.Count <= 0)
      return;
    double num = (double) this.currentSpaOccupants[0].SetBodyAnimation("kiss", true);
  }

  [CompilerGenerated]
  public void \u003CCheckForSpaCouples\u003Eb__59_1()
  {
    if (this.currentSpaOccupants.Count <= 1)
      return;
    double num = (double) this.currentSpaOccupants[1].SetBodyAnimation("kiss", true);
  }

  [CompilerGenerated]
  public void \u003CCheckForSpaCouples\u003Eb__59_2()
  {
    if (this.currentSpaOccupants.Count <= 2)
      return;
    double num = (double) this.currentSpaOccupants[2].SetBodyAnimation("kiss", true);
  }

  [CompilerGenerated]
  public void \u003CCheckForSpaCouples\u003Eb__59_3()
  {
    if (this.currentSpaOccupants.Count <= 3)
      return;
    double num = (double) this.currentSpaOccupants[3].SetBodyAnimation("kiss", true);
  }

  [CompilerGenerated]
  public void \u003CCheckForSpaCouples\u003Eb__59_4()
  {
    if (this.currentSpaOccupants.Count <= 4)
      return;
    double num = (double) this.currentSpaOccupants[4].SetBodyAnimation("kiss", true);
  }
}
