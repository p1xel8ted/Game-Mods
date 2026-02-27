// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityUI_Text
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityUI_Text : LocalizeTarget<Text>
{
  private TextAnchor mAlignment_RTL = TextAnchor.UpperRight;
  private TextAnchor mAlignment_LTR;
  private bool mAlignmentWasRTL;
  private bool mInitializeAlignment = true;

  static LocalizeTarget_UnityUI_Text() => LocalizeTarget_UnityUI_Text.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void AutoRegister()
  {
    LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text> desc = new LocalizeTargetDesc_Type<Text, LocalizeTarget_UnityUI_Text>();
    desc.Name = "Text";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.Text;

  public override eTermType GetSecondaryTermType(Localize cmp) => eTermType.Font;

  public override bool CanUseSecondaryTerm() => true;

  public override bool AllowMainTermToBeRTL() => true;

  public override bool AllowSecondTermToBeRTL() => false;

  public override void GetFinalTerms(
    Localize cmp,
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm)
  {
    primaryTerm = (bool) (Object) this.mTarget ? this.mTarget.text : (string) null;
    secondaryTerm = (Object) this.mTarget.font != (Object) null ? this.mTarget.font.name : string.Empty;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
    if ((Object) secondaryTranslatedObj != (Object) null && (Object) secondaryTranslatedObj != (Object) this.mTarget.font)
      this.mTarget.font = secondaryTranslatedObj;
    if (this.mInitializeAlignment)
    {
      this.mInitializeAlignment = false;
      this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
      this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
    }
    else
    {
      TextAnchor alignLTR;
      TextAnchor alignRTL;
      this.InitAlignment(this.mAlignmentWasRTL, this.mTarget.alignment, out alignLTR, out alignRTL);
      if (this.mAlignmentWasRTL && this.mAlignment_RTL != alignRTL || !this.mAlignmentWasRTL && this.mAlignment_LTR != alignLTR)
      {
        this.mAlignment_LTR = alignLTR;
        this.mAlignment_RTL = alignRTL;
      }
      this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
    }
    if (mainTranslation == null || !(this.mTarget.text != mainTranslation))
      return;
    if (cmp.CorrectAlignmentForRTL)
      this.mTarget.alignment = LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR;
    this.mTarget.text = mainTranslation;
    this.mTarget.SetVerticesDirty();
  }

  private void InitAlignment(
    bool isRTL,
    TextAnchor alignment,
    out TextAnchor alignLTR,
    out TextAnchor alignRTL)
  {
    alignLTR = alignRTL = alignment;
    if (isRTL)
    {
      switch (alignment)
      {
        case TextAnchor.UpperLeft:
          alignLTR = TextAnchor.UpperRight;
          break;
        case TextAnchor.UpperRight:
          alignLTR = TextAnchor.UpperLeft;
          break;
        case TextAnchor.MiddleLeft:
          alignLTR = TextAnchor.MiddleRight;
          break;
        case TextAnchor.MiddleRight:
          alignLTR = TextAnchor.MiddleLeft;
          break;
        case TextAnchor.LowerLeft:
          alignLTR = TextAnchor.LowerRight;
          break;
        case TextAnchor.LowerRight:
          alignLTR = TextAnchor.LowerLeft;
          break;
      }
    }
    else
    {
      switch (alignment)
      {
        case TextAnchor.UpperLeft:
          alignRTL = TextAnchor.UpperRight;
          break;
        case TextAnchor.UpperRight:
          alignRTL = TextAnchor.UpperLeft;
          break;
        case TextAnchor.MiddleLeft:
          alignRTL = TextAnchor.MiddleRight;
          break;
        case TextAnchor.MiddleRight:
          alignRTL = TextAnchor.MiddleLeft;
          break;
        case TextAnchor.LowerLeft:
          alignRTL = TextAnchor.LowerRight;
          break;
        case TextAnchor.LowerRight:
          alignRTL = TextAnchor.LowerLeft;
          break;
      }
    }
  }
}
