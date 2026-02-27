// Decompiled with JetBrains decompiler
// Type: EnemyMillipedePoisonerMiniboss
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class EnemyMillipedePoisonerMiniboss : EnemyMillipedeSpiker
{
  [SerializeField]
  private EnemyBomb bombPrefab;
  [SerializeField]
  private float shootAnticipation;
  [SerializeField]
  private float bombDuration;
  [SerializeField]
  private Vector2 amountToShoot;
  [SerializeField]
  private float timeBetweenShots;
  [SerializeField]
  private float shootingCooldown;
  [Space]
  [SerializeField]
  private float aggressionSpeedMultiplier;
  [SerializeField]
  private Vector2 aggressionDuration;
  [SerializeField]
  private Vector2 timeBetweenAggression;
  [SerializeField]
  private AssetReferenceGameObject[] spawnable;
  [Space]
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnticipationAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "Spine")]
  private string shootAnimation;
  private float shootTimestamp;
  private float aggressionTimestamp;
  private bool aggressive;
  private bool spawnedSecondWave;
  private bool spawnedThirdWave;
  private List<Vector3> spawnPositions = new List<Vector3>(2)
  {
    new Vector3(-3f, 0.0f, 0.0f),
    new Vector3(3f, 0.0f, 0.0f)
  };

  protected override void Start()
  {
    base.Start();
    this.SpawnEnemies();
    this.aggressionTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.aggressionDuration.x, this.aggressionDuration.y);
  }

  private void SpawnEnemies()
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/vocals/spider/attack", this.gameObject);
    for (int index = 0; index < 2; ++index)
      Addressables.InstantiateAsync((object) this.spawnable[UnityEngine.Random.Range(0, this.spawnable.Length)], this.spawnPositions[index], Quaternion.identity, this.transform.parent).Completed += (System.Action<AsyncOperationHandle<GameObject>>) (obj =>
      {
        obj.Result.gameObject.SetActive(false);
        EnemySpawner.CreateWithAndInitInstantiatedEnemy(obj.Result.transform.position, this.transform.parent, obj.Result);
      });
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
    float shootTimestamp = this.shootTimestamp;
    if ((double) currentTime.GetValueOrDefault() > (double) shootTimestamp & currentTime.HasValue && !this.attacking && !this.aggressive)
      this.StartCoroutine((IEnumerator) this.ShootPoison());
    currentTime = GameManager.GetInstance()?.CurrentTime;
    float aggressionTimestamp1 = this.aggressionTimestamp;
    if ((double) currentTime.GetValueOrDefault() > (double) aggressionTimestamp1 & currentTime.HasValue && !this.attacking && !this.aggressive)
    {
      this.aggressive = true;
      this.aggressionTimestamp = GameManager.GetInstance().CurrentTime + UnityEngine.Random.Range(this.aggressionDuration.x, this.aggressionDuration.y);
      this.SpeedMultiplier = this.aggressionSpeedMultiplier;
      this.focusOnTarget = true;
    }
    else
    {
      currentTime = GameManager.GetInstance()?.CurrentTime;
      float aggressionTimestamp2 = this.aggressionTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) aggressionTimestamp2 & currentTime.HasValue && this.aggressive)
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

  private IEnumerator ShootPoison()
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
      t += Time.deltaTime;
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
      UnityEngine.Object.Instantiate<EnemyBomb>(poisonerMiniboss.bombPrefab, position, Quaternion.identity, poisonerMiniboss.transform.parent).Play(poisonerMiniboss.transform.position, poisonerMiniboss.bombDuration + UnityEngine.Random.Range(-0.5f, 0.5f));
      AudioManager.Instance.PlayOneShot("event:/enemy/spit_gross_projectile", poisonerMiniboss.gameObject);
      if ((double) poisonerMiniboss.timeBetweenShots != 0.0)
        yield return (object) new WaitForSeconds(poisonerMiniboss.timeBetweenShots);
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
}
