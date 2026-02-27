// Decompiled with JetBrains decompiler
// Type: FrogBossIntroRitual
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
public class FrogBossIntroRitual : BaseMonoBehaviour
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
  [SerializeField]
  public GameObject healEffect;
  [SerializeField]
  public SkeletonAnimation healEffectSpine;
  public Interaction_WeaponSelectionPodium RelicPodium;
  public SkeletonGraphic SermonOverlay;
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
  public bool healingHeket;
  public GameObject offsetObject;

  public string term1
  {
    get
    {
      return GameManager.Layer2 ? "Conversation_NPC/Story/Dungeon2/Leader1/Boss1_Layer2" : "Conversation_NPC/Story/Dungeon1/Leader2/Boss1";
    }
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
    foreach (GameObject environmentTrap in this.environmentTraps)
      environmentTrap.SetActive(GameManager.Layer2);
    this.healingHeket = DataManager.Instance.HealingHeketQuestActive;
    if (!DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive && !this.healingHeket)
      return;
    this.cultLeader.gameObject.SetActive(false);
    this.frogBoss.gameObject.SetActive(true);
    this.frogBoss.BossIntro.GetComponentInChildren<SkeletonAnimation>().AnimationState.AddAnimation(0, "idle", true, 0.0f);
    foreach (Component enemySpine in this.enemySpines)
      enemySpine.gameObject.SetActive(false);
    foreach (Component surroundingSpine in this.surroundingSpines)
      surroundingSpine.gameObject.SetActive(false);
    this.frogBoss.EnemiesToTrack[0].enabled = false;
    if (!this.healingHeket)
      return;
    this.frogBoss.gameObject.SetActive(false);
  }

  public void Start() => this.StartCoroutine(this.CompleteHealingBishopRoom());

  public void Update()
  {
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null) || this.healingHeket || this.skipped || !this.skippable || MonoSingleton<UIManager>.Instance.MenusBlocked || !InputManager.Gameplay.GetAttackButtonDown() && !DungeonSandboxManager.Active && !DataManager.Instance.RemovedStoryMomentsActive)
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
    this.frogBoss.Play();
    LetterBox.Instance.HideSkipPrompt();
    MMConversation.mmConversation?.Close();
    AmplifyColorEffect component = Camera.main.GetComponent<AmplifyColorEffect>();
    component.LutHighlightTexture = this.originalHighlightLut;
    component.LutTexture = this.originalLut;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/boss/frog/roar", this.frogBoss.gameObject);
    this.skipped = true;
    this.frogBoss.EnemiesToTrack[0].enabled = true;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.triggered || this.skipped || !collision.CompareTag("Player") || MMTransition.IsPlaying)
      return;
    PlayerFarming componentInParent = collision.GetComponentInParent<PlayerFarming>();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = componentInParent;
    if (this.healingHeket)
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
    FrogBossIntroRitual frogBossIntroRitual = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.InActive;
    if ((bool) (UnityEngine.Object) MMConversation.mmConversation)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) MMConversation.mmConversation.gameObject);
      MMConversation.ClearConversation();
    }
    SimulationManager.Pause();
    HUD_Manager.Instance.HideTopRight();
    frogBossIntroRitual.originalHighlightLut = frogBossIntroRitual.amplifyColorEffect.LutHighlightTexture;
    frogBossIntroRitual.originalLut = frogBossIntroRitual.amplifyColorEffect.LutTexture;
    frogBossIntroRitual.amplifyColorEffect.LutHighlightBlendTexture = (Texture) frogBossIntroRitual.lutTexture;
    frogBossIntroRitual.triggered = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_intro_zoom");
    if (DungeonSandboxManager.Active || DataManager.Instance.RemovedStoryMomentsActive)
    {
      frogBossIntroRitual.skippable = true;
    }
    else
    {
      List<ConversationEntry> Entries = new List<ConversationEntry>()
      {
        new ConversationEntry(frogBossIntroRitual.offsetObject, frogBossIntroRitual.term1),
        new ConversationEntry(frogBossIntroRitual.offsetObject, "Conversation_NPC/Story/Dungeon1/Leader2/Boss2")
      };
      Entries[0].CharacterName = "NAMES/CultLeaders/Dungeon2";
      Entries[0].Offset = new Vector3(0.0f, 3f, 0.0f);
      Entries[0].Animation = "talk";
      Entries[0].SkeletonData = frogBossIntroRitual.cultLeaderSpine;
      Entries[1].CharacterName = "NAMES/CultLeaders/Dungeon2";
      Entries[1].Offset = new Vector3(0.0f, 3f, 0.0f);
      Entries[1].Animation = "talk";
      Entries[1].SkeletonData = frogBossIntroRitual.cultLeaderSpine;
      foreach (ConversationEntry conversationEntry in Entries)
        conversationEntry.soundPath = !GameManager.Layer2 ? "event:/dialogue/dun2_cult_leader_heket/standard_heket" : "event:/dialogue/dun2_cult_leader_heket/undead_standard_heket";
      if (GameManager.Layer2)
        Entries.RemoveAt(1);
      MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null), false, SetPlayerIdleOnComplete: false, showControlPrompt: false);
      MMConversation.mmConversation.SpeechBubble.ScreenOffset = 350f;
      yield return (object) new WaitForSeconds(1f);
      frogBossIntroRitual.skippable = DataManager.Instance.BossesEncountered.Contains(PlayerFarming.Location);
      if (frogBossIntroRitual.skippable && !frogBossIntroRitual.skipped)
        LetterBox.Instance.ShowSkipPrompt();
      while (MMConversation.CURRENT_CONVERSATION != null)
        yield return (object) null;
      UnityEngine.Object.Destroy((UnityEngine.Object) frogBossIntroRitual.offsetObject);
      yield return (object) new WaitForEndOfFrame();
      AudioManager.Instance.PlayOneShot("event:/boss/frog/transition_zoom_back");
      AudioManager.Instance.PlayOneShot("event:/boss/frog/after_intro_grunt");
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.cameraTarget, 12f);
      yield return (object) new WaitForSeconds(1f);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/cultist_sequence");
      foreach (Component enemySpine in frogBossIntroRitual.enemySpines)
        enemySpine.GetComponentInChildren<WorshipperBubble>(true).Play(WorshipperBubble.SPEECH_TYPE.BOSSCROWN2, 4.5f, UnityEngine.Random.Range(0.0f, 0.3f));
      foreach (SkeletonAnimation enemySpine in frogBossIntroRitual.enemySpines)
        enemySpine.AnimationState.SetAnimation(0, "dance", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
      yield return (object) new WaitForSeconds(3f);
      foreach (SkeletonAnimation enemySpine in frogBossIntroRitual.enemySpines)
        enemySpine.AnimationState.SetAnimation(0, "ritual/sacrifice", false).TrackTime = UnityEngine.Random.Range(0.0f, 0.3f);
      yield return (object) new WaitForSeconds(1f);
      frogBossIntroRitual.cultLeaderSpine.AnimationState.SetAnimation(0, "transform", false);
      AudioManager.Instance.PlayOneShot("event:/boss/frog/transform_sequence");
      yield return (object) new WaitForSeconds(0.3f);
      foreach (SkeletonAnimation surroundingSpine in frogBossIntroRitual.surroundingSpines)
        surroundingSpine.AnimationState.SetAnimation(0, "worship", true).TrackTime = UnityEngine.Random.Range(0.0f, 0.4f);
      foreach (GameObject bloodSpray in frogBossIntroRitual.bloodSprays)
      {
        bloodSpray.SetActive(true);
        AudioManager.Instance.PlayOneShot("event:/enemy/impact_squishy", bloodSpray.gameObject);
        float z = Vector3.Angle(bloodSpray.transform.position, frogBossIntroRitual.cultLeader.transform.position);
        BiomeConstants.Instance.EmitBloodSplatter(bloodSpray.transform.position, new Vector3(0.0f, 0.0f, z), Color.black);
      }
      foreach (Renderer renderer in frogBossIntroRitual.blood)
        renderer.material.DOFloat(4f, "_BlotchMultiply", 4f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine);
      GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.cameraTarget, 10f);
      frogBossIntroRitual.tweens.Add((Tween) DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 6f, 6f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InSine));
      Camera.main.GetComponent<AmplifyColorEffect>().BlendTo((Texture) frogBossIntroRitual.lutTexture, 6f, (System.Action) null);
      for (int index = 0; index < frogBossIntroRitual.enemySpines.Length; ++index)
        frogBossIntroRitual.StartCoroutine(frogBossIntroRitual.SpawnSouls(frogBossIntroRitual.enemySpines[index].transform.position));
      yield return (object) new WaitForSeconds(1f);
      frogBossIntroRitual.bloodParticle.SetActive(true);
      yield return (object) new WaitForSeconds(2f);
      frogBossIntroRitual.symbols.material.DOFloat(5f, "_BlotchMultiply", 5f);
      frogBossIntroRitual.BloodPortalEffect.transform.DOScale(new Vector3(5.3f, 2f, 5.3f), 2f);
      CameraManager.instance.ShakeCameraForDuration(1f, 1.5f, 10f);
      BiomeConstants.Instance.ImpactFrameForDuration();
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      yield return (object) new WaitForSeconds(4f);
      frogBossIntroRitual.BloodPortalEffect.transform.DOScale(Vector3.zero, 2f);
    }
  }

  public IEnumerator SpawnSouls(Vector3 position)
  {
    FrogBossIntroRitual frogBossIntroRitual = this;
    float delay = 0.3f;
    int ParticleCount = 50;
    for (int i = 0; i < ParticleCount; ++i)
    {
      float time = (float) i / (float) ParticleCount;
      delay *= 1f - frogBossIntroRitual.absorbSoulCurve.Evaluate(time);
      SoulCustomTarget.Create(frogBossIntroRitual.cameraTarget, new Vector3(position.x, position.y, position.z + 1f), Color.red, (System.Action) null, 0.2f, (float) (100.0 * (1.0 + (double) frogBossIntroRitual.absorbSoulCurve.Evaluate(time)))).transform.parent = frogBossIntroRitual.transform;
      yield return (object) new WaitForSeconds(delay);
    }
  }

  public IEnumerator BossTransformed()
  {
    FrogBossIntroRitual frogBossIntroRitual = this;
    frogBossIntroRitual.skippable = false;
    frogBossIntroRitual.cultLeader.SetActive(false);
    frogBossIntroRitual.frogBoss.gameObject.SetActive(true);
    frogBossIntroRitual.frogBoss.Play();
    if ((UnityEngine.Object) frogBossIntroRitual.playerBehind != (UnityEngine.Object) null && frogBossIntroRitual.playerBehind.GoToAndStopping)
    {
      frogBossIntroRitual.playerBehind.EndGoToAndStop();
      frogBossIntroRitual.playerBehind.transform.position = frogBossIntroRitual.playerBehindTargetPosition;
    }
    frogBossIntroRitual.distortionObject.DOScale(50f, 5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(frogBossIntroRitual.\u003CBossTransformed\u003Eb__43_0));
    AmplifyColorEffect component = Camera.main.GetComponent<AmplifyColorEffect>();
    component.LutHighlightTexture = frogBossIntroRitual.originalHighlightLut;
    component.LutTexture = frogBossIntroRitual.originalLut;
    CameraManager.instance.StopAllCoroutines();
    yield return (object) new WaitForEndOfFrame();
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 0.3f);
    yield return (object) new WaitForSeconds(0.1f);
    foreach (SkeletonAnimation enemySpine in frogBossIntroRitual.enemySpines)
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
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(enemySpine.transform.position, Vector3.zero, frogBossIntroRitual.bloodColor);
      enemySpine.gameObject.SetActive(false);
      frogBossIntroRitual.SpawnDeadBody(enemySpine.transform.position, enemySpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (SkeletonAnimation surroundingSpine in frogBossIntroRitual.surroundingSpines)
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
      BiomeConstants.Instance.EmitBloodSplatterGroundParticles(surroundingSpine.transform.position, Vector3.zero, frogBossIntroRitual.bloodColor);
      surroundingSpine.gameObject.SetActive(false);
      frogBossIntroRitual.SpawnDeadBody(surroundingSpine.transform.position, surroundingSpine.transform.localScale);
    }
    yield return (object) new WaitForSeconds(0.15f);
    foreach (LongGrass longGrass in frogBossIntroRitual.surroundingGrass)
      longGrass.StartCoroutine(longGrass.ShakeGrassRoutine(frogBossIntroRitual.gameObject, 2f));
  }

  public IEnumerator CompleteHealingBishopRoom()
  {
    if (this.healingHeket)
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
    UnityEngine.Object.Instantiate<GameObject>(this.deathParticlePrefab, pos, Quaternion.identity, this.transform.parent);
    DeadBodySliding deadBodySliding = UnityEngine.Object.Instantiate<DeadBodySliding>(this.enemyBody, pos, Quaternion.identity, this.transform.parent);
    deadBodySliding.Init(this.gameObject, UnityEngine.Random.Range(0.0f, 360f), (float) UnityEngine.Random.Range(500, 1000));
    deadBodySliding.transform.localScale = scale;
  }

  public IEnumerator HealingIE()
  {
    FrogBossIntroRitual frogBossIntroRitual = this;
    frogBossIntroRitual.triggered = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.gameObject);
    Demon_Spirit demon = (Demon_Spirit) null;
    foreach (GameObject demon1 in Demon_Arrows.Demons)
    {
      Demon_Spirit component = demon1.GetComponent<Demon_Spirit>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.FollowerInfo.ID == 99991)
      {
        demon = component;
        break;
      }
    }
    demon.enabled = false;
    demon.spine.transform.DOMove(Vector3.zero, 3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
    yield return (object) new WaitForSeconds(3f);
    GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.gameObject, 6f);
    if ((UnityEngine.Object) frogBossIntroRitual.playerBehind != (UnityEngine.Object) null && frogBossIntroRitual.playerBehind.GoToAndStopping)
      frogBossIntroRitual.playerBehind.transform.position = frogBossIntroRitual.playerBehindTargetPosition;
    yield return (object) new WaitForSeconds(1f);
    demon.spine.AnimationState.SetAnimation(0, "transform", false);
    AudioManager.Instance.PlayOneShot("event:/relics/demon_spawn");
    yield return (object) new WaitForSeconds(0.5f);
    FollowerManager.SpawnedFollower follower = FollowerManager.SpawnCopyFollower(FollowerInfo.GetInfoByID(99991), Vector3.zero, frogBossIntroRitual.transform, FollowerLocation.Dungeon1_1);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", frogBossIntroRitual.gameObject);
    follower.Follower.OverridingEmotions = true;
    follower.Follower.SetFaceAnimation("Emotions/emotion-angry", true);
    double num1 = (double) follower.Follower.SetBodyAnimation("Conversations/react-mean1", false);
    follower.Follower.AddBodyAnimation("idle", true, 0.0f);
    MMConversation.ClearEventListenerSFX(follower.Follower.gameObject, "VO/talk short nice");
    yield return (object) new WaitForSeconds(2f);
    List<ConversationEntry> Entries1 = new List<ConversationEntry>()
    {
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Second/0"),
      new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Second/1")
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
      conversationEntry.Animation = "Conversations/talk-mean" + UnityEngine.Random.Range(1, 3).ToString();
      conversationEntry.LoopAnimation = false;
      conversationEntry.soundPath = " ";
    }
    MMConversation.Play(new ConversationObject(Entries1, Responses, (System.Action) null), false);
    while (MMConversation.isPlaying || !answered)
      yield return (object) null;
    if (accepted)
    {
      bool changedBishopSkin = !follower.FollowerBrain.Info.SkinName.Contains("CultLeader");
      AudioManager.Instance.PlayOneShot("event:/Stings/bishop_healed", frogBossIntroRitual.gameObject);
      GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.gameObject, 6f);
      frogBossIntroRitual.healEffect.gameObject.SetActive(true);
      frogBossIntroRitual.healEffectSpine.AnimationState.SetAnimation(0, "animation", false);
      frogBossIntroRitual.healEffectSpine.AnimationState.AddAnimation(0, "off", true, 0.0f);
      double num2 = (double) follower.Follower.SetBodyAnimation("bishop-heal", false);
      follower.Follower.AddBodyAnimation("idle", true, 0.0f);
      frogBossIntroRitual.SermonOverlay.gameObject.SetActive(true);
      frogBossIntroRitual.SermonOverlay.AnimationState.SetAnimation(0, "1", false);
      yield return (object) new WaitForSeconds(1.15f);
      yield return (object) new WaitForSeconds(1.55f);
      DataManager.Instance.HeketHealed = true;
      follower.FollowerBrain.Info.SkinName = "CultLeader 2 Healed";
      follower.FollowerBrain.Info.SkinCharacter = WorshipperData.Instance.GetSkinIndexFromName(follower.FollowerBrain.Info.SkinName);
      FollowerBrain.SetFollowerCostume(follower.Follower.Spine.Skeleton, follower.FollowerBrain._directInfoAccess);
      BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
      AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", frogBossIntroRitual.gameObject);
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
      frogBossIntroRitual.healEffect.gameObject.SetActive(false);
      List<ConversationEntry> Entries2 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Give/0"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Give/1"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Give/2"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Give/3"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Give/4"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Give/5")
      };
      foreach (ConversationEntry conversationEntry in Entries2)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-mean" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = " ";
      }
      MMConversation.Play(new ConversationObject(Entries2, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
      GameManager.GetInstance().OnConversationNext(frogBossIntroRitual.gameObject, 5f);
      double num3 = (double) follower.Follower.SetBodyAnimation("Reactions/react-bow", false);
      yield return (object) new WaitForSeconds(1.93333328f);
    }
    else
    {
      List<ConversationEntry> Entries3 = new List<ConversationEntry>()
      {
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Deny/0"),
        new ConversationEntry(follower.Follower.gameObject, "Conversation_NPC/Heket/Healing/Deny/1")
      };
      foreach (ConversationEntry conversationEntry in Entries3)
      {
        conversationEntry.CharacterName = follower.FollowerFakeInfo.Name;
        conversationEntry.Animation = "Conversations/talk-mean" + UnityEngine.Random.Range(1, 3).ToString();
        conversationEntry.LoopAnimation = false;
        conversationEntry.soundPath = " ";
      }
      MMConversation.Play(new ConversationObject(Entries3, (List<MMTools.Response>) null, (System.Action) null), false);
      while (MMConversation.isPlaying)
        yield return (object) null;
    }
    FollowerManager.CleanUpCopyFollower(follower);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(Vector3.zero);
    AudioManager.Instance.PlayOneShot("event:/relics/puff_of_smoke", frogBossIntroRitual.gameObject);
    BiomeConstants.Instance.ShakeCamera();
    CameraManager.instance.ShakeCameraForDuration(0.0f, 1f, 0.5f);
    demon.gameObject.SetActive(true);
    demon.transform.position = Vector3.zero;
    demon.enabled = true;
    if (accepted)
      frogBossIntroRitual.DoRelicUnlock(RelicType.IncreaseDamageForDuration);
    ObjectiveManager.CompleteCustomObjective(Objectives.CustomQuestTypes.HealingBishop_Heket, 99991);
    DataManager.Instance.GaveHeketHealingQuest = true;
    yield return (object) new WaitForSeconds(1f);
    DataManager.Instance.HeketHealQuestCompleted = true;
    GameManager.GetInstance().OnConversationEnd();
    RoomLockController.RoomCompleted();
    frogBossIntroRitual.SermonOverlay.gameObject.SetActive(false);
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
  public void \u003CBossTransformed\u003Eb__43_0()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.distortionObject.gameObject);
  }
}
