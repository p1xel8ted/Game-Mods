// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_SpriteRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_SpriteRenderer : LocalizeTarget<SpriteRenderer>
{
  static LocalizeTarget_UnityStandard_SpriteRenderer()
  {
    LocalizeTarget_UnityStandard_SpriteRenderer.AutoRegister();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer> desc = new LocalizeTargetDesc_Type<SpriteRenderer, LocalizeTarget_UnityStandard_SpriteRenderer>();
    desc.Name = "SpriteRenderer";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.Sprite;

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
    Sprite sprite = this.mTarget.sprite;
    primaryTerm = (Object) sprite != (Object) null ? sprite.name : string.Empty;
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
