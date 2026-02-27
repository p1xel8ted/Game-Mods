// Decompiled with JetBrains decompiler
// Type: PlayerSpells
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using MMRoomGeneration;
using Rewired;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayerSpells : BaseMonoBehaviour
{
  public const int AmmoCostBaseLine = 45;
  private PlayerFarming playerFarming;
  private StateMachine state;
  private Health health;
  private PlayerController playerController;
  private float castTimer;
  public SkeletonAnimation Spine;
  private float aimTimer;
  private static float minAimTime = 0.15f;
  private static float maxAimTime = 0.3f;
  [SerializeField]
  private GameObject projectileCorrectTimingParticle;
  public static System.Action CurseDamaged;
  public static System.Action CurseBroken;
  private const float projectileAOEShootDistance = 5f;
  [SerializeField]
  private LayerMask collisionMask;
  private float explosiveProjectileMaxChargeDuration = 0.75f;
  private float megaSlashMaxChargeDuration = 0.75f;
  private float chargeTimer;
  public static PlayerSpells.CurseEvent OnCurseChanged;
  private Vector3 targetPosition;
  private float acceleration;
  private bool aiming;
  private Plane plane = new Plane(Vector3.forward, Vector3.zero);
  private const float maxProjectileAOEDistance = 6f;
  private float TimeScaleDelay;
  public bool ForceSpells;
  private bool cantafford;
  public float AimAngle;
  private Health AimTarget;
  private float ArrowAttackDelay;
  private bool invokedDamage;
  public static System.Action OnCastSpell;

  public static int AmmoCost
  {
    get
    {
      return Mathf.RoundToInt(45f / TrinketManager.GetAmmoEfficiencyMultiplier() * DataManager.Instance.CurseFeverMultiplier * PlayerFleeceManager.GetCursesFeverMultiplier());
    }
  }

  public float SlowMotionSpeed
  {
    get
    {
      switch (DataManager.Instance.CurrentCurse)
      {
        case EquipmentType.ProjectileAOE:
        case EquipmentType.ProjectileAOE_ExplosiveImpact:
        case EquipmentType.ProjectileAOE_BiggerRadius:
        case EquipmentType.ProjectileAOE_GoopTrail:
        case EquipmentType.ProjectileAOE_Charm:
          return 0.25f;
        default:
          return 0.4f;
      }
    }
  }

  private void Start()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.playerController = this.GetComponent<PlayerController>();
    this.SetSpell(DataManager.Instance.CurrentCurse, DataManager.Instance.CurrentCurseLevel);
  }

  public void SetSpell(EquipmentType Spell, int CurseLevel)
  {
    DataManager.Instance.CurrentCurse = Spell;
    DataManager.Instance.CurrentCurseLevel = CurseLevel;
    PlayerSpells.CurseEvent onCurseChanged = PlayerSpells.OnCurseChanged;
    if (onCurseChanged == null)
      return;
    onCurseChanged(DataManager.Instance.CurrentCurse, DataManager.Instance.CurrentCurseLevel);
  }

  private Transform spellParent
  {
    get
    {
      return !(bool) (UnityEngine.Object) GenerateRoom.Instance ? this.transform.parent : GenerateRoom.Instance.transform;
    }
  }

  private void Update()
  {
    if (!DataManager.Instance.EnabledSpells || DataManager.Instance.CurrentCurse == EquipmentType.None)
      return;
    this.ArrowAttackDelay -= Time.deltaTime;
    if (this.playerFarming.GoToAndStopping || (double) Time.timeScale == 0.0)
      return;
    if (InputManager.Gameplay.GetCurseButtonDown())
      this.aiming = true;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
      case StateMachine.State.Aiming:
        if (!LocationManager.LocationIsDungeon(PlayerFarming.Location) && !this.ForceSpells || !this.aiming)
          return;
        if (this.state.CURRENT_STATE == StateMachine.State.Aiming && InputManager.General.MouseInputActive)
          this.state.facingAngle = this.state.LookAngle = this.playerController.forceDir = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(this.transform.position), (Vector3) InputManager.General.GetMousePosition());
        if (!FaithAmmo.CanAfford((float) PlayerSpells.AmmoCost))
        {
          if (InputManager.Gameplay.GetCurseButtonDown())
          {
            FaithAmmo.UseAmmo((float) PlayerSpells.AmmoCost);
            AudioManager.Instance.PlayOneShot("event:/player/Curses/noarrows", this.gameObject);
          }
        }
        else if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).CanAim)
        {
          if (InputManager.Gameplay.GetCurseButtonUp())
            this.CastCurrentSpell();
        }
        else if (InputManager.Gameplay.GetCurseButtonDown())
          this.CastCurrentSpell(false);
        if (this.state.CURRENT_STATE == StateMachine.State.Aiming)
        {
          EquipmentType primaryEquipmentType = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType;
          switch (primaryEquipmentType)
          {
            case EquipmentType.Fireball:
              this.ChargeFireball();
              break;
            case EquipmentType.MegaSlash:
              this.ChargeMegaSlash();
              break;
          }
          if (primaryEquipmentType == EquipmentType.Fireball || primaryEquipmentType == EquipmentType.Tentacles || primaryEquipmentType == EquipmentType.MegaSlash)
          {
            GameManager.GetInstance().CamFollowTarget.SetOffset((Vector3) (Utils.DegreeToVector2(this.state.facingAngle) * 2f));
            break;
          }
          break;
        }
        GameManager.GetInstance().CamFollowTarget.SetOffset(Vector3.zero);
        break;
      case StateMachine.State.Dodging:
        if (this.aiming)
        {
          this.aiming = false;
          this.chargeTimer = 0.0f;
          this.aimTimer = 0.0f;
          GameManager.GetInstance().CamFollowTarget.SetOffset(Vector3.zero);
        }
        if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).CanAim && this.CanCastSpell() && InputManager.Gameplay.GetCurseButtonDown() && FaithAmmo.CanAfford((float) PlayerSpells.AmmoCost) && !LetterBox.IsPlaying && this.state.CURRENT_STATE != StateMachine.State.CustomAnimation)
        {
          this.aiming = true;
          this.state.CURRENT_STATE = StateMachine.State.Aiming;
          this.playerController.speed = 0.0f;
          break;
        }
        break;
      case StateMachine.State.Casting:
        if ((double) (this.castTimer -= Time.deltaTime) <= 0.0)
        {
          this.state.CURRENT_STATE = StateMachine.State.Idle;
          if ((UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) null)
          {
            Interactor.CurrentInteraction.HasChanged = true;
            break;
          }
          break;
        }
        break;
      default:
        this.playerFarming.HideProjectileChargeBars();
        if (!LocationManager.LocationIsDungeon(PlayerFarming.Location) && !this.ForceSpells)
          return;
        break;
    }
    if (InputManager.Gameplay.GetCurseButtonDown())
      this.AimAngle = this.state.LookAngle;
    if (!FaithAmmo.CanAfford((float) PlayerSpells.AmmoCost))
      this.cantafford = true;
    if (this.cantafford && FaithAmmo.CanAfford((float) PlayerSpells.AmmoCost))
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/reload", this.transform.position);
      this.cantafford = false;
    }
    if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).CanAim && this.CanCastSpell() && InputManager.Gameplay.GetCurseButtonHeld() && FaithAmmo.CanAfford((float) PlayerSpells.AmmoCost) && !LetterBox.IsPlaying && this.state.CURRENT_STATE != StateMachine.State.Attacking && this.state.CURRENT_STATE != StateMachine.State.CustomAnimation && this.aiming)
    {
      if ((double) (this.aimTimer += Time.unscaledDeltaTime) <= (double) PlayerSpells.minAimTime)
        return;
      Time.timeScale = this.SlowMotionSpeed;
      if (this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving)
      {
        this.state.CURRENT_STATE = StateMachine.State.Aiming;
        if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType == EquipmentType.MegaSlash)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/mega_slash_charge", this.gameObject);
        else if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType == EquipmentType.ProjectileAOE)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_charge", this.gameObject);
        else
          AudioManager.Instance.PlayOneShot("event:/player/Curses/start_cast", this.gameObject);
        if (!string.IsNullOrEmpty(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PerformActionAnimationLoop))
          this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Aiming, EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PerformActionAnimationLoop);
        this.targetPosition = Vector3.zero;
        this.playerController.speed = 0.0f;
      }
      this.AimAngle = InputManager.General.GetLastActiveController().type != ControllerType.Keyboard || (double) Mathf.DeltaAngle(this.AimAngle, this.state.LookAngle) >= 100.0 ? this.state.LookAngle : Mathf.LerpAngle(this.AimAngle, this.state.LookAngle, 15f * Time.unscaledTime);
      if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType == EquipmentType.ProjectileAOE)
      {
        Vector3 b = this.targetPosition + this.transform.position;
        if (this.state.CURRENT_STATE == StateMachine.State.Aiming && InputManager.General.MouseInputActive)
          b = this.GetProjectileAOETargetWithMouse();
        for (int index = 0; index < 20; ++index)
        {
          float num = (float) index / 20f;
          Vector3 position = Vector3.Lerp(this.transform.position + Vector3.forward, b, num) with
          {
            z = this.transform.position.z + (float) (-(double) this.playerFarming.CurseAimingCurve.Evaluate(num) * 3.0)
          };
          this.playerFarming.CurseAimLine.SetPosition(index, position);
        }
        this.playerFarming.CurseTarget.SetActive(true);
        this.playerFarming.CurseTarget.transform.position = b;
        if ((double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) > 0.10000000149011612 || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) > 0.10000000149011612)
          this.acceleration = Mathf.Clamp01(this.acceleration + 8f * Time.unscaledDeltaTime);
        else if ((double) this.acceleration > 0.0)
          this.acceleration = Mathf.Clamp01(this.acceleration - 11f * Time.unscaledDeltaTime);
        this.targetPosition += (Vector3) Utils.DegreeToVector2(this.AimAngle) * 10f * this.acceleration * Time.unscaledDeltaTime;
        this.targetPosition = Vector3.ClampMagnitude(this.targetPosition, 6f);
      }
      else
      {
        this.playerFarming.ShowProjectileChargeBars();
        float z;
        if (InputManager.General.GetLastActiveController().type == ControllerType.Keyboard)
        {
          Health autoAimTarget = this.GetAutoAimTarget(true);
          float num = this.AimAngle;
          if ((UnityEngine.Object) autoAimTarget != (UnityEngine.Object) null)
            num = Utils.GetAngle(this.transform.position, autoAimTarget.transform.position);
          z = num;
        }
        else
          z = this.AimAngle;
        this.playerFarming.SetAimingRecticuleScaleAndRotation(0, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, z));
        this.playerFarming.SetAimingRecticuleScaleAndRotation(1, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, Mathf.Repeat(z + 180f, 360f)));
        this.playerFarming.SetAimingRecticuleScaleAndRotation(2, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, Mathf.Repeat(z + 90f, 360f)));
        this.playerFarming.SetAimingRecticuleScaleAndRotation(3, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, Mathf.Repeat(z + 270f, 360f)));
      }
    }
    else
    {
      Time.timeScale = 1f;
      this.aimTimer = 0.0f;
      this.chargeTimer = 0.0f;
      this.playerFarming.HideProjectileChargeBars();
      this.targetPosition = Vector3.zero;
      if (this.state.CURRENT_STATE == StateMachine.State.Aiming)
      {
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        if ((UnityEngine.Object) Interactor.CurrentInteraction != (UnityEngine.Object) null)
          Interactor.CurrentInteraction.HasChanged = true;
      }
      if (!this.playerFarming.CurseTarget.activeSelf)
        return;
      this.playerFarming.CurseAimLine.SetPositions(new Vector3[20]);
      this.playerFarming.CurseTarget.SetActive(false);
    }
  }

  private Vector3 GetProjectileAOETargetWithMouse()
  {
    Ray ray = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().ScreenPointToRay((Vector3) InputManager.General.GetMousePosition());
    float enter;
    this.plane.Raycast(ray, out enter);
    Vector3 vector3 = ray.GetPoint(enter) - this.transform.position;
    return this.transform.position + vector3.normalized * Mathf.Min(6f, vector3.magnitude);
  }

  private void ChargeFireball()
  {
    this.playerFarming.ShowProjectileChargeBars();
    this.playerFarming.UpdateProjectileChargeBar(this.chargeTimer / this.explosiveProjectileMaxChargeDuration);
    this.chargeTimer += Time.unscaledDeltaTime;
  }

  private IEnumerator SlowMo()
  {
    GameManager.GetInstance().CameraSetZoom(6f);
    float Progress = 0.0f;
    while ((double) (Progress += Time.unscaledDeltaTime) < 0.699999988079071)
    {
      GameManager.SetTimeScale(0.2f);
      yield return (object) null;
    }
    GameManager.SetTimeScale(1f);
    GameManager.GetInstance().CameraResetTargetZoom();
  }

  private void ChargeMegaSlash()
  {
    this.playerFarming.ShowProjectileChargeBars();
    this.playerFarming.UpdateProjectileChargeBar(this.chargeTimer / this.megaSlashMaxChargeDuration);
    this.chargeTimer += Time.unscaledDeltaTime;
    if (!(this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PerformActionAnimationLoop))
      return;
    this.playerFarming.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PerformActionAnimationLoop, true);
  }

  private Health GetAutoAimTarget(bool prioritizeClosest = false, List<Health> blacklist = null)
  {
    float AutoAimArc = 180f;
    float maxValue = float.MaxValue;
    int currentState = (int) this.state.CURRENT_STATE;
    float num1 = 0.0f;
    float num2 = AutoAimArc / 2f;
    foreach (Health allUnit in Health.allUnits)
    {
      if (this.IsPotentialAutoAimTarget(allUnit, AutoAimArc))
      {
        float num3 = Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position);
        if ((double) num3 > (double) num1 && (double) num3 < (double) maxValue)
          num1 = num3;
      }
    }
    float num4 = 1f;
    this.AimTarget = (Health) null;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team == Health.Team.Team2 && this.IsPotentialAutoAimTarget(allUnit, AutoAimArc) && (!((UnityEngine.Object) this.AimTarget != (UnityEngine.Object) null) || blacklist == null || !blacklist.Contains(allUnit)))
      {
        float num5 = Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.transform.position, allUnit.transform.position)) / maxValue;
        if ((double) Vector3.Dot((Vector3) Utils.DegreeToVector2(this.AimAngle), (allUnit.transform.position - this.transform.position).normalized) >= (1.0 - (double) AutoAimArc / 360.0) * (double) num5)
        {
          float num6 = Mathf.Abs(Mathf.DeltaAngle(this.AimAngle, Utils.GetAngle(this.transform.position, allUnit.transform.position)));
          float num7 = Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.transform.position);
          float num8 = 1f - Mathf.Cos((float) ((double) Mathf.Clamp01(num6 / num2) * 3.1415927410125732 / 2.0));
          if (prioritizeClosest)
            num8 += Mathf.Clamp01(num7 / num1) * 0.5f;
          float num9 = num8 / allUnit.autoAimAttractionFactor;
          if ((double) num9 < (double) num4)
          {
            this.AimTarget = allUnit;
            num4 = num9;
          }
        }
      }
    }
    return this.AimTarget;
  }

  private bool CanCastSpell() => (double) this.ArrowAttackDelay <= 0.0;

  private void CastCurrentSpell(bool autoAim = true, bool consumeAmmo = true, bool wasSpell = true)
  {
    if ((double) this.ArrowAttackDelay > 0.0)
      return;
    if (!FaithAmmo.UseAmmo((float) PlayerSpells.AmmoCost))
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/noarrows", this.gameObject);
    }
    else
    {
      if (InputManager.General.MouseInputActive)
        this.state.facingAngle = this.state.LookAngle = this.playerController.forceDir = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(this.transform.position), (Vector3) InputManager.General.GetMousePosition());
      this.aiming = false;
      this.CastSpell(DataManager.Instance.CurrentCurse, autoAim, consumeAmmo, wasSpell);
    }
  }

  private void CurseBreak()
  {
    Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 1f, 1f);
    System.Action curseBroken = PlayerSpells.CurseBroken;
    if (curseBroken != null)
      curseBroken();
    DataManager.Instance.CurrentCurse = EquipmentType.None;
    DataManager.Instance.CurrentCurseLevel = 0;
    this.SetSpell(DataManager.Instance.CurrentCurse, DataManager.Instance.CurrentCurseLevel);
  }

  public static event PlayerSpells.CastEvent OnSpellCast;

  private void CastSpell(EquipmentType curseType, bool autoAim = true, bool consumeAmmo = true, bool wasSpell = true)
  {
    if ((UnityEngine.Object) this.AimTarget == (UnityEngine.Object) null || this.AimTarget.invincible)
    {
      if (autoAim)
        this.AimAngle = InputManager.General.MouseInputActive || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis()) <= 0.20000000298023224 && (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis()) <= 0.20000000298023224 ? this.state.facingAngle : Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(InputManager.Gameplay.GetHorizontalAxis(), InputManager.Gameplay.GetVerticalAxis()));
      if (InputManager.General.GetLastActiveController().type == ControllerType.Keyboard)
        this.AimTarget = this.GetAutoAimTarget(true);
    }
    else
      this.AimAngle = Utils.GetAngle(this.transform.position, this.AimTarget.transform.position);
    AudioManager.Instance.PlayOneShot(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PerformActionSound, this.transform.position);
    CameraManager.shakeCamera(0.5f, this.state.facingAngle);
    switch (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PrimaryEquipmentType)
    {
      case EquipmentType.Tentacles:
        this.Spell_Tentacle();
        break;
      case EquipmentType.EnemyBlast:
        this.Spell_EnemyBlast();
        break;
      case EquipmentType.ProjectileAOE:
        this.Spell_ProjectileAOE();
        break;
      case EquipmentType.Fireball:
        this.Spell_Fireball(DataManager.Instance.CurrentCurse);
        break;
      case EquipmentType.MegaSlash:
        this.Spell_MegaSlash();
        break;
    }
    System.Action onCastSpell = PlayerSpells.OnCastSpell;
    if (onCastSpell != null)
      onCastSpell();
    if (wasSpell)
    {
      PlayerSpells.CastEvent onSpellCast = PlayerSpells.OnSpellCast;
      if (onSpellCast != null)
        onSpellCast(curseType);
    }
    this.ArrowAttackDelay = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Delay;
    this.state.CURRENT_STATE = StateMachine.State.Casting;
    if ((bool) (UnityEngine.Object) this.Spine && this.Spine.AnimationState != null)
      this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Casting, EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).PerformActionAnimation);
    this.castTimer = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).CastingDuration;
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this.AimTarget = (Health) null)));
  }

  private IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private bool IsPotentialAutoAimTarget(Health h, float AutoAimArc)
  {
    return h.team != this.health.team && (double) h.HP > 0.0 && !h.invincible && !h.untouchable && (h.gameObject.layer == LayerMask.NameToLayer("Units") || h.gameObject.layer == LayerMask.NameToLayer("Obstacles Player Ignore") || h.gameObject.layer == LayerMask.NameToLayer("ObstaclesAndPlayer")) && (double) Mathf.Abs(Vector3.Angle((Vector3) Utils.DegreeToVector2(this.AimAngle), (Vector3) (Vector2) (this.transform.position - h.transform.position).normalized) - 180f) < (double) AutoAimArc;
  }

  public static float GetCurseDamageMultiplier()
  {
    return (float) ((double) PlayerFleeceManager.GetCursesDamageMultiplier() + (double) TrinketManager.GetCurseDamageMultiplerIncrease() + (0.25 * (double) DataManager.Instance.PLAYER_DAMAGE_LEVEL + (double) DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL) + (double) DataManager.Instance.CurrentCurseLevel * 0.1666666716337204);
  }

  public void Spell_Fireball(EquipmentType fireballType, float directionOffset = 0.0f, bool knockback = true)
  {
    if (fireballType == EquipmentType.Fireball_Swarm)
    {
      this.StartCoroutine((IEnumerator) this.Spell_Fireball_Swarm());
    }
    else
    {
      bool flag = fireballType == EquipmentType.Fireball_Triple;
      int num1 = flag ? 2 : 1;
      int num2 = flag ? -1 : 0;
      if ((double) directionOffset != 0.0)
        this.AimAngle = Mathf.Repeat(this.AimAngle + directionOffset, 360f);
      List<Health> blacklist = new List<Health>();
      for (int index = num2; index < num1; ++index)
      {
        if (!flag)
        {
          this.AimAngle = this.state.LookAngle;
          if ((UnityEngine.Object) this.AimTarget == (UnityEngine.Object) null || InputManager.General.GetLastActiveController().type == ControllerType.Joystick)
          {
            this.AimTarget = this.GetAutoAimTarget(true, blacklist);
            if ((UnityEngine.Object) this.AimTarget != (UnityEngine.Object) null)
              this.AimAngle = Utils.GetAngle(this.transform.position, this.AimTarget.transform.position);
          }
        }
        Projectile component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(fireballType).Prefab, this.spellParent).GetComponent<Projectile>();
        float num3 = this.AimAngle + (float) (index * 15);
        component.transform.position = this.transform.position + new Vector3(0.5f * Mathf.Cos(num3 * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(num3 * ((float) Math.PI / 180f)), -0.5f);
        component.Angle = num3;
        component.team = this.health.team;
        component.Owner = this.health;
        if (EquipmentManager.GetCurseData(fireballType).EquipmentType == EquipmentType.Fireball_Charm)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", component.gameObject);
        if (EquipmentManager.GetCurseData(fireballType).EquipmentType == EquipmentType.Fireball_Charm && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.Fireball_Charm).Chance)
          component.AttackFlags = Health.AttackFlags.Charm;
        component.Damage = EquipmentManager.GetCurseData(fireballType).Damage * PlayerSpells.GetCurseDamageMultiplier();
        component.Explosive = this.playerFarming.CorrectProjectileChargeRelease();
        if (this.playerFarming.CorrectProjectileChargeRelease())
        {
          AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.gameObject);
          Explosion.CreateExplosion(this.transform.position + new Vector3(0.5f * Mathf.Cos(num3 * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(num3 * ((float) Math.PI / 180f))), Health.Team.PlayerTeam, this.health, 3f, 1f, 1f);
          GameManager.GetInstance().HitStop();
        }
        else
          AudioManager.Instance.PlayOneShot("event:/player/Curses/fireball", this.gameObject);
        if (!flag)
        {
          component.homeInOnTarget = true;
          component.SetTarget(this.AimTarget);
        }
        blacklist.Add(this.AimTarget);
        this.chargeTimer = 0.0f;
        this.playerFarming.HideProjectileChargeBars();
        if ((double) directionOffset == 0.0 & knockback)
          this.playerController.unitObject.DoKnockBack((float) (((double) this.AimAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 1f, 0.3f);
      }
      this.playerFarming.UpdateProjectileChargeBar(0.0f);
    }
  }

  protected virtual IEnumerator Spell_Fireball_Swarm()
  {
    PlayerSpells playerSpells = this;
    int num = 5;
    List<float> shootAngles = new List<float>(10);
    for (int index = 0; index < num; ++index)
      shootAngles.Add(360f / (float) num * (float) index);
    shootAngles.Shuffle<float>();
    float initAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shootAngles.Count; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/fireball", playerSpells.gameObject);
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(EquipmentType.Fireball_Swarm).Prefab, playerSpells.transform.parent).GetComponent<Projectile>();
      component.transform.localScale /= 1.5f;
      component.transform.position = playerSpells.transform.position;
      component.Angle = initAngle + shootAngles[i];
      component.team = playerSpells.health.team;
      component.Speed = UnityEngine.Random.Range(2f, 3f);
      component.turningSpeed = 4f;
      component.angleNoiseFrequency = 0.66f;
      component.angleNoiseAmplitude = 135f;
      component.LifeTime = 10f;
      component.Owner = playerSpells.health;
      component.homeInOnTarget = true;
      component.ScreenShakeMultiplier = 0.1f;
      component.Acceleration = 5f;
      component.Damage = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier();
      if ((UnityEngine.Object) playerSpells.AimTarget == (UnityEngine.Object) null)
        playerSpells.AimTarget = playerSpells.GetAutoAimTarget();
      component.SetTarget(playerSpells.AimTarget);
      yield return (object) new WaitForSeconds(0.03f);
    }
  }

  public void Spell_Tentacle(float directionOffset = 0.0f)
  {
    Tentacle.TotalDamagedEnemies.Clear();
    this.AimAngle = Mathf.Repeat(this.AimAngle + directionOffset, 360f);
    float num1 = 1.5f;
    Vector3 origin = this.transform.position + new Vector3(num1 * Mathf.Cos(this.AimAngle * ((float) Math.PI / 180f)), (float) ((double) num1 * (double) Mathf.Sin(this.AimAngle * ((float) Math.PI / 180f)) - 0.30000001192092896), 0.5f);
    int num2 = 0;
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) origin, Utils.DegreeToVector2(this.state.facingAngle), 30f, (int) this.collisionMask);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      num2 = Mathf.CeilToInt(Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.transform.position, (Vector3) raycastHit2D.point)) / 5f);
    if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).EquipmentType != EquipmentType.Tentacles_Ice)
      num2 = 1;
    int num3 = num2 * 5;
    float delay = 0.0f;
    Vector3 position1 = this.transform.position;
    for (int index = 0; index < num2; ++index)
    {
      Vector3 position2 = this.transform.position + new Vector3((float) (index * 7) * Mathf.Cos(this.AimAngle * ((float) Math.PI / 180f)), (float) (index * 7) * Mathf.Sin(this.AimAngle * ((float) Math.PI / 180f)), 0.0f);
      this.StartCoroutine((IEnumerator) this.SpawnGroundCrack(delay, position2, Mathf.Repeat(this.AimAngle, 360f)));
      delay += 0.2f;
    }
    AudioManager.Instance.PlayOneShot("event:/player/Curses/tentacles", this.gameObject);
    if (DataManager.Instance.CurrentCurse == EquipmentType.Tentacles_Ice)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/ice_curse", this.gameObject);
    int t = 0;
    while (t++ < num3)
    {
      float num4 = 0.05f * (float) t;
      Tentacle component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Prefab, this.spellParent, true).GetComponent<Tentacle>();
      component.transform.position = origin;
      float damage = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Damage;
      if (DataManager.Instance.CurrentCurse == EquipmentType.Tentacles_Necromancy)
        ProjectileGhost.SpawnGhost(component.transform.position, num4, damage + DataManager.GetWeaponDamageMultiplier(DataManager.Instance.CurrentCurseLevel));
      if (DataManager.Instance.CurrentCurse == EquipmentType.Tentacles_Ice && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.Tentacles_Ice).Chance)
        component.AttackFlags = Health.AttackFlags.Ice;
      component.Play(num4, 0.5f, damage * PlayerSpells.GetCurseDamageMultiplier(), this.health.team, false, (int) Mathf.Repeat((float) t, 7f), true);
      origin = this.transform.position + new Vector3(((float) t * 1.25f + num1) * Mathf.Cos(this.AimAngle * ((float) Math.PI / 180f)), (float) (((double) t * 1.25 + (double) num1) * (double) Mathf.Sin(this.AimAngle * ((float) Math.PI / 180f)) - 0.30000001192092896), 0.5f);
    }
    if ((double) directionOffset == 0.0)
    {
      if (EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).EquipmentType == EquipmentType.Tentacles_Circular)
        this.Spell_Tentacle(Mathf.Repeat(90f, 360f));
      else
        this.playerController.unitObject.DoKnockBack((float) (((double) this.AimAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 1f, 0.3f);
    }
    else
    {
      if ((double) directionOffset == 270.0 || EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).EquipmentType != EquipmentType.Tentacles_Circular)
        return;
      this.Spell_Tentacle(Mathf.Repeat(directionOffset + 90f, 360f));
    }
  }

  private IEnumerator SpawnGroundCrack(float delay, Vector3 position, float aimAngle)
  {
    yield return (object) new WaitForSeconds(delay);
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).SecondaryPrefab, this.spellParent);
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, aimAngle);
  }

  public void Spell_EnemyBlast()
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Prefab, this.spellParent, true);
    if (DataManager.Instance.CurrentCurse == EquipmentType.EnemyBlast_Ice)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/ice_curse", this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/blast_push", this.gameObject);
    gameObject.transform.position = this.transform.position;
    if ((bool) (UnityEngine.Object) gameObject.GetComponent<Vortex>())
      gameObject.GetComponent<Vortex>().LifeTimeMultiplier = 0.5f;
    if (DataManager.Instance.CurrentCurse == EquipmentType.EnemyBlast_DeflectsProjectiles)
      this.playerController.MakeUntouchable(3f);
    else
      this.playerController.MakeInvincible(0.66f);
  }

  public void Spell_Vortex()
  {
    UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Prefab, this.spellParent, true).GetComponent<Vortex>().transform.position = this.transform.position;
  }

  public void Spell_ProjectileAOE()
  {
    float damage = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Damage;
    GoopBomb component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Prefab, this.spellParent, true).GetComponent<GoopBomb>();
    component.DamageMultiplier = damage * PlayerSpells.GetCurseDamageMultiplier();
    component.TickDurationMultiplier = 0.75f;
    component.impactDamage = damage * PlayerSpells.GetCurseDamageMultiplier();
    component.transform.position = this.targetPosition + this.transform.position;
    if (InputManager.General.MouseInputActive)
      component.transform.position = this.GetProjectileAOETargetWithMouse();
    component.Play(this.transform.position, 0.5f);
    component.RotateBomb = true;
    AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_shot", this.gameObject);
    if (DataManager.Instance.CurrentCurse == EquipmentType.ProjectileAOE_Charm)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", this.gameObject);
    if (DataManager.Instance.CurrentCurse != EquipmentType.ProjectileAOE_GoopTrail)
      return;
    PoisonTrail poisonTrail = component.BombVisual.gameObject.AddComponent<PoisonTrail>();
    poisonTrail.PoisonPrefab = component.PoisonPrefab;
    poisonTrail.Parent = component.transform.parent;
    poisonTrail.enabled = true;
  }

  public void Spell_MegaSlash()
  {
    bool flag = this.playerFarming.CorrectProjectileChargeRelease();
    this.chargeTimer = flag ? 1f : this.chargeTimer;
    MegaSlash component1 = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Prefab, this.spellParent, true).GetComponent<MegaSlash>();
    component1.transform.position = this.transform.position;
    component1.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.AimAngle);
    this.chargeTimer = Mathf.Clamp(this.chargeTimer, 0.0f, this.megaSlashMaxChargeDuration);
    component1.Play(this.chargeTimer / this.megaSlashMaxChargeDuration);
    this.playerFarming.UpdateProjectileChargeBar(0.0f);
    if (DataManager.Instance.CurrentCurse == EquipmentType.MegaSlash_Ice)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/ice_curse", this.gameObject);
    if (DataManager.Instance.CurrentCurse == EquipmentType.MegaSlash_Charm)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", this.gameObject);
    if (flag)
    {
      MegaSlash component2 = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Prefab, this.spellParent, true).GetComponent<MegaSlash>();
      component2.transform.position = this.transform.position;
      component2.transform.eulerAngles = new Vector3(0.0f, 0.0f, Mathf.Repeat(this.AimAngle + 180f, 360f));
      this.chargeTimer = Mathf.Clamp(this.chargeTimer, 0.0f, this.megaSlashMaxChargeDuration);
      component2.Play(this.chargeTimer / this.megaSlashMaxChargeDuration);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/mega_slash", this.gameObject);
    }
    else
      AudioManager.Instance.PlayOneShot("event:/player/Curses/mega_slash_double", this.gameObject);
    this.chargeTimer = 0.0f;
  }

  private float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  private IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public delegate void CurseEvent(EquipmentType curse, int Level);

  public delegate void CastEvent(EquipmentType type);
}
