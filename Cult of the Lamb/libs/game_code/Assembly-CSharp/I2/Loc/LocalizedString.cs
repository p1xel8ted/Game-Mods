// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizedString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace I2.Loc;

[Serializable]
public struct LocalizedString
{
  public string mTerm;
  public bool mRTL_IgnoreArabicFix;
  public int mRTL_MaxLineLength;
  public bool mRTL_ConvertNumbers;
  public bool m_DontLocalizeParameters;

  public static implicit operator string(LocalizedString s) => s.ToString();

  public static implicit operator LocalizedString(string term)
  {
    return new LocalizedString() { mTerm = term };
  }

  public LocalizedString(LocalizedString str)
  {
    this.mTerm = str.mTerm;
    this.mRTL_IgnoreArabicFix = str.mRTL_IgnoreArabicFix;
    this.mRTL_MaxLineLength = str.mRTL_MaxLineLength;
    this.mRTL_ConvertNumbers = str.mRTL_ConvertNumbers;
    this.m_DontLocalizeParameters = str.m_DontLocalizeParameters;
  }

  public override string ToString()
  {
    string translation = LocalizationManager.GetTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers, true);
    LocalizationManager.ApplyLocalizationParams(ref translation, !this.m_DontLocalizeParameters);
    return translation;
  }
}
