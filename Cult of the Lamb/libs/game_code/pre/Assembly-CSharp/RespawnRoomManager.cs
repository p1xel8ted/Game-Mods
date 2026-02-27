// Decompiled with JetBrains decompiler
// Type: RespawnRoomManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
using UnityEngine;

#nullable disable
public class RespawnRoomManager : BaseMonoBehaviour
{
  public static RespawnRoomManager Instance;
  private BiomeGenerator biomeGenerator;
  private GenerateRoom generateRoom;
  private GameObject Player;
  public GameObject PlayerPrefab;
  public GameObject LightingOverride;
  [SerializeField]
  private Interaction_SimpleConversation conversation;
  [SerializeField]
  private AnimationCurve absorbSoulCurve;
  private AstarPath astarPath;
  private Stylizer cameraStylizer;
  public Follower FollowerPrefab;
  public Transform FollowerPosition;
  private List<FollowerManager.SpawnedFollower> followers = new List<FollowerManager.SpawnedFollower>();
  private EventInstance LoopInstance;
  public static float HP;
  public static float SpiritHearts;
  public static float BlueHearts;
  public static float BlackHearts;

  public bool Respawning { get; private set; }

  private void OnEnable()
  {
    RespawnRoomManager.Instance = this;
    this.generateRoom = this.GetComponent<GenerateRoom>();
    this.cameraStylizer = Camera.main.gameObject.GetComponent<Stylizer>();
    if ((UnityEngine.Object) this.cameraStylizer == (UnityEngine.Object) null)
      Debug.Log((object) "Camera null");
    this.cameraStylizer.enabled = true;
  }

  private void OnDestroy() => RespawnRoomManager.Instance = (RespawnRoomManager) null;

  public void Init(BiomeGenerator biomeGenerator) => this.biomeGenerator = biomeGenerator;

