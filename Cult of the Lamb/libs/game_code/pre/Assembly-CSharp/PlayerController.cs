// Decompiled with JetBrains decompiler
// Type: PlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using MMBiomeGeneration;
using MMTools;
using Spine.Unity.Examples;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayerController : BaseMonoBehaviour
{
  public AudioClip GameOverMusic;
  private bool GameOver;
  [HideInInspector]
  public UnitObject unitObject;
  private StateMachine state;
  public float forceDir;
  public float speed;
  public float xDir;
  public float yDir;
  private Vector3 SpinePosition;
  private PlayerFarming playerFarming;
  private float DodgeTimer;
  public float DodgeSpeed = 12f;
  public float DodgeDuration = 0.3f;
  public float DodgeMaxDuration = 0.5f;
  public float DodgeDelay = 0.3f;
  private float DodgeCollisionDelay;
  public float HitDuration = 1f;
  public float HitTimer;
  public float HitRecoverTimer;
  private float ConversionTimer;
  private float DeathTimer;
  private float RespawnTimer;
  private float LungeDuration;
  private float LungeTimer;
  private float LungeSpeed;
  public LineRenderer GrappleChain;
  public Transform BoneTool;
  public GameObject playerDieColorVolume;
  private UIDeathScreenOverlayController _deathScreenInstance;
  public SimpleSpineFlash SimpleSpineFlash;
  public bool CarryingBody;
  private bool _ShowRunSmoke = true;
  public System.Action TimedActionCallback;
  public SpineEventListener FootStepSoundsObject;
  private Inventory inventory;
  private PlayerWeapon playerWeapon;
  private PlayerSpells playerSpells;
  public float RunSpeed = 5.5f;
  [HideInInspector]
  public float DefaultRunSpeed = 5.5f;
  public static float MinInputForMovement = 0.3f;
  public static bool CanParryAttacks = false;
  public static bool CanRespawn = true;
  private string carryUp = "corpse/corpse-run-up";
  private string carryDown = "corpse/corpse-run-down";
  private string carryDownDiagonal = "corpse/corpse-run";
  private string carryUpDiagonal = "corpse/corpse-run-up-diagonal";
  private string carryHorizontal = "corpse/corpse-run-horizontal";
  [SerializeField]
  private UIDeathScreenOverlayController _deathScreenTemplate;
  private Health health;
  private float untouchableTimer;
  private float invincibleTimer;
  private int untouchableTimerFlash;
  private float KnockBackAngle;
  public Coroutine HitEffectsCoroutine;
  private float KnockbackVelocity = 0.2f;
  public float KnockbackGravity = 0.03f;
  public float KnockbackBounce = -0.8f;
  private float VZ;
  private float Z;
  public Transform SpineTransform;
  public SimpleSFX sfx;
  private float FootPrints;
  private Color FootStepColor;
  private float FootPrintsNum = 10f;
  private float FootPrintModifier = 5f;
  private List<MeshRenderer> EnemiesTurnedOff = new List<MeshRenderer>();
  private List<SpriteRenderer> SpritesTurnedOff = new List<SpriteRenderer>();
  private List<PlayerController.DroppedItem> droppedItems = new List<PlayerController.DroppedItem>();
  public Interaction_Grapple TargetGrapple;
  private float GrappleProgress;
  private float ElevatorProgress;
  private float ElevatorProgressSpeed;
  private CircleCollider2D circleCollider2D;
  private Interaction_Elevator TargetElevator;
  private float CurretElevatorZ;
  private float TargetElevatorZ;
  private Vector3 ElevatorPosition;
  private bool ElevatorChangedFloor;
  private Vector3 TargetPosition;
  public bool SpawnInShowHUD = true;
  public float LungeDampener = 0.5f;
  private float SeperationRadius;
  private float SeperationDistance;
  private float SeperationAngle;
  private Camera currentMain;
  private float previousClipPlane;
  private static readonly int GlobalDitherIntensity = Shader.PropertyToID("_GlobalDitherIntensity");

  public bool ShowRunSmoke
  {
    get => this._ShowRunSmoke;
    set
    {
      this._ShowRunSmoke = value;
      this.FootStepSoundsObject.enabled = this._ShowRunSmoke;
    }
  }

  private void Start()
  {
    this.unitObject = this.gameObject.GetComponent<UnitObject>();
    this.state = this.gameObject.GetComponent<StateMachine>();
    this.inventory = this.gameObject.GetComponent<Inventory>();
    this.playerFarming = this.gameObject.GetComponent<PlayerFarming>();
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
    this.GrappleChain.gameObject.SetActive(false);
    this.DefaultRunSpeed = this.RunSpeed;
    this.playerWeapon = this.gameObject.GetComponent<PlayerWeapon>();
    DOTween.To(new DOSetter<float>(this.SetDither), Shader.GetGlobalFloat(PlayerController.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
  }

  private void OnEnable()
  {
    this.health = this.gameObject.GetComponent<Health>();
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  private void OnDestroy()
  {
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnHit -= new Health.HitAction(this.OnHit);
      this.health.OnDie -= new Health.DieAction(this.OnDie);
    }
    this.EnemiesTurnedOff.Clear();
    this.SpritesTurnedOff.Clear();
  }

  private void SetDither(float value)
  {
    Shader.SetGlobalFloat(PlayerController.GlobalDitherIntensity, value);
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.health.untouchable)
      return;
    this.playerFarming.simpleSpineAnimator.FlashRedTint();
    this.state.facingAngle = !((UnityEngine.Object) Attacker == (UnityEngine.Object) null) ? Utils.GetAngle(this.transform.position, Attacker.transform.position) : Utils.GetAngle(this.transform.position, AttackLocation);
    this.forceDir = this.state.facingAngle + 180f;
    CameraManager.shakeCamera(0.5f, -this.state.facingAngle);
    if (this.state.CURRENT_STATE != StateMachine.State.Grabbed)
    {
      if (this.HitEffectsCoroutine != null)
        this.StopCoroutine(this.HitEffectsCoroutine);
      this.HitEffectsCoroutine = this.StartCoroutine((IEnumerator) this.HitEffects(StateMachine.State.HitThrown, StateMachine.State.HitRecover));
      this.state.CURRENT_STATE = StateMachine.State.HitThrown;
      this.MakeUntouchable(1f * DifficultyManager.GetInvincibleTimeMultiplier());
    }
    BiomeConstants.Instance.EmitHitVFX(AttackLocation, Quaternion.identity.z, "HitFX_Blocked");
    AudioManager.Instance.ToggleFilter(SoundParams.HitFilter, true);
    AudioManager.Instance.ToggleFilter(SoundParams.HitFilter, false, 0.2f);
    GameManager.GetInstance().HitStop();
    AudioManager.Instance.PlayOneShot("event:/player/gethit", this.transform.position);
  }

  public void MakeUntouchable(float duration)
  {
    if ((double) duration <= 0.0)
      return;
    if ((double) this.untouchableTimer < (double) duration)
      this.untouchableTimer = duration;
    this.health.untouchable = true;
    KeyboardLightingManager.TransitionAllKeys(Color.red, Color.red, 0.0f, KeyboardLightingManager.F_KEYS);
    this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() =>
    {
      KeyboardLightingManager.TransitionAllKeys(Color.white, Color.white, 0.0f, KeyboardLightingManager.F_KEYS);
      this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() =>
      {
        KeyboardLightingManager.TransitionAllKeys(Color.red, Color.red, 0.0f, KeyboardLightingManager.F_KEYS);
        this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() => KeyboardLightingManager.PulseAllKeys(Color.white, Color.black, 0.1f, KeyboardLightingManager.F_KEYS))));
      })));
    })));
  }

  private IEnumerator Delay(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void MakeInvincible(float duration)
  {
    this.invincibleTimer = duration;
    this.health.invincible = true;
  }

  public IEnumerator HitEffects(StateMachine.State EntryState, StateMachine.State NextState)
  {
    this.HitTimer = 0.0f;
    this.speed = 0.0f;
    this.HitDuration = 1f;
    this.health.untouchable = true;
    this.untouchableTimer = 1f * DifficultyManager.GetInvincibleTimeMultiplier();
    this.Z = 0.0f;
    while ((double) (this.HitTimer += Time.unscaledDeltaTime) < 0.10000000149011612)
      yield return (object) null;
    this.VZ = this.KnockbackVelocity;
    this.speed = 10f;
    while ((double) (this.speed -= 0.9f * GameManager.DeltaTime) > 0.0)
    {
      this.HitTimer += Time.deltaTime;
      yield return (object) null;
    }
    if (NextState != StateMachine.State.GameOver && NextState != StateMachine.State.Resurrecting)
      this.untouchableTimer = 1f * DifficultyManager.GetInvincibleTimeMultiplier();
    this.state.CURRENT_STATE = NextState;
    this.HitEffectsCoroutine = (Coroutine) null;
  }

  public void StopHitEffects()
  {
    if (this.HitEffectsCoroutine == null)
      return;
    this.health.untouchable = true;
    this.untouchableTimer = 1f * DifficultyManager.GetInvincibleTimeMultiplier();
    this.StopCoroutine(this.HitEffectsCoroutine);
    this.HitEffectsCoroutine = (Coroutine) null;
  }

  public void BlockAttacker(GameObject attacker)
  {
    if (!this.health.IsAttackerInDamageEventQueue(attacker))
      return;
    this.health.ForgiveRecentDamage();
  }

  private void KillPlayer()
  {
    DataManager.Instance.Followers.Clear();
    this.OnDie(this.gameObject, Vector3.zero, (Health) null, Health.AttackTypes.Melee, (Health.AttackFlags) 0);
  }

  public void SetFootSteps(Color FootStepColor)
  {
    this.FootPrints = this.FootPrintsNum;
    this.FootStepColor = FootStepColor;
  }

  public void EmitFootprints()
  {
    AudioManager.Instance.PlayFootstepPlayer(this.transform.position);
    if ((double) --this.FootPrints <= 0.0)
      return;
    BiomeConstants.Instance.EmitFootprintsParticles(this.transform.position, this.FootStepColor, Mathf.Min(1f, this.FootPrints / this.FootPrintModifier));
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.StopActiveLoops();
    AudioManager.Instance.PlayOneShot("event:/player/death_hit", this.gameObject);
    this.SimpleSpineFlash.FlashWhite(false);
    this.health.invincible = true;
    this.health.untouchable = true;
    this.GetComponent<Interactor>().HideIndicator();
    GameManager.GetInstance().HitStop(0.3f);
    this.state.CURRENT_STATE = StateMachine.State.Dieing;
    this.state.facingAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
    this.forceDir = this.state.facingAngle + 180f;
    DataManager.Instance.PlayerDamageReceived += 50f;
    Debug.Log((object) ("ResurrectOnHud.HasRessurection " + ResurrectOnHud.HasRessurection.ToString()));
    if (MMConversation.isPlaying)
      MMConversation.mmConversation?.Close();
    PlayerFarming.Instance.Spine.UseDeltaTime = false;
    PlayerFarming.Instance.Spine.timeScale = 1f;
    this.state.CURRENT_STATE = StateMachine.State.GameOver;
    this.state.LockStateChanges = true;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(1f, 0.0f, 1f);
    MeshRenderer[] objectsOfType1 = UnityEngine.Object.FindObjectsOfType<MeshRenderer>();
    this.EnemiesTurnedOff.Clear();
    foreach (MeshRenderer meshRenderer in objectsOfType1)
    {
      if (meshRenderer.gameObject.activeSelf && meshRenderer.gameObject.layer != 14 && meshRenderer.gameObject.layer != 15 && (double) Vector3.Distance(meshRenderer.transform.position, this.transform.position) < 1.0 && (double) meshRenderer.transform.position.y < (double) this.transform.position.y)
      {
        meshRenderer.enabled = false;
        this.EnemiesTurnedOff.Add(meshRenderer);
      }
    }
    SpriteRenderer[] objectsOfType2 = UnityEngine.Object.FindObjectsOfType<SpriteRenderer>();
    this.SpritesTurnedOff.Clear();
    foreach (SpriteRenderer spriteRenderer in objectsOfType2)
    {
      if (spriteRenderer.gameObject.activeSelf && (double) Vector3.Distance(spriteRenderer.transform.position, this.transform.position) < 2.0 && (double) spriteRenderer.transform.position.y < (double) this.transform.position.y)
      {
        spriteRenderer.enabled = false;
        this.SpritesTurnedOff.Add(spriteRenderer);
      }
    }
    DOTween.To(new DOSetter<float>(this.SetDither), Shader.GetGlobalFloat(PlayerController.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance * 2f, 1f).SetEase<Tweener>(Ease.OutQuart);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(GameObject.FindWithTag("Player Camera Bone"), 4f);
    ++DataManager.Instance.playerDeaths;
    ++DataManager.Instance.playerDeathsInARow;
    AudioManager.Instance.PlayMusic("event:/music/game_over/game_over");
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentX == BiomeGenerator.BossCoords.x && BiomeGenerator.Instance.CurrentY == BiomeGenerator.BossCoords.y)
      ++DataManager.Instance.playerDeathsInARowFightingLeader;
    UIBossHUD.Hide();
    CameraManager.shakeCamera(0.5f, -this.state.facingAngle);
  }

  public void OnFinalGameOver()
  {
    AudioManager.Instance.PlayOneShot("event:/player/death_hit", this.gameObject);
    this.health.invincible = true;
    this.health.untouchable = true;
    this.GetComponent<Interactor>().HideIndicator();
    GameManager.GetInstance().HitStop(0.3f);
    this.state.CURRENT_STATE = StateMachine.State.FinalGameOver;
    if (MMConversation.isPlaying)
      MMConversation.mmConversation?.Close();
    PlayerFarming.Instance.Spine.UseDeltaTime = false;
    PlayerFarming.Instance.Spine.timeScale = 1f;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Clear();
    PlayerFarming.Instance.Spine.CustomMaterialOverride.Add(PlayerFarming.Instance.originalMaterial, PlayerFarming.Instance.BW_Material);
    HUD_Manager.Instance.ShowBW(1f, 0.0f, 1f);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(GameObject.FindWithTag("Player Camera Bone"), 4f);
    ++DataManager.Instance.playerDeaths;
    AudioManager.Instance.PlayMusic("event:/music/game_over/game_over");
    UIBossHUD.Hide();
    CameraManager.shakeCamera(0.5f, -this.state.facingAngle);
  }

  public void DoGrapple(Interaction_Grapple GrappleTarget)
  {
    GameManager.GetInstance().HitStop();
    this.TargetGrapple = GrappleTarget;
    this.speed = 0.0f;
    this.state.facingAngle = Utils.GetAngle(this.transform.position, GrappleTarget.transform.position);
    this.state.CURRENT_STATE = StateMachine.State.Grapple;
    this.circleCollider2D.enabled = false;
    this.playerFarming.simpleSpineAnimator.Animate("grapple", 0, false);
    this.playerFarming.simpleSpineAnimator.AddAnimate("grapple-fly", 0, true, 0.0f);
    this.GrappleChain.gameObject.SetActive(true);
    this.GrappleChain.textureMode = LineTextureMode.Tile;
    this.ElevatorProgress = this.GrappleProgress = 0.0f;
    this.GrappleChain.SetPosition(0, this.BoneTool.position);
    this.GrappleChain.SetPosition(1, this.BoneTool.position);
  }

  public void DoElevator(
    Vector3 ElevatorPosition,
    Interaction_Elevator TargetElevator,
    float CurretElevatorZ,
    float TargetElevatorZ)
  {
    GameManager.GetInstance().HitStop();
    this.TargetElevator = TargetElevator;
    this.speed = 0.0f;
    this.state.facingAngle = Utils.GetAngle(this.transform.position, TargetElevator.transform.position);
    this.state.CURRENT_STATE = StateMachine.State.Elevator;
    this.circleCollider2D.enabled = false;
    this.playerFarming.simpleSpineAnimator.Animate("grapple", 0, false);
    this.playerFarming.simpleSpineAnimator.AddAnimate("grapple-fly", 0, true, 0.0f);
    this.GrappleChain.gameObject.SetActive(true);
    this.GrappleChain.textureMode = LineTextureMode.Tile;
    this.GrappleProgress = 0.0f;
    this.ElevatorProgress = 0.0f;
    this.ElevatorProgressSpeed = 0.0f;
    this.GrappleChain.SetPosition(0, this.BoneTool.position);
    this.GrappleChain.SetPosition(1, this.BoneTool.position);
    this.CurretElevatorZ = CurretElevatorZ;
    this.TargetElevatorZ = TargetElevatorZ;
    this.ElevatorPosition = ElevatorPosition;
    this.ElevatorChangedFloor = false;
  }

  public void DoIslandDash(Vector3 TargetPosition)
  {
    this.DodgeTimer = 0.0f;
    this.TargetPosition = TargetPosition;
    this.speed = this.DodgeSpeed * 1.2f;
    this.state.CURRENT_STATE = StateMachine.State.DashAcrossIsland;
    this.circleCollider2D.enabled = false;
  }

  private void DoIdle() => this.state.CURRENT_STATE = StateMachine.State.Idle;

  private void Update()
  {
    if ((double) Time.timeScale <= 0.0 && this.state.CURRENT_STATE != StateMachine.State.Resurrecting && this.state.CURRENT_STATE != StateMachine.State.GameOver && this.state.CURRENT_STATE != StateMachine.State.FinalGameOver)
      return;
    if ((double) this.untouchableTimer > 0.0)
    {
      this.SimpleSpineFlash.FlashMeWhite(1f, 7);
      this.untouchableTimer -= Time.deltaTime;
      if ((double) this.untouchableTimer <= 0.0)
      {
        this.health.untouchable = false;
        KeyboardLightingManager.StopAll();
        KeyboardLightingManager.UpdateLocation();
        if ((double) this.health.HP <= 1.0)
          KeyboardLightingManager.PulseAllKeys(Color.white, Color.red, 0.35f, new KeyCode[0]);
      }
    }
    else if (this.SimpleSpineFlash.isFillWhite)
      this.SimpleSpineFlash.FlashWhite(false);
    if ((double) this.invincibleTimer > 0.0)
    {
      this.invincibleTimer -= Time.deltaTime;
      if ((double) this.invincibleTimer <= 0.0)
        this.health.untouchable = false;
    }
    if (!this.playerFarming.GoToAndStopping)
    {
      this.xDir = InputManager.Gameplay.GetHorizontalAxis();
      this.yDir = InputManager.Gameplay.GetVerticalAxis();
      if (this.state.CURRENT_STATE == StateMachine.State.Moving)
        this.speed *= Mathf.Clamp01(new Vector2(this.xDir, this.yDir).magnitude);
      this.speed = Mathf.Max(this.speed, 0.0f);
      this.unitObject.vx = this.speed * Mathf.Cos(this.forceDir * ((float) Math.PI / 180f));
      this.unitObject.vy = this.speed * Mathf.Sin(this.forceDir * ((float) Math.PI / 180f));
    }
    else
    {
      this.xDir = this.yDir = 0.0f;
      this.playerFarming.Spine.AnimationState.TimeScale = 1f;
    }
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Z = 0.0f;
        this.SpineTransform.localPosition = Vector3.zero;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement)
          this.state.CURRENT_STATE = StateMachine.State.Moving;
        if (DataManager.Instance.GameOver)
        {
          this.OnFinalGameOver();
          goto case StateMachine.State.Stealth;
        }
        if (DataManager.Instance.DisplayGameOverWarning)
        {
          DataManager.Instance.DisplayGameOverWarning = false;
          DataManager.Instance.InGameOver = true;
          if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.GameOver))
          {
            UITutorialOverlayController overlayController = MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.GameOver);
            overlayController.OnHidden = overlayController.OnHidden + (System.Action) (() => ObjectiveManager.Add((ObjectivesData) new Objectives_Custom("Objectives/GroupTitles/GameOver", Objectives.CustomQuestTypes.GameOver, questExpireDuration: 4800f)));
            goto case StateMachine.State.Stealth;
          }
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Moving:
        if (!this.playerFarming.GoToAndStopping && (double) Time.timeScale != 0.0)
        {
          if ((double) Mathf.Abs(this.xDir) <= (double) PlayerController.MinInputForMovement && (double) Mathf.Abs(this.yDir) <= (double) PlayerController.MinInputForMovement)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            goto case StateMachine.State.Stealth;
          }
          this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
          if ((double) this.unitObject.vx != 0.0 || (double) this.unitObject.vy != 0.0)
            this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
          this.state.LookAngle = this.state.facingAngle;
          this.speed += (float) (((double) this.RunSpeed * ((double) this.playerWeapon.GetCurrentWeapon().MovementSpeedMultiplier * (double) TrinketManager.GetMovementSpeedMultiplier()) - (double) this.speed) / 3.0) * GameManager.DeltaTime;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Attacking:
        if (!this.playerFarming.GoToAndStopping)
        {
          this.LungeTimer -= Time.deltaTime;
          this.forceDir = this.state.facingAngle;
          this.speed += (float) (((double) this.LungeSpeed * ((double) this.LungeTimer / (double) this.LungeDuration) - (double) this.speed) / 3.0) * GameManager.DeltaTime;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.SignPostAttack:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement)
          this.state.facingAngle = this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
        if ((double) (this.state.Timer -= Time.deltaTime) < 0.0)
        {
          this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
          AudioManager.Instance.PlayOneShot("event:/player/attack", this.gameObject);
          if (this.TimedActionCallback != null)
          {
            this.TimedActionCallback();
            goto case StateMachine.State.Stealth;
          }
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.RecoverFromAttack:
        this.speed += (float) ((0.0 - (double) this.speed) / 7.0) * GameManager.DeltaTime;
        this.state.Timer -= Time.deltaTime;
        if ((double) this.state.Timer < 0.0)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Dodging:
        this.Z = 0.0f;
        this.SpineTransform.localPosition = Vector3.zero;
        this.forceDir = this.state.facingAngle;
        if ((double) this.DodgeCollisionDelay < 0.0)
          this.speed = Mathf.Lerp(this.speed, this.DodgeSpeed, 2f * Time.deltaTime);
        this.DodgeCollisionDelay -= Time.deltaTime;
        this.DodgeTimer += Time.deltaTime;
        if ((double) this.DodgeTimer < 0.10000000149011612 && ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement))
        {
          MMVibrate.Rumble(0.2f, 0.2f, 0.25f, (MonoBehaviour) this);
          this.state.facingAngle = this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
        }
        if (!InputManager.Gameplay.GetDodgeButtonHeld() && (double) this.DodgeTimer > (double) this.DodgeDuration || (double) this.DodgeTimer > (double) this.DodgeMaxDuration)
        {
          this.DodgeTimer = 0.0f;
          this.DodgeCollisionDelay = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.TimedAction:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.state.Timer -= Time.deltaTime) < 0.0)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if (this.TimedActionCallback != null)
          {
            this.TimedActionCallback();
            goto case StateMachine.State.Stealth;
          }
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.HitThrown:
        if ((double) this.HitTimer > 0.10000000149011612)
        {
          this.VZ -= this.KnockbackGravity;
          this.Z += this.VZ;
          if ((double) this.Z < 0.0)
          {
            this.VZ *= this.KnockbackBounce;
            this.Z = 0.0f;
          }
          this.SpinePosition = this.SpineTransform.localPosition;
          this.SpineTransform.localPosition = this.SpinePosition;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.HitRecover:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.HitRecoverTimer += Time.deltaTime) > 0.30000001192092896)
        {
          this.HitRecoverTimer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.SpinePosition = this.SpineTransform.localPosition;
          this.SpinePosition.z = 0.0f;
          this.SpineTransform.localPosition = this.SpinePosition;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Teleporting:
        this.speed = 0.0f;
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Converting:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.ConversionTimer += Time.deltaTime) > 7.3000001907348633)
        {
          this.ConversionTimer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Dieing:
        if ((double) this.HitTimer > 0.30000001192092896)
        {
          this.VZ -= this.KnockbackGravity;
          this.Z += this.VZ;
          if ((double) this.Z < 0.0)
          {
            this.VZ *= this.KnockbackBounce;
            this.Z = 0.0f;
          }
          this.SpinePosition = this.SpineTransform.localPosition;
          this.SpineTransform.localPosition = this.SpinePosition;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Dead:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.DeathTimer += Time.deltaTime) > 2.0 && !this.GameOver)
        {
          this.GameOver = true;
          this.DeathTimer = 0.0f;
          GameManager.ToShip();
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Respawning:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.RespawnTimer += Time.deltaTime) > 6.0)
        {
          GameManager.GetInstance().OnConversationEnd();
          this.RespawnTimer = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.SpawnIn:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.state.Timer += Time.deltaTime) > 3.0)
        {
          Debug.Log((object) ("SpawnInShowHUD: ".Colour(Color.green) + this.SpawnInShowHUD.ToString()));
          GameManager.GetInstance().OnConversationEnd(ShowHUD: this.SpawnInShowHUD);
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.SpawnInShowHUD = true;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Grapple:
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetGrapple.transform.position);
        this.forceDir = this.state.facingAngle;
        this.GrappleChain.SetPosition(0, this.BoneTool.position);
        if ((double) this.GrappleProgress < 1.0)
        {
          this.GrappleProgress += 0.1f;
          if ((double) this.GrappleProgress >= 1.0)
          {
            this.TargetGrapple.BecomeTarget();
            this.speed = -10f;
            CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetGrapple.transform.position));
          }
        }
        else if ((double) this.speed < 20.0)
          this.speed += 1f * GameManager.DeltaTime;
        this.GrappleChain.SetPosition(1, Vector3.Lerp(this.BoneTool.position, this.TargetGrapple.BoneTarget.position, this.GrappleProgress));
        if ((double) Vector2.Distance((Vector2) this.transform.position, (Vector2) this.TargetGrapple.transform.position) < (double) this.speed * (double) Time.deltaTime)
        {
          this.transform.position = this.TargetGrapple.transform.position;
          this.speed = 0.0f;
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.circleCollider2D.enabled = true;
          this.TargetGrapple.BecomeOrigin();
          this.GrappleChain.gameObject.SetActive(false);
          CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetGrapple.transform.position));
          GameManager.GetInstance().HitStop();
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.DashAcrossIsland:
        this.speed = Mathf.Lerp(this.speed, this.DodgeSpeed, 2f * Time.deltaTime);
        this.DodgeTimer += Time.deltaTime;
        if ((double) this.DodgeTimer > (double) this.DodgeDuration)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          this.circleCollider2D.enabled = true;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.ChargingHeavyAttack:
        if (!this.playerFarming.GoToAndStopping)
        {
          if ((double) Mathf.Abs(this.xDir) <= (double) PlayerController.MinInputForMovement && (double) Mathf.Abs(this.yDir) <= (double) PlayerController.MinInputForMovement)
          {
            this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
            goto case StateMachine.State.Stealth;
          }
          this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
          this.speed += (float) (((double) this.RunSpeed * 0.75 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Elevator:
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetElevator.transform.position);
        this.forceDir = this.state.facingAngle;
        this.GrappleChain.SetPosition(0, this.BoneTool.position);
        this.speed = 0.0f;
        this.GrappleChain.SetPosition(1, Vector3.Lerp(this.BoneTool.position, this.TargetElevator.transform.position, this.GrappleProgress));
        if ((double) this.GrappleProgress < 1.0)
        {
          this.GrappleProgress += 5f * Time.deltaTime;
          if ((double) this.GrappleProgress >= 1.0)
          {
            this.ElevatorProgressSpeed = 0.0f;
            CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetElevator.transform.position));
            goto case StateMachine.State.Stealth;
          }
          goto case StateMachine.State.Stealth;
        }
        if ((double) this.GrappleProgress >= 1.0)
        {
          if ((double) this.ElevatorProgressSpeed < 6.0)
            this.ElevatorProgressSpeed += 0.15f;
          this.ElevatorProgress += this.ElevatorProgressSpeed * Time.deltaTime;
          this.transform.position = Vector3.Lerp(this.ElevatorPosition, this.TargetElevator.transform.position, this.ElevatorProgress);
          if ((double) this.ElevatorProgress >= 0.5 && !this.ElevatorChangedFloor)
            this.ElevatorChangedFloor = true;
          if ((double) this.ElevatorProgress >= 1.0)
          {
            GameManager.GetInstance().HitStop();
            CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetElevator.transform.position));
            this.transform.position = this.TargetElevator.transform.position;
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            this.circleCollider2D.enabled = true;
            this.GrappleChain.gameObject.SetActive(false);
            goto case StateMachine.State.Stealth;
          }
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Stealth:
label_146:
        if (this.state.CURRENT_STATE != StateMachine.State.Dodging && this.state.CURRENT_STATE != StateMachine.State.HitThrown && this.state.CURRENT_STATE != StateMachine.State.DashAcrossIsland && this.state.CURRENT_STATE != StateMachine.State.Grapple)
        {
          this.SeperateFromEnemies();
        }
        else
        {
          this.unitObject.seperatorVX = 0.0f;
          this.unitObject.seperatorVY = 0.0f;
        }
        if (!((UnityEngine.Object) this.playerFarming.Spine != (UnityEngine.Object) null) || this.playerFarming.Spine.AnimationState == null)
          break;
        if (this.state.CURRENT_STATE == StateMachine.State.Moving && !this.playerFarming.GoToAndStopping)
        {
          this.playerFarming.Spine.AnimationState.TimeScale = Mathf.Clamp01(this.speed / this.RunSpeed);
          break;
        }
        this.playerFarming.Spine.AnimationState.TimeScale = 1f;
        break;
      case StateMachine.State.GameOver:
        if (PlayerController.CanRespawn)
          Time.timeScale = 0.0f;
        PlayerFarming.Instance.Spine.UseDeltaTime = false;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        this.state.Timer += Time.unscaledDeltaTime;
        if (TrinketManager.HasTrinket(TarotCards.Card.TheDeal) && !TrinketManager.IsOnCooldown(TarotCards.Card.TheDeal))
        {
          this.state.LockStateChanges = false;
          this.state.CURRENT_STATE = StateMachine.State.Resurrecting;
          goto case StateMachine.State.Stealth;
        }
        if (ResurrectOnHud.HasRessurection && PlayerFarming.Location != FollowerLocation.Boss_5)
        {
          this.state.LockStateChanges = false;
          if (PlayerController.CanRespawn && (double) this.state.Timer > 2.0 && !MMTransition.IsPlaying)
          {
            HUD_Manager.Instance.ShowBW(0.5f, 1f, 0.0f);
            PlayerFarming.Instance.Spine.UseDeltaTime = true;
            RespawnRoomManager.Play();
            foreach (MeshRenderer meshRenderer in this.EnemiesTurnedOff)
            {
              if ((UnityEngine.Object) meshRenderer != (UnityEngine.Object) null)
                meshRenderer.enabled = true;
            }
            foreach (SpriteRenderer spriteRenderer in this.SpritesTurnedOff)
            {
              if ((UnityEngine.Object) spriteRenderer != (UnityEngine.Object) null)
                spriteRenderer.enabled = true;
            }
            this.EnemiesTurnedOff.Clear();
            this.SpritesTurnedOff.Clear();
            goto case StateMachine.State.Stealth;
          }
          goto case StateMachine.State.Stealth;
        }
        if (PlayerController.CanRespawn && (double) this.state.Timer > 2.0 && (UnityEngine.Object) this._deathScreenInstance == (UnityEngine.Object) null && (UnityEngine.Object) UIDeathScreenOverlayController.Instance == (UnityEngine.Object) null)
        {
          this._deathScreenInstance = MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.Killed);
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Aiming:
        if (!this.playerFarming.GoToAndStopping)
        {
          if ((double) new Vector2(this.xDir, this.yDir).sqrMagnitude > 0.0)
            this.state.facingAngle = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
          this.state.LookAngle = this.state.facingAngle;
          this.speed += (float) ((0.0 - (double) this.speed) / 7.0) * GameManager.DeltaTime;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Resurrecting:
        int num = 0;
        int count = this.droppedItems.Count;
        for (int index = this.droppedItems.Count - 1; index >= 0; --index)
        {
          ResourceCustomTarget.Create(this.gameObject, this.transform.position + (Vector3) Utils.DegreeToVector2((float) num * (360f / (float) count)), (InventoryItem.ITEM_TYPE) this.droppedItems[index].Type, (System.Action) null);
          Inventory.AddItem(this.droppedItems[index].Type, this.droppedItems[index].Quantity);
          this.droppedItems.Remove(this.droppedItems[index]);
          ++num;
        }
        Time.timeScale = 0.0f;
        this.playerFarming.Spine.UseDeltaTime = false;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) (this.state.Timer += Time.unscaledDeltaTime) > 3.0)
        {
          Time.timeScale = 1f;
          this.playerFarming.Spine.UseDeltaTime = true;
          this.health.enabled = true;
          this.health.invincible = false;
          this.untouchableTimer = 1f * DifficultyManager.GetInvincibleTimeMultiplier();
          this.health.untouchable = true;
          HUD_Manager.Instance.ShowBW(1f, 0.0f, 0.0f);
          GameManager.GetInstance().OnConversationEnd();
          GameManager.GetInstance().CameraResetTargetZoom();
          this.droppedItems.Clear();
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && (bool) (UnityEngine.Object) RespawnRoomManager.Instance && !RespawnRoomManager.Instance.Respawning)
          {
            AudioManager.Instance.PlayMusic(BiomeGenerator.Instance.biomeMusicPath);
            if (BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null)
              AudioManager.Instance.SetMusicRoomID(BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID);
          }
          HealthPlayer component = this.playerFarming.GetComponent<HealthPlayer>();
          switch (ResurrectOnHud.ResurrectionType)
          {
            case ResurrectionType.DealTarot:
              TrinketManager.TriggerCooldown(TarotCards.Card.TheDeal);
              component.HP = 2f;
              break;
            default:
              component.HP = RespawnRoomManager.HP;
              component.SpiritHearts = RespawnRoomManager.SpiritHearts;
              component.BlueHearts = RespawnRoomManager.BlueHearts;
              component.BlackHearts = RespawnRoomManager.BlackHearts;
              break;
          }
          ResurrectOnHud.ResurrectionType = ResurrectionType.None;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Idle_CarryingBody:
        this.Z = 0.0f;
        this.SpineTransform.localPosition = Vector3.zero;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement)
        {
          this.state.CURRENT_STATE = StateMachine.State.Moving_CarryingBody;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Moving_CarryingBody:
        if (!this.playerFarming.GoToAndStopping)
        {
          if ((double) Mathf.Abs(this.xDir) <= (double) PlayerController.MinInputForMovement && (double) Mathf.Abs(this.yDir) <= (double) PlayerController.MinInputForMovement)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle_CarryingBody;
            goto case StateMachine.State.Stealth;
          }
          this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
          this.state.LookAngle = this.state.facingAngle;
          this.speed += (float) (((double) this.RunSpeed * 0.5 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
          switch (Utils.GetAngleDirectionFull(this.forceDir))
          {
            case Utils.DirectionFull.Up:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryUp)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryUp, true);
                goto label_146;
              }
              goto label_146;
            case Utils.DirectionFull.Down:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryDown)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryDown, true);
                goto label_146;
              }
              goto label_146;
            case Utils.DirectionFull.Left:
            case Utils.DirectionFull.Right:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryHorizontal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryHorizontal, true);
                goto label_146;
              }
              goto label_146;
            case Utils.DirectionFull.Up_Diagonal:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryUpDiagonal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryUpDiagonal, true);
                goto label_146;
              }
              goto label_146;
            case Utils.DirectionFull.Down_Diagonal:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryDownDiagonal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryDownDiagonal, true);
                goto label_146;
              }
              goto label_146;
            default:
              goto label_146;
          }
        }
        else
          goto case StateMachine.State.Stealth;
      case StateMachine.State.FinalGameOver:
        Time.timeScale = 0.0f;
        PlayerFarming.Instance.Spine.UseDeltaTime = false;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        this.state.Timer += Time.unscaledDeltaTime;
        if ((double) this.state.Timer > 2.0 && (UnityEngine.Object) this._deathScreenInstance == (UnityEngine.Object) null)
        {
          this._deathScreenInstance = MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.GameOver, 0);
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      default:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        goto case StateMachine.State.Stealth;
    }
  }

  public void SetCarryingObjectAnimations(
    string idle,
    string carryUp,
    string carryDown,
    string carryDownDiagonal,
    string carryUpDiagonal,
    string carryHorizontal)
  {
    PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle_CarryingBody, idle);
    this.carryUp = carryUp;
    this.carryDown = carryDown;
    this.carryDownDiagonal = carryDownDiagonal;
    this.carryUpDiagonal = carryUpDiagonal;
    this.carryHorizontal = carryHorizontal;
  }

  public void ResetCarryingObjectAnimations()
  {
    PlayerFarming.Instance.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle_CarryingBody, "corpse/corpse-idle");
    this.carryUp = "corpse/corpse-run-up";
    this.carryDown = "corpse/corpse-run-down";
    this.carryDownDiagonal = "corpse/corpse-run";
    this.carryUpDiagonal = "corpse/corpse-run-up-diagonal";
    this.carryHorizontal = "corpse/corpse-run-horizontal";
  }

  public void Lunge(float lungeDuration, float lungeSpeed)
  {
    this.LungeDuration = lungeDuration;
    this.LungeTimer = this.LungeDuration;
    this.LungeSpeed = lungeSpeed * this.LungeDampener;
  }

  public void CancelLunge(float hitKnockback)
  {
    this.LungeTimer = 0.0f;
    this.speed = 0.0f;
    if ((double) hitKnockback == 0.0)
      return;
    this.unitObject.DoKnockBack(Mathf.Repeat(this.state.facingAngle + 180f, 360f) * ((float) Math.PI / 180f), hitKnockback, 0.1f);
  }

  private void SeperateFromEnemies() => this.unitObject.Seperate(1f);

  private struct DroppedItem
  {
    public int Type;
    public int Quantity;
  }
}
