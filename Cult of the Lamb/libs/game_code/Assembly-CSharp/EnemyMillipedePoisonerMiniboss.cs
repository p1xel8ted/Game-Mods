// Decompiled with JetBrains decompiler
// Type: EnemyMillipedePoisonerMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyMillipedePoisonerMiniboss : EnemyMillipedeSpiker
{
  [SerializeField]
  public EnemyBomb bombPrefab;
  [SerializeField]
  public float shootAnticipation;
  [SerializeField]
  public float bombDuration;
  [SerializeField]
  public Vector2 amountToShoot;
  [SerializeField]
  public float timeBetweenShots;
  [SerializeField]
  public float shootingCooldown;
  [Space]
  [SerializeField]
  public float aggressionSpeedMultiplier;
  [SerializeField]
  public Vector2 aggressionDuration;
  [SerializeField]
  public Vector2 timeBetweenAggression;
  [SerializeField]
  public AssetReferenceGameObject[] spawnable;
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  public string shootAnimation;
  public float shootTimestamp;
  public float aggressionTimestamp;
  public bool aggressive;
  public bool spawnedSecondWave;
  public bool spawnedThirdWave;
  public List<Vector3> spawnPositions = new List<Vector3>(2)
  {
    new Vector3(-3f, 0.0f, 0.0f),
    new Vector3(3f, 0.0f, 0.0f)
  };
  public List<AsyncOperationHandle<GameObject>> loadedAddressableAssets = new List<AsyncOperationHandle<GameObject>>();
  public int spawnCount = 2;

  public void Preload()
  {
    for (int index = 0; index < this.spawnable.Length; ++index)
    {
      AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) this.spawnable[index]);
      asyncOperationHandle.Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        this.loadedAddressableAssets.Add(obj);
        obj.Result.CreatePool(this.spawnCount, true);
      });
      asyncOperationHandle.WaitForCompletion();
    }
  }

  public IEnumerator Start()
  {
    yield return (object) null;
    this.Preload();
    this.\u003C\u003En__0();
    this.SpawnEnemies();
    this.aggressionTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.aggressionDuration.x, this.aggressionDuration.y);
  }

  public void SpawnEnemies()
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/attack", this.gameObject);
    for (int index = 0; index < this.spawnCount; ++index)
    {
      GameObject Spawn = ObjectPool.Spawn(this.loadedAddressableAssets[UnityEngine.Random.Range(0, this.loadedAddressableAssets.Count)].Result, this.transform.parent, this.spawnPositions[index], Quaternion.identity);
      Spawn.gameObject.SetActive(false);
      EnemySpawner.CreateWithAndInitInstantiatedEnemy(Spawn.transform.position, this.transform.parent, Spawn);
    }
  }

  public override void OnEnable()
  {
    base.OnEnable();
    this.shootTimestamp = (bool) (UnityEngine.Object) GameManager.GetInstance() ? GameManager.GetInstance().CurrentTime + this.shootingCooldown : Time.time + this.shootingCooldown;
  }

  public override void Update()
  {
    base.Update();
    float? currentTime = GameManager.GetInstance()?.CurrentTime;
    float num1 = this.shootTimestamp / this.Spine.timeScale;
    if ((double) currentTime.GetValueOrDefault() > (double) num1 & currentTime.HasValue && !this.attacking && !this.aggressive)
      this.StartCoroutine((IEnumerator) this.ShootPoison());
    currentTime = GameManager.GetInstance()?.CurrentTime;
    float num2 = this.aggressionTimestamp / this.Spine.timeScale;
    if ((double) currentTime.GetValueOrDefault() > (double) num2 & currentTime.HasValue && !this.attacking && !this.aggressive)
    {
      this.aggressive = true;
      this.aggressionTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.aggressionDuration.x, this.aggressionDuration.y);
      this.SpeedMultiplier = this.aggressionSpeedMultiplier;
      this.focusOnTarget = true;
    }
    else
    {
      currentTime = GameManager.GetInstance()?.CurrentTime;
      float num3 = this.aggressionTimestamp / this.Spine.timeScale;
      if ((double) currentTime.GetValueOrDefault() > (double) num3 & currentTime.HasValue && this.aggressive)
      {
        this.aggressive = false;
        this.aggressionTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.timeBetweenAggression.x, this.timeBetweenAggression.y);
        this.SpeedMultiplier = 1f;
        this.focusOnTarget = false;
      }
    }
    if (!this.spawnedSecondWave && (double) this.health.HP < (double) this.health.totalHP * 0.60000002384185791)
    {
      this.SpawnEnemies();
      this.spawnedSecondWave = true;
    }
    if (this.spawnedThirdWave || (double) this.health.HP >= (double) this.health.totalHP * 0.30000001192092896)
      return;
    this.SpawnEnemies();
    this.spawnedThirdWave = true;
  }

  public IEnumerator ShootPoison()
  {
    EnemyMillipedePoisonerMiniboss poisonerMiniboss = this;
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/warning", poisonerMiniboss.gameObject);
    poisonerMiniboss.attacking = true;
    poisonerMiniboss.SetAnimation(poisonerMiniboss.shootAnticipationAnimation, true);
    yield return (object) new WaitForEndOfFrame();
    poisonerMiniboss.moveVX = 0.0f;
    poisonerMiniboss.moveVY = 0.0f;
    float t = 0.0f;
    while ((double) t < (double) poisonerMiniboss.shootAnticipation)
    {
      float amt = t / poisonerMiniboss.shootAnticipation;
      foreach (SimpleSpineFlash flash in poisonerMiniboss.flashes)
        flash.FlashWhite(amt);
      t += Time.deltaTime * poisonerMiniboss.Spine.timeScale;
      yield return (object) null;
    }
    foreach (SimpleSpineFlash flash in poisonerMiniboss.flashes)
      flash.FlashWhite(false);
    poisonerMiniboss.SetAnimation(poisonerMiniboss.shootAnimation);
    poisonerMiniboss.AddAnimation(poisonerMiniboss.idleAnimation, true);
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/attack", poisonerMiniboss.gameObject);
    int amount = UnityEngine.Random.Range((int) poisonerMiniboss.amountToShoot.x, (int) poisonerMiniboss.amountToShoot.y + 1);
    for (int i = 0; i < amount; ++i)
    {
      Vector3 position = (Vector3) UnityEngine.Random.insideUnitCircle * 5f;
      UnityEngine.Object.Instantiate<EnemyBomb>(poisonerMiniboss.bombPrefab, position, Quaternion.identity, poisonerMiniboss.transform.parent).Play(poisonerMiniboss.transform.position, poisonerMiniboss.bombDuration + UnityEngine.Random.Range(-0.5f, 0.5f), poisonerMiniboss.health.team);
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", poisonerMiniboss.gameObject);
      if ((double) poisonerMiniboss.timeBetweenShots != 0.0)
      {
        float time = 0.0f;
        while ((double) (time += Time.deltaTime * poisonerMiniboss.Spine.timeScale) < (double) poisonerMiniboss.timeBetweenShots)
          yield return (object) null;
      }
    }
    poisonerMiniboss.shootTimestamp = GameManager.GetInstance().CurrentTime + poisonerMiniboss.shootingCooldown;
    poisonerMiniboss.attacking = false;
  }

  public override void OnDie(
    GameObject Attacker,
    Vector3 AttackLocation,
    Health Victim,
    Health.AttackTypes AttackType,
    Health.AttackFlags AttackFlags)
  {
    AudioManager.Instance.StopActiveLoops();
    base.OnDie(Attacker, AttackLocation, Victim, AttackType, AttackFlags);
    for (int index = 0; index < Health.team2.Count; ++index)
    {
      if ((UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) null && (UnityEngine.Object) Health.team2[index] != (UnityEngine.Object) this.health)
      {
        Health.team2[index].invincible = false;
        Health.team2[index].enabled = true;
        Health.team2[index].DealDamage(Health.team2[index].totalHP, this.gameObject, this.transform.position);
      }
    }
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

  [CompilerGenerated]
  public void \u003CPreload\u003Eb__20_0(AsyncOperationHandle<GameObject> obj)
  {
    this.loadedAddressableAssets.Add(obj);
    obj.Result.CreatePool(this.spawnCount, true);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public void \u003C\u003En__0() => base.Start();
}
