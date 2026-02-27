// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Passive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_Shrine_Passive : StructureBrain
{
  public List<FollowerTask_Pray> Prayers = new List<FollowerTask_Pray>();
  public Action<int> OnSoulsGained;

  public int SoulMax
  {
    get
    {
      switch (this.Data.Type)
      {
        case StructureBrain.TYPES.SHRINE_PASSIVE:
          return 10;
        case StructureBrain.TYPES.SHRINE_PASSIVE_II:
          return 20;
        case StructureBrain.TYPES.SHRINE_PASSIVE_III:
          return 40;
        default:
          return 0;
      }
    }
  }

  public static float timeBetweenPrays(StructureBrain.TYPES Type)
  {
    if (Type == StructureBrain.TYPES.SHRINE_PASSIVE)
      return 240f;
    if (Type == StructureBrain.TYPES.SHRINE_PASSIVE_II)
      return 168f;
    return Type == StructureBrain.TYPES.SHRINE_PASSIVE_III ? 120f : 240f;
  }

  public static float Range(StructureBrain.TYPES Type)
  {
    switch (Type)
    {
      case StructureBrain.TYPES.SHRINE_PASSIVE:
        return 4f;
      case StructureBrain.TYPES.SHRINE_PASSIVE_II:
        return 6f;
      case StructureBrain.TYPES.SHRINE_PASSIVE_III:
        return 8f;
      default:
        return 10f;
    }
  }

  public float DevotionSpeedMultiplier
  {
    get
    {
      return this.Data.FullyFueled && UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Shrine_PassiveShrinesFlames) ? 1.4f : 1f;
    }
  }

  public int SoulCount
  {
    get => this.Data.SoulCount;
    set
    {
      int soulCount = this.SoulCount;
      this.Data.SoulCount = Mathf.Clamp(value, 0, this.SoulMax);
      if (this.SoulCount <= soulCount)
        return;
      Action<int> onSoulsGained = this.OnSoulsGained;
      if (onSoulsGained == null)
        return;
      onSoulsGained(this.SoulCount - soulCount);
    }
  }

  public bool PrayAvailable(StructureBrain.TYPES Type)
  {
    return (double) TimeManager.TotalElapsedGameTime > (double) this.Data.LastPrayTime + (double) Structures_Shrine_Passive.timeBetweenPrays(Type) / (double) this.DevotionSpeedMultiplier && this.SoulCount < this.SoulMax;
  }

  public void SetFollowerPrayed() => this.Data.LastPrayTime = TimeManager.TotalElapsedGameTime;
}
