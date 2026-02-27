// Decompiled with JetBrains decompiler
// Type: EnemySpiderJump
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderJump : EnemySpider
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
  private GameObject slamParticlePrefab;
  [SerializeField]
  private SpriteRenderer indicatorIcon;
  private float SlamTimer;
  private Color indicatorColor = Color.white;
  private ShowHPBar hpBar;
  private int UpdateEveryFrameNum = 5;
  private int curFrame;

  public override void OnEnable()
  {
    this.SlamTimer = this.timeBetweenSlams + Random.Range(0.0f, 3f);
    this.hpBar = this.GetComponent<ShowHPBar>();
    this.health.DontCombo = false;
    this.indicatorIcon?.gameObject.SetActive(false);
    base.OnEnable();
  }

  public override void OnDisable()
  {
    base.OnDisable();
    this.StopAllCoroutines();
  }

  protected override bool ShouldAttack()
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
    if (this.curFrame == this.UpdateEveryFrameNum)
    {
      this.indicatorColor = this.indicatorColor == Color.white ? Color.red : Color.white;
      this.indicatorIcon.material.SetColor("_Color", this.indicatorColor);
      this.curFrame = 0;
    }
    else
      ++this.curFrame;
  }

  private void Slam() => this.StartCoroutine((IEnumerator) this.AttackRoutine());

  protected override IEnumerator AttackRoutine()
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
    yield return (object) new WaitForSeconds(1f / enemySpiderJump.Spine.timeScale);
    enemySpiderJump.hpBar.Hide();
    enemySpiderJump.health.invincible = true;
    AudioManager.Instance.PlayOneShot(enemySpiderJump.jumpSfx, enemySpiderJump.transform.position);
    yield return (object) new WaitForSeconds((float) (((double) enemySpiderJump.slamInAirDuration - 1.0) / 2.0) / enemySpiderJump.Spine.timeScale);
    enemySpiderJump.indicatorIcon.gameObject.SetActive(true);
    Vector3 a = ((Object) enemySpiderJump.TargetEnemy != (Object) null ? enemySpiderJump.TargetEnemy.transform.position : enemySpiderJump.transform.position) + (Vector3) Random.insideUnitCircle * enemySpiderJump.randomOffsetRadius;
    Vector3 normalized = (a - enemySpiderJump.transform.position).normalized;
    float distance = Vector3.Distance(a, enemySpiderJump.transform.position);
    if ((Object) Physics2D.Raycast((Vector2) enemySpiderJump.transform.position, (Vector2) normalized, distance, (int) enemySpiderJump.layerToCheck).collider != (Object) null)
      a = (Object) enemySpiderJump.TargetEnemy != (Object) null ? enemySpiderJump.TargetEnemy.transform.position : enemySpiderJump.transform.position;
    enemySpiderJump.transform.position = a;
    yield return (object) new WaitForSeconds((float) (((double) enemySpiderJump.slamInAirDuration - 1.0) / 2.0) / enemySpiderJump.Spine.timeScale);
    AudioManager.Instance.PlayOneShot(enemySpiderJump.attackSfx, enemySpiderJump.transform.position);
    SpriteRenderer shadowSpriteRenderer = enemySpiderJump.ShadowSpriteRenderer;
    if (shadowSpriteRenderer != null)
      shadowSpriteRenderer.transform.DOScale(targetShadowScale, enemySpiderJump.slamLandDuration);
    enemySpiderJump.SetAnimation(enemySpiderJump.swinInAnimation);
    enemySpiderJump.AddAnimation(enemySpiderJump.StuckAnimation);
    enemySpiderJump.AddAnimation(enemySpiderJump.UnstuckAnimation);
    yield return (object) new WaitForSeconds(enemySpiderJump.slamLandDuration / enemySpiderJump.Spine.timeScale);
    AudioManager.Instance.PlayOneShot(enemySpiderJump.stuckSfx, enemySpiderJump.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/impact_blunt", enemySpiderJump.transform.position);
    enemySpiderJump.indicatorIcon.gameObject.SetActive(false);
    CameraManager.instance.ShakeCameraForDuration(enemySpiderJump.screenShake, enemySpiderJump.screenShake, 0.1f);
    Object.Instantiate<GameObject>(enemySpiderJump.slamParticlePrefab, enemySpiderJump.transform.position, Quaternion.identity);
    enemySpiderJump.damageColliderEvents.gameObject.SetActive(true);
    enemySpiderJump.health.invincible = false;
    enemySpiderJump.health.DontCombo = true;
    yield return (object) new WaitForSeconds(0.1f / enemySpiderJump.Spine.timeScale);
    enemySpiderJump.damageColliderEvents.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds((enemySpiderJump.slamCooldown - 0.5f) / enemySpiderJump.Spine.timeScale);
    AudioManager.Instance.PlayOneShot(enemySpiderJump.breakFreeSfx, enemySpiderJump.transform.position);
    yield return (object) new WaitForSeconds(0.5f / enemySpiderJump.Spine.timeScale);
    enemySpiderJump.health.DontCombo = false;
    enemySpiderJump.TargetEnemy = (Health) null;
    enemySpiderJump.IdleWait = 0.0f;
    enemySpiderJump.SlamTimer = enemySpiderJump.timeBetweenSlams;
    enemySpiderJump.Attacking = false;
    enemySpiderJump.updateDirection = true;
    enemySpiderJump.state.CURRENT_STATE = StateMachine.State.Idle;
    enemySpiderJump.Spine.ForceVisible = false;
  }
}
