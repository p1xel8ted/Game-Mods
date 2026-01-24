// Decompiled with JetBrains decompiler
// Type: BreakableSpiderNest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class BreakableSpiderNest : BaseMonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public float duration;
  [SerializeField]
  public AssetReferenceGameObject[] enemiesList;
  [SerializeField]
  public Vector2 amount;
  [SerializeField]
  public AnimationCurve heightCurve;
  [SerializeField]
  public AnimationCurve moveCurve;
  [SerializeField]
  public float height;
  [SerializeField]
  public float radius;
  [SerializeField]
  public float popOutDuration;
  [SerializeField]
  public float growDuration;
  [SerializeField]
  public string spawnAnimation = "";
  [SerializeField]
  public GameObject spawnParticle;
  [SerializeField]
  public Renderer renderer;
  [SerializeField]
  public LayerMask layersToCheck;
  public float spawnTimestamp = -1f;
  public ShowHPBar hpBar;
  public Health health;
  public Color originalColor;
  public bool spawned;

  public void OnEnable()
  {
    this.health = this.GetComponent<Health>();
    this.hpBar = this.GetComponent<ShowHPBar>();
    this.health.OnHit += new Health.HitAction(this.OnHit);
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.originalColor = this.renderer.material.color;
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.OnRoomCleared);
    this.StartCoroutine((IEnumerator) this.Sub());
  }

  public IEnumerator Sub()
  {
    BreakableSpiderNest breakableSpiderNest = this;
    while (breakableSpiderNest.spine.AnimationState == null)
      yield return (object) null;
    breakableSpiderNest.spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(breakableSpiderNest.AnimationState_Event);
  }

  public void OnDisable()
  {
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.OnRoomCleared);
    if (this.spine.AnimationState == null)
      return;
    this.spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "break"))
      return;
    AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile");
  }

  public void OnHit(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health.AttackTypes AttackType,
    bool FromBehind = false)
  {
    this.StartCoroutine((IEnumerator) this.HitIE());
  }

  public IEnumerator HitIE()
  {
    this.renderer.material.color = Color.red;
    yield return (object) new WaitForSeconds(0.1f);
    this.renderer.material.color = this.originalColor;
  }

  public void Update()
  {
    if (PlayerRelic.TimeFrozen || Health.isGlobalTimeFreeze)
    {
      if (!((UnityEngine.Object) this.spine != (UnityEngine.Object) null))
        return;
      this.spine.timeScale = 0.0001f;
    }
    else
    {
      if ((UnityEngine.Object) this.spine != (UnityEngine.Object) null)
        this.spine.timeScale = 1f;
      if ((double) this.spawnTimestamp == -1.0)
      {
        if (!GameManager.RoomActive)
          return;
        this.spawnTimestamp = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime + this.duration : Time.time + this.duration;
      }
      else
      {
        if (BiomeGenerator.Instance.CurrentRoom.Completed)
          return;
        float? currentTime = GameManager.GetInstance()?.CurrentTime;
        float spawnTimestamp = this.spawnTimestamp;
        if (!((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue))
          return;
        this.SpawnEnemies();
      }
    }
  }

  public void OnRoomCleared()
  {
    this.StopAllCoroutines();
    this.health.DealDamage(999f, this.gameObject, this.transform.position);
  }

  public void SpawnEnemies()
  {
    if (this.spawned || BiomeGenerator.Instance.CurrentRoom.Completed)
      return;
    this.spawned = true;
    AudioManager.Instance.PlayOneShot("event:/boss/frog/tongue_impact", this.gameObject);
    Interaction_Chest.Instance?.Enemies.Add((Health) null);
    int num = (int) UnityEngine.Random.Range(this.amount.x, this.amount.y + 1f);
    for (int index = 0; index < num && !BiomeGenerator.Instance.CurrentRoom.Completed; ++index)
      Addressables_wrapper.InstantiateAsync((object) this.enemiesList[UnityEngine.Random.Range(0, this.enemiesList.Length)], this.transform.position, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        if (BiomeGenerator.Instance.CurrentRoom.Completed)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) obj.Result);
          Addressables.Release<GameObject>(obj);
        }
        else
        {
          Interaction_Chest.Instance?.Enemies.Remove((Health) null);
          EnemySpider component = obj.Result.GetComponent<EnemySpider>();
          Interaction_Chest.Instance?.AddEnemy(component.health);
          EnemyRoundsBase.Instance?.AddEnemyToRound(component.health);
          component.health.CanIncreaseDamageMultiplier = this.health.CanIncreaseDamageMultiplier;
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
        }
      }));
    this.health.DealDamage(this.health.totalHP, this.gameObject, this.transform.position);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    this.gameObject.SetActive(false);
    Health.team2.Remove(this.health);
    Interaction_Chest.Instance?.Enemies.Remove(this.health);
    Bounds bounds = this.GetComponent<Collider2D>().bounds;
    AstarPath.active.UpdateGraphs(bounds);
  }

  public IEnumerator SpawnAnimIE(EnemySpider enemy)
  {
    Vector3 fromPosition = enemy.transform.position;
    Vector3 targetPosition = fromPosition + (Vector3) UnityEngine.Random.insideUnitCircle * this.radius;
    float startTime = GameManager.GetInstance().CurrentTime;
    for (int index = 0; index < 100; ++index)
    {
      targetPosition = fromPosition + (Vector3) UnityEngine.Random.insideUnitCircle * this.radius;
      if ((UnityEngine.Object) Physics2D.Raycast((Vector2) fromPosition, (Vector2) (targetPosition - fromPosition).normalized, this.radius, (int) this.layersToCheck).collider == (UnityEngine.Object) null)
        break;
    }
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

  public IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
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

  public void OnDrawGizmos() => Utils.DrawCircleXY(this.transform.position, this.radius, Color.red);

  [CompilerGenerated]
  public void \u003CSpawnEnemies\u003Eb__27_0(AsyncOperationHandle<GameObject> obj)
  {
    if (BiomeGenerator.Instance.CurrentRoom.Completed)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) obj.Result);
      Addressables.Release<GameObject>(obj);
    }
    else
    {
      Interaction_Chest.Instance?.Enemies.Remove((Health) null);
      EnemySpider component = obj.Result.GetComponent<EnemySpider>();
      Interaction_Chest.Instance?.AddEnemy(component.health);
      EnemyRoundsBase.Instance?.AddEnemyToRound(component.health);
      component.health.CanIncreaseDamageMultiplier = this.health.CanIncreaseDamageMultiplier;
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
    }
  }
}
