// Decompiled with JetBrains decompiler
// Type: OriginalFollowerLookData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public struct OriginalFollowerLookData
{
  public FollowerClothingType Clothing;
  public InventoryItem.ITEM_TYPE Necklace;

  public OriginalFollowerLookData(FollowerInfo followerInfo)
  {
    this.Clothing = followerInfo.Clothing;
    this.Necklace = followerInfo.Necklace;
  }
}
