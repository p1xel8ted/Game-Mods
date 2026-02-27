// Decompiled with JetBrains decompiler
// Type: Structures_SiloSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_SiloSeed : StructureBrain
{
  public float Capacity = 15f;

  public static Structures_SiloSeed GetClosestSeeder(
    Vector3 fromPosition,
    FollowerLocation location)
  {
    List<Structures_SiloSeed> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_SiloSeed>(location);
    Structures_SiloSeed closestSeeder = (Structures_SiloSeed) null;
    foreach (Structures_SiloSeed structuresSiloSeed in structuresOfType)
    {
      if (structuresSiloSeed.Data.Inventory.Count > 0 && structuresSiloSeed.Data.Inventory[0].quantity > 0 && (closestSeeder == null || (double) Vector3.Distance(structuresSiloSeed.Data.Position, fromPosition) < (double) Vector3.Distance(closestSeeder.Data.Position, fromPosition)))
        closestSeeder = structuresSiloSeed;
    }
    return closestSeeder;
  }
}
