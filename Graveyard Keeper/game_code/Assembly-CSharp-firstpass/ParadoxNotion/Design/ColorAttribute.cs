// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Design.ColorAttribute
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Globalization;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Design;

[AttributeUsage(AttributeTargets.Class)]
public class ColorAttribute : Attribute
{
  public string hexColor;
  public Color32? resolved;

  public ColorAttribute(string hexColor) => this.hexColor = hexColor;

  public Color32 Resolve()
  {
    if (this.resolved.HasValue)
      return this.resolved.Value;
    this.resolved = new Color32?(new Color32());
    if (this.hexColor.Length == 6)
      this.resolved = new Color32?(new Color32(byte.Parse(this.hexColor.Substring(0, 2), NumberStyles.HexNumber), byte.Parse(this.hexColor.Substring(2, 2), NumberStyles.HexNumber), byte.Parse(this.hexColor.Substring(4, 2), NumberStyles.HexNumber), byte.MaxValue));
    return this.resolved.Value;
  }
}
