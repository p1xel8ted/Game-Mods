// Decompiled with JetBrains decompiler
// Type: EnemyMortar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyMortar : UnitObject
{
  public GameObject ThrowBone;
  public GameObject RockToThrow;
  public float SeperationRadius = 0.5f;
  public SimpleSpineAnimator simpleSpineAnimator;
  public SpriteRenderer sprite;
  public const string SHADER_COLOR_NAME = "_Color";
  public Rigidbody2D rb2D;
  public GameObject TargetObject;
  public float Range = 6f;
  public float KnockbackSpeed = 1f;
  public float ExplosionRadius = 1f;
  public List<GameObject> ToSpawn = new List<GameObject>();
  public SimpleSpineEventListener simpleSpineEventListener;
  public BruteRock b;
  public Coroutine ChasePlayerCoroutine;
  public float StartSpeed = 0.4f;
  public bool Thrown;
  public GameObject g;
  public bool Attacked;
  public float WhiteFade;
  public List<Collider2D> collider2DList;
  public Collider2D DamageCollider;
  public Health EnemyHealth;

  public void Start()
  {
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    this.rb2D = this.GetComponent<Rigidbody2D>();
    this.SeperateObject = true;
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.simpleSpineEventListener = this.GetComponent<SimpleSpineEventListener>();
    this.simpleSpineEventListener.OnSpineEvent += new SimpleSpineEventListener.SpineEvent(this.OnSpineEvent);
  }

  public void OnSpineEvent(string EventName)
  {
    switch (EventName)
    {
      case "shoot":
        if (this.simpleSpineAnimator.IsVisible)
          CameraManager.shakeCamera(0.4f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
        this.Thrown = true;
        this.b.Play(this.ThrowBone.transform.position);
        break;
      case "shoot complete":
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.StartCoroutine((IEnumerator) this.ChasePlayer());
        break;
      case "attack":
        if (this.simpleSpineAnimator.IsVisible)
          CameraManager.shakeCamera(0.3f, Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
        this.Attacked = true;
        this.collider2DList = new List<Collider2D>();
        this.DamageCollider.GetContacts((List<Collider2D>) this.collider2DList);
        using (List<Collider2D>.Enumerator enumerator = this.collider2DList.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            this.EnemyHealth = enumerator.Current.gameObject.GetComponent<Health>();
            if ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null && (this.EnemyHealth.team != this.health.team || this.health.team == Health.Team.PlayerTeam))
              this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
          }
          break;
        }
      case "attack complete":
        this.state.CURRENT_STATE = StateMachine.State.Idle;
        this.StartCoroutine((IEnumerator) this.ChasePlayer());
        break;
    }
  }

  public override void OnDisable() => base.OnDisable();

  public IEnumerator WaitForTarget()
  {
    EnemyMortar enemyMortar = this;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      enemyMortar.Seperate(enemyMortar.SeperationRadius);
      yield return (object) null;
    }
    while ((UnityEngine.Object) enemyMortar.TargetObject == (UnityEngine.Object) null)
    {
      enemyMortar.TargetObject = PlayerFarming.FindClosestPlayerGameObject(enemyMortar.transform.position);
      yield return (object) null;
    }
    while ((double) Vector3.Distance(enemyMortar.TargetObject.transform.position, enemyMortar.transform.position) > (double) enemyMortar.Range)
      yield return (object) null;
    enemyMortar.ChasePlayerCoroutine = enemyMortar.StartCoroutine((IEnumerator) enemyMortar.ChasePlayer());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.simpleSpineAnimator.FlashFillRed();
    BiomeConstants.Instance.EmitHitVFX(AttackLocation - Vector3.back * 0.5f, Quaternion.identity.z, "HitFX_Weak");
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    GameObject gameObject = BiomeConstants.Instance.GroundSmash_Medium.Spawn();
    gameObject.transform.position = this.transform.position;
    gameObject.transform.rotation = Quaternion.identity;
    CameraManager.shakeCamera(0.3f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.ClearPaths();
    this.StopAllCoroutines();
  }

  public IEnumerator ThrowRock()
  {
    EnemyMortar enemyMortar = this;
    float RandomDelay = UnityEngine.Random.Range(0.2f, 1f);
    while ((double) (RandomDelay -= Time.deltaTime) > 0.0)
      yield return (object) null;
    enemyMortar.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    enemyMortar.state.facingAngle = Utils.GetAngle(enemyMortar.transform.position, enemyMortar.TargetObject.transform.position);
    enemyMortar.simpleSpineAnimator.Animate("shoot", 0, false);
    enemyMortar.Thrown = false;
    enemyMortar.g = UnityEngine.Object.Instantiate<GameObject>(enemyMortar.RockToThrow, enemyMortar.TargetObject.transform.position, Quaternion.identity, enemyMortar.transform.parent);
    enemyMortar.b = enemyMortar.g.GetComponent<BruteRock>();
    while (!enemyMortar.Thrown)
    {
      if ((double) Vector2.Distance((Vector2) enemyMortar.transform.position, (Vector2) enemyMortar.TargetObject.transform.position) > 3.0)
        enemyMortar.g.transform.position = Vector3.Lerp(enemyMortar.g.transform.position, enemyMortar.TargetObject.transform.position, 10f * Time.deltaTime);
      yield return (object) null;
    }
    enemyMortar.simpleSpineAnimator.FlashWhite(false);
  }

  public IEnumerator Attack()
  {
    EnemyMortar enemyMortar = this;
    enemyMortar.state.CURRENT_STATE = StateMachine.State.CustomAction0;
    enemyMortar.state.facingAngle = Utils.GetAngle(enemyMortar.transform.position, enemyMortar.TargetObject.transform.position);
    enemyMortar.simpleSpineAnimator.Animate("attack", 0, false);
    enemyMortar.Attacked = false;
    while (!enemyMortar.Attacked)
    {
      if (Time.frameCount % 5 == 0)
        enemyMortar.simpleSpineAnimator.FlashWhite(enemyMortar.simpleSpineAnimator.isFillWhite = !enemyMortar.simpleSpineAnimator.isFillWhite);
      yield return (object) null;
    }
    enemyMortar.simpleSpineAnimator.FlashWhite(false);
  }

  public IEnumerator ChasePlayer()
  {
    EnemyMortar enemyMortar = this;
    enemyMortar.state.CURRENT_STATE = StateMachine.State.Idle;
    bool Loop = true;
    float AttackTimer = UnityEngine.Random.Range(0.5f, 1.5f);
    while (Loop)
    {
      if ((UnityEngine.Object) enemyMortar.TargetObject == (UnityEngine.Object) null)
      {
        enemyMortar.StartCoroutine((IEnumerator) enemyMortar.WaitForTarget());
        break;
      }
      if (enemyMortar.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack)
        enemyMortar.Seperate(enemyMortar.SeperationRadius);
      if (enemyMortar.state.CURRENT_STATE == StateMachine.State.Idle)
      {
        AttackTimer -= Time.deltaTime;
        float num = Vector2.Distance((Vector2) enemyMortar.transform.position, (Vector2) enemyMortar.TargetObject.transform.position);
        if ((double) num < 20.0 && (double) num > 3.0 && (double) AttackTimer < 0.0)
        {
          enemyMortar.StartCoroutine((IEnumerator) enemyMortar.ThrowRock());
          break;
        }
      }
      yield return (object) null;
    }
  }
}
