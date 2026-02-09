// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedItemNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MessagePack;
using System;

#nullable disable
namespace Lamb.UI;

[MessagePackObject(false)]
[Serializable]
public class FinalizedItemNotification : FinalizedNotification
{
  [Key(3)]
  public InventoryItem.ITEM_TYPE ItemType;
  [Key(4)]
  public int ItemDelta;
}
