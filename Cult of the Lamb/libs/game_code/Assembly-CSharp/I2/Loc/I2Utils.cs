// Decompiled with JetBrains decompiler
// Type: I2.Loc.I2Utils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#nullable disable
namespace I2.Loc;

public static class I2Utils
{
  public const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
  public const string NumberChars = "0123456789";
  public const string ValidNameSymbols = ".-_$#@*()[]{}+:?!&',^=<>~`";

  public static string ReverseText(string source)
  {
    // ISSUE: variable of a compiler-generated type
    I2Utils.\u003C\u003Ec__DisplayClass3_0 cDisplayClass30;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.source = source;
    // ISSUE: reference to a compiler-generated field
    int length = cDisplayClass30.source.Length;
    // ISSUE: reference to a compiler-generated field
    cDisplayClass30.output = new char[length];
    char[] anyOf = new char[2]{ '\r', '\n' };
    int index = 0;
label_6:
    while (index < length)
    {
      // ISSUE: reference to a compiler-generated field
      int num = cDisplayClass30.source.IndexOfAny(anyOf, index);
      if (num < 0)
        num = length;
      I2Utils.\u003CReverseText\u003Eg__Reverse\u007C3_0(index, num - 1, ref cDisplayClass30);
      index = num;
      while (true)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (index < length && (cDisplayClass30.source[index] == '\r' || cDisplayClass30.source[index] == '\n'))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          cDisplayClass30.output[index] = cDisplayClass30.source[index];
          ++index;
        }
        else
          goto label_6;
      }
    }
    // ISSUE: reference to a compiler-generated field
    return new string(cDisplayClass30.output);
  }

  public static string RemoveNonASCII(string text, bool allowCategory = false)
  {
    if (string.IsNullOrEmpty(text))
      return text;
    int length = 0;
    char[] chArray = new char[text.Length];
    bool flag = false;
    foreach (char c1 in text.Trim())
    {
      char c2 = ' ';
      if (allowCategory && (c1 == '\\' || c1 == '"' || c1 == '/') || char.IsLetterOrDigit(c1) || ".-_$#@*()[]{}+:?!&',^=<>~`".IndexOf(c1) >= 0)
        c2 = c1;
      if (char.IsWhiteSpace(c2))
      {
        if (!flag)
        {
          if (length > 0)
            chArray[length++] = ' ';
          flag = true;
        }
      }
      else
      {
        flag = false;
        chArray[length++] = c2;
      }
    }
    return new string(chArray, 0, length);
  }

  public static string GetValidTermName(string text, bool allowCategory = false)
  {
    if (text == null)
      return (string) null;
    text = I2Utils.RemoveTags(text);
    return I2Utils.RemoveNonASCII(text, allowCategory);
  }

  public static string SplitLine(string line, int maxCharacters)
  {
    if (maxCharacters <= 0 || line.Length < maxCharacters)
      return line;
    char[] charArray = line.ToCharArray();
    bool flag1 = true;
    bool flag2 = false;
    int index = 0;
    int num = 0;
    for (; index < charArray.Length; ++index)
    {
      if (flag1)
      {
        ++num;
        if (charArray[index] == '\n')
          num = 0;
        if (num >= maxCharacters && char.IsWhiteSpace(charArray[index]))
        {
          charArray[index] = '\n';
          flag1 = false;
          flag2 = false;
        }
      }
      else if (!char.IsWhiteSpace(charArray[index]))
      {
        flag1 = true;
        num = 0;
      }
      else if (charArray[index] != '\n')
      {
        charArray[index] = char.MinValue;
      }
      else
      {
        if (!flag2)
          charArray[index] = char.MinValue;
        flag2 = true;
      }
    }
    return new string(((IEnumerable<char>) charArray).Where<char>((Func<char, bool>) (c => c > char.MinValue)).ToArray<char>());
  }

  public static bool FindNextTag(string line, int iStart, out int tagStart, out int tagEnd)
  {
    tagStart = -1;
    tagEnd = -1;
    int length = line.Length;
    tagStart = iStart;
    while (tagStart < length && line[tagStart] != '[' && line[tagStart] != '(' && line[tagStart] != '{' && line[tagStart] != '<')
      ++tagStart;
    if (tagStart == length)
      return false;
    bool flag = false;
    tagEnd = tagStart + 1;
    while (tagEnd < length)
    {
      char ch = line[tagEnd];
      switch (ch)
      {
        case ')':
        case '>':
        case ']':
        case '}':
          return !flag || I2Utils.FindNextTag(line, tagEnd + 1, out tagStart, out tagEnd);
        default:
          if (ch > 'ÿ')
            flag = true;
          ++tagEnd;
          continue;
      }
    }
    return false;
  }

  public static string RemoveTags(string text)
  {
    return Regex.Replace(text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>", "");
  }

  public static bool RemoveResourcesPath(ref string sPath)
  {
    int num1 = Mathf.Max(sPath.IndexOf("\\Resources\\", StringComparison.Ordinal), sPath.IndexOf("\\Resources/", StringComparison.Ordinal), sPath.IndexOf("/Resources\\", StringComparison.Ordinal), sPath.IndexOf("/Resources/", StringComparison.Ordinal));
    bool flag = false;
    if (num1 >= 0)
    {
      sPath = sPath.Substring(num1 + 11);
      flag = true;
    }
    else
    {
      int num2 = sPath.LastIndexOfAny(LanguageSourceData.CategorySeparators);
      if (num2 > 0)
        sPath = sPath.Substring(num2 + 1);
    }
    string extension = Path.GetExtension(sPath);
    if (!string.IsNullOrEmpty(extension))
      sPath = sPath.Substring(0, sPath.Length - extension.Length);
    return flag;
  }

  public static bool IsPlaying() => Application.isPlaying;

  public static string GetPath(this Transform tr)
  {
    Transform parent = tr.parent;
    return (UnityEngine.Object) tr == (UnityEngine.Object) null ? tr.name : $"{parent.GetPath()}/{tr.name}";
  }

  public static Transform FindObject(string objectPath)
  {
    return I2Utils.FindObject(SceneManager.GetActiveScene(), objectPath);
  }

  public static Transform FindObject(Scene scene, string objectPath)
  {
    foreach (GameObject rootGameObject in scene.GetRootGameObjects())
    {
      Transform transform = rootGameObject.transform;
      if (transform.name == objectPath)
        return transform;
      if (objectPath.StartsWith(transform.name + "/", StringComparison.Ordinal))
        return I2Utils.FindObject(transform, objectPath.Substring(transform.name.Length + 1));
    }
    return (Transform) null;
  }

  public static Transform FindObject(Transform root, string objectPath)
  {
    for (int index = 0; index < root.childCount; ++index)
    {
      Transform child = root.GetChild(index);
      if (child.name == objectPath)
        return child;
      if (objectPath.StartsWith(child.name + "/", StringComparison.Ordinal))
        return I2Utils.FindObject(child, objectPath.Substring(child.name.Length + 1));
    }
    return (Transform) null;
  }

  public static H FindInParents<H>(Transform tr) where H : Component
  {
    if (!(bool) (UnityEngine.Object) tr)
      return default (H);
    H component;
    for (component = tr.GetComponent<H>(); !(bool) (UnityEngine.Object) component && (bool) (UnityEngine.Object) tr; tr = tr.parent)
      component = tr.GetComponent<H>();
    return component;
  }

  public static string GetCaptureMatch(Match match)
  {
    for (int groupnum = match.Groups.Count - 1; groupnum >= 0; --groupnum)
    {
      if (match.Groups[groupnum].Success)
        return match.Groups[groupnum].ToString();
    }
    return match.ToString();
  }

  public static void SendWebRequest(UnityWebRequest www) => www.SendWebRequest();

  [CompilerGenerated]
  public static void \u003CReverseText\u003Eg__Reverse\u007C3_0(
    int start,
    int end,
    [In] ref I2Utils.\u003C\u003Ec__DisplayClass3_0 obj2)
  {
    for (int index = 0; index <= end - start; ++index)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      obj2.output[end - index] = obj2.source[start + index];
    }
  }
}
