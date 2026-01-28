// Decompiled with JetBrains decompiler
// Type: Interaction_TraitManipulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class Interaction_TraitManipulator : Interaction
{
  public static List<Interaction_TraitManipulator> TraitManipulators = new List<Interaction_TraitManipulator>();
  public Structure Structure;
  [SerializeField]
  public GameObject followerPosition;
  [SerializeField]
  public GameObject targetFollowerPosition;
  [SerializeField]
  public Canvas capacityIndicatorCanvas;
  [SerializeField]
  public Image capacityIndicator;
  [SerializeField]
  public GameObject onEffects;
  public string fireCrackleLoopSFX = "event:/dlc/building/exorcismaltar/exorcism_fire_crackle_loop";
  public EventInstance fireCrackleLoopInstance;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_TraitManipulator StructureBrain
  {
    get => this.Structure.Brain as Structures_TraitManipulator;
  }

  public GameObject FollowerPosition => this.followerPosition;

  public GameObject TargetFollowerPosition => this.targetFollowerPosition;

  public void Start()
  {
    this.Structure.OnBrainRemoved += new System.Action(this.OnBrainRemoved);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Structure.OnBrainRemoved -= new System.Action(this.OnBrainRemoved);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (!DataManager.Instance.Followers_TraitManipulating_IDs.Contains(this.Structure.Brain.Data.FollowerID))
      this.Structure.Brain.Data.FollowerID = -1;
    Structures_TraitManipulator structureBrain = this.StructureBrain;
    if ((structureBrain != null ? (structureBrain.Data.FollowerID != -1 ? 1 : 0) : 1) == 0)
      return;
    this.TurnEffectsOn();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_TraitManipulator.TraitManipulators.Add(this);
    if (this.StructureBrain == null || this.StructureBrain.Data.FollowerID == -1)
      return;
    this.TurnEffectsOn();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_TraitManipulator.TraitManipulators.Remove(this);
    AudioManager.Instance.StopLoop(this.fireCrackleLoopInstance);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = true;
    this.Label = ScriptLocalization.FollowerInteractions.MakeDemand;
  }

  public override void Update()
  {
    base.Update();
    if (this.Structure.Brain == null)
      return;
    this.DisplayUI();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    GameManager.GetInstance().OnConversationNew();
    SimulationManager.Pause();
    if (this.StructureInfo.FollowerID != -1 && (double) this.StructureInfo.Progress >= (double) this.StructureBrain.Duration)
    {
      AudioManager.Instance.PlayOneShot("event:/ui/open_menu");
      UITraitManipulatorResultsScreen menu = MonoSingleton<UIManager>.Instance.TraitManipulatorResultMenuTemplate.Instantiate<UITraitManipulatorResultsScreen>();
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID);
      menu.Show(infoById, infoById.TraitManipulateType);
      MonoSingleton<UIManager>.Instance.SetMenuInstance((UIMenuBase) menu);
      UITraitManipulatorResultsScreen manipulatorResultsScreen = menu;
      manipulatorResultsScreen.OnHide = manipulatorResultsScreen.OnHide + (System.Action) (() => this.StartCoroutine((IEnumerator) this.CompletedIE()));
    }
    else if (this.StructureInfo.FollowerID != -1)
    {
      UITraitManipulatorInProgressMenuController menu = MonoSingleton<UIManager>.Instance.TraitManipulatorInProgressMenuTemplate.Instantiate<UITraitManipulatorInProgressMenuController>();
      FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureInfo.FollowerID);
      menu.Show(infoById, (global::StructureBrain) this.StructureBrain);
      MonoSingleton<UIManager>.Instance.SetMenuInstance((UIMenuBase) menu);
      UITraitManipulatorInProgressMenuController progressMenuController = menu;
      progressMenuController.OnHide = progressMenuController.OnHide + (System.Action) (() => this.EndTraitManipulator());
    }
    else
    {
      List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
      foreach (Follower follower in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
      {
        if (!follower.Brain.Info.IsSnowman)
          followerSelectEntries.Add(new FollowerSelectEntry(follower, FollowerManager.GetFollowerAvailabilityStatus(follower.Brain)));
      }
      UITraitManipulatorMenuController menu = MonoSingleton<UIManager>.Instance.TraitManipulatorMenuTemplate.Instantiate<UITraitManipulatorMenuController>();
      menu.Show(followerSelectEntries, (global::StructureBrain) this.StructureBrain);
      MonoSingleton<UIManager>.Instance.SetMenuInstance((UIMenuBase) menu);
      menu.OnFollowerChosen += (System.Action<FollowerInfo, UITraitManipulatorMenuController.Type, FollowerTrait.TraitType>) ((info, selectionType, traitType) => this.ManipulateFollower(FollowerBrain.GetOrCreateBrain(info), selectionType, traitType));
      UITraitManipulatorMenuController manipulatorMenuController1 = menu;
      manipulatorMenuController1.OnCancel = manipulatorMenuController1.OnCancel + (System.Action) (() => this.EndTraitManipulator());
      UITraitManipulatorMenuController manipulatorMenuController2 = menu;
      manipulatorMenuController2.OnShow = manipulatorMenuController2.OnShow + (System.Action) (() =>
      {
        foreach (FollowerInformationBox followerInfoBox in menu.FollowerInfoBoxes)
          followerInfoBox.ShowItemCostValue(Interaction_TraitManipulator.GetCost());
      });
      UITraitManipulatorMenuController manipulatorMenuController3 = menu;
      manipulatorMenuController3.OnShownCompleted = manipulatorMenuController3.OnShownCompleted + (System.Action) (() =>
      {
        foreach (FollowerInformationBox followerInfoBox in menu.FollowerInfoBoxes)
          followerInfoBox.ShowItemCostValue(Interaction_TraitManipulator.GetCost());
      });
    }
  }

  public void TurnEffectsOn()
  {
    this.onEffects.SetActive(true);
    AudioManager.Instance.StopLoop(this.fireCrackleLoopInstance);
    this.fireCrackleLoopInstance = AudioManager.Instance.CreateLoop(this.fireCrackleLoopSFX, this.gameObject, true);
  }

  public void TurnEffectsOff()
  {
    this.onEffects.SetActive(false);
    AudioManager.Instance.StopLoop(this.fireCrackleLoopInstance);
  }

  public void ManipulateFollower(
    FollowerBrain follower,
    UITraitManipulatorMenuController.Type selectionType,
    FollowerTrait.TraitType traitType)
  {
    bool flag1 = true;
    List<InventoryItem> cost = Interaction_TraitManipulator.GetCost();
    foreach (InventoryItem inventoryItem in cost)
    {
      if (Inventory.GetItemQuantity(inventoryItem.type) < inventoryItem.quantity)
        flag1 = false;
    }
    bool flag2 = selectionType != UITraitManipulatorMenuController.Type.Remove || !FollowerTrait.UniqueTraits.Contains(traitType);
    if (flag1 & flag2)
    {
      if (selectionType == UITraitManipulatorMenuController.Type.Shuffle)
        follower._directInfoAccess.TargetTraits = follower._directInfoAccess.RandomisedTraits(follower._directInfoAccess.ID + TimeManager.CurrentDay);
      else
        follower._directInfoAccess.TargetTraits = new List<FollowerTrait.TraitType>()
        {
          traitType
        };
      follower._directInfoAccess.TraitManipulateType = selectionType;
      this.StructureInfo.Progress = 0.0f;
      this.StructureInfo.FollowerID = follower.Info.ID;
      this.StartCoroutine((IEnumerator) this.ManipulateFollowerIE(follower));
      foreach (InventoryItem inventoryItem in cost)
        Inventory.ChangeItemQuantity(inventoryItem.type, -inventoryItem.quantity);
    }
    else
      this.EndTraitManipulator();
  }

  public IEnumerator ManipulateFollowerIE(FollowerBrain follower)
  {
    Interaction_TraitManipulator traitManipulator = this;
    yield return (object) new WaitForEndOfFrame();
    traitManipulator.TurnEffectsOn();
    Follower f = FollowerManager.FindFollowerByID(follower.Info.ID);
    follower.CompleteCurrentTask();
    follower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    f.transform.position = traitManipulator.transform.position;
    yield return (object) new WaitForEndOfFrame();
    f.LockToGround = false;
    f.transform.position = traitManipulator.TargetFollowerPosition.transform.position;
    f.SimpleAnimator.ChangeStateAnimation(StateMachine.State.Idle, "Rituals/lobotomy-rehabilitating");
    double num = (double) f.SetBodyAnimation("Rituals/lobotomy-rehabilitating", true);
    if (!DataManager.Instance.Followers_TraitManipulating_IDs.Contains(follower.Info.ID))
      DataManager.Instance.Followers_TraitManipulating_IDs.Add(follower.Info.ID);
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/scared_short", f.transform.position);
    AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_follower_strap_on", traitManipulator.transform.position);
    traitManipulator.EndTraitManipulator();
  }

  public IEnumerator CompletedIE()
  {
    Interaction_TraitManipulator traitManipulator = this;
    DataManager.Instance.Followers_TraitManipulating_IDs.Remove(traitManipulator.StructureInfo.FollowerID);
    Follower followerById = FollowerManager.FindFollowerByID(traitManipulator.StructureInfo.FollowerID);
    followerById.Brain.CompleteCurrentTask();
    followerById.Brain.CheckChangeState();
    traitManipulator.TurnEffectsOff();
    if (followerById.Brain._directInfoAccess.TraitManipulateType == UITraitManipulatorMenuController.Type.Shuffle)
      followerById.Brain._directInfoAccess.Traits = new List<FollowerTrait.TraitType>((IEnumerable<FollowerTrait.TraitType>) followerById.Brain._directInfoAccess.TargetTraits);
    else if (followerById.Brain._directInfoAccess.TraitManipulateType == UITraitManipulatorMenuController.Type.Remove)
      followerById.Brain._directInfoAccess.Traits.Remove(followerById.Brain._directInfoAccess.TargetTraits[0]);
    else if (followerById.Brain._directInfoAccess.TraitManipulateType == UITraitManipulatorMenuController.Type.Add)
      followerById.Brain._directInfoAccess.Traits.Add(followerById.Brain._directInfoAccess.TargetTraits[0]);
    followerById.Brain._directInfoAccess.LeavingCult = false;
    followerById.Brain.CheckChangeState();
    followerById.ResetStateAnimations();
    followerById.Brain._directInfoAccess.TargetTraits.Clear();
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.ChangeTraits, followerById.Brain.Info.ID);
    traitManipulator.StructureInfo.FollowerID = -1;
    traitManipulator.StructureInfo.Progress = 0.0f;
    FollowerBrain.SetFollowerCostume(followerById.Spine.Skeleton, followerById.Brain._directInfoAccess, forceUpdate: true);
    yield return (object) new WaitForEndOfFrame();
    traitManipulator.EndTraitManipulator();
    traitManipulator.HasChanged = true;
  }

  public void DisplayUI()
  {
    this.capacityIndicatorCanvas.gameObject.SetActive(this.StructureInfo.FollowerID != -1 && (double) this.StructureInfo.Progress < (double) this.StructureBrain.Duration);
    this.capacityIndicator.fillAmount = this.StructureInfo.Progress / this.StructureBrain.Duration;
  }

  public static List<InventoryItem> GetCost()
  {
    return new List<InventoryItem>()
    {
      new InventoryItem(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD, 20)
    };
  }

  public void OnStructureCreated(StructuresData structure)
  {
    if (!((UnityEngine.Object) this.Structure != (UnityEngine.Object) null) || this.Structure.Brain == null || structure != this.Structure.Brain.Data)
      return;
    this.Structure.Brain.Data.FollowerID = -1;
  }

  public void OnBrainRemoved()
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(this.StructureBrain.Data.FollowerID);
    if (infoById == null)
      return;
    if (DataManager.Instance.Followers_TraitManipulating_IDs.Contains(infoById.ID))
      DataManager.Instance.Followers_TraitManipulating_IDs.Remove(infoById.ID);
    FollowerBrain.GetOrCreateBrain(infoById)?.CompleteCurrentTask();
  }

  public void EndTraitManipulator()
  {
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__24_0()
  {
    this.StartCoroutine((IEnumerator) this.CompletedIE());
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__24_1() => this.EndTraitManipulator();

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__24_2(
    FollowerInfo info,
    UITraitManipulatorMenuController.Type selectionType,
    FollowerTrait.TraitType traitType)
  {
    this.ManipulateFollower(FollowerBrain.GetOrCreateBrain(info), selectionType, traitType);
  }

  [CompilerGenerated]
  public void \u003COnInteract\u003Eb__24_3() => this.EndTraitManipulator();
}
