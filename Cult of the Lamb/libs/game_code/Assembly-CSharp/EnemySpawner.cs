// Decompiled with JetBrains decompiler
// Type: EnemySpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public GameObject g;
  public float Duration = 1f;
  public ParticleSystem particleSystem;
  public SkeletonAnimation Spine;
  public SimpleVFX goop_0;
  public SimpleVFX goop_1;
  public SimpleVFX goop_2;
  public ParticleSystem groundGoop;
  public float waitDurationForGoop = 0.35f;
  public static GameObject enemySpawnerPrefab;
  public bool isPreloaded;
  public float Timer;

  public static void Preload() => ObjectPool.CreatePool(EnemySpawner.LoadEnemySpawnerPrefab(), 10);

  public void PreloadSpine()
  {
    if (this.isPreloaded)
      return;
    this.isPreloaded = true;
    if ((Object) this.Spine != (Object) null)
      this.Spine.skeletonDataAsset.GetSkeletonData(true);
    if ((Object) this.goop_0 != (Object) null && (Object) this.goop_0.Spine != (Object) null)
      this.goop_0.Spine.skeletonDataAsset.GetSkeletonData(true);
    if ((Object) this.goop_1 != (Object) null && (Object) this.goop_1.Spine != (Object) null)
      this.goop_0.Spine.skeletonDataAsset.GetSkeletonData(true);
    if (!((Object) this.goop_2 != (Object) null) || !((Object) this.goop_2.Spine != (Object) null))
      return;
    this.goop_0.Spine.skeletonDataAsset.GetSkeletonData(true);
  }

  public static GameObject LoadEnemySpawnerPrefab()
  {
    if ((Object) EnemySpawner.enemySpawnerPrefab == (Object) null)
      EnemySpawner.enemySpawnerPrefab = Resources.Load("Prefabs/Enemy Spawner/EnemySpawner") as GameObject;
    return EnemySpawner.enemySpawnerPrefab;
  }

  public static void Unload() => EnemySpawner.enemySpawnerPrefab = (GameObject) null;

  public static GameObject Create(Vector3 Position, Transform Parent, GameObject Spawn)
  {
    EnemySpawner component = ObjectPool.Spawn(EnemySpawner.LoadEnemySpawnerPrefab(), Parent, Position, Quaternion.identity).GetComponent<EnemySpawner>();
    component.transform.position = Position;
    return component.InitAndInstantiate(Spawn);
  }

  public static void CreateWithAndInitInstantiatedEnemy(
    Vector3 Position,
    Transform Parent,
    GameObject Spawn)
  {
    EnemySpawner component = ObjectPool.Spawn(EnemySpawner.LoadEnemySpawnerPrefab(), Parent, Position, Quaternion.identity).GetComponent<EnemySpawner>();
    component.transform.position = Position;
    component.Init(Spawn);
  }

  public GameObject InitAndInstantiate(GameObject g)
  {
    this.g = ObjectPool.Spawn(g, this.transform.parent, this.transform.position, Quaternion.identity);
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

  public void OnDestroy()
  {
    AudioManager.Instance.PlayOneShot("event:/enemy/teleport_away", this.transform.position);
    this.StopAllCoroutines();
    if (!((Object) this.g != (Object) null) || !this.g.activeSelf)
      return;
    ObjectPool.Recycle(this.g);
  }

  public void OnEnable()
  {
    EnemySpawner.EnemySpawners.Add(this);
    this.particleSystem.Play();
    this.Spine.state.SetAnimation(0, "animation", false);
  }

  public void OnDisable() => EnemySpawner.EnemySpawners.Remove(this);

  public IEnumerator spawnVFX()
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

  public IEnumerator SpawnEnemy()
  {
    EnemySpawner enemySpawner = this;
    enemySpawner.Timer = 0.0f;
    enemySpawner.sprite.transform.localScale = Vector3.zero;
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
      if (PlayerRelic.TimeFrozen)
        PlayerFarming.Instance.playerRelic.AddFrozenEnemy(enemySpawner.g.GetComponent<Health>());
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
    ObjectPool.Recycle(enemySpawner.gameObject);
  }
}
