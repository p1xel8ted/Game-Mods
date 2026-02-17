// Decompiled with JetBrains decompiler
// Type: Structures_Outhouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
