// Decompiled with JetBrains decompiler
// Type: Interaction_EggFollower
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_EggFollower : Interaction
{
  public static List<Interaction_EggFollower> Interaction_EggFollowers = new List<Interaction_EggFollower>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public GameObject lightingVolume;
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public GameObject rottenEgg;
  [SerializeField]
  public GameObject sicknessWarning;
  public string sPickup;
  public string sPlace;
  public bool rotten;
  public bool nearHatchery;
  public bool carryingEgg;
  public bool crakingEgg;
  public FollowerSpecialType special;
  public Interaction_Hatchery closestHatchery;
  public float closestPosition = 100f;
  public bool foundHatchery;
  public bool addedOutline;
  public bool playedSfx;
  public LayerMask collisionMask;
  public System.Random random;
  public SkeletonAnimation skeletonAnimation;

  public Structure Structure => this.structure;

  public SkeletonAnimation Spine => this.spine;

  public bool CarryingEgg => this.carryingEgg;

  public void Start()
  {
    this.carryingEgg = false;
    this.UpdateLocalisation();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    this.SecondaryInteractable = true;
    this.HasSecondaryInteraction = true;
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Island"));
    this.collisionMask = (LayerMask) ((int) this.collisionMask | 1 << LayerMask.NameToLayer("Obstacles"));
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain.Data.EggInfo == null)
    {
      this.structure.Brain.Remove();
    }
    else
    {
      ((Structures_EggFollower) this.Structure.Brain).OnRotten += new System.Action(this.OnRotten);
      this.random = new System.Random(this.structure.Brain.Data.EggInfo.EggSeed);
      this.UpdateEgg(false, this.Structure.Brain.Data.Rotten, this.Structure.Brain.Data.EggInfo.Rotting, this.Structure.Brain.Data.EggInfo.Golden, this.structure.Brain.Data.EggInfo.Special);
    }
  }

  public void OnRotten() => this.UpdateEgg(false, true, false, false, this.special);

  public override void OnDestroy()
  {
    base.OnDestroy();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain == null)
      return;
    ((Structures_EggFollower) this.Structure.Brain).OnRotten -= new System.Action(this.OnRotten);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_EggFollower.Interaction_EggFollowers.Add(this);
    this.UpdateLocalisation();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_EggFollower.Interaction_EggFollowers.Remove(this);
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.carryingEgg = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sPickup = this.special != FollowerSpecialType.Midas_Arm ? ScriptLocalization.Interactions.PickUp : LocalizationManager.GetTranslation("Interactions/TakeMidasSkull");
    this.sPlace = ScriptLocalization.Interactions.PlaceBuilding;
  }

  public override void GetLabel()
  {
    this.sPickup = this.special != FollowerSpecialType.Midas_Arm ? ScriptLocalization.Interactions.PickUp : LocalizationManager.GetTranslation("Interactions/TakeMidasSkull");
    if ((UnityEngine.Object) this.Structure == (UnityEngine.Object) null || this.Structure.Brain == null || this.Structure.Brain.Data == null)
    {
      this.Interactable = false;
      this.Label = "";
    }
    else if (this.rotten)
    {
      this.Label = ScriptLocalization.Interactions.CleanRottenEgg;
      this.Interactable = true;
      this.HasSecondaryInteraction = false;
    }
    else if (this.nearHatchery)
    {
      this.Label = this.sPlace;
      this.SecondaryLabel = "";
      this.SecondaryInteractable = false;
    }
    else
    {
      this.Label = this.sPickup;
      this.Interactable = true;
      if (this.Structure.Brain.Data.EggInfo.Special == FollowerSpecialType.None)
      {
        this.SecondaryLabel = LocalizationManager.GetTranslation("Interactions/Crack");
        this.SecondaryInteractable = true;
      }
      else
      {
        this.SecondaryLabel = "";
        this.SecondaryInteractable = false;
      }
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.carryingEgg || this.crakingEgg)
      return;
    base.OnInteract(state);
    this.structure.enabled = false;
    if (this.rotten)
      this.StartCoroutine((IEnumerator) this.DoClean());
    else
      this.StartCoroutine((IEnumerator) this.PickUpBody());
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    this.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.transform.DOKill();
    this.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f);
    if (this.playedSfx)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
    this.playedSfx = true;
  }

  public IEnumerator DoClean()
  {
    Interaction_EggFollower interactionEggFollower = this;
    interactionEggFollower.playedSfx = false;
    interactionEggFollower.skeletonAnimation = interactionEggFollower.playerFarming.Spine;
    interactionEggFollower.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionEggFollower.HandleEvent);
    interactionEggFollower.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionEggFollower.state.facingAngle = Utils.GetAngle(interactionEggFollower.state.transform.position, interactionEggFollower.transform.position);
    yield return (object) new WaitForEndOfFrame();
    interactionEggFollower.playerFarming.simpleSpineAnimator.Animate("cleaning", 0, true);
    yield return (object) new WaitForSeconds(0.933333337f);
    interactionEggFollower._playerFarming.playerChoreXPBarController.AddChoreXP(interactionEggFollower.playerFarming);
    float Progress = 0.0f;
    while (InputManager.Gameplay.GetInteractButtonHeld() && (double) (Progress += Time.deltaTime) < 2.0 * ((double) interactionEggFollower.Structure.Brain.Data.GrowthStage / 5.0))
    {
      interactionEggFollower.Structure.Brain.Data.StartingScale = (float) (1.0 - (double) Progress / (2.0 * ((double) interactionEggFollower.Structure.Brain.Data.GrowthStage / 5.0)));
      yield return (object) null;
    }
    interactionEggFollower.skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionEggFollower.HandleEvent);
    interactionEggFollower.state.CURRENT_STATE = StateMachine.State.Idle;
    interactionEggFollower.transform.DOScale(Vector3.zero, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(interactionEggFollower.structure.Brain.Remove));
    interactionEggFollower.structure.Brain.Remove();
    UnityEngine.Object.Destroy((UnityEngine.Object) interactionEggFollower.gameObject);
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    if (this.carryingEgg)
      return;
    base.OnSecondaryInteract(state);
    this.crakingEgg = true;
    state.GetComponent<PlayerFarming>().StartCoroutine((IEnumerator) this.CrackIE());
  }

  public IEnumerator CrackIE()
  {
    Interaction_EggFollower interactionEggFollower = this;
    interactionEggFollower.playerFarming.BlockMeditation = true;
    interactionEggFollower.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionEggFollower.playerFarming.state.facingAngle = interactionEggFollower.playerFarming.state.LookAngle = Utils.GetAngle(interactionEggFollower.playerFarming.transform.position, interactionEggFollower.transform.position);
    int x = (double) interactionEggFollower.transform.position.x < (double) interactionEggFollower.playerFarming.transform.position.x ? 1 : -1;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) interactionEggFollower.transform.position, (Vector2) new Vector3((float) x, 0.0f, 0.0f), 2f, (int) interactionEggFollower.collisionMask).collider != (UnityEngine.Object) null)
      x *= -1;
    Vector3 TargetPosition = interactionEggFollower.transform.position + Vector3.right * (float) x * 1.5f;
    bool waiting = true;
    interactionEggFollower.playerFarming.GoToAndStop(TargetPosition, interactionEggFollower.gameObject, GoToCallback: (System.Action) (() => waiting = false));
    float time = 0.0f;
    while (waiting && (double) time < 2.0)
    {
      time += Time.deltaTime;
      yield return (object) null;
      if (!CoopManager.CoopActive && !interactionEggFollower.playerFarming.isLamb)
      {
        interactionEggFollower.crakingEgg = false;
        yield break;
      }
    }
    yield return (object) new WaitForEndOfFrame();
    if ((UnityEngine.Object) interactionEggFollower == (UnityEngine.Object) null)
    {
      interactionEggFollower.playerFarming.BlockMeditation = false;
      PlayerFarming.SetStateForAllPlayers();
      interactionEggFollower.crakingEgg = false;
    }
    else if (!CoopManager.CoopActive && !interactionEggFollower.playerFarming.isLamb)
    {
      interactionEggFollower.crakingEgg = false;
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/weapon/hammer_heavy/hammer_release_swing", interactionEggFollower.gameObject);
      interactionEggFollower.playerFarming.Spine.AnimationState.SetAnimation(0, "Egg/egg-harvest", false);
      interactionEggFollower.playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(0.825f);
      int quantity = interactionEggFollower.Structure.Brain.Data.EggInfo == null || !interactionEggFollower.Structure.Brain.Data.EggInfo.Golden ? 1 : 5;
      ++DataManager.Instance.eggsCracked;
      if (DataManager.Instance.eggsCracked >= 3 && DataManager.Instance.TailorEnabled && !DataManager.Instance.UnlockedClothing.Contains(FollowerClothingType.Special_1) && !FoundItemPickUp.IsOutfitPickUpActive(FollowerClothingType.Special_1))
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_OUTFIT, 1, interactionEggFollower.transform.position).GetComponent<FoundItemPickUp>().clothingType = FollowerClothingType.Special_1;
      if (interactionEggFollower.structure.Brain.Data.EggInfo.Rotting)
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.MAGMA_STONE, UnityEngine.Random.Range(12, 21), interactionEggFollower.transform.position);
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.YOLK, quantity, interactionEggFollower.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/hopper_miniboss/hopper_miniboss_egg_crack", interactionEggFollower.gameObject);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.25f, 0.2f);
      interactionEggFollower.Structure.Brain.Remove();
      yield return (object) new WaitForSeconds(0.9166667f);
      interactionEggFollower.playerFarming.BlockMeditation = false;
      PlayerFarming.SetStateForAllPlayers();
    }
  }

  public override void OnDisableInteraction()
  {
    if (!this.carryingEgg || !((UnityEngine.Object) this.structure != (UnityEngine.Object) null) || !((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null))
      return;
    this.DropBody();
  }

  public IEnumerator PickUpBody()
  {
    Interaction_EggFollower interactionEggFollower = this;
    interactionEggFollower.carryingEgg = true;
    AudioManager.Instance.PlayOneShot("event:/player/body_pickup", interactionEggFollower.gameObject);
    BaseGoopDoor.DoorUp(contributor: interactionEggFollower.playerFarming);
    interactionEggFollower.Label = ScriptLocalization.Interactions.Drop;
    interactionEggFollower.container.gameObject.SetActive(false);
    if (interactionEggFollower.Structure.Brain.Data.EggInfo.Golden)
      interactionEggFollower.playerFarming.playerController.SetSpecialMovingAnimations("egg/gold-idle", "egg/gold-run-up", "egg/gold-run-down", "egg/gold-run", "egg/gold-run-up-diagonal", "egg/gold-run-horizontal");
    else
      interactionEggFollower.playerFarming.playerController.SetSpecialMovingAnimations("egg/idle", "egg/run-up", "egg/run-down", "egg/run", "egg/run-up-diagonal", "egg/run-horizontal");
    interactionEggFollower.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
    Skin skin = interactionEggFollower.playerFarming.Spine.Skeleton.Skin;
    if (interactionEggFollower.Structure.Brain.Data.EggInfo.Rotting)
      skin.AddSkin(interactionEggFollower.playerFarming.Spine.Skeleton.Data.FindSkin("Eggs/Mutated"));
    else if (interactionEggFollower.Structure.Brain.Data.EggInfo.Golden)
      skin.AddSkin(interactionEggFollower.playerFarming.Spine.Skeleton.Data.FindSkin("Eggs/Gold"));
    else if (interactionEggFollower.Structure.Brain.Data.EggInfo.Special != FollowerSpecialType.None)
      skin.AddSkin(interactionEggFollower.playerFarming.Spine.Skeleton.Data.FindSkin($"Eggs/{interactionEggFollower.Structure.Brain.Data.EggInfo.Special}"));
    else
      skin.AddSkin(interactionEggFollower.playerFarming.Spine.Skeleton.Data.FindSkin("Eggs/Normal"));
    interactionEggFollower.playerFarming.Spine.Skeleton.SetSkin(skin);
    while (!InputManager.Gameplay.GetInteractButtonUp(interactionEggFollower.playerFarming))
      yield return (object) null;
    int origPlayers = PlayerFarming.playersCount;
    bool coopPlayerJoinedOrLeft = false;
    while (!InputManager.Gameplay.GetInteractButtonHeld(interactionEggFollower.playerFarming) || MonoSingleton<UIManager>.Instance.MenusBlocked || (double) Time.deltaTime <= 0.0)
    {
      int num;
      if (PlayerFarming.playersCount == origPlayers)
      {
        CoopManager instance = CoopManager.Instance;
        num = instance != null ? (instance.IsSpawningOrRemovingPlayer ? 1 : 0) : 0;
      }
      else
        num = 1;
      coopPlayerJoinedOrLeft = num != 0;
      if (!coopPlayerJoinedOrLeft && !((UnityEngine.Object) interactionEggFollower.playerFarming == (UnityEngine.Object) null) && interactionEggFollower.playerFarming.gameObject.activeSelf)
      {
        if (!LetterBox.IsPlaying && interactionEggFollower.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && interactionEggFollower.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody)
          interactionEggFollower.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
        yield return (object) null;
      }
      else
        break;
    }
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(interactionEggFollower.transform.position);
    if (tileAtWorldPosition != null && tileAtWorldPosition.ObjectID == interactionEggFollower.structure.Structure_Info.ID)
      interactionEggFollower.structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
    StructureManager.RemoveStructure(interactionEggFollower.structure.Brain);
    if (interactionEggFollower.foundHatchery && !coopPlayerJoinedOrLeft)
    {
      interactionEggFollower.playerFarming.playerController.ResetSpecialMovingAnimations();
      interactionEggFollower.closestHatchery.OutlineEffect.OutlineLayers[0].Clear();
      interactionEggFollower.closestHatchery.OutlineEffect.RemoveGameObject(interactionEggFollower.closestHatchery.OutlineTarget);
      interactionEggFollower.closestHatchery.AddEgg(interactionEggFollower.Structure.Brain.Data);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionEggFollower.closestHatchery.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", interactionEggFollower.gameObject);
      PlayerFarming.SetStateForAllPlayers();
      interactionEggFollower.carryingEgg = false;
      interactionEggFollower.lightingVolume.transform.parent = (Transform) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionEggFollower.gameObject);
    }
    else
      interactionEggFollower.DropBody(coopPlayerJoinedOrLeft);
    BaseGoopDoor.DoorDown(interactionEggFollower.playerFarming);
  }

  public void DropBody(bool leaveAnimations = false)
  {
    this.playerFarming.playerController.ResetSpecialMovingAnimations();
    if (!this.carryingEgg)
      return;
    this.Interactable = false;
    this.carryingEgg = false;
    StructuresData.EggData eggData = this.Structure.Brain.Data.EggInfo;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.EGG_FOLLOWER, 0);
    infoByType.EggInfo = eggData;
    StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.playerFarming.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      if (!leaveAnimations)
        PlayerFarming.SetStateForAllPlayers();
      StructureBrain brain = g.GetComponent<Structure>().Brain;
      brain.Data.EggInfo = eggData;
      g.GetComponent<Interaction_EggFollower>().UpdateEgg(false, false, eggData.Rotting, eggData.Golden, this.special);
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(g.transform.position);
      if (tileAtWorldPosition != null)
      {
        AudioManager.Instance.PlayOneShot("event:/player/body_drop", tileAtWorldPosition.WorldPosition);
        brain.AddToGrid(tileAtWorldPosition.Position);
      }
      if (eggData.Rotting && (UnityEngine.Object) PathTileManager.Instance != (UnityEngine.Object) null && PathTileManager.Instance.GetTileTypeAtPosition(g.transform.position) == StructureBrain.TYPES.NONE)
        PathTileManager.Instance.SetTile(StructureBrain.TYPES.DECORATION_DLC_ROT_FLOOR, g.transform.position);
      this.carryingEgg = false;
    }));
  }

  public override void Update()
  {
    base.Update();
    if (!this.carryingEgg || this.rotten)
      return;
    this.foundHatchery = false;
    this.playerFarming.NearGrave = false;
    this.playerFarming.CarryingEgg = false;
    this.playerFarming.NearCompostBody = false;
    this.closestPosition = 100f;
    foreach (Interaction_Hatchery hatchery in Interaction_Hatchery.Hatcheries)
    {
      float num = Vector3.Distance(hatchery.gameObject.transform.position, this.playerFarming.gameObject.transform.position);
      if ((double) num < 1.0 && hatchery.Structure.Brain.Data.EggInfo == null)
      {
        if ((double) num < (double) this.closestPosition)
        {
          this.closestPosition = num;
          this.closestHatchery = hatchery;
          if (!this.addedOutline)
          {
            this.Outliner.OutlineLayers[0].Clear();
            this.Outliner.OutlineLayers[0].Add(this.closestHatchery.OutlineTarget.gameObject);
            this.addedOutline = true;
          }
        }
        this.foundHatchery = true;
      }
    }
    if (this.foundHatchery)
    {
      this.playerFarming.NearGrave = true;
      this.playerFarming.CarryingEgg = true;
      this.GetLabel();
    }
    if (this.foundHatchery)
      return;
    if ((UnityEngine.Object) this.closestHatchery != (UnityEngine.Object) null && this.addedOutline)
    {
      this.Outliner.OutlineLayers[0].Clear();
      this.addedOutline = false;
    }
    this.closestPosition = 100f;
  }

  public void UpdateEgg(
    bool cracked,
    bool rotten,
    bool rotted,
    bool golden,
    FollowerSpecialType special = FollowerSpecialType.None)
  {
    if (rotten)
    {
      this.spine.gameObject.SetActive(false);
      this.rottenEgg.gameObject.SetActive(true);
    }
    else if (rotted)
    {
      this.spine.Skeleton.SetSkin("Eggs/Mutated");
      this.spine.AnimationState.SetAnimation(0, cracked ? "Egg/egg-ready" : "Egg/egg-idle", true);
    }
    else if (golden)
    {
      this.spine.Skeleton.SetSkin("Eggs/Gold");
      this.spine.AnimationState.SetAnimation(0, cracked ? "Egg/egg-ready-gold" : "Egg/egg-idle-gold", true);
    }
    else if (special != FollowerSpecialType.None)
    {
      this.spine.Skeleton.SetSkin($"Eggs/{special}");
      this.spine.AnimationState.SetAnimation(0, cracked ? "Egg/egg-ready" : "Egg/egg-idle", true);
    }
    else
    {
      this.spine.Skeleton.SetSkin("Eggs/Normal");
      this.spine.AnimationState.SetAnimation(0, cracked ? "Egg/egg-ready" : "Egg/egg-idle", true);
    }
    if (this.structure.Brain != null)
      this.structure.Brain.Data.EggInfo.Special = special;
    this.rotten = rotten;
    this.special = special;
    this.sicknessWarning.SetActive(rotten);
  }
}
