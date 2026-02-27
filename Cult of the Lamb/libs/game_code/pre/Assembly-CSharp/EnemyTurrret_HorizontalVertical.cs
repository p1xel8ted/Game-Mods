// Decompiled with JetBrains decompiler
// Type: EnemyTurrret_HorizontalVertical
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyTurrret_HorizontalVertical : UnitObject
{
  public SpriteRenderer Aiming;
  public SpriteRenderer Image;
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public Sprite ClosedEye;
  public Sprite OpenEye;
  public float LookAngle;
  private float ShootDelay;
  public int ShotsToFire = 5;
  public float DetectEnemyRange = 8f;
  public GameObject Arrow;
  private bool Shooting;
  private GameObject TargetObject;
  private Health EnemyHealth;
  public EnemyTurrret_HorizontalVertical.ShootDirection shootingDirection;
  public float shakeDuration = 0.5f;
  public Vector3 shakeStrength = new Vector3(0.25f, 0.25f, 0.01f);
  public int vibrato = 10;
  public float randomness = 90f;
  public bool ForceShootingDirection;
  public float UpAngle;
  public float DownAngle;
  public float RightAngle;
  public float LeftAngle;
  public float Offset;
  public bool changedDirection;
  private float currentAngle;
  public float anticipationWaitTime = 1f;
  public float coolDownForcedDirection = 0.5f;
  public float coolDownChangingDirection = 0.33f;

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.Aiming.gameObject.SetActive(false);
    this.Spine.AnimationState.SetAnimation(0, "closed", true);
  }

  private new void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    BiomeConstants.Instance.EmitHitVFX(AttackLocation, Quaternion.identity.z, "HitFX_Weak");
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
  }

  private new void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  private new void Update()
  {
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      if (Time.frameCount % 10 != 0)
        return;
      this.GetNewTarget();
    }
    else if (!this.Shooting)
    {
      if ((double) (this.ShootDelay -= Time.deltaTime) >= 0.0)
        return;
      if (!this.ForceShootingDirection)
      {
        if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
          this.LookAngle = this.ClampAngle(Utils.GetAngle(this.transform.position, this.TargetObject.transform.position));
      }
      else
      {
        switch (this.shootingDirection)
        {
          case EnemyTurrret_HorizontalVertical.ShootDirection.Up:
            this.LookAngle = 90f;
            break;
          case EnemyTurrret_HorizontalVertical.ShootDirection.Down:
            this.LookAngle = 270f;
            break;
          case EnemyTurrret_HorizontalVertical.ShootDirection.Left:
            this.LookAngle = 180f;
            break;
          case EnemyTurrret_HorizontalVertical.ShootDirection.Right:
            this.LookAngle = 0.0f;
            break;
        }
      }
      this.StartCoroutine((IEnumerator) this.ShootArrowRoutine());
    }
    else
    {
      if ((double) Vector3.Distance(this.TargetObject.transform.position, this.transform.position) <= 12.0)
        return;
      this.TargetObject = (GameObject) null;
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    }
  }

  private float ClampAngle(float a)
  {
    Debug.Log((object) a);
    a = (double) a <= (double) this.RightAngle + (double) this.Offset || (double) a >= (double) this.UpAngle + (double) this.Offset ? ((double) a <= (double) this.UpAngle + (double) this.Offset || (double) a >= (double) this.LeftAngle + (double) this.Offset ? ((double) a <= (double) this.LeftAngle + (double) this.Offset || (double) a >= (double) this.DownAngle + (double) this.Offset ? ((double) a <= (double) this.DownAngle + (double) this.Offset || (double) a >= 360.0 + (double) this.Offset ? 0.0f : 270f) : 180f) : 90f) : 0.0f;
    return a;
  }

  private void OnDrawGizmos()
  {
    if (!this.ForceShootingDirection)
    {
      Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos((float) (((double) this.UpAngle + (double) this.Offset) * (Math.PI / 180.0))), 2f * Mathf.Sin((float) (((double) this.UpAngle + (double) this.Offset) * (Math.PI / 180.0)))), Color.blue);
      Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos((float) (((double) this.DownAngle + (double) this.Offset) * (Math.PI / 180.0))), 2f * Mathf.Sin((float) (((double) this.DownAngle + (double) this.Offset) * (Math.PI / 180.0)))), Color.blue);
      Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos((float) (((double) this.RightAngle + (double) this.Offset) * (Math.PI / 180.0))), 2f * Mathf.Sin((float) (((double) this.RightAngle + (double) this.Offset) * (Math.PI / 180.0)))), Color.blue);
      Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos((float) (((double) this.LeftAngle + (double) this.Offset) * (Math.PI / 180.0))), 2f * Mathf.Sin((float) (((double) this.LeftAngle + (double) this.Offset) * (Math.PI / 180.0)))), Color.blue);
    }
    else
    {
      switch (this.shootingDirection)
      {
        case EnemyTurrret_HorizontalVertical.ShootDirection.Up:
          Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos(1.57079637f), 2f * Mathf.Sin(1.57079637f)), Color.blue);
          break;
        case EnemyTurrret_HorizontalVertical.ShootDirection.Down:
          Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos(4.712389f), 2f * Mathf.Sin(4.712389f)), Color.blue);
          break;
        case EnemyTurrret_HorizontalVertical.ShootDirection.Left:
          Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos(3.14159274f), 2f * Mathf.Sin(3.14159274f)), Color.blue);
          break;
        case EnemyTurrret_HorizontalVertical.ShootDirection.Right:
          Utils.DrawLine(this.transform.position, this.transform.position + new Vector3(2f * Mathf.Cos(0.0f), 2f * Mathf.Sin(0.0f)), Color.blue);
          break;
      }
    }
  }

  private IEnumerator ShootArrowRoutine()
  {
    EnemyTurrret_HorizontalVertical horizontalVertical = this;
    horizontalVertical.Image.sprite = horizontalVertical.OpenEye;
    horizontalVertical.Shooting = true;
    int i = horizontalVertical.ShotsToFire;
    while (--i >= 0)
    {
      horizontalVertical.Aiming.gameObject.SetActive(true);
      horizontalVertical.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, horizontalVertical.LookAngle);
      if (Time.frameCount % 5 == 0)
        horizontalVertical.Aiming.color = horizontalVertical.Aiming.color == Color.red ? Color.white : Color.red;
      yield return (object) new WaitForSeconds(horizontalVertical.anticipationWaitTime / (float) horizontalVertical.ShotsToFire);
    }
    i = horizontalVertical.ShotsToFire;
    while (--i >= 0)
    {
      horizontalVertical.Spine.AnimationState.SetAnimation(0, "shoot", false);
      horizontalVertical.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      CameraManager.shakeCamera(0.2f, horizontalVertical.LookAngle);
      Projectile component = ObjectPool.Spawn(horizontalVertical.Arrow, horizontalVertical.transform.parent).GetComponent<Projectile>();
      component.transform.position = horizontalVertical.transform.position;
      component.Angle = horizontalVertical.LookAngle;
      component.team = horizontalVertical.health.team;
      component.Speed = 6f;
      component.Owner = horizontalVertical.health;
      yield return (object) new WaitForSeconds(0.2f);
      if (!horizontalVertical.ForceShootingDirection)
      {
        if ((UnityEngine.Object) horizontalVertical.TargetObject != (UnityEngine.Object) null)
          horizontalVertical.currentAngle = horizontalVertical.ClampAngle(Utils.GetAngle(horizontalVertical.transform.position, horizontalVertical.TargetObject.transform.position));
        if ((double) horizontalVertical.currentAngle == (double) horizontalVertical.LookAngle)
        {
          horizontalVertical.ShootDelay = horizontalVertical.coolDownChangingDirection;
          horizontalVertical.changedDirection = false;
        }
        else
        {
          horizontalVertical.changedDirection = true;
          horizontalVertical.ShootDelay = horizontalVertical.coolDownChangingDirection;
        }
      }
      else
        horizontalVertical.ShootDelay = horizontalVertical.coolDownForcedDirection;
    }
    horizontalVertical.Shooting = false;
    horizontalVertical.Image.sprite = horizontalVertical.ClosedEye;
  }

  public void GetNewTarget()
  {
    Health health = (Health) null;
    float num1 = float.MaxValue;
    foreach (Health allUnit in Health.allUnits)
    {
      if (allUnit.team != this.health.team && !allUnit.InanimateObject && allUnit.team != Health.Team.Neutral && (this.health.team != Health.Team.PlayerTeam || this.health.team == Health.Team.PlayerTeam && allUnit.team != Health.Team.DangerousAnimals) && (double) Vector2.Distance((Vector2) this.transform.position, (Vector2) allUnit.gameObject.transform.position) < (double) this.DetectEnemyRange && this.CheckLineOfSight(allUnit.gameObject.transform.position, Vector2.Distance((Vector2) allUnit.gameObject.transform.position, (Vector2) this.transform.position)))
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
    this.Spine.AnimationState.SetAnimation(0, "idle", true);
  }

  public new bool CheckLineOfSight(Vector3 pointToCheck, float distance)
  {
    if ((UnityEngine.Object) this.ColliderRadius == (UnityEngine.Object) null)
      this.ColliderRadius = this.GetComponent<CircleCollider2D>();
    if ((UnityEngine.Object) Physics2D.Raycast((Vector2) this.transform.position, (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck).collider != (UnityEngine.Object) null)
      return false;
    float angle = Utils.GetAngle(this.transform.position, pointToCheck);
    RaycastHit2D raycastHit2D = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle + 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle + 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      return false;
    raycastHit2D = Physics2D.Raycast((Vector2) (this.transform.position + new Vector3(this.ColliderRadius.radius * Mathf.Cos((float) (((double) angle - 90.0) * (Math.PI / 180.0))), this.ColliderRadius.radius * Mathf.Sin((float) (((double) angle - 90.0) * (Math.PI / 180.0))))), (Vector2) (pointToCheck - this.transform.position), distance, (int) this.layerToCheck);
    return !((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null);
  }

  public enum ShootDirection
  {
    Up,
    Down,
    Left,
    Right,
  }
}
