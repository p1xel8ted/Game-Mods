// Decompiled with JetBrains decompiler
// Type: GuardianPet_BulletHell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GuardianPet_BulletHell : GuardianPet
{
  public GameObject PetGameObject;
  [Header("Shooting")]
  public List<GuardianPet_BulletHell.BulletPattern> BulletPatterns = new List<GuardianPet_BulletHell.BulletPattern>();
  public GameObject BulletPrefab;
  public GameObject GrenadeBulletPrefab;
  public List<GuardianPet_BulletHell.BulletPattern> activeBulletPatterns = new List<GuardianPet_BulletHell.BulletPattern>();
  public int bulletPatternIndex = -1;
  public float acceleration = 2f;
  public float turningSpeed = 1f;
  public float angleNoiseAmplitude;
  public float angleNoiseFrequency;
  public float timestamp;
  public float IdleSpeed = 0.03f;
  public float ChaseSpeed = 0.1f;
  public float MaximumRange = 5f;
  [SerializeField]
  public bool useAcceleration;
  public float Angle;
  public bool ChasingPlayer;
  public bool avoidTarget;
  public float noticePlayerDistance = 5f;
  public bool NoticedPlayer;
  public Vector3? StartingPosition;
  public Vector3 TargetPosition;
  public float AttackCoolDown;
  public Vector2 AttackCoolDownDuration = new Vector2(1f, 2f);
  public int RanDirection = 1;
  public int NumberOfAttacks = 3;
  [SerializeField]
  public ColliderEvents colliderEvents;
  public int KnockBackForce = 750;
  public int AttackCycle;
  public GuardianPet_BulletHell.BulletPattern b;
  public GrenadeBullet GrenadeBullet;
  public int ShotCount;

  public override void Play()
  {
    base.Play();
    Debug.Log((object) "PLAY!".Colour(Color.yellow));
    this.timestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.PetGameObject = this.gameObject;
    this.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.RanDirection = (double) UnityEngine.Random.value < 0.5 ? -1 : 1;
    if ((UnityEngine.Object) this.health == (UnityEngine.Object) null)
      this.health = this.GetComponent<Health>();
    this.health.HP = this.health.totalHP;
    this.ShotCount = 0;
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    if ((UnityEngine.Object) this.colliderEvents != (UnityEngine.Object) null)
      this.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.activeBulletPatterns = this.GetActiveBulletPatterns();
    if (this.activeBulletPatterns.Count == 0)
      Debug.Log((object) "NO ACTIVE BULLET PATTERNS IN GUARDIAN PET");
    this.transform.DOMoveZ(-1f, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() =>
    {
      this.health.enabled = true;
      this.StartCoroutine(this.ActiveRoutine());
    })).SetDelay<TweenerCore<Vector3, Vector3, VectorOptions>>(0.5f);
    this.transform.DOMoveY(this.transform.position.y - 1f, 1f);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.StopAllCoroutines();
    this.StartCoroutine(this.KnockBackRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), (System.Action) (() =>
    {
      this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
      this.StartCoroutine(this.ActiveRoutine());
    })));
  }

  public IEnumerator KnockBackRoutine(float angle, System.Action Callback)
  {
    GuardianPet_BulletHell guardianPetBulletHell = this;
    guardianPetBulletHell.DisableForces = true;
    Vector3 force = (Vector3) new Vector2((float) guardianPetBulletHell.KnockBackForce * Mathf.Cos(angle), (float) guardianPetBulletHell.KnockBackForce * Mathf.Sin(angle));
    guardianPetBulletHell.rb.AddForce((Vector2) force);
    yield return (object) new WaitForSeconds(0.5f);
    guardianPetBulletHell.DisableForces = false;
    guardianPetBulletHell.rb.velocity = Vector2.zero;
    guardianPetBulletHell.AttackCoolDown = 0.0f;
    System.Action action = Callback;
    if (action != null)
      action();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    Debug.Log((object) "DIE!".Colour(Color.yellow));
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == Health.Team.Team2 && !component.IsCharmedEnemy && component.team != Health.Team.PlayerTeam)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (!((UnityEngine.Object) this.colliderEvents != (UnityEngine.Object) null))
      return;
    this.colliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void ReturnToController()
  {
    Debug.Log((object) "ReturnToController!".Colour(Color.yellow));
    if ((UnityEngine.Object) this.colliderEvents != (UnityEngine.Object) null)
      this.colliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.Return();
    this.StopAllCoroutines();
  }

  public Health GetClosestTarget() => this.GetClosestTarget(true);

  public virtual IEnumerator ActiveRoutine()
  {
    GuardianPet_BulletHell guardianPetBulletHell = this;
    guardianPetBulletHell.maxSpeed = guardianPetBulletHell.ChaseSpeed;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      float turningSpeed = guardianPetBulletHell.turningSpeed;
      float num1;
      if ((UnityEngine.Object) guardianPetBulletHell.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance(guardianPetBulletHell.transform.position, guardianPetBulletHell.GetClosestTarget().transform.position) > 12.0)
      {
        if (guardianPetBulletHell.StartingPosition.HasValue)
          guardianPetBulletHell.TargetPosition = guardianPetBulletHell.StartingPosition.Value;
        guardianPetBulletHell.maxSpeed = guardianPetBulletHell.IdleSpeed;
        guardianPetBulletHell.ChasingPlayer = false;
      }
      else
      {
        if (guardianPetBulletHell.avoidTarget)
        {
          guardianPetBulletHell.TargetPosition = -guardianPetBulletHell.GetClosestTarget().transform.position;
          int num2 = 0;
          while (num2 < 10 && (double) Vector3.Magnitude(guardianPetBulletHell.TargetPosition - guardianPetBulletHell.transform.position) < 3.0)
          {
            num1 = Vector3.Magnitude(guardianPetBulletHell.TargetPosition - guardianPetBulletHell.transform.position);
            Debug.Log((object) $"Dist {num1.ToString()} {num2.ToString()}");
            ++num2;
            guardianPetBulletHell.TargetPosition *= 3f;
          }
        }
        else
          guardianPetBulletHell.TargetPosition = guardianPetBulletHell.GetClosestTarget().transform.position;
        if ((UnityEngine.Object) guardianPetBulletHell.state != (UnityEngine.Object) null && (UnityEngine.Object) guardianPetBulletHell.GetClosestTarget() != (UnityEngine.Object) null)
          guardianPetBulletHell.state.LookAngle = Utils.GetAngle(guardianPetBulletHell.transform.position, guardianPetBulletHell.GetClosestTarget().transform.position);
      }
      guardianPetBulletHell.AttackCoolDown = num1 = guardianPetBulletHell.AttackCoolDown - Time.deltaTime;
      if ((double) num1 >= 0.0)
      {
        guardianPetBulletHell.Angle = Mathf.LerpAngle(guardianPetBulletHell.Angle, Utils.GetAngle(guardianPetBulletHell.transform.position, guardianPetBulletHell.TargetPosition), Time.deltaTime * turningSpeed);
        if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) guardianPetBulletHell.angleNoiseAmplitude > 0.0 && (double) guardianPetBulletHell.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(guardianPetBulletHell.TargetPosition, guardianPetBulletHell.transform.position) < (double) guardianPetBulletHell.MaximumRange)
          guardianPetBulletHell.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(guardianPetBulletHell.timestamp) * guardianPetBulletHell.angleNoiseFrequency, 0.0f) - 0.5f) * guardianPetBulletHell.angleNoiseAmplitude * (float) guardianPetBulletHell.RanDirection;
        if (!guardianPetBulletHell.useAcceleration)
          guardianPetBulletHell.speed = guardianPetBulletHell.maxSpeed * guardianPetBulletHell.SpeedMultiplier;
        guardianPetBulletHell.state.facingAngle = guardianPetBulletHell.Angle;
        yield return (object) null;
      }
      else
        break;
    }
    guardianPetBulletHell.StartShootRoutine();
  }

  public virtual IEnumerator FleeRoutine()
  {
    yield return (object) null;
  }

  public virtual bool ShouldStartCharging() => false;

  public virtual IEnumerator ChargingRoutine()
  {
    yield return (object) null;
  }

  public virtual bool ShouldAttack() => false;

  public void StartShootRoutine()
  {
    this.StopAllCoroutines();
    this.StartCoroutine(this.ShootRoutine());
  }

  public IEnumerator ShootRoutine()
  {
    GuardianPet_BulletHell guardianPetBulletHell = this;
    guardianPetBulletHell.state.CURRENT_STATE = StateMachine.State.Attacking;
    Health currentTarget = guardianPetBulletHell.GetClosestTarget();
    guardianPetBulletHell.spine.AnimationState.SetAnimation(0, "attack-projectiles", false);
    guardianPetBulletHell.spine.AnimationState.AddAnimation(0, "attack-projectiles-loop", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < 0.5)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/humanoid_large/attack", guardianPetBulletHell.transform.position);
    ++guardianPetBulletHell.bulletPatternIndex;
    if (guardianPetBulletHell.bulletPatternIndex >= guardianPetBulletHell.activeBulletPatterns.Count)
      guardianPetBulletHell.bulletPatternIndex = 0;
    guardianPetBulletHell.b = guardianPetBulletHell.activeBulletPatterns[guardianPetBulletHell.bulletPatternIndex];
    float angle = Utils.GetAngle(guardianPetBulletHell.transform.position, (UnityEngine.Object) currentTarget != (UnityEngine.Object) null ? currentTarget.transform.position : Vector3.zero) - guardianPetBulletHell.b.Arc / 2f;
    float wave = 0.0f;
    int i = -1;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    while (++i < guardianPetBulletHell.b.BulletsToShoot)
    {
      if (guardianPetBulletHell.b.UpdateAngle)
        angle = Utils.GetAngle(guardianPetBulletHell.transform.position, (UnityEngine.Object) currentTarget != (UnityEngine.Object) null ? currentTarget.transform.position : Vector3.zero) - guardianPetBulletHell.b.Arc / 2f;
      if (guardianPetBulletHell.b.IsGrenade)
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", guardianPetBulletHell.transform.position);
        guardianPetBulletHell.GrenadeBullet = ObjectPool.Spawn(guardianPetBulletHell.GrenadeBulletPrefab, guardianPetBulletHell.transform.parent, guardianPetBulletHell.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
        guardianPetBulletHell.GrenadeBullet.SetOwner(guardianPetBulletHell.gameObject);
        guardianPetBulletHell.GrenadeBullet.Play(-1f, (float) ((double) angle + (double) guardianPetBulletHell.b.WaveSize * (double) Mathf.Cos(wave += guardianPetBulletHell.b.WaveSpeed) + (double) guardianPetBulletHell.b.Arc / (double) guardianPetBulletHell.b.BulletsToShoot * (double) i), UnityEngine.Random.Range(guardianPetBulletHell.b.RandomRange.x, guardianPetBulletHell.b.RandomRange.y), UnityEngine.Random.Range(guardianPetBulletHell.b.GravSpeed - 0.5f, guardianPetBulletHell.b.GravSpeed + 0.5f));
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", guardianPetBulletHell.transform.position);
        Projectile component = ObjectPool.Spawn(guardianPetBulletHell.BulletPrefab, guardianPetBulletHell.transform.parent).GetComponent<Projectile>();
        Vector3 normalized = (guardianPetBulletHell.GetClosestTarget().transform.position - guardianPetBulletHell.transform.position).normalized;
        component.transform.position = new Vector3(guardianPetBulletHell.transform.position.x, guardianPetBulletHell.transform.position.y, -0.2f) + normalized * 0.5f;
        component.Angle = (float) ((double) angle + (double) guardianPetBulletHell.b.WaveSize * (double) Mathf.Cos(wave += guardianPetBulletHell.b.WaveSpeed) + (double) guardianPetBulletHell.b.Arc / (double) guardianPetBulletHell.b.BulletsToShoot * (double) i);
        component.team = guardianPetBulletHell.health.team;
        component.Speed = guardianPetBulletHell.b.Speed;
        component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
        component.Owner = guardianPetBulletHell.health;
      }
      time = 0.0f;
      while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < (double) guardianPetBulletHell.b.DelayBetweenShots)
        yield return (object) null;
    }
    guardianPetBulletHell.spine.AnimationState.SetAnimation(0, "pop", false);
    guardianPetBulletHell.spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * (PlayerRelic.TimeFrozen ? 0.0f : 1f)) < 1.0)
      yield return (object) null;
    guardianPetBulletHell.state.CURRENT_STATE = StateMachine.State.Idle;
    guardianPetBulletHell.AttackCoolDown = UnityEngine.Random.Range(guardianPetBulletHell.AttackCoolDownDuration.x, guardianPetBulletHell.AttackCoolDownDuration.y);
    guardianPetBulletHell.EndShootAttack();
    guardianPetBulletHell.StartCoroutine(guardianPetBulletHell.ActiveRoutine());
  }

  public void EndShootAttack()
  {
    this.health.invincible = false;
    if ((UnityEngine.Object) this.colliderEvents != (UnityEngine.Object) null)
    {
      this.colliderEvents.OnTriggerStayEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.colliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    }
    this.transform.DOMoveZ(-1f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public List<GuardianPet_BulletHell.BulletPattern> GetActiveBulletPatterns()
  {
    List<GuardianPet_BulletHell.BulletPattern> activeBulletPatterns = new List<GuardianPet_BulletHell.BulletPattern>();
    foreach (GuardianPet_BulletHell.BulletPattern bulletPattern in this.BulletPatterns)
    {
      if (bulletPattern.IsActive)
        activeBulletPatterns.Add(bulletPattern);
    }
    return activeBulletPatterns;
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__28_0()
  {
    this.health.enabled = true;
    this.StartCoroutine(this.ActiveRoutine());
  }

  [CompilerGenerated]
  public void \u003COnHit\u003Eb__29_0()
  {
    this.AttackCoolDown = UnityEngine.Random.Range(this.AttackCoolDownDuration.x, this.AttackCoolDownDuration.y);
    this.StartCoroutine(this.ActiveRoutine());
  }

  [Serializable]
  public class BulletPattern
  {
    public bool IsActive = true;
    public bool IsGrenade;
    public Vector2 RandomRange = new Vector2(3f, 5f);
    public float GravSpeed = -8f;
    public int BulletsToShoot = 10;
    public float WaveSize = 20f;
    public float WaveSpeed = 5f;
    public float Arc;
    public float DelayBetweenShots = 0.1f;
    public float Speed = 5f;
    public bool UpdateAngle;
  }
}
