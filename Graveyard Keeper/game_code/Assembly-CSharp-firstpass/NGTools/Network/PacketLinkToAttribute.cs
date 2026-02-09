// Decompiled with JetBrains decompiler
// Type: NGTools.Network.PacketLinkToAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NGTools.Network;

[AttributeUsage(AttributeTargets.Class)]
public class PacketLinkToAttribute : Attribute
{
  public int packetId;
  public bool isBatchable;

  public PacketLinkToAttribute(int packetId, bool isBatchable = false)
  {
    this.packetId = packetId;
    this.isBatchable = isBatchable;
  }
}
