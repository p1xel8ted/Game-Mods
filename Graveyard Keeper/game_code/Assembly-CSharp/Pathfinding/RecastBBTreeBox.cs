// Decompiled with JetBrains decompiler
// Type: Pathfinding.RecastBBTreeBox
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class RecastBBTreeBox
{
  public Rect rect;
  public RecastMeshObj mesh;
  public RecastBBTreeBox c1;
  public RecastBBTreeBox c2;

  public RecastBBTreeBox(RecastMeshObj mesh)
  {
    this.mesh = mesh;
    Vector3 min = mesh.bounds.min;
    Vector3 max = mesh.bounds.max;
    this.rect = Rect.MinMaxRect(min.x, min.z, max.x, max.z);
  }

  public bool Contains(Vector3 p) => this.rect.Contains(p);
}
