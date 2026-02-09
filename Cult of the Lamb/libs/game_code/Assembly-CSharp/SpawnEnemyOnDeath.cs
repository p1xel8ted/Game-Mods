// Decompiled with JetBrains decompiler
// Type: SpawnEnemyOnDeath
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class SpawnEnemyOnDeath : BaseMonoBehaviour
{
  [SerializeField]
  public AssetReferenceGameObject[] enemiesList;
  [SerializeField]
  public int amount = 2;
  [SerializeField]
  public Vector2 randomSpawnAmountOffset = new Vector2(0.0f, 0.0f);
  [SerializeField]
  [Range(0.0f, 1f)]
  public float spawnChance = 1f;
  [SerializeField]
  public float spawnForce;
  [SerializeField]
  public float growDuration;
  [SerializeField]
  public string spawnAnimation = "";
  public Health health;
  [CompilerGenerated]
  public UnitObject[] \u003CSpawnedEnemies\u003Ek__BackingField;
  public LayerMask islandMask;
  [CompilerGenerated]
  public bool \u003CSpawnEnemies\u003Ek__BackingField = true;

  public int Amount
  {
    get => this.amount;
    set => this.amount = value;
  }

  public event SpawnEnemyOnDeath.SpawnEvent OnEnemySpawned;

  public event SpawnEnemyOnDeath.SpawnEvent OnEnemyDespawned;

  public UnitObject[] SpawnedEnemies
  {
    get => this.\u003CSpawnedEnemies\u003Ek__BackingField;
    set => this.\u003CSpawnedEnemies\u003Ek__BackingField = value;
  }

  public bool SpawnEnemies
  {
    get => this.\u003CSpawnEnemies\u003Ek__BackingField;
    set => this.\u003CSpawnEnemies\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    this.health = this.GetComponent<Health>();
    this.health.OnDie += new Health.DieAction(this.OnDie);
    this.islandMask = (LayerMask) ((int) this.islandMask | 1 << LayerMask.NameToLayer("Island"));
  }

  public void OnDestroy()
  {
    if (!(bool) (UnityEngine.Object) this.health)
      return;
    this.health.OnDie -= new Health.DieAction(this.OnDie);
  }

  public void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    if (!this.SpawnEnemies)
      return;
    this.SpawnEnemies = false;
    if ((double) UnityEngine.Random.value > (double) this.spawnChance)
      return;
    int num = this.amount + UnityEngine.Random.Range((int) this.randomSpawnAmountOffset.x, (int) this.randomSpawnAmountOffset.y);
    this.SpawnedEnemies = new UnitObject[num + 1];
    float randomStartAngle = (float) UnityEngine.Random.Range(0, 360);
    Health.team2.Add((Health) null);
    Interaction_Chest.Instance?.Enemies.Add((Health) null);
    SpawnEnemyOnDeath.SpawnEvent onEnemySpawned1 = this.OnEnemySpawned;
    if (onEnemySpawned1 != null)
      onEnemySpawned1((UnitObject) null);
    for (int i = 0; i < num; i++)
    {
      Addressables_wrapper.InstantiateAsync((object) this.enemiesList[UnityEngine.Random.Range(0, this.enemiesList.Length)], this.transform.position, Quaternion.identity, this.transform.parent, (Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        if (Health.team2.Contains((Health) null))
        {
          Health.team2.Remove((Health) null);
          SpawnEnemyOnDeath.SpawnEvent onEnemyDespawned = this.OnEnemyDespawned;
          if (onEnemyDespawned != null)
            onEnemyDespawned((UnitObject) null);
          Interaction_Chest.Instance?.Enemies.Remove((Health) null);
        }
        UnitObject component1 = obj.Result.GetComponent<UnitObject>();
        component1.CanHaveModifier = false;
        component1.RemoveModifier();
        Interaction_Chest.Instance?.AddEnemy(component1.health);
        EnemyRoundsBase.Instance?.AddEnemyToRound(component1.GetComponent<Health>());
        this.SpawnedEnemies[i] = component1;
        component1.RemoveModifier();
        SpawnEnemyOnDeath component2 = obj.Result.GetComponent<SpawnEnemyOnDeath>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          component2.enabled = false;
        if ((double) this.growDuration != 0.0)
        {
          Vector3 localScale = component1.transform.localScale;
          component1.transform.localScale = Vector3.zero;
          component1.transform.DOScale(localScale, this.growDuration).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.Linear);
        }
        if (!string.IsNullOrEmpty(this.spawnAnimation))
        {
          foreach (SkeletonAnimation componentsInChild in component1.GetComponentsInChildren<SkeletonAnimation>())
          {
            componentsInChild.AnimationState.SetAnimation(0, this.spawnAnimation, false);
            componentsInChild.AnimationState.AddAnimation(0, "idle", true, 0.0f);
          }
        }
        component1.DoKnockBack(randomStartAngle, this.spawnForce, 0.5f);
        component1.StartCoroutine((IEnumerator) this.DelayedEnemyHealthEnable(component1));
        SpawnEnemyOnDeath.SpawnEvent onEnemySpawned2 = this.OnEnemySpawned;
        if (onEnemySpawned2 == null)
          return;
        onEnemySpawned2(component1);
      }));
      if ((bool) Physics2D.Raycast((Vector2) this.transform.position, Utils.DegreeToVector2(randomStartAngle), 0.5f, (int) this.islandMask))
        randomStartAngle = Utils.Repeat(randomStartAngle + 180f, 360f);
      randomStartAngle = Utils.Repeat(randomStartAngle + (float) (360 / num), 360f);
    }
  }

  public IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    enemy.health.invincible = true;
    yield return (object) new WaitForSeconds(0.5f);
    enemy.health.invincible = false;
    foreach (Collider2D collider2D in Physics2D.OverlapCircleAll((Vector2) enemy.transform.position, 0.5f))
    {
      Health component = collider2D.GetComponent<Health>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.team == Health.Team.Neutral)
        collider2D.GetComponent<Health>().DealDamage((float) int.MaxValue, enemy.gameObject, Vector3.Lerp(component.transform.position, enemy.transform.position, 0.7f));
    }
  }

  public delegate void SpawnEvent(UnitObject spawnedEnemies);
}
