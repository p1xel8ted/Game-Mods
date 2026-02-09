// Decompiled with JetBrains decompiler
// Type: LabelEffect
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public class LabelEffect
{
  public bool enabled;
  public Color color;
  public Vector2 distance;
  public int padding;

  public LabelEffect(bool enabled, Color color, int padding)
    : this(enabled, color, Vector2.zero)
  {
    this.padding = padding;
  }

  public LabelEffect(bool enabled, Color color, Vector2 distance)
  {
    this.enabled = enabled;
    this.color = color;
    this.distance = distance;
  }
}
