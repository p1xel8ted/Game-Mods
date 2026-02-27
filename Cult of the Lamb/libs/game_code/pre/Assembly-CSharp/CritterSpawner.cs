// Decompiled with JetBrains decompiler
// Type: CritterSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class CritterSpawner : BaseMonoBehaviour
{
  public Vector2Int ButterfliesToSpawn;
  public GameObject ButterflyPrefab;
  public Vector2Int BeesToSpawn;
  public GameObject BeesPrefab;
  public Vector2Int BirdsToSpawn;
  public GameObject BirdsPrefab;
  public Vector2Int SpidersToSpawn;
  public GameObject SpiderPrefab;
  public Vector2Int FireFliesToSpawn;
  public GameObject FireFliesPrefab;
  public AssetReferenceGameObject halloweenGhost;
  private bool SpawnedDayCritters;
  public PolygonCollider2D Polygon;
  public Transform SpawnParent;

  private void Start()
  {
    this.OnNewPhaseStarted();
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    this.ButterflyPrefab.CreatePool(this.ButterfliesToSpawn.y);
    this.BeesPrefab.CreatePool(this.BeesToSpawn.y);
    this.BirdsPrefab.CreatePool(this.BirdsToSpawn.y);
    this.SpiderPrefab.CreatePool(this.SpidersToSpawn.y);
    this.FireFliesPrefab.CreatePool(this.FireFliesToSpawn.y);
  }

  private void OnDestroy() => TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);

  private void OnNewPhaseStarted()
  {
    if (TimeManager.CurrentPhase == DayPhase.Night)
    {
      this.SpawnNightCritters();
    }
    else
    {
      if (!this.SpawnedDayCritters)
        this.SpawnDayCritters();
      this.SpawnBirds();
    }
    this.SpawnHalloweenGhosts();
  }

  private void SpawnDayCritters()
  {
    this.SpawnedDayCritters = true;
    int num1 = -1;
    int num2 = UnityEngine.Random.Range(this.ButterfliesToSpawn.x, this.ButterfliesToSpawn.y) - CritterBee.ButterFlys.Count;
    while (++num1 < num2)
    {
      Vector3 vector3 = this.RandomPointInCollider();
      this.ButterflyPrefab.Spawn(this.SpawnParent, vector3, Quaternion.identity).GetComponent<CritterBee>().Setup(vector3);
    }
    int num3 = -1;
    int num4 = UnityEngine.Random.Range(this.BeesToSpawn.x, this.BeesToSpawn.y) - CritterBee.Bees.Count;
    while (++num3 < num4)
    {
      Vector3 vector3 = this.RandomPointInCollider();
      this.BeesPrefab.Spawn(this.SpawnParent, vector3, Quaternion.identity).GetComponent<CritterBee>().Setup(vector3);
    }
  }

  private void SpawnBirds()
  {
    int num1 = -1;
    int num2 = UnityEngine.Random.Range(this.BirdsToSpawn.x, this.BirdsToSpawn.y) - CritterBaseBird.Birds.Count;
    while (++num1 < num2)
      this.BirdsPrefab.Spawn(this.SpawnParent, this.RandomPointInCollider(), Quaternion.identity);
  }

  private void SpawnNightCritters()
  {
    this.SpawnedDayCritters = false;
    int num1 = -1;
    int num2 = UnityEngine.Random.Range(this.SpidersToSpawn.x, this.SpidersToSpawn.y) - CritterSpider.Spiders.Count;
    while (++num1 < num2)
      this.SpiderPrefab.Spawn(this.SpawnParent, this.RandomPointInCollider(), Quaternion.identity);
    int num3 = -1;
    int num4 = UnityEngine.Random.Range(this.FireFliesToSpawn.x, this.FireFliesToSpawn.y) - CritterBee.FireFlys.Count;
    while (++num3 < num4)
    {
      Vector3 vector3 = this.RandomPointInCollider();
      this.FireFliesPrefab.Spawn(this.SpawnParent, vector3, Quaternion.identity).GetComponent<CritterBee>().Setup(vector3);
    }
  }

  private void SpawnHalloweenGhosts()
  {
    if (!FollowerBrainStats.IsBloodMoon || this.halloweenGhost == null || (UnityEngine.Object) BaseLocationManager.Instance == (UnityEngine.Object) null)
      return;
    int num = Mathf.Min(DataManager.Instance.Followers_Dead_IDs.Count, 4 - Interaction_HalloweenGhost.HalloweenGhosts.Count);
    SeasonalEventManager.GetSeasonalEventData(SeasonalEventType.Halloween);
    List<FollowerInfo> followerInfoList = new List<FollowerInfo>((IEnumerable<FollowerInfo>) DataManager.Instance.Followers_Dead);
    foreach (Interaction_HalloweenGhost halloweenGhost in Interaction_HalloweenGhost.HalloweenGhosts)
    {
      if (followerInfoList.Contains(halloweenGhost.FollowerInfo))
        followerInfoList.Remove(halloweenGhost.FollowerInfo);
    }
    for (int index = 0; index < num; ++index)
    {
      if (followerInfoList.Count <= 0)
        break;
      FollowerInfo info = followerInfoList[UnityEngine.Random.Range(0, followerInfoList.Count)];
      followerInfoList.Remove(info);
      Addressables.InstantiateAsync((object) this.halloweenGhost, this.RandomPointInCollider(), Quaternion.identity, this.SpawnParent).Completed += (Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<Interaction_HalloweenGhost>().Configure(info));
    }
  }

  private Vector3 RandomPointInCollider()
  {
    Bounds bounds1 = this.Polygon.bounds;
    double x1 = (double) bounds1.center.x;
    bounds1 = this.Polygon.bounds;
    double x2 = (double) bounds1.extents.x;
    float num1 = (float) (x1 - x2);
    Bounds bounds2 = this.Polygon.bounds;
    double y1 = (double) bounds2.center.y;
    bounds2 = this.Polygon.bounds;
    double y2 = (double) bounds2.extents.y;
    float num2 = (float) (y1 - y2);
    int num3 = 500;
    while (--num3 > 0)
    {
      Vector3 point = new Vector3(num1 + UnityEngine.Random.Range(0.0f, this.Polygon.bounds.extents.x * 2f), num2 + UnityEngine.Random.Range(0.0f, this.Polygon.bounds.extents.y * 2f));
      if (this.Polygon.OverlapPoint((Vector2) point))
        return point;
    }
    Debug.Log((object) "Failed to find result vector zero");
    return Vector3.zero;
  }
}
