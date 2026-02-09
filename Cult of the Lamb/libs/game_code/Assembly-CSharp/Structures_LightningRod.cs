// Decompiled with JetBrains decompiler
// Type: Structures_LightningRod
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Structures_LightningRod : StructureBrain
{
  public override bool Collapse(
    bool showNotifications = true,
    bool refreshFollowerTasks = true,
    bool struckByLightning = false)
  {
    if (!struckByLightning)
      return base.Collapse(showNotifications, refreshFollowerTasks, struckByLightning);
    for (int index = 0; index < Random.Range(9, 21); ++index)
      this.DepositItemUnstacked(InventoryItem.ITEM_TYPE.LIGHTNING_SHARD);
    return false;
  }
}
