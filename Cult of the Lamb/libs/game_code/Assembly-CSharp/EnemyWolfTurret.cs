// Decompiled with JetBrains decompiler
// Type: EnemyWolfTurret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyWolfTurret : UnitObject, IProjectileTrap
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public SkeletonAnimation warning;
  [SerializeField]
  public SimpleSpineFlash simpleSpineFlash;
  [SerializeField]
  public GameObject gfxRunes;
  [SerializeField]
  public SpriteRenderer aiming;
  [SerializeField]
  public float visionRange = 15f;
  [SerializeField]
  public bool lockAngleTo90Degrees;
  [SerializeField]
  public EnemyWolfTurret.TurretCustomPattern[] patterns;
  public string ActivateSFX = "event:/dlc/dungeon05/trap/wolf_turret/activate";
  public string AttackStartSFX = "event:/dlc/dungeon05/trap/wolf_turret/attack_start";
  public string DeactivateSFX = "event:/dlc/dungeon05/trap/wolf_turret/deactivate";
  public string GetHitSFX = "event:/dlc/dungeon05/trap/wolf_turret/gethit";
  public float lookAngle;
  public bool shooting;
  public GameObject targetObject;
  public Health enemyHealth;
  public int patternIndex;
  public float shootDelay;
  public float headAngle;
  public float targetHeadAngle;
  public bool canTurnHead = true;
  public float headRotationSpeed = 90f;
  public bool deactivated;
  public float targetSwitchCooldown = 1f;
  public float targetSwitchTimer;
  public bool AlternateShots;
  public int AlternateShotSide = 1;
  public GameObject headToLopOff;
  public Health bodyBreakableHealth;
  public Collider2D bodyBreakableCOllider;
  public GameObject bodyToHide;
  public bool headLoppingOff;
  public Coroutine headLoppingOffRoutine;

  public void Start()
  {
    this.aiming.DOFade(0.0f, 0.0f);
    this.spine.AnimationState.SetAnimation(0, "activate", false);
    this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    this.warning.gameObject.SetActive(false);
  }

  public new void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.shooting = false;
    this.shootDelay = Health.team2.Count > 1 ? UnityEngine.Random.Range(0.0f, 1f) : 0.0f;
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.OnRoomCleared);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    base.OnEnable();
  }

  public new void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.DeactivateWolfTurret();
    this.LopOffHead(Attacker.transform.position);
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public void LopOffHead(Vector3 attackerPosition)
  {
    if (this.headLoppingOff)
      return;
    this.headLoppingOff = true;
    if ((UnityEngine.Object) this.headToLopOff == (UnityEngine.Object) null || !this.headToLopOff.activeSelf)
      return;
    this.headToLopOff.transform.SetParent((Transform) null);
    this.headLoppingOffRoutine = this.StartCoroutine((IEnumerator) this.LopHeadRoutine(attackerPosition));
  }

  public IEnumerator LopHeadRoutine(Vector3 attackerPosition)
  {
    EnemyWolfTurret enemyWolfTurret = this;
    Vector3 velocity = new Vector3(UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(1f, 2f), UnityEngine.Random.Range(-9f, -12f));
    if ((double) enemyWolfTurret.transform.position.x < (double) attackerPosition.x)
      velocity.x *= -1f;
    List<Health> healthList = new List<Health>((IEnumerable<Health>) Health.team2);
    float num1 = 8f;
    Health health1 = (Health) null;
    for (int index = 0; index < healthList.Count; ++index)
    {
      Health health2 = healthList[index];
      float num2 = Vector3.Distance(health2.transform.position, enemyWolfTurret.transform.position);
      if ((UnityEngine.Object) health2 != (UnityEngine.Object) enemyWolfTurret.health && (double) num2 < (double) num1)
      {
        health1 = health2;
        num1 = num2;
      }
    }
    if ((UnityEngine.Object) health1 != (UnityEngine.Object) null)
    {
      Vector3 normalized1 = (health1.transform.position - enemyWolfTurret.headToLopOff.transform.position).normalized;
      Vector3 normalized2 = (enemyWolfTurret.headToLopOff.transform.position - attackerPosition).normalized;
      if ((double) Math.Abs(Vector3.Dot(normalized1, normalized2)) < 0.5)
      {
        velocity.x = Mathf.Lerp(velocity.x, normalized1.x * 2f, 0.8f);
        velocity.y = Mathf.Lerp(velocity.y, normalized1.y * 2f, 0.8f);
      }
    }
    float spinSpeed = UnityEngine.Random.Range(360f, 540f);
    if ((double) UnityEngine.Random.value < 0.5)
      spinSpeed = -spinSpeed;
    float gravity = 30f;
    float currentZ = enemyWolfTurret.headToLopOff.transform.eulerAngles.z;
    while ((UnityEngine.Object) enemyWolfTurret.headToLopOff != (UnityEngine.Object) null && (double) enemyWolfTurret.headToLopOff.transform.position.z < 0.0)
    {
      velocity.z += gravity * Time.deltaTime * enemyWolfTurret.spine.timeScale;
      enemyWolfTurret.headToLopOff.transform.position += velocity * Time.deltaTime * enemyWolfTurret.spine.timeScale;
      currentZ += spinSpeed * Time.deltaTime * enemyWolfTurret.spine.timeScale;
      enemyWolfTurret.headToLopOff.transform.eulerAngles = new Vector3(enemyWolfTurret.headToLopOff.transform.eulerAngles.x, enemyWolfTurret.headToLopOff.transform.eulerAngles.y, currentZ);
      yield return (object) null;
    }
    Vector3 position = enemyWolfTurret.headToLopOff.transform.position with
    {
      z = 0.0f
    };
    enemyWolfTurret.headToLopOff.transform.position = position;
    Explosion.CreateExplosion(position, Health.Team.PlayerTeam, enemyWolfTurret.health, 3f);
    if ((UnityEngine.Object) enemyWolfTurret.headToLopOff != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) enemyWolfTurret.headToLopOff);
    if (enemyWolfTurret.headLoppingOffRoutine != null)
    {
      enemyWolfTurret.StopCoroutine(enemyWolfTurret.headLoppingOffRoutine);
      enemyWolfTurret.headLoppingOffRoutine = (Coroutine) null;
      DOVirtual.DelayedCall(0.5f, new TweenCallback(enemyWolfTurret.\u003CLopHeadRoutine\u003Eb__38_0));
    }
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    foreach (SimpleSpineFlash componentsInChild in this.gameObject.GetComponentsInChildren<SimpleSpineFlash>(true))
      componentsInChild.FlashFillRed();
    if (string.IsNullOrEmpty(this.GetHitSFX))
      return;
    AudioManager.Instance.PlayOneShot(this.GetHitSFX, this.gameObject);
  }

  public new void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.OnRoomCleared);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void OnRoomCleared()
  {
    if ((UnityEngine.Object) this.headToLopOff != (UnityEngine.Object) null)
    {
      Explosion.CreateExplosion(this.headToLopOff.transform.position, Health.Team.PlayerTeam, this.health, 3f);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.headToLopOff);
    }
    if (this.headLoppingOffRoutine != null)
    {
      this.StopCoroutine(this.headLoppingOffRoutine);
      this.headLoppingOffRoutine = (Coroutine) null;
    }
    if (this.gameObject.activeInHierarchy)
      this.DeactivateWolfTurret();
    if (!((UnityEngine.Object) this.aiming != (UnityEngine.Object) null) || !((UnityEngine.Object) this.aiming.gameObject != (UnityEngine.Object) null))
      return;
    this.aiming.gameObject.SetActive(false);
  }

  public void DeactivateWolfTurret()
  {
    if (this.deactivated)
      return;
    Debug.Log((object) "Wolf turret deactivated");
    this.deactivated = true;
    this.StopAllCoroutines();
    this.spine.AnimationState.SetAnimation(0, "deactivate", false);
    this.gfxRunes.SetActive(false);
    if (!string.IsNullOrEmpty(this.DeactivateSFX))
      AudioManager.Instance.PlayOneShot(this.DeactivateSFX, this.transform.position);
    if ((UnityEngine.Object) this.aiming != (UnityEngine.Object) null && (UnityEngine.Object) this.aiming.gameObject != (UnityEngine.Object) null)
      this.aiming.gameObject.SetActive(false);
    if ((UnityEngine.Object) this.health != (UnityEngine.Object) null)
      this.health.HP = 0.1f;
    if (!((UnityEngine.Object) this.bodyBreakableCOllider != (UnityEngine.Object) null) || this.bodyBreakableCOllider.enabled)
      return;
    this.bodyBreakableCOllider.enabled = true;
  }

  public Health GetClosestTarget() => this.GetClosestTarget(true);

  public new void Update()
  {
    if (this.deactivated)
      return;
    this.aiming.enabled = !CheatConsole.HidingUI;
    if ((double) this.targetSwitchTimer > 0.0)
      this.targetSwitchTimer -= Time.deltaTime * this.spine.timeScale;
    if ((UnityEngine.Object) this.targetObject == (UnityEngine.Object) null)
    {
      if (Time.frameCount % 10 != 0)
        return;
      this.GetNewTarget();
    }
    else
    {
      this.UpdateHeadOrientation();
      this.ValidateTargetDistance();
      if (this.shooting || !((UnityEngine.Object) this.targetObject != (UnityEngine.Object) null))
        return;
      this.shootDelay -= Time.deltaTime * this.spine.timeScale;
      if ((double) this.shootDelay >= 0.5)
        return;
      this.StartCoroutine((IEnumerator) this.ShootArrowRoutine(this.targetObject.transform.position));
      if (!CoopManager.CoopActive || (double) this.targetSwitchTimer > 0.0 || (double) Vector3.Distance(this.targetObject.transform.position, this.transform.position) <= (double) this.visionRange * 0.33000001311302185 && (double) this.enemyHealth.HP > 0.0)
        return;
      this.GetNewTarget();
      this.targetSwitchTimer = this.targetSwitchCooldown;
    }
  }

  public void UpdateHeadOrientation()
  {
    if (!this.canTurnHead)
      return;
    this.targetHeadAngle = Utils.GetAngle(this.transform.position, this.targetObject.transform.position);
    this.headAngle = Mathf.MoveTowardsAngle(this.headAngle, this.targetHeadAngle, this.headRotationSpeed * Time.deltaTime * this.spine.timeScale);
    int num = Mathf.RoundToInt(this.headAngle / 45f);
    this.spine.skeleton.ScaleX = num <= 2 || num >= 7 ? 1f : -1f;
    string skinName = "diag-front";
    switch (num)
    {
      case 0:
      case 4:
        skinName = "side";
        break;
      case 1:
      case 3:
        skinName = "diag-back";
        break;
      case 2:
        skinName = "back";
        break;
      case 6:
        skinName = "front";
        break;
    }
    this.spine.skeleton.SetSkin(skinName);
    this.spine.skeleton.SetSlotsToSetupPose();
    this.aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, this.headAngle);
  }

  public void ValidateTargetDistance()
  {
    if ((double) Vector3.Distance(this.targetObject.transform.position, this.transform.position) <= (double) this.visionRange)
      return;
    this.targetObject = (GameObject) null;
    this.spine.AnimationState.SetAnimation(0, "deactivate", true);
  }

  public void LockAiming()
  {
    this.canTurnHead = false;
    this.aiming.DOFade(1f, 0.33f);
  }

  public void ResetAiming()
  {
    this.aiming.DOFade(0.0f, 0.33f).OnComplete<TweenerCore<Color, Color, ColorOptions>>((TweenCallback) (() => this.canTurnHead = true));
    this.spine.AnimationState.SetAnimation(0, "idle", true);
  }

  public void CyclePattern() => this.patternIndex = (this.patternIndex + 1) % this.patterns.Length;

  public IEnumerator ShootArrowRoutine(Vector3 targetLastPosition)
  {
    EnemyWolfTurret enemyWolfTurret = this;
    enemyWolfTurret.shooting = true;
    int shotsFired = 0;
    if (PlayerFleeceManager.FleecePreventsRoll())
      enemyWolfTurret.AlternateShots = true;
    enemyWolfTurret.AlternateShotSide = -enemyWolfTurret.AlternateShotSide;
    AudioManager.Instance.PlayOneShot(enemyWolfTurret.AttackStartSFX, enemyWolfTurret.transform.position);
    enemyWolfTurret.spine.AnimationState.SetAnimation(0, "anticipation", false);
    EnemyWolfTurret.TurretCustomPattern pattern = enemyWolfTurret.patterns[enemyWolfTurret.patternIndex];
    float seconds = Mathf.Max(0.0f, 0.83f - pattern.TimeBetweenShooting);
    enemyWolfTurret.aiming.DOFade(1f, 0.33f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(seconds, enemyWolfTurret.spine);
    enemyWolfTurret.LockAiming();
    while (++shotsFired <= pattern.ShotsToFire)
    {
      yield return (object) CoroutineStatics.WaitForScaledSeconds(pattern.TimeBetweenShooting, enemyWolfTurret.spine);
      AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", enemyWolfTurret.transform.position);
      enemyWolfTurret.spine.AnimationState.SetAnimation(0, "shoot", false);
      CameraManager.shakeCamera(0.2f, enemyWolfTurret.headAngle);
      enemyWolfTurret.SpawnArrowPair(pattern, shotsFired, targetLastPosition);
    }
    enemyWolfTurret.ResetAiming();
    enemyWolfTurret.CyclePattern();
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.1f, enemyWolfTurret.spine);
    enemyWolfTurret.shooting = false;
    enemyWolfTurret.shootDelay = pattern.ShootDelayTime;
  }

  public void SpawnArrowPair(
    EnemyWolfTurret.TurretCustomPattern pattern,
    int shotIndex,
    Vector3 targetPosition)
  {
    float num = pattern.rowAngle * pattern.curve.Evaluate((float) shotIndex / (float) pattern.ShotsToFire);
    float rowDistance = pattern.rowDistance;
    if (!this.AlternateShots || this.AlternateShotSide == 1)
    {
      Projectile component = ObjectPool.Spawn(pattern.projectile, this.transform.parent).GetComponent<Projectile>();
      component.transform.position = this.GetOffsetPosition(rowDistance, this.transform.position, targetPosition);
      component.Angle = !this.lockAngleTo90Degrees ? this.headAngle + num : (float) Mathf.FloorToInt(this.lookAngle / 90f) * 90f;
      component.team = Health.Team.Team2;
      component.AttackFlags = Health.AttackFlags.Trap;
      component.SetOwner(this.gameObject);
      component.SetParentSpine(this.spine);
    }
    if (this.AlternateShots && this.AlternateShotSide != -1)
      return;
    Projectile component1 = ObjectPool.Spawn(pattern.projectile, this.transform.parent).GetComponent<Projectile>();
    component1.transform.position = this.GetOffsetPosition(-rowDistance, this.transform.position, targetPosition);
    component1.Angle = !this.lockAngleTo90Degrees ? this.headAngle - num : (float) Mathf.FloorToInt(this.lookAngle / 90f) * 90f;
    component1.team = Health.Team.Team2;
    component1.AttackFlags = Health.AttackFlags.Trap;
    component1.SetOwner(this.gameObject);
    component1.SetParentSpine(this.spine);
  }

  public Vector3 GetOffsetPosition(float xOffset, Vector3 originalPosition, Vector3 targetPosition)
  {
    Vector3 vector3 = Vector3.Cross((this.transform.position - targetPosition).normalized, Vector3.forward) * xOffset;
    return originalPosition + vector3;
  }

  public void GetNewTarget()
  {
    Health closestTarget = this.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget == (UnityEngine.Object) null || (double) Vector3.Distance(closestTarget.transform.position, this.transform.position) > (double) this.visionRange)
      return;
    this.targetObject = closestTarget.gameObject;
    this.enemyHealth = closestTarget;
    this.enemyHealth.attackers.Add(this.gameObject);
    this.spine.AnimationState.SetAnimation(0, "activate", false);
    this.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    this.StartCoroutine((IEnumerator) this.ShowWarning());
    ++this.shootDelay;
  }

  public IEnumerator ShowWarning()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyWolfTurret enemyWolfTurret = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyWolfTurret.warning.gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyWolfTurret.warning.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot(enemyWolfTurret.ActivateSFX, enemyWolfTurret.transform.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) enemyWolfTurret.warning.YieldForAnimation("warn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.visionRange, Color.red);
  }

  [CompilerGenerated]
  public void \u003CLopHeadRoutine\u003Eb__38_0() => this.bodyBreakableCOllider.enabled = true;

  [CompilerGenerated]
  public void \u003CResetAiming\u003Eb__48_0() => this.canTurnHead = true;

  [Serializable]
  public class TurretCustomPattern
  {
    public AnimationCurve curve;
    public float rowDistance;
    public float rowAngle;
    public float ShootDelayTime = 1f;
    public float TimeBetweenShooting = 1f;
    public int ShotsToFire = 5;
    public GameObject projectile;
  }
}
