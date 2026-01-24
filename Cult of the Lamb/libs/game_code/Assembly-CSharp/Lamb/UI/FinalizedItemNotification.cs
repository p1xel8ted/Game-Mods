// Decompiled with JetBrains decompiler
// Type: Lamb.UI.FinalizedItemNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
