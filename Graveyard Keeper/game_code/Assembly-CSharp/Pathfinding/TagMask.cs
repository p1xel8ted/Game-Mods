// Decompiled with JetBrains decompiler
// Type: Pathfinding.TagMask
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Pathfinding;

[Serializable]
public class TagMask
{
  public int tagsChange;
  public int tagsSet;

  public TagMask()
  {
  }

  public TagMask(int change, int set)
  {
    this.tagsChange = change;
    this.tagsSet = set;
  }

  public override string ToString()
  {
    return $"{Convert.ToString(this.tagsChange, 2)}\n{Convert.ToString(this.tagsSet, 2)}";
  }
}
