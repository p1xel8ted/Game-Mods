// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchSelect.RanchSelectEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace Lamb.UI.RanchSelect;

[Serializable]
public class RanchSelectEntry
{
  [CompilerGenerated]
  public StructuresData.Ranchable_Animal \u003CAnimalInfo\u003Ek__BackingField;
  [CompilerGenerated]
  public RanchSelectEntry.Status \u003CAvailabilityStatus\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CShowNecklaceReward\u003Ek__BackingField;

  public StructuresData.Ranchable_Animal AnimalInfo
  {
    set => this.\u003CAnimalInfo\u003Ek__BackingField = value;
    get => this.\u003CAnimalInfo\u003Ek__BackingField;
  }

  public RanchSelectEntry.Status AvailabilityStatus
  {
    set => this.\u003CAvailabilityStatus\u003Ek__BackingField = value;
    get => this.\u003CAvailabilityStatus\u003Ek__BackingField;
  }

  public bool ShowNecklaceReward
  {
    get => this.\u003CShowNecklaceReward\u003Ek__BackingField;
    set => this.\u003CShowNecklaceReward\u003Ek__BackingField = value;
  }

  public RanchSelectEntry(
    StructuresData.Ranchable_Animal animalInfo,
    RanchSelectEntry.Status availabilityStatus = RanchSelectEntry.Status.Available,
    bool showNecklaceReward = false)
  {
    this.AnimalInfo = animalInfo;
    this.AvailabilityStatus = availabilityStatus;
    this.ShowNecklaceReward = showNecklaceReward;
  }

  public enum Status
  {
    Available,
    Unavailable,
    UnavailableTooYoung,
    UnavailableOvercrowded,
    UnavailableStinky,
    UnavailableFeral,
    UnavailableHungry,
  }
}
