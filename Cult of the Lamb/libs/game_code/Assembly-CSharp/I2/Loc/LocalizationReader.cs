// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizationReader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizationReader
{
  public static Dictionary<string, string> ReadTextAsset(TextAsset asset)
  {
    StringReader stringReader = new StringReader(Encoding.UTF8.GetString(asset.bytes, 0, asset.bytes.Length).Replace("\r\n", "\n").Replace("\r", "\n"));
    Dictionary<string, string> dictionary = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.Ordinal);
    string line;
    while ((line = stringReader.ReadLine()) != null)
    {
      string key;
      string str;
      if (LocalizationReader.TextAsset_ReadLine(line, out key, out str, out string _, out string _, out string _) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(str))
        dictionary[key] = str;
    }
    return dictionary;
  }

  public static bool TextAsset_ReadLine(
    string line,
    out string key,
    out string value,
    out string category,
    out string comment,
    out string termType)
  {
    key = string.Empty;
    category = string.Empty;
    comment = string.Empty;
    termType = string.Empty;
    value = string.Empty;
    int length1 = line.LastIndexOf("//", StringComparison.Ordinal);
    if (length1 >= 0)
    {
      comment = line.Substring(length1 + 2).Trim();
      comment = LocalizationReader.DecodeString(comment);
      line = line.Substring(0, length1);
    }
    int length2 = line.IndexOf("=", StringComparison.Ordinal);
    if (length2 < 0)
      return false;
    key = line.Substring(0, length2).Trim();
    value = line.Substring(length2 + 1).Trim();
    value = value.Replace("\r\n", "\n").Replace("\n", "\\n");
    value = LocalizationReader.DecodeString(value);
    if (key.Length > 2 && key[0] == '[')
    {
      int num = key.IndexOf(']');
      if (num >= 0)
      {
        termType = key.Substring(1, num - 1);
        key = key.Substring(num + 1);
      }
    }
    LocalizationReader.ValidateFullTerm(ref key);
    return true;
  }

  public static string ReadCSVfile(string Path, Encoding encoding)
  {
    string str = string.Empty;
    using (StreamReader streamReader = new StreamReader(Path, encoding))
      str = streamReader.ReadToEnd();
    return str.Replace("\r\n", "\n").Replace("\r", "\n");
  }

  public static List<string[]> ReadCSV(string Text, char Separator = ',')
  {
    int iStart = 0;
    List<string[]> strArrayList = new List<string[]>();
    while (iStart < Text.Length)
    {
      string[] csVline = LocalizationReader.ParseCSVline(Text, ref iStart, Separator);
      if (csVline != null)
        strArrayList.Add(csVline);
      else
        break;
    }
    return strArrayList;
  }

  public static string[] ParseCSVline(string Line, ref int iStart, char Separator)
  {
    List<string> list = new List<string>();
    int length = Line.Length;
    int iWordStart = iStart;
    bool flag = false;
    while (iStart < length)
    {
      char ch = Line[iStart];
      if (flag)
      {
        if (ch == '"')
        {
          if (iStart + 1 >= length || Line[iStart + 1] != '"')
            flag = false;
          else if (iStart + 2 < length && Line[iStart + 2] == '"')
          {
            flag = false;
            iStart += 2;
          }
          else
            ++iStart;
        }
      }
      else if (ch == '\n' || (int) ch == (int) Separator)
      {
        LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
        if (ch == '\n')
        {
          ++iStart;
          break;
        }
      }
      else if (ch == '"')
        flag = true;
      ++iStart;
    }
    if (iStart > iWordStart)
      LocalizationReader.AddCSVtoken(ref list, ref Line, iStart, ref iWordStart);
    return list.ToArray();
  }

  public static void AddCSVtoken(
    ref List<string> list,
    ref string Line,
    int iEnd,
    ref int iWordStart)
  {
    string str1 = Line.Substring(iWordStart, iEnd - iWordStart);
    iWordStart = iEnd + 1;
    string str2 = str1.Replace("\"\"", "\"");
    if (str2.Length > 1 && str2[0] == '"' && str2[str2.Length - 1] == '"')
      str2 = str2.Substring(1, str2.Length - 2);
    list.Add(str2);
  }

  public static List<string[]> ReadI2CSV(string Text)
  {
    string[] separator1 = new string[1]{ "[*]" };
    string[] separator2 = new string[1]{ "[ln]" };
    List<string[]> strArrayList = new List<string[]>();
    foreach (string str in Text.Split(separator2, StringSplitOptions.None))
      strArrayList.Add(str.Split(separator1, StringSplitOptions.None));
    return strArrayList;
  }

  public static void ValidateFullTerm(ref string Term)
  {
    Term = Term.Replace('\\', '/');
    int num = Term.IndexOf('/');
    if (num < 0)
      return;
    int startIndex;
    while ((startIndex = Term.LastIndexOf('/')) != num)
      Term = Term.Remove(startIndex, 1);
  }

  public static string EncodeString(string str)
  {
    return string.IsNullOrEmpty(str) ? string.Empty : str.Replace("\r\n", "<\\n>").Replace("\r", "<\\n>").Replace("\n", "<\\n>");
  }

  public static string DecodeString(string str)
  {
    return string.IsNullOrEmpty(str) ? string.Empty : str.Replace("<\\n>", "\r\n");
  }
}
