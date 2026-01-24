// Decompiled with JetBrains decompiler
// Type: Structures_Shrine_Passive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class Structures_Shrine_Passive : StructureBrain
{
  public List<FollowerTask_Pray> Prayers = new List<FollowerTask_Pray>();

  public override int SoulMax
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
        case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
          return 5;
        case StructureBrain.TYPES.SHRINE_PLEASURE:
          return 4;
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
      case StructureBrain.TYPES.SHRINE_DISCIPLE_BOOST:
        return 500f;
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

  public bool PrayAvailable(StructureBrain.TYPES Type)
  {
    return (double) TimeManager.TotalElapsedGameTime > (double) this.Data.LastPrayTime + (double) Structures_Shrine_Passive.timeBetweenPrays(Type) / (double) this.DevotionSpeedMultiplier && this.SoulCount < this.SoulMax;
  }

  public void SetFollowerPrayed() => this.Data.LastPrayTime = TimeManager.TotalElapsedGameTime;
}
