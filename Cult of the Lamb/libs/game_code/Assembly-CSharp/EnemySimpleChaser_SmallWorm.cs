// Decompiled with JetBrains decompiler
// Type: EnemySimpleChaser_SmallWorm
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using Spine.Unity.Modules;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySimpleChaser_SmallWorm : UnitObject
{
  public List<Collider2D> collider2DList;
  public ColliderEvents damageColliderEvents;
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
  public SkeletonAnimation Spine;
  public SkeletonUtilityEyeConstraint skeletonUtilityEyeConstraint;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string Head;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string HeadFacingUp;
  public bool FacingUp;
  public float tailDistance;
  public float headDistance;
  public float shakeDuration = 0.5f;
  public Vector3 shakeStrength = new Vector3(0.5f, 0.5f, 0.01f);
  [Range(0.0f, 30f)]
  public int vibrato = 10;
  [Range(0.0f, 180f)]
  public float randomness = 90f;
  public float AttackSpeed = 0.5f;
  public bool reacted;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();

  public new void LateUpdate()
  {
    if ((double) this.state.facingAngle > 45.0 && (double) this.state.facingAngle < 135.0)
    {
      if (this.FacingUp)
        return;
      this.Spine.skeleton.SetSkin(this.HeadFacingUp);
      this.Spine.skeleton.SetSlotsToSetupPose();
      this.FacingUp = true;
    }
    else
    {
      if (!this.FacingUp)
        return;
      this.Spine.skeleton.SetSkin(this.Head);
      this.Spine.skeleton.SetSlotsToSetupPose();
      this.FacingUp = false;
    }
  }

  public void Start()
  {
    this.maxSpeedIncrease = this.maxSpeed * 3f;
    this.SimpleSpineFlash = this.GetComponentInChildren<SimpleSpineFlash>();
  }

  public override void OnEnable()
  {
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
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(true);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.TargetObject = (GameObject) null;
    this.Delay = 0.0f;
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    base.OnHit(Attacker, AttackLocation, AttackType);
    this.knockBackVX = -1f * Mathf.Cos(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
    this.knockBackVY = -1f * Mathf.Sin(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
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
    EnemySimpleChaser_SmallWorm simpleChaserSmallWorm = this;
    simpleChaserSmallWorm.Spine.AnimationState.SetAnimation(0, "attack-impact", true);
    simpleChaserSmallWorm.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
    simpleChaserSmallWorm.Spine.GetComponent<SimpleSpineFlash>().FlashFillRed();
    simpleChaserSmallWorm.spriteRenderer.color = Color.red;
    foreach (Component tailPiece in simpleChaserSmallWorm.TailPieces)
      tailPiece.GetComponent<SpriteRenderer>().color = Color.red;
    yield return (object) new WaitForSeconds(0.2f);
    simpleChaserSmallWorm.spriteRenderer.color = Color.white;
    foreach (Component tailPiece in simpleChaserSmallWorm.TailPieces)
      tailPiece.GetComponent<SpriteRenderer>().color = Color.white;
    simpleChaserSmallWorm.state.CURRENT_STATE = StateMachine.State.Moving;
  }

  public override void Update()
  {
    base.Update();
    if ((double) (this.Delay -= Time.deltaTime) > 0.0)
      return;
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
      this.damageColliderEvents.SetActive(true);
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
          if (!this.reacted)
          {
            this.Spine.AnimationState.SetAnimation(0, "notice-player", true);
            this.Spine.AnimationState.AddAnimation(0, "animation", true, 0.0f);
            this.reacted = true;
          }
          this.skeletonUtilityEyeConstraint.targetPosition = this.TargetObject.transform.position + Vector3.back * 0.5f;
          if ((double) (this.RepathTimer += Time.deltaTime) > 0.5)
          {
            this.RepathTimer = 0.0f;
            this.givePath(this.TargetObject.transform.position);
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
          if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
            this.damageColliderEvents.SetActive(true);
          if ((double) (this.state.Timer += Time.deltaTime) > 1.0)
          {
            this.state.CURRENT_STATE = StateMachine.State.Idle;
            break;
          }
          break;
      }
      int index = -1;
      while (++index < this.TailPieces.Count)
        this.TailPieces[index].UpdatePosition();
    }
    else
      this.GetNewTarget();
    this.spriteRenderer.transform.localScale = new Vector3((double) this.state.facingAngle >= 90.0 || (double) this.state.facingAngle <= -90.0 ? 1f : -1f, 1f, 1f);
    if (!this.SeperateObject)
      return;
    this.Seperate(this.SeperationRadius, true);
  }

  public void GetTailPieces()
  {
    this.TailPieces = new List<FollowAsTail>((IEnumerable<FollowAsTail>) this.GetComponentsInChildren<FollowAsTail>());
  }

  public void SetTailPieces()
  {
    int index = -1;
    while (++index < this.TailPieces.Count)
      this.TailPieces[index].FollowObject = index != 0 ? this.TailPieces[index - 1].transform : this.Spine.transform;
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

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.team == this.health.team)
      return;
    component.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, component.transform.position, 0.7f));
  }
}
