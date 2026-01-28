// Decompiled with JetBrains decompiler
// Type: RatauGiveSeeds
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
