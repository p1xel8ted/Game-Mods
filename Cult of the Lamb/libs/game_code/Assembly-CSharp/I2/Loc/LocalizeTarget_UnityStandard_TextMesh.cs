// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_TextMesh
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_TextMesh : LocalizeTarget<TextMesh>
{
  public TextAlignment mAlignment_RTL = TextAlignment.Right;
  public TextAlignment mAlignment_LTR;
  public bool mAlignmentWasRTL;
  public bool mInitializeAlignment = true;

  static LocalizeTarget_UnityStandard_TextMesh()
  {
    LocalizeTarget_UnityStandard_TextMesh.AutoRegister();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh> desc = new LocalizeTargetDesc_Type<TextMesh, LocalizeTarget_UnityStandard_TextMesh>();
    desc.Name = "TextMesh";
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
    secondaryTerm = !string.IsNullOrEmpty(Secondary) || !((Object) this.mTarget.font != (Object) null) ? (string) null : this.mTarget.font.name;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    Font secondaryTranslatedObj = cmp.GetSecondaryTranslatedObj<Font>(ref mainTranslation, ref secondaryTranslation);
    if ((Object) secondaryTranslatedObj != (Object) null && (Object) this.mTarget.font != (Object) secondaryTranslatedObj)
    {
      this.mTarget.font = secondaryTranslatedObj;
      this.mTarget.GetComponentInChildren<MeshRenderer>().material = secondaryTranslatedObj.material;
    }
    if (this.mInitializeAlignment)
    {
      this.mInitializeAlignment = false;
      this.mAlignment_LTR = this.mAlignment_RTL = this.mTarget.alignment;
      if (LocalizationManager.IsRight2Left && this.mAlignment_RTL == TextAlignment.Right)
        this.mAlignment_LTR = TextAlignment.Left;
      if (!LocalizationManager.IsRight2Left && this.mAlignment_LTR == TextAlignment.Left)
        this.mAlignment_RTL = TextAlignment.Right;
    }
    if (mainTranslation == null || !(this.mTarget.text != mainTranslation))
      return;
    if (cmp.CorrectAlignmentForRTL && this.mTarget.alignment != TextAlignment.Center)
      this.mTarget.alignment = LocalizationManager.IsRight2Left ? this.mAlignment_RTL : this.mAlignment_LTR;
    this.mTarget.font.RequestCharactersInTexture(mainTranslation);
    this.mTarget.text = mainTranslation;
  }
}
