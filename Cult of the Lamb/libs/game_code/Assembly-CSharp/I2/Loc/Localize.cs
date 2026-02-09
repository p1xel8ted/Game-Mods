// Decompiled with JetBrains decompiler
// Type: I2.Loc.Localize
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace I2.Loc;

[AddComponentMenu("I2/Localization/I2 Localize")]
public class Localize : MonoBehaviour
{
  public string mTerm = string.Empty;
  public string mTermSecondary = string.Empty;
  [NonSerialized]
  public string FinalTerm;
  [NonSerialized]
  public string FinalSecondaryTerm;
  public Localize.TermModification PrimaryTermModifier;
  public Localize.TermModification SecondaryTermModifier;
  public string TermPrefix;
  public string TermSuffix;
  public bool LocalizeOnAwake = true;
  public string LastLocalizedLanguage;
  public bool IgnoreRTL;
  public int MaxCharactersInRTL;
  public bool IgnoreNumbersInRTL = true;
  public bool CorrectAlignmentForRTL = true;
  public bool AddSpacesToJoinedLanguages;
  public bool AllowLocalizedParameters = true;
  public bool AllowParameters = true;
  public List<UnityEngine.Object> TranslatedObjects = new List<UnityEngine.Object>();
  [NonSerialized]
  public Dictionary<string, UnityEngine.Object> mAssetDictionary = new Dictionary<string, UnityEngine.Object>((IEqualityComparer<string>) StringComparer.Ordinal);
  public UnityEvent LocalizeEvent = new UnityEvent();
  public static string MainTranslation;
  public static string SecondaryTranslation;
  public static string CallBackTerm;
  public static string CallBackSecondaryTerm;
  public static Localize CurrentLocalizeComponent;
  public bool AlwaysForceLocalize;
  [SerializeField]
  public EventCallback LocalizeCallBack = new EventCallback();
  public bool mGUI_ShowReferences;
  public bool mGUI_ShowTems = true;
  public bool mGUI_ShowCallback;
  public ILocalizeTarget mLocalizeTarget;
  public string mLocalizeTargetName;
  public TextMeshProUGUI _text;

  public string Term
  {
    get => this.mTerm;
    set => this.SetTerm(value);
  }

  public string SecondaryTerm
  {
    get => this.mTermSecondary;
    set => this.SetTerm((string) null, value);
  }

  public void Awake()
  {
    this.UpdateAssetDictionary();
    this.FindTarget();
    if ((UnityEngine.Object) this._text == (UnityEngine.Object) null)
      this._text = this.gameObject.GetComponent<TextMeshProUGUI>();
    if (!this.LocalizeOnAwake)
      return;
    this.OnLocalize();
  }

  public void OnEnable() => this.OnLocalize(LocalizationManager.CurrentLanguage != "English");

  public void SetupRTL(TextMeshProUGUI tmpUGUI)
  {
    if ((UnityEngine.Object) tmpUGUI == (UnityEngine.Object) null)
      return;
    bool flag = LocalizationManager.IsRight2Left && !this.IgnoreRTL;
    if (LocalizationManager.CurrentLanguage == "Arabic")
    {
      if (Localize.IsValidNumericString(tmpUGUI.text))
        tmpUGUI.isRightToLeftText = false;
      else
        tmpUGUI.isRightToLeftText = flag;
    }
    else
      tmpUGUI.isRightToLeftText = false;
  }

  public bool HasCallback()
  {
    return this.LocalizeCallBack.HasCallback() || this.LocalizeEvent.GetPersistentEventCount() > 0;
  }

