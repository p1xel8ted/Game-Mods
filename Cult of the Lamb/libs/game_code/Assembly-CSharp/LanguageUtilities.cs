// Decompiled with JetBrains decompiler
// Type: LanguageUtilities
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Unify;
using UnityEngine;

#nullable disable
public class LanguageUtilities
{
  public static string[] AllLanguages = new string[15]
  {
    "English",
    "Japanese",
    "Russian",
    "French",
    "German",
    "Spanish",
    "Portuguese (Brazil)",
    "Chinese (Simplified)",
    "Chinese (Traditional)",
    "Korean",
    "Italian",
    "Dutch",
    "Turkish",
    "French (Canadian)",
    "Arabic"
  };
  public static string[] AllLanguagesLocalizations = new string[15]
  {
    "UI/Settings/Game/Languages/English",
    "UI/Settings/Game/Languages/Japanese",
    "UI/Settings/Game/Languages/Russian",
    "UI/Settings/Game/Languages/French",
    "UI/Settings/Game/Languages/German",
    "UI/Settings/Game/Languages/Spanish",
    "UI/Settings/Game/Languages/Portuguese (Brazil)",
    "UI/Settings/Game/Languages/Chinese (Simplified)",
    "UI/Settings/Game/Languages/Chinese (Traditional)",
    "UI/Settings/Game/Languages/Korean",
    "UI/Settings/Game/Languages/Italian",
    "UI/Settings/Game/Languages/Dutch",
    "UI/Settings/Game/Languages/Turkish",
    "UI/Settings/Game/Languages/FrenchCanadian",
    "UI/Settings/Game/Languages/Arabic"
  };

  public static string SystemLanguageToLanguage(SystemLanguage language)
  {
    string language1;
    switch (language)
    {
      case SystemLanguage.Arabic:
        language1 = "Arabic";
        break;
      case SystemLanguage.Dutch:
        language1 = "Dutch";
        break;
      case SystemLanguage.French:
        language1 = "French";
        break;
      case SystemLanguage.German:
        language1 = "German";
        break;
      case SystemLanguage.Italian:
        language1 = "Italian";
        break;
      case SystemLanguage.Japanese:
        language1 = "Japanese";
        break;
      case SystemLanguage.Korean:
        language1 = "Korean";
        break;
      case SystemLanguage.Portuguese:
        language1 = "Portuguese (Brazil)";
        break;
      case SystemLanguage.Russian:
        language1 = "Russian";
        break;
      case SystemLanguage.Spanish:
        language1 = "Spanish";
        break;
      case SystemLanguage.Turkish:
        language1 = "Turkish";
        break;
      case SystemLanguage.ChineseSimplified:
        language1 = "Chinese (Simplified)";
        break;
      case SystemLanguage.ChineseTraditional:
        language1 = "Chinese (Traditional)";
        break;
      default:
        language1 = "English";
        break;
    }
    return language1;
  }

  public static string GetDefaultLanguage()
  {
    string defaultLanguage = "English";
    switch (UnifyManager.language)
    {
      case SystemLanguage.Arabic:
        defaultLanguage = "Arabic";
        break;
      case SystemLanguage.Dutch:
        defaultLanguage = "Dutch";
        break;
      case SystemLanguage.English:
        defaultLanguage = "English";
        break;
      case SystemLanguage.French:
        defaultLanguage = "French";
        break;
      case SystemLanguage.German:
        defaultLanguage = "German";
        break;
      case SystemLanguage.Italian:
        defaultLanguage = "Italian";
        break;
      case SystemLanguage.Japanese:
        defaultLanguage = "Japanese";
        break;
      case SystemLanguage.Korean:
        defaultLanguage = "Korean";
        break;
      case SystemLanguage.Portuguese:
        defaultLanguage = "Portuguese (Brazil)";
        break;
      case SystemLanguage.Russian:
        defaultLanguage = "Russian";
        break;
      case SystemLanguage.Spanish:
        defaultLanguage = "Spanish";
        break;
      case SystemLanguage.Turkish:
        defaultLanguage = "Turkish";
        break;
      case SystemLanguage.ChineseSimplified:
        defaultLanguage = "Chinese (Simplified)";
        break;
      case SystemLanguage.ChineseTraditional:
        defaultLanguage = "Chinese (Traditional)";
        break;
    }
    Debug.Log((object) ("Load system language:" + defaultLanguage));
    return defaultLanguage;
  }
}
