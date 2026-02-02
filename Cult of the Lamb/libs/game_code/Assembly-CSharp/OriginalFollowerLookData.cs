// Decompiled with JetBrains decompiler
// Type: OriginalFollowerLookData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
