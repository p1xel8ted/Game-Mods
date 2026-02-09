// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.StringUtils
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

#nullable disable
namespace ParadoxNotion;

public static class StringUtils
{
  public static Dictionary<string, string> splitCaseCache = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.Ordinal);

  public static string SplitCamelCase(this string s)
  {
    if (string.IsNullOrEmpty(s))
      return s;
    string input;
    if (StringUtils.splitCaseCache.TryGetValue(s, out input))
      return input;
    input = s.Replace("_", " ");
    input = char.ToUpper(input[0]).ToString() + input.Substring(1);
    input = Regex.Replace(input, "(?<=[a-z])([A-Z])", " $1").Trim();
    return StringUtils.splitCaseCache[s] = input;
  }

  public static string CapLength(this string s, int max)
  {
    if (string.IsNullOrEmpty(s))
      return s;
    string str = s.Substring(0, Mathf.Min(s.Length, max));
    if (str.Length < s.Length)
      str += "...";
    return str;
  }

  public static string GetCapitals(this string s)
  {
    if (string.IsNullOrEmpty(s))
      return string.Empty;
    string str = "";
    foreach (char c in s)
    {
      if (char.IsUpper(c))
        str += c.ToString();
    }
    return str.Trim();
  }

  public static string GetAlphabetLetter(int index)
  {
    if (index < 0)
      return (string) null;
    string str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    return index >= str.Length ? index.ToString() : str[index].ToString();
  }

  public static string GetStringWithin(this string input, string from, string to)
  {
    return new Regex($"{from}(.*?){to}").Match(input).Groups[1].ToString();
  }

  public static string ToStringAdvanced(this object o)
  {
    if (o == null || o.Equals((object) null))
      return "NULL";
    if (o is string)
      return $"\"{(string) o}\"";
    if ((object) (o as UnityEngine.Object) != null)
      return (o as UnityEngine.Object).name;
    System.Type type = o.GetType();
    if (!type.RTIsSubclassOf(typeof (Enum)) || !ReflectionTools.RTIsDefined<FlagsAttribute>(type, true))
      return o.ToString();
    string str = "";
    int num = 0;
    Array values = Enum.GetValues(type);
    foreach (object obj in values)
    {
      if ((Convert.ToInt32(obj) & Convert.ToInt32(o)) == Convert.ToInt32(obj))
      {
        ++num;
        str = !(str == "") ? "Mixed..." : obj.ToString();
      }
    }
    if (num == 0)
      return "Nothing";
    return num == values.Length ? "Everything" : str;
  }
}
