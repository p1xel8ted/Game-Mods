// Decompiled with JetBrains decompiler
// Type: EnemyForestHead
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyForestHead : BaseMonoBehaviour
{
  public Health health;
  public Rigidbody2D rb2D;
  public SimpleSpineAnimator simpleSpineAnimator;
  public StateMachine state;
  public float KnockbackSpeed = 1500f;
  public Transform SpawnPoint1;
  public Transform SpawnPoint2;
  public GameObject ToSpawn;
  public float MoveSpeed = -2f;
  public GameObject Player;
  public Vector3 Seperator;
  public float SeperationRadius = 3f;

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.rb2D = this.GetComponent<Rigidbody2D>();
    this.state = this.GetComponent<StateMachine>();
    EnemyForestMushroom.enemyForestMushrooms.Add(this.gameObject);
  }

  public void Start() => this.StartCoroutine((IEnumerator) this.ChasePlayer());

  public void OnDisable()
  {
    this.health.OnHit -= new Health.HitAction(this.OnHit);
    EnemyForestMushroom.enemyForestMushrooms.Remove(this.gameObject);
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
  }

  public IEnumerator AddForce(Vector3 Force)
  {
    this.MoveSpeed = 0.0f;
    yield return (object) new WaitForSeconds(0.05f);
    this.rb2D.AddForce((Vector2) Force);
    this.simpleSpineAnimator.FlashFillRed();
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) < 0.5)
    {
      this.MoveSpeed = 0.0f;
      yield return (object) null;
    }
  }

  public IEnumerator ChasePlayer()
  {
    EnemyForestHead enemyForestHead = this;
    while ((UnityEngine.Object) enemyForestHead.Player == (UnityEngine.Object) null)
    {
      enemyForestHead.Player = PlayerFarming.FindClosestPlayerGameObject(enemyForestHead.transform.position);
      yield return (object) null;
    }
    enemyForestHead.state.CURRENT_STATE = StateMachine.State.Idle;
    bool Loop = true;
    bool FacingUp = false;
    float AttackDelay = 3f;
    while (Loop)
    {
      double num = (double) Vector3.Distance(enemyForestHead.transform.position, enemyForestHead.Player.transform.position);
      if (num > 5.0)
      {
        if ((double) enemyForestHead.MoveSpeed < 1.0)
          enemyForestHead.MoveSpeed += 0.1f;
      }
      else
        enemyForestHead.MoveSpeed = Mathf.Lerp(enemyForestHead.MoveSpeed, 0.0f, 15f * Time.deltaTime);
      enemyForestHead.state.facingAngle = Utils.SmoothAngle(enemyForestHead.state.facingAngle, Utils.GetAngle(enemyForestHead.transform.position, enemyForestHead.Player.transform.position), 30f);
      float f = enemyForestHead.state.facingAngle * ((float) Math.PI / 180f);
      Vector3 vector3 = new Vector3(enemyForestHead.MoveSpeed * Mathf.Cos(f), enemyForestHead.MoveSpeed * Mathf.Sin(f)) * Time.deltaTime;
      enemyForestHead.transform.position = enemyForestHead.transform.position + vector3;
      if ((double) enemyForestHead.state.facingAngle > 30.0 && (double) enemyForestHead.state.facingAngle < 150.0)
      {
        if (!FacingUp)
        {
          FacingUp = true;
          enemyForestHead.simpleSpineAnimator.Animate("moving_backView", 0, true);
        }
      }
      else if (FacingUp)
      {
        FacingUp = false;
        enemyForestHead.simpleSpineAnimator.Animate("moving", 0, true);
      }
      if (num < 5.0 && (double) (AttackDelay -= Time.deltaTime) < 0.0 && EnemyForestMushroom.enemyForestMushrooms.Count < 5)
      {
        Loop = false;
        enemyForestHead.StartCoroutine((IEnumerator) enemyForestHead.SpawnEnemy());
      }
      else
        yield return (object) null;
    }
  }

  public IEnumerator SpawnEnemy()
  {
    EnemyForestHead enemyForestHead = this;
    enemyForestHead.MoveSpeed = 0.0f;
    enemyForestHead.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
    yield return (object) new WaitForSeconds(1f);
    enemyForestHead.StartCoroutine((IEnumerator) enemyForestHead.DoSpawn());
    yield return (object) new WaitForSeconds(1f);
    enemyForestHead.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
    yield return (object) new WaitForSeconds(0.5f);
    enemyForestHead.StartCoroutine((IEnumerator) enemyForestHead.ChasePlayer());
  }

  public IEnumerator DoSpawn()
  {
    if (this.simpleSpineAnimator.IsVisible)
      CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    UnityEngine.Object.Instantiate<GameObject>(this.ToSpawn).transform.position = this.SpawnPoint1.position with
    {
      z = 0.0f
    };
    BiomeConstants.Instance.SpawnInWhite.Spawn().transform.position = this.SpawnPoint1.position;
    yield return (object) new WaitForSeconds(0.5f);
    if (this.simpleSpineAnimator.IsVisible)
      CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    UnityEngine.Object.Instantiate<GameObject>(this.ToSpawn).transform.position = this.SpawnPoint2.position with
    {
      z = 0.0f
    };
    BiomeConstants.Instance.SpawnInWhite.Spawn().transform.position = this.SpawnPoint2.position;
  }

  public void Update()
  {
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
