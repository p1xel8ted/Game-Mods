// Decompiled with JetBrains decompiler
// Type: BiomeBaseManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using MMRoomGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BiomeBaseManager : MonoBehaviour
{
  public static bool HasLoadedData = false;
  public static bool Testing = true;
  public static bool EnterTemple = false;
  private static BiomeBaseManager _Instance;
  public GameObject PlayerSpawnLocation;
  public GameObject PlayerReturnFromDoorRoomLocation;
  public GameObject RecruitSpawnLocation;
  public ParticleSystem recruitParticles;
  public GameObject PlayerGreetLocation;
  public GameObject RunResults;
  public GameObject Church;
  public GenerateRoom DoorRoom;
  public GameObject Lumberjack;
  public bool LoadData = true;
  [EventRef]
  public string biomeAtmosPath;
  [EventRef]
  public string nightBiomeAtmosPath;
  private EventInstance biomeAtmosInstance;
  public GenerateRoom Room;
  public static bool WalkedBack = false;
  private bool wasNight = true;
  private bool createdLoop;
  public System.Action OnNewRecruitRevealed;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  private static readonly int DepthThreshold = Shader.PropertyToID("_DepthThreshold");

  public static BiomeBaseManager Instance
  {
    get => BiomeBaseManager._Instance;
    set => BiomeBaseManager._Instance = value;
  }

  private void Awake()
  {
    if (PlayerFarming.Location == FollowerLocation.Boss_5 && !DataManager.Instance.CameFromDeathCatFight)
      this.Room.gameObject.SetActive(false);
    BiomeBaseManager.Instance = this;
  }

  private void PlayAtmos()
  {
    if (!this.createdLoop)
    {
      if (!TimeManager.IsNight)
      {
        AudioManager.Instance.PlayAtmos(this.biomeAtmosPath);
        this.wasNight = false;
      }
      else
      {
        AudioManager.Instance.PlayAtmos(this.nightBiomeAtmosPath);
        this.wasNight = false;
      }
      this.createdLoop = true;
    }
    else if (!TimeManager.IsNight)
    {
      if (this.wasNight)
      {
        AudioManager.Instance.PlayAtmos(this.biomeAtmosPath);
        this.wasNight = false;
      }
    }
    else if (!this.wasNight)
    {
      AudioManager.Instance.PlayAtmos(this.nightBiomeAtmosPath);
      this.wasNight = true;
    }
    DataManager.Instance.AllowSaving = true;
  }

  private void OnEnable()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.UpgradeBase);
    if (!Application.isEditor || SaveAndLoad.Loaded)
      this.LoadData = false;
    if (!this.LoadData || BiomeBaseManager.HasLoadedData)
      return;
    Debug.Log((object) "LOAD!");
    SaveAndLoad.Load(SaveAndLoad.SAVE_SLOT);
    BiomeBaseManager.HasLoadedData = true;
  }

  private IEnumerator CheckMusic()
  {
    if (DataManager.Instance.Followers.Count == 0)
    {
      Debug.Log((object) "0 followers play no follower track");
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.NoFollowers);
      while (DataManager.Instance.Followers.Count == 0)
        yield return (object) new WaitForSeconds(0.5f);
      Debug.Log((object) "1 follower play standard ambience track");
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
    }
    else
    {
      Debug.Log((object) ("Follower Count = " + (object) DataManager.Instance.Followers.Count));
      if (FollowerBrainStats.IsBloodMoon)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.blood_moon);
    }
  }

  private void OnDisable()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    if ((UnityEngine.Object) BiomeBaseManager.Instance == (UnityEngine.Object) this)
      BiomeBaseManager.Instance = (BiomeBaseManager) null;
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.UpgradeBase);
  }

  private void OnNewPhaseStarted()
  {
    this.PlayAtmos();
    if (DataManager.Instance.ShopKeeperChefEnragedDay > TimeManager.CurrentDay - 1 || DataManager.Instance.ShopKeeperChefState != 1)
      return;
    DataManager.Instance.ShopKeeperChefState = 0;
  }

  private void Start()
  {
    SimulationManager.Pause();
    this.StartCoroutine((IEnumerator) this.StartSetup());
  }

  private IEnumerator StartSetup()
  {
    BiomeBaseManager biomeBaseManager = this;
    FollowerLocation targetLocation = PlayerFarming.Location != FollowerLocation.Boss_5 || DataManager.Instance.CameFromDeathCatFight ? FollowerLocation.Base : FollowerLocation.DoorRoom;
    while (LocationManager.GetLocationState(targetLocation) != LocationState.Active)
      yield return (object) null;
    biomeBaseManager.PlayAtmos();
    biomeBaseManager.InitMusic();
    biomeBaseManager.PlacePlayer();
    biomeBaseManager.Church.SetActive(false);
    biomeBaseManager.Lumberjack.SetActive(false);
    if (targetLocation == FollowerLocation.Base)
    {
      biomeBaseManager.DoorRoom.gameObject.SetActive(false);
      biomeBaseManager.Room.SetColliderAndUpdatePathfinding();
    }
    else
    {
      biomeBaseManager.DoorRoom.gameObject.SetActive(true);
      biomeBaseManager.DoorRoom.SetColliderAndUpdatePathfinding();
    }
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (LocationManager.GetLocationState(follower.Location) == LocationState.Unloaded && FollowerManager.FindSimFollowerByID(follower.ID) == null)
      {
        SimFollower simFollower = new SimFollower(FollowerBrain.GetOrCreateBrain(follower));
        FollowerManager.SimFollowersAtLocation(follower.Location).Add(simFollower);
      }
    }
    foreach (FollowerLocation location in LocationManager.LocationsInState(LocationState.Unloaded))
    {
      foreach (StructuresData data in StructureManager.StructuresDataAtLocation(location))
      {
        if (StructureBrain.FindBrainByID(data.ID) == null)
          StructureBrain.CreateBrain(data);
      }
    }
    yield return (object) null;
    HUD_Manager.Instance.Hidden = false;
    HUD_Manager.Instance.Hide(true);
    while (((UnityEngine.Object) BaseLocationManager.Instance == (UnityEngine.Object) null || !BaseLocationManager.Instance.StructuresPlaced) && targetLocation == FollowerLocation.Base)
      yield return (object) null;
    MMTransition.ResumePlay();
    if (DataManager.Instance.LastRunResults == UIDeathScreenOverlayController.Results.BeatenBoss || DataManager.Instance.LastRunResults == UIDeathScreenOverlayController.Results.BeatenBossNoDamage)
      biomeBaseManager.StartCoroutine((IEnumerator) biomeBaseManager.BeatenLeaderIE());
    else
      SimulationManager.UnPause();
    if (targetLocation == FollowerLocation.DoorRoom)
      HUD_Manager.Instance.Show();
    yield return (object) new WaitForSeconds(5f);
    if (DataManager.Instance.LastRunResults != UIDeathScreenOverlayController.Results.None)
    {
      switch (DataManager.Instance.LastRunResults)
      {
        case UIDeathScreenOverlayController.Results.Killed:
          CultFaithManager.AddThought(Thought.Cult_DiedInDungeon);
          break;
        case UIDeathScreenOverlayController.Results.BeatenMiniBoss:
          CultFaithManager.AddThought(Thought.Cult_KilledMiniBoss);
          break;
        case UIDeathScreenOverlayController.Results.BeatenBoss:
        case UIDeathScreenOverlayController.Results.BeatenBossNoDamage:
          CultFaithManager.AddThought(Thought.Cult_KilledBoss);
          break;
      }
      DataManager.Instance.LastRunResults = UIDeathScreenOverlayController.Results.None;
    }
  }

  public bool SpawnExistingRecruits { get; set; }

  private void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || !DataManager.Instance.InTutorial || DataManager.Instance.Followers_Recruit.Count <= 0 || this.SpawnExistingRecruits || MMConversation.isPlaying || (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.RecruitSpawnLocation.transform.position) >= 8.0)
      return;
    this.SpawnExistingRecruits = true;
    System.Action newRecruitRevealed = this.OnNewRecruitRevealed;
    if (newRecruitRevealed != null)
      newRecruitRevealed();
    this.recruitParticles.Play();
    FollowerManager.SpawnExistingRecruits(this.RecruitSpawnLocation.transform.position);
  }

  private void PlacePlayer()
  {
    if (PlayerFarming.Location == FollowerLocation.Boss_5 && !DataManager.Instance.CameFromDeathCatFight)
    {
      this.StartCoroutine((IEnumerator) this.SpawnFromChainDoor());
    }
    else
    {
      LocationManager.LocationManagers[FollowerLocation.Base].PlacePlayer();
      Interaction_BaseTeleporter.Instance.TeleportIn();
    }
    DataManager.Instance.CameFromDeathCatFight = false;
  }

  private IEnumerator SpawnFromChainDoor()
  {
    LocationManager.LocationManagers[FollowerLocation.DoorRoom].PlacePlayer();
    PlayerFarming.Instance.transform.position = new Vector3(0.18f, 45.4f, -3f);
    GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.transform.position);
    while (LocationManager.GetLocationState(FollowerLocation.DoorRoom) != LocationState.Active)
      yield return (object) null;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    this.DoorRoom.gameObject.SetActive(true);
    this.DoorRoom.SetCollider();
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.DungeonDoor);
    this.Room.gameObject.SetActive(false);
    this.Church.SetActive(false);
    GameManager.RecalculatePaths(true);
  }

  private IEnumerator EndConversationZoom()
  {
    while (PlayerFarming.Instance.GoToAndStopping)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
  }

  public void ToggleChurch()
  {
    if (this.Church.activeSelf)
      this.ActivateRoom();
    else
      this.ActivateChurch();
  }

  public void ActivateChurch()
  {
    AudioManager.Instance.footstepOverride = "event:/material/footstep_hard";
    BiomeConstants.Instance.DisableIndicators();
    WeatherController.InsideBuilding = true;
    WeatherController.Instance.CheckWeather();
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom", AudioManager.Instance.Listener.gameObject);
    this.Church.SetActive(true);
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    AudioManager.Instance.StopCurrentAtmos();
    this.Room.gameObject.SetActive(false);
    this.DoorRoom.gameObject.SetActive(false);
    GameManager.RecalculatePaths(true);
    ChurchFollowerManager.Instance.UpdateChurch();
    KeyboardLightingManager.ForceTempleLayout();
  }

  public void ActivateRoom(bool changeMusic = true)
  {
    AudioManager.Instance.footstepOverride = string.Empty;
    WeatherController.InsideBuilding = false;
    WeatherController.Instance.CheckWeather();
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back", AudioManager.Instance.Listener.gameObject);
    if (changeMusic)
    {
      if (FollowerBrainStats.IsBloodMoon)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.blood_moon);
      else
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
    }
    this.Room.gameObject.SetActive(true);
    this.Room.SetCollider();
    this.PlayMusic();
    if (!this.biomeAtmosInstance.isValid())
    {
      if (!TimeManager.IsNight)
        AudioManager.Instance.PlayAtmos(this.biomeAtmosPath);
      else
        AudioManager.Instance.PlayAtmos(this.nightBiomeAtmosPath);
    }
    this.Church.SetActive(false);
    this.DoorRoom.gameObject.SetActive(false);
    GameManager.RecalculatePaths(true);
    if (!changeMusic)
      return;
    KeyboardLightingManager.ForceBaseLayout();
  }

  public void ActivateDoorRoom()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom", PlayerFarming.Instance.gameObject);
    this.DoorRoom.gameObject.SetActive(true);
    this.DoorRoom.SetCollider();
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.DungeonDoor);
    this.Room.gameObject.SetActive(false);
    this.Church.SetActive(false);
    GameManager.RecalculatePaths(true);
  }

  private void InitMusic()
  {
    AudioManager.Instance.PlayMusic("event:/music/base/base_main");
    this.StartCoroutine((IEnumerator) this.CheckMusic());
  }

  private void PlayMusic()
  {
  }

  private void PlayChurchMusic()
  {
  }

  public void BtnCreateRecruit()
  {
  }

  public void BtnCreateFollower()
  {
    FollowerManager.CreateNewFollower(FollowerLocation.Base, Vector3.zero);
  }

  public void BtnReset()
  {
    SimulationManager.Pause();
    FollowerManager.Reset();
    StructureManager.Reset();
    UIDynamicNotificationCenter.Reset();
    KeyboardLightingManager.Reset();
    GameManager.GoG_Initialised = false;
    TwitchManager.Abort();
    SaveAndLoad.ResetSave(SaveAndLoad.SAVE_SLOT, false);
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Base Biome 1", 0.5f, "", (System.Action) null);
  }

  public void BtnNextDay()
  {
    SaveAndLoad.Save();
    SimulationManager.Pause();
    FollowerManager.Followers.Clear();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Base Biome 1", 0.5f, "", (System.Action) null);
  }

  public void BtnFakeRunLog()
  {
    Inventory.AddItem(1, 20);
    ++DataManager.Instance.FollowerTokens;
    CheatConsole.SkipToPhase(TimeManager.CurrentPhase + 3);
  }

  public void BtnFakeRunBerries()
  {
    Inventory.AddItem(21, 35);
    ++DataManager.Instance.FollowerTokens;
    CheatConsole.SkipToPhase(TimeManager.CurrentPhase + 3);
  }

  private void UpgradeBase(UpgradeSystem.Type upgradeType)
  {
    if (upgradeType != UpgradeSystem.Type.Building_Temple2 && upgradeType != UpgradeSystem.Type.Temple_III && upgradeType != UpgradeSystem.Type.Temple_IV)
      return;
    this.StartCoroutine((IEnumerator) this.UpgradeBaseRoutine(upgradeType));
  }

  private IEnumerator UpgradeBaseRoutine(UpgradeSystem.Type upgradeType)
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    PlayerFarming p = PlayerFarming.Instance;
    PlayerFarming.Instance.gameObject.SetActive(false);
    GameObject shrine = BuildingShrine.Shrines[0].gameObject;
    SpriteRenderer componentInChildren = BuildingShrine.Shrines[0].ShrineCanLevelUp.GetComponentInChildren<SpriteRenderer>();
    if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null)
    {
      Material material = new Material(componentInChildren.material);
      componentInChildren.material = material;
      material.SetFloat(BiomeBaseManager.DepthThreshold, 0.0f);
    }
    SpriteRenderer component = BuildingShrine.Shrines[0].ShrineCantLevelUp.GetComponent<SpriteRenderer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Material material = new Material(component.material);
      component.material = material;
      material.SetFloat(BiomeBaseManager.DepthThreshold, 0.0f);
    }
    StructureBrain shrineBrain = (StructureBrain) BuildingShrine.Shrines[0].StructureBrain;
    Vector3 shrinePosition = shrine.transform.position;
    BuildingShrine.Shrines[0].EndIndicateHighlighted();
    GameManager.GetInstance().CamFollowTarget.AddTarget(shrine, 2f);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(shrine);
    MonoSingleton<Indicator>.Instance.gameObject.SetActive(false);
    SimulationManager.Pause();
    List<Follower> followers = new List<Follower>();
    foreach (Follower follower1 in FollowerManager.FollowersAtLocation(FollowerLocation.Base))
    {
      Follower follower = follower1;
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID) && follower.Brain.Info.CursedState == Thought.None)
      {
        Vector3 destination = shrine.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(3f, 3.5f);
        followers.Add(follower);
        follower.HideAllFollowerIcons();
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        follower.GoTo(destination, (System.Action) (() =>
        {
          if (!((UnityEngine.Object) shrine != (UnityEngine.Object) null))
            return;
          follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
          double num = (double) follower.SetBodyAnimation("build", true);
          follower.FacePosition(shrine.transform.position);
        }));
      }
    }
    MMVibrate.RumbleContinuous(0.0f, 0.25f);
    CameraManager.instance.ShakeCameraForDuration(0.05f, 0.2f, 5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/upgrade_cult", shrine.transform.position);
    yield return (object) new WaitForSeconds(5f);
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 4f, "", (System.Action) null);
    yield return (object) new WaitForSeconds(1.5f);
    MMVibrate.StopRumble();
    p.gameObject.SetActive(true);
    p.transform.position = shrine.transform.position - Vector3.up * 3f;
    p.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    StructureBrain brain = (StructureBrain) StructureManager.GetAllStructuresOfType<Structures_Temple>()[0];
    Vector3 position = brain.Data.Position;
    int type = (int) brain.Data.Type;
    StructureBrain.TYPES Type1 = StructureBrain.TYPES.NONE;
    StructureBrain.TYPES Type2 = StructureBrain.TYPES.NONE;
    StructureManager.RemoveStructure(shrineBrain);
    StructureManager.RemoveStructure(brain);
    switch (upgradeType)
    {
      case UpgradeSystem.Type.Building_Temple2:
        Type1 = StructureBrain.TYPES.SHRINE_II;
        Type2 = StructureBrain.TYPES.TEMPLE_II;
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Shrine_II);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Building_Temple2);
        break;
      case UpgradeSystem.Type.Temple_III:
        Type1 = StructureBrain.TYPES.SHRINE_III;
        Type2 = StructureBrain.TYPES.TEMPLE_III;
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Shrine_III);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Temple_III);
        break;
      case UpgradeSystem.Type.Temple_IV:
        Type1 = StructureBrain.TYPES.SHRINE_IV;
        Type2 = StructureBrain.TYPES.TEMPLE_IV;
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Shrine_IV);
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Temple_IV);
        break;
    }
    int counter = 0;
    StructuresData newShrineData = StructuresData.GetInfoByType(Type1, 0);
    newShrineData.Bounds = shrineBrain.Data.Bounds;
    newShrineData.GridTilePosition = shrineBrain.Data.GridTilePosition;
    newShrineData.PlacementRegionPosition = new Vector3Int((int) shrinePosition.x, (int) shrinePosition.y, 0);
    newShrineData.Fuel = shrineBrain.Data.Fuel;
    newShrineData.FullyFueled = shrineBrain.Data.FullyFueled;
    newShrineData.SoulCount = shrineBrain.Data.SoulCount;
    StructureManager.BuildStructure(FollowerLocation.Base, newShrineData, shrinePosition, new Vector2Int(2, 2), false, (Action<GameObject>) (obj =>
    {
      PlacementRegion.Instance.structureBrain.AddStructureToGrid(newShrineData, true);
      shrine = obj;
      ++counter;
    }));
    StructuresData newTempleData = StructuresData.GetInfoByType(Type2, 0);
    newTempleData.Bounds = brain.Data.Bounds;
    newTempleData.GridTilePosition = brain.Data.GridTilePosition;
    newTempleData.PlacementRegionPosition = new Vector3Int((int) position.x, (int) position.y, 0);
    PlacementRegion.Instance.structureBrain.RemoveFromGrid(newTempleData.GridTilePosition);
    StructureManager.BuildStructure(FollowerLocation.Base, newTempleData, position, new Vector2Int(5, 5), false, (Action<GameObject>) (obj =>
    {
      PlacementRegion.Instance.structureBrain.AddStructureToGrid(newTempleData, true);
      ++counter;
    }));
    while (counter < 2)
      yield return (object) null;
    foreach (Follower follower in followers)
    {
      follower.transform.position = shrine.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * UnityEngine.Random.Range(4f, 7f);
      follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) follower.SetBodyAnimation("cheer", true);
      follower.FacePosition(shrine.transform.position);
    }
    AudioManager.Instance.PlayOneShot("event:/building/finished_stone", shrine.transform.position);
    yield return (object) new WaitForSeconds(0.5f);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(shrine.transform.position);
    HUD_DisplayName.Play("UI/BaseUpgraded", 3, HUD_DisplayName.Positions.Centre);
    yield return (object) new WaitForSeconds(4f);
    foreach (Follower follower in followers)
      follower.Brain.CompleteCurrentTask();
    yield return (object) new WaitForSeconds(0.5f);
    GameManager.GetInstance().CamFollowTarget.RemoveTarget(shrine);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    p.state.CURRENT_STATE = StateMachine.State.Idle;
    MonoSingleton<Indicator>.Instance.gameObject.SetActive(true);
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    CultFaithManager.AddThought(Thought.Cult_BaseUpgraded);
  }

  public void BeatenLeaderRoutine() => this.StartCoroutine((IEnumerator) this.BeatenLeaderIE());

  private IEnumerator BeatenLeaderIE()
  {
    yield return (object) new WaitForSeconds(0.1f);
    List<Follower> availableFollowers = new List<Follower>();
    foreach (Follower follower in Follower.Followers)
    {
      if (!FollowerManager.FollowerLocked(follower.Brain.Info.ID))
        availableFollowers.Add(follower);
    }
    if (availableFollowers.Count > 0)
    {
      for (int index = 0; index < availableFollowers.Count; ++index)
      {
        availableFollowers[index].Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
        availableFollowers[index].transform.position = this.GetCirclePosition(availableFollowers, availableFollowers[index]);
        availableFollowers[index].FacePosition(PlayerFarming.Instance.transform.position);
        availableFollowers[index].State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) availableFollowers[index].SetBodyAnimation("dance", true);
      }
      while (LetterBox.IsPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
      yield return (object) new WaitForEndOfFrame();
      DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
      yield return (object) new WaitForSeconds(2f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "show-heart", false);
      AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_beat", PlayerFarming.Instance.transform.position);
      AudioManager.Instance.PlayOneShot("event:/monster_heart/monster_heart_sequence_Short", PlayerFarming.Instance.transform.position);
      MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      BiomeConstants.Instance.EmitConfettiVFX(PlayerFarming.Instance.transform.position);
      GameManager.GetInstance().CamFollowTarget.targetDistance = 10f;
      GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject, 12f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.2f, 0.5f);
      yield return (object) new WaitForSeconds(0.25f);
      for (int index = 0; index < availableFollowers.Count; ++index)
      {
        double num = (double) availableFollowers[index].SetBodyAnimation("cheer", true);
      }
      yield return (object) new WaitForSeconds(2.75f);
      yield return (object) new WaitForSeconds(2f);
      SimulationManager.UnPause();
      for (int index = 0; index < availableFollowers.Count; ++index)
        availableFollowers[index].Brain.CompleteCurrentTask();
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().CamFollowTarget.targetDistance = 10f;
    }
  }

  public Vector3 GetCirclePosition(List<Follower> availableFollowers, Follower follower)
  {
    int num1 = availableFollowers.IndexOf(follower);
    if (availableFollowers.Count <= 12)
    {
      float num2 = 2f;
      float f = (float) ((double) num1 * (360.0 / (double) availableFollowers.Count) * (Math.PI / 180.0));
      return PlayerFarming.Instance.transform.position + new Vector3(num2 * Mathf.Cos(f), num2 * Mathf.Sin(f));
    }
    int b = 8;
    float num3;
    float f1;
    if (num1 < b)
    {
      num3 = 2f;
      f1 = (float) ((double) num1 * (360.0 / (double) Mathf.Min(availableFollowers.Count, b)) * (Math.PI / 180.0));
    }
    else
    {
      num3 = 3f;
      f1 = (float) ((double) (num1 - b) * (360.0 / (double) (availableFollowers.Count - b)) * (Math.PI / 180.0));
    }
    return PlayerFarming.Instance.transform.position + new Vector3(num3 * Mathf.Cos(f1), num3 * Mathf.Sin(f1));
  }
}
