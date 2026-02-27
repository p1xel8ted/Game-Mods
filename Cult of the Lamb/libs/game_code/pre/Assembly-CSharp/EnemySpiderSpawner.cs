// Decompiled with JetBrains decompiler
// Type: EnemySpiderSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using UnityEngine;

#nullable disable
public class EnemySpiderSpawner : EnemySpider
{
  [SerializeField]
  private float spawnAnticipation;
  [SerializeField]
  private Vector2 timeBetweenSpawn;
  [SerializeField]
  private GameObject spawnable;
  [SerializeField]
  private float spawnMinDistance;
  private float spawnTimestamp = -1f;
  private bool spawning;

  public override void Update()
  {
    base.Update();
    if (this.ShouldSpawn())
      this.StartCoroutine((IEnumerator) this.SpawnIE());
    if (!GameManager.RoomActive || (double) this.spawnTimestamp != -1.0)
      return;
    this.spawnTimestamp = GameManager.GetInstance().CurrentTime + Random.Range(this.timeBetweenSpawn.x, this.timeBetweenSpawn.y);
  }

  protected override bool ShouldAttack()
  {
    return base.ShouldAttack() && !this.spawning && (bool) (Object) this.TargetEnemy && (double) Vector3.Distance(this.transform.position, this.TargetEnemy.transform.position) < (double) this.spawnMinDistance;
  }

  private bool ShouldSpawn()
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

  private IEnumerator SpawnIE()
  {
    EnemySpiderSpawner enemySpiderSpawner = this;
    enemySpiderSpawner.spawning = true;
    float t = 0.0f;
    while ((double) t < (double) enemySpiderSpawner.spawnAnticipation / (double) enemySpiderSpawner.Spine.timeScale)
    {
      enemySpiderSpawner.SimpleSpineFlash.FlashWhite(t / enemySpiderSpawner.spawnAnticipation);
      t += Time.deltaTime;
      yield return (object) null;
    }
    enemySpiderSpawner.SimpleSpineFlash.FlashWhite(false);
    GameObject gameObject = Object.Instantiate<GameObject>(enemySpiderSpawner.spawnable, enemySpiderSpawner.transform.position, Quaternion.identity, enemySpiderSpawner.transform.parent);
    gameObject.transform.localScale = Vector3.zero;
    gameObject.transform.DOScale(Vector3.one, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine);
    yield return (object) new WaitForSeconds(0.5f / enemySpiderSpawner.Spine.timeScale);
    enemySpiderSpawner.spawning = false;
    enemySpiderSpawner.spawnTimestamp = GameManager.GetInstance().CurrentTime + Random.Range(enemySpiderSpawner.timeBetweenSpawn.x, enemySpiderSpawner.timeBetweenSpawn.y);
  }
}
