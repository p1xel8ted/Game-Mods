// Decompiled with JetBrains decompiler
// Type: I2.Loc.SpecializationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace I2.Loc;

public class SpecializationManager : BaseSpecializationManager
{
  public static SpecializationManager Singleton = new SpecializationManager();

  public SpecializationManager() => this.InitializeSpecializations();

  public static string GetSpecializedText(string text, string specialization = null)
  {
    int length = text.IndexOf("[i2s_", StringComparison.Ordinal);
    if (length < 0)
      return text;
    if (string.IsNullOrEmpty(specialization))
      specialization = SpecializationManager.Singleton.GetCurrentSpecialization();
    for (; !string.IsNullOrEmpty(specialization) && specialization != "Any"; specialization = SpecializationManager.Singleton.GetFallbackSpecialization(specialization))
    {
      string str = $"[i2s_{specialization}]";
      int num1 = text.IndexOf(str, StringComparison.Ordinal);
      if (num1 < 0)
        continue;
      int startIndex = num1 + str.Length;
      int num2 = text.IndexOf("[i2s_", startIndex, StringComparison.Ordinal);
      if (num2 < 0)
        num2 = text.Length;
      return text.Substring(startIndex, num2 - startIndex);
    }
    return text.Substring(0, length);
  }

  public static string SetSpecializedText(string text, string newText, string specialization)
  {
    if (string.IsNullOrEmpty(specialization))
      specialization = "Any";
    if ((text == null || !text.Contains("[i2s_")) && specialization == "Any")
      return newText;
    Dictionary<string, string> specializations = SpecializationManager.GetSpecializations(text);
    specializations[specialization] = newText;
    return SpecializationManager.SetSpecializedText(specializations);
  }

  public static string SetSpecializedText(Dictionary<string, string> specializations)
  {
    string str;
    if (!specializations.TryGetValue("Any", out str))
      str = string.Empty;
    foreach (KeyValuePair<string, string> specialization in specializations)
    {
      if (specialization.Key != "Any" && !string.IsNullOrEmpty(specialization.Value))
        str = $"{str}[i2s_{specialization.Key}]{specialization.Value}";
    }
    return str;
  }

  public static Dictionary<string, string> GetSpecializations(
    string text,
    Dictionary<string, string> buffer = null)
  {
    if (buffer == null)
      buffer = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.Ordinal);
    else
      buffer.Clear();
    if (text == null)
    {
      buffer["Any"] = "";
      return buffer;
    }
    int length = text.IndexOf("[i2s_", StringComparison.Ordinal);
    if (length < 0)
      length = text.Length;
    buffer["Any"] = text.Substring(0, length);
    int num1;
    for (int index = length; index < text.Length; index = num1)
    {
      int startIndex1 = index + "[i2s_".Length;
      int num2 = text.IndexOf(']', startIndex1);
      if (num2 >= 0)
      {
        string key = text.Substring(startIndex1, num2 - startIndex1);
        int startIndex2 = num2 + 1;
        num1 = text.IndexOf("[i2s_", startIndex2, StringComparison.Ordinal);
        if (num1 < 0)
          num1 = text.Length;
        string str = text.Substring(startIndex2, num1 - startIndex2);
        buffer[key] = str;
      }
      else
        break;
    }
    return buffer;
  }

  public static void AppendSpecializations(string text, List<string> list = null)
  {
    if (text == null)
      return;
    if (list == null)
      list = new List<string>();
    if (!list.Contains("Any"))
      list.Add("Any");
    int startIndex = 0;
    while (startIndex < text.Length)
    {
      int num1 = text.IndexOf("[i2s_", startIndex, StringComparison.Ordinal);
      if (num1 < 0)
        break;
      startIndex = num1 + "[i2s_".Length;
      int num2 = text.IndexOf(']', startIndex);
      if (num2 < 0)
        break;
      string str = text.Substring(startIndex, num2 - startIndex);
      if (!list.Contains(str))
        list.Add(str);
    }
  }
}
