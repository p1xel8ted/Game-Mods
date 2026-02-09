// Decompiled with JetBrains decompiler
// Type: Pathfinding.INavmesh
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
namespace Pathfinding;

public interface INavmesh
{
  void GetNodes(GraphNodeDelegateCancelable del);
}
