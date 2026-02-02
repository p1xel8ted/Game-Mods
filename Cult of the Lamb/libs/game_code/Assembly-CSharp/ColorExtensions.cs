// Decompiled with JetBrains decompiler
// Type: ColorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
