// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_TextMeshPro_UGUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_TextMeshPro_UGUI : LocalizeTarget<TextMeshProUGUI>
{
  public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;
  public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;
  public bool mAlignmentWasRTL;
  public bool mInitializeAlignment = true;

  static LocalizeTarget_TextMeshPro_UGUI() => LocalizeTarget_TextMeshPro_UGUI.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void AutoRegister()
  {
    LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI> desc = new LocalizeTargetDesc_Type<TextMeshProUGUI, LocalizeTarget_TextMeshPro_UGUI>();
    desc.Name = "TextMeshPro UGUI";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.Text;

  public override eTermType GetSecondaryTermType(Localize cmp) => eTermType.TextMeshPFont;

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
    primaryTerm = (bool) (UnityEngine.Object) this.mTarget ? this.mTarget.text : (string) null;
    secondaryTerm = (UnityEngine.Object) this.mTarget.font != (UnityEngine.Object) null ? this.mTarget.font.name : string.Empty;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    TMP_FontAsset secondaryTranslatedObj1 = cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref mainTranslation, ref secondaryTranslation);
    if ((UnityEngine.Object) secondaryTranslatedObj1 != (UnityEngine.Object) null)
    {
      LocalizeTarget_TextMeshPro_Label.SetFont((TMP_Text) this.mTarget, secondaryTranslatedObj1);
    }
    else
    {
      Material secondaryTranslatedObj2 = cmp.GetSecondaryTranslatedObj<Material>(ref mainTranslation, ref secondaryTranslation);
      if ((UnityEngine.Object) secondaryTranslatedObj2 != (UnityEngine.Object) null && (UnityEngine.Object) this.mTarget.fontMaterial != (UnityEngine.Object) secondaryTranslatedObj2)
      {
        if (!secondaryTranslatedObj2.name.StartsWith(this.mTarget.font.name, StringComparison.Ordinal))
        {
          TMP_FontAsset fontFromMaterial = LocalizeTarget_TextMeshPro_Label.GetTMPFontFromMaterial(cmp, secondaryTranslation.EndsWith(secondaryTranslatedObj2.name, StringComparison.Ordinal) ? secondaryTranslation : secondaryTranslatedObj2.name);
          if ((UnityEngine.Object) fontFromMaterial != (UnityEngine.Object) null)
            LocalizeTarget_TextMeshPro_Label.SetFont((TMP_Text) this.mTarget, fontFromMaterial);
        }
        LocalizeTarget_TextMeshPro_Label.SetMaterial((TMP_Text) this.mTarget, secondaryTranslatedObj2);
      }
    }
    if (this.mInitializeAlignment)
    {
      this.mInitializeAlignment = false;
      this.mAlignmentWasRTL = LocalizationManager.IsRight2Left;
      LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out this.mAlignment_LTR, out this.mAlignment_RTL);
    }
    else
    {
      TextAlignmentOptions alignLTR;
      TextAlignmentOptions alignRTL;
      LocalizeTarget_TextMeshPro_Label.InitAlignment_TMPro(this.mAlignmentWasRTL, this.mTarget.alignment, out alignLTR, out alignRTL);
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
    this.mTarget.isRightToLeftText = LocalizationManager.IsRight2Left;
    if (LocalizationManager.IsRight2Left)
      mainTranslation = I2Utils.ReverseText(mainTranslation);
    this.mTarget.text = mainTranslation;
  }
}
