// Decompiled with JetBrains decompiler
// Type: WeedManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class WeedManager : MonoBehaviour
{
  public static List<WeedManager> WeedManagers = new List<WeedManager>();
  public static int WeedsDestroyed = 0;
  public List<WeedManager.SeasonalWeed> WeedTypes = new List<WeedManager.SeasonalWeed>();
  public GameObject ChosenWeed;
  public Interaction_Weed Interaction_Weed;
  public bool SelfInitialise;
  public int weedTypeChosen = -1;
  public int growthStageOffset;
  public bool spawning;

  public int WeedTypeChosen
  {
    get => this.weedTypeChosen;
    set
    {
      this.weedTypeChosen = value;
      this.UpdateWeedGrowth();
    }
  }

  public int GrowthStageOffset
  {
    get => this.growthStageOffset;
    set
    {
      this.growthStageOffset = Mathf.Clamp(value, 0, this.WeedTypes[Mathf.Clamp(this.WeedTypeChosen, 0, this.WeedTypes.Count - 1)].Weeds.Count - 1);
      this.UpdateWeedGrowth();
    }
  }

  public void Start()
  {
    SeasonsManager.OnSeasonChanged += new SeasonsManager.SeasonEvent(this.UpdateSeasonalWeeds);
    if (!this.SelfInitialise)
      return;
    this.WeedTypeChosen = UnityEngine.Random.Range(0, this.WeedTypes.Count);
  }

  public void OnDestroy()
  {
    SeasonsManager.OnSeasonChanged -= new SeasonsManager.SeasonEvent(this.UpdateSeasonalWeeds);
  }

  public void Configure(int type, int growth)
  {
    this.weedTypeChosen = type;
    this.growthStageOffset = growth;
    this.UpdateWeedGrowth();
  }

  public void OnEnable()
  {
    this.OnBrainAssigned();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    WeedManager.WeedManagers.Add(this);
  }

  public void OnNewDayStarted() => this.UpdateWeedGrowth();

  public void OnDisable()
  {
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
    WeedManager.WeedManagers.Remove(this);
  }

  public void Hide() => WeedManager.HideAll();

  public static void HideAll()
  {
    foreach (WeedManager weedManager in WeedManager.WeedManagers)
    {
      if ((bool) (UnityEngine.Object) weedManager.ChosenWeed)
        weedManager.ChosenWeed.SetActive(false);
    }
  }

  public void Show() => WeedManager.ShowAll();

  public static void ShowAll()
  {
    foreach (WeedManager weedManager in WeedManager.WeedManagers)
    {
      if ((bool) (UnityEngine.Object) weedManager.ChosenWeed)
        weedManager.ChosenWeed.SetActive(true);
    }
  }

  public void OnBrainAssigned() => this.UpdateWeedType();

  public void UpdateWeedType() => this.UpdateWeedGrowth();

  public void UpdateWeedGrowth()
  {
    if (this.WeedTypeChosen == -1 || this.spawning)
      return;
    if ((UnityEngine.Object) this.ChosenWeed != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.ChosenWeed.gameObject);
    this.spawning = true;
    AssetReferenceGameObject randomWeed = this.GetRandomWeed();
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter && (double) UnityEngine.Random.value < 0.75)
    {
      this.spawning = false;
    }
    else
    {
      if (randomWeed == null)
        return;
      ObjectPool.Spawn(randomWeed.AssetGUID, Vector3.zero, Quaternion.identity, this.transform, (Action<GameObject>) (obj =>
      {
        this.ChosenWeed = obj;
        this.spawning = false;
      }));
    }
  }

  public AssetReferenceGameObject GetRandomWeed()
  {
    foreach (WeedManager.SeasonalWeed weedType in this.WeedTypes)
    {
      if (weedType.Season == SeasonsManager.CurrentSeason || !SeasonsManager.Active && weedType.Season == SeasonsManager.Season.Spring)
      {
        int index = Mathf.Clamp(this.WeedTypeChosen, 0, weedType.Weeds.Count - 1);
        return weedType.Weeds[index].Weeds[Mathf.Clamp(Mathf.FloorToInt((float) TimeManager.CurrentDay / 2f) + this.growthStageOffset, 0, weedType.Weeds[index].Weeds.Count - 1)];
      }
    }
    return (AssetReferenceGameObject) null;
  }

  public void UpdateSeasonalWeeds(SeasonsManager.Season season)
  {
    if ((UnityEngine.Object) this.ChosenWeed != (UnityEngine.Object) null)
    {
      for (int index = this.transform.childCount - 1; index >= 0; --index)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.transform.GetChild(index).gameObject);
      this.ChosenWeed = (GameObject) null;
      ++WeedManager.WeedsDestroyed;
    }
    else
      ++WeedManager.WeedsDestroyed;
    if (WeedManager.WeedsDestroyed < WeedManager.WeedManagers.Count)
      return;
    foreach (WeedManager weedManager in WeedManager.WeedManagers)
      weedManager.UpdateWeedGrowth();
    WeedManager.WeedsDestroyed = 0;
  }

  [CompilerGenerated]
  public void \u003CUpdateWeedGrowth\u003Eb__29_0(GameObject obj)
  {
    this.ChosenWeed = obj;
    this.spawning = false;
  }

  [Serializable]
  public struct Weed
  {
    public List<AssetReferenceGameObject> Weeds;
  }

  [Serializable]
  public struct SeasonalWeed
  {
    public SeasonsManager.Season Season;
    public List<WeedManager.Weed> Weeds;
  }
}
