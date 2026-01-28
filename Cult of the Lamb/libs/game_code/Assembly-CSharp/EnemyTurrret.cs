// Decompiled with JetBrains decompiler
// Type: EnemyTurrret
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemyTurrret : UnitObject
{
  public SkeletonAnimation Spine;
  public SkeletonAnimation Warning;
  public SimpleSpineFlash SimpleSpineFlash;
  public SpriteRenderer Aiming;
  public float LookAngle;
  public float ShootDelay;
  public int ShotsToFire = 5;
  public float DetectEnemyRange = 8f;
  public GameObject Arrow;
  public bool Shooting;
  public GameObject TargetObject;
  public Health EnemyHealth;
  public bool playedAnticipation;
  public bool LockAngleTo90Degrees;
  public float ShootDelayTime = 1f;
  public float TimeBetweenShooting = 1f;

  public void Start()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnHit += new Health.HitAction(((UnitObject) this).OnHit);
    this.health.OnDie += new Health.DieAction(((UnitObject) this).OnDie);
    this.Aiming.DOFade(0.0f, 0.0f);
    this.Spine.AnimationState.SetAnimation(0, "closed", true);
    this.Warning.gameObject.SetActive(false);
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.Shooting = false;
    this.ShootDelay = Health.team2.Count > 1 ? Random.Range(0.0f, 1f) : 0.0f;
    this.health.OnAddCharm += new Health.StasisEvent(this.GetNewTarget);
    this.health.OnStasisCleared += new Health.StasisEvent(this.GetNewTarget);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.health.OnAddCharm -= new Health.StasisEvent(this.GetNewTarget);
    this.health.OnStasisCleared -= new Health.StasisEvent(this.GetNewTarget);
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
    if (!CheatConsole.HidingUI)
      this.Aiming.enabled = false;
    else
      this.Aiming.enabled = true;
    if ((Object) this.TargetObject == (Object) null)
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
      if (!CoopManager.CoopActive || (double) Vector3.Distance(this.TargetObject.transform.position, this.transform.position) <= (double) this.VisionRange * 0.33000001311302185)
        return;
      Health closestTarget = this.GetClosestTarget();
      if (!((Object) closestTarget != (Object) null) || !((Object) this.TargetObject != (Object) closestTarget.gameObject))
        return;
      this.TargetObject = closestTarget.gameObject;
    }
    else
    {
      if ((double) Vector3.Distance(this.TargetObject.transform.position, this.transform.position) <= (double) this.VisionRange)
        return;
      this.TargetObject = (GameObject) null;
      this.Spine.AnimationState.SetAnimation(0, "closed", true);
    }
  }

  public IEnumerator ShootArrowRoutine()
  {
    EnemyTurrret enemyTurrret = this;
    enemyTurrret.Shooting = true;
    int i = enemyTurrret.ShotsToFire;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/warning", enemyTurrret.transform.position);
    enemyTurrret.Spine.AnimationState.SetAnimation(0, "anticipation", false);
    enemyTurrret.Spine.AnimationState.AddAnimation(0, "shoot", false, 0.0f);
    enemyTurrret.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTurrret.Spine.timeScale) < 0.82999998331069946 - (double) enemyTurrret.TimeBetweenShooting)
      yield return (object) null;
    while (--i >= 0)
    {
      enemyTurrret.Aiming.DOFade(1f, 0.33f);
      float Progress = 0.0f;
      while ((double) (Progress += Time.deltaTime * enemyTurrret.Spine.timeScale) < (double) enemyTurrret.TimeBetweenShooting)
      {
        if ((Object) enemyTurrret.TargetObject != (Object) null)
          enemyTurrret.LookAngle = Utils.GetAngle(enemyTurrret.transform.position, enemyTurrret.TargetObject.transform.position);
        enemyTurrret.Aiming.transform.eulerAngles = new Vector3(0.0f, 0.0f, enemyTurrret.LookAngle);
        yield return (object) null;
      }
      AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", enemyTurrret.transform.position);
      AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/attack", enemyTurrret.transform.position);
      enemyTurrret.Aiming.DOFade(0.0f, 0.33f);
      CameraManager.shakeCamera(0.2f, enemyTurrret.LookAngle);
      Projectile component = ObjectPool.Spawn(enemyTurrret.Arrow, enemyTurrret.transform.parent).GetComponent<Projectile>();
      component.transform.position = enemyTurrret.transform.position;
      component.Angle = enemyTurrret.LockAngleTo90Degrees ? (float) Mathf.FloorToInt(enemyTurrret.LookAngle / 90f) * 90f : enemyTurrret.LookAngle;
      component.team = enemyTurrret.health.team;
      component.Speed = 6f;
      component.Owner = enemyTurrret.health;
    }
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemyTurrret.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    enemyTurrret.Shooting = false;
    enemyTurrret.ShootDelay = enemyTurrret.ShootDelayTime;
  }

  public void GetNewTarget()
  {
    Health closestTarget = this.GetClosestTarget();
    if (!((Object) closestTarget != (Object) null) || (double) Vector3.Distance(closestTarget.transform.position, this.transform.position) > (double) this.VisionRange)
      return;
    this.TargetObject = closestTarget.gameObject;
    this.EnemyHealth = closestTarget;
    this.EnemyHealth.attackers.Add(this.gameObject);
    this.Spine.AnimationState.SetAnimation(0, "idle", true);
    this.StartCoroutine((IEnumerator) this.ShowWarning());
    ++this.ShootDelay;
  }

  public IEnumerator ShowWarning()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyTurrret enemyTurrret = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyTurrret.Warning.gameObject.SetActive(false);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    enemyTurrret.Warning.gameObject.SetActive(true);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm/warning", enemyTurrret.transform.position);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) enemyTurrret.Warning.YieldForAnimation("warn");
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.DetectEnemyRange, Color.red);
  }
}
