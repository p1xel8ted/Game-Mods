// Decompiled with JetBrains decompiler
// Type: EnemySummoner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemySummoner : UnitObject
{
  public bool Summon = true;
  public bool FireBalls = true;
  public bool Mortar = true;
  public bool HealOthers = true;
  public SkeletonAnimation skeletonAnimation;
  public SimpleSpineFlash simpleSpineFlash;
  public GameObject Arrow;
  public SpriteRenderer Shadow;
  public ParticleSystem summonParticles;
  public ParticleSystem teleportEffect;
  public float SeperationRadius = 0.5f;
  private GameObject TargetObject;
  public float Range = 6f;
  public float KnockbackSpeed = 0.2f;
  private CircleCollider2D CircleCollider;
  public AssetReferenceGameObject[] EnemyList;
  [EventRef]
  public string AttackVO = string.Empty;
  [EventRef]
  public string DeathVO = string.Empty;
  [EventRef]
  public string GetHitVO = string.Empty;
  [EventRef]
  public string WarningVO = string.Empty;
  [EventRef]
  public string SummonSfx = "event:/enemy/summon";
  private float StartSpeed = 0.4f;
  private int SummonedCount;
  private float SummonDelay = 1f;
  private float FireBallDelay = 1f;
  private float MortarDelay = 1f;
  private float HealDelay = 1f;
  private float TeleportDelay = 3f;
  private float FleeDelay = 1f;
  private GameObject EnemySpawnerGO;
  public int NumToSpawn;
  private bool Stunned;
  public int NumToShoot = 3;
  private bool Shooting;
  public GameObject projectilePrefab;
  protected const float minBombRange = 2.5f;
  protected const float maxBombRange = 5f;
  public float timeBetweenShots = 0.5f;
  public float bombDuration = 0.75f;
  public int MortarShotsToFire = 2;
  private bool Teleporting;
  private Coroutine cTeleporting;
  public float CircleCastRadius = 0.5f;
  public float CircleCastOffset = 1f;
  public bool ShowDebug;
  public List<Vector3> Points = new List<Vector3>();
  public List<Vector3> PointsLink = new List<Vector3>();
  public List<Vector3> EndPoints = new List<Vector3>();
  public List<Vector3> EndPointsLink = new List<Vector3>();

  private void Start()
  {
    this.SeperateObject = true;
    this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationStateEvent);
    this.CircleCollider = this.GetComponent<CircleCollider2D>();
  }

  private void HandleAnimationStateEvent(TrackEntry trackEntry, Spine.Event e)
  {
    switch (e.Data.Name)
    {
      case "Teleport":
        this.Teleport();
        break;
      case "Fireball":
        if (!string.IsNullOrEmpty(this.AttackVO))
          AudioManager.Instance.PlayOneShot(this.AttackVO, this.transform.position);
        Projectile component = ObjectPool.Spawn(this.Arrow, this.transform.parent).GetComponent<Projectile>();
        component.transform.position = this.transform.position;
        if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
          this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
        component.Angle = Mathf.Round(this.state.facingAngle / 45f) * 45f;
        component.team = this.health.team;
        component.Owner = this.health;
        break;
    }
  }

  public override void OnEnable()
  {
    this.StartCoroutine((IEnumerator) this.WaitForTarget());
    base.OnEnable();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.ClearPaths();
    this.StopAllCoroutines();
  }

  private IEnumerator WaitForTarget()
  {
    EnemySummoner enemySummoner = this;
    while ((UnityEngine.Object) enemySummoner.TargetObject == (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null)
        enemySummoner.TargetObject = PlayerFarming.Instance.gameObject;
      yield return (object) null;
    }
    while ((double) Vector3.Distance(enemySummoner.TargetObject.transform.position, enemySummoner.transform.position) > (double) enemySummoner.Range)
      yield return (object) null;
    enemySummoner.StopAllCoroutines();
    enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind)
  {
    this.simpleSpineFlash.FlashWhite(false);
    if (AttackType == Health.AttackTypes.Projectile)
    {
      if (!this.Stunned)
      {
        if (!string.IsNullOrEmpty(this.GetHitVO))
          AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
        CameraManager.shakeCamera(0.4f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
        GameManager.GetInstance().HitStop();
        BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
        this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
        this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, AttackLocation) * ((float) Math.PI / 180f));
        this.simpleSpineFlash.FlashFillRed();
        this.StopAllCoroutines();
        this.StartCoroutine((IEnumerator) this.DoStunned());
      }
      else
      {
        CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
        GameObject gameObject = BiomeConstants.Instance.HitFX_Blocked.Spawn();
        gameObject.transform.position = AttackLocation + Vector3.back * 0.5f;
        gameObject.transform.rotation = Quaternion.identity;
      }
    }
    else
    {
      if (!this.Stunned)
      {
        int num = this.Shooting ? 1 : 0;
      }
      CameraManager.shakeCamera(0.1f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
      BiomeConstants.Instance.EmitHitVFX(AttackLocation + Vector3.back * 1f, Quaternion.identity.z, "HitFX_Weak");
      this.knockBackVX = -this.KnockbackSpeed * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.knockBackVY = -this.KnockbackSpeed * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
      this.simpleSpineFlash.FlashFillRed();
      this.state.facingAngle = Utils.GetAngle(this.transform.position, Attacker.transform.position);
    }
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Dieing)
      return;
    if (!string.IsNullOrEmpty(this.DeathVO))
      AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
    this.knockBackVX = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Cos(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    this.knockBackVY = (float) (-(double) this.KnockbackSpeed * 1.0) * Mathf.Sin(Utils.GetAngle(this.transform.position, Attacker.transform.position) * ((float) Math.PI / 180f));
    CameraManager.shakeCamera(0.5f, Utils.GetAngle(Attacker.transform.position, this.transform.position));
  }

  private IEnumerator ChasePlayer()
  {
    EnemySummoner enemySummoner = this;
    enemySummoner.state.CURRENT_STATE = StateMachine.State.Idle;
    bool Loop = true;
    while (Loop)
    {
      enemySummoner.state.facingAngle = Utils.GetAngle(enemySummoner.transform.position, enemySummoner.TargetObject.transform.position);
      float num = Vector3.Distance(enemySummoner.TargetObject.transform.position, enemySummoner.transform.position);
      enemySummoner.FleeDelay -= Time.deltaTime;
      enemySummoner.TeleportDelay -= Time.deltaTime;
      if ((double) enemySummoner.FleeDelay < 0.0 && (double) num < 2.5 || (double) enemySummoner.TeleportDelay < 0.0)
      {
        if ((double) enemySummoner.FleeDelay < 0.0 && (double) num < 2.5)
        {
          enemySummoner.FireBallDelay = UnityEngine.Random.Range(0.0f, 1f);
          enemySummoner.SummonDelay = UnityEngine.Random.Range(0.0f, 1f);
        }
        enemySummoner.FleeDelay = 2f;
        enemySummoner.TeleportDelay = UnityEngine.Random.Range(3f, 5f);
        yield return (object) enemySummoner.StartCoroutine((IEnumerator) enemySummoner.DoTeleport());
      }
      if (enemySummoner.Summon && (double) (enemySummoner.SummonDelay -= Time.deltaTime) < 0.0 && enemySummoner.SummonedCount < 2)
      {
        enemySummoner.StopAllCoroutines();
        enemySummoner.StartCoroutine((IEnumerator) enemySummoner.DoSummon());
        break;
      }
      if (enemySummoner.FireBalls && (double) (enemySummoner.FireBallDelay -= Time.deltaTime) < 0.0)
      {
        enemySummoner.StopAllCoroutines();
        enemySummoner.FireBallDelay = UnityEngine.Random.Range(2f, 3f);
        enemySummoner.StartCoroutine((IEnumerator) enemySummoner.DoThrowFireBall());
        break;
      }
      if (enemySummoner.Mortar && (double) (enemySummoner.MortarDelay -= Time.deltaTime) < 0.0)
      {
        enemySummoner.StopAllCoroutines();
        enemySummoner.MortarDelay = UnityEngine.Random.Range(3f, 5f);
        enemySummoner.StartCoroutine((IEnumerator) enemySummoner.DoThrowMortar());
        break;
      }
      if (enemySummoner.HealOthers && (double) (enemySummoner.HealDelay -= Time.deltaTime) < 0.0 && Health.team2.Count > 1)
      {
        enemySummoner.StopAllCoroutines();
        enemySummoner.HealDelay = UnityEngine.Random.Range(3f, 4f);
        enemySummoner.StartCoroutine((IEnumerator) enemySummoner.DoHealOthers());
        break;
      }
      yield return (object) null;
    }
  }

  private IEnumerator DoSummon()
  {
    EnemySummoner enemySummoner = this;
    enemySummoner.ClearPaths();
    for (int SpawnCount = enemySummoner.NumToSpawn; SpawnCount > 0; --SpawnCount)
    {
      if (!string.IsNullOrEmpty(enemySummoner.WarningVO))
        AudioManager.Instance.PlayOneShot(enemySummoner.WarningVO, enemySummoner.transform.position);
      enemySummoner.summonParticles.startDelay = 1.5f;
      enemySummoner.summonParticles.Play();
      enemySummoner.skeletonAnimation.AnimationState.SetAnimation(0, "summon", false);
      enemySummoner.skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      Vector3 normalized = (enemySummoner.TargetObject.transform.position - enemySummoner.transform.position).normalized;
      Vector3 position = enemySummoner.transform.position + normalized * 2f;
      if ((bool) Physics2D.Raycast((Vector2) enemySummoner.transform.position, (Vector2) normalized, 2f, (int) enemySummoner.layerToCheck))
      {
        position = enemySummoner.transform.position + normalized * -2f;
        if ((bool) Physics2D.Raycast((Vector2) enemySummoner.transform.position, (Vector2) (normalized * -1f), 2f, (int) enemySummoner.layerToCheck))
          position = enemySummoner.transform.position;
      }
      Health.team2.Add((Health) null);
      Interaction_Chest.Instance?.Enemies.Add((Health) null);
      // ISSUE: reference to a compiler-generated method
      Addressables.InstantiateAsync((object) enemySummoner.EnemyList[UnityEngine.Random.Range(0, enemySummoner.EnemyList.Length)], position, Quaternion.identity, enemySummoner.transform.parent).Completed += new System.Action<AsyncOperationHandle<GameObject>>(enemySummoner.\u003CDoSummon\u003Eb__39_0);
      ++enemySummoner.SummonedCount;
      enemySummoner.SummonDelay = 4f;
      yield return (object) new WaitForSeconds(1.6f);
      enemySummoner.EnemySpawnerGO = (GameObject) null;
    }
    enemySummoner.StopAllCoroutines();
    enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
  }

  private void RemoveSpawned(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    --this.SummonedCount;
    Victim.OnDie -= new Health.DieAction(this.RemoveSpawned);
  }

  private IEnumerator DoStunned()
  {
    EnemySummoner enemySummoner = this;
    enemySummoner.Stunned = true;
    enemySummoner.health.ArrowAttackVulnerability = 1f;
    enemySummoner.health.MeleeAttackVulnerability = 1f;
    enemySummoner.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySummoner.skeletonAnimation.AnimationState.SetAnimation(0, "stunned", true);
    yield return (object) new WaitForSeconds(2f);
    enemySummoner.Stunned = false;
    enemySummoner.health.ArrowAttackVulnerability = 1f;
    if (!enemySummoner.HealOthers)
      enemySummoner.health.MeleeAttackVulnerability = 0.1f;
    enemySummoner.StopAllCoroutines();
    enemySummoner.StartCoroutine((IEnumerator) enemySummoner.DoTeleport());
  }

  private IEnumerator DoThrowFireBall()
  {
    EnemySummoner enemySummoner = this;
    enemySummoner.Shooting = true;
    for (int NumToShootCount = enemySummoner.NumToShoot; NumToShootCount > 0; --NumToShootCount)
    {
      enemySummoner.state.CURRENT_STATE = StateMachine.State.Attacking;
      enemySummoner.skeletonAnimation.AnimationState.SetAnimation(0, "projectile", false);
      enemySummoner.skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      float Timer = 0.0f;
      while ((double) (Timer += Time.deltaTime) < 1.3500000238418579)
      {
        enemySummoner.simpleSpineFlash.FlashWhite(Timer / 1.35f);
        yield return (object) null;
      }
      enemySummoner.simpleSpineFlash.FlashWhite(false);
    }
    enemySummoner.Shooting = false;
    enemySummoner.StopAllCoroutines();
    enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
  }

  private void FireMortar()
  {
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.DoThrowMortar());
  }

  private IEnumerator DoThrowMortar()
  {
    EnemySummoner enemySummoner = this;
    Vector3 targetPosition = enemySummoner.TargetObject.transform.position;
    for (int i = 0; i < enemySummoner.MortarShotsToFire; ++i)
    {
      if ((UnityEngine.Object) enemySummoner.TargetObject == (UnityEngine.Object) null)
      {
        enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
        yield break;
      }
      if (enemySummoner.MortarShotsToFire > 1)
        targetPosition = enemySummoner.TargetObject.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
      MortarBomb component = UnityEngine.Object.Instantiate<GameObject>(enemySummoner.projectilePrefab, enemySummoner.TargetObject.transform.position, Quaternion.identity, enemySummoner.transform.parent).GetComponent<MortarBomb>();
      if ((double) Vector2.Distance((Vector2) enemySummoner.transform.position, (Vector2) targetPosition) < 2.5)
        component.transform.position = enemySummoner.transform.position + (targetPosition - enemySummoner.transform.position).normalized * 2.5f;
      else
        component.transform.position = enemySummoner.transform.position + (targetPosition - enemySummoner.transform.position).normalized * 5f;
      component.Play(enemySummoner.transform.position + new Vector3(0.0f, 0.0f, -1.5f), enemySummoner.bombDuration, Health.Team.Team2);
      enemySummoner.simpleSpineFlash.FlashWhite(false);
      yield return (object) new WaitForSeconds(enemySummoner.timeBetweenShots);
    }
    enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
  }

  private IEnumerator DoTeleport()
  {
    EnemySummoner enemySummoner = this;
    enemySummoner.state.CURRENT_STATE = StateMachine.State.Teleporting;
    enemySummoner.Shadow.enabled = false;
    enemySummoner.Teleporting = true;
    yield return (object) new WaitForSeconds(0.15f);
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", enemySummoner.gameObject);
    enemySummoner.summonParticles.startDelay = 0.0f;
    enemySummoner.summonParticles.Play();
    enemySummoner.teleportEffect.Play();
    enemySummoner.skeletonAnimation.AnimationState.SetAnimation(0, "teleport", false);
    enemySummoner.skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
    enemySummoner.CircleCollider.enabled = false;
    yield return (object) new WaitForSeconds(0.8f);
    enemySummoner.StopAllCoroutines();
    enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
  }

  private void Teleport()
  {
    float num = 100f;
    while ((double) --num > 0.0)
    {
      float f = (float) UnityEngine.Random.Range(0, 360) * ((float) Math.PI / 180f);
      float distance = (float) UnityEngine.Random.Range(4, 7);
      Vector3 vector3 = this.TargetObject.transform.position + new Vector3(distance * Mathf.Cos(f), distance * Mathf.Sin(f));
      RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) this.TargetObject.transform.position, this.CircleCastRadius, (Vector2) Vector3.Normalize(vector3 - this.TargetObject.transform.position), distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) raycastHit2D.collider != (UnityEngine.Object) null)
      {
        if ((double) Vector3.Distance(this.TargetObject.transform.position, (Vector3) raycastHit2D.centroid) > 3.0)
        {
          if (this.ShowDebug)
          {
            this.Points.Add(new Vector3(raycastHit2D.centroid.x, raycastHit2D.centroid.y));
            this.PointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
          }
          this.transform.position = (Vector3) raycastHit2D.centroid + Vector3.Normalize(this.TargetObject.transform.position - vector3) * this.CircleCastOffset;
          break;
        }
      }
      else
      {
        if (this.ShowDebug)
        {
          this.EndPoints.Add(new Vector3(vector3.x, vector3.y));
          this.EndPointsLink.Add(new Vector3(this.transform.position.x, this.transform.position.y));
        }
        this.transform.position = vector3;
        break;
      }
    }
    if ((UnityEngine.Object) this.TargetObject != (UnityEngine.Object) null)
      this.state.facingAngle = Utils.GetAngle(this.transform.position, this.TargetObject.transform.position);
    this.CircleCollider.enabled = true;
    this.Shadow.enabled = true;
    this.summonParticles.startDelay = 0.0f;
    this.summonParticles.Play();
    this.teleportEffect.Play();
    this.Teleporting = false;
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", this.gameObject);
  }

  private Health FindTargetToHeal()
  {
    Health targetToHeal = (Health) null;
    float num = float.MaxValue;
    foreach (Health health in Health.team2)
    {
      if ((UnityEngine.Object) health != (UnityEngine.Object) null && (double) health.HP < (double) health.totalHP && (double) health.HP < (double) num && (UnityEngine.Object) health != (UnityEngine.Object) this.health)
      {
        targetToHeal = health;
        num = health.HP;
      }
    }
    return targetToHeal;
  }

  private IEnumerator DoHealOthers()
  {
    EnemySummoner enemySummoner = this;
    Health TargetToHeal = enemySummoner.FindTargetToHeal();
    Debug.Log((object) (" DoHealOthers() - TargetToHeal: " + (object) TargetToHeal));
    if ((UnityEngine.Object) TargetToHeal == (UnityEngine.Object) null)
    {
      Debug.Log((object) "no target!");
      enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
    }
    else
    {
      enemySummoner.ClearPaths();
      if (!string.IsNullOrEmpty(enemySummoner.WarningVO))
        AudioManager.Instance.PlayOneShot(enemySummoner.WarningVO, enemySummoner.transform.position);
      enemySummoner.summonParticles.startDelay = 1.5f;
      enemySummoner.summonParticles.Play();
      enemySummoner.skeletonAnimation.AnimationState.SetAnimation(0, "summon", false);
      enemySummoner.skeletonAnimation.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      yield return (object) new WaitForSeconds(1.33333337f);
      TargetToHeal = enemySummoner.FindTargetToHeal();
      if ((UnityEngine.Object) TargetToHeal != (UnityEngine.Object) null)
        SoulCustomTarget.Create(TargetToHeal.gameObject, enemySummoner.transform.position, Color.white, (System.Action) (() =>
        {
          if (!((UnityEngine.Object) TargetToHeal != (UnityEngine.Object) null))
            return;
          TargetToHeal.HP = TargetToHeal.totalHP;
          TargetToHeal.GetComponent<ShowHPBar>()?.OnHit(TargetToHeal.gameObject, Vector3.zero, Health.AttackTypes.Melee, false);
          AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", TargetToHeal.gameObject.transform.position);
          BiomeConstants.Instance.EmitHeartPickUpVFX(TargetToHeal.transform.position - Vector3.forward, 0.0f, "red", "burst_big");
        }), 0.75f);
      yield return (object) new WaitForSeconds(0.466666579f);
      enemySummoner.StopAllCoroutines();
      enemySummoner.StartCoroutine((IEnumerator) enemySummoner.ChasePlayer());
    }
  }

  private void OnDrawGizmos()
  {
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
}
