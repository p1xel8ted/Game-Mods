// Decompiled with JetBrains decompiler
// Type: GameManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using Lamb.UI;
using Lamb.UI.Assets;
using Lamb.UI.PauseMenu;
using Microsoft.CSharp.RuntimeBinder;
using MMBiomeGeneration;
using MMTools;
using Pathfinding;
using src.UINavigator;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

#nullable disable
public class GameManager : MonoBehaviour
{
  private static GameManager instance;
  private static bool reactivateGraph = false;
  private static float reactivateGraphTimer;
  public GameObject GameOverScreen;
  public static float CurrentZ;
  public static bool IsQuitting;
  public UpgradeTreeConfiguration UpgradeTreeConfiguration;
  public UpgradeTreeConfiguration UpgradePlayerConfiguration;
  public RenderTexture LightingRenderTexture;
  private Color StartItemsInWoodsColor = new Color(0.0741f, 0.0f, 0.129f, 1f);
  public float _UnscaledTime;
  private float scaledTimeElapsed;
  private AudioSource audioSource;
  public static bool GoG_Initialised = false;
  private float autoPauseTimestamp;
  private const float idleTimeUntilAutoPause = 600f;
  public static bool overridePlayerPosition = false;
  private static Coroutine AdjustShadersCoroutine;
  public static float _timeinDungeon = 0.0f;
  private List<int> GameSpeed = new List<int>() { 1, 2, 3 };
  private int CurrentGameSpeed;
  public CameraFollowTarget _CamFollowTarget;
  public List<CameraFollowTarget.Target> CachedCamTargets = new List<CameraFollowTarget.Target>();
  public static bool InMenu = false;
  private UIPauseMenuController _pauseMenuInstance;
  private bool DisplayingInactiveWarning;
  private float HoldToResetDemo;
  public Coroutine cGeneratePathfinding;
  private static float TimeScale = 1f;
  public static bool DungeonUseAllLayers = false;
  public static int CurrentDungeonLayer = 1;
  public static int PreviousDungeonLayer = 0;
  public static int CurrentDungeonFloor = 1;
  private static int _DungeonEndlessLevel = 1;
  public static string DungeonNameTerm;
  public static bool InitialDungeonEnter = true;
  public static bool CULTISTPAC_DLC = false;
  public static bool CTHULHUWOR_DLC = false;
  public EndOfDay EndOfDayScreen;
  private static readonly int GlobalDitherIntensity = Shader.PropertyToID("_GlobalDitherIntensity");
  private static readonly int GlobalResourceHighlight = Shader.PropertyToID("_GlobalResourceHighlight");
  private static readonly int TimeOfDayColor = Shader.PropertyToID("_TimeOfDayColor");
  private static readonly int CloudAlpha = Shader.PropertyToID("_CloudAlpha");
  private static readonly int CloudDensity = Shader.PropertyToID("_CloudDensity");
  private static readonly int VerticalFogGradientSpread = Shader.PropertyToID("_VerticalFog_GradientSpread");
  private static readonly int VerticalFogZOffset = Shader.PropertyToID("_VerticalFog_ZOffset");
  private static readonly int LightingRenderTexture1 = Shader.PropertyToID("_Lighting_RenderTexture");
  private static readonly int WindDensity = Shader.PropertyToID("_WindDensity");
  private static readonly int WindSpeed = Shader.PropertyToID("_WindSpeed");
  private static readonly int WindDiection = Shader.PropertyToID("_WindDiection");
  private static readonly int ItemInWoodsColor = Shader.PropertyToID("_ItemInWoodsColor");
  private static readonly int GlobalTimeUnscaled = Shader.PropertyToID("_GlobalTimeUnscaled");

