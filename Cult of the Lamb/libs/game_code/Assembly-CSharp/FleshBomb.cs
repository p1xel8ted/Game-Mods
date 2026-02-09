// Decompiled with JetBrains decompiler
// Type: FleshBomb
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Spine.Unity;
using System.Collections;
using UnityEngine;

#nullable disable
public class FleshBomb : EnemyBomb
{
  [Space]
  [SerializeField]
  public UnitObject[] enemySpawns;
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
  public int additionalAmount;
  [SerializeField]
  public float additionalRadius;

  public override void BombLanded()
  {
    AudioManager.Instance.PlayOneShot("event:/fishing/splash", this.gameObject);
    int num = Random.Range((int) this.spawnAmount.x, (int) this.spawnAmount.y + 1);
    for (int index = 0; index < num; ++index)
    {
      UnitObject unitObject = Object.Instantiate<UnitObject>(this.enemySpawns[Random.Range(0, this.enemySpawns.Length)], this.transform.position, Quaternion.identity, this.transform.parent);
      unitObject.gameObject.SetActive(true);
      EnemyFleshSwarmer component = unitObject.GetComponent<EnemyFleshSwarmer>();
      if ((Object) component != (Object) null)
        component.InstantSpawnOnBomb((float) (1.0 + (double) Random.value * 1.0));
    }
  }

  public IEnumerator SpawnAnimIE(UnitObject enemy)
  {
    Vector3 fromPosition = enemy.transform.position;
    Vector3 targetPosition = fromPosition + (Vector3) Random.insideUnitCircle * this.spawnRadius;
    float startTime = GameManager.GetInstance().CurrentTime;
    Vector3 normalized = (targetPosition - fromPosition).normalized;
    RaycastHit2D raycastHit2D = Physics2D.CircleCast((Vector2) fromPosition, this.spawnRadius, (Vector2) normalized, 0.0f, (int) this.layersToCheck);
    if ((bool) (Object) raycastHit2D.collider && (double) Vector3.Dot((Vector3) raycastHit2D.point - fromPosition, normalized) > 0.0)
      targetPosition = fromPosition + (fromPosition - (Vector3) raycastHit2D.point).normalized * this.spawnRadius;
    SkeletonAnimation component = enemy.GetComponent<SkeletonAnimation>();
    Transform spineTransform = !((Object) component != (Object) null) ? enemy.transform : component.transform;
    float t = 0.0f;
    while ((double) t < (double) this.popOutDuration)
    {
      float time = GameManager.GetInstance().TimeSince(startTime) / this.popOutDuration;
      if ((Object) spineTransform.parent == (Object) enemy.transform)
        spineTransform.localPosition = -Vector3.forward * this.heightCurve.Evaluate(time) * this.height;
      else
        spineTransform.position = enemy.transform.TransformPoint(-Vector3.forward * this.heightCurve.Evaluate(time) * this.height);
      enemy.transform.position = Vector3.Lerp(fromPosition, targetPosition, this.moveCurve.Evaluate(time));
      t += Time.deltaTime;
      yield return (object) null;
    }
    if ((Object) spineTransform.parent == (Object) enemy.transform)
      spineTransform.localPosition = Vector3.zero;
    else
      spineTransform.position = enemy.transform.TransformPoint(Vector3.zero);
  }

  public IEnumerator DelayedEnemyHealthEnable(UnitObject enemy)
  {
    enemy.health.enabled = false;
    yield return (object) new WaitForSeconds(0.5f);
    enemy.health.enabled = true;
  }
}
