// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_TextMeshPro_Label
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_TextMeshPro_Label : LocalizeTarget<TextMeshPro>
{
  public TextAlignmentOptions mAlignment_RTL = TextAlignmentOptions.Right;
  public TextAlignmentOptions mAlignment_LTR = TextAlignmentOptions.Left;
  public bool mAlignmentWasRTL;
  public bool mInitializeAlignment = true;

  static LocalizeTarget_TextMeshPro_Label() => LocalizeTarget_TextMeshPro_Label.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
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
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    LocalizeTarget_TextMeshPro_Label.\u003C\u003Ec__DisplayClass12_0 cDisplayClass120 = new LocalizeTarget_TextMeshPro_Label.\u003C\u003Ec__DisplayClass12_0()
    {
      \u003C\u003E4__this = this,
      cmp = cmp,
      mainTranslation = mainTranslation,
      secondaryTranslation = secondaryTranslation
    };
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    // ISSUE: reference to a compiler-generated field
    cDisplayClass120.newFont = cDisplayClass120.cmp.GetSecondaryTranslatedObj<TMP_FontAsset>(ref cDisplayClass120.mainTranslation, ref cDisplayClass120.secondaryTranslation);
    if (LocalizationManager.CurrentLanguage == "English" && SettingsManager.Settings != null && SettingsManager.Settings.Accessibility.DyslexicFont)
    {
      if ((UnityEngine.Object) LocalizationManager.DyslexicFontAsset != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        cDisplayClass120.newFont = LocalizationManager.DyslexicFontAsset;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        LocalizationManager.CheckForDyslexicFonts(new Action<TMP_FontAsset>(cDisplayClass120.\u003CDoLocalize\u003Eb__0));
        return;
      }
    }
    // ISSUE: reference to a compiler-generated method
    cDisplayClass120.\u003CDoLocalize\u003Eg__SetNewFont\u007C1();
  }

  public static TMP_FontAsset GetTMPFontFromMaterial(Localize cmp, string matName)
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

  public static void InitAlignment_TMPro(
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

  public static void SetFont(TMP_Text label, TMP_FontAsset newFont)
  {
    if ((UnityEngine.Object) label.font != (UnityEngine.Object) newFont)
      label.font = newFont;
    if ((UnityEngine.Object) label.linkedTextComponent != (UnityEngine.Object) null)
      LocalizeTarget_TextMeshPro_Label.SetFont(label.linkedTextComponent, newFont);
    label.font.material.SetInt("unity_GUIZTestMode", 8);
  }

  public static void SetMaterial(TMP_Text label, Material newMat)
  {
    if ((UnityEngine.Object) label.fontSharedMaterial != (UnityEngine.Object) newMat)
      label.fontSharedMaterial = newMat;
    if (!((UnityEngine.Object) label.linkedTextComponent != (UnityEngine.Object) null))
      return;
    LocalizeTarget_TextMeshPro_Label.SetMaterial(label.linkedTextComponent, newMat);
  }
}