  public void OnLocalize(bool Force = false)
  {
    if (!Force && (!this.enabled || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null || !this.gameObject.activeInHierarchy) || string.IsNullOrEmpty(LocalizationManager.CurrentLanguage) || !this.AlwaysForceLocalize && !Force && !this.HasCallback() && this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
      return;
    this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
    if (string.IsNullOrEmpty(this.FinalTerm) || string.IsNullOrEmpty(this.FinalSecondaryTerm))
      this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
    bool flag1 = I2Utils.IsPlaying() && this.HasCallback();
    if (!flag1 && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
      return;
    Localize.CallBackTerm = this.FinalTerm;
    Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
    Localize.MainTranslation = string.IsNullOrEmpty(this.FinalTerm) || this.FinalTerm == "-" ? (string) null : LocalizationManager.GetTranslation(this.FinalTerm, false);
    Localize.SecondaryTranslation = string.IsNullOrEmpty(this.FinalSecondaryTerm) || this.FinalSecondaryTerm == "-" ? (string) null : LocalizationManager.GetTranslation(this.FinalSecondaryTerm, false);
    if (!flag1 && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
      return;
    Localize.CurrentLocalizeComponent = this;
    this.LocalizeCallBack.Execute((UnityEngine.Object) this);
    this.LocalizeEvent.Invoke();
    if (this.AllowParameters)
      LocalizationManager.ApplyLocalizationParams(ref Localize.MainTranslation, this.gameObject, this.AllowLocalizedParameters);
    if (!this.FindTarget())
      return;
    bool flag2 = LocalizationManager.IsRight2Left && !this.IgnoreRTL;
    if (Localize.MainTranslation != null)
    {
      switch (this.PrimaryTermModifier)
      {
        case Localize.TermModification.ToUpper:
          Localize.MainTranslation = Localize.MainTranslation.ToUpper();
          break;
        case Localize.TermModification.ToLower:
          Localize.MainTranslation = Localize.MainTranslation.ToLower();
          break;
        case Localize.TermModification.ToUpperFirst:
          Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
          break;
        case Localize.TermModification.ToTitle:
          Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
          break;
      }
      if (!string.IsNullOrEmpty(this.TermPrefix))
        Localize.MainTranslation = flag2 ? Localize.MainTranslation + this.TermPrefix : this.TermPrefix + Localize.MainTranslation;
      if (!string.IsNullOrEmpty(this.TermSuffix))
        Localize.MainTranslation = flag2 ? this.TermSuffix + Localize.MainTranslation : Localize.MainTranslation + this.TermSuffix;
      if (this.AddSpacesToJoinedLanguages && LocalizationManager.HasJoinedWords && !string.IsNullOrEmpty(Localize.MainTranslation))
      {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Localize.MainTranslation[0]);
        int index = 1;
        for (int length = Localize.MainTranslation.Length; index < length; ++index)
        {
          stringBuilder.Append(' ');
          stringBuilder.Append(Localize.MainTranslation[index]);
        }
        Localize.MainTranslation = stringBuilder.ToString();
      }
      if (flag2 && this.mLocalizeTarget.AllowMainTermToBeRTL() && !string.IsNullOrEmpty(Localize.MainTranslation))
        Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL, this.IgnoreNumbersInRTL);
    }
    if (Localize.SecondaryTranslation != null)
    {
      switch (this.SecondaryTermModifier)
      {
        case Localize.TermModification.ToUpper:
          Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
          break;
        case Localize.TermModification.ToLower:
          Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
          break;
        case Localize.TermModification.ToUpperFirst:
          Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
          break;
        case Localize.TermModification.ToTitle:
          Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
          break;
      }
      if (flag2 && this.mLocalizeTarget.AllowSecondTermToBeRTL() && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
        Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
    }
    if (LocalizationManager.HighlightLocalizedTargets)
      Localize.MainTranslation = "LOC:" + this.FinalTerm;
    this.mLocalizeTarget.DoLocalize(this, Localize.MainTranslation, Localize.SecondaryTranslation);
    Localize.CurrentLocalizeComponent = (Localize) null;
    this.SetupRTL(this._text);
    if (!(LocalizationManager.CurrentLanguage == "Arabic") || !((UnityEngine.Object) this._text != (UnityEngine.Object) null))
      return;
    this._text.text = Localize.StripRichText(this._text.text);
  }

  public static bool IsValidNumericString(string s)
  {
    if (s == null || s.Length == 0)
      return false;
    bool flag = false;
    for (int index = 0; index < s.Length; ++index)
    {
      char ch = s[index];
      switch (ch)
      {
        case '<':
          flag = true;
          break;
        case '>':
          flag = false;
          break;
        default:
          if (!flag && (ch < '0' || ch > '9') && ch != ':' && ch != '%' && ch != ' ')
            return false;
          break;
      }
    }
    return true;
  }

  public bool FindTarget()
  {
    if ((UnityEngine.Object) this.mLocalizeTarget != (UnityEngine.Object) null && this.mLocalizeTarget.IsValid(this))
      return true;
    if ((UnityEngine.Object) this.mLocalizeTarget != (UnityEngine.Object) null)
    {
      UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mLocalizeTarget);
      this.mLocalizeTarget = (ILocalizeTarget) null;
      this.mLocalizeTargetName = (string) null;
    }
    if (!string.IsNullOrEmpty(this.mLocalizeTargetName))
    {
      foreach (ILocalizeTargetDescriptor mLocalizeTarget in LocalizationManager.mLocalizeTargets)
      {
        if (this.mLocalizeTargetName == mLocalizeTarget.GetTargetType().ToString())
        {
          if (mLocalizeTarget.CanLocalize(this))
            this.mLocalizeTarget = mLocalizeTarget.CreateTarget(this);
          if ((UnityEngine.Object) this.mLocalizeTarget != (UnityEngine.Object) null)
            return true;
        }
      }
    }
    foreach (ILocalizeTargetDescriptor mLocalizeTarget in LocalizationManager.mLocalizeTargets)
    {
      if (mLocalizeTarget.CanLocalize(this))
      {
        this.mLocalizeTarget = mLocalizeTarget.CreateTarget(this);
        this.mLocalizeTargetName = mLocalizeTarget.GetTargetType().ToString();
        if ((UnityEngine.Object) this.mLocalizeTarget != (UnityEngine.Object) null)
          return true;
      }
    }
    return false;
  }

  public void GetFinalTerms(out string primaryTerm, out string secondaryTerm)
  {
    primaryTerm = string.Empty;
    secondaryTerm = string.Empty;
    if (!this.FindTarget())
      return;
    if ((UnityEngine.Object) this.mLocalizeTarget != (UnityEngine.Object) null)
    {
      this.mLocalizeTarget.GetFinalTerms(this, this.mTerm, this.mTermSecondary, out primaryTerm, out secondaryTerm);
      primaryTerm = I2Utils.GetValidTermName(primaryTerm);
    }
    if (!string.IsNullOrEmpty(this.mTerm))
      primaryTerm = this.mTerm;
    if (!string.IsNullOrEmpty(this.mTermSecondary))
      secondaryTerm = this.mTermSecondary;
    if (primaryTerm != null)
      primaryTerm = primaryTerm.Trim();
    if (secondaryTerm == null)
      return;
    secondaryTerm = secondaryTerm.Trim();
  }

  public string GetMainTargetsText()
  {
    string primaryTerm = (string) null;
    string secondaryTerm = (string) null;
    if ((UnityEngine.Object) this.mLocalizeTarget != (UnityEngine.Object) null)
      this.mLocalizeTarget.GetFinalTerms(this, (string) null, (string) null, out primaryTerm, out secondaryTerm);
    return !string.IsNullOrEmpty(primaryTerm) ? primaryTerm : this.mTerm;
  }

  public void SetFinalTerms(
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm,
    bool RemoveNonASCII)
  {
    primaryTerm = RemoveNonASCII ? I2Utils.GetValidTermName(Main) : Main;
    secondaryTerm = Secondary;
  }

  public void SetTerm(string primary)
  {
    if (!string.IsNullOrEmpty(primary))
      this.FinalTerm = this.mTerm = primary;
    this.OnLocalize(true);
  }

  public void SetTerm(string primary, string secondary)
  {
    if (!string.IsNullOrEmpty(primary))
      this.FinalTerm = this.mTerm = primary;
    this.FinalSecondaryTerm = this.mTermSecondary = secondary;
    this.OnLocalize(true);
  }

  public T GetSecondaryTranslatedObj<T>(ref string mainTranslation, ref string secondaryTranslation) where T : UnityEngine.Object
  {
    string str;
    string secondary;
    this.DeserializeTranslation(mainTranslation, out str, out secondary);
    T secondaryTranslatedObj = default (T);
    if (!string.IsNullOrEmpty(secondary))
    {
      secondaryTranslatedObj = this.GetObject<T>(secondary);
      if ((UnityEngine.Object) secondaryTranslatedObj != (UnityEngine.Object) null)
      {
        mainTranslation = str;
        secondaryTranslation = secondary;
      }
    }
    if ((UnityEngine.Object) secondaryTranslatedObj == (UnityEngine.Object) null)
      secondaryTranslatedObj = this.GetObject<T>(secondaryTranslation);
    return secondaryTranslatedObj;
  }

  public void UpdateAssetDictionary()
  {
    this.TranslatedObjects.RemoveAll((Predicate<UnityEngine.Object>) (x => x == (UnityEngine.Object) null));
    this.mAssetDictionary = this.TranslatedObjects.Distinct<UnityEngine.Object>().GroupBy<UnityEngine.Object, string>((Func<UnityEngine.Object, string>) (o => o.name)).ToDictionary<IGrouping<string, UnityEngine.Object>, string, UnityEngine.Object>((Func<IGrouping<string, UnityEngine.Object>, string>) (g => g.Key), (Func<IGrouping<string, UnityEngine.Object>, UnityEngine.Object>) (g => g.First<UnityEngine.Object>()));
  }

  public T GetObject<T>(string Translation) where T : UnityEngine.Object
  {
    return string.IsNullOrEmpty(Translation) ? default (T) : this.GetTranslatedObject<T>(Translation);
  }

  public T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
  {
    return this.FindTranslatedObject<T>(Translation);
  }

  public void DeserializeTranslation(string translation, out string value, out string secondary)
  {
    if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
    {
      int num = translation.IndexOf(']');
      if (num > 0)
      {
        secondary = translation.Substring(1, num - 1);
        value = translation.Substring(num + 1);
        return;
      }
    }
    value = translation;
    secondary = string.Empty;
  }

  public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
  {
    if (string.IsNullOrEmpty(value))
      return default (T);
    if (this.mAssetDictionary == null || this.mAssetDictionary.Count != this.TranslatedObjects.Count)
      this.UpdateAssetDictionary();
    foreach (KeyValuePair<string, UnityEngine.Object> mAsset in this.mAssetDictionary)
    {
      if (mAsset.Value is T && value.EndsWith(mAsset.Key, StringComparison.OrdinalIgnoreCase) && string.Compare(value, mAsset.Key, StringComparison.OrdinalIgnoreCase) == 0)
        return (T) mAsset.Value;
    }
    T asset = LocalizationManager.FindAsset(value) as T;
    if ((bool) (UnityEngine.Object) asset)
      return asset;
    T translatedObject = ResourceManager.pInstance.GetAsset<T>(value);
    if ((UnityEngine.Object) translatedObject == (UnityEngine.Object) null)
      translatedObject = !value.Contains("Fonts") ? ResourceManager.pInstance.LoadFromAddressables<T>(value) : ResourceManager.pInstance.LoadFromAddressables<T>(value, true);
    return translatedObject;
  }

  public bool HasTranslatedObject(UnityEngine.Object Obj)
  {
    return this.TranslatedObjects.Contains(Obj) || ResourceManager.pInstance.HasAsset(Obj);
  }

  public void AddTranslatedObject(UnityEngine.Object Obj)
  {
    if (this.TranslatedObjects.Contains(Obj))
      return;
    this.TranslatedObjects.Add(Obj);
    this.UpdateAssetDictionary();
  }

  public void SetGlobalLanguage(string Language) => LocalizationManager.CurrentLanguage = Language;

  public static string StripRichText(string input)
  {
    if (string.IsNullOrEmpty(input))
      return input;
    input = Regex.Replace(input, "<[^>]+>", string.Empty);
    input = Regex.Replace(input, ">[^<]+<", string.Empty);
    return input;
  }

  public enum TermModification
  {
    DontModify,
    ToUpper,
    ToLower,
    ToUpperFirst,
    ToTitle,
  }
}