  public static bool RoomActive
  {
    get
    {
      return !((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null) || BiomeGenerator.Instance.CurrentRoom.Active;
    }
  }

  public float CurrentTime => this.scaledTimeElapsed;

  public static float DeltaTime => Time.deltaTime * 60f;

  public static float UnscaledDeltaTime => Time.unscaledDeltaTime * 60f;

  public static float FixedDeltaTime => Time.fixedDeltaTime * 60f;

  public static float FixedUnscaledDeltaTime => Time.fixedUnscaledDeltaTime * 60f;

  private void Awake()
  {
    GarbageCollector.incrementalTimeSliceNanoseconds = 1000000UL;
    this.autoPauseTimestamp = Time.unscaledTime + 600f;
    Singleton<SettingsManager>.Instance.LoadAndApply();
    DataManager.Instance.CurrentWeapon = EquipmentType.None;
    DataManager.Instance.CurrentWeaponLevel = 0;
    DataManager.Instance.CurrentCurse = EquipmentType.None;
    DataManager.Instance.CurrentCurseLevel = 0;
    DataManager.Instance.PLAYER_STARTING_HEALTH_CACHED = DataManager.Instance.PLAYER_STARTING_HEALTH;
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
    if (DataManager.Instance.RecipesDiscovered.Count == 0)
      DataManager.Instance.RecipesDiscovered.Add(InventoryItem.ITEM_TYPE.MEAL_BERRIES);
    if (DataManager.Instance.DiscoveredLocations.Count == 0)
    {
      DataManager.Instance.DiscoveredLocations.Add(FollowerLocation.Base);
      DataManager.Instance.VisitedLocations.Add(FollowerLocation.Base);
    }
    if (DataManager.Instance.UnlockedUpgrades.Count == 0)
    {
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_HeartsOfTheFaithful1);
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_UnlockCurse);
      DataManager.Instance.UnlockedUpgrades.Add(UpgradeSystem.Type.Ritual_UnlockWeapon);
    }
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
    if (DataManager.Instance.WeaponSelectionPositions.Count == 0)
      DataManager.Instance.WeaponSelectionPositions = new List<TarotCards.Card>()
      {
        TarotCards.Card.Dagger,
        TarotCards.Card.Axe,
        TarotCards.Card.Gauntlet,
        TarotCards.Card.Hammer
      };
    TwitchAuthentication.OnAuthenticated += new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
  }

  private void TwitchAuthentication_OnAuthenticated()
  {
    this.StartCoroutine((IEnumerator) Wait());

    static IEnumerator Wait()
    {
      while ((UnityEngine.Object) MonoSingleton<UIManager>.Instance == (UnityEngine.Object) null || MonoSingleton<UIManager>.Instance.MenusBlocked)
        yield return (object) null;
      if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Twitch))
        MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Twitch);
    }
  }

  private void Start()
  {
    this.SetGlobalShaders();
    SteamAPI.Init();
    if (!DataManager.CheckIfThereAreSkinsAvailableAll())
    {
      Debug.Log((object) "Follower Skin Achievement Unlocked");
      AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("ALL_SKINS_UNLOCKED"));
    }
    this.CheckDLCStatus();
  }

  private void OnEnable()
  {
    GameManager.instance = this;
    this.CachedCamTargets = new List<CameraFollowTarget.Target>();
    AudioManager.Instance.SetGameManager(this);
    MMConversation.OnConversationNew += new MMConversation.ConversationNew(this.OnConversationNew);
    MMConversation.OnConversationNext += new MMConversation.ConversationNext(this.OnConversationNext);
    MMConversation.OnConversationEnd += new MMConversation.ConversationEnd(this.OnConversationEnd);
    Application.quitting += (System.Action) new System.Action(this.OnQuit);
    TimeManager.OnNewDayStarted += new System.Action(this.Save);
    TwitchAuthentication.TryAuthenticate((TwitchRequest.ConnectionResponse) null);
    if (CheatConsole.IN_DEMO)
      CheatConsole.ForceResetTimeSinceLastKeyPress();
    this.CheckAchievements();
  }

  private void CheckAchievements()
  {
    if ((UnityEngine.Object) Achievements.Instance == (UnityEngine.Object) null || DataManager.Instance.PlayerFoundTrinkets.Count < DataManager.AllTrinkets.Count)
      return;
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("ALL_TAROTS_UNLOCKED"));
  }

  public static bool AuthenticateCultistDLC()
  {
    Debug.Log((object) "## AuthenticateCultistDLC");
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2015880U));
  }

  public static bool AuthenticatePrePurchaseDLC()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(2013550U));
  }

  public static bool AuthenticatePlushBonusDLC()
  {
    return SteamAPI.Init() && SteamApps.BIsSubscribedApp(new AppId_t(1944680U));
  }

  private void OnDestroy()
  {
    TwitchAuthentication.OnAuthenticated -= new TwitchAuthentication.AuthenticationResponse(this.TwitchAuthentication_OnAuthenticated);
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) GameManager.instance == (UnityEngine.Object) this)
      GameManager.instance = (GameManager) null;
    MMConversation.OnConversationNew -= new MMConversation.ConversationNew(this.OnConversationNew);
    MMConversation.OnConversationNext -= new MMConversation.ConversationNext(this.OnConversationNext);
    MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(this.OnConversationEnd);
    Application.quitting -= (System.Action) new System.Action(this.OnQuit);
    TimeManager.OnNewDayStarted -= new System.Action(this.Save);
  }

  private void Save()
  {
    if (PlayerFarming.Location != FollowerLocation.Base)
      return;
    SaveAndLoad.Save();
  }

  private void OnQuit() => GameManager.IsQuitting = true;

  private void CheckDLCStatus()
  {
    this.StartCoroutine((IEnumerator) this.WaitForDLCCheck(1f, (System.Action) (() =>
    {
      if (!GameManager.AuthenticateCultistDLC() && DataManager.Instance.DLC_Cultist_Pack)
      {
        Debug.Log((object) "## DeactivateCultistDLC");
        DataManager.DeactivateCultistDLC();
      }
      if (PlayerFarming.Location != FollowerLocation.Base)
        return;
      if (GameManager.AuthenticateCultistDLC())
      {
        Debug.Log((object) "## ActivateCultistDLC");
        if (DataManager.ActivateCultistDLC())
          NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CultistEdition");
      }
      if (GameManager.AuthenticatePrePurchaseDLC() && DataManager.ActivatePrePurchaseDLC())
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "UI/DLC/CthuluPack");
      if (GameManager.AuthenticatePlushBonusDLC() && DataManager.ActivatePlushBonusDLC())
        NotificationCentre.Instance.PlayGenericNotificationLocalizedParams("UI/DLC/ActivatedPack", "Structures/DECORATION_PLUSH");
      if (GameManager.AuthenticateTwitchDrop1() && DataManager.ActivateTwitchDrop1())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop2() && DataManager.ActivateTwitchDrop2())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop3() && DataManager.ActivateTwitchDrop3())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (GameManager.AuthenticateTwitchDrop4() && DataManager.ActivateTwitchDrop4())
        NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
      if (!GameManager.AuthenticateTwitchDrop5() || !DataManager.ActivateTwitchDrop5())
        return;
      NotificationCentre.Instance.PlayTwitchNotification("Notifications/Twitch/ReceivedDrop");
    })));
  }

  private IEnumerator WaitForDLCCheck(float delay, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(delay);
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
    Shader.SetGlobalInt(GameManager.GlobalResourceHighlight, 1);
  }

  private void GetPlayerPosition()
  {
    if (GameManager.overridePlayerPosition || !((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    Shader.SetGlobalVector("_PlayerPosition", (Vector4) PlayerFarming.Instance.gameObject.transform.position);
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

  public static void SetDither(float value)
  {
    Shader.SetGlobalFloat(GameManager.GlobalDitherIntensity, value);
  }

  public void SetDitherTween(float value, float duration = 1f)
  {
    DOTween.Kill((object) this);
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + value, duration).SetEase<Tweener>(Ease.OutQuart);
  }

  private IEnumerator adjustGlobalShaders(
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
          if (MyList[index].types == BiomeVolume.ShaderTypes.Float)
          {
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__50.\u003C\u003Ep__1 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__50.\u003C\u003Ep__1 = CallSite<Func<CallSite, object, float>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (float), typeof (GameManager)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, float> target = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__1.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, float>> p1 = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__1;
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__50.\u003C\u003Ep__0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__50.\u003C\u003Ep__0 = CallSite<Func<CallSite, System.Type, object, float, float, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Lerp", (IEnumerable<System.Type>) null, typeof (GameManager), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__0.Target((CallSite) GameManager.\u003C\u003Eo__50.\u003C\u003Ep__0, typeof (Mathf), startValues[index], MyList[index].valueToGoTo, Mathf.SmoothStep(Start, Target, t));
            float num = target((CallSite) p1, obj);
            Shader.SetGlobalFloat(MyList[index].shaderName, num);
          }
          else if (MyList[index].types == BiomeVolume.ShaderTypes.Color)
          {
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__50.\u003C\u003Ep__3 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__50.\u003C\u003Ep__3 = CallSite<Func<CallSite, object, Color>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Color), typeof (GameManager)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Color> target = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__3.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Color>> p3 = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__3;
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__50.\u003C\u003Ep__2 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__50.\u003C\u003Ep__2 = CallSite<Func<CallSite, System.Type, object, Color, float, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Lerp", (IEnumerable<System.Type>) null, typeof (GameManager), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__2.Target((CallSite) GameManager.\u003C\u003Eo__50.\u003C\u003Ep__2, typeof (Color), startValues[index], MyList[index].colorToGoTo, Mathf.SmoothStep(Start, Target, t));
            Color color = target((CallSite) p3, obj);
            Shader.SetGlobalColor(MyList[index].shaderName, color);
          }
          else if (MyList[index].types != BiomeVolume.ShaderTypes.Texture && MyList[index].types == BiomeVolume.ShaderTypes.Vector2)
          {
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__50.\u003C\u003Ep__5 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__50.\u003C\u003Ep__5 = CallSite<Func<CallSite, object, Vector2>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (Vector2), typeof (GameManager)));
            }
            // ISSUE: reference to a compiler-generated field
            Func<CallSite, object, Vector2> target = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__5.Target;
            // ISSUE: reference to a compiler-generated field
            CallSite<Func<CallSite, object, Vector2>> p5 = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__5;
            // ISSUE: reference to a compiler-generated field
            if (GameManager.\u003C\u003Eo__50.\u003C\u003Ep__4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              GameManager.\u003C\u003Eo__50.\u003C\u003Ep__4 = CallSite<Func<CallSite, System.Type, object, Vector2, float, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "Lerp", (IEnumerable<System.Type>) null, typeof (GameManager), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
              {
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
              }));
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            object obj = GameManager.\u003C\u003Eo__50.\u003C\u003Ep__4.Target((CallSite) GameManager.\u003C\u003Eo__50.\u003C\u003Ep__4, typeof (Vector2), startValues[index], MyList[index].Vector2ToGoTo, Mathf.SmoothStep(Start, Target, t));
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
        if (MyList[index].types == BiomeVolume.ShaderTypes.Float)
          Shader.SetGlobalFloat(MyList[index].shaderName, MyList[index].valueToGoTo);
        else if (MyList[index].types == BiomeVolume.ShaderTypes.Color)
          Shader.SetGlobalColor(MyList[index].shaderName, MyList[index].colorToGoTo);
        else if (MyList[index].types != BiomeVolume.ShaderTypes.Texture && MyList[index].types == BiomeVolume.ShaderTypes.Float)
          Shader.SetGlobalVector(MyList[index].shaderName, (Vector4) MyList[index].Vector2ToGoTo);
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

  public static float TimeInDungeon
  {
    get => GameManager._timeinDungeon;
    set => GameManager._timeinDungeon = value;
  }

  public void NextGameSpeed()
  {
    this.CurrentGameSpeed = ++this.CurrentGameSpeed % this.GameSpeed.Count;
    GameManager.SetTimeScale((float) this.GameSpeed[this.CurrentGameSpeed]);
    Debug.Log((object) $"{(object) Time.timeScale}   {(object) this.CurrentGameSpeed}  {(object) this.GameSpeed[this.CurrentGameSpeed]}");
  }

  public CameraFollowTarget CamFollowTarget
  {
    get
    {
      if ((UnityEngine.Object) this._CamFollowTarget == (UnityEngine.Object) null || !this._CamFollowTarget.gameObject.activeSelf)
        this._CamFollowTarget = CameraFollowTarget.Instance;
      return this._CamFollowTarget;
    }
  }

  public void OnConversationNew(bool SetPlayerInactive = true, bool SnapLetterBox = false)
  {
    this.ResetCachedCameraTargets();
    this.CameraSetConversationMode(true);
    this.CacheCameraTargets();
    this.RemoveAllFromCamera();
    if (SetPlayerInactive && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if (PlayerFarming.Instance.GoToAndStopping)
        PlayerFarming.Instance.unitObject.EndOfPath += new System.Action(this.PlayerInactiveOnArrival);
      else
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    }
    LetterBox.Show(SnapLetterBox);
    DOTween.Kill((object) this);
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + 5f, 1f).SetEase<Tweener>(Ease.OutQuart);
  }

  private void PlayerInactiveOnArrival()
  {
    PlayerFarming.Instance.unitObject.EndOfPath -= new System.Action(this.PlayerInactiveOnArrival);
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
  }

  public void OnConversationNew(bool SetPlayerInactive, bool SnapLetterBox, bool showLetterBox)
  {
    if ((UnityEngine.Object) NotificationCentreScreen.Instance != (UnityEngine.Object) null)
      NotificationCentreScreen.Instance.FadeAndStop();
    this.ResetCachedCameraTargets();
    this.CameraSetConversationMode(true);
    this.CacheCameraTargets();
    this.RemoveAllFromCamera();
    GameObject.FindWithTag("Player");
    if (SetPlayerInactive && (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      if (PlayerFarming.Instance.GoToAndStopping)
        PlayerFarming.Instance.unitObject.EndOfPath += new System.Action(this.PlayerInactiveOnArrival);
      else
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    }
    if (showLetterBox)
    {
      if (SnapLetterBox)
        HUD_Manager.Instance.Hide(true, 0);
      LetterBox.Show(SnapLetterBox);
      DOTween.Kill((object) this);
      DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance + 5f, 1f).SetEase<Tweener>(Ease.OutQuart);
    }
    else
      HUD_Manager.Instance.Hide(false, 0);
  }

  public void OnConversationNext(GameObject Speaker, float Zoom = 9f)
  {
    if ((UnityEngine.Object) Speaker != (UnityEngine.Object) null)
    {
      this.RemoveAllFromCamera();
      this.AddToCamera(Speaker.gameObject);
    }
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
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
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
  }

  public void AddPlayerToCamera()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().AddToCamera(PlayerFarming.Instance.CameraBone);
  }

  public static GameManager GetInstance() => GameManager.instance;

  public float CachedZoom { get; set; } = -999f;

  private void CacheCameraTargets()
  {
    for (int index = this.CamFollowTarget.targets.Count - 1; index >= 0; --index)
    {
      if (this.CamFollowTarget.targets[index] == null || (UnityEngine.Object) this.CamFollowTarget.targets[index].gameObject == (UnityEngine.Object) null)
        this.CamFollowTarget.targets.RemoveAt(index);
    }
    if (this.CamFollowTarget.targets.Count <= 0)
      this.AddPlayerToCamera();
    this.CachedCamTargets = new List<CameraFollowTarget.Target>((IEnumerable<CameraFollowTarget.Target>) this.CamFollowTarget.targets);
    this.CachedZoom = this.CamFollowTarget.targetDistance;
  }

  private void ResetCachedCameraTargets()
  {
    this.CamFollowTarget.targets = new List<CameraFollowTarget.Target>((IEnumerable<CameraFollowTarget.Target>) this.CachedCamTargets);
    if ((double) this.CachedZoom != -999.0)
      this.CamFollowTarget.targetDistance = this.CachedZoom;
    if (this.CamFollowTarget.targets.Count > 0)
      return;
    this.AddPlayerToCamera();
  }

  public void RemoveAllFromCamera() => this.CamFollowTarget.ClearAllTargets();

  public void AddToCamera(GameObject gameObject, float Weight = 1f)
  {
    if ((UnityEngine.Object) this.CamFollowTarget == (UnityEngine.Object) null)
      return;
    this.CamFollowTarget.AddTarget(gameObject, Weight);
  }

  public void RemoveFromCamera(GameObject gameObject)
  {
    this.CamFollowTarget.RemoveTarget(gameObject);
  }

  public bool CameraContains(GameObject gameObject) => this.CamFollowTarget.Contains(gameObject);

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
    this.StartCoroutine((IEnumerator) this.HitStopRoutine(SleepDuration));
  }

  private IEnumerator HitStopRoutine(float SleepDuration)
  {
    Time.timeScale = 0.0f;
    yield return (object) new WaitForSecondsRealtime(SleepDuration);
    Time.timeScale = 1f;
  }

  public static bool IsDungeon(FollowerLocation location)
  {
    switch (location)
    {
      case FollowerLocation.Dungeon1_1:
      case FollowerLocation.Dungeon1_2:
      case FollowerLocation.Dungeon1_3:
      case FollowerLocation.Dungeon1_4:
      case FollowerLocation.Boss_5:
      case FollowerLocation.Dungeon1_5:
      case FollowerLocation.IntroDungeon:
        return true;
      default:
        return false;
    }
  }

  private void Update()
  {
    this.GetPlayerPosition();
    this._UnscaledTime = Time.unscaledTime;
    this.scaledTimeElapsed += Time.deltaTime;
    Shader.SetGlobalFloat("_GlobalTimeUnscaled", this._UnscaledTime);
    Shader.SetGlobalFloat("_GlobalTimeUnscaled1", this._UnscaledTime);
    if (TwitchAuthentication.IsAuthenticated)
      TwitchManager.UpdateEvents();
    if (CheatConsole.IN_DEMO)
    {
      if (InputManager.UI.GetPageNavigateLeftHeld() && InputManager.UI.GetPageNavigateRightHeld())
      {
        CheatConsole.Instance.DisplayText("Restarting... " + (object) Mathf.Max(0, Mathf.FloorToInt((float) (10.0 - (double) this.HoldToResetDemo / 5.0 * 10.0))), Color.red);
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
        CheatConsole.Instance.DisplayText("Game Inactive. Restarting... " + (object) Mathf.Max(0, Mathf.FloorToInt(120f - CheatConsole.TimeSinceLastKeyPress)), Color.red);
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
      MonoSingleton<UIManager>.Instance.ShowPauseMenu();
      this.autoPauseTimestamp = Time.unscaledTime + 600f;
    }
  }

  public static void RecalculatePaths(bool immediate = false)
  {
    if (GameManager.GetInstance().cGeneratePathfinding != null)
      GameManager.GetInstance().StopCoroutine(GameManager.GetInstance().cGeneratePathfinding);
    GameManager.GetInstance().cGeneratePathfinding = GameManager.GetInstance().StartCoroutine((IEnumerator) GameManager.GetInstance().GeneratePathfinding(immediate ? 0.0f : 1f));
  }

  private IEnumerator GeneratePathfinding(float Delay)
  {
    while ((double) (Delay -= Time.deltaTime) > 0.0)
      yield return (object) null;
    AstarPath.active.Scan((NavGraph) AstarPath.active.data.gridGraph);
    MMTransition.ResumePlay();
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

  public void QuitToMenu() => SceneManager.LoadScene("Main Menu");

  private void DemoQuitToMenu()
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
      Debug.Log((object) $"CHANGING ENDLESS LEVEL {(object) GameManager._DungeonEndlessLevel}   -   {(object) value}");
      GameManager._DungeonEndlessLevel = value;
    }
  }

  public static bool SandboxDungeonEnabled
  {
    get => (UnityEngine.Object) DungeonSandboxManager.Instance != (UnityEngine.Object) null;
  }

  public static void NewRun(string PlaceName, bool InDungeon)
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
    DataManager.SetNewRun();
    GameManager.TimeInDungeon = DataManager.Instance.TimeInGame;
    if (UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Resurrection))
      ResurrectOnHud.ResurrectionType = ResurrectionType.Pyre;
    Health.team2.Clear();
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
    SaveAndLoad.Save();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, Effect, Scene, Duration, "", new System.Action(GameManager.ToShipCallback));
  }

  private static void ToShipCallback() => GameManager.SetTimeScale(1f);

  public static void healPlayer()
  {
    DataManager.Instance.PLAYER_HEALTH = DataManager.Instance.PLAYER_TOTAL_HEALTH;
    HealthPlayer component = PlayerFarming.Instance.GetComponent<HealthPlayer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.HP += DataManager.Instance.PLAYER_HEALTH;
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
    return (UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null || GameManager.GetInstance().UpgradeTreeConfiguration.HasUnlockAvailable();
  }

  public void CreateName() => Debug.Log((object) Villager_Info.GenerateName());

  public float UnscaledTimeSince(float timestamp) => Time.unscaledTime - timestamp;

  public float TimeSince(float timestamp) => this.scaledTimeElapsed - timestamp;

  public void WaitForSeconds(float seconds, System.Action callback)
  {
    this.StartCoroutine((IEnumerator) this.WaitForSecondsIE(seconds, callback));
  }

  private IEnumerator WaitForSecondsIE(float seconds, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(seconds);
    System.Action action = callback;
    if (action != null)
      action();
  }
}
