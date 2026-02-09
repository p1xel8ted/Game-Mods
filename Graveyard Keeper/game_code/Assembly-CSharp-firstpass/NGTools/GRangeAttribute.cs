// Decompiled with JetBrains decompiler
// Type: NGTools.GRangeAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace NGTools;

public class GRangeAttribute : PropertyAttribute
{
  public float min;
  public float max;

  public GRangeAttribute(float min, float max)
  {
    this.min = min;
    this.max = max;
  }

  public GRangeAttribute(int min, int max)
  {
    this.min = (float) min;
    this.max = (float) max;
  }
}
