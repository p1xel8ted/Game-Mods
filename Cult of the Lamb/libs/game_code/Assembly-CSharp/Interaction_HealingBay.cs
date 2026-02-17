// Decompiled with JetBrains decompiler
// Type: Interaction_HealingBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_HealingBay : Interaction
{
  public static List<Interaction_HealingBay> HealingBays = new List<Interaction_HealingBay>();
  public Structure Structure;
  public Structures_HealingBay _StructureInfo;
  public GameObject FollowerPosition;
  public GameObject EntrancePosition;
  public bool Activated;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_HealingBay structureBrain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_HealingBay;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public int PatientID => this.StructureInfo.FollowerID;

  public void Start() => Interaction_HealingBay.HealingBays.Add(this);

  public override void GetLabel()
  {
    base.GetLabel();
    if (this.Activated)
      this.Label = "";
    else
      this.Label = ScriptLocalization.Interactions_Bed.Assign;
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.Activated)
      return;
    this.Activated = true;
    bool isUpgraded = this.structureBrain.Data.Type == StructureBrain.TYPES.HEALING_BAY_2;
    Time.timeScale = 0.0f;
    HUD_Manager.Instance.Hide(false, 0);
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (Follower follower in Follower.Followers)
    {
      if (Interaction_HealingBay.RequiresHealing(follower.Brain))
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain._directInfoAccess, true, true)));
      else
        followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerSelectEntry.Status.UnavailableDoesNotNeedHealing));
    }
    UIFollowerSelectMenuController followerSelectMenu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectMenu.VotingType = TwitchVoting.VotingType.HEALING_BAY;
    followerSelectMenu.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, true, true, true, false, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenu;
    selectMenuController1.OnFollowerSelected = selectMenuController1.OnFollowerSelected + new System.Action<FollowerInfo>(this.OnFollowerChosenForConversion);
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenu;
    selectMenuController2.OnCancel = selectMenuController2.OnCancel + (System.Action) (() =>
    {
      followerSelectMenu = (UIFollowerSelectMenuController) null;
      this.OnHidden();
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectMenu;
    selectMenuController3.OnShow = selectMenuController3.OnShow + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerInfo.CursedState == Thought.Ill || followerInfoBox.FollowerInfo.CursedState == Thought.Injured || followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.Zombie) || followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.ExistentialDread) || followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
          followerInfoBox.ShowItemCostValue(Interaction_HealingBay.GetCost(followerInfoBox.followBrain, isUpgraded));
        else if (followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.Mutated) && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.RemoveRot))
          followerInfoBox.ShowItemCostValue(Interaction_HealingBay.GetCost(followerInfoBox.followBrain, isUpgraded));
      }
    });
    UIFollowerSelectMenuController selectMenuController4 = followerSelectMenu;
    selectMenuController4.OnShownCompleted = selectMenuController4.OnShownCompleted + (System.Action) (() =>
    {
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenu.FollowerInfoBoxes)
      {
        if (followerInfoBox.FollowerInfo.CursedState == Thought.Ill || followerInfoBox.FollowerInfo.CursedState == Thought.Injured || followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.Zombie) || followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.ExistentialDread) || followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
          followerInfoBox.ShowItemCostValue(Interaction_HealingBay.GetCost(followerInfoBox.followBrain, isUpgraded));
        else if (followerInfoBox.followBrain.HasTrait(FollowerTrait.TraitType.Mutated) && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.RemoveRot))
          followerInfoBox.ShowItemCostValue(Interaction_HealingBay.GetCost(followerInfoBox.followBrain, isUpgraded));
      }
    });
  }

  public static bool RequiresHealing(FollowerBrain follower)
  {
    return follower.Info.CursedState == Thought.Ill || follower.Info.CursedState == Thought.Injured || follower.HasTrait(FollowerTrait.TraitType.Zombie) || follower.HasTrait(FollowerTrait.TraitType.Mutated) && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.RemoveRot) || follower.HasTrait(FollowerTrait.TraitType.ExistentialDread) || follower.HasTrait(FollowerTrait.TraitType.MissionaryTerrified);
  }

  public void OnFollowerChosenForConversion(FollowerInfo followerInfo)
  {
    bool flag = true;
    List<InventoryItem> cost = Interaction_HealingBay.GetCost(FollowerBrain.GetOrCreateBrain(followerInfo), this.structureBrain.Data.Type == StructureBrain.TYPES.HEALING_BAY_2);
    foreach (InventoryItem inventoryItem in cost)
    {
      if (Inventory.GetItemQuantity(inventoryItem.type) < inventoryItem.quantity)
        flag = false;
    }
    if (flag)
    {
      Follower followerById = FollowerManager.FindFollowerByID(followerInfo.ID);
      if ((bool) (UnityEngine.Object) followerById)
        this.StartCoroutine((IEnumerator) this.HealingRoutine(followerById));
      foreach (InventoryItem inventoryItem in cost)
        Inventory.ChangeItemQuantity(inventoryItem.type, -inventoryItem.quantity);
    }
    else
      this.OnHidden();
  }

  public IEnumerator HealingRoutine(Follower follower)
  {
    Interaction_HealingBay interactionHealingBay = this;
    interactionHealingBay.structureBrain.ReservedByPlayer = true;
    Time.timeScale = 1f;
    bool wasRotten = follower.Brain.HasTrait(FollowerTrait.TraitType.Mutated);
    if (wasRotten)
      AudioManager.Instance.PlayOneShot("event:/dlc/ritual/healdecay_confirm");
    interactionHealingBay.playerFarming.GoToAndStop(interactionHealingBay.EntrancePosition.transform.position + Vector3.right * 2f, follower.gameObject);
    follower.Brain.CurrentTask?.Abort();
    FollowerTask_ManualControl task = new FollowerTask_ManualControl();
    follower.Brain.HardSwapToTask((FollowerTask) task);
    follower.transform.position = interactionHealingBay.EntrancePosition.transform.position;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(follower.gameObject);
    yield return (object) new WaitForSeconds(1f);
    bool waiting = true;
    task.GoToAndStop(follower, interactionHealingBay.FollowerPosition.transform.position, (System.Action) (() => waiting = false));
    SimulationManager.Pause();
    while (waiting)
      yield return (object) null;
    follower.SimpleAnimator.Animate("sleep_bedrest_justhead", 1, true, 0.0f);
    int cursedState = (int) follower.Brain.Info.CursedState;
    float illness = follower.Brain._directInfoAccess.Illness;
    float injury = follower.Brain._directInfoAccess.Injured;
    List<InventoryItem> costs = Interaction_HealingBay.GetCost(follower.Brain, interactionHealingBay.structureBrain.Data.Type == StructureBrain.TYPES.HEALING_BAY_2);
    float t = 0.0f;
    float duration = 3f;
    while ((double) t < (double) duration)
    {
      if ((double) Time.deltaTime > 0.0)
      {
        t += Time.deltaTime;
        float t1 = t / duration;
        follower.Brain.Stats.Illness = Mathf.Lerp(illness, 0.0f, t1);
        follower.Brain.Stats.Injured = Mathf.Lerp(injury, 0.0f, t1);
        if (Time.frameCount % 10 == 0 && (double) t > 0.5 && (double) t < (double) duration - 0.5)
          ResourceCustomTarget.Create(follower.gameObject, interactionHealingBay.playerFarming.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 0.5f), (InventoryItem.ITEM_TYPE) costs[UnityEngine.Random.Range(0, costs.Count)].type, (System.Action) null);
      }
      yield return (object) null;
    }
    follower.Brain.ClearPersonalOverrideTaskProvider();
    Interaction_HealingBay.HealFollower(follower.Brain);
    yield return (object) new WaitForSeconds(0.5f);
    if (wasRotten)
      AudioManager.Instance.PlayOneShot("event:/dlc/ritual/healdecay_transform");
    FollowerBrain.SetFollowerCostume(follower.Spine.skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
    follower.SimpleAnimator.Animate("idle", 1, true, 0.0f);
    double num = (double) follower.SetBodyAnimation("Reactions/react-happy1", false);
    follower.SetFaceAnimation("Reactions/react-happy1", false);
    yield return (object) new WaitForSeconds(0.5f);
    CameraManager.shakeCamera(1.5f, (float) UnityEngine.Random.Range(0, 360));
    BiomeConstants.Instance.EmitHeartPickUpVFX(follower.gameObject.transform.position, 0.0f, "black", "burst_big");
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", follower.gameObject);
    yield return (object) new WaitForSeconds(1.6f);
    SimulationManager.UnPause();
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.Brain.CheckChangeState();
    follower.Brain.CompleteCurrentTask();
    follower.Brain.CurrentState.SetStateAnimations(follower, true);
    GameManager.GetInstance().OnConversationEnd();
    ObjectiveManager.CheckObjectives(Objectives.TYPES.SEND_FOLLOWER_BED_REST);
    interactionHealingBay.Activated = false;
    interactionHealingBay.structureBrain.ReservedByPlayer = false;
  }

  public IEnumerator HealingRoutineFromMedic(Follower medic, Follower follower, System.Action callback)
  {
    this.Activated = true;
    medic.GoTo(this.EntrancePosition.transform.position + Vector3.right, (System.Action) (() =>
    {
      medic.FacePosition(follower.transform.position);
      medic.TimedAnimation("action", 4.5f);
    }));
    follower.transform.position = this.FollowerPosition.transform.position;
    double num1 = (double) follower.SetBodyAnimation("sleep_bedrest_justhead", true);
    yield return (object) new WaitForSeconds(1f);
    int cursedState = (int) follower.Brain.Info.CursedState;
    float illness = follower.Brain._directInfoAccess.Illness;
    float injury = follower.Brain._directInfoAccess.Injured;
    List<InventoryItem> costs = Interaction_HealingBay.GetCost(follower.Brain, this.structureBrain.Data.Type == StructureBrain.TYPES.HEALING_BAY_2);
    float t = 0.0f;
    float duration = 3f;
    while ((double) t < (double) duration)
    {
      if ((double) Time.deltaTime > 0.0)
      {
        t += Time.deltaTime;
        float t1 = t / duration;
        follower.Brain.Stats.Illness = Mathf.Lerp(illness, 0.0f, t1);
        follower.Brain.Stats.Injured = Mathf.Lerp(injury, 0.0f, t1);
        if (Time.frameCount % 10 == 0 && (double) t > 0.5 && (double) t < (double) duration - 0.5)
          ResourceCustomTarget.Create(follower.gameObject, medic.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 0.5f), (InventoryItem.ITEM_TYPE) costs[UnityEngine.Random.Range(0, costs.Count)].type, (System.Action) null);
      }
      yield return (object) null;
    }
    Interaction_HealingBay.HealFollower(follower.Brain);
    yield return (object) new WaitForSeconds(0.5f);
    FollowerBrain.SetFollowerCostume(follower.Spine.skeleton, follower.Brain._directInfoAccess, forceUpdate: true);
    follower.SimpleAnimator.Animate("idle", 1, true, 0.0f);
    double num2 = (double) follower.SetBodyAnimation("Reactions/react-happy1", false);
    follower.SetFaceAnimation("Reactions/react-happy1", false);
    BiomeConstants.Instance.EmitHeartPickUpVFX(follower.gameObject.transform.position, 0.0f, "black", "burst_big");
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", follower.gameObject);
    follower.SimpleAnimator.ResetAnimationsToDefaults();
    follower.Brain.CheckChangeState();
    follower.Brain.CompleteCurrentTask();
    follower.Brain.CurrentState.SetStateAnimations(follower, true);
    follower.Brain.ClearPersonalOverrideTaskProvider();
    this.Activated = false;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void OnHidden()
  {
    this.Activated = false;
    Time.timeScale = 1f;
    HUD_Manager.Instance.Show();
    GameManager.GetInstance().OnConversationEnd();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_HealingBay.HealingBays.Remove(this);
  }

  public static void HealFollower(FollowerBrain brain)
  {
    brain._directInfoAccess.Illness = 0.0f;
    brain._directInfoAccess.Injured = 0.0f;
    brain.DiedOfIllness = false;
    brain.DiedOfInjury = false;
    brain.DiedFromRot = false;
    if (brain.HasTrait(FollowerTrait.TraitType.Zombie))
      brain.RemoveTrait(FollowerTrait.TraitType.Zombie, true);
    if (brain.HasTrait(FollowerTrait.TraitType.Mutated) && DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.RemoveRot))
    {
      brain.RemoveTrait(FollowerTrait.TraitType.Mutated, true);
      brain.AddThought(Thought.RemoveRot);
      Follower followerById = FollowerManager.FindFollowerByID(brain.Info.ID);
      if ((bool) (UnityEngine.Object) followerById)
        followerById.Interaction_FollowerInteraction.eventListener.RefreshRotValue();
    }
    if (brain.HasTrait(FollowerTrait.TraitType.ExistentialDread))
      brain.RemoveTrait(FollowerTrait.TraitType.ExistentialDread, true);
    if (brain.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      brain.RemoveTrait(FollowerTrait.TraitType.MissionaryTerrified, true);
    if (brain.Info.CursedState == Thought.Ill)
    {
      brain.RemoveCurseState(Thought.Ill);
      FollowerBrainStats.StatStateChangedEvent illnessStateChanged = FollowerBrainStats.OnIllnessStateChanged;
      if (illnessStateChanged != null)
        illnessStateChanged(brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
    }
    if (brain.Info.CursedState != Thought.Injured)
      return;
    brain.RemoveCurseState(Thought.Injured);
    FollowerBrainStats.StatStateChangedEvent injuredStateChanged = FollowerBrainStats.OnInjuredStateChanged;
    if (injuredStateChanged == null)
      return;
    injuredStateChanged(brain.Info.ID, FollowerStatState.Off, FollowerStatState.On);
  }

  public static List<InventoryItem> GetCost(FollowerBrain brain, bool isUpgraded)
  {
    if (brain == null)
      return new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, isUpgraded ? 10 : 15)
      };
    if (brain.Info.HasTrait(FollowerTrait.TraitType.Zombie))
      return new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, isUpgraded ? 10 : 20),
        new InventoryItem(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, isUpgraded ? 10 : 20),
        new InventoryItem(InventoryItem.ITEM_TYPE.CRYSTAL, isUpgraded ? 5 : 10)
      };
    if (brain.Info.HasTrait(FollowerTrait.TraitType.Mutated))
      return new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1)
      };
    if (brain.Info.HasTrait(FollowerTrait.TraitType.ExistentialDread))
      return new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.CRYSTAL, isUpgraded ? 5 : 10)
      };
    if (brain.Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
      return new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.CRYSTAL, isUpgraded ? 5 : 10)
      };
    if (brain.Info.CursedState == Thought.Ill)
      return new List<InventoryItem>()
      {
        new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, isUpgraded ? 10 : 15)
      };
    return new List<InventoryItem>()
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.FLOWER_RED, isUpgraded ? 10 : 15)
    };
  }

  public void SetActivated(bool activated) => this.Activated = activated;
}
