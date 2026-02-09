// Decompiled with JetBrains decompiler
// Type: CoopManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using MMBiomeGeneration;
using MMTools;
using Rewired;
using src.Extensions;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify;
using Unify.Input;
using UnityEngine;

#nullable disable
public class CoopManager : MonoBehaviour
{
  public static CoopManager Instance;
  public static bool CoopActive = false;
  public static bool PreventWeaponSpawn = false;
  public static int Player2RewiredId = 1;
  public GameObject playerPrefab;
  [HideInInspector]
  public int playerCount;
  [CompilerGenerated]
  public bool \u003CLockedAddRemoveCoopPlayer\u003Ek__BackingField;
  public static int LambController = -1;
  public static int GoatController = -1;
  public System.Action OnPlayerJoined;
  public System.Action OnPlayerLeft;
  public System.Action OnKnockedPlayerAwoken;
  public static User AddtionalUser = (User) null;
  [HideInInspector]
  public bool transferRelicOnRemovePlayer;
  [HideInInspector]
  public bool currentlyRemovingPlayer;
  public bool _isSpawningPlayer;
  public static bool CoopBlockersEnabled;
  public float AutoReviveBuffer;
  public float WakeUpKnockedOnRoomCompleteDelay = 1f;
  public Coroutine WakeAllPlayersRoutine;
  public Coroutine WakeAllPlayersRoutineBackup;
  public static bool MainPlayerResetOnConversationEnd = false;
  public static bool PreventRecycleInCurrentRoom = false;
  public GameObject spawnAnimationFx;
  public GameObject despawnAnimationFx;
  public float spawnSfxVolume = 1f;
  public bool temporaryDisableRemoval;
  public int frameCount;
  public bool disableCoopSpawnWhileButtonHeld;
  [HideInInspector]
  public static GameObject[] AllPlayerGameObjects = new GameObject[2];

  public bool LockedAddRemoveCoopPlayer
  {
    get => this.\u003CLockedAddRemoveCoopPlayer\u003Ek__BackingField;
    set => this.\u003CLockedAddRemoveCoopPlayer\u003Ek__BackingField = value;
  }

  public bool IsSpawningPlayer => this._isSpawningPlayer;

  public bool IsSpawningOrRemovingPlayer => this.IsSpawningPlayer || this.currentlyRemovingPlayer;

  public void Awake() => CoopManager.Instance = this;

  public void Start()
  {
    if (!CoopManager.CoopActive)
      UICoopAssignController.SetInputForSoloPlay();
    this.ResetCoopWeapons();
  }

  public void OnEnable()
  {
    MMConversation.OnConversationNew += new MMConversation.ConversationNew(this.OnConversationNew);
    MMConversation.OnConversationEnd += new MMConversation.ConversationEnd(this.ResetMainPlayerOnConversationEnd);
    RoomLockController.OnRoomCompleted += new RoomLockController.RoomEvent(this.WakeAllKnockedOutPlayers);
    BiomeGenerator.OnRoomActive += new BiomeGenerator.BiomeAction(this.WakeAllKnockedOutPlayers);
    Interaction_BaseTeleporter.OnPlayerTeleportedIn += new System.Action(this.RespawnInBase);
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
  }

  public void OnControllerConnected(ControllerStatusChangedEventArgs args)
  {
    if (!CoopManager.CoopActive)
      UICoopAssignController.SetInputForSoloPlay();
    Debug.Log((object) ("SOLO PLAY SET FOR NEWLY ADDED GAMEPAD?: " + CoopManager.CoopActive.ToString()));
  }

  public void OnDisable()
  {
    MMConversation.OnConversationNew -= new MMConversation.ConversationNew(this.OnConversationNew);
    MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(this.ResetMainPlayerOnConversationEnd);
    RoomLockController.OnRoomCompleted -= new RoomLockController.RoomEvent(this.WakeAllKnockedOutPlayers);
    BiomeGenerator.OnRoomActive -= new BiomeGenerator.BiomeAction(this.WakeAllKnockedOutPlayers);
    Interaction_BaseTeleporter.OnPlayerTeleportedIn -= new System.Action(this.RespawnInBase);
    ReInput.ControllerConnectedEvent -= (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
  }

  public static void EnableCoopBlockers(bool enabled = true, bool force = false)
  {
    if (CoopManager.CoopBlockersEnabled == enabled && !force)
      return;
    CoopManager.CoopBlockersEnabled = enabled;
    foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("CoopBlocker"))
    {
      foreach (Behaviour componentsInChild in gameObject.transform.GetComponentsInChildren<Collider2D>(true))
        componentsInChild.enabled = enabled;
    }
  }

