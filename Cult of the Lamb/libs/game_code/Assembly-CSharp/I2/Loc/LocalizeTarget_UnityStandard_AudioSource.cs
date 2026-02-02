// Decompiled with JetBrains decompiler
// Type: I2.Loc.LocalizeTarget_UnityStandard_AudioSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace I2.Loc;

public class LocalizeTarget_UnityStandard_AudioSource : LocalizeTarget<AudioSource>
{
  static LocalizeTarget_UnityStandard_AudioSource()
  {
    LocalizeTarget_UnityStandard_AudioSource.AutoRegister();
  }

  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
  public static void AutoRegister()
  {
    LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource> desc = new LocalizeTargetDesc_Type<AudioSource, LocalizeTarget_UnityStandard_AudioSource>();
    desc.Name = "AudioSource";
    desc.Priority = 100;
    LocalizationManager.RegisterTarget((ILocalizeTargetDescriptor) desc);
  }

  public override eTermType GetPrimaryTermType(Localize cmp) => eTermType.AudioClip;

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
    AudioClip clip = this.mTarget.clip;
    primaryTerm = (bool) (Object) clip ? clip.name : string.Empty;
    secondaryTerm = (string) null;
  }

  public override void DoLocalize(
    Localize cmp,
    string mainTranslation,
    string secondaryTranslation)
  {
    int num = this.mTarget.isPlaying || this.mTarget.loop ? (Application.isPlaying ? 1 : 0) : 0;
    AudioClip clip = this.mTarget.clip;
    AudioClip translatedObject = cmp.FindTranslatedObject<AudioClip>(mainTranslation);
    AudioClip audioClip = translatedObject;
    if ((Object) clip != (Object) audioClip)
      this.mTarget.clip = translatedObject;
    if (num == 0 || !(bool) (Object) this.mTarget.clip)
      return;
    this.mTarget.Play();
  }
}
