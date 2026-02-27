// Decompiled with JetBrains decompiler
// Type: BreakableSpiderNest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class BreakableSpiderNest : BaseMonoBehaviour
{
  [SerializeField]
  private SkeletonAnimation spine;
  [SerializeField]
  private float duration;
  [SerializeField]
  public AssetReferenceGameObject[] enemiesList;
  [SerializeField]
  private Vector2 amount;
  [SerializeField]
  private AnimationCurve heightCurve;
  [SerializeField]
  private AnimationCurve moveCurve;
  [SerializeField]
  private float height;
  [SerializeField]
  private float radius;
  [SerializeField]
  private float popOutDuration;
  [SerializeField]
  private float growDuration;
  [SerializeField]
  private string spawnAnimation = "";
  [SerializeField]
  private GameObject spawnParticle;
  [SerializeField]
  private Renderer renderer;
  [SerializeField]
  private LayerMask layersToCheck;
  private float spawnTimestamp = -1f;
  private ShowHPBar hpBar;
  private Health health;
  private Color originalColor;
  private bool spawned;

  private void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.hpBar = this.GetComponent<ShowHPBar>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.originalColor = this.renderer.material.color;
    this.StartCoroutine((IEnumerator) this.Sub());
  }

  private IEnumerator Sub()
  {
    BreakableSpiderNest breakableSpiderNest = this;
    while (breakableSpiderNest.spine.AnimationState == null)
      yield return (object) null;
    breakableSpiderNest.spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(breakableSpiderNest.AnimationState_Event);
  }

  private void OnDisable()
  {
    if (this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  private void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "break"))
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile");
  }

  private void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.StartCoroutine((IEnumerator) this.HitIE());
  }

  private IEnumerator HitIE()
  {
    this.renderer.material.color = Color.red;
    yield return (object) new WaitForSeconds(0.1f);
    this.renderer.material.color = this.originalColor;
  }

  private void Update()
  {
    if ((double) this.spawnTimestamp == -1.0)
    {
      if (!GameManager.RoomActive)
        return;
      this.spawnTimestamp = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime + this.duration : Time.time + this.duration;
    }
    else
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float spawnTimestamp = this.spawnTimestamp;
      if (!((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue))
        return;
      this.SpawnEnemies();
    }
  }

  private void SpawnEnemies()
  {
    if (this.spawned)
      return;
    this.spawned = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact", this.gameObject);
    int num = (int) UnityEngine.Random.Range(this.amount.x, this.amount.y + 1f);
    for (int index = 0; index < num; ++index)
      Addressables.InstantiateAsync((object) this.enemiesList[UnityEngine.Random.Range(0, this.enemiesList.Length)], this.transform.position, Quaternion.identity, this.transform.parent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        EnemySpider component = obj.Result.GetComponent<EnemySpider>();
        Interaction_Chest.Instance?.AddEnemy(component.health);
        EnemyRoundsBase.Instance?.AddEnemyToRound(component.health);
        foreach (SkeletonAnimation componentsInChild in component.GetComponentsInChildren<SkeletonAnimation>())
        {
          if ((double) this.growDuration != 0.0)
          {
            Vector3 localScale = componentsInChild.transform.localScale;
            componentsInChild.transform.localScale = Vector3.zero;
            componentsInChild.transform.DOScale(localScale, this.growDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
          }
          if (!string.IsNullOrEmpty(this.spawnAnimation))
          {
            componentsInChild.AnimationState.SetAnimation(0, this.spawnAnimation, false);
            componentsInChild.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        component.StartCoroutine((IEnumerator) this.SpawnAnimIE(component));
        component.StartCoroutine((IEnumerator) this.DelayedEnemyHealthEnable((UnitObject) component));
        UnityEngine.Object.Instantiate<GameObject>(this.spawnParticle, this.transform.position, Quaternion.identity);
        this.hpBar.DestroyHPBar();
      });
    this.health.DealDamage(this.health.totalHP, this.gameObject, this.transform.position);
  }

  private void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.gameObject.SetActive(false);
    Bounds bounds = this.GetComponent<Collider2D>().bounds;
    AstarPath.active.UpdateGraphs(bounds);
  }

  private IEnumerator SpawnAnimIE(EnemySpider enemy)
  {
    Vector3 fromPosition = enemy.transform.position;
    Vector3 targetPosition = fromPosition + (Vector3) UnityEngine.Random.insideUnitCircle * this.radius;
    float startTime = GameManager.GetInstance().CurrentTime;
    Vector3 normalized = (targetPosition - fromPosition).normalized;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) fromPosition, this.radius, (Vector2) normalized, 0.0f, (int) this.layersToCheck);
    if ((bool) (UnityEngine.Object) raycastHit2D.collider && (double) Vector3.Dot((Vector3) raycastHit2D.point - fromPosition, normalized) > 0.0)
      targetPosition = fromPosition + (fromPosition - (Vector3) raycastHit2D.point).normalized * this.radius;
    float t = 0.0f;
    while ((double) t < (double) this.popOutDuration)
    {
      float time = GameManager.GetInstance().TimeSince(startTime) / this.popOutDuration;
      if ((UnityEngine.Object) enemy.Spine.transform.parent == (UnityEngine.Object) enemy.transform)
        enemy.Spine.transform.localPosition = -Vector3.forward * this.heightCurve.Evaluate(time) * this.height;
      else
        enemy.Spine.transform.position = enemy.transform.TransformPoint(-Vector3.forward * this.heightCurve.Evaluate(time) * this.height);
      enemy.transform.position = Vector3.Lerp(fromPosition, targetPosition, this.moveCurve.Evaluate(time));
      t += Time.deltaTime;
      yield return (object) null;
      if ((UnityEngine.Object) enemy.Spine.transform.parent == (UnityEngine.Object) enemy.transform)
        enemy.Spine.transform.localPosition = Vector3.zero;
      else
        enemy.Spine.transform.position = enemy.transform.TransformPoint(Vector3.zero);
    }
    enemy.transform.position = enemy.Spine.transform.position;
    enemy.Spine.transform.localPosition = Vector3.zero;
  }

  private IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    enemy.health.enabled = false;
    yield return (object) new WaitForSeconds(0.5f);
    enemy.health.enabled = true;
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) enemy.transform.position, 0.5f))
    {
      Health component = collider2D.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team == Health.Team.Neutral)
        collider2D.GetComponent<Health>().DealDamage((float) int.MaxValue, enemy.gameObject, Vector3.Lerp(component.transform.position, enemy.transform.position, 0.7f));
    }
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.radius, Color.red);
  }
}
