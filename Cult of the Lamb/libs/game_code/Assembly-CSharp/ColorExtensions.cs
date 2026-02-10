// Decompiled with JetBrains decompiler
// Type: ColorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
