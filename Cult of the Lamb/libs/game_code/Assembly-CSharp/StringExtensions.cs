// Decompiled with JetBrains decompiler
// Type: StringExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
public static class StringExtensions
{
  public static string Colour(this string str, Color colour)
  {
    return $"<color=#{ColorUtility.ToHtmlStringRGB(colour)}>{str}</color>";
  }

  public static string Colour(this string str, string hexColour)
  {
    return str.Colour(hexColour.ColourFromHex());
  }

  public static string Bold(this string str) => $"<b>{str}</b>";

  public static string Size(this string str, int size) => $"<size={size}>{str}</size>";

  public static string Italic(this string str) => $"<i>{str}</i>";

  public static string NewLine(this string str) => str + "\n";

  public static string Wave(this string str) => $"<wave>{str}</wave>";

  public static string StripHtml(this string str) => Regex.Replace(str, "<.*?>", string.Empty);

  public static string StripColourHtml(this string str)
  {
    return Regex.Replace(str, "<color.*?>", string.Empty).Replace("</color>", string.Empty);
  }

  public static string StripNumbers(this string str) => Regex.Replace(str, "[\\d-]", string.Empty);

  public static string StripWhitespace(this string str) => Regex.Replace(str, "\\s+", "");

  public static string StripNullTerminator(this string str) => Regex.Replace(str, "\0", "");

  public static string Localized(this string str) => LocalizationManager.GetTranslation(str);

  public static string TryLocalize(this string term)
  {
    string Translation;
    return LocalizationManager.TryGetTranslation(term, out Translation) ? Translation : term;
  }
}
