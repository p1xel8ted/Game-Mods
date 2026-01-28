// Decompiled with JetBrains decompiler
// Type: Structures_Outhouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Text;

#nullable disable
public class Structures_Outhouse : StructureBrain
{
  public static int Capacity(StructureBrain.TYPES Type)
  {
    if (Type == StructureBrain.TYPES.OUTHOUSE)
      return 5;
    return Type == StructureBrain.TYPES.OUTHOUSE_2 ? 15 : 0;
  }

  public override bool IsFull
  {
    get => this.GetPoopCount() >= Structures_Outhouse.Capacity(this.Data.Type);
  }

  public int GetPoopCount()
  {
    int poopCount = 0;
    foreach (InventoryItem inventoryItem in this.Data.Inventory)
    {
      if (inventoryItem.type == 39)
        poopCount += inventoryItem.quantity;
    }
    return poopCount;
  }

  public override void ToDebugString(StringBuilder sb)
  {
    base.ToDebugString(sb);
    sb.AppendLine($"Poop: {this.GetPoopCount()}/{Structures_Outhouse.Capacity(this.Data.Type)}");
  }
}
