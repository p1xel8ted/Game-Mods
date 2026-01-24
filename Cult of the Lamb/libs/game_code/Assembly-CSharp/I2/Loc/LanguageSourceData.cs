// Decompiled with JetBrains decompiler
// Type: I2.Loc.LanguageSourceData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

#nullable disable
namespace I2.Loc;

[ExecuteInEditMode]
[Serializable]
public class LanguageSourceData
{
  [NonSerialized]
  public ILanguageSource owner;
  public bool UserAgreesToHaveItOnTheScene;
  public bool UserAgreesToHaveItInsideThePluginsFolder;
  public bool GoogleLiveSyncIsUptoDate = true;
  [NonSerialized]
  public bool mIsGlobalSource;
  public List<TermData> mTerms = new List<TermData>();
  public bool CaseInsensitiveTerms;
  [NonSerialized]
  public Dictionary<string, TermData> mDictionary = new Dictionary<string, TermData>((IEqualityComparer<string>) StringComparer.Ordinal);
  public LanguageSourceData.MissingTranslationAction OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Fallback;
  public string mTerm_AppName;
  public List<LanguageData> mLanguages = new List<LanguageData>();
  public bool IgnoreDeviceLanguage;
  public LanguageSourceData.eAllowUnloadLanguages _AllowUnloadingLanguages;
  public string Google_WebServiceURL;
  public string Google_SpreadsheetKey;
  public string Google_SpreadsheetName;
  public string Google_LastUpdatedVersion;
  public LanguageSourceData.eGoogleUpdateFrequency GoogleUpdateFrequency = LanguageSourceData.eGoogleUpdateFrequency.Weekly;
  public LanguageSourceData.eGoogleUpdateFrequency GoogleInEditorCheckFrequency = LanguageSourceData.eGoogleUpdateFrequency.Daily;
  public LanguageSourceData.eGoogleUpdateSynchronization GoogleUpdateSynchronization = LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded;
  public float GoogleUpdateDelay;
  public List<UnityEngine.Object> Assets = new List<UnityEngine.Object>();
  [NonSerialized]
  public Dictionary<string, UnityEngine.Object> mAssetDictionary = new Dictionary<string, UnityEngine.Object>((IEqualityComparer<string>) StringComparer.Ordinal);
  public string mDelayedGoogleData;
  public static string EmptyCategory = "Default";
  public static char[] CategorySeparators = "/\\".ToCharArray();

  public UnityEngine.Object ownerObject => this.owner as UnityEngine.Object;

  public event LanguageSource.fnOnSourceUpdated Event_OnSourceUpdateFromGoogle;

  public void Awake()
  {
    LocalizationManager.AddSource(this);
    this.UpdateDictionary();
    this.UpdateAssetDictionary();
    LocalizationManager.LocalizeAll(true);
  }

  public void OnDestroy() => LocalizationManager.RemoveSource(this);

  public bool IsEqualTo(LanguageSourceData Source)
  {
    if (Source.mLanguages.Count != this.mLanguages.Count)
      return false;
    int index1 = 0;
    for (int count = this.mLanguages.Count; index1 < count; ++index1)
    {
      if (Source.GetLanguageIndex(this.mLanguages[index1].Name) < 0)
        return false;
    }
    if (Source.mTerms.Count != this.mTerms.Count)
      return false;
    for (int index2 = 0; index2 < this.mTerms.Count; ++index2)
    {
      if (Source.GetTermData(this.mTerms[index2].Term) == null)
        return false;
    }
    return true;
  }

