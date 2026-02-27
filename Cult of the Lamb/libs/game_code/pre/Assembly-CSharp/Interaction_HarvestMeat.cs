// Decompiled with JetBrains decompiler
// Type: Interaction_HarvestMeat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.UI.Overlays.TutorialOverlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_HarvestMeat : Interaction
{
  public static List<Interaction_HarvestMeat> Interaction_HarvestMeats = new List<Interaction_HarvestMeat>();
  public static Interaction_HarvestMeat CurrentMovingBody;
  public Structure structure;
  public MeshRenderer meshRenderer;
  public DeadWorshipper DeadWorshipper;
  public DeadWorshipper DeadWorshipperTmp;
  [SerializeField]
  private SpriteRenderer _indicator;
  private string sHarvestMeat;
  private string sHarvestRottenMeat;
  private string sPrepareForBurial;
  private string sPickup;
  private string sBury;
  private string sCompost;
  private bool CarryingBody;
  private Grave ClosestGrave;
  private Interaction_CompostBinDeadBody ClosestCompost;
  private float ClosestPosition = 100f;
  private bool FoundOne;
  private bool FoundCompost;
  private bool NearGraveWithBody;
  private bool NearCompostWithBody;
  private bool addedOutline;

  private void Start()
  {
    if (this.DeadWorshipper.StructureInfo != null && this.DeadWorshipper.StructureInfo.BodyWrapped)
    {
      if (this.DeadWorshipper.followerInfo != null && this.DeadWorshipper.followerInfo.Necklace != InventoryItem.ITEM_TYPE.NONE)
        this.HasSecondaryInteraction = true;
      else
        this.HasSecondaryInteraction = false;
    }
    else
      this.HasSecondaryInteraction = true;
    this.UpdateLocalisation();
    this.CarryingBody = false;
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  private void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.DeleteDuplicateBodies();
  }

  private void DeleteDuplicateBodies()
  {
    for (int index = Interaction_HarvestMeat.Interaction_HarvestMeats.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index] != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index].structure != (UnityEngine.Object) null && Interaction_HarvestMeat.Interaction_HarvestMeats[index].structure.Brain != null && (UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index] != (UnityEngine.Object) this && (UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index].DeadWorshipper != (UnityEngine.Object) null && Interaction_HarvestMeat.Interaction_HarvestMeats[index].DeadWorshipper.followerInfo != null && Interaction_HarvestMeat.Interaction_HarvestMeats[index].DeadWorshipper.followerInfo.ID == this.DeadWorshipper.followerInfo.ID)
        StructureManager.RemoveStructure(Interaction_HarvestMeat.Interaction_HarvestMeats[index].structure.Brain);
    }
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    if (!(bool) (UnityEngine.Object) this.structure)
      return;
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sHarvestMeat = ScriptLocalization.Interactions.HarvestMeat;
    this.sHarvestRottenMeat = ScriptLocalization.Interactions.HarvestRottenMeat;
    this.sPrepareForBurial = ScriptLocalization.Interactions.PrepareForBurial;
    this.sPickup = ScriptLocalization.Interactions.PickUp;
    this.sBury = ScriptLocalization.Interactions.BuryBody;
    this.sCompost = ScriptLocalization.Interactions.CompostBody;
  }

  public override void GetLabel()
  {
    if (!this.NearGraveWithBody && !this.NearCompostWithBody)
    {
      if ((UnityEngine.Object) this.DeadWorshipper != (UnityEngine.Object) null && this.DeadWorshipper.StructureInfo != null && this.DeadWorshipper.StructureInfo.BodyWrapped)
        this.Label = this.sPickup;
      else
        this.Label = this.sPrepareForBurial;
    }
    else if (this.NearGraveWithBody)
    {
      this.Label = this.sBury;
    }
    else
    {
      if (!this.NearCompostWithBody)
        return;
      this.Label = this.sCompost;
    }
  }

  public bool Rotten
  {
    get
    {
      return !((UnityEngine.Object) this.DeadWorshipper == (UnityEngine.Object) null) && this.DeadWorshipper.StructureInfo != null && this.DeadWorshipper.StructureInfo.Rotten;
    }
  }

  public override void GetSecondaryLabel()
  {
    if (this.DeadWorshipper.StructureInfo != null)
    {
      if (this.DeadWorshipper.StructureInfo.BodyWrapped)
      {
        if (this.DeadWorshipper.followerInfo != null && this.DeadWorshipper.followerInfo.Necklace != InventoryItem.ITEM_TYPE.NONE)
        {
          this.SecondaryInteractable = true;
          this.SecondaryLabel = ScriptLocalization.Interactions.TakeLoot;
        }
        else
        {
          this.SecondaryInteractable = false;
          this.SecondaryLabel = "";
        }
      }
      else if (this.DeadWorshipper.StructureInfo.Rotten)
        this.SecondaryLabel = this.sHarvestRottenMeat;
      else
        this.SecondaryLabel = this.sHarvestMeat;
    }
    else
      this.SecondaryLabel = "";
  }

  public override void OnInteract(StateMachine state)
  {
    if (!DataManager.Instance.OnboardedDeadFollower)
    {
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.DeadFollower))
      {
        UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.DeadFollower);
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
        {
          if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_BodyPit))
            ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuryDead", UpgradeSystem.Type.Building_BodyPit));
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuryDead", Objectives.CustomQuestTypes.BuryBody));
        });
      }
      DataManager.Instance.OnboardedDeadFollower = true;
    }
    else
    {
      if (this.CarryingBody)
        return;
      base.OnInteract(state);
      if (!this.DeadWorshipper.StructureInfo.BodyWrapped)
      {
        this.StartCoroutine((IEnumerator) this.PrepareForBurial());
      }
      else
      {
        this.DeadWorshipperTmp = this.DeadWorshipper;
        Debug.Log((object) ("PICKUP! DeadWorshipperTmp " + (object) this.DeadWorshipperTmp.StructureInfo.FollowerID));
        this.structure.enabled = false;
        this.DeadWorshipper.enabled = false;
        this.StartCoroutine((IEnumerator) this.PickUpBody());
      }
      foreach (Structures_Prison structuresPrison in StructureManager.GetAllStructuresOfType<Structures_Prison>())
      {
        if (structuresPrison.Data.FollowerID == this.DeadWorshipper.StructureInfo.FollowerID)
          structuresPrison.Data.FollowerID = -1;
      }
    }
  }

  protected override void OnEnable()
  {
    base.OnEnable();
    Interaction_HarvestMeat.Interaction_HarvestMeats.Add(this);
  }

  public override void OnDisableInteraction()
  {
    if (this.CarryingBody && (UnityEngine.Object) this.structure != (UnityEngine.Object) null && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      Debug.Log((object) "Carrying body! ");
      Debug.Log((object) ("Drop body ID: " + (object) this.DeadWorshipperTmp.StructureInfo.FollowerID));
      StructuresData structure = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
      structure.BodyWrapped = true;
      structure.FollowerID = this.DeadWorshipperTmp.StructureInfo.FollowerID;
      this.CarryingBody = false;
      StructureManager.BuildStructure(FollowerLocation.Base, structure, PlayerFarming.Instance.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
      {
        DeadWorshipper component = g.GetComponent<DeadWorshipper>();
        component.WrapBody();
        component.Setup();
        PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(structure.Position);
        if (tileAtWorldPosition != null)
          component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }), (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
    }
    Interaction_HarvestMeat.Interaction_HarvestMeats.Remove(this);
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    if (this.DeadWorshipper.StructureInfo.BodyWrapped)
      return;
    string deathText = this.DeadWorshipper.followerInfo.GetDeathText();
    if (string.IsNullOrEmpty(deathText))
      return;
    MonoSingleton<Indicator>.Instance.ShowTopInfo(deathText);
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    MonoSingleton<Indicator>.Instance.HideTopInfo();
  }

  private new void Update()
  {
    if (!this.CarryingBody)
      return;
    this.FoundOne = false;
    this.FoundCompost = false;
    this.NearGraveWithBody = false;
    this.NearCompostWithBody = false;
    PlayerFarming.Instance.NearGrave = false;
    PlayerFarming.Instance.NearCompostBody = false;
    this.ClosestPosition = 100f;
    foreach (Grave grave in Grave.Graves)
    {
      float num = Vector3.Distance(grave.gameObject.transform.position, PlayerFarming.Instance.gameObject.transform.position);
      if ((double) num < 0.44999998807907104 && grave.StructureInfo.FollowerID == -1)
      {
        if ((double) num < (double) this.ClosestPosition)
        {
          this.ClosestPosition = num;
          this.ClosestGrave = grave;
          if (!this.addedOutline)
          {
            this.Outliner.OutlineLayers[0].Clear();
            this.Outliner.OutlineLayers[0].Add(this.ClosestGrave.gameObject);
            this.addedOutline = true;
          }
        }
        this.FoundOne = true;
        this.ClosestGrave = grave;
      }
    }
    if (this.FoundOne)
    {
      PlayerFarming.Instance.NearGrave = true;
      this.NearGraveWithBody = true;
      this.GetLabel();
    }
    if (this.FoundOne)
      return;
    if ((UnityEngine.Object) this.ClosestGrave != (UnityEngine.Object) null && this.addedOutline)
    {
      this.Outliner.OutlineLayers[0].Clear();
      this.addedOutline = false;
    }
    this.ClosestPosition = 100f;
    foreach (Interaction_CompostBinDeadBody compostBinDeadBody in Interaction_CompostBinDeadBody.DeadBodyCompost)
    {
      float message = Vector3.Distance(compostBinDeadBody.transform.position, PlayerFarming.Instance.transform.position);
      Debug.Log((object) message);
      if ((double) message < 1.5 && compostBinDeadBody.StructureBrain.CompostCount <= 0 && compostBinDeadBody.StructureBrain.PoopCount <= 0)
      {
        Debug.Log((object) ("1 " + (object) compostBinDeadBody));
        if ((double) message < (double) this.ClosestPosition)
        {
          this.ClosestPosition = message;
          this.ClosestCompost = compostBinDeadBody;
          this.FoundCompost = true;
        }
      }
    }
    if (!this.FoundCompost)
      return;
    PlayerFarming.Instance.NearCompostBody = true;
    this.NearCompostWithBody = true;
    this.GetLabel();
  }

  private IEnumerator PickUpBody()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    interactionHarvestMeat.DeadWorshipper.StructureInfo.Animation = "dead";
    Interaction_HarvestMeat.CurrentMovingBody = interactionHarvestMeat;
    interactionHarvestMeat._indicator.DOColor(Color.black, 0.5f);
    interactionHarvestMeat.CarryingBody = true;
    AudioManager.Instance.PlayOneShot("event:/player/body_pickup", interactionHarvestMeat.gameObject);
    PlayerFarming.Instance.CarryingDeadFollowerID = interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID;
    bool wasGoopDoorOpen = BaseGoopDoor.Instance.IsOpen;
    BaseGoopDoor.Instance.DoorUp();
    interactionHarvestMeat.Label = ScriptLocalization.Interactions.Drop;
    interactionHarvestMeat.meshRenderer.gameObject.SetActive(false);
    interactionHarvestMeat._indicator.gameObject.SetActive(false);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
    while (!InputManager.Gameplay.GetInteractButtonUp())
      yield return (object) null;
    while (!InputManager.Gameplay.GetInteractButtonHeld())
      yield return (object) null;
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(interactionHarvestMeat.DeadWorshipper.transform.position);
    if (tileAtWorldPosition != null)
      interactionHarvestMeat.DeadWorshipper.Structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
    StructureManager.RemoveStructure(interactionHarvestMeat.DeadWorshipper.Structure.Brain);
    Interaction_HarvestMeat.CurrentMovingBody = (Interaction_HarvestMeat) null;
    if (interactionHarvestMeat.FoundOne)
    {
      interactionHarvestMeat.ClosestGrave.OutlineEffect.OutlineLayers[0].Clear();
      interactionHarvestMeat.ClosestGrave.OutlineEffect.RemoveGameObject(interactionHarvestMeat.ClosestGrave.OutlineTarget);
      interactionHarvestMeat.ClosestGrave.StructureInfo.FollowerID = interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID;
      interactionHarvestMeat.ClosestGrave.SetGameObjects();
      foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
      {
        if (followerInfo.ID == interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID)
        {
          followerInfo.LastPosition = interactionHarvestMeat.ClosestGrave.StructureBrain.Data.Position;
          break;
        }
      }
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.ClosestGrave.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", interactionHarvestMeat.gameObject);
      CultFaithManager.AddThought(Thought.Cult_FollowerBuried, PlayerFarming.Instance.CarryingDeadFollowerID);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      interactionHarvestMeat.CarryingBody = false;
      PlayerFarming.Instance.CarryingDeadFollowerID = -1;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuryBody);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat.gameObject);
    }
    else if (interactionHarvestMeat.FoundCompost)
    {
      interactionHarvestMeat.ClosestCompost.BuryBody();
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.ClosestCompost.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", interactionHarvestMeat.gameObject);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      interactionHarvestMeat.CarryingBody = false;
      PlayerFarming.Instance.CarryingDeadFollowerID = -1;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuryBody);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat.gameObject);
    }
    else
      interactionHarvestMeat.DropBody();
    if (wasGoopDoorOpen)
      BaseGoopDoor.Instance.DoorDown();
  }

  public void DropBody()
  {
    Interaction_HarvestMeat.CurrentMovingBody = (Interaction_HarvestMeat) null;
    if (!this.CarryingBody)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/body_drop", this.gameObject);
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.BodyWrapped = true;
    infoByType.FollowerID = this.DeadWorshipperTmp.StructureInfo.FollowerID;
    this.CarryingBody = false;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, PlayerFarming.Instance.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      DeadWorshipper component = g.GetComponent<DeadWorshipper>();
      component.WrapBody();
      component.Setup();
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
      PlayerFarming.Instance.CarryingDeadFollowerID = -1;
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(component.transform.position);
      if (tileAtWorldPosition != null)
        component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }), (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  private IEnumerator PrepareForBurial()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    MonoSingleton<Indicator>.Instance.HideTopInfo();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/player/body_wrap", interactionHarvestMeat.gameObject);
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/player/body_wrap_done", interactionHarvestMeat.gameObject);
    interactionHarvestMeat.DeadWorshipper.WrapBody();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionHarvestMeat.HasChanged = true;
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (!DataManager.Instance.OnboardedDeadFollower)
    {
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.DeadFollower))
      {
        UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.DeadFollower);
        overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() =>
        {
          if (!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Building_BodyPit))
            ObjectiveManager.Add((ObjectivesData) new Objectives_UnlockUpgrade("Objectives/GroupTitles/BuryDead", UpgradeSystem.Type.Building_BodyPit));
          ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/BuryDead", Objectives.CustomQuestTypes.BuryBody));
        });
      }
      DataManager.Instance.OnboardedDeadFollower = true;
    }
    else if (!this.DeadWorshipper.StructureInfo.BodyWrapped)
    {
      base.OnSecondaryInteract(state);
      this.StartCoroutine((IEnumerator) this.HarvestMeatIE());
    }
    else
    {
      if (this.DeadWorshipper.followerInfo == null || this.DeadWorshipper.followerInfo.Necklace == InventoryItem.ITEM_TYPE.NONE)
        return;
      base.OnSecondaryInteract(state);
      this.StartCoroutine((IEnumerator) this.LootBody());
    }
  }

  private IEnumerator HarvestMeatIE()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    MonoSingleton<Indicator>.Instance.HideTopInfo();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    PlayerFarming.Instance.GoToAndStop(interactionHarvestMeat.transform.position, interactionHarvestMeat.gameObject);
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat", interactionHarvestMeat.gameObject);
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
    PlayerFarming.Instance.CustomAnimation("actions/harvest_meat", true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    interactionHarvestMeat.meshRenderer.enabled = false;
    AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", interactionHarvestMeat.gameObject);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 5, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 2, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
    if (interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace != InventoryItem.ITEM_TYPE.NONE)
      InventoryItem.Spawn(interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace, 1, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
    interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace = InventoryItem.ITEM_TYPE.NONE;
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.Idle;
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      if (allBrain.Location == PlayerFarming.Location)
      {
        if (allBrain.CurrentTaskType == FollowerTaskType.Sleep || allBrain.CurrentTaskType == FollowerTaskType.SleepBedRest)
          allBrain.AddThought(Thought.SleptThroughYouButcheringDeadFollower);
        else if (allBrain.HasTrait(FollowerTrait.TraitType.Cannibal))
          allBrain.AddThought(Thought.SawYouButcheringDeadFollowerCannibalTrait);
        else
          allBrain.AddThought(Thought.SawYouButcheringDeadFollower);
      }
    }
    JudgementMeter.ShowModify(-1);
    ObjectiveManager.FailCustomObjective(Objectives.CustomQuestTypes.BuryBody);
    if (!TimeManager.IsNight)
      CultFaithManager.AddThought(Thought.Cult_ButcheredFollowerMeat, faithMultiplier: DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.Cannibal) ? 0.0f : 1f);
    StructureManager.RemoveStructure(interactionHarvestMeat.DeadWorshipper.Structure.Brain);
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat.gameObject);
  }

  private IEnumerator LootBody()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForEndOfFrame();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    InventoryItem.Spawn(interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace, 1, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
    interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace = InventoryItem.ITEM_TYPE.NONE;
    interactionHarvestMeat.DeadWorshipper.SetOutfit();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.Idle;
    CultFaithManager.AddThought(Thought.Cult_LootedCorpse);
  }
}
