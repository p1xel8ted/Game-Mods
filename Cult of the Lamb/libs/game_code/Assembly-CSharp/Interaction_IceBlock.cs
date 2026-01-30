// Decompiled with JetBrains decompiler
// Type: Interaction_IceBlock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Menus.DoctrineChoicesMenu;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_IceBlock : Interaction
{
  public static List<Interaction_IceBlock> IceBlocks = new List<Interaction_IceBlock>();
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject container;
  public UIBuildSnowmanMinigameOverlayController uIBuildSnowmanMinigameOverlayController;
  public bool carryingSnowball;
  public bool playedSfx;
  public bool activated;
  public bool isLoadingAssets;

  public bool CarryingSnowball => this.carryingSnowball;

  public Structure Structure => this.structure;

  public void Start()
  {
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    Interaction_IceBlock.IceBlocks.Add(this);
  }

  public override void OnDestroy()
  {
    this.uIBuildSnowmanMinigameOverlayController?.RequestAbort();
    base.OnDestroy();
    Interaction_IceBlock.IceBlocks.Remove(this);
  }

  public new void OnDisable()
  {
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.carryingSnowball = false;
  }

  public override void GetLabel()
  {
    base.GetLabel();
    if (!this.Interactable)
      this.Label = "";
    else if (this.structure.Type == StructureBrain.TYPES.ICE_BLOCK)
      this.Label = LocalizationManager.GetTranslation("Interactions/BuildSnowman");
    else if (this.structure.Type == StructureBrain.TYPES.SNOW_BALL)
      this.Label = LocalizationManager.GetTranslation("Interactions/PickUp");
    else
      this.Label = LocalizationManager.GetTranslation("Interactions/ClearSnow");
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.isLoadingAssets)
      return;
    base.OnInteract(state);
    if (this.structure.Type == StructureBrain.TYPES.ICE_BLOCK)
    {
      if (this.activated)
        return;
      this.activated = true;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone);
      this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadBuildSnowmanMinigameAssets(), (System.Action) (() =>
      {
        SimulationManager.Pause();
        this.uIBuildSnowmanMinigameOverlayController = MonoSingleton<UIManager>.Instance.BuildSnowmanMinigameOverlayControllerTemplate.Instantiate<UIBuildSnowmanMinigameOverlayController>();
        this.uIBuildSnowmanMinigameOverlayController.Initialize();
        this.uIBuildSnowmanMinigameOverlayController.Show();
        this.uIBuildSnowmanMinigameOverlayController.OnFail += (System.Action) (() =>
        {
          SimulationManager.UnPause();
          if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
          {
            GameManager.GetInstance().OnConversationEnd();
          }
          else
          {
            AudioManager.Instance.PlayOneShot("event:/Stings/generic_negative", this.transform.position);
            MMVibrate.Haptic(MMVibrate.HapticTypes.Failure);
            BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
            this.structure.Brain.Remove();
            GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
            for (int index = 0; index < UnityEngine.Random.Range(1, 4); ++index)
              InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SNOW_CHUNK, 1, this.transform.position, (float) UnityEngine.Random.Range(9, 11), (System.Action<PickUp>) (pickUp =>
              {
                AudioManager.Instance.PlayOneShot("event:/dlc/env/snowman/minigame_spawn_snowball", pickUp.gameObject);
                GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_impact_ground")));
              }));
          }
        });
        this.uIBuildSnowmanMinigameOverlayController.OnSucceed += (System.Action<int>) (level =>
        {
          SimulationManager.UnPause();
          if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
          {
            GameManager.GetInstance().OnConversationEnd();
          }
          else
          {
            UIManager.PlayAudio("event:/dlc/env/snowman/minigame_spawn");
            this.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 1f);
            GameManager.GetInstance().StartCoroutine((IEnumerator) this.RemoveStructureAfterDelay(this.structure.Brain, 3f));
            StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.SNOWMAN, 0);
            if (level < 6)
            {
              infoByType.VariantIndex = UnityEngine.Random.Range(6, 9);
              NotificationCentre.Instance.PlayGenericNotification("Notifications/CreatedSnowman/Bad");
              AudioManager.Instance.PlayOneShot("event:/dlc/env/snowman/minigame_spawn_shoddy", this.transform.position);
            }
            else if (level <= 9)
            {
              infoByType.VariantIndex = UnityEngine.Random.Range(3, 6);
              NotificationCentre.Instance.PlayGenericNotification("Notifications/CreatedSnowman/Mid");
              AudioManager.Instance.PlayOneShot("event:/dlc/env/snowman/minigame_spawn_decent", this.transform.position);
            }
            else
            {
              infoByType.VariantIndex = UnityEngine.Random.Range(0, 3);
              NotificationCentre.Instance.PlayGenericNotification("Notifications/CreatedSnowman/Good");
              AudioManager.Instance.PlayOneShot("event:/dlc/env/snowman/minigame_spawn_perfect", this.transform.position);
              ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.BuildGoodSnowman);
            }
            BiomeConstants.Instance.EmitWinterSuccessVFX(this.transform.position + new Vector3(0.0f, 0.84f, -0.75f), this.transform.parent);
            MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
            infoByType.Level = level;
            infoByType.Picked = true;
            if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null && PlacementRegion.Instance.structureBrain != null)
            {
              PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.transform.position);
              if (tileAtWorldPosition != null)
              {
                infoByType.PlacementRegionPosition = new Vector3Int((int) PlacementRegion.Instance.structureBrain.Data.Position.x, (int) PlacementRegion.Instance.structureBrain.Data.Position.y, 0);
                infoByType.GridTilePosition = tileAtWorldPosition.Position;
              }
            }
            StructureManager.BuildStructure(FollowerLocation.Base, infoByType, this.transform.position, new Vector2Int(infoByType.TILE_WIDTH, infoByType.TILE_HEIGHT));
            if ((UnityEngine.Object) PlacementRegion.Instance != (UnityEngine.Object) null && PlacementRegion.Instance.structureBrain != null)
              PlacementRegion.Instance.structureBrain.AddStructureToGrid(infoByType);
            this.Interactable = false;
            ++DataManager.Instance.SnowmenCreated;
            if (DataManager.Instance.SnowmenCreated >= 3 && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Snowman))
              GameManager.GetInstance().StartCoroutine((IEnumerator) this.SnowmanRitualIE());
            else
              GameManager.GetInstance().OnConversationEnd();
          }
        });
        state.CURRENT_STATE = StateMachine.State.CustomAction0;
      })));
    }
    else if (this.structure.Type == StructureBrain.TYPES.SNOW_BALL)
    {
      this.structure.enabled = false;
      this.StartCoroutine((IEnumerator) this.PickUpSnowball());
    }
    else
      this.StartCoroutine((IEnumerator) this.DoClean());
  }

  public IEnumerator RemoveStructureAfterDelay(StructureBrain brain, float seconds)
  {
    yield return (object) new WaitForSeconds(seconds);
    brain.Remove();
  }

  public void SnowManRitual() => this.StartCoroutine((IEnumerator) this.SnowmanRitualIE());

  public IEnumerator SnowmanRitualIE()
  {
    Interaction_IceBlock interactionIceBlock = this;
    AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake", interactionIceBlock.transform.position);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(interactionIceBlock.transform.gameObject);
    GameManager.GetInstance().CameraSetTargetZoom(5f);
    CameraManager.instance.ShakeCameraForDuration(1f, 2f, 1f);
    interactionIceBlock.transform.DOComplete();
    interactionIceBlock.transform.DOShakePosition(1f, new Vector3(0.1f, 0.0f, 0.0f));
    yield return (object) new WaitForSeconds(1f);
    interactionIceBlock.transform.DOShakeScale(0.8f, new Vector3(0.1f, 0.1f, 0.0f));
    for (int i = 0; i < 4; ++i)
    {
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SNOW_CHUNK, 1, interactionIceBlock.transform.position, (float) UnityEngine.Random.Range(9, 11), (System.Action<PickUp>) (pickUp =>
      {
        pickUp.SetInitialSpeedAndDiraction((float) UnityEngine.Random.Range(9, 11), (float) UnityEngine.Random.Range(-360, 360));
        AudioManager.Instance.PlayOneShot("event:/dlc/env/snowman/minigame_spawn_snowball", pickUp.GetComponent<Interaction_IceBlock>().gameObject);
      }));
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(0.2f);
    MMConversation.CURRENT_CONVERSATION = new ConversationObject((List<ConversationEntry>) null, (List<MMTools.Response>) null, (System.Action) null, new List<DoctrineResponse>()
    {
      new DoctrineResponse(SermonCategory.Special, 5, true, (System.Action) null)
    });
    UIDoctrineChoicesMenuController doctrineChoicesInstance = MonoSingleton<UIManager>.Instance.DoctrineChoicesMenuTemplate.Instantiate<UIDoctrineChoicesMenuController>();
    doctrineChoicesInstance.Show();
    while (doctrineChoicesInstance.gameObject.activeInHierarchy)
      yield return (object) null;
    UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Ritual_Snowman);
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator DoClean()
  {
    Interaction_IceBlock interactionIceBlock = this;
    SkeletonAnimation skeletonAnimation = interactionIceBlock.playerFarming.Spine;
    skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(interactionIceBlock.HandleEvent);
    interactionIceBlock.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    interactionIceBlock.state.facingAngle = Utils.GetAngle(interactionIceBlock.state.transform.position, interactionIceBlock.transform.position);
    yield return (object) new WaitForEndOfFrame();
    interactionIceBlock.playerFarming.simpleSpineAnimator.Animate("cleaning", 0, true);
    AudioManager.Instance.PlayOneShot("event:/dlc/material/snow_sweep_os", interactionIceBlock.transform.position);
    float ChoreDuration = DataManager.GetChoreDuration(interactionIceBlock.playerFarming);
    yield return (object) new WaitForSeconds(ChoreDuration / 2f);
    interactionIceBlock._playerFarming.playerChoreXPBarController.AddChoreXP(interactionIceBlock.playerFarming);
    yield return (object) new WaitForSeconds(ChoreDuration / 2f);
    interactionIceBlock.structure.Brain.Remove();
    for (int index = 0; index < UnityEngine.Random.Range(1, 4); ++index)
      InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SNOW_CHUNK, 1, interactionIceBlock.transform.position, (float) UnityEngine.Random.Range(9, 11), (System.Action<PickUp>) (pickUp =>
      {
        AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_throw");
        GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
        {
          pickUp.GetComponent<Interaction_IceBlock>();
          AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_impact_ground");
        }));
      }));
    AudioManager.Instance.PlayOneShot("event:/dlc/material/snow_sweep_complete", interactionIceBlock.transform.position);
    ++DataManager.Instance.itemsCleaned;
    BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionIceBlock.transform.position);
    skeletonAnimation.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(interactionIceBlock.HandleEvent);
    System.Action onCrownReturn = interactionIceBlock.playerFarming.OnCrownReturn;
    if (onCrownReturn != null)
      onCrownReturn();
    interactionIceBlock.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "sfxTrigger"))
      return;
    CameraManager.shakeCamera(0.05f, Utils.GetAngle(this.playerFarming.gameObject.transform.position, this.transform.position));
    MMVibrate.Haptic(MMVibrate.HapticTypes.SoftImpact, this.playerFarming, coroutineSupport: (MonoBehaviour) GameManager.GetInstance());
    this.transform.DOKill();
    this.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f);
    if (this.playedSfx)
      return;
    AudioManager.Instance.PlayOneShot("event:/player/sweep", this.transform.position);
    this.playedSfx = true;
  }

  public IEnumerator PickUpSnowball()
  {
    Interaction_IceBlock interactionIceBlock = this;
    if (!interactionIceBlock.carryingSnowball)
    {
      interactionIceBlock.carryingSnowball = true;
      AudioManager.Instance.PlayOneShot("event:/dlc/material/snowball_pickup", interactionIceBlock.gameObject);
      Debug.Log((object) "test");
      BaseGoopDoor.DoorUp(contributor: interactionIceBlock._playerFarming);
      BaseGoopDoor.BlockGoopDoor();
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(interactionIceBlock.transform.position);
      if ((UnityEngine.Object) interactionIceBlock.structure != (UnityEngine.Object) null && interactionIceBlock.structure.Structure_Info != null && tileAtWorldPosition != null && tileAtWorldPosition.ObjectID == interactionIceBlock.structure.Structure_Info.ID)
      {
        interactionIceBlock.structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
        StructureManager.RemoveStructure(interactionIceBlock.structure.Brain);
      }
      interactionIceBlock.Label = ScriptLocalization.Interactions.Drop;
      interactionIceBlock.container.gameObject.SetActive(false);
      interactionIceBlock.playerFarming.playerController.SetSpecialMovingAnimations("snowball/idle", "snowball/run-up", "snowball/run-down", "snowball/run", "snowball/run-up-diagonal", "snowball/run-horizontal");
      interactionIceBlock.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
      Vector3 position = interactionIceBlock.transform.position;
      while (!InputManager.Gameplay.GetInteractButtonUp(interactionIceBlock.playerFarming))
        yield return (object) null;
      interactionIceBlock.playerFarming.CarryingSnowball = true;
    }
  }

  public void DropSnowball()
  {
    this.playerFarming.playerController.ResetSpecialMovingAnimations();
    this.playerFarming.CarryingSnowball = false;
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
    {
      this.playerFarming.playerController.SetSpecialMovingAnimations("blizzard/idle-called-center", "blizzard/run-up-called-center", "blizzard/run-down-called-center", "blizzard/run-called-center", "blizzard/run-up-diagonal-called-center", "blizzard/run-horizontal-called-center", StateMachine.State.Idle_Winter);
      this.playerFarming.playerController.OverrideBlizzardAnims = true;
    }
    this.playerFarming.HideHeavyChargeBars();
    this.playerFarming.chargeSnowball = 0.0f;
    if (!this.carryingSnowball)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }
}
