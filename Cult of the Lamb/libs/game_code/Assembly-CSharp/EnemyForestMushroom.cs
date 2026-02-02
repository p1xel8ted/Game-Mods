// Decompiled with JetBrains decompiler
// Type: EnemyForestMushroom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyForestMushroom : BaseMonoBehaviour
{
  public static List<GameObject> enemyForestMushrooms = new List<GameObject>();
  public Health health;
  public Rigidbody2D rb2D;
  public SimpleSpineAnimator simpleSpineAnimator;
  public StateMachine state;
  public float KnockbackSpeed = 1500f;
  public int Damage = 1;
  public Coroutine ChasePlayerCoroutine;
  public GameObject Player;
  public Vector3 Seperator;
  public float SeperationRadius = 1f;

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.rb2D = this.GetComponent<Rigidbody2D>();
    this.simpleSpineAnimator = this.GetComponentInChildren<SimpleSpineAnimator>();
    this.state = this.GetComponent<StateMachine>();
    EnemyForestMushroom.enemyForestMushrooms.Add(this.gameObject);
  }

  public void OnDisable()
  {
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    this.health.OnDie -= new Health.DieAction(this.OnDie);
    EnemyForestMushroom.enemyForestMushrooms.Remove(this.gameObject);
  }

  public void Start() => this.StartCoroutine((IEnumerator) this.SpawnIn());

  public IEnumerator SpawnIn()
  {
    EnemyForestMushroom enemyForestMushroom = this;
    enemyForestMushroom.state.CURRENT_STATE = StateMachine.State.SpawnIn;
    if ((UnityEngine.Object) enemyForestMushroom.Player == (UnityEngine.Object) null)
    {
      enemyForestMushroom.Player = PlayerFarming.FindClosestPlayerGameObject(enemyForestMushroom.transform.position);
      if ((UnityEngine.Object) enemyForestMushroom.Player != (UnityEngine.Object) null)
        enemyForestMushroom.state.facingAngle = Utils.GetAngle(enemyForestMushroom.transform.position, enemyForestMushroom.Player.transform.position);
    }
    yield return (object) new WaitForSeconds(0.5f);
    enemyForestMushroom.ChasePlayerCoroutine = enemyForestMushroom.StartCoroutine((IEnumerator) enemyForestMushroom.ChasePlayer());
  }

  public IEnumerator ChasePlayer()
  {
    EnemyForestMushroom enemyForestMushroom = this;
    while ((UnityEngine.Object) enemyForestMushroom.Player == (UnityEngine.Object) null)
    {
      enemyForestMushroom.Player = PlayerFarming.FindClosestPlayerGameObject(enemyForestMushroom.transform.position);
      yield return (object) null;
    }
    enemyForestMushroom.state.CURRENT_STATE = StateMachine.State.Idle;
    bool Loop = true;
    float MoveSpeed = -2f;
    while (Loop)
    {
      if ((double) MoveSpeed < 2.0)
        MoveSpeed += 0.25f;
      enemyForestMushroom.state.facingAngle = Utils.SmoothAngle(enemyForestMushroom.state.facingAngle, Utils.GetAngle(enemyForestMushroom.transform.position, enemyForestMushroom.Player.transform.position), 10f);
      float f = enemyForestMushroom.state.facingAngle * ((float) Math.PI / 180f);
      Vector3 vector3 = new Vector3(MoveSpeed * Mathf.Cos(f), MoveSpeed * Mathf.Sin(f)) * Time.deltaTime;
      enemyForestMushroom.transform.position = enemyForestMushroom.transform.position + vector3;
      if ((double) Vector3.Distance(enemyForestMushroom.transform.position, enemyForestMushroom.Player.transform.position) <= 1.0)
        Loop = false;
      else
        yield return (object) null;
    }
    enemyForestMushroom.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      if (Time.frameCount % 5 == 0)
        enemyForestMushroom.simpleSpineAnimator.FlashWhite(!enemyForestMushroom.simpleSpineAnimator.isFillWhite);
      yield return (object) null;
    }
    enemyForestMushroom.simpleSpineAnimator.FlashWhite(false);
    if (enemyForestMushroom.simpleSpineAnimator.IsVisible)
      CameraManager.shakeCamera(0.5f, Utils.GetAngle(enemyForestMushroom.transform.position, enemyForestMushroom.Player.transform.position));
    enemyForestMushroom.DealDamage();
    enemyForestMushroom.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    yield return (object) new WaitForSeconds(1f);
    enemyForestMushroom.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyForestMushroom.ChasePlayerCoroutine = enemyForestMushroom.StartCoroutine((IEnumerator) enemyForestMushroom.ChasePlayer());
  }

  public void Update() => this.SeperateMushrooms();

  public void DealDamage()
  {
    foreach (Component component1 in Physics2D.OverlapCircleAll((Vector2) this.transform.position, 1f))
    {
      Health component2 = component1.gameObject.GetComponent<Health>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.team != this.health.team)
        component2.DealDamage((float) this.Damage, this.gameObject, Vector3.Lerp(this.transform.position, component2.transform.position, 0.8f));
    }
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    float f = Utils.GetAngle(Attacker.transform.position, this.transform.position) * ((float) Math.PI / 180f);
    this.StartCoroutine((IEnumerator) this.AddForce((Vector3) new Vector2(this.KnockbackSpeed * Mathf.Cos(f), this.KnockbackSpeed * Mathf.Sin(f))));
    this.simpleSpineAnimator.FillColor(Color.red);
    CameraManager.shakeCamera(0.2f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.state.CURRENT_STATE = StateMachine.State.HitThrown;
    this.StopCoroutine(this.ChasePlayerCoroutine);
  }

  public IEnumerator AddForce(Vector3 Force)
  {
    EnemyForestMushroom enemyForestMushroom = this;
    yield return (object) new WaitForSeconds(0.05f);
    enemyForestMushroom.rb2D.AddForce((Vector2) Force);
    enemyForestMushroom.simpleSpineAnimator.FlashFillRed();
    yield return (object) new WaitForSeconds(0.3f);
    enemyForestMushroom.ChasePlayerCoroutine = enemyForestMushroom.StartCoroutine((IEnumerator) enemyForestMushroom.ChasePlayer());
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  public void SeperateMushrooms()
  {
    this.Seperator = Vector3.zero;
    foreach (GameObject enemyForestMushroom in EnemyForestMushroom.enemyForestMushrooms)
    {
      if ((UnityEngine.Object) enemyForestMushroom != (UnityEngine.Object) this.gameObject && (UnityEngine.Object) enemyForestMushroom != (UnityEngine.Object) null && this.state.CURRENT_STATE != StateMachine.State.SignPostAttack && this.state.CURRENT_STATE != StateMachine.State.RecoverFromAttack && this.state.CURRENT_STATE != StateMachine.State.Defending)
      {
        float num = Vector2.Distance((Vector2) enemyForestMushroom.gameObject.transform.position, (Vector2) this.transform.position);
        float angle = Utils.GetAngle(enemyForestMushroom.gameObject.transform.position, this.transform.position);
        if ((double) num < (double) this.SeperationRadius)
        {
          this.Seperator.x += (float) (((double) this.SeperationRadius - (double) num) / 2.0) * Mathf.Cos(angle * ((float) Math.PI / 180f)) * GameManager.DeltaTime;
          this.Seperator.y += (float) (((double) this.SeperationRadius - (double) num) / 2.0) * Mathf.Sin(angle * ((float) Math.PI / 180f)) * GameManager.DeltaTime;
        }
      }
    }
    this.transform.position = this.transform.position + this.Seperator;
  }
}
