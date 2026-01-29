// Decompiled with JetBrains decompiler
// Type: PlayerController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.DeathScreen;
using MMBiomeGeneration;
using MMRoomGeneration;
using MMTools;
using Spine;
using Spine.Unity;
using Spine.Unity.Examples;
using src.UI.Overlays.TutorialOverlay;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerController : BaseMonoBehaviour
{
  public AudioClip GameOverMusic;
  public bool GameOver;
  [HideInInspector]
  public UnitObject unitObject;
  public StateMachine state;
  public float forceDir;
  public float speed;
  public float xDir;
  public float yDir;
  public Vector3 SpinePosition;
  public PlayerFarming playerFarming;
  public float DodgeTimer;
  public float DodgeSpeed = 12f;
  public float DodgeDuration = 0.3f;
  public float DodgeMaxDuration = 0.5f;
  public float DodgeDelay = 0.3f;
  public float DodgeCollisionDelay;
  public float HitDuration = 1f;
  public float HitTimer;
  public float HitRecoverTimer;
  public float ConversionTimer;
  public float DeathTimer;
  public float RespawnTimer;
  public float LungeDuration;
  public float LungeTimer;
  public float LungeSpeed;
  public LineRenderer GrappleChain;
  public Transform BoneTool;
  public GameObject playerDieColorVolume;
  public UIDeathScreenOverlayController _deathScreenInstance;
  public SimpleSpineFlash SimpleSpineFlash;
  public float WinterSpeedModifier = 1f;
  public SkeletonAnimationLODManager SkeletonLOD;
  [SerializeField]
  public DamageCollider enlargedCollider;
  [SerializeField]
  public SkeletonGhost _skeletonGhost;
  public bool CarryingBody;
  public bool _ShowRunSmoke = true;
  public System.Action TimedActionCallback;
  public SpineEventListener FootStepSoundsObject;
  public Inventory inventory;
  public PlayerWeapon playerWeapon;
  public PlayerSpells playerSpells;
  public float RunSpeed = 5.5f;
  [HideInInspector]
  public float DefaultRunSpeed = 5.5f;
  public static float MinInputForMovement = 0.3f;
  public static bool CanParryAttacks = false;
  public static bool CanRespawn = true;
  public bool showGhostEffect;
  public string carryUp = "corpse/corpse-run-up";
  public string carryDown = "corpse/corpse-run-down";
  public string carryDownDiagonal = "corpse/corpse-run";
  public string carryUpDiagonal = "corpse/corpse-run-up-diagonal";
  public string carryHorizontal = "corpse/corpse-run-horizontal";
  public string snowballUp = "snowball/charged-run-up";
  public string snowballDown = "snowball/charged-run-down";
  public string snowballDownDiagonal = "snowball/charged-run";
  public string snowballUpDiagonal = "snowball/charged-run-up-diagonal";
  public string snowballHorizontal = "snowball/charged-run-horizontal";
  public bool overridingAnimations;
  [SerializeField]
  public UIDeathScreenOverlayController _deathScreenTemplate;
  [CompilerGenerated]
  public bool \u003COverrideForwardMovement\u003Ek__BackingField;
  public bool OverrideBlizzardAnims;
  public Health health;
  public float untouchableTimer;
  public float invincibleTimer;
  public float immuneToProjectilesTimer;
  public int untouchableTimerFlash;
  public float KnockBackAngle;
  public Coroutine HitEffectsCoroutine;
  public float KnockbackVelocity = 0.2f;
  public float KnockbackGravity = 0.03f;
  public float KnockbackBounce = -0.8f;
  public float VZ;
  public float Z;
  public Transform SpineTransform;
  public SimpleSFX sfx;
  public float FootPrints;
  public Color FootStepColor;
  public float FootPrintsNum = 10f;
  public float FootPrintModifier = 5f;
  public List<MeshRenderer> EnemiesTurnedOff = new List<MeshRenderer>();
  public List<SpriteRenderer> SpritesTurnedOff = new List<SpriteRenderer>();
  public List<PlayerController.DroppedItem> droppedItems = new List<PlayerController.DroppedItem>();
  public Interaction_Grapple TargetGrapple;
  public float GrappleProgress;
  public float ElevatorProgress;
  public float ElevatorProgressSpeed;
  public CircleCollider2D circleCollider2D;
  public Interaction_Elevator TargetElevator;
  public float CurretElevatorZ;
  public float TargetElevatorZ;
  public Vector3 ElevatorPosition;
  public bool ElevatorChangedFloor;
  public Vector3 TargetPosition;
  public bool SpawnInShowHUD = true;
  public EventInstance _loopSound;
  public bool playedLoop;
  public bool playedFirstTickSfx;
  public bool playedSecondTickSfx;
  public bool playedThirdTickSfx;
  [CompilerGenerated]
  public bool \u003CPlayInvincibleSFX\u003Ek__BackingField;
  public float LungeDampener = 0.5f;
  public Coroutine UpdateRecoilRoutine;
  public float SeperationRadius;
  public float SeperationDistance;
  public float SeperationAngle;
  public Camera currentMain;
  public float previousClipPlane;

  public StateMachine.State CurrentState => this.state.CURRENT_STATE;

  public bool ShowRunSmoke
  {
    get => this._ShowRunSmoke;
    set
    {
      this._ShowRunSmoke = value;
      this.FootStepSoundsObject.enabled = this._ShowRunSmoke;
    }
  }

  public bool OverrideForwardMovement
  {
    get => this.\u003COverrideForwardMovement\u003Ek__BackingField;
    set => this.\u003COverrideForwardMovement\u003Ek__BackingField = value;
  }

  public bool BlizzardAnims
  {
    get
    {
      return this.OverrideBlizzardAnims || WeatherSystemController.Instance.CurrentWeatherType == WeatherSystemController.WeatherType.Snowing && WeatherSystemController.Instance.CurrentWeatherStrength == WeatherSystemController.WeatherStrength.Extreme && !GameManager.IsDungeon(PlayerFarming.Location) && !LocationManager.IndoorLocations.Contains(PlayerFarming.Location);
    }
  }

  public void Awake() => this.playerFarming = this.gameObject.GetComponent<PlayerFarming>();

  public void Start()
  {
    this.unitObject = this.gameObject.GetComponent<UnitObject>();
    this.state = this.gameObject.GetComponent<StateMachine>();
    this.inventory = this.gameObject.GetComponent<Inventory>();
    this.circleCollider2D = this.GetComponent<CircleCollider2D>();
    this.GrappleChain.gameObject.SetActive(false);
    this.DefaultRunSpeed = this.RunSpeed;
    this.playerWeapon = this.gameObject.GetComponent<PlayerWeapon>();
    this._skeletonGhost.ghostingEnabled = false;
    this.enlargedCollider = this.GetComponentInChildren<DamageCollider>(true);
    this.SkeletonLOD = this.SpineTransform.GetComponent<SkeletonAnimationLODManager>();
    if ((UnityEngine.Object) this.SkeletonLOD != (UnityEngine.Object) null)
    {
      this.SkeletonLOD.IgnoreCulling = true;
    }
    else
    {
      SkeletonAnimation component = this.SpineTransform.GetComponent<SkeletonAnimation>();
      if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null)
        SkeletonAnimationLODGlobalManager.Instance.DisableCulling(this.SpineTransform, component);
    }
    this.playerFarming.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.state.OnStateChange += new StateMachine.StateChange(this.ResetDodge);
    this.state.OnStateChange += new StateMachine.StateChange(this.OnStateChanged);
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance, 1f).SetEase<Tweener>(Ease.OutQuart);
  }

  public void OnEnable()
  {
    this.health = this.gameObject.GetComponent<Health>();
    if (!((UnityEngine.Object) this.health != (UnityEngine.Object) null))
      return;
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null && (UnityEngine.Object) this.playerFarming.Spine != (UnityEngine.Object) null)
      this.playerFarming.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
    {
      this.health.OnHit -= new Health.HitAction(this.OnHit);
      this.health.OnDie -= new Health.DieAction(this.OnDie);
    }
    if ((UnityEngine.Object) this.state != (UnityEngine.Object) null)
    {
      this.state.OnStateChange -= new StateMachine.StateChange(this.ResetDodge);
      this.state.OnStateChange -= new StateMachine.StateChange(this.OnStateChanged);
    }
    this.EnemiesTurnedOff.Clear();
    this.SpritesTurnedOff.Clear();
  }

  public float UntouchableTimer => this.untouchableTimer;

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (this.health.untouchable)
      return;
    this.playerWeapon.CancelHeavyAttackFromDamage();
    this.playerFarming.simpleSpineAnimator.FlashRedTint();
    this.state.facingAngle = !((UnityEngine.Object) Attacker == (UnityEngine.Object) null) ? Utils.GetAngle(this.transform.position, Attacker.transform.position) : Utils.GetAngle(this.transform.position, AttackLocation);
    this.forceDir = this.state.facingAngle + 180f;
    CameraManager.shakeCamera(0.5f, -this.state.facingAngle);
    if (this.state.CURRENT_STATE != StateMachine.State.Grabbed)
    {
      if (this.HitEffectsCoroutine != null)
        this.StopCoroutine(this.HitEffectsCoroutine);
      this.HitEffectsCoroutine = this.StartCoroutine((IEnumerator) this.HitEffects(StateMachine.State.HitThrown, StateMachine.State.HitRecover));
      this.MakeUntouchable(1f * DifficultyManager.GetInvincibleTimeMultiplier(), false);
    }
    BiomeConstants.Instance.EmitHitVFX(AttackLocation, Quaternion.identity.z, "HitFX_Blocked");
    AudioManager.Instance.ToggleFilter(SoundParams.HitFilter, true);
    AudioManager.Instance.ToggleFilter(SoundParams.HitFilter, false, 0.2f);
    GameManager.GetInstance().HitStop();
    AudioManager.Instance.PlayOneShot("event:/player/gethit", this.transform.position);
  }

  public void MakeUntouchable(float duration, bool ghostEffect = true)
  {
    if ((double) duration <= 0.0)
      return;
    if ((double) this.untouchableTimer < (double) duration)
      this.untouchableTimer = duration;
    this.showGhostEffect = ghostEffect;
    this.health.untouchable = true;
    DeviceLightingManager.TransitionLighting(Color.red, Color.red, 0.0f, DeviceLightingManager.F_KEYS);
    this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() =>
    {
      DeviceLightingManager.TransitionLighting(Color.white, Color.white, 0.0f, DeviceLightingManager.F_KEYS);
      this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() =>
      {
        DeviceLightingManager.TransitionLighting(Color.red, Color.red, 0.0f, DeviceLightingManager.F_KEYS);
        this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() => DeviceLightingManager.PulseAllLighting(Color.white, Color.black, 0.1f, DeviceLightingManager.F_KEYS))));
      })));
    })));
  }

  public IEnumerator Delay(float delay, System.Action callback)
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

  public void MakeImmuneToProjectiles(float duration)
  {
    this.immuneToProjectilesTimer = duration;
    this.health.IgnoreProjectiles = true;
  }

  public IEnumerator HitEffects(StateMachine.State EntryState, StateMachine.State NextState)
  {
    this.HitTimer = 0.0f;
    this.speed = 0.0f;
    this.HitDuration = 1f;
    this.health.untouchable = true;
    this.untouchableTimer = 1f * DifficultyManager.GetInvincibleTimeMultiplier();
    this.Z = 0.0f;
    this.state.CURRENT_STATE = EntryState;
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

  public void KillPlayer()
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
    if ((double) this.playerFarming.playerRelic.PlayerScaleModifier > 1.0)
      CameraManager.instance.ShakeCameraForDuration(0.15f, 0.2f, 0.1f);
    if ((double) --this.FootPrints <= 0.0)
      return;
    BiomeConstants.Instance.EmitFootprintsParticles(this.transform.position, this.FootStepColor, Mathf.Min(1f, this.FootPrints / this.FootPrintModifier));
  }

  public void OnKnockout()
  {
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.SimpleSpineFlash.FlashWhite(false);
    this.health.invincible = true;
    this.health.untouchable = true;
    this.GetComponent<Interactor>().HideIndicator();
    this.state.CURRENT_STATE = StateMachine.State.Dieing;
    if ((UnityEngine.Object) Attacker != (UnityEngine.Object) null)
      this.state.facingAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
    this.forceDir = this.state.facingAngle + 180f;
    DataManager.Instance.PlayerDamageReceived += 50f;
    if (MMConversation.isPlaying)
      MMConversation.mmConversation?.Close();
    if (CoopManager.CoopActive)
    {
      bool flag = false;
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
      {
        PlayerFarming player = PlayerFarming.players[index];
        if (!player.IsKnockedOut && (UnityEngine.Object) player != (UnityEngine.Object) this.playerFarming)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
      {
        PlayerFarming.SetStateForAllPlayers(StateMachine.State.InActive);
        this.state.CURRENT_STATE = StateMachine.State.GameOver;
      }
      else
      {
        CoopManager.KnockOutPlayer(this.playerFarming);
        return;
      }
    }
    else
      GameManager.GetInstance().HitStop(0.3f);
    this.state.CURRENT_STATE = StateMachine.State.GameOver;
    this.state.LockStateChanges = true;
    this.playerFarming.Spine.UseDeltaTime = false;
    this.playerFarming.Spine.timeScale = 1f;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.playerFarming.Spine.CustomMaterialOverride.Clear();
    this.playerFarming.Spine.CustomMaterialOverride.Add(this.playerFarming.originalMaterial, this.playerFarming.BW_Material);
    HUD_Manager.Instance.ShowBW(1f, 0.0f, 1f);
    AudioManager.Instance.StopActiveLoops();
    AudioManager.Instance.PlayOneShot("event:/player/death_hit", this.gameObject);
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
      if (spriteRenderer.enabled && spriteRenderer.gameObject.activeSelf && (double) Vector3.Distance(spriteRenderer.transform.position, this.transform.position) < 2.0 && (double) spriteRenderer.transform.position.y < (double) this.transform.position.y)
      {
        spriteRenderer.enabled = false;
        this.SpritesTurnedOff.Add(spriteRenderer);
      }
    }
    DOTween.To(new DOSetter<float>(GameManager.SetDither), Shader.GetGlobalFloat(GameManager.GlobalDitherIntensity), SettingsManager.Settings.Accessibility.DitherFadeDistance * 2f, 1f).SetEase<Tweener>(Ease.OutQuart);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 4f);
    ++DataManager.Instance.playerDeaths;
    ++DataManager.Instance.playerDeathsInARow;
    if (!TrinketManager.HasTrinket(TarotCards.Card.TheDeal, this.playerFarming) || TrinketManager.IsOnCooldown(TarotCards.Card.TheDeal, this.playerFarming))
    {
      UIBossHUD.Hide();
      AudioManager.Instance.PlayMusic("event:/music/game_over/game_over");
    }
    if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && BiomeGenerator.Instance.CurrentX == BiomeGenerator.BossCoords.x && BiomeGenerator.Instance.CurrentY == BiomeGenerator.BossCoords.y)
      ++DataManager.Instance.playerDeathsInARowFightingLeader;
    CameraManager.shakeCamera(0.5f, -this.state.facingAngle);
    foreach (CompanionCrusade allCompanion in CompanionCrusade.AllCompanions)
    {
      CompanionCrusade thisCompanion = allCompanion;
      GameManager.GetInstance().RemoveFromCamera(thisCompanion.gameObject);
      thisCompanion.transform.DOScale(Vector3.zero, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
      {
        GameManager.GetInstance().RemoveFromCamera(thisCompanion.gameObject);
        thisCompanion.gameObject.SetActive(false);
      }));
    }
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
    this.playerFarming.Spine.UseDeltaTime = false;
    this.playerFarming.Spine.timeScale = 1f;
    DOTween.To((DOGetter<float>) (() => GameManager.GetInstance().CamFollowTarget.targetDistance), (DOSetter<float>) (x => GameManager.GetInstance().CamFollowTarget.targetDistance = x), 4f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.OutSine).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    this.playerFarming.Spine.CustomMaterialOverride.Clear();
    this.playerFarming.Spine.CustomMaterialOverride.Add(this.playerFarming.originalMaterial, this.playerFarming.BW_Material);
    HUD_Manager.Instance.ShowBW(1f, 0.0f, 1f);
    GameManager.GetInstance().OnConversationNew(false);
    GameManager.GetInstance().OnConversationNext(this.playerFarming.CameraBone, 4f);
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

  public void ResetDodge(StateMachine.State NewState, StateMachine.State PrevState)
  {
    if (PrevState != StateMachine.State.Dodging || PrevState == NewState || (double) this.DodgeTimer <= 0.05000000074505806)
      return;
    this.DodgeTimer = 0.0f;
    this.DodgeCollisionDelay = 0.0f;
  }

  public void OnStateChanged(StateMachine.State NewState, StateMachine.State PrevState)
  {
    if (NewState != StateMachine.State.Idle && NewState != StateMachine.State.Moving || !this.BlizzardAnims)
      return;
    this.state.CURRENT_STATE = NewState == StateMachine.State.Idle ? StateMachine.State.Idle_Winter : StateMachine.State.Moving_Winter;
    this.SetSpecialMovingAnimations("blizzard/idle", "blizzard/run-up", "blizzard/run-down", "blizzard/run", "blizzard/run-up-diagonal", "blizzard/run-horizontal", StateMachine.State.Idle_Winter);
  }

  public void ToggleGhost(bool toggle) => this._skeletonGhost.ghostingEnabled = toggle;

  public void DoIdle() => this.state.CURRENT_STATE = StateMachine.State.Idle;

  public bool PlayInvincibleSFX
  {
    get => this.\u003CPlayInvincibleSFX\u003Ek__BackingField;
    set => this.\u003CPlayInvincibleSFX\u003Ek__BackingField = value;
  }

  public void Update()
  {
    if ((double) Time.timeScale <= 0.0 && this.state.CURRENT_STATE != StateMachine.State.CoopReviving && this.state.CURRENT_STATE != StateMachine.State.Resurrecting && this.state.CURRENT_STATE != StateMachine.State.GameOver && this.state.CURRENT_STATE != StateMachine.State.FinalGameOver)
      return;
    if ((double) this.untouchableTimer > 0.0 && this.state.CURRENT_STATE != StateMachine.State.InActive && !this.playerFarming.GoToAndStopping)
    {
      if (this.showGhostEffect)
      {
        if (this.playerFarming.playerRelic.InvincibleFromRelic)
        {
          Time.timeScale = 1.25f;
          AudioManager.Instance.SetMusicPitch(1.25f);
        }
        if (!this.playedLoop)
        {
          this._loopSound = AudioManager.Instance.CreateLoop("event:/relics/invincible", this.gameObject, true);
          this.playedLoop = true;
        }
        this._skeletonGhost.ghostingEnabled = true;
      }
      this.SimpleSpineFlash.FlashMeWhite(1f, 7);
      if (this.state.CURRENT_STATE != StateMachine.State.InActive && !this.playerFarming.GoToAndStopping)
        this.untouchableTimer -= Time.deltaTime;
      if (!this.playedFirstTickSfx && (double) this.untouchableTimer < 3.0 && (double) this.untouchableTimer > 0.0)
      {
        if (this.PlayInvincibleSFX)
          AudioManager.Instance.PlayOneShot("event:/dlc/combat/effect_invincible_countdown_tick", this.transform.position);
        this.playedFirstTickSfx = true;
      }
      else if (!this.playedSecondTickSfx && (double) this.untouchableTimer < 2.0 && (double) this.untouchableTimer > 0.0)
      {
        if (this.PlayInvincibleSFX)
          AudioManager.Instance.PlayOneShot("event:/dlc/combat/effect_invincible_countdown_tick", this.transform.position);
        this.playedSecondTickSfx = true;
      }
      else if (!this.playedThirdTickSfx && (double) this.untouchableTimer < 1.0 && (double) this.untouchableTimer > 0.0)
      {
        if (this.PlayInvincibleSFX)
          AudioManager.Instance.PlayOneShot("event:/dlc/combat/effect_invincible_countdown_tick_final", this.transform.position);
        this.playedThirdTickSfx = true;
      }
      if ((double) this.untouchableTimer <= 0.0)
      {
        if (this.PlayInvincibleSFX)
          AudioManager.Instance.PlayOneShot("event:/dlc/combat/effect_invincible_stop", this.transform.position);
        this.PlayInvincibleSFX = false;
        this.playedFirstTickSfx = false;
        this.playedSecondTickSfx = false;
        if (this.showGhostEffect)
        {
          if (this.playerFarming.playerRelic.InvincibleFromRelic)
          {
            Time.timeScale = 1f;
            AudioManager.Instance.SetMusicPitch(1f);
            this.playerFarming.playerRelic.InvincibleFromRelic = false;
          }
          AudioManager.Instance.StopLoop(this._loopSound);
          this.playedLoop = false;
          this._skeletonGhost.ghostingEnabled = false;
        }
        this.health.untouchable = false;
        DeviceLightingManager.StopAll();
        DeviceLightingManager.UpdateLocation();
        if ((double) this.health.HP <= 1.0)
          DeviceLightingManager.PulseAllLighting(Color.white, Color.red, 0.35f, new KeyCode[0]);
      }
    }
    else if (this.SimpleSpineFlash.isFillWhite)
      this.SimpleSpineFlash.FlashWhite(false);
    if ((double) this.invincibleTimer > 0.0)
    {
      this.invincibleTimer -= Time.deltaTime;
      if ((double) this.invincibleTimer <= 0.0)
        this.health.invincible = false;
    }
    if ((double) this.immuneToProjectilesTimer > 0.0)
    {
      this.immuneToProjectilesTimer -= Time.deltaTime;
      if ((double) this.immuneToProjectilesTimer <= 0.0)
        this.health.IgnoreProjectiles = false;
    }
    if (!this.playerFarming.GoToAndStopping)
    {
      if (this.OverrideForwardMovement)
      {
        float num1 = InputManager.Gameplay.GetHorizontalAxis(this.playerFarming);
        float num2 = InputManager.Gameplay.GetVerticalAxis(this.playerFarming);
        if ((double) Mathf.Abs(num1) <= 0.10000000149011612 && (double) Mathf.Abs(num2) <= 0.10000000149011612)
        {
          Vector2 normalized = Utils.DegreeToVector2(this.state.facingAngle).normalized;
          num1 = normalized.x;
          num2 = normalized.y;
        }
        this.xDir = Mathf.Lerp(this.xDir, num1, 3f * Time.deltaTime);
        this.yDir = Mathf.Lerp(this.yDir, num2, 3f * Time.deltaTime);
      }
      else
      {
        this.xDir = InputManager.Gameplay.GetHorizontalAxis(this.playerFarming);
        this.yDir = InputManager.Gameplay.GetVerticalAxis(this.playerFarming);
      }
      if (this.state.CURRENT_STATE == StateMachine.State.Moving && SettingsManager.Settings.Accessibility.MovementMode == 0)
        this.speed *= Mathf.Clamp01(new Vector2(this.xDir, this.yDir).magnitude);
      if (this.playerFarming.IsKnockedOut)
        this.speed *= 0.33f;
      this.speed *= this.WinterSpeedModifier;
      this.speed = Mathf.Max(this.speed, 0.0f);
      this.unitObject.vx = this.speed * Mathf.Cos(this.forceDir * ((float) Math.PI / 180f));
      this.unitObject.vy = this.speed * Mathf.Sin(this.forceDir * ((float) Math.PI / 180f));
    }
    else
    {
      this.xDir = this.yDir = 0.0f;
      this.playerFarming.Spine.AnimationState.TimeScale = 1f;
    }
    this.enlargedCollider.gameObject.SetActive((double) this.playerFarming.playerRelic.PlayerScaleModifier > 1.0);
    if ((this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving || this.state.CURRENT_STATE == StateMachine.State.Meditate) && DataManager.Instance.GameOver)
      this.OnFinalGameOver();
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
        this.Z = 0.0f;
        this.SpineTransform.localPosition = Vector3.zero;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement)
        {
          this.state.CURRENT_STATE = !this.BlizzardAnims ? StateMachine.State.Moving : StateMachine.State.Moving_Winter;
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
            this.state.CURRENT_STATE = !this.BlizzardAnims ? StateMachine.State.Idle : StateMachine.State.Idle_Winter;
            goto case StateMachine.State.Stealth;
          }
          this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
          if ((double) this.unitObject.vx != 0.0 || (double) this.unitObject.vy != 0.0)
            this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
          this.state.LookAngle = this.state.facingAngle;
          this.speed += (float) (((double) this.GetPlayerMaxSpeed() - (double) this.speed) / 3.0) * GameManager.DeltaTime;
          if (TrinketManager.HasTrinket(TarotCards.Card.CorruptedGoopyTrail, this.playerFarming) && !TrinketManager.IsOnCooldown(TarotCards.Card.CorruptedGoopyTrail, this.playerFarming) && !TrinketManager.IsCorruptedPositiveEffectNegated(TarotCards.Card.CorruptedGoopyTrail, this.playerFarming))
          {
            TrapGoop.CreateGoop(this.transform.position, 1, 0.2f, this.gameObject, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
            TrinketManager.TriggerCooldown(TarotCards.Card.CorruptedGoopyTrail, this.playerFarming);
            goto case StateMachine.State.Stealth;
          }
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
          MMVibrate.Rumble(0.2f, 0.2f, 0.25f, (MonoBehaviour) this, this.playerFarming);
          this.state.facingAngle = this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
        }
        if (!InputManager.Gameplay.GetDodgeButtonHeld(this.playerFarming) && (double) this.DodgeTimer > (double) this.DodgeDuration || (double) this.DodgeTimer > (double) this.DodgeMaxDuration)
        {
          this.DodgeTimer = 0.0f;
          this.DodgeCollisionDelay = 0.0f;
          this.state.CURRENT_STATE = this.BlizzardAnims ? StateMachine.State.Idle_Winter : StateMachine.State.Idle;
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
        for (int index = 0; index < PlayerFarming.playersCount; ++index)
        {
          if (this.playerFarming.isLamb)
          {
            this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
            if ((double) (this.DeathTimer += Time.deltaTime) > 2.0 && !this.GameOver)
            {
              this.GameOver = true;
              this.DeathTimer = 0.0f;
              CoopManager.Instance.RemoveCoopPlayer(this.playerFarming);
              GameManager.ToShip();
            }
          }
          else
            CoopManager.Instance.RemoveCoopPlayer(this.playerFarming);
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
          Vector3 vector3 = new Vector3(this.unitObject.vx, this.unitObject.vy);
          if (vector3 != Vector3.zero)
            this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + vector3);
          this.speed += (float) (((double) this.RunSpeed * 0.699999988079071 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
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
      case StateMachine.State.CoopReviving:
label_246:
        if (this.state.CURRENT_STATE != StateMachine.State.Dodging && this.state.CURRENT_STATE != StateMachine.State.HitThrown && this.state.CURRENT_STATE != StateMachine.State.DashAcrossIsland && this.state.CURRENT_STATE != StateMachine.State.Grapple)
        {
          this.SeperateFromEnemies();
        }
        else
        {
          this.unitObject.seperatorVX = 0.0f;
          this.unitObject.seperatorVY = 0.0f;
        }
        if ((UnityEngine.Object) this.playerFarming.Spine != (UnityEngine.Object) null && this.playerFarming.Spine.AnimationState != null)
          this.playerFarming.Spine.AnimationState.TimeScale = this.state.CURRENT_STATE != StateMachine.State.Moving || this.playerFarming.GoToAndStopping || this.playerFarming.IsKnockedOut ? 1f : Mathf.Clamp01(this.speed / this.RunSpeed);
        if (!this.overridingAnimations)
        {
          if ((double) DataManager.Instance.SurvivalMode_Hunger <= 10.0)
          {
            this.playerFarming.unitObject.maxSpeed = 0.09f;
            this.playerFarming.playerController.RunSpeed = 5.5f;
            this.playerFarming.playerController.DefaultRunSpeed = 5.5f;
            this.SetSpecificMovementAnimations("starving/idle", "starving/run-up", "starving/run-down", "starving/run", "starving/run-up-diagonal", "starving/run-horizontal");
            break;
          }
          if ((double) DataManager.Instance.SurvivalMode_Sleep > 10.0)
            break;
          this.playerFarming.unitObject.maxSpeed = 0.03f;
          this.playerFarming.playerController.RunSpeed = 2f;
          this.playerFarming.playerController.DefaultRunSpeed = 2f;
          this.SetSpecificMovementAnimations("exhausted/idle", "exhausted/run-up", "exhausted/run-down", "exhausted/run", "exhausted/run-up-diagonal", "exhausted/run-horizontal");
          break;
        }
        if ((double) DataManager.Instance.SurvivalMode_Hunger <= 10.0 || (double) DataManager.Instance.SurvivalMode_Sleep <= 10.0 || this.playerFarming.IsKnockedOut || this.playerFarming.IsBurrowing || !DataManager.Instance.SurvivalModeActive)
          break;
        this.playerFarming.unitObject.maxSpeed = 0.09f;
        this.playerFarming.playerController.RunSpeed = 5.5f;
        this.playerFarming.playerController.DefaultRunSpeed = 5.5f;
        this.ResetSpecificMovementAnimations();
        break;
      case StateMachine.State.GameOver:
        if (PlayerController.CanRespawn)
          Time.timeScale = 0.0f;
        if ((bool) (UnityEngine.Object) this.playerFarming)
          this.playerFarming.Spine.UseDeltaTime = false;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        this.state.Timer += Time.unscaledDeltaTime;
        bool flag1 = PlayerFleeceManager.FleecePreventsRespawn();
        bool flag2 = DataManager.Instance.PlayerFleece == 5;
        if (flag1 && !DataManager.Instance.UnlockedCorruptedRelicsAndTarots && DataManager.Instance.DeathCatBeaten)
          flag1 = false;
        bool flag3 = PlayerFarming.Location == FollowerLocation.Boss_Wolf || (UnityEngine.Object) EnemyWolfBoss.Instance != (UnityEngine.Object) null;
        bool flag4 = PlayerFarming.Location == FollowerLocation.Boss_Yngya || (UnityEngine.Object) EnemyYngyaBoss.Instance != (UnityEngine.Object) null;
        bool flag5 = PlayerFarming.Location == FollowerLocation.Boss_5 | flag3 | flag4;
        if (!flag1 && TrinketManager.HasTrinket(TarotCards.Card.TheDeal, this.playerFarming) && !TrinketManager.IsOnCooldown(TarotCards.Card.TheDeal, this.playerFarming))
        {
          this.state.LockStateChanges = false;
          this.state.CURRENT_STATE = StateMachine.State.Resurrecting;
          this.ReenableTurnedOffEnemies();
          this.ReenableTurnedOffSpriteRenderers();
          goto case StateMachine.State.Stealth;
        }
        if (RespawnRoomManager.forceRespawnOnce || !flag1 && ResurrectOnHud.HasRessurection && !((HealthPlayer) this.health).IsNoHeartSlotsLeft | flag2 && !flag5 && !DungeonSandboxManager.Active)
        {
          if (RespawnRoomManager.forceRespawnOnce || PlayerController.CanRespawn && (double) this.state.Timer > 2.0 && !MMTransition.IsPlaying)
          {
            HUD_Manager.Instance.ShowBW(0.5f, 1f, 0.0f);
            this.playerFarming.Spine.UseDeltaTime = true;
            for (int index = 0; index < PlayerFarming.playersCount; ++index)
            {
              PlayerFarming player = PlayerFarming.players[index];
              if (!player.isLamb)
                CoopManager.Instance.RemoveCoopPlayer(player, withDelay: false);
            }
            RespawnRoomManager.Play();
            this.state.LockStateChanges = false;
            this.ReenableTurnedOffEnemies();
            this.ReenableTurnedOffSpriteRenderers();
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
          {
            this.state.facingAngle = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
            if (SettingsManager.Settings.Game.InvertAiming && !SettingsManager.Settings.Accessibility.InvertMovement)
              this.state.facingAngle -= 180f;
          }
          this.state.LookAngle = this.state.facingAngle;
          this.speed += (float) ((0.0 - (double) this.speed) / 7.0) * GameManager.DeltaTime;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Resurrecting:
        ResurrectionType resurrectionType = ResurrectOnHud.ResurrectionType;
        if (resurrectionType != ResurrectionType.CoopRevive)
        {
          int num = 0;
          int count = this.droppedItems.Count;
          for (int index = this.droppedItems.Count - 1; index >= 0; --index)
          {
            ResourceCustomTarget.Create(this.gameObject, this.transform.position + (Vector3) Utils.DegreeToVector2((float) num * (360f / (float) count)), (InventoryItem.ITEM_TYPE) this.droppedItems[index].Type, (System.Action) null);
            Inventory.AddItem(this.droppedItems[index].Type, this.droppedItems[index].Quantity);
            this.droppedItems.Remove(this.droppedItems[index]);
            ++num;
          }
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
          if (TrinketManager.HasTrinket(TarotCards.Card.KillEnemiesOnResurrect))
          {
            for (int index = Health.team2.Count - 1; index >= 0; --index)
            {
              UnitObject component = Health.team2[index].GetComponent<UnitObject>();
              if ((!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.IsBoss) && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<BossIntro>() == (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index].GetComponentInParent<DeathCatController>() == (UnityEngine.Object) null)
                this.StartCoroutine((IEnumerator) this.\u003CUpdate\u003Eg__DamageEnemy\u007C141_2(Health.team2[index]));
            }
          }
          if (resurrectionType != ResurrectionType.CoopRevive)
            this.droppedItems.Clear();
          if ((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null && (bool) (UnityEngine.Object) RespawnRoomManager.Instance && !RespawnRoomManager.Instance.Respawning && (resurrectionType == ResurrectionType.Pyre || resurrectionType == ResurrectionType.CorruptedMonolith))
          {
            AudioManager.Instance.PlayMusic(BiomeGenerator.Instance.biomeMusicPath);
            if (BiomeGenerator.Instance.CurrentRoom != null && (UnityEngine.Object) BiomeGenerator.Instance.CurrentRoom.generateRoom != (UnityEngine.Object) null)
              AudioManager.Instance.SetMusicRoomID(BiomeGenerator.Instance.CurrentRoom.generateRoom.roomMusicID);
          }
          System.Action action = (System.Action) (() =>
          {
            foreach (PlayerFarming player in PlayerFarming.players)
            {
              if (!player.isLamb)
                player.state.LockStateChanges = false;
            }
          });
          this.playerFarming.state.LockStateChanges = false;
          HealthPlayer component1 = this.playerFarming.GetComponent<HealthPlayer>();
          switch (ResurrectOnHud.ResurrectionType)
          {
            case ResurrectionType.DealTarot:
              TrinketManager.TriggerForAllCooldown(TarotCards.Card.TheDeal);
              if ((double) component1.totalHP < 2.0)
                component1.totalHP = 2f;
              component1.HP = 2f;
              this.playerFarming.Spine.CustomMaterialOverride.Clear();
              break;
            case ResurrectionType.CoopRevive:
              this.playerFarming.Spine.CustomMaterialOverride.Clear();
              break;
            case ResurrectionType.CorruptedMonolith:
              action();
              using (List<PlayerFarming>.Enumerator enumerator = PlayerFarming.players.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  PlayerFarming current = enumerator.Current;
                  if (!current.isLamb)
                  {
                    CoopManager.WakeKnockedOutPlayer(current, 0.0f);
                  }
                  else
                  {
                    current.state.CURRENT_STATE = StateMachine.State.Idle;
                    current.playerController.ResetSpecificMovementAnimations();
                  }
                  current.health.FullHeal();
                }
                break;
              }
            default:
              action();
              using (List<PlayerFarming>.Enumerator enumerator = PlayerFarming.players.GetEnumerator())
              {
                while (enumerator.MoveNext())
                {
                  PlayerFarming current = enumerator.Current;
                  if (!current.isLamb)
                    CoopManager.WakeKnockedOutPlayer(current, 0.0f);
                  else
                    current.playerController.ResetSpecificMovementAnimations();
                  current.health.HP = RespawnRoomManager.HP;
                  current.health.SpiritHearts = RespawnRoomManager.SpiritHearts;
                  current.health.BlueHearts = RespawnRoomManager.BlueHearts;
                  current.health.BlackHearts = RespawnRoomManager.BlackHearts;
                  current.health.FireHearts = RespawnRoomManager.FireHearts;
                  current.health.IceHearts = RespawnRoomManager.IceHearts;
                }
                break;
              }
          }
          if (ResurrectOnHud.CachedResurrectionType != ResurrectionType.None)
          {
            ResurrectOnHud.ResurrectionType = ResurrectOnHud.CachedResurrectionType;
            ResurrectOnHud.CachedResurrectionType = ResurrectionType.None;
          }
          else
            ResurrectOnHud.ResurrectionType = ResurrectionType.None;
          using (List<CompanionCrusade>.Enumerator enumerator = CompanionCrusade.AllCompanions.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              CompanionCrusade thisCompanion = enumerator.Current;
              thisCompanion.gameObject.SetActive(true);
              GameManager.GetInstance().RemoveFromCamera(thisCompanion.gameObject);
              thisCompanion.transform.DOScale(Vector3.one, 0.33f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => thisCompanion.RestartFollow()));
            }
            goto case StateMachine.State.Stealth;
          }
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Idle_CarryingBody:
      case StateMachine.State.Idle_Winter:
        this.Z = 0.0f;
        this.SpineTransform.localPosition = Vector3.zero;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        if ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement)
        {
          this.state.CURRENT_STATE = this.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody ? StateMachine.State.Moving_CarryingBody : StateMachine.State.Moving_Winter;
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.Moving_CarryingBody:
      case StateMachine.State.Moving_Winter:
        if (!this.playerFarming.GoToAndStopping)
        {
          if (this.carryUp.Contains("blizzard") && !this.BlizzardAnims)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            this.ResetSpecialMovingAnimations();
            goto case StateMachine.State.Stealth;
          }
          if ((double) Mathf.Abs(this.xDir) <= (double) PlayerController.MinInputForMovement && (double) Mathf.Abs(this.yDir) <= (double) PlayerController.MinInputForMovement)
          {
            this.state.CURRENT_STATE = this.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody ? StateMachine.State.Idle_CarryingBody : StateMachine.State.Idle_Winter;
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
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Down:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryDown)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryDown, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Left:
            case Utils.DirectionFull.Right:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryHorizontal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryHorizontal, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Up_Diagonal:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryUpDiagonal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryUpDiagonal, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Down_Diagonal:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.carryDownDiagonal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.carryDownDiagonal, true);
                goto label_246;
              }
              goto label_246;
            default:
              goto label_246;
          }
        }
        else
          goto case StateMachine.State.Stealth;
      case StateMachine.State.FinalGameOver:
        Time.timeScale = 0.0f;
        this.playerFarming.Spine.UseDeltaTime = false;
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        this.state.Timer += Time.unscaledDeltaTime;
        if ((double) this.state.Timer > 2.0 && (UnityEngine.Object) this._deathScreenInstance == (UnityEngine.Object) null)
        {
          this._deathScreenInstance = MonoSingleton<UIManager>.Instance.ShowDeathScreenOverlay(UIDeathScreenOverlayController.Results.GameOver, 0);
          goto case StateMachine.State.Stealth;
        }
        goto case StateMachine.State.Stealth;
      case StateMachine.State.ChargingSnowball:
        if ((double) Mathf.Abs(this.xDir) > (double) PlayerController.MinInputForMovement || (double) Mathf.Abs(this.yDir) > (double) PlayerController.MinInputForMovement)
        {
          this.forceDir = Utils.GetAngle(Vector3.zero, new Vector3(this.xDir, this.yDir));
          if ((double) this.unitObject.vx != 0.0 || (double) this.unitObject.vy != 0.0)
            this.state.facingAngle = Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(this.unitObject.vx, this.unitObject.vy));
          this.state.LookAngle = this.state.facingAngle;
          this.speed += (float) (((double) this.RunSpeed * 0.5 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
          switch (Utils.GetAngleDirectionFull(this.state.facingAngle))
          {
            case Utils.DirectionFull.Up:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.snowballUp)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.snowballUp, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Down:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.snowballDown)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.snowballDown, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Left:
            case Utils.DirectionFull.Right:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.snowballHorizontal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.snowballHorizontal, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Up_Diagonal:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.snowballUpDiagonal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.snowballUpDiagonal, true);
                goto label_246;
              }
              goto label_246;
            case Utils.DirectionFull.Down_Diagonal:
              if (this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != this.snowballDownDiagonal)
              {
                this.playerFarming.Spine.AnimationState.SetAnimation(0, this.snowballDownDiagonal, true);
                goto label_246;
              }
              goto label_246;
            default:
              goto label_246;
          }
        }
        else
        {
          if (this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
            this.state.facingAngle = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(this.transform.position), (Vector3) InputManager.General.GetMousePosition(this.playerFarming));
          string name = this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name;
          if (!(name == "snowball/idle-charge") && !(name == "snowball/idle-charged"))
          {
            Debug.Log((object) "Setting snowball idle charge animation");
            if ((double) this.playerFarming.chargeSnowball >= 1.0)
            {
              this.playerFarming.Spine.AnimationState.SetAnimation(0, "snowball/idle-charged", true);
            }
            else
            {
              TrackEntry trackEntry = this.playerFarming.Spine.AnimationState.SetAnimation(0, "snowball/idle-charge", false);
              trackEntry.TrackTime = Mathf.Lerp(trackEntry.AnimationStart, trackEntry.AnimationEnd, this.playerFarming.chargeSnowball);
              this.playerFarming.Spine.AnimationState.AddAnimation(0, "snowball/idle-charged", true, 0.0f);
            }
          }
          this.speed = 0.0f;
          goto case StateMachine.State.Stealth;
        }
      default:
        this.speed += (float) ((0.0 - (double) this.speed) / 3.0) * GameManager.DeltaTime;
        goto case StateMachine.State.Stealth;
    }
  }

  public float GetPlayerMaxSpeed()
  {
    return (float) ((double) this.RunSpeed * ((double) this.playerFarming.CurrentWeaponInfo.MovementSpeedMultiplier * (double) TrinketManager.GetMovementSpeedMultiplier(this.playerFarming)) * (double) Mathf.Lerp(1.25f, 0.7f, this.playerFarming.playerRelic.PlayerScaleModifier / 2f) * (GameManager.IsDungeon(PlayerFarming.Location) ? (double) PlayerFleeceManager.FleeceSpeedMultiplier() : 1.0));
  }

  public void SetSpecificMovementAnimations(
    string idle,
    string up,
    string down,
    string downDiagonal,
    string upDiagonal,
    string horizontal)
  {
    this.overridingAnimations = true;
    this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, idle);
    this.playerFarming.simpleSpineAnimator.Idle = this.playerFarming.simpleSpineAnimator.GetAnimationReference(idle);
    this.playerFarming.simpleSpineAnimator.NorthIdle = this.playerFarming.simpleSpineAnimator.GetAnimationReference(idle);
    this.playerFarming.simpleSpineAnimator.NorthMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference(up);
    this.playerFarming.simpleSpineAnimator.SouthMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference(down);
    this.playerFarming.simpleSpineAnimator.NorthDiagonalMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference(upDiagonal);
    this.playerFarming.simpleSpineAnimator.SouthDiagonalMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference(downDiagonal);
    this.playerFarming.simpleSpineAnimator.Moving = this.playerFarming.simpleSpineAnimator.GetAnimationReference(horizontal);
  }

  public void ResetSpecificMovementAnimations()
  {
    this.overridingAnimations = false;
    this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Idle, "idle");
    this.playerFarming.simpleSpineAnimator.Idle = this.playerFarming.simpleSpineAnimator.GetAnimationReference("idle");
    this.playerFarming.simpleSpineAnimator.NorthIdle = this.playerFarming.simpleSpineAnimator.GetAnimationReference("idle");
    this.playerFarming.simpleSpineAnimator.NorthMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference("run-up");
    this.playerFarming.simpleSpineAnimator.SouthMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference("run-down");
    this.playerFarming.simpleSpineAnimator.NorthDiagonalMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference("run-up-diagonal");
    this.playerFarming.simpleSpineAnimator.SouthDiagonalMoving = this.playerFarming.simpleSpineAnimator.GetAnimationReference("run");
    this.playerFarming.simpleSpineAnimator.Moving = this.playerFarming.simpleSpineAnimator.GetAnimationReference("run-horizontal");
  }

  public void SetSpecialMovingAnimations(
    string idle,
    string carryUp,
    string carryDown,
    string carryDownDiagonal,
    string carryUpDiagonal,
    string carryHorizontal,
    StateMachine.State idleState = StateMachine.State.Idle_CarryingBody)
  {
    this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(idleState, idle);
    this.carryUp = carryUp;
    this.carryDown = carryDown;
    this.carryDownDiagonal = carryDownDiagonal;
    this.carryUpDiagonal = carryUpDiagonal;
    this.carryHorizontal = carryHorizontal;
  }

  public void ResetSpecialMovingAnimations(StateMachine.State idleState = StateMachine.State.Idle_CarryingBody)
  {
    this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(idleState, "corpse/corpse-idle");
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
    this.unitObject.DoKnockBack(Utils.Repeat(this.state.facingAngle + 180f, 360f) * ((float) Math.PI / 180f), hitKnockback, 0.1f);
  }

  public void Recoil(AnimationCurve recoilCurve, float duration, float powerMultiplier)
  {
    if (this.UpdateRecoilRoutine != null)
      this.StopCoroutine(this.UpdateRecoilRoutine);
    this.UpdateRecoilRoutine = this.StartCoroutine((IEnumerator) this.UpdateRecoil(recoilCurve, duration, powerMultiplier));
  }

  public IEnumerator UpdateRecoil(
    AnimationCurve recoilCurve,
    float duration,
    float powerMultiplier)
  {
    float elapsedTime = 0.0f;
    while ((double) elapsedTime < (double) duration)
    {
      Vector3 vector3 = (Vector3) new Vector2(Mathf.Cos((float) (((double) this.state.facingAngle + 180.0) * (Math.PI / 180.0))), Mathf.Sin((float) (((double) this.state.facingAngle + 180.0) * (Math.PI / 180.0))));
      vector3.Normalize();
      float num = Time.deltaTime * recoilCurve.Evaluate(1f / duration * elapsedTime) * powerMultiplier;
      this.unitObject.moveVX = vector3.x * num;
      this.unitObject.moveVY = vector3.y * num;
      elapsedTime += Time.fixedDeltaTime;
      yield return (object) null;
    }
  }

  public void SeperateFromEnemies() => this.unitObject.Seperate(1f);

  public void ReenableTurnedOffEnemies()
  {
    foreach (MeshRenderer meshRenderer in this.EnemiesTurnedOff)
    {
      if ((UnityEngine.Object) meshRenderer != (UnityEngine.Object) null)
        meshRenderer.enabled = true;
    }
    this.EnemiesTurnedOff.Clear();
  }

  public void ReenableTurnedOffSpriteRenderers()
  {
    foreach (SpriteRenderer spriteRenderer in this.SpritesTurnedOff)
    {
      if ((UnityEngine.Object) spriteRenderer != (UnityEngine.Object) null)
        spriteRenderer.enabled = true;
    }
    this.SpritesTurnedOff.Clear();
  }

  public void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "invincibility_ON":
        this.health.invincible = true;
        break;
      case "invincibility_OFF":
        if ((double) this.invincibleTimer > 0.0)
          break;
        this.health.invincible = false;
        break;
    }
  }

  [CompilerGenerated]
  public void \u003CMakeUntouchable\u003Eb__84_0()
  {
    DeviceLightingManager.TransitionLighting(Color.white, Color.white, 0.0f, DeviceLightingManager.F_KEYS);
    this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() =>
    {
      DeviceLightingManager.TransitionLighting(Color.red, Color.red, 0.0f, DeviceLightingManager.F_KEYS);
      this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() => DeviceLightingManager.PulseAllLighting(Color.white, Color.black, 0.1f, DeviceLightingManager.F_KEYS))));
    })));
  }

  [CompilerGenerated]
  public void \u003CMakeUntouchable\u003Eb__84_1()
  {
    DeviceLightingManager.TransitionLighting(Color.red, Color.red, 0.0f, DeviceLightingManager.F_KEYS);
    this.StartCoroutine((IEnumerator) this.Delay(0.1f, (System.Action) (() => DeviceLightingManager.PulseAllLighting(Color.white, Color.black, 0.1f, DeviceLightingManager.F_KEYS))));
  }

  [CompilerGenerated]
  public IEnumerator \u003CUpdate\u003Eg__DamageEnemy\u007C141_2(Health health)
  {
    health.invincible = false;
    BiomeConstants.Instance.ShowTarotCardDamage(health.transform, Vector3.up * 1.5f);
    yield return (object) new WaitForSeconds(0.75f);
    health.invincible = false;
    health.untouchable = false;
    StateMachine.State currentState = this.state.CURRENT_STATE;
    this.state.CURRENT_STATE = StateMachine.State.Resurrecting;
    try
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null)
        health.DealDamage(health.HP, health.gameObject, health.transform.position, dealDamageImmediately: true, AttackFlags: Health.AttackFlags.ForceKill);
    }
    finally
    {
      this.state.CURRENT_STATE = currentState;
    }
  }

  public struct DroppedItem
  {
    public int Type;
    public int Quantity;
  }
}
