// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.Assets;
using Lamb.UI.PauseMenu;
using Microsoft.CSharp.RuntimeBinder;
using MMBiomeGeneration;
using MMTools;
using Pathfinding;
using src.UI.Overlays.TutorialOverlay;
using src.UINavigator;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

#nullable disable
public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public BoxCollider2D boxCollider;
  public static bool reactivateGraph = false;
  public static float reactivateGraphTimer;
  public GameObject GameOverScreen;
  public static float CurrentZ;
  public static bool IsQuitting;
  public UpgradeTreeConfiguration UpgradeTreeConfiguration;
  public UpgradeTreeConfiguration UpgradePlayerConfiguration;
  public UpgradeTreeConfiguration DLCUpgradeTreeConfiguration;
  public RenderTexture LightingRenderTexture;
  public Color StartItemsInWoodsColor = new Color(0.0741f, 0.0f, 0.129f, 1f);
  public float _UnscaledTime;
  public float scaledTimeElapsed;
  public AudioSource audioSource;
  public static bool GoG_Initialised = false;
  public float autoPauseTimestamp;
  public const float idleTimeUntilAutoPause = 600f;
  public bool shouldPlayerBeInactiveDuringConversation;
  public System.Action OnDLCsAuthenticated;
  public static bool overridePlayerPosition = false;
  public static Coroutine AdjustShadersCoroutine;
  public static float _timeinDungeon = 0.0f;
  public List<int> GameSpeed = new List<int>() { 1, 2, 3 };
  public int CurrentGameSpeed;
  public CameraFollowTarget _CamFollowTarget;
  public List<CameraFollowTarget.Target> CachedCamTargets = new List<CameraFollowTarget.Target>();
  [CompilerGenerated]
  public float \u003CCachedZoom\u003Ek__BackingField = -999f;
  public Coroutine currentZoomRoutine;
  public static bool InMenu = false;
  public UIPauseMenuController _pauseMenuInstance;
  public bool DisplayingInactiveWarning;
  public float HoldToResetDemo;
  public Coroutine cGeneratePathfinding;
  public static float TimeScale = 1f;
  public static bool DungeonUseAllLayers = false;
  public static int CurrentDungeonLayer = 1;
  public static int PreviousDungeonLayer = 0;
  public static int CurrentDungeonFloor = 1;
  public static int _DungeonEndlessLevel = 1;
  public static string DungeonNameTerm;
  public static bool InitialDungeonEnter = true;
  public static bool CULTISTPAC_DLC = false;
  public static bool HERETICPAC_DLC = false;
  public static bool CTHULHUWOR_DLC = false;
  public static bool SINFULPAC_DLC = false;
  public static bool PILGRIMPAC_DLC = false;
  public EndOfDay EndOfDayScreen;
  public static int GlobalDitherIntensity = Shader.PropertyToID("_GlobalDitherIntensity");
  public static int GlobablOcclusionEnabled = Shader.PropertyToID("_GlobablOcclusionEnabled");
  public static int GlobalResourceHighlight = Shader.PropertyToID("_GlobalResourceHighlight");
  public static int TimeOfDayColor = Shader.PropertyToID("_TimeOfDayColor");
  public static int CloudAlpha = Shader.PropertyToID("_CloudAlpha");
  public static int CloudDensity = Shader.PropertyToID("_CloudDensity");
  public static int VerticalFogGradientSpread = Shader.PropertyToID("_VerticalFog_GradientSpread");
  public static int VerticalFogZOffset = Shader.PropertyToID("_VerticalFog_ZOffset");
  public static int LightingRenderTexture1 = Shader.PropertyToID("_Lighting_RenderTexture");
  public static int WindDensity = Shader.PropertyToID("_WindDensity");
  public static int WindSpeed = Shader.PropertyToID("_WindSpeed");
  public static int WindDiection = Shader.PropertyToID("_WindDiection");
  public static int ItemInWoodsColor = Shader.PropertyToID("_ItemInWoodsColor");
  public static int GlobalTimeUnscaled = Shader.PropertyToID("_GlobalTimeUnscaled");

  public static bool RoomActive
  {
    get
    {
      return !((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null) || BiomeGenerator.Instance.CurrentRoom.Active;
    }
  }

  public static BoxCollider2D BoxCollider => GameManager.instance.boxCollider;

  public float CurrentTime => this.scaledTimeElapsed;

  public static float DeltaTime => Time.deltaTime * 60f;

  public static float UnscaledDeltaTime => Time.unscaledDeltaTime * 60f;

  public static float FixedDeltaTime => Time.fixedDeltaTime * 60f;

  public static float FixedUnscaledDeltaTime => Time.fixedUnscaledDeltaTime * 60f;

  public void Awake()
  {
    GameManager.ForceFontReload();
    GameManager.instance = this;
    this.boxCollider = this.gameObject.AddComponent<BoxCollider2D>();
    this.boxCollider.isTrigger = true;
    GarbageCollector.incrementalTimeSliceNanoseconds = 1000000UL;
    this.autoPauseTimestamp = Time.unscaledTime + 600f;
    Singleton<SettingsManager>.Instance.LoadAndApply();
    DifficultyManager.LoadCurrentDifficulty();
    DataManager.Instance.ReplaceDeprication(this);
    DataManager.Instance.Followers_Transitioning_IDs.Clear();
    if (DataManager.Instance.WeaponPool.Count == 0)
    {
      DataManager.Instance.AddWeapon(EquipmentType.Sword);
      if (CheatConsole.IN_DEMO)
      {
        DataManager.Instance.AddWeapon(EquipmentType.Dagger);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Dagger_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Axe);
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Axe_Poison);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Critical);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Fervour);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Healing);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Nercomancy);
        DataManager.Instance.AddWeapon(EquipmentType.Sword_Poison);
      }
    }
    if (DataManager.Instance.CursePool.Count == 0)
    {
      DataManager.Instance.AddCurse(EquipmentType.Fireball);
      if (CheatConsole.IN_DEMO)
      {
        DataManager.Instance.AddCurse(EquipmentType.Tentacles);
        DataManager.Instance.AddCurse(EquipmentType.EnemyBlast);
        DataManager.Instance.AddCurse(EquipmentType.ProjectileAOE);
        DataManager.Instance.AddCurse(EquipmentType.MegaSlash);
      }
    }
    if (DataManager.Instance.UnlockedFleeces.Count == 0)
      DataManager.Instance.UnlockedFleeces.Add(0);
    if (!DataManager.Instance.UnlockedFleeces.Contains(1003) && DataManager.Instance.UnlockedFleeces.Count > 0)
      DataManager.Instance.UnlockedFleeces.Add(1003);
    if (DataManager.Instance.RecipesDiscovered.Count == 0)
      DataManager.Instance.RecipesDiscovered.Add(InventoryItem.ITEM_TYPE.MEAL_BERRIES);
    if (DataManager.Instance.DiscoveredLocations.Count == 0)
    {
      DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.Base);
      DataManager.Instance.VisitedLocations.Add(FollowerLocation.Base);
    }
    if (DataManager.Instance.UnlockedUpgrades.Count == 0)
    {
      if (!DataManager.Instance.SurvivalModeActive)
        DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.System_PlayerTent);
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_UnlockCurse);
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_UnlockWeapon);
    }
    if (!DataManager.Instance.SurvivalModeActive && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.System_PlayerTent))
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.System_PlayerTent);
    if (DataManager.Instance.PlayerFoundTrinkets.Count == 0)
      DataManager.Instance.PlayerFoundTrinkets = new List<TarotCards.Card>()
      {
        TarotCards.Card.Hearts1,
        TarotCards.Card.Lovers1,
        TarotCards.Card.EyeOfWeakness,
        TarotCards.Card.Telescope,
        TarotCards.Card.DiseasedHeart,
        TarotCards.Card.Spider,
        TarotCards.Card.AttackRate,
        TarotCards.Card.IncreasedDamage,
        TarotCards.Card.IncreaseBlackSoulsDrop,
        TarotCards.Card.NegateDamageChance,
        TarotCards.Card.AmmoEfficient,
        TarotCards.Card.HealTwiceAmount,
        TarotCards.Card.DeathsDoor,
        TarotCards.Card.GiftFromBelow,
        TarotCards.Card.RabbitFoot
      };
    List<FollowerClothingType> followerClothingTypeList = new List<FollowerClothingType>((IEnumerable<FollowerClothingType>) DataManager.Instance.UnlockedClothing);
    for (int index = followerClothingTypeList.Count - 1; index >= 0; --index)
    {
      if (DataManager.Instance.Cultist_DLC_Clothing.Contains<FollowerClothingType>(followerClothingTypeList[index]) || DataManager.Instance.Heretic_DLC_Clothing.Contains<FollowerClothingType>(followerClothingTypeList[index]))
        followerClothingTypeList.RemoveAt(index);
    }
    if (followerClothingTypeList.Count == 0)
    {
      DataManager.Instance.UnlockedClothing.Add(FollowerClothingType.None);
      DataManager.Instance.UnlockedClothing.Add(FollowerClothingType.Normal_1);
      DataManager.Instance.UnlockedClothing.Add(FollowerClothingType.Normal_2);
      DataManager.Instance.UnlockedClothing.Add(FollowerClothingType.Normal_3);
    }
    if (DataManager.Instance.WeaponSelectionPositions.Count == 0)
      DataManager.Instance.WeaponSelectionPositions = new List<TarotCards.Card>()
      {
        TarotCards.Card.Dagger,
        TarotCards.Card.Axe,
        TarotCards.Card.Gauntlet,
        TarotCards.Card.Hammer
      };
    TwitchAuthentication.OnAuthenticated += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    TwitchFollowers.SendFollowers();
  }

  public void TwitchAuthentication_OnAuthenticated()
  {
    this.StartCoroutine((IEnumerator) GameManager.\u003CTwitchAuthentication_OnAuthenticated\u003Eg__Wait\u007C35_0());
    TwitchFollowers.SendFollowers();
    DataManager.Instance.TwitchSentFollowers = true;
  }

  public void Start()
  {
    this.SetGlobalShaders();
    SteamAPI.Init();
    this.CheckDLCStatus();
    SeasonsManager.Initialise();
    if (DataManager.Instance.eggsHatched < 1)
      return;
    AudioManager.Instance.SetMusicParam(SoundParams.BabyFollowerAdded, 1f);
  }

  public void OnEnable()
  {
    GameManager.instance = this;
    this.CachedCamTargets = new List<CameraFollowTarget.Target>();
    AudioManager.Instance.SetGameManager(this);
    MMConversation.OnConversationNew += new MMConversation.ConversationNew(this.OnConversationNew);
    MMConversation.OnConversationNext += new MMConversation.ConversationNext(this.OnConversationNext);
    MMConversation.OnConversationEnd += new MMConversation.ConversationEnd(this.OnConversationEnd);
    Application.quitting += (System.Action) new System.Action(this.OnQuit);
    TimeManager.OnNewDayStarted += new System.Action(this.Save);
    TwitchAuthentication.TryAuthenticate((Action<TwitchRequest.ResponseType>) null);
    if (CheatConsole.IN_DEMO)
      CheatConsole.ForceResetTimeSinceLastKeyPress();
    this.CheckAchievements();
    Singleton<AccessibilityManager>.Instance.OnStopTimeInCrusadeChanged += new Action<bool>(this.OnStopTimeInCrusadeSettingChanged);
  }

  public void OnApplicationFocus(bool focus)
  {
    TwitchAuthentication.TryAuthenticate((Action<TwitchRequest.ResponseType>) null);
  }

  public void OnApplicationQuit() => TwitchManager.Abort();

  public void CheckAchievements()
  {
    if ((UnityEngine.Object) Unify.Achievements.Instance == (UnityEngine.Object) null)
      return;
    int num1 = 0;
    for (int index = 0; index < DataManager.Instance.PlayerFoundTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.Instance.PlayerFoundTrinkets[index]) && !TarotCards.MajorDLCCards.Contains<TarotCards.Card>(DataManager.Instance.PlayerFoundTrinkets[index]))
        ++num1;
    }
    int num2 = 0;
    for (int index = 0; index < DataManager.AllTrinkets.Count; ++index)
    {
      if (!TarotCards.CoopCards.Contains<TarotCards.Card>(DataManager.AllTrinkets[index]) && !TarotCards.MajorDLCCards.Contains<TarotCards.Card>(DataManager.AllTrinkets[index]))
        ++num2;
    }
    if (num1 >= num2)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
    if (!DataManager.CheckIfThereAreSkinsAvailableAll())
    {
      Debug.Log((object) "Follower Skin Achievement Unlocked");
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_SKINS_UNLOCKED"));
    }
    if (DataManager.Instance.BeatenExecutioner)
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup(AchievementsWrapper.Tags.BEAT_EXECUTIONER));
    if (TailorManager.HasUnlockedAllClothing())
      AchievementsWrapper.UnlockAchievement(Unify.Achievements.Instance.Lookup("ALL_OUTFITS"));
    DataManager.CheckAllLegendaryWeaponsUnlocked();
  }

  public static bool AuthenticateCultistDLC()
  {
    Debug.Log((object) "## AuthenticateCultistDLC");
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2015880U));
  }

  public static bool AuthenticateHereticDLC()
  {
    Debug.Log((object) "## AuthenticateHereticDLC");
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2331540U));
  }

  public static bool AuthenticateSinfulDLC()
  {
    Debug.Log((object) "## AuthenticateSinfulDLC");
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2646130U));
  }

  public static bool AuthenticatePilgrimDLC()
  {
    Debug.Log((object) "## AuthenticatePilgrimDLC");
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2890190U));
  }

  public static bool AuthenticatePrePurchaseDLC()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2013550U));
  }

  public static bool AuthenticatePlushBonusDLC()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(1944680U));
  }

  public static bool AuthenticatePAXDLC()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202626U));
  }

  public static bool AuthenticateMajorDLC()
  {
    return !CheatConsole.MAJOR_DLC_DISABLED && SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(3840050U));
  }

  public void OnDestroy()
  {
    TwitchAuthentication.OnAuthenticated -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
    SeasonsManager.Deinitialise();
  }

  public void OnDisable()
  {
    if ((UnityEngine.Object) GameManager.instance == (UnityEngine.Object) this)
      GameManager.instance = (GameManager) null;
    MMConversation.OnConversationNew -= new MMConversation.ConversationNew(this.OnConversationNew);
    MMConversation.OnConversationNext -= new MMConversation.ConversationNext(this.OnConversationNext);
    MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(this.OnConversationEnd);
    Application.quitting -= (System.Action) new System.Action(this.OnQuit);
    TimeManager.OnNewDayStarted -= new System.Action(this.Save);
    Singleton<AccessibilityManager>.Instance.OnStopTimeInCrusadeChanged -= new Action<bool>(this.OnStopTimeInCrusadeSettingChanged);
  }

  public void Save()
  {
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
      return;
    SaveAndLoad.Save();
  }

  public void OnQuit() => GameManager.IsQuitting = true;

  public void CheckDLCStatus()
  {
    Debug.Log((object) "## Wait For DLC Check");
    this.StartCoroutine((IEnumerator) GameManager.WaitForTime(1f, (System.Action) (() =>
    {
      Debug.Log((object) "## Checking DLC");
      if (!GameManager.AuthenticateHereticDLC() && DataManager.Instance.DLC_Heretic_Pack)
      {
        Debug.Log((object) "## Deactivate HERETIC DLC");
        DataManager.DeactivateHereticDLC();
      }
      if (!GameManager.AuthenticateCultistDLC() && DataManager.Instance.DLC_Cultist_Pack)
      {
        Debug.Log((object) "## Deactivate CULTIST DLC");
        DataManager.DeactivateCultistDLC();
      }
      if (!GameManager.AuthenticateSinfulDLC() && DataManager.Instance.DLC_Sinful_Pack)
      {
        Debug.Log((object) "## Deactivate SINFUL DLC");
        DataManager.DeactivateSinfulDLC();
      }
      if (!GameManager.AuthenticatePilgrimDLC() && DataManager.Instance.DLC_Pilgrim_Pack)
      {
        Debug.Log((object) "## Deactivate SINFUL DLC");
        DataManager.DeactivatePilgrimDLC();
      }
      if (!GameManager.AuthenticatePrePurchaseDLC() && DataManager.Instance.DLC_Pre_Purchase)
      {
        Debug.Log((object) "## Deactivate PRE PURCHASE");
        DataManager.DeactivatePrePurchaseDLC();
      }
      if (!GameManager.AuthenticateMajorDLC() && DataManager.Instance.MAJOR_DLC)
      {
        Debug.Log((object) "## Deactivate PRE PURCHASE");
        DataManager.DeactivateWoolhavenDLC();
      }
      this.StartCoroutine((IEnumerator) GameManager.WaitForDLCCheck(1f, (System.Action) (() =>
      {
        float delay = 2f;
        if (PlayerFarming.Location == FollowerLocation.Base)
        {
          Debug.Log((object) "Authenticating DLC");
          if (GameManager.AuthenticateSinfulDLC())
          {
            Debug.Log((object) "## Activate SINGFUL DLC");
            if (DataManager.ActivateSinfulDLC())
              this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
              {
                Debug.Log((object) "## SINGFUL DLC Notification");
                NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/SinfulEdition");
              })));
          }
          if (GameManager.AuthenticateHereticDLC())
          {
            delay += 0.5f;
            Debug.Log((object) "## Activate HERETIC DLC");
            if (DataManager.ActivateHereticDLC())
              this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
              {
                Debug.Log((object) "## HERETIC DLC Notification");
                NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/HereticEdition");
              })));
          }
          if (GameManager.AuthenticateCultistDLC())
          {
            delay += 0.5f;
            Debug.Log((object) "## Activate CULTIST DLC");
            if (DataManager.ActivateCultistDLC())
              this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
              {
                Debug.Log((object) "## CULTIST DLC Notification");
                NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CultistEdition");
              })));
          }
          if (GameManager.AuthenticatePilgrimDLC())
          {
            delay += 0.5f;
            Debug.Log((object) "## Activate PILGRIM DLC");
            if (DataManager.ActivatePilgrimDLC())
              this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
              {
                Debug.Log((object) "## PILGRIM DLC Notification");
                NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/PilgrimPack");
              })));
          }
          if (GameManager.AuthenticateMajorDLC())
          {
            delay += 0.5f;
            Debug.Log((object) "## Activate MAJOR DLC");
            if (DataManager.ActivateMajorDLC())
              this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
              {
                Debug.Log((object) "## MAJOR DLC Notification");
                NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/Name");
              })));
          }
          else
            DataManager.Instance.MAJOR_DLC = false;
          if (GameManager.AuthenticatePrePurchaseDLC())
          {
            float num = delay + 0.5f;
            Debug.Log((object) "## Activate PRE PURCHASE");
            if (DataManager.ActivatePrePurchaseDLC())
              this.StartCoroutine((IEnumerator) GameManager.WaitForTime(num + 1.5f, (System.Action) (() =>
              {
                Debug.Log((object) "## PRE PURCHASE DLC Notification");
                NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CthuluPack");
              })));
          }
          if (GameManager.AuthenticatePlushBonusDLC() && DataManager.ActivatePlushBonusDLC())
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "Structures/DECORATION_PLUSH");
          if (GameManager.AuthenticatePAXDLC() && DataManager.ActivatePAXDLC())
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "PAX DLC");
          if (GameManager.AuthenticateTwitchDrop1() && DataManager.ActivateTwitchDrop1())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop2() && DataManager.ActivateTwitchDrop2())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop3() && DataManager.ActivateTwitchDrop3())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop4() && DataManager.ActivateTwitchDrop4())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop5() && DataManager.ActivateTwitchDrop5())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop6() && DataManager.ActivateTwitchDrop6())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop7() && DataManager.ActivateTwitchDrop7())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
          if (GameManager.AuthenticateTwitchDrop8() && DataManager.ActivateTwitchDrop8())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop9() && DataManager.ActivateTwitchDrop9())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop10() && DataManager.ActivateTwitchDrop10())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop11() && DataManager.ActivateTwitchDrop11())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop12() && DataManager.ActivateTwitchDrop12())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop13() && DataManager.ActivateTwitchDrop13())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop14() && DataManager.ActivateTwitchDrop14())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop15() && DataManager.ActivateTwitchDrop15())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop16() && DataManager.ActivateTwitchDrop16())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop17() && DataManager.ActivateTwitchDrop17())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop18() && DataManager.ActivateTwitchDrop18())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop19() && DataManager.ActivateTwitchDrop19())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
          if (GameManager.AuthenticateTwitchDrop20() && DataManager.ActivateTwitchDrop20())
            NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        }
        System.Action dlCsAuthenticated = this.OnDLCsAuthenticated;
        if (dlCsAuthenticated == null)
          return;
        dlCsAuthenticated();
      })));
    })));
  }

  public static IEnumerator WaitForDLCCheck(float delay, System.Action callback)
  {
    while (PlayerFarming.Location != FollowerLocation.Base)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static IEnumerator WaitForTime(float delay, System.Action callback)
  {
    Debug.Log((object) (nameof (WaitForTime) + delay.ToString()));
    yield return (object) new UnityEngine.WaitForSecondsRealtime(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void SetGlobalShaders()
  {
    Shader.SetGlobalFloat(GameManager.GlobalTimeUnscaled, this._UnscaledTime);
    Shader.SetGlobalColor(GameManager.ItemInWoodsColor, this.StartItemsInWoodsColor);
    Vector2 vector2 = new Vector2(1f, 0.2f);
    Shader.SetGlobalVector(GameManager.WindDiection, (Vector4) vector2);
    Shader.SetGlobalFloat(GameManager.WindSpeed, 3f);
    Shader.SetGlobalFloat(GameManager.WindDensity, 0.1f);
    Shader.SetGlobalTexture(GameManager.LightingRenderTexture1, (Texture) this.LightingRenderTexture);
    Shader.SetGlobalFloat(GameManager.VerticalFogZOffset, 0.1f);
    Shader.SetGlobalFloat(GameManager.VerticalFogGradientSpread, 1f);
    Shader.SetGlobalFloat(GameManager.CloudDensity, 1f);
    Shader.SetGlobalFloat(GameManager.CloudAlpha, 0.1f);
    Shader.SetGlobalColor(GameManager.TimeOfDayColor, Color.white);
    Shader.SetGlobalFloat(GameManager.GlobalDitherIntensity, SettingsManager.Settings.Accessibility.DitherFadeDistance);
    Shader.SetGlobalFloat(GameManager.GlobablOcclusionEnabled, 1f);
    Shader.SetGlobalInt(GameManager.GlobalResourceHighlight, 1);
  }

  public void GetPlayerPosition()
  {
    if (GameManager.overridePlayerPosition || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    Shader.SetGlobalVector("_PlayerPosition", (Vector4) PlayerFarming.AveragePlayerPosition);
  }

  public static void setDefaultGlobalShaders() => GameManager.instance.SetGlobalShaders();

  public static void startCoroutineAdjustGlobalShaders(
    List<BiomeVolume> MyList,
    float BlendTime,
    float Start,
    float Target)
  {
    if (GameManager.AdjustShadersCoroutine != null)
      GameManager.instance.StopCoroutine(GameManager.AdjustShadersCoroutine);
    GameManager.AdjustShadersCoroutine = GameManager.instance.StartCoroutine((IEnumerator) GameManager.instance.adjustGlobalShaders(MyList, BlendTime, Start, Target));
  }

  public static void SetGlobalOcclusionActive(bool active)
  {
    Shader.SetGlobalFloat(GameManager.GlobablOcclusionEnabled, active ? 1f : 0.0f);
  }

  public static void SetDither(float ditherIntensity)
  {
    Shader.SetGlobalFloat(GameManager.GlobalDitherIntensity, ditherIntensity * PlayerFarming.CoopDitherMultiplier);
  }

  public void SetDitherTween(float value, float duration = 1f)
  {
    DOTween.Kill((object) this);
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + value, duration).SetEase<Tweener>(Ease.OutQuart);
  }

  public IEnumerator adjustGlobalShaders(
    List<BiomeVolume> MyList,
    float duration,
    float Start,
    float Target)
  {
    float Progress = 0.0f;
    List<object> startValues = new List<object>();
    for (int index = 0; index < MyList.Count; ++index)
    {
      if (MyList[index].types == BiomeVolume.ShaderTypes.Float)
        startValues.Add((object) Shader.GetGlobalFloat(MyList[index].shaderName));
      else if (MyList[index].types == BiomeVolume.ShaderTypes.Color)
        startValues.Add((object) Shader.GetGlobalColor(MyList[index].shaderName));
      else if (MyList[index].types == BiomeVolume.ShaderTypes.Texture)
        startValues.Add((object) Shader.GetGlobalTexture(MyList[index].shaderName));
      else if (MyList[index].types == BiomeVolume.ShaderTypes.Vector2)
        startValues.Add((object) Shader.GetGlobalVector(MyList[index].shaderName));
    }
    if ((double) duration > 0.0)
    {
      while ((double) (Progress += Time.deltaTime) < (double) duration)
      {
        float t = Progress / duration;
        for (int index = 0; index < MyList.Count; ++index)
        {
          if (MyList[index] == null)
            Debug.Log((object) $"MyList{index.ToString()}Is Null and will soft lock the game");
          else if (MyList[index].types == BiomeVolume.ShaderTypes.Float)
          {
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__65.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__65.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, float>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (float), typeof (GameManager)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, float> target = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__1.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, float>> p1 = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__1;
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__65.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__65.\u003C\u003Ep__0 = CallSite<Func<CallSite, System.Type, object, float, float, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Lerp", (IEnumerable<System.Type>) null, typeof (GameManager), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__0.Target((CallSite) GameManager.\u003C\u003Eo__65.\u003C\u003Ep__0, typeof (Mathf), startValues[index], MyList[index].valueToGoTo, Mathf.SmoothStep(Start, Target, t));
            float num = target((CallSite) p1, obj);
            Shader.SetGlobalFloat(MyList[index].shaderName, num);
          }
          else if (MyList[index].types == BiomeVolume.ShaderTypes.Color)
          {
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__65.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__65.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Color), typeof (GameManager)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Color> target = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__3.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Color>> p3 = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__3;
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__65.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__65.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, Color, float, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Lerp", (IEnumerable<System.Type>) null, typeof (GameManager), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__2.Target((CallSite) GameManager.\u003C\u003Eo__65.\u003C\u003Ep__2, typeof (Color), startValues[index], MyList[index].colorToGoTo, Mathf.SmoothStep(Start, Target, t));
            Color color = target((CallSite) p3, obj);
            Shader.SetGlobalColor(MyList[index].shaderName, color);
          }
          else if (MyList[index].types != BiomeVolume.ShaderTypes.Texture && MyList[index].types == BiomeVolume.ShaderTypes.Vector2)
          {
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__65.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__65.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Vector2>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Vector2), typeof (GameManager)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Vector2> target = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Vector2>> p5 = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__5;
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__65.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__65.\u003C\u003Ep__4 = CallSite<Func<CallSite, System.Type, object, Vector2, float, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Lerp", (IEnumerable<System.Type>) null, typeof (GameManager), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = GameManager.\u003C\u003Eo__65.\u003C\u003Ep__4.Target((CallSite) GameManager.\u003C\u003Eo__65.\u003C\u003Ep__4, typeof (Vector2), startValues[index], MyList[index].Vector2ToGoTo, Mathf.SmoothStep(Start, Target, t));
            Vector2 vector2 = target((CallSite) p5, obj);
            Shader.SetGlobalVector(MyList[index].shaderName, (Vector4) vector2);
          }
        }
        yield return (object) null;
      }
    }
    else
    {
      for (int index = 0; index < MyList.Count; ++index)
      {
        try
        {
          if (MyList[index].types == BiomeVolume.ShaderTypes.Float)
            Shader.SetGlobalFloat(MyList[index].shaderName, MyList[index].valueToGoTo);
          else if (MyList[index].types == BiomeVolume.ShaderTypes.Color)
            Shader.SetGlobalColor(MyList[index].shaderName, MyList[index].colorToGoTo);
          else if (MyList[index].types != BiomeVolume.ShaderTypes.Texture)
          {
            if (MyList[index].types == BiomeVolume.ShaderTypes.Float)
              Shader.SetGlobalVector(MyList[index].shaderName, (Vector4) MyList[index].Vector2ToGoTo);
          }
        }
        catch (Exception ex)
        {
          Debug.Log((object) $"AdjustGlobalShaders exception shader=${MyList[index].shaderName} index=${index}");
          Debug.Log((object) MyList[index]);
        }
      }
    }
  }

  public static bool AuthenticateTwitchDrop1()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2090901U));
  }

  public static bool AuthenticateTwitchDrop2()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2071370U));
  }

  public static bool AuthenticateTwitchDrop3()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2090900U));
  }

  public static bool AuthenticateTwitchDrop4()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202621U));
  }

  public static bool AuthenticateTwitchDrop5()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202620U));
  }

  public static bool AuthenticateTwitchDrop6()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202622U));
  }

  public static bool AuthenticateTwitchDrop7()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202623U));
  }

  public static bool AuthenticateTwitchDrop8()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202624U));
  }

  public static bool AuthenticateTwitchDrop9()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2202625U));
  }

  public static bool AuthenticateTwitchDrop10()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2647140U));
  }

  public static bool AuthenticateTwitchDrop11()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2647150U));
  }

  public static bool AuthenticateTwitchDrop12()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2647160U));
  }

  public static bool AuthenticateTwitchDrop13()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2945130U));
  }

  public static bool AuthenticateTwitchDrop14()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2945140U));
  }

  public static bool AuthenticateTwitchDrop15()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2945120U));
  }

  public static bool AuthenticateTwitchDrop16()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(4153010U));
  }

  public static bool AuthenticateTwitchDrop17()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(4153020U));
  }

  public static bool AuthenticateTwitchDrop18()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(4153030U));
  }

  public static bool AuthenticateTwitchDrop19()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(4153040U));
  }

  public static bool AuthenticateTwitchDrop20()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(4153050U));
  }

  public static bool AuthenticateSupportStreamer()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(3017350U));
  }

  public static float TimeInDungeon
  {
    get => GameManager._timeinDungeon;
    set => GameManager._timeinDungeon = value;
  }

  public void NextGameSpeed()
  {
    this.CurrentGameSpeed = ++this.CurrentGameSpeed % this.GameSpeed.Count;
    GameManager.SetTimeScale((float) this.GameSpeed[this.CurrentGameSpeed]);
    Debug.Log((object) $"{Time.timeScale.ToString()}   {this.CurrentGameSpeed.ToString()}  {this.GameSpeed[this.CurrentGameSpeed].ToString()}");
  }

  public CameraFollowTarget CamFollowTarget
  {
    get
    {
      if ((UnityEngine.Object) this._CamFollowTarget == (UnityEngine.Object) null)
        this._CamFollowTarget = Camera.main?.GetComponent<CameraFollowTarget>();
      if ((UnityEngine.Object) this._CamFollowTarget == (UnityEngine.Object) null || !this._CamFollowTarget.gameObject.activeSelf)
        this._CamFollowTarget = CameraFollowTarget.Instance;
      return this._CamFollowTarget;
    }
  }

  public void OnConversationNew(PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    this.OnConversationNew(true, true, false, playerFarming);
  }

  public void OnConversationNew(
    bool SetPlayerInactive = true,
    bool SnapLetterBox = true,
    PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    this.OnConversationNew(SetPlayerInactive, SnapLetterBox, true, playerFarming);
  }

  public void FreezeAllies()
  {
    List<Health> healthList = new List<Health>();
    for (int index = Health.playerTeam.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null && Health.playerTeam[index].gameObject.activeSelf && (UnityEngine.Object) Health.playerTeam[index].GetComponent<UnitObject>() != (UnityEngine.Object) null && Health.playerTeam[index].CanBeFreezedInCustscene)
        healthList.Add(Health.playerTeam[index]);
    }
    foreach (Health health in healthList)
    {
      if (!health.isPlayer)
        health.AddFreezeTime();
    }
    for (int index = Familiar.Familiars.Count - 1; index >= 0; --index)
    {
      if (Familiar.Familiars[index].team == Health.Team.PlayerTeam)
        Familiar.Familiars[index].FreezeForCutscene();
    }
  }

  public void UnFreezeAllies()
  {
    List<Health> healthList = new List<Health>();
    for (int index = Health.playerTeam.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.playerTeam[index] != (UnityEngine.Object) null && Health.playerTeam[index].gameObject.activeSelf && (UnityEngine.Object) Health.playerTeam[index].GetComponent<UnitObject>() != (UnityEngine.Object) null && Health.playerTeam[index].CanBeFreezedInCustscene)
        healthList.Add(Health.playerTeam[index]);
    }
    foreach (Health health in healthList)
    {
      if (!health.isPlayer && health.TimeFrozen)
        health.ClearFreezeTime();
    }
    for (int index = Familiar.Familiars.Count - 1; index >= 0; --index)
    {
      if (Familiar.Familiars[index].team == Health.Team.PlayerTeam)
        Familiar.Familiars[index].UnFreezeForCutscene();
    }
  }

  public bool IsPlayerInactiveInConversation()
  {
    return LetterBox.IsPlaying && this.shouldPlayerBeInactiveDuringConversation;
  }

  public void OnConversationNew(
    bool SetPlayerInactive,
    bool SnapLetterBox,
    bool showLetterBox,
    PlayerFarming playerFarming)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
      playerFarming = PlayerFarming.Instance;
    if ((UnityEngine.Object) NotificationCentreScreen.Instance != (UnityEngine.Object) null)
      NotificationCentreScreen.Instance.FadeAndStop();
    this.shouldPlayerBeInactiveDuringConversation = SetPlayerInactive;
    this.ResetCachedCameraTargets();
    this.CameraSetConversationMode(true);
    this.CacheCameraTargets();
    this.RemoveAllFromCamera();
    if (SetPlayerInactive && (UnityEngine.Object) playerFarming != (UnityEngine.Object) null)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
        player.SetInactive();
      this.FreezeAllies();
    }
    if (showLetterBox)
    {
      if (SnapLetterBox && (bool) (UnityEngine.Object) HUD_Manager.Instance)
        HUD_Manager.Instance.Hide(true, 0);
      LetterBox.Show(SnapLetterBox);
      DOTween.Kill((object) this);
      DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + 5f, 1f).SetEase<Tweener>(Ease.OutQuart);
    }
    else
    {
      if (!(bool) (UnityEngine.Object) HUD_Manager.Instance)
        return;
      HUD_Manager.Instance.Hide(false, 0);
    }
  }

  public void OnConversationNext(GameObject Speaker, float Zoom = 9f)
  {
    if ((UnityEngine.Object) Speaker != (UnityEngine.Object) null)
    {
      this.RemoveAllFromCamera();
      this.AddToCamera(Speaker.gameObject);
      Debug.Log((object) $"SPeaker game object added{Speaker.name} {Speaker.gameObject.name}");
    }
    else
      Debug.Log((object) "SPeaker is null - check coop");
    this.CamFollowTarget.targetDistance = Zoom;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    BiomeConstants.Instance.DepthOfFieldTween(1.5f, Zoom + 1f, 10f, 1f, 145f);
  }

  public void OnConversationEnd(bool SetPlayerToIdle = true, bool ShowHUD = true)
  {
    if (SetPlayerToIdle && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if (PlayerFarming.Instance.GoToAndStopping)
      {
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Moving;
        PlayerFarming.Instance.IdleOnEnd = true;
      }
      else
        PlayerFarming.SetStateForAllPlayers();
    }
    DOTween.Kill((object) this);
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
    if (GameManager.IsDungeon(PlayerFarming.Location))
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 0.0f);
    else
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    this.CameraSetConversationMode(false);
    this.ResetCachedCameraTargets();
    LetterBox.Hide(ShowHUD);
    this.UnFreezeAllies();
  }

  public void AddPlayerToCamera()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && PlayerFarming.Instance.gameObject.activeSelf)
    {
      Debug.Log((object) "Cam Player farming assed found to add?");
      this.AddToCamera(PlayerFarming.Instance.CameraBone);
    }
    else
      Debug.Log((object) "Cam Player farming not found to add?");
  }

  public void AddPlayersToCamera()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      this.AddToCamera(player.CameraBone);
  }

  public static GameManager GetInstance() => GameManager.instance;

  public float CachedZoom
  {
    get => this.\u003CCachedZoom\u003Ek__BackingField;
    set => this.\u003CCachedZoom\u003Ek__BackingField = value;
  }

  public void CacheCameraTargets()
  {
    for (int index = this.CamFollowTarget.targets.Count - 1; index >= 0; --index)
    {
      if (this.CamFollowTarget.targets[index] == null || (UnityEngine.Object) this.CamFollowTarget.targets[index].gameObject == (UnityEngine.Object) null)
        this.CamFollowTarget.targets.RemoveAt(index);
    }
    if (this.CamFollowTarget.targets.Count <= 0)
      this.AddPlayersToCamera();
    this.CachedCamTargets = new List<CameraFollowTarget.Target>((IEnumerable<CameraFollowTarget.Target>) this.CamFollowTarget.targets);
    this.CachedZoom = this.CamFollowTarget.targetDistance;
  }

  public void ResetCachedCameraTargets()
  {
    this.CamFollowTarget.targets = new List<CameraFollowTarget.Target>((IEnumerable<CameraFollowTarget.Target>) this.CachedCamTargets);
    if ((double) this.CachedZoom != -999.0)
      this.CamFollowTarget.targetDistance = this.CachedZoom;
    if (this.CamFollowTarget.targets.Count > 0)
      return;
    this.AddPlayersToCamera();
  }

  public void RemoveAllFromCamera() => this.CamFollowTarget.ClearAllTargets();

  public void AddToCamera(GameObject gameObject, float Weight = 1f)
  {
    if ((UnityEngine.Object) this.CamFollowTarget == (UnityEngine.Object) null)
      return;
    Health component = gameObject.GetComponent<Health>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Dormant)
      return;
    this.CamFollowTarget.AddTarget(gameObject, Weight);
  }

  public void RemoveFromCamera(GameObject gameObject)
  {
    this.CamFollowTarget.RemoveTarget(gameObject);
  }

  public bool CameraContains(GameObject gameObject) => this.CamFollowTarget.Contains(gameObject);

  public void CameraZoom(float zoom, float duration)
  {
    if (this.currentZoomRoutine != null)
    {
      this.StopCoroutine(this.currentZoomRoutine);
      this.currentZoomRoutine = (Coroutine) null;
    }
    this.currentZoomRoutine = this.StartCoroutine((IEnumerator) this.CameraZoomRoutine(zoom, duration));
  }

  public IEnumerator CameraZoomRoutine(float zoom, float duration)
  {
    if (!((UnityEngine.Object) this.CamFollowTarget == (UnityEngine.Object) null))
    {
      float startZoom = this.CamFollowTarget.targetDistance;
      float t = 0.0f;
      while ((double) t < (double) duration)
      {
        t += Time.deltaTime;
        this.CamFollowTarget.targetDistance = Mathf.Lerp(startZoom, zoom, t / duration);
        yield return (object) null;
      }
    }
  }

  public void CameraSetZoom(float Zoom)
  {
    this.CamFollowTarget.distance = this.CamFollowTarget.targetDistance = Zoom;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    BiomeConstants.Instance.DepthOfFieldTween(1.5f, Zoom + 1f, 10f, 1f, 145f);
  }

  public void CameraResetTargetZoom()
  {
    this.CamFollowTarget.distance = this.CamFollowTarget.targetDistance = 12f;
    if (!((UnityEngine.Object) BiomeConstants.Instance != (UnityEngine.Object) null))
      return;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 0.0f);
    else
      BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
  }

  public void CameraSetTargetZoom(float Zoom)
  {
    this.CamFollowTarget.targetDistance = Zoom;
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    BiomeConstants.Instance.DepthOfFieldTween(1.5f, Zoom + 1f, 10f, 1f, 145f);
  }

  public void CameraSetOffset(Vector3 Offset) => this.CamFollowTarget.TargetOffset = Offset;

  public void CameraSetConversationMode(bool toggle)
  {
    this.CamFollowTarget.IN_CONVERSATION = toggle;
  }

  public void CameraSnapToPosition(Vector3 position) => this.CamFollowTarget.SnapTo(position);

  public void HitStop(float SleepDuration = 0.1f)
  {
    if ((double) Time.timeScale != 1.0)
      return;
    if (CoopManager.CoopActive)
      SleepDuration /= 2f;
    this.StartCoroutine((IEnumerator) this.HitStopRoutine(SleepDuration));
  }

  public IEnumerator HitStopRoutine(float SleepDuration)
  {
    Time.timeScale = 0.0f;
    yield return (object) new UnityEngine.WaitForSecondsRealtime(SleepDuration);
    Time.timeScale = 1f;
  }

  public static bool IsDungeon(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.None:
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Boss_5:
      case FollowerLocation.Dungeon1_5:
      case FollowerLocation.IntroDungeon:
      case FollowerLocation.Dungeon1_6:
      case FollowerLocation.Boss_Yngya:
      case FollowerLocation.Boss_Wolf:
        return true;
      default:
        return false;
    }
  }

  public void Update()
  {
    this.GetPlayerPosition();
    this._UnscaledTime = Time.unscaledTime;
    this.scaledTimeElapsed += Time.deltaTime;
    Shader.SetGlobalFloat("_GlobalTimeUnscaled", this._UnscaledTime);
    Shader.SetGlobalFloat("_GlobalTimeUnscaled1", this._UnscaledTime);
    if (SettingsManager.Settings.Accessibility.StopTimeInCrusade && GameManager.IsDungeon(PlayerFarming.Location))
      SimulationManager.Pause();
    if (TwitchAuthentication.IsAuthenticated)
      TwitchManager.UpdateEvents();
    if (CheatConsole.IN_DEMO)
    {
      int num;
      if (InputManager.UI.GetPageNavigateLeftHeld() && InputManager.UI.GetPageNavigateRightHeld())
      {
        CheatConsole instance = CheatConsole.Instance;
        num = Mathf.Max(0, Mathf.FloorToInt((float) (10.0 - (double) this.HoldToResetDemo / 5.0 * 10.0)));
        string Message = "Restarting... " + num.ToString();
        Color red = Color.red;
        instance.DisplayText(Message, red);
        this.HoldToResetDemo += Time.unscaledDeltaTime;
      }
      else
      {
        if ((double) this.HoldToResetDemo != 0.0)
          CheatConsole.Instance.DisplayText("", Color.red);
        this.HoldToResetDemo = 0.0f;
      }
      if ((double) this.HoldToResetDemo > 5.0)
      {
        this.DemoQuitToMenu();
        return;
      }
      if ((double) this.HoldToResetDemo == 0.0 && (double) CheatConsole.TimeSinceLastKeyPress > 90.0)
      {
        this.DisplayingInactiveWarning = true;
        CheatConsole instance = CheatConsole.Instance;
        num = Mathf.Max(0, Mathf.FloorToInt(120f - CheatConsole.TimeSinceLastKeyPress));
        string Message = "Game Inactive. Restarting... " + num.ToString();
        Color red = Color.red;
        instance.DisplayText(Message, red);
      }
      else if (this.DisplayingInactiveWarning)
      {
        this.DisplayingInactiveWarning = false;
        CheatConsole.Instance.DisplayText("", Color.red);
      }
      if ((double) CheatConsole.TimeSinceLastKeyPress >= 120.0)
      {
        CheatConsole.ForceAutoAttractMode = true;
        this.DemoQuitToMenu();
        return;
      }
      if (!MMTransition.IsPlaying && (double) (CheatConsole.DemoBeginTime += Time.unscaledDeltaTime) > 1200.0)
      {
        Debug.Log((object) "OUT OF TME!!");
        this.QuitToThanksForPlaying();
        return;
      }
    }
    if (InputManager.General.GetAnyButton() || (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) > 0.10000000149011612 || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) > 0.10000000149011612)
    {
      this.autoPauseTimestamp = Time.unscaledTime + 600f;
    }
    else
    {
      if ((double) Time.unscaledTime <= (double) this.autoPauseTimestamp || !((UnityEngine.Object) MonoSingleton<UIManager>.Instance != (UnityEngine.Object) null) || MonoSingleton<UIManager>.Instance.MenusBlocked)
        return;
      if (PlayerFarming.players.Count == 0)
        MonoSingleton<UIManager>.Instance.ShowPauseMenu((PlayerFarming) null);
      else
        MonoSingleton<UIManager>.Instance.ShowPauseMenu(PlayerFarming.players[0]);
      this.autoPauseTimestamp = Time.unscaledTime + 600f;
    }
  }

  public static void RecalculatePaths(bool immediate = false, bool resumePlay = true, System.Action callback = null)
  {
    if (!((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null))
      return;
    if (GameManager.GetInstance().cGeneratePathfinding != null)
      GameManager.GetInstance().StopCoroutine(GameManager.GetInstance().cGeneratePathfinding);
    GameManager.GetInstance().cGeneratePathfinding = GameManager.GetInstance().StartCoroutine((IEnumerator) GameManager.GetInstance().GeneratePathfinding(immediate ? 0.0f : 1f, resumePlay, callback));
  }

  public IEnumerator GeneratePathfinding(float Delay, bool resumePlay = true, System.Action callback = null)
  {
    while ((double) (Delay -= Time.deltaTime) > 0.0)
      yield return (object) null;
    List<GridGraph> gridGraphList = new List<GridGraph>();
    foreach (NavGraph graph in AstarPath.active.data.graphs)
    {
      if (graph != null && graph is GridGraph gridGraph)
        gridGraphList.Add(gridGraph);
    }
    AstarPath.active.Scan((NavGraph[]) gridGraphList.ToArray());
    if (resumePlay)
      MMTransition.ResumePlay();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void ShowGameOverScreen()
  {
    this.GameOverScreen.SetActive(true);
    GameManager.SetTimeScale(0.1f);
  }

  public static void SetTimeScale(float NewTimeScale)
  {
    GameManager.TimeScale = NewTimeScale;
    Time.timeScale = GameManager.TimeScale;
  }

  public void QuitToMenu()
  {
    AchievementsWrapper.LoadAchievementData();
    SceneManager.LoadScene("Main Menu");
  }

  public void DemoQuitToMenu()
  {
    if ((double) CheatConsole.DemoBeginTime == 0.0)
      return;
    CheatConsole.DemoBeginTime = 0.0f;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIMenuBase.ActiveMenus.Clear();
    GameManager.SetTimeScale(1f);
    UIManager.PlayAudio("event:/sermon/select_upgrade");
    UIManager.PlayAudio("event:/sermon/Sermon Speech Bubble2");
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "Main Menu", 3f, "", (System.Action) (() => { }));
  }

  public void QuitToThanksForPlaying()
  {
    if ((double) CheatConsole.DemoBeginTime == 0.0)
      return;
    CheatConsole.DemoBeginTime = 0.0f;
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIMenuBase.ActiveMenus.Clear();
    GameManager.SetTimeScale(1f);
    UIManager.PlayAudio("event:/sermon/select_upgrade");
    UIManager.PlayAudio("event:/sermon/select_upgrade");
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeSceneAutoResume, MMTransition.Effect.BlackFade, "DemoOver", 3f, "", (System.Action) (() => { }));
  }

  public static int DungeonEndlessLevel
  {
    get => GameManager._DungeonEndlessLevel;
    set
    {
      Debug.Log((object) $"CHANGING ENDLESS LEVEL {GameManager._DungeonEndlessLevel.ToString()}   -   {value.ToString()}");
      GameManager._DungeonEndlessLevel = value;
    }
  }

  public static bool Layer2
  {
    get
    {
      if (!DataManager.Instance.DeathCatBeaten || DungeonSandboxManager.Active)
        return false;
      return (UnityEngine.Object) BiomeGenerator.Instance == (UnityEngine.Object) null || !BiomeGenerator.DLC_DUNGEONS.Contains(BiomeGenerator.Instance.DungeonLocation);
    }
  }

  public static bool SandboxDungeonEnabled
  {
    get => (UnityEngine.Object) DungeonSandboxManager.Instance != (UnityEngine.Object) null;
  }

  public static void NewRun(string PlaceName, bool InDungeon, FollowerLocation location = FollowerLocation.None)
  {
    Debug.Log((object) ">>>>>>>>>>>>>>>   NewRun ".Colour(Color.yellow));
    GameManager.InitialDungeonEnter = true;
    GameManager.CurrentDungeonFloor = 1;
    if (!InDungeon)
      GameManager.DungeonEndlessLevel = 1;
    BiomeGenerator.UsedEncounters = new List<string>();
    TimeManager.SetOverrideScheduledActivity(ScheduledActivity.None);
    MiniBossManager.CurrentIndex = 0;
    GameManager.DungeonNameTerm = PlaceName;
    DataManager.SetNewRun(location);
    GameManager.TimeInDungeon = DataManager.Instance.TimeInGame;
    if (!DataManager.Instance.UnlockedCorruptedRelicsAndTarots && DataManager.Instance.DeathCatBeaten)
      ResurrectOnHud.ResurrectionType = ResurrectionType.CorruptedMonolith;
    else if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Resurrection))
      ResurrectOnHud.ResurrectionType = ResurrectionType.Pyre;
    Health.team2.Clear();
    try
    {
      GameManager.instance.OnStopTimeInCrusadeSettingChanged(SettingsManager.Settings.Accessibility.StopTimeInCrusade);
    }
    catch
    {
    }
  }

  public void OnStopTimeInCrusadeSettingChanged(bool state)
  {
    if (!GameManager.IsDungeon(PlayerFarming.Location))
      return;
    if (state)
      SimulationManager.Pause();
    else
      SimulationManager.UnPause();
  }

  public static void NextDungeonLayer(int NewLayer)
  {
    DataManager.RandomSeed = new System.Random(DataManager.RandomSeed.Next(int.MinValue, int.MaxValue));
    GameManager.PreviousDungeonLayer = GameManager.CurrentDungeonLayer;
    GameManager.CurrentDungeonLayer = NewLayer;
  }

  public static void ToShip(string Scene = "Base Biome 1", float Duration = 2f, MMTransition.Effect Effect = MMTransition.Effect.BlackFade)
  {
    SimulationManager.Pause();
    DataManager.ResetRunData();
    DataManager.Instance.CurrentDLCDungeonID = -1;
    DataManager.Instance.CurrentDLCNodeType = DungeonWorldMapIcon.NodeType.Base;
    DataManager.Instance.HasYngyaConvo = false;
    SaveAndLoad.Save();
    PlayerFarming.CleanAllCharacters();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, Effect, Scene, Duration, "", new System.Action(GameManager.ToShipCallback));
  }

  public static void ToShipCallback() => GameManager.SetTimeScale(1f);

  public static void healPlayer(PlayerFarming playerFarming)
  {
    playerFarming.health.HP += playerFarming.health.PLAYER_TOTAL_HEALTH;
  }

  public void EndCurrentDay(bool PlayerDied)
  {
    if (!((UnityEngine.Object) this.EndOfDayScreen != (UnityEngine.Object) null))
      return;
    GameObject withTag = GameObject.FindWithTag("Player");
    if (!((UnityEngine.Object) withTag != (UnityEngine.Object) null))
      return;
    SaveAndLoad.Save();
    UnityEngine.Object.Destroy((UnityEngine.Object) withTag);
    SceneManager.LoadScene("Ship");
  }

  public static bool HasUnlockAvailable()
  {
    return (UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null || GameManager.GetInstance().UpgradeTreeConfiguration.HasUnlockAvailable() || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.WinterSystem) && GameManager.GetInstance().DLCUpgradeTreeConfiguration.HasUnlockAvailable();
  }

  public static bool HasDLCUnlockAvailable()
  {
    return (UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null || GameManager.GetInstance().DLCUpgradeTreeConfiguration.HasUnlockAvailable();
  }

  public static bool IsDLCUnlock(UpgradeSystem.Type type)
  {
    foreach (UpgradeSystem.Type allUpgrade in GameManager.GetInstance().DLCUpgradeTreeConfiguration.AllUpgrades)
    {
      if (allUpgrade == type)
        return true;
    }
    return false;
  }

  public void CreateName() => Debug.Log((object) Villager_Info.GenerateName());

  public float UnscaledTimeSince(float timestamp) => Time.unscaledTime - timestamp;

  public float TimeSince(float timestamp) => this.scaledTimeElapsed - timestamp;

  public void WaitForLetterbox(System.Action callback)
  {
    this.StartCoroutine((IEnumerator) this.WaitForLetterboxIE(callback));
  }

  public IEnumerator WaitForLetterboxIE(System.Action callback)
  {
    while (LetterBox.IsPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void WaitForSeconds(float seconds, System.Action callback)
  {
    this.StartCoroutine((IEnumerator) this.WaitForSecondsIE(seconds, false, callback));
  }

  public void WaitForSecondsRealtime(float seconds, System.Action callback)
  {
    this.StartCoroutine((IEnumerator) this.WaitForSecondsIE(seconds, true, callback));
  }

  public IEnumerator WaitForSecondsIE(float seconds, bool realtime, System.Action callback)
  {
    if ((double) seconds == 0.0)
      yield return (object) new WaitForEndOfFrame();
    else if (realtime)
      yield return (object) new UnityEngine.WaitForSecondsRealtime(seconds);
    else
      yield return (object) new UnityEngine.WaitForSeconds(seconds);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public static void ForceFontReload()
  {
    LocalizationManager.ForceLoadSynchronous = true;
    LocalizationManager.EnableChangingCultureInfo(true);
    LocalizationManager.SetupFonts();
    LocalizationManager.ForceLoadSynchronous = false;
  }

  [CompilerGenerated]
  public static IEnumerator \u003CTwitchAuthentication_OnAuthenticated\u003Eg__Wait\u007C35_0()
  {
    while ((UnityEngine.Object) MonoSingleton<UIManager>.Instance == (UnityEngine.Object) null || MonoSingleton<UIManager>.Instance.MenusBlocked)
      yield return (object) null;
    bool loop = true;
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Twitch))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Twitch);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => loop = false);
    }
    else
      loop = false;
    while (loop)
      yield return (object) null;
    loop = true;
    DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Twitch_2);
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Twitch_3))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Twitch_3);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => loop = false);
    }
    else
      loop = false;
    while (loop)
      yield return (object) null;
    loop = true;
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Twitch_4))
    {
      UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Twitch_4);
      overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => loop = false);
    }
    else
      loop = false;
    while (loop)
      yield return (object) null;
  }

  [CompilerGenerated]
  public void \u003CCheckDLCStatus\u003Eb__53_0()
  {
    Debug.Log((object) "## Checking DLC");
    if (!GameManager.AuthenticateHereticDLC() && DataManager.Instance.DLC_Heretic_Pack)
    {
      Debug.Log((object) "## Deactivate HERETIC DLC");
      DataManager.DeactivateHereticDLC();
    }
    if (!GameManager.AuthenticateCultistDLC() && DataManager.Instance.DLC_Cultist_Pack)
    {
      Debug.Log((object) "## Deactivate CULTIST DLC");
      DataManager.DeactivateCultistDLC();
    }
    if (!GameManager.AuthenticateSinfulDLC() && DataManager.Instance.DLC_Sinful_Pack)
    {
      Debug.Log((object) "## Deactivate SINFUL DLC");
      DataManager.DeactivateSinfulDLC();
    }
    if (!GameManager.AuthenticatePilgrimDLC() && DataManager.Instance.DLC_Pilgrim_Pack)
    {
      Debug.Log((object) "## Deactivate SINFUL DLC");
      DataManager.DeactivatePilgrimDLC();
    }
    if (!GameManager.AuthenticatePrePurchaseDLC() && DataManager.Instance.DLC_Pre_Purchase)
    {
      Debug.Log((object) "## Deactivate PRE PURCHASE");
      DataManager.DeactivatePrePurchaseDLC();
    }
    if (!GameManager.AuthenticateMajorDLC() && DataManager.Instance.MAJOR_DLC)
    {
      Debug.Log((object) "## Deactivate PRE PURCHASE");
      DataManager.DeactivateWoolhavenDLC();
    }
    this.StartCoroutine((IEnumerator) GameManager.WaitForDLCCheck(1f, (System.Action) (() =>
    {
      float delay = 2f;
      if (PlayerFarming.Location == FollowerLocation.Base)
      {
        Debug.Log((object) "Authenticating DLC");
        if (GameManager.AuthenticateSinfulDLC())
        {
          Debug.Log((object) "## Activate SINGFUL DLC");
          if (DataManager.ActivateSinfulDLC())
            this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
            {
              Debug.Log((object) "## SINGFUL DLC Notification");
              NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/SinfulEdition");
            })));
        }
        if (GameManager.AuthenticateHereticDLC())
        {
          delay += 0.5f;
          Debug.Log((object) "## Activate HERETIC DLC");
          if (DataManager.ActivateHereticDLC())
            this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
            {
              Debug.Log((object) "## HERETIC DLC Notification");
              NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/HereticEdition");
            })));
        }
        if (GameManager.AuthenticateCultistDLC())
        {
          delay += 0.5f;
          Debug.Log((object) "## Activate CULTIST DLC");
          if (DataManager.ActivateCultistDLC())
            this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
            {
              Debug.Log((object) "## CULTIST DLC Notification");
              NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CultistEdition");
            })));
        }
        if (GameManager.AuthenticatePilgrimDLC())
        {
          delay += 0.5f;
          Debug.Log((object) "## Activate PILGRIM DLC");
          if (DataManager.ActivatePilgrimDLC())
            this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
            {
              Debug.Log((object) "## PILGRIM DLC Notification");
              NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/PilgrimPack");
            })));
        }
        if (GameManager.AuthenticateMajorDLC())
        {
          delay += 0.5f;
          Debug.Log((object) "## Activate MAJOR DLC");
          if (DataManager.ActivateMajorDLC())
            this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
            {
              Debug.Log((object) "## MAJOR DLC Notification");
              NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/Name");
            })));
        }
        else
          DataManager.Instance.MAJOR_DLC = false;
        if (GameManager.AuthenticatePrePurchaseDLC())
        {
          float num = delay + 0.5f;
          Debug.Log((object) "## Activate PRE PURCHASE");
          if (DataManager.ActivatePrePurchaseDLC())
            this.StartCoroutine((IEnumerator) GameManager.WaitForTime(num + 1.5f, (System.Action) (() =>
            {
              Debug.Log((object) "## PRE PURCHASE DLC Notification");
              NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CthuluPack");
            })));
        }
        if (GameManager.AuthenticatePlushBonusDLC() && DataManager.ActivatePlushBonusDLC())
          NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "Structures/DECORATION_PLUSH");
        if (GameManager.AuthenticatePAXDLC() && DataManager.ActivatePAXDLC())
          NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "PAX DLC");
        if (GameManager.AuthenticateTwitchDrop1() && DataManager.ActivateTwitchDrop1())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop2() && DataManager.ActivateTwitchDrop2())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop3() && DataManager.ActivateTwitchDrop3())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop4() && DataManager.ActivateTwitchDrop4())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop5() && DataManager.ActivateTwitchDrop5())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop6() && DataManager.ActivateTwitchDrop6())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop7() && DataManager.ActivateTwitchDrop7())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
        if (GameManager.AuthenticateTwitchDrop8() && DataManager.ActivateTwitchDrop8())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop9() && DataManager.ActivateTwitchDrop9())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop10() && DataManager.ActivateTwitchDrop10())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop11() && DataManager.ActivateTwitchDrop11())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop12() && DataManager.ActivateTwitchDrop12())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop13() && DataManager.ActivateTwitchDrop13())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop14() && DataManager.ActivateTwitchDrop14())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop15() && DataManager.ActivateTwitchDrop15())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop16() && DataManager.ActivateTwitchDrop16())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop17() && DataManager.ActivateTwitchDrop17())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop18() && DataManager.ActivateTwitchDrop18())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop19() && DataManager.ActivateTwitchDrop19())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
        if (GameManager.AuthenticateTwitchDrop20() && DataManager.ActivateTwitchDrop20())
          NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      }
      System.Action dlCsAuthenticated = this.OnDLCsAuthenticated;
      if (dlCsAuthenticated == null)
        return;
      dlCsAuthenticated();
    })));
  }

  [CompilerGenerated]
  public void \u003CCheckDLCStatus\u003Eb__53_1()
  {
    float delay = 2f;
    if (PlayerFarming.Location == FollowerLocation.Base)
    {
      Debug.Log((object) "Authenticating DLC");
      if (GameManager.AuthenticateSinfulDLC())
      {
        Debug.Log((object) "## Activate SINGFUL DLC");
        if (DataManager.ActivateSinfulDLC())
          this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
          {
            Debug.Log((object) "## SINGFUL DLC Notification");
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/SinfulEdition");
          })));
      }
      if (GameManager.AuthenticateHereticDLC())
      {
        delay += 0.5f;
        Debug.Log((object) "## Activate HERETIC DLC");
        if (DataManager.ActivateHereticDLC())
          this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
          {
            Debug.Log((object) "## HERETIC DLC Notification");
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/HereticEdition");
          })));
      }
      if (GameManager.AuthenticateCultistDLC())
      {
        delay += 0.5f;
        Debug.Log((object) "## Activate CULTIST DLC");
        if (DataManager.ActivateCultistDLC())
          this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
          {
            Debug.Log((object) "## CULTIST DLC Notification");
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CultistEdition");
          })));
      }
      if (GameManager.AuthenticatePilgrimDLC())
      {
        delay += 0.5f;
        Debug.Log((object) "## Activate PILGRIM DLC");
        if (DataManager.ActivatePilgrimDLC())
          this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
          {
            Debug.Log((object) "## PILGRIM DLC Notification");
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/PilgrimPack");
          })));
      }
      if (GameManager.AuthenticateMajorDLC())
      {
        delay += 0.5f;
        Debug.Log((object) "## Activate MAJOR DLC");
        if (DataManager.ActivateMajorDLC())
          this.StartCoroutine((IEnumerator) GameManager.WaitForTime(delay, (System.Action) (() =>
          {
            Debug.Log((object) "## MAJOR DLC Notification");
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/Name");
          })));
      }
      else
        DataManager.Instance.MAJOR_DLC = false;
      if (GameManager.AuthenticatePrePurchaseDLC())
      {
        float num = delay + 0.5f;
        Debug.Log((object) "## Activate PRE PURCHASE");
        if (DataManager.ActivatePrePurchaseDLC())
          this.StartCoroutine((IEnumerator) GameManager.WaitForTime(num + 1.5f, (System.Action) (() =>
          {
            Debug.Log((object) "## PRE PURCHASE DLC Notification");
            NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CthuluPack");
          })));
      }
      if (GameManager.AuthenticatePlushBonusDLC() && DataManager.ActivatePlushBonusDLC())
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "Structures/DECORATION_PLUSH");
      if (GameManager.AuthenticatePAXDLC() && DataManager.ActivatePAXDLC())
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "PAX DLC");
      if (GameManager.AuthenticateTwitchDrop1() && DataManager.ActivateTwitchDrop1())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop2() && DataManager.ActivateTwitchDrop2())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop3() && DataManager.ActivateTwitchDrop3())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop4() && DataManager.ActivateTwitchDrop4())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop5() && DataManager.ActivateTwitchDrop5())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop6() && DataManager.ActivateTwitchDrop6())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop7() && DataManager.ActivateTwitchDrop7())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop8() && DataManager.ActivateTwitchDrop8())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop9() && DataManager.ActivateTwitchDrop9())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop10() && DataManager.ActivateTwitchDrop10())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop11() && DataManager.ActivateTwitchDrop11())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop12() && DataManager.ActivateTwitchDrop12())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop13() && DataManager.ActivateTwitchDrop13())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop14() && DataManager.ActivateTwitchDrop14())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop15() && DataManager.ActivateTwitchDrop15())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop16() && DataManager.ActivateTwitchDrop16())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop17() && DataManager.ActivateTwitchDrop17())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop18() && DataManager.ActivateTwitchDrop18())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop19() && DataManager.ActivateTwitchDrop19())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
      if (GameManager.AuthenticateTwitchDrop20() && DataManager.ActivateTwitchDrop20())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDropDecoration");
    }
    System.Action dlCsAuthenticated = this.OnDLCsAuthenticated;
    if (dlCsAuthenticated == null)
      return;
    dlCsAuthenticated();
  }
}
