// Decompiled with JetBrains decompiler
// Type: RespawnRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class RespawnRoomManager : BaseMonoBehaviour
{
  public static RespawnRoomManager Instance;
  public BiomeGenerator biomeGenerator;
  public GenerateRoom generateRoom;
  public GameObject Player;
  public GameObject PlayerPrefab;
  public GameObject LightingOverride;
  [SerializeField]
  public Interaction_SimpleConversation conversation;
  [SerializeField]
  public AnimationCurve absorbSoulCurve;
  [SerializeField]
  public GameObject teleporter;
  public AstarPath astarPath;
  public Stylizer cameraStylizer;
  [SerializeField]
  public bool isCorruptedRoom = true;
  [SerializeField]
  public GoatShadowController GoatShadowController;
  [SerializeField]
  public GoatShadowController GoatShadowControllerRed;
  [CompilerGenerated]
  public bool \u003CRespawning\u003Ek__BackingField;
  public static bool forceRespawnOnce;
  public static bool RespawnPlayInProgress;
  public Follower FollowerPrefab;
  public Transform FollowerPosition;
  public Transform CorruptedSpawnPlayerPosition;
  public List<FollowerManager.SpawnedFollower> followers = new List<FollowerManager.SpawnedFollower>();
  public EventInstance LoopInstance;
  public static float HP;
  public static float SpiritHearts;
  public static float BlueHearts;
  public static float BlackHearts;
  public static float FireHearts;
  public static float IceHearts;
  public List<GameObject> EnableCorrupted = new List<GameObject>();
  public List<GameObject> DisableCorrupted = new List<GameObject>();

  public static event System.Action OnRespawnRoomShown;

  public bool Respawning
  {
    get => this.\u003CRespawning\u003Ek__BackingField;
    set => this.\u003CRespawning\u003Ek__BackingField = value;
  }

  public void OnEnable()
  {
    RespawnRoomManager.Instance = this;
    this.generateRoom = this.GetComponent<GenerateRoom>();
    this.isCorruptedRoom = ResurrectOnHud.ResurrectionType == ResurrectionType.CorruptedMonolith;
    this.cameraStylizer = Camera.main.gameObject.GetComponent<Stylizer>();
    if ((UnityEngine.Object) this.cameraStylizer == (UnityEngine.Object) null)
      Debug.Log((object) "Camera null");
    this.cameraStylizer.enabled = !this.isCorruptedRoom;
    if (!DataManager.Instance.PermadeDeathActive)
      return;
    this.teleporter.SetActive(false);
  }

  public void OnDestroy() => RespawnRoomManager.Instance = (RespawnRoomManager) null;

  public void Init(BiomeGenerator biomeGenerator) => this.biomeGenerator = biomeGenerator;

  public static void Play()
  {
    if (RespawnRoomManager.RespawnPlayInProgress || MMTransition.IsPlaying)
      return;
    Debug.Log((object) "PLAY");
    RespawnRoomManager.forceRespawnOnce = false;
    RespawnRoomManager.RespawnPlayInProgress = true;
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      if (RespawnRoomManager.Instance.gameObject.activeSelf)
      {
        RespawnRoomManager.RespawnPlayInProgress = false;
      }
      else
      {
        Time.timeScale = 1f;
        if (LightingManager.Instance.IsTransitionActive)
          LightingManager.Instance.StartCoroutine((IEnumerator) RespawnRoomManager.Instance.PlayRoutineDelayed());
        else
          RespawnRoomManager.Instance.StartPlayRoutine();
      }
    }));
  }

  public void StartPlayRoutine()
  {
    RespawnRoomManager.Instance.gameObject.SetActive(true);
    RespawnRoomManager.Instance.StartCoroutine((IEnumerator) RespawnRoomManager.Instance.PlayRoutine());
    System.Action respawnRoomShown = RespawnRoomManager.OnRespawnRoomShown;
    if (respawnRoomShown == null)
      return;
    respawnRoomShown();
  }

  public IEnumerator PlayRoutineDelayed()
  {
    float startTime = Time.realtimeSinceStartup;
    while (LightingManager.Instance.IsTransitionActive || (double) Time.realtimeSinceStartup - (double) startTime < 5.0)
    {
      Time.timeScale = 1f;
      yield return (object) null;
    }
    this.StartPlayRoutine();
  }

  public void SpawnFollowers()
  {
    List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation(FollowerLocation.Base);
    for (int index = 0; index < simFollowerList.Count; ++index)
    {
      if (simFollowerList[index].Brain.Info.CursedState != Thought.Child)
      {
        FollowerManager.SpawnedFollower f = FollowerManager.SpawnCopyFollower(simFollowerList[index].Brain._directInfoAccess, this.transform.position, this.FollowerPosition, PlayerFarming.Location);
        this.followers.Add(f);
        f.Follower.GetComponentInChildren<UIFollowerName>(true).Show();
        f.Follower.transform.position = this.GetCirclePosition(simFollowerList.Count, index);
        f.Follower.Interaction_FollowerInteraction.enabled = false;
        f.FollowerFakeBrain.HardSwapToTask((FollowerTask) new FollowerTask_AwaitConsuming());
        f.Follower.State.LookAngle = f.Follower.State.facingAngle = Utils.GetAngle(f.Follower.transform.position, this.FollowerPosition.transform.position);
        f.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
        double num = (double) f.Follower.SetBodyAnimation("pray", true);
        Interaction_ConsumeFollower interactionConsumeFollower = f.Follower.gameObject.AddComponent<Interaction_ConsumeFollower>();
        interactionConsumeFollower.followerSpine = f.Follower.Spine;
        interactionConsumeFollower.Play(simFollowerList[index].Brain._directInfoAccess, (Action<int, int, int, int, int, int>) ((HP, SpiritHearts, BlueHearts, BlackHearts, IceHearts, FireHearts) =>
        {
          RespawnRoomManager.HP = (float) HP;
          RespawnRoomManager.SpiritHearts = (float) SpiritHearts;
          RespawnRoomManager.BlueHearts = (float) BlueHearts;
          RespawnRoomManager.BlackHearts = (float) BlackHearts;
          RespawnRoomManager.IceHearts = (float) IceHearts;
          RespawnRoomManager.FireHearts = (float) FireHearts;
          this.StartCoroutine((IEnumerator) this.ConsumefollowerRoutine(f));
        }));
      }
    }
  }

  public Vector3 GetCirclePosition(int followersCount, int index, float forceTargetDistance = -1f)
  {
    float num = Mathf.Max(Mathf.Clamp((float) (followersCount / 3), 2f, 3.5f), forceTargetDistance);
    float f = (float) ((double) index * (360.0 / (double) followersCount) * (Math.PI / 180.0));
    Vector3 circlePosition = this.FollowerPosition.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    circlePosition.x *= 1.2f;
    circlePosition.y /= 1.2f;
    ++circlePosition.y;
    return circlePosition;
  }

  public IEnumerator ConsumefollowerRoutine(FollowerManager.SpawnedFollower sacraficeFollower)
  {
    RespawnRoomManager respawnRoomManager = this;
    PlayerFarming playerFarming = respawnRoomManager.Player.GetComponent<PlayerFarming>();
    HUD_Manager.Instance.Hide(false);
    respawnRoomManager.generateRoom.RoomTransform.gameObject.SetActive(false);
    sacraficeFollower.FollowerFakeBrain.CompleteCurrentTask();
    FollowerTask_ManualControl Task = new FollowerTask_ManualControl();
    sacraficeFollower.FollowerFakeBrain.HardSwapToTask((FollowerTask) Task);
    respawnRoomManager.followers.Remove(sacraficeFollower);
    yield return (object) new WaitForEndOfFrame();
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    if (DataManager.Instance.First_Dungeon_Resurrecting)
    {
      for (int index = 0; index < respawnRoomManager.followers.Count; ++index)
      {
        FollowerTask_ManualControl nextTask = new FollowerTask_ManualControl();
        Follower f = respawnRoomManager.followers[index].Follower;
        respawnRoomManager.followers[index].FollowerFakeBrain.HardSwapToTask((FollowerTask) nextTask);
        nextTask.GoToAndStop(f, respawnRoomManager.GetCirclePosition(respawnRoomManager.followers.Count, index, 4f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.SpectatorCheer(f))));
      }
      int x = (double) sacraficeFollower.Follower.transform.position.x > (double) playerFarming.transform.position.x ? -1 : 1;
      bool waitForFollower = true;
      playerFarming.GoToAndStop(respawnRoomManager.FollowerPosition.transform.position + new Vector3((float) x, 0.0f, 0.0f), sacraficeFollower.Follower.gameObject, GoToCallback: (System.Action) (() => playerFarming.state.LookAngle = playerFarming.state.facingAngle = Utils.GetAngle(playerFarming.transform.position, this.FollowerPosition.position)));
      Task.GoToAndStop(sacraficeFollower.Follower, respawnRoomManager.FollowerPosition.transform.position + new Vector3((float) -x, 0.0f, 0.0f), (System.Action) (() =>
      {
        sacraficeFollower.Follower.State.LookAngle = sacraficeFollower.Follower.State.facingAngle = Utils.GetAngle(sacraficeFollower.Follower.transform.position, this.FollowerPosition.position);
        waitForFollower = false;
      }));
      while (waitForFollower)
        yield return (object) null;
    }
    foreach (FollowerManager.SpawnedFollower follower in respawnRoomManager.followers)
    {
      if ((UnityEngine.Object) follower.Follower.Spine != (UnityEngine.Object) null)
      {
        double num = (double) follower.Follower.SetBodyAnimation("cheer", true);
      }
    }
    if (DataManager.Instance.First_Dungeon_Resurrecting)
    {
      GameManager.GetInstance().OnConversationNext(sacraficeFollower.Follower.gameObject, 8f);
      GameManager.GetInstance().AddPlayerToCamera();
      yield return (object) new WaitForSeconds(1f);
    }
    GameManager.GetInstance().OnConversationNext(sacraficeFollower.Follower.gameObject, 10f);
    GameManager.GetInstance().AddPlayerToCamera();
    if (DataManager.Instance.First_Dungeon_Resurrecting)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/consume_start", respawnRoomManager.gameObject);
      playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      playerFarming.simpleSpineAnimator.Animate("sacrifice-long", 0, false);
      playerFarming.simpleSpineAnimator.AddAnimate("warp-out-down", 0, true, 0.0f);
      double num = (double) sacraficeFollower.Follower.SetBodyAnimation("sacrifice-long", false);
      sacraficeFollower.Follower.State.LookAngle = sacraficeFollower.Follower.State.facingAngle = Utils.GetAngle(respawnRoomManager.transform.position, playerFarming.transform.position);
      DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      yield return (object) new WaitForSeconds(2f);
      respawnRoomManager.LoopInstance = AudioManager.Instance.CreateLoop("event:/followers/consume_loop", respawnRoomManager.gameObject, true);
      yield return (object) new WaitForSeconds(1f);
      GameManager.GetInstance().CamFollowTarget.targetDistance += 2f;
      respawnRoomManager.StartCoroutine((IEnumerator) respawnRoomManager.SpawnSouls(sacraficeFollower.Follower.transform.position, 0.2f, 0.05f));
      yield return (object) new WaitForSeconds(3f);
      playerFarming.transform.DOMove(respawnRoomManager.FollowerPosition.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    }
    else
    {
      bool waitForPlayer = true;
      playerFarming.GoToAndStop(sacraficeFollower.Follower.transform.position + new Vector3((double) sacraficeFollower.Follower.transform.position.x > (double) playerFarming.transform.position.x ? -1f : 1f, 0.0f, 0.0f) * 2f, sacraficeFollower.Follower.gameObject, GoToCallback: (System.Action) (() =>
      {
        waitForPlayer = false;
        playerFarming.state.LookAngle = playerFarming.state.facingAngle = Utils.GetAngle(playerFarming.transform.position, sacraficeFollower.Follower.transform.position);
      }));
      while (waitForPlayer)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot("event:/followers/consume_start", respawnRoomManager.gameObject);
      playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      float duration = playerFarming.simpleSpineAnimator.Animate("sacrifice-short", 0, false).Animation.Duration;
      playerFarming.simpleSpineAnimator.AddAnimate("warp-out-down", 0, true, 0.0f);
      double num = (double) sacraficeFollower.Follower.SetBodyAnimation("sacrifice", false);
      sacraficeFollower.Follower.State.LookAngle = sacraficeFollower.Follower.State.facingAngle = Utils.GetAngle(playerFarming.transform.position, respawnRoomManager.transform.position);
      DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      yield return (object) new WaitForSeconds((float) (((double) duration - 1.5) / 2.0));
      respawnRoomManager.LoopInstance = AudioManager.Instance.CreateLoop("event:/followers/consume_loop", respawnRoomManager.gameObject, true);
      yield return (object) new WaitForSeconds((float) (((double) duration - 1.5) / 2.0));
      respawnRoomManager.StartCoroutine((IEnumerator) respawnRoomManager.SpawnSouls(sacraficeFollower.Follower.transform.position, 0.025f, 0.015f));
      GameManager.GetInstance().CamFollowTarget.targetDistance += 2f;
    }
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.StopLoop(respawnRoomManager.LoopInstance);
    AudioManager.Instance.PlayOneShot("event:/followers/consume_end", respawnRoomManager.gameObject);
    UnityEngine.Object.Destroy((UnityEngine.Object) sacraficeFollower.Follower.gameObject);
    respawnRoomManager.StartCoroutine((IEnumerator) respawnRoomManager.FollowerConsumed());
    DataManager.Instance.First_Dungeon_Resurrecting = false;
  }

  public IEnumerator SpectatorCheer(Follower follower)
  {
    yield return (object) new WaitForEndOfFrame();
    follower.State.LookAngle = follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.FollowerPosition.transform.position);
    double num = (double) follower.SetBodyAnimation("cheer", true);
  }

  public void PlayRespawn()
  {
    this.generateRoom.RoomTransform.gameObject.SetActive(false);
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = false;
    this.StartCoroutine((IEnumerator) this.FollowerConsumed());
  }

  public IEnumerator FollowerConsumed()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    RespawnRoomManager respawnRoomManager = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      AstarPath.active = respawnRoomManager.astarPath;
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", new System.Action(respawnRoomManager.\u003CFollowerConsumed\u003Eb__39_0));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void Respawn()
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RespawnRoutine());
  }

  public IEnumerator RespawnRoutine()
  {
    RespawnRoomManager respawnRoomManager = this;
    AudioManager.Instance.PlayOneShot("event:/player/resurrect");
    respawnRoomManager.Respawning = true;
    AstarPath.active = respawnRoomManager.astarPath;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    AudioManager.Instance.PlayMusic(BiomeGenerator.Instance.biomeMusicPath);
    AudioManager.Instance.SetMusicRoomID(BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID);
    RespawnRoomManager.Instance = (RespawnRoomManager) null;
    PlayerFarming component1 = respawnRoomManager.Player.GetComponent<PlayerFarming>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.hudHearts != (UnityEngine.Object) null)
    {
      component1.hudHearts.gameObject.SetActive(false);
      component1.hudHearts.playerFarming = (PlayerFarming) null;
      component1.hudHearts = (HUD_Hearts) null;
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) respawnRoomManager.Player.gameObject);
    respawnRoomManager.Player = (GameObject) null;
    PlayerFarming playerFarming = respawnRoomManager.biomeGenerator.Player.GetComponent<PlayerFarming>();
    PlayerFarming.Instance = playerFarming;
    respawnRoomManager.biomeGenerator.gameObject.SetActive(true);
    playerFarming.gameObject.SetActive(true);
    playerFarming.IsKnockedOut = false;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.gameObject.SetActive(true);
      player.hudHearts.gameObject.SetActive(true);
      player.GetComponent<Interaction_CoopRevive>().enabled = false;
    }
    if (CoopManager.CoopActive && PlayerFarming.players.Count > 1)
    {
      PlayerFarming.ResetMainPlayer();
      CoopManager.Instance.RemoveCoopPlayer(PlayerFarming.players[1]);
    }
    StateMachine component2 = playerFarming.GetComponent<StateMachine>();
    component2.CURRENT_STATE = StateMachine.State.Resurrecting;
    component2.LockStateChanges = true;
    yield return (object) null;
    GameManager.GetInstance().CameraSnapToPosition(playerFarming.transform.position);
    if (CoopManager.CoopActive)
    {
      CoopManager.Instance.SpawnCoopPlayer(1, false, playerFarming.health.HP);
      PlayerFarming.PositionAllPlayers(playerFarming.transform.position);
    }
    yield return (object) null;
    respawnRoomManager.LightingOverride.SetActive(false);
    if (CoopManager.CoopActive)
    {
      while (PlayerFarming.players.Count <= 1)
        yield return (object) null;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if (!player.isLamb)
        {
          player.CustomAnimation("Downed/Goat/idle-goat", true);
          player.state.LockStateChanges = true;
        }
      }
    }
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 6f);
    respawnRoomManager.biomeGenerator.CurrentRoom.generateRoom.SetColliderAndUpdatePathfinding();
    respawnRoomManager.biomeGenerator.DoSpawnDemons(true);
    HUD_Manager.Instance.Show(0);
    respawnRoomManager.gameObject.SetActive(false);
    PlayerFarming.ReloadAllFaith();
    PlayerRelic.Reload();
    Interaction_Chest instance = Interaction_Chest.Instance;
    if ((instance != null ? (instance.MyState == Interaction_Chest.State.Hidden ? 1 : 0) : 0) != 0)
      RoomLockController.CloseAll(false);
  }

  public IEnumerator SpawnSouls(Vector3 fromPosition, float startingDelay, float min)
  {
    PlayerFarming playerFarming = this.Player.GetComponent<PlayerFarming>();
    float delay = startingDelay;
    for (int i = 0; i < 30; ++i)
    {
      float time = (float) i / 30f;
      delay = Mathf.Clamp(delay * (1f - this.absorbSoulCurve.Evaluate(time)), min, float.MaxValue);
      SoulCustomTarget.Create(playerFarming.gameObject, fromPosition, Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) this.absorbSoulCurve.Evaluate(time))));
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public void ResetPathFinding() => AstarPath.active = this.astarPath;

  public void OnDisable()
  {
    this.cameraStylizer.enabled = false;
    RespawnRoomManager.RespawnPlayInProgress = false;
    foreach (FollowerManager.SpawnedFollower follower in this.followers)
      FollowerManager.CleanUpCopyFollower(follower);
  }

  public IEnumerator PlayRoutine()
  {
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    RespawnRoomManager.Instance.generateRoom.SetColliderAndUpdatePathfinding();
    RespawnRoomManager.Instance.biomeGenerator.gameObject.SetActive(false);
    RespawnRoomManager.Instance.biomeGenerator.Player.SetActive(false);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.gameObject.SetActive(false);
      player.hudHearts.gameObject.SetActive(false);
    }
    yield return (object) null;
    PlayerFarming.SetResetHealthData(false);
    this.Player = this.PlayerPrefab;
    PlayerFarming playerFarming = this.Player.GetComponent<PlayerFarming>();
    playerFarming.playerRelic.PlayerScaleModifier = PlayerFarming.players[0].playerRelic.PlayerScaleModifier;
    playerFarming.Init();
    this.Player.SetActive(true);
    this.Player.transform.position = this.isCorruptedRoom ? this.CorruptedSpawnPlayerPosition.position : this.FollowerPosition.position;
    if ((UnityEngine.Object) this.GoatShadowController != (UnityEngine.Object) null)
      this.GoatShadowController.SetPlayer(playerFarming);
    this.GoatShadowControllerRed.SetPlayer(playerFarming);
    this.Player.GetComponent<Health>().untouchable = true;
    GameManager.GetInstance().RemoveAllFromCamera();
    yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(playerFarming.gameObject, 8f);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CameraSnapToPosition(this.Player.transform.position);
    GameManager.GetInstance().AddToCamera(playerFarming.gameObject);
    StateMachine state = this.Player.GetComponent<StateMachine>();
    state.facingAngle = 85f;
    state.CURRENT_STATE = StateMachine.State.SpawnIn;
    playerFarming.playerController.SpawnInShowHUD = false;
    this.astarPath = AstarPath.active;
    AstarPath.active = (AstarPath) null;
    this.SetCorruptedRoomGameObjects();
    if (!this.isCorruptedRoom)
      this.SpawnFollowers();
    this.Player.GetComponent<PlayerWeapon>().enabled = false;
    this.Player.GetComponent<PlayerSpells>().enabled = false;
    this.Player.GetComponent<PlayerRelic>().enabled = false;
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.First_Dungeon_Resurrecting)
      this.conversation.enabled = true;
    yield return (object) null;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    while (state.CURRENT_STATE == StateMachine.State.SpawnIn)
      yield return (object) null;
    if (!this.isCorruptedRoom)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
        player.AbortGoTo();
    }
    GameManager.GetInstance().OnConversationEnd(ShowHUD: false);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(playerFarming.gameObject);
    RespawnRoomManager.RespawnPlayInProgress = false;
  }

  public void SetCorruptedRoomGameObjects()
  {
    this.StartCoroutine((IEnumerator) this.SetCorruptedRoomGameObjectsRoutine());
  }

  public IEnumerator SetCorruptedRoomGameObjectsRoutine()
  {
    yield return (object) new WaitForSeconds(0.1f);
    foreach (GameObject gameObject in this.DisableCorrupted)
      gameObject.SetActive(!this.isCorruptedRoom);
    foreach (GameObject gameObject in this.EnableCorrupted)
      gameObject.SetActive(this.isCorruptedRoom);
    if (this.isCorruptedRoom)
      HUD_Manager.Instance.Hide(false);
  }

  [CompilerGenerated]
  public void \u003CFollowerConsumed\u003Eb__39_0()
  {
    GameManager.GetInstance().OnConversationEnd();
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.RespawnRoutine());
  }
}
