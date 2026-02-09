// Decompiled with JetBrains decompiler
// Type: MinMaxRangeAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class MinMaxRangeAttribute : PropertyAttribute
{
  public float minLimit;
  public float maxLimit;

  public MinMaxRangeAttribute(float minLimit, float maxLimit)
  {
    this.minLimit = minLimit;
    this.maxLimit = maxLimit;
  }
}
