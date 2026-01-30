// Decompiled with JetBrains decompiler
// Type: EnemyHole
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using FMODUnity;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyHole : UnitObject
{
  [EventRef]
  public string AttackWakeUpSFX = "event:/dlc/dungeon06/enemy/hole/attack_wakeup";
  [EventRef]
  public string AttackSlamSFX = "event:/dlc/dungeon06/enemy/hole/attack_slam";
  [EventRef]
  public string AttackProjectileStartBossSFX = "event:/dlc/dungeon06/enemy/miniboss_hole/attack_projectile";
  [EventRef]
  public string AttackProjectileSFX = "event:/enemy/shoot_magicenergy";
  [EventRef]
  public string BossAttackBurrowInSFX = "event:/dlc/dungeon06/enemy/miniboss_hole/attack_burrow_in";
  [EventRef]
  public string BossAttackBurrowLoopSFX = "event:/dlc/dungeon06/enemy/miniboss_hole/attack_burrow_loop";
  public EventInstance bossBurrowLoopSFXInstance;
  [EventRef]
  public string GetHitVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/gethit";
  [EventRef]
  public string AttackVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/attack";
  [EventRef]
  public string DeathVO = "event:/dlc/dungeon06/enemy/vocals_shared/mutated_monster_large/death";
  public static List<EnemyHole> Holes = new List<EnemyHole>();
  public GameObject BulletPrefab;
  public Transform bulletSpawnPosition;
  public ParticleSystem dustParticles;
  public SkeletonAnimation Spine;
  public SkeletonAnimation Trap;
  public GameObject BurrowTrail;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string WalkAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ShootAnimation;
  [SerializeField]
  public SimpleSpineFlash SimpleSpineFlash;
  public ColliderEvents snapColliderEvents;
  public ColliderEvents damageColliderEvents;
  public float timeBetweenTrapAnimation;
  public bool isAttacking;
  public Animator animator;
  public Tween moveTween;
  public GameObject targetIndicatorPrefab;
  public GameObject[] rockPrefabs;
  public float rockDropSpeed = 10f;
  public float rockDamageRadius = 1f;
  public float explosionScale = 2f;
  public EnemyHole.SlamAttackType slamAttackType = EnemyHole.SlamAttackType.Cycle;
  public int spreadRockCount = 12;
  public float spreadDistance = 5f;
  public float spreadCircleRadius = 2.5f;
  public GameObject[] enemysToSpawnAtHalfHealth;
  public bool lastAttackWasSlam;
  public int cycleIndex;
  public bool firstEnable = true;
  public Coroutine attackRoutineRef;
  public bool startedAfterIntro;
  public float sideSlamDistance = 3f;
  public float returnToHoleDistance = 32f;
  public float slamTimer;
  public float slamFrequency = 3f;
  public float burrowTimer;
  public float burrowFrequency = 2f;
  public bool isInterrupted;
  public Vector3 interruptedPosition;

  public override void Awake()
  {
    base.Awake();
    this.rb.constraints = RigidbodyConstraints2D.FreezeAll;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    EnemyHole.Holes.Add(this);
    this.StopAllCoroutines();
    if (this.IsBoss)
    {
      if (this.enemysToSpawnAtHalfHealth != null)
      {
        for (int index = 0; index < this.enemysToSpawnAtHalfHealth.Length; ++index)
        {
          this.enemysToSpawnAtHalfHealth[index].SetActive(false);
          this.enemysToSpawnAtHalfHealth[index].transform.SetParent(this.transform.parent.transform.parent);
        }
      }
    }
    else if (this.enemysToSpawnAtHalfHealth != null)
    {
      for (int index = 0; index < this.enemysToSpawnAtHalfHealth.Length; ++index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.enemysToSpawnAtHalfHealth[index]);
      this.enemysToSpawnAtHalfHealth = (GameObject[]) null;
    }
    this.health.OnHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.snapColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.onSnapColliderTriggerEnter);
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.onDamageColliderTriggerEnter);
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
    this.timeBetweenTrapAnimation = UnityEngine.Random.Range(5f, 15f);
    if (this.isAttacking)
    {
      this.Spine.gameObject.SetActive(true);
      this.Trap.gameObject.SetActive(false);
      this.damageColliderEvents.gameObject.SetActive(false);
      this.health.invincible = false;
      if (this.attackRoutineRef != null)
        this.StopCoroutine(this.attackRoutineRef);
      this.attackRoutineRef = this.StartCoroutine((IEnumerator) this.AttackRoutine());
    }
    else if (this.firstEnable || !this.IsBoss)
    {
      this.StopAllCoroutines();
    }
    else
    {
      this.StopAllCoroutines();
      this.Spine.gameObject.SetActive(true);
      this.Trap.gameObject.SetActive(false);
      this.damageColliderEvents.gameObject.SetActive(false);
      this.health.invincible = false;
      if (this.attackRoutineRef != null)
        this.StopCoroutine(this.attackRoutineRef);
      this.attackRoutineRef = this.StartCoroutine((IEnumerator) this.AttackRoutine());
    }
    this.firstEnable = false;
  }

  public override void Update()
  {
    base.Update();
    if (!this.isAttacking && (double) (this.timeBetweenTrapAnimation -= Time.deltaTime * this.Spine.timeScale) < 0.0)
    {
      this.timeBetweenTrapAnimation = UnityEngine.Random.Range(5f, 15f);
      this.Trap.AnimationState.SetAnimation(0, "lure", false);
      this.Trap.AnimationState.AddAnimation(0, "static", true, 0.0f);
    }
    this.burrowTimer += Time.deltaTime * this.Spine.timeScale;
    this.slamTimer += Time.deltaTime * this.Spine.timeScale;
  }

  public IEnumerator AttackPlayer()
  {
    EnemyHole enemyHole = this;
    if (enemyHole.health.Dormant)
    {
      enemyHole.health.Dormant = false;
      enemyHole.rb.constraints = RigidbodyConstraints2D.None;
      enemyHole.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
      if (RoomLockController.DoorsOpen)
        RoomLockController.CloseAll();
      if ((bool) (UnityEngine.Object) DormantEnemyChecker.Instance)
        DormantEnemyChecker.Instance.nextCheck = Time.time + 0.1f;
      Health closestTarget = enemyHole.GetClosestTarget();
      Vector2 position1 = (Vector2) enemyHole.transform.position;
      Vector2 position2 = (Vector2) closestTarget.transform.position;
      Vector2 normalized = (position2 - position1).normalized;
      double num = (double) Vector2.Distance(position1, position2);
      enemyHole.isAttacking = true;
      enemyHole.health.invincible = true;
      enemyHole.InterruptAndLockPosition();
      Tween moveTween = enemyHole.moveTween;
      if (moveTween != null)
        moveTween.Kill();
      yield return (object) enemyHole.TriggerSurpriseAttack();
      if (enemyHole.attackRoutineRef != null)
        enemyHole.StopCoroutine(enemyHole.attackRoutineRef);
      enemyHole.attackRoutineRef = enemyHole.StartCoroutine((IEnumerator) enemyHole.AttackRoutine());
    }
  }

  public IEnumerator TriggerSurpriseAttack(string customSFX = "")
  {
    EnemyHole enemyHole = this;
    enemyHole.Trap.AnimationState.SetAnimation(0, "trigger-attack", false);
    string soundPath = !string.IsNullOrEmpty(customSFX) ? customSFX : enemyHole.AttackWakeUpSFX;
    if (!string.IsNullOrEmpty(soundPath))
      AudioManager.Instance.PlayOneShot(soundPath, enemyHole.transform.position);
    MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.33f, enemyHole.Spine);
    enemyHole.Trap.gameObject.SetActive(false);
    enemyHole.Spine.gameObject.SetActive(true);
    enemyHole.Spine.AnimationState.SetAnimation(0, "chomp", false);
    enemyHole.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    enemyHole.health.invincible = true;
    enemyHole.damageColliderEvents.gameObject.SetActive(true);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.2f, enemyHole.Spine);
    CameraManager.shakeCamera(8f);
    enemyHole.damageColliderEvents.gameObject.SetActive(false);
    enemyHole.health.invincible = false;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(1f, enemyHole.Spine);
    if (enemyHole.IsBoss && !enemyHole.startedAfterIntro)
    {
      enemyHole.startedAfterIntro = true;
      enemyHole.Spine.gameObject.SetActive(true);
      enemyHole.Trap.gameObject.SetActive(false);
      enemyHole.damageColliderEvents.gameObject.SetActive(false);
      enemyHole.health.invincible = false;
      if (enemyHole.attackRoutineRef != null)
        enemyHole.StopCoroutine(enemyHole.attackRoutineRef);
      enemyHole.attackRoutineRef = enemyHole.StartCoroutine((IEnumerator) enemyHole.AttackRoutine());
    }
  }

  public IEnumerator TriggerSideSlam(Vector3 direction)
  {
    yield return (object) this.TriggerSideSlam((GameObject) null, direction);
  }

  public IEnumerator TriggerSideSlam(GameObject target)
  {
    yield return (object) this.TriggerSideSlam(target, Vector3.zero);
  }

  public IEnumerator TriggerSideSlam(GameObject target, Vector3 direction)
  {
    EnemyHole enemyHole = this;
    float meleeAttackTime = 0.3f;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null && (double) Vector2.Distance((Vector2) enemyHole.transform.position, (Vector2) target.transform.position) <= (double) enemyHole.sideSlamDistance || (UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      bool right = false;
      bool down = (double) direction.y > 0.0;
      Health health = (UnityEngine.Object) target != (UnityEngine.Object) null ? target.GetComponent<Health>() : enemyHole.GetClosestTarget();
      if ((UnityEngine.Object) health == (UnityEngine.Object) null)
        health = enemyHole.GetClosestTarget();
      if ((UnityEngine.Object) health != (UnityEngine.Object) null && (UnityEngine.Object) health.transform != (UnityEngine.Object) null)
      {
        right = (double) health.transform.position.x > (double) enemyHole.transform.position.x;
        down = (double) health.transform.position.y < (double) enemyHole.transform.position.y;
      }
      else if (direction != Vector3.zero)
        right = (double) direction.x > 0.0;
      if (right)
        enemyHole.Spine.transform.localScale = new Vector3(1f, 1f, 1f);
      else
        enemyHole.Spine.transform.localScale = new Vector3(-1f, 1f, 1f);
      if (!string.IsNullOrEmpty(enemyHole.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyHole.AttackVO, enemyHole.transform.position);
      if (!string.IsNullOrEmpty(enemyHole.AttackSlamSFX))
        AudioManager.Instance.PlayOneShot(enemyHole.AttackSlamSFX, enemyHole.transform.position);
      enemyHole.Spine.AnimationState.SetAnimation(0, "slam", false);
      enemyHole.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyHole.Spine.timeScale) < (double) meleeAttackTime)
      {
        enemyHole.SimpleSpineFlash.FlashMeWhite(time / meleeAttackTime);
        yield return (object) null;
      }
      enemyHole.SimpleSpineFlash.FlashWhite(false);
      if (enemyHole.IsBoss)
      {
        enemyHole.StartCoroutine((IEnumerator) enemyHole.TriggerTridentShockwave(right));
        if ((double) enemyHole.health.HP <= (double) enemyHole.health.totalHP / 2.0)
          enemyHole.StartCoroutine((IEnumerator) enemyHole.TriggerTridentShockwaveVertical(down));
        CameraManager.shakeCamera(6f);
        yield return (object) CoroutineStatics.WaitForScaledSeconds(0.25f, enemyHole.Spine);
      }
      else
      {
        CameraManager.shakeCamera(3f);
        MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
        Explosion.CreateExplosion(enemyHole.transform.position + (right ? Vector3.right * 1.5f : Vector3.left * 1.5f), enemyHole.health.team, enemyHole.health, 1f);
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * enemyHole.Spine.timeScale) < (double) meleeAttackTime * 2.0)
        yield return (object) null;
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.1f, enemyHole.Spine);
  }

  public IEnumerator TriggerTridentShockwave(bool right)
  {
    EnemyHole enemyHole = this;
    Vector3 vector3_1 = right ? Vector3.right : Vector3.left;
    Explosion.CreateExplosion(enemyHole.transform.position, enemyHole.health.team, enemyHole.health, enemyHole.rockDamageRadius * 0.8f, 0.0f, enemyHole.explosionScale * 0.7f);
    CameraManager.shakeCamera(1f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    Health closestTarget = enemyHole.GetClosestTarget();
    Vector3 vector3_2 = (UnityEngine.Object) closestTarget != (UnityEngine.Object) null ? closestTarget.transform.position : enemyHole.transform.position + vector3_1 * enemyHole.spreadDistance;
    Vector3 vector3_3 = vector3_2 - enemyHole.transform.position;
    Vector3 normalized1 = vector3_3.normalized;
    EnemyHole.SlamAttackType slamAttackType = enemyHole.slamAttackType;
    if (enemyHole.slamAttackType == EnemyHole.SlamAttackType.Random)
      slamAttackType = (EnemyHole.SlamAttackType) UnityEngine.Random.Range(0, 5);
    else if (enemyHole.slamAttackType == EnemyHole.SlamAttackType.Cycle)
    {
      slamAttackType = (EnemyHole.SlamAttackType) (enemyHole.cycleIndex % 5);
      ++enemyHole.cycleIndex;
    }
    switch (slamAttackType)
    {
      case EnemyHole.SlamAttackType.Circle:
        Vector3 vector3_4 = enemyHole.transform.position + new Vector3(normalized1.x, 0.0f, 0.0f) * (enemyHole.spreadDistance * 1.5f);
        vector3_4.y += (float) (((double) vector3_2.y - (double) enemyHole.transform.position.y) * 0.30000001192092896);
        float num1 = 360f / (float) enemyHole.spreadRockCount;
        for (int index = 0; index < enemyHole.spreadRockCount; ++index)
        {
          float f = (float) ((double) index * (double) num1 * (Math.PI / 180.0));
          Vector3 vector3_5 = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f) * enemyHole.spreadCircleRadius;
          Vector3 vector3_6 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          Vector3 position = vector3_4 + vector3_5 + vector3_6;
          float delay = (float) index * 0.05f;
          enemyHole.SpawnRockFall(position, delay);
        }
        yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyHole.Spine);
        break;
      case EnemyHole.SlamAttackType.CrissCross:
        Vector3 vector3_7 = enemyHole.transform.position + new Vector3(normalized1.x, 0.0f, 0.0f) * (enemyHole.spreadDistance * 1.5f);
        vector3_7.y += (float) (((double) vector3_2.y - (double) enemyHole.transform.position.y) * 0.30000001192092896);
        int num2 = enemyHole.spreadRockCount / 4;
        float num3 = enemyHole.spreadCircleRadius / (float) num2;
        for (int index = 0; index < num2; ++index)
        {
          float num4 = (float) (index + 1) * num3;
          Vector3 vector3_8 = vector3_7 + new Vector3(num4, num4, 0.0f);
          Vector3 vector3_9 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_8 + vector3_9, (float) index * 0.05f);
          Vector3 vector3_10 = vector3_7 + new Vector3(-num4, num4, 0.0f);
          Vector3 vector3_11 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_10 + vector3_11, (float) index * 0.05f);
          Vector3 vector3_12 = vector3_7 + new Vector3(num4, -num4, 0.0f);
          Vector3 vector3_13 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_12 + vector3_13, (float) index * 0.05f);
          Vector3 vector3_14 = vector3_7 + new Vector3(-num4, -num4, 0.0f);
          Vector3 vector3_15 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_14 + vector3_15, (float) index * 0.05f);
        }
        yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyHole.Spine);
        break;
      case EnemyHole.SlamAttackType.StraightCross:
        Vector3 vector3_16 = enemyHole.transform.position + new Vector3(normalized1.x, 0.0f, 0.0f) * (enemyHole.spreadDistance * 1.5f);
        vector3_16.y += (float) (((double) vector3_2.y - (double) enemyHole.transform.position.y) * 0.30000001192092896);
        int num5 = enemyHole.spreadRockCount / 4;
        float num6 = enemyHole.spreadCircleRadius / (float) num5;
        for (int index = 0; index < num5; ++index)
        {
          float num7 = (float) (index + 1) * num6;
          Vector3 vector3_17 = vector3_16 + new Vector3(num7, 0.0f, 0.0f);
          Vector3 vector3_18 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_17 + vector3_18, (float) index * 0.05f);
          Vector3 vector3_19 = vector3_16 + new Vector3(-num7, 0.0f, 0.0f);
          Vector3 vector3_20 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_19 + vector3_20, (float) index * 0.05f);
          Vector3 vector3_21 = vector3_16 + new Vector3(0.0f, num7, 0.0f);
          Vector3 vector3_22 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_21 + vector3_22, (float) index * 0.05f);
          Vector3 vector3_23 = vector3_16 + new Vector3(0.0f, -num7, 0.0f);
          Vector3 vector3_24 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
          enemyHole.SpawnRockFall(vector3_23 + vector3_24, (float) index * 0.05f);
        }
        yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyHole.Spine);
        break;
      case EnemyHole.SlamAttackType.ThreeProngs:
        int num8 = 3;
        int num9 = 5;
        float num10 = 1f;
        float num11 = 25f;
        Vector3[] vector3Array = new Vector3[3];
        float num12 = Mathf.Atan2(normalized1.y, normalized1.x) * 57.29578f;
        for (int index = 0; index < num8; ++index)
        {
          float num13 = (float) (index - 1) * num11;
          float f = (float) (((double) num12 + (double) num13) * (Math.PI / 180.0));
          vector3Array[index] = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f);
        }
        for (int index1 = 0; index1 < num8; ++index1)
        {
          for (int index2 = 1; index2 <= num9; ++index2)
          {
            Vector3 vector3_25 = enemyHole.transform.position + vector3Array[index1] * (float) ((double) index2 * (double) num10 + (double) enemyHole.spreadDistance * 0.5);
            Vector3 vector3_26 = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0.0f);
            enemyHole.SpawnRockFall(vector3_25 + vector3_26, (float) (index1 * num9 + index2) * 0.04f);
          }
        }
        yield return (object) CoroutineStatics.WaitForScaledSeconds(2f, enemyHole.Spine);
        break;
      default:
        int num14 = 3;
        float num15 = 2.5f;
        float num16 = 0.3f;
        float num17 = 0.8f;
        vector3_3 = new Vector3(vector3_1.x, 0.0f, 0.0f);
        Vector3 normalized2 = vector3_3.normalized;
        Vector3 to = new Vector3(normalized1.x, normalized1.y, 0.0f);
        float num18 = Mathf.Clamp(Vector3.SignedAngle(normalized2, to, Vector3.forward), -45f, 45f);
        float f1 = (float) (((double) Mathf.Atan2(normalized2.y, normalized2.x) * 57.295780181884766 + (double) num18) * (Math.PI / 180.0));
        Vector3 vector3_27 = new Vector3(Mathf.Cos(f1), Mathf.Sin(f1), 0.0f);
        Vector3 vector3_28 = new Vector3(-vector3_27.y, vector3_27.x, 0.0f);
        for (int index3 = 1; index3 < num14 + 1; ++index3)
        {
          Vector3 position = enemyHole.transform.position + vector3_27 * (float) ((double) index3 * (double) num15 + (double) enemyHole.spreadDistance * 0.5);
          int num19 = (3 + (index3 - 1) * 2) / 2;
          enemyHole.SpawnRockFall(position, num16 * (float) index3);
          for (int index4 = 1; index4 <= num19; ++index4)
          {
            enemyHole.SpawnRockFall(position + vector3_28 * num17 * (float) index4, num16 * (float) index3);
            enemyHole.SpawnRockFall(position - vector3_28 * num17 * (float) index4, num16 * (float) index3);
          }
        }
        yield return (object) CoroutineStatics.WaitForScaledSeconds(num16 * (float) (num14 + 1), enemyHole.Spine);
        break;
    }
  }

  public void CreateIndicator(Vector3 pos, float destroyTime)
  {
    if (!((UnityEngine.Object) this.targetIndicatorPrefab != (UnityEngine.Object) null))
      return;
    pos.z = -0.1f;
    GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.targetIndicatorPrefab, pos, Quaternion.identity);
    gameObject.SetActive(true);
    int num = (UnityEngine.Object) gameObject.GetComponent<IndicatorFlash>() != (UnityEngine.Object) null ? 1 : 0;
    UnityEngine.Object.Destroy((UnityEngine.Object) gameObject, destroyTime);
  }

  public IEnumerator TriggerTridentShockwaveVertical(bool down)
  {
    EnemyHole enemyHole = this;
    Vector3 from = down ? Vector3.down : Vector3.up;
    int num1 = 8;
    float num2 = 0.8f;
    float num3 = 0.3f;
    Health closestTarget = enemyHole.GetClosestTarget();
    if ((UnityEngine.Object) closestTarget != (UnityEngine.Object) null)
    {
      Vector3 normalized = (closestTarget.transform.position - enemyHole.transform.position).normalized;
      float num4 = Mathf.Clamp(Vector3.SignedAngle(from, normalized, Vector3.forward), -45f, 45f);
      float f = (float) (((double) Mathf.Atan2(from.y, from.x) * 57.295780181884766 + (double) num4) * (Math.PI / 180.0));
      from = new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0.0f);
    }
    Vector3 vector3 = new Vector3(-from.y, from.x, 0.0f);
    for (int index = 1; index < num1 + 1; ++index)
    {
      Vector3 position = enemyHole.transform.position + from * ((float) index * num2);
      enemyHole.SpawnRockFall(position, num3 * (float) index);
      enemyHole.SpawnRockFall(position + vector3 * 1.2f * (float) index, num3 * (float) index);
      enemyHole.SpawnRockFall(position + vector3 * 0.6f * (float) index, num3 * (float) index);
      enemyHole.SpawnRockFall(position - vector3 * 0.6f * (float) index, num3 * (float) index);
      enemyHole.SpawnRockFall(position - vector3 * 1.2f * (float) index, num3 * (float) index);
    }
    yield return (object) CoroutineStatics.WaitForScaledSeconds(num3 * (float) (num1 + 1), enemyHole.Spine);
  }

  public void SpawnRockFall(Vector3 position, float delay)
  {
    if (this.rockPrefabs == null || this.rockPrefabs.Length == 0)
      return;
    this.StartCoroutine((IEnumerator) this.DropRockRoutine(position, delay));
  }

  public IEnumerator DropRockRoutine(Vector3 targetPosition, float delay)
  {
    EnemyHole enemyHole = this;
    float seconds = delay + UnityEngine.Random.Range(-0.15f, 0.15f);
    if ((double) seconds < 0.0)
      seconds = 0.0f;
    enemyHole.CreateIndicator(targetPosition, seconds + 1.5f);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(seconds, enemyHole.Spine);
    GameObject rock = UnityEngine.Object.Instantiate<GameObject>(enemyHole.rockPrefabs[UnityEngine.Random.Range(0, enemyHole.rockPrefabs.Length)], targetPosition with
    {
      z = -10f
    }, Quaternion.identity, enemyHole.transform.parent);
    rock.SetActive(true);
    rock.transform.localScale = Vector3.zero;
    rock.transform.DOScale(Vector3.one, 0.3f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    rock.transform.DOMove(targetPosition, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InQuad).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      MMVibrate.Haptic(MMVibrate.HapticTypes.HeavyImpact);
      Explosion.CreateExplosion(targetPosition, this.health.team, this.health, this.rockDamageRadius, 0.0f, this.explosionScale);
      CameraManager.shakeCamera(1.5f);
      if (!((UnityEngine.Object) rock != (UnityEngine.Object) null))
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) rock);
    }));
  }

  public IEnumerator TriggerChaseAttack()
  {
    yield return (object) this.TriggerChaseAttack((GameObject) null);
  }

  public IEnumerator TriggerChaseAttack(GameObject target)
  {
    yield return (object) this.TriggerChaseAttack(target, Vector3.zero);
  }

  public IEnumerator TriggerChaseAttack(GameObject target, Vector3 fallbackTargetVector)
  {
    EnemyHole enemyHole = this;
    enemyHole.dustParticles.Play();
    enemyHole.Spine.AnimationState.SetAnimation(0, "burrow", false);
    if (!string.IsNullOrEmpty(enemyHole.BossAttackBurrowInSFX))
      AudioManager.Instance.PlayOneShot(enemyHole.BossAttackBurrowInSFX);
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyHole.Spine);
    enemyHole.Spine.gameObject.SetActive(false);
    enemyHole.Trap.gameObject.SetActive(true);
    enemyHole.health.invincible = true;
    enemyHole.isImmuneToKnockback = true;
    enemyHole.Trap.AnimationState.SetAnimation(0, "move", true);
    if ((UnityEngine.Object) enemyHole.SimpleSpineFlash != (UnityEngine.Object) null)
      enemyHole.SimpleSpineFlash.FlashWhite(false);
    MMVibrate.RumbleContinuous(0.5f, 0.75f);
    if (!AudioManager.Instance.IsEventInstancePlaying(enemyHole.bossBurrowLoopSFXInstance))
      enemyHole.bossBurrowLoopSFXInstance = AudioManager.Instance.CreateLoop(enemyHole.BossAttackBurrowLoopSFX, enemyHole.gameObject, true);
    Vector3 currentTargetPosition = fallbackTargetVector;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      currentTargetPosition = target.transform.position;
    float num = Vector3.Distance(currentTargetPosition, enemyHole.transform.position);
    Vector3 initialPosition = enemyHole.transform.position;
    float moveDuration = num / 10f;
    float trackTime = moveDuration / 2f;
    float trailTime = 0.0f;
    float elapsedTime = 0.0f;
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
      trackTime = 0.0f;
    while ((double) elapsedTime < (double) moveDuration)
    {
      if ((double) elapsedTime < (double) trackTime)
        currentTargetPosition = target.transform.position;
      if ((double) Vector3.Distance(currentTargetPosition, enemyHole.transform.position) > 0.20000000298023224)
      {
        enemyHole.transform.position = Vector3.Lerp(initialPosition, currentTargetPosition, elapsedTime / moveDuration);
        elapsedTime += Time.deltaTime * enemyHole.Spine.timeScale;
        trailTime -= Time.deltaTime * enemyHole.Spine.timeScale;
        if ((double) trailTime < 0.0)
        {
          trailTime = 0.025f;
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(enemyHole.BurrowTrail, enemyHole.transform.position + (Vector3) (UnityEngine.Random.insideUnitCircle * 0.4f), Quaternion.identity);
          gameObject.transform.localScale = Vector3.one * (float) (1.0 + (double) UnityEngine.Random.value * 0.33000001311302185);
          gameObject.SetActive(true);
          gameObject.transform.SetParent(enemyHole.transform.parent);
          CameraManager.shakeCamera(0.5f);
        }
        yield return (object) null;
      }
      else
      {
        enemyHole.dustParticles.Stop();
        break;
      }
    }
    enemyHole.dustParticles.Stop();
    AudioManager.Instance.StopLoop(enemyHole.bossBurrowLoopSFXInstance);
    MMVibrate.StopRumble();
    enemyHole.health.invincible = false;
    enemyHole.isImmuneToKnockback = false;
    yield return (object) enemyHole.TriggerSurpriseAttack();
  }

  public IEnumerator AttackRoutine()
  {
    EnemyHole enemyHole = this;
    Health target = (Health) null;
    while (true)
    {
      do
      {
        target = enemyHole.GetClosestTarget();
        if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
        {
          float num1 = Vector2.Distance((Vector2) enemyHole.transform.position, (Vector2) target.transform.position);
          Debug.Log((object) ("Player distaince is " + num1.ToString()));
          float num2 = enemyHole.lastAttackWasSlam ? 0.2f : 0.8f;
          if (((double) num1 > (double) enemyHole.sideSlamDistance || (double) enemyHole.slamTimer < (double) enemyHole.slamFrequency ? 0 : ((double) UnityEngine.Random.value < (double) num2 ? 1 : 0)) != 0)
          {
            enemyHole.slamTimer = 0.0f;
            enemyHole.lastAttackWasSlam = true;
            yield return (object) enemyHole.TriggerSideSlam(target.gameObject);
          }
          else if ((double) UnityEngine.Random.value < 0.33000001311302185)
          {
            enemyHole.lastAttackWasSlam = false;
            yield return (object) enemyHole.TriggerShootAttack(target.gameObject);
          }
          else if (!enemyHole.IsBoss)
          {
            enemyHole.lastAttackWasSlam = false;
            yield return (object) enemyHole.TriggerRunningAttack(target.gameObject);
          }
          else if ((double) UnityEngine.Random.value < 0.33000001311302185 && (double) enemyHole.burrowTimer >= (double) enemyHole.burrowFrequency)
          {
            enemyHole.burrowTimer = 0.0f;
            enemyHole.lastAttackWasSlam = false;
            yield return (object) enemyHole.TriggerChaseAttack(target.gameObject);
          }
          else
          {
            enemyHole.lastAttackWasSlam = true;
            if ((double) enemyHole.transform.position.x > 5.0 || (double) enemyHole.transform.position.x < -5.0)
            {
              yield return (object) enemyHole.TriggerChaseAttack((GameObject) null, new Vector3(0.0f, 0.0f, 0.0f));
              yield return (object) enemyHole.TriggerSideSlam((GameObject) null, Vector3.left);
              yield return (object) enemyHole.TriggerSideSlam((GameObject) null, Vector3.right);
            }
            else if ((double) enemyHole.transform.position.x < 0.0)
            {
              yield return (object) enemyHole.TriggerChaseAttack((GameObject) null, new Vector3(6f, 0.0f, 0.0f));
              yield return (object) enemyHole.TriggerSideSlam((GameObject) null, Vector3.right);
            }
          }
        }
        else
          goto label_17;
      }
      while ((double) enemyHole.transform.position.x <= 0.0);
      yield return (object) enemyHole.TriggerChaseAttack((GameObject) null, new Vector3(-6f, 0.0f, 0.0f));
      yield return (object) enemyHole.TriggerSideSlam((GameObject) null, Vector3.left);
    }
label_17:
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.1f, enemyHole.Spine);
    enemyHole.health.invincible = true;
    yield return (object) CoroutineStatics.WaitForScaledSeconds(0.5f, enemyHole.Spine);
    enemyHole.isAttacking = false;
    int mask = LayerMask.GetMask("Island", "Obstacles");
    int newLocationAttempts = 0;
    while (++newLocationAttempts < 20)
    {
      Vector2 normalized = UnityEngine.Random.insideUnitCircle.normalized;
      float distance = 6f;
      float num = 1f;
      float radius = 1f;
      if (!(bool) (UnityEngine.Object) Physics2D.CircleCast((Vector2) enemyHole.transform.position, radius, normalized, distance, mask).collider)
      {
        Vector3 vector3 = enemyHole.transform.position + (Vector3) (normalized * distance);
        float duration = Vector3.Distance(enemyHole.transform.position, vector3) / num;
        enemyHole.moveTween = (Tween) enemyHole.transform.DOMove(vector3, duration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).OnKill<TweenerCore<Vector3, Vector3, VectorOptions>>(new TweenCallback(enemyHole.\u003CAttackRoutine\u003Eb__64_0));
        yield return (object) enemyHole.moveTween.WaitForCompletion();
        break;
      }
    }
    Debug.Log((object) $"took {newLocationAttempts.ToString()} attempts");
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      enemyHole.health.invincible = false;
      enemyHole.attackRoutineRef = enemyHole.StartCoroutine((IEnumerator) enemyHole.AttackRoutine());
    }
  }

  public IEnumerator TriggerRunningAttack(GameObject target)
  {
    EnemyHole enemyHole = this;
    float time = 0.0f;
    float attackTime = UnityEngine.Random.Range(2f, 4f);
    enemyHole.Spine.AnimationState.SetAnimation(0, enemyHole.WalkAnimation, true);
    while (true)
    {
      time += Time.deltaTime * enemyHole.Spine.timeScale;
      attackTime -= Time.deltaTime * enemyHole.Spine.timeScale;
      if ((double) time > 0.15000000596046448)
      {
        enemyHole.givePath(target.transform.position);
        time = 0.0f;
      }
      if ((double) Vector2.Distance((Vector2) enemyHole.transform.position, (Vector2) target.transform.position) >= 2.0 && (double) attackTime > 0.0)
        yield return (object) null;
      else
        break;
    }
    enemyHole.ClearPaths();
    enemyHole.Spine.AnimationState.SetAnimation(0, "idle", true);
  }

  public IEnumerator TriggerShootAttack(GameObject target)
  {
    EnemyHole enemyHole = this;
    if ((double) target.transform.position.x < (double) enemyHole.transform.position.x)
      enemyHole.Spine.transform.localScale = new Vector3(1f, 1f, 1f);
    else
      enemyHole.Spine.transform.localScale = new Vector3(-1f, 1f, 1f);
    enemyHole.Spine.AnimationState.SetAnimation(0, enemyHole.ShootAnimation, false);
    enemyHole.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    if (enemyHole.IsBoss)
      AudioManager.Instance.PlayOneShot(enemyHole.AttackProjectileStartBossSFX, enemyHole.transform.position);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyHole.Spine.timeScale) < 0.30000001192092896)
    {
      enemyHole.SimpleSpineFlash.FlashMeWhite(time / 0.3f);
      yield return (object) null;
    }
    enemyHole.SimpleSpineFlash.FlashWhite(false);
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    float bulletCount = enemyHole.IsBoss ? 9f : 3f;
    if (!((UnityEngine.Object) target == (UnityEngine.Object) null))
    {
      Vector3 targetLastPosition = target.transform.position;
      for (int i = 0; (double) i < (double) bulletCount; ++i)
      {
        if ((UnityEngine.Object) target != (UnityEngine.Object) null)
          targetLastPosition = target.transform.position;
        if (!enemyHole.IsBoss)
          AudioManager.Instance.PlayOneShot(enemyHole.AttackProjectileSFX, enemyHole.transform.position);
        Projectile component = UnityEngine.Object.Instantiate<GameObject>(enemyHole.BulletPrefab, enemyHole.transform.parent).GetComponent<Projectile>();
        component.gameObject.SetActive(true);
        Vector3 normalized = (targetLastPosition - enemyHole.transform.position).normalized;
        component.transform.position = enemyHole.bulletSpawnPosition.position;
        component.Angle = Utils.GetAngle(enemyHole.transform.position, targetLastPosition + (Vector3) UnityEngine.Random.insideUnitCircle * 2f);
        component.team = enemyHole.health.team;
        component.Speed = (float) (((double) UnityEngine.Random.value + 2.0) * 2.0);
        component.LifeTime = 8f;
        component.Owner = enemyHole.health;
        yield return (object) CoroutineStatics.WaitForScaledSeconds(0.075f, enemyHole.Spine);
      }
    }
  }

  public void onSnapColliderTriggerEnter(Collider2D collider)
  {
    if (PlayerRelic.TimeFrozen || Health.isGlobalTimeFreeze || !(bool) (UnityEngine.Object) collider.GetComponent<PlayerFarming>() || this.isAttacking)
      return;
    this.StartCoroutine((IEnumerator) this.AttackPlayer());
  }

  public void onDamageColliderTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team != Health.Team.PlayerTeam || this.health.team != Health.Team.Team2)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.GetHitVO))
      AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
    if ((UnityEngine.Object) this.SimpleSpineFlash != (UnityEngine.Object) null)
      this.SimpleSpineFlash.FlashFillRed();
    this.DoKnockBack(Attacker, 0.25f, 0.25f);
    CameraManager.shakeCamera(2f);
    if (!PlayerRelic.TimeFrozen && !Health.isGlobalTimeFreeze && !this.isAttacking)
      this.StartCoroutine((IEnumerator) this.AttackPlayer());
    if (this.enemysToSpawnAtHalfHealth != null && (double) this.health.HP < (double) this.health.totalHP * 0.5)
    {
      for (int index = 0; index < this.enemysToSpawnAtHalfHealth.Length; ++index)
        this.enemysToSpawnAtHalfHealth[index].SetActive(true);
      this.enemysToSpawnAtHalfHealth = (GameObject[]) null;
    }
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    AudioManager.Instance.StopLoop(this.bossBurrowLoopSFXInstance);
    MMVibrate.StopRumble();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    EnemyHole.Holes.Remove(this);
    this.health.OnHit -= new Health.HitAction(((UnitObject) this).OnHit);
    this.snapColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.onSnapColliderTriggerEnter);
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.onDamageColliderTriggerEnter);
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public override void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.bossBurrowLoopSFXInstance);
    MMVibrate.StopRumble();
    base.OnDestroy();
  }

  public void RoomLockController_OnRoomCleared()
  {
    if (this.isAttacking || this.health.Dormant)
      return;
    this.isAttacking = true;
    this.Trap.AnimationState.SetAnimation(0, "lure", false);
    this.Trap.transform.DOLocalMoveZ(1.5f, 1f).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject)));
  }

  public void InterruptAndLockPosition()
  {
    this.isInterrupted = true;
    this.interruptedPosition = this.transform.position;
    this.transform.DOKill();
  }

  [CompilerGenerated]
  public void \u003CAttackRoutine\u003Eb__64_0() => this.moveTween = (Tween) null;

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__73_0()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public enum SlamAttackType
  {
    Waves,
    Circle,
    CrissCross,
    StraightCross,
    ThreeProngs,
    Random,
    Cycle,
  }
}
