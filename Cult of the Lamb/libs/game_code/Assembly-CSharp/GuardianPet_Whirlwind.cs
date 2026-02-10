// Decompiled with JetBrains decompiler
// Type: GuardianPet_Whirlwind
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class GuardianPet_Whirlwind : GuardianPet
{
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
  public int RanDirection = 1;

  public new void Awake() => this.health = this.GetComponent<Health>();

  public override void Play()
  {
    base.Play();
    Debug.Log((object) "PLAY!".Colour(Color.yellow));
    this.timestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    this.turningSpeed += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseFrequency += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.angleNoiseAmplitude += UnityEngine.Random.Range(-0.1f, 0.1f);
    this.RanDirection = (double) UnityEngine.Random.value < 0.5 ? -1 : 1;
    if ((UnityEngine.Object) this.health == (UnityEngine.Object) null)
      this.health = this.GetComponent<Health>();
    this.health.HP = this.health.totalHP;
    this.health.enabled = true;
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.KnockBackRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), (System.Action) (() => this.StartCoroutine((IEnumerator) this.ActiveRoutine()))));
  }

  public IEnumerator KnockBackRoutine(float angle, System.Action Callback)
  {
    GuardianPet_Whirlwind guardianPetWhirlwind = this;
    guardianPetWhirlwind.DisableForces = true;
    Vector3 force = (Vector3) new Vector2(1000f * Mathf.Cos(angle), 1000f * Mathf.Sin(angle));
    guardianPetWhirlwind.rb.AddForce((Vector2) force);
    yield return (object) new WaitForSeconds(0.5f);
    guardianPetWhirlwind.DisableForces = false;
    guardianPetWhirlwind.rb.velocity = Vector2.zero;
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
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.KnockBackRoutine(Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f), (System.Action) (() => this.ReturnToController())));
  }

  public void ReturnToController()
  {
    Debug.Log((object) "ReturnToController!".Colour(Color.yellow));
    this.Return();
    this.StopAllCoroutines();
  }

  public Health GetClosestTarget() => this.GetClosestTarget(true);

  public virtual IEnumerator ActiveRoutine()
  {
    GuardianPet_Whirlwind guardianPetWhirlwind = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      Debug.Log((object) "LOOPING coroutine".Colour(Color.yellow));
      float turningSpeed = guardianPetWhirlwind.turningSpeed;
      if (!guardianPetWhirlwind.ChasingPlayer)
      {
        guardianPetWhirlwind.state.LookAngle = guardianPetWhirlwind.state.facingAngle;
        if (GameManager.RoomActive && (UnityEngine.Object) guardianPetWhirlwind.GetClosestTarget() != (UnityEngine.Object) null && (double) Vector3.Distance(guardianPetWhirlwind.transform.position, guardianPetWhirlwind.GetClosestTarget().transform.position) < (double) guardianPetWhirlwind.noticePlayerDistance)
        {
          int num = guardianPetWhirlwind.NoticedPlayer ? 1 : 0;
          guardianPetWhirlwind.maxSpeed = guardianPetWhirlwind.ChaseSpeed;
          guardianPetWhirlwind.ChasingPlayer = true;
        }
      }
      else
      {
        float num1;
        if ((UnityEngine.Object) guardianPetWhirlwind.GetClosestTarget() == (UnityEngine.Object) null || (double) Vector3.Distance(guardianPetWhirlwind.transform.position, guardianPetWhirlwind.GetClosestTarget().transform.position) > 12.0)
        {
          guardianPetWhirlwind.TargetPosition = guardianPetWhirlwind.StartingPosition.Value;
          guardianPetWhirlwind.maxSpeed = guardianPetWhirlwind.IdleSpeed;
          guardianPetWhirlwind.ChasingPlayer = false;
        }
        else
        {
          if (guardianPetWhirlwind.avoidTarget)
          {
            guardianPetWhirlwind.TargetPosition = -guardianPetWhirlwind.GetClosestTarget().transform.position;
            int num2 = 0;
            while (num2 < 10 && (double) Vector3.Magnitude(guardianPetWhirlwind.TargetPosition - guardianPetWhirlwind.transform.position) < 3.0)
            {
              num1 = Vector3.Magnitude(guardianPetWhirlwind.TargetPosition - guardianPetWhirlwind.transform.position);
              Debug.Log((object) $"Dist {num1.ToString()} {num2.ToString()}");
              ++num2;
              guardianPetWhirlwind.TargetPosition *= 3f;
            }
          }
          else
            guardianPetWhirlwind.TargetPosition = guardianPetWhirlwind.GetClosestTarget().transform.position;
          guardianPetWhirlwind.state.LookAngle = Utils.GetAngle(guardianPetWhirlwind.transform.position, guardianPetWhirlwind.GetClosestTarget().transform.position);
        }
        if (guardianPetWhirlwind.avoidTarget)
        {
          guardianPetWhirlwind.StartCoroutine((IEnumerator) guardianPetWhirlwind.FleeRoutine());
        }
        else
        {
          guardianPetWhirlwind.AttackCoolDown = num1 = guardianPetWhirlwind.AttackCoolDown - Time.deltaTime;
          if ((double) num1 < 0.0)
          {
            if (!guardianPetWhirlwind.ShouldStartCharging())
            {
              if (guardianPetWhirlwind.ShouldAttack())
                goto label_18;
            }
            else
              break;
          }
        }
      }
      guardianPetWhirlwind.Angle = Mathf.LerpAngle(guardianPetWhirlwind.Angle, Utils.GetAngle(guardianPetWhirlwind.transform.position, guardianPetWhirlwind.TargetPosition), Time.deltaTime * turningSpeed);
      if ((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null && (double) guardianPetWhirlwind.angleNoiseAmplitude > 0.0 && (double) guardianPetWhirlwind.angleNoiseFrequency > 0.0 && (double) Vector3.Distance(guardianPetWhirlwind.TargetPosition, guardianPetWhirlwind.transform.position) < (double) guardianPetWhirlwind.MaximumRange)
        guardianPetWhirlwind.Angle += (Mathf.PerlinNoise(GameManager.GetInstance().TimeSince(guardianPetWhirlwind.timestamp) * guardianPetWhirlwind.angleNoiseFrequency, 0.0f) - 0.5f) * guardianPetWhirlwind.angleNoiseAmplitude * (float) guardianPetWhirlwind.RanDirection;
      if (!guardianPetWhirlwind.useAcceleration)
        guardianPetWhirlwind.speed = guardianPetWhirlwind.maxSpeed * guardianPetWhirlwind.SpeedMultiplier;
      guardianPetWhirlwind.state.facingAngle = guardianPetWhirlwind.Angle;
      yield return (object) null;
    }
    guardianPetWhirlwind.StartCoroutine((IEnumerator) guardianPetWhirlwind.ChargingRoutine());
    yield break;
label_18:
    guardianPetWhirlwind.StartCoroutine((IEnumerator) guardianPetWhirlwind.AttackRoutine());
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

  public virtual IEnumerator AttackRoutine()
  {
    yield return (object) null;
  }

  [CompilerGenerated]
  public void \u003COnHit\u003Eb__20_0() => this.StartCoroutine((IEnumerator) this.ActiveRoutine());

  [CompilerGenerated]
  public void \u003COnDie\u003Eb__22_0() => this.ReturnToController();
}
