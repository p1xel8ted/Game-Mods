// Decompiled with JetBrains decompiler
// Type: EnemyTurretAreaAttack
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyTurretAreaAttack : UnitObject
{
  public SkeletonAnimation Spine;
  public SimpleSpineFlash SimpleSpineFlash;
  public SkeletonAnimation Warning;
  private float LookAngle;
  private float ShootDelay;
  public int ShotsToFire = 12;
  public float DetectEnemyRange = 8f;
  public GameObject Arrow;
  private bool Shooting;
  private GameObject TargetObject;
  private Health EnemyHealth;
  public float ShootInterval = 2f;
  public float LifeTime = 2f;

  private void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnDie += new Health.DieAction(((UnitObject) this).OnDie);
    this.Spine.AnimationState.SetAnimation(0, "closed", true);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Shooting = false;
    this.ShootDelay = 1f;
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/gethit", this.transform.position);
    this.StopAllCoroutines();
    this.Spine.AnimationState.SetAnimation(0, "idle", true);
    this.Shooting = false;
    if ((double) this.ShootDelay < 1.0)
      this.ShootDelay = 1f;
    BiomeConstants.Instance.EmitHitVFX(AttackLocation, Quaternion.identity.z, "HitFX_Weak");
    CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
    this.SimpleSpineFlash.FlashFillRed();
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/death", this.transform.position);
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.TargetObject == (UnityEngine.Object) null)
    {
      if (Time.frameCount % 10 != 0)
        return;
      this.GetNewTarget();
    }
    else if (!this.Shooting)
    {
      if ((double) (this.ShootDelay -= Time.deltaTime) >= 0.5)
        return;
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

  private IEnumerator ShootArrowRoutine()
  {
    EnemyTurretAreaAttack turretAreaAttack = this;
    turretAreaAttack.Shooting = true;
    float Progress = 0.0f;
    while ((double) (Progress += Time.deltaTime) < (double) turretAreaAttack.ShootInterval)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/warning", turretAreaAttack.transform.position);
    yield return (object) turretAreaAttack.Spine.YieldForAnimation("anticipation");
    turretAreaAttack.SimpleSpineFlash.FlashWhite(false);
    CameraManager.shakeCamera(0.2f, turretAreaAttack.LookAngle);
    turretAreaAttack.Spine.AnimationState.SetAnimation(0, "shoot", false);
    turretAreaAttack.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", turretAreaAttack.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/attack", turretAreaAttack.transform.position);
    float shotsToFire = (float) turretAreaAttack.ShotsToFire;
    while ((double) --shotsToFire >= 0.0)
    {
      Projectile component = ObjectPool.Spawn(turretAreaAttack.Arrow, turretAreaAttack.transform.parent).GetComponent<Projectile>();
      component.transform.position = turretAreaAttack.transform.position;
      component.Angle = 360f / (float) turretAreaAttack.ShotsToFire * shotsToFire;
      component.team = turretAreaAttack.health.team;
      component.Speed = 5f;
      component.LifeTime = turretAreaAttack.LifeTime + UnityEngine.Random.Range(0.0f, 0.3f);
      component.Owner = turretAreaAttack.health;
    }
    yield return (object) new WaitForSeconds(0.2f);
    turretAreaAttack.Shooting = false;
    turretAreaAttack.ShootDelay = 2f;
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
    this.StartCoroutine((IEnumerator) this.ShowWarning());
    ++this.ShootDelay;
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

  private IEnumerator ShowWarning()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTurretAreaAttack turretAreaAttack = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      turretAreaAttack.Warning.gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    turretAreaAttack.Warning.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/warning", turretAreaAttack.transform.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) turretAreaAttack.Warning.YieldForAnimation("warn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }
}
