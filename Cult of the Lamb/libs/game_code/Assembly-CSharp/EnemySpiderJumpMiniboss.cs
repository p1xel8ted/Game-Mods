// Decompiled with JetBrains decompiler
// Type: EnemySpiderJumpMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemySpiderJumpMiniboss : EnemySpider
{
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string swinAwayAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string swinInAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string StuckAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string UnstuckAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string shootAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string shootAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string jumpAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string landAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string jumpQuickAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  public string jumpAnticipateAnimation;
  [Space]
  [SerializeField]
  public int attacksBeforeSlam;
  [SerializeField]
  public float slamAttackMinRange;
  [SerializeField]
  public float timeBetweenSlams;
  [SerializeField]
  public float slamInAirDuration;
  [SerializeField]
  public float slamLandDuration;
  [SerializeField]
  public float slamCooldown;
  [SerializeField]
  public float randomOffsetRadius = 0.25f;
  [SerializeField]
  public float screenShake;
  [SerializeField]
  public ProjectilePatternBase slamProjectilePattern;
  [SerializeField]
  public ProjectilePatternBase projectilePattern;
  [SerializeField]
  public float projectileAnticipation;
  [SerializeField]
  public Vector2 timeBetweenProjectileAttacks;
  [SerializeField]
  public bool canTargetJump;
  [SerializeField]
  public float jumpMaxDistance;
  [SerializeField]
  public float jumpDuration;
  [SerializeField]
  public Vector2 targetJumpAmount;
  [SerializeField]
  public float delayBetweenJumps;
  [SerializeField]
  public EnemySpiderJumpMiniboss.SpawnSet[] spawnSets;
  [SerializeField]
  public float spawnForce;
  [SerializeField]
  public GameObject slamParticlePrefab;
  [SerializeField]
  public SpriteRenderer indicatorIcon;
  public Color indicatorColor = Color.white;
  public float SlamTimer;
  public int attackCounter;
  public float projectileTimer;
  public int UpdateEveryFrameNum = 5;
  public int curFrame;

  public IEnumerator Start()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    if ((UnityEngine.Object) SkeletonAnimationLODGlobalManager.Instance != (UnityEngine.Object) null)
      SkeletonAnimationLODGlobalManager.Instance.DisableCulling(spiderJumpMiniboss.Spine.transform, spiderJumpMiniboss.Spine);
    yield return (object) null;
    spiderJumpMiniboss.Preload();
  }

  public void Preload()
  {
    for (int i = 0; i < this.spawnSets.Length; i++)
    {
      this.spawnSets[i].loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
      for (int index = 0; index < this.spawnSets[i].EnemiesList.Length; ++index)
      {
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.spawnSets[i].EnemiesList[index]);
        asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          this.spawnSets[i].loadedAddressableAssets.Add(obj);
          obj.Result.CreatePool(Mathf.CeilToInt(this.spawnSets[i].Amount.y) * 2, true);
        });
        asyncOperationHandle.WaitForCompletion();
      }
    }
  }

  public override void OnEnable()
  {
    this.SlamTimer = this.timeBetweenSlams + UnityEngine.Random.Range(0.0f, 3f);
    this.indicatorIcon.gameObject.SetActive(false);
    this.projectileTimer = UnityEngine.Random.Range(this.timeBetweenProjectileAttacks.x, this.timeBetweenProjectileAttacks.y);
    this.Spine.transform.localPosition = Vector3.zero;
    base.OnEnable();
  }

  public override void Update()
  {
    if ((double) Time.timeScale == 0.0)
      return;
    this.AttackingTargetPosition = this.transform.position - Vector3.up;
    base.Update();
    if (!this.indicatorIcon.gameObject.activeSelf)
      return;
    if (this.curFrame == this.UpdateEveryFrameNum)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
      this.curFrame = 0;
    }
    else
      ++this.curFrame;
  }

  public override IEnumerator ActiveRoutine()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while (GameManager.RoomActive)
      {
        if (spiderJumpMiniboss.state.CURRENT_STATE == StateMachine.State.Idle && (double) (spiderJumpMiniboss.IdleWait -= Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) <= 0.0 && (double) GameManager.GetInstance().CurrentTime > (double) spiderJumpMiniboss.initialMovementDelay)
        {
          if (spiderJumpMiniboss.wander && spiderJumpMiniboss.attackCounter < spiderJumpMiniboss.attacksBeforeSlam)
            spiderJumpMiniboss.GetNewTargetPosition();
          else
            spiderJumpMiniboss.Flee();
          spiderJumpMiniboss.speed = spiderJumpMiniboss.maxSpeed;
        }
        if ((UnityEngine.Object) spiderJumpMiniboss.TargetEnemy != (UnityEngine.Object) null && !spiderJumpMiniboss.Attacking && !spiderJumpMiniboss.IsStunned)
          spiderJumpMiniboss.state.LookAngle = Utils.GetAngle(spiderJumpMiniboss.transform.position, spiderJumpMiniboss.TargetEnemy.transform.position);
        else
          spiderJumpMiniboss.state.LookAngle = spiderJumpMiniboss.state.facingAngle;
        if (spiderJumpMiniboss.MovingAnimation != "" && (double) GameManager.GetInstance().CurrentTime > (double) spiderJumpMiniboss.initialMovementDelay)
        {
          if (spiderJumpMiniboss.state.CURRENT_STATE == StateMachine.State.Moving && spiderJumpMiniboss.Spine.AnimationName != spiderJumpMiniboss.MovingAnimation)
            spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.MovingAnimation, true);
          if (spiderJumpMiniboss.state.CURRENT_STATE == StateMachine.State.Idle && spiderJumpMiniboss.Spine.AnimationName != spiderJumpMiniboss.IdleAnimation)
            spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.IdleAnimation, true);
        }
        int num = UnityEngine.Random.Range(0, 10);
        if (spiderJumpMiniboss.ShouldProjectileAttack() && num < 2)
        {
          AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.warningSfx, spiderJumpMiniboss.gameObject);
          spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.ProjectileAttack());
        }
        else if (spiderJumpMiniboss.ShouldAttack() && num < 5)
        {
          AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.warningSfx, spiderJumpMiniboss.gameObject);
          spiderJumpMiniboss.attackCounter = 1;
          spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.AttackRoutine());
        }
        else if (spiderJumpMiniboss.ShouldSlam() && num < 7)
        {
          AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.warningSfx, spiderJumpMiniboss.gameObject);
          spiderJumpMiniboss.attackCounter = 0;
          spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.SlamRoutine());
        }
        else if (spiderJumpMiniboss.ShouldTargetJump())
        {
          AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.warningSfx, spiderJumpMiniboss.gameObject);
          spiderJumpMiniboss.attackCounter = 0;
          spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.TargetJumpRoutine());
        }
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  public override bool ShouldAttack() => base.ShouldAttack();

  public bool ShouldTargetJump()
  {
    return this.canTargetJump && (double) (this.SlamTimer -= Time.deltaTime * this.Spine.timeScale) < 0.0 && !this.Attacking && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  public bool ShouldSlam()
  {
    Health closestTarget = this.GetClosestTarget();
    Health health = (UnityEngine.Object) closestTarget == (UnityEngine.Object) null ? (Health) PlayerFarming.Instance.health : closestTarget;
    return (double) (this.SlamTimer -= Time.deltaTime * this.Spine.timeScale) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, health.transform.position) > (double) this.slamAttackMinRange && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  public bool ShouldProjectileAttack()
  {
    return (double) (this.projectileTimer -= Time.deltaTime * this.Spine.timeScale) < 0.0 && !this.Attacking && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    if (!(bool) (UnityEngine.Object) this.GetComponentInParent<MiniBossController>())
      return;
    for (int index = EnemySpider.EnemySpiders.Count - 1; index >= 0; --index)
    {
      if ((bool) (UnityEngine.Object) EnemySpider.EnemySpiders[index] && (UnityEngine.Object) EnemySpider.EnemySpiders[index] != (UnityEngine.Object) this)
      {
        SpawnEnemyOnDeath component = EnemySpider.EnemySpiders[index].GetComponent<SpawnEnemyOnDeath>();
        if ((bool) (UnityEngine.Object) component)
          component.Amount = 0;
        EnemySpider.EnemySpiders[index].health.enabled = true;
        EnemySpider.EnemySpiders[index].health.DealDamage(EnemySpider.EnemySpiders[index].health.totalHP, this.gameObject, EnemySpider.EnemySpiders[index].transform.position);
      }
    }
  }

  public void Slam() => this.StartCoroutine((IEnumerator) this.SlamRoutine());

  public IEnumerator SlamRoutine()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    spiderJumpMiniboss.Spine.ForceVisible = true;
    spiderJumpMiniboss.Attacking = true;
    spiderJumpMiniboss.updateDirection = false;
    spiderJumpMiniboss.ClearPaths();
    Health closestTarget = spiderJumpMiniboss.GetClosestTarget();
    Vector3 targetPosition = (UnityEngine.Object) closestTarget != (UnityEngine.Object) null ? closestTarget.transform.position : PlayerFarming.Instance.transform.position;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.swinAwayAnimation);
    Vector3 targetShadowScale = spiderJumpMiniboss.ShadowSpriteRenderer.transform.localScale;
    spiderJumpMiniboss.ShadowSpriteRenderer.transform.DOScale(0.0f, 1f);
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.warningSfx, spiderJumpMiniboss.transform.position);
    yield return (object) new WaitForEndOfFrame();
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 1.0)
      yield return (object) null;
    if (EnemySpider.EnemySpiders.Count < 3)
      spiderJumpMiniboss.SpawnEnemies();
    spiderJumpMiniboss.health.enabled = false;
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.jumpSfx, spiderJumpMiniboss.transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < ((double) spiderJumpMiniboss.slamInAirDuration - 1.0) / 2.0)
      yield return (object) null;
    spiderJumpMiniboss.transform.position = targetPosition + (Vector3) UnityEngine.Random.insideUnitCircle * spiderJumpMiniboss.randomOffsetRadius;
    spiderJumpMiniboss.transform.position = new Vector3(Mathf.Clamp(spiderJumpMiniboss.transform.position.x, -6.5f, 6.5f), Mathf.Clamp(spiderJumpMiniboss.transform.position.y, -4f, 4f), spiderJumpMiniboss.transform.position.z);
    spiderJumpMiniboss.indicatorIcon.gameObject.SetActive(true);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < ((double) spiderJumpMiniboss.slamInAirDuration - 1.0) / 2.0)
      yield return (object) null;
    spiderJumpMiniboss.ShadowSpriteRenderer.transform.DOScale(targetShadowScale, spiderJumpMiniboss.slamLandDuration);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < (double) spiderJumpMiniboss.slamLandDuration * 0.5)
      yield return (object) null;
    spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.swinInAnimation);
    spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.StuckAnimation);
    spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.UnstuckAnimation);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < (double) spiderJumpMiniboss.slamLandDuration * 0.5)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.stuckSfx, spiderJumpMiniboss.transform.position);
    CameraManager.instance.ShakeCameraForDuration(spiderJumpMiniboss.screenShake, spiderJumpMiniboss.screenShake, 0.1f);
    UnityEngine.Object.Instantiate<GameObject>(spiderJumpMiniboss.slamParticlePrefab, spiderJumpMiniboss.transform.position, Quaternion.identity);
    spiderJumpMiniboss.damageColliderEvents.gameObject.SetActive(true);
    spiderJumpMiniboss.indicatorIcon.gameObject.SetActive(false);
    spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.slamProjectilePattern.ShootIE());
    spiderJumpMiniboss.health.enabled = true;
    spiderJumpMiniboss.health.DontCombo = true;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    spiderJumpMiniboss.damageColliderEvents.gameObject.SetActive(false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < (double) spiderJumpMiniboss.slamCooldown - 0.5)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.breakFreeSfx, spiderJumpMiniboss.transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
    spiderJumpMiniboss.health.DontCombo = false;
    spiderJumpMiniboss.IdleWait = 0.0f;
    spiderJumpMiniboss.SlamTimer = spiderJumpMiniboss.timeBetweenSlams;
    spiderJumpMiniboss.Attacking = false;
    spiderJumpMiniboss.updateDirection = true;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    spiderJumpMiniboss.Spine.ForceVisible = false;
  }

  public IEnumerator TargetJumpRoutine()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    spiderJumpMiniboss.Attacking = true;
    spiderJumpMiniboss.ClearPaths();
    AnimationCurve curve = new AnimationCurve();
    curve.AddKey(0.0f, 0.0f);
    curve.AddKey(0.5f, 1f);
    curve.AddKey(1f, 0.0f);
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    int jumpAmount = (int) UnityEngine.Random.Range(spiderJumpMiniboss.targetJumpAmount.x, spiderJumpMiniboss.targetJumpAmount.y + 1f);
    for (int i = 0; i < jumpAmount; ++i)
    {
      spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.jumpAnticipateAnimation);
      yield return (object) new WaitForSeconds(0.2f);
      AudioManager.Instance.PlayOneShot("event:/enemy/jump_large", spiderJumpMiniboss.transform.position);
      spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.jumpQuickAnimation);
      spiderJumpMiniboss.LookAtTarget();
      Vector3 vector3 = Vector3.ClampMagnitude(spiderJumpMiniboss.GetClosestTarget().transform.position - spiderJumpMiniboss.transform.position, spiderJumpMiniboss.jumpMaxDistance);
      spiderJumpMiniboss.transform.DOMove(spiderJumpMiniboss.transform.position + vector3, spiderJumpMiniboss.jumpDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
      float t = 0.0f;
      float dur = spiderJumpMiniboss.jumpDuration - 0.2f;
      while ((double) t < (double) dur)
      {
        t += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale;
        spiderJumpMiniboss.Spine.transform.localPosition = new Vector3(spiderJumpMiniboss.Spine.transform.localPosition.x, spiderJumpMiniboss.Spine.transform.localPosition.y, curve.Evaluate(t / dur) * -2f);
        yield return (object) null;
      }
      spiderJumpMiniboss.Spine.transform.localPosition = Vector3.zero;
      spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.landAnimation);
      spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.IdleAnimation, true);
      float time = 0.0f;
      while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 0.10000000149011612)
        yield return (object) null;
      CameraManager.instance.ShakeCameraForDuration(spiderJumpMiniboss.screenShake, spiderJumpMiniboss.screenShake, 0.1f);
      spiderJumpMiniboss.damageColliderEvents.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/enemy/land_large", spiderJumpMiniboss.transform.position);
      UnityEngine.Object.Instantiate<GameObject>(spiderJumpMiniboss.slamParticlePrefab, spiderJumpMiniboss.transform.position, Quaternion.identity);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 0.10000000149011612)
        yield return (object) null;
      spiderJumpMiniboss.damageColliderEvents.gameObject.SetActive(false);
      time = 0.0f;
      while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < (double) spiderJumpMiniboss.delayBetweenJumps - 0.10000000149011612)
        yield return (object) null;
    }
    spiderJumpMiniboss.IdleWait = 0.0f;
    spiderJumpMiniboss.SlamTimer = spiderJumpMiniboss.timeBetweenSlams;
    spiderJumpMiniboss.Attacking = false;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public void SpawnEnemies()
  {
    EnemySpiderJumpMiniboss.SpawnSet spawnSet = this.spawnSets[UnityEngine.Random.Range(0, this.spawnSets.Length)];
    float angle = (float) UnityEngine.Random.Range(0, 360);
    int num = (int) UnityEngine.Random.Range(spawnSet.Amount.x, spawnSet.Amount.y + 1f);
    for (int index = 0; index < num; ++index)
    {
      UnitObject component1 = ObjectPool.Spawn(spawnSet.loadedAddressableAssets[UnityEngine.Random.Range(0, spawnSet.loadedAddressableAssets.Count)].Result, this.transform.parent, this.transform.position, Quaternion.identity).GetComponent<UnitObject>();
      Interaction_Chest.Instance?.AddEnemy(component1.health);
      DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
      if ((bool) (UnityEngine.Object) component2)
        component2.GiveXP = false;
      foreach (SkeletonAnimation componentsInChild in component1.GetComponentsInChildren<SkeletonAnimation>())
      {
        componentsInChild.AnimationState.SetAnimation(0, "spawn", false);
        componentsInChild.AnimationState.AddAnimation(0, "idle", true, 0.0f);
      }
      component1.DoKnockBack(angle, this.spawnForce, 0.5f);
      angle = Utils.Repeat(angle + (float) (360 / num), 360f);
      component1.StartCoroutine((IEnumerator) this.DelayedEnemyHealthEnable(component1));
    }
  }

  public IEnumerator ProjectileAttack()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    spiderJumpMiniboss.Attacking = true;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.shootAnticipationAnimation);
    spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.shootAnimation);
    spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.IdleAnimation, true);
    spiderJumpMiniboss.SimpleSpineFlash.FlashWhite(false);
    float t = 0.0f;
    while ((double) t < (double) spiderJumpMiniboss.projectileAnticipation)
    {
      t += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale;
      spiderJumpMiniboss.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) spiderJumpMiniboss.projectileAnticipation * 0.75));
      spiderJumpMiniboss.LookAtTarget();
      yield return (object) null;
    }
    spiderJumpMiniboss.SimpleSpineFlash.FlashWhite(false);
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.attackSfx, spiderJumpMiniboss.gameObject);
    yield return (object) spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.projectilePattern.ShootIE());
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 1.0)
      yield return (object) null;
    spiderJumpMiniboss.projectileTimer = UnityEngine.Random.Range(spiderJumpMiniboss.timeBetweenProjectileAttacks.x, spiderJumpMiniboss.timeBetweenProjectileAttacks.y);
    spiderJumpMiniboss.Attacking = false;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  public IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    enemy.health.invincible = true;
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * spiderJumpMiniboss.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemy.health.invincible = false;
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    for (int index = 0; index < this.spawnSets.Length; ++index)
    {
      if (this.spawnSets[index].loadedAddressableAssets != null)
      {
        foreach (AsyncOperationHandle<GameObject> addressableAsset in this.spawnSets[index].loadedAddressableAssets)
          Addressables.Release((AsyncOperationHandle) addressableAsset);
        this.spawnSets[index].loadedAddressableAssets.Clear();
        this.spawnSets[index].loadedAddressableAssets = (List<AsyncOperationHandle<GameObject>>) null;
      }
    }
  }

  [Serializable]
  public struct SpawnSet
  {
    public AssetReferenceGameObject[] EnemiesList;
    public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets;
    public Vector2 Amount;
  }
}
