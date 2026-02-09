// Decompiled with JetBrains decompiler
// Type: Pathfinding.Serialization.GraphMeta
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Pathfinding.WindowsStore;
using System;
using System.Collections.Generic;

#nullable disable
namespace Pathfinding.Serialization;

public class GraphMeta
{
  public Version version;
  public int graphs;
  public List<string> guids;
  public List<string> typeNames;

  public System.Type GetGraphType(int i)
  {
    if (string.IsNullOrEmpty(this.typeNames[i]))
      return (System.Type) null;
    System.Type type = WindowsStoreCompatibility.GetTypeInfo(typeof (AstarPath)).Assembly.GetType(this.typeNames[i]);
    return !object.Equals((object) type, (object) null) ? type : throw new Exception($"No graph of type '{this.typeNames[i]}' could be created, type does not exist");
  }
}
