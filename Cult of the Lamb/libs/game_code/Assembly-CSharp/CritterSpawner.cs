// Decompiled with JetBrains decompiler
// Type: CritterSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
  public bool SpawnedDayCritters;
  public PolygonCollider2D Polygon;
  public Transform SpawnParent;
  public Coroutine waitForPlayerNewPhase;
  public Coroutine waitForPlayerNewWeather;

  public void Start()
  {
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    SeasonsManager.OnWeatherBegan += new SeasonsManager.WeatherTypeEvent(this.OnWeatherBegan);
    SeasonsManager.OnWeatherEnded += new SeasonsManager.WeatherTypeEvent(this.OnWeatherEnded);
    this.ButterflyPrefab.CreatePool(this.ButterfliesToSpawn.y);
    this.BeesPrefab.CreatePool(this.BeesToSpawn.y);
    this.BirdsPrefab.CreatePool(this.BirdsToSpawn.y);
    this.SpiderPrefab.CreatePool(this.SpidersToSpawn.y);
    this.FireFliesPrefab.CreatePool(this.FireFliesToSpawn.y);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnNewPhaseStarted);
  }

  public void OnStructureAdded(StructuresData structure)
  {
  }

  public void OnDestroy()
  {
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnNewPhaseStarted);
    SeasonsManager.OnWeatherBegan -= new SeasonsManager.WeatherTypeEvent(this.OnWeatherBegan);
    SeasonsManager.OnWeatherEnded -= new SeasonsManager.WeatherTypeEvent(this.OnWeatherEnded);
    if (this.waitForPlayerNewPhase != null)
      this.StopCoroutine(this.waitForPlayerNewPhase);
    if (this.waitForPlayerNewWeather == null)
      return;
    this.StopCoroutine(this.waitForPlayerNewWeather);
  }

  public void OnWeatherBegan(SeasonsManager.WeatherEvent weatherEvent)
  {
    if (this.waitForPlayerNewWeather != null)
      this.StopCoroutine(this.waitForPlayerNewWeather);
    this.waitForPlayerNewWeather = this.StartCoroutine((IEnumerator) this.WaitForPlayerInBase((System.Action) (() =>
    {
      if (weatherEvent != SeasonsManager.WeatherEvent.Blizzard || TimeManager.CurrentPhase == DayPhase.Night)
        return;
      int num1 = -1;
      int num2 = UnityEngine.Random.Range(this.FireFliesToSpawn.x, this.FireFliesToSpawn.y) - CritterBee.FireFlys.Count;
      while (++num1 < num2)
      {
        Vector3 vector3 = this.RandomPointInCollider();
        this.FireFliesPrefab.Spawn(this.SpawnParent, vector3, Quaternion.identity).GetComponent<CritterBee>().Setup(vector3);
      }
    })));
  }

  public void OnWeatherEnded(SeasonsManager.WeatherEvent weatherEvent)
  {
  }

  public void OnNewPhaseStarted()
  {
    if (this.waitForPlayerNewPhase != null)
      this.StopCoroutine(this.waitForPlayerNewPhase);
    this.waitForPlayerNewPhase = this.StartCoroutine((IEnumerator) this.WaitForPlayerInBase((System.Action) (() =>
    {
      if (TimeManager.CurrentPhase == DayPhase.Night)
      {
        this.SpawnNightCritters();
      }
      else
      {
        if (BiomeBaseManager.Instance.IsInTemple)
          this.RecycleNightCritters();
        if (!this.SpawnedDayCritters)
          this.SpawnDayCritters();
        this.SpawnBirds();
      }
      this.SpawnHalloweenGhosts();
    })));
  }

  public void RecycleNightCritters() => this.SpiderPrefab.RecycleAll();

  public void SpawnDayCritters()
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

  public void SpawnBirds()
  {
    int num1 = -1;
    int num2 = UnityEngine.Random.Range(this.BirdsToSpawn.x, this.BirdsToSpawn.y) - CritterBaseBird.Birds.Count;
    while (++num1 < num2)
    {
      Vector3 position = this.RandomPointInCollider();
      CritterBaseBird component = this.BirdsPrefab.Spawn(this.SpawnParent, position, Quaternion.identity).GetComponent<CritterBaseBird>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        component.TargetPosition = position;
        component.PrepareForSpawn();
      }
    }
  }

  public void SpawnNightCritters()
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
    foreach (FarmPlot farmPlot in FarmPlot.FarmPlots)
    {
      if (farmPlot.StructureBrain != null && farmPlot.StructureBrain.IsFullyGrown && farmPlot.StructureBrain.GetFertilizer() != null && farmPlot.StructureBrain.GetFertilizer().type == 144 /*0x90*/ && !farmPlot.StructureBrain.Data.Withered)
      {
        for (int index = 0; index < UnityEngine.Random.Range(3, 7); ++index)
          this.SpawnFireFly(farmPlot.transform.position);
      }
    }
    foreach (Interaction_Poop poop in Interaction_Poop.Poops)
    {
      if ((UnityEngine.Object) poop != (UnityEngine.Object) null && poop.StructureInfo != null && poop.StructureInfo.Type == StructureBrain.TYPES.POOP_GLOW)
      {
        for (int index = 0; index < UnityEngine.Random.Range(1, 4); ++index)
          this.SpawnFireFly(poop.transform.position);
      }
    }
  }

  public void SpawnFireFly(Vector3 pos)
  {
    CritterBee component = this.FireFliesPrefab.Spawn(this.SpawnParent, pos, Quaternion.identity).GetComponent<CritterBee>();
    component.BaseHeight /= 2f;
    component.MaximumRange = 0.5f;
    component.Speed /= 3f;
    component.AvoidPlayer = false;
    component.Setup(pos);
  }

  public void SpawnHalloweenGhosts()
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
      Addressables_wrapper.InstantiateAsync((object) this.halloweenGhost, this.RandomPointInCollider(), Quaternion.identity, this.SpawnParent, (Action<AsyncOperationHandle<GameObject>>) (obj => obj.Result.GetComponent<Interaction_HalloweenGhost>().Configure(info)));
    }
  }

  public Vector3 RandomPointInCollider()
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

  public IEnumerator WaitForPlayerInBase(System.Action callback)
  {
    while (PlayerFarming.Location != FollowerLocation.Base)
      yield return (object) null;
    System.Action action = callback;
    if (action != null)
      action();
  }

  [CompilerGenerated]
  public void \u003COnNewPhaseStarted\u003Eb__21_0()
  {
    if (TimeManager.CurrentPhase == DayPhase.Night)
    {
      this.SpawnNightCritters();
    }
    else
    {
      if (BiomeBaseManager.Instance.IsInTemple)
        this.RecycleNightCritters();
      if (!this.SpawnedDayCritters)
        this.SpawnDayCritters();
      this.SpawnBirds();
    }
    this.SpawnHalloweenGhosts();
  }
}
