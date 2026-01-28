// Decompiled with JetBrains decompiler
// Type: LocalizeIntegration
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using RTLTMPro;
using UnityEngine;

#nullable disable
public class LocalizeIntegration : MonoBehaviour
{
  public const char INVISIBLE_MARKER = '\u200B';
  public static FastStringBuilder textSB = new FastStringBuilder(2048 /*0x0800*/);
  public static bool Arabic = false;

  public static string ConvertToRTL(string text, bool reverse = true)
  {
    if (!LocalizeIntegration.IsArabic())
      return text;
    LocalizeIntegration.textSB.Clear();
    RTLSupport.FixRTL(text, LocalizeIntegration.textSB, false, preserveNumbers: true);
    if (reverse)
      LocalizeIntegration.textSB.Reverse();
    return LocalizeIntegration.textSB.ToString();
  }

  public static bool IsReversed(string text)
  {
    foreach (char ch in text)
    {
      if (ch == '\u200B')
        return true;
    }
    return false;
  }

  public static string FixEnglishWord(string text)
  {
    return !LocalizeIntegration.IsArabic() || LocalizeIntegration.IsRTLInput(text) || LocalizeIntegration.IsReversed(text) ? text : LocalizeIntegration.ReverseText(text);
  }

  public static string ReverseText(string text)
  {
    if (!LocalizeIntegration.IsArabic())
      return text;
    LocalizeIntegration.textSB.Clear();
    LocalizeIntegration.textSB.SetValue(text);
    LocalizeIntegration.textSB.Reverse();
    return LocalizeIntegration.textSB.ToString();
  }

  public static bool IsArabic() => LocalizeIntegration.Arabic;

  public static bool IsRTLInput(string text) => TextUtils.IsRTLInput(text);

  public static string Arabic_ReverseNonRTL(string text, bool forceReverse = false)
  {
    if (!LocalizeIntegration.IsArabic() || LocalizeIntegration.IsRTLInput(text))
      return text;
    if (forceReverse)
      return LocalizeIntegration.ReverseText(text);
    string str;
    if (text.EndsWith('\u200B'))
    {
      str = LocalizeIntegration.ReverseText(text);
    }
    else
    {
      if (text.StartsWith('\u200B'))
        return text;
      str = "\u200B" + LocalizeIntegration.ReverseText(text);
    }
    return str;
  }

  public static string FormatCurrentMax(string val1, string val2)
  {
    return LocalizeIntegration.IsArabic() ? $"{LocalizeIntegration.ReverseText(val2)} / {LocalizeIntegration.ReverseText(val1)}" : $"{val1} / {val2}";
  }
}
