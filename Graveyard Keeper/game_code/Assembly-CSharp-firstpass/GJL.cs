// Decompiled with JetBrains decompiler
// Type: GJL
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GJL : ScriptableObject
{
  public static string[] LANGUAGES = new string[11]
  {
    "en",
    "de",
    "fr",
    "pt-br",
    "es",
    "ru",
    "it",
    "pl",
    "ja",
    "zh_cn",
    "ko"
  };
  [NonSerialized]
  public Dictionary<string, string> dict = new Dictionary<string, string>();
  public string id = "en";
  [SerializeField]
  public List<string> txt_ids = new List<string>();
  [SerializeField]
  public List<string> txts = new List<string>();
  public static GJL cur_lng = (GJL) null;
  public static string[] AVAILABLE_LOCALES = new string[11]
  {
    "en",
    "de",
    "fr",
    "ru",
    "es",
    "pt-br",
    "it",
    "ja",
    "zh_cn",
    "ko",
    "pl"
  };
  public static string[] AVAILABLE_LOCALE_NAMES = new string[11]
  {
    "English",
    "Deutsch",
    "Français",
    "Русский",
    "Español",
    "Português do Brasil",
    "Italiano",
    "Japanese",
    "Chinese",
    "Korean",
    "Polski"
  };
  public List<string> aliases_1 = new List<string>();
  public List<string> aliases_2 = new List<string>();
  public static Dictionary<string, GJL.CustomFont> CUSTOM_FONTS = new Dictionary<string, GJL.CustomFont>()
  {
    {
      "ja",
      new GJL.CustomFont()
      {
        filename = "ngui_fonts/ch_jp_12",
        size = 12,
        spacing_y = 2
      }
    },
    {
      "zh_cn",
      new GJL.CustomFont()
      {
        filename = "ngui_fonts/ch_jp_12",
        size = 12,
        spacing_y = 2
      }
    },
    {
      "ko",
      new GJL.CustomFont()
      {
        filename = "ngui_fonts/korean",
        size = 13,
        spacing_y = 1
      }
    }
  };
  public static Dictionary<UILabel, GJL.LabelCacheData> _labels_cache = new Dictionary<UILabel, GJL.LabelCacheData>();
  public static Dictionary<string, UIFont> _loaded_fonts = new Dictionary<string, UIFont>();

  public static void LoadLanguageResource(string lng_id)
  {
    GJL.cur_lng = Resources.Load<GJL>("Locales/lng_" + lng_id);
    if ((UnityEngine.Object) GJL.cur_lng == (UnityEngine.Object) null && lng_id != "en")
      GJL.LoadLanguageResource("en");
    Debug.Log((object) $"LoadLanguageResource(\"{lng_id}\"), {GJL.cur_lng.txts.Count.ToString()} lines loaded");
    GJL.cur_lng.InitHashDictionary();
  }

  public void InitHashDictionary()
  {
    this.dict.Clear();
    for (int index = 0; index < this.txt_ids.Count; ++index)
      this.dict.Add(this.txt_ids[index], this.txts[index]);
  }

  public static string GetCurrentLocaleCode()
  {
    string currentLocaleCode = GJL.GetLocaleCode().ToLower();
    switch (currentLocaleCode)
    {
      case "ptbr":
        currentLocaleCode = "pt-br";
        break;
      case "es-es":
      case "es-mx":
        currentLocaleCode = "es";
        break;
    }
    if (!((IEnumerable<string>) GJL.LANGUAGES).Contains<string>(currentLocaleCode))
    {
      Debug.LogWarning((object) $"Language '{currentLocaleCode}' not found. Loading EN...");
      currentLocaleCode = "en";
    }
    return currentLocaleCode;
  }

  public static string GetLocaleCode()
  {
    switch (Application.systemLanguage)
    {
      case SystemLanguage.Belarusian:
      case SystemLanguage.Russian:
      case SystemLanguage.Ukrainian:
        return "ru";
      case SystemLanguage.Chinese:
        return "zh_cn";
      case SystemLanguage.French:
        return "fr";
      case SystemLanguage.German:
        return "de";
      case SystemLanguage.Japanese:
        return "ja";
      case SystemLanguage.Korean:
        return "ko";
      case SystemLanguage.Portuguese:
        return "pt-br";
      case SystemLanguage.Spanish:
        return "es";
      default:
        return "en";
    }
  }

  public static void LoadLangugage()
  {
    string lng_id = GJL.GetLocaleCode();
    if (lng_id == "ptbr")
      lng_id = "pt-br";
    GJL.LoadLanguageResource(lng_id);
  }

  public void AddLngString(string id, string txt)
  {
    int index = this.txt_ids.IndexOf(id);
    if (index == -1)
    {
      this.txt_ids.Add(id);
      this.txts.Add(txt);
    }
    else
    {
      this.txt_ids[index] = id;
      this.txts[index] = txt;
    }
  }

  public static string L(string lng_id)
  {
    if (lng_id == null)
    {
      Debug.LogError((object) "lng_id is null");
      return "";
    }
    if ((UnityEngine.Object) GJL.cur_lng != (UnityEngine.Object) null)
    {
      int index = GJL.cur_lng.aliases_1.IndexOf(lng_id);
      if (index != -1)
        return GJL.L(GJL.cur_lng.aliases_2[index]);
    }
    return ((UnityEngine.Object) GJL.cur_lng == (UnityEngine.Object) null || !GJL.cur_lng.dict.ContainsKey(lng_id) ? lng_id : GJL.cur_lng.dict[lng_id]).Replace("&#xA;", "\n").Replace("’", "'");
  }

  public static string L_colored(string lng_id, string v1, string color_1)
  {
    return GJL.L(lng_id).Replace("%1", $"[{color_1}]{v1}[-]");
  }

  public static string L(string lng_id, string v1) => GJL.L(lng_id).Replace("%1", v1);

  public static string L(string lng_id, string v1, string v2)
  {
    return GJL.L(lng_id).Replace("%1", v1).Replace("%2", v2);
  }

  public static string L(string lng_id, string v1, string v2, string v3)
  {
    return GJL.L(lng_id).Replace("%1", v1).Replace("%2", v2).Replace("%3", v3);
  }

  public static string L(string lng_id, int v1) => GJL.L(lng_id, v1.ToString());

  public static string L(string lng_id, float v1) => GJL.L(lng_id, v1.ToString());

  public static string L(string lng_id, int v1, int v2)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString());
  }

  public static string L(string lng_id, string v1, int v2)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString());
  }

  public static string L(string lng_id, int v1, string v2)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString());
  }

  public static string L(string lng_id, float v1, float v2)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString());
  }

  public static string L(string lng_id, string v1, float v2)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString());
  }

  public static string L(string lng_id, float v1, string v2)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString());
  }

  public static string L(string lng_id, string v1, int v2, int v3)
  {
    return GJL.L(lng_id, v1.ToString(), v2.ToString(), v3.ToString());
  }

  public static string GetLocaleNameByCode(string locale_code)
  {
    for (int index = 0; index < GJL.AVAILABLE_LOCALES.Length; ++index)
    {
      if (GJL.AVAILABLE_LOCALES[index] == locale_code)
        return GJL.AVAILABLE_LOCALE_NAMES[index];
    }
    return "?";
  }

  public static string GetCurLng() => GJL.cur_lng.id;

  public static bool IsEastern()
  {
    if ((UnityEngine.Object) GJL.cur_lng == (UnityEngine.Object) null)
      return false;
    return GJL.cur_lng.id == "ja" || GJL.cur_lng.id == "zh_cn" || GJL.cur_lng.id == "ko";
  }

  public static void EnsureChildLabelsHasCorrectFont(GameObject go, bool do_cache = true)
  {
    foreach (UILabel componentsInChild in go.GetComponentsInChildren<UILabel>(true))
      GJL.EnsureLabelHasCorrectFont(componentsInChild, do_cache);
  }

  public static void ApplyCustomFontSettings(GameObject go)
  {
    GJL.CustomFont customFont = (GJL.CustomFont) null;
    GJL.CUSTOM_FONTS.TryGetValue(GJL.cur_lng.id, out customFont);
    foreach (CustomFontSettings componentsInChild in go.GetComponentsInChildren<CustomFontSettings>(true))
    {
      if (!((UnityEngine.Object) componentsInChild.GetComponent<UILabel>() != (UnityEngine.Object) null))
      {
        if (customFont == null)
          componentsInChild.Restore();
        else
          componentsInChild.Apply();
      }
    }
  }

  public static void EnsureLabelHasCorrectFont(UILabel label, bool do_cache = true)
  {
    GJL.LabelCacheData labelCacheData;
    if (!GJL._labels_cache.TryGetValue(label, out labelCacheData))
    {
      labelCacheData = new GJL.LabelCacheData()
      {
        font_can_be_changed = (UnityEngine.Object) label.GetComponent<StaticFontLabel>() == (UnityEngine.Object) null,
        orig_bitmap_font = label.bitmapFont,
        orig_spacing_y = label.spacingY,
        orig_overflow_method = label.overflowMethod,
        orig_font_size = label.fontSize,
        current_font_settings = (GJL.CustomFont) null
      };
      if (do_cache)
        GJL._labels_cache.Add(label, labelCacheData);
    }
    if (!labelCacheData.font_can_be_changed)
      return;
    GJL.CustomFont customFont = (GJL.CustomFont) null;
    GJL.CUSTOM_FONTS.TryGetValue(GJL.cur_lng.id, out customFont);
    if (labelCacheData.current_font_settings == customFont)
      return;
    CustomFontSettings component = label.GetComponent<CustomFontSettings>();
    labelCacheData.current_font_settings = customFont;
    if (customFont == null)
    {
      label.bitmapFont = labelCacheData.orig_bitmap_font;
      label.spacingY = labelCacheData.orig_spacing_y;
      label.overflowMethod = labelCacheData.orig_overflow_method;
      label.fontSize = labelCacheData.orig_font_size;
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.Restore();
    }
    else
    {
      label.fontSize = customFont.size;
      UIFont uiFont = (UIFont) null;
      if (!GJL._loaded_fonts.TryGetValue(customFont.filename, out uiFont))
      {
        uiFont = Resources.Load<UIFont>(customFont.filename);
        if ((UnityEngine.Object) uiFont == (UnityEngine.Object) null)
        {
          Debug.LogError((object) ("Couldn't load a custom font: " + customFont.filename));
          return;
        }
        GJL._loaded_fonts.Add(customFont.filename, uiFont);
      }
      label.bitmapFont = uiFont;
      label.spacingY = customFont.spacing_y;
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
        return;
      component.Apply();
    }
  }

  public class CustomFont
  {
    public string filename;
    public int size;
    public int spacing_y;
  }

  public class LabelCacheData
  {
    public bool font_can_be_changed;
    public UIFont orig_bitmap_font;
    public int orig_spacing_y;
    public UILabel.Overflow orig_overflow_method;
    public int orig_font_size;
    public GJL.CustomFont current_font_settings;
  }
}
