// Decompiled with JetBrains decompiler
// Type: Interaction_HarvestMeat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
  public SpriteRenderer _indicator;
  public UIRebuildBedMinigameOverlayController _uiCookingMinigameOverlayController;
  public bool harvesting;
  public string sHarvestMeat;
  public string sHarvestRottenMeat;
  public string sPrepareForBurial;
  public string sPickup;
  public string sBury;
  public string sCompost;
  public RaycastHit LockToGroundHit;
  public Vector3 LockToGroundPosition;
  public Vector3 LockToGroundNewPosition;
  public bool CarryingBody;
  public Interaction previousStructure;
  public StructureBrain previousBrain;
  public Grave ClosestGrave;
  public Interaction_Crypt ClosestCrypt;
  public Interaction_CompostBinDeadBody ClosestCompost;
  public Interaction_DLCFurnace ClosestFurnace;
  public float ClosestPosition = 100f;
  public bool FoundOne;
  public bool FoundCompost;
  public bool FoundFurnace;
  public bool NearGraveWithBody;
  public bool NearCompostWithBody;
  public bool NearFurnaceWithBody;

  public bool canPickUpLegendaryAxe
  {
    get
    {
      return !DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Axe_Legendary) && (DataManager.Instance.ExecutionerDamned || DataManager.Instance.ExecutionerWoolhavenExecuted) && DataManager.Instance.NPCGhostGraveyardRescued && DataManager.Instance.OnboardedLegendaryWeapons;
    }
  }

  public void Start()
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

  public void OnBrainAssigned()
  {
    this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.DeleteDuplicateBodies();
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
    this.SeasonsManager_OnSeasonChanged(SeasonsManager.CurrentSeason);
  }

  public void DeleteDuplicateBodies()
  {
    for (int index = Interaction_HarvestMeat.Interaction_HarvestMeats.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index] != (UnityEngine.Object) null && (UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index].structure != (UnityEngine.Object) null && Interaction_HarvestMeat.Interaction_HarvestMeats[index].structure.Brain != null && (UnityEngine.Object) this.DeadWorshipper != (UnityEngine.Object) null && this.DeadWorshipper.followerInfo != null && (UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index] != (UnityEngine.Object) this && (UnityEngine.Object) Interaction_HarvestMeat.Interaction_HarvestMeats[index].DeadWorshipper != (UnityEngine.Object) null && Interaction_HarvestMeat.Interaction_HarvestMeats[index].DeadWorshipper.followerInfo != null && Interaction_HarvestMeat.Interaction_HarvestMeats[index].DeadWorshipper.followerInfo.ID == this.DeadWorshipper.followerInfo.ID)
        StructureManager.RemoveStructure(Interaction_HarvestMeat.Interaction_HarvestMeats[index].structure.Brain);
    }
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if ((bool) (UnityEngine.Object) this.structure)
      this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.SeasonsManager_OnSeasonChanged);
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
      {
        this.Label = this.sPickup;
      }
      else
      {
        this.Label = this.sPrepareForBurial;
        FollowerInfo infoById = FollowerInfo.GetInfoByID(this.DeadWorshipper.StructureInfo.FollowerID, true);
        if (infoById == null || !infoById.FrozeToDeath)
          return;
        if (DoctrineUpgradeSystem.GetUnlocked(DoctrineUpgradeSystem.DoctrineType.Special_HealingTouch))
        {
          this.Interactable = true;
          this.Label = LocalizationManager.GetTranslation("FollowerInteractions/HealingTouch");
        }
        else
        {
          this.Interactable = false;
          this.Label = "";
        }
      }
    }
    else if (this.NearGraveWithBody)
      this.Label = this.sBury;
    else if (this.NearCompostWithBody)
    {
      this.Label = this.sCompost;
    }
    else
    {
      if (!this.NearFurnaceWithBody)
        return;
      this.Label = LocalizationManager.GetTranslation("Interactions/BurnBody");
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
    this.state = state;
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
        FollowerInfo infoById = FollowerInfo.GetInfoByID(this.DeadWorshipper.StructureInfo.FollowerID, true);
        if (infoById != null && infoById.FrozeToDeath)
          this.StartCoroutine(this.InteractRoutine());
        else
          this.StartCoroutine(this.PrepareForBurial());
      }
      else
      {
        this.DeadWorshipperTmp = this.DeadWorshipper;
        Debug.Log((object) ("PICKUP! DeadWorshipperTmp " + this.DeadWorshipperTmp.StructureInfo.FollowerID.ToString()));
        this.structure.enabled = false;
        this.DeadWorshipper.enabled = false;
        this.StartCoroutine(this.PickUpBody());
      }
      foreach (Structures_Prison structuresPrison in StructureManager.GetAllStructuresOfType<Structures_Prison>())
      {
        if (structuresPrison.Data.FollowerID == this.DeadWorshipper.StructureInfo.FollowerID)
          structuresPrison.Data.FollowerID = -1;
      }
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_HarvestMeat.Interaction_HarvestMeats.Add(this);
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
  }

  public override void OnDisableInteraction()
  {
    if (this.CarryingBody && (UnityEngine.Object) this.structure != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
    {
      Debug.Log((object) "Carrying body! ");
      Debug.Log((object) ("Drop body ID: " + this.DeadWorshipperTmp.StructureInfo.FollowerID.ToString()));
      StructuresData structure = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
      structure.BodyWrapped = true;
      structure.FollowerID = this.DeadWorshipperTmp.StructureInfo.FollowerID;
      this.CarryingBody = false;
      StructureManager.BuildStructure(FollowerLocation.Base, structure, this.playerFarming.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
      {
        DeadWorshipper component = g.GetComponent<DeadWorshipper>();
        component.WrapBody();
        component.Setup();
        PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(structure.Position);
        if (tileAtWorldPosition != null && tileAtWorldPosition.CanPlaceStructure)
          component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
        UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
      }), (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
    }
    Interaction_HarvestMeat.Interaction_HarvestMeats.Remove(this);
  }

  public override void OnBecomeCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeCurrent(playerFarming);
    if (this.DeadWorshipper.StructureInfo.BodyWrapped)
      return;
    string deathText = this.DeadWorshipper.followerInfo.GetDeathText();
    if (string.IsNullOrEmpty(deathText))
      return;
    playerFarming.indicator.ShowTopInfo(deathText);
  }

  public override void OnBecomeNotCurrent(PlayerFarming playerFarming)
  {
    base.OnBecomeNotCurrent(playerFarming);
    playerFarming.indicator.HideTopInfo();
  }

  public override void Update()
  {
    base.Update();
    if (this.CarryingBody)
    {
      this.FoundOne = false;
      this.FoundCompost = false;
      this.FoundFurnace = false;
      this.NearGraveWithBody = false;
      this.NearCompostWithBody = false;
      this.NearFurnaceWithBody = false;
      this.playerFarming.NearGrave = false;
      this.playerFarming.NearStructure = (StructureBrain) null;
      this.playerFarming.NearCompostBody = false;
      this.playerFarming.NearFurnace = false;
      this.ClosestPosition = 100f;
      this.ClosestGrave = (Grave) null;
      this.ClosestCrypt = (Interaction_Crypt) null;
      if ((UnityEngine.Object) this.ClosestFurnace != (UnityEngine.Object) null)
        this.ClosestFurnace.EndIndicateHighlighted(this.playerFarming);
      this.ClosestFurnace = (Interaction_DLCFurnace) null;
      foreach (Grave grave in Grave.Graves)
      {
        float num = Vector3.Distance(grave.gameObject.transform.position, this.playerFarming.gameObject.transform.position);
        if ((double) num < 0.44999998807907104 && grave.StructureInfo.FollowerID == -1)
        {
          if ((double) num < (double) this.ClosestPosition)
          {
            this.ClosestPosition = num;
            this.ClosestGrave = grave;
            if ((UnityEngine.Object) this.previousStructure != (UnityEngine.Object) this.ClosestGrave)
              this.ClosestGrave.IndicateHighlighted(this.playerFarming);
          }
          this.FoundOne = true;
          this.ClosestGrave = grave;
        }
      }
      if (!this.FoundOne)
      {
        this.ClosestPosition = 100f;
        foreach (Interaction_Crypt crypt in Interaction_Crypt.Crypts)
        {
          float num = Vector3.Distance(crypt.gameObject.transform.position, this.playerFarming.gameObject.transform.position);
          if ((double) num < 1.0)
          {
            if ((double) num < (double) this.ClosestPosition)
            {
              this.ClosestPosition = num;
              this.ClosestCrypt = crypt;
              if ((UnityEngine.Object) this.previousStructure != (UnityEngine.Object) this.ClosestCrypt && !crypt.structureBrain.IsFull)
                this.ClosestCrypt.IndicateHighlighted(this.playerFarming);
            }
            this.FoundOne = true;
            this.ClosestCrypt = crypt;
          }
        }
      }
      if (((UnityEngine.Object) this.ClosestGrave == (UnityEngine.Object) null || this.previousBrain != this.ClosestGrave.structureBrain) && ((UnityEngine.Object) this.ClosestCrypt == (UnityEngine.Object) null || this.previousBrain != this.ClosestCrypt.structureBrain) && (UnityEngine.Object) this.previousStructure != (UnityEngine.Object) null)
      {
        if (this.previousStructure is Interaction_Crypt)
          ((Interaction_Crypt) this.previousStructure).SetDoors(false);
        this.previousStructure.EndIndicateHighlighted(this.playerFarming);
        this.previousStructure = (Interaction) null;
        this.previousBrain = (StructureBrain) null;
      }
      if (this.FoundOne)
      {
        this.playerFarming.NearGrave = true;
        this.playerFarming.NearStructure = (UnityEngine.Object) this.ClosestGrave != (UnityEngine.Object) null ? (StructureBrain) this.ClosestGrave.structureBrain : (StructureBrain) this.ClosestCrypt.structureBrain;
        this.previousBrain = this.playerFarming.NearStructure;
        this.previousStructure = (UnityEngine.Object) this.ClosestGrave != (UnityEngine.Object) null ? (Interaction) this.ClosestGrave : (Interaction) this.ClosestCrypt;
        this.NearGraveWithBody = true;
        this.GetLabel();
        if ((UnityEngine.Object) this.ClosestCrypt != (UnityEngine.Object) null && this.playerFarming.NearStructure == this.ClosestCrypt.structureBrain && !this.ClosestCrypt.structureBrain.IsFull)
          this.ClosestCrypt.SetDoors(true);
      }
      else
      {
        if ((UnityEngine.Object) this.ClosestGrave != (UnityEngine.Object) null)
          this.ClosestGrave.EndIndicateHighlighted(this.playerFarming);
        this.ClosestPosition = 100f;
        foreach (Interaction_CompostBinDeadBody compostBinDeadBody in Interaction_CompostBinDeadBody.DeadBodyCompost)
        {
          float message = Vector3.Distance(compostBinDeadBody.transform.position, this.playerFarming.transform.position);
          Debug.Log((object) message);
          if ((double) message < 1.5 && compostBinDeadBody.StructureBrain.CompostCount <= 0 && compostBinDeadBody.StructureBrain.PoopCount <= 0)
          {
            Debug.Log((object) ("1 " + compostBinDeadBody?.ToString()));
            if ((double) message < (double) this.ClosestPosition)
            {
              this.ClosestPosition = message;
              this.ClosestCompost = compostBinDeadBody;
              this.FoundCompost = true;
              this.previousStructure = (Interaction) compostBinDeadBody;
              compostBinDeadBody.IndicateHighlighted(this.playerFarming);
            }
          }
        }
        if (this.FoundCompost)
        {
          this.playerFarming.NearCompostBody = true;
          this.NearCompostWithBody = true;
          this.GetLabel();
        }
        else if ((UnityEngine.Object) this.previousStructure != (UnityEngine.Object) null)
        {
          this.previousStructure.EndIndicateHighlighted(this.playerFarming);
          this.previousStructure = (Interaction) null;
        }
      }
      if (!DataManager.Instance.CultTraits.Contains(FollowerTrait.TraitType.FurnaceFollower))
        return;
      float num1 = 2f;
      foreach (Interaction_DLCFurnace furnace in Interaction_DLCFurnace.Furnaces)
      {
        float num2 = Vector3.Distance(furnace.gameObject.transform.position, this.playerFarming.gameObject.transform.position);
        if ((double) num2 < (double) num1 && furnace.CanAddFuel())
        {
          if ((double) num2 < (double) this.ClosestPosition)
          {
            this.ClosestPosition = num2;
            this.ClosestFurnace = furnace;
            if ((UnityEngine.Object) this.previousStructure != (UnityEngine.Object) this.ClosestFurnace)
              this.ClosestFurnace.IndicateHighlighted(this.playerFarming);
          }
          this.FoundFurnace = true;
          this.FoundOne = true;
          this.ClosestFurnace = furnace;
        }
        if (this.FoundFurnace)
        {
          this.playerFarming.NearFurnace = true;
          this.NearFurnaceWithBody = true;
          this.GetLabel();
        }
        else if ((UnityEngine.Object) this.previousStructure != (UnityEngine.Object) null)
        {
          this.previousStructure.EndIndicateHighlighted(this.playerFarming);
          this.previousStructure = (Interaction) null;
        }
      }
    }
    else
    {
      this.LockToGroundPosition = this.transform.position + Vector3.back * 3f;
      if (UnityEngine.Physics.Raycast(this.LockToGroundPosition, Vector3.forward, out this.LockToGroundHit, 4f))
      {
        if (!((UnityEngine.Object) this.LockToGroundHit.collider.gameObject.GetComponent<MeshCollider>() != (UnityEngine.Object) null))
          return;
        this.LockToGroundNewPosition = this.transform.position;
        this.LockToGroundNewPosition.z = this.LockToGroundHit.point.z;
        this.transform.position = this.LockToGroundNewPosition;
      }
      else
      {
        this.LockToGroundNewPosition = this.transform.position;
        this.LockToGroundNewPosition.z = 0.0f;
        this.transform.position = this.LockToGroundNewPosition;
      }
    }
  }

  public IEnumerator PickUpBody()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    interactionHarvestMeat.DeadWorshipper.StructureInfo.Animation = "dead";
    Interaction_HarvestMeat.CurrentMovingBody = interactionHarvestMeat;
    interactionHarvestMeat._indicator.DOColor(Color.black, 0.5f);
    interactionHarvestMeat.CarryingBody = true;
    AudioManager.Instance.PlayOneShot("event:/player/body_pickup", interactionHarvestMeat.gameObject);
    interactionHarvestMeat.playerFarming.CarryingDeadFollowerID = interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID;
    BaseGoopDoor.DoorUp(contributor: interactionHarvestMeat.playerFarming);
    interactionHarvestMeat.Label = ScriptLocalization.Interactions.Drop;
    interactionHarvestMeat.meshRenderer.gameObject.SetActive(false);
    interactionHarvestMeat._indicator.gameObject.SetActive(false);
    interactionHarvestMeat.playerFarming.playerController.ResetSpecialMovingAnimations();
    interactionHarvestMeat.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
    while (!InputManager.Gameplay.GetInteractButtonUp())
      yield return (object) null;
    while ((!InputManager.Gameplay.GetInteractButtonHeld(interactionHarvestMeat.playerFarming) || MonoSingleton<UIManager>.Instance.MenusBlocked || (double) Time.deltaTime <= 0.0) && !((UnityEngine.Object) interactionHarvestMeat.playerFarming == (UnityEngine.Object) null) && interactionHarvestMeat.playerFarming.gameObject.activeSelf)
    {
      if (!LetterBox.IsPlaying && interactionHarvestMeat.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && interactionHarvestMeat.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody)
        interactionHarvestMeat.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
      yield return (object) null;
    }
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(interactionHarvestMeat.DeadWorshipper.transform.position);
    if (tileAtWorldPosition != null && tileAtWorldPosition.ObjectOnTile == StructureBrain.TYPES.DEAD_WORSHIPPER && tileAtWorldPosition.ObjectID == interactionHarvestMeat.structure.Structure_Info.ID)
      interactionHarvestMeat.DeadWorshipper.Structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
    StructureManager.RemoveStructure(interactionHarvestMeat.DeadWorshipper.Structure.Brain);
    Interaction_HarvestMeat.CurrentMovingBody = (Interaction_HarvestMeat) null;
    if (interactionHarvestMeat.FoundOne)
    {
      bool flag = true;
      if ((bool) (UnityEngine.Object) interactionHarvestMeat.ClosestGrave)
      {
        interactionHarvestMeat.ClosestGrave.EndIndicateHighlighted(interactionHarvestMeat.playerFarming);
        interactionHarvestMeat.ClosestGrave.StructureInfo.FollowerID = interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID;
        interactionHarvestMeat.ClosestGrave.SetGameObjects();
        foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
        {
          if (followerInfo.ID == interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID)
          {
            followerInfo.LastPosition = interactionHarvestMeat.ClosestGrave.structureBrain.Data.Position;
            break;
          }
        }
        BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.ClosestGrave.transform.position);
      }
      else if ((bool) (UnityEngine.Object) interactionHarvestMeat.ClosestCrypt)
      {
        if (!interactionHarvestMeat.ClosestCrypt.structureBrain.IsFull)
        {
          interactionHarvestMeat.ClosestCrypt.EndIndicateHighlighted(interactionHarvestMeat.playerFarming);
          interactionHarvestMeat.ClosestCrypt.structureBrain.DepositBody(interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID);
          foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
          {
            if (followerInfo.ID == interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID)
            {
              followerInfo.LastPosition = interactionHarvestMeat.ClosestCrypt.structureBrain.Data.Position;
              break;
            }
          }
          interactionHarvestMeat.ClosestCrypt.SetDoors(false);
          BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.ClosestCrypt.transform.position);
        }
        else
          flag = false;
      }
      else if ((bool) (UnityEngine.Object) interactionHarvestMeat.ClosestFurnace)
      {
        interactionHarvestMeat.ClosestFurnace.EndIndicateHighlighted(interactionHarvestMeat.playerFarming);
        foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
        {
          if (followerInfo.ID == interactionHarvestMeat.DeadWorshipperTmp.StructureInfo.FollowerID)
          {
            followerInfo.LastPosition = interactionHarvestMeat.ClosestFurnace.Structure.Brain.Data.Position;
            break;
          }
        }
        flag = true;
        BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.ClosestFurnace.transform.position);
      }
      if (flag)
      {
        AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", interactionHarvestMeat.gameObject);
        if (!interactionHarvestMeat.DeadWorshipper.followerInfo.HasBeenBuried && !interactionHarvestMeat.FoundFurnace)
          CultFaithManager.AddThought(Thought.Cult_FollowerBuried, interactionHarvestMeat.playerFarming.CarryingDeadFollowerID);
        else if (interactionHarvestMeat.FoundFurnace)
        {
          CultFaithManager.AddThought(Thought.Cult_FollowerBurned, interactionHarvestMeat.playerFarming.CarryingDeadFollowerID);
          Interaction_DLCFurnace.Instance.OnInteract(interactionHarvestMeat.state);
        }
        interactionHarvestMeat.DeadWorshipper.followerInfo.HasBeenBuried = true;
        interactionHarvestMeat.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
        interactionHarvestMeat.CarryingBody = false;
        interactionHarvestMeat.playerFarming.CarryingDeadFollowerID = -1;
        interactionHarvestMeat.playerFarming.NearFurnace = false;
        ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuryBody);
        UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat.gameObject);
      }
      else
        interactionHarvestMeat.DropBody();
    }
    else if (interactionHarvestMeat.FoundCompost)
    {
      interactionHarvestMeat.ClosestCompost.BuryBody();
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.ClosestCompost.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", interactionHarvestMeat.gameObject);
      interactionHarvestMeat.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      interactionHarvestMeat.CarryingBody = false;
      interactionHarvestMeat.playerFarming.CarryingDeadFollowerID = -1;
      interactionHarvestMeat.playerFarming.NearCompostBody = false;
      ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuryBody);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat.gameObject);
    }
    else
      interactionHarvestMeat.DropBody();
    BaseGoopDoor.DoorDown(interactionHarvestMeat.playerFarming);
  }

  public void DropBody()
  {
    Interaction_HarvestMeat.CurrentMovingBody = (Interaction_HarvestMeat) null;
    if (!this.CarryingBody)
      return;
    BaseGoopDoor.DoorDown(this.playerFarming);
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.DEAD_WORSHIPPER, 0);
    infoByType.BodyWrapped = true;
    infoByType.FollowerID = this.DeadWorshipperTmp.StructureInfo.FollowerID;
    this.CarryingBody = false;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.playerFarming.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      DeadWorshipper component = g.GetComponent<DeadWorshipper>();
      component.WrapBody();
      component.Setup();
      this.playerFarming.state.CURRENT_STATE = this.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive ? StateMachine.State.InActive : StateMachine.State.Idle;
      this.playerFarming.CarryingDeadFollowerID = -1;
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
      if (tileAtWorldPosition != null && tileAtWorldPosition.CanPlaceStructure)
      {
        AudioManager.Instance.PlayOneShot("event:/player/body_drop", tileAtWorldPosition.WorldPosition);
        component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
      }
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }), (System.Action) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  public IEnumerator PrepareForBurial()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    interactionHarvestMeat.structure.Brain.ReservedByPlayer = true;
    interactionHarvestMeat.playerFarming.indicator.HideTopInfo();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForEndOfFrame();
    AudioManager.Instance.PlayOneShot("event:/player/body_wrap", interactionHarvestMeat.gameObject);
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionHarvestMeat.playerFarming.CameraBone, 6f);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/player/body_wrap_done", interactionHarvestMeat.gameObject);
    interactionHarvestMeat.DeadWorshipper.WrapBody();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.InActive;
    if (interactionHarvestMeat.DeadWorshipper.followerInfo.BurntToDeath)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.CHARCOAL, UnityEngine.Random.Range(5, 11), interactionHarvestMeat.transform.position);
    else if (interactionHarvestMeat.DeadWorshipper.followerInfo.DiedFromRot && interactionHarvestMeat.canPickUpLegendaryAxe)
      yield return (object) interactionHarvestMeat.StartCoroutine(interactionHarvestMeat.PlayerPickUpWeapon());
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionHarvestMeat.HasChanged = true;
    interactionHarvestMeat.structure.Brain.ReservedByPlayer = false;
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
      this.StartCoroutine(this.HarvestMeatIE());
    }
    else
    {
      if (this.DeadWorshipper.followerInfo == null || this.DeadWorshipper.followerInfo.Necklace == InventoryItem.ITEM_TYPE.NONE)
        return;
      base.OnSecondaryInteract(state);
      this.StartCoroutine(this.LootBody());
    }
  }

  public IEnumerator HarvestMeatIE()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    interactionHarvestMeat.harvesting = true;
    try
    {
      interactionHarvestMeat.playerFarming.indicator.HideTopInfo();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().AddPlayerToCamera();
      interactionHarvestMeat.playerFarming.GoToAndStop(interactionHarvestMeat.transform.position, interactionHarvestMeat.gameObject);
      while (interactionHarvestMeat.playerFarming.GoToAndStopping)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot(interactionHarvestMeat.DeadWorshipper.followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated) ? "event:/dlc/follower/death_rot_harvest" : "event:/player/harvest_meat", interactionHarvestMeat.gameObject);
      interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
      interactionHarvestMeat.playerFarming.CustomAnimation("actions/harvest_meat", true);
      GameManager.GetInstance().OnConversationNext(interactionHarvestMeat.playerFarming.CameraBone, 6f);
      yield return (object) new WaitForSeconds(2f);
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
      interactionHarvestMeat.meshRenderer.enabled = false;
      AudioManager.Instance.PlayOneShot("event:/player/harvest_meat_done", interactionHarvestMeat.gameObject);
      if (interactionHarvestMeat.DeadWorshipper.followerInfo.SkinName == "Mushroom" || interactionHarvestMeat.DeadWorshipper.followerInfo.ID == 99996)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MUSHROOM_SMALL, 10, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
      else if (interactionHarvestMeat.DeadWorshipper.followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
      {
        interactionHarvestMeat.DeadWorshipper.Rotted.gameObject.SetActive(false);
        interactionHarvestMeat.DeadWorshipper.ItemIndicator.gameObject.SetActive(false);
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MAGMA_STONE, UnityEngine.Random.Range(10, 15), interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
      }
      else
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 5, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 2, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
      }
      if (interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace != InventoryItem.ITEM_TYPE.NONE)
      {
        InventoryItem.Spawn(interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace, 1, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
        interactionHarvestMeat.RemoveTraitGivenByItem();
      }
      interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace = InventoryItem.ITEM_TYPE.NONE;
      interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.InActive;
      if (interactionHarvestMeat.DeadWorshipper.followerInfo.DiedFromRot && interactionHarvestMeat.canPickUpLegendaryAxe)
        yield return (object) interactionHarvestMeat.StartCoroutine(interactionHarvestMeat.PlayerPickUpWeapon());
      yield return (object) new WaitForSeconds(0.5f);
      ++DataManager.Instance.TotalBodiesHarvested;
      if (DataManager.Instance.TotalBodiesHarvested >= 5 && !DataManager.Instance.PlayerFoundRelics.Contains(RelicType.SpawnCombatFollowerFromBodies) && DataManager.Instance.OnboardedRelics)
      {
        bool waiting = true;
        RelicCustomTarget.Create(interactionHarvestMeat.transform.position, interactionHarvestMeat.transform.position - Vector3.forward, 1.5f, RelicType.SpawnCombatFollowerFromBodies, (System.Action) (() => waiting = false));
        while (waiting)
          yield return (object) null;
      }
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
      interactionHarvestMeat.DeadWorshipper.Structure.Brain.Remove();
    }
    finally
    {
      this.harvesting = false;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionHarvestMeat.gameObject);
  }

  public IEnumerator LootBody()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    AudioManager.Instance.PlayOneShot("event:/dlc/player/mv_follower_corpse_loot", interactionHarvestMeat.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().AddPlayerToCamera();
    yield return (object) new WaitForEndOfFrame();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
    GameManager.GetInstance().OnConversationNext(interactionHarvestMeat.playerFarming.CameraBone, 6f);
    yield return (object) new WaitForSeconds(2f);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.3f);
    InventoryItem.Spawn(interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace, 1, interactionHarvestMeat.transform.position + Vector3.back * 0.5f);
    interactionHarvestMeat.RemoveTraitGivenByItem();
    interactionHarvestMeat.DeadWorshipper.followerInfo.Necklace = InventoryItem.ITEM_TYPE.NONE;
    interactionHarvestMeat.DeadWorshipper.SetOutfit();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().OnConversationEnd();
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.Idle;
    CultFaithManager.AddThought(Thought.Cult_LootedCorpse);
  }

  public void RemoveTraitGivenByItem()
  {
    if (this.DeadWorshipper.followerInfo.Necklace != InventoryItem.ITEM_TYPE.Necklace_Gold_Skull || 666 == this.DeadWorshipper.followerInfo.ID || 10009 == this.DeadWorshipper.followerInfo.ID)
      return;
    this.DeadWorshipper.followerInfo.Traits.Remove(FollowerTrait.TraitType.Immortal);
  }

  public IEnumerator PlayerPickUpWeapon()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    PickUp pickup = (PickUp) null;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionHarvestMeat.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1, interactionHarvestMeat.transform.position, result: (System.Action<PickUp>) (p =>
    {
      pickup = p;
      pickup.GetComponent<Interaction_BrokenWeapon>().SetWeapon(EquipmentType.Axe_Legendary);
    }));
    while ((UnityEngine.Object) pickup == (UnityEngine.Object) null)
      yield return (object) null;
    pickup.enabled = false;
    pickup.child.transform.localScale = Vector3.one;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", interactionHarvestMeat.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 5f);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    PlayerSimpleInventory component = PlayerFarming.Instance.GetComponent<PlayerSimpleInventory>();
    Vector3 itemTargetPosition = new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f);
    bool isMoving = true;
    TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = pickup.transform.DOMove(itemTargetPosition, 1f).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    tweenerCore.onComplete = tweenerCore.onComplete + (TweenCallback) (() => isMoving = false);
    while (isMoving)
      yield return (object) null;
    pickup.transform.position = itemTargetPosition;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
    yield return (object) new WaitForSeconds(1.5f);
    Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
    DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Axe_Legendary);
    pickup.GetComponent<Interaction_BrokenWeapon>().StartBringWeaponToBlacksmithObjective();
    UnityEngine.Object.Destroy((UnityEngine.Object) pickup.gameObject);
  }

  public void SeasonsManager_OnSeasonChanged(SeasonsManager.Season newSeason)
  {
    if (newSeason == SeasonsManager.Season.Winter || this.DeadWorshipper.StructureInfo.BodyWrapped || this.harvesting)
      return;
    FollowerInfo infoById = FollowerInfo.GetInfoByID(this.DeadWorshipper.StructureInfo.FollowerID, true);
    if (infoById == null || !infoById.FrozeToDeath)
      return;
    infoById.FrozeToDeath = false;
    infoById.CursedState = Thought.None;
    infoById.Freezing = 0.0f;
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.DeadWorshipper.transform.position);
    if (tileAtWorldPosition != null && tileAtWorldPosition.ObjectOnTile == StructureBrain.TYPES.DEAD_WORSHIPPER && tileAtWorldPosition.ObjectID == this.structure.Structure_Info.ID)
      this.DeadWorshipper.Structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
    StructureManager.RemoveStructure(this.DeadWorshipper.Structure.Brain);
    DataManager.Instance.Followers_Dead_IDs.Remove(this.DeadWorshipper.followerInfo.ID);
    DataManager.Instance.Followers_Dead.Remove(this.DeadWorshipper.followerInfo);
    Follower follower = FollowerManager.CreateNewFollower(this.DeadWorshipper.followerInfo, this.transform.position);
    FollowerBrain brain = follower.Brain;
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.State.LockStateChanges = true;
    brain.AddThought(Thought.FrozenAllWinter);
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() =>
    {
      double num = (double) follower.SetBodyAnimation((double) UnityEngine.Random.value < 0.5 ? "Deathdoor/resurrect-frozen" : "Deathdoor/resurrect-frozen2", false);
      GameManager.GetInstance().WaitForSeconds(4.33333349f, (System.Action) (() =>
      {
        if ((UnityEngine.Object) follower == (UnityEngine.Object) null)
        {
          brain.CompleteCurrentTask();
        }
        else
        {
          follower.State.LockStateChanges = false;
          follower.State.CURRENT_STATE = StateMachine.State.Idle;
          follower.Brain.CompleteCurrentTask();
        }
      }));
    }));
  }

  public IEnumerator InteractRoutine()
  {
    Interaction_HarvestMeat interactionHarvestMeat = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionHarvestMeat.playerFarming.CameraBone, 6f);
    interactionHarvestMeat.playerFarming.GoToAndStop(interactionHarvestMeat.transform.position + Vector3.left / 1.5f + Vector3.down / 1.5f, interactionHarvestMeat.gameObject);
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(0.2f);
    System.Threading.Tasks.Task loadTask = MonoSingleton<UIManager>.Instance.LoadRebuildBedMinigameAssets();
    yield return (object) new WaitUntil((Func<bool>) (() => loadTask.IsCompleted));
    interactionHarvestMeat.GetComponent<Structure>();
    interactionHarvestMeat._uiCookingMinigameOverlayController = MonoSingleton<UIManager>.Instance.RebuildBedMinigameOverlayControllerTemplate.Instantiate<UIRebuildBedMinigameOverlayController>();
    interactionHarvestMeat._uiCookingMinigameOverlayController.Initialise("Interactions/HealingStatue", 3f);
    interactionHarvestMeat._uiCookingMinigameOverlayController.OnCook += new System.Action(interactionHarvestMeat.OnCook);
    interactionHarvestMeat._uiCookingMinigameOverlayController.OnUnderCook += new System.Action(interactionHarvestMeat.OnUnderCook);
    interactionHarvestMeat._uiCookingMinigameOverlayController.OnBurn += new System.Action(interactionHarvestMeat.OnBurn);
    interactionHarvestMeat.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    interactionHarvestMeat.state.facingAngle = Utils.GetAngle(interactionHarvestMeat.state.transform.position, interactionHarvestMeat.transform.position);
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    AudioManager.Instance.PlayOneShot("event:/material/dirt_dig", interactionHarvestMeat.transform.position);
  }

  public void OnCook()
  {
    FollowerInfo infoById = FollowerInfo.GetInfoByID(this.DeadWorshipper.StructureInfo.FollowerID, true);
    infoById.FrozeToDeath = false;
    Follower follower = FollowerManager.CreateNewFollower(this.DeadWorshipper.followerInfo, this.transform.position);
    follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    follower.Brain.Stats.Freezing = 0.0f;
    follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    follower.State.LockStateChanges = true;
    string animationName = (double) UnityEngine.Random.value < 0.5 ? "Deathdoor/resurrect-frozen" : "Deathdoor/resurrect-frozen2";
    if (DataManager.Instance.Followers_Imprisoned_IDs.Contains(infoById.ID))
      animationName = "Deathdoor/resurrect-stocks";
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.UnfrozeFollower, follower.Brain.Info, NotificationFollower.Animation.Happy);
    AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_freeze_meltingtouch", this.transform.position);
    follower.Spine.AnimationState.SetAnimation(1, animationName, false);
    GameManager.GetInstance().WaitForSeconds(4.33333349f, (System.Action) (() =>
    {
      follower.Brain.CompleteCurrentTask();
      follower.State.LockStateChanges = false;
      follower.State.CURRENT_STATE = StateMachine.State.Idle;
    }));
    DataManager.Instance.Followers_Dead_IDs.Remove(this.DeadWorshipper.followerInfo.ID);
    DataManager.Instance.Followers_Dead.Remove(this.DeadWorshipper.followerInfo);
    this.Complete();
  }

  public void OnBurn()
  {
    this.HasChanged = true;
    this.Complete();
    AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_freeze_meltingtouch_fail", this.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 5, this.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 5, this.transform.position);
    CultFaithManager.AddThought(Thought.Cult_UnfreezeFollowerFailed, this.DeadWorshipper.StructureInfo.FollowerID);
  }

  public void OnUnderCook()
  {
    this.HasChanged = true;
    this.Complete();
    AudioManager.Instance.PlayOneShot("event:/dlc/follower/death_freeze_meltingtouch_fail", this.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BONE, 5, this.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOLLOWER_MEAT, 5, this.transform.position);
    CultFaithManager.AddThought(Thought.Cult_UnfreezeFollowerFailed, this.DeadWorshipper.StructureInfo.FollowerID);
  }

  public void Complete()
  {
    this.structure.Brain.ReservedByPlayer = false;
    CameraManager.instance.ShakeCameraForDuration(0.1f, 0.2f, 0.5f);
    this.state.CURRENT_STATE = StateMachine.State.Idle;
    this._uiCookingMinigameOverlayController.OnCook -= new System.Action(this.OnCook);
    this._uiCookingMinigameOverlayController.OnUnderCook -= new System.Action(this.OnUnderCook);
    this._uiCookingMinigameOverlayController.OnBurn -= new System.Action(this.OnBurn);
    UnityEngine.Object.Destroy((UnityEngine.Object) this._uiCookingMinigameOverlayController.gameObject);
    this._uiCookingMinigameOverlayController = (UIRebuildBedMinigameOverlayController) null;
    GameManager.GetInstance().WaitForSeconds(0.3f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.DeadWorshipper.transform.position);
    if (tileAtWorldPosition != null && tileAtWorldPosition.ObjectOnTile == StructureBrain.TYPES.DEAD_WORSHIPPER && tileAtWorldPosition.ObjectID == this.structure.Structure_Info.ID)
      this.DeadWorshipper.Structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
    StructureManager.RemoveStructure(this.DeadWorshipper.Structure.Brain);
  }

  [CompilerGenerated]
  public void \u003COnDisableInteraction\u003Eb__32_1() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  [CompilerGenerated]
  public void \u003CDropBody\u003Eb__50_0(GameObject g)
  {
    DeadWorshipper component = g.GetComponent<DeadWorshipper>();
    component.WrapBody();
    component.Setup();
    this.playerFarming.state.CURRENT_STATE = this.playerFarming.state.CURRENT_STATE == StateMachine.State.InActive ? StateMachine.State.InActive : StateMachine.State.Idle;
    this.playerFarming.CarryingDeadFollowerID = -1;
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(component.transform.position);
    if (tileAtWorldPosition != null && tileAtWorldPosition.CanPlaceStructure)
    {
      AudioManager.Instance.PlayOneShot("event:/player/body_drop", tileAtWorldPosition.WorldPosition);
      component.Structure.Brain.AddToGrid(tileAtWorldPosition.Position);
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [CompilerGenerated]
  public void \u003CDropBody\u003Eb__50_1() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
}
