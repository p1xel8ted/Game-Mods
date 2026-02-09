// Decompiled with JetBrains decompiler
// Type: Pathfinding.PathNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Pathfinding;

public class PathNode
{
  public GraphNode node;
  public PathNode parent;
  public ushort pathID;
  public uint flags;
  public const uint CostMask = 268435455 /*0x0FFFFFFF*/;
  public const int Flag1Offset = 28;
  public const uint Flag1Mask = 268435456 /*0x10000000*/;
  public const int Flag2Offset = 29;
  public const uint Flag2Mask = 536870912 /*0x20000000*/;
  public uint g;
  public uint h;

  public uint cost
  {
    get => this.flags & 268435455U /*0x0FFFFFFF*/;
    set => this.flags = this.flags & 4026531840U /*0xF0000000*/ | value;
  }

  public bool flag1
  {
    get => (this.flags & 268435456U /*0x10000000*/) > 0U;
    set
    {
      this.flags = (uint) ((int) this.flags & -268435457 /*0xEFFFFFFF*/ | (value ? 268435456 /*0x10000000*/ : 0));
    }
  }

  public bool flag2
  {
    get => (this.flags & 536870912U /*0x20000000*/) > 0U;
    set
    {
      this.flags = (uint) ((int) this.flags & -536870913 /*0xDFFFFFFF*/ | (value ? 536870912 /*0x20000000*/ : 0));
    }
  }

  public uint G
  {
    get => this.g;
    set => this.g = value;
  }

  public uint H
  {
    get => this.h;
    set => this.h = value;
  }

  public uint F => this.g + this.h;
}
