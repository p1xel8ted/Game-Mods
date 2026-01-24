// Decompiled with JetBrains decompiler
// Type: YoutubeLight.HTTPHelperYoutube
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

#nullable disable
namespace YoutubeLight;

public static class HTTPHelperYoutube
{
  public static string HtmlDecode(string value) => value.DecodeHtmlChars();

  public static string DecodeHtmlChars(this string source)
  {
    string[] strArray = source.Split(new string[1]{ "&#x" }, StringSplitOptions.None);
    for (int index = 1; index < strArray.Length; ++index)
    {
      int length = strArray[index].IndexOf(';');
      string str = strArray[index].Substring(0, length);
      try
      {
        int int32 = Convert.ToInt32(str, 16 /*0x10*/);
        strArray[index] = ((char) int32).ToString() + strArray[index].Substring(length + 1);
      }
      catch
      {
      }
    }
    return string.Join("", strArray);
  }

  public static IDictionary<string, string> ParseQueryString(string s)
  {
    if (s.StartsWith("http") && s.Contains("?"))
      s = s.Substring(s.IndexOf('?') + 1);
    Dictionary<string, string> queryString = new Dictionary<string, string>();
    foreach (string input in Regex.Split(s, "&"))
    {
      string[] source = Regex.Split(input, "=");
      string key = source[0];
      string str = string.Empty;
      if (source.Length == 2)
        str = source[1];
      else if (source.Length > 2)
        str = string.Join("=", ((IEnumerable<string>) source).Skip<string>(1).Take<string>(source.Length).ToArray<string>());
      queryString.Add(key, str);
    }
    return (IDictionary<string, string>) queryString;
  }

  public static string ReplaceQueryStringParameter(
    string currentPageUrl,
    string paramToReplace,
    string newValue,
    string lsig)
  {
    IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(currentPageUrl);
    queryString[paramToReplace] = newValue;
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = true;
    foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) queryString)
    {
      if (!flag)
        stringBuilder.Append("&");
      if (keyValuePair.Key == nameof (lsig))
      {
        if (keyValuePair.Value == "" || keyValuePair.Value == string.Empty)
        {
          stringBuilder.Append(keyValuePair.Key);
          stringBuilder.Append("=");
          stringBuilder.Append(lsig);
        }
        else
        {
          stringBuilder.Append(keyValuePair.Key);
          stringBuilder.Append("=");
          stringBuilder.Append(keyValuePair.Value);
        }
      }
      else
      {
        stringBuilder.Append(keyValuePair.Key);
        stringBuilder.Append("=");
        stringBuilder.Append(keyValuePair.Value);
      }
      flag = false;
    }
    return new UriBuilder(currentPageUrl)
    {
      Query = stringBuilder.ToString()
    }.ToString();
  }

  public static string ReplaceQueryStringParameter(
    string currentPageUrl,
    string paramToReplace,
    string newValue)
  {
    IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(currentPageUrl);
    queryString[paramToReplace] = newValue;
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = true;
    foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) queryString)
    {
      if (!flag)
        stringBuilder.Append("&");
      if (keyValuePair.Key == "lsig")
      {
        if (keyValuePair.Value == "" || keyValuePair.Value == string.Empty)
        {
          stringBuilder.Append(keyValuePair.Key);
          stringBuilder.Append("=");
        }
        else
        {
          stringBuilder.Append(keyValuePair.Key);
          stringBuilder.Append("=");
          stringBuilder.Append(keyValuePair.Value);
        }
      }
      else
      {
        stringBuilder.Append(keyValuePair.Key);
        stringBuilder.Append("=");
        stringBuilder.Append(keyValuePair.Value);
      }
      flag = false;
    }
    return new UriBuilder(currentPageUrl)
    {
      Query = stringBuilder.ToString()
    }.ToString();
  }

  public static string ReplaceQueryStringParameterx(
    string currentPageUrl,
    string paramToReplace,
    string newValue)
  {
    IDictionary<string, string> queryString = HTTPHelperYoutube.ParseQueryString(currentPageUrl);
    queryString[paramToReplace] = newValue;
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = true;
    foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) queryString)
    {
      if (!flag)
        stringBuilder.Append("&");
      stringBuilder.Append(keyValuePair.Key);
      stringBuilder.Append("=");
      stringBuilder.Append(keyValuePair.Value);
      flag = false;
    }
    return new UriBuilder(currentPageUrl)
    {
      Query = stringBuilder.ToString()
    }.ToString();
  }

  public static string UrlDecode(string url) => UnityWebRequest.UnEscapeURL(url);
}
