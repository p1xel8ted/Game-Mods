// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedItemNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
