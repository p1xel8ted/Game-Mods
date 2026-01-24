// Decompiled with JetBrains decompiler
// Type: WolfBossIntro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using MMTools;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class WolfBossIntro : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation normalSpine;
  [SerializeField]
  public GameObject cameraTarget;
  [SerializeField]
  public EnemyWolfBoss wolfBoss;
  [SerializeField]
  public GameObject burstLeftParitcle;
  [SerializeField]
  public GameObject burstRightParitcle;
  [SerializeField]
  public GameObject burstTopParitcle;
  [SerializeField]
  public UnityEngine.Transform roarFXSlot;
  [SerializeField]
  public RotSpread rot;
  [SerializeField]
  public AreaBurnTick rotDamage;
  [SerializeField]
  public TrapLightning[] lightningTraps;
  public bool skippable;
  public bool skipped;
  public string IntroTransformationSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/story_intro_transformation";
  public string IntroTransformationShortSFX = "event:/dlc/dungeon06/enemy/miniboss_marchosias/story_intro_transformation_short";
  public string bossMusic = "event:/music/snowy_mountain/snowy_mountain";
  public EventInstance introTransformationEventInstance;
  public EventInstance rotSpreadLoop;
  public EventInstance rotSpreadMusic;
  public float movementSpeed;

  public void Start() => this.rotDamage.Initialize();

  public void Health_OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.rotDamage.gameObject.SetActive(false);
  }

  public void SetSkippable()
  {
    if (!DataManager.Instance.DiedToWolfBoss)
      return;
    this.skippable = true;
    LetterBox.Instance.ShowSkipPrompt();
  }

  public void DisableLightningTraps()
  {
    foreach (TrapLightning lightningTrap in this.lightningTraps)
      lightningTrap.StopTrap();
  }

  public void OnEnable()
  {
    if (!((Object) GameManager.GetInstance() != (Object) null))
      return;
    if (PlayerFarming.Location == FollowerLocation.Boss_Wolf)
      this.StartCoroutine((IEnumerator) this.InitPlayerWalking());
    this.rotSpreadLoop = AudioManager.Instance.CreateLoop("event:/dlc/env/rot/spread_loop", PlayerFarming.Instance.gameObject, true);
    this.rotSpreadMusic = AudioManager.Instance.CreateLoop("event:/dlc/music/marchosias/rot_spread_loop", true);
  }

  public void OnDestroy()
  {
    if ((Object) this.wolfBoss != (Object) null && (Object) this.wolfBoss.health != (Object) null)
      this.wolfBoss.health.OnDie += new Health.DieAction(this.Health_OnDie);
    AudioManager.Instance.StopLoop(this.rotSpreadLoop);
    AudioManager.Instance.StopLoop(this.rotSpreadMusic);
  }

  public void Update()
  {
    if ((Object) PlayerFarming.Instance != (Object) null && !this.skipped && this.skippable && !MonoSingleton<UIManager>.Instance.MenusBlocked && (InputManager.Gameplay.GetAttackButtonDown() || DungeonSandboxManager.Active))
    {
      this.skipped = true;
      this.StopAllCoroutines();
      CameraManager.instance.Stopshake();
      PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
      this.ResetPlayerWalking();
      MMConversation.mmConversation?.Close();
      LetterBox.Instance.HideSkipPrompt();
      GameManager.GetInstance().CameraSetOffset(Vector3.zero);
      AudioManager.Instance.StopOneShotInstanceEarly(this.introTransformationEventInstance, STOP_MODE.IMMEDIATE);
      AudioManager.Instance.PlayOneShot(this.IntroTransformationShortSFX);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.RoarIE());
    }
    if (!((Object) PlayerFarming.Instance != (Object) null))
      return;
    this.CalculateRotSpreadSFX();
  }

  public void CalculateRotSpreadSFX()
  {
    float num = -17f;
    PlayerFarming playerFarming = PlayerFarming.Instance;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((double) player.transform.position.y < (double) playerFarming.transform.position.y)
        playerFarming = player;
    }
    if ((double) playerFarming.transform.position.y > (double) num)
    {
      AudioManager.Instance.StopLoop(this.rotSpreadLoop);
      AudioManager.Instance.StopLoop(this.rotSpreadMusic);
    }
    else
    {
      this.movementSpeed = Mathf.Lerp(this.movementSpeed, (double) playerFarming.playerController.speed > 0.0 ? 1f : 0.0f, 2f * Time.deltaTime);
      if ((double) playerFarming.playerController.speed <= 0.0 || playerFarming.state.CURRENT_STATE == StateMachine.State.Idle)
        this.movementSpeed = 0.0f;
      AudioManager.Instance.SetEventInstanceParameter(this.rotSpreadLoop, "playerMovementSpeed", this.movementSpeed);
      AudioManager.Instance.SetEventInstanceParameter(this.rotSpreadMusic, "playerMovementSpeed", this.movementSpeed);
    }
  }

  public void Transform() => this.StartCoroutine((IEnumerator) this.TransformIE());

  public IEnumerator TransformIE()
  {
    if (!this.skipped)
    {
      if (this.skippable)
        LetterBox.Instance.ShowSkipPrompt();
      yield return (object) new WaitForEndOfFrame();
      GameManager.GetInstance().OnConversationNew();
      GameManager.GetInstance().OnConversationNext(this.cameraTarget, 7f);
      this.normalSpine.AnimationState.SetAnimation(0, "intro-transform", false);
      this.introTransformationEventInstance = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.IntroTransformationSFX, (UnityEngine.Transform) null, false);
      yield return (object) new WaitForSeconds(1.33333337f);
      GameManager.GetInstance().OnConversationNext(this.cameraTarget, 8f);
      CameraManager.instance.ShakeCameraForDuration(1.2f, 1.4f, 0.2f);
      MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.2f);
      this.burstRightParitcle.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(1.33333337f);
      GameManager.GetInstance().OnConversationNext(this.cameraTarget, 10f);
      CameraManager.instance.ShakeCameraForDuration(1.2f, 1.4f, 0.2f);
      MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 0.2f);
      this.burstLeftParitcle.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(1.66666663f);
      GameManager.GetInstance().OnConversationNext(this.cameraTarget, 12f);
      CameraManager.instance.ShakeCameraForDuration(1.2f, 1.4f, 1f);
      MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 1f);
      this.burstTopParitcle.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(1.66666663f);
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.RoarIE());
    }
  }

  public IEnumerator RoarIE()
  {
    WolfBossIntro wolfBossIntro = this;
    AudioManager.Instance.StopLoop(wolfBossIntro.rotSpreadLoop);
    AudioManager.Instance.StopLoop(wolfBossIntro.rotSpreadMusic);
    wolfBossIntro.rot.Play();
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(wolfBossIntro.cameraTarget, 12f);
    wolfBossIntro.skippable = false;
    LetterBox.Instance.HideSkipPrompt();
    wolfBossIntro.normalSpine.AnimationState.SetAnimation(0, "roar", false);
    wolfBossIntro.normalSpine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    RoomLockController.CloseAll();
    yield return (object) new WaitForEndOfFrame();
    PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
    wolfBossIntro.SetCoopPlayerPosition();
    yield return (object) new WaitForSeconds(0.9f);
    GameManager.GetInstance().OnConversationNext(wolfBossIntro.cameraTarget, 15f);
    CameraManager.instance.ShakeCameraForDuration(1.5f, 2f, 2.4333334f);
    MMVibrate.RumbleForAllPlayers(1.5f, 1.75f, 2.4333334f);
    BiomeConstants.Instance.ImpactFrameForDuration();
    wolfBossIntro.RoarPlayerSequence();
    yield return (object) new WaitForSeconds(0.766666651f);
    HUD_DisplayName.Play("NAMES/Wolf", 2, HUD_DisplayName.Positions.Centre);
    AudioManager.Instance.StopCurrentMusic();
    AudioManager.Instance.PlayMusic(wolfBossIntro.bossMusic);
    AudioManager.Instance.SetMusicRoomID(SoundConstants.RoomID.MainBossB);
    yield return (object) new WaitForSeconds(3f);
    wolfBossIntro.normalSpine.gameObject.SetActive(false);
    wolfBossIntro.wolfBoss.gameObject.SetActive(true);
    wolfBossIntro.ResetPlayerWalking();
    GameManager.GetInstance().OnConversationEnd();
    DataManager.Instance.DiedToWolfBoss = true;
    foreach (TrapLightning lightningTrap in wolfBossIntro.lightningTraps)
      lightningTrap.StartTrap();
    yield return (object) new WaitForEndOfFrame();
    wolfBossIntro.wolfBoss.health.OnDie += new Health.DieAction(wolfBossIntro.Health_OnDie);
  }

  public void SetCoopPlayerPosition()
  {
    if (!CoopManager.CoopActive || !((Object) this.normalSpine != (Object) null))
      return;
    PlayerFarming playerFarming = (PlayerFarming) null;
    float num1 = float.MaxValue;
    Vector3 position = this.normalSpine.gameObject.transform.position;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      float num2 = Vector3.Distance(player.transform.position, position);
      if ((double) num2 < (double) num1)
      {
        num1 = num2;
        playerFarming = player;
      }
    }
    if (!((Object) playerFarming != (Object) null))
      return;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((Object) player != (Object) playerFarming)
      {
        Vector3 TargetPosition = playerFarming.transform.position - new Vector3(0.0f, 1f, 0.0f);
        player.GoToAndStop(TargetPosition, maxDuration: 3f, forcePositionOnTimeout: true);
      }
    }
  }

  public IEnumerator InitPlayerWalking()
  {
    if (!DataManager.Instance.DiedToWolfBoss)
    {
      while ((Object) PlayerFarming.Instance == (Object) null || PlayerFarming.Instance.GoToAndStopping)
        yield return (object) null;
      foreach (PlayerFarming player in PlayerFarming.players)
      {
        PlayerFarming.SetMainPlayer(player);
        PlayerFarming.Instance.unitObject.maxSpeed = 0.03f;
        PlayerFarming.Instance.playerController.RunSpeed = 2f;
        PlayerFarming.Instance.playerController.DefaultRunSpeed = 2f;
        PlayerFarming.Instance.simpleSpineAnimator.NorthIdle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-up-slow");
        PlayerFarming.Instance.simpleSpineAnimator.Idle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-slow");
        PlayerFarming.Instance.simpleSpineAnimator.DefaultLoop = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-slow");
        PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle-slow");
        PlayerFarming.Instance.simpleSpineAnimator.NorthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-slow");
        PlayerFarming.Instance.simpleSpineAnimator.SouthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-down-slow");
        PlayerFarming.Instance.simpleSpineAnimator.NorthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-diagonal-slow");
        PlayerFarming.Instance.simpleSpineAnimator.SouthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-slow");
        PlayerFarming.Instance.simpleSpineAnimator.ForceDirectionalMovement = true;
        PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Moving, "run-horizontal-slow");
        PlayerFarming.Instance.simpleSpineAnimator.UpdateIdleAndMoving();
        PlayerFarming.Instance.playerWeapon.enabled = false;
        PlayerFarming.Instance.playerSpells.enabled = false;
        PlayerFarming.Instance.AllowDodging = false;
      }
    }
  }

  public void ResetPlayerWalking()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      PlayerFarming.SetMainPlayer(player);
      PlayerFarming.Instance.unitObject.maxSpeed = 0.09f;
      PlayerFarming.Instance.playerController.RunSpeed = 5.5f;
      PlayerFarming.Instance.playerController.DefaultRunSpeed = 5.5f;
      PlayerFarming.Instance.simpleSpineAnimator.NorthIdle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle-up");
      PlayerFarming.Instance.simpleSpineAnimator.Idle = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle");
      PlayerFarming.Instance.simpleSpineAnimator.DefaultLoop = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("idle");
      PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
      PlayerFarming.Instance.simpleSpineAnimator.SetDefault(StateMachine.State.Idle, "idle");
      PlayerFarming.Instance.simpleSpineAnimator.NorthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up");
      PlayerFarming.Instance.simpleSpineAnimator.SouthMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-down");
      PlayerFarming.Instance.simpleSpineAnimator.NorthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run-up-diagonal");
      PlayerFarming.Instance.simpleSpineAnimator.SouthDiagonalMoving = PlayerFarming.Instance.simpleSpineAnimator.GetAnimationReference("run");
      PlayerFarming.Instance.simpleSpineAnimator.ForceDirectionalMovement = false;
      PlayerFarming.Instance.simpleSpineAnimator.SetDefault(StateMachine.State.Moving, "run-horizontal");
      PlayerFarming.Instance.simpleSpineAnimator.ResetAnimationsToDefaults();
      PlayerFarming.Instance.simpleSpineAnimator.UpdateIdleAndMoving();
      PlayerFarming.Instance.playerWeapon.enabled = true;
      PlayerFarming.Instance.playerSpells.enabled = true;
      PlayerFarming.Instance.AllowDodging = true;
    }
  }

  public void RoarPlayerSequence()
  {
    BiomeConstants.Instance.EmitRoarDistortionVFX(this.roarFXSlot.position);
    foreach (PlayerFarming player in PlayerFarming.players)
      this.StartCoroutine((IEnumerator) this.RoarPlayerKnocbackSequence(player));
  }

  public IEnumerator RoarPlayerKnocbackSequence(PlayerFarming player)
  {
    player.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    string knockbackAnim = !player.isLamb ? "Downed/Goat/knockback-to-downed-goat" : "Downed/knockback-to-downed";
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(player.simpleSpineAnimator.Animate(knockbackAnim, 0, false).Animation.Duration);
    yield return (object) new WaitForSeconds(player.simpleSpineAnimator.Animate("Downed/idle", 0, false).Animation.Duration);
    yield return (object) new WaitForSeconds(player.simpleSpineAnimator.Animate("knockback-reset", 0, false).Animation.Duration);
    player.simpleSpineAnimator.Animate("idle", 0, true);
  }
}
