// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_VideoPlayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Video;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_VideoPlayer : LocalizeTarget<VideoPlayer>
{
  static LocalizeTarget_UnityStandard_VideoPlayer()
  {
    LocalizeTarget_UnityStandard_VideoPlayer.AutoRegister();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Type<VideoPlayer, LocalizeTarget_UnityStandard_VideoPlayer> desc = new LocalizeTargetDesc_Type<VideoPlayer, LocalizeTarget_UnityStandard_VideoPlayer>();
    desc.Name = "VideoPlayer";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.Video;

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
    VideoClip clip = this.mTarget.clip;
    primaryTerm = (Object) clip != (Object) null ? clip.name : string.Empty;
    secondaryTerm = (string) null;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    VideoClip clip = this.mTarget.clip;
    if (!((Object) clip == (Object) null) && !(clip.name != mainTranslation))
      return;
    this.mTarget.clip = cmp.FindTranslatedObject<VideoClip>(mainTranslation);
  }
}
