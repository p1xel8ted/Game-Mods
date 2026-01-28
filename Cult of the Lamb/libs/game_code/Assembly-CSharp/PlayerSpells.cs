// Decompiled with JetBrains decompiler
// Type: PlayerSpells
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using MMBiomeGeneration;
using MMRoomGeneration;
using Pathfinding;
using Rewired;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class PlayerSpells : BaseMonoBehaviour
{
  public const int AmmoCostBaseLine = 44;
  public PlayerFarming playerFarming;
  public StateMachine state;
  public Health health;
  public PlayerController playerController;
  public float castTimer;
  public SkeletonAnimation Spine;
  public float aimTimer;
  public static float minAimTime = 0.15f;
  public static float maxAimTime = 0.3f;
  [SerializeField]
  public GameObject projectileCorrectTimingParticle;
  public static System.Action CurseDamaged;
  public static System.Action CurseBroken;
  public const float projectileAOEShootDistance = 5f;
  [SerializeField]
  public LayerMask collisionMask;
  public float explosiveProjectileMaxChargeDuration = 0.75f;
  public float megaSlashMaxChargeDuration = 0.75f;
  public float chargeTimer;
  public static PlayerSpells.CurseEvent OnCurseChanged;
  public Vector3 targetPosition;
  public float acceleration;
  public bool aiming;
  public Plane plane = new Plane(Vector3.forward, Vector3.zero);
  public const float maxProjectileAOEDistance = 6f;
  public const float maxTeleportDistance = 6f;
  [SerializeField]
  public Seeker teleportSeeker;
  public float teleportAimDuration = 3f;
  public float currentTeleportAimTimer;
  public FaithAmmo faithAmmo;
  [SerializeField]
  public PlayerAmmo playerAmmo;
  public string teleportDisappearSFX = "event:/dlc/curse/teleport/down";
  public string teleportTimeoutSFX = "event:/dlc/curse/teleport/runout";
  public string teleportCountdownSFX = "event:/dlc/curse/teleport/countdown";
  public string enemyBlastIceSFX = "event:/player/Curses/ice_curse";
  public string enemyBlastFlameSFX = "event:/dlc/curse/fire/blast_push";
  public string megaSlashIceSFX = "event:/player/Curses/ice_curse";
  public string megaSlashCharmSFX = "event:/player/Curses/charm_curse";
  public string megaSlashFlameSFX = "event:/dlc/curse/fire/mega_slash_double";
  public string tentaclesFlameSFX = "event:/dlc/curse/fire/tentacles";
  public EventInstance teleportCountDownInstanceSFX;
  public float TimeScaleDelay;
  public bool ForceSpells;
  public bool cantafford;
  public float AimAngle;
  public Health AimTarget;
  public float ArrowAttackDelay;
  public bool invokedDamage;
  public static System.Action OnCastSpell;

  public int AmmoCost
  {
    get
    {
      float num = 1f;
      if ((UnityEngine.Object) this.playerFarming != (UnityEngine.Object) null)
        num = TrinketManager.GetAmmoEfficiencyMultiplier(this.playerFarming);
      return Mathf.RoundToInt(44f / num * DataManager.Instance.CurseFeverMultiplier * PlayerFleeceManager.GetCursesFeverMultiplier());
    }
  }

  public float SlowMotionSpeed
  {
    get
    {
      switch (this.playerFarming.currentCurse)
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

  public void Init()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    if ((bool) (UnityEngine.Object) this.playerFarming && (bool) (UnityEngine.Object) this.playerFarming.hudHearts)
    {
      this.faithAmmo = this.playerFarming.hudHearts.faithAmmo;
      this.playerAmmo.Initialzie();
    }
    this.state = this.GetComponent<StateMachine>();
    this.health = this.GetComponent<Health>();
    this.playerController = this.GetComponent<PlayerController>();
    this.SetSpell(this.playerFarming.currentCurse, this.playerFarming.currentCurseLevel);
  }

  public void SetSpell(EquipmentType Spell, int CurseLevel)
  {
    this.playerFarming.currentCurse = Spell;
    this.playerFarming.currentCurseLevel = CurseLevel;
    PlayerSpells.CurseEvent onCurseChanged = PlayerSpells.OnCurseChanged;
    if (onCurseChanged == null)
      return;
    onCurseChanged(this.playerFarming.currentCurse, this.playerFarming.currentCurseLevel, this.playerFarming);
  }

  public Transform spellParent
  {
    get
    {
      return !(bool) (UnityEngine.Object) GenerateRoom.Instance ? this.transform.parent : GenerateRoom.Instance.transform;
    }
  }

  public void Update()
  {
    if (!DataManager.Instance.EnabledSpells || this.playerFarming.currentCurse == EquipmentType.None)
      return;
    this.ArrowAttackDelay -= Time.deltaTime;
    if (this.playerFarming.GoToAndStopping || this.playerFarming.IsKnockedOut || (double) Time.timeScale == 0.0)
      return;
    if (InputManager.Gameplay.GetCurseButtonDown(this.playerFarming))
      this.aiming = true;
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
      case StateMachine.State.Aiming:
        if (!LocationManager.LocationIsDungeon(PlayerFarming.Location) && !this.ForceSpells || !this.aiming)
          return;
        if (this.playerFarming.canUseKeyboard && this.state.CURRENT_STATE == StateMachine.State.Aiming && InputManager.General.MouseInputActive)
          this.state.facingAngle = this.state.LookAngle = this.playerController.forceDir = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(this.transform.position), (Vector3) InputManager.General.GetMousePosition(this.playerFarming));
        if (!this.faithAmmo.CanAfford((float) this.AmmoCost))
        {
          if (InputManager.Gameplay.GetCurseButtonDown(this.playerFarming))
          {
            this.faithAmmo.UseAmmo((float) this.AmmoCost);
            AudioManager.Instance.PlayOneShot("event:/player/Curses/noarrows", this.gameObject);
          }
        }
        else if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).CanAim)
        {
          if (InputManager.Gameplay.GetCurseButtonUp(this.playerFarming))
          {
            if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType == EquipmentType.Teleport)
              AudioManager.Instance.StopOneShotInstanceEarly(this.teleportCountDownInstanceSFX, STOP_MODE.IMMEDIATE);
            if ((double) this.aimTimer < (double) PlayerSpells.minAimTime)
            {
              Health closestEnemy = this.GetClosestEnemy(10f);
              if ((UnityEngine.Object) closestEnemy != (UnityEngine.Object) null)
              {
                Debug.Log((object) "A!!");
                this.targetPosition = closestEnemy.transform.position - this.transform.position;
              }
              else
              {
                Debug.Log((object) "B!!");
                this.targetPosition = Vector3.zero;
              }
            }
            this.CastCurrentSpell();
          }
        }
        else if (InputManager.Gameplay.GetCurseButtonDown(this.playerFarming))
          this.CastCurrentSpell(false);
        if (this.state.CURRENT_STATE == StateMachine.State.Aiming)
        {
          EquipmentType primaryEquipmentType = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType;
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
        if ((UnityEngine.Object) this.faithAmmo != (UnityEngine.Object) null && this.faithAmmo.CanAfford((float) this.AmmoCost) && this.playerFarming.currentCurse != EquipmentType.None)
        {
          CurseData curseData = EquipmentManager.GetCurseData(this.playerFarming.currentCurse);
          if ((UnityEngine.Object) curseData != (UnityEngine.Object) null && curseData.CanAim && this.CanCastSpell() && InputManager.Gameplay.GetCurseButtonDown(this.playerFarming) && !LetterBox.IsPlaying && this.state.CURRENT_STATE != StateMachine.State.CustomAnimation)
          {
            this.aiming = true;
            this.state.CURRENT_STATE = StateMachine.State.Aiming;
            this.playerController.speed = 0.0f;
            break;
          }
          break;
        }
        break;
      case StateMachine.State.Casting:
        if ((double) (this.castTimer -= Time.deltaTime) <= 0.0)
        {
          if (!GameManager.GetInstance().IsPlayerInactiveInConversation())
            this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
          if ((UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) null)
          {
            this.playerFarming.interactor.CurrentInteraction.HasChanged = true;
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
    if (InputManager.Gameplay.GetCurseButtonDown(this.playerFarming))
      this.AimAngle = this.state.LookAngle;
    if ((bool) (UnityEngine.Object) this.faithAmmo && !this.faithAmmo.CanAfford((float) this.AmmoCost))
      this.cantafford = true;
    if ((bool) (UnityEngine.Object) this.faithAmmo && this.cantafford && this.faithAmmo.CanAfford((float) this.AmmoCost))
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/reload", this.transform.position);
      this.cantafford = false;
    }
    if (!((UnityEngine.Object) this.faithAmmo != (UnityEngine.Object) null))
      return;
    CurseData curseData1 = EquipmentManager.GetCurseData(this.playerFarming.currentCurse);
    if ((UnityEngine.Object) curseData1 != (UnityEngine.Object) null && curseData1.CanAim && this.CanCastSpell() && InputManager.Gameplay.GetCurseButtonHeld(this.playerFarming) && (bool) (UnityEngine.Object) this.faithAmmo && this.faithAmmo.CanAfford((float) this.AmmoCost) && !LetterBox.IsPlaying && this.state.CURRENT_STATE != StateMachine.State.Attacking && this.state.CURRENT_STATE != StateMachine.State.CustomAnimation && this.state.CURRENT_STATE != StateMachine.State.ChargingHeavyAttack && this.state.CURRENT_STATE != StateMachine.State.Meditate && this.state.CURRENT_STATE != StateMachine.State.InActive && this.state.CURRENT_STATE != StateMachine.State.HitRecover && this.state.CURRENT_STATE != StateMachine.State.HitThrown && this.aiming)
    {
      if ((double) (this.aimTimer += Time.unscaledDeltaTime) <= (double) PlayerSpells.minAimTime)
        return;
      Time.timeScale = this.SlowMotionSpeed;
      if (this.state.CURRENT_STATE == StateMachine.State.Idle || this.state.CURRENT_STATE == StateMachine.State.Moving)
      {
        this.state.CURRENT_STATE = StateMachine.State.Aiming;
        if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType == EquipmentType.MegaSlash)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/mega_slash_charge", this.gameObject);
        else if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType == EquipmentType.ProjectileAOE)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_charge", this.gameObject);
        else
          AudioManager.Instance.PlayOneShot("event:/player/Curses/start_cast", this.gameObject);
        if (!string.IsNullOrEmpty(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PerformActionAnimationLoop))
          this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Aiming, EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PerformActionAnimationLoop);
        this.targetPosition = Vector3.zero;
        this.playerController.speed = 0.0f;
      }
      this.AimAngle = InputManager.General.GetLastActiveController(this.playerFarming).type != ControllerType.Keyboard || (double) Mathf.DeltaAngle(this.AimAngle, this.state.LookAngle) >= 100.0 ? this.state.LookAngle : Mathf.LerpAngle(this.AimAngle, this.state.LookAngle, 15f * Time.unscaledTime);
      if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType == EquipmentType.ProjectileAOE)
      {
        Vector3 b = this.targetPosition + this.transform.position;
        if (this.playerFarming.canUseKeyboard && this.state.CURRENT_STATE == StateMachine.State.Aiming && InputManager.General.MouseInputActive)
          b = this.GetTargetWithMouse(6f);
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
        this.playerFarming.CurseTarget.transform.position = new Vector3(b.x, b.y, -0.1f);
        if ((double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(this.playerFarming)) > 0.10000000149011612 || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(this.playerFarming)) > 0.10000000149011612)
          this.acceleration = Mathf.Clamp01(this.acceleration + 8f * Time.unscaledDeltaTime);
        else if ((double) this.acceleration > 0.0)
          this.acceleration = Mathf.Clamp01(this.acceleration - 11f * Time.unscaledDeltaTime);
        Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.AimAngle);
        vector2.x = vector2.x * 10f * this.acceleration * SettingsManager.Settings.Game.HorizontalAimSensitivity * Time.unscaledDeltaTime;
        vector2.y = vector2.y * 10f * this.acceleration * SettingsManager.Settings.Game.VerticalAimSensitivity * Time.unscaledDeltaTime;
        this.targetPosition += vector2;
        this.targetPosition = Vector3.ClampMagnitude(this.targetPosition, 6f);
      }
      else if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType == EquipmentType.Teleport)
      {
        if ((double) this.currentTeleportAimTimer > (double) this.teleportAimDuration)
        {
          AudioManager.Instance.PlayOneShot(this.teleportTimeoutSFX, this.gameObject);
          AudioManager.Instance.StopOneShotInstanceEarly(this.teleportCountDownInstanceSFX, STOP_MODE.IMMEDIATE);
          this.playerFarming.CustomAnimationWithCallback("Downed/knockback-to-downed", false, (System.Action) (() =>
          {
            if (GameManager.GetInstance().IsPlayerInactiveInConversation())
              return;
            this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
          }));
          this.currentTeleportAimTimer = 0.0f;
          this.playerFarming.simpleSpineAnimator.FlashWhite(false);
        }
        else
        {
          if ((double) this.currentTeleportAimTimer > (double) this.teleportAimDuration * 0.699999988079071)
          {
            if (!AudioManager.Instance.IsEventInstancePlaying(this.teleportCountDownInstanceSFX))
              this.teleportCountDownInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup(this.teleportCountdownSFX, this.transform);
            this.playerFarming.simpleSpineAnimator.FlashMeWhite();
            this.playerFarming.CurseTarget.GetComponent<TargetWarning>().SetFlashTickMultiplier(0.35f);
          }
          Vector3 vector3 = this.targetPosition + this.transform.position;
          if (this.playerFarming.canUseKeyboard && this.state.CURRENT_STATE == StateMachine.State.Aiming && InputManager.General.MouseInputActive)
            vector3 = this.GetTargetWithMouse(6f);
          this.playerFarming.CurseTarget.SetActive(true);
          this.playerFarming.CurseTarget.transform.position = new Vector3(vector3.x, vector3.y, -0.1f);
          if ((double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(this.playerFarming)) > 0.10000000149011612 || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(this.playerFarming)) > 0.10000000149011612)
            this.acceleration = Mathf.Clamp01(this.acceleration + 8f * Time.unscaledDeltaTime);
          else if ((double) this.acceleration > 0.0)
            this.acceleration = Mathf.Clamp01(this.acceleration - 11f * Time.unscaledDeltaTime);
          Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.AimAngle);
          vector2.x = vector2.x * 10f * this.acceleration * SettingsManager.Settings.Game.HorizontalAimSensitivity * Time.unscaledDeltaTime;
          vector2.y = vector2.y * 10f * this.acceleration * SettingsManager.Settings.Game.VerticalAimSensitivity * Time.unscaledDeltaTime;
          this.targetPosition += vector2;
          this.targetPosition = Vector3.ClampMagnitude(this.targetPosition, 6f);
          this.currentTeleportAimTimer += Time.unscaledDeltaTime;
        }
      }
      else if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType == EquipmentType.Barrier)
      {
        Vector3 vector3 = this.targetPosition + this.transform.position;
        if (this.playerFarming.canUseKeyboard && this.state.CURRENT_STATE == StateMachine.State.Aiming && InputManager.General.MouseInputActive)
          this.GetTargetWithMouse(1.5f);
        this.playerFarming.CurseLine.SetActive(true);
        this.playerFarming.CurseLine.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.AimAngle);
        if ((double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(this.playerFarming)) > 0.10000000149011612 || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(this.playerFarming)) > 0.10000000149011612)
          this.acceleration = Mathf.Clamp01(this.acceleration + 8f * Time.unscaledDeltaTime);
        else if ((double) this.acceleration > 0.0)
          this.acceleration = Mathf.Clamp01(this.acceleration - 11f * Time.unscaledDeltaTime);
        Vector3 vector2 = (Vector3) Utils.DegreeToVector2(this.AimAngle);
        vector2.x = vector2.x * 10f * this.acceleration * SettingsManager.Settings.Game.HorizontalAimSensitivity * Time.unscaledDeltaTime;
        vector2.y = vector2.y * 10f * this.acceleration * SettingsManager.Settings.Game.VerticalAimSensitivity * Time.unscaledDeltaTime;
        this.targetPosition += vector2;
        this.targetPosition = Vector3.ClampMagnitude(this.targetPosition, 6f);
        this.currentTeleportAimTimer += Time.unscaledDeltaTime;
      }
      else
      {
        this.playerFarming.ShowProjectileChargeBars();
        float z;
        if (InputManager.General.GetLastActiveController(this.playerFarming).type == ControllerType.Keyboard)
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
        this.playerFarming.SetAimingRecticuleScaleAndRotation(1, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, Utils.Repeat(z + 180f, 360f)));
        this.playerFarming.SetAimingRecticuleScaleAndRotation(2, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, Utils.Repeat(z + 90f, 360f)));
        this.playerFarming.SetAimingRecticuleScaleAndRotation(3, new Vector3(Mathf.SmoothStep(0.0f, 1f, Mathf.Clamp01(this.aimTimer / PlayerSpells.maxAimTime)), 1f, 1f), new Vector3(0.0f, 0.0f, Utils.Repeat(z + 270f, 360f)));
      }
    }
    else
    {
      Time.timeScale = 1f;
      this.aimTimer = 0.0f;
      this.chargeTimer = 0.0f;
      this.currentTeleportAimTimer = 0.0f;
      this.playerFarming.HideProjectileChargeBars();
      this.targetPosition = Vector3.zero;
      if (this.state.CURRENT_STATE == StateMachine.State.Aiming)
      {
        if (!GameManager.GetInstance().IsPlayerInactiveInConversation())
          this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
        if ((UnityEngine.Object) this.playerFarming.interactor.CurrentInteraction != (UnityEngine.Object) null)
          this.playerFarming.interactor.CurrentInteraction.HasChanged = true;
      }
      if (this.playerFarming.CurseTarget.activeSelf)
      {
        this.playerFarming.CurseAimLine.SetPositions(new Vector3[20]);
        this.playerFarming.CurseTarget.SetActive(false);
      }
      if (this.playerFarming.CurseLine.activeSelf)
        this.playerFarming.CurseLine.SetActive(false);
      AudioManager.Instance.StopOneShotInstanceEarly(this.teleportCountDownInstanceSFX, STOP_MODE.IMMEDIATE);
    }
  }

  public void HideChargeBars()
  {
    Time.timeScale = 1f;
    this.aimTimer = 0.0f;
    this.chargeTimer = 0.0f;
    this.playerFarming.HideProjectileChargeBars();
    this.targetPosition = Vector3.zero;
    if (this.playerFarming.CurseTarget.activeSelf)
    {
      this.playerFarming.CurseAimLine.SetPositions(new Vector3[20]);
      this.playerFarming.CurseTarget.SetActive(false);
    }
    if (!this.playerFarming.CurseLine.activeSelf)
      return;
    this.playerFarming.CurseLine.SetActive(false);
  }

  public Vector3 GetTargetWithMouse(float maxDistance)
  {
    Ray ray = GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().ScreenPointToRay((Vector3) InputManager.General.GetMousePosition(this.playerFarming));
    float enter;
    this.plane.Raycast(ray, out enter);
    Vector3 vector3 = ray.GetPoint(enter) - this.transform.position;
    Vector3 normalized = vector3.normalized;
    float num = Mathf.Min(maxDistance, vector3.magnitude);
    return SettingsManager.Settings.Game.InvertAiming ? this.transform.position - normalized * num : this.transform.position + normalized * num;
  }

  public void ChargeFireball()
  {
    this.playerFarming.ShowProjectileChargeBars();
    this.playerFarming.UpdateProjectileChargeBar(this.chargeTimer / this.explosiveProjectileMaxChargeDuration);
    this.chargeTimer += Time.unscaledDeltaTime;
  }

  public IEnumerator SlowMo()
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

  public void ChargeMegaSlash()
  {
    this.playerFarming.ShowProjectileChargeBars();
    this.playerFarming.UpdateProjectileChargeBar(this.chargeTimer / this.megaSlashMaxChargeDuration);
    this.chargeTimer += Time.unscaledDeltaTime;
    if (!(this.playerFarming.Spine.AnimationState.GetCurrent(0).Animation.Name != EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PerformActionAnimationLoop))
      return;
    this.playerFarming.Spine.AnimationState.SetAnimation(0, EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PerformActionAnimationLoop, true);
  }

  public Health GetClosestEnemy(float maxDistance)
  {
    Health closestEnemy = (Health) null;
    float num = maxDistance * maxDistance;
    foreach (Health allUnit in Health.allUnits)
    {
      if (!((UnityEngine.Object) allUnit == (UnityEngine.Object) null) && allUnit.team == Health.Team.Team2 && allUnit.enabled && (double) allUnit.HP > 0.0 && !allUnit.invincible)
      {
        float sqrMagnitude = (allUnit.transform.position - this.transform.position).sqrMagnitude;
        if ((double) sqrMagnitude < (double) num)
        {
          num = sqrMagnitude;
          closestEnemy = allUnit;
        }
      }
    }
    return closestEnemy;
  }

  public Health GetAutoAimTarget(bool prioritizeClosest = false, List<Health> blacklist = null)
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

  public bool CanCastSpell()
  {
    return (double) this.ArrowAttackDelay <= 0.0 && !GameManager.GetInstance().IsPlayerInactiveInConversation();
  }

  public void CastCurrentSpell(bool autoAim = true, bool consumeAmmo = true, bool wasSpell = true)
  {
    if ((double) this.ArrowAttackDelay > 0.0)
      return;
    if (!this.faithAmmo.UseAmmo((float) this.AmmoCost))
    {
      AudioManager.Instance.PlayOneShot("event:/player/Curses/noarrows", this.gameObject);
    }
    else
    {
      if (this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
        this.state.facingAngle = this.state.LookAngle = this.playerController.forceDir = Utils.GetAngle(GameManager.GetInstance().CamFollowTarget.GetComponent<Camera>().WorldToScreenPoint(this.transform.position), (Vector3) InputManager.General.GetMousePosition(this.playerFarming));
      this.aiming = false;
      this.CastSpell(this.playerFarming.currentCurse, autoAim, consumeAmmo, wasSpell, shooter: this.gameObject);
    }
  }

  public void CurseBreak()
  {
    Explosion.CreateExplosion(this.transform.position, Health.Team.PlayerTeam, this.health, 1f, 1f);
    System.Action curseBroken = PlayerSpells.CurseBroken;
    if (curseBroken != null)
      curseBroken();
    this.playerFarming.currentCurse = EquipmentType.None;
    this.playerFarming.currentCurseLevel = 0;
    this.SetSpell(this.playerFarming.currentCurse, this.playerFarming.currentCurseLevel);
  }

  public static event PlayerSpells.CastEvent OnSpellCast;

  public void CastSpell(
    EquipmentType curseType,
    bool autoAim = true,
    bool consumeAmmo = true,
    bool wasSpell = true,
    bool smallScale = false,
    GameObject shooter = null,
    float damageMultiplier = 1f,
    bool isFromFamiliar = false)
  {
    if (smallScale)
    {
      this.AimTarget = UnitObject.GetClosestTarget(shooter.transform, Health.Team.Team2);
      if ((UnityEngine.Object) this.AimTarget != (UnityEngine.Object) null)
        this.AimAngle = Utils.GetAngle(this.transform.position, this.AimTarget.transform.position);
    }
    else if ((UnityEngine.Object) this.AimTarget == (UnityEngine.Object) null || this.AimTarget.invincible)
    {
      if (autoAim)
        this.AimAngle = this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive || (double) Mathf.Abs(InputManager.Gameplay.GetHorizontalAxis(this.playerFarming)) <= 0.20000000298023224 && (double) Mathf.Abs(InputManager.Gameplay.GetVerticalAxis(this.playerFarming)) <= 0.20000000298023224 ? this.state.facingAngle : Utils.GetAngle(this.transform.position, this.transform.position + new Vector3(InputManager.Gameplay.GetHorizontalAxis(this.playerFarming), InputManager.Gameplay.GetVerticalAxis(this.playerFarming)));
      if (InputManager.General.GetLastActiveController(this.playerFarming).type == ControllerType.Keyboard)
        this.AimTarget = this.GetAutoAimTarget(true);
    }
    else
      this.AimAngle = Utils.GetAngle(this.transform.position, this.AimTarget.transform.position);
    AudioManager.Instance.PlayOneShot(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PerformActionSound, this.transform.position);
    CameraManager.shakeCamera(0.5f, this.state.facingAngle);
    switch (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PrimaryEquipmentType)
    {
      case EquipmentType.Tentacles:
        this.Spell_Tentacle(smallScale: smallScale, shooter: shooter, damageMultiplier: damageMultiplier);
        break;
      case EquipmentType.EnemyBlast:
        this.Spell_EnemyBlast(smallScale, shooter, damageMultiplier);
        break;
      case EquipmentType.ProjectileAOE:
        this.Spell_ProjectileAOE(smallScale, shooter, damageMultiplier);
        break;
      case EquipmentType.Fireball:
        this.Spell_Fireball(this.playerFarming.currentCurse, smallScale: smallScale, shooter: shooter, damageMultiplier: damageMultiplier);
        break;
      case EquipmentType.MegaSlash:
        this.Spell_MegaSlash(smallScale, shooter, damageMultiplier);
        break;
      case EquipmentType.Teleport:
        this.Spell_Teleport(this.playerFarming.currentCurse, smallScale, shooter, damageMultiplier, isFromFamiliar);
        break;
      case EquipmentType.Barrier:
        this.Spell_Barrier(this.playerFarming.currentCurse, smallScale, shooter, damageMultiplier, this.AimAngle);
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
    int num = this.IsShooterAPlayer(shooter) ? 1 : 0;
    if (num != 0 || !smallScale)
    {
      this.ArrowAttackDelay = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Delay;
      this.state.CURRENT_STATE = StateMachine.State.Casting;
      if ((bool) (UnityEngine.Object) this.Spine && this.Spine.AnimationState != null)
        this.playerFarming.simpleSpineAnimator.ChangeStateAnimation(StateMachine.State.Casting, EquipmentManager.GetCurseData(this.playerFarming.currentCurse).PerformActionAnimation);
    }
    if (num != 0)
    {
      this.ArrowAttackDelay = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Delay;
      this.castTimer = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).CastingDuration;
    }
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() => this.AimTarget = (Health) null)));
  }

  public bool IsShooterAPlayer(GameObject shooter)
  {
    return (UnityEngine.Object) shooter == (UnityEngine.Object) null || (UnityEngine.Object) shooter == (UnityEngine.Object) this.gameObject;
  }

  public bool IsShooterAFamiliar(GameObject shooter)
  {
    return (UnityEngine.Object) shooter.GetComponent<Familiar>() != (UnityEngine.Object) null;
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public bool IsPotentialAutoAimTarget(Health h, float AutoAimArc)
  {
    return h.team != this.health.team && (double) h.HP > 0.0 && !h.invincible && !h.untouchable && (h.gameObject.layer == LayerMask.NameToLayer("Units") || h.gameObject.layer == LayerMask.NameToLayer("Obstacles Player Ignore") || h.gameObject.layer == LayerMask.NameToLayer("ObstaclesAndPlayer")) && (double) Mathf.Abs(Vector3.Angle((Vector3) Utils.DegreeToVector2(this.AimAngle), (Vector3) (Vector2) (this.transform.position - h.transform.position).normalized) - 180f) < (double) AutoAimArc;
  }

  public static float GetCurseDamageMultiplier(PlayerFarming playerFarming)
  {
    float damageMultiplier = PlayerFleeceManager.GetCursesDamageMultiplier() + TrinketManager.GetCurseDamageMultiplerIncrease(playerFarming) + DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL + (float) playerFarming.currentCurseLevel * 0.166666672f + (playerFarming.playerRelic.DoubleDamage ? 1f : 0.0f) + playerFarming.playerRelic.DamageMultiplier;
    if (playerFarming.currentWeapon == EquipmentType.Sword_Ratau)
      ++damageMultiplier;
    if (EquipmentManager.IsIceCurse(playerFarming.currentCurse))
      damageMultiplier += TrinketManager.GetIceCurseDamageMultiplier(playerFarming);
    return damageMultiplier;
  }

  public void Spell_Fireball(
    EquipmentType fireballType,
    float directionOffset = 0.0f,
    bool knockback = true,
    bool smallScale = false,
    GameObject shooter = null,
    float damageMultiplier = 1f,
    bool playSFX = true)
  {
    if (fireballType == EquipmentType.Fireball_Swarm)
    {
      this.StartCoroutine((IEnumerator) this.Spell_Fireball_Swarm(smallScale, shooter, playSFX: playSFX));
    }
    else
    {
      bool flag1 = fireballType == EquipmentType.Fireball_Triple;
      int num1 = flag1 ? 2 : 1;
      int num2 = flag1 ? -1 : 0;
      if ((double) directionOffset != 0.0)
        this.AimAngle = Utils.Repeat(this.AimAngle + directionOffset, 360f);
      List<Health> blacklist = new List<Health>();
      Vector3 fromPosition = ((UnityEngine.Object) shooter == (UnityEngine.Object) null ? this.transform.position : shooter.transform.position) with
      {
        z = 0.0f
      };
      bool flag2 = this.IsShooterAPlayer(shooter);
      for (int index = num2; index < num1; ++index)
      {
        if (!flag1)
        {
          this.AimAngle = this.state.LookAngle;
          if ((UnityEngine.Object) this.AimTarget == (UnityEngine.Object) null || InputManager.General.GetLastActiveController(this.playerFarming).type == ControllerType.Joystick)
          {
            this.AimTarget = this.GetAutoAimTarget(true, blacklist);
            if ((UnityEngine.Object) this.AimTarget != (UnityEngine.Object) null)
              this.AimAngle = Utils.GetAngle(fromPosition, this.AimTarget.transform.position);
          }
        }
        Projectile component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(fireballType).Prefab, this.spellParent).GetComponent<Projectile>();
        float num3 = this.AimAngle + (float) (index * 15);
        component.transform.position = fromPosition + new Vector3(0.5f * Mathf.Cos(num3 * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(num3 * ((float) Math.PI / 180f)), -0.5f);
        component.Angle = num3;
        component.team = this.health.team;
        component.Owner = this.health;
        component.transform.localScale *= smallScale ? 0.5f : 1f;
        if (EquipmentManager.GetCurseData(fireballType).EquipmentType == EquipmentType.Fireball_Charm & playSFX)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", component.gameObject);
        if (EquipmentManager.GetCurseData(fireballType).EquipmentType == EquipmentType.Fireball_Charm && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.Fireball_Charm).Chance)
          component.AttackFlags = Health.AttackFlags.Charm;
        component.Damage = EquipmentManager.GetCurseData(fireballType).Damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming) * damageMultiplier;
        component.Explosive = this.playerFarming.CorrectProjectileChargeRelease();
        if (smallScale)
          component.Damage *= 0.5f;
        if (this.playerFarming.CorrectProjectileChargeRelease())
        {
          if (playSFX)
            AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.gameObject);
          Explosion.CreateExplosion(fromPosition + new Vector3(0.5f * Mathf.Cos(num3 * ((float) Math.PI / 180f)), 0.5f * Mathf.Sin(num3 * ((float) Math.PI / 180f))), Health.Team.PlayerTeam, this.health, 3f, 1f, 1f);
          GameManager.GetInstance().HitStop();
        }
        else if (playSFX)
          AudioManager.Instance.PlayOneShot("event:/player/Curses/fireball", this.gameObject);
        if (!flag1)
        {
          component.homeInOnTarget = true;
          component.SetTarget(this.AimTarget);
        }
        blacklist.Add(this.AimTarget);
        if (flag2 || !smallScale)
        {
          this.chargeTimer = 0.0f;
          this.playerFarming.HideProjectileChargeBars();
          if ((double) directionOffset == 0.0 & knockback && !smallScale)
            this.playerController.unitObject.DoKnockBack((float) (((double) this.AimAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 1f, 0.3f);
        }
      }
      if (!flag2 && smallScale)
        return;
      this.playerFarming.UpdateProjectileChargeBar(0.0f);
    }
  }

  public virtual IEnumerator Spell_Fireball_Swarm(
    bool smallScale = false,
    GameObject shooter = null,
    float damageMultiplier = 1f,
    bool playSFX = true)
  {
    PlayerSpells playerSpells = this;
    int num = 5;
    List<float> shootAngles = new List<float>(10);
    for (int index = 0; index < num; ++index)
      shootAngles.Add(360f / (float) num * (float) index);
    Vector3 p = (UnityEngine.Object) shooter == (UnityEngine.Object) null ? playerSpells.transform.position : shooter.transform.position;
    shootAngles.Shuffle<float>();
    float initAngle = UnityEngine.Random.Range(0.0f, 360f);
    for (int i = 0; i < shootAngles.Count; ++i)
    {
      float f = (float) (((double) initAngle + (double) shootAngles[i]) * (Math.PI / 180.0));
      Vector3 vector3 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), p.z) * 0.3f;
      if (playSFX)
        AudioManager.Instance.PlayOneShot("event:/player/Curses/fireball", playerSpells.gameObject);
      Projectile component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(EquipmentType.Fireball_Swarm).Prefab, playerSpells.transform.parent).GetComponent<Projectile>();
      component.transform.localScale /= 1.5f;
      component.transform.position = p + vector3;
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
      component.Damage = EquipmentManager.GetCurseData(playerSpells.playerFarming.currentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(playerSpells.playerFarming) * damageMultiplier;
      component.transform.localScale *= smallScale ? 0.5f : 1f;
      component.Damage *= smallScale ? 0.5f : 1f;
      if ((UnityEngine.Object) playerSpells.AimTarget == (UnityEngine.Object) null)
        playerSpells.AimTarget = playerSpells.GetAutoAimTarget();
      component.SetTarget(playerSpells.AimTarget);
      yield return (object) new WaitForSeconds(0.03f);
    }
  }

  public void Spell_Tentacle(
    float directionOffset = 0.0f,
    bool smallScale = false,
    GameObject shooter = null,
    float damageMultiplier = 1f)
  {
    Tentacle.TotalDamagedEnemies.Clear();
    this.AimAngle = Utils.Repeat(this.AimAngle + directionOffset, 360f);
    float num1 = 1.5f;
    if (smallScale)
      num1 *= 0.5f;
    Vector3 vector3_1 = ((UnityEngine.Object) shooter == (UnityEngine.Object) null ? this.transform.position : shooter.transform.position) with
    {
      z = 0.0f
    };
    Vector3 vector3_2 = vector3_1 + new Vector3(num1 * Mathf.Cos(this.AimAngle * ((float) Math.PI / 180f)), (float) ((double) num1 * (double) Mathf.Sin(this.AimAngle * ((float) Math.PI / 180f)) - 0.30000001192092896), 0.5f);
    int num2 = 0;
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) vector3_2, Utils.DegreeToVector2(this.state.facingAngle), 30f, (int) this.collisionMask);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      num2 = Mathf.CeilToInt(Mathf.Sqrt(this.MagnitudeFindDistanceBetween(this.transform.position, (Vector3) raycastHit2D.point)) / 5f);
    if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).EquipmentType != EquipmentType.Tentacles_Ice)
      num2 = 1;
    int num3 = num2 * 5;
    float delay = 0.0f;
    float num4 = 7f;
    if (smallScale)
      num4 *= 0.5f;
    for (int index = 0; index < num2; ++index)
    {
      Vector3 position = vector3_1 + new Vector3((float) index * num4 * Mathf.Cos(this.AimAngle * ((float) Math.PI / 180f)), (float) index * num4 * Mathf.Sin(this.AimAngle * ((float) Math.PI / 180f)), 0.0f);
      this.StartCoroutine((IEnumerator) this.SpawnGroundCrack(delay, position, Utils.Repeat(this.AimAngle, 360f), smallScale));
      delay += 0.2f;
    }
    AudioManager.Instance.PlayOneShot("event:/player/Curses/tentacles", this.gameObject);
    if (this.playerFarming.currentCurse == EquipmentType.Tentacles_Ice)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/ice_curse", this.gameObject);
    float num5 = 1.25f;
    if (smallScale)
      num5 *= 0.5f;
    int v1 = 0;
    while (v1++ < num3)
    {
      float num6 = 0.05f * (float) v1;
      Tentacle component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true).GetComponent<Tentacle>();
      component.transform.position = vector3_2;
      component.SetOwner(shooter);
      float num7 = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Damage * damageMultiplier;
      if (smallScale)
        num7 *= 0.5f;
      if (this.playerFarming.currentCurse == EquipmentType.Tentacles_Necromancy)
        ProjectileGhost.SpawnGhost(component.transform.position, num6, num7 + DataManager.GetWeaponDamageMultiplier(this.playerFarming.currentCurseLevel), smallScale ? 0.5f : 1f);
      if (this.playerFarming.currentCurse == EquipmentType.Tentacles_Ice && (double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.Tentacles_Ice).Chance)
        component.AttackFlags = Health.AttackFlags.Ice;
      if (this.playerFarming.currentCurse == EquipmentType.Tentacles_Flame)
      {
        AudioManager.Instance.PlayOneShot(this.tentaclesFlameSFX, vector3_2);
        if ((double) UnityEngine.Random.value <= (double) EquipmentManager.GetCurseData(EquipmentType.Tentacles_Flame).Chance)
          component.AttackFlags = Health.AttackFlags.Burn;
      }
      component.transform.localScale *= smallScale ? 0.5f : 1f;
      component.Play(num6, 0.5f, num7 * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming), this.health.team, false, (int) Utils.Repeat((float) v1, 7f), true);
      vector3_2 = vector3_1 + new Vector3(((float) v1 * num5 + num1) * Mathf.Cos(this.AimAngle * ((float) Math.PI / 180f)), (float) (((double) v1 * (double) num5 + (double) num1) * (double) Mathf.Sin(this.AimAngle * ((float) Math.PI / 180f)) - 0.30000001192092896), smallScale ? 0.0f : 0.5f);
    }
    if ((double) directionOffset == 0.0)
    {
      if (EquipmentManager.GetCurseData(this.playerFarming.currentCurse).EquipmentType == EquipmentType.Tentacles_Circular)
      {
        this.Spell_Tentacle(Utils.Repeat(90f, 360f), smallScale, shooter, damageMultiplier);
      }
      else
      {
        if (!this.IsShooterAPlayer(shooter) || smallScale)
          return;
        this.playerController.unitObject.DoKnockBack((float) (((double) this.AimAngle + 180.0) % 360.0 * (Math.PI / 180.0)), 1f, 0.3f);
      }
    }
    else
    {
      if ((double) directionOffset == 270.0 || EquipmentManager.GetCurseData(this.playerFarming.currentCurse).EquipmentType != EquipmentType.Tentacles_Circular)
        return;
      this.Spell_Tentacle(Utils.Repeat(directionOffset + 90f, 360f), smallScale, shooter, damageMultiplier);
    }
  }

  public IEnumerator SpawnGroundCrack(
    float delay,
    Vector3 position,
    float aimAngle,
    bool smallScale = false)
  {
    yield return (object) new WaitForSeconds(delay);
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).SecondaryPrefab, this.spellParent);
    gameObject.transform.localScale *= smallScale ? 0.5f : 1f;
    gameObject.transform.position = position;
    gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, aimAngle);
  }

  public void Spell_EnemyBlast(bool smallScale = false, GameObject shooter = null, float damageMultiplier = 1f)
  {
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true);
    BlastPush component = gameObject.GetComponent<BlastPush>();
    component.SetOwner(shooter);
    component.SetDamageMultiplier(damageMultiplier);
    if (this.playerFarming.currentCurse == EquipmentType.EnemyBlast_Ice)
      AudioManager.Instance.PlayOneShot(this.enemyBlastIceSFX, this.gameObject);
    else if (this.playerFarming.currentCurse == EquipmentType.EnemyBlast_Flame)
      AudioManager.Instance.PlayOneShot(this.enemyBlastFlameSFX, this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/blast_push", this.gameObject);
    Vector3 vector3 = ((UnityEngine.Object) shooter == (UnityEngine.Object) null ? this.transform.position : shooter.transform.position) with
    {
      z = 0.0f
    };
    gameObject.transform.position = vector3;
    gameObject.transform.localScale *= smallScale ? 0.5f : 1f;
    if ((bool) (UnityEngine.Object) gameObject.GetComponent<Vortex>())
      gameObject.GetComponent<Vortex>().LifeTimeMultiplier = 0.5f;
    if (this.playerFarming.currentCurse == EquipmentType.EnemyBlast_DeflectsProjectiles)
      this.playerController.MakeUntouchable(smallScale ? 1.5f : 3f);
    else
      this.playerController.MakeInvincible(smallScale ? 0.33f : 0.66f);
  }

  public void Spell_Vortex()
  {
    UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true).GetComponent<Vortex>().transform.position = this.transform.position;
  }

  public void Spell_ProjectileAOE(bool smallScale = false, GameObject shooter = null, float damageMultiplier = 1f)
  {
    float damage = EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Damage;
    Vector3 Position = ((UnityEngine.Object) shooter == (UnityEngine.Object) null ? this.transform.position : shooter.transform.position) with
    {
      z = 0.0f
    };
    GoopBomb component = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true).GetComponent<GoopBomb>();
    component.DamageMultiplier = damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming) * damageMultiplier;
    component.TickDurationMultiplier = 0.75f;
    component.impactDamage = damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming) * damageMultiplier;
    component.transform.position = this.targetPosition + Position;
    component.transform.localScale *= smallScale ? 0.5f : 1f;
    component.SetOwner(shooter);
    if (this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      component.transform.position = this.GetTargetWithMouse(6f);
    component.Play(Position, 0.5f, Health.Team.PlayerTeam);
    component.RotateBomb = true;
    AudioManager.Instance.PlayOneShot("event:/player/Curses/goop_shot", this.gameObject);
    if (this.playerFarming.currentCurse == EquipmentType.ProjectileAOE_Charm)
      AudioManager.Instance.PlayOneShot("event:/player/Curses/charm_curse", this.gameObject);
    if (this.playerFarming.currentCurse != EquipmentType.ProjectileAOE_GoopTrail)
      return;
    PoisonTrail poisonTrail = component.BombVisual.gameObject.AddComponent<PoisonTrail>();
    poisonTrail.PoisonPrefab = component.PoisonPrefab;
    poisonTrail.enabled = true;
    poisonTrail.Parent = component.transform.parent;
  }

  public void Spell_MegaSlash(bool smallScale = false, GameObject shooter = null, float damageMultiplier = 1f)
  {
    bool flag1 = this.playerFarming.CorrectProjectileChargeRelease();
    this.chargeTimer = flag1 ? 1f : this.chargeTimer;
    Vector3 vector3 = ((UnityEngine.Object) shooter == (UnityEngine.Object) null ? this.transform.position : shooter.transform.position) with
    {
      z = 0.0f
    };
    MegaSlash component1 = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true).GetComponent<MegaSlash>();
    component1.transform.position = vector3;
    component1.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.AimAngle);
    component1.SetOwner(shooter);
    component1.SetDamageMultiplier(damageMultiplier);
    bool ignorePlayer = this.IsShooterAPlayer(shooter);
    bool flag2 = false;
    if (!ignorePlayer)
      flag2 = this.IsShooterAFamiliar(shooter);
    if (ignorePlayer)
    {
      PlayerFarming playerFarming = shooter.GetComponent<PlayerFarming>();
      if ((bool) (UnityEngine.Object) playerFarming)
      {
        playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
        playerFarming.Spine.AnimationState.SetAnimation(0, "Curses/attack-slash-curse", false);
        playerFarming.Spine.AnimationState.AddAnimation(0, "idle", false, 0.0f);
        GameManager.GetInstance().WaitForSeconds(1.5f, (System.Action) (() =>
        {
          if (GameManager.GetInstance().IsPlayerInactiveInConversation() || playerFarming.playerWeapon.DoingHeavyAttack)
            return;
          playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
        }));
      }
    }
    float num = ignorePlayer ? Mathf.Clamp(this.chargeTimer, 0.0f, this.megaSlashMaxChargeDuration) : 0.0f;
    component1.Play(num / this.megaSlashMaxChargeDuration, ignorePlayer | flag2);
    if (ignorePlayer)
    {
      this.chargeTimer = num;
      this.playerFarming.UpdateProjectileChargeBar(0.0f);
    }
    if (this.playerFarming.currentCurse == EquipmentType.MegaSlash_Ice)
      AudioManager.Instance.PlayOneShot(this.megaSlashIceSFX, this.gameObject);
    if (this.playerFarming.currentCurse == EquipmentType.MegaSlash_Charm)
      AudioManager.Instance.PlayOneShot(this.megaSlashCharmSFX, this.gameObject);
    if (this.playerFarming.currentCurse == EquipmentType.MegaSlash_Flame)
      AudioManager.Instance.PlayOneShot(this.megaSlashFlameSFX, this.gameObject);
    if (flag1)
    {
      MegaSlash component2 = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true).GetComponent<MegaSlash>();
      component2.transform.position = vector3;
      component2.transform.eulerAngles = new Vector3(0.0f, 0.0f, Utils.Repeat(this.AimAngle + 180f, 360f));
      this.chargeTimer = Mathf.Clamp(this.chargeTimer, 0.0f, this.megaSlashMaxChargeDuration);
      component2.Play(this.chargeTimer / this.megaSlashMaxChargeDuration, ignorePlayer);
      AudioManager.Instance.PlayOneShot("event:/player/Curses/mega_slash", this.gameObject);
    }
    else
      AudioManager.Instance.PlayOneShot("event:/player/Curses/mega_slash_double", this.gameObject);
    if (!ignorePlayer)
      return;
    this.chargeTimer = 0.0f;
  }

  public float MagnitudeFindDistanceBetween(Vector3 a, Vector3 b)
  {
    double num1 = (double) a.x - (double) b.x;
    float num2 = a.y - b.y;
    float num3 = a.z - b.z;
    return (float) (num1 * num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3);
  }

  public IEnumerator DelayCallback(float delay, System.Action callback)
  {
    yield return (object) new WaitForSeconds(delay);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void Spell_Teleport(
    EquipmentType currentCurse,
    bool smallScale = false,
    GameObject shooter = null,
    float damageMultiplier = 1f,
    bool isFromFamiliar = false)
  {
    if (!isFromFamiliar)
    {
      this.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
      this.state.LockStateChanges = true;
      this.playerFarming.simpleSpineAnimator.FlashWhite(false);
      this.playerFarming.Spine.AnimationState.SetAnimation(0, "spawn-out-goat", false).TimeScale = 3f;
    }
    Vector3 targetPosition = this.transform.position + this.targetPosition;
    if (isFromFamiliar && Health.team2.Count > 0)
    {
      int index = UnityEngine.Random.Range(0, Health.team2.Count);
      Debug.Log((object) ("Familiar is targeting enemy: " + Health.team2[index].name));
      targetPosition = Health.team2[index].transform.position;
      float num = Vector3.Distance(this.transform.position, targetPosition);
      if ((double) num > 6.0)
        targetPosition = Vector3.Lerp(this.transform.position, targetPosition, 6f / num);
      this.Teleport(targetPosition, currentCurse, smallScale, shooter, damageMultiplier, true);
    }
    else
    {
      Vector2 raycastDirection = (Vector2) (targetPosition - this.transform.position).normalized;
      if (this.playerFarming.canUseKeyboard && InputManager.General.MouseInputActive)
      {
        targetPosition = this.GetTargetWithMouse(6f);
        raycastDirection = (Vector2) (targetPosition - this.transform.position).normalized;
      }
      else if ((double) (targetPosition - this.transform.position).magnitude > 6.0)
        targetPosition = this.transform.position + (Vector3) raycastDirection * 6f;
      GraphNode node = AstarPath.active.GetNearest(targetPosition).node;
      if (node == null)
        return;
      this.teleportSeeker.StartPath(this.transform.position, (Vector3) node.position, (OnPathDelegate) (path =>
      {
        Vector3 targetPosition1 = targetPosition;
        if (path.path.Count > 0)
          targetPosition1 = (Vector3) path.path[path.path.Count - 1].position;
        bool flag = RoomLockController.IsPositionOutOfRoom(targetPosition);
        if (((node == null ? 1 : (path.path.Count == 0 ? 1 : 0)) | (flag ? 1 : 0)) != 0)
        {
          RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) this.transform.position, raycastDirection, 30f, (int) (LayerMask) ((int) (LayerMask) (1 << LayerMask.NameToLayer("Island")) | 1 << LayerMask.NameToLayer("Obstacles")));
          if ((bool) raycastHit2D && (double) Vector3.Distance(this.transform.position, (Vector3) raycastHit2D.point) < 6.0)
            targetPosition1 = (Vector3) (raycastHit2D.point - raycastDirection * 0.3f);
        }
        this.Teleport(targetPosition1, currentCurse, smallScale, shooter, damageMultiplier, isFromFamiliar);
      }));
    }
  }

  public void Teleport(
    Vector3 targetPosition,
    EquipmentType currentCurse,
    bool smallScale = false,
    GameObject shooter = null,
    float damageMultiplier = 1f,
    bool isFromFamiliar = false)
  {
    if (currentCurse == EquipmentType.Teleport_Exploder)
    {
      float maxInclusive = 0.5f;
      for (int index = 0; index < 3; ++index)
        Bomb.CreateBomb(this.transform.position + new Vector3(UnityEngine.Random.Range(-maxInclusive, maxInclusive), UnityEngine.Random.Range(-maxInclusive, maxInclusive), 0.0f), this.health, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent, PlayerSpells.GetCurseDamageMultiplier(this.playerFarming));
    }
    else if (currentCurse == EquipmentType.Teleport_Goop)
      TrapGoop.CreateGoop(this.transform.position, 5, 1f, this.gameObject, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
    TrapGoop.CreateGoop(this.transform.position, 3, 0.1f, this.gameObject, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
    float num1 = 0.68f;
    if (!isFromFamiliar)
    {
      this.playerFarming.playerController.MakeInvincible(num1);
      AudioManager.Instance.PlayOneShot(this.teleportDisappearSFX, this.gameObject);
    }
    GameManager.GetInstance().WaitForSeconds(num1 - 0.2f, (System.Action) (() =>
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(EquipmentManager.GetCurseData(this.playerFarming.currentCurse).Prefab, this.spellParent, true);
      BlastPush component = gameObject.GetComponent<BlastPush>();
      component.SetOwner(shooter);
      component.SetDamageMultiplier(damageMultiplier);
      gameObject.transform.position = targetPosition;
      AudioManager.Instance.PlayOneShot("event:/player/Curses/blast_push", this.gameObject);
      if (!(bool) (UnityEngine.Object) gameObject.GetComponent<Vortex>())
        return;
      gameObject.GetComponent<Vortex>().LifeTimeMultiplier = 0.5f;
    }));
    GameManager.GetInstance().WaitForSeconds(num1, (System.Action) (() =>
    {
      float seconds = 0.3f;
      float invincibleTime = 1f + seconds;
      if (!isFromFamiliar)
      {
        this.playerFarming.Spine.AnimationState.SetAnimation(0, "respawn-fast-goat", false).TimeScale = 3f;
        this.transform.position = targetPosition;
      }
      Explosion.CreateExplosion(targetPosition, this.health.team, this.health, 4f);
      if (currentCurse == EquipmentType.Teleport_Projectile)
      {
        int num2 = 8;
        for (int index = 0; index < num2; ++index)
          Projectile.CreatePlayerProjectiles(1, this.health, targetPosition, "Assets/Prefabs/Enemies/Weapons/ArrowPlayer.prefab", 16f, PlayerSpells.GetCurseDamageMultiplier(this.playerFarming), 360f / (float) num2 * (float) index);
      }
      if (currentCurse == EquipmentType.Teleport_Goop)
        TrapGoop.CreateGoop(targetPosition, 5, 1f, this.gameObject, (UnityEngine.Object) GenerateRoom.Instance != (UnityEngine.Object) null ? GenerateRoom.Instance.transform : this.transform.parent);
      GameManager.GetInstance().WaitForSeconds(seconds, (System.Action) (() =>
      {
        if (!isFromFamiliar)
          this.playerFarming.playerController.MakeUntouchable(invincibleTime);
        this.state.LockStateChanges = false;
        if (isFromFamiliar || GameManager.GetInstance().IsPlayerInactiveInConversation())
          return;
        this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
      }));
    }));
  }

  public void Spell_Barrier(
    EquipmentType currentCurse,
    bool smallScale,
    GameObject shooter,
    float damageMultiplier,
    float aimAngle,
    float moveDistance = 5f,
    float moveSpeed = 5f,
    float showHideDuration = 0.5f)
  {
    Vector3 vector2 = (Vector3) Utils.DegreeToVector2(aimAngle);
    GameObject gameObject = ObjectPool.Spawn(EquipmentManager.GetCurseData(currentCurse).Prefab, BiomeGenerator.Instance.CurrentRoom.generateRoom.transform, this.transform.position + vector2 * 1.5f, Quaternion.identity);
    gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.state.facingAngle);
    gameObject.GetComponent<BarrierAbility>().Setup(this.gameObject, EquipmentManager.GetCurseData(currentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(this.playerFarming) * damageMultiplier, moveDistance, moveSpeed, showHideDuration);
  }

  [CompilerGenerated]
  public void \u003CUpdate\u003Eb__52_0()
  {
    if (GameManager.GetInstance().IsPlayerInactiveInConversation())
      return;
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  [CompilerGenerated]
  public void \u003CCastSpell\u003Eb__72_0() => this.AimTarget = (Health) null;

  public delegate void CurseEvent(EquipmentType curse, int Level, PlayerFarming playerfarming);

  public delegate void CastEvent(EquipmentType type);
}
