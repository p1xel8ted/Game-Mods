// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.LinkedVoxelSpan
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Pathfinding.Voxels;

public struct LinkedVoxelSpan
{
  public uint bottom;
  public uint top;
  public int next;
  public int area;

  public LinkedVoxelSpan(uint bottom, uint top, int area)
  {
    this.bottom = bottom;
    this.top = top;
    this.area = area;
    this.next = -1;
  }

  public LinkedVoxelSpan(uint bottom, uint top, int area, int next)
  {
    this.bottom = bottom;
    this.top = top;
    this.area = area;
    this.next = next;
  }
}
