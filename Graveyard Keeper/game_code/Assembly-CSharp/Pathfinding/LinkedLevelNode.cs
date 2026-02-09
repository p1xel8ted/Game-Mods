// Decompiled with JetBrains decompiler
// Type: Pathfinding.LinkedLevelNode
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Pathfinding;

public class LinkedLevelNode
{
  public Vector3 position;
  public bool walkable;
  public RaycastHit hit;
  public float height;
  public LinkedLevelNode next;
}
