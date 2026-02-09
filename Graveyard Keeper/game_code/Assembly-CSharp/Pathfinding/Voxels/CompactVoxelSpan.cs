// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.CompactVoxelSpan
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Pathfinding.Voxels;

public struct CompactVoxelSpan(ushort bottom, uint height)
{
  public ushort y = bottom;
  public uint con = 24;
  public uint h = height;
  public int reg = 0;

  public void SetConnection(int dir, uint value)
  {
    int num = dir * 6;
    this.con = (uint) ((ulong) this.con & (ulong) ~(63 /*0x3F*/ << num) | (ulong) (uint) (((int) value & 63 /*0x3F*/) << num));
  }

  public int GetConnection(int dir) => (int) this.con >> dir * 6 & 63 /*0x3F*/;
}
