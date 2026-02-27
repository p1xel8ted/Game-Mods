// Decompiled with JetBrains decompiler
// Type: Structures_FishingSpot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_FishingSpot : StructureBrain
{
  public const int FISH_SPAWN_PER_DAY = 10;
  public int FishSpawnedToday;
  public List<Interaction_Fishing.FishType> SpawnedFish = new List<Interaction_Fishing.FishType>();

  public bool CanSpawnFish => this.FishSpawnedToday < 10;

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

  private void OnNewDay() => this.FishSpawnedToday = this.SpawnedFish.Count;

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
