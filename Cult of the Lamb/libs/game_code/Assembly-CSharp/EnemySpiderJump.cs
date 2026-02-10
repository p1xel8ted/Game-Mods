// Decompiled with JetBrains decompiler
// Type: EnemySpiderJump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderJump : EnemySpider
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
  public GameObject slamParticlePrefab;
  [SerializeField]
  public SpriteRenderer indicatorIcon;
  public float SlamTimer;
  public float flashTickTimer;
  public Color indicatorColor = Color.red;
  public ShowHPBar hpBar;

  public override void OnEnable()
  {
    this.SlamTimer = this.timeBetweenSlams + Random.Range(0.0f, 3f);
    this.hpBar = this.GetComponent<ShowHPBar>();
    this.health.DontCombo = false;
    this.indicatorIcon?.gameObject.SetActive(false);
    base.OnEnable();
  }

  public void Start()
  {
    if (!((Object) SkeletonAnimationLODGlobalManager.Instance != (Object) null))
      return;
    SkeletonAnimationLODGlobalManager.Instance.DisableCulling(this.Spine.transform, this.Spine);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.StopAllCoroutines();
  }

  public override bool ShouldAttack()
  {
    return (double) (this.SlamTimer -= Time.deltaTime) < 0.0 && (Object) this.TargetEnemy != (Object) null && !this.Attacking && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > (double) this.slamAttackMinRange && (double) GameManager.GetInstance().CurrentTime > (double) this.initialAttackDelayTimer;
  }

  public override void Update()
  {
    base.Update();
    if ((double) Time.timeScale == 0.0)
      return;
    this.AttackingTargetPosition = this.transform.position - Vector3.up;
    if (!this.indicatorIcon.gameObject.activeSelf)
      return;
    if ((double) this.flashTickTimer >= 0.11999999731779099 && BiomeConstants.Instance.IsFlashLightsActive)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
      this.flashTickTimer = 0.0f;
    }
    else
      this.flashTickTimer += Time.deltaTime;
  }

  public void Slam() => this.StartCoroutine((IEnumerator) this.AttackRoutine());

  public override IEnumerator AttackRoutine()
  {
    EnemySpiderJump enemySpiderJump = this;
    enemySpiderJump.Spine.ForceVisible = true;
    enemySpiderJump.Attacking = true;
    enemySpiderJump.updateDirection = false;
    enemySpiderJump.ClearPaths();
    enemySpiderJump.state.CURRENT_STATE = StateMachine.State.Attacking;
    enemySpiderJump.SetAnimation(enemySpiderJump.swinAwayAnimation);
    AudioManager.Instance.PlayOneShot(enemySpiderJump.warningSfx, enemySpiderJump.transform.position);
    Vector3 targetShadowScale = Vector3.zero;
    if ((bool) (Object) enemySpiderJump.ShadowSpriteRenderer)
    {
      targetShadowScale = enemySpiderJump.ShadowSpriteRenderer.transform.localScale;
      enemySpiderJump.ShadowSpriteRenderer.transform.DOScale(0.0f, 1f);
    }
    yield return (object) new WaitForEndOfFrame();
    if ((bool) (Object) enemySpiderJump.Body.Spine)
      enemySpiderJump.Body.Spine.skeleton.ScaleX = 1f;
    enemySpiderJump.Body.transform.localScale = new Vector3(enemySpiderJump.Spine.skeleton.ScaleX, 1f, 1f);
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < 1.0)
      yield return (object) null;
    enemySpiderJump.hpBar.Hide();
    enemySpiderJump.health.invincible = true;
    AudioManager.Instance.PlayOneShot(enemySpiderJump.jumpSfx, enemySpiderJump.transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < ((double) enemySpiderJump.slamInAirDuration - 1.0) / 2.0)
      yield return (object) null;
    if ((Object) enemySpiderJump.ColliderRadius == (Object) null)
      enemySpiderJump.ColliderRadius = enemySpiderJump.GetComponent<CircleCollider2D>();
    enemySpiderJump.ColliderRadius.enabled = false;
    enemySpiderJump.indicatorIcon.gameObject.SetActive(true);
    Vector3 a = ((Object) enemySpiderJump.TargetEnemy != (Object) null ? enemySpiderJump.TargetEnemy.transform.position : enemySpiderJump.transform.position) + (Vector3) Random.insideUnitCircle * enemySpiderJump.randomOffsetRadius;
    Vector3 normalized = (a - enemySpiderJump.transform.position).normalized;
    float distance = Vector3.Distance(a, enemySpiderJump.transform.position);
    if ((Object) Physics2D.Raycast((Vector2) enemySpiderJump.transform.position, (Vector2) normalized, distance, (int) enemySpiderJump.layerToCheck).collider != (Object) null)
      a = (Object) enemySpiderJump.TargetEnemy != (Object) null ? enemySpiderJump.TargetEnemy.transform.position : enemySpiderJump.transform.position;
    enemySpiderJump.transform.position = a;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < ((double) enemySpiderJump.slamInAirDuration - 1.0) / 2.0)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(enemySpiderJump.attackSfx, enemySpiderJump.transform.position);
    SpriteRenderer shadowSpriteRenderer = enemySpiderJump.ShadowSpriteRenderer;
    if (shadowSpriteRenderer != null)
      shadowSpriteRenderer.transform.DOScale(targetShadowScale, enemySpiderJump.slamLandDuration);
    enemySpiderJump.SetAnimation(enemySpiderJump.swinInAnimation);
    enemySpiderJump.AddAnimation(enemySpiderJump.StuckAnimation);
    enemySpiderJump.AddAnimation(enemySpiderJump.UnstuckAnimation);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < (double) enemySpiderJump.slamLandDuration)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(enemySpiderJump.stuckSfx, enemySpiderJump.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/impact_blunt", enemySpiderJump.transform.position);
    enemySpiderJump.indicatorIcon.gameObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(enemySpiderJump.screenShake, enemySpiderJump.screenShake, 0.1f);
    Object.Instantiate<GameObject>(enemySpiderJump.slamParticlePrefab, enemySpiderJump.transform.position, Quaternion.identity);
    enemySpiderJump.ColliderRadius.enabled = true;
    enemySpiderJump.damageColliderEvents.gameObject.SetActive(true);
    enemySpiderJump.health.invincible = false;
    enemySpiderJump.health.DontCombo = true;
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < 0.10000000149011612)
      yield return (object) null;
    enemySpiderJump.damageColliderEvents.gameObject.SetActive(false);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < (double) enemySpiderJump.slamCooldown - 0.5)
      yield return (object) null;
    AudioManager.Instance.PlayOneShot(enemySpiderJump.breakFreeSfx, enemySpiderJump.transform.position);
    time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderJump.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySpiderJump.health.DontCombo = false;
    enemySpiderJump.TargetEnemy = (Health) null;
    enemySpiderJump.IdleWait = 0.0f;
    enemySpiderJump.SlamTimer = enemySpiderJump.timeBetweenSlams;
    enemySpiderJump.Attacking = false;
    enemySpiderJump.updateDirection = true;
    enemySpiderJump.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiderJump.Spine.ForceVisible = false;
  }

  public override void DoKnockBack(
    GameObject Attacker,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
      return;
    base.DoKnockBack(Attacker, KnockbackModifier, Duration, appendForce);
  }

  public override void DoKnockBack(
    float angle,
    float KnockbackModifier,
    float Duration,
    bool appendForce = true)
  {
    if (this.state.CURRENT_STATE == StateMachine.State.Attacking)
      return;
    base.DoKnockBack(angle, KnockbackModifier, Duration, appendForce);
  }
}
