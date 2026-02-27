// Decompiled with JetBrains decompiler
// Type: EnemyMaggotMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemyMaggotMiniBoss : UnitObject
{
  public List<SimpleSpineFlash> SimpleSpineFlashes;
  public SkeletonAnimation Spine;
  public SkeletonAnimation Warning;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string IdleAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string RoarAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string ShootAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AncitipationShootStraightAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AncitipationShootSpiralAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AncitipationJumpAnimation;
  private ShowHPBar ShowHPBar;
  private float Angle;
  private Vector3 Force;
  public float ArcHeight = 5f;
  private EnemyMaggotMiniBoss.AttackPatterns _ActionPaterrn;
  private bool firstSeen;
  private bool phase2;
  public int NumberOfDives = 3;
  public float RevealMeshRenderer;
  public float HideMeshRenderer = 0.1f;
  public ColliderEvents damageColliderEvents;
  public ParticleSystem AoEParticles;
  public GameObject BulletPrefab;
  public GameObject GrenadeBulletPrefab;
  private int CurrentBulletPattern;
  public List<EnemyMaggotMiniBoss.BulletPattern> BulletPatterns = new List<EnemyMaggotMiniBoss.BulletPattern>();
  private EnemyMaggotMiniBoss.BulletPattern b;
  private GameObject g;
  private GrenadeBullet GrenadeBullet;
  private bool EscapeIfHit;
  public FollowAsTail[] TailPieces;
  public float MoveSpeed = 5f;
  public Vector2 DistanceRange = new Vector2(2f, 4f);
  public Vector2 IdleWaitRange = new Vector2(3f, 5f);
  private float IdleWait;
  private float RandomDirection;
  private Vector3 TargetPosition;
  private bool PathingToPlayer;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  private Health EnemyHealth;

  public override void OnEnable()
  {
    base.OnEnable();
    if ((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null)
    {
      this.damageColliderEvents.OnTriggerEnterEvent += new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
      this.damageColliderEvents.SetActive(false);
    }
    this.DisableForces = true;
    this.ShowHPBar = this.GetComponent<ShowHPBar>();
    this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
    foreach (FollowAsTail tailPiece in this.TailPieces)
      tailPiece.ForcePosition(Vector3.up);
    this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 0.0f);
    this.StartCoroutine((IEnumerator) this.ActiveRoutine());
  }

  public override void OnDisable()
  {
    base.OnDisable();
    if (!((UnityEngine.Object) this.damageColliderEvents != (UnityEngine.Object) null))
      return;
    this.damageColliderEvents.OnTriggerEnterEvent -= new ColliderEvents.TriggerEvent(this.OnDamageTriggerEnter);
    this.damageColliderEvents.SetActive(false);
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.EscapeIfHit = false;
    if (AttackType != Health.AttackTypes.NoKnockBack)
      this.StartCoroutine((IEnumerator) this.ApplyForceRoutine(Attacker));
    foreach (SimpleSpineFlash simpleSpineFlash in this.SimpleSpineFlashes)
      simpleSpineFlash.FlashFillRed();
  }

  private IEnumerator DelayActiveRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyMaggotMiniBoss enemyMaggotMiniBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      enemyMaggotMiniBoss.StartCoroutine((IEnumerator) enemyMaggotMiniBoss.ActiveRoutine());
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForSeconds(0.5f);
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  private IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyMaggotMiniBoss enemyMaggotMiniBoss = this;
    enemyMaggotMiniBoss.Angle = Utils.GetAngle(Attacker.transform.position, enemyMaggotMiniBoss.transform.position) * ((float) Math.PI / 180f);
    enemyMaggotMiniBoss.Force = (Vector3) new Vector2(50f * Mathf.Cos(enemyMaggotMiniBoss.Angle), 50f * Mathf.Sin(enemyMaggotMiniBoss.Angle));
    enemyMaggotMiniBoss.rb.AddForce((Vector2) enemyMaggotMiniBoss.Force);
    yield return (object) new WaitForSeconds(0.5f);
    if (enemyMaggotMiniBoss.state.CURRENT_STATE == StateMachine.State.Idle)
      enemyMaggotMiniBoss.IdleWait = 0.0f;
  }

  private EnemyMaggotMiniBoss.AttackPatterns ActionPaterrn
  {
    get => this._ActionPaterrn;
    set
    {
      this._ActionPaterrn = (EnemyMaggotMiniBoss.AttackPatterns) Mathf.Repeat((float) value, 2f);
    }
  }

  private IEnumerator ActiveRoutine()
  {
    EnemyMaggotMiniBoss enemyMaggotMiniBoss = this;
    if (enemyMaggotMiniBoss.gameObject.activeInHierarchy)
    {
      if (!enemyMaggotMiniBoss.firstSeen)
      {
        yield return (object) enemyMaggotMiniBoss.Warning.YieldForAnimation("warn");
        enemyMaggotMiniBoss.firstSeen = true;
      }
      enemyMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
      switch (enemyMaggotMiniBoss.ActionPaterrn)
      {
        case EnemyMaggotMiniBoss.AttackPatterns.Dive:
          enemyMaggotMiniBoss.StartCoroutine((IEnumerator) enemyMaggotMiniBoss.DiveMoveRoutine());
          break;
        case EnemyMaggotMiniBoss.AttackPatterns.Shoot:
          enemyMaggotMiniBoss.StartCoroutine((IEnumerator) enemyMaggotMiniBoss.ShootRoutine());
          break;
      }
      ++enemyMaggotMiniBoss.ActionPaterrn;
    }
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.health.HP >= (double) this.health.totalHP / 3.0 || this.phase2)
      return;
    this.NumberOfDives = 4;
    this.MoveSpeed *= 1.35f;
    this.phase2 = true;
  }

  private IEnumerator DiveMoveRoutine()
  {
    EnemyMaggotMiniBoss enemyMaggotMiniBoss = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm_large/warning", enemyMaggotMiniBoss.transform.position);
    int i = -1;
    while (++i < enemyMaggotMiniBoss.NumberOfDives)
    {
      if (enemyMaggotMiniBoss.GetNewTargetPosition())
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_jump", enemyMaggotMiniBoss.transform.position);
        enemyMaggotMiniBoss.health.enabled = false;
        enemyMaggotMiniBoss.Spine.AnimationState.SetAnimation(0, "jump", false);
        Vector3 StartPosition = enemyMaggotMiniBoss.transform.position;
        float Progress = 0.0f;
        float Duration = Vector3.Distance(StartPosition, enemyMaggotMiniBoss.TargetPosition) / enemyMaggotMiniBoss.MoveSpeed;
        Vector3 Curve = StartPosition + (enemyMaggotMiniBoss.TargetPosition - StartPosition) / 2f + Vector3.back * enemyMaggotMiniBoss.ArcHeight;
        while ((double) (Progress += Time.deltaTime) < (double) Duration)
        {
          Vector3 a = Vector3.Lerp(StartPosition, Curve, Progress / Duration);
          Vector3 b = Vector3.Lerp(Curve, enemyMaggotMiniBoss.TargetPosition, Progress / Duration);
          enemyMaggotMiniBoss.transform.position = Vector3.Lerp(a, b, Progress / Duration);
          yield return (object) null;
        }
        enemyMaggotMiniBoss.TargetPosition.z = 0.0f;
        enemyMaggotMiniBoss.transform.position = enemyMaggotMiniBoss.TargetPosition;
        enemyMaggotMiniBoss.Spine.transform.localPosition = Vector3.zero;
        enemyMaggotMiniBoss.Spine.AnimationState.SetAnimation(0, enemyMaggotMiniBoss.LandAnimation, false);
        enemyMaggotMiniBoss.Spine.AnimationState.AddAnimation(0, enemyMaggotMiniBoss.IdleAnimation, true, 0.0f);
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        enemyMaggotMiniBoss.AoEParticles.Play();
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_land", enemyMaggotMiniBoss.transform.position);
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_boss/patrol_boss_fire_aoe", enemyMaggotMiniBoss.transform.position);
        enemyMaggotMiniBoss.damageColliderEvents.SetActive(true);
        enemyMaggotMiniBoss.health.enabled = true;
        enemyMaggotMiniBoss.transform.DOMove(enemyMaggotMiniBoss.transform.position + Vector3.down * 0.5f, 0.2f);
        yield return (object) new WaitForSeconds(0.3f);
        enemyMaggotMiniBoss.damageColliderEvents.SetActive(false);
        if (i < enemyMaggotMiniBoss.NumberOfDives - 1)
          yield return (object) new WaitForSeconds(0.2f);
        StartPosition = new Vector3();
        Curve = new Vector3();
      }
    }
    yield return (object) new WaitForEndOfFrame();
    enemyMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyMaggotMiniBoss.StartCoroutine((IEnumerator) enemyMaggotMiniBoss.ActiveRoutine());
  }

  private void ForceTailDirection(Vector3 Direction)
  {
    Debug.Log((object) ("TAILS : " + (object) this.TailPieces.Length));
    foreach (FollowAsTail tailPiece in this.TailPieces)
    {
      Debug.Log((object) tailPiece.name);
      tailPiece.ForcePosition(Direction);
    }
  }

  private IEnumerator ShootRoutine()
  {
    EnemyMaggotMiniBoss enemyMaggotMiniBoss1 = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm_large/warning", enemyMaggotMiniBoss1.transform.position);
    if (enemyMaggotMiniBoss1.CurrentBulletPattern == 0)
      yield return (object) enemyMaggotMiniBoss1.Spine.YieldForAnimation(enemyMaggotMiniBoss1.AncitipationShootSpiralAnimation);
    else if (enemyMaggotMiniBoss1.CurrentBulletPattern == 1)
    {
      yield return (object) enemyMaggotMiniBoss1.Spine.YieldForAnimation(enemyMaggotMiniBoss1.AncitipationShootStraightAnimation);
    }
    else
    {
      enemyMaggotMiniBoss1.Spine.AnimationState.SetAnimation(0, "attack-charge", true);
      yield return (object) new WaitForSeconds(1f);
    }
    foreach (FollowAsTail tailPiece in enemyMaggotMiniBoss1.TailPieces)
      tailPiece.ForcePosition(Vector3.up);
    enemyMaggotMiniBoss1.b = enemyMaggotMiniBoss1.BulletPatterns[enemyMaggotMiniBoss1.CurrentBulletPattern];
    EnemyMaggotMiniBoss enemyMaggotMiniBoss2 = enemyMaggotMiniBoss1;
    int num1 = enemyMaggotMiniBoss1.CurrentBulletPattern + 1;
    int num2 = num1;
    enemyMaggotMiniBoss2.CurrentBulletPattern = num2;
    if (num1 >= enemyMaggotMiniBoss1.BulletPatterns.Count)
      enemyMaggotMiniBoss1.CurrentBulletPattern = 0;
    float Angle = Utils.GetAngle(enemyMaggotMiniBoss1.transform.position, (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? PlayerFarming.Instance.transform.position : Vector3.zero) - enemyMaggotMiniBoss1.b.Arc / 2f;
    float Wave = 0.0f;
    int i = -1;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    while (++i < enemyMaggotMiniBoss1.b.BulletsToShoot)
    {
      if (enemyMaggotMiniBoss1.b.UpdateAngle)
        Angle = Utils.GetAngle(enemyMaggotMiniBoss1.transform.position, (UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null ? PlayerFarming.Instance.transform.position : Vector3.zero) - enemyMaggotMiniBoss1.b.Arc / 2f;
      if (enemyMaggotMiniBoss1.b.IsGrenade)
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", enemyMaggotMiniBoss1.transform.position);
        enemyMaggotMiniBoss1.GrenadeBullet = ObjectPool.Spawn(enemyMaggotMiniBoss1.GrenadeBulletPrefab, enemyMaggotMiniBoss1.transform.parent, enemyMaggotMiniBoss1.transform.position, Quaternion.identity).GetComponent<GrenadeBullet>();
        enemyMaggotMiniBoss1.GrenadeBullet.Play(-1f, (float) ((double) Angle + (double) enemyMaggotMiniBoss1.b.WaveSize * (double) Mathf.Cos(Wave += enemyMaggotMiniBoss1.b.WaveSpeed) + (double) enemyMaggotMiniBoss1.b.Arc / (double) enemyMaggotMiniBoss1.b.BulletsToShoot * (double) i), UnityEngine.Random.Range(enemyMaggotMiniBoss1.b.RandomRange.x, enemyMaggotMiniBoss1.b.RandomRange.y), UnityEngine.Random.Range(enemyMaggotMiniBoss1.b.GravSpeed - 0.5f, enemyMaggotMiniBoss1.b.GravSpeed + 0.5f));
      }
      else
      {
        AudioManager.Instance.PlayOneShot("event:/enemy/shoot_magicenergy", enemyMaggotMiniBoss1.transform.position);
        enemyMaggotMiniBoss1.Spine.AnimationState.SetAnimation(0, enemyMaggotMiniBoss1.ShootAnimation, false);
        enemyMaggotMiniBoss1.Spine.AnimationState.AddAnimation(0, enemyMaggotMiniBoss1.IdleAnimation, true, 0.0f);
        Projectile component = ObjectPool.Spawn(enemyMaggotMiniBoss1.BulletPrefab, enemyMaggotMiniBoss1.transform.parent).GetComponent<Projectile>();
        component.transform.position = enemyMaggotMiniBoss1.transform.position;
        component.Angle = (float) ((double) Angle + (double) enemyMaggotMiniBoss1.b.WaveSize * (double) Mathf.Cos(Wave += enemyMaggotMiniBoss1.b.WaveSpeed) + (double) enemyMaggotMiniBoss1.b.Arc / (double) enemyMaggotMiniBoss1.b.BulletsToShoot * (double) i);
        component.team = enemyMaggotMiniBoss1.health.team;
        component.Speed = enemyMaggotMiniBoss1.b.Speed;
        component.LifeTime = 4f + UnityEngine.Random.Range(0.0f, 0.3f);
        component.Owner = enemyMaggotMiniBoss1.health;
      }
      if (enemyMaggotMiniBoss1.CurrentBulletPattern == 0)
        yield return (object) new WaitForSeconds(enemyMaggotMiniBoss1.b.DelayBetweenShots);
    }
    enemyMaggotMiniBoss1.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyMaggotMiniBoss1.EscapeIfHit = true;
    yield return (object) new WaitForSeconds(0.5f);
    foreach (FollowAsTail tailPiece in enemyMaggotMiniBoss1.TailPieces)
      tailPiece.ForcePosition(Vector3.up);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    enemyMaggotMiniBoss1.Spine.AnimationState.SetAnimation(0, enemyMaggotMiniBoss1.RoarAnimation, false);
    AudioManager.Instance.PlayOneShot("event:/enemy/patrol_boss/patrol_boss_roar", enemyMaggotMiniBoss1.transform.position);
    enemyMaggotMiniBoss1.Spine.AnimationState.AddAnimation(0, enemyMaggotMiniBoss1.IdleAnimation, true, 0.0f);
    yield return (object) new WaitForSeconds(2f);
    enemyMaggotMiniBoss1.EscapeIfHit = false;
    enemyMaggotMiniBoss1.StartCoroutine((IEnumerator) enemyMaggotMiniBoss1.ActiveRoutine());
  }

  private IEnumerator MoveRoutine()
  {
    EnemyMaggotMiniBoss enemyMaggotMiniBoss = this;
    enemyMaggotMiniBoss.ShowHPBar.Hide();
    enemyMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Fleeing;
    enemyMaggotMiniBoss.health.enabled = false;
    Vector3 StartPosition = enemyMaggotMiniBoss.transform.position;
    float Progress = 0.0f;
    float Duration = Vector3.Distance(StartPosition, enemyMaggotMiniBoss.TargetPosition) / enemyMaggotMiniBoss.MoveSpeed;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      enemyMaggotMiniBoss.transform.position = Vector3.Lerp(StartPosition, enemyMaggotMiniBoss.TargetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    enemyMaggotMiniBoss.transform.position = enemyMaggotMiniBoss.TargetPosition;
    enemyMaggotMiniBoss.health.enabled = true;
    yield return (object) new WaitForSeconds(0.5f);
    enemyMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyMaggotMiniBoss.StartCoroutine((IEnumerator) enemyMaggotMiniBoss.ActiveRoutine());
  }

  public bool GetNewTargetPosition()
  {
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
    {
      float num = Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position);
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (double) num < 10.0 && (double) num >= 4.0)
      {
        this.TargetPosition = PlayerFarming.Instance.transform.position;
        if ((double) Mathf.Abs(this.TargetPosition.x) > 6.5 || (double) Mathf.Abs(this.TargetPosition.y) > 4.0)
          this.TargetPosition = new Vector3(UnityEngine.Random.Range(-6.5f, 6.5f), UnityEngine.Random.Range(-4f, 4f), 0.0f);
        return true;
      }
    }
    float num1 = 100f;
    if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) < 8.0 && this.CheckLineOfSight(PlayerFarming.Instance.transform.position, 8f))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, PlayerFarming.Instance.transform.position) * ((float) Math.PI / 180f);
    }
    else
      this.RandomDirection = (float) UnityEngine.Random.Range(0, 360);
    while ((double) --num1 > 0.0)
    {
      float distance = UnityEngine.Random.Range(this.DistanceRange.x, this.DistanceRange.y);
      if (!this.PathingToPlayer)
        this.RandomDirection += (float) UnityEngine.Random.Range(-90, 90) * ((float) Math.PI / 180f);
      float radius = 0.2f;
      Vector3 vector3 = this.transform.position + new Vector3(distance * Mathf.Cos(this.RandomDirection), distance * Mathf.Sin(this.RandomDirection));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.transform.position, radius, (Vector2) Vector3.Normalize(vector3 - this.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if (this.ShowDebug)
        {
          this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y) + Vector3.Normalize(this.transform.position - vector3) * this.CircleCastOffset);
          this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.RandomDirection = 180f - this.RandomDirection;
      }
      else
      {
        if (this.ShowDebug)
        {
          this.EndPoints.Add(new Vector3(vector3.x, vector3.y));
          this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.IdleWait = UnityEngine.Random.Range(this.IdleWaitRange.x, this.IdleWaitRange.y);
        this.TargetPosition = vector3;
        if ((double) Mathf.Abs(this.TargetPosition.x) > 6.5 || (double) Mathf.Abs(this.TargetPosition.y) > 4.0)
          this.TargetPosition = new Vector3(UnityEngine.Random.Range(-6.5f, 6.5f), UnityEngine.Random.Range(-4f, 4f), 0.0f);
        return true;
      }
    }
    return false;
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, (float) this.VisionRange, Color.yellow);
    int index1 = -1;
    while (++index1 < this.Points.Count)
    {
      Utils.DrawCircleXY(this.PointsLink[index1], 0.5f, Color.blue);
      Utils.DrawCircleXY(this.Points[index1], this.CircleCastRadius, Color.blue);
      Utils.DrawLine(this.Points[index1], this.PointsLink[index1], Color.blue);
    }
    int index2 = -1;
    while (++index2 < this.EndPoints.Count)
    {
      Utils.DrawCircleXY(this.EndPointsLink[index2], 0.5f, Color.red);
      Utils.DrawCircleXY(this.EndPoints[index2], this.CircleCastRadius, Color.red);
      Utils.DrawLine(this.EndPointsLink[index2], this.EndPoints[index2], Color.red);
    }
  }

  private void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  public enum AttackPatterns
  {
    Dive,
    Shoot,
    Total,
  }

  [Serializable]
  public class BulletPattern
  {
    public bool IsGrenade;
    public Vector2 RandomRange = new Vector2(3f, 5f);
    public float GravSpeed = -8f;
    public int BulletsToShoot = 10;
    public float WaveSize = 20f;
    public float WaveSpeed = 5f;
    public float Arc;
    public float DelayBetweenShots = 0.1f;
    public float Speed = 5f;
    public bool UpdateAngle;
  }
}
