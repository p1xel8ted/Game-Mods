// Decompiled with JetBrains decompiler
// Type: IntExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class IntExtensions
{
  public static Dictionary<string, int> _numeralDict = new Dictionary<string, int>()
  {
    {
      "M",
      1000
    },
    {
      "CM",
      900
    },
    {
      "D",
      500
    },
    {
      "CD",
      400
    },
    {
      "C",
      100
    },
    {
      "XC",
      90
    },
    {
      "L",
      50
    },
    {
      "XL",
      40
    },
    {
      "X",
      10
    },
    {
      "IX",
      9
    },
    {
      "V",
      5
    },
    {
      "IV",
      4
    },
    {
      "I",
      1
    }
  };

  public static string ToRefreshRateString(this int refreshRate) => $"{refreshRate} hz";

  public static bool ToBool(this int i) => i > 0;

  public static string ToNumeral(this int i)
  {
    if (!SettingsManager.Settings.Accessibility.RomanNumerals)
      return i.ToString();
    string text = string.Empty;
    if (i <= 0)
      return text;
    foreach (KeyValuePair<string, int> keyValuePair in IntExtensions._numeralDict)
    {
      for (; i >= keyValuePair.Value; i -= keyValuePair.Value)
        text += keyValuePair.Key;
    }
    if (LocalizeIntegration.IsArabic())
      text = LocalizeIntegration.ReverseText(text);
    return text;
  }

  public static int GetStableHashCode(this string str)
  {
    int num1 = 352654597 /*0x15051505*/;
    int num2 = num1;
    for (int index = 0; index < str.Length; index += 2)
    {
      num1 = (num1 << 5) + num1 ^ (int) str[index];
      if (index != str.Length - 1)
        num2 = (num2 << 5) + num2 ^ (int) str[index + 1];
      else
        break;
    }
    return num1 + num2 * 1566083941;
  }
}
