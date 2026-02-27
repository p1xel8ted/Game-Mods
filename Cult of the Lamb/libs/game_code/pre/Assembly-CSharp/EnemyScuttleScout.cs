// Decompiled with JetBrains decompiler
// Type: EnemyScuttleScout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyScuttleScout : EnemyScuttleSwiper
{
  public AssetReferenceGameObject[] EnemyList;
  public SimpleSpineFlash SimpleSpineFlash;
  public float MortarDelay = 1f;
  private float MortarTimer;
  private float Distance = 2f;
  private float SpawnCircleCastRadius = 1f;
  public int SummonedCount;
  private List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  [EventRef]
  public string attackSoundPath = string.Empty;
  private bool canBeParried;
  private static float signPostParryWindow = 0.2f;
  private static float attackParryWindow = 0.15f;
  public float SignPostCloseCombatDelay = 1f;
  private float CloseCombatCooldown;
  public GameObject projectilePrefab;
  protected const float minBombRange = 2.5f;
  protected const float maxBombRange = 8f;
  public float timeBetweenShots = 0.5f;
  public float bombDuration = 0.75f;
  public int MortarShotsToFire = 2;
  private List<Vector3> TeleportPositions;
  private Vector3 Direction;
  private RaycastHit2D Results;

  public override void Awake()
  {
    base.Awake();
    if (!((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null))
      return;
    this.GetComponent<Health>().totalHP *= BiomeGenerator.Instance.HumanoidHealthMultiplier;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    if (this.loadedAddressableAssets == null)
      return;
    foreach (AsyncOperationHandle<GameObject> addressableAsset in this.loadedAddressableAssets)
      Addressables.Release((AsyncOperationHandle) addressableAsset);
    this.loadedAddressableAssets.Clear();
  }

  public override void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    base.OnHit(Attacker, AttackLocation, AttackType, FromBehind);
    this.DoKnockBack(Attacker, this.KnockbackModifier, 0.5f);
    if (string.IsNullOrEmpty(this.GetHitVO))
      return;
    AudioManager.Instance.PlayOneShot(this.GetHitVO, this.transform.position);
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (string.IsNullOrEmpty(this.DeathVO))
      return;
    AudioManager.Instance.PlayOneShot(this.DeathVO, this.transform.position);
  }

  protected override IEnumerator ActiveRoutine()
  {
    EnemyScuttleScout enemyScuttleScout = this;
    enemyScuttleScout.MortarTimer = (float) UnityEngine.Random.Range(3, 5);
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      if (enemyScuttleScout.state.CURRENT_STATE == StateMachine.State.Idle && (double) (enemyScuttleScout.IdleWait -= Time.deltaTime) <= 0.0)
        enemyScuttleScout.GetNewTargetPosition();
      if ((UnityEngine.Object) enemyScuttleScout.TargetObject != (UnityEngine.Object) null && !enemyScuttleScout.Attacking && !enemyScuttleScout.IsStunned && GameManager.RoomActive)
        enemyScuttleScout.state.LookAngle = Utils.GetAngle(enemyScuttleScout.transform.position, enemyScuttleScout.TargetObject.transform.position);
      else
        enemyScuttleScout.state.LookAngle = enemyScuttleScout.state.facingAngle;
      if (enemyScuttleScout.MovingAnimation != "")
      {
        if (enemyScuttleScout.state.CURRENT_STATE == StateMachine.State.Moving && enemyScuttleScout.Spine.AnimationName != enemyScuttleScout.MovingAnimation)
          enemyScuttleScout.Spine.AnimationState.SetAnimation(0, enemyScuttleScout.MovingAnimation, true);
        if (enemyScuttleScout.state.CURRENT_STATE == StateMachine.State.Idle && enemyScuttleScout.Spine.AnimationName != enemyScuttleScout.IdleAnimation)
          enemyScuttleScout.Spine.AnimationState.SetAnimation(0, enemyScuttleScout.IdleAnimation, true);
      }
      if ((UnityEngine.Object) enemyScuttleScout.TargetObject == (UnityEngine.Object) null)
      {
        enemyScuttleScout.GetNewTarget();
        if ((UnityEngine.Object) enemyScuttleScout.TargetObject != (UnityEngine.Object) null)
          enemyScuttleScout.ShowWarningIcon();
      }
      else
      {
        if (!enemyScuttleScout.Attacking && (double) (enemyScuttleScout.CloseCombatCooldown -= Time.deltaTime) < 0.0 && (double) Vector3.Distance(enemyScuttleScout.transform.position, enemyScuttleScout.TargetObject.transform.position) < 2.0)
          enemyScuttleScout.StartCoroutine((IEnumerator) enemyScuttleScout.CloseCombatAttack());
        if (enemyScuttleScout.ShouldMortar())
          enemyScuttleScout.StartCoroutine((IEnumerator) enemyScuttleScout.DoThrowMortar());
      }
      yield return (object) null;
    }
  }

  private IEnumerator CloseCombatAttack()
  {
    EnemyScuttleScout enemyScuttleScout = this;
    enemyScuttleScout.Attacking = true;
    enemyScuttleScout.ClearPaths();
    enemyScuttleScout.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    float Progress = 0.0f;
    AudioManager.Instance.PlayOneShot(enemyScuttleScout.WarningVO, enemyScuttleScout.gameObject);
    enemyScuttleScout.Spine.AnimationState.SetAnimation(0, "grunt-attack-charge2", false);
    enemyScuttleScout.state.facingAngle = enemyScuttleScout.state.LookAngle = Utils.GetAngle(enemyScuttleScout.transform.position, enemyScuttleScout.TargetObject.transform.position);
    enemyScuttleScout.Spine.skeleton.ScaleX = (double) enemyScuttleScout.state.LookAngle <= 90.0 || (double) enemyScuttleScout.state.LookAngle >= 270.0 ? -1f : 1f;
    while ((double) (Progress += Time.deltaTime) < (double) enemyScuttleScout.SignPostCloseCombatDelay)
    {
      if ((double) Progress >= (double) enemyScuttleScout.SignPostCloseCombatDelay - (double) EnemyScuttleScout.signPostParryWindow)
        enemyScuttleScout.canBeParried = true;
      enemyScuttleScout.SimpleSpineFlash.FlashWhite(Progress / enemyScuttleScout.SignPostCloseCombatDelay);
      yield return (object) null;
    }
    enemyScuttleScout.speed = 0.2f;
    enemyScuttleScout.SimpleSpineFlash.FlashWhite(false);
    enemyScuttleScout.Spine.AnimationState.SetAnimation(0, "grunt-attack-impact2", false);
    if (!string.IsNullOrEmpty(enemyScuttleScout.AttackVO))
      AudioManager.Instance.PlayOneShot(enemyScuttleScout.AttackVO, enemyScuttleScout.transform.position);
    if (!string.IsNullOrEmpty(enemyScuttleScout.attackSoundPath))
      AudioManager.Instance.PlayOneShot(enemyScuttleScout.attackSoundPath, enemyScuttleScout.transform.position);
    Progress = 0.0f;
    float Duration = 0.2f;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      if ((UnityEngine.Object) enemyScuttleScout.damageColliderEvents != (UnityEngine.Object) null)
        enemyScuttleScout.damageColliderEvents.SetActive(true);
      enemyScuttleScout.canBeParried = (double) Progress <= (double) EnemyScuttleScout.attackParryWindow;
      yield return (object) null;
    }
    if ((UnityEngine.Object) enemyScuttleScout.damageColliderEvents != (UnityEngine.Object) null)
      enemyScuttleScout.damageColliderEvents.SetActive(false);
    enemyScuttleScout.canBeParried = false;
    yield return (object) new WaitForSeconds(0.8f);
    enemyScuttleScout.CloseCombatCooldown = 1f;
    enemyScuttleScout.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleScout.Attacking = false;
  }

  private IEnumerator DoThrowMortar()
  {
    EnemyScuttleScout enemyScuttleScout = this;
    enemyScuttleScout.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemyScuttleScout.Attacking = true;
    Vector3 targetPosition = enemyScuttleScout.TargetObject.transform.position;
    for (int i = 0; i < enemyScuttleScout.MortarShotsToFire; ++i)
    {
      AudioManager.Instance.PlayOneShot(enemyScuttleScout.WarningVO, enemyScuttleScout.gameObject);
      enemyScuttleScout.Spine.AnimationState.SetAnimation(0, "throw-bomb", false);
      enemyScuttleScout.Spine.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      float Progress = 0.0f;
      float Duration = 1f;
      while ((double) (Progress += Time.deltaTime) < (double) Duration)
      {
        enemyScuttleScout.SimpleSpineFlash.FlashWhite(Progress / Duration);
        yield return (object) null;
      }
      if ((UnityEngine.Object) enemyScuttleScout.TargetObject == (UnityEngine.Object) null)
      {
        enemyScuttleScout.MortarTimer = enemyScuttleScout.MortarDelay;
        yield break;
      }
      if (enemyScuttleScout.MortarShotsToFire > 1)
        targetPosition = enemyScuttleScout.TargetObject.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * 2f;
      MortarBomb component = UnityEngine.Object.Instantiate<GameObject>(enemyScuttleScout.projectilePrefab, enemyScuttleScout.TargetObject.transform.position, Quaternion.identity, enemyScuttleScout.transform.parent).GetComponent<MortarBomb>();
      float num = Vector2.Distance((Vector2) enemyScuttleScout.transform.position, (Vector2) targetPosition);
      if ((double) num < 2.5)
        component.transform.position = enemyScuttleScout.transform.position + (targetPosition - enemyScuttleScout.transform.position).normalized * 2.5f;
      else if ((double) num > 8.0)
        component.transform.position = enemyScuttleScout.transform.position + (targetPosition - enemyScuttleScout.transform.position).normalized * 8f;
      component.Play(enemyScuttleScout.transform.position + new Vector3(0.0f, 0.0f, -1.5f), enemyScuttleScout.bombDuration, Health.Team.KillAll);
      enemyScuttleScout.SimpleSpineFlash.FlashWhite(false);
      if (!string.IsNullOrEmpty(enemyScuttleScout.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyScuttleScout.AttackVO, enemyScuttleScout.transform.position);
      if (!string.IsNullOrEmpty(enemyScuttleScout.attackSoundPath))
        AudioManager.Instance.PlayOneShot(enemyScuttleScout.attackSoundPath, enemyScuttleScout.transform.position);
      yield return (object) new WaitForSeconds(enemyScuttleScout.timeBetweenShots);
    }
    enemyScuttleScout.SimpleSpineFlash.FlashWhite(false);
    enemyScuttleScout.state.CURRENT_STATE = StateMachine.State.Idle;
    enemyScuttleScout.Attacking = false;
    enemyScuttleScout.MortarTimer = enemyScuttleScout.MortarDelay;
  }

  protected virtual bool ShouldMortar()
  {
    return (double) (this.MortarTimer -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, this.TargetObject.transform.position) < 8.0 && GameManager.RoomActive;
  }

  protected override IEnumerator AttackRoutine()
  {
    EnemyScuttleScout enemyScuttleScout = this;
    Debug.Log((object) "ATTACK!!!");
    if (enemyScuttleScout.GetAvailableSpawnPositions().Count <= 0 || enemyScuttleScout.SummonedCount >= 2)
    {
      enemyScuttleScout.AttackDelay = enemyScuttleScout.AttackDelayTime;
    }
    else
    {
      enemyScuttleScout.state.CURRENT_STATE = StateMachine.State.Attacking;
      enemyScuttleScout.Attacking = true;
      if (!string.IsNullOrEmpty(enemyScuttleScout.AttackVO))
        AudioManager.Instance.PlayOneShot(enemyScuttleScout.AttackVO, enemyScuttleScout.transform.position);
      enemyScuttleScout.Spine.AnimationState.SetAnimation(0, "alarm", false);
      enemyScuttleScout.Spine.AnimationState.AddAnimation(0, "dance", true, 0.0f);
      yield return (object) new WaitForSeconds(0.8333333f);
      Health.team2.Add((Health) null);
      Interaction_Chest.Instance?.Enemies.Add((Health) null);
      foreach (Vector3 teleportPosition in enemyScuttleScout.TeleportPositions)
      {
        // ISSUE: reference to a compiler-generated method
        Addressables.LoadAssetAsync<GameObject>((object) enemyScuttleScout.EnemyList[UnityEngine.Random.Range(0, enemyScuttleScout.EnemyList.Length)]).Completed += new System.Action<AsyncOperationHandle<GameObject>>(enemyScuttleScout.\u003CAttackRoutine\u003Eb__28_0);
        ++enemyScuttleScout.SummonedCount;
      }
      yield return (object) new WaitForSeconds(1.26666665f);
      enemyScuttleScout.state.CURRENT_STATE = StateMachine.State.Idle;
      enemyScuttleScout.Attacking = false;
      enemyScuttleScout.AttackDelay = enemyScuttleScout.AttackDelayTime;
    }
  }

  private List<Vector3> GetAvailableSpawnPositions()
  {
    this.TeleportPositions = new List<Vector3>();
    int num = -3;
    while ((num += 2) <= 1)
    {
      float f = (float) (((double) this.state.LookAngle + (double) (45 * num)) * (Math.PI / 180.0));
      this.Direction = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
      this.Results = Physics2D.CircleCast((Vector2) this.transform.position, this.SpawnCircleCastRadius, (Vector2) this.Direction, this.Distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) this.Results.collider == (UnityEngine.Object) null)
        this.TeleportPositions.Add(this.transform.position + this.Direction * this.Distance);
    }
    if (this.TeleportPositions.Count <= 0)
    {
      this.Direction = new Vector3(Mathf.Cos(this.state.LookAngle), Mathf.Sin(this.state.LookAngle));
      this.Results = Physics2D.CircleCast((Vector2) this.transform.position, this.SpawnCircleCastRadius, (Vector2) this.Direction, this.Distance, (int) this.layerToCheck);
      if ((UnityEngine.Object) this.Results.collider == (UnityEngine.Object) null)
        this.TeleportPositions.Add(this.transform.position + this.Direction * this.Distance);
    }
    return this.TeleportPositions;
  }

  private void RemoveSpawned(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.AttackDelay = this.AttackDelayTime;
    --this.SummonedCount;
    Victim.OnDie -= new Health.DieAction(this.RemoveSpawned);
  }
}
