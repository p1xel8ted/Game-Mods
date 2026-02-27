// Decompiled with JetBrains decompiler
// Type: IntExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public static class IntExtensions
{
  private static readonly Dictionary<string, int> _numeralDict = new Dictionary<string, int>()
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
    string empty = string.Empty;
    if (i <= 0)
      return empty;
    foreach (KeyValuePair<string, int> keyValuePair in IntExtensions._numeralDict)
    {
      for (; i >= keyValuePair.Value; i -= keyValuePair.Value)
        empty += keyValuePair.Key;
    }
    return empty;
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
