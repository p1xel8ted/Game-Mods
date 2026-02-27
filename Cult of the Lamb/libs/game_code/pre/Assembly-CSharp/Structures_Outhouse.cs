// Decompiled with JetBrains decompiler
// Type: Structures_Outhouse
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
