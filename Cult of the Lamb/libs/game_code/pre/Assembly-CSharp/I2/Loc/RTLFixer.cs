// Decompiled with JetBrains decompiler
// Type: I2.Loc.RTLFixer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;

#nullable disable
namespace I2.Loc;

public class RTLFixer
{
  public static string Fix(string str) => RTLFixer.Fix(str, false, true);

  public static string Fix(string str, bool rtl)
  {
    if (rtl)
      return RTLFixer.Fix(str);
    string[] strArray = str.Split(' ');
    string str1 = "";
    string str2 = "";
    foreach (string str3 in strArray)
    {
      if (char.IsLower(str3.ToLower()[str3.Length / 2]))
      {
        str1 = $"{str1}{RTLFixer.Fix(str2)}{str3} ";
        str2 = "";
      }
      else
        str2 = $"{str2}{str3} ";
    }
    if (str2 != "")
      str1 += RTLFixer.Fix(str2);
    return str1;
  }

  public static string Fix(string str, bool showTashkeel, bool useHinduNumbers)
  {
    string str1 = HindiFixer.Fix(str);
    if (str1 != str)
      return str1;
    RTLFixerTool.showTashkeel = showTashkeel;
    RTLFixerTool.useHinduNumbers = useHinduNumbers;
    if (str.Contains("\n"))
      str = str.Replace("\n", Environment.NewLine);
    if (!str.Contains(Environment.NewLine))
      return RTLFixerTool.FixLine(str);
    string[] separator = new string[1]
    {
      Environment.NewLine
    };
    string[] strArray = str.Split(separator, StringSplitOptions.None);
    if (strArray.Length == 0 || strArray.Length == 1)
      return RTLFixerTool.FixLine(str);
    string str2 = RTLFixerTool.FixLine(strArray[0]);
    int index = 1;
    if (strArray.Length > 1)
    {
      for (; index < strArray.Length; ++index)
        str2 = str2 + Environment.NewLine + RTLFixerTool.FixLine(strArray[index]);
    }
    return str2;
  }
}