  public static void HideCoopPlayerTemporarily()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!player.isLamb)
      {
        player.gameObject.SetActive(false);
        player.indicator.gameObject.SetActive(false);
        player.transform.position = PlayerFarming.Instance.transform.position;
      }
    }
  }

  public static void UnHideCoopPlayerTemporarily()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!player.isLamb)
      {
        player.gameObject.SetActive(true);
        player.indicator.gameObject.SetActive(true);
        player.transform.position = PlayerFarming.Instance.transform.position;
      }
    }
  }

  public void Update()
  {
    CoopManager.EnableCoopBlockers(false);
    if (!CoopManager.CoopActive)
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (player.state.CURRENT_STATE == StateMachine.State.InActive || player.state.CURRENT_STATE == StateMachine.State.CustomAnimation)
      {
        CoopManager.EnableCoopBlockers();
        break;
      }
    }
    if (RoomLockController.DoorsOpen)
    {
      if ((double) Time.time <= (double) this.AutoReviveBuffer + 5.0)
        return;
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        PlayerFarming player = PlayerFarming.players[index];
        if (player.IsKnockedOut)
          CoopManager.WakeKnockedOutPlayer(player);
      }
      this.AutoReviveBuffer = Time.time;
    }
    else
      this.AutoReviveBuffer = Time.time;
  }

  public void RespawnInBase()
  {
  }

  public void WakeAllKnockedOutPlayers()
  {
    if (this.WakeAllPlayersRoutine != null)
    {
      this.StopCoroutine(this.WakeAllPlayersRoutine);
      this.WakeAllPlayersRoutine = (Coroutine) null;
    }
    this.WakeAllPlayersRoutine = this.StartCoroutine((IEnumerator) this.WakeAllKnockedOutPlayersRoutine(this.WakeUpKnockedOnRoomCompleteDelay));
    if (this.WakeAllPlayersRoutineBackup != null)
    {
      this.StopCoroutine(this.WakeAllPlayersRoutineBackup);
      this.WakeAllPlayersRoutineBackup = (Coroutine) null;
    }
    this.WakeAllPlayersRoutineBackup = this.StartCoroutine((IEnumerator) this.WakeAllKnockedOutPlayersRoutine(this.WakeUpKnockedOnRoomCompleteDelay + 3f));
  }

  public IEnumerator WakeAllKnockedOutPlayersRoutine(float timeBeforeWake)
  {
    yield return (object) new WaitForSeconds(timeBeforeWake);
    while (true)
    {
      int num = 0;
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        if (!PlayerFarming.players[index].GoToAndStopping)
          ++num;
      }
      if (num < PlayerFarming.playersCount)
        yield return (object) null;
      else
        break;
    }
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
      CoopManager.WakeKnockedOutPlayer(PlayerFarming.players[index]);
  }

  public void WakeAllKnockedOutPlayersWithHealth(float wakeUpHealth = 2f)
  {
    GameManager.GetInstance().WaitForSeconds(this.WakeUpKnockedOnRoomCompleteDelay, (System.Action) (() =>
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
        CoopManager.WakeKnockedOutPlayer(PlayerFarming.players[index], wakeUpHealth);
    }));
  }

  public static void KnockOutPlayer(PlayerFarming playerFarming)
  {
    if (playerFarming.IsKnockedOut)
      return;
    playerFarming.IsKnockedOut = true;
    playerFarming.health.invincible = true;
    playerFarming.health.untouchable = true;
    playerFarming.health.ClearAllStasisEffects();
    playerFarming.playerSpells?.HideChargeBars();
    AudioManager.Instance.PlayOneShot("event:/player/death_hit", playerFarming.gameObject);
    string prefix = "Downed/";
    string suffix = "";
    if (!playerFarming.isLamb || playerFarming.IsGoat)
    {
      prefix = "Downed/Goat/";
      suffix = "-goat";
    }
    else if (playerFarming.isLamb && DataManager.Instance.PlayerVisualFleece == 676)
      prefix = "Downed/Cowboy/cowboy-";
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.LockStateChanges = true;
    playerFarming.CustomAnimation($"{prefix}knockback-to-downed{suffix}", false);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() =>
    {
      playerFarming.state.LockStateChanges = false;
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      playerFarming.playerController.SetSpecificMovementAnimations($"{prefix}idle{suffix}", $"{prefix}walk-up{suffix}", $"{prefix}walk-down{suffix}", $"{prefix}walk{suffix}", $"{prefix}walk-up-diagonal{suffix}", $"{prefix}walk-horizontal{suffix}");
      playerFarming.GetComponent<Interaction_CoopRevive>().enabled = true;
    }));
  }

  public static void WakeKnockedOutPlayer(
    PlayerFarming playerFarming,
    float wakeUpHealth = 2f,
    PlayerFarming playerWhoIsReviving = null,
    Interaction_HeartPickupBase.HeartPickupType heartType = Interaction_HeartPickupBase.HeartPickupType.Red,
    bool pauseTime = false)
  {
    float currentHp = playerFarming.health.CurrentHP;
    playerFarming.GetComponent<Interaction_CoopRevive>().enabled = false;
    playerFarming.IsKnockedOut = false;
    if (playerFarming.IsKnockedOut || (double) currentHp <= 0.0)
    {
      if (pauseTime)
      {
        foreach (PlayerFarming player in PlayerFarming.players)
          player.SpineUseDeltaTime(false);
        DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 0.0f, 0.2f);
      }
      MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
      playerFarming.health.invincible = false;
      playerFarming.health.untouchable = false;
      foreach (PlayerFarming player in PlayerFarming.players)
        player.playerController.MakeInvincible(0.5f);
      playerFarming.health.enabled = true;
      playerFarming.playerController.ResetSpecificMovementAnimations();
      playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      if (DataManager.Instance.PlayerFleece == 5)
        playerFarming.health.BlueHearts = wakeUpHealth * 1.5f;
      else if (heartType == Interaction_HeartPickupBase.HeartPickupType.Blue)
        playerFarming.health.BlueHearts = wakeUpHealth;
      else if ((double) currentHp <= 0.0)
        playerFarming.health.Heal(wakeUpHealth);
      CoopManager.Instance.SetWakeValues(playerFarming, wakeUpHealth);
      float delayAnimationTimeMultiplier = 1f;
      if ((UnityEngine.Object) playerWhoIsReviving == (UnityEngine.Object) null)
        delayAnimationTimeMultiplier = 0.1f;
      CoopManager.Instance.ReviveSpawnEffects(playerFarming, playerWhoIsReviving, delayAnimationTimeMultiplier, pauseTime);
      if (playerFarming.isLamb)
        BiomeGenerator.Instance.DoSpawnDemons(true);
    }
    GameManager.GetInstance().WaitForSecondsRealtime(0.5f, (System.Action) (() => CoopManager.Instance.SetWakeValues(playerFarming, wakeUpHealth)));
  }

  public void SetWakeValues(PlayerFarming playerFarming, float wakeUpHealth)
  {
    if (!MMConversation.isPlaying)
      GameManager.GetInstance().AddToCamera(playerFarming.CameraBone);
    if ((double) playerFarming.health.CurrentHP <= 0.0)
      playerFarming.health.HP = wakeUpHealth;
    playerFarming.GetComponent<Interaction_CoopRevive>().enabled = false;
    playerFarming.IsKnockedOut = false;
  }

  public void OnConversationNew(
    bool SetPlayerInactive = true,
    bool SnapLetterBox = false,
    bool ShowLetterBox = true,
    PlayerFarming playerFarming = null)
  {
    if (!SetPlayerInactive)
      return;
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
  }

  public void ResetMainPlayerOnConversationEnd(bool SetPlayerToIdle = true, bool ShowHUD = true)
  {
    if (CoopManager.MainPlayerResetOnConversationEnd)
      PlayerFarming.ResetMainPlayer();
    if (!SetPlayerToIdle)
      return;
    PlayerFarming.SetStateForAllPlayers();
  }

  public void OnDestroy()
  {
    MMConversation.OnConversationEnd -= new MMConversation.ConversationEnd(this.ResetMainPlayerOnConversationEnd);
  }

  public void SpawnCoopPlayer(int slot, bool playEffects = true, float startingHealth = -1f)
  {
    if (DataManager.Instance.PlayerVisualFleece == 1003)
    {
      DataManager.Instance.PlayerVisualFleece = 0;
      DataManager.Instance.PlayerFleece = 0;
      PlayerFarming.players[0].SetSkin();
    }
    CoopManager.PreventRecycleInCurrentRoom = true;
    this._isSpawningPlayer = true;
    this.StartCoroutine((IEnumerator) this.WaitTillPlayersRady((System.Action) (() =>
    {
      CoopManager.CoopActive = true;
      if (playEffects)
        BiomeConstants.Instance.CoOpScreenEffect();
      GameObject coopPlayer = this.CreateCoopPlayer(slot);
      coopPlayer.transform.position = this.CheckClearDirection(PlayerFarming.players[0].transform, 2f, 1.5f);
      PlayerFarming playerFarming = coopPlayer.GetComponent<PlayerFarming>();
      playerFarming.isLamb = false;
      playerFarming.Init();
      playerFarming.playerID = slot;
      playerFarming.transform.parent = PlayerFarming.players[0].transform.parent;
      playerFarming.gameObject.SetActive(true);
      playerFarming.Spine.GetComponent<MeshRenderer>().enabled = true;
      if ((UnityEngine.Object) playerFarming.hudHearts != (UnityEngine.Object) null)
        playerFarming.hudHearts.gameObject.SetActive(true);
      if (playEffects)
        this.CoopSpawnEffects(playerFarming);
      GameManager.GetInstance();
      if (LocationManager.LocationIsDungeon(PlayerFarming.Location))
      {
        if ((bool) (UnityEngine.Object) BiomeGenerator.Instance && !GameManager.InitialDungeonEnter && !playEffects)
        {
          this.StartCoroutine((IEnumerator) BiomeGenerator.Instance.ApplyCurrentFleeceModifiersIE(playerFarming));
          if (!DataManager.Instance.UnlockedCoopRelicsAndTarots)
            this.StartCoroutine((IEnumerator) this.GiveCoopCards(playerFarming));
        }
        if (LocationManager._Instance.Location == FollowerLocation.IntroDungeon)
          this.StartCoroutine((IEnumerator) this.WaitUntilPlayerIsInitialised(playerFarming, (System.Action) (() => this.StartCoroutine((IEnumerator) this.SpawnCoopWeapons(playerFarming)))));
        else
          this.StartCoroutine((IEnumerator) this.SpawnCoopWeapons(playerFarming));
      }
      PlayerFarming.RefreshPlayersCount();
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        if (PlayerFarming.players[index].health.GodMode == Health.CheatMode.Demigod)
          playerFarming.health.GodMode = Health.CheatMode.Demigod;
      }
      if (!CoopManager.PreventWeaponSpawn && GameManager.IsDungeon(PlayerFarming.Location))
        this.StartCoroutine((IEnumerator) this.WaitUntilPlayerIsInitialised(playerFarming, (System.Action) (() =>
        {
          for (int index = 0; index < PlayerFarming.playersCount; ++index)
          {
            PlayerFarming player = PlayerFarming.players[index];
            if (player.isLamb)
            {
              if (playerFarming.currentWeapon == EquipmentType.None && player.currentWeapon != EquipmentType.None)
                playerFarming.playerWeapon.SetWeapon(player.currentWeapon, player.currentWeaponLevel);
              if (playerFarming.currentCurse == EquipmentType.None && player.currentCurse != EquipmentType.None)
                playerFarming.playerSpells.SetSpell(player.currentCurse, player.currentCurseLevel);
            }
          }
          if (playerFarming.currentWeapon != EquipmentType.None || PlayerFleeceManager.FleeceSwapsWeaponForCurse())
            return;
          playerFarming.playerWeapon.SetWeapon(EquipmentType.Sword, 1);
        })));
      if ((double) startingHealth != -1.0)
        playerFarming.health.HP = startingHealth;
      CoopManager.RefreshCoopPlayerRewired();
      UserHelper.OnPlayerUserChanged += new UserHelper.PlayerUserChangedDelegate(CoopManager.OnPlayerUserChanged);
      UserHelper.OnPlayerGamePadChanged += new UserHelper.PlayerGamePadChangedDelegate(CoopManager.OnPlayerGamePadChanged);
      this.StartCoroutine((IEnumerator) this.WaitUntilPlayerIsInitialised(playerFarming, (System.Action) (() => this._isSpawningPlayer = false)));
      System.Action onPlayerJoined = this.OnPlayerJoined;
      if (onPlayerJoined != null)
        onPlayerJoined();
      DifficultyManager.LoadCurrentDifficulty();
    })));
  }

  public IEnumerator GiveTarotCardsSequance(PlayerFarming playerFarming, System.Action animationCallback = null)
  {
    CoopManager coopManager = this;
    if (LocationManager.LocationIsDungeon(PlayerFarming.Location) && (bool) (UnityEngine.Object) BiomeGenerator.Instance && !GameManager.InitialDungeonEnter)
      yield return (object) coopManager.StartCoroutine((IEnumerator) BiomeGenerator.Instance.ApplyCurrentFleeceModifiersIE(playerFarming));
    System.Action action = animationCallback;
    if (action != null)
      action();
    if (!DataManager.Instance.UnlockedCoopRelics || !DataManager.Instance.UnlockedCoopTarots)
      DataManager.Instance.UnlockedCoopRelicsAndTarots = false;
    yield return (object) coopManager.StartCoroutine((IEnumerator) coopManager.GiveCoopCards(playerFarming));
  }

  public IEnumerator GiveCoopCards(PlayerFarming playerFarming)
  {
    if (DataManager.Instance.UnlockedCoopTarots && DataManager.Instance.UnlockedCoopRelics)
    {
      DataManager.Instance.UnlockedCoopRelicsAndTarots = true;
    }
    else
    {
      yield return (object) new WaitForEndOfFrame();
      Time.timeScale = 0.0f;
      GameManager.GetInstance().OnConversationNew();
      if (DataManager.Instance.HasEncounteredTarot && !DataManager.Instance.UnlockedCoopTarots)
      {
        MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.players[0];
        MonoSingleton<UINavigatorNew>.Instance.TemporarilyAllowInputOnlyFromAnyPlayer = true;
        UITarotCardsMenuController menu = MonoSingleton<UIManager>.Instance.TarotCardsMenuTemplate.Instantiate<UITarotCardsMenuController>();
        menu.Show(TarotCards.CoopCards);
        menu.SetCoopTitleHeader();
        yield return (object) menu.YieldUntilHidden();
        DataManager.Instance.UnlockedCoopTarots = true;
      }
      if (DataManager.Instance.OnboardedRelics && !DataManager.Instance.UnlockedCoopRelics)
      {
        MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.players[0];
        MonoSingleton<UINavigatorNew>.Instance.TemporarilyAllowInputOnlyFromAnyPlayer = true;
        UIRelicMenuController menu = MonoSingleton<UIManager>.Instance.RelicMenuTemplate.Instantiate<UIRelicMenuController>();
        List<RelicType> relicTypes = new List<RelicType>();
        foreach (RelicData relicData in EquipmentManager.RelicData)
        {
          if (relicData.RelicSubType == RelicSubType.Coop)
            relicTypes.Add(relicData.RelicType);
        }
        DataManager.Instance.UnlockedCoopRelics = true;
        UpgradeSystem.UnlockAbility(UpgradeSystem.Type.Relic_Pack_Coop, true);
        menu.Show(relicTypes);
        menu.SetCoopTitleHeader();
        yield return (object) menu.YieldUntilHidden();
        Time.timeScale = 1f;
      }
      if (DataManager.Instance.UnlockedCoopTarots && DataManager.Instance.UnlockedCoopRelics)
        DataManager.Instance.UnlockedCoopRelicsAndTarots = true;
      MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = (PlayerFarming) null;
      MonoSingleton<UINavigatorNew>.Instance.TemporarilyAllowInputOnlyFromAnyPlayer = false;
      LetterBox.Hide();
      HUD_Manager.Instance.Show(0);
      GameManager.GetInstance().OnConversationEnd();
      GameManager.GetInstance().CameraResetTargetZoom();
      GameManager.GetInstance().AddPlayerToCamera();
      PlayerFarming.SetStateForAllPlayers();
      Time.timeScale = 1f;
    }
  }

  public IEnumerator WaitTillPlayersRady(System.Action callback)
  {
    while (PlayerFarming.Instance.GoToAndStopping || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.InActive || LetterBox.IsPlaying || MMConversation.isPlaying)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator WaitUntilPlayerIsInitialised(PlayerFarming playerFarming, System.Action callback)
  {
    while (playerFarming.GoToAndStopping || playerFarming.state.CURRENT_STATE == StateMachine.State.InActive)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  public GameObject CreateCoopPlayer(int slot)
  {
    GameObject coopPlayer = CoopManager.AllPlayerGameObjects[slot];
    if ((UnityEngine.Object) coopPlayer == (UnityEngine.Object) null)
    {
      coopPlayer = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab);
      coopPlayer.name = "COOP PLAYER" + slot.ToString();
      CoopManager.AllPlayerGameObjects[slot] = coopPlayer;
    }
    coopPlayer.SetActive(false);
    return coopPlayer;
  }

  public void CoopSpawnEffects(PlayerFarming playerFarming)
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/goat_spawn");
    GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() => AudioManager.Instance.PlayOneShot("event:/player/standard_jump_spin_float")));
    PlayerFarming.Instance.StopRidingOnAnimal();
    PlayerFarming.Instance.CustomAnimation("spawn-goat", false);
    playerFarming.gameObject.SetActive(false);
    playerFarming.Spine.GetComponent<MeshRenderer>().enabled = false;
    GameManager.GetInstance().WaitForSeconds(3f, (System.Action) (() => PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive));
    GameManager.GetInstance().WaitForSeconds(1.36666667f, (System.Action) (() =>
    {
      AudioManager.Instance.PlayOneShot("event:/relics/demon_spawn", PlayerFarming.Instance.transform.position);
      for (int index = 0; index < 5; ++index)
      {
        Vector3 vector3 = new Vector3(-0.033f, 0.33f, -1.2f);
        SoulCustomTarget.Create(playerFarming.gameObject, PlayerFarming.Instance.transform.position + vector3, Color.black, (System.Action) null, 0.2f, AddZOffset: false, fromPool: false, sfxPath: "", collectSfxPath: "");
      }
      GameManager.GetInstance().WaitForSeconds(0.5f, (System.Action) (() =>
      {
        AudioManager.Instance.PlayOneShot("event:/relics/lightning/lightning_impact", playerFarming.transform.position);
        playerFarming.gameObject.SetActive(true);
        playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        playerFarming.state.LockStateChanges = true;
        playerFarming.CustomAnimation("respawn-fast-goat", false);
        GameManager.GetInstance().WaitForSeconds(0.1f, (System.Action) (() => playerFarming.Spine.GetComponent<MeshRenderer>().enabled = true));
        if ((UnityEngine.Object) this.spawnAnimationFx != (UnityEngine.Object) null)
        {
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.spawnAnimationFx, playerFarming.transform.parent);
          gameObject.transform.position = playerFarming.transform.position;
          UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 3f);
        }
        GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => this.StartCoroutine((IEnumerator) this.GiveTarotCardsSequance(playerFarming, (System.Action) (() =>
        {
          playerFarming.state.LockStateChanges = false;
          PlayerFarming.SetStateForAllPlayers();
          GameManager.GetInstance().OnConversationEnd();
        })))));
        int num = (int) AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/player/goat_player/goat_bell", (Transform) null).setVolume(this.spawnSfxVolume);
        CameraManager.shakeCamera(2f);
      }));
    }));
  }

  public void ReviveSpawnEffects(
    PlayerFarming playerFarming,
    PlayerFarming playerWhoIsReviving = null,
    float delayAnimationTimeMultiplier = 1f,
    bool pauseTime = false)
  {
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 6f);
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    playerFarming.state.LockStateChanges = true;
    playerFarming.CustomAnimation("Downed/revive", true);
    if ((bool) (UnityEngine.Object) playerWhoIsReviving)
    {
      playerWhoIsReviving.CustomAnimation("spawn-goat", false);
      GameManager.GetInstance().WaitForSecondsRealtime(3f, (System.Action) (() => playerWhoIsReviving.state.CURRENT_STATE = StateMachine.State.Idle));
    }
    GameManager.GetInstance().WaitForSecondsRealtime(1.36666667f * delayAnimationTimeMultiplier, (System.Action) (() =>
    {
      if ((bool) (UnityEngine.Object) playerWhoIsReviving)
      {
        Vector3 position = playerWhoIsReviving.transform.position - Vector3.forward;
        for (int index = 0; index < 5; ++index)
          SoulCustomTarget.Create(playerFarming.gameObject, position, Color.red, (System.Action) null, 0.2f);
      }
      GameManager.GetInstance().WaitForSecondsRealtime(0.5f * delayAnimationTimeMultiplier, (System.Action) (() =>
      {
        playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        playerFarming.state.LockStateChanges = true;
        if (playerFarming.isLamb)
          playerFarming.CustomAnimation("revived", false);
        else
          playerFarming.CustomAnimation("revived-goat", false);
        GameManager.GetInstance().WaitForSecondsRealtime(2f, (System.Action) (() =>
        {
          playerFarming.state.LockStateChanges = false;
          playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
          GameManager.GetInstance().OnConversationEnd();
          System.Action knockedPlayerAwoken = this.OnKnockedPlayerAwoken;
          if (knockedPlayerAwoken != null)
            knockedPlayerAwoken();
          if (pauseTime)
          {
            foreach (PlayerFarming player in PlayerFarming.players)
              player.SpineUseDeltaTime(true);
            DOTween.To((DOGetter<float>) (() => Time.timeScale), (DOSetter<float>) (x => Time.timeScale = x), 1f, 0.2f).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
          }
          MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
        }));
        int num = (int) AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/player/goat_player/goat_bell", (Transform) null).setVolume(this.spawnSfxVolume);
        CameraManager.shakeCamera(2f);
      }));
    }));
  }

  public IEnumerator StaggerSoulCustomTargerSpawns(GameObject spawnedPlayerGameObject)
  {
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i < 5; ++i)
    {
      yield return (object) new WaitForSeconds(0.2f);
      Vector3 vector3 = new Vector3(-0.033f, 0.33f, -1.2f);
      SoulCustomTarget.Create(PlayerFarming.Instance.transform.position + vector3, spawnedPlayerGameObject.transform.position, Color.black, (System.Action) null, 0.2f, AddZOffset: false, fromPool: false, sfxPath: "", collectSfxPath: "event:/relics/demon_spawn");
    }
    yield return (object) null;
  }

  public void CoopHideEffects(GameObject spawnedPlayerGameObject)
  {
    this.StartCoroutine((IEnumerator) this.StaggerSoulCustomTargerSpawns(spawnedPlayerGameObject));
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.despawnAnimationFx, spawnedPlayerGameObject.transform.parent);
    gameObject.transform.position = spawnedPlayerGameObject.transform.position;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, 3f);
    AudioManager.Instance.PlayOneShot("event:/relics/lightning/lightning_impact", gameObject.transform.position);
    int num = (int) AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/player/goat_player/goat_bell", (Transform) null).setVolume(this.spawnSfxVolume);
    CameraManager.shakeCamera(2f);
    HUD_Manager.Instance.Show(Force: true);
  }

  public void ResetCoopWeapons()
  {
  }

  public static void AddPlayerFromMenu()
  {
    if (PlayerFarming.playersCount != 1)
      return;
    UICoopAssignController coopAssignMenu = MonoSingleton<UIManager>.Instance.ShowCoopAssignMenu();
    UICoopAssignController assignController1 = coopAssignMenu;
    assignController1.OnShownCompleted = assignController1.OnShownCompleted + (System.Action) (() => { });
    UICoopAssignController assignController2 = coopAssignMenu;
    assignController2.OnHidden = assignController2.OnHidden + (System.Action) (() =>
    {
      coopAssignMenu = (UICoopAssignController) null;
      if (!CoopManager.CoopActive)
        return;
      CoopManager.AddtionalUser = UserHelper.GetPlayer(1);
    });
  }

  public static void ClearCoopMode()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if (!player.isLamb)
        CoopManager.RemoveCoopPlayerStatic(player, disengagePlayer: true, withDelay: false);
      UnityEngine.Object.Destroy((UnityEngine.Object) player.gameObject);
    }
    PlayerFarming.players.Clear();
    CoopManager.RemovePlayerFromMenu();
    PlayerFarming.playersCount = 0;
    CoopManager.CoopActive = false;
  }

  public static void RemovePlayerFromMenu()
  {
    if (PlayerFarming.playersCount <= 1 || !CoopManager.CoopActive)
      return;
    if ((bool) (UnityEngine.Object) CoopManager.Instance)
    {
      CoopManager.Instance.OnPlayerLeft += new System.Action(CoopManager.CleanKnockedPlayer);
      CoopManager.Instance.RemoveCoopPlayer(PlayerFarming.players[1], MMConversation.CURRENT_CONVERSATION != null, true);
    }
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = PlayerFarming.players[0];
    UserHelper.OnPlayerUserChanged -= new UserHelper.PlayerUserChangedDelegate(CoopManager.OnPlayerUserChanged);
    UserHelper.OnPlayerGamePadChanged -= new UserHelper.PlayerGamePadChangedDelegate(CoopManager.OnPlayerGamePadChanged);
    Engagement.GlobalAllowEngagement = false;
    UICoopAssignController.SetInputForSoloPlay();
    CoopManager.CoopActive = false;
    CoopManager.EnableCoopBlockers(false, true);
    DifficultyManager.LoadCurrentDifficulty();
  }

  public static void CleanKnockedPlayer()
  {
    PlayerFarming player = PlayerFarming.players[0];
    if (!player.IsKnockedOut || !LocationManager.IsDungeonActive())
      return;
    player.health.invincible = false;
    player.health.untouchable = false;
    player.health.enabled = true;
    player.IsKnockedOut = false;
    player.GetComponent<Interaction_CoopRevive>().enabled = false;
    player.state.CURRENT_STATE = StateMachine.State.Idle;
    player.health.DealDamage(9999f, player.gameObject, player.transform.position, false, Health.AttackTypes.Melee, false, (Health.AttackFlags) 0);
    CoopManager.Instance.OnPlayerLeft -= new System.Action(CoopManager.CleanKnockedPlayer);
  }

  public void LockAddRemovePlayer() => this.LockedAddRemoveCoopPlayer = true;

  public void UnlockAddRemovePlayer() => this.LockedAddRemoveCoopPlayer = false;

  public void ForceCoopWeapons()
  {
  }

  public IEnumerator SpawnCoopWeapons(
    PlayerFarming playerFarming,
    bool forceWeapon = false,
    bool forceCurse = true)
  {
    yield break;
  }

  public void RemoveCoopPlayer(
    PlayerFarming playerToRemove,
    bool instant = true,
    bool disengagePlayer = false,
    bool withDelay = true)
  {
    if (playerToRemove.IsRidingAnimal())
      playerToRemove.StopRidingOnAnimal();
    if (playerToRemove.IsLeashingAnimal())
      playerToRemove.StopLeashingAnimal();
    if (playerToRemove.CarryingSnowball)
    {
      Debug.Log((object) "SNOWBALL CODE");
      playerToRemove.RemoveSnowballInstant();
    }
    if ((UnityEngine.Object) playerToRemove.PuzzlePieceCarried != (UnityEngine.Object) null)
    {
      Debug.Log((object) "Puzzle piece CODE");
      playerToRemove.PuzzlePieceCarried.DropBody();
    }
    if (instant)
    {
      if (this.transferRelicOnRemovePlayer && playerToRemove.currentRelicType != RelicType.None && PlayerFarming.players[0].currentRelicType == RelicType.None)
      {
        PlayerFarming.players[0].playerRelic.EquipRelic(EquipmentManager.GetRelicData(playerToRemove.currentRelicType), false);
        PlayerFarming.players[0].playerRelic.ChargedAmount = playerToRemove.playerRelic.ChargedAmount;
        playerToRemove.playerRelic.RemoveRelic(false);
      }
      if (disengagePlayer)
        UserHelper.DisengagePlayer(1);
      HUD_Manager.Instance?.ClearPlayersWidgets();
      PlayerFarming.HidePlayer(playerToRemove, withDelay);
      this.disableCoopSpawnWhileButtonHeld = true;
      PlayerFarming.RefreshPlayersCount();
      PlayerFarming.ResetMainPlayer();
      CoopManager.RefreshCoopPlayerRewired();
      Shader.SetGlobalVector("_GlobalSecondPlayerPos", (Vector4) Vector3.zero);
      System.Action onPlayerLeft = this.OnPlayerLeft;
      if (onPlayerLeft == null)
        return;
      onPlayerLeft();
    }
    else
      this.StartCoroutine((IEnumerator) this.AnimateRemoveCoopPlayer(playerToRemove, disengagePlayer));
  }

  public static void RemoveCoopPlayerStatic(
    PlayerFarming playerToRemove,
    bool instant = true,
    bool disengagePlayer = false,
    bool withDelay = true)
  {
    if ((UnityEngine.Object) playerToRemove != (UnityEngine.Object) null && playerToRemove.currentRelicType != RelicType.None && PlayerFarming.players[0].currentRelicType == RelicType.None)
    {
      PlayerFarming.players[0].playerRelic.EquipRelic(EquipmentManager.GetRelicData(playerToRemove.currentRelicType), false);
      PlayerFarming.players[0].playerRelic.ChargedAmount = playerToRemove.playerRelic.ChargedAmount;
      playerToRemove.playerRelic.RemoveRelic(false);
    }
    if (disengagePlayer)
      UserHelper.DisengagePlayer(1);
    HUD_Manager.Instance?.ClearPlayersWidgets();
    PlayerFarming.HidePlayer(playerToRemove, withDelay);
    PlayerFarming.RefreshPlayersCount();
    PlayerFarming.ResetMainPlayer();
    CoopManager.RefreshCoopPlayerRewired();
  }

  public IEnumerator AnimateRemoveCoopPlayer(PlayerFarming playerFarming, bool disengagePlayer)
  {
    this.currentlyRemovingPlayer = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 7f);
    playerFarming.AbortGoTo();
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForEndOfFrame();
    playerFarming.state.LockStateChanges = true;
    playerFarming.CustomAnimation("spawn-out-goat", false);
    yield return (object) new WaitForSeconds(0.6f);
    CoopManager.Instance.CoopHideEffects(playerFarming.gameObject);
    yield return (object) new WaitForSeconds(0.6f);
    playerFarming.Spine.GetComponent<MeshRenderer>().enabled = false;
    playerFarming.state.LockStateChanges = false;
    playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
    this.RemoveCoopPlayer(playerFarming, disengagePlayer: disengagePlayer);
    GameManager.GetInstance().OnConversationEnd();
    yield return (object) new WaitForSeconds(0.5f);
    UICoopAssignController.SetInputForSoloPlay();
    CoopManager.RefreshCoopPlayerRewired();
    this.currentlyRemovingPlayer = false;
  }

  public Vector3 CheckClearDirection(
    Transform mainPlayer,
    float checkDistance,
    float spawnDistance)
  {
    LayerMask mask = (LayerMask) LayerMask.GetMask("Island", "Obstacles");
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) mainPlayer.position, (Vector2) Vector3.right, checkDistance, (int) mask).collider == (UnityEngine.Object) null)
      return mainPlayer.position + Vector3.right * spawnDistance;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) mainPlayer.position, (Vector2) Vector3.left, checkDistance, (int) mask).collider == (UnityEngine.Object) null)
      return mainPlayer.position + Vector3.left * spawnDistance;
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) mainPlayer.position, (Vector2) Vector3.up, checkDistance, (int) mask).collider == (UnityEngine.Object) null)
      return mainPlayer.position + Vector3.up * spawnDistance;
    return (UnityEngine.Object) Physics2D.Raycast((Vector2) mainPlayer.position, (Vector2) Vector3.down, checkDistance, (int) mask).collider == (UnityEngine.Object) null ? mainPlayer.position + Vector3.down * spawnDistance : mainPlayer.position;
  }

  public static void HandleCoopCreation()
  {
    bool flag = false;
    for (int index = 1; index < CoopManager.AllPlayerGameObjects.Length; ++index)
    {
      if ((UnityEngine.Object) CoopManager.AllPlayerGameObjects[index] == (UnityEngine.Object) null || !CoopManager.AllPlayerGameObjects[index].activeSelf)
      {
        if ((UnityEngine.Object) CoopManager.AllPlayerGameObjects[index] != (UnityEngine.Object) null && (double) Time.time < (double) CoopManager.AllPlayerGameObjects[index].GetComponent<PlayerFarming>().playerWasHidden + 2.0)
          return;
        Player player = RewiredInputManager.GetPlayer(index);
        if (player != null && ((ICollection<Joystick>) player.controllers.Joysticks).Count > 0)
        {
          if ((bool) (UnityEngine.Object) CoopManager.Instance)
            CoopManager.Instance.SpawnCoopPlayer(index);
          flag = true;
        }
      }
    }
    if (!flag)
      return;
    CoopManager.RefreshCoopPlayerRewired();
  }

  public static void OnPlayerUserChanged(int playerNo, User was, User now)
  {
    if (now != null || CoopManager.AddtionalUser == null || was.id != CoopManager.AddtionalUser.id)
      return;
    CoopManager.AddtionalUser = (User) null;
    CoopManager.RemovePlayerFromMenu();
  }

  public static void HandleSignOutDuringTransistions()
  {
  }

  public static void OnPlayerGamePadChanged(int playerNo, User user)
  {
    CoopManager.RefreshCoopPlayerRewired();
  }

  public static void RefreshCoopPlayerRewired()
  {
    if (ReInput.controllers == null)
      return;
    int joystickCount = ReInput.controllers.joystickCount;
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      PlayerFarming player = PlayerFarming.players[index];
      if (player.gameObject.activeSelf)
      {
        player.rewiredPlayer = RewiredInputManager.GetPlayer(index);
        player.canUseKeyboard = player.rewiredPlayer.controllers.hasKeyboard;
        if (player.canUseKeyboard && (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null && UnityEngine.Input.mousePosition != MonoSingleton<UINavigatorNew>.Instance.PreviousMouseInput)
          Cursor.visible = true;
      }
    }
  }
}
