// Decompiled with JetBrains decompiler
// Type: EnemySimpleChaser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using Spine.Unity.Modules;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySimpleChaser : UnitObject
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
  public Transform scaleMe;
  public SimpleSpineFlash SimpleSpineFlash;
  public float DamageRadius = 2f;
  public float AttackTriggerRange = 1f;
  public GameObject WarningObject;
  public float AttackCooldown = 0.5f;
  public float AttackCooldownTimer;
  public ParticleSystem particleSystem;
  public SkeletonAnimation Spine;
  public SkeletonUtilityEyeConstraint skeletonUtilityEyeConstraint;
  public float AttackSpeed = 0.7f;
  public List<FollowAsTail> TailPieces = new List<FollowAsTail>();
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string Head;
  [SpineSkin("", "", true, false, false, dataField = "Spine")]
  public string HeadFacingUp;
  public bool FacingUp;

  public void Start() => this.SeperateObject = true;

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
    this.WarningObject.SetActive(false);
    this.particleSystem.Stop();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.TargetObject = (GameObject) null;
    this.Delay = 0.0f;
  }

  public override void Update()
  {
    base.Update();
    if ((double) (this.Delay -= Time.deltaTime) > 0.0)
      return;
    if ((Object) this.TargetObject != (Object) null)
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
          this.AttackCooldown -= Time.deltaTime;
          this.skeletonUtilityEyeConstraint.targetPosition = this.TargetObject.transform.position + Vector3.back * 0.5f;
          if ((double) (this.RepathTimer += Time.deltaTime) > 0.5)
          {
            this.RepathTimer = 0.0f;
            this.givePath(this.TargetObject.transform.position);
            break;
          }
          if ((double) this.AttackCooldown < 0.0 && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < (double) this.AttackTriggerRange)
          {
            this.WarningObject.SetActive(true);
            this.state.CURRENT_STATE = StateMachine.State.SignPostAttack;
            break;
          }
          break;
        case StateMachine.State.SignPostAttack:
          this.SimpleSpineFlash.FlashWhite(this.state.Timer / 0.5f);
          if ((double) (this.state.Timer += Time.deltaTime) > 0.5)
          {
            this.SimpleSpineFlash.FlashWhite(false);
            this.WarningObject.SetActive(false);
            this.particleSystem.Play();
            CameraManager.shakeCamera(0.4f);
            this.state.CURRENT_STATE = StateMachine.State.RecoverFromAttack;
            break;
          }
          break;
        case StateMachine.State.RecoverFromAttack:
          if ((double) this.state.Timer > 0.05000000074505806 && (double) this.state.Timer < 0.20000000298023224)
          {
            int index = -1;
            while (++index < Health.allUnits.Count)
            {
              Health allUnit = Health.allUnits[index];
              if ((Object) allUnit != (Object) null && allUnit.team != this.health.team && (double) Vector3.Distance(allUnit.transform.position, this.transform.position) < (double) this.DamageRadius)
                allUnit.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, allUnit.transform.position, 0.7f));
            }
          }
          if ((double) (this.state.Timer += Time.deltaTime) > 1.0)
          {
            this.state.CURRENT_STATE = StateMachine.State.Moving;
            this.AttackCooldownTimer = 0.5f;
            break;
          }
          break;
      }
    }
    else
      this.GetNewTarget();
    this.Spine.skeleton.ScaleX = (double) this.state.facingAngle <= 90.0 || (double) this.state.facingAngle >= 270.0 ? -1f : 1f;
    if (this.SeperateObject)
      this.Seperate(this.SeperationRadius, true);
    int index1 = -1;
    while (++index1 < this.TailPieces.Count)
      this.TailPieces[index1].UpdatePosition();
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
    if (!((Object) health != (Object) null))
      return;
    this.TargetObject = health.gameObject;
    this.EnemyHealth = health;
    this.EnemyHealth.attackers.Add(this.gameObject);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.AttackTriggerRange, Color.white);
    Utils.DrawCircleXY(this.transform.position, this.DamageRadius, Color.red);
  }
}
