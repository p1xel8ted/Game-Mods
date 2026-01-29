// Decompiled with JetBrains decompiler
// Type: EnemyBrute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMODUnity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyBrute : UnitObject
{
  public SimpleSpineAnimator simpleSpineAnimator;
  public SimpleSpineEventListener simpleSpineEventListener;
  public GameObject TargetObject;
  public float GrappleCoolDown;
  public ColliderEvents damageColliderEvents;
  public ParticleSystem ParticleSystem;
  public TargetWarning TargetWarning;
  public Projectile ProjectilePrefab;
  public LineRenderer lineRenderer;
  [EventRef]
  public string punchSoundPath = string.Empty;
  [EventRef]
  public string areaAttackSoundPath = string.Empty;
  [EventRef]
  public string swipeAttackSoundPath = string.Empty;
  [EventRef]
  public string onHitSoundPath = string.Empty;
  public float LassoDistance = 4f;
  public float LassoMaxDistance = 9f;
  public float PostAttackDuration;
  public EnemyBrute.AttackTypes AttackType;
  public int AlternateAttack;
  public GameObject RockToThrow;
  public Transform ThrowBone;
  public float TargetDistance;
  public float MaxThrowRange = 20f;

  public Vector3 TargetPosition
  {
    get
    {
      return !((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null) ? this.TargetObject.transform.position : Vector3.zero;
    }
  }

  public void Start() => this.state.facingAngle = (float) UnityEngine.Random.Range(0, 360);

  public override void OnEnable()
  {
    base.OnEnable();
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    this.TargetWarning.gameObject.SetActive(false);
    this.ParticleSystem.Stop();
    this.StartCoroutine((IEnumerator) this.WaitForEnemy());
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    if (!string.IsNullOrEmpty(this.onHitSoundPath))
      AudioManager.Instance.PlayOneShot(this.onHitSoundPath, this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.simpleSpineAnimator.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.TargetWarning.gameObject.SetActive(false);
    this.simpleSpineAnimator.FlashWhite(false);
    this.ClearTarget();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.StopAllCoroutines();
    if ((UnityEngine.Object) this.simpleSpineEventListener != (UnityEngine.Object) null)
      this.simpleSpineEventListener.OnSpineEvent -= new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "throw":
        if (this.simpleSpineAnimator.IsVisible)
          CameraManager.shakeCamera(0.4f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
        Projectile component = ObjectPool.Spawn<Projectile>(this.ProjectilePrefab, this.transform.parent).GetComponent<Projectile>();
        component.transform.position = this.transform.position;
        component.Angle = this.state.facingAngle;
        component.team = Health.Team.Team2;
        component.Owner = this.health;
        break;
      case "shake":
        if (!this.simpleSpineAnimator.IsVisible)
          break;
        CameraManager.shakeCamera(0.4f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        break;
    }
  }

  public IEnumerator WaitForEnemy()
  {
    EnemyBrute enemyBrute = this;
    while ((UnityEngine.Object) (enemyBrute.TargetObject = PlayerFarming.FindClosestPlayerGameObject(enemyBrute.transform.position)) == (UnityEngine.Object) null)
      yield return (object) null;
    enemyBrute.TargetObject.GetComponent<Health>().attackers.Add(enemyBrute.gameObject);
    enemyBrute.StartCoroutine((IEnumerator) enemyBrute.ChasePlayer());
  }

  public IEnumerator LassoPlayer()
  {
    EnemyBrute enemyBrute = this;
    enemyBrute.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyBrute.simpleSpineAnimator.Animate("quickattack-charge", 0, false);
    yield return (object) new WaitForSeconds(0.3f);
    enemyBrute.simpleSpineAnimator.Animate("quickattack-impact", 0, false);
    enemyBrute.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    enemyBrute.lineRenderer.gameObject.SetActive(true);
    float Timer = 0.0f;
    float Progress = 0.0f;
    while ((double) (Timer += Time.deltaTime) <= 0.20000000298023224)
    {
      Progress = Timer / 0.2f;
      enemyBrute.lineRenderer.SetPosition(0, enemyBrute.transform.position - Vector3.forward * 0.5f);
      enemyBrute.lineRenderer.SetPosition(1, Vector3.Lerp(enemyBrute.transform.position - Vector3.forward * 0.5f, enemyBrute.TargetPosition - Vector3.forward * 0.5f, Progress));
      yield return (object) null;
    }
    GameObject gameObject = BiomeConstants.Instance.HitFX_Blocked.Spawn();
    gameObject.transform.position = enemyBrute.TargetPosition - Vector3.forward;
    gameObject.transform.rotation = Quaternion.identity;
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(enemyBrute.transform.position, enemyBrute.TargetPosition));
    GameManager.GetInstance().HitStop();
    enemyBrute.lineRenderer.SetPosition(0, enemyBrute.transform.position - Vector3.forward * 0.5f);
    enemyBrute.lineRenderer.SetPosition(1, enemyBrute.TargetPosition - Vector3.forward);
    StateMachine PlayerState = enemyBrute.TargetObject.GetComponent<StateMachine>();
    PlayerState.CURRENT_STATE = StateMachine.State.InActive;
    yield return (object) new WaitForSeconds(0.2f);
    Progress = 0.0f;
    float ProgressSpeed = -2f;
    float Distance = Vector2.Distance((Vector2) enemyBrute.transform.position, (Vector2) enemyBrute.TargetPosition);
    while ((double) Progress <= 1.0)
    {
      Progress += ProgressSpeed * Time.deltaTime;
      if ((double) ProgressSpeed < 5.0)
        ProgressSpeed += 0.5f;
      float num = Mathf.LerpUnclamped(Distance, 2f, Progress);
      float f = Utils.GetAngle(enemyBrute.transform.position, enemyBrute.TargetPosition) * ((float) Math.PI / 180f);
      enemyBrute.TargetObject.transform.position = enemyBrute.transform.position + new Vector3(num * Mathf.Cos(f), num * Mathf.Sin(f));
      enemyBrute.lineRenderer.SetPosition(0, enemyBrute.transform.position - Vector3.forward * 0.5f);
      enemyBrute.lineRenderer.SetPosition(1, enemyBrute.TargetPosition - Vector3.forward * 0.5f);
      yield return (object) null;
    }
    PlayerState.CURRENT_STATE = StateMachine.State.Idle;
    enemyBrute.lineRenderer.gameObject.SetActive(false);
    enemyBrute.StartCoroutine((IEnumerator) enemyBrute.ChasePlayer());
    enemyBrute.AttackType = EnemyBrute.AttackTypes.Area;
    enemyBrute.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    enemyBrute.simpleSpineAnimator.Animate("attack-charge", 0, false);
    enemyBrute.StartCoroutine((IEnumerator) enemyBrute.LassoTimerCountDown());
  }

  public IEnumerator LassoTimerCountDown()
  {
    this.GrappleCoolDown = 3f;
    while ((double) (this.GrappleCoolDown -= Time.deltaTime) > 0.0)
      yield return (object) null;
  }

  public IEnumerator ThrowRock()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyBrute enemyBrute = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyBrute.state.CURRENT_STATE = StateMachine.State.Idle;
      enemyBrute.StartCoroutine((IEnumerator) enemyBrute.ChasePlayer());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyBrute.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    enemyBrute.state.facingAngle = Utils.GetAngle(enemyBrute.transform.position, enemyBrute.TargetObject.transform.position);
    enemyBrute.simpleSpineAnimator.Animate("throw", 0, false);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(3.2f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public virtual IEnumerator ChasePlayer()
  {
    EnemyBrute enemyBrute1 = this;
    enemyBrute1.givePath(enemyBrute1.TargetObject.transform.position);
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    float RepathTimer = 0.0f;
    bool Loop = true;
    while (Loop)
    {
      switch (enemyBrute1.state.CURRENT_STATE)
      {
        case StateMachine.State.Moving:
          if ((UnityEngine.Object) enemyBrute1.damageColliderEvents != (UnityEngine.Object) null)
            enemyBrute1.damageColliderEvents.SetActive(false);
          if ((double) Vector2.Distance((Vector2) enemyBrute1.transform.position, (Vector2) enemyBrute1.TargetPosition) < 3.0)
          {
            enemyBrute1.AttackType = EnemyBrute.AttackTypes.Area;
            switch (enemyBrute1.AttackType)
            {
              case EnemyBrute.AttackTypes.Area:
                Vector3 normalized = (PlayerFarming.Instance.transform.position - enemyBrute1.transform.position).normalized;
                Vector3 vector3 = enemyBrute1.transform.position + normalized * 2f;
                enemyBrute1.damageColliderEvents.transform.position = vector3;
                enemyBrute1.TargetWarning.transform.position = vector3;
                enemyBrute1.TargetWarning.gameObject.SetActive(true);
                enemyBrute1.simpleSpineAnimator.Animate("attack-charge", 0, false);
                break;
              case EnemyBrute.AttackTypes.Punch:
                enemyBrute1.simpleSpineAnimator.Animate("quickattack-charge", 0, false);
                break;
            }
            enemyBrute1.state.facingAngle = Utils.GetAngle(enemyBrute1.transform.position, enemyBrute1.TargetPosition);
            enemyBrute1.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          if ((double) (RepathTimer += Time.deltaTime) > 0.20000000298023224)
          {
            RepathTimer = 0.0f;
            double num = (double) Vector3.Distance(enemyBrute1.transform.position, enemyBrute1.TargetPosition);
            enemyBrute1.givePath(PlayerFarming.Instance.transform.position);
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          enemyBrute1.simpleSpineAnimator.FlashMeWhite();
          switch (enemyBrute1.AttackType)
          {
            case EnemyBrute.AttackTypes.Area:
              if ((double) (enemyBrute1.state.Timer += Time.deltaTime) >= 1.0)
              {
                enemyBrute1.PostAttackDuration = 1.5f;
                enemyBrute1.simpleSpineAnimator.Animate("attack-impact", 0, false);
                enemyBrute1.simpleSpineAnimator.FlashWhite(false);
                enemyBrute1.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
                CameraManager.instance.ShakeCameraForDuration(0.2f, 0.5f, 0.3f);
                if (!string.IsNullOrEmpty(enemyBrute1.areaAttackSoundPath))
                  AudioManager.Instance.PlayOneShot(enemyBrute1.areaAttackSoundPath, enemyBrute1.transform.position);
                GameManager.GetInstance().HitStop();
                GameManager.GetInstance().WaitForSeconds(0.1f, new System.Action(enemyBrute1.\u003CChasePlayer\u003Eb__35_0));
                break;
              }
              if ((UnityEngine.Object) enemyBrute1.damageColliderEvents != (UnityEngine.Object) null)
              {
                enemyBrute1.damageColliderEvents.SetActive(false);
                break;
              }
              break;
            case EnemyBrute.AttackTypes.Punch:
              if ((double) (enemyBrute1.state.Timer += Time.deltaTime) >= 0.5)
              {
                EnemyBrute enemyBrute = enemyBrute1;
                enemyBrute1.PostAttackDuration = 1f;
                enemyBrute1.simpleSpineAnimator.Animate("quickattack-impact", 0, false);
                enemyBrute1.simpleSpineAnimator.FlashWhite(false);
                enemyBrute1.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
                if (!string.IsNullOrEmpty(enemyBrute1.punchSoundPath))
                  AudioManager.Instance.PlayOneShot(enemyBrute1.punchSoundPath, enemyBrute1.transform.position);
                float AttackRange = 2f;
                Addressables_wrapper.InstantiateAsync((object) "Assets/Prefabs/Enemies/Weapons/Swipe.prefab", enemyBrute1.transform.position + new Vector3(AttackRange * Mathf.Cos(enemyBrute1.state.facingAngle * ((float) Math.PI / 180f)), AttackRange * Mathf.Sin(enemyBrute1.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), Quaternion.identity, enemyBrute1.transform.parent, (System.Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<Swipe>().Init(enemyBrute.transform.position + new Vector3(AttackRange * Mathf.Cos(enemyBrute.state.facingAngle * ((float) Math.PI / 180f)), AttackRange * Mathf.Sin(enemyBrute.state.facingAngle * ((float) Math.PI / 180f)), -0.5f), enemyBrute.state.facingAngle, enemyBrute.health.team, enemyBrute.health, (System.Action<Health, Health.AttackTypes, Health.AttackFlags, float>) null, AttackRange)));
              }
              if ((UnityEngine.Object) enemyBrute1.damageColliderEvents != (UnityEngine.Object) null)
              {
                enemyBrute1.damageColliderEvents.SetActive(false);
                break;
              }
              break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          enemyBrute1.state.Timer += Time.deltaTime;
          if ((double) enemyBrute1.state.Timer >= (double) enemyBrute1.PostAttackDuration)
          {
            enemyBrute1.AttackType = EnemyBrute.AttackTypes.Punch;
            if ((UnityEngine.Object) enemyBrute1.TargetObject == (UnityEngine.Object) null)
            {
              enemyBrute1.ClearTarget();
              Loop = false;
              enemyBrute1.StartCoroutine((IEnumerator) enemyBrute1.WaitForEnemy());
            }
            else
            {
              double num = (double) Vector3.Distance(enemyBrute1.transform.position, enemyBrute1.TargetPosition);
              enemyBrute1.givePath(enemyBrute1.TargetObject.transform.position);
            }
          }
          if ((double) enemyBrute1.state.Timer >= 0.10000000149011612 && (UnityEngine.Object) enemyBrute1.damageColliderEvents != (UnityEngine.Object) null)
          {
            enemyBrute1.damageColliderEvents.SetActive(false);
            break;
          }
          break;
      }
      yield return (object) null;
    }
  }

  public void ClearTarget()
  {
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
      this.TargetObject.GetComponent<Health>().attackers.Remove(this.gameObject);
    this.TargetObject = (GameObject) null;
  }

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }

  [CompilerGenerated]
  public void \u003CChasePlayer\u003Eb__35_0()
  {
    this.ParticleSystem.Play();
    this.ParticleSystem.transform.position = this.damageColliderEvents.transform.position;
    this.TargetWarning.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.SetActive(true);
  }

  public enum AttackTypes
  {
    Area,
    Punch,
    Last,
  }
}
