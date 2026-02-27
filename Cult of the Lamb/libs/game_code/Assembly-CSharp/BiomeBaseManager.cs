// Decompiled with JetBrains decompiler
// Type: BiomeBaseManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using MMRoomGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class BiomeBaseManager : MonoBehaviour
{
  public static bool HasLoadedData = false;
  public static bool Testing = true;
  public static bool EnterTemple = false;
  public static BiomeBaseManager _Instance;
  public GameObject PlayerSpawnLocation;
  public GameObject PlayerReturnFromDoorRoomLocation;
  public GameObject PlayerReturnFromEndlessLocation;
  public GameObject RecruitSpawnLocation;
  public ParticleSystem recruitParticles;
  public GameObject PlayerGreetLocation;
  public GameObject RunResults;
  public GameObject Church;
  public GenerateRoom DoorRoom;
  public GenerateRoom DLC_ShrineRoom;
  public Transform DLC_ShrineRoomPosition;
  public Transform DLC_ShrineRoomEntrance;
  public GameObject Lumberjack;
  public bool LoadData = true;
  public GameObject NorthBase;
  public GameObject SouthBase;
  public spiderShop SpiderShop;
  public string baseAtmosPath = "event:/atmos/forest/nightbugs_wind_birds";
  public string baseNightAtmosPath = "event:/atmos/forest/night";
  public string woolhavenAtmosPath = "event:/dlc/atmos/woolhaven";
  public string woolhavenNightAtmosPath = "event:/dlc/atmos/woolhaven_night";
  public EventInstance biomeAtmosInstance;
  [SerializeField]
  public Interaction_Ranch.Animal[] animals;
  public GenerateRoom Room;
  public static bool WalkedBack = false;
  public bool wasNight = true;
  public bool createdLoop;
  public Vector2[] cachedColliderPoints;
  public static List<int> SpawnedAnimalIDs = new List<int>();
  public bool tweening;
  public System.Action OnNewRecruitRevealed;
  [CompilerGenerated]
  public bool \u003CSpawnExistingRecruits\u003Ek__BackingField;
  public EventInstance iceOverlayLoopingSFX;
  public bool iceShown;
  public Tween iceTween;
  public BiomeLightingSettings LightingSettings;
  public OverrideLightingProperties overrideLightingProperties;
  public static int DepthThreshold = Shader.PropertyToID("_DepthThreshold");
  public static Dictionary<(int ranchID, int animalCount), List<Vector3>> _spawnPositionCache = new Dictionary<(int, int), List<Vector3>>();
  public int _nextSpawnIndex;
  public bool wolvesSucceeded;

  public static BiomeBaseManager Instance
  {
    get => BiomeBaseManager._Instance;
    set => BiomeBaseManager._Instance = value;
  }

  public string BiomeAtmosPath
  {
    get
    {
      return PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom ? this.woolhavenAtmosPath : this.baseAtmosPath;
    }
  }

  public string NightBiomeAtmosPath
  {
    get
    {
      return PlayerFarming.Location == FollowerLocation.DLC_ShrineRoom ? this.woolhavenNightAtmosPath : this.baseNightAtmosPath;
    }
  }

  public bool IsInTemple => (UnityEngine.Object) this.Church != (UnityEngine.Object) null && this.Church.activeInHierarchy;

  public Vector2[] CachedColliderPoints => this.cachedColliderPoints;

  public void Awake()
  {
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.SetIceOverlayReveal(0.0f);
    this.cachedColliderPoints = this.Room.Pieces[0].Collider.points;
    if ((PlayerFarming.Location == FollowerLocation.Boss_5 || PlayerFarming.LastLocation == FollowerLocation.Endless) && !DataManager.Instance.CameFromDeathCatFight)
      this.Room.gameObject.SetActive(false);
    BiomeBaseManager.Instance = this;
    if ((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null)
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    if (UIMenuBase.ActiveMenus != null)
      UIMenuBase.ActiveMenus.Clear();
    this.DLC_ShrineRoom.transform.position = this.DLC_ShrineRoomPosition.position;
    this.DLC_ShrineRoom.gameObject.SetActive(false);
    Interaction_WolfBase.OnWolvesSucceeded += new Interaction_WolfBase.WolfEvent(this.OnWolvesSucceeded);
    Interaction_WolfBase.OnWolvesFailed += new Interaction_WolfBase.WolfEvent(this.OnWolvesFailed);
  }

  public void PlayAtmos()
  {
    if (SeasonsManager.CurrentWeatherEvent == SeasonsManager.WeatherEvent.Blizzard)
      return;
    if (!this.createdLoop)
    {
      if (!TimeManager.IsNight)
      {
        AudioManager.Instance.PlayAtmos(this.BiomeAtmosPath);
        this.wasNight = false;
      }
      else
      {
        AudioManager.Instance.PlayAtmos(this.NightBiomeAtmosPath);
        this.wasNight = false;
      }
      this.createdLoop = true;
    }
    else if (!TimeManager.IsNight)
    {
      if (this.wasNight)
      {
        AudioManager.Instance.PlayAtmos(this.BiomeAtmosPath);
        this.wasNight = false;
      }
    }
    else if (!this.wasNight)
    {
      AudioManager.Instance.PlayAtmos(this.NightBiomeAtmosPath);
      this.wasNight = true;
    }
    DataManager.Instance.AllowSaving = true;
  }

  public void OnEnable()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    UpgradeSystem.OnUpgradeUnlocked += new UpgradeSystem.UnlockEvent(this.UpgradeBase);
    if (!Application.isEditor || SaveAndLoad.Loaded)
      this.LoadData = false;
    if (this.LoadData && !BiomeBaseManager.HasLoadedData)
    {
      Debug.Log((object) "LOAD!");
      SaveAndLoad.Load(SaveAndLoad.SAVE_SLOT);
      BiomeBaseManager.HasLoadedData = true;
    }
    DataManager.Instance.SandboxModeEnabled = false;
    AchievementsWrapper.DoAchievementsMismatchCheck();
  }

  public IEnumerator CheckMusic()
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
      Debug.Log((object) ("Follower Count = " + DataManager.Instance.Followers.Count.ToString()));
      if (FollowerBrainStats.IsBloodMoon)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.blood_moon);
      else if (FollowerBrainStats.IsNudism)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.nudist_ritual);
      else if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.winter_random);
    }
  }

  public void OnDisable()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    if ((UnityEngine.Object) BiomeBaseManager.Instance == (UnityEngine.Object) this)
      BiomeBaseManager.Instance = (BiomeBaseManager) null;
    AudioManager.Instance.StopLoop(this.iceOverlayLoopingSFX);
    UpgradeSystem.OnUpgradeUnlocked -= new UpgradeSystem.UnlockEvent(this.UpgradeBase);
  }

  public void OnNewPhaseStarted()
  {
    this.PlayAtmos();
    if (DataManager.Instance.ShopKeeperChefEnragedDay <= TimeManager.CurrentDay - 1 && DataManager.Instance.ShopKeeperChefState == 1)
      DataManager.Instance.ShopKeeperChefState = 0;
    if (SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter)
      return;
    if ((double) UnityEngine.Random.value < 0.25)
      StructureManager.PlaceIceBlock(FollowerLocation.Base, new List<Structures_PlacementRegion>()
      {
        PlacementRegion.Instance.structureBrain
      });
    if ((double) UnityEngine.Random.value >= 0.5)
      return;
    StructureManager.PlaceSnowDrift(FollowerLocation.Base, new List<Structures_PlacementRegion>()
    {
      PlacementRegion.Instance.structureBrain
    });
  }

  public void Start()
  {
    if (DataManager.Instance.YngyaOffering >= 2 && !SeasonsManager.Active)
      WeatherSystemController.Instance.SetWeather(WeatherSystemController.WeatherType.Snowing, WeatherSystemController.WeatherStrength.Dusting, 0.0f);
    SimulationManager.Pause();
    this.StartCoroutine(this.StartSetup());
    if (DataManager.Instance.LandPurchased != -1 && GameManager.AuthenticateMajorDLC())
      DLCLandController.Instance.ShowSlot(DataManager.Instance.LandPurchased);
    if (!LoreSystem.LoreAvailable(16 /*0x10*/))
      return;
    this.UpdateSpiderShopPosition();
  }

  public void UpdateSpiderShopPosition()
  {
    this.SpiderShop.transform.parent = DLCShrineRoomLocationManager.Instance.StructureLayer.transform;
    this.SpiderShop.transform.localPosition = new Vector3(30f, 0.0f, 0.0f);
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.iceOverlayLoopingSFX);
    BiomeBaseManager.SpawnedAnimalIDs.Clear();
    Interaction_WolfBase.OnWolvesSucceeded -= new Interaction_WolfBase.WolfEvent(this.OnWolvesSucceeded);
    Interaction_WolfBase.OnWolvesFailed -= new Interaction_WolfBase.WolfEvent(this.OnWolvesFailed);
    if ((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null)
      BiomeConstants.Instance.SetIceOverlayReveal(0.0f);
    if (Interaction_WolfBase.WolfTarget == -1)
      return;
    List<Structures_Ranch> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_Ranch>();
    structuresOfType.Shuffle<Structures_Ranch>();
    foreach (Structures_Ranch structuresRanch in structuresOfType)
    {
      if (structuresRanch.Data.Animals.Count > 0)
      {
        bool flag = false;
        for (int index = 0; index < structuresRanch.Data.Animals.Count; ++index)
        {
          if (structuresRanch.Data.Animals[index].State != Interaction_Ranchable.State.InsideHutch && structuresRanch.Data.Animals[index].State != Interaction_Ranchable.State.BabyInHutch)
          {
            structuresRanch.Data.Animals[index].State = Interaction_Ranchable.State.Dead;
            flag = true;
            break;
          }
        }
        if (flag)
          break;
      }
    }
    this.OnWolvesFailed();
  }

  public IEnumerator StartSetup()
  {
    BiomeBaseManager biomeBaseManager = this;
    MMTransition.CanResume = false;
    bool wasInEndlessDungeon = PlayerFarming.LastLocation == FollowerLocation.Endless;
    FollowerLocation targetLocation = PlayerFarming.LastLocation != FollowerLocation.Endless && PlayerFarming.Location != FollowerLocation.Boss_5 || DataManager.Instance.CameFromDeathCatFight ? FollowerLocation.Base : FollowerLocation.DoorRoom;
    LocationManager[] locationManagerArray = UnityEngine.Object.FindObjectsOfType<LocationManager>();
    for (int index = 0; index < locationManagerArray.Length; ++index)
    {
      LocationManager location = locationManagerArray[index];
      yield return (object) new WaitUntil((Func<bool>) (() => location.IsInitialized));
    }
    locationManagerArray = (LocationManager[]) null;
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
      biomeBaseManager.DLC_ShrineRoom.gameObject.SetActive(false);
      biomeBaseManager.Room.SetColliderAndUpdatePathfinding();
    }
    else
    {
      biomeBaseManager.DoorRoom.gameObject.SetActive(true);
      biomeBaseManager.DoorRoom.SetColliderAndUpdatePathfinding();
    }
    DataManager.Instance.Followers.Sort((Comparison<FollowerInfo>) ((a, b) => a.ID.CompareTo(b.ID)));
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (LocationManager.GetLocationState(follower.Location) == LocationState.Unloaded && FollowerManager.FindSimFollowerByID(follower.ID) == null)
      {
        SimFollower simFollower = new SimFollower(FollowerBrain.GetOrCreateBrain(follower));
        FollowerManager.SimFollowersAtLocation(follower.Location).Add(simFollower);
      }
    }
    if (DataManager.Instance.SpyDay == -1)
      DataManager.Instance.SpyDay = TimeManager.CurrentDay >= 15 ? TimeManager.CurrentDay + UnityEngine.Random.Range(1, 10) : 15;
    if (TimeManager.CurrentDay > DataManager.Instance.SpyDay && FollowerManager.GetSpyCount() <= 0)
    {
      if (TimeManager.CurrentDay > 25 && (double) UnityEngine.Random.value < 0.5)
      {
        DataManager.Instance.SpyDay = TimeManager.CurrentDay + UnityEngine.Random.Range(10, 20);
      }
      else
      {
        biomeBaseManager.AddSpyFollower();
        DataManager.Instance.SpyDay = TimeManager.CurrentDay + UnityEngine.Random.Range(35, 65);
      }
    }
    foreach (FollowerLocation location in LocationManager.LocationsInState(LocationState.Unloaded))
    {
      foreach (StructuresData data in StructureManager.StructuresDataAtLocation(location))
      {
        if (!StructureBrain.TryFindBrainByID(in data.ID, out StructureBrain _))
          StructureBrain.CreateBrain(data);
      }
    }
    yield return (object) null;
    HUD_Manager.Instance.Hidden = false;
    HUD_Manager.Instance.Hide(true);
    while (((UnityEngine.Object) BaseLocationManager.Instance == (UnityEngine.Object) null || !BaseLocationManager.Instance.StructuresPlaced || !BaseLocationManager.Instance.FollowersSpawned) && targetLocation == FollowerLocation.Base)
      yield return (object) null;
    MMTransition.CanResume = true;
    MMTransition.ResumePlay();
    if (DataManager.Instance.LastRunResults == UIDeathScreenOverlayController.Results.BeatenBoss || DataManager.Instance.LastRunResults == UIDeathScreenOverlayController.Results.BeatenBossNoDamage)
      biomeBaseManager.StartCoroutine(biomeBaseManager.BeatenLeaderIE());
    else
      SimulationManager.UnPause();
    if (DataManager.Instance.BeatenExecutioner && !CultUpgradeData.IsUnlocked(CultUpgradeData.TYPE.Border6))
      biomeBaseManager.StartCoroutine(biomeBaseManager.WaitForPlayerToBeReady((System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationNew();
        UICultUpgradesMenuController upgradesMenuController = MonoSingleton<UIManager>.Instance.ShowCultUpgradesMenu();
        upgradesMenuController.preventCloseUntilRevealComplete = true;
        upgradesMenuController.Revealing = CultUpgradeData.TYPE.Border6;
        upgradesMenuController.OnHide += (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
      })));
    else if (DataManager.Instance.BeatenWolf && !CultUpgradeData.IsUnlocked(CultUpgradeData.TYPE.Border5))
      biomeBaseManager.StartCoroutine(biomeBaseManager.WaitForPlayerToBeReady((System.Action) (() =>
      {
        GameManager.GetInstance().OnConversationNew();
        UICultUpgradesMenuController upgradesMenuController = MonoSingleton<UIManager>.Instance.ShowCultUpgradesMenu();
        upgradesMenuController.preventCloseUntilRevealComplete = true;
        upgradesMenuController.Revealing = CultUpgradeData.TYPE.Border5;
        upgradesMenuController.OnHide += (System.Action) (() => GameManager.GetInstance().OnConversationEnd());
      })));
    if (targetLocation == FollowerLocation.DoorRoom)
      HUD_Manager.Instance.Show();
    Interaction_WolfBase.ResetWolvesEnounterData();
    if (targetLocation == FollowerLocation.Base)
      biomeBaseManager.SpawnAnimals();
    yield return (object) new WaitForSeconds(5f);
    if (DataManager.Instance.LastRunResults != UIDeathScreenOverlayController.Results.None)
    {
      switch (DataManager.Instance.LastRunResults)
      {
        case UIDeathScreenOverlayController.Results.Killed:
          if (!wasInEndlessDungeon)
            CultFaithManager.AddThought(Thought.Cult_DiedInDungeon);
          DataManager.Instance.PlayerDeathDay = TimeManager.CurrentDay;
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

  public IEnumerator WaitForPlayerToBeReady(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    while (LetterBox.IsPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public bool SpawnExistingRecruits
  {
    get => this.\u003CSpawnExistingRecruits\u003Ek__BackingField;
    set => this.\u003CSpawnExistingRecruits\u003Ek__BackingField = value;
  }

  public void Update()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && DataManager.Instance.InTutorial && DataManager.Instance.Followers_Recruit.Count > 0 && !this.SpawnExistingRecruits && PlayerFarming.Location == FollowerLocation.Base && !MMConversation.isPlaying && (double) Vector3.Distance(PlayerFarming.Instance.transform.position, this.RecruitSpawnLocation.transform.position) < 8.0)
    {
      this.SpawnExistingRecruits = true;
      System.Action newRecruitRevealed = this.OnNewRecruitRevealed;
      if (newRecruitRevealed != null)
        newRecruitRevealed();
      this.recruitParticles.Play();
      FollowerManager.SpawnExistingRecruits(this.RecruitSpawnLocation.transform.position);
    }
    this.iceShown = BiomeConstants.Instance.ShowingIceOverlay();
    if (this.iceOverlayLoopingSFX.isValid() && !this.iceShown)
      AudioManager.Instance.StopLoop(this.iceOverlayLoopingSFX);
    if (PlayerFarming.Location != FollowerLocation.Base || !DataManager.Instance.BuiltFurnace || !((UnityEngine.Object) Interaction_DLCFurnace.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) Interaction_DLCFurnace.Instance.Structure != (UnityEngine.Object) null) || Interaction_DLCFurnace.Instance.Structure.Brain == null)
      return;
    int fuel = Interaction_DLCFurnace.Instance.Structure.Brain.Data.Fuel;
    if (FollowerBrainStats.IsWarmthRitual)
    {
      if (!this.iceShown || this.tweening)
        return;
      AudioManager.Instance.StopLoop(this.iceOverlayLoopingSFX);
      AudioManager.Instance.PlayOneShot("event:/dlc/atmos/ice_overlay_stop");
      this.tweening = true;
      float t = 1f;
      this.iceTween.Kill();
      this.iceTween = (Tween) DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 0.0f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => BiomeConstants.Instance.SetIceOverlayReveal(t))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.tweening = false));
    }
    else if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
    {
      if (fuel <= 0 && !this.iceShown && !this.tweening)
      {
        this.iceOverlayLoopingSFX = AudioManager.Instance.CreateLoop("event:/dlc/atmos/ice_overlay_loop", true);
        AudioManager.Instance.PlayOneShot("event:/dlc/atmos/ice_overlay_start");
        this.tweening = true;
        float t = 0.0f;
        this.iceTween.Kill();
        this.iceTween = (Tween) DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 1f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => BiomeConstants.Instance.SetIceOverlayReveal(t))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.tweening = false));
      }
      else
      {
        if (fuel <= 0 || !this.iceShown || this.tweening)
          return;
        AudioManager.Instance.StopLoop(this.iceOverlayLoopingSFX);
        AudioManager.Instance.PlayOneShot("event:/dlc/atmos/ice_overlay_stop");
        this.tweening = true;
        float t = 1f;
        this.iceTween.Kill();
        this.iceTween = (Tween) DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 0.0f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => BiomeConstants.Instance.SetIceOverlayReveal(t))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.tweening = false));
      }
    }
    else
    {
      if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || !this.iceShown || this.tweening)
        return;
      AudioManager.Instance.StopLoop(this.iceOverlayLoopingSFX);
      AudioManager.Instance.PlayOneShot("event:/dlc/atmos/ice_overlay_stop");
      this.tweening = true;
      float t = 1f;
      this.iceTween.Kill();
      this.iceTween = (Tween) DOTween.To((DOGetter<float>) (() => t), (DOSetter<float>) (x => t = x), 0.0f, 1f).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => BiomeConstants.Instance.SetIceOverlayReveal(t))).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.tweening = false));
    }
  }

  public void PlacePlayer()
  {
    if (PlayerFarming.LastLocation == FollowerLocation.Boss_5 && !DataManager.Instance.CameFromDeathCatFight)
      this.StartCoroutine(this.SpawnFromChainDoor());
    else if (PlayerFarming.LastLocation == FollowerLocation.Endless)
    {
      PlayerFarming.LastLocation = PlayerFarming.Location;
      LocationManager.DeactivateLocation(FollowerLocation.Dungeon1_1);
      this.StartCoroutine(this.SpawnFromEndlessPortal());
    }
    else if (DataManager.Instance.SurvivalModeActive && DataManager.Instance.SurvivalModeFirstSpawn)
    {
      DataManager.Instance.SurvivalModeFirstSpawn = false;
      LocationManager.LocationManagers[FollowerLocation.Base].PlacePlayer();
      Interaction_BaseTeleporter.Instance.WakeUpInBase();
    }
    else if (DataManager.Instance.WinterModeActive && DataManager.Instance.SurvivalModeFirstSpawn)
    {
      DataManager.Instance.SurvivalModeFirstSpawn = false;
      LocationManager.LocationManagers[FollowerLocation.Base].PlacePlayer();
      Interaction_BaseTeleporter.Instance.WakeUpInBase();
    }
    else
    {
      LocationManager.DeactivateLocation(FollowerLocation.Dungeon1_1);
      LocationManager.LocationManagers[FollowerLocation.Base].PlacePlayer();
      Interaction_BaseTeleporter.Instance.TeleportIn();
    }
    DataManager.Instance.CameFromDeathCatFight = false;
    if (!PlayerFarming.AutoRespawn)
      return;
    if (PlayerFarming.playersCount == 1)
      CoopManager.Instance.SpawnCoopPlayer(1);
    PlayerFarming.AutoRespawn = false;
  }

  public IEnumerator SpawnFromChainDoor()
  {
    LocationManager.LocationManagers[FollowerLocation.DoorRoom].PlacePlayer();
    PlayerFarming.PositionAllPlayers(new Vector3(0.18f, 44.5f, -2.94f));
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
    PlayerFarming.RefreshPlayersCount();
    GameManager.RecalculatePaths(true);
  }

  public IEnumerator SpawnFromEndlessPortal()
  {
    LocationManager.LocationManagers[FollowerLocation.DoorRoom].PlacePlayer();
    PlayerFarming.Instance.transform.position = this.PlayerReturnFromEndlessLocation.transform.position;
    GameManager.GetInstance().CameraSnapToPosition(PlayerFarming.Instance.transform.position);
    while (LocationManager.GetLocationState(FollowerLocation.DoorRoom) != LocationState.Active)
      yield return (object) null;
    yield return (object) new WaitForEndOfFrame();
    this.DoorRoom.gameObject.SetActive(true);
    this.DoorRoom.SetCollider();
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.DungeonDoor);
    this.Room.gameObject.SetActive(false);
    this.Church.SetActive(false);
    GameManager.RecalculatePaths(true);
    GameManager.GetInstance().OnConversationNew((PlayerFarming) null);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.gameObject);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.state.facingAngle = PlayerFarming.Instance.state.LookAngle = 0.0f;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "warp-in-up", false);
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.circleCollider2D.enabled = false;
    PlayerFarming.Instance.transform.DOMove(this.PlayerReturnFromEndlessLocation.transform.position - Vector3.up * 3f, 1f);
    yield return (object) new WaitForSeconds(2.7f);
    SimulationManager.UnPause();
    PlayerFarming.Instance.circleCollider2D.enabled = true;
    GameManager.GetInstance().OnConversationEnd();
  }

  public IEnumerator EndConversationZoom()
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

  public void ActivateChurch() => this.ActivateChurch(true);

  public void ActivateChurch(bool playTempleMusic)
  {
    AudioManager.Instance.footstepOverride = "event:/material/footstep_hard";
    BiomeConstants.Instance.DisableIndicators();
    BiomeConstants.Instance.SetIceOverlayReveal(0.0f);
    WeatherSystemController.Instance.EnteredBuilding();
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom", AudioManager.Instance.Listener.gameObject);
    this.Church.SetActive(true);
    if (playTempleMusic)
      AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.Temple);
    AudioManager.Instance.StopCurrentAtmos();
    this.Room.gameObject.SetActive(false);
    this.DoorRoom.gameObject.SetActive(false);
    this.DLC_ShrineRoom.gameObject.SetActive(false);
    GameManager.RecalculatePaths(true);
    ChurchFollowerManager.Instance.UpdateChurch();
    DeviceLightingManager.ForceTempleLayout();
  }

  public void ActivateRoom(bool changeMusic = true)
  {
    Interaction_TempleAltar.Instance.Interacted = false;
    AudioManager.Instance.footstepOverride = string.Empty;
    if (PlayerFarming.Location == FollowerLocation.Church)
      WeatherSystemController.Instance.ExitedBuilding();
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back", AudioManager.Instance.Listener.gameObject);
    if (changeMusic)
    {
      if (FollowerBrainStats.IsBloodMoon)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.blood_moon);
      else if (FollowerBrainStats.IsNudism)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.nudist_ritual);
      else if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.winter_random);
      else
        AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.StandardAmbience);
    }
    this.Room.gameObject.SetActive(true);
    if (changeMusic)
      this.Room.UpdateAstar();
    this.Room.SetCollider();
    this.PlayMusic();
    if (!this.biomeAtmosInstance.isValid() && SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
    {
      if (!TimeManager.IsNight)
        AudioManager.Instance.PlayAtmos(this.BiomeAtmosPath);
      else
        AudioManager.Instance.PlayAtmos(this.NightBiomeAtmosPath);
    }
    this.Church.SetActive(false);
    this.DoorRoom.gameObject.SetActive(false);
    this.DLC_ShrineRoom.gameObject.SetActive(false);
    GameManager.RecalculatePaths(true);
    if (changeMusic)
      DeviceLightingManager.ForceBaseLayout();
    this.RefreshCoopBlockers();
  }

  public void RefreshCoopBlockers()
  {
    if (CoopManager.CoopActive)
      return;
    CoopManager.EnableCoopBlockers(false, true);
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
    BiomeConstants.Instance.SetIceOverlayReveal(0.0f);
  }

  public void ActivateDLCShrineRoom()
  {
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom", PlayerFarming.Instance.gameObject);
    this.DLC_ShrineRoom.gameObject.SetActive(true);
    this.DLC_ShrineRoom.SetCollider();
    AudioManager.Instance.SetMusicBaseID(SoundConstants.BaseID.woolhaven);
    if (SeasonsManager.CurrentWeatherEvent != SeasonsManager.WeatherEvent.Blizzard)
      AudioManager.Instance.PlayAtmos("event:/dlc/atmos/woolhaven");
    this.Room.gameObject.SetActive(false);
    this.Church.SetActive(false);
    GameManager.GetInstance().WaitForSeconds(0.0f, (System.Action) (() => GameManager.RecalculatePaths(true)));
    BiomeConstants.Instance.SetIceOverlayReveal(0.0f);
  }

  public void InitMusic()
  {
    if (TimeManager.CurrentPhase == DayPhase.Night)
      AudioManager.Instance.ToggleFilter(SoundParams.Night, true);
    AudioManager.Instance.PlayMusic("event:/music/base/base_main");
    this.StartCoroutine(this.CheckMusic());
  }

  public void PlayMusic()
  {
  }

  public void PlayChurchMusic()
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
    DeviceLightingManager.Reset();
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

  public void UpgradeBase(UpgradeSystem.Type upgradeType)
  {
    if (upgradeType != UpgradeSystem.Type.Building_Temple2 && upgradeType != UpgradeSystem.Type.Temple_III && upgradeType != UpgradeSystem.Type.Temple_IV)
      return;
    this.StartCoroutine(this.UpgradeBaseRoutine(upgradeType));
  }

  public IEnumerator UpgradeBaseRoutine(UpgradeSystem.Type upgradeType)
  {
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    foreach (Component player in PlayerFarming.players)
      player.gameObject.SetActive(false);
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
    BuildingShrine.Shrines[0].EndIndicateHighlighted(BuildingShrine.Shrines[0].playerFarming);
    GameManager.GetInstance().CamFollowTarget.AddTarget(shrine, 2f);
    GameManager.GetInstance().OnConversationNew((PlayerFarming) null);
    GameManager.GetInstance().OnConversationNext(shrine);
    foreach (PlayerFarming player in PlayerFarming.players)
      player.indicator.SetGameObjectActive(false);
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
    PlayerFarming.SetFacingAngleForAll(270f);
    PlayerFarming.PositionAllPlayers(shrine.transform.position - Vector3.up * 3f);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.gameObject.SetActive(true);
      player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      player.simpleSpineAnimator.Animate("idle", 0, true);
    }
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
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.state.CURRENT_STATE = StateMachine.State.Idle;
      player.indicator.SetGameObjectActive(true);
    }
    SimulationManager.UnPause();
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    CultFaithManager.AddThought(Thought.Cult_BaseUpgraded);
  }

  public void BeatenLeaderRoutine() => this.StartCoroutine(this.BeatenLeaderIE());

  public IEnumerator BeatenLeaderIE()
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

  public void AddSpyFollower()
  {
    Follower newFollower = FollowerManager.CreateNewFollower(FollowerLocation.Base, Vector3.zero);
    newFollower.Brain.Info.FollowerRole = FollowerRole.Worshipper;
    newFollower.Brain.Info.Outfit = FollowerOutfitType.Follower;
    newFollower.SetOutfit(FollowerOutfitType.Follower, false);
    newFollower.Brain.AddTrait(FollowerTrait.TraitType.Spy);
    ThoughtData data = FollowerThoughts.GetData((Thought) UnityEngine.Random.Range(420, 425));
    data.Init();
    newFollower.Brain._directInfoAccess.Thoughts.Add(data);
    newFollower.Brain.CheckChangeTask();
    DataManager.Instance.SpyJoinedDay = TimeManager.CurrentDay;
    DataManager.Instance.SpyDay = UnityEngine.Random.Range(35, 65);
  }

  public AssetReferenceGameObject GetAddressable(StructuresData.Ranchable_Animal animal)
  {
    foreach (Interaction_Ranch.Animal animal1 in this.animals)
    {
      if (animal1.Type == animal.Type)
        return animal1.Addressable;
    }
    return (AssetReferenceGameObject) null;
  }

  public void SpawnAnimals()
  {
    for (int index = 0; index < DataManager.Instance.BreakingOutAnimals.Count; ++index)
    {
      Vector3 position = DataManager.Instance.BreakingOutAnimals[index].LastPosition + (Vector3) UnityEngine.Random.insideUnitCircle * 5f;
      this.SpawnAnimal(DataManager.Instance.BreakingOutAnimals[index], position, true, (Interaction_Ranch) null, BaseLocationManager.Instance.StructureLayer);
    }
    for (int index = 0; index < DataManager.Instance.FollowingPlayerAnimals.Length; ++index)
    {
      if (DataManager.Instance.FollowingPlayerAnimals[index] != null)
      {
        Vector3 position = DataManager.Instance.FollowingPlayerAnimals[index].LastPosition + (Vector3) UnityEngine.Random.insideUnitCircle * 5f;
        this.SpawnAnimal(DataManager.Instance.FollowingPlayerAnimals[index], position, true, (Interaction_Ranch) null, BaseLocationManager.Instance.StructureLayer);
      }
    }
    this._nextSpawnIndex = 0;
    BiomeBaseManager._spawnPositionCache.Clear();
    foreach (Structures_Ranch structuresRanch in StructureManager.GetAllStructuresOfType<Structures_Ranch>())
    {
      for (int index = structuresRanch.Data.Animals.Count - 1; index >= 0; --index)
      {
        Interaction_Ranch ranch = Interaction_Ranch.GetRanch(structuresRanch.Data.ID);
        Vector3 nextSpawnPosition = this.GetNextSpawnPosition(ranch, ref this._nextSpawnIndex);
        this.SpawnAnimal(structuresRanch.Data.Animals[index], nextSpawnPosition, false, ranch, BaseLocationManager.Instance.StructureLayer);
      }
      this._nextSpawnIndex = 0;
    }
  }

  public void SpawnAnimal(
    StructuresData.Ranchable_Animal animal,
    Vector3 position,
    bool breakingOut,
    Interaction_Ranch ranch,
    Transform parent,
    Action<Interaction_Ranchable> callback = null)
  {
    if (animal.ID != -1)
    {
      for (int index = 0; index < BiomeBaseManager.SpawnedAnimalIDs.Count; ++index)
      {
        if (BiomeBaseManager.SpawnedAnimalIDs[index] == animal.ID)
          return;
      }
      BiomeBaseManager.SpawnedAnimalIDs.Add(animal.ID);
    }
    Addressables.InstantiateAsync((object) this.GetAddressable(animal), parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
    {
      Interaction_Ranchable component = obj.Result.GetComponent<Interaction_Ranchable>();
      if (animal.State == Interaction_Ranchable.State.Dead)
        component.transform.position = animal.LastPosition;
      else
        component.transform.position = new Vector3(Mathf.Clamp(position.x, PlacementRegion.X_Constraints.x + 5f, PlacementRegion.X_Constraints.y - 5f), Mathf.Clamp(position.y, PlacementRegion.Y_Constraints.x + 5f, PlacementRegion.Y_Constraints.y - 5f), 0.0f);
      component.Configure(animal, ranch);
      Action<Interaction_Ranchable> action = callback;
      if (action != null)
        action(component);
      if (!breakingOut)
        return;
      component.CheckBreakingOut();
    });
  }

  public Vector3 GetRandomSpawnPosition(Interaction_Ranch ranch)
  {
    return (UnityEngine.Object) ranch != (UnityEngine.Object) null && ranch.Brain.RanchingTiles.Count > 0 ? ranch.Brain.RanchingTiles[UnityEngine.Random.Range(0, ranch.Brain.RanchingTiles.Count)].WorldPosition : this.transform.position - Vector3.forward + Vector3.right * UnityEngine.Random.Range(-1f, 1f);
  }

  public Vector3 GetNextSpawnPosition(Interaction_Ranch ranch, ref int nextSpawnIndex)
  {
    if ((UnityEngine.Object) ranch != (UnityEngine.Object) null && ranch.Brain.RanchingTiles.Count > 0)
    {
      int id = ranch.Brain.Data.ID;
      return BiomeBaseManager.GetEvenlyDistributedPositions(ranch.Brain.RanchingTiles, ranch.Brain.Data.Animals.Count, id)[nextSpawnIndex++];
    }
    return !((UnityEngine.Object) ranch != (UnityEngine.Object) null) ? Vector3.zero : ranch.transform.position;
  }

  public static List<Vector3> GetEvenlyDistributedPositions(
    List<PlacementRegion.TileGridTile> tiles,
    int count,
    int ranchID)
  {
    if (tiles == null || tiles.Count == 0 || count <= 0)
      return new List<Vector3>();
    (int, int) key = (ranchID, count);
    List<Vector3> distributedPositions1;
    if (BiomeBaseManager._spawnPositionCache.TryGetValue(key, out distributedPositions1))
      return distributedPositions1;
    List<PlacementRegion.TileGridTile> list = tiles.OrderBy<PlacementRegion.TileGridTile, float>((Func<PlacementRegion.TileGridTile, float>) (t => t.WorldPosition.y)).ThenBy<PlacementRegion.TileGridTile, float>((Func<PlacementRegion.TileGridTile, float>) (t => t.WorldPosition.x)).ToList<PlacementRegion.TileGridTile>();
    List<Vector3> distributedPositions2 = new List<Vector3>();
    for (int index1 = 0; index1 < count; ++index1)
    {
      int index2 = Mathf.RoundToInt((float) index1 * ((float) list.Count - 1f) / (float) Mathf.Max(count - 1, 1));
      distributedPositions2.Add(list[index2].WorldPosition);
    }
    BiomeBaseManager._spawnPositionCache[key] = distributedPositions2;
    return distributedPositions2;
  }

  public IEnumerator FrameDelayCallback(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void CombineDLCRoomColliders()
  {
    if (!DataManager.Instance.MAJOR_DLC || DataManager.Instance.LandPurchased == -1 || !GameManager.AuthenticateMajorDLC())
      return;
    this.Room.Pieces[0].Collider = DLCLandController.Instance.CombineColliders(this.cachedColliderPoints, this.Room.Pieces[0].Collider);
    this.Room.SetColliderAndUpdatePathfinding();
  }

  public void OnWolvesSucceeded() => this.StartCoroutine(this.WolvesSucceededIE());

  public IEnumerator WolvesSucceededIE()
  {
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || MMConversation.isPlaying || LetterBox.IsPlaying || PlayerFarming.Instance.GoToAndStopping || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || SimulationManager.IsPaused)
      yield return (object) null;
    if (!this.wolvesSucceeded)
    {
      this.wolvesSucceeded = true;
      yield return (object) new WaitForSeconds(2f);
      CultFaithManager.AddThought(Thought.Cult_WolvesSucceeded);
      Interaction_Ranch interactionRanch = (Interaction_Ranch) null;
      foreach (Interaction_Ranch ranch in Interaction_Ranch.Ranches)
      {
        if (ranch.Brain.Data.Animals.Count > 0 && ((UnityEngine.Object) interactionRanch == (UnityEngine.Object) null || ranch.Brain.Data.Animals.Count > interactionRanch.Brain.Data.Animals.Count))
          interactionRanch = ranch;
      }
      GameObject obj = new GameObject();
      if ((UnityEngine.Object) interactionRanch != (UnityEngine.Object) null)
      {
        List<Interaction_Ranchable> animals = interactionRanch.GetAnimals();
        Vector3 zero = Vector3.zero;
        foreach (Interaction_Ranchable interactionRanchable in animals)
          zero += interactionRanchable.transform.position;
        obj.transform.position = zero / (float) animals.Count;
      }
      else if (Interaction_Ranchable.Ranchables.Count > 0)
      {
        int index = UnityEngine.Random.Range(0, Interaction_Ranchable.Ranchables.Count);
        obj.transform.position = Interaction_Ranchable.Ranchables[index].transform.position;
      }
      else
      {
        Debug.LogError((object) "No Ranchables after wolves attack!");
        obj.transform.position = PlayerFarming.Instance.transform.position;
      }
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(obj, 7f);
      yield return (object) new WaitForSeconds(2f);
      foreach (Interaction_Ranchable ranchable in Interaction_Ranchable.Ranchables)
      {
        if (ranchable.CurrentState != Interaction_Ranchable.State.BabyInHutch && ranchable.CurrentState != Interaction_Ranchable.State.Dead)
        {
          ranchable.AddAdoration(50f);
          BiomeConstants.Instance.EmitHeartPickUpVFX(ranchable.transform.position, 0.0f, "red", "burst_small");
          AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/love_hearts", ranchable.transform.position);
        }
        yield return (object) new WaitForSeconds(0.2f);
      }
      yield return (object) new WaitForSeconds(1f);
      this.InitMusic();
      GameManager.GetInstance().OnConversationEnd();
      Interaction_WolfBase.ResetWolvesEnounterData();
      UnityEngine.Object.Destroy((UnityEngine.Object) obj);
      this.wolvesSucceeded = false;
    }
  }

  public void OnWolvesFailed()
  {
    this.InitMusic();
    CultFaithManager.AddThought(Thought.Cult_WolvesFailed);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__62_3() => this.tweening = false;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__62_7() => this.tweening = false;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__62_11() => this.tweening = false;

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__62_15() => this.tweening = false;
}
