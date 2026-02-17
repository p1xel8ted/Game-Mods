// Decompiled with JetBrains decompiler
// Type: Structures_FishingSpot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_FishingSpot : StructureBrain
{
  public const int FISH_SPAWN_PER_DAY = 10;
  public int FishSpawnedToday;
  public List<Interaction_Fishing.FishType> SpawnedFish = new List<Interaction_Fishing.FishType>();

  public bool CanSpawnFish
  {
    get
    {
      return SeasonsManager.CurrentSeason != SeasonsManager.Season.Winter && this.FishSpawnedToday < 10;
    }
  }

  public override void OnAdded()
  {
    base.OnAdded();
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDay);
  }

  public override void OnRemoved()
  {
    base.OnRemoved();
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDay);
  }

  public void OnNewDay() => this.FishSpawnedToday = this.SpawnedFish.Count;

  public void AddFishSpawned(Interaction_Fishing.FishType spawnedFish)
  {
    this.SpawnedFish.Add(spawnedFish);
    ++this.FishSpawnedToday;
  }

  public void FishCaught(Interaction_Fishing.FishType spawnedFish)
  {
    this.SpawnedFish.Add(spawnedFish);
  }
}
