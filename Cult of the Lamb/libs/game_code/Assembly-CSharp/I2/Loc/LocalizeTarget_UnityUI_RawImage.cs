// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityUI_RawImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityUI_RawImage : LocalizeTarget<RawImage>
{
  static LocalizeTarget_UnityUI_RawImage() => LocalizeTarget_UnityUI_RawImage.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage> desc = new LocalizeTargetDesc_Type<RawImage, LocalizeTarget_UnityUI_RawImage>();
    desc.Name = "RawImage";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.Texture;

  public override eTermType GetSecondaryTermType(Localize cmp) => eTermType.Text;

  public override bool CanUseSecondaryTerm() => false;

  public override bool AllowMainTermToBeRTL() => false;

  public override bool AllowSecondTermToBeRTL() => false;

  public override void GetFinalTerms(
    Localize cmp,
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm)
  {
    primaryTerm = (bool) (Object) this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "";
    secondaryTerm = (string) null;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    Texture texture = this.mTarget.texture;
    if (!((Object) texture == (Object) null) && !(texture.name != mainTranslation))
      return;
    this.mTarget.texture = cmp.FindTranslatedObject<Texture>(mainTranslation);
  }
}
