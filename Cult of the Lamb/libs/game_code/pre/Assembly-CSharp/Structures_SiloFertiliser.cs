// Decompiled with JetBrains decompiler
// Type: Structures_SiloFertiliser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Structures_SiloFertiliser : StructureBrain
{
  public float Capacity = 15f;

  public static Structures_SiloFertiliser GetClosestFertiliser(
    Vector3 fromPosition,
    FollowerLocation location)
  {
    List<Structures_SiloFertiliser> structuresOfType = StructureManager.GetAllStructuresOfType<Structures_SiloFertiliser>(location);
    Structures_SiloFertiliser closestFertiliser = (Structures_SiloFertiliser) null;
    foreach (Structures_SiloFertiliser structuresSiloFertiliser in structuresOfType)
    {
      if (structuresSiloFertiliser.Data.Inventory.Count > 0 && structuresSiloFertiliser.Data.Inventory[0].quantity > 0 && (closestFertiliser == null || (double) Vector3.Distance(structuresSiloFertiliser.Data.Position, fromPosition) < (double) Vector3.Distance(closestFertiliser.Data.Position, fromPosition)))
        closestFertiliser = structuresSiloFertiliser;
    }
    return closestFertiliser;
  }
}
