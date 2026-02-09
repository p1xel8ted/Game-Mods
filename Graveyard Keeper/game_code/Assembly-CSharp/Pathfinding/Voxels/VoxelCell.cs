// Decompiled with JetBrains decompiler
// Type: Pathfinding.Voxels.VoxelCell
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding.Voxels;

public struct VoxelCell
{
  public VoxelSpan firstSpan;

  public void AddSpan(uint bottom, uint top, int area, int voxelWalkableClimb)
  {
    VoxelSpan voxelSpan1 = new VoxelSpan(bottom, top, area);
    if (this.firstSpan == null)
    {
      this.firstSpan = voxelSpan1;
    }
    else
    {
      VoxelSpan voxelSpan2 = (VoxelSpan) null;
      VoxelSpan voxelSpan3 = this.firstSpan;
      while (voxelSpan3 != null && voxelSpan3.bottom <= voxelSpan1.top)
      {
        if (voxelSpan3.top < voxelSpan1.bottom)
        {
          voxelSpan2 = voxelSpan3;
          voxelSpan3 = voxelSpan3.next;
        }
        else
        {
          if (voxelSpan3.bottom < bottom)
            voxelSpan1.bottom = voxelSpan3.bottom;
          if (voxelSpan3.top > top)
            voxelSpan1.top = voxelSpan3.top;
          if (Math.Abs((int) voxelSpan1.top - (int) voxelSpan3.top) <= voxelWalkableClimb)
            voxelSpan1.area = Math.Max(voxelSpan1.area, voxelSpan3.area);
          VoxelSpan next = voxelSpan3.next;
          if (voxelSpan2 != null)
            voxelSpan2.next = next;
          else
            this.firstSpan = next;
          voxelSpan3 = next;
        }
      }
      if (voxelSpan2 != null)
      {
        voxelSpan1.next = voxelSpan2.next;
        voxelSpan2.next = voxelSpan1;
      }
      else
      {
        voxelSpan1.next = this.firstSpan;
        this.firstSpan = voxelSpan1;
      }
    }
  }
}
