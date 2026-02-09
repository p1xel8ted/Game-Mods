// Decompiled with JetBrains decompiler
// Type: I2.Loc.TranslationJob_WEB
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
namespace I2.Loc;

public class TranslationJob_WEB : TranslationJob_WWW
{
  public Dictionary<string, TranslationQuery> _requests;
  public GoogleTranslation.fnOnTranslationReady _OnTranslationReady;
  public string mErrorMessage;
  public string mCurrentBatch_ToLanguageCode;
  public string mCurrentBatch_FromLanguageCode;
  public List<string> mCurrentBatch_Text;
  public List<KeyValuePair<string, string>> mQueries;

  public TranslationJob_WEB(
    Dictionary<string, TranslationQuery> requests,
    GoogleTranslation.fnOnTranslationReady OnTranslationReady)
  {
    this._requests = requests;
    this._OnTranslationReady = OnTranslationReady;
    this.FindAllQueries();
    this.ExecuteNextBatch();
  }

  public void FindAllQueries()
  {
    this.mQueries = new List<KeyValuePair<string, string>>();
    foreach (KeyValuePair<string, TranslationQuery> request in this._requests)
    {
      foreach (string str in request.Value.TargetLanguagesCode)
        this.mQueries.Add(new KeyValuePair<string, string>(request.Value.OrigText, $"{request.Value.LanguageCode}:{str}"));
    }
    this.mQueries.Sort((Comparison<KeyValuePair<string, string>>) ((a, b) => a.Value.CompareTo(b.Value)));
  }

  public void ExecuteNextBatch()
  {
    if (this.mQueries.Count == 0)
    {
      this.mJobState = TranslationJob.eJobState.Succeeded;
    }
    else
    {
      this.mCurrentBatch_Text = new List<string>();
      string str1 = (string) null;
      int num1 = 200;
      StringBuilder stringBuilder = new StringBuilder();
      int num2;
      for (num2 = 0; num2 < this.mQueries.Count; ++num2)
      {
        string key = this.mQueries[num2].Key;
        string str2 = this.mQueries[num2].Value;
        if (str1 == null || str2 == str1)
        {
          if (num2 != 0)
            stringBuilder.Append("|||");
          stringBuilder.Append(key);
          this.mCurrentBatch_Text.Add(key);
          str1 = str2;
        }
        if (stringBuilder.Length > num1)
          break;
      }
      this.mQueries.RemoveRange(0, num2);
      string[] strArray = str1.Split(':', StringSplitOptions.None);
      this.mCurrentBatch_FromLanguageCode = strArray[0];
      this.mCurrentBatch_ToLanguageCode = strArray[1];
      string str3 = $"http://www.google.com/translate_t?hl=en&vi=c&ie=UTF8&oe=UTF8&submit=Translate&langpair={this.mCurrentBatch_FromLanguageCode}|{this.mCurrentBatch_ToLanguageCode}&text={Uri.EscapeUriString(stringBuilder.ToString())}";
      Debug.Log((object) str3);
      this.www = UnityWebRequest.Get(str3);
      I2Utils.SendWebRequest(this.www);
    }
  }

  public override TranslationJob.eJobState GetState()
  {
    if (this.www != null && this.www.isDone)
    {
      this.ProcessResult(this.www.downloadHandler.data, this.www.error);
      this.www.Dispose();
      this.www = (UnityWebRequest) null;
    }
    if (this.www == null)
      this.ExecuteNextBatch();
    return this.mJobState;
  }

  public void ProcessResult(byte[] bytes, string errorMsg)
  {
    if (string.IsNullOrEmpty(errorMsg))
    {
      Debug.Log((object) this.ParseTranslationResult(Encoding.UTF8.GetString(bytes, 0, bytes.Length), "aab"));
      if (string.IsNullOrEmpty(errorMsg))
      {
        if (this._OnTranslationReady == null)
          return;
        this._OnTranslationReady(this._requests, (string) null);
        return;
      }
    }
    this.mJobState = TranslationJob.eJobState.Failed;
    this.mErrorMessage = errorMsg;
  }

  public string ParseTranslationResult(string html, string OriginalText)
  {
    try
    {
      int startIndex = html.IndexOf("TRANSLATED_TEXT='", StringComparison.Ordinal) + "TRANSLATED_TEXT='".Length;
      int num = html.IndexOf("';var", startIndex, StringComparison.Ordinal);
      string s = Regex.Replace(Regex.Replace(html.Substring(startIndex, num - startIndex), "\\\\x([a-fA-F0-9]{2})", (MatchEvaluator) (match => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value, NumberStyles.HexNumber)))), "&#(\\d+);", (MatchEvaluator) (match => char.ConvertFromUtf32(int.Parse(match.Groups[1].Value)))).Replace("<br>", "\n");
      if (OriginalText.ToUpper() == OriginalText)
        s = s.ToUpper();
      else if (GoogleTranslation.UppercaseFirst(OriginalText) == OriginalText)
        s = GoogleTranslation.UppercaseFirst(s);
      else if (GoogleTranslation.TitleCase(OriginalText) == OriginalText)
        s = GoogleTranslation.TitleCase(s);
      return s;
    }
    catch (Exception ex)
    {
      Debug.LogError((object) ex.Message);
      return string.Empty;
    }
  }
}
