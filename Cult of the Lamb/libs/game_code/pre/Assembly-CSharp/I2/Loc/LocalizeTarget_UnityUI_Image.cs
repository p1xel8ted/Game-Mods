// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityUI_Image
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityUI_Image : LocalizeTarget<Image>
{
  static LocalizeTarget_UnityUI_Image() => LocalizeTarget_UnityUI_Image.AutoRegister();

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  private static void AutoRegister()
  {
    LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image> desc = new LocalizeTargetDesc_Type<Image, LocalizeTarget_UnityUI_Image>();
    desc.Name = "Image";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override bool CanUseSecondaryTerm() => false;

  public override bool AllowMainTermToBeRTL() => false;

  public override bool AllowSecondTermToBeRTL() => false;

  public override eTermType GetPrimaryTermType(Localize cmp)
  {
    return !((Object) this.mTarget.sprite == (Object) null) ? eTermType.Sprite : eTermType.Texture;
  }

  public override eTermType GetSecondaryTermType(Localize cmp) => eTermType.Text;

  public override void GetFinalTerms(
    Localize cmp,
    string Main,
    string Secondary,
    out string primaryTerm,
    out string secondaryTerm)
  {
    primaryTerm = (bool) (Object) this.mTarget.mainTexture ? this.mTarget.mainTexture.name : "";
    if ((Object) this.mTarget.sprite != (Object) null && this.mTarget.sprite.name != primaryTerm)
      primaryTerm = $"{primaryTerm}.{this.mTarget.sprite.name}";
    secondaryTerm = (string) null;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    Sprite sprite = this.mTarget.sprite;
    if (!((Object) sprite == (Object) null) && !(sprite.name != mainTranslation))
      return;
    this.mTarget.sprite = cmp.FindTranslatedObject<Sprite>(mainTranslation);
  }
}