  public static void Play()
  {
    Debug.Log((object) "PLAY");
    MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", (System.Action) (() =>
    {
      Time.timeScale = 1f;
      RespawnRoomManager.Instance.gameObject.SetActive(true);
      RespawnRoomManager.Instance.StartCoroutine((IEnumerator) RespawnRoomManager.Instance.PlayRoutine());
    }));
  }

  public void SpawnFollowers()
  {
    this.astarPath = AstarPath.active;
    AstarPath.active = (AstarPath) null;
    List<SimFollower> simFollowerList = FollowerManager.SimFollowersAtLocation(FollowerLocation.Base);
    for (int index = 0; index < simFollowerList.Count; ++index)
    {
      FollowerManager.SpawnedFollower f = FollowerManager.SpawnCopyFollower(simFollowerList[index].Brain._directInfoAccess, this.transform.position, this.FollowerPosition, PlayerFarming.Location);
      this.followers.Add(f);
      f.Follower.GetComponentInChildren<UIFollowerTwitchName>(true).Show();
      f.Follower.transform.position = this.GetCirclePosition(simFollowerList.Count, index);
      f.Follower.Interaction_FollowerInteraction.enabled = false;
      f.FollowerFakeBrain.HardSwapToTask((FollowerTask) new FollowerTask_AwaitConsuming());
      f.Follower.State.LookAngle = f.Follower.State.facingAngle = Utils.GetAngle(f.Follower.transform.position, this.FollowerPosition.transform.position);
      f.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
      double num = (double) f.Follower.SetBodyAnimation("pray", true);
      Interaction_ConsumeFollower interactionConsumeFollower = f.Follower.gameObject.AddComponent<Interaction_ConsumeFollower>();
      interactionConsumeFollower.followerSpine = f.Follower.Spine;
      interactionConsumeFollower.Play(simFollowerList[index].Brain._directInfoAccess, (Action<int, int, int, int>) ((HP, SpiritHearts, BlueHearts, BlackHearts) =>
      {
        RespawnRoomManager.HP = (float) HP;
        RespawnRoomManager.SpiritHearts = (float) SpiritHearts;
        RespawnRoomManager.BlueHearts = (float) BlueHearts;
        RespawnRoomManager.BlackHearts = (float) BlackHearts;
        this.StartCoroutine((IEnumerator) this.ConsumefollowerRoutine(f));
      }));
    }
  }

  private Vector3 GetCirclePosition(int followersCount, int index, float forceTargetDistance = -1f)
  {
    float num = Mathf.Max(Mathf.Clamp((float) (followersCount / 3), 2f, 3.5f), forceTargetDistance);
    float f = (float) ((double) index * (360.0 / (double) followersCount) * (Math.PI / 180.0));
    Vector3 circlePosition = this.FollowerPosition.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
    circlePosition.x *= 1.2f;
    circlePosition.y /= 1.2f;
    ++circlePosition.y;
    return circlePosition;
  }

  private IEnumerator ConsumefollowerRoutine(FollowerManager.SpawnedFollower sacraficeFollower)
  {
    RespawnRoomManager respawnRoomManager = this;
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
      int x = (double) sacraficeFollower.Follower.transform.position.x > (double) PlayerFarming.Instance.transform.position.x ? -1 : 1;
      bool waitForFollower = true;
      PlayerFarming.Instance.GoToAndStop(respawnRoomManager.FollowerPosition.transform.position + new Vector3((float) x, 0.0f, 0.0f), sacraficeFollower.Follower.gameObject, GoToCallback: (System.Action) (() => PlayerFarming.Instance.state.LookAngle = PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, this.FollowerPosition.position)));
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
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.simpleSpineAnimator.Animate("sacrifice-long", 0, false);
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("warp-out-down", 0, true, 0.0f);
      double num = (double) sacraficeFollower.Follower.SetBodyAnimation("sacrifice-long", false);
      sacraficeFollower.Follower.State.LookAngle = sacraficeFollower.Follower.State.facingAngle = Utils.GetAngle(respawnRoomManager.transform.position, PlayerFarming.Instance.transform.position);
      DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InOutSine);
      yield return (object) new WaitForSeconds(2f);
      respawnRoomManager.LoopInstance = AudioManager.Instance.CreateLoop("event:/followers/consume_loop", respawnRoomManager.gameObject, true);
      yield return (object) new WaitForSeconds(1f);
      GameManager.GetInstance().CamFollowTarget.targetDistance += 2f;
      respawnRoomManager.StartCoroutine((IEnumerator) respawnRoomManager.SpawnSouls(sacraficeFollower.Follower.transform.position, 0.2f, 0.05f));
      yield return (object) new WaitForSeconds(3f);
      PlayerFarming.Instance.transform.DOMove(respawnRoomManager.FollowerPosition.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    }
    else
    {
      bool waitForPlayer = true;
      int x1 = (double) sacraficeFollower.Follower.transform.position.x > (double) PlayerFarming.Instance.transform.position.x ? -1 : 1;
      PlayerFarming.Instance.GoToAndStop(sacraficeFollower.Follower.transform.position + new Vector3((float) x1, 0.0f, 0.0f) * 2f, sacraficeFollower.Follower.gameObject, GoToCallback: (System.Action) (() =>
      {
        waitForPlayer = false;
        PlayerFarming.Instance.state.LookAngle = PlayerFarming.Instance.state.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, sacraficeFollower.Follower.transform.position);
      }));
      while (waitForPlayer)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot("event:/followers/consume_start", respawnRoomManager.gameObject);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      float duration = PlayerFarming.Instance.simpleSpineAnimator.Animate("sacrifice-short", 0, false).Animation.Duration;
      PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("warp-out-down", 0, true, 0.0f);
      double num = (double) sacraficeFollower.Follower.SetBodyAnimation("sacrifice", false);
      sacraficeFollower.Follower.State.LookAngle = sacraficeFollower.Follower.State.facingAngle = Utils.GetAngle(PlayerFarming.Instance.transform.position, respawnRoomManager.transform.position);
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

  private IEnumerator SpectatorCheer(Follower follower)
  {
    yield return (object) new WaitForEndOfFrame();
    follower.State.LookAngle = follower.State.facingAngle = Utils.GetAngle(follower.transform.position, this.FollowerPosition.transform.position);
    double num = (double) follower.SetBodyAnimation("cheer", true);
  }

  private IEnumerator FollowerConsumed()
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
      // ISSUE: reference to a compiler-generated method
      MMTransition.Play(MMTransition.TransitionType.ChangeRoom, MMTransition.Effect.BlackFade, MMTransition.NO_SCENE, 1f, "", new System.Action(respawnRoomManager.\u003CFollowerConsumed\u003Eb__26_0));
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

  private IEnumerator RespawnRoutine()
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
    UnityEngine.Object.Destroy((UnityEngine.Object) respawnRoomManager.Player);
    respawnRoomManager.Player = (GameObject) null;
    respawnRoomManager.biomeGenerator.gameObject.SetActive(true);
    respawnRoomManager.biomeGenerator.Player.SetActive(true);
    respawnRoomManager.biomeGenerator.Player.GetComponent<StateMachine>().CURRENT_STATE = StateMachine.State.Resurrecting;
    yield return (object) null;
    GameManager.GetInstance().CameraSnapToPosition(respawnRoomManager.biomeGenerator.Player.transform.position);
    GameManager.GetInstance().AddToCamera(PlayerFarming.Instance.CameraBone);
    yield return (object) null;
    respawnRoomManager.LightingOverride.SetActive(false);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 6f);
    respawnRoomManager.biomeGenerator.CurrentRoom.generateRoom.SetColliderAndUpdatePathfinding();
    respawnRoomManager.biomeGenerator.SpawnDemons();
    HUD_Manager.Instance.Show(0);
    respawnRoomManager.gameObject.SetActive(false);
    FaithAmmo.Reload();
    Interaction_Chest instance = Interaction_Chest.Instance;
    if ((instance != null ? (instance.MyState == Interaction_Chest.State.Hidden ? 1 : 0) : 0) != 0)
      RoomLockController.CloseAll();
  }

  private IEnumerator SpawnSouls(Vector3 fromPosition, float startingDelay, float min)
  {
    float delay = startingDelay;
    for (int i = 0; i < 30; ++i)
    {
      float time = (float) i / 30f;
      delay = Mathf.Clamp(delay * (1f - this.absorbSoulCurve.Evaluate(time)), min, float.MaxValue);
      SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, fromPosition, Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) this.absorbSoulCurve.Evaluate(time))));
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public void ResetPathFinding() => AstarPath.active = this.astarPath;

  private void OnDisable()
  {
    this.cameraStylizer.enabled = false;
    foreach (FollowerManager.SpawnedFollower follower in this.followers)
      FollowerManager.CleanUpCopyFollower(follower);
  }

  private IEnumerator PlayRoutine()
  {
    RespawnRoomManager respawnRoomManager = this;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().CachedCamTargets = new List<CameraFollowTarget.Target>();
    RespawnRoomManager.Instance.generateRoom.SetColliderAndUpdatePathfinding();
    RespawnRoomManager.Instance.biomeGenerator.gameObject.SetActive(false);
    RespawnRoomManager.Instance.biomeGenerator.Player.SetActive(false);
    yield return (object) null;
    HealthPlayer.ResetHealthData = false;
    respawnRoomManager.Player = UnityEngine.Object.Instantiate<GameObject>(respawnRoomManager.PlayerPrefab, respawnRoomManager.FollowerPosition.position, Quaternion.identity, respawnRoomManager.transform);
    GameManager.GetInstance().CameraSnapToPosition(respawnRoomManager.Player.transform.position);
    GameManager.GetInstance().AddToCamera(PlayerFarming.Instance.CameraBone);
    yield return (object) null;
    GameManager.GetInstance().OnConversationNew(false, true);
    GameManager.GetInstance().OnConversationNext(PlayerFarming.Instance.CameraBone, 8f);
    StateMachine component = respawnRoomManager.Player.GetComponent<StateMachine>();
    component.facingAngle = 85f;
    component.CURRENT_STATE = StateMachine.State.SpawnIn;
    PlayerFarming.Instance.playerController.SpawnInShowHUD = false;
    respawnRoomManager.SpawnFollowers();
    yield return (object) new WaitForSeconds(1f);
    if (DataManager.Instance.First_Dungeon_Resurrecting)
      respawnRoomManager.conversation.enabled = true;
    yield return (object) null;
    GameManager.GetInstance().CamFollowTarget.DisablePlayerLook = true;
    yield return (object) new WaitForSeconds(3f);
    yield return (object) null;
  }
}
