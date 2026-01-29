// Decompiled with JetBrains decompiler
// Type: OriginalFollowerLookData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
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