  public bool ManagerHasASimilarSource()
  {
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      LanguageSourceData source = LocalizationManager.Sources[index];
      if (source != null && source.IsEqualTo(this) && source != this)
        return true;
    }
    return false;
  }

  public void ClearAllData()
  {
    this.mTerms.Clear();
    this.mLanguages.Clear();
    this.mDictionary.Clear();
    this.mAssetDictionary.Clear();
  }

  public bool IsGlobalSource() => this.mIsGlobalSource;

  public void Editor_SetDirty()
  {
  }

  public void UpdateAssetDictionary()
  {
    this.Assets.RemoveAll((Predicate<UnityEngine.Object>) (x => x == (UnityEngine.Object) null));
    this.mAssetDictionary = this.Assets.Distinct<UnityEngine.Object>().GroupBy<UnityEngine.Object, string>((Func<UnityEngine.Object, string>) (o => o.name), (IEqualityComparer<string>) StringComparer.Ordinal).ToDictionary<IGrouping<string, UnityEngine.Object>, string, UnityEngine.Object>((Func<IGrouping<string, UnityEngine.Object>, string>) (g => g.Key), (Func<IGrouping<string, UnityEngine.Object>, UnityEngine.Object>) (g => g.First<UnityEngine.Object>()), (IEqualityComparer<string>) StringComparer.Ordinal);
  }

  public UnityEngine.Object FindAsset(string Name)
  {
    if (this.Assets != null)
    {
      if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.Assets.Count)
        this.UpdateAssetDictionary();
      UnityEngine.Object asset;
      if (this.mAssetDictionary.TryGetValue(Name, out asset))
        return asset;
    }
    return (UnityEngine.Object) null;
  }

  public bool HasAsset(UnityEngine.Object Obj) => this.Assets.Contains(Obj);

  public void AddAsset(UnityEngine.Object Obj)
  {
    if (this.Assets.Contains(Obj))
      return;
    this.Assets.Add(Obj);
    this.UpdateAssetDictionary();
  }

  public string Export_I2CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
  {
    StringBuilder Builder = new StringBuilder();
    Builder.Append("Key[*]Type[*]Desc");
    foreach (LanguageData mLanguage in this.mLanguages)
    {
      Builder.Append("[*]");
      if (!mLanguage.IsEnabled())
        Builder.Append('$');
      Builder.Append(GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code));
    }
    Builder.Append("[ln]");
    this.mTerms.Sort((Comparison<TermData>) ((a, b) => string.CompareOrdinal(a.Term, b.Term)));
    int count = this.mLanguages.Count;
    bool flag = true;
    foreach (TermData mTerm in this.mTerms)
    {
      string Term;
      if (string.IsNullOrEmpty(Category) || Category == LanguageSourceData.EmptyCategory && mTerm.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0)
        Term = mTerm.Term;
      else if (mTerm.Term.StartsWith(Category + "/", StringComparison.Ordinal) && Category != mTerm.Term)
        Term = mTerm.Term.Substring(Category.Length + 1);
      else
        continue;
      if (!flag)
        Builder.Append("[ln]");
      flag = false;
      if (!specializationsAsRows)
      {
        LanguageSourceData.AppendI2Term(Builder, count, Term, mTerm, Separator, (string) null);
      }
      else
      {
        List<string> allSpecializations = mTerm.GetAllSpecializations();
        for (int index = 0; index < allSpecializations.Count; ++index)
        {
          if (index != 0)
            Builder.Append("[ln]");
          string forceSpecialization = allSpecializations[index];
          LanguageSourceData.AppendI2Term(Builder, count, Term, mTerm, Separator, forceSpecialization);
        }
      }
    }
    return Builder.ToString();
  }

  public static void AppendI2Term(
    StringBuilder Builder,
    int nLanguages,
    string Term,
    TermData termData,
    char Separator,
    string forceSpecialization)
  {
    LanguageSourceData.AppendI2Text(Builder, Term);
    if (!string.IsNullOrEmpty(forceSpecialization) && forceSpecialization != "Any")
    {
      Builder.Append("[");
      Builder.Append(forceSpecialization);
      Builder.Append("]");
    }
    Builder.Append("[*]");
    Builder.Append(termData.TermType.ToString());
    Builder.Append("[*]");
    Builder.Append(termData.Description);
    for (int idx = 0; idx < Mathf.Min(nLanguages, termData.Languages.Length); ++idx)
    {
      Builder.Append("[*]");
      string text = termData.Languages[idx];
      if (!string.IsNullOrEmpty(forceSpecialization))
        text = termData.GetTranslation(idx, forceSpecialization);
      LanguageSourceData.AppendI2Text(Builder, text);
    }
  }

  public static void AppendI2Text(StringBuilder Builder, string text)
  {
    if (string.IsNullOrEmpty(text))
      return;
    if (text.StartsWith("'", StringComparison.Ordinal) || text.StartsWith("=", StringComparison.Ordinal))
      Builder.Append('\'');
    Builder.Append(text);
  }

  public string Export_Language_to_Cache(int langIndex, bool fillTermWithFallback)
  {
    if (!this.mLanguages[langIndex].IsLoaded())
      return (string) null;
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < this.mTerms.Count; ++index)
    {
      if (index > 0)
        stringBuilder.Append("[i2t]");
      TermData mTerm = this.mTerms[index];
      stringBuilder.Append(mTerm.Term);
      stringBuilder.Append("=");
      string Translation = mTerm.Languages[langIndex];
      if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && string.IsNullOrEmpty(Translation) && this.TryGetFallbackTranslation(mTerm, out Translation, langIndex, skipDisabled: true))
      {
        stringBuilder.Append("[i2fb]");
        if (fillTermWithFallback)
          mTerm.Languages[langIndex] = Translation;
      }
      if (!string.IsNullOrEmpty(Translation))
        stringBuilder.Append(Translation);
    }
    return stringBuilder.ToString();
  }

  public string Export_CSV(string Category, char Separator = ',', bool specializationsAsRows = true)
  {
    StringBuilder Builder = new StringBuilder();
    int count = this.mLanguages.Count;
    Builder.AppendFormat("Key{0}Type{0}Desc", (object) Separator);
    foreach (LanguageData mLanguage in this.mLanguages)
    {
      Builder.Append(Separator);
      if (!mLanguage.IsEnabled())
        Builder.Append('$');
      LanguageSourceData.AppendString(Builder, GoogleLanguages.GetCodedLanguage(mLanguage.Name, mLanguage.Code), Separator);
    }
    Builder.Append("\n");
    this.mTerms.Sort((Comparison<TermData>) ((a, b) => string.CompareOrdinal(a.Term, b.Term)));
    foreach (TermData mTerm in this.mTerms)
    {
      string Term;
      if (string.IsNullOrEmpty(Category) || Category == LanguageSourceData.EmptyCategory && mTerm.Term.IndexOfAny(LanguageSourceData.CategorySeparators) < 0)
        Term = mTerm.Term;
      else if (mTerm.Term.StartsWith(Category + "/", StringComparison.Ordinal) && Category != mTerm.Term)
        Term = mTerm.Term.Substring(Category.Length + 1);
      else
        continue;
      if (specializationsAsRows)
      {
        foreach (string allSpecialization in mTerm.GetAllSpecializations())
          LanguageSourceData.AppendTerm(Builder, count, Term, mTerm, allSpecialization, Separator);
      }
      else
        LanguageSourceData.AppendTerm(Builder, count, Term, mTerm, (string) null, Separator);
    }
    return Builder.ToString();
  }

  public static void AppendTerm(
    StringBuilder Builder,
    int nLanguages,
    string Term,
    TermData termData,
    string specialization,
    char Separator)
  {
    LanguageSourceData.AppendString(Builder, Term, Separator);
    if (!string.IsNullOrEmpty(specialization) && specialization != "Any")
      Builder.AppendFormat("[{0}]", (object) specialization);
    Builder.Append(Separator);
    Builder.Append(termData.TermType.ToString());
    Builder.Append(Separator);
    LanguageSourceData.AppendString(Builder, termData.Description, Separator);
    for (int idx = 0; idx < Mathf.Min(nLanguages, termData.Languages.Length); ++idx)
    {
      Builder.Append(Separator);
      string Text = termData.Languages[idx];
      if (!string.IsNullOrEmpty(specialization))
        Text = termData.GetTranslation(idx, specialization);
      LanguageSourceData.AppendTranslation(Builder, Text, Separator, (string) null);
    }
    Builder.Append("\n");
  }

  public static void AppendString(StringBuilder Builder, string Text, char Separator)
  {
    if (string.IsNullOrEmpty(Text))
      return;
    Text = Text.Replace("\\n", "\n");
    if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
    {
      Text = Text.Replace("\"", "\"\"");
      Builder.AppendFormat("\"{0}\"", (object) Text);
    }
    else
      Builder.Append(Text);
  }

  public static void AppendTranslation(
    StringBuilder Builder,
    string Text,
    char Separator,
    string tags)
  {
    if (string.IsNullOrEmpty(Text))
      return;
    Text = Text.Replace("\\n", "\n");
    if (Text.IndexOfAny((Separator.ToString() + "\n\"").ToCharArray()) >= 0)
    {
      Text = Text.Replace("\"", "\"\"");
      Builder.AppendFormat("\"{0}{1}\"", (object) tags, (object) Text);
    }
    else
    {
      Builder.Append(tags);
      Builder.Append(Text);
    }
  }

  public UnityWebRequest Export_Google_CreateWWWcall(eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
  {
    string data = this.Export_Google_CreateData();
    WWWForm formData = new WWWForm();
    formData.AddField("key", this.Google_SpreadsheetKey);
    formData.AddField("action", "SetLanguageSource");
    formData.AddField("data", data);
    formData.AddField("updateMode", UpdateMode.ToString());
    UnityWebRequest www = UnityWebRequest.Post(LocalizationManager.GetWebServiceURL(this), formData);
    I2Utils.SendWebRequest(www);
    return www;
  }

  public string Export_Google_CreateData()
  {
    List<string> categories = this.GetCategories(true);
    StringBuilder stringBuilder = new StringBuilder();
    bool flag = true;
    foreach (string Category in categories)
    {
      if (flag)
        flag = false;
      else
        stringBuilder.Append("<I2Loc>");
      bool specializationsAsRows = true;
      string str = this.Export_I2CSV(Category, specializationsAsRows: specializationsAsRows);
      stringBuilder.Append(Category);
      stringBuilder.Append("<I2Loc>");
      stringBuilder.Append(str);
    }
    return stringBuilder.ToString();
  }

  public string Import_CSV(
    string Category,
    string CSVstring,
    eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace,
    char Separator = ',')
  {
    List<string[]> CSV = LocalizationReader.ReadCSV(CSVstring, Separator);
    return this.Import_CSV(Category, CSV, UpdateMode);
  }

  public string Import_I2CSV(
    string Category,
    string I2CSVstring,
    eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
  {
    List<string[]> CSV = LocalizationReader.ReadI2CSV(I2CSVstring);
    return this.Import_CSV(Category, CSV, UpdateMode);
  }

  public string Import_CSV(string Category, List<string[]> CSV, eSpreadsheetUpdateMode UpdateMode = eSpreadsheetUpdateMode.Replace)
  {
    string[] strArray1 = CSV[0];
    int num1 = 1;
    int index1 = -1;
    int index2 = -1;
    string[] strArray2 = new string[1]{ "Key" };
    string[] strArray3 = new string[1]{ "Type" };
    string[] strArray4 = new string[2]
    {
      "Desc",
      "Description"
    };
    if (strArray1.Length <= 1 || !this.ArrayContains(strArray1[0], strArray2))
      return "Bad Spreadsheet Format.\nFirst columns should be 'Key', 'Type' and 'Desc'";
    if (UpdateMode == eSpreadsheetUpdateMode.Replace)
      this.ClearAllData();
    if (strArray1.Length > 2)
    {
      if (this.ArrayContains(strArray1[1], strArray3))
      {
        index1 = 1;
        num1 = 2;
      }
      if (this.ArrayContains(strArray1[1], strArray4))
      {
        index2 = 1;
        num1 = 2;
      }
    }
    if (strArray1.Length > 3)
    {
      if (this.ArrayContains(strArray1[2], strArray3))
      {
        index1 = 2;
        num1 = 3;
      }
      if (this.ArrayContains(strArray1[2], strArray4))
      {
        index2 = 2;
        num1 = 3;
      }
    }
    int length = Mathf.Max(strArray1.Length - num1, 0);
    int[] numArray = new int[length];
    for (int index3 = 0; index3 < length; ++index3)
    {
      if (string.IsNullOrEmpty(strArray1[index3 + num1]))
      {
        numArray[index3] = -1;
      }
      else
      {
        string CodedLanguage = strArray1[index3 + num1];
        bool flag = true;
        if (CodedLanguage.StartsWith("$", StringComparison.Ordinal))
        {
          flag = false;
          CodedLanguage = CodedLanguage.Substring(1);
        }
        string Language;
        string code;
        GoogleLanguages.UnPackCodeFromLanguageName(CodedLanguage, out Language, out code);
        int num2 = string.IsNullOrEmpty(code) ? this.GetLanguageIndex(Language, SkipDisabled: false) : this.GetLanguageIndexFromCode(code);
        if (num2 < 0)
        {
          this.mLanguages.Add(new LanguageData()
          {
            Name = Language,
            Code = code,
            Flags = (byte) (0 | (flag ? 0 : 1))
          });
          num2 = this.mLanguages.Count - 1;
        }
        numArray[index3] = num2;
      }
    }
    int count1 = this.mLanguages.Count;
    int index4 = 0;
    for (int count2 = this.mTerms.Count; index4 < count2; ++index4)
    {
      TermData mTerm = this.mTerms[index4];
      if (mTerm.Languages.Length < count1)
      {
        Array.Resize<string>(ref mTerm.Languages, count1);
        Array.Resize<byte>(ref mTerm.Flags, count1);
      }
    }
    int index5 = 1;
    for (int count3 = CSV.Count; index5 < count3; ++index5)
    {
      string[] strArray5 = CSV[index5];
      string Term = string.IsNullOrEmpty(Category) ? strArray5[0] : $"{Category}/{strArray5[0]}";
      string specialization = (string) null;
      if (Term.EndsWith("]", StringComparison.Ordinal))
      {
        int startIndex = Term.LastIndexOf('[');
        if (startIndex > 0)
        {
          specialization = Term.Substring(startIndex + 1, Term.Length - startIndex - 2);
          if (specialization == "touch")
            specialization = "Touch";
          Term = Term.Remove(startIndex);
        }
      }
      LanguageSourceData.ValidateFullTerm(ref Term);
      if (!string.IsNullOrEmpty(Term))
      {
        TermData termData = this.GetTermData(Term);
        if (termData == null)
        {
          termData = new TermData();
          termData.Term = Term;
          termData.Languages = new string[this.mLanguages.Count];
          termData.Flags = new byte[this.mLanguages.Count];
          for (int index6 = 0; index6 < this.mLanguages.Count; ++index6)
            termData.Languages[index6] = string.Empty;
          this.mTerms.Add(termData);
          this.mDictionary.Add(Term, termData);
        }
        else if (UpdateMode == eSpreadsheetUpdateMode.AddNewTerms)
          continue;
        if (index1 > 0)
          termData.TermType = LanguageSourceData.GetTermType(strArray5[index1]);
        if (index2 > 0)
          termData.Description = strArray5[index2];
        for (int index7 = 0; index7 < numArray.Length && index7 < strArray5.Length - num1; ++index7)
        {
          if (!string.IsNullOrEmpty(strArray5[index7 + num1]))
          {
            int idx = numArray[index7];
            if (idx >= 0)
            {
              string translation = strArray5[index7 + num1];
              switch (translation)
              {
                case "-":
                  translation = string.Empty;
                  break;
                case "":
                  translation = (string) null;
                  break;
              }
              termData.SetTranslation(idx, translation, specialization);
            }
          }
        }
      }
    }
    if (Application.isPlaying)
      this.SaveLanguages(this.HasUnloadedLanguages());
    return string.Empty;
  }

  public bool ArrayContains(string MainText, params string[] texts)
  {
    int index = 0;
    for (int length = texts.Length; index < length; ++index)
    {
      if (MainText.IndexOf(texts[index], StringComparison.OrdinalIgnoreCase) >= 0)
        return true;
    }
    return false;
  }

  public static eTermType GetTermType(string type)
  {
    int termType = 0;
    for (int index = 10; termType <= index; ++termType)
    {
      if (string.Equals(((eTermType) termType).ToString(), type, StringComparison.OrdinalIgnoreCase))
        return (eTermType) termType;
    }
    return eTermType.Text;
  }

  public void Import_Language_from_Cache(
    int langIndex,
    string langData,
    bool useFallback,
    bool onlyCurrentSpecialization)
  {
    int num1;
    for (int startIndex1 = 0; startIndex1 < langData.Length; startIndex1 = num1 + 5)
    {
      num1 = langData.IndexOf("[i2t]", startIndex1, StringComparison.Ordinal);
      if (num1 < 0)
        num1 = langData.Length;
      int num2 = langData.IndexOf("=", startIndex1, StringComparison.Ordinal);
      if (num2 >= num1)
        break;
      string term = langData.Substring(startIndex1, num2 - startIndex1);
      int startIndex2 = num2 + 1;
      TermData termData = this.GetTermData(term);
      if (termData != null)
      {
        string text = (string) null;
        if (startIndex2 != num1)
        {
          text = langData.Substring(startIndex2, num1 - startIndex2);
          if (text.StartsWith("[i2fb]", StringComparison.Ordinal))
            text = useFallback ? text.Substring(6) : (string) null;
          if (onlyCurrentSpecialization && text != null)
            text = SpecializationManager.GetSpecializedText(text);
        }
        termData.Languages[langIndex] = text;
      }
    }
  }

  public static void FreeUnusedLanguages()
  {
    LanguageSourceData source = LocalizationManager.Sources[0];
    int languageIndex = source.GetLanguageIndex(LocalizationManager.CurrentLanguage);
    for (int index1 = 0; index1 < source.mTerms.Count; ++index1)
    {
      TermData mTerm = source.mTerms[index1];
      for (int index2 = 0; index2 < mTerm.Languages.Length; ++index2)
      {
        if (index2 != languageIndex)
          mTerm.Languages[index2] = (string) null;
      }
    }
  }

  public void Import_Google_FromCache()
  {
    if (this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never || !I2Utils.IsPlaying())
      return;
    string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
    string JsonString = PersistentStorage.LoadFile(PersistentStorage.eFileType.Persistent, $"I2Source_{sourcePlayerPrefName}.loc", false);
    if (string.IsNullOrEmpty(JsonString))
      return;
    if (JsonString.StartsWith("[i2e]", StringComparison.Ordinal))
      JsonString = StringObfucator.Decode(JsonString.Substring(5, JsonString.Length - 5));
    bool flag = false;
    string newVersion = this.Google_LastUpdatedVersion;
    if (PersistentStorage.HasSetting("I2SourceVersion_" + sourcePlayerPrefName))
    {
      newVersion = PersistentStorage.GetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, this.Google_LastUpdatedVersion);
      flag = this.IsNewerVersion(this.Google_LastUpdatedVersion, newVersion);
    }
    if (!flag)
    {
      PersistentStorage.DeleteFile(PersistentStorage.eFileType.Persistent, $"I2Source_{sourcePlayerPrefName}.loc", false);
      PersistentStorage.DeleteSetting("I2SourceVersion_" + sourcePlayerPrefName);
    }
    else
    {
      if (newVersion.Length > 19)
        newVersion = string.Empty;
      this.Google_LastUpdatedVersion = newVersion;
      this.Import_Google_Result(JsonString, eSpreadsheetUpdateMode.Replace);
    }
  }

  public bool IsNewerVersion(string currentVersion, string newVersion)
  {
    if (string.IsNullOrEmpty(newVersion))
      return false;
    long result1;
    long result2;
    return string.IsNullOrEmpty(currentVersion) || !long.TryParse(newVersion, out result1) || !long.TryParse(currentVersion, out result2) || result1 > result2;
  }

  public void Import_Google(bool ForceUpdate, bool justCheck)
  {
    if (!ForceUpdate && this.GoogleUpdateFrequency == LanguageSourceData.eGoogleUpdateFrequency.Never || !I2Utils.IsPlaying())
      return;
    LanguageSourceData.eGoogleUpdateFrequency googleUpdateFrequency = this.GoogleUpdateFrequency;
    string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
    if (!ForceUpdate && googleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Always)
    {
      string settingString = PersistentStorage.GetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, "");
      try
      {
        DateTime result;
        if (DateTime.TryParse(settingString, out result))
        {
          double totalDays = (DateTime.Now - result).TotalDays;
          switch (googleUpdateFrequency)
          {
            case LanguageSourceData.eGoogleUpdateFrequency.Daily:
              if (totalDays < 1.0)
                return;
              break;
            case LanguageSourceData.eGoogleUpdateFrequency.Weekly:
              if (totalDays < 8.0)
                return;
              break;
            case LanguageSourceData.eGoogleUpdateFrequency.Monthly:
              if (totalDays < 31.0)
                return;
              break;
            case LanguageSourceData.eGoogleUpdateFrequency.OnlyOnce:
              return;
            case LanguageSourceData.eGoogleUpdateFrequency.EveryOtherDay:
              if (totalDays < 2.0)
                return;
              break;
          }
        }
      }
      catch (Exception ex)
      {
      }
    }
    PersistentStorage.SetSetting_String("LastGoogleUpdate_" + sourcePlayerPrefName, DateTime.Now.ToString());
    CoroutineManager.Start(this.Import_Google_Coroutine(justCheck));
  }

  public string GetSourcePlayerPrefName()
  {
    if (this.owner == null)
      return (string) null;
    string name = (this.owner as UnityEngine.Object).name;
    if (!string.IsNullOrEmpty(this.Google_SpreadsheetKey))
      name += this.Google_SpreadsheetKey;
    return Array.IndexOf<string>(LocalizationManager.GlobalSources, (this.owner as UnityEngine.Object).name) >= 0 ? name : $"{SceneManager.GetActiveScene().name}_{name}";
  }

  public IEnumerator Import_Google_Coroutine(bool JustCheck)
  {
    LanguageSourceData source = this;
    UnityWebRequest www = source.Import_Google_CreateWWWcall(false, JustCheck);
    if (www != null)
    {
      while (!www.isDone)
        yield return (object) null;
      if ((string.IsNullOrEmpty(www.error) ? 1 : 0) != 0)
      {
        byte[] data = www.downloadHandler.data;
        string str = Encoding.UTF8.GetString(data, 0, data.Length);
        bool flag = string.IsNullOrEmpty(str) || str == "\"\"";
        if (JustCheck)
        {
          if (flag)
            yield break;
          Debug.LogWarning((object) "Spreadsheet is not up-to-date and Google Live Synchronization is enabled\nWhen playing in the device the Spreadsheet will be downloaded and translations may not behave as what you see in the editor.\nTo fix this, Import or Export replace to Google");
          source.GoogleLiveSyncIsUptoDate = false;
          yield break;
        }
        if (!flag)
        {
          source.mDelayedGoogleData = str;
          switch (source.GoogleUpdateSynchronization)
          {
            case LanguageSourceData.eGoogleUpdateSynchronization.Manual:
              yield break;
            case LanguageSourceData.eGoogleUpdateSynchronization.OnSceneLoaded:
              SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(source.ApplyDownloadedDataOnSceneLoaded);
              yield break;
            case LanguageSourceData.eGoogleUpdateSynchronization.AsSoonAsDownloaded:
              source.ApplyDownloadedDataFromGoogle();
              yield break;
            default:
              yield break;
          }
        }
      }
      if (source.Event_OnSourceUpdateFromGoogle != null)
        source.Event_OnSourceUpdateFromGoogle(source, false, www.error);
      Debug.Log((object) "Language Source was up-to-date with Google Spreadsheet");
    }
  }

  public void ApplyDownloadedDataOnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.ApplyDownloadedDataOnSceneLoaded);
    this.ApplyDownloadedDataFromGoogle();
  }

  public void ApplyDownloadedDataFromGoogle()
  {
    if (string.IsNullOrEmpty(this.mDelayedGoogleData))
      return;
    if (string.IsNullOrEmpty(this.Import_Google_Result(this.mDelayedGoogleData, eSpreadsheetUpdateMode.Replace, true)))
    {
      if (this.Event_OnSourceUpdateFromGoogle != null)
        this.Event_OnSourceUpdateFromGoogle(this, true, "");
      LocalizationManager.LocalizeAll(true);
      Debug.Log((object) "Done Google Sync");
    }
    else
    {
      if (this.Event_OnSourceUpdateFromGoogle != null)
        this.Event_OnSourceUpdateFromGoogle(this, false, "");
      Debug.Log((object) "Done Google Sync: source was up-to-date");
    }
  }

  public UnityWebRequest Import_Google_CreateWWWcall(bool ForceUpdate, bool justCheck)
  {
    if (!this.HasGoogleSpreadsheet())
      return (UnityWebRequest) null;
    string currentVersion = PersistentStorage.GetSetting_String("I2SourceVersion_" + this.GetSourcePlayerPrefName(), this.Google_LastUpdatedVersion);
    if (currentVersion.Length > 19)
      currentVersion = string.Empty;
    if (this.IsNewerVersion(currentVersion, this.Google_LastUpdatedVersion))
      this.Google_LastUpdatedVersion = currentVersion;
    UnityWebRequest www = UnityWebRequest.Get($"{LocalizationManager.GetWebServiceURL(this)}?key={this.Google_SpreadsheetKey}&action=GetLanguageSource&version={(ForceUpdate ? (object) "0" : (object) this.Google_LastUpdatedVersion)}");
    I2Utils.SendWebRequest(www);
    return www;
  }

  public bool HasGoogleSpreadsheet()
  {
    return !string.IsNullOrEmpty(this.Google_WebServiceURL) && !string.IsNullOrEmpty(this.Google_SpreadsheetKey) && !string.IsNullOrEmpty(LocalizationManager.GetWebServiceURL(this));
  }

  public string Import_Google_Result(
    string JsonString,
    eSpreadsheetUpdateMode UpdateMode,
    bool saveInPlayerPrefs = false)
  {
    try
    {
      string empty = string.Empty;
      if (string.IsNullOrEmpty(JsonString) || JsonString == "\"\"")
        return empty;
      int num1 = JsonString.IndexOf("version=", StringComparison.Ordinal);
      int num2 = JsonString.IndexOf("script_version=", StringComparison.Ordinal);
      if (num1 < 0 || num2 < 0)
        return "Invalid Response from Google, Most likely the WebService needs to be updated";
      int startIndex1 = num1 + "version=".Length;
      int startIndex2 = num2 + "script_version=".Length;
      string newVersion = JsonString.Substring(startIndex1, JsonString.IndexOf(",", startIndex1, StringComparison.Ordinal) - startIndex1);
      int num3 = int.Parse(JsonString.Substring(startIndex2, JsonString.IndexOf(",", startIndex2, StringComparison.Ordinal) - startIndex2));
      if (newVersion.Length > 19)
        newVersion = string.Empty;
      int webServiceVersion = LocalizationManager.GetRequiredWebServiceVersion();
      if (num3 != webServiceVersion)
        return "The current Google WebService is not supported.\nPlease, delete the WebService from the Google Drive and Install the latest version.";
      if (saveInPlayerPrefs && !this.IsNewerVersion(this.Google_LastUpdatedVersion, newVersion))
        return "LanguageSource is up-to-date";
      if (saveInPlayerPrefs)
      {
        string sourcePlayerPrefName = this.GetSourcePlayerPrefName();
        PersistentStorage.SaveFile(PersistentStorage.eFileType.Persistent, $"I2Source_{sourcePlayerPrefName}.loc", "[i2e]" + StringObfucator.Encode(JsonString));
        PersistentStorage.SetSetting_String("I2SourceVersion_" + sourcePlayerPrefName, newVersion);
        PersistentStorage.ForceSaveSettings();
      }
      this.Google_LastUpdatedVersion = newVersion;
      if (UpdateMode == eSpreadsheetUpdateMode.Replace)
        this.ClearAllData();
      int num4 = JsonString.IndexOf("[i2category]", StringComparison.Ordinal);
      while (num4 > 0)
      {
        int startIndex3 = num4 + "[i2category]".Length;
        int num5 = JsonString.IndexOf("[/i2category]", startIndex3, StringComparison.Ordinal);
        string Category = JsonString.Substring(startIndex3, num5 - startIndex3);
        int startIndex4 = num5 + "[/i2category]".Length;
        int startIndex5 = JsonString.IndexOf("[/i2csv]", startIndex4, StringComparison.Ordinal);
        string I2CSVstring = JsonString.Substring(startIndex4, startIndex5 - startIndex4);
        num4 = JsonString.IndexOf("[i2category]", startIndex5, StringComparison.Ordinal);
        this.Import_I2CSV(Category, I2CSVstring, UpdateMode);
        if (UpdateMode == eSpreadsheetUpdateMode.Replace)
          UpdateMode = eSpreadsheetUpdateMode.Merge;
      }
      this.GoogleLiveSyncIsUptoDate = true;
      if (I2Utils.IsPlaying())
        this.SaveLanguages(true);
      if (!string.IsNullOrEmpty(empty))
        this.Editor_SetDirty();
      return empty;
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ex);
      return ex.ToString();
    }
  }

  public int GetLanguageIndex(string language, bool AllowDiscartingRegion = true, bool SkipDisabled = true)
  {
    int index1 = 0;
    for (int count = this.mLanguages.Count; index1 < count; ++index1)
    {
      if ((!SkipDisabled || this.mLanguages[index1].IsEnabled()) && string.Compare(this.mLanguages[index1].Name, language, StringComparison.OrdinalIgnoreCase) == 0)
        return index1;
    }
    if (AllowDiscartingRegion)
    {
      int languageIndex = -1;
      int num = 0;
      int index2 = 0;
      for (int count = this.mLanguages.Count; index2 < count; ++index2)
      {
        if (!SkipDisabled || this.mLanguages[index2].IsEnabled())
        {
          int wordInLanguageNames = LanguageSourceData.GetCommonWordInLanguageNames(this.mLanguages[index2].Name, language);
          if (wordInLanguageNames > num)
          {
            num = wordInLanguageNames;
            languageIndex = index2;
          }
        }
      }
      if (languageIndex >= 0)
        return languageIndex;
    }
    return -1;
  }

  public LanguageData GetLanguageData(string language, bool AllowDiscartingRegion = true)
  {
    int languageIndex = this.GetLanguageIndex(language, AllowDiscartingRegion, false);
    return languageIndex >= 0 ? this.mLanguages[languageIndex] : (LanguageData) null;
  }

  public bool IsCurrentLanguage(int languageIndex)
  {
    return LocalizationManager.CurrentLanguage == this.mLanguages[languageIndex].Name;
  }

  public int GetLanguageIndexFromCode(string Code, bool exactMatch = true, bool ignoreDisabled = false)
  {
    int index1 = 0;
    for (int count = this.mLanguages.Count; index1 < count; ++index1)
    {
      if ((!ignoreDisabled || this.mLanguages[index1].IsEnabled()) && string.Compare(this.mLanguages[index1].Code, Code, StringComparison.OrdinalIgnoreCase) == 0)
        return index1;
    }
    if (!exactMatch)
    {
      int index2 = 0;
      for (int count = this.mLanguages.Count; index2 < count; ++index2)
      {
        if ((!ignoreDisabled || this.mLanguages[index2].IsEnabled()) && string.Compare(this.mLanguages[index2].Code, 0, Code, 0, 2, StringComparison.OrdinalIgnoreCase) == 0)
          return index2;
      }
    }
    return -1;
  }

  public static int GetCommonWordInLanguageNames(string Language1, string Language2)
  {
    if (string.IsNullOrEmpty(Language1) || string.IsNullOrEmpty(Language2))
      return 0;
    char[] charArray = "( )-/\\".ToCharArray();
    string[] array1 = Language1.ToLower().Split(charArray);
    string[] array2 = Language2.ToLower().Split(charArray);
    int wordInLanguageNames = 0;
    foreach (string str in array1)
    {
      if (!string.IsNullOrEmpty(str) && array2.Contains<string>(str))
        ++wordInLanguageNames;
    }
    foreach (string str in array2)
    {
      if (!string.IsNullOrEmpty(str) && array1.Contains<string>(str))
        ++wordInLanguageNames;
    }
    return wordInLanguageNames;
  }

  public static bool AreTheSameLanguage(string Language1, string Language2)
  {
    Language1 = LanguageSourceData.GetLanguageWithoutRegion(Language1);
    Language2 = LanguageSourceData.GetLanguageWithoutRegion(Language2);
    return string.Compare(Language1, Language2, StringComparison.OrdinalIgnoreCase) == 0;
  }

  public static string GetLanguageWithoutRegion(string Language)
  {
    int length = Language.IndexOfAny("(/\\[,{".ToCharArray());
    return length < 0 ? Language : Language.Substring(0, length).Trim();
  }

  public void AddLanguage(string LanguageName)
  {
    this.AddLanguage(LanguageName, GoogleLanguages.GetLanguageCode(LanguageName));
  }

  public void AddLanguage(string LanguageName, string LanguageCode)
  {
    if (this.GetLanguageIndex(LanguageName, false) >= 0)
      return;
    this.mLanguages.Add(new LanguageData()
    {
      Name = LanguageName,
      Code = LanguageCode
    });
    int count1 = this.mLanguages.Count;
    int index = 0;
    for (int count2 = this.mTerms.Count; index < count2; ++index)
    {
      Array.Resize<string>(ref this.mTerms[index].Languages, count1);
      Array.Resize<byte>(ref this.mTerms[index].Flags, count1);
    }
    this.Editor_SetDirty();
  }

  public void RemoveLanguage(string LanguageName)
  {
    int languageIndex = this.GetLanguageIndex(LanguageName, false, false);
    if (languageIndex < 0)
      return;
    int count1 = this.mLanguages.Count;
    int index1 = 0;
    for (int count2 = this.mTerms.Count; index1 < count2; ++index1)
    {
      for (int index2 = languageIndex + 1; index2 < count1; ++index2)
      {
        this.mTerms[index1].Languages[index2 - 1] = this.mTerms[index1].Languages[index2];
        this.mTerms[index1].Flags[index2 - 1] = this.mTerms[index1].Flags[index2];
      }
      Array.Resize<string>(ref this.mTerms[index1].Languages, count1 - 1);
      Array.Resize<byte>(ref this.mTerms[index1].Flags, count1 - 1);
    }
    this.mLanguages.RemoveAt(languageIndex);
    this.Editor_SetDirty();
  }

  public List<string> GetLanguages(bool skipDisabled = true)
  {
    List<string> languages = new List<string>();
    int index = 0;
    for (int count = this.mLanguages.Count; index < count; ++index)
    {
      if (!skipDisabled || this.mLanguages[index].IsEnabled())
        languages.Add(this.mLanguages[index].Name);
    }
    return languages;
  }

  public List<string> GetLanguagesCode(bool allowRegions = true, bool skipDisabled = true)
  {
    List<string> languagesCode = new List<string>();
    int index = 0;
    for (int count = this.mLanguages.Count; index < count; ++index)
    {
      if (!skipDisabled || this.mLanguages[index].IsEnabled())
      {
        string str = this.mLanguages[index].Code;
        if (!allowRegions && str != null && str.Length > 2)
          str = str.Substring(0, 2);
        if (!string.IsNullOrEmpty(str) && !languagesCode.Contains(str))
          languagesCode.Add(str);
      }
    }
    return languagesCode;
  }

  public bool IsLanguageEnabled(string Language)
  {
    int languageIndex = this.GetLanguageIndex(Language, false);
    return languageIndex >= 0 && this.mLanguages[languageIndex].IsEnabled();
  }

  public void EnableLanguage(string Language, bool bEnabled)
  {
    int languageIndex = this.GetLanguageIndex(Language, false, false);
    if (languageIndex < 0)
      return;
    this.mLanguages[languageIndex].SetEnabled(bEnabled);
  }

  public bool AllowUnloadingLanguages() => this._AllowUnloadingLanguages != 0;

  public string GetSavedLanguageFileName(int languageIndex)
  {
    if (languageIndex < 0)
      return (string) null;
    return $"LangSource_{this.GetSourcePlayerPrefName()}_{this.mLanguages[languageIndex].Name}.loc";
  }

  public void LoadLanguage(
    int languageIndex,
    bool UnloadOtherLanguages,
    bool useFallback,
    bool onlyCurrentSpecialization,
    bool forceLoad)
  {
    if (!this.AllowUnloadingLanguages() || !PersistentStorage.CanAccessFiles())
      return;
    if (languageIndex >= 0 && (forceLoad || !this.mLanguages[languageIndex].IsLoaded()))
    {
      string langData = PersistentStorage.LoadFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(languageIndex), false);
      if (!string.IsNullOrEmpty(langData))
      {
        this.Import_Language_from_Cache(languageIndex, langData, useFallback, onlyCurrentSpecialization);
        this.mLanguages[languageIndex].SetLoaded(true);
      }
    }
    if (!UnloadOtherLanguages || !I2Utils.IsPlaying())
      return;
    for (int languageIndex1 = 0; languageIndex1 < this.mLanguages.Count; ++languageIndex1)
    {
      if (languageIndex1 != languageIndex)
        this.UnloadLanguage(languageIndex1);
    }
  }

  public void LoadAllLanguages(bool forceLoad = false)
  {
    for (int languageIndex = 0; languageIndex < this.mLanguages.Count; ++languageIndex)
      this.LoadLanguage(languageIndex, false, false, false, forceLoad);
  }

  public void UnloadLanguage(int languageIndex)
  {
    if (!this.AllowUnloadingLanguages() || !PersistentStorage.CanAccessFiles() || !I2Utils.IsPlaying() || !this.mLanguages[languageIndex].IsLoaded() || !this.mLanguages[languageIndex].CanBeUnloaded() || this.IsCurrentLanguage(languageIndex) || !PersistentStorage.HasFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(languageIndex)))
      return;
    foreach (TermData mTerm in this.mTerms)
      mTerm.Languages[languageIndex] = (string) null;
    this.mLanguages[languageIndex].SetLoaded(false);
  }

  public void SaveLanguages(bool unloadAll, PersistentStorage.eFileType fileLocation = PersistentStorage.eFileType.Temporal)
  {
    if (!this.AllowUnloadingLanguages() || !PersistentStorage.CanAccessFiles())
      return;
    for (int index = 0; index < this.mLanguages.Count; ++index)
    {
      string cache = this.Export_Language_to_Cache(index, this.IsCurrentLanguage(index));
      if (!string.IsNullOrEmpty(cache))
        PersistentStorage.SaveFile(PersistentStorage.eFileType.Temporal, this.GetSavedLanguageFileName(index), cache);
    }
    if (!unloadAll)
      return;
    for (int languageIndex = 0; languageIndex < this.mLanguages.Count; ++languageIndex)
    {
      if (unloadAll && !this.IsCurrentLanguage(languageIndex))
        this.UnloadLanguage(languageIndex);
    }
  }

  public bool HasUnloadedLanguages()
  {
    for (int index = 0; index < this.mLanguages.Count; ++index)
    {
      if (!this.mLanguages[index].IsLoaded())
        return true;
    }
    return false;
  }

  public List<string> GetCategories(bool OnlyMainCategory = false, List<string> Categories = null)
  {
    if (Categories == null)
      Categories = new List<string>();
    foreach (TermData mTerm in this.mTerms)
    {
      string categoryFromFullTerm = LanguageSourceData.GetCategoryFromFullTerm(mTerm.Term, OnlyMainCategory);
      if (!Categories.Contains(categoryFromFullTerm))
        Categories.Add(categoryFromFullTerm);
    }
    Categories.Sort();
    return Categories;
  }

  public static string GetKeyFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
  {
    int num = OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators);
    return num >= 0 ? FullTerm.Substring(num + 1) : FullTerm;
  }

  public static string GetCategoryFromFullTerm(string FullTerm, bool OnlyMainCategory = false)
  {
    int length = OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators);
    return length >= 0 ? FullTerm.Substring(0, length) : LanguageSourceData.EmptyCategory;
  }

  public static void DeserializeFullTerm(
    string FullTerm,
    out string Key,
    out string Category,
    bool OnlyMainCategory = false)
  {
    int length = OnlyMainCategory ? FullTerm.IndexOfAny(LanguageSourceData.CategorySeparators) : FullTerm.LastIndexOfAny(LanguageSourceData.CategorySeparators);
    if (length < 0)
    {
      Category = LanguageSourceData.EmptyCategory;
      Key = FullTerm;
    }
    else
    {
      Category = FullTerm.Substring(0, length);
      Key = FullTerm.Substring(length + 1);
    }
  }

  public void UpdateDictionary(bool force = false)
  {
    if (!force && this.mDictionary != null && this.mDictionary.Count == this.mTerms.Count)
      return;
    StringComparer comparer = this.CaseInsensitiveTerms ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
    if (this.mDictionary.Comparer != comparer)
      this.mDictionary = new Dictionary<string, TermData>((IEqualityComparer<string>) comparer);
    else
      this.mDictionary.Clear();
    int index = 0;
    for (int count = this.mTerms.Count; index < count; ++index)
    {
      TermData mTerm = this.mTerms[index];
      LanguageSourceData.ValidateFullTerm(ref mTerm.Term);
      this.mDictionary[mTerm.Term] = this.mTerms[index];
      this.mTerms[index].Validate();
    }
    if (!I2Utils.IsPlaying())
      return;
    this.SaveLanguages(true);
  }

  public string GetTranslation(
    string term,
    string overrideLanguage = null,
    string overrideSpecialization = null,
    bool skipDisabled = false,
    bool allowCategoryMistmatch = false)
  {
    string Translation;
    return this.TryGetTranslation(term, out Translation, overrideLanguage, overrideSpecialization, skipDisabled, allowCategoryMistmatch) ? Translation : string.Empty;
  }

  public bool TryGetTranslation(
    string term,
    out string Translation,
    string overrideLanguage = null,
    string overrideSpecialization = null,
    bool skipDisabled = false,
    bool allowCategoryMistmatch = false)
  {
    int languageIndex = this.GetLanguageIndex(overrideLanguage == null ? LocalizationManager.CurrentLanguage : overrideLanguage, SkipDisabled: false);
    if (languageIndex >= 0 && (!skipDisabled || this.mLanguages[languageIndex].IsEnabled()))
    {
      TermData termData = this.GetTermData(term, allowCategoryMistmatch);
      if (termData != null)
      {
        Translation = termData.GetTranslation(languageIndex, overrideSpecialization, true);
        if (Translation == "---")
        {
          Translation = string.Empty;
          return true;
        }
        if (!string.IsNullOrEmpty(Translation))
          return true;
        Translation = (string) null;
      }
      if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowWarning)
      {
        Translation = $"<!-Missing Translation [{term}]-!>";
        return true;
      }
      if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Fallback && termData != null)
        return this.TryGetFallbackTranslation(termData, out Translation, languageIndex, overrideSpecialization, skipDisabled);
      if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.Empty)
      {
        Translation = string.Empty;
        return true;
      }
      if (this.OnMissingTranslation == LanguageSourceData.MissingTranslationAction.ShowTerm)
      {
        Translation = term;
        return true;
      }
    }
    Translation = (string) null;
    return false;
  }

  public bool TryGetFallbackTranslation(
    TermData termData,
    out string Translation,
    int langIndex,
    string overrideSpecialization = null,
    bool skipDisabled = false)
  {
    string str = this.mLanguages[langIndex].Code;
    if (!string.IsNullOrEmpty(str))
    {
      if (str.Contains('-'))
        str = str.Substring(0, str.IndexOf('-'));
      for (int index = 0; index < this.mLanguages.Count; ++index)
      {
        if (index != langIndex && this.mLanguages[index].Code.StartsWith(str, StringComparison.Ordinal) && (!skipDisabled || this.mLanguages[index].IsEnabled()))
        {
          Translation = termData.GetTranslation(index, overrideSpecialization, true);
          if (!string.IsNullOrEmpty(Translation))
            return true;
        }
      }
    }
    for (int index = 0; index < this.mLanguages.Count; ++index)
    {
      if (index != langIndex && (!skipDisabled || this.mLanguages[index].IsEnabled()) && (str == null || !this.mLanguages[index].Code.StartsWith(str, StringComparison.Ordinal)))
      {
        Translation = termData.GetTranslation(index, overrideSpecialization, true);
        if (!string.IsNullOrEmpty(Translation))
          return true;
      }
    }
    Translation = (string) null;
    return false;
  }

  public TermData AddTerm(string term) => this.AddTerm(term, eTermType.Text);

  public TermData GetTermData(string term, bool allowCategoryMistmatch = false)
  {
    if (string.IsNullOrEmpty(term))
      return (TermData) null;
    if (this.mDictionary.Count == 0)
      this.UpdateDictionary();
    TermData termData1;
    if (this.mDictionary.TryGetValue(term, out termData1))
      return termData1;
    TermData termData2 = (TermData) null;
    if (allowCategoryMistmatch)
    {
      string keyFromFullTerm = LanguageSourceData.GetKeyFromFullTerm(term);
      foreach (KeyValuePair<string, TermData> m in this.mDictionary)
      {
        if (m.Value.IsTerm(keyFromFullTerm, true))
        {
          if (termData2 != null)
            return (TermData) null;
          termData2 = m.Value;
        }
      }
    }
    return termData2;
  }

  public bool ContainsTerm(string term) => this.GetTermData(term) != null;

  public List<string> GetTermsList(string Category = null)
  {
    if (this.mDictionary.Count != this.mTerms.Count)
      this.UpdateDictionary();
    if (string.IsNullOrEmpty(Category))
      return new List<string>((IEnumerable<string>) this.mDictionary.Keys);
    List<string> termsList = new List<string>();
    for (int index = 0; index < this.mTerms.Count; ++index)
    {
      TermData mTerm = this.mTerms[index];
      if (LanguageSourceData.GetCategoryFromFullTerm(mTerm.Term) == Category)
        termsList.Add(mTerm.Term);
    }
    return termsList;
  }

  public TermData AddTerm(string NewTerm, eTermType termType, bool SaveSource = true)
  {
    LanguageSourceData.ValidateFullTerm(ref NewTerm);
    NewTerm = NewTerm.Trim();
    if (this.mLanguages.Count == 0)
      this.AddLanguage("English", "en");
    TermData termData = this.GetTermData(NewTerm);
    if (termData == null)
    {
      termData = new TermData();
      termData.Term = NewTerm;
      termData.TermType = termType;
      termData.Languages = new string[this.mLanguages.Count];
      termData.Flags = new byte[this.mLanguages.Count];
      this.mTerms.Add(termData);
      this.mDictionary.Add(NewTerm, termData);
    }
    return termData;
  }

  public void RemoveTerm(string term)
  {
    int index = 0;
    for (int count = this.mTerms.Count; index < count; ++index)
    {
      if (this.mTerms[index].Term == term)
      {
        this.mTerms.RemoveAt(index);
        this.mDictionary.Remove(term);
        break;
      }
    }
  }

  public static void ValidateFullTerm(ref string Term)
  {
    Term = Term.Replace('\\', '/');
    Term = Term.Trim();
    if (Term.StartsWith(LanguageSourceData.EmptyCategory, StringComparison.Ordinal) && Term.Length > LanguageSourceData.EmptyCategory.Length && Term[LanguageSourceData.EmptyCategory.Length] == '/')
      Term = Term.Substring(LanguageSourceData.EmptyCategory.Length + 1);
    Term = I2Utils.GetValidTermName(Term, true);
  }

  public enum MissingTranslationAction
  {
    Empty,
    Fallback,
    ShowWarning,
    ShowTerm,
  }

  public enum eAllowUnloadLanguages
  {
    Never,
    OnlyInDevice,
    EditorAndDevice,
  }

  public enum eGoogleUpdateFrequency
  {
    Always,
    Never,
    Daily,
    Weekly,
    Monthly,
    OnlyOnce,
    EveryOtherDay,
  }

  public enum eGoogleUpdateSynchronization
  {
    Manual,
    OnSceneLoaded,
    AsSoonAsDownloaded,
  }
}
