// Decompiled with JetBrains decompiler
// Type: DungeonLeaderMechanics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class DungeonLeaderMechanics : BaseMonoBehaviour
{
  public static DungeonLeaderMechanics Instance;
  [SerializeField]
  public DungeonLeaderMechanics.IntroType introType;
  [SerializeField]
  public Interaction_SimpleConversation conversation;
  [SerializeField]
  public SkeletonAnimation leaderSpine;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public GameObject goopFloorParticle;
  [SerializeField]
  public Material leaderOldMaterial;
  [SerializeField]
  public Material leaderNewMaterial;
  [SerializeField]
  public DungeonLeaderMechanics[] otherLeaders;
  [Space]
  [SerializeField]
  public Vector2 randomTime;
  [SerializeField]
  public ColliderEvents distortionObject;
  [Space]
  [SerializeField]
  public GameObject podiumHighlight;
  [SerializeField]
  public ColliderEvents middleCollider;
  [Space]
  [SerializeField]
  public float maxScreenShake;
  [SerializeField]
  public float shakeDuration;
  [SerializeField]
  public AnimationCurve shakeCurve;
  [SerializeField]
  public AnimationCurve controllerRumbleCurve;
  [SerializeField]
  public bool spawnsFollower;
  [SerializeField]
  public bool hasQuestion;
  [SerializeField]
  public bool hasEnemyRounds;
  [SerializeField]
  public bool hasTraps;
  [SerializeField]
  public string idle = nameof (idle);
  [SerializeField]
  public string enter = nameof (enter);
  [SerializeField]
  public string talk = nameof (talk);
  [Space]
  [SerializeField]
  public EnemyRoundsBase enemyRounds;
  [Space]
  [SerializeField]
  public bool isCombatFollower;
  [SerializeField]
  public bool isRottingFollower;
  [SerializeField]
  public Vector3 followerSpawnOffset;
  [Space]
  [SerializeField]
  public int spawnAmount;
  [SerializeField]
  public DungeonLeaderMechanics.Conversation[] spawnedConvo;
  [Space]
  [SerializeField]
  public DungeonLeaderMechanics.Question question;
  [SerializeField]
  public UnityEvent resultAFinishedCallback;
  [SerializeField]
  public UnityEvent resultBFinishedCallback;
  [Space]
  [SerializeField]
  public bool requireConditions;
  [SerializeField]
  public List<BiomeGenerator.VariableAndCondition> conditionalVariables = new List<BiomeGenerator.VariableAndCondition>();
  [SerializeField]
  public int dungeonNumber = -1;
  [SerializeField]
  public Interaction_SimpleConversation alternateConversation;
  [SerializeField]
  public bool isExecutioner;
  [SerializeField]
  public bool isInside;
  [Space]
  [SerializeField]
  public UnityEvent awakeCallback;
  [SerializeField]
  public UnityEvent callback;
  public string insideAtmosParam = "inside";
  public bool introStarted;
  public bool startedInteraction;
  public bool waiting;
  public int followersDead;
  public float midCombatTimestamp = -1f;
  public bool addedFakeHealth;
  public bool fadedRed;
  public bool completed;
  public bool playerAnimsPlayed;
  [SerializeField]
  public LightingManagerVolume lightOverride;
  [SerializeField]
  public List<GameObject> traps;
  public List<FollowerManager.SpawnedFollower> spawnedFollowers = new List<FollowerManager.SpawnedFollower>();
  public List<Material> modifiedMaterials = new List<Material>();
  public Coroutine podiumIntroRoutine;
  public Coroutine music;
  public Material followerMaterial;
  public static int LeaderEncounterColorBoost = Shader.PropertyToID("_LeaderEncounterColorBoost");

  public bool endCombat => this.introType == DungeonLeaderMechanics.IntroType.EndCombat;

  public bool midCombat => this.introType == DungeonLeaderMechanics.IntroType.MidCombat;

  public bool beforeCombat => this.introType == DungeonLeaderMechanics.IntroType.BeforeCombat;

  public void Awake()
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
    if (!this.isRottingFollower || !this.spawnsFollower)
      return;
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && this.isRottingFollower)
        follower.Traits.Contains(FollowerTrait.TraitType.Mutated);
    }
    this.conversation.Entries.RemoveAt(this.conversation.Entries.Count - 1);
  }

  public void Start()
  {
    Transform transform = BiomeGenerator.Instance.CurrentRoom == null || !((UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null) ? BiomeGenerator.Instance.transform : BiomeGenerator.Instance.CurrentRoom.generateRoom.transform;
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
    if (componentsInChildren6.Length != 0)
    {
      for (int index = componentsInChildren6.Length - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) componentsInChildren6[index].gameObject);
    }
    EnemyWolfTurret[] componentsInChildren7 = transform.GetComponentsInChildren<EnemyWolfTurret>();
    if (componentsInChildren7.Length == 0)
      return;
    for (int index = componentsInChildren7.Length - 1; index >= 0; --index)
      componentsInChildren7[index].DeactivateWolfTurret();
  }

  public void OnEnable()
  {
    if (this.introType == DungeonLeaderMechanics.IntroType.None && !GameManager.RoomActive && (UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null && !BiomeGenerator.Instance.CurrentRoom.Completed)
      BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID = SoundConstants.RoomID.CultLeaderAmbience;
    if (this.completed)
      this.Hide();
    if ((bool) (UnityEngine.Object) this.conversation)
      this.conversation.OnInteraction += new Interaction.InteractionEvent(this.OnInteraction);
    DungeonLeaderMechanics.Instance = this;
    if (this.introStarted && GameManager.RoomActive && !RoomLockController.DoorsOpen && this.hasEnemyRounds && this.enemyRounds.CurrentRound < this.enemyRounds.TotalRounds)
    {
      if (this.music != null)
        this.StopCoroutine(this.music);
      this.music = this.StartCoroutine((IEnumerator) this.PlayMusicDelayed());
    }
    AudioManager.Instance.AdjustAtmosParameter(this.insideAtmosParam, 1f);
    if (this.podiumIntroRoutine == null || this.introStarted)
      return;
    this.StopCoroutine(this.podiumIntroRoutine);
    this.podiumIntroRoutine = this.StartCoroutine((IEnumerator) this.PodiumIntroIE());
  }

  public IEnumerator PlayMusicDelayed()
  {
    yield return (object) new WaitForSecondsRealtime(1f);
    Debug.Log((object) "Set offering combat music ID");
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.OfferingCombat);
    AudioManager.Instance.SetMusicCombatState();
  }

  public void OnDestroy()
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

  public void OnDisable()
  {
    if ((bool) (UnityEngine.Object) this.conversation)
      this.conversation.OnInteraction -= new Interaction.InteractionEvent(this.OnInteraction);
    if (this.introStarted && Health.team2.Count <= 0)
      this.gameObject.SetActive(false);
    foreach (Material modifiedMaterial in this.modifiedMaterials)
      modifiedMaterial.SetColor(UnitObject.LeaderEncounterColorBoost, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    this.modifiedMaterials.Clear();
    DungeonLeaderMechanics.Instance = (DungeonLeaderMechanics) null;
    AudioManager.Instance.AdjustAtmosParameter(this.insideAtmosParam, 0.0f);
  }

  public void Hide()
  {
    if ((UnityEngine.Object) this.leaderSpine != (UnityEngine.Object) null)
      this.leaderSpine.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.goopFloorParticle != (UnityEngine.Object) null)
      this.goopFloorParticle.gameObject.SetActive(false);
    foreach (DungeonLeaderMechanics otherLeader in this.otherLeaders)
    {
      if ((bool) (UnityEngine.Object) otherLeader)
        otherLeader.Hide();
    }
  }

  public void OnInteraction(StateMachine state)
  {
    this.startedInteraction = true;
    foreach (PlayerFarming player in PlayerFarming.players)
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.SetPlayerAnimations(player));
    this.playerAnimsPlayed = true;
  }

  public void GoToAndStopBegin(Vector3 targetPosition)
  {
    if (!this.gameObject.activeInHierarchy || PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
      return;
    RoomLockController.CloseAll();
    if (!this.introStarted && !this.startedInteraction)
      return;
    if ((bool) (UnityEngine.Object) this.conversation)
      this.conversation.MovePlayerToListenPosition = false;
    foreach (PlayerFarming player1 in PlayerFarming.players)
    {
      PlayerFarming player = player1;
      player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      player.state.LockStateChanges = true;
      player.Spine.AnimationState.SetAnimation(0, "floating-boss-start", false);
      player.Spine.AnimationState.AddAnimation(0, "floating-boss-loop", true, 0.0f);
      player.state.LockStateChanges = true;
      player.IdleOnEnd = false;
      if (!player.isLamb)
      {
        if ((double) Mathf.Abs(targetPosition.x) < 1.0)
          targetPosition.x += 1.5f;
        else
          targetPosition.x *= -1f;
      }
      player.transform.DOMove(targetPosition, 2f).SetSpeedBased<TweenerCore<Vector3, Vector3, VectorOptions>>().SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        player.IdleOnEnd = true;
        player.EndGoToAndStop();
        this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
        {
          player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
          player.Spine.AnimationState.SetAnimation(0, "floating-boss-loop", true);
        })));
      }));
    }
    PlayerFarming.OnGoToAndStopBegin -= new PlayerFarming.GoToEvent(this.GoToAndStopBegin);
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    callback();
  }

  public IEnumerator SetPlayerAnimations(PlayerFarming player)
  {
    if (!this.playerAnimsPlayed && PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
    {
      yield return (object) new WaitForEndOfFrame();
      while (player.GoToAndStopping)
        yield return (object) null;
      player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      player.state.LockStateChanges = true;
      player.Spine.AnimationState.AddAnimation(0, "floating-boss-loop", true, 0.0f);
      yield return (object) new WaitForSeconds(3.3f);
      while (MMConversation.CURRENT_CONVERSATION != null || !this.conversation.Interactable || this.waiting)
        yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      player.transform.DOKill();
      player.simpleSpineAnimator.Animate("floating-boss-land", 0, false);
      player.Spine.AnimationState.AddAnimation(0, "idle-up", true, 0.0f);
      yield return (object) new WaitForSeconds(1.76f);
      if (LetterBox.IsPlaying)
      {
        while (MMConversation.CURRENT_CONVERSATION != null || this.waiting)
          yield return (object) null;
        yield return (object) new WaitForSeconds(0.5f);
        while (MMConversation.CURRENT_CONVERSATION != null || this.waiting)
          yield return (object) null;
      }
      player.EndGoToAndStop();
      player.state.LockStateChanges = false;
      PlayerFarming.SetStateForAllPlayers();
    }
  }

  public void EndConversation() => GameManager.GetInstance().OnConversationEnd();

  public void Update()
  {
    if (!this.introStarted && GameManager.RoomActive && !RoomLockController.DoorsOpen)
    {
      if (this.introType == DungeonLeaderMechanics.IntroType.EndCombat)
      {
        if (!this.addedFakeHealth && this.introType == DungeonLeaderMechanics.IntroType.EndCombat)
        {
          Health.team2.Add((Health) null);
          Interaction_Chest.Instance?.AddEnemy((Health) null);
          this.addedFakeHealth = true;
        }
        if (Health.team2.Count <= 0 || Health.team2.Count == 1 && (UnityEngine.Object) Health.team2[0] == (UnityEngine.Object) null)
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
          DeviceLightingManager.TransitionLighting(Color.white, Color.red, 2f, DeviceLightingManager.F_KEYS);
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

  public IEnumerator DrawnOutIntroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    AudioManager.Instance.SetMusicCombatState(false);
    dungeonLeaderMechanics.introStarted = true;
    RoomLockController.CloseAll();
    yield return (object) new WaitForSeconds(0.5f);
    List<Health> breakables = Health.neutralTeam;
    bool conversationActive = false;
    DeviceLightingManager.TransitionLighting(Color.white, Color.red, dungeonLeaderMechanics.shakeDuration / 1.5f, DeviceLightingManager.F_KEYS, Ease.InSine);
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
        GameManager.GetInstance().OnConversationNew(false, false);
        GameManager.GetInstance().OnConversationNext(dungeonLeaderMechanics.cameraTarget);
        if ((UnityEngine.Object) dungeonLeaderMechanics.goopFloorParticle != (UnityEngine.Object) null)
          dungeonLeaderMechanics.ShowGoop();
      }
      if ((double) time > 0.75 && !dungeonLeaderMechanics.conversation.Interactable)
      {
        while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.InActive && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Attacking && (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.CustomAnimation || !(PlayerFarming.Instance.Spine.AnimationState.GetCurrent(0).Animation.Name == "floating-boss-loop")))
          yield return (object) null;
        foreach (ConversationEntry entry in dungeonLeaderMechanics.conversation.Entries)
        {
          entry.SetZoom = true;
          entry.Zoom = 11f;
        }
        TrapPoison.RemoveAllPoison();
        dungeonLeaderMechanics.conversation.Interactable = true;
        dungeonLeaderMechanics.conversation.OnInteract(PlayerFarming.Instance.state);
        if (dungeonLeaderMechanics.leaderSpine.AnimationState != null)
          dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.talk, true, 0.0f);
        dungeonLeaderMechanics.conversation.Callback.AddListener(new UnityAction(dungeonLeaderMechanics.\u003CDrawnOutIntroIE\u003Eb__79_0));
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
        health.DealDamage(health.totalHP, health.gameObject, dungeonLeaderMechanics.transform.position);
        lastBreakTime = t + UnityEngine.Random.Range(0.0f, 1f);
      }
      t += Time.deltaTime;
      yield return (object) null;
    }
    MMVibrate.StopRumble();
    Health.team2.Clear();
  }

  public IEnumerator InstantSpawnIntroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    DeviceLightingManager.TransitionLighting(Color.white, Color.red, 2f, DeviceLightingManager.F_KEYS);
    RoomLockController.CloseAll();
    dungeonLeaderMechanics.FadeRedIn();
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    AudioManager.Instance.SetMusicCombatState(false);
    dungeonLeaderMechanics.introStarted = true;
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5 || PlayerFarming.Location == FollowerLocation.Dungeon1_6)
    {
      PlayerFarming.Instance.GoToAndStop(dungeonLeaderMechanics.conversation.ListenPosition, dungeonLeaderMechanics.leaderSpine.gameObject);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(dungeonLeaderMechanics.cameraTarget, 6f);
      dungeonLeaderMechanics.leaderSpine.AnimationState.SetAnimation(0, dungeonLeaderMechanics.enter, false);
      dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.idle, true, 0.0f);
      yield return (object) new WaitForSeconds(3f);
    }
    else
    {
      if ((UnityEngine.Object) dungeonLeaderMechanics.goopFloorParticle != (UnityEngine.Object) null)
        dungeonLeaderMechanics.ShowGoop();
      yield return (object) new WaitForSeconds(0.75f);
      dungeonLeaderMechanics.SpawnDistortionObject();
      yield return (object) new WaitForSeconds(0.75f);
      dungeonLeaderMechanics.leaderSpine.timeScale = 5f;
      dungeonLeaderMechanics.leaderSpine.AnimationState.SetAnimation(0, dungeonLeaderMechanics.enter, false);
      dungeonLeaderMechanics.leaderSpine.AnimationState.AddAnimation(0, dungeonLeaderMechanics.idle, true, 0.0f);
      yield return (object) new WaitForSeconds(0.75f);
    }
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
      dungeonLeaderMechanics.conversation.Callback.AddListener(new UnityAction(dungeonLeaderMechanics.\u003CInstantSpawnIntroIE\u003Eb__80_0));
    }
  }

  public void PodiumIntro(Collider2D collider)
  {
    if (this.introStarted)
      return;
    this.podiumIntroRoutine = this.StartCoroutine((IEnumerator) this.PodiumIntroIE());
    this.middleCollider.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.PodiumIntro);
  }

  public IEnumerator PodiumIntroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    while (!dungeonLeaderMechanics.introStarted && !GameManager.RoomActive)
      yield return (object) null;
    DeviceLightingManager.TransitionLighting(Color.white, Color.red, 2f, DeviceLightingManager.F_KEYS);
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
    while (true)
    {
      bool flag = false;
      if (PlayerFarming.AnyPlayerGotoAndStopping())
        flag = true;
      if (flag)
        yield return (object) null;
      else
        break;
    }
    RoomLockController.CloseAll();
    if ((UnityEngine.Object) dungeonLeaderMechanics.goopFloorParticle != (UnityEngine.Object) null)
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
    foreach (PlayerFarming player in PlayerFarming.players)
      GameManager.GetInstance().StartCoroutine((IEnumerator) dungeonLeaderMechanics.SetPlayerAnimations(player));
    dungeonLeaderMechanics.playerAnimsPlayed = true;
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
      dungeonLeaderMechanics.conversation.Callback.AddListener(new UnityAction(dungeonLeaderMechanics.\u003CPodiumIntroIE\u003Eb__82_0));
    }
  }

  public void DamageEnemies(Collider2D collider)
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
      this.goopFloorParticle.GetComponent<SkeletonAnimation>().AnimationState?.AddAnimation(0, "leader-loop", true, 0.0f);
      AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", this.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/summoned", this.transform.position);
    }
    else
      this.StartCoroutine((IEnumerator) this.HideGoopDelayIE());
  }

  public IEnumerator HideGoopDelayIE()
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
    DeviceLightingManager.StopAll();
    DeviceLightingManager.TransitionLighting(Color.red, Color.white, 1f, DeviceLightingManager.F_KEYS);
    GameManager.GetInstance().WaitForSeconds(1f, (System.Action) (() => DeviceLightingManager.UpdateLocation()));
    GameManager.GetInstance().SetDitherTween(0.0f);
    if ((UnityEngine.Object) this.lightOverride != (UnityEngine.Object) null)
      this.lightOverride.gameObject.SetActive(false);
    if ((UnityEngine.Object) AudioManager.Instance != (UnityEngine.Object) null)
    {
      AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.StandardAmbience);
      if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null)
        BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID = SoundConstants.RoomID.StandardAmbience;
    }
    if (!(bool) (UnityEngine.Object) Interaction_Chest.Instance)
      return;
    GameManager.GetInstance().RemoveFromCamera(Interaction_Chest.Instance.gameObject);
  }

  public void FadeRedIn()
  {
    if ((UnityEngine.Object) this.lightOverride != (UnityEngine.Object) null)
      this.lightOverride.gameObject.SetActive(true);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.CultLeaderAmbience);
    BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID = SoundConstants.RoomID.StandardAmbience;
  }

  public void SpawnDistortionObject()
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

  public IEnumerator KillAll()
  {
    yield return (object) new WaitForSeconds(0.25f);
    foreach (Health health in new List<Health>((IEnumerable<Health>) Health.team2))
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
      {
        health.invincible = false;
        health.enabled = true;
        health.DealDamage(float.PositiveInfinity, health.gameObject, Vector3.zero, AttackType: Health.AttackTypes.Projectile);
        yield return (object) new WaitForSeconds(0.05f);
      }
    }
  }

  public void BeginEnemyRounds() => this.StartCoroutine((IEnumerator) this.BeginEnemyRoundsIE());

  public IEnumerator BeginEnemyRoundsIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    GameManager.GetInstance().OnConversationEnd();
    RoomLockController.CloseAll();
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
    dungeonLeaderMechanics.enemyRounds.BeginCombat(false, new System.Action(dungeonLeaderMechanics.\u003CBeginEnemyRoundsIE\u003Eb__92_0));
    dungeonLeaderMechanics.enemyRounds.OnEnemySpawned += new EnemyRoundsBase.EnemyEvent(dungeonLeaderMechanics.\u003CBeginEnemyRoundsIE\u003Eb__92_1);
  }

  public void Bow() => this.StartCoroutine((IEnumerator) this.BowIE());

  public IEnumerator BowIE()
  {
    this.waiting = true;
    TrackEntry trackEntry = (TrackEntry) null;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.state.LookAngle = 90f;
      player.state.facingAngle = 90f;
      player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      trackEntry = player.Spine.AnimationState.SetAnimation(0, "pray", false);
      player.Spine.AnimationState.AddAnimation(0, "idle-up", true, 0.0f);
    }
    yield return (object) new WaitForSeconds(trackEntry.Animation.Duration);
    CultFaithManager.AddThought(Thought.Cult_LostRespect, faithMultiplier: 5f);
    this.waiting = false;
  }

  public void GiveRotstone() => this.StartCoroutine((IEnumerator) this.GiveRotstoneIE());

  public IEnumerator GiveRotstoneIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    PlayerFarming playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    int sootAmount = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MAGMA_STONE);
    float increment = 1f / (float) sootAmount;
    for (int i = 0; i < sootAmount; ++i)
    {
      Inventory.GetItemByType(172);
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", playerFarming.gameObject.transform.position);
      ResourceCustomTarget.Create(dungeonLeaderMechanics.gameObject, playerFarming.gameObject.transform.position, InventoryItem.ITEM_TYPE.MAGMA_STONE, (System.Action) null);
      Inventory.ChangeItemQuantity(172, -1);
      yield return (object) new WaitForSeconds(increment);
    }
    dungeonLeaderMechanics.waiting = false;
    GameManager.GetInstance().OnConversationEnd();
    dungeonLeaderMechanics.completed = true;
    RoomLockController.RoomCompleted();
    dungeonLeaderMechanics.FadeRedAway();
  }

  public void GiveFood() => this.StartCoroutine((IEnumerator) this.GiveFoodIE());

  public IEnumerator GiveFoodIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    PlayerFarming playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    for (int t = 0; t < InventoryItem.AllFoods.Count; ++t)
    {
      int sootAmount = Inventory.GetItemQuantity(InventoryItem.AllFoods[t]);
      sootAmount = Mathf.Min(5, sootAmount);
      float increment = 0.25f / (float) sootAmount;
      for (int i = 0; i < sootAmount; ++i)
      {
        Inventory.GetItemByType(InventoryItem.AllFoods[t]);
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", playerFarming.gameObject.transform.position);
        ResourceCustomTarget.Create(dungeonLeaderMechanics.gameObject, playerFarming.gameObject.transform.position, InventoryItem.AllFoods[t], (System.Action) null);
        yield return (object) new WaitForSeconds(increment);
      }
      Inventory.ChangeItemQuantity(InventoryItem.AllFoods[t], -Inventory.GetItemQuantity(InventoryItem.AllFoods[t]));
    }
    dungeonLeaderMechanics.waiting = false;
    GameManager.GetInstance().OnConversationEnd();
    dungeonLeaderMechanics.callback?.Invoke();
    dungeonLeaderMechanics.completed = true;
  }

  public void GiveFoodAndPermanentHealth()
  {
    this.StartCoroutine((IEnumerator) this.GiveFoodAndPermanentHealthIE());
  }

  public void GiveFoodPrecentage()
  {
    int daysOfFood = 5;
    if (CookingData.GetDaysOfFood(DataManager.Instance.items, DataManager.Instance.Followers.Count) <= 5)
      daysOfFood = 0;
    this.StartCoroutine((IEnumerator) this.GiveFoodPrecentageIE(daysOfFood));
  }

  public IEnumerator GiveFoodAndPermanentHealthIE()
  {
    if (CookingData.GetDaysOfFood(DataManager.Instance.items, DataManager.Instance.Followers.Count) > 5)
      yield return (object) this.GiveFoodPrecentageIE(10, false);
    yield return (object) this.GiveHealthPermanentIE();
    while (this.waiting)
      yield return (object) null;
    GameManager.GetInstance().OnConversationEnd();
    this.callback?.Invoke();
    this.completed = true;
    RoomLockController.RoomCompleted();
    this.FadeRedAway();
  }

  public IEnumerator GiveFoodPrecentageIE(int daysOfFood, bool endConvo = true)
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    PlayerFarming playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    List<InventoryItem> leftoverFoodAfterDays = CookingData.GetLeftoverFoodAfterDays(DataManager.Instance.items, DataManager.Instance.Followers.Count, daysOfFood);
    List<InventoryItem> unqiueFoodItems = new List<InventoryItem>();
    for (int index1 = 0; index1 < leftoverFoodAfterDays.Count; ++index1)
    {
      bool flag = true;
      for (int index2 = 0; index2 < unqiueFoodItems.Count; ++index2)
      {
        if (unqiueFoodItems[index2].type == leftoverFoodAfterDays[index1].type)
        {
          flag = false;
          unqiueFoodItems[index2].quantity += leftoverFoodAfterDays[index1].quantity;
          break;
        }
      }
      if (flag)
        unqiueFoodItems.Add(new InventoryItem(leftoverFoodAfterDays[index1]));
    }
    DataManager.Instance.GivenUpWolfFood.AddRange((IEnumerable<InventoryItem>) unqiueFoodItems);
    int minFoodToSpawn = 5;
    if (unqiueFoodItems.Count > 25)
      minFoodToSpawn = 3;
    int x;
    for (x = 0; x < unqiueFoodItems.Count; ++x)
    {
      int foodToSpawn = Mathf.Min(minFoodToSpawn, unqiueFoodItems[x].quantity);
      if (foodToSpawn != 0)
      {
        Inventory.ChangeItemQuantity(unqiueFoodItems[x].type, -unqiueFoodItems[x].quantity);
        for (int y = 0; y < foodToSpawn; ++y)
        {
          ResourceCustomTarget.Create(dungeonLeaderMechanics.gameObject, playerFarming.gameObject.transform.position, (InventoryItem.ITEM_TYPE) unqiueFoodItems[x].type, (System.Action) null);
          AudioManager.Instance.PlayOneShot("event:/followers/pop_in", playerFarming.gameObject.transform.position);
          yield return (object) new WaitForSeconds(0.1f / (float) foodToSpawn);
        }
      }
    }
    int itemQuantity = Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.GRASS);
    int Quantity = itemQuantity - Mathf.Min(itemQuantity, 20);
    if (itemQuantity > 20)
    {
      DataManager.Instance.GivenUpWolfFood.Add(new InventoryItem(InventoryItem.ITEM_TYPE.GRASS, Quantity));
      Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.GRASS, -Quantity);
      for (x = 0; x < minFoodToSpawn; ++x)
      {
        AudioManager.Instance.PlayOneShot("event:/followers/pop_in", playerFarming.gameObject.transform.position);
        ResourceCustomTarget.Create(dungeonLeaderMechanics.gameObject, playerFarming.gameObject.transform.position, InventoryItem.ITEM_TYPE.GRASS, (System.Action) null);
        yield return (object) new WaitForSeconds(0.01f);
      }
    }
    if (endConvo)
    {
      dungeonLeaderMechanics.waiting = false;
      GameManager.GetInstance().OnConversationEnd();
      dungeonLeaderMechanics.callback?.Invoke();
      dungeonLeaderMechanics.completed = true;
      RoomLockController.RoomCompleted();
      dungeonLeaderMechanics.FadeRedAway();
    }
  }

  public IEnumerator GiveHealthPermanentIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    PlayerFarming playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration = playerFarming.Spine.AnimationState.SetAnimation(0, "rip-heart", false).Animation.Duration;
    playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/marchosiaslibrary/heart_give", playerFarming.transform.position);
    yield return (object) new WaitForSeconds(duration - 1f);
    for (int index = 0; index < PlayerFarming.players.Count; ++index)
    {
      HealthPlayer health = PlayerFarming.players[index].health;
      if ((double) health.HP > 1.0)
        --health.HP;
      if (!PlayerFleeceManager.FleecePreventsRespawn() && (double) health.totalHP > 1.0)
        --health.totalHP;
    }
    --DataManager.Instance.PLAYER_HEALTH_MODIFIED;
    DataManager.Instance.GivenUpHeartToWolf = true;
    bool cachedNoHeartDrops = DataManager.Instance.NoHeartDrops;
    DataManager.Instance.NoHeartDrops = false;
    ResourceCustomTarget.Create(dungeonLeaderMechanics.gameObject, playerFarming.gameObject.transform.position, InventoryItem.ITEM_TYPE.HALF_HEART, (System.Action) (() =>
    {
      this.waiting = false;
      DataManager.Instance.NoHeartDrops = cachedNoHeartDrops;
    }));
  }

  public void CompleteRoom()
  {
    RoomLockController.RoomCompleted();
    this.FadeRedAway();
  }

  public void GiveHealth() => this.StartCoroutine((IEnumerator) this.GiveHealthIE());

  public IEnumerator GiveHealthIE()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    PlayerFarming playerFarming;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      playerFarming.health.HP -= 2f;
      ResourceCustomTarget.Create(dungeonLeaderMechanics.gameObject, playerFarming.gameObject.transform.position, InventoryItem.ITEM_TYPE.RED_HEART, new System.Action(dungeonLeaderMechanics.\u003CGiveHealthIE\u003Eb__106_0));
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    dungeonLeaderMechanics.waiting = true;
    playerFarming = MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer;
    playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float duration = playerFarming.Spine.AnimationState.SetAnimation(0, "rip-heart", false).Animation.Duration;
    playerFarming.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/dlc/env/marchosiaslibrary/heart_give", playerFarming.transform.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(duration - 1f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void AskQuestion()
  {
    if (this.isRottingFollower && this.spawnsFollower)
    {
      bool flag = false;
      foreach (FollowerInfo follower in DataManager.Instance.Followers)
      {
        if (follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && (!this.isRottingFollower || follower.Traits.Contains(FollowerTrait.TraitType.Mutated)))
          flag = true;
      }
      if (!flag)
      {
        this.BeginEnemyRounds();
        return;
      }
    }
    this.StartCoroutine((IEnumerator) this.AskQuestionIE());
  }

  public void LightningStrikePlayer()
  {
    BiomeConstants.Instance.EmitLightningStrike(PlayerFarming.Instance.transform.position);
  }

  public IEnumerator AskQuestionIE()
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
    Entries[0].Speaker = !(bool) (UnityEngine.Object) dungeonLeaderMechanics.conversation || dungeonLeaderMechanics.conversation.Entries.Count <= 0 ? Entries[dungeonLeaderMechanics.question.SpeakerEntryIndex].Speaker : dungeonLeaderMechanics.conversation.Entries[dungeonLeaderMechanics.question.SpeakerEntryIndex].Speaker;
    Entries[0].SetZoom = true;
    Entries[0].Zoom = 12f;
    if (dungeonLeaderMechanics.question.CharacterName == "NAMES/CultLeaders/Dungeon2")
      Entries[0].soundPath = "event:/dialogue/dun2_cult_leader_heket/standard_heket";
    else if (dungeonLeaderMechanics.question.CharacterName == "NAMES/CultLeaders/Dungeon4")
      Entries[0].soundPath = "event:/dialogue/dun4_cult_leader_shamura/standard_shamura";
    List<MMTools.Response> Responses = new List<MMTools.Response>()
    {
      new MMTools.Response(dungeonLeaderMechanics.question.AnswerA, new System.Action(dungeonLeaderMechanics.\u003CAskQuestionIE\u003Eb__109_0), dungeonLeaderMechanics.question.AnswerA),
      new MMTools.Response(dungeonLeaderMechanics.question.AnswerB, new System.Action(dungeonLeaderMechanics.\u003CAskQuestionIE\u003Eb__109_1), dungeonLeaderMechanics.question.AnswerB)
    };
    MMConversation.Play(new ConversationObject(Entries, Responses, (System.Action) null), false);
    yield return (object) new WaitForEndOfFrame();
  }

  public IEnumerator ResponseIE(bool responseWasA, DungeonLeaderMechanics.Question question)
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
    Entries[0].Speaker = !(bool) (UnityEngine.Object) dungeonLeaderMechanics.conversation || dungeonLeaderMechanics.conversation.Entries.Count <= 0 ? Entries[question.SpeakerEntryIndex].Speaker : dungeonLeaderMechanics.conversation.Entries[question.SpeakerEntryIndex].Speaker;
    Entries[0].SetZoom = true;
    Entries[0].Zoom = 12f;
    Entries[0].SkeletonData = !((UnityEngine.Object) dungeonLeaderMechanics.conversation != (UnityEngine.Object) null) || dungeonLeaderMechanics.conversation.Entries == null || dungeonLeaderMechanics.conversation.Entries.Count <= 0 ? (SkeletonAnimation) null : dungeonLeaderMechanics.conversation.Entries[question.SpeakerEntryIndex].SkeletonData;
    Entries[0].Animation = !((UnityEngine.Object) dungeonLeaderMechanics.conversation != (UnityEngine.Object) null) || dungeonLeaderMechanics.conversation.Entries == null || dungeonLeaderMechanics.conversation.Entries.Count <= 0 ? "" : dungeonLeaderMechanics.conversation.Entries[question.SpeakerEntryIndex].Animation;
    Entries[0].DefaultAnimation = !((UnityEngine.Object) dungeonLeaderMechanics.conversation != (UnityEngine.Object) null) || dungeonLeaderMechanics.conversation.Entries == null || dungeonLeaderMechanics.conversation.Entries.Count <= 0 ? "" : dungeonLeaderMechanics.conversation.Entries[question.SpeakerEntryIndex].DefaultAnimation;
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

  public IEnumerator ACallbackIE()
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

  public IEnumerator BCallbackIE()
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

  public IEnumerator ConvertAllMapNodesIE()
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

  public IEnumerator ConvertMiniBossNodeToBossNodeIE()
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

  public void SetRottingCombatFollower(bool rotting)
  {
    this.isRottingFollower = rotting;
    this.SpawnInCombatFollower();
  }

  public void SpawnInCombatFollower()
  {
    this.StartCoroutine((IEnumerator) this.SpawnInCombatFollowerIE());
  }

  public IEnumerator SpawnInCombatFollowerIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    PlayerReturnToBase.Disabled = true;
    if ((bool) (UnityEngine.Object) dungeonLeaderMechanics.distortionObject)
      dungeonLeaderMechanics.distortionObject.gameObject.SetActive(false);
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && (!dungeonLeaderMechanics.isRottingFollower && !follower.Traits.Contains(FollowerTrait.TraitType.Mutated) || dungeonLeaderMechanics.isRottingFollower && follower.Traits.Contains(FollowerTrait.TraitType.Mutated)))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      Vector3[] spawnPositions = new Vector3[3]
      {
        dungeonLeaderMechanics.transform.position + Vector3.right * 2f + dungeonLeaderMechanics.followerSpawnOffset,
        dungeonLeaderMechanics.transform.position - Vector3.right * 2f + dungeonLeaderMechanics.followerSpawnOffset,
        dungeonLeaderMechanics.transform.position - Vector3.right * 4f + dungeonLeaderMechanics.followerSpawnOffset
      };
      for (int i = 0; i < dungeonLeaderMechanics.spawnAmount && possibleFollowers.Count != 0; ++i)
      {
        FollowerInfo followerInfo = possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)];
        possibleFollowers.Remove(followerInfo);
        FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(followerInfo, spawnPositions[i]);
        FollowerManager.RetireSimFollowerByID(spawnedFollower.FollowerBrain.Info.ID);
        Health component = spawnedFollower.Follower.GetComponent<Health>();
        if ((bool) (UnityEngine.Object) component)
          component.invincible = true;
        spawnedFollower.Follower.OverridingEmotions = true;
        dungeonLeaderMechanics.SetFollowerOutfit(spawnedFollower, Thought.Dissenter);
        dungeonLeaderMechanics.spawnedFollowers.Add(spawnedFollower);
        GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 5f);
        yield return (object) new WaitForSeconds(1f);
      }
      dungeonLeaderMechanics.spawnAmount = dungeonLeaderMechanics.spawnedFollowers.Count;
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

  public void SetFollowerOutfit(
    FollowerManager.SpawnedFollower spawnedFollower,
    Thought cursedState)
  {
    FollowerSpecialType special = spawnedFollower.FollowerBrain.Info.Special;
    if (this.isRottingFollower)
      special = FollowerSpecialType.Rot;
    FollowerBrain.SetFollowerCostume(spawnedFollower.Follower.Spine.Skeleton, 0, spawnedFollower.FollowerBrain._directInfoAccess.SkinName, spawnedFollower.FollowerBrain._directInfoAccess.SkinColour, this.isRottingFollower ? FollowerOutfitType.Dissenter : FollowerOutfitType.None, FollowerHatType.None, spawnedFollower.FollowerBrain.Info.Clothing, spawnedFollower.FollowerBrain.Info.Customisation, special, spawnedFollower.FollowerBrain.Info.Necklace, spawnedFollower.FollowerBrain.Info.ClothingVariant, spawnedFollower.FollowerBrain._directInfoAccess);
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(0, "Emotions/emotion-dissenter", true);
  }

  public void FollowerDied(
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
        FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeInfo.ID);
        FollowerBrain.RemoveBrain(spawnedFollower.FollowerBrain.Info.ID);
        FollowerBrain.AllBrains.Add(spawnedFollower.FollowerBrain);
        FollowerBrain.AllBrains.Sort((Comparison<FollowerBrain>) ((a, b) =>
        {
          if (a.Info == null && b.Info == null)
            return 0;
          if (a.Info == null && b.Info != null)
            return -1;
          return a.Info != null && b.Info == null ? 1 : a.Info.ID.CompareTo(b.Info.ID);
        }));
        FollowerManager.FollowerDie(spawnedFollower.FollowerBrain.Info.ID, NotificationCentre.NotificationType.Died);
        break;
      }
    }
    ++this.followersDead;
    if (this.followersDead < this.spawnAmount)
      return;
    PlayerReturnToBase.Disabled = false;
    if ((UnityEngine.Object) this.followerMaterial != (UnityEngine.Object) null)
      this.followerMaterial.SetColor(DungeonLeaderMechanics.LeaderEncounterColorBoost, new Color(0.0f, 0.0f, 0.0f, 0.0f));
    Interaction_Chest.Instance?.Reveal();
    RoomLockController.RoomCompleted(true);
    this.FadeRedAway();
  }

  public void MakeRandomFollowerIll()
  {
    this.StartCoroutine((IEnumerator) this.MakeRandomFollowerIllIE());
  }

  public IEnumerator MakeRandomFollowerIllIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Illness <= 0.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID))
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
      spawnedFollower = (FollowerManager.SpawnedFollower) null;
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  public IEnumerator MakeFollowerIll(FollowerManager.SpawnedFollower spawnedFollower, float delay)
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
    FollowerStarvingWarning componentInChildren3 = spawnedFollower.Follower.GetComponentInChildren<FollowerStarvingWarning>(true);
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

  public IEnumerator MakeAllFollowersIllIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Illness <= 0.0 && follower.CursedState == Thought.None && possibleFollowers.Count < 6 && !FollowerManager.FollowerLocked(in follower.ID))
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
        startingAngle = Utils.Repeat(startingAngle + angleBetween, 360f);
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

  public IEnumerator MakeRandomFollowerStarvingIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Satiation > 30.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && !FollowerBrain.GetOrCreateBrain(follower).HasTrait(FollowerTrait.TraitType.DontStarve))
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
      spawnedFollower = (FollowerManager.SpawnedFollower) null;
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    dungeonLeaderMechanics.waiting = false;
  }

  public IEnumerator MakeFollowerStarving(
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
    FollowerStarvingWarning followerStarvingWArning = spawnedFollower.Follower.GetComponentInChildren<FollowerStarvingWarning>(true);
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

  public IEnumerator MakeAllFollowersStarvingIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if ((double) follower.Satiation > 25.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && !FollowerBrain.GetOrCreateBrain(follower).HasTrait(FollowerTrait.TraitType.DontStarve))
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
        startingAngle = Utils.Repeat(startingAngle + angleBetween, 360f);
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

  public void MakeRandomFollowerMutated()
  {
    this.StartCoroutine((IEnumerator) this.MakeRandomFollowerMutatedIE());
  }

  public IEnumerator MakeRandomFollowerMutatedIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (possibleFollowers.Count < 2 && dungeonLeaderMechanics.CanMutateFollower(follower) && !FollowerManager.UniqueFollowerIDs.Contains(follower.ID))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(possibleFollowers[UnityEngine.Random.Range(0, possibleFollowers.Count)], dungeonLeaderMechanics.transform.position + Vector3.up * -1f);
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(spawnedFollower.Follower.gameObject, 12f);
      yield return (object) dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.MakeFollowerMutated(spawnedFollower, 0.9f));
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

  public IEnumerator MakeFollowerMutated(
    FollowerManager.SpawnedFollower spawnedFollower,
    float delay)
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-feared", false, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Conversations/idle-hate", false, 0.0f);
    yield return (object) new WaitForSeconds(delay);
    yield return (object) new WaitForSeconds(0.5f);
    for (int i = 0; i < UnityEngine.Random.Range(4, 7); ++i)
    {
      SoulCustomTarget.Create(spawnedFollower.Follower.gameObject, dungeonLeaderMechanics.transform.position + new Vector3(0.0f, 0.0f, -0.5f) + UnityEngine.Random.insideUnitSphere * 0.5f, Color.red, (System.Action) null, 0.2f, 100f);
      yield return (object) new WaitForSeconds(0.05f);
    }
    yield return (object) new WaitForSeconds(0.5f);
    float offset = UnityEngine.Random.Range(0.0f, 0.3f);
    TrackEntry e = spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "Sick/chunder", false);
    e.TrackTime = offset;
    TrackEntry track = spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "spawn-out-angry", false, 0.0f);
    yield return (object) new WaitForSeconds(0.5f);
    spawnedFollower.FollowerBrain.AddTrait(FollowerTrait.TraitType.Mutated, true);
    spawnedFollower.FollowerFakeInfo.Traits.Add(FollowerTrait.TraitType.Mutated);
    FollowerBrain.SetFollowerCostume(spawnedFollower.Follower.Spine.Skeleton, spawnedFollower.FollowerFakeInfo, forceUpdate: true);
    yield return (object) new WaitForSeconds((float) ((double) e.Animation.Duration - (double) offset - 0.5));
    yield return (object) new WaitForSeconds(track.Animation.Duration);
    spawnedFollower.Follower.gameObject.SetActive(false);
    FollowerBrain.RemoveBrain(spawnedFollower.FollowerFakeBrain.Info.ID);
    dungeonLeaderMechanics.waiting = false;
  }

  public void MakeFollowerMutatedAndBeginCombat()
  {
    this.StartCoroutine((IEnumerator) this.Make3FollowersMutatedIE((System.Action) (() => { })));
  }

  public IEnumerator Make3FollowersMutatedIE(System.Action callback = null)
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    dungeonLeaderMechanics.waiting = true;
    List<FollowerInfo> possibleFollowers = new List<FollowerInfo>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      if (dungeonLeaderMechanics.CanMutateFollower(follower) && possibleFollowers.Count < 6 && !FollowerManager.UniqueFollowerIDs.Contains(follower.ID))
        possibleFollowers.Add(follower);
    }
    if (possibleFollowers.Count > 0)
    {
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      int total = Mathf.Min(possibleFollowers.Count, 3);
      float angleBetween = 180f / (float) (total - 1);
      float startingAngle = -180f;
      float delay = 1f / (float) possibleFollowers.Count;
      for (int i = 0; i < total; ++i)
      {
        FollowerManager.SpawnedFollower spawnedFollower = dungeonLeaderMechanics.SpawnFollower(possibleFollowers[i], dungeonLeaderMechanics.transform.position + Vector3.right * 2f);
        GameManager.GetInstance().AddToCamera(spawnedFollower.Follower.gameObject);
        Vector3 fromPosition = dungeonLeaderMechanics.transform.position + (Vector3) Utils.DegreeToVector2(startingAngle) * 3.5f + Vector3.down;
        spawnedFollower.Follower.transform.position = fromPosition;
        spawnedFollower.Follower.State.LookAngle = spawnedFollower.Follower.State.facingAngle = Utils.GetAngle(fromPosition, dungeonLeaderMechanics.transform.position);
        dungeonLeaderMechanics.StartCoroutine((IEnumerator) dungeonLeaderMechanics.MakeFollowerMutated(spawnedFollower, (float) (1.0 - (double) delay * (double) (i + 1) + 0.5)));
        startingAngle = Utils.Repeat(startingAngle + angleBetween, 360f);
        yield return (object) new WaitForSeconds(delay);
      }
      yield return (object) new WaitForSeconds(5f);
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().OnConversationEnd();
      System.Action action = callback;
      if (action != null)
        action();
      dungeonLeaderMechanics.completed = true;
      RoomLockController.RoomCompleted();
      dungeonLeaderMechanics.FadeRedAway();
    }
    else
      dungeonLeaderMechanics.BeginEnemyRounds();
    System.Action action1 = callback;
    if (action1 != null)
      action1();
    dungeonLeaderMechanics.waiting = false;
  }

  public bool CanMutateFollower(FollowerInfo follower)
  {
    return (double) follower.Illness <= 0.0 && follower.CursedState == Thought.None && !FollowerManager.FollowerLocked(in follower.ID) && !follower.Traits.Contains(FollowerTrait.TraitType.Mutated) && !follower.Traits.Contains(FollowerTrait.TraitType.MutatedVisual) && !follower.Traits.Contains(FollowerTrait.TraitType.MutatedImmune);
  }

  public void SetTraps() => this.StartCoroutine((IEnumerator) this.SetTrapsIE());

  public IEnumerator SetTrapsIE()
  {
    while (PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Idle && PlayerFarming.Instance.state.CURRENT_STATE != StateMachine.State.Moving)
      yield return (object) null;
    for (int x = 0; x < this.traps.Count; ++x)
    {
      this.traps[x].SetActive(true);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.traps[x].transform.position);
      yield return (object) new WaitForSeconds(0.2f);
    }
  }

  public FollowerManager.SpawnedFollower SpawnFollower(FollowerInfo followerInfo, Vector3 position)
  {
    FollowerManager.SpawnedFollower spawnedFollower = FollowerManager.SpawnCopyFollower(this.isCombatFollower ? FollowerManager.CombatFollowerPrefab : FollowerManager.FollowerPrefab, followerInfo, position, this.transform.parent, BiomeGenerator.Instance.DungeonLocation);
    spawnedFollower.Follower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    spawnedFollower.Follower.Spine.AnimationState.SetAnimation(1, "spawn-in", false);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Reactions/react-worried1", true, 0.0f);
    spawnedFollower.Follower.Spine.AnimationState.AddAnimation(1, "Conversations/idle-hate", true, 0.0f);
    if (this.isCombatFollower)
    {
      spawnedFollower.Follower.GetComponent<EnemyFollower>().CanShoot = true;
      if (followerInfo.Traits.Contains(FollowerTrait.TraitType.Mutated))
        spawnedFollower.Follower.GetComponent<DropLootOnDeath>().LootToDrop = InventoryItem.ITEM_TYPE.MAGMA_STONE;
    }
    return spawnedFollower;
  }

  public void ExecutionerOutro() => this.StartCoroutine((IEnumerator) this.ExecutionerOutroIE());

  public IEnumerator ExecutionerOutroIE()
  {
    DungeonLeaderMechanics dungeonLeaderMechanics = this;
    Vector3 exitPosition = LocationManager.LocationManagers[PlayerFarming.Location].GetExitPosition(FollowerLocation.Base);
    if ((double) exitPosition.x < (double) dungeonLeaderMechanics.transform.position.x)
      dungeonLeaderMechanics.transform.localScale = new Vector3(dungeonLeaderMechanics.transform.localScale.x * -1f, dungeonLeaderMechanics.transform.localScale.y, dungeonLeaderMechanics.transform.localScale.z);
    dungeonLeaderMechanics.leaderSpine.AnimationState.SetAnimation(0, "run-fast", true);
    dungeonLeaderMechanics.leaderSpine.transform.DOMove(exitPosition, 2f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
    yield return (object) new WaitForSeconds(1.5f);
    CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 0.3f);
    AudioManager.Instance.PlayOneShot("event:/dlc/dungeon06/enemy/miniboss_executioner/story_dungeon_encounter_portal", dungeonLeaderMechanics.leaderSpine.transform.position);
    float t = 0.0f;
    while ((double) (t += Time.deltaTime) < 0.5)
    {
      dungeonLeaderMechanics.leaderSpine.Skeleton.A = Mathf.Lerp(1f, 0.0f, t / 0.5f);
      yield return (object) null;
    }
    dungeonLeaderMechanics.leaderSpine.gameObject.SetActive(false);
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

  public void Dungeon5Encounter1()
  {
    if (CookingData.GetDaysOfFood(DataManager.Instance.items, DataManager.Instance.Followers.Count) > 5)
      return;
    this.question.Line1 = "Conversation_NPC/Story/Dungeon5/Wolf1/5_Alt";
    this.question.AnswerA = "Conversation_NPC/Story/Dungeon5/Wolf1/5_Alt_Choice1";
    this.question.AnswerB = "Conversation_NPC/Story/Dungeon5/Wolf1/5_Alt_Choice2";
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

  [CompilerGenerated]
  public void \u003CDrawnOutIntroIE\u003Eb__79_0()
  {
    if ((UnityEngine.Object) this.goopFloorParticle != (UnityEngine.Object) null)
    {
      SimpleSpineDeactivateAfterPlay component = this.goopFloorParticle.GetComponent<SimpleSpineDeactivateAfterPlay>();
      component.Init = false;
      component.Animation = "leader-stop";
    }
    if (!this.hasEnemyRounds)
    {
      RoomLockController.RoomCompleted();
      this.FadeRedAway();
    }
    else
    {
      if (this.spawnsFollower || this.hasQuestion)
        return;
      Health.team2.Clear();
      this.BeginEnemyRounds();
    }
  }

  [CompilerGenerated]
  public void \u003CInstantSpawnIntroIE\u003Eb__80_0()
  {
    Health.team2.Remove((Health) null);
    if (this.hasEnemyRounds)
      return;
    RoomLockController.RoomCompleted();
    this.FadeRedAway();
  }

  [CompilerGenerated]
  public void \u003CPodiumIntroIE\u003Eb__82_0()
  {
    Health.team2.Remove((Health) null);
    if (this.hasEnemyRounds || this.hasQuestion)
      return;
    RoomLockController.RoomCompleted();
    this.FadeRedAway();
  }

  [CompilerGenerated]
  public void \u003CSpawnDistortionObject\u003Eb__89_0()
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
  }

  [CompilerGenerated]
  public void \u003CBeginEnemyRoundsIE\u003Eb__92_0()
  {
    RoomLockController.RoomCompleted(true);
    Interaction_Chest.Instance?.Reveal();
    this.FadeRedAway();
    for (int index = Health.team2.Count - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null)
      {
        Health.team2[index].invincible = false;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position);
      }
    }
  }

  [CompilerGenerated]
  public void \u003CBeginEnemyRoundsIE\u003Eb__92_1(UnitObject enemy)
  {
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_5)
      return;
    foreach (MeshRenderer componentsInChild in enemy.GetComponentsInChildren<MeshRenderer>())
    {
      if ((UnityEngine.Object) componentsInChild != (UnityEngine.Object) null && (UnityEngine.Object) componentsInChild.sharedMaterial != (UnityEngine.Object) null && !this.modifiedMaterials.Contains(componentsInChild.sharedMaterial))
        this.modifiedMaterials.Add(componentsInChild.sharedMaterial);
    }
    MeshRenderer component = enemy.GetComponent<MeshRenderer>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.sharedMaterial != (UnityEngine.Object) null) || this.modifiedMaterials.Contains(component.sharedMaterial))
      return;
    this.modifiedMaterials.Add(component.sharedMaterial);
  }

  [CompilerGenerated]
  public void \u003CGiveHealthIE\u003Eb__106_0()
  {
    this.waiting = false;
    GameManager.GetInstance().OnConversationEnd();
    this.callback?.Invoke();
    this.completed = true;
    RoomLockController.RoomCompleted();
    this.FadeRedAway();
  }

  [CompilerGenerated]
  public void \u003CAskQuestionIE\u003Eb__109_0()
  {
    this.StartCoroutine((IEnumerator) this.ResponseIE(true, this.question));
  }

  [CompilerGenerated]
  public void \u003CAskQuestionIE\u003Eb__109_1()
  {
    this.StartCoroutine((IEnumerator) this.ResponseIE(false, this.question));
  }

  [Serializable]
  public enum IntroType
  {
    None,
    EndCombat,
    MidCombat,
    BeforeCombat,
  }

  [Serializable]
  public struct Conversation
  {
    public int SpawnedCharacterIndex;
    public string Line;
  }

  [Serializable]
  public struct Question
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
    public int SpeakerEntryIndex;
  }
}
