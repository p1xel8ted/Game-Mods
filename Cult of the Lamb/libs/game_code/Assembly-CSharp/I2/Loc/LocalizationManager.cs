// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizationManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using TMPro;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public static class LocalizationManager
{
  public static bool _initialized;
  public static string mCurrentLanguage;
  public static string mLanguageCode;
  public static CultureInfo mCurrentCulture;
  public static bool mChangeCultureInfo;
  public static bool IsRight2Left;
  public static bool HasJoinedWords;
  public static List<ILocalizationParamsManager> ParamManagers = new List<ILocalizationParamsManager>();
  public static LocalizationManager.FnCustomApplyLocalizationParams CustomApplyLocalizationParams;
  public static string[] LanguagesRTL = new string[21]
  {
    "ar-DZ",
    "ar",
    "ar-BH",
    "ar-EG",
    "ar-IQ",
    "ar-JO",
    "ar-KW",
    "ar-LB",
    "ar-LY",
    "ar-MA",
    "ar-OM",
    "ar-QA",
    "ar-SA",
    "ar-SY",
    "ar-TN",
    "ar-AE",
    "ar-YE",
    "fa",
    "he",
    "ur",
    "ji"
  };
  public static List<LanguageSourceData> Sources = new List<LanguageSourceData>();
  public static string[] GlobalSources = new string[1]
  {
    "I2Languages"
  };
  public static Func<LanguageSourceData, bool> Callback_AllowSyncFromGoogle = (Func<LanguageSourceData, bool>) null;
  public static string mCurrentDeviceLanguage;
  public static List<ILocalizeTargetDescriptor> mLocalizeTargets = new List<ILocalizeTargetDescriptor>();
  public const string DYSLEXIC_FONT_PATH = "Fonts/Dyslexic/OpenDyslexic-Regular SDF";
  public const string LAPTURE_REGURAL = "Fonts/Headings/LaptureRegular";
  [CompilerGenerated]
  public static TMP_FontAsset \u003CDyslexicFontAsset\u003Ek__BackingField;
  public static bool mLocalizeIsScheduled;
  public static bool mLocalizeIsScheduledWithForcedValue;
  public static bool HighlightLocalizedTargets = false;
  public static List<string> previouslyUsedFontPaths = new List<string>();
  public static List<TMP_FontAsset> previouslyUsedFonts = new List<TMP_FontAsset>();
  public static bool FontsLoadedonce = false;
  public static bool ForceLoadSynchronous = false;
  public static string lastLanguage = "";

  public static void InitializeIfNeeded()
  {
    if (!string.IsNullOrEmpty(LocalizationManager.mCurrentLanguage) && LocalizationManager.Sources.Count != 0)
      return;
    LocalizationManager.AutoLoadGlobalParamManagers();
    LocalizationManager.UpdateSources();
    LocalizationManager.SelectStartupLanguage();
  }

  public static string GetVersion() => "2.8.17 f1";

  public static int GetRequiredWebServiceVersion() => 5;

  public static string GetWebServiceURL(LanguageSourceData source = null)
  {
    if (source != null && !string.IsNullOrEmpty(source.Google_WebServiceURL))
      return source.Google_WebServiceURL;
    LocalizationManager.InitializeIfNeeded();
    for (int index = 0; index < LocalizationManager.Sources.Count; ++index)
    {
      if (LocalizationManager.Sources[index] != null && !string.IsNullOrEmpty(LocalizationManager.Sources[index].Google_WebServiceURL))
        return LocalizationManager.Sources[index].Google_WebServiceURL;
    }
    return string.Empty;
  }

  public static string CurrentLanguage
  {
    get
    {
      LocalizationManager.InitializeIfNeeded();
      return LocalizationManager.mCurrentLanguage;
    }
    set
    {
      LocalizationManager.InitializeIfNeeded();
      string supportedLanguage = LocalizationManager.GetSupportedLanguage(value);
      if (string.IsNullOrEmpty(supportedLanguage) || !(LocalizationManager.mCurrentLanguage != supportedLanguage))
        return;
      LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage));
    }
  }

  public static string CurrentLanguageCode
  {
    get
    {
      LocalizationManager.InitializeIfNeeded();
      return LocalizationManager.mLanguageCode;
    }
    set
    {
      LocalizationManager.InitializeIfNeeded();
      if (!(LocalizationManager.mLanguageCode != value))
        return;
      string languageFromCode = LocalizationManager.GetLanguageFromCode(value);
      if (string.IsNullOrEmpty(languageFromCode))
        return;
      LocalizationManager.SetLanguageAndCode(languageFromCode, value);
    }
  }

  public static string CurrentRegion
  {
    get
    {
      string currentLanguage = LocalizationManager.CurrentLanguage;
      int num1 = currentLanguage.IndexOfAny("/\\".ToCharArray());
      if (num1 > 0)
        return currentLanguage.Substring(num1 + 1);
      int num2 = currentLanguage.IndexOfAny("[(".ToCharArray());
      int num3 = currentLanguage.LastIndexOfAny("])".ToCharArray());
      return num2 > 0 && num2 != num3 ? currentLanguage.Substring(num2 + 1, num3 - num2 - 1) : string.Empty;
    }
    set
    {
      string str = LocalizationManager.CurrentLanguage;
      int num1 = str.IndexOfAny("/\\".ToCharArray());
      if (num1 > 0)
      {
        LocalizationManager.CurrentLanguage = str.Substring(num1 + 1) + value;
      }
      else
      {
        int startIndex = str.IndexOfAny("[(".ToCharArray());
        int num2 = str.LastIndexOfAny("])".ToCharArray());
        if (startIndex > 0 && startIndex != num2)
          str = str.Substring(startIndex);
        LocalizationManager.CurrentLanguage = $"{str}({value})";
      }
    }
  }

  public static string CurrentRegionCode
  {
    get
    {
      string currentLanguageCode = LocalizationManager.CurrentLanguageCode;
      int num = currentLanguageCode.IndexOfAny(" -_/\\".ToCharArray());
      return num >= 0 ? currentLanguageCode.Substring(num + 1) : string.Empty;
    }
    set
    {
      string str = LocalizationManager.CurrentLanguageCode;
      int length = str.IndexOfAny(" -_/\\".ToCharArray());
      if (length > 0)
        str = str.Substring(0, length);
      LocalizationManager.CurrentLanguageCode = $"{str}-{value}";
    }
  }

  public static CultureInfo CurrentCulture => LocalizationManager.mCurrentCulture;

  public static void SetLanguageAndCode(
    string LanguageName,
    string LanguageCode,
    bool RememberLanguage = true,
    bool Force = false)
  {
    if (((LocalizationManager.mCurrentLanguage != LanguageName ? 1 : (LocalizationManager.mLanguageCode != LanguageCode ? 1 : 0)) | (Force ? 1 : 0)) == 0)
      return;
    if (RememberLanguage)
      PersistentStorage.SetSetting_String("I2 Language", LanguageName);
    LocalizationManager.mCurrentLanguage = LanguageName;
    LocalizeIntegration.Arabic = LocalizationManager.mCurrentLanguage == "Arabic";
    LocalizationManager.mLanguageCode = LanguageCode;
    LocalizationManager.mCurrentCulture = LocalizationManager.CreateCultureForCode(LanguageCode);
    if (LocalizationManager.mChangeCultureInfo)
      LocalizationManager.SetCurrentCultureInfo();
    LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
    LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
    LocalizationManager.LocalizeAll(Force);
  }

  public static CultureInfo CreateCultureForCode(string code)
  {
    try
    {
      return CultureInfo.CreateSpecificCulture(code);
    }
    catch (Exception ex)
    {
      return CultureInfo.InvariantCulture;
    }
  }

  public static void EnableChangingCultureInfo(bool bEnable)
  {
    if (!LocalizationManager.mChangeCultureInfo & bEnable)
      LocalizationManager.SetCurrentCultureInfo();
    LocalizationManager.mChangeCultureInfo = bEnable;
  }

  public static void SetCurrentCultureInfo()
  {
    Thread.CurrentThread.CurrentCulture = LocalizationManager.mCurrentCulture;
  }

  public static void SelectStartupLanguage()
  {
    if (LocalizationManager.Sources.Count == 0)
      return;
    string settingString = PersistentStorage.GetSetting_String("I2 Language", string.Empty);
    string currentDeviceLanguage = LocalizationManager.GetCurrentDeviceLanguage();
    if (!string.IsNullOrEmpty(settingString) && LocalizationManager.HasLanguage(settingString, Initialize: false))
    {
      LocalizationManager.SetLanguageAndCode(settingString, LocalizationManager.GetLanguageCode(settingString));
    }
    else
    {
      if (!LocalizationManager.Sources[0].IgnoreDeviceLanguage)
      {
        string supportedLanguage = LocalizationManager.GetSupportedLanguage(currentDeviceLanguage, true);
        if (!string.IsNullOrEmpty(supportedLanguage))
        {
          LocalizationManager.SetLanguageAndCode(supportedLanguage, LocalizationManager.GetLanguageCode(supportedLanguage), false);
          return;
        }
      }
      int index1 = 0;
      for (int count = LocalizationManager.Sources.Count; index1 < count; ++index1)
      {
        if (LocalizationManager.Sources[index1].mLanguages.Count > 0)
        {
          for (int index2 = 0; index2 < LocalizationManager.Sources[index1].mLanguages.Count; ++index2)
          {
            if (LocalizationManager.Sources[index1].mLanguages[index2].IsEnabled())
            {
              LocalizationManager.SetLanguageAndCode(LocalizationManager.Sources[index1].mLanguages[index2].Name, LocalizationManager.Sources[index1].mLanguages[index2].Code, false);
              return;
            }
          }
        }
      }
    }
  }

  public static bool HasLanguage(
    string Language,
    bool AllowDiscartingRegion = true,
    bool Initialize = true,
    bool SkipDisabled = true)
  {
    if (Initialize)
      LocalizationManager.InitializeIfNeeded();
    int index1 = 0;
    for (int count = LocalizationManager.Sources.Count; index1 < count; ++index1)
    {
      if (LocalizationManager.Sources[index1].GetLanguageIndex(Language, false, SkipDisabled) >= 0)
        return true;
    }
    if (AllowDiscartingRegion)
    {
      int index2 = 0;
      for (int count = LocalizationManager.Sources.Count; index2 < count; ++index2)
      {
        if (LocalizationManager.Sources[index2].GetLanguageIndex(Language, SkipDisabled: SkipDisabled) >= 0)
          return true;
      }
    }
    return false;
  }

  public static string GetSupportedLanguage(string Language, bool ignoreDisabled = false)
  {
    string languageCode = GoogleLanguages.GetLanguageCode(Language);
    if (!string.IsNullOrEmpty(languageCode))
    {
      int index1 = 0;
      for (int count = LocalizationManager.Sources.Count; index1 < count; ++index1)
      {
        int languageIndexFromCode = LocalizationManager.Sources[index1].GetLanguageIndexFromCode(languageCode, ignoreDisabled: ignoreDisabled);
        if (languageIndexFromCode >= 0)
          return LocalizationManager.Sources[index1].mLanguages[languageIndexFromCode].Name;
      }
      int index2 = 0;
      for (int count = LocalizationManager.Sources.Count; index2 < count; ++index2)
      {
        int languageIndexFromCode = LocalizationManager.Sources[index2].GetLanguageIndexFromCode(languageCode, false, ignoreDisabled);
        if (languageIndexFromCode >= 0)
          return LocalizationManager.Sources[index2].mLanguages[languageIndexFromCode].Name;
      }
    }
    int index3 = 0;
    for (int count = LocalizationManager.Sources.Count; index3 < count; ++index3)
    {
      int languageIndex = LocalizationManager.Sources[index3].GetLanguageIndex(Language, false, ignoreDisabled);
      if (languageIndex >= 0)
        return LocalizationManager.Sources[index3].mLanguages[languageIndex].Name;
    }
    int index4 = 0;
    for (int count = LocalizationManager.Sources.Count; index4 < count; ++index4)
    {
      int languageIndex = LocalizationManager.Sources[index4].GetLanguageIndex(Language, SkipDisabled: ignoreDisabled);
      if (languageIndex >= 0)
        return LocalizationManager.Sources[index4].mLanguages[languageIndex].Name;
    }
    return string.Empty;
  }

  public static string GetLanguageCode(string Language)
  {
    if (LocalizationManager.Sources.Count == 0)
      LocalizationManager.UpdateSources();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      int languageIndex = LocalizationManager.Sources[index].GetLanguageIndex(Language);
      if (languageIndex >= 0)
        return LocalizationManager.Sources[index].mLanguages[languageIndex].Code;
    }
    return string.Empty;
  }

  public static string GetLanguageFromCode(string Code, bool exactMatch = true)
  {
    if (LocalizationManager.Sources.Count == 0)
      LocalizationManager.UpdateSources();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      int languageIndexFromCode = LocalizationManager.Sources[index].GetLanguageIndexFromCode(Code, exactMatch);
      if (languageIndexFromCode >= 0)
        return LocalizationManager.Sources[index].mLanguages[languageIndexFromCode].Name;
    }
    return string.Empty;
  }

  public static List<string> GetAllLanguages(bool SkipDisabled = true)
  {
    if (LocalizationManager.Sources.Count == 0)
      LocalizationManager.UpdateSources();
    List<string> Languages = new List<string>();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
      Languages.AddRange(LocalizationManager.Sources[index].GetLanguages(SkipDisabled).Where<string>((Func<string, bool>) (x => !Languages.Contains(x))));
    return Languages;
  }

  public static List<string> GetAllLanguagesCode(bool allowRegions = true, bool SkipDisabled = true)
  {
    List<string> Languages = new List<string>();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
      Languages.AddRange(LocalizationManager.Sources[index].GetLanguagesCode(allowRegions, SkipDisabled).Where<string>((Func<string, bool>) (x => !Languages.Contains(x))));
    return Languages;
  }

  public static bool IsLanguageEnabled(string Language)
  {
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      if (!LocalizationManager.Sources[index].IsLanguageEnabled(Language))
        return false;
    }
    return true;
  }

  public static void LoadCurrentLanguage()
  {
    for (int index = 0; index < LocalizationManager.Sources.Count; ++index)
    {
      int languageIndex = LocalizationManager.Sources[index].GetLanguageIndex(LocalizationManager.mCurrentLanguage, SkipDisabled: false);
      LocalizationManager.Sources[index].LoadLanguage(languageIndex, true, true, true, false);
    }
  }

  public static void PreviewLanguage(string NewLanguage)
  {
    LocalizationManager.mCurrentLanguage = NewLanguage;
    LocalizationManager.mLanguageCode = LocalizationManager.GetLanguageCode(LocalizationManager.mCurrentLanguage);
    LocalizationManager.IsRight2Left = LocalizationManager.IsRTL(LocalizationManager.mLanguageCode);
    LocalizationManager.HasJoinedWords = GoogleLanguages.LanguageCode_HasJoinedWord(LocalizationManager.mLanguageCode);
  }

  public static void AutoLoadGlobalParamManagers()
  {
    foreach (LocalizationParamsManager message in UnityEngine.Object.FindObjectsOfType<LocalizationParamsManager>())
    {
      if (message._IsGlobalManager && !LocalizationManager.ParamManagers.Contains((ILocalizationParamsManager) message))
      {
        Debug.Log((object) message);
        LocalizationManager.ParamManagers.Add((ILocalizationParamsManager) message);
      }
    }
  }

  public static void ApplyLocalizationParams(ref string translation, bool allowLocalizedParameters = true)
  {
    LocalizationManager.ApplyLocalizationParams(ref translation, (LocalizationManager._GetParam) (p => (object) LocalizationManager.GetLocalizationParam(p, (GameObject) null)), allowLocalizedParameters);
  }

  public static void ApplyLocalizationParams(
    ref string translation,
    GameObject root,
    bool allowLocalizedParameters = true)
  {
    LocalizationManager.ApplyLocalizationParams(ref translation, (LocalizationManager._GetParam) (p => (object) LocalizationManager.GetLocalizationParam(p, root)), allowLocalizedParameters);
  }

  public static void ApplyLocalizationParams(
    ref string translation,
    Dictionary<string, object> parameters,
    bool allowLocalizedParameters = true)
  {
    LocalizationManager.ApplyLocalizationParams(ref translation, (LocalizationManager._GetParam) (p =>
    {
      object obj = (object) null;
      return parameters.TryGetValue(p, out obj) ? obj : (object) null;
    }), allowLocalizedParameters);
  }

  public static void ApplyLocalizationParams(
    ref string translation,
    LocalizationManager._GetParam getParam,
    bool allowLocalizedParameters = true)
  {
    if (translation == null || (LocalizationManager.CustomApplyLocalizationParams == null ? 0 : (LocalizationManager.CustomApplyLocalizationParams(ref translation, getParam, allowLocalizedParameters) ? 1 : 0)) != 0)
      return;
    string str1 = (string) null;
    int length = translation.Length;
    int startIndex1 = 0;
    while (startIndex1 >= 0 && startIndex1 < translation.Length)
    {
      int startIndex2 = translation.IndexOf("{[", startIndex1, StringComparison.Ordinal);
      if (startIndex2 >= 0)
      {
        int num1 = translation.IndexOf("]}", startIndex2, StringComparison.Ordinal);
        if (num1 >= 0)
        {
          int num2 = translation.IndexOf("{[", startIndex2 + 1, StringComparison.Ordinal);
          if (num2 > 0 && num2 < num1)
          {
            startIndex1 = num2;
          }
          else
          {
            int num3 = translation[startIndex2 + 2] == '#' ? 3 : 2;
            string str2 = translation.Substring(startIndex2 + num3, num1 - startIndex2 - num3);
            string str3 = (string) getParam(str2);
            if (str3 != null)
            {
              if (allowLocalizedParameters)
              {
                LanguageSourceData source;
                TermData termData = LocalizationManager.GetTermData(str3, out source);
                if (termData != null)
                {
                  int languageIndex = source.GetLanguageIndex(LocalizationManager.CurrentLanguage);
                  if (languageIndex >= 0)
                    str3 = termData.GetTranslation(languageIndex);
                }
              }
              string oldValue = translation.Substring(startIndex2, num1 - startIndex2 + 2);
              translation = translation.Replace(oldValue, str3);
              int result = 0;
              if (int.TryParse(str3, out result))
                str1 = GoogleLanguages.GetPluralType(LocalizationManager.CurrentLanguageCode, result).ToString();
              startIndex1 = startIndex2 + str3.Length;
            }
            else
              startIndex1 = num1 + 2;
          }
        }
        else
          break;
      }
      else
        break;
    }
    if (str1 == null)
      return;
    string str4 = $"[i2p_{str1}]";
    int num4 = translation.IndexOf(str4, StringComparison.OrdinalIgnoreCase);
    int startIndex3 = num4 >= 0 ? num4 + str4.Length : 0;
    int num5 = translation.IndexOf("[i2p_", startIndex3 + 1, StringComparison.OrdinalIgnoreCase);
    if (num5 < 0)
      num5 = translation.Length;
    translation = translation.Substring(startIndex3, num5 - startIndex3);
  }

  public static string GetLocalizationParam(string ParamName, GameObject root)
  {
    if ((bool) (UnityEngine.Object) root)
    {
      MonoBehaviour[] components = root.GetComponents<MonoBehaviour>();
      int index = 0;
      for (int length = components.Length; index < length; ++index)
      {
        if (components[index] is ILocalizationParamsManager localizationParamsManager && components[index].enabled)
        {
          string parameterValue = localizationParamsManager.GetParameterValue(ParamName);
          if (parameterValue != null)
            return parameterValue;
        }
      }
    }
    int index1 = 0;
    for (int count = LocalizationManager.ParamManagers.Count; index1 < count; ++index1)
    {
      string parameterValue = LocalizationManager.ParamManagers[index1].GetParameterValue(ParamName);
      if (parameterValue != null)
        return parameterValue;
    }
    return (string) null;
  }

  public static string GetPluralType(
    MatchCollection matches,
    string langCode,
    LocalizationManager._GetParam getParam)
  {
    int i = 0;
    for (int count = matches.Count; i < count; ++i)
    {
      Match match = matches[i];
      string str = match.Groups[match.Groups.Count - 1].Value;
      string s = (string) getParam(str);
      if (s != null)
      {
        int result = 0;
        if (int.TryParse(s, out result))
          return GoogleLanguages.GetPluralType(langCode, result).ToString();
      }
    }
    return (string) null;
  }

  public static string ApplyRTLfix(string line) => LocalizationManager.ApplyRTLfix(line, 0, true);

  public static string ApplyRTLfix(string line, int maxCharacters, bool ignoreNumbers)
  {
    if (string.IsNullOrEmpty(line))
      return line;
    char ch = line[0];
    switch (ch)
    {
      case '!':
      case '.':
      case '?':
        line = line.Substring(1) + ch.ToString();
        break;
    }
    int tagStart = -1;
    int num1 = 40000;
    int tagEnd = 0;
    List<string> stringList = new List<string>();
    for (; I2Utils.FindNextTag(line, tagEnd, out tagStart, out tagEnd); tagEnd = tagStart + 5)
    {
      string str = $"@@{((char) (num1 + stringList.Count)).ToString()}@@";
      stringList.Add(line.Substring(tagStart, tagEnd - tagStart + 1));
      line = line.Substring(0, tagStart) + str + line.Substring(tagEnd + 1);
    }
    line = line.Replace("\r\n", "\n");
    line = I2Utils.SplitLine(line, maxCharacters);
    line = RTLFixer.Fix(line, true, !ignoreNumbers);
    for (int index1 = 0; index1 < stringList.Count; ++index1)
    {
      int length = line.Length;
      for (int index2 = 0; index2 < length - 4; ++index2)
      {
        if (line[index2] == '@' && line[index2 + 1] == '@' && (int) line[index2 + 2] >= num1 && line[index2 + 3] == '@' && line[index2 + 4] == '@')
        {
          int num2 = (int) line[index2 + 2] - num1;
          int index3 = num2 % 2 != 0 ? num2 - 1 : num2 + 1;
          if (index3 >= stringList.Count)
            index3 = stringList.Count - 1;
          line = line.Substring(0, index2) + stringList[index3] + line.Substring(index2 + 5);
          break;
        }
      }
    }
    return line;
  }

  public static string FixRTL_IfNeeded(string text, int maxCharacters = 0, bool ignoreNumber = false)
  {
    return LocalizationManager.IsRight2Left ? LocalizationManager.ApplyRTLfix(text, maxCharacters, ignoreNumber) : text;
  }

  public static bool IsRTL(string Code)
  {
    return Array.IndexOf<string>(LocalizationManager.LanguagesRTL, Code) >= 0;
  }

  public static bool UpdateSources()
  {
    LocalizationManager.UnregisterDeletededSources();
    LocalizationManager.RegisterSourceInResources();
    LocalizationManager.RegisterSceneSources();
    return LocalizationManager.Sources.Count > 0;
  }

  public static void UnregisterDeletededSources()
  {
    for (int index = LocalizationManager.Sources.Count - 1; index >= 0; --index)
    {
      if (LocalizationManager.Sources[index] == null)
        LocalizationManager.RemoveSource(LocalizationManager.Sources[index]);
    }
  }

  public static void RegisterSceneSources()
  {
    foreach (LanguageSource languageSource in (LanguageSource[]) UnityEngine.Resources.FindObjectsOfTypeAll((System.Type) typeof (LanguageSource)))
    {
      if (!LocalizationManager.Sources.Contains(languageSource.mSource))
      {
        if (languageSource.mSource.owner == null)
          languageSource.mSource.owner = (ILanguageSource) languageSource;
        LocalizationManager.AddSource(languageSource.mSource);
      }
    }
  }

  public static void RegisterSourceInResources()
  {
    foreach (string globalSource in LocalizationManager.GlobalSources)
    {
      LanguageSourceAsset asset = ResourceManager.pInstance.GetAsset<LanguageSourceAsset>(globalSource);
      if ((bool) (UnityEngine.Object) asset && !LocalizationManager.Sources.Contains(asset.mSource))
      {
        if (!asset.mSource.mIsGlobalSource)
          asset.mSource.mIsGlobalSource = true;
        asset.mSource.owner = (ILanguageSource) asset;
        LocalizationManager.AddSource(asset.mSource);
      }
    }
  }

  public static bool AllowSyncFromGoogle(LanguageSourceData Source)
  {
    return LocalizationManager.Callback_AllowSyncFromGoogle == null || LocalizationManager.Callback_AllowSyncFromGoogle(Source);
  }

  public static void AddSource(LanguageSourceData Source)
  {
    if (LocalizationManager.Sources.Contains(Source))
      return;
    LocalizationManager.Sources.Add(Source);
    if (Source.HasGoogleSpreadsheet() && Source.GoogleUpdateFrequency != LanguageSourceData.eGoogleUpdateFrequency.Never && LocalizationManager.AllowSyncFromGoogle(Source))
    {
      Source.Import_Google_FromCache();
      bool justCheck = false;
      if ((double) Source.GoogleUpdateDelay > 0.0)
        CoroutineManager.Start(LocalizationManager.Delayed_Import_Google(Source, Source.GoogleUpdateDelay, justCheck));
      else
        Source.Import_Google(false, justCheck);
    }
    for (int index = 0; index < Source.mLanguages.Count; ++index)
      Source.mLanguages[index].SetLoaded(true);
    if (Source.mDictionary.Count != 0)
      return;
    Source.UpdateDictionary(true);
  }

  public static IEnumerator Delayed_Import_Google(
    LanguageSourceData source,
    float delay,
    bool justCheck)
  {
    yield return (object) new WaitForSeconds(delay);
    source?.Import_Google(false, justCheck);
  }

  public static void RemoveSource(LanguageSourceData Source)
  {
    LocalizationManager.Sources.Remove(Source);
  }

  public static bool IsGlobalSource(string SourceName)
  {
    return Array.IndexOf<string>(LocalizationManager.GlobalSources, SourceName) >= 0;
  }

  public static LanguageSourceData GetSourceContaining(string term, bool fallbackToFirst = true)
  {
    if (!string.IsNullOrEmpty(term))
    {
      int index = 0;
      for (int count = LocalizationManager.Sources.Count; index < count; ++index)
      {
        if (LocalizationManager.Sources[index].GetTermData(term) != null)
          return LocalizationManager.Sources[index];
      }
    }
    return !fallbackToFirst || LocalizationManager.Sources.Count <= 0 ? (LanguageSourceData) null : LocalizationManager.Sources[0];
  }

  public static UnityEngine.Object FindAsset(string value)
  {
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      UnityEngine.Object asset = LocalizationManager.Sources[index].FindAsset(value);
      if ((bool) asset)
        return asset;
    }
    return (UnityEngine.Object) null;
  }

  public static void ApplyDownloadedDataFromGoogle()
  {
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
      LocalizationManager.Sources[index].ApplyDownloadedDataFromGoogle();
  }

  public static string GetCurrentDeviceLanguage(bool force = false)
  {
    if (force || string.IsNullOrEmpty(LocalizationManager.mCurrentDeviceLanguage))
      LocalizationManager.DetectDeviceLanguage();
    return LocalizationManager.mCurrentDeviceLanguage;
  }

  public static void DetectDeviceLanguage()
  {
    LocalizationManager.mCurrentDeviceLanguage = Application.systemLanguage.ToString();
    if (LocalizationManager.mCurrentDeviceLanguage == "ChineseSimplified")
      LocalizationManager.mCurrentDeviceLanguage = "Chinese (Simplified)";
    if (!(LocalizationManager.mCurrentDeviceLanguage == "ChineseTraditional"))
      return;
    LocalizationManager.mCurrentDeviceLanguage = "Chinese (Traditional)";
  }

  public static void RegisterTarget(ILocalizeTargetDescriptor desc)
  {
    if (LocalizationManager.mLocalizeTargets.FindIndex((Predicate<ILocalizeTargetDescriptor>) (x => x.Name == desc.Name)) != -1)
      return;
    for (int index = 0; index < LocalizationManager.mLocalizeTargets.Count; ++index)
    {
      if (LocalizationManager.mLocalizeTargets[index].Priority > desc.Priority)
      {
        LocalizationManager.mLocalizeTargets.Insert(index, desc);
        return;
      }
    }
    LocalizationManager.mLocalizeTargets.Add(desc);
  }

  public static TMP_FontAsset DyslexicFontAsset
  {
    get => LocalizationManager.\u003CDyslexicFontAsset\u003Ek__BackingField;
    set => LocalizationManager.\u003CDyslexicFontAsset\u003Ek__BackingField = value;
  }

  public static event LocalizationManager.OnLocalizeCallback OnLocalizeEvent;

  public static string GetTranslation(
    string Term,
    bool FixForRTL = true,
    int maxLineLengthForRTL = 0,
    bool ignoreRTLnumbers = true,
    bool applyParameters = false,
    GameObject localParametersRoot = null,
    string overrideLanguage = null,
    bool allowLocalizedParameters = true)
  {
    if (Term == "NAMES/Knucklebones/Knucklebones_NPC_1" && DataManager.Instance.TookBopToTailor)
      Term = "NAMES/Knucklebones/Knucklebones_NPC_1/NoBop";
    string Translation = (string) null;
    LocalizationManager.TryGetTranslation(Term, out Translation, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
    return Translation;
  }

  public static string GetTermTranslation(
    string Term,
    bool FixForRTL = true,
    int maxLineLengthForRTL = 0,
    bool ignoreRTLnumbers = true,
    bool applyParameters = false,
    GameObject localParametersRoot = null,
    string overrideLanguage = null,
    bool allowLocalizedParameters = true)
  {
    return LocalizationManager.GetTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreRTLnumbers, applyParameters, localParametersRoot, overrideLanguage, allowLocalizedParameters);
  }

  public static bool TryGetTranslation(
    string Term,
    out string Translation,
    bool FixForRTL = true,
    int maxLineLengthForRTL = 0,
    bool ignoreRTLnumbers = true,
    bool applyParameters = false,
    GameObject localParametersRoot = null,
    string overrideLanguage = null,
    bool allowLocalizedParameters = true)
  {
    if (CheatConsole.SHOWING_LOC_KEYS)
    {
      Translation = Term;
      return true;
    }
    Translation = (string) null;
    if (string.IsNullOrEmpty(Term))
      return false;
    LocalizationManager.InitializeIfNeeded();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      if (LocalizationManager.Sources[index].TryGetTranslation(Term, out Translation, overrideLanguage))
      {
        if (applyParameters)
          LocalizationManager.ApplyLocalizationParams(ref Translation, localParametersRoot, allowLocalizedParameters);
        if (LocalizationManager.IsRight2Left & FixForRTL)
          Translation = LocalizeIntegration.ConvertToRTL(Translation);
        return true;
      }
    }
    return false;
  }

  public static T GetTranslatedObject<T>(string AssetName, Localize optionalLocComp = null) where T : UnityEngine.Object
  {
    if ((UnityEngine.Object) optionalLocComp != (UnityEngine.Object) null)
      return optionalLocComp.FindTranslatedObject<T>(AssetName);
    T asset = LocalizationManager.FindAsset(AssetName) as T;
    return (bool) (UnityEngine.Object) asset ? asset : ResourceManager.pInstance.GetAsset<T>(AssetName);
  }

  public static T GetTranslatedObjectByTermName<T>(string Term, Localize optionalLocComp = null) where T : UnityEngine.Object
  {
    return LocalizationManager.GetTranslatedObject<T>(LocalizationManager.GetTranslation(Term, false));
  }

  public static string GetAppName(string languageCode)
  {
    if (!string.IsNullOrEmpty(languageCode))
    {
      for (int index = 0; index < LocalizationManager.Sources.Count; ++index)
      {
        if (!string.IsNullOrEmpty(LocalizationManager.Sources[index].mTerm_AppName))
        {
          int languageIndexFromCode = LocalizationManager.Sources[index].GetLanguageIndexFromCode(languageCode, false);
          if (languageIndexFromCode >= 0)
          {
            TermData termData = LocalizationManager.Sources[index].GetTermData(LocalizationManager.Sources[index].mTerm_AppName);
            if (termData != null)
            {
              string translation = termData.GetTranslation(languageIndexFromCode);
              if (!string.IsNullOrEmpty(translation))
                return translation;
            }
          }
        }
      }
    }
    return Application.productName;
  }

  public static void LocalizeAll(bool Force = false)
  {
    LocalizationManager.LoadCurrentLanguage();
    if (!Application.isPlaying)
    {
      LocalizationManager.DoLocalizeAll(Force);
    }
    else
    {
      LocalizationManager.mLocalizeIsScheduledWithForcedValue |= Force;
      if (LocalizationManager.mLocalizeIsScheduled)
        return;
      CoroutineManager.Start(LocalizationManager.Coroutine_LocalizeAll());
    }
  }

  public static IEnumerator Coroutine_LocalizeAll()
  {
    LocalizationManager.CheckForDyslexicFonts();
    if (LocalizationManager.CurrentLanguage == "English" && SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.DyslexicFont)
      yield return (object) new WaitWhile((Func<bool>) (() => (UnityEngine.Object) LocalizationManager.DyslexicFontAsset == (UnityEngine.Object) null));
    LocalizationManager.mLocalizeIsScheduled = true;
    yield return (object) null;
    LocalizationManager.mLocalizeIsScheduled = false;
    int num = LocalizationManager.mLocalizeIsScheduledWithForcedValue ? 1 : 0;
    LocalizationManager.mLocalizeIsScheduledWithForcedValue = false;
    LocalizationManager.DoLocalizeAll(num != 0);
  }

  public static void DoLocalizeAll(bool Force = false)
  {
    Localize[] objectsOfTypeAll = (Localize[]) UnityEngine.Resources.FindObjectsOfTypeAll((System.Type) typeof (Localize));
    int index = 0;
    for (int length = objectsOfTypeAll.Length; index < length; ++index)
    {
      Localize localize = objectsOfTypeAll[index];
      try
      {
        localize.OnLocalize(Force);
      }
      catch (Exception ex)
      {
        Debug.LogWarning((object) ("Exception calling OnLocalize: " + ex?.ToString()));
      }
    }
    if (LocalizationManager.OnLocalizeEvent == null)
      return;
    try
    {
      LocalizationManager.OnLocalizeEvent();
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("Exception calling OnLocalizeEvent: " + ex?.ToString()));
    }
  }

  public static void GetLaptureRegularFont(Action<TMP_FontAsset> onLoaded)
  {
    ResourceManager.pInstance.LoadFromAddressables<TMP_FontAsset>("Fonts/Headings/LaptureRegular", true, (Action<TMP_FontAsset>) (font =>
    {
      Action<TMP_FontAsset> action = onLoaded;
      if (action == null)
        return;
      action(font);
    }));
  }

  public static void CheckForDyslexicFonts(Action<TMP_FontAsset> onLoaded = null)
  {
    if (LocalizationManager.CurrentLanguage == "English" && SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.DyslexicFont)
    {
      ResourceManager.pInstance.LoadFromAddressables<TMP_FontAsset>("Fonts/Dyslexic/OpenDyslexic-Regular SDF", true, (Action<TMP_FontAsset>) (font =>
      {
        LocalizationManager.DyslexicFontAsset = font;
        Action<TMP_FontAsset> action = onLoaded;
        if (action == null)
          return;
        action(font);
      }));
    }
    else
    {
      if (!((UnityEngine.Object) LocalizationManager.DyslexicFontAsset != (UnityEngine.Object) null))
        return;
      ResourceManager.pInstance.UnloadAddressable("Fonts/Dyslexic/OpenDyslexic-Regular SDF");
      LocalizationManager.DyslexicFontAsset = (TMP_FontAsset) null;
    }
  }

  public static void SetupFonts()
  {
    if (LocalizationManager.FontsLoadedonce)
    {
      string currentLanguage = LocalizationManager.CurrentLanguage;
      if (currentLanguage == LocalizationManager.lastLanguage || LocalizationManager.lastLanguage.Contains("Chinese") && currentLanguage.Contains("Chinese"))
        return;
      LocalizationManager.lastLanguage = currentLanguage;
    }
    if (!LocalizationManager.FontsLoadedonce)
    {
      LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/Russian/NotoSans-Regular SDF", true);
      LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/FontAwesome/Font Awesome 6 Pro-Solid-900 SDF", true);
      LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/Headings/LaptureRegular", true);
      LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/Headings/Lapture-Bold", true);
      LocalizationManager.FontsLoadedonce = true;
      LocalizationManager.previouslyUsedFontPaths.Clear();
      LocalizationManager.previouslyUsedFonts.Clear();
    }
    LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/Chinese(Simplified)/Chinese(Simplified)-NotoSansSC-Regular SDF", true);
    LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/Chinese(Tranditional)/Chinese(Traditional)-NotoSansTC-Regular SDF", true);
    LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/Korean/NotoSansKR-Regular SDF", true);
    LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/Japanese/NotoSansJP-Regular SDF", true);
    LocalizationManager.\u003CSetupFonts\u003Eg__LoadFont\u007C104_0("Fonts/LocalisedFonts/Arabic/NotoSansArabic-Regular", true);
  }

  public static List<string> GetCategories()
  {
    List<string> Categories = new List<string>();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
      LocalizationManager.Sources[index].GetCategories(Categories: Categories);
    return Categories;
  }

  public static List<string> GetTermsList(string Category = null)
  {
    if (LocalizationManager.Sources.Count == 0)
      LocalizationManager.UpdateSources();
    if (LocalizationManager.Sources.Count == 1)
      return LocalizationManager.Sources[0].GetTermsList(Category);
    HashSet<string> collection = new HashSet<string>();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
      collection.UnionWith((IEnumerable<string>) LocalizationManager.Sources[index].GetTermsList(Category));
    return new List<string>((IEnumerable<string>) collection);
  }

  public static TermData GetTermData(string term)
  {
    LocalizationManager.InitializeIfNeeded();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      TermData termData = LocalizationManager.Sources[index].GetTermData(term);
      if (termData != null)
        return termData;
    }
    return (TermData) null;
  }

  public static TermData GetTermData(string term, out LanguageSourceData source)
  {
    LocalizationManager.InitializeIfNeeded();
    int index = 0;
    for (int count = LocalizationManager.Sources.Count; index < count; ++index)
    {
      TermData termData = LocalizationManager.Sources[index].GetTermData(term);
      if (termData != null)
      {
        source = LocalizationManager.Sources[index];
        return termData;
      }
    }
    source = (LanguageSourceData) null;
    return (TermData) null;
  }

  [CompilerGenerated]
  public static void \u003CSetupFonts\u003Eg__LoadFont\u007C104_0(
    string path,
    bool async = false,
    Action<string, TMP_FontAsset> onLoaded = null)
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    LocalizationManager.\u003C\u003Ec__DisplayClass104_0 displayClass1040 = new LocalizationManager.\u003C\u003Ec__DisplayClass104_0();
    // ISSUE: reference to a compiler-generated field
    displayClass1040.onLoaded = onLoaded;
    // ISSUE: reference to a compiler-generated field
    displayClass1040.path = path;
    if (LocalizationManager.ForceLoadSynchronous)
      async = false;
    if (!async)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      displayClass1040.\u003CSetupFonts\u003Eg__AddFont\u007C3(ResourceManager.pInstance.LoadFromAddressables<TMP_FontAsset>(displayClass1040.path), displayClass1040.onLoaded);
    }
    else
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourceManager.pInstance.LoadFromAddressables<TMP_FontAsset>(displayClass1040.path, true, new Action<TMP_FontAsset>(displayClass1040.\u003CSetupFonts\u003Eb__2));
    }
  }

  [CompilerGenerated]
  public static void \u003CSetupFonts\u003Eg__UnloadFont\u007C104_1(
    string path,
    ref TMP_FontAsset storedFont)
  {
    if (!((UnityEngine.Object) storedFont != (UnityEngine.Object) null))
      return;
    ResourceManager.pInstance.UnloadAddressable(path);
    if (TMP_Settings.fallbackFontAssets.Contains(storedFont))
      TMP_Settings.fallbackFontAssets.Remove(storedFont);
    storedFont = (TMP_FontAsset) null;
  }

  public delegate bool FnCustomApplyLocalizationParams(
    ref string translation,
    LocalizationManager._GetParam getParam,
    bool allowLocalizedParameters);

  public delegate object _GetParam(string param);

  public delegate void OnLocalizeCallback();
}
