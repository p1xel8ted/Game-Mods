// Decompiled with JetBrains decompiler
// Type: Structures_ProximityFurnace
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Structures_ProximityFurnace : StructureBrain
{
  public bool NearbyHeatingStructure(Vector3 structurePosition)
  {
    if ((Object) GameManager.GetInstance() == (Object) null)
      return false;
    BoxCollider2D boxCollider2D = GameManager.GetInstance().GetComponent<BoxCollider2D>();
    if ((Object) boxCollider2D == (Object) null)
    {
      boxCollider2D = GameManager.GetInstance().gameObject.AddComponent<BoxCollider2D>();
      boxCollider2D.isTrigger = true;
    }
    boxCollider2D.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -45f));
    foreach (StructureBrain structureBrain in StructureManager.GetAllStructuresOfType(StructureBrain.TYPES.PROXIMITY_FURNACE))
    {
      if (!((Object) Interaction_DLCFurnace.Instance == (Object) null) && Interaction_DLCFurnace.Instance.Lit)
      {
        Vector3 position = structureBrain.Data.Position;
        double num1 = (double) Vector3.Distance(structurePosition, position);
        boxCollider2D.transform.position = position;
        boxCollider2D.size = Vector2.one * 15f;
        double num2 = 7.5 + 0.5;
        if (num1 <= num2 && boxCollider2D.OverlapPoint((Vector2) structurePosition))
          return true;
      }
    }
    StructureBrain.TYPES[] typesArray = new StructureBrain.TYPES[3]
    {
      StructureBrain.TYPES.FURNACE_1,
      StructureBrain.TYPES.FURNACE_2,
      StructureBrain.TYPES.FURNACE_3
    };
    foreach (StructureBrain structuresOfType in StructureManager.GetAllStructuresOfTypes(typesArray))
    {
      if (!((Object) Interaction_DLCFurnace.Instance == (Object) null) && Interaction_DLCFurnace.Instance.Lit)
      {
        int rangeForType = Interaction_DLCFurnace.GetRangeForType(structuresOfType.Data.Type);
        if ((double) Vector3.Distance(structurePosition, structuresOfType.Data.Position) <= (double) rangeForType)
          return true;
      }
    }
    return false;
  }
}
