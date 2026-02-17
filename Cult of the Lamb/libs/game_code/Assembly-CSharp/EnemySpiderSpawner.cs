// Decompiled with JetBrains decompiler
// Type: EnemySpiderSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderSpawner : EnemySpider
{
  [SerializeField]
  public float spawnAnticipation;
  [SerializeField]
  public Vector2 timeBetweenSpawn;
  [SerializeField]
  public GameObject spawnable;
  [SerializeField]
  public float spawnMinDistance;
  [SerializeField]
  public bool isSpawnablesIncreasingDamageMultiplier = true;
  public float spawnTimestamp = -1f;
  public bool spawning;

  public override void Update()
  {
    base.Update();
    if (this.ShouldSpawn())
      this.StartCoroutine((IEnumerator) this.SpawnIE());
    if (!GameManager.RoomActive || (double) this.spawnTimestamp != -1.0)
      return;
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime + Random.Range(this.timeBetweenSpawn.x, this.timeBetweenSpawn.y);
  }

  public override bool ShouldAttack()
  {
    return base.ShouldAttack() && !this.spawning && (bool) (Object) this.TargetEnemy && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) < (double) this.spawnMinDistance;
  }

  public bool ShouldSpawn()
  {
    if (GameManager.RoomActive)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float spawnTimestamp = this.spawnTimestamp;
      if ((double) currentTime.GetValueOrDefault() > (double) spawnTimestamp & currentTime.HasValue && (bool) (Object) this.TargetEnemy && !this.spawning && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) > (double) this.spawnMinDistance)
        return Health.team2.Count <= 8;
    }
    return false;
  }

  public IEnumerator SpawnIE()
  {
    EnemySpiderSpawner enemySpiderSpawner = this;
    enemySpiderSpawner.spawning = true;
    float t = 0.0f;
    while ((double) t < (double) enemySpiderSpawner.spawnAnticipation)
    {
      enemySpiderSpawner.SimpleSpineFlash.FlashWhite(t / enemySpiderSpawner.spawnAnticipation);
      t += Time.deltaTime * enemySpiderSpawner.Spine.timeScale;
      yield return (object) null;
    }
    enemySpiderSpawner.SimpleSpineFlash.FlashWhite(false);
    GameObject gameObject = Object.Instantiate<GameObject>(enemySpiderSpawner.spawnable, enemySpiderSpawner.transform.position, Quaternion.identity, enemySpiderSpawner.transform.parent);
    gameObject.transform.localScale = Vector3.zero;
    gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    Health component = gameObject.GetComponent<Health>();
    if ((Object) component != (Object) null)
    {
      Interaction_Chest.Instance?.AddEnemy(component);
      component.CanIncreaseDamageMultiplier = enemySpiderSpawner.isSpawnablesIncreasingDamageMultiplier;
    }
    float time = 0.0f;
    while ((double) (time += Time.deltaTime * enemySpiderSpawner.Spine.timeScale) < 0.5)
      yield return (object) null;
    enemySpiderSpawner.spawning = false;
    enemySpiderSpawner.spawnTimestamp = GameManager.GetInstance().CurrentTime + Random.Range(enemySpiderSpawner.timeBetweenSpawn.x, enemySpiderSpawner.timeBetweenSpawn.y);
  }
}
