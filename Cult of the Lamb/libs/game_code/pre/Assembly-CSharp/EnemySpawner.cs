// Decompiled with JetBrains decompiler
// Type: EnemySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class EnemySpawner : BaseMonoBehaviour
{
  public static List<EnemySpawner> EnemySpawners = new List<EnemySpawner>();
  public SpriteRenderer sprite;
  private GameObject g;
  public float Duration = 1f;
  public ParticleSystem particleSystem;
  public SkeletonAnimation Spine;
  public SimpleVFX goop_0;
  public SimpleVFX goop_1;
  public SimpleVFX goop_2;
  public ParticleSystem groundGoop;
  public float waitDurationForGoop = 0.35f;
  private float Timer;

  public static GameObject Create(Vector3 Position, Transform Parent, GameObject Spawn)
  {
    return Object.Instantiate<GameObject>(Resources.Load("Prefabs/Enemy Spawner/EnemySpawner") as GameObject, Position, Quaternion.identity, Parent).GetComponent<EnemySpawner>().InitAndInstantiate(Spawn);
  }

  public static void CreateWithAndInitInstantiatedEnemy(
    Vector3 Position,
    Transform Parent,
    GameObject Spawn)
  {
    Object.Instantiate<GameObject>(Resources.Load("Prefabs/Enemy Spawner/EnemySpawner") as GameObject, Position, Quaternion.identity, Parent).GetComponent<EnemySpawner>().Init(Spawn);
  }

  public GameObject InitAndInstantiate(GameObject g)
  {
    this.g = Object.Instantiate<GameObject>(g, this.transform.position, Quaternion.identity, this.transform.parent);
    DropLootOnDeath component = g.GetComponent<DropLootOnDeath>();
    if ((bool) (Object) component && component.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL)
      component.GiveXP = false;
    EnemyRoundsBase.Instance?.AddEnemyToRound(this.g.GetComponent<Health>());
    this.g.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_appear", this.transform.position);
    AudioManager.Instance.PlayOneShot("event:/enemy/summoned", this.transform.position);
    this.StartCoroutine((IEnumerator) this.SpawnEnemy());
    this.StartCoroutine((IEnumerator) this.spawnVFX());
    return this.g;
  }

  public void Init(GameObject g)
  {
    this.g = g;
    DropLootOnDeath component = g.GetComponent<DropLootOnDeath>();
    if ((bool) (Object) component && component.LootToDrop == InventoryItem.ITEM_TYPE.BLACK_SOUL)
      component.GiveXP = false;
    Debug.Log((object) "Init()");
    EnemyRoundsBase.Instance?.AddEnemyToRound(g.GetComponent<Health>());
    g.SetActive(false);
    AudioManager.Instance.PlayOneShot("event:/enemy/summoned", this.transform.position);
    this.StartCoroutine((IEnumerator) this.SpawnEnemy());
    this.StartCoroutine((IEnumerator) this.spawnVFX());
  }

  private void OnDestroy()
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", this.transform.position);
    this.StopAllCoroutines();
    if (!((Object) this.g != (Object) null) || this.g.activeSelf)
      return;
    Object.Destroy((Object) this.g);
  }

  private void OnEnable() => EnemySpawner.EnemySpawners.Add(this);

  private void OnDisable() => EnemySpawner.EnemySpawners.Remove(this);

  private IEnumerator spawnVFX()
  {
    this.groundGoop.gameObject.SetActive(true);
    this.groundGoop.Play();
    this.Spine.transform.localScale = Vector3.zero;
    this.Spine.transform.DOScale(1f, 0.25f);
    yield return (object) new WaitForSeconds(this.waitDurationForGoop);
    this.goop_0.gameObject.SetActive(true);
    this.goop_1.gameObject.SetActive(true);
    this.goop_2.gameObject.SetActive(true);
    this.goop_0.Play();
    this.goop_1.Play();
    this.goop_2.Play();
  }

  private IEnumerator SpawnEnemy()
  {
    EnemySpawner enemySpawner = this;
    while ((double) (enemySpawner.Timer += Time.deltaTime) < (double) enemySpawner.Duration)
    {
      enemySpawner.sprite.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, enemySpawner.Timer / enemySpawner.Duration);
      yield return (object) null;
    }
    enemySpawner.particleSystem.Stop();
    yield return (object) new WaitForSeconds(0.3f);
    CameraManager.shakeCamera(0.4f, (float) Random.Range(0, 360));
    if ((bool) (Object) enemySpawner.g)
    {
      enemySpawner.g.SetActive(true);
      enemySpawner.g = BiomeConstants.Instance.SpawnInWhite.Spawn();
      enemySpawner.g.transform.position = enemySpawner.transform.position;
    }
    float ScaleSpeed = 0.2f;
    float Scale = 1f;
    while ((double) enemySpawner.sprite.transform.localScale.x > 0.0)
    {
      ScaleSpeed -= 0.02f;
      Scale += ScaleSpeed;
      enemySpawner.sprite.transform.localScale = new Vector3(Scale, Scale, Scale);
      yield return (object) null;
    }
    yield return (object) new WaitForSeconds(2f);
    Object.Destroy((Object) enemySpawner.gameObject);
  }
}
