// Decompiled with JetBrains decompiler
// Type: WormBossIntroRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine;
using Spine.Unity;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WormBossIntroRitual : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject cultLeader;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public SkeletonAnimation cultLeaderSpine;
  [SerializeField]
  public Renderer[] blood;
  [SerializeField]
  public GameObject bloodParticle;
  [SerializeField]
  public Renderer symbols;
  [SerializeField]
  public AnimationCurve absorbSoulCurve;
  [SerializeField]
  public Texture2D lutTexture;
  [SerializeField]
  public Transform distortionObject;
  [Space]
  [SerializeField]
  public DeadBodySliding enemyBody;
  [SerializeField]
  public GameObject deathParticlePrefab;
  [SerializeField]
  public GameObject[] bloodSprays;
  [SerializeField]
  public SkeletonAnimation[] enemySpines;
  [SerializeField]
  public SkeletonAnimation[] surroundingSpines;
  [Space]
  [SerializeField]
  public MiniBossController frogBoss;
  [SerializeField]
  public GameObject[] environmentTraps;
  public LongGrass[] surroundingGrass;
  public GameObject BloodPortalEffect;
  public Interaction_WeaponSelectionPodium RelicPodium;
  public SkeletonGraphic SermonOverlay;
  [SerializeField]
  public GameObject healEffect;
  [SerializeField]
  public SkeletonAnimation healEffectSpine;
  public Texture originalLut;
  public Texture originalHighlightLut;
  public int camZoom = 10;
  public bool triggered;
  public PlayerFarming playerBehind;
  public Vector3 playerBehindTargetPosition;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  public bool skippable;
  public bool skipped;
  public List<Tween> tweens = new List<Tween>();
  public Camera mainCamera;
  public AmplifyColorEffect amplifyColorEffect;
  public bool healingLeshy;
  public GameObject offsetObject;

  public string term1
  {
    get
    {
      return GameManager.Layer2 ? "Conversation_NPC/Story/Dungeon1/Leader1/Boss1_Layer2" : "Conversation_NPC/Story/Dungeon1/Leader1/Boss1";
    }
  }

  public string term2
  {
    get
    {
      return GameManager.Layer2 ? "Conversation_NPC/Story/Dungeon1/Leader1/Boss2_Layer2" : "Conversation_NPC/Story/Dungeon1/Leader1/Boss2";
    }
  }

  public void Preload()
  {
    int initialPoolSize = this.enemySpines.Length + this.surroundingSpines.Length;
    ObjectPool.CreatePool(this.deathParticlePrefab, initialPoolSize);
    ObjectPool.CreatePool<DeadBodySliding>(this.enemyBody, initialPoolSize);
    SoulCustomTarget.CreatePool(this.enemySpines.Length * 10);
  }

  public void Awake()
  {
    foreach (Renderer renderer in this.blood)
      renderer.material.SetFloat("_BlotchMultiply", 0.0f);
    this.symbols.material.SetFloat("_BlotchMultiply", 0.0f);
    foreach (GameObject bloodSpray in this.bloodSprays)
      bloodSpray.SetActive(false);
    this.cultLeaderSpine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.LeaderEvent);
    this.surroundingGrass = this.transform.parent.GetComponentsInChildren<LongGrass>();
    this.amplifyColorEffect = Camera.main.GetComponent<AmplifyColorEffect>();
    string skinName = GameManager.Layer2 ? "Beaten" : "Normal";
    this.frogBoss.BossIntro.GetComponentInChildren<SkeletonAnimation>().Skeleton.SetSkin(skinName);
    this.cultLeaderSpine.Skeleton.SetSkin(skinName);
    this.healingLeshy = DataManager.Instance.HealingLeshyQuestActive;
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive || this.healingLeshy)
    {
      this.cultLeader.gameObject.SetActive(false);
      this.frogBoss.gameObject.SetActive(true);
      this.frogBoss.BossIntro.GetComponentInChildren<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0.0f);
      foreach (Component enemySpine in this.enemySpines)
        enemySpine.gameObject.SetActive(false);
      foreach (Component surroundingSpine in this.surroundingSpines)
        surroundingSpine.gameObject.SetActive(false);
      this.frogBoss.EnemiesToTrack[0].enabled = false;
      if (this.healingLeshy)
        this.frogBoss.gameObject.SetActive(false);
    }
    foreach (GameObject environmentTrap in this.environmentTraps)
      environmentTrap.SetActive(GameManager.Layer2);
    this.mainCamera = Camera.main;
    this.amplifyColorEffect = this.mainCamera.GetComponent<AmplifyColorEffect>();
    BiomeConstants.Instance.CreateBossPools();
  }

  public IEnumerator Start()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    WormBossIntroRitual wormBossIntroRitual = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      wormBossIntroRitual.frogBoss.EnemiesToTrack[0].gameObject.GetComponent<EnemyWormBoss>();
      wormBossIntroRitual.Preload();
      wormBossIntroRitual.StartCoroutine(wormBossIntroRitual.CompleteHealingBishopRoom());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) null;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnEnable()
  {
    this.frogBoss.EnemiesToTrack[0].gameObject.GetComponent<EnemyWormBoss>().Preload();
  }

  public void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || this.healingLeshy || this.skipped || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked || !InputManager.Gameplay.GetAttackButtonDown() && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      return;
    this.StopAllCoroutines();
    this.gameObject.SetActive(false);
    CameraManager.instance.Stopshake();
    foreach (Tween tween in this.tweens)
      tween.Kill();
    if ((UnityEngine.Object) this.playerBehind != (UnityEngine.Object) null)
    {
      if (this.playerBehind.GoToAndStopping)
      {
        this.playerBehind.EndGoToAndStop();
        this.playerBehind.transform.position = this.playerBehindTargetPosition;
      }
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    }
    this.frogBoss.gameObject.SetActive(true);
    this.frogBoss.Play(true);
    AudioManager.Instance.PlayOneShot("event:/boss/worm/roar", this.frogBoss.gameObject);
    MMConversation.mmConversation?.Close();
    LetterBox.Instance.HideSkipPrompt();
    AmplifyColorEffect amplifyColorEffect = this.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = this.originalHighlightLut;
    amplifyColorEffect.LutTexture = this.originalLut;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    this.skipped = true;
    this.frogBoss.EnemiesToTrack[0].enabled = true;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || this.skipped || !collision.CompareTag("Player") || MMTransition.IsPlaying)
      return;
    PlayerFarming componentInParent = collision.GetComponentInParent<PlayerFarming>();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = componentInParent;
    if (this.healingLeshy)
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        player.GoToAndStop(Vector3.down * 3f, DisableCollider: true);
        if ((UnityEngine.Object) player != (UnityEngine.Object) componentInParent)
        {
          this.playerBehind = player;
          this.playerBehindTargetPosition = Vector3.down * 6f;
        }
      }
      this.StartCoroutine(this.HealingIE());
    }
    else
    {
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        if ((UnityEngine.Object) player != (UnityEngine.Object) componentInParent)
        {
          this.playerBehind = player;
          player.GoToAndStop(collision.transform.position, DisableCollider: true);
          this.playerBehindTargetPosition = collision.transform.position + Vector3.right * 1.5f;
        }
      }
      this.StartCoroutine(this.RitualRoutine());
    }
  }

  public IEnumerator RitualRoutine()
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    if ((bool) (UnityEngine.Object) MMConversation.mmConversation)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) MMConversation.mmConversation.gameObject);
      MMConversation.ClearConversation();
    }
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    wormBossIntroRitual.originalHighlightLut = wormBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    wormBossIntroRitual.originalLut = wormBossIntroRitual.amplifyColorEffect.LutTexture;
    wormBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) wormBossIntroRitual.lutTexture;
    wormBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
    {
      wormBossIntroRitual.skippable = true;
    }
    else
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>();
      Entries.Add(new ConversationEntry(wormBossIntroRitual.offsetObject, wormBossIntroRitual.term1));
      Entries.Add(new ConversationEntry(wormBossIntroRitual.offsetObject, wormBossIntroRitual.term2));
      string str = !GameManager.Layer2 ? "event:/dialogue/dun1_cult_leader_leshy/standard_leshy" : "event:/dialogue/dun1_cult_leader_leshy/undead_standard_leshy";
      Entries[0].CharacterName = "NAMES/CultLeaders/Dungeon1";
      Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
      Entries[0].soundPath = str;
      Entries[0].Animation = "talk";
      Entries[0].SkeletonData = wormBossIntroRitual.cultLeaderSpine;
      Entries[1].CharacterName = "NAMES/CultLeaders/Dungeon1";
      Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
      Entries[1].soundPath = str;
      Entries[1].Animation = "talk";
      Entries[1].SkeletonData = wormBossIntroRitual.cultLeaderSpine;
      wormBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "talk", true);
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
      yield return (object) new WaitForSeconds(1f);
      wormBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
      if (wormBossIntroRitual.skippable && !wormBossIntroRitual.skipped)
        LetterBox.Instance.ShowSkipPrompt();
      while (MMConversation.CURRENT_CONVERSATION != null)
        yield return (object) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) wormBossIntroRitual.offsetObject);
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
      AudioManager.Instance.PlayOneShot("event:/boss/frog/after_intro_grunt");
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.cultLeaderSpine.gameObject, 12f);
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/cultist_sequence");
      foreach (Component enemySpine in wormBossIntroRitual.enemySpines)
        enemySpine.GetComponentInChildren<WorshipperBubble>(true).Play(WorshipperBubble.SPEECH_TYPE.BOSSCROWN1, 4.5f, UnityEngine.Random.Range(0.0f, 0.3f));
      foreach (SkeletonAnimation enemySpine in wormBossIntroRitual.enemySpines)
        enemySpine.AnimationState.SetAnimation(0, "ritual/preach2", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.15f);
      yield return (object) new WaitForSeconds(2.8f);
      foreach (SkeletonAnimation enemySpine in wormBossIntroRitual.enemySpines)
        enemySpine.AnimationState.SetAnimation(0, "ritual/sacrifice2", false).TrackTime = UnityEngine.Random.Range(0.0f, 0.15f);
      yield return (object) new WaitForSeconds(1f);
      wormBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", false);
      AudioManager.Instance.PlayOneShot("event:/boss/worm/transform");
      yield return (object) new WaitForSeconds(0.3f);
      foreach (SkeletonAnimation surroundingSpine in wormBossIntroRitual.surroundingSpines)
        surroundingSpine.AnimationState.SetAnimation(0, "ritual/preach", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.4f);
      foreach (GameObject bloodSpray in wormBossIntroRitual.bloodSprays)
      {
        bloodSpray.SetActive(true);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", bloodSpray.gameObject);
        float z = Vector3.Angle(bloodSpray.transform.position, wormBossIntroRitual.cultLeader.transform.position);
        BiomeConstants.Instance.EmitBloodSplatter(bloodSpray.transform.position, new Vector3(0.0f, 0.0f, z), Color.black);
      }
      foreach (Renderer renderer in wormBossIntroRitual.blood)
        renderer.material.DOFloat(4f, "_BlotchMultiply", 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
      GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.cultLeaderSpine.gameObject, 10f);
      wormBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
      wormBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<Vector3>) (() => GameManager.GetInstance().CamFollowTarget.TargetOffset), (DOSetter<Vector3>) (x => GameManager.GetInstance().CamFollowTarget.TargetOffset = x), Vector3.forward * -2f, 6f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine));
      wormBossIntroRitual.amplifyColorEffect.BlendTo((Texture) wormBossIntroRitual.lutTexture, 6f, (System.Action) null);
      for (int index = 0; index < wormBossIntroRitual.enemySpines.Length; ++index)
        wormBossIntroRitual.StartCoroutine(wormBossIntroRitual.SpawnSouls(wormBossIntroRitual.enemySpines[index].transform.position));
      yield return (object) new WaitForSeconds(1f);
      wormBossIntroRitual.bloodParticle.SetActive(true);
      yield return (object) new WaitForSeconds(2f);
      wormBossIntroRitual.symbols.material.DOFloat(5f, "_BlotchMultiply", 5f);
      wormBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      yield return (object) new WaitForSeconds(4f);
      wormBossIntroRitual.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
    }
  }

  public IEnumerator SpawnSouls(Vector3 position)
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - wormBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(wormBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) wormBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = wormBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public IEnumerator BossTransformed()
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    wormBossIntroRitual.skippable = false;
    wormBossIntroRitual.cultLeader.SetActive(false);
    wormBossIntroRitual.frogBoss.gameObject.SetActive(true);
    wormBossIntroRitual.frogBoss.Play();
    if ((UnityEngine.Object) wormBossIntroRitual.playerBehind != (UnityEngine.Object) null && wormBossIntroRitual.playerBehind.GoToAndStopping)
    {
      wormBossIntroRitual.playerBehind.EndGoToAndStop();
      wormBossIntroRitual.playerBehind.transform.position = wormBossIntroRitual.playerBehindTargetPosition;
    }
    GameManager.GetInstance().CamFollowTarget.TargetOffset = Vector3.zero;
    wormBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(wormBossIntroRitual.\u003CBossTransformed\u003Eb__48_0));
    AmplifyColorEffect amplifyColorEffect = wormBossIntroRitual.amplifyColorEffect;
    amplifyColorEffect.LutHighlightTexture = wormBossIntroRitual.originalHighlightLut;
    amplifyColorEffect.LutTexture = wormBossIntroRitual.originalLut;
    CameraManager.instance.StopAllCoroutines();
    yield return (object) new WaitForEndOfFrame();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.3f);
    yield return (object) new WaitForSeconds(0.1f);
    foreach (SkeletonAnimation enemySpine in wormBossIntroRitual.enemySpines)
    {
      string[] strArray = new string[3]
      {
        "BloodImpact_0",
        "BloodImpact_1",
        "BloodImpact_2"
      };
      int index = UnityEngine.Random.Range(0, strArray.Length - 1);
      if (strArray[index] != null)
        BiomeConstants.Instance.EmitBloodImpact(enemySpine.transform.position + Vector3.back * 0.5f, (float) UnityEngine.Random.Range(0, 360), "black", strArray[index]);
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemySpine.transform.position, Vector3.zero, wormBossIntroRitual.bloodColor);
      enemySpine.gameObject.SetActive(false);
      wormBossIntroRitual.SpawnDeadBody(enemySpine.transform.position, enemySpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (SkeletonAnimation surroundingSpine in wormBossIntroRitual.surroundingSpines)
    {
      string[] strArray = new string[3]
      {
        "BloodImpact_0",
        "BloodImpact_1",
        "BloodImpact_2"
      };
      int index = UnityEngine.Random.Range(0, strArray.Length - 1);
      if (strArray[index] != null)
        BiomeConstants.Instance.EmitBloodImpact(surroundingSpine.transform.position + Vector3.back * 0.5f, (float) UnityEngine.Random.Range(0, 360), "black", strArray[index]);
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(surroundingSpine.transform.position, Vector3.zero, wormBossIntroRitual.bloodColor);
      surroundingSpine.gameObject.SetActive(false);
      wormBossIntroRitual.SpawnDeadBody(surroundingSpine.transform.position, surroundingSpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (LongGrass longGrass in wormBossIntroRitual.surroundingGrass)
      longGrass.StartCoroutine(longGrass.ShakeGrassRoutine(wormBossIntroRitual.gameObject, 2f));
  }

  public IEnumerator CompleteHealingBishopRoom()
  {
    if (this.healingLeshy)
    {
      yield return (object) new WaitForEndOfFrame();
      RoomLockController.RoomCompleted(true);
    }
  }

  public void LeaderEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "transform"))
      return;
    this.StartCoroutine(this.BossTransformed());
  }

  public void SpawnDeadBody(Vector3 pos, Vector3 scale)
  {
    ObjectPool.Spawn(this.deathParticlePrefab, this.transform.parent, pos, Quaternion.identity);
    DeadBodySliding deadBodySliding = ObjectPool.Spawn<DeadBodySliding>(this.enemyBody, this.transform.parent, pos, Quaternion.identity);
    deadBodySliding.Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), (float) UnityEngine.Random.Range(500, 1000));
    deadBodySliding.transform.localScale = scale;
  }

  public IEnumerator HealingIE()
  {
    WormBossIntroRitual wormBossIntroRitual = this;
    wormBossIntroRitual.triggered = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.gameObject);
    Demon_Spirit demon = (Demon_Spirit) null;
    foreach (GameObject demon1 in Demon_Arrows.Demons)
    {
      Demon_Spirit component = demon1.GetComponent<Demon_Spirit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.FollowerInfo.ID == 99990)
      {
        demon = component;
        break;
      }
    }
    Vector3 leshyPosition = Vector3.zero;
    demon.enabled = false;
    demon.spine.transform.DOMove(leshyPosition, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.gameObject, 6f);
    if ((UnityEngine.Object) wormBossIntroRitual.playerBehind != (UnityEngine.Object) null && wormBossIntroRitual.playerBehind.GoToAndStopping)
      wormBossIntroRitual.playerBehind.transform.position = wormBossIntroRitual.playerBehindTargetPosition;
    yield return (object) new WaitForSeconds(1f);
    demon.spine.AnimationState.SetAnimation(0, "transform", false);
    AudioManager.Instance.PlayOneShot("event:/relics/demon_spawn");
    yield return (object) new WaitForSeconds(0.5f);
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(FollowerInfo.GetInfoByID(99990), leshyPosition, wormBossIntroRitual.transform, FollowerLocation.Dungeon1_1);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(leshyPosition);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", wormBossIntroRitual.gameObject);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(false);
    double num1 = (double) follower.Follower.SetBodyAnimation("Reactions/react-admire" + UnityEngine.Random.Range(1, 4).ToString(), false);
    follower.Follower.AddBodyAnimation("idle", true, 0.0f);
    MMConversation.ClearEventListenerSFX(follower.Follower.gameObject, "VO/talk short nice");
    yield return (object) new WaitForSeconds(2f);
    List<ConversationEntry> Entries1 = new List<ConversationEntry>()
    {
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Second/0"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Second/1"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Second/2"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Second/3"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Second/4")
    };
    bool accepted = false;
    bool answered = false;
    List<MMTools.Response> Responses = new List<MMTools.Response>()
    {
      new MMTools.Response(ScriptLocalization.UI_Generic.Accept, (System.Action) (() =>
      {
        accepted = true;
        answered = true;
      }), ScriptLocalization.UI_Generic.Accept),
      new MMTools.Response(ScriptLocalization.UI_Generic.Decline, (System.Action) (() =>
      {
        accepted = false;
        answered = true;
      }), ScriptLocalization.UI_Generic.Decline)
    };
    foreach (ConversationEntry conversationEntry in Entries1)
    {
      conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
      conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
      conversationEntry.LoopAnimation = false;
      conversationEntry.soundPath = "event:/dialogue/followers/boss/fol_leshy";
    }
    MMConversation.Play(new ConversationObject(Entries1, Responses, (System.Action) null), false);
    while (MMConversation.isPlaying || !answered)
      yield return (object) null;
    if (accepted)
    {
      bool changedBishopSkin = !follower.FollowerBrain.Info.SkinName.Contains("CultLeader");
      AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", wormBossIntroRitual.gameObject);
      GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.gameObject, 5f);
      wormBossIntroRitual.healEffect.gameObject.SetActive(true);
      wormBossIntroRitual.healEffectSpine.AnimationState.SetAnimation(0, "animation", false);
      wormBossIntroRitual.healEffectSpine.AnimationState.AddAnimation(0, "off", true, 0.0f);
      double num2 = (double) follower.Follower.SetBodyAnimation("bishop-heal", false);
      follower.Follower.AddBodyAnimation("idle", true, 0.0f);
      wormBossIntroRitual.SermonOverlay.gameObject.SetActive(true);
      wormBossIntroRitual.SermonOverlay.AnimationState.SetAnimation(0, "1", false);
      yield return (object) new WaitForSeconds(1.15f);
      yield return (object) new WaitForSeconds(1.55f);
      DataManager.Instance.LeshyHealed = true;
      follower.FollowerBrain.Info.SkinName = "CultLeader 1 Healed";
      follower.FollowerBrain.Info.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(follower.FollowerBrain.Info.SkinName);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain._directInfoAccess);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
      AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", wormBossIntroRitual.gameObject);
      follower.Follower.Spine.Skeleton.SetSkin(follower.FollowerBrain.Info.SkinName);
      WorshipperData.SkinAndData colourData = WorshipperData.Instance.GetColourData(follower.FollowerBrain.Info.SkinName);
      foreach (WorshipperData.SlotAndColor slotAndColour in (changedBishopSkin ? colourData.SlotAndColours[0] : colourData.SlotAndColours[follower.FollowerBrain.Info.SkinColour]).SlotAndColours)
      {
        Slot slot = follower.Follower.Spine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      follower.Follower.Outfit.SetInfo(follower.FollowerBrain._directInfoAccess);
      follower.Follower.Outfit.SetOutfit(follower.Follower.Spine, false);
      follower.FollowerBrain.AddTrait(FollowerTrait.TraitType.BishopOfCult, true);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain.Info.XPLevel, follower.FollowerBrain.Info.SkinName, follower.FollowerBrain.Info.SkinColour, follower.FollowerBrain.Info.Outfit, follower.FollowerBrain.Info.Hat, follower.FollowerBrain.Info.Clothing, follower.FollowerBrain.Info.Customisation, follower.FollowerBrain.Info.Special, follower.FollowerBrain.Info.Necklace, follower.FollowerBrain.Info.ClothingVariant, follower.FollowerBrain._directInfoAccess);
      yield return (object) new WaitForSeconds(2f);
      wormBossIntroRitual.healEffect.gameObject.SetActive(false);
      List<ConversationEntry> Entries2 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Give/0")
      };
      foreach (ConversationEntry conversationEntry in Entries2)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = "event:/dialogue/followers/boss/fol_leshy";
      }
      MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNext(wormBossIntroRitual.gameObject, 5f);
      double num3 = (double) follower.Follower.SetBodyAnimation("Reactions/react-bow", false);
      yield return (object) new WaitForSeconds(1.93333328f);
    }
    else
    {
      List<ConversationEntry> Entries3 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Deny/0"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Leshy/Healing/Deny/1")
      };
      foreach (ConversationEntry conversationEntry in Entries3)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = "event:/dialogue/followers/boss/fol_leshy";
      }
      MMConversation.Play(new ConversationObject(Entries3, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    FollowerManager.CleanUpCopyFollower(follower);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(true);
    demon.transform.position = Vector3.zero;
    demon.enabled = true;
    if (accepted)
      wormBossIntroRitual.DoRelicUnlock(RelicType.DamageOnTouch_Familiar);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.HealingBishop_Leshy, 99990);
    DataManager.Instance.GaveLeshyHealingQuest = true;
    yield return (object) new WaitForSeconds(1f);
    GameManager.GetInstance().OnConversationEnd();
    RoomLockController.RoomCompleted();
    DataManager.Instance.LeshyHealQuestCompleted = true;
    wormBossIntroRitual.SermonOverlay.gameObject.SetActive(false);
  }

  public void DoRelicUnlock(RelicType relicType)
  {
    if (!((UnityEngine.Object) this.RelicPodium != (UnityEngine.Object) null))
      return;
    this.RelicPodium.RemoveIfNotFirstLayer = false;
    this.RelicPodium.ForceSetRelic(relicType);
    this.RelicPodium.Reveal();
  }

  [CompilerGenerated]
  public void \u003CBossTransformed\u003Eb__48_0()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.distortionObject.gameObject);
  }
}
