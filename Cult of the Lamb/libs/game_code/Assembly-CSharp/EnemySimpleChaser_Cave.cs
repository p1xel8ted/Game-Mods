// Decompiled with JetBrains decompiler
// Type: EnemySimpleChaser_Cave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySimpleChaser_Cave : UnitObject
{
  public List<Collider2D> collider2DList;
  public Collider2D DamageCollider;
  public Health EnemyHealth;
  public GameObject TargetObject;
  public float DetectEnemyRange = 8f;
  public float RepathTimer;
  public float SeperationRadius = 0.4f;
  public bool SetStartPosition;
  public Vector3 StartPosition = Vector3.one * (float) int.MaxValue;
  public float Delay;
  public SpriteRenderer spriteRenderer;
  public Material wormMaterial;
  public int colorID;
  public float maxSpeedIncrease;
  public SimpleSpineFlash SimpleSpineFlash;
  public Color wormColor;
  public float AttackSpeed = 0.5f;

  public void Start()
  {
    this.maxSpeedIncrease = this.maxSpeed * 3f;
    this.SeperateObject = true;
    this.colorID = Shader.PropertyToID("_GlowColour");
    this.wormMaterial.SetColor(this.colorID, Color.blue);
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
    this.wormMaterial.SetColor(this.colorID, Color.blue);
    base.OnEnable();
    if (this.SetStartPosition)
    {
      this.transform.position = this.StartPosition;
    }
    else
    {
      this.StartPosition = this.transform.position;
      this.SetStartPosition = true;
    }
    this.Delay = 0.0f;
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.TargetObject = (GameObject) null;
    this.Delay = 0.0f;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.wormMaterial.SetColor(this.colorID, Color.red);
    base.OnHit(Attacker, AttackLocation, AttackType);
    this.knockBackVX = -0.5f * Mathf.Cos(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
    this.knockBackVY = -0.5f * Mathf.Sin(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
    this.SeperateObject = true;
    this.UsePathing = true;
    this.health.invincible = false;
    this.StopAllCoroutines();
    if ((double) AttackLocation.x > (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitRight)
      this.state.CURRENT_STATE = StateMachine.State.HitRight;
    if ((double) AttackLocation.x < (double) this.transform.position.x && this.state.CURRENT_STATE != StateMachine.State.HitLeft)
      this.state.CURRENT_STATE = StateMachine.State.HitLeft;
    this.StartCoroutine((IEnumerator) this.HurtRoutine());
  }

  public IEnumerator HurtRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemySimpleChaser_Cave simpleChaserCave = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      simpleChaserCave.state.CURRENT_STATE = StateMachine.State.Moving;
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.3f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public override void Update()
  {
    base.Update();
    if ((double) (this.Delay -= Time.deltaTime) > 0.0)
      return;
    if (this.wormMaterial.GetColor(this.colorID) == Color.red && (double) this.maxSpeed != (double) this.maxSpeedIncrease)
      this.maxSpeed = this.maxSpeedIncrease;
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
    {
      switch (this.state.CURRENT_STATE)
      {
        case StateMachine.State.Idle:
          if ((double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < 8.0)
          {
            this.givePath(this.TargetObject.transform.position);
            break;
          }
          break;
        case StateMachine.State.Moving:
          if ((double) (this.RepathTimer += Time.deltaTime) > 0.5)
          {
            this.RepathTimer = 0.0f;
            this.givePath(this.TargetObject.transform.position);
            break;
          }
          if ((double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < 2.0)
          {
            this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          if ((UnityEngine.Object) this.SimpleSpineFlash != (UnityEngine.Object) null)
            this.SimpleSpineFlash.FlashWhite(this.state.Timer / 0.5f);
          if ((double) (this.state.Timer += Time.deltaTime) > 0.5)
          {
            this.speed = this.AttackSpeed;
            if ((UnityEngine.Object) this.SimpleSpineFlash != (UnityEngine.Object) null)
              this.SimpleSpineFlash.FlashWhite(false);
            this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) this.state.Timer < 0.20000000298023224)
          {
            this.collider2DList = new List<Collider2D>();
            this.DamageCollider.GetContacts((List<Collider2D>) this.collider2DList);
            foreach (Component component in this.collider2DList)
            {
              this.EnemyHealth = component.gameObject.GetComponent<Health>();
              if ((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null && this.EnemyHealth.team != this.health.team)
                this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
            }
          }
          if ((double) (this.state.Timer += Time.deltaTime) > 1.0)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
      }
    }
    else
      this.GetNewTarget();
    this.spriteRenderer.transform.localScale = new Vector3((double) this.state.facingAngle >= 90.0 || (double) this.state.facingAngle <= -90.0 ? 1f : -1f, 1f, 1f);
    if (!this.SeperateObject)
      return;
    this.Seperate(this.SeperationRadius, true);
  }

  public void GetNewTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.CheckLineOfSightOnTarget(allUnit.gameObject, allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
      {
        float num2 = Vector3.Distance(this.transform.position, allUnit.gameObject.transform.position);
        if ((double) num2 < (double) num1)
        {
          health = allUnit;
          num1 = num2;
        }
      }
    }
    if (!((UnityEngine.Object) health != (UnityEngine.Object) null))
      return;
    this.TargetObject = health.gameObject;
    this.EnemyHealth = health;
    this.EnemyHealth.attackers.Add(this.gameObject);
  }
}
