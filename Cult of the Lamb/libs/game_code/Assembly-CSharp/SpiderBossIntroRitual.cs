// Decompiled with JetBrains decompiler
// Type: SpiderBossIntroRitual
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SpiderBossIntroRitual : BaseMonoBehaviour
{
  [SerializeField]
  public bool skipIntro;
  [Space]
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
  [SerializeField]
  public GameObject transformParticles;
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
  public Interaction_WeaponSelectionPodium RelicPodium;
  public SkeletonGraphic SermonOverlay;
  [Space]
  [SerializeField]
  public MiniBossController spiderBoss;
  [SerializeField]
  public Health spiderBossHealth;
  [SerializeField]
  public GameObject[] environmentTraps;
  public LongGrass[] surroundingGrass;
  public GameObject BloodPortalEffect;
  [SerializeField]
  public GameObject healEffect;
  [SerializeField]
  public SkeletonAnimation healEffectSpine;
  public Texture originalLut;
  public Texture originalHighlightLut;
  public AmplifyColorEffect amplifyColorEffect;
  public int camZoom = 10;
  public bool triggered;
  public PlayerFarming playerBehind;
  public Vector3 playerBehindTargetPosition;
  public Color bloodColor = new Color(0.47f, 0.11f, 0.11f, 1f);
  public bool skippable;
  public bool skipped;
  public List<Tween> tweens = new List<Tween>();
  public bool healingShamura;
  public GameObject offsetObject;

  public string term1
  {
    get
    {
      return GameManager.Layer2 ? "Conversation_NPC/Story/Dungeon4/Leader1/Boss1_Layer2" : "Conversation_NPC/Story/Dungeon1/Leader4/Boss1";
    }
  }

  public string term2
  {
    get
    {
      return GameManager.Layer2 ? "Conversation_NPC/Story/Dungeon4/Leader1/Boss2_Layer2" : "Conversation_NPC/Story/Dungeon1/Leader4/Boss2";
    }
  }

  public void Awake()
  {
    foreach (Renderer renderer in this.blood)
      renderer.material.SetFloat("_BlotchMultiply", 0.0f);
    this.symbols.material.SetFloat("_BlotchMultiply", 0.0f);
    foreach (GameObject bloodSpray in this.bloodSprays)
      bloodSpray.SetActive(false);
    this.surroundingGrass = this.transform.parent.GetComponentsInChildren<LongGrass>();
    this.amplifyColorEffect = Camera.main.GetComponent<AmplifyColorEffect>();
    this.cultLeaderSpine.Skeleton.SetSkin(GameManager.Layer2 ? "Beaten" : "Mask");
    this.spiderBossHealth.untouchable = true;
    foreach (GameObject environmentTrap in this.environmentTraps)
      environmentTrap.SetActive(GameManager.Layer2);
  }

  public void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || this.healingShamura || this.skipped || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked || !InputManager.Gameplay.GetAttackButtonDown() && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
      return;
    this.StopAllCoroutines();
    this.gameObject.SetActive(false);
    CameraManager.instance.Stopshake();
    foreach (Tween tween in this.tweens)
      tween.Kill();
    MMConversation.mmConversation?.Close();
    LetterBox.Instance.HideSkipPrompt();
    this.cultLeaderSpine.AnimationState.SetAnimation(0, "mutate", false).TrackTime = 10.25f;
    AudioManager.Instance.PlayOneShot("event:/boss/spider/roar");
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().StartCoroutine((IEnumerator) this.IntroDone());
    this.skipped = true;
    this.spiderBossHealth.untouchable = false;
    this.spiderBoss.EnemiesToTrack[0].enabled = true;
  }

  public void Start()
  {
    this.healingShamura = DataManager.Instance.HealingShamuraQuestActive;
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive || this.healingShamura)
    {
      this.cultLeaderSpine.AnimationState.SetAnimation(0, "idle-boss", true);
      foreach (Component enemySpine in this.enemySpines)
        enemySpine.gameObject.SetActive(false);
      foreach (Component surroundingSpine in this.surroundingSpines)
        surroundingSpine.gameObject.SetActive(false);
      this.spiderBoss.EnemiesToTrack[0].enabled = false;
      if (this.healingShamura)
        this.spiderBoss.EnemiesToTrack[0].gameObject.SetActive(false);
    }
    else
      this.cultLeaderSpine.AnimationState.SetAnimation(0, "idle", true);
    this.StartCoroutine((IEnumerator) this.CompleteHealingBishopRoom());
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || this.skipped || !collision.CompareTag("Player") || MMTransition.IsPlaying)
      return;
    PlayerFarming componentInParent = collision.GetComponentInParent<PlayerFarming>();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = componentInParent;
    if (this.healingShamura)
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
      this.StartCoroutine((IEnumerator) this.HealingIE());
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
      this.StartCoroutine((IEnumerator) this.RitualRoutine());
    }
  }

  public IEnumerator RitualRoutine()
  {
    SpiderBossIntroRitual spiderBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    if ((bool) (UnityEngine.Object) MMConversation.mmConversation)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) MMConversation.mmConversation.gameObject);
      MMConversation.ClearConversation();
    }
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    spiderBossIntroRitual.originalHighlightLut = spiderBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    spiderBossIntroRitual.originalLut = spiderBossIntroRitual.amplifyColorEffect.LutTexture;
    spiderBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) spiderBossIntroRitual.lutTexture;
    spiderBossIntroRitual.triggered = true;
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
    {
      spiderBossIntroRitual.skippable = true;
    }
    else
    {
      if (!spiderBossIntroRitual.skipIntro)
      {
        if (!DungeonSandboxManager.Active)
          spiderBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "talk", true);
        AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
        List<ConversationEntry> Entries = new List<ConversationEntry>()
        {
          new ConversationEntry(spiderBossIntroRitual.offsetObject, spiderBossIntroRitual.term1),
          new ConversationEntry(spiderBossIntroRitual.offsetObject, spiderBossIntroRitual.term2)
        };
        Entries[0].CharacterName = "NAMES/CultLeaders/Dungeon4";
        Entries[0].Offset = new Vector3(0.0f, 2f, 0.0f);
        Entries[0].Animation = "talk";
        Entries[0].SkeletonData = spiderBossIntroRitual.cultLeaderSpine;
        Entries[1].CharacterName = "NAMES/CultLeaders/Dungeon4";
        Entries[1].Offset = new Vector3(0.0f, 2f, 0.0f);
        Entries[1].Animation = "talk";
        Entries[1].SkeletonData = spiderBossIntroRitual.cultLeaderSpine;
        foreach (ConversationEntry conversationEntry in Entries)
          conversationEntry.soundPath = !GameManager.Layer2 ? "event:/dialogue/dun4_cult_leader_shamura/standard_shamura" : "event:/dialogue/dun4_cult_leader_shamura/undead_standard_shamura";
        MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
        MMConversation.mmConversation.SpeechBubble.ScreenOffset = 350f;
        yield return (object) new WaitForSeconds(1f);
        spiderBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
        if (spiderBossIntroRitual.skippable && !spiderBossIntroRitual.skipped)
          LetterBox.Instance.ShowSkipPrompt();
        while (MMConversation.CURRENT_CONVERSATION != null)
          yield return (object) null;
        spiderBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "idle", true);
        UnityEngine.Object.Destroy((UnityEngine.Object) spiderBossIntroRitual.offsetObject);
        yield return (object) new WaitForEndOfFrame();
        AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
        AudioManager.Instance.PlayOneShot("event:/boss/frog/after_intro_grunt");
        GameManager.GetInstance().OnConversationNew();
        GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.cameraTarget, 12f);
        yield return (object) new WaitForSeconds(1f);
        AudioManager.Instance.PlayOneShot("event:/boss/frog/cultist_sequence");
        foreach (Component enemySpine in spiderBossIntroRitual.enemySpines)
          enemySpine.GetComponentInChildren<WorshipperBubble>(true).Play(WorshipperBubble.SPEECH_TYPE.BOSSCROWN4, 4.5f, UnityEngine.Random.Range(0.0f, 0.3f));
        foreach (SkeletonAnimation enemySpine in spiderBossIntroRitual.enemySpines)
          enemySpine.AnimationState.SetAnimation(0, "dance", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
        yield return (object) new WaitForSeconds(3f);
        foreach (SkeletonAnimation enemySpine in spiderBossIntroRitual.enemySpines)
          enemySpine.AnimationState.SetAnimation(0, "ritual/sacrifice", false).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
        yield return (object) new WaitForSeconds(1f);
        spiderBossIntroRitual.transformParticles.SetActive(true);
        spiderBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "mutate", true);
        AudioManager.Instance.PlayOneShot("event:/boss/spider/transform_sequence", spiderBossIntroRitual.gameObject);
        yield return (object) new WaitForSeconds(0.3f);
        foreach (SkeletonAnimation surroundingSpine in spiderBossIntroRitual.surroundingSpines)
          surroundingSpine.AnimationState.SetAnimation(0, "worship", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.4f);
        foreach (GameObject bloodSpray in spiderBossIntroRitual.bloodSprays)
        {
          bloodSpray.SetActive(true);
          AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", bloodSpray.gameObject);
          float z = Vector3.Angle(bloodSpray.transform.position, spiderBossIntroRitual.cultLeader.transform.position);
          BiomeConstants.Instance.EmitBloodSplatter(bloodSpray.transform.position, new Vector3(0.0f, 0.0f, z), Color.black);
        }
        foreach (Renderer renderer in spiderBossIntroRitual.blood)
          renderer.material.DOFloat(4f, "_BlotchMultiply", 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
        GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.cameraTarget, 10f);
        spiderBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
        Camera.main.GetComponent<AmplifyColorEffect>().BlendTo((Texture) spiderBossIntroRitual.lutTexture, 6f, (System.Action) null);
        for (int index = 0; index < spiderBossIntroRitual.enemySpines.Length; ++index)
          spiderBossIntroRitual.StartCoroutine((IEnumerator) spiderBossIntroRitual.SpawnSouls(spiderBossIntroRitual.enemySpines[index].transform.position));
        yield return (object) new WaitForSeconds(1f);
        spiderBossIntroRitual.bloodParticle.SetActive(true);
        yield return (object) new WaitForSeconds(2f);
        spiderBossIntroRitual.symbols.material.DOFloat(5f, "_BlotchMultiply", 5f);
        spiderBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
        CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
        yield return (object) new WaitForSeconds(4f);
        spiderBossIntroRitual.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
      }
      CameraManager.instance.StopAllCoroutines();
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.cultLeader, 12f);
      spiderBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(spiderBossIntroRitual.\u003CRitualRoutine\u003Eb__46_0));
      AmplifyColorEffect component = Camera.main.GetComponent<AmplifyColorEffect>();
      component.LutHighlightTexture = spiderBossIntroRitual.originalHighlightLut;
      component.LutTexture = spiderBossIntroRitual.originalLut;
      CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.1f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      yield return (object) new WaitForSeconds(0.1f);
      foreach (SkeletonAnimation enemySpine in spiderBossIntroRitual.enemySpines)
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
        BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemySpine.transform.position, Vector3.zero, spiderBossIntroRitual.bloodColor);
        enemySpine.gameObject.SetActive(false);
        spiderBossIntroRitual.SpawnDeadBody(enemySpine.transform.position, enemySpine.transform.localScale);
      }
      yield return (object) new WaitForSeconds(0.15f);
      foreach (SkeletonAnimation surroundingSpine in spiderBossIntroRitual.surroundingSpines)
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
        BiomeConstants.Instance.EmitBloodSplatterGroundParticles(surroundingSpine.transform.position, Vector3.zero, spiderBossIntroRitual.bloodColor);
        surroundingSpine.gameObject.SetActive(false);
        spiderBossIntroRitual.SpawnDeadBody(surroundingSpine.transform.position, surroundingSpine.transform.localScale);
      }
      yield return (object) new WaitForSeconds(0.15f);
      foreach (LongGrass longGrass in spiderBossIntroRitual.surroundingGrass)
        longGrass.StartCoroutine((IEnumerator) longGrass.ShakeGrassRoutine(spiderBossIntroRitual.gameObject, 2f));
      yield return (object) new WaitForSeconds(2.3f);
      spiderBossIntroRitual.StartCoroutine((IEnumerator) spiderBossIntroRitual.IntroDone());
    }
  }

  public IEnumerator IntroDone()
  {
    this.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    GameManager.GetInstance().OnConversationNext(this.cultLeader, 16f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.7f);
    this.cultLeaderSpine.AnimationState.AddAnimation(0, "idle-boss", true, 0.0f);
    if ((UnityEngine.Object) this.playerBehind != (UnityEngine.Object) null)
    {
      if (this.playerBehind.GoToAndStopping)
      {
        this.playerBehind.EndGoToAndStop();
        this.playerBehind.transform.position = this.playerBehindTargetPosition;
      }
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    }
    yield return (object) new WaitForSeconds(2.5f);
    yield return (object) new WaitForEndOfFrame();
    this.spiderBoss.Play();
    this.spiderBossHealth.untouchable = false;
    yield return (object) new WaitForSeconds(0.1f);
    GameManager.GetInstance().OnConversationEnd();
    this.spiderBoss.GetComponentInChildren<EnemySpiderMonster>().Play();
    if (!DataManager.Instance.BossesEncountered.Contains(FollowerLocation.Dungeon1_4))
      DataManager.Instance.BossesEncountered.Add(FollowerLocation.Dungeon1_4);
  }

  public IEnumerator SpawnSouls(Vector3 position)
  {
    SpiderBossIntroRitual spiderBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - spiderBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(spiderBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) spiderBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = spiderBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public IEnumerator CompleteHealingBishopRoom()
  {
    if (this.healingShamura)
    {
      yield return (object) null;
      yield return (object) new WaitForEndOfFrame();
      RoomLockController.RoomCompleted(true);
    }
  }

  public void SpawnDeadBody(Vector3 pos, Vector3 scale)
  {
    UnityEngine.Object.Instantiate<GameObject>(this.deathParticlePrefab, pos, Quaternion.identity, this.transform.parent);
    DeadBodySliding deadBodySliding = UnityEngine.Object.Instantiate<DeadBodySliding>(this.enemyBody, pos, Quaternion.identity, this.transform.parent);
    deadBodySliding.Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), (float) UnityEngine.Random.Range(500, 1000));
    deadBodySliding.transform.localScale = scale;
  }

  public IEnumerator HealingIE()
  {
    SpiderBossIntroRitual spiderBossIntroRitual = this;
    spiderBossIntroRitual.triggered = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.gameObject);
    Demon_Spirit demon = (Demon_Spirit) null;
    foreach (GameObject demon1 in Demon_Arrows.Demons)
    {
      Demon_Spirit component = demon1.GetComponent<Demon_Spirit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.FollowerInfo.ID == 99993)
      {
        demon = component;
        break;
      }
    }
    demon.enabled = false;
    demon.spine.transform.DOMove(Vector3.zero, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.gameObject, 6f);
    if ((UnityEngine.Object) spiderBossIntroRitual.playerBehind != (UnityEngine.Object) null && spiderBossIntroRitual.playerBehind.GoToAndStopping)
      spiderBossIntroRitual.playerBehind.transform.position = spiderBossIntroRitual.playerBehindTargetPosition;
    yield return (object) new WaitForSeconds(1f);
    demon.spine.AnimationState.SetAnimation(0, "transform", false);
    AudioManager.Instance.PlayOneShot("event:/relics/demon_spawn");
    yield return (object) new WaitForSeconds(0.5f);
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(FollowerInfo.GetInfoByID(99993), Vector3.zero, spiderBossIntroRitual.transform, FollowerLocation.Dungeon1_1);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", spiderBossIntroRitual.gameObject);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(false);
    follower.Follower.OverridingEmotions = true;
    follower.Follower.SetFaceAnimation("Emotions/emotion-unhappy", true);
    double num1 = (double) follower.Follower.SetBodyAnimation("Reactions/react-sad", false);
    follower.Follower.AddBodyAnimation("idle", true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    List<ConversationEntry> Entries1 = new List<ConversationEntry>()
    {
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/0"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/1"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/2"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/3"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/4"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/5"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Second/6")
    };
    bool accepted = false;
    bool answered = false;
    List<MMTools.Response> Responses1 = new List<MMTools.Response>()
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
      conversationEntry.soundPath = " ";
    }
    MMConversation.Play(new ConversationObject(Entries1, Responses1, (System.Action) null), false);
    while (MMConversation.isPlaying || !answered)
      yield return (object) null;
    if (accepted)
    {
      AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", spiderBossIntroRitual.gameObject);
      GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.gameObject, 5f);
      spiderBossIntroRitual.healEffect.gameObject.SetActive(true);
      spiderBossIntroRitual.healEffectSpine.AnimationState.SetAnimation(0, "animation", false);
      spiderBossIntroRitual.healEffectSpine.AnimationState.AddAnimation(0, "off", true, 0.0f);
      double num2 = (double) follower.Follower.SetBodyAnimation("bishop-heal", false);
      follower.Follower.AddBodyAnimation("idle", true, 0.0f);
      spiderBossIntroRitual.SermonOverlay.gameObject.SetActive(true);
      spiderBossIntroRitual.SermonOverlay.AnimationState.SetAnimation(0, "1", false);
      yield return (object) new WaitForSeconds(1.15f);
      yield return (object) new WaitForSeconds(1.55f);
      DataManager.Instance.ShamuraHealed = true;
      follower.FollowerBrain.Info.SkinName = "CultLeader 4 Healed";
      follower.FollowerBrain.Info.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(follower.FollowerBrain.Info.SkinName);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain._directInfoAccess);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
      AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", spiderBossIntroRitual.gameObject);
      follower.Follower.Spine.Skeleton.SetSkin(follower.FollowerBrain.Info.SkinName);
      foreach (WorshipperData.SlotAndColor slotAndColour in WorshipperData.Instance.GetColourData(follower.FollowerBrain.Info.SkinName).SlotAndColours[follower.FollowerBrain.Info.SkinColour].SlotAndColours)
      {
        Spine.Slot slot = follower.Follower.Spine.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      follower.Follower.Outfit.SetInfo(follower.FollowerBrain._directInfoAccess);
      follower.Follower.Outfit.SetOutfit(follower.Follower.Spine, false);
      follower.FollowerBrain.AddTrait(FollowerTrait.TraitType.BishopOfCult, true);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain.Info.XPLevel, follower.FollowerBrain.Info.SkinName, follower.FollowerBrain.Info.SkinColour, follower.FollowerBrain.Info.Outfit, follower.FollowerBrain.Info.Hat, follower.FollowerBrain.Info.Clothing, follower.FollowerBrain.Info.Customisation, follower.FollowerBrain.Info.Special, follower.FollowerBrain.Info.Necklace, follower.FollowerBrain.Info.ClothingVariant, follower.FollowerBrain._directInfoAccess);
      yield return (object) new WaitForSeconds(2f);
      spiderBossIntroRitual.healEffect.gameObject.SetActive(false);
      List<ConversationEntry> Entries2 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/0"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/1")
      };
      foreach (ConversationEntry conversationEntry in Entries2)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = " ";
      }
      accepted = false;
      answered = false;
      List<MMTools.Response> Responses2 = new List<MMTools.Response>()
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
      MMConversation.Play(new ConversationObject(Entries2, Responses2, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      if (accepted)
      {
        List<ConversationEntry> Entries3 = new List<ConversationEntry>()
        {
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/3_Give"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/4"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/5"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/6"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/7"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/8"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/9"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/10"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/11"),
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/12")
        };
        foreach (ConversationEntry conversationEntry in Entries3)
        {
          conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
          conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
          conversationEntry.LoopAnimation = false;
          conversationEntry.soundPath = " ";
        }
        MMConversation.Play(new ConversationObject(Entries3, (List<MMTools.Response>) null, (System.Action) null), false);
        while (MMConversation.isPlaying)
          yield return (object) null;
      }
      else
      {
        List<ConversationEntry> Entries4 = new List<ConversationEntry>()
        {
          new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Give/3_Deny")
        };
        foreach (ConversationEntry conversationEntry in Entries4)
        {
          conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
          conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
          conversationEntry.LoopAnimation = false;
          conversationEntry.soundPath = " ";
        }
        MMConversation.Play(new ConversationObject(Entries4, (List<MMTools.Response>) null, (System.Action) null), false);
        while (MMConversation.isPlaying)
          yield return (object) null;
      }
      GameManager.GetInstance().OnConversationNext(spiderBossIntroRitual.gameObject, 5f);
      double num3 = (double) follower.Follower.SetBodyAnimation("Reactions/react-bow", false);
      yield return (object) new WaitForSeconds(1.93333328f);
    }
    else
    {
      List<ConversationEntry> Entries5 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Deny/0"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Deny/1"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Shamura/Healing/Deny/2")
      };
      foreach (ConversationEntry conversationEntry in Entries5)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-nice" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
      }
      MMConversation.Play(new ConversationObject(Entries5, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    FollowerManager.CleanUpCopyFollower(follower);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", spiderBossIntroRitual.gameObject);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(true);
    demon.transform.position = Vector3.zero;
    demon.enabled = true;
    if (accepted)
      spiderBossIntroRitual.DoRelicUnlock(RelicType.GungeonBlank);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.HealingBishop_Shamura, 99993);
    DataManager.Instance.GaveShamuraHealingQuest = true;
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.ShamuraHealQuestCompleted = true;
    GameManager.GetInstance().OnConversationEnd();
    RoomLockController.RoomCompleted();
    spiderBossIntroRitual.SermonOverlay.gameObject.SetActive(false);
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
  public void \u003CRitualRoutine\u003Eb__46_0()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.distortionObject.gameObject);
  }
}
