// Decompiled with JetBrains decompiler
// Type: LanguageModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using TMPro;
using Unify;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class LanguageModifier : MonoBehaviour
{
  public LanguageModifier Parent;
  public string Token;
  public UnifyManager.Platform[] Platforms;
  public string EnglishText;
  public string GermanText;
  public string FrenchText;
  public string SpanishText;
  public string RussianText;
  public string SChineseText;
  public string TChineseText;
  public string JapaneseText;
  public string KoreanText;
  public string PortugueseText;
  public string BrazilianPortugueseText;
  public string TurkishText;
  public string ItalianText;
  public string ArabicText;
  public Text text;
  public TextMeshProUGUI text_TMP;
  public UnifyManager unifyManager;

  public void OnEnable()
  {
    if (this.unifyManager == null)
    {
      this.unifyManager = UnifyManager.Get();
      if (this.unifyManager != null)
        this.unifyManager.OnPlatformDetailsChanged += new UnifyManager.PlatformDetailsChanged(this.OnPlatformDetailsChanged);
    }
    this.Process();
  }

  public void OnDestroy()
  {
    if (this.unifyManager == null)
      return;
    this.unifyManager.OnPlatformDetailsChanged -= new UnifyManager.PlatformDetailsChanged(this.OnPlatformDetailsChanged);
  }

  public void OnPlatformDetailsChanged() => this.Process();

  public string Replace(string source, string replacement, string token = null)
  {
    return token == null || token.Length <= 0 ? replacement : source.Replace(token, replacement);
  }

  public void Process()
  {
    if (this.Platforms.Length != 0)
    {
      bool flag = false;
      foreach (UnifyManager.Platform platform in this.Platforms)
      {
        if (platform == UnifyManager.platform)
          flag = true;
      }
      if (!flag)
        return;
    }
    if ((Object) this.Parent != (Object) null)
      this.Parent.Process();
    string text;
    try
    {
      this.text = this.GetComponent<Text>();
      text = this.text.text;
    }
    catch
    {
      this.text_TMP = this.GetComponent<TextMeshProUGUI>();
      text = this.text_TMP.text;
    }
    string replacement = this.EnglishText;
    switch (LocalizationManager.CurrentLanguage)
    {
      case "Arabic":
        replacement = this.ArabicText;
        break;
      case "Chinese (Simplified)":
        replacement = this.SChineseText;
        break;
      case "Chinese (Traditional)":
        replacement = this.TChineseText;
        break;
      case "English":
        replacement = this.EnglishText;
        break;
      case "French":
        replacement = this.FrenchText;
        break;
      case "German":
        replacement = this.GermanText;
        break;
      case "Italian":
        replacement = this.ItalianText;
        break;
      case "Japanese":
        replacement = this.JapaneseText;
        break;
      case "Korean":
        replacement = this.KoreanText;
        break;
      case "Portuguese":
      case "Portuguese (Brazil)":
        replacement = this.BrazilianPortugueseText;
        break;
      case "Russian":
        replacement = this.RussianText;
        break;
      case "Spanish":
        replacement = this.SpanishText;
        break;
      case "Turkish":
        replacement = this.TurkishText;
        break;
    }
    if (replacement == null || replacement.Length <= 0)
      replacement = this.EnglishText;
    if ((Object) this.text != (Object) null)
      this.text.text = this.Replace(text, replacement, this.Token);
    else
      this.text_TMP.text = this.Replace(text, replacement, this.Token);
  }
}
