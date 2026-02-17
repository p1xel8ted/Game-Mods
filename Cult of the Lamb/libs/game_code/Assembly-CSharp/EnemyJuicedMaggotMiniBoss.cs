// Decompiled with JetBrains decompiler
// Type: EnemyJuicedMaggotMiniBoss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyJuicedMaggotMiniBoss : UnitObject
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
  public string ShootAnimationLong;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string LandAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AncitipationShootStraightAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AncitipationShootSpiralAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AncitipationJumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AnticipationRingAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string AnticipationBounceyAnimation;
  public ShowHPBar ShowHPBar;
  public float Angle;
  public Vector3 Force;
  public float ArcHeight = 5f;
  public EnemyJuicedMaggotMiniBoss.AttackPatterns _ActionPaterrn;
  public bool firstSeen;
  public bool phase2;
  public int NumberOfDives = 3;
  public float RevealMeshRenderer;
  public float HideMeshRenderer = 0.1f;
  public ColliderEvents damageColliderEvents;
  public ParticleSystem AoEParticles;
  public ProjectilePattern projectilePatternOnLand;
  public GameObject BulletPrefab;
  public GameObject GrenadeBulletPrefab;
  public int CurrentBulletPattern;
  public List<ProjectilePatternBase> BulletPatterns = new List<ProjectilePatternBase>();
  public ProjectilePatternBase b;
  public GameObject g;
  public GrenadeBullet GrenadeBullet;
  public bool EscapeIfHit;
  public FollowAsTail[] TailPieces;
  public float MoveSpeed = 5f;
  public Vector2 DistanceRange = new Vector2(2f, 4f);
  public Vector2 IdleWaitRange = new Vector2(3f, 5f);
  public float IdleWait;
  public float RandomDirection;
  public Vector3 TargetPosition;
  public bool PathingToPlayer;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();
  public Health EnemyHealth;

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
    {
      tailPiece.transform.parent = this.transform.parent;
      tailPiece.ForcePosition(Vector3.up);
    }
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

  public IEnumerator DelayActiveRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJuicedMaggotMiniBoss juicedMaggotMiniBoss = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.ActiveRoutine());
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

  public IEnumerator ApplyForceRoutine(GameObject Attacker)
  {
    EnemyJuicedMaggotMiniBoss juicedMaggotMiniBoss = this;
    juicedMaggotMiniBoss.Angle = Utils.GetAngle(Attacker.transform.position, juicedMaggotMiniBoss.transform.position) * ((float) Math.PI / 180f);
    juicedMaggotMiniBoss.Force = (Vector3) new Vector2(50f * Mathf.Cos(juicedMaggotMiniBoss.Angle), 50f * Mathf.Sin(juicedMaggotMiniBoss.Angle));
    juicedMaggotMiniBoss.rb.AddForce((Vector2) juicedMaggotMiniBoss.Force);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
    if (juicedMaggotMiniBoss.state.CURRENT_STATE == StateMachine.State.Idle)
      juicedMaggotMiniBoss.IdleWait = 0.0f;
  }

  public EnemyJuicedMaggotMiniBoss.AttackPatterns ActionPaterrn
  {
    get => this._ActionPaterrn;
    set
    {
      this._ActionPaterrn = (EnemyJuicedMaggotMiniBoss.AttackPatterns) Utils.Repeat((float) value, 2f);
    }
  }

  public IEnumerator ActiveRoutine()
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    EnemyJuicedMaggotMiniBoss juicedMaggotMiniBoss = this;
    if (num != 0)
      return false;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    if (!juicedMaggotMiniBoss.gameObject.activeInHierarchy)
      return false;
    if (!juicedMaggotMiniBoss.firstSeen)
      juicedMaggotMiniBoss.firstSeen = true;
    juicedMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    switch (juicedMaggotMiniBoss.ActionPaterrn)
    {
      case EnemyJuicedMaggotMiniBoss.AttackPatterns.Dive:
        juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.DiveMoveRoutine());
        break;
      case EnemyJuicedMaggotMiniBoss.AttackPatterns.Shoot:
        juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.ShootRoutine());
        break;
    }
    ++juicedMaggotMiniBoss.ActionPaterrn;
    return false;
  }

  public override void Update()
  {
    base.Update();
    if ((double) this.health.HP >= (double) this.health.totalHP / 2.0 || this.phase2)
      return;
    this.NumberOfDives = 6;
    this.MoveSpeed *= 1.35f;
    this.phase2 = true;
    foreach (ProjectilePatternBase bulletPattern in this.BulletPatterns)
    {
      if (bulletPattern is ProjectilePattern projectilePattern)
      {
        for (int index = 0; index < projectilePattern.Waves.Length; ++index)
          projectilePattern.Waves[index].Speed *= 1.5f;
      }
      else if (bulletPattern is ProjectilePatternBeam projectilePatternBeam)
      {
        for (int index = 0; index < projectilePatternBeam.BulletWaves.Length; ++index)
          projectilePatternBeam.BulletWaves[index].Speed *= 1.5f;
      }
    }
  }

  public IEnumerator DiveMoveRoutine()
  {
    EnemyJuicedMaggotMiniBoss juicedMaggotMiniBoss = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm_large/warning", juicedMaggotMiniBoss.transform.position);
    int i = -1;
    while (++i < UnityEngine.Random.Range(juicedMaggotMiniBoss.NumberOfDives / 2, juicedMaggotMiniBoss.NumberOfDives))
    {
      if (juicedMaggotMiniBoss.GetNewTargetPosition())
      {
        while ((double) juicedMaggotMiniBoss.Spine.timeScale == 9.9999997473787516E-05)
          yield return (object) null;
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_jump", juicedMaggotMiniBoss.transform.position);
        juicedMaggotMiniBoss.health.untouchable = true;
        juicedMaggotMiniBoss.Spine.AnimationState.SetAnimation(0, "jump", false);
        Vector3 StartPosition = juicedMaggotMiniBoss.transform.position;
        float Progress = 0.0f;
        float Duration = Vector3.Distance(StartPosition, juicedMaggotMiniBoss.TargetPosition) / juicedMaggotMiniBoss.MoveSpeed;
        Vector3 Curve = StartPosition + (juicedMaggotMiniBoss.TargetPosition - StartPosition) / 2f + Vector3.back * juicedMaggotMiniBoss.ArcHeight;
        while ((double) (Progress += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < (double) Duration)
        {
          Vector3 a = Vector3.Lerp(StartPosition, Curve, Progress / Duration);
          Vector3 b = Vector3.Lerp(Curve, juicedMaggotMiniBoss.TargetPosition, Progress / Duration);
          juicedMaggotMiniBoss.transform.position = Vector3.Lerp(a, b, Progress / Duration);
          yield return (object) null;
        }
        juicedMaggotMiniBoss.TargetPosition.z = 0.0f;
        juicedMaggotMiniBoss.transform.position = juicedMaggotMiniBoss.TargetPosition;
        juicedMaggotMiniBoss.Spine.transform.localPosition = Vector3.zero;
        if ((UnityEngine.Object) juicedMaggotMiniBoss.projectilePatternOnLand != (UnityEngine.Object) null)
          juicedMaggotMiniBoss.projectilePatternOnLand.Shoot();
        juicedMaggotMiniBoss.Spine.AnimationState.SetAnimation(0, juicedMaggotMiniBoss.LandAnimation, false);
        juicedMaggotMiniBoss.Spine.AnimationState.AddAnimation(0, juicedMaggotMiniBoss.IdleAnimation, true, 0.0f);
        CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
        juicedMaggotMiniBoss.AoEParticles.Play();
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_worm/patrol_worm_land", juicedMaggotMiniBoss.transform.position);
        AudioManager.Instance.PlayOneShot("event:/enemy/patrol_boss/patrol_boss_fire_aoe", juicedMaggotMiniBoss.transform.position);
        juicedMaggotMiniBoss.damageColliderEvents.SetActive(true);
        juicedMaggotMiniBoss.health.untouchable = false;
        if (juicedMaggotMiniBoss.phase2)
          Projectile.CreateProjectiles(7, juicedMaggotMiniBoss.health, juicedMaggotMiniBoss.transform.position, 14f, angleOffset: (float) UnityEngine.Random.Range(0, 360));
        juicedMaggotMiniBoss.transform.DOMove(juicedMaggotMiniBoss.transform.position + Vector3.down * 0.5f, 0.2f);
        float time = 0.0f;
        while ((double) (time += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < 0.30000001192092896)
          yield return (object) null;
        juicedMaggotMiniBoss.damageColliderEvents.SetActive(false);
        if (i < juicedMaggotMiniBoss.NumberOfDives - 1)
        {
          time = 0.0f;
          while ((double) (time += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < 0.20000000298023224)
            yield return (object) null;
        }
        StartPosition = new Vector3();
        Curve = new Vector3();
      }
    }
    yield return (object) new WaitForEndOfFrame();
    juicedMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.ActiveRoutine());
  }

  public void ForceTailDirection(Vector3 Direction)
  {
    Debug.Log((object) ("TAILS : " + this.TailPieces.Length.ToString()));
    foreach (FollowAsTail tailPiece in this.TailPieces)
    {
      Debug.Log((object) tailPiece.name);
      tailPiece.ForcePosition(Direction);
    }
  }

  public IEnumerator ShootRoutine()
  {
    EnemyJuicedMaggotMiniBoss juicedMaggotMiniBoss = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/worm_large/warning", juicedMaggotMiniBoss.transform.position);
    int index = UnityEngine.Random.Range(0, juicedMaggotMiniBoss.BulletPatterns.Count);
    juicedMaggotMiniBoss.b = juicedMaggotMiniBoss.BulletPatterns[index];
    switch (index)
    {
      case 1:
        yield return (object) juicedMaggotMiniBoss.Spine.YieldForAnimation(juicedMaggotMiniBoss.AnticipationRingAnimation);
        break;
      case 2:
        yield return (object) juicedMaggotMiniBoss.Spine.YieldForAnimation(juicedMaggotMiniBoss.AnticipationBounceyAnimation);
        break;
      default:
        yield return (object) juicedMaggotMiniBoss.Spine.YieldForAnimation(juicedMaggotMiniBoss.AncitipationShootStraightAnimation);
        break;
    }
    foreach (FollowAsTail tailPiece in juicedMaggotMiniBoss.TailPieces)
      tailPiece.ForcePosition(Vector3.up);
    bool doRoar = true;
    if (juicedMaggotMiniBoss.b is ProjectilePattern)
    {
      ((ProjectilePattern) juicedMaggotMiniBoss.b).OnProjectileWaveShot += new ProjectilePattern.ProjectileWaveEvent(juicedMaggotMiniBoss.\u003CShootRoutine\u003Eb__49_0);
    }
    else
    {
      CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 1.5f);
      juicedMaggotMiniBoss.Spine.AnimationState.SetAnimation(0, juicedMaggotMiniBoss.ShootAnimationLong, false);
      doRoar = false;
    }
    yield return (object) juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.b.ShootIE());
    juicedMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    juicedMaggotMiniBoss.EscapeIfHit = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
    foreach (FollowAsTail tailPiece in juicedMaggotMiniBoss.TailPieces)
      tailPiece.ForcePosition(Vector3.up);
    if (doRoar)
    {
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
      juicedMaggotMiniBoss.Spine.AnimationState.SetAnimation(0, juicedMaggotMiniBoss.RoarAnimation, false);
    }
    AudioManager.Instance.PlayOneShot("event:/enemy/patrol_boss/patrol_boss_roar", juicedMaggotMiniBoss.transform.position);
    juicedMaggotMiniBoss.Spine.AnimationState.AddAnimation(0, juicedMaggotMiniBoss.IdleAnimation, true, 0.0f);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < 2.0)
      yield return (object) null;
    juicedMaggotMiniBoss.EscapeIfHit = false;
    juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.ActiveRoutine());
  }

  public IEnumerator MoveRoutine()
  {
    EnemyJuicedMaggotMiniBoss juicedMaggotMiniBoss = this;
    juicedMaggotMiniBoss.ShowHPBar.Hide();
    juicedMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Fleeing;
    juicedMaggotMiniBoss.health.enabled = false;
    Vector3 StartPosition = juicedMaggotMiniBoss.transform.position;
    float Progress = 0.0f;
    float Duration = Vector3.Distance(StartPosition, juicedMaggotMiniBoss.TargetPosition) / juicedMaggotMiniBoss.MoveSpeed;
    while ((double) (Progress += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < (double) Duration)
    {
      juicedMaggotMiniBoss.transform.position = Vector3.Lerp(StartPosition, juicedMaggotMiniBoss.TargetPosition, Mathf.SmoothStep(0.0f, 1f, Progress / Duration));
      yield return (object) null;
    }
    juicedMaggotMiniBoss.transform.position = juicedMaggotMiniBoss.TargetPosition;
    juicedMaggotMiniBoss.health.enabled = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * juicedMaggotMiniBoss.Spine.timeScale) < 0.5)
      yield return (object) null;
    juicedMaggotMiniBoss.state.CURRENT_STATE = StateMachine.State.Idle;
    juicedMaggotMiniBoss.StartCoroutine((IEnumerator) juicedMaggotMiniBoss.ActiveRoutine());
  }

  public bool GetNewTargetPosition()
  {
    PlayerFarming closestPlayer = PlayerFarming.FindClosestPlayer(this.transform.position);
    if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null)
    {
      float num = Vector3.Distance(this.transform.position, closestPlayer.transform.position);
      if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null && (double) num < 10.0 && (double) num >= 4.0)
      {
        this.TargetPosition = closestPlayer.transform.position;
        if ((double) Mathf.Abs(this.TargetPosition.x) > 6.5 || (double) Mathf.Abs(this.TargetPosition.y) > 4.0)
          this.TargetPosition = new Vector3(UnityEngine.Random.Range(-6.5f, 6.5f), UnityEngine.Random.Range(-4f, 4f), 0.0f);
        return true;
      }
    }
    float num1 = 100f;
    if ((UnityEngine.Object) closestPlayer != (UnityEngine.Object) null && (double) Vector3.Distance(this.transform.position, closestPlayer.transform.position) < 8.0 && this.CheckLineOfSightOnTarget(closestPlayer.gameObject, closestPlayer.transform.position, 8f))
    {
      this.PathingToPlayer = true;
      this.RandomDirection = Utils.GetAngle(this.transform.position, closestPlayer.transform.position) * ((float) Math.PI / 180f);
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

  public void OnDrawGizmos()
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

  public void OnDamageTriggerEnter(Collider2D collider)
  {
    this.EnemyHealth = collider.GetComponent<Health>();
    if (!((UnityEngine.Object) this.EnemyHealth != (UnityEngine.Object) null) || this.EnemyHealth.team == this.health.team)
      return;
    this.EnemyHealth.DealDamage(1f, this.gameObject, Vector3.Lerp(this.transform.position, this.EnemyHealth.transform.position, 0.7f));
  }

  [CompilerGenerated]
  public void \u003CShootRoutine\u003Eb__49_0(ProjectilePattern.BulletWave wave)
  {
    if (string.IsNullOrEmpty(wave.AnimationToPlay))
      return;
    CameraManager.instance.ShakeCameraForDuration(0.3f, 0.4f, 0.2f);
    this.Spine.AnimationState.SetAnimation(0, this.ShootAnimation, false);
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
