// Decompiled with JetBrains decompiler
// Type: RatauGiveSeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
