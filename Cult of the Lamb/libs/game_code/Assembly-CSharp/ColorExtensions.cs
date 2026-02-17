// Decompiled with JetBrains decompiler
// Type: ColorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
