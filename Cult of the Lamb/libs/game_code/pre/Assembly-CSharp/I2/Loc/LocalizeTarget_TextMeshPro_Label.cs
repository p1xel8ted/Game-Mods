// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_TextMeshPro_Label
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
{
  private TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;
  private TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;
  private bool mAlignmentWasRTL;
  private bool mInitializeAlignment = true;

  static LocalizeTarget_TextMeshPro_Label() => LocalizeTarget_TextMeshPro_Label.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void AutoRegister()
  {
    LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label> desc = new LocalizeTargetDesc_Type<TextMeshPro, LocalizeTarget_TextMeshPro_Label>();
    desc.Name = "TextMeshPro Label";
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

  internal static TMP_FontAsset GetTMPFontFromMaterial(Localize cmp, string matName)
  {
    string str = " .\\/-[]()";
    int index = matName.Length - 1;
label_8:
    while (index > 0)
    {
      while (index > 0 && str.IndexOf(matName[index]) >= 0)
        --index;
      if (index > 0)
      {
        string Translation = matName.Substring(0, index + 1);
        TMP_FontAsset fontFromMaterial = cmp.GetObject<TMP_FontAsset>(Translation);
        if ((UnityEngine.Object) fontFromMaterial != (UnityEngine.Object) null)
          return fontFromMaterial;
        while (true)
        {
          if (index > 0 && str.IndexOf(matName[index]) < 0)
            --index;
          else
            goto label_8;
        }
      }
      else
        break;
    }
    return (TMP_FontAsset) null;
  }

  internal static void InitAlignment_TMPro(
    bool isRTL,
    TextAlignmentOptions alignment,
    out TextAlignmentOptions alignLTR,
    out TextAlignmentOptions alignRTL)
  {
    alignLTR = alignRTL = alignment;
    if (isRTL)
    {
      switch (alignment)
      {
        case TextAlignmentOptions.TopLeft:
          alignLTR = TextAlignmentOptions.TopRight;
          break;
        case TextAlignmentOptions.TopRight:
          alignLTR = TextAlignmentOptions.TopLeft;
          break;
        case TextAlignmentOptions.Left:
          alignLTR = TextAlignmentOptions.Right;
          break;
        case TextAlignmentOptions.Right:
          alignLTR = TextAlignmentOptions.Left;
          break;
        case TextAlignmentOptions.BottomLeft:
          alignLTR = TextAlignmentOptions.BottomRight;
          break;
        case TextAlignmentOptions.BottomRight:
          alignLTR = TextAlignmentOptions.BottomLeft;
          break;
        case TextAlignmentOptions.BaselineLeft:
          alignLTR = TextAlignmentOptions.BaselineRight;
          break;
        case TextAlignmentOptions.BaselineRight:
          alignLTR = TextAlignmentOptions.BaselineLeft;
          break;
        case TextAlignmentOptions.MidlineLeft:
          alignLTR = TextAlignmentOptions.MidlineRight;
          break;
        case TextAlignmentOptions.MidlineRight:
          alignLTR = TextAlignmentOptions.MidlineLeft;
          break;
        case TextAlignmentOptions.CaplineLeft:
          alignLTR = TextAlignmentOptions.CaplineRight;
          break;
        case TextAlignmentOptions.CaplineRight:
          alignLTR = TextAlignmentOptions.CaplineLeft;
          break;
      }
    }
    else
    {
      switch (alignment)
      {
        case TextAlignmentOptions.TopLeft:
          alignRTL = TextAlignmentOptions.TopRight;
          break;
        case TextAlignmentOptions.TopRight:
          alignRTL = TextAlignmentOptions.TopLeft;
          break;
        case TextAlignmentOptions.Left:
          alignRTL = TextAlignmentOptions.Right;
          break;
        case TextAlignmentOptions.Right:
          alignRTL = TextAlignmentOptions.Left;
          break;
        case TextAlignmentOptions.BottomLeft:
          alignRTL = TextAlignmentOptions.BottomRight;
          break;
        case TextAlignmentOptions.BottomRight:
          alignRTL = TextAlignmentOptions.BottomLeft;
          break;
        case TextAlignmentOptions.BaselineLeft:
          alignRTL = TextAlignmentOptions.BaselineRight;
          break;
        case TextAlignmentOptions.BaselineRight:
          alignRTL = TextAlignmentOptions.BaselineLeft;
          break;
        case TextAlignmentOptions.MidlineLeft:
          alignRTL = TextAlignmentOptions.MidlineRight;
          break;
        case TextAlignmentOptions.MidlineRight:
          alignRTL = TextAlignmentOptions.MidlineLeft;
          break;
        case TextAlignmentOptions.CaplineLeft:
          alignRTL = TextAlignmentOptions.CaplineRight;
          break;
        case TextAlignmentOptions.CaplineRight:
          alignRTL = TextAlignmentOptions.CaplineLeft;
          break;
      }
    }
  }

  internal static void SetFont(TMP_Text label, TMP_FontAsset newFont)
  {
    if ((UnityEngine.Object) label.font != (UnityEngine.Object) newFont)
      label.font = newFont;
    if (!((UnityEngine.Object) label.linkedTextComponent != (UnityEngine.Object) null))
      return;
    LocalizeTarget_TextMeshPro_Label.SetFont(label.linkedTextComponent, newFont);
  }

  internal static void SetMaterial(TMP_Text label, Material newMat)
  {
    if ((UnityEngine.Object) label.fontSharedMaterial != (UnityEngine.Object) newMat)
      label.fontSharedMaterial = newMat;
    if (!((UnityEngine.Object) label.linkedTextComponent != (UnityEngine.Object) null))
      return;
    LocalizeTarget_TextMeshPro_Label.SetMaterial(label.linkedTextComponent, newMat);
  }
}
