// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizedString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
