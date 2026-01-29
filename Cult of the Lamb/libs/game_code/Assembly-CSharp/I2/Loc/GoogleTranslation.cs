// Decompiled with JetBrains decompiler
// Type: I2.Loc.GoogleTranslation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

#nullable disable
namespace I2.Loc;

public static class GoogleTranslation
{
  public static List<UnityWebRequest> mCurrentTranslations = new List<UnityWebRequest>();
  public static List<TranslationJob> mTranslationJobs = new List<TranslationJob>();

  public static bool CanTranslate()
  {
    return LocalizationManager.Sources.Count > 0 && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL());
  }

  public static void Translate(
    string text,
    string LanguageCodeFrom,
    string LanguageCodeTo,
    GoogleTranslation.fnOnTranslated OnTranslationReady)
  {
    LocalizationManager.InitializeIfNeeded();
    if (!GoogleTranslation.CanTranslate())
      OnTranslationReady((string) null, "WebService is not set correctly or needs to be reinstalled");
    else if (LanguageCodeTo == LanguageCodeFrom)
    {
      OnTranslationReady(text, (string) null);
    }
    else
    {
      Dictionary<string, TranslationQuery> queries = new Dictionary<string, TranslationQuery>((IEqualityComparer<string>) StringComparer.Ordinal);
      if (string.IsNullOrEmpty(LanguageCodeTo))
      {
        OnTranslationReady(string.Empty, (string) null);
      }
      else
      {
        GoogleTranslation.CreateQueries(text, LanguageCodeFrom, LanguageCodeTo, queries);
        GoogleTranslation.Translate(queries, (GoogleTranslation.fnOnTranslationReady) ((results, error) =>
        {
          if (!string.IsNullOrEmpty(error) || results.Count == 0)
            OnTranslationReady((string) null, error);
          else
            OnTranslationReady(GoogleTranslation.RebuildTranslation(text, queries, LanguageCodeTo), (string) null);
        }));
      }
    }
  }

  public static string ForceTranslate(string text, string LanguageCodeFrom, string LanguageCodeTo)
  {
    Dictionary<string, TranslationQuery> dictionary = new Dictionary<string, TranslationQuery>((IEqualityComparer<string>) StringComparer.Ordinal);
    GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dictionary);
    TranslationJob_Main translationJobMain = new TranslationJob_Main(dictionary, (GoogleTranslation.fnOnTranslationReady) null);
    TranslationJob.eJobState state;
    do
    {
      state = translationJobMain.GetState();
    }
    while (state == TranslationJob.eJobState.Running);
    return state == TranslationJob.eJobState.Failed ? (string) null : GoogleTranslation.GetQueryResult(text, "", dictionary);
  }

  public static void Translate(
    Dictionary<string, TranslationQuery> requests,
    GoogleTranslation.fnOnTranslationReady OnTranslationReady,
    bool usePOST = true)
  {
    GoogleTranslation.AddTranslationJob((TranslationJob) new TranslationJob_Main(requests, OnTranslationReady));
  }

  public static bool ForceTranslate(Dictionary<string, TranslationQuery> requests, bool usePOST = true)
  {
    TranslationJob_Main translationJobMain = new TranslationJob_Main(requests, (GoogleTranslation.fnOnTranslationReady) null);
    TranslationJob.eJobState state;
    do
    {
      state = translationJobMain.GetState();
    }
    while (state == TranslationJob.eJobState.Running);
    return state != TranslationJob.eJobState.Failed;
  }

  public static List<string> ConvertTranslationRequest(
    Dictionary<string, TranslationQuery> requests,
    bool encodeGET)
  {
    List<string> stringList = new List<string>();
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<string, TranslationQuery> request in requests)
    {
      TranslationQuery translationQuery = request.Value;
      if (stringBuilder.Length > 0)
        stringBuilder.Append("<I2Loc>");
      stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(translationQuery.LanguageCode));
      stringBuilder.Append(":");
      for (int index = 0; index < translationQuery.TargetLanguagesCode.Length; ++index)
      {
        if (index != 0)
          stringBuilder.Append(",");
        stringBuilder.Append(GoogleLanguages.GetGoogleLanguageCode(translationQuery.TargetLanguagesCode[index]));
      }
      stringBuilder.Append("=");
      string stringToEscape = GoogleTranslation.TitleCase(translationQuery.Text) == translationQuery.Text ? translationQuery.Text.ToLowerInvariant() : translationQuery.Text;
      if (!encodeGET)
      {
        stringBuilder.Append(stringToEscape);
      }
      else
      {
        stringBuilder.Append(Uri.EscapeDataString(stringToEscape));
        if (stringBuilder.Length > 4000)
        {
          stringList.Add(stringBuilder.ToString());
          stringBuilder.Length = 0;
        }
      }
    }
    stringList.Add(stringBuilder.ToString());
    return stringList;
  }

  public static void AddTranslationJob(TranslationJob job)
  {
    GoogleTranslation.mTranslationJobs.Add(job);
    if (GoogleTranslation.mTranslationJobs.Count != 1)
      return;
    CoroutineManager.Start(GoogleTranslation.WaitForTranslations());
  }

  public static IEnumerator WaitForTranslations()
  {
    while (GoogleTranslation.mTranslationJobs.Count > 0)
    {
      foreach (TranslationJob translationJob in GoogleTranslation.mTranslationJobs.ToArray())
      {
        if (translationJob.GetState() != TranslationJob.eJobState.Running)
          GoogleTranslation.mTranslationJobs.Remove(translationJob);
      }
      yield return (object) null;
    }
  }

  public static string ParseTranslationResult(
    string html,
    Dictionary<string, TranslationQuery> requests)
  {
    if (html.StartsWith("<!DOCTYPE html>") || html.StartsWith("<HTML>"))
    {
      if (html.Contains("The script completed but did not return anything"))
        return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
      return html.Contains("Service invoked too many times in a short time") ? "" : "There was a problem contacting the WebService. Please try again later\n" + html;
    }
    string[] strArray = html.Split(new string[1]
    {
      "<I2Loc>"
    }, StringSplitOptions.None);
    string[] separator = new string[1]{ "<i2>" };
    int num = 0;
    foreach (string str1 in requests.Keys.ToArray<string>())
    {
      TranslationQuery queryFromOrigText = GoogleTranslation.FindQueryFromOrigText(str1, requests);
      string str2 = strArray[num++];
      if (queryFromOrigText.Tags != null)
      {
        for (int tagNumber = queryFromOrigText.Tags.Length - 1; tagNumber >= 0; --tagNumber)
          str2 = str2.Replace(GoogleTranslation.GetGoogleNoTranslateTag(tagNumber), queryFromOrigText.Tags[tagNumber]);
      }
      queryFromOrigText.Results = str2.Split(separator, StringSplitOptions.None);
      if (GoogleTranslation.TitleCase(str1) == str1)
      {
        for (int index = 0; index < queryFromOrigText.Results.Length; ++index)
          queryFromOrigText.Results[index] = GoogleTranslation.TitleCase(queryFromOrigText.Results[index]);
      }
      requests[queryFromOrigText.OrigText] = queryFromOrigText;
    }
    return (string) null;
  }

  public static bool IsTranslating()
  {
    return GoogleTranslation.mCurrentTranslations.Count > 0 || GoogleTranslation.mTranslationJobs.Count > 0;
  }

  public static void CancelCurrentGoogleTranslations()
  {
    GoogleTranslation.mCurrentTranslations.Clear();
    foreach (TranslationJob mTranslationJob in GoogleTranslation.mTranslationJobs)
      mTranslationJob.Dispose();
    GoogleTranslation.mTranslationJobs.Clear();
  }

  public static void CreateQueries(
    string text,
    string LanguageCodeFrom,
    string LanguageCodeTo,
    Dictionary<string, TranslationQuery> dict)
  {
    if (!text.Contains("[i2s_"))
    {
      GoogleTranslation.CreateQueries_Plurals(text, LanguageCodeFrom, LanguageCodeTo, dict);
    }
    else
    {
      foreach (KeyValuePair<string, string> specialization in SpecializationManager.GetSpecializations(text))
        GoogleTranslation.CreateQueries_Plurals(specialization.Value, LanguageCodeFrom, LanguageCodeTo, dict);
    }
  }

  public static void CreateQueries_Plurals(
    string text,
    string LanguageCodeFrom,
    string LanguageCodeTo,
    Dictionary<string, TranslationQuery> dict)
  {
    bool flag1 = text.Contains("{[#");
    bool flag2 = text.Contains("[i2p_");
    if (!GoogleTranslation.HasParameters(text) || !flag1 && !flag2)
    {
      GoogleTranslation.AddQuery(text, LanguageCodeFrom, LanguageCodeTo, dict);
    }
    else
    {
      bool forceTag = flag1;
      for (ePluralType pluralType1 = ePluralType.Zero; pluralType1 <= ePluralType.Plural; ++pluralType1)
      {
        string pluralType2 = pluralType1.ToString();
        if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, pluralType2))
        {
          string text1 = GoogleTranslation.GetPluralText(text, pluralType2);
          int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, pluralType1);
          string pluralParameter = GoogleTranslation.GetPluralParameter(text1, forceTag);
          if (!string.IsNullOrEmpty(pluralParameter))
            text1 = text1.Replace(pluralParameter, pluralTestNumber.ToString());
          GoogleTranslation.AddQuery(text1, LanguageCodeFrom, LanguageCodeTo, dict);
        }
      }
    }
  }

  public static void AddQuery(
    string text,
    string LanguageCodeFrom,
    string LanguageCodeTo,
    Dictionary<string, TranslationQuery> dict)
  {
    if (string.IsNullOrEmpty(text))
      return;
    if (!dict.ContainsKey(text))
    {
      TranslationQuery query = new TranslationQuery()
      {
        OrigText = text,
        LanguageCode = LanguageCodeFrom,
        TargetLanguagesCode = new string[1]
        {
          LanguageCodeTo
        }
      } with
      {
        Text = text
      };
      GoogleTranslation.ParseNonTranslatableElements(ref query);
      dict[text] = query;
    }
    else
    {
      TranslationQuery translationQuery = dict[text];
      if (Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo) < 0)
        translationQuery.TargetLanguagesCode = ((IEnumerable<string>) translationQuery.TargetLanguagesCode).Concat<string>((IEnumerable<string>) new string[1]
        {
          LanguageCodeTo
        }).Distinct<string>().ToArray<string>();
      dict[text] = translationQuery;
    }
  }

  public static string GetTranslation(
    string text,
    string LanguageCodeTo,
    Dictionary<string, TranslationQuery> dict)
  {
    if (!dict.ContainsKey(text))
      return (string) null;
    TranslationQuery translationQuery = dict[text];
    int index = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
    return index < 0 || translationQuery.Results == null ? "" : translationQuery.Results[index];
  }

  public static TranslationQuery FindQueryFromOrigText(
    string origText,
    Dictionary<string, TranslationQuery> dict)
  {
    foreach (KeyValuePair<string, TranslationQuery> keyValuePair in dict)
    {
      if (keyValuePair.Value.OrigText == origText)
        return keyValuePair.Value;
    }
    return new TranslationQuery();
  }

  public static bool HasParameters(string text)
  {
    int startIndex = text.IndexOf("{[", StringComparison.Ordinal);
    return startIndex >= 0 && text.IndexOf("]}", startIndex, StringComparison.Ordinal) > 0;
  }

  public static string GetPluralParameter(string text, bool forceTag)
  {
    int startIndex = text.IndexOf("{[#", StringComparison.Ordinal);
    if (startIndex < 0)
    {
      if (forceTag)
        return (string) null;
      startIndex = text.IndexOf("{[", StringComparison.Ordinal);
    }
    if (startIndex < 0)
      return (string) null;
    int num = text.IndexOf("]}", startIndex + 2, StringComparison.Ordinal);
    return num < 0 ? (string) null : text.Substring(startIndex, num - startIndex + 2);
  }

  public static string GetPluralText(string text, string pluralType)
  {
    pluralType = $"[i2p_{pluralType}]";
    int num1 = text.IndexOf(pluralType, StringComparison.Ordinal);
    if (num1 >= 0)
    {
      int startIndex = num1 + pluralType.Length;
      int num2 = text.IndexOf("[i2p_", startIndex, StringComparison.Ordinal);
      if (num2 < 0)
        num2 = text.Length;
      return text.Substring(startIndex, num2 - startIndex);
    }
    int length = text.IndexOf("[i2p_", StringComparison.Ordinal);
    if (length < 0)
      return text;
    if (length > 0)
      return text.Substring(0, length);
    int num3 = text.IndexOf("]", StringComparison.Ordinal);
    if (num3 < 0)
      return text;
    int startIndex1 = num3 + 1;
    int num4 = text.IndexOf("[i2p_", startIndex1, StringComparison.Ordinal);
    if (num4 < 0)
      num4 = text.Length;
    return text.Substring(startIndex1, num4 - startIndex1);
  }

  public static int FindClosingTag(string tag, MatchCollection matches, int startIndex)
  {
    int i = startIndex;
    for (int count = matches.Count; i < count; ++i)
    {
      string captureMatch = I2Utils.GetCaptureMatch(matches[i]);
      if (captureMatch[0] == '/' && tag.StartsWith(captureMatch.Substring(1), StringComparison.Ordinal))
        return i;
    }
    return -1;
  }

  public static string GetGoogleNoTranslateTag(int tagNumber)
  {
    if (tagNumber < 70)
      return "++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++".Substring(0, tagNumber + 1);
    string googleNoTranslateTag = "";
    for (int index = -1; index < tagNumber; ++index)
      googleNoTranslateTag += "+";
    return googleNoTranslateTag;
  }

  public static void ParseNonTranslatableElements(ref TranslationQuery query)
  {
    MatchCollection matches = Regex.Matches(query.Text, "\\{\\[(.*?)]}|\\[(.*?)]|\\<(.*?)>");
    if (matches == null || matches.Count == 0)
      return;
    string str1 = query.Text;
    List<string> stringList = new List<string>();
    int num = 0;
    for (int count = matches.Count; num < count; ++num)
    {
      string captureMatch = I2Utils.GetCaptureMatch(matches[num]);
      int closingTag = GoogleTranslation.FindClosingTag(captureMatch, matches, num);
      if (closingTag < 0)
      {
        string oldValue = matches[num].ToString();
        if (oldValue.StartsWith("{[", StringComparison.Ordinal) && oldValue.EndsWith("]}", StringComparison.Ordinal))
        {
          str1 = str1.Replace(oldValue, GoogleTranslation.GetGoogleNoTranslateTag(stringList.Count) + " ");
          stringList.Add(oldValue);
        }
      }
      else if (captureMatch == "i2nt")
      {
        string oldValue = query.Text.Substring(matches[num].Index, matches[closingTag].Index - matches[num].Index + matches[closingTag].Length);
        str1 = str1.Replace(oldValue, GoogleTranslation.GetGoogleNoTranslateTag(stringList.Count) + " ");
        stringList.Add(oldValue);
      }
      else
      {
        string oldValue1 = matches[num].ToString();
        string str2 = str1.Replace(oldValue1, GoogleTranslation.GetGoogleNoTranslateTag(stringList.Count) + " ");
        stringList.Add(oldValue1);
        string oldValue2 = matches[closingTag].ToString();
        str1 = str2.Replace(oldValue2, GoogleTranslation.GetGoogleNoTranslateTag(stringList.Count) + " ");
        stringList.Add(oldValue2);
      }
    }
    query.Text = str1;
    query.Tags = stringList.ToArray();
  }

  public static string GetQueryResult(
    string text,
    string LanguageCodeTo,
    Dictionary<string, TranslationQuery> dict)
  {
    if (!dict.ContainsKey(text))
      return (string) null;
    TranslationQuery translationQuery = dict[text];
    if (translationQuery.Results == null || translationQuery.Results.Length < 0)
      return (string) null;
    if (string.IsNullOrEmpty(LanguageCodeTo))
      return translationQuery.Results[0];
    int index = Array.IndexOf<string>(translationQuery.TargetLanguagesCode, LanguageCodeTo);
    return index < 0 ? (string) null : translationQuery.Results[index];
  }

  public static string RebuildTranslation(
    string text,
    Dictionary<string, TranslationQuery> dict,
    string LanguageCodeTo)
  {
    if (!text.Contains("[i2s_"))
      return GoogleTranslation.RebuildTranslation_Plural(text, dict, LanguageCodeTo);
    Dictionary<string, string> specializations1 = SpecializationManager.GetSpecializations(text);
    Dictionary<string, string> specializations2 = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.Ordinal);
    foreach (KeyValuePair<string, string> keyValuePair in specializations1)
      specializations2[keyValuePair.Key] = GoogleTranslation.RebuildTranslation_Plural(keyValuePair.Value, dict, LanguageCodeTo);
    return SpecializationManager.SetSpecializedText(specializations2);
  }

  public static string RebuildTranslation_Plural(
    string text,
    Dictionary<string, TranslationQuery> dict,
    string LanguageCodeTo)
  {
    bool flag1 = text.Contains("{[#");
    bool flag2 = text.Contains("[i2p_");
    if (!GoogleTranslation.HasParameters(text) || !flag1 && !flag2)
      return GoogleTranslation.GetTranslation(text, LanguageCodeTo, dict);
    StringBuilder stringBuilder = new StringBuilder();
    string str1 = (string) null;
    bool forceTag = flag1;
    for (ePluralType pluralType1 = ePluralType.Plural; pluralType1 >= ePluralType.Zero; --pluralType1)
    {
      string pluralType2 = pluralType1.ToString();
      if (GoogleLanguages.LanguageHasPluralType(LanguageCodeTo, pluralType2))
      {
        string text1 = GoogleTranslation.GetPluralText(text, pluralType2);
        int pluralTestNumber = GoogleLanguages.GetPluralTestNumber(LanguageCodeTo, pluralType1);
        string pluralParameter = GoogleTranslation.GetPluralParameter(text1, forceTag);
        if (!string.IsNullOrEmpty(pluralParameter))
          text1 = text1.Replace(pluralParameter, pluralTestNumber.ToString());
        string str2 = GoogleTranslation.GetTranslation(text1, LanguageCodeTo, dict);
        if (!string.IsNullOrEmpty(pluralParameter))
          str2 = str2.Replace(pluralTestNumber.ToString(), pluralParameter);
        if (pluralType1 == ePluralType.Plural)
          str1 = str2;
        else if (!(str2 == str1))
          stringBuilder.AppendFormat("[i2p_{0}]", (object) pluralType2);
        else
          continue;
        stringBuilder.Append(str2);
      }
    }
    return stringBuilder.ToString();
  }

  public static string UppercaseFirst(string s)
  {
    if (string.IsNullOrEmpty(s))
      return string.Empty;
    char[] charArray = s.ToLower().ToCharArray();
    charArray[0] = char.ToUpper(charArray[0]);
    return new string(charArray);
  }

  public static string TitleCase(string s)
  {
    return string.IsNullOrEmpty(s) ? string.Empty : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s);
  }

  public delegate void fnOnTranslated(string Translation, string Error);

  public delegate void fnOnTranslationReady(Dictionary<string, TranslationQuery> dict, string error);
}
