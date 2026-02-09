// Decompiled with JetBrains decompiler
// Type: TrailDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[CreateAssetMenu]
public class TrailDefinition : ScriptableObject
{
  public List<TrailTypeDefinition> trails = new List<TrailTypeDefinition>();
  [NonSerialized]
  public List<Ground.GroudType> _types = new List<Ground.GroudType>();

  public TrailTypeDefinition GetByType(Ground.GroudType type)
  {
    if (this._types.Count == 0)
    {
      foreach (TrailTypeDefinition trail in this.trails)
        this._types.Add(trail.type);
    }
    int index = this._types.IndexOf(type);
    return index == -1 ? (TrailTypeDefinition) null : this.trails[index];
  }
}
