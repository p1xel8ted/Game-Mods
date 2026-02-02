// Decompiled with JetBrains decompiler
// Type: SpiderBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class SpiderBomb : EnemyBomb
{
  [Space]
  [SerializeField]
  public EnemySpider[] spiders;
  [SerializeField]
  public Vector2 spawnAmount;
  [SerializeField]
  public float spawnRadius;
  [SerializeField]
  public float popOutDuration;
  [SerializeField]
  public float height;
  [SerializeField]
  public float growDuration;
  [SerializeField]
  public AnimationCurve heightCurve;
  [SerializeField]
  public AnimationCurve moveCurve;
  [SerializeField]
  public string spawnAnimation = "spawn";
  [SerializeField]
  public LayerMask layersToCheck;
  [Space]
  [SerializeField]
  public GameObject additionalObjectToSpawn;
  [SerializeField]
  public int additionalAmount;
  [SerializeField]
  public float additionalRadius;

  public override void BombLanded()
  {
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", this.gameObject);
    int num = Random.Range((int) this.spawnAmount.x, (int) this.spawnAmount.y + 1);
    for (int index = 0; index < num; ++index)
    {
      EnemySpider enemy = Object.Instantiate<EnemySpider>(this.spiders[Random.Range(0, this.spiders.Length)], this.transform.position, Quaternion.identity, this.transform.parent);
      Interaction_Chest.Instance?.AddEnemy(enemy.health);
      foreach (SkeletonAnimation componentsInChild in enemy.GetComponentsInChildren<SkeletonAnimation>())
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
      enemy.StartCoroutine((IEnumerator) this.SpawnAnimIE(enemy));
      enemy.StartCoroutine((IEnumerator) this.DelayedEnemyHealthEnable((UnitObject) enemy));
    }
    if (!this.additionalObjectToSpawn.name.Contains("Trap Poison"))
      return;
    TrapPoison.CreatePoison(this.transform.position, this.additionalAmount, this.additionalRadius, this.transform.parent);
  }

  public IEnumerator SpawnAnimIE(EnemySpider enemy)
  {
    Vector3 fromPosition = enemy.transform.position;
    Vector3 targetPosition = fromPosition + (Vector3) Random.insideUnitCircle * this.spawnRadius;
    float startTime = GameManager.GetInstance().CurrentTime;
    Vector3 normalized = (targetPosition - fromPosition).normalized;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) fromPosition, this.spawnRadius, (Vector2) normalized, 0.0f, (int) this.layersToCheck);
    if ((bool) (Object) raycastHit2D.collider && (double) Vector3.Dot((Vector3) raycastHit2D.point - fromPosition, normalized) > 0.0)
      targetPosition = fromPosition + (fromPosition - (Vector3) raycastHit2D.point).normalized * this.spawnRadius;
    float t = 0.0f;
    while ((double) t < (double) this.popOutDuration)
    {
      float time = GameManager.GetInstance().TimeSince(startTime) / this.popOutDuration;
      if ((Object) enemy.Spine.transform.parent == (Object) enemy.transform)
        enemy.Spine.transform.localPosition = -Vector3.forward * this.heightCurve.Evaluate(time) * this.height;
      else
        enemy.Spine.transform.position = enemy.transform.TransformPoint(-Vector3.forward * this.heightCurve.Evaluate(time) * this.height);
      enemy.transform.position = Vector3.Lerp(fromPosition, targetPosition, this.moveCurve.Evaluate(time));
      t += Time.deltaTime;
      yield return (object) null;
    }
    if ((Object) enemy.Spine.transform.parent == (Object) enemy.transform)
      enemy.Spine.transform.localPosition = Vector3.zero;
    else
      enemy.Spine.transform.position = enemy.transform.TransformPoint(Vector3.zero);
  }

  public IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    enemy.health.enabled = false;
    yield return (object) new WaitForSeconds(0.5f);
    enemy.health.enabled = true;
  }
}
