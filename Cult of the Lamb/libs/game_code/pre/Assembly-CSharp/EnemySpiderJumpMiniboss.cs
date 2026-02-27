// Decompiled with JetBrains decompiler
// Type: EnemySpiderJumpMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemySpiderJumpMiniboss : EnemySpider
{
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string swinAwayAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string swinInAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string StuckAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string UnstuckAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string shootAnticipationAnimation;
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  [SerializeField]
  private string shootAnimation;
  [Space]
  [SerializeField]
  private int attacksBeforeSlam;
  [SerializeField]
  private float slamAttackMinRange;
  [SerializeField]
  private float timeBetweenSlams;
  [SerializeField]
  private float slamInAirDuration;
  [SerializeField]
  private float slamLandDuration;
  [SerializeField]
  private float slamCooldown;
  [SerializeField]
  private float randomOffsetRadius = 0.25f;
  [SerializeField]
  private float screenShake;
  [SerializeField]
  private ProjectilePatternBase slamProjectilePattern;
  [SerializeField]
  private ProjectilePatternBase projectilePattern;
  [SerializeField]
  private float projectileAnticipation;
  [SerializeField]
  private Vector2 timeBetweenProjectileAttacks;
  [SerializeField]
  private EnemySpiderJumpMiniboss.SpawnSet[] spawnSets;
  [SerializeField]
  private float spawnForce;
  [SerializeField]
  private GameObject slamParticlePrefab;
  [SerializeField]
  private SpriteRenderer indicatorIcon;
  private Color indicatorColor = Color.white;
  private float SlamTimer;
  private int attackCounter;
  private float projectileTimer;
  private int UpdateEveryFrameNum = 5;
  private int curFrame;

  public override void OnEnable()
  {
    this.SlamTimer = this.timeBetweenSlams + UnityEngine.Random.Range(0.0f, 3f);
    this.indicatorIcon.gameObject.SetActive(false);
    this.projectileTimer = UnityEngine.Random.Range(this.timeBetweenProjectileAttacks.x, this.timeBetweenProjectileAttacks.y);
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

  protected override IEnumerator ActiveRoutine()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    yield return (object) new WaitForEndOfFrame();
    while (true)
    {
      while (GameManager.RoomActive)
      {
        if (spiderJumpMiniboss.state.CURRENT_STATE == StateMachine.State.Idle && (double) (spiderJumpMiniboss.IdleWait -= Time.deltaTime) <= 0.0 && (double) GameManager.GetInstance().CurrentTime > (double) spiderJumpMiniboss.initialMovementDelay)
        {
          if (spiderJumpMiniboss.wander && spiderJumpMiniboss.attackCounter < spiderJumpMiniboss.attacksBeforeSlam)
            spiderJumpMiniboss.GetNewTargetPosition();
          else
            spiderJumpMiniboss.Flee();
          spiderJumpMiniboss.speed = spiderJumpMiniboss.maxSpeed;
        }
        if ((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null && !spiderJumpMiniboss.Attacking && !spiderJumpMiniboss.IsStunned)
          spiderJumpMiniboss.state.LookAngle = Utils.GetAngle(spiderJumpMiniboss.transform.position, PlayerFarming.Instance.transform.position);
        else
          spiderJumpMiniboss.state.LookAngle = spiderJumpMiniboss.state.facingAngle;
        if (spiderJumpMiniboss.MovingAnimation != "" && (double) GameManager.GetInstance().CurrentTime > (double) spiderJumpMiniboss.initialMovementDelay)
        {
          if (spiderJumpMiniboss.state.CURRENT_STATE == StateMachine.State.Moving && spiderJumpMiniboss.Spine.AnimationName != spiderJumpMiniboss.MovingAnimation)
            spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.MovingAnimation, true);
          if (spiderJumpMiniboss.state.CURRENT_STATE == StateMachine.State.Idle && spiderJumpMiniboss.Spine.AnimationName != spiderJumpMiniboss.IdleAnimation)
            spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.IdleAnimation, true);
        }
        int num = UnityEngine.Random.Range(0, 7);
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
        yield return (object) null;
      }
      yield return (object) null;
    }
  }

  protected override bool ShouldAttack() => base.ShouldAttack();

  private bool ShouldSlam()
  {
    return (double) (this.SlamTimer -= Time.deltaTime) < 0.0 && !this.Attacking && (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) > (double) this.slamAttackMinRange && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  private bool ShouldProjectileAttack()
  {
    return (double) (this.projectileTimer -= Time.deltaTime) < 0.0 && !this.Attacking && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
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

  private void Slam() => this.StartCoroutine((IEnumerator) this.SlamRoutine());

  private IEnumerator SlamRoutine()
  {
    EnemySpiderJumpMiniboss spiderJumpMiniboss = this;
    spiderJumpMiniboss.Spine.ForceVisible = true;
    spiderJumpMiniboss.Attacking = true;
    spiderJumpMiniboss.updateDirection = false;
    spiderJumpMiniboss.ClearPaths();
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Attacking;
    spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.swinAwayAnimation);
    Vector3 targetShadowScale = spiderJumpMiniboss.ShadowSpriteRenderer.transform.localScale;
    spiderJumpMiniboss.ShadowSpriteRenderer.transform.DOScale(0.0f, 1f);
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.warningSfx, spiderJumpMiniboss.transform.position);
    yield return (object) new WaitForEndOfFrame();
    yield return (object) new WaitForSeconds(1f / spiderJumpMiniboss.Spine.timeScale);
    if (EnemySpider.EnemySpiders.Count < 3)
      spiderJumpMiniboss.SpawnEnemies();
    spiderJumpMiniboss.health.enabled = false;
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.jumpSfx, spiderJumpMiniboss.transform.position);
    yield return (object) new WaitForSeconds((float) (((double) spiderJumpMiniboss.slamInAirDuration - 1.0) / 2.0) / spiderJumpMiniboss.Spine.timeScale);
    spiderJumpMiniboss.transform.position = PlayerFarming.Instance.transform.position + (Vector3) UnityEngine.Random.insideUnitCircle * spiderJumpMiniboss.randomOffsetRadius;
    spiderJumpMiniboss.transform.position = new Vector3(Mathf.Clamp(spiderJumpMiniboss.transform.position.x, -6.5f, 6.5f), Mathf.Clamp(spiderJumpMiniboss.transform.position.y, -4f, 4f), spiderJumpMiniboss.transform.position.z);
    spiderJumpMiniboss.indicatorIcon.gameObject.SetActive(true);
    yield return (object) new WaitForSeconds((float) (((double) spiderJumpMiniboss.slamInAirDuration - 1.0) / 2.0) / spiderJumpMiniboss.Spine.timeScale);
    spiderJumpMiniboss.ShadowSpriteRenderer.transform.DOScale(targetShadowScale, spiderJumpMiniboss.slamLandDuration);
    yield return (object) new WaitForSeconds(spiderJumpMiniboss.slamLandDuration * 0.5f / spiderJumpMiniboss.Spine.timeScale);
    spiderJumpMiniboss.SetAnimation(spiderJumpMiniboss.swinInAnimation);
    spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.StuckAnimation);
    spiderJumpMiniboss.AddAnimation(spiderJumpMiniboss.UnstuckAnimation);
    yield return (object) new WaitForSeconds(spiderJumpMiniboss.slamLandDuration * 0.5f / spiderJumpMiniboss.Spine.timeScale);
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.stuckSfx, spiderJumpMiniboss.transform.position);
    CameraManager.instance.ShakeCameraForDuration(spiderJumpMiniboss.screenShake, spiderJumpMiniboss.screenShake, 0.1f);
    UnityEngine.Object.Instantiate<GameObject>(spiderJumpMiniboss.slamParticlePrefab, spiderJumpMiniboss.transform.position, Quaternion.identity);
    spiderJumpMiniboss.damageColliderEvents.gameObject.SetActive(true);
    spiderJumpMiniboss.indicatorIcon.gameObject.SetActive(false);
    spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.slamProjectilePattern.ShootIE());
    spiderJumpMiniboss.health.enabled = true;
    spiderJumpMiniboss.health.DontCombo = true;
    yield return (object) new WaitForSeconds(0.1f / spiderJumpMiniboss.Spine.timeScale);
    spiderJumpMiniboss.damageColliderEvents.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds((spiderJumpMiniboss.slamCooldown - 0.5f) / spiderJumpMiniboss.Spine.timeScale);
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.breakFreeSfx, spiderJumpMiniboss.transform.position);
    yield return (object) new WaitForSeconds(0.5f / spiderJumpMiniboss.Spine.timeScale);
    spiderJumpMiniboss.health.DontCombo = false;
    spiderJumpMiniboss.IdleWait = 0.0f;
    spiderJumpMiniboss.SlamTimer = spiderJumpMiniboss.timeBetweenSlams;
    spiderJumpMiniboss.Attacking = false;
    spiderJumpMiniboss.updateDirection = true;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
    spiderJumpMiniboss.Spine.ForceVisible = false;
  }

  private void SpawnEnemies()
  {
    EnemySpiderJumpMiniboss.SpawnSet spawnSet = this.spawnSets[UnityEngine.Random.Range(0, this.spawnSets.Length)];
    float randomStartAngle = (float) UnityEngine.Random.Range(0, 360);
    int randomAmount = (int) UnityEngine.Random.Range(spawnSet.Amount.x, spawnSet.Amount.y + 1f);
    for (int index = 0; index < randomAmount; ++index)
      Addressables.InstantiateAsync((object) spawnSet.EnemiesList[UnityEngine.Random.Range(0, spawnSet.EnemiesList.Length)], this.transform.position, Quaternion.identity, this.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        UnitObject component1 = obj.Result.GetComponent<UnitObject>();
        Interaction_Chest.Instance?.AddEnemy(component1.health);
        DropLootOnDeath component2 = component1.GetComponent<DropLootOnDeath>();
        if ((bool) (UnityEngine.Object) component2)
          component2.GiveXP = false;
        foreach (SkeletonAnimation componentsInChild in component1.GetComponentsInChildren<SkeletonAnimation>())
        {
          componentsInChild.AnimationState.SetAnimation(0, "spawn", false);
          componentsInChild.AnimationState.AddAnimation(0, "idle", true, 0.0f);
        }
        component1.DoKnockBack(randomStartAngle, this.spawnForce, 0.5f);
        randomStartAngle = Mathf.Repeat(randomStartAngle + (float) (360 / randomAmount), 360f);
        component1.StartCoroutine((IEnumerator) this.DelayedEnemyHealthEnable(component1));
      });
  }

  private IEnumerator ProjectileAttack()
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
      t += Time.deltaTime;
      spiderJumpMiniboss.SimpleSpineFlash.FlashWhite((float) ((double) t / (double) spiderJumpMiniboss.projectileAnticipation * 0.75));
      yield return (object) null;
    }
    spiderJumpMiniboss.SimpleSpineFlash.FlashWhite(false);
    AudioManager.Instance.PlayOneShot(spiderJumpMiniboss.attackSfx, spiderJumpMiniboss.gameObject);
    yield return (object) spiderJumpMiniboss.StartCoroutine((IEnumerator) spiderJumpMiniboss.projectilePattern.ShootIE());
    yield return (object) new WaitForSeconds(1f / spiderJumpMiniboss.Spine.timeScale);
    spiderJumpMiniboss.projectileTimer = UnityEngine.Random.Range(spiderJumpMiniboss.timeBetweenProjectileAttacks.x, spiderJumpMiniboss.timeBetweenProjectileAttacks.y);
    spiderJumpMiniboss.Attacking = false;
    spiderJumpMiniboss.state.CURRENT_STATE = StateMachine.State.Idle;
  }

  private IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    enemy.health.invincible = true;
    yield return (object) new WaitForSeconds(0.5f);
    enemy.health.invincible = false;
  }

  [Serializable]
  private struct SpawnSet
  {
    public AssetReferenceGameObject[] EnemiesList;
    public Vector2 Amount;
  }
}
