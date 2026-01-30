// Decompiled with JetBrains decompiler
// Type: RatauGiveSeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class RatauGiveSeeds : BaseMonoBehaviour
{
  public void Play()
  {
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SEED, 6, this.transform.position);
    InventoryItem.Spawn(InventoryItem.ITEM_TYPE.SEED_PUMPKIN, 3, this.transform.position);
  }
}
