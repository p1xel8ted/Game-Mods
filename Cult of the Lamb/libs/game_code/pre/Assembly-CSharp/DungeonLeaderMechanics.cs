// Decompiled with JetBrains decompiler
// Type: DungeonLeaderMechanics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Map;
using MMBiomeGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DungeonLeaderMechanics : BaseMonoBehaviour
{
  [SerializeField]
  private DungeonLeaderMechanics.IntroType introType;
  [SerializeField]
  private Interaction_SimpleConversation conversation;
  [SerializeField]
  private SkeletonAnimation leaderSpine;
  [SerializeField]
  private GameObject cameraTarget;
  [SerializeField]
  private GameObject goopFloorParticle;
  [SerializeField]
  private Material leaderOldMaterial;
  [SerializeField]
  private Material leaderNewMaterial;
  [SerializeField]
  private DungeonLeaderMechanics[] otherLeaders;
  [Space]
  [SerializeField]
  private Vector2 randomTime;
  [SerializeField]
  private ColliderEvents distortionObject;
  [Space]
  [SerializeField]
  private GameObject podiumHighlight;
  [SerializeField]
  private ColliderEvents middleCollider;
  [Space]
  [SerializeField]
  private float maxScreenShake;
  [SerializeField]
  private float shakeDuration;
  [SerializeField]
  private AnimationCurve shakeCurve;
  [SerializeField]
  private AnimationCurve controllerRumbleCurve;
  [SerializeField]
  private bool spawnsFollower;
  [SerializeField]
  private bool hasQuestion;
  [SerializeField]
  private bool hasEnemyRounds;
  [SerializeField]
  private string idle = nameof (idle);
  [SerializeField]
  private string enter = nameof (enter);
  [SerializeField]
  private string talk = nameof (talk);
  [Space]
  [SerializeField]
  private EnemyRoundsBase enemyRounds;
  [Space]
  [SerializeField]
  private bool isCombatFollower;
  [Space]
  [SerializeField]
  private int spawnAmount;
  [SerializeField]
  private DungeonLeaderMechanics.Conversation[] spawnedConvo;
  [Space]
  [SerializeField]
  private DungeonLeaderMechanics.Question question;
  [SerializeField]
  private UnityEvent resultAFinishedCallback;
  [SerializeField]
  private UnityEvent resultBFinishedCallback;
  [Space]
  [SerializeField]
  private bool requireConditions;
  [SerializeField]
  private List<BiomeGenerator.VariableAndCondition> conditionalVariables = new List<BiomeGenerator.VariableAndCondition>();
  [SerializeField]
  private int dungeonNumber = -1;
  [SerializeField]
  private Interaction_SimpleConversation alternateConversation;
  [Space]
  [SerializeField]
  private UnityEvent awakeCallback;
  [SerializeField]
  private UnityEvent callback;
  private bool introStarted;
  private bool waiting;
  private int followersDead;
  private float midCombatTimestamp = -1f;
  private bool addedFakeHealth;
  private bool fadedRed;
  private bool completed;
  private bool playerAnimsPlayed;
  [SerializeField]
  private LightingManagerVolume lightOverride;
  private List<FollowerManager.SpawnedFollower> spawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  private List<Material> modifiedMaterials = new List<Material>();
  private Material followerMaterial;
  private static readonly int LeaderEncounterColorBoost = Shader.PropertyToID("_LeaderEncounterColorBoost");

  private bool endCombat => this.introType == DungeonLeaderMechanics.IntroType.EndCombat;

  private bool midCombat => this.introType == DungeonLeaderMechanics.IntroType.MidCombat;

  private bool beforeCombat => this.introType == DungeonLeaderMechanics.IntroType.BeforeCombat;

  private void Awake()
  {
    this.awakeCallback?.Invoke();
    if ((UnityEngine.Object) this.lightOverride != (UnityEngine.Object) null)
      this.lightOverride.gameObject.SetActive(false);
    if ((bool) (UnityEngine.Object) this.conversation && this.introType != DungeonLeaderMechanics.IntroType.None)
      this.conversation.Interactable = false;
    if ((bool) (UnityEngine.Object) this.distortionObject)
    {
      this.distortionObject.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.DamageEnemies);
      if (UnifyManager.platform != UnifyManager.Platform.Standalone)
        this.distortionObject.Performance = true;
    }
    if ((bool) (UnityEngine.Object) this.middleCollider)
      this.middleCollider.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.PodiumIntro);
    if (this.requireConditions)
    {
      bool flag = true;
      foreach (BiomeGenerator.VariableAndCondition conditionalVariable in this.conditionalVariables)
      {
        if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
          flag = false;
      }
      if (!flag)
        this.gameObject.SetActive(false);
    }
    if (this.dungeonNumber != -1 && (this.dungeonNumber == 1 && DataManager.Instance.ShownDungeon1FinalLeaderEncounter || this.dungeonNumber == 2 && DataManager.Instance.ShownDungeon2FinalLeaderEncounter || this.dungeonNumber == 3 && DataManager.Instance.ShownDungeon3FinalLeaderEncounter || this.dungeonNumber == 4 && DataManager.Instance.ShownDungeon4FinalLeaderEncounter))
      this.gameObject.SetActive(false);
    this.leaderSpine.CustomMaterialOverride.Add(this.leaderOldMaterial, this.leaderNewMaterial);
    PlayerFarming.OnGoToAndStopBegin += new PlayerFarming.GoToEvent(this.GoToAndStopBegin);
  }

  private void Start()
  {
    Transform transform = BiomeGenerator.Instance.CurrentRoom != null ? BiomeGenerator.Instance.CurrentRoom.generateRoom.transform : BiomeGenerator.Instance.transform;
    BreakableSpiderNest[] componentsInChildren1 = transform.GetComponentsInChildren<BreakableSpiderNest>();
    if (componentsInChildren1.Length != 0)
    {
      for (int index = componentsInChildren1.Length - 1; index >= 0; --index)
        componentsInChildren1[index].GetComponent<Health>().DealDamage(float.MaxValue, this.gameObject, this.transform.position);
    }
    SpiderNest[] componentsInChildren2 = transform.GetComponentsInChildren<SpiderNest>();
    if (componentsInChildren2.Length != 0)
    {
      for (int index = componentsInChildren2.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren2[index].gameObject);
    }
    TrapCharger[] componentsInChildren3 = transform.GetComponentsInChildren<TrapCharger>();
    if (componentsInChildren3.Length != 0)
    {
      for (int index = componentsInChildren3.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren3[index].gameObject);
    }
    TrapSpikes[] componentsInChildren4 = transform.GetComponentsInChildren<TrapSpikes>();
    if (componentsInChildren4.Length != 0)
    {
      for (int index = componentsInChildren4.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren4[index].ParentToDestroy);
    }
    TrapProjectileCross[] componentsInChildren5 = transform.GetComponentsInChildren<TrapProjectileCross>();
    if (componentsInChildren5.Length != 0)
    {
      for (int index = componentsInChildren5.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren5[index].gameObject);
    }
    TrapRockFall[] componentsInChildren6 = transform.GetComponentsInChildren<TrapRockFall>();
    if (componentsInChildren6.Length == 0)
      return;
    for (int index = componentsInChildren6.Length - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren6[index].gameObject);
  }

  private void OnEnable()
  {
    if (this.introType == DungeonLeaderMechanics.IntroType.None && !GameManager.RoomActive && (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null && !BiomeGenerator.Instance.CurrentRoom.Completed)
      BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID = SoundConstants.RoomID.CultLeaderAmbience;
    if (this.completed)
      this.Hide();
    if (!(bool) (UnityEngine.Object) this.conversation)
      return;
    this.conversation.OnInteraction += new Interaction.InteractionEvent(this.OnInteraction);
  }

  private void OnDestroy()
  {
    PlayerFarming.OnGoToAndStopBegin -= new PlayerFarming.GoToEvent(this.GoToAndStopBegin);
    foreach (FollowerManager.SpawnedFollower spawnedFollower in this.spawnedFollowers)
    {
      if ((UnityEngine.Object) spawnedFollower.Follower != (UnityEngine.Object) null)
      {
        SimFollower simFollowerById = FollowerManager.FindSimFollowerByID(spawnedFollower.FollowerBrain.Info.ID);
        if (simFollowerById != null)
          FollowerManager.SimFollowersAtLocation(FollowerLocation.Base).Add(simFollowerById);
      }
      FollowerManager.CleanUpCopyFollower(spawnedFollower);
    }
    this.spawnedFollowers.Clear();
  }

  private void OnDisable()
  {
    if ((bool) (UnityEngine.Object) this.conversation)
      this.conversation.OnInteraction -= new Interaction.InteractionEvent(this.OnInteraction);
    if (this.introStarted)
      this.gameObject.SetActive(false);
    foreach (Material modifiedMaterial in this.modifiedMaterials)
      modifiedMaterial.SetColor(UnitObject.LeaderEncounterColorBoost, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    this.modifiedMaterials.Clear();
  }

  public void Hide()
  {
    this.leaderSpine?.gameObject.SetActive(false);
    this.goopFloorParticle?.gameObject.SetActive(false);
    foreach (DungeonLeaderMechanics otherLeader in this.otherLeaders)
    {
      if ((bool) (UnityEngine.Object) otherLeader)
        otherLeader.Hide();
    }
  }

  private void OnInteraction(StateMachine state)
  {
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.SetPlayerAnimations());
  }

  private void GoToAndStopBegin(Vector3 targetPosition)
  {
    if (!this.gameObject.activeInHierarchy || !this.introStarted && !MMConversation.isPlaying)
      return;
    if ((bool) (UnityEngine.Object) this.conversation)
      this.conversation.MovePlayerToListenPosition = false;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "floating-boss-start", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "floating-boss-loop", true, 0.0f);
    PlayerFarming.Instance.IdleOnEnd = false;
    PlayerFarming.Instance.transform.DOMove(targetPosition, 2f).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      PlayerFarming.Instance.EndGoToAndStop();
      PlayerFarming.Instance.IdleOnEnd = true;
      this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "floating-boss-loop", true))));
    }));
    PlayerFarming.OnGoToAndStopBegin -= new PlayerFarming.GoToEvent(this.GoToAndStopBegin);
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    callback();
  }

  private IEnumerator SetPlayerAnimations()
  {
    if (!this.playerAnimsPlayed)
    {
      this.playerAnimsPlayed = true;
      yield return (object) new WaitForEndOfFrame();
      while (PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      PlayerFarming.Instance.state.LockStateChanges = true;
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "floating-boss-loop", true, 0.0f);
      yield return (object) new WaitForSeconds(3.3f);
      while (MMConversation.CURRENT_CONVERSATION != null || !this.conversation.Interactable || this.waiting)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      PlayerFarming.Instance.transform.DOKill();
      PlayerFarming.Instance.simpleSpineAnimator.Animate("floating-boss-land", 0, false);
      PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle-up", true, 0.0f);
      yield return (object) new WaitForSeconds(1.76f);
      if (LetterBox.IsPlaying)
      {
        while (MMConversation.CURRENT_CONVERSATION != null || this.waiting)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.5f);
        while (MMConversation.CURRENT_CONVERSATION != null || this.waiting)
          yield return (object) null;
      }
      PlayerFarming.Instance.EndGoToAndStop();
      PlayerFarming.Instance.state.LockStateChanges = false;
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
    }
  }

  public void EndConversation() => GameManager.GetInstance().OnConversationEnd();

  private void Update()
  {
    if (!this.introStarted && GameManager.RoomActive)
    {
      if (this.introType == DungeonLeaderMechanics.IntroType.EndCombat)
      {
        if (!this.addedFakeHealth && this.introType == DungeonLeaderMechanics.IntroType.EndCombat)
        {
          Health.team2.Add((Health) null);
          Interaction_Chest.Instance?.AddEnemy((Health) null);
          this.addedFakeHealth = true;
        }
        if (Health.team2.Count <= 1)
        {
          this.introStarted = false;
          this.StartCoroutine((IEnumerator) this.DrawnOutIntroIE());
        }
        else
        {
          for (int index = Health.team2.Count - 1; index >= 0; --index)
          {
            if ((UnityEngine.Object) Health.team2[index] == (UnityEngine.Object) null)
              Health.team2.RemoveAt(index);
          }
          Health.team2.Add((Health) null);
        }
      }
      else if (this.introType == DungeonLeaderMechanics.IntroType.MidCombat)
      {
        if ((double) this.midCombatTimestamp == -1.0)
          this.midCombatTimestamp = Time.time + UnityEngine.Random.Range(this.randomTime.x, this.randomTime.y);
        else if ((double) this.midCombatTimestamp != -1.0 && (double) Time.time > (double) this.midCombatTimestamp)
        {
          this.introStarted = false;
          this.StartCoroutine((IEnumerator) this.InstantSpawnIntroIE());
        }
      }
      else if (this.introType == DungeonLeaderMechanics.IntroType.None)
      {
        if (!this.addedFakeHealth)
        {
          Health.team2.Add((Health) null);
          Interaction_Chest.Instance?.AddEnemy((Health) null);
          this.addedFakeHealth = true;
        }
        if (!this.fadedRed)
        {
          KeyboardLightingManager.TransitionAllKeys(Color.white, Color.red, 2f, KeyboardLightingManager.F_KEYS);
          this.fadedRed = true;
          this.FadeRedIn();
        }
      }
    }
    if (GameManager.RoomActive || !((UnityEngine.Object) Interaction_Chest.Instance != (UnityEngine.Object) null) || this.introType != DungeonLeaderMechanics.IntroType.MidCombat)
      return;
    Health.team2.Add((Health) null);
    Interaction_Chest.Instance?.AddEnemy((Health) null);
  }

  private IEnumerator DrawnOutIntroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    AudioManager.Instance.SetMusicCombatState(false);
    dungeonLeaderMechanics.introStarted = true;
    yield return (object) new WaitForSeconds(0.5f);
    List<Health> breakables = Health.neutralTeam;
    bool conversationActive = false;
    KeyboardLightingManager.TransitionAllKeys(Color.white, Color.red, dungeonLeaderMechanics.shakeDuration / 1.5f, KeyboardLightingManager.F_KEYS, Ease.InSine);
    float lastBreakTime = 1f;
    float t = 0.0f;
    while ((double) t < (double) dungeonLeaderMechanics.shakeDuration)
    {
      float time = t / dungeonLeaderMechanics.shakeDuration;
      CameraManager.shakeCamera(dungeonLeaderMechanics.maxScreenShake * dungeonLeaderMechanics.shakeCurve.Evaluate(time));
      dungeonLeaderMechanics.FadeRedIn();
      MMVibrate.RumbleContinuous(dungeonLeaderMechanics.controllerRumbleCurve.Evaluate(time), dungeonLeaderMechanics.controllerRumbleCurve.Evaluate(time) * 2f);
      if ((double) time > 0.60000002384185791 && !conversationActive)
      {
        conversationActive = true;
        GameManager.GetInstance().OnConversationNew(false);
        GameManager.GetInstance().OnConversationNext(dungeonLeaderMechanics.cameraTarget);
        dungeonLeaderMechanics.ShowGoop();
      }
      if ((double) time > 0.75 && !dungeonLeaderMechanics.conversation.Interactable)
      {
        while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
          yield return (object) null;
        foreach (ConversationEntry entry in dungeonLeaderMechanics.conversation.Entries)
        {
          entry.SetZoom = true;
          entry.Zoom = 11f;
        }
        TrapPoison.RemoveAllPoison();
        dungeonLeaderMechanics.conversation.Interactable = true;
        dungeonLeaderMechanics.conversation.OnInteract(PlayerFarming.Instance.state);
        dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.talk, true, 0.0f);
        // ISSUE: reference to a compiler-generated method
        dungeonLeaderMechanics.conversation.Callback.AddListener(new UnityAction(dungeonLeaderMechanics.\u003CDrawnOutIntroIE\u003Eb__67_0));
      }
      if ((double) t > (double) lastBreakTime && (double) UnityEngine.Random.Range(0.0f, 1f) > 0.60000002384185791 && breakables.Count > 0)
      {
        Health health = breakables[UnityEngine.Random.Range(0, breakables.Count)];
        DropLootOnDeath component = health.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component)
          component.LootToDrop = InventoryItem.ITEM_TYPE.NONE;
        health.ImpactOnHit = false;
        health.ScreenshakeOnDie = false;
        health.ScreenshakeOnHit = false;
        health.DealDamage(health.totalHP, dungeonLeaderMechanics.gameObject, dungeonLeaderMechanics.transform.position);
        lastBreakTime = t + UnityEngine.Random.Range(0.0f, 1f);
      }
      t += Time.deltaTime;
      yield return (object) null;
    }
    MMVibrate.StopRumble();
    Health.team2.Clear();
  }

  private IEnumerator InstantSpawnIntroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    KeyboardLightingManager.TransitionAllKeys(Color.white, Color.red, 2f, KeyboardLightingManager.F_KEYS);
    dungeonLeaderMechanics.FadeRedIn();
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    AudioManager.Instance.SetMusicCombatState(false);
    dungeonLeaderMechanics.introStarted = true;
    dungeonLeaderMechanics.ShowGoop();
    yield return (object) new WaitForSeconds(0.75f);
    dungeonLeaderMechanics.SpawnDistortionObject();
    yield return (object) new WaitForSeconds(0.75f);
    dungeonLeaderMechanics.leaderSpine.timeScale = 5f;
    dungeonLeaderMechanics.leaderSpine.AnimationState.SetAnimation(0, dungeonLeaderMechanics.enter, false);
    dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.idle, true, 0.0f);
    yield return (object) new WaitForSeconds(0.75f);
    dungeonLeaderMechanics.leaderSpine.timeScale = 1f;
    TrapPoison.RemoveAllPoison();
    yield return (object) new WaitForSeconds(1f);
    while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
      yield return (object) null;
    if ((bool) (UnityEngine.Object) dungeonLeaderMechanics.conversation)
    {
      foreach (ConversationEntry entry in dungeonLeaderMechanics.conversation.Entries)
      {
        entry.SetZoom = true;
        entry.Zoom = 11f;
      }
      dungeonLeaderMechanics.conversation.Interactable = true;
      dungeonLeaderMechanics.conversation.OnInteract(PlayerFarming.Instance.state);
      dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.talk, true, 0.0f);
      // ISSUE: reference to a compiler-generated method
      dungeonLeaderMechanics.conversation.Callback.AddListener(new UnityAction(dungeonLeaderMechanics.\u003CInstantSpawnIntroIE\u003Eb__68_0));
    }
  }

  private void PodiumIntro(Collider2D collider)
  {
    if (this.introStarted || RoomLockController.DoorsOpen)
      return;
    this.StartCoroutine((IEnumerator) this.PodiumIntroIE());
  }

  private IEnumerator PodiumIntroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    if (!dungeonLeaderMechanics.introStarted && GameManager.RoomActive)
    {
      KeyboardLightingManager.TransitionAllKeys(Color.white, Color.red, 2f, KeyboardLightingManager.F_KEYS);
      dungeonLeaderMechanics.FadeRedIn();
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
      AudioManager.Instance.SetMusicCombatState(false);
      dungeonLeaderMechanics.introStarted = true;
      if ((bool) (UnityEngine.Object) dungeonLeaderMechanics.podiumHighlight)
        dungeonLeaderMechanics.podiumHighlight.gameObject.SetActive(true);
      if ((UnityEngine.Object) dungeonLeaderMechanics.podiumHighlight == (UnityEngine.Object) null && (bool) (UnityEngine.Object) dungeonLeaderMechanics.conversation)
        dungeonLeaderMechanics.middleCollider.transform.position = dungeonLeaderMechanics.conversation.ListenPosition;
      while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null || PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive)
        yield return (object) null;
      dungeonLeaderMechanics.ShowGoop();
      yield return (object) new WaitForSeconds(0.75f);
      dungeonLeaderMechanics.SpawnDistortionObject();
      yield return (object) new WaitForSeconds(0.75f);
      dungeonLeaderMechanics.leaderSpine.timeScale = 5f;
      dungeonLeaderMechanics.leaderSpine.AnimationState.SetAnimation(0, dungeonLeaderMechanics.enter, false);
      dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.idle, true, 0.0f);
      yield return (object) new WaitForSeconds(0.75f);
      dungeonLeaderMechanics.leaderSpine.timeScale = 1f;
      TrapPoison.RemoveAllPoison();
      for (int index = 0; index < dungeonLeaderMechanics.otherLeaders.Length; ++index)
        dungeonLeaderMechanics.SpawnOtherLeader(index);
      yield return (object) new WaitForSeconds((bool) (UnityEngine.Object) dungeonLeaderMechanics.podiumHighlight ? 3f : 1f);
      if ((UnityEngine.Object) dungeonLeaderMechanics.middleCollider != (UnityEngine.Object) null && (PlayerFarming.Instance.Spine.AnimationState.GetCurrent(0).Animation.Name != "floating-boss-start" || PlayerFarming.Instance.Spine.AnimationState.GetCurrent(0).Animation.Name != "floating-boss-loop"))
        dungeonLeaderMechanics.GoToAndStopBegin(dungeonLeaderMechanics.middleCollider.transform.position);
      GameManager.GetInstance().StartCoroutine((IEnumerator) dungeonLeaderMechanics.SetPlayerAnimations());
      if ((bool) (UnityEngine.Object) dungeonLeaderMechanics.conversation)
      {
        foreach (ConversationEntry entry in dungeonLeaderMechanics.conversation.Entries)
        {
          entry.SetZoom = true;
          entry.Zoom = 11f;
        }
        dungeonLeaderMechanics.conversation.Interactable = true;
        dungeonLeaderMechanics.conversation.OnInteract(PlayerFarming.Instance.state);
        dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.talk, true, 0.0f);
        // ISSUE: reference to a compiler-generated method
        dungeonLeaderMechanics.conversation.Callback.AddListener(new UnityAction(dungeonLeaderMechanics.\u003CPodiumIntroIE\u003Eb__70_0));
      }
    }
  }

  private void DamageEnemies(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == Health.Team.PlayerTeam)
      return;
    component.ImpactOnHit = false;
    component.ScreenshakeOnDie = false;
    component.ScreenshakeOnHit = false;
    component.invincible = false;
    component.untouchable = false;
    component.DealDamage(component.totalHP, this.gameObject, this.transform.position, AttackType: Health.AttackTypes.Poison);
  }

  public void SpawnOtherLeader(int index)
  {
    this.otherLeaders[index].StartCoroutine((IEnumerator) this.otherLeaders[index].InstantSpawnIntroIE());
  }

  public void ShowGoop()
  {
    if (!this.goopFloorParticle.gameObject.activeSelf)
    {
      this.goopFloorParticle.gameObject.SetActive(true);
      this.goopFloorParticle.GetComponent<SkeletonAnimation>().AnimationState.AddAnimation(0, "leader-loop", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", this.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/summoned", this.transform.position);
    }
    else
      this.StartCoroutine((IEnumerator) this.HideGoopDelayIE());
  }

  private IEnumerator HideGoopDelayIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      dungeonLeaderMechanics.goopFloorParticle.GetComponent<SimpleSpineDeactivateAfterPlay>().enabled = true;
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", dungeonLeaderMechanics.transform.position);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void FadeRedAway()
  {
    KeyboardLightingManager.StopAll();
    KeyboardLightingManager.TransitionAllKeys(Color.red, Color.white, 1f, KeyboardLightingManager.F_KEYS);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => KeyboardLightingManager.UpdateLocation()));
    GameManager.GetInstance().SetDitherTween(0.0f);
    if ((UnityEngine.Object) this.lightOverride != (UnityEngine.Object) null)
      this.lightOverride.gameObject.SetActive(false);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
    BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID = SoundConstants.RoomID.StandardAmbience;
    if (!(bool) (UnityEngine.Object) Interaction_Chest.Instance)
      return;
    GameManager.GetInstance().RemoveFromCamera(Interaction_Chest.Instance.gameObject);
  }

  private void FadeRedIn()
  {
    if ((UnityEngine.Object) this.lightOverride != (UnityEngine.Object) null)
      this.lightOverride.gameObject.SetActive(true);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID = SoundConstants.RoomID.StandardAmbience;
  }

  private void SpawnDistortionObject()
  {
    if (!(bool) (UnityEngine.Object) this.distortionObject)
      return;
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && Health.team2[index].invincible)
        Health.team2[index].invincible = false;
    }
    this.distortionObject.SetActive(true);
    this.distortionObject.transform.DOScale(25f, 3.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      try
      {
        TrapPoison.RemoveAllPoison();
        UnityEngine.Object.Destroy((UnityEngine.Object) this.distortionObject.gameObject);
      }
      catch
      {
        Debug.Log((object) "Unable to Detroy distortionObject.gameObject");
      }
    }));
    for (int index = EnemySpider.EnemySpiders.Count - 1; index >= 0; --index)
    {
      if ((bool) (UnityEngine.Object) EnemySpider.EnemySpiders[index] && (UnityEngine.Object) EnemySpider.EnemySpiders[index] != (UnityEngine.Object) this)
      {
        SpawnEnemyOnDeath component = EnemySpider.EnemySpiders[index].GetComponent<SpawnEnemyOnDeath>();
        if ((bool) (UnityEngine.Object) component)
          component.Amount = 0;
        EnemySpider.EnemySpiders[index].health.enabled = true;
        EnemySpider.EnemySpiders[index].health.DealDamage(EnemySpider.EnemySpiders[index].health.totalHP, this.gameObject, EnemySpider.EnemySpiders[index].transform.position);
      }
    }
    this.StartCoroutine((IEnumerator) this.KillAll());
  }

  private IEnumerator KillAll()
  {
    yield return (object) new WaitForSeconds(0.25f);
    foreach (Health health in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      {
        health.invincible = false;
        health.enabled = true;
        health.DealDamage(float.PositiveInfinity, health.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
      }
    }
  }

  public void BeginEnemyRounds() => this.StartCoroutine((IEnumerator) this.BeginEnemyRoundsIE());

  private IEnumerator BeginEnemyRoundsIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.callback?.Invoke();
    dungeonLeaderMechanics.completed = true;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    yield return (object) new WaitForSeconds(2f);
    GameManager.GetInstance().SetDitherTween(2f, 3f);
    Debug.Log((object) "Set offering combat music ID");
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.OfferingCombat);
    AudioManager.Instance.SetMusicCombatState();
    while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    // ISSUE: reference to a compiler-generated method
    dungeonLeaderMechanics.enemyRounds.BeginCombat(false, new System.Action(dungeonLeaderMechanics.\u003CBeginEnemyRoundsIE\u003Eb__80_0));
    // ISSUE: reference to a compiler-generated method
    dungeonLeaderMechanics.enemyRounds.OnEnemySpawned += new EnemyRoundsBase.EnemyEvent(dungeonLeaderMechanics.\u003CBeginEnemyRoundsIE\u003Eb__80_1);
  }

  public void Bow() => this.StartCoroutine((IEnumerator) this.BowIE());

  private IEnumerator BowIE()
  {
    this.waiting = true;
    PlayerFarming.Instance.state.LookAngle = 90f;
    PlayerFarming.Instance.state.facingAngle = 90f;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    TrackEntry trackEntry = PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "pray", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle-up", true, 0.0f);
    yield return (object) new WaitForSeconds(trackEntry.Animation.Duration);
    CultFaithManager.AddThought(Thought.Cult_LostRespect);
    this.waiting = false;
  }

  public void AskQuestion() => this.StartCoroutine((IEnumerator) this.AskQuestionIE());

  private IEnumerator AskQuestionIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    while (MMConversation.isPlaying)
      yield return (object) null;
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(dungeonLeaderMechanics.gameObject, dungeonLeaderMechanics.question.Line1)
    };
    Entries[0].CharacterName = dungeonLeaderMechanics.question.CharacterName;
    Entries[0].Offset = new Vector3(0.0f, 3f, 0.0f);
    Entries[0].SetZoom = true;
    Entries[0].Zoom = 12f;
    if (dungeonLeaderMechanics.question.CharacterName == "NAMES/CultLeaders/Dungeon2")
      Entries[0].soundPath = "event:/dialogue/dun2_cult_leader_heket/standard_heket";
    else if (dungeonLeaderMechanics.question.CharacterName == "NAMES/CultLeaders/Dungeon4")
      Entries[0].soundPath = "event:/dialogue/dun4_cult_leader_shamura/standard_shamura";
    // ISSUE: reference to a compiler-generated method
    // ISSUE: reference to a compiler-generated method
    List<MMTools.Response> Responses = new List<MMTools.Response>()
    {
      new MMTools.Response(dungeonLeaderMechanics.question.AnswerA, new System.Action(dungeonLeaderMechanics.\u003CAskQuestionIE\u003Eb__84_0), dungeonLeaderMechanics.question.AnswerA),
      new MMTools.Response(dungeonLeaderMechanics.question.AnswerB, new System.Action(dungeonLeaderMechanics.\u003CAskQuestionIE\u003Eb__84_1), dungeonLeaderMechanics.question.AnswerB)
    };
    MMConversation.Play(new ConversationObject(Entries, Responses, (System.Action) null), false);
    yield return (object) new WaitForEndOfFrame();
  }

  private IEnumerator ResponseIE(bool responseWasA, DungeonLeaderMechanics.Question question)
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    yield return (object) null;
    if (responseWasA)
      yield return (object) dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.ACallbackIE());
    else
      yield return (object) dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.BCallbackIE());
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(dungeonLeaderMechanics.gameObject, responseWasA ? question.ResultA : question.ResultB)
    };
    Entries[0].CharacterName = question.CharacterName;
    Entries[0].Offset = new Vector3(0.0f, 3f, 0.0f);
    Entries[0].SetZoom = true;
    Entries[0].Zoom = 12f;
    if (question.CharacterName == "NAMES/CultLeaders/Dungeon2")
      Entries[0].soundPath = "event:/dialogue/dun2_cult_leader_heket/standard_heket";
    else if (question.CharacterName == "NAMES/CultLeaders/Dungeon4")
      Entries[0].soundPath = "event:/dialogue/dun4_cult_leader_shamura/standard_shamura";
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false);
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    if (responseWasA)
      dungeonLeaderMechanics.resultAFinishedCallback?.Invoke();
    else
      dungeonLeaderMechanics.resultBFinishedCallback?.Invoke();
  }

  private IEnumerator ACallbackIE()
  {
    UnityEvent[] unityEventArray = this.question.ResultACallbacks;
    for (int index = 0; index < unityEventArray.Length; ++index)
    {
      unityEventArray[index]?.Invoke();
      while (this.waiting)
        yield return (object) null;
    }
    unityEventArray = (UnityEvent[]) null;
  }

  private IEnumerator BCallbackIE()
  {
    UnityEvent[] unityEventArray = this.question.ResultBCallbacks;
    for (int index = 0; index < unityEventArray.Length; ++index)
    {
      unityEventArray[index]?.Invoke();
      while (this.waiting)
        yield return (object) null;
    }
    unityEventArray = (UnityEvent[]) null;
  }

  public void ConvertAllMapNodes()
  {
    this.StartCoroutine((IEnumerator) this.ConvertAllMapNodesIE());
  }

  private IEnumerator ConvertAllMapNodesIE()
  {
    if (MapManager.Instance.CurrentLayer < MapGenerator.Nodes.Count - 1 || MapGenerator.Nodes.Count == 0)
    {
      UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
      while (adventureMapOverlayController.IsShowing)
        yield return (object) null;
      yield return (object) adventureMapOverlayController.ConvertAllNodesToCombatNodes();
      MapManager.Instance.CloseMap();
      while (adventureMapOverlayController.IsHiding)
        yield return (object) null;
      RoomLockController.RoomCompleted();
      this.FadeRedAway();
      this.callback?.Invoke();
      this.completed = true;
      adventureMapOverlayController = (UIAdventureMapOverlayController) null;
    }
    else
      this.BeginEnemyRounds();
  }

  public void ConvertMiniBossNodeToBossNode()
  {
    this.StartCoroutine((IEnumerator) this.ConvertMiniBossNodeToBossNodeIE());
  }

  private IEnumerator ConvertMiniBossNodeToBossNodeIE()
  {
    UIAdventureMapOverlayController adventureMapOverlayController = MapManager.Instance.ShowMap(true);
    while (adventureMapOverlayController.IsShowing)
      yield return (object) null;
    yield return (object) adventureMapOverlayController.ConvertMiniBossNodeToBossNode();
    MapManager.Instance.CloseMap();
    while (adventureMapOverlayController.IsHiding)
      yield return (object) null;
    this.FadeRedAway();
    this.callback?.Invoke();
  }

  public void SpawnInCombatFollower()
  {
    this.StartCoroutine((IEnumerator) this.SpawnInCombatFollowerIE());
  }

  private IEnumerator SpawnInCombatFollowerIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    if ((bool) (UnityEngine.Object) dungeonLeaderMechanics.distortionObject)
      dungeonLeaderMechanics.distortionObject.gameObject.SetActive(false);
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(follower.ID))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      Vector3[] spawnPositions = new Vector3[2]
      {
        dungeonLeaderMechanics.transform.position + Vector3.right * 2f,
        dungeonLeaderMechanics.transform.position - Vector3.right * 2f
      };
      for (int i = 0; i < dungeonLeaderMechanics.spawnAmount && possibleFollowers.Count != 0; ++i)
      {
        FollowerInfo followerInfo = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
        FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(followerInfo, spawnPositions[i]);
        FollowerManager.RetireSimFollowerByID(spawnedFollower.FollowerBrain.Info.ID);
        Health component = spawnedFollower.Follower.GetComponent<Health>();
        if ((bool) (UnityEngine.Object) component)
          component.invincible = true;
        spawnedFollower.Follower.OverridingEmotions = true;
        dungeonLeaderMechanics.SetFollowerOutfit(spawnedFollower, Thought.Dissenter);
        dungeonLeaderMechanics.spawnedFollowers.Add(spawnedFollower);
        possibleFollowers.Remove(followerInfo);
        GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 5f);
        yield return (object) new WaitForSeconds(1f);
      }
      List<ConversationEntry> Entries = new List<ConversationEntry>();
      for (int index = 0; index < dungeonLeaderMechanics.spawnedConvo.Length; ++index)
      {
        Entries.Add(new ConversationEntry(dungeonLeaderMechanics.spawnedFollowers[0].Follower.gameObject, dungeonLeaderMechanics.spawnedConvo[index].Line));
        Entries[index].CharacterName = dungeonLeaderMechanics.spawnedFollowers[0].FollowerBrain.Info.Name;
        Entries[index].Offset = new Vector3(0.0f, 1f, 0.0f);
        Entries[index].SetZoom = true;
        Entries[index].Zoom = 5f;
      }
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
      while (MMConversation.CURRENT_CONVERSATION != null)
        yield return (object) null;
      dungeonLeaderMechanics.waiting = false;
      while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
        yield return (object) null;
      if ((UnityEngine.Object) dungeonLeaderMechanics.spawnedFollowers[0].Follower != (UnityEngine.Object) null)
        dungeonLeaderMechanics.followerMaterial = dungeonLeaderMechanics.spawnedFollowers[0].Follower.Spine.gameObject.GetComponent<MeshRenderer>().sharedMaterial;
      foreach (FollowerManager.SpawnedFollower spawnedFollower in dungeonLeaderMechanics.spawnedFollowers)
      {
        spawnedFollower.Follower.GetComponent<EnemyFollower>().enabled = true;
        spawnedFollower.Follower.GetComponent<Health>().invincible = false;
        spawnedFollower.Follower.GetComponent<Health>().OnDie += new Health.DieAction(dungeonLeaderMechanics.FollowerDied);
      }
      GameManager.GetInstance().OnConversationEnd();
      dungeonLeaderMechanics.callback?.Invoke();
      dungeonLeaderMechanics.completed = true;
      spawnPositions = (Vector3[]) null;
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  private void SetFollowerOutfit(
    FollowerManager.SpawnedFollower spawnedFollower,
    Thought cursedState)
  {
    Skin newSkin = new Skin("New Skin");
    Skin skin = spawnedFollower.Follower.Spine.Skeleton.Data.FindSkin(spawnedFollower.FollowerBrain._directInfoAccess.SkinName);
    if (skin != null)
    {
      newSkin.AddSkin(skin);
    }
    else
    {
      newSkin.AddSkin(spawnedFollower.Follower.Spine.Skeleton.Data.FindSkin("Cat"));
      spawnedFollower.FollowerBrain._directInfoAccess.SkinName = "Cat";
    }
    string outfitSkinName = spawnedFollower.Follower.Outfit.GetOutfitSkinName(spawnedFollower.FollowerBrain._directInfoAccess.Outfit);
    if (!string.IsNullOrEmpty(outfitSkinName))
      newSkin.AddSkin(spawnedFollower.Follower.Spine.skeleton.Data.FindSkin(outfitSkinName));
    if (cursedState == Thought.Dissenter)
    {
      newSkin.AddSkin(spawnedFollower.Follower.Spine.skeleton.Data.FindSkin("Other/Dissenter"));
      spawnedFollower.Follower.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-dissenter", true);
    }
    spawnedFollower.Follower.Spine.Skeleton.SetSkin(newSkin);
    spawnedFollower.Follower.Spine.skeleton.SetSlotsToSetupPose();
    WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(spawnedFollower.FollowerBrain._directInfoAccess.SkinName);
    if (colourData == null)
      return;
    foreach (WorshipperData.SlotAndColor slotAndColour in colourData.SlotAndColours[Mathf.Clamp(spawnedFollower.FollowerBrain._directInfoAccess.SkinColour, 0, colourData.SlotAndColours.Count - 1)].SlotAndColours)
    {
      Slot slot = spawnedFollower.Follower.Spine.skeleton.FindSlot(slotAndColour.Slot);
      if (slot != null)
        slot.SetColor(slotAndColour.color);
    }
  }

  private void FollowerDied(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    foreach (FollowerManager.SpawnedFollower spawnedFollower in this.spawnedFollowers)
    {
      if ((UnityEngine.Object) spawnedFollower.Follower != (UnityEngine.Object) null && (UnityEngine.Object) spawnedFollower.Follower.GetComponent<Health>() == (UnityEngine.Object) Victim)
      {
        FollowerBrain.AllBrains.Add(spawnedFollower.FollowerBrain);
        FollowerManager.FollowerDie(spawnedFollower.FollowerBrain.Info.ID, NotificationCentre.NotificationType.Died);
        break;
      }
    }
    ++this.followersDead;
    if (this.followersDead < this.spawnAmount)
      return;
    if ((UnityEngine.Object) this.followerMaterial != (UnityEngine.Object) null)
      this.followerMaterial.SetColor(DungeonLeaderMechanics.LeaderEncounterColorBoost, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    this.FadeRedAway();
    Interaction_Chest.Instance?.Reveal();
    RoomLockController.RoomCompleted(true);
  }

  public void MakeRandomFollowerIll()
  {
    this.StartCoroutine((IEnumerator) this.MakeRandomFollowerIllIE());
  }

  private IEnumerator MakeRandomFollowerIllIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Illness <= 0.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(follower.ID))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)], dungeonLeaderMechanics.transform.position + Vector3.up * -1f);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 12f);
      yield return (object) new WaitForSeconds(0.5f);
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(spawnedFollower.Follower.gameObject, "Conversation_NPC/Story/Dungeon3/Leader1/3")
      }, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      yield return (object) dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.MakeFollowerIll(spawnedFollower, 0.9f));
      GameManager.GetInstance().OnConversationEnd();
      dungeonLeaderMechanics.callback?.Invoke();
      dungeonLeaderMechanics.completed = true;
      RoomLockController.RoomCompleted();
      dungeonLeaderMechanics.FadeRedAway();
      spawnedFollower = new FollowerManager.SpawnedFollower();
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  private IEnumerator MakeFollowerIll(FollowerManager.SpawnedFollower spawnedFollower, float delay)
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-feared", false, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Conversations/idle-hate", false, 0.0f);
    yield return (object) new WaitForSeconds(delay);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i < UnityEngine.Random.Range(4, 7); ++i)
    {
      SoulCustomTarget.Create(spawnedFollower.Follower.gameObject, dungeonLeaderMechanics.transform.position + new Vector3(0.0f, 0.0f, -0.5f) + UnityEngine.Random.insideUnitSphere * 0.5f, Color.green, (System.Action) null, 0.2f, 100f);
      yield return (object) new WaitForSeconds(0.05f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    float offset = UnityEngine.Random.Range(0.0f, 0.3f);
    TrackEntry e = spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "Sick/chunder", false);
    e.TrackTime = offset;
    TrackEntry track = spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "spawn-out-angry", false, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    spawnedFollower.FollowerBrain.MakeSick();
    DataManager.Instance.LastFollowerToBecomeIll = -1f;
    spawnedFollower.Follower.ShowAllFollowerIcons();
    FollowerExhaustionWarning componentInChildren1 = spawnedFollower.Follower.GetComponentInChildren<FollowerExhaustionWarning>(true);
    if ((UnityEngine.Object) componentInChildren1 != (UnityEngine.Object) null)
      componentInChildren1.Hide();
    FollowerReeducationWarning componentInChildren2 = spawnedFollower.Follower.GetComponentInChildren<FollowerReeducationWarning>(true);
    if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
      componentInChildren2.Hide();
    FollowerStarvingWArning componentInChildren3 = spawnedFollower.Follower.GetComponentInChildren<FollowerStarvingWArning>(true);
    if ((bool) (UnityEngine.Object) componentInChildren3)
      componentInChildren3.Hide();
    FollowerIllnessWarning followerIllnessWarning = spawnedFollower.Follower.GetComponentInChildren<FollowerIllnessWarning>(true);
    if ((UnityEngine.Object) followerIllnessWarning != (UnityEngine.Object) null)
      followerIllnessWarning.Show();
    yield return (object) new WaitForSeconds((float) ((double) e.Animation.Duration - (double) offset - 0.5));
    if ((UnityEngine.Object) followerIllnessWarning != (UnityEngine.Object) null)
      followerIllnessWarning.Hide();
    yield return (object) new WaitForSeconds(track.Animation.Duration);
    spawnedFollower.Follower.gameObject.SetActive(false);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
    dungeonLeaderMechanics.waiting = false;
  }

  public void MakeAllFollowersIll()
  {
    this.StartCoroutine((IEnumerator) this.MakeAllFollowersIllIE());
  }

  private IEnumerator MakeAllFollowersIllIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Illness <= 0.0 && follower.CursedState == Thought.None && possibleFollowers.Count < 6 && !FollowerManager.FollowerLocked(follower.ID))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      int total = Mathf.Min(possibleFollowers.Count, 5);
      float angleBetween = 180f / (float) (total - 1);
      float startingAngle = -180f;
      double num = (double) Mathf.Max((float) Mathf.Clamp(total / 2, 2, 4), 3f);
      float delay = 1f / (float) possibleFollowers.Count;
      for (int i = 0; i < total; ++i)
      {
        FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(possibleFollowers[i], dungeonLeaderMechanics.transform.position + Vector3.right * 2f);
        GameManager.GetInstance().AddToCamera(spawnedFollower.Follower.gameObject);
        Vector3 fromPosition = dungeonLeaderMechanics.transform.position + (Vector3) Utils.DegreeToVector2(startingAngle) * 3.5f;
        spawnedFollower.Follower.transform.position = fromPosition;
        spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle = Utils.GetAngle(fromPosition, dungeonLeaderMechanics.transform.position);
        dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.MakeFollowerIll(spawnedFollower, (float) (1.0 - (double) delay * (double) (i + 1) + 0.5)));
        startingAngle = Mathf.Repeat(startingAngle + angleBetween, 360f);
        yield return (object) new WaitForSeconds(delay);
      }
      yield return (object) new WaitForSeconds(5f);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().OnConversationEnd();
      dungeonLeaderMechanics.callback?.Invoke();
      dungeonLeaderMechanics.completed = true;
      RoomLockController.RoomCompleted();
      dungeonLeaderMechanics.FadeRedAway();
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  public void MakeRandomFollowerStarving()
  {
    this.StartCoroutine((IEnumerator) this.MakeRandomFollowerStarvingIE());
  }

  private IEnumerator MakeRandomFollowerStarvingIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Satiation > 30.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(follower.ID) && !FollowerBrain.GetOrCreateBrain(follower).HasTrait(FollowerTrait.TraitType.DontStarve))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)], dungeonLeaderMechanics.transform.position + Vector3.up * -1f);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 12f);
      yield return (object) new WaitForSeconds(0.5f);
      MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
      {
        new ConversationEntry(spawnedFollower.Follower.gameObject, "Conversation_NPC/Story/Dungeon2/Leader1/3")
      }, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      yield return (object) dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.MakeFollowerStarving(spawnedFollower, 0.9f));
      GameManager.GetInstance().OnConversationEnd();
      dungeonLeaderMechanics.callback?.Invoke();
      dungeonLeaderMechanics.completed = true;
      RoomLockController.RoomCompleted();
      dungeonLeaderMechanics.FadeRedAway();
      spawnedFollower = new FollowerManager.SpawnedFollower();
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  private IEnumerator MakeFollowerStarving(
    FollowerManager.SpawnedFollower spawnedFollower,
    float delay)
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-feared", false, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Conversations/idle-hate", false, 0.0f);
    yield return (object) new WaitForSeconds(delay);
    float offset = UnityEngine.Random.Range(0.0f, 0.3f);
    TrackEntry e = spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "Hungry/get-hungry", false);
    e.TrackTime = offset;
    TrackEntry track = spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "spawn-out-angry", false, 0.0f);
    for (int i = 0; i < UnityEngine.Random.Range(4, 7); ++i)
    {
      SoulCustomTarget.Create(dungeonLeaderMechanics.gameObject, spawnedFollower.Follower.transform.position + new Vector3(0.0f, 0.0f, -0.5f) + UnityEngine.Random.insideUnitSphere * 0.5f, Color.red, (System.Action) null, 0.2f, 100f);
      yield return (object) new WaitForSeconds(0.05f);
    }
    NotificationCentre.NotificationsEnabled = false;
    spawnedFollower.FollowerBrain.MakeStarve();
    spawnedFollower.Follower.ShowAllFollowerIcons();
    spawnedFollower.Follower.ShowAllFollowerIcons();
    FollowerExhaustionWarning componentInChildren1 = spawnedFollower.Follower.GetComponentInChildren<FollowerExhaustionWarning>(true);
    if ((UnityEngine.Object) componentInChildren1 != (UnityEngine.Object) null)
      componentInChildren1.Hide();
    FollowerReeducationWarning componentInChildren2 = spawnedFollower.Follower.GetComponentInChildren<FollowerReeducationWarning>(true);
    if ((UnityEngine.Object) componentInChildren2 != (UnityEngine.Object) null)
      componentInChildren2.Hide();
    FollowerIllnessWarning componentInChildren3 = spawnedFollower.Follower.GetComponentInChildren<FollowerIllnessWarning>(true);
    if ((bool) (UnityEngine.Object) componentInChildren3)
      componentInChildren3.Hide();
    FollowerStarvingWArning followerStarvingWArning = spawnedFollower.Follower.GetComponentInChildren<FollowerStarvingWArning>(true);
    if ((bool) (UnityEngine.Object) followerStarvingWArning)
      followerStarvingWArning.Show();
    NotificationCentre.NotificationsEnabled = true;
    NotificationCentre.Instance.PlayFollowerNotification(NotificationCentre.NotificationType.Starving, spawnedFollower.FollowerBrain.Info, NotificationFollower.Animation.Unhappy);
    yield return (object) new WaitForSeconds((float) ((double) e.Animation.Duration - (double) offset - 0.5));
    if ((bool) (UnityEngine.Object) followerStarvingWArning)
      followerStarvingWArning.Hide();
    yield return (object) new WaitForSeconds(track.Animation.Duration);
    spawnedFollower.Follower.gameObject.SetActive(false);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
    dungeonLeaderMechanics.waiting = false;
  }

  public void MakeAllFollowersStarving()
  {
    this.StartCoroutine((IEnumerator) this.MakeAllFollowersStarvingIE());
  }

  private IEnumerator MakeAllFollowersStarvingIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Satiation > 25.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(follower.ID) && !FollowerBrain.GetOrCreateBrain(follower).HasTrait(FollowerTrait.TraitType.DontStarve))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      int total = Mathf.Min(possibleFollowers.Count, 5);
      float angleBetween = 180f / (float) (total - 1);
      float startingAngle = -180f;
      float distanceBetween = Mathf.Max((float) Mathf.Clamp(total / 2, 2, 4), 3f);
      float delay = 1f / (float) possibleFollowers.Count;
      for (int i = 0; i < total; ++i)
      {
        FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(possibleFollowers[i], dungeonLeaderMechanics.transform.position + Vector3.right * 2f);
        GameManager.GetInstance().AddToCamera(spawnedFollower.Follower.gameObject);
        Vector3 fromPosition = dungeonLeaderMechanics.transform.position + (Vector3) Utils.DegreeToVector2(startingAngle) * distanceBetween;
        --fromPosition.y;
        spawnedFollower.Follower.transform.position = fromPosition;
        spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle = Utils.GetAngle(fromPosition, dungeonLeaderMechanics.transform.position);
        dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.MakeFollowerStarving(spawnedFollower, (float) (1.0 - (double) delay * (double) (i + 1) + 0.5)));
        startingAngle = Mathf.Repeat(startingAngle + angleBetween, 360f);
        yield return (object) new WaitForSeconds(delay);
      }
      yield return (object) new WaitForSeconds(5f);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().OnConversationEnd();
      dungeonLeaderMechanics.callback?.Invoke();
      dungeonLeaderMechanics.completed = true;
      RoomLockController.RoomCompleted();
      dungeonLeaderMechanics.FadeRedAway();
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  private FollowerManager.SpawnedFollower SpawnFollower(FollowerInfo followerInfo, Vector3 position)
  {
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(this.isCombatFollower ? FollowerManager.CombatFollowerPrefab : FollowerManager.FollowerPrefab, followerInfo, position, this.transform.parent, BiomeGenerator.Instance.DungeonLocation);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-in", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-worried1", true, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Conversations/idle-hate", true, 0.0f);
    return spawnedFollower;
  }

  public void AlternateDungeon1Leader3()
  {
    if (!(bool) (UnityEngine.Object) this.alternateConversation || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_2) && !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3))
      return;
    for (int index = this.otherLeaders.Length - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.otherLeaders[index].gameObject);
    this.otherLeaders = new DungeonLeaderMechanics[0];
    UnityEngine.Object.Destroy((UnityEngine.Object) this.conversation);
    this.alternateConversation.gameObject.SetActive(true);
    this.conversation = this.alternateConversation;
    this.podiumHighlight = (GameObject) null;
  }

  public void AlternateDungeon2Leader1()
  {
    if (!(bool) (UnityEngine.Object) this.alternateConversation || DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.conversation);
    this.alternateConversation.gameObject.SetActive(true);
    this.conversation = this.alternateConversation;
    this.podiumHighlight = (GameObject) null;
  }

  public void AlternateDungeon2Leader2()
  {
    if (!(bool) (UnityEngine.Object) this.alternateConversation || !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_3) && !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4) && DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      return;
    for (int index = this.otherLeaders.Length - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.otherLeaders[index].gameObject);
    this.otherLeaders = new DungeonLeaderMechanics[0];
    UnityEngine.Object.Destroy((UnityEngine.Object) this.conversation);
    this.alternateConversation.gameObject.SetActive(true);
    this.conversation = this.alternateConversation;
    this.podiumHighlight = (GameObject) null;
  }

  public void AlternateDungeon2Leader4()
  {
    if (!(bool) (UnityEngine.Object) this.alternateConversation || DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.conversation);
    this.alternateConversation.gameObject.SetActive(true);
    this.conversation = this.alternateConversation;
    this.podiumHighlight = (GameObject) null;
  }

  public void AlternateDungeon2Leader5()
  {
    if (DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_1))
      return;
    this.conversation.Entries.RemoveAt(0);
  }

  public void AlternateDungeon3Leader3()
  {
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
      return;
    this.conversation.Entries.RemoveAt(2);
  }

  public void AlternateDungeon3Leader4()
  {
    if (!DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_4))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void Dungeon2Encounter1()
  {
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Satiation > 30.0 && follower.CursedState == Thought.None)
        followerInfoList.Add(follower);
    }
    if (followerInfoList.Count <= 0)
      return;
    this.conversation.Entries.Add(new ConversationEntry(this.cameraTarget, "Conversation_NPC/Story/Dungeon2/Leader1/2")
    {
      CharacterName = ScriptLocalization.NAMES_CultLeaders.Dungeon2
    });
  }

  public void Dungeon2Encounter3()
  {
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Satiation > 30.0 && follower.CursedState == Thought.None)
        followerInfoList.Add(follower);
    }
    if (followerInfoList.Count <= 0)
      return;
    this.conversation.Entries.Add(new ConversationEntry(this.cameraTarget, "Conversation_NPC/Story/Dungeon2/Leader3/2")
    {
      CharacterName = ScriptLocalization.NAMES_CultLeaders.Dungeon2
    });
  }

  public void Dungeon3Encounter1()
  {
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Illness <= 0.0 && follower.CursedState == Thought.None)
        followerInfoList.Add(follower);
    }
    if (followerInfoList.Count <= 0)
      return;
    this.conversation.Entries.Add(new ConversationEntry(this.cameraTarget, "Conversation_NPC/Story/Dungeon3/Leader1/2")
    {
      CharacterName = ScriptLocalization.NAMES_CultLeaders.Dungeon3
    });
  }

  public void Dungeon3Encounter2()
  {
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Illness <= 0.0 && follower.CursedState == Thought.None)
        followerInfoList.Add(follower);
    }
    if (followerInfoList.Count <= 0)
      return;
    this.conversation.Entries.Add(new ConversationEntry(this.cameraTarget, "Conversation_NPC/Story/Dungeon3/Leader2/1")
    {
      CharacterName = ScriptLocalization.NAMES_CultLeaders.Dungeon3
    });
  }

  public void Dungeon4Encounter1()
  {
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.CursedState == Thought.None)
        followerInfoList.Add(follower);
    }
    if (followerInfoList.Count <= 0)
      return;
    this.conversation.Entries.Add(new ConversationEntry(this.cameraTarget, "Conversation_NPC/Story/Dungeon4/Leader1/3")
    {
      CharacterName = ScriptLocalization.NAMES_CultLeaders.Dungeon4
    });
  }

  public void ShownFinalLeaderEncounter()
  {
    if (this.dungeonNumber == 1)
      DataManager.Instance.ShownDungeon1FinalLeaderEncounter = true;
    else if (this.dungeonNumber == 2)
      DataManager.Instance.ShownDungeon2FinalLeaderEncounter = true;
    else if (this.dungeonNumber == 3)
    {
      DataManager.Instance.ShownDungeon3FinalLeaderEncounter = true;
    }
    else
    {
      if (this.dungeonNumber != 4)
        return;
      DataManager.Instance.ShownDungeon4FinalLeaderEncounter = true;
    }
  }

  [Serializable]
  private enum IntroType
  {
    None,
    EndCombat,
    MidCombat,
    BeforeCombat,
  }

  [Serializable]
  private struct Conversation
  {
    public int SpawnedCharacterIndex;
    public string Line;
  }

  [Serializable]
  private struct Question
  {
    [TermsPopup("")]
    public string CharacterName;
    [TermsPopup("")]
    public string Line1;
    [Space]
    [TermsPopup("")]
    public string AnswerA;
    [TermsPopup("")]
    public string ResultA;
    public UnityEvent[] ResultACallbacks;
    [Space]
    [TermsPopup("")]
    public string AnswerB;
    [TermsPopup("")]
    public string ResultB;
    public UnityEvent[] ResultBCallbacks;
  }
}
