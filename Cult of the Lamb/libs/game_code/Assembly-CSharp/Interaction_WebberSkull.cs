// Decompiled with JetBrains decompiler
// Type: Interaction_WebberSkull
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using MMTools;
using src.Extensions;
using src.Managers;
using src.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Interaction_WebberSkull : Interaction
{
  public static Interaction_WebberSkull WebberSkull;
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public GameObject container;
  [SerializeField]
  public GameObject lightingVolume;
  public string sPickup;
  public string sBury;
  public bool CarryingBody;
  public Grave ClosestGrave;
  public float ClosestPosition = 100f;
  public bool FoundOne;
  public bool FoundCompost;
  public bool NearGraveWithBody;
  public bool NearCompostWithBody;
  public bool addedOutline;

  public Structure Structure => this.structure;

  public void Start()
  {
    if ((UnityEngine.Object) Interaction_WebberSkull.WebberSkull != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) Interaction_WebberSkull.WebberSkull.gameObject);
    this.CarryingBody = false;
    this.UpdateLocalisation();
    Interaction_WebberSkull.WebberSkull = this;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) Interaction_WebberSkull.WebberSkull == (UnityEngine.Object) this))
      return;
    Interaction_WebberSkull.WebberSkull = (Interaction_WebberSkull) null;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.CarryingBody = false;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sPickup = ScriptLocalization.Interactions.PickUp;
  }

  public override void GetLabel()
  {
    if (!this.NearGraveWithBody)
      this.Label = this.sPickup;
    else
      this.Label = this.sBury;
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.CarryingBody)
      return;
    base.OnInteract(state);
    this.structure.enabled = false;
    this.StartCoroutine((IEnumerator) this.PickUpBody());
  }

  public override void OnDisableInteraction()
  {
    if (!this.CarryingBody || !((UnityEngine.Object) this.structure != (UnityEngine.Object) null) || !((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null))
      return;
    this.CarryingBody = false;
    StructuresData structure = StructuresData.GetInfoByType(StructureBrain.TYPES.WEBBER_SKULL, 0);
    StructureManager.BuildStructure(FollowerLocation.Base, structure, this.playerFarming.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(structure.Position);
      if (tileAtWorldPosition == null)
        return;
      g.GetComponent<Structure>().Brain.AddToGrid(tileAtWorldPosition.Position);
    }));
  }

  public override void Update()
  {
    base.Update();
    if (!this.CarryingBody)
      return;
    this.FoundOne = false;
    this.FoundCompost = false;
    this.NearGraveWithBody = false;
    this.NearCompostWithBody = false;
    this.playerFarming.NearGrave = false;
    this.playerFarming.NearCompostBody = false;
    this.playerFarming.NearFurnace = false;
    this.ClosestPosition = 100f;
    foreach (Grave grave in Grave.Graves)
    {
      float num = Vector3.Distance(grave.gameObject.transform.position, this.playerFarming.gameObject.transform.position);
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
      this.playerFarming.NearGrave = true;
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
  }

  public static List<StructureBrain.TYPES> GetDecorationsToUnlock()
  {
    return new List<StructureBrain.TYPES>()
    {
      StructureBrain.TYPES.DECORATION_DST_ALCHEMY,
      StructureBrain.TYPES.DECORATION_DST_DEERCLOPS,
      StructureBrain.TYPES.DECORATION_DST_MARBLETREE,
      StructureBrain.TYPES.DECORATION_DST_PIGSTICK,
      StructureBrain.TYPES.DECORATION_DST_SCIENCEMACHINE,
      StructureBrain.TYPES.DECORATION_DST_TREE,
      StructureBrain.TYPES.DECORATION_DST_WALL
    };
  }

  public IEnumerator PickUpBody()
  {
    Interaction_WebberSkull interactionWebberSkull = this;
    interactionWebberSkull.CarryingBody = true;
    AudioManager.Instance.PlayOneShot("event:/player/body_pickup", interactionWebberSkull.gameObject);
    BaseGoopDoor.DoorUp(contributor: interactionWebberSkull._playerFarming);
    interactionWebberSkull.Label = ScriptLocalization.Interactions.Drop;
    interactionWebberSkull.container.gameObject.SetActive(false);
    interactionWebberSkull.playerFarming.playerController.SetSpecialMovingAnimations("webber/idle", "webber/run-up", "webber/run-down", "webber/run", "webber/run-up-diagonal", "webber/run-horizontal");
    interactionWebberSkull.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
    while (!InputManager.Gameplay.GetInteractButtonUp(interactionWebberSkull.playerFarming))
      yield return (object) null;
    while ((!InputManager.Gameplay.GetInteractButtonHeld(interactionWebberSkull.playerFarming) || MonoSingleton<UIManager>.Instance.MenusBlocked || (double) Time.deltaTime <= 0.0) && !((UnityEngine.Object) interactionWebberSkull.playerFarming == (UnityEngine.Object) null) && interactionWebberSkull.playerFarming.gameObject.activeSelf)
    {
      if (!LetterBox.IsPlaying && interactionWebberSkull.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle_CarryingBody && interactionWebberSkull.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving_CarryingBody)
        interactionWebberSkull.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
      yield return (object) null;
    }
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(interactionWebberSkull.transform.position);
    if (tileAtWorldPosition != null && tileAtWorldPosition.ObjectID == interactionWebberSkull.structure.Structure_Info.ID)
      interactionWebberSkull.structure.Brain.RemoveFromGrid(tileAtWorldPosition.Position);
    StructureManager.RemoveStructure(interactionWebberSkull.structure.Brain);
    if (interactionWebberSkull.FoundOne)
    {
      interactionWebberSkull.ClosestGrave.OutlineEffect.OutlineLayers[0].Clear();
      interactionWebberSkull.ClosestGrave.OutlineEffect.RemoveGameObject(interactionWebberSkull.ClosestGrave.OutlineTarget);
      interactionWebberSkull.ClosestGrave.SetGameObjects();
      BiomeConstants.Instance.EmitSmokeExplosionVFX(interactionWebberSkull.ClosestGrave.transform.position);
      AudioManager.Instance.PlayOneShot("event:/player/body_drop_grave", interactionWebberSkull.gameObject);
      interactionWebberSkull.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      interactionWebberSkull.CarryingBody = false;
      interactionWebberSkull.lightingVolume.transform.parent = (Transform) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionWebberSkull.gameObject);
      GameManager.GetInstance().StartCoroutine((IEnumerator) interactionWebberSkull.SpawnWebberFollowerIE(interactionWebberSkull.ClosestGrave));
    }
    else
      interactionWebberSkull.DropBody();
    BaseGoopDoor.DoorDown(interactionWebberSkull.playerFarming);
  }

  public void DropBody()
  {
    this.playerFarming.playerController.ResetSpecialMovingAnimations();
    if (!this.CarryingBody)
      return;
    this.CarryingBody = false;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    StructureManager.BuildStructure(FollowerLocation.Base, StructuresData.GetInfoByType(StructureBrain.TYPES.WEBBER_SKULL, 0), this.playerFarming.transform.position, Vector2Int.one, false, (System.Action<GameObject>) (g =>
    {
      PlayerFarming.SetStateForAllPlayers();
      PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(g.transform.position);
      if (tileAtWorldPosition != null)
      {
        AudioManager.Instance.PlayOneShot("event:/player/body_drop", tileAtWorldPosition.WorldPosition);
        g.GetComponent<Structure>().Brain.AddToGrid(tileAtWorldPosition.Position);
      }
      this.CarryingBody = false;
    }));
  }

  public IEnumerator SpawnWebberFollowerIE(Grave grave)
  {
    Interaction_WebberSkull interactionWebberSkull = this;
    string webberSkin = "Webber";
    Vector3 pos = grave.transform.position;
    while (PlayerFarming.Location != FollowerLocation.Base || LetterBox.IsPlaying || MMConversation.isPlaying || SimulationManager.IsPaused || interactionWebberSkull.playerFarming.state.CURRENT_STATE != StateMachine.State.Idle && interactionWebberSkull.playerFarming.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    interactionWebberSkull.playerFarming.GoToAndStop(grave.transform.position + new Vector3(1f, -1f, 0.0f), grave.gameObject);
    if (!DataManager.GetFollowerSkinUnlocked(webberSkin))
    {
      interactionWebberSkull.lightingVolume.SetActive(true);
      FollowerInfo info = FollowerInfo.NewCharacter(FollowerLocation.Base, webberSkin);
      DataManager.SetFollowerSkinUnlocked(webberSkin);
      info.Pets = new List<FollowerPet.FollowerPetType>()
      {
        FollowerPet.FollowerPetType.Spider,
        FollowerPet.FollowerPetType.Spider,
        FollowerPet.FollowerPetType.Spider
      };
      info.Name = "Webber";
      FollowerBrain resurrectingFollower = FollowerBrain.GetOrCreateBrain(info);
      resurrectingFollower.ResetStats();
      if (resurrectingFollower.Info.Age > resurrectingFollower.Info.LifeExpectancy)
        resurrectingFollower.Info.LifeExpectancy = resurrectingFollower.Info.Age + UnityEngine.Random.Range(20, 30);
      else
        resurrectingFollower.Info.LifeExpectancy += UnityEngine.Random.Range(20, 30);
      Follower revivedFollower = FollowerManager.CreateNewFollower(resurrectingFollower._directInfoAccess, grave.transform.position);
      revivedFollower.Brain.AddTrait(FollowerTrait.TraitType.DontStarve);
      resurrectingFollower.Location = FollowerLocation.Base;
      resurrectingFollower.DesiredLocation = FollowerLocation.Base;
      resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
      revivedFollower.SetOutfit(FollowerOutfitType.Follower, false);
      revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      revivedFollower.State.LockStateChanges = true;
      FollowerBrain.SetFollowerCostume(revivedFollower.Spine.Skeleton, 0, revivedFollower.Brain.Info.SkinName, revivedFollower.Brain.Info.SkinCharacter, FollowerOutfitType.None, FollowerHatType.None, FollowerClothingType.None, FollowerCustomisationType.None, FollowerSpecialType.None, InventoryItem.ITEM_TYPE.NONE, info: revivedFollower.Brain._directInfoAccess);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(revivedFollower.gameObject, 3f);
      yield return (object) new WaitForEndOfFrame();
      double num = (double) revivedFollower.SetBodyAnimation("grave-spawn", false);
      revivedFollower.AddBodyAnimation("idle", true, 0.0f);
      revivedFollower.HideAllFollowerIcons();
      AudioManager.Instance.PlayOneShot("event:/Stings/thenight_sacrifice_followers");
      float t = 0.0f;
      while ((double) t < 3.2999999523162842)
      {
        t += Time.deltaTime;
        CameraManager.instance.shakeCamera1(t / 6f, UnityEngine.Random.Range(0.0f, 360f));
        MMVibrate.RumbleContinuous(t / 6f, t / 4f, interactionWebberSkull.playerFarming);
        yield return (object) null;
      }
      AudioManager.Instance.PlayOneShot("event:/enemy/tunnel_worm/tunnel_worm_burst_out_of_ground");
      AudioManager.Instance.PlayOneShot("event:/tarot/tarot_card_reveal");
      CameraManager.instance.ShakeCameraForDuration(1.25f, 1.5f, 0.25f);
      GameManager.GetInstance().OnConversationNext(grave.gameObject, 6f);
      revivedFollower.State.LockStateChanges = false;
      MMVibrate.StopRumble(interactionWebberSkull.playerFarming);
      yield return (object) null;
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact, interactionWebberSkull.playerFarming);
      yield return (object) new WaitForSeconds(3f);
      UnityEngine.Object.Destroy((UnityEngine.Object) interactionWebberSkull.lightingVolume);
      resurrectingFollower.CompleteCurrentTask();
      revivedFollower.ShowAllFollowerIcons();
      FollowerBrain.SetFollowerCostume(revivedFollower.Spine.Skeleton, revivedFollower.Brain._directInfoAccess, forceUpdate: true);
      resurrectingFollower = (FollowerBrain) null;
      revivedFollower = (Follower) null;
    }
    List<StructureBrain.TYPES> decorations = Interaction_WebberSkull.GetDecorationsToUnlock();
    interactionWebberSkull.playerFarming.playerController.ResetSpecialMovingAnimations();
    if (!PersistenceManager.PersistentData.UnlockedSurvivalMode)
    {
      UIMenuConfirmationWindow menu = MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate.Instantiate<UIMenuConfirmationWindow>();
      menu.Show();
      menu.Configure(ScriptLocalization.UI_PostGameUnlock.Header, LocalizationManager.GetTranslation("UI/SurvivalMode/Description"), true);
      yield return (object) menu.YieldUntilHidden();
      PersistenceManager.PersistentData.UnlockedSurvivalMode = true;
      PersistenceManager.Save();
    }
    List<GameObject> decos = new List<GameObject>();
    for (int i = 0; i < decorations.Count; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", pos);
      FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, pos).GetComponent<FoundItemPickUp>();
      component.DecorationType = decorations[i];
      decos.Add(component.gameObject);
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    foreach (GameObject gameObject in decos)
    {
      GameObject deco = gameObject;
      deco.transform.DOMove(interactionWebberSkull.playerFarming.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject)));
    }
    yield return (object) new WaitForSeconds(0.5f);
    bool wait = true;
    for (int index = 0; index < decorations.Count; ++index)
    {
      StructuresData.CompleteResearch(decorations[index]);
      StructuresData.SetRevealed(decorations[index]);
    }
    UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
    buildMenuController.Show(decorations);
    UIBuildMenuController buildMenuController1 = buildMenuController;
    buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
    {
      wait = false;
      buildMenuController = (UIBuildMenuController) null;
    });
    while (wait)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  [CompilerGenerated]
  public void \u003CDropBody\u003Eb__26_0(GameObject g)
  {
    PlayerFarming.SetStateForAllPlayers();
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetFreeClosestTileGridTileAtWorldPosition(g.transform.position);
    if (tileAtWorldPosition != null)
    {
      AudioManager.Instance.PlayOneShot("event:/player/body_drop", tileAtWorldPosition.WorldPosition);
      g.GetComponent<Structure>().Brain.AddToGrid(tileAtWorldPosition.Position);
    }
    this.CarryingBody = false;
  }
}
