// Decompiled with JetBrains decompiler
// Type: DungeonGenerator.IntVectors
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DungeonGenerator;

[Serializable]
public class IntVectors
{
  [SerializeField]
  public List<IntVector2> list = new List<IntVector2>();

  public IntVectors() => this.list = new List<IntVector2>();

  public IntVectors(List<IntVector2> t_list) => this.list = t_list;
}
