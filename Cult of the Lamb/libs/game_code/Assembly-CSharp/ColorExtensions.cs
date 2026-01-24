// Decompiled with JetBrains decompiler
// Type: ColorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class ColorExtensions
{
  public static Color ColourFromHex(this string hex)
  {
    Color color;
    return ColorUtility.TryParseHtmlString(hex, out color) ? color : Color.magenta;
  }
}
