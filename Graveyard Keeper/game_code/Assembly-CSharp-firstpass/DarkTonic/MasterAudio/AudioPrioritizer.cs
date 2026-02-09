// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioPrioritizer
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class AudioPrioritizer
{
  public const int MaxPriority = 0;
  public const int HighestPriority = 16 /*0x10*/;
  public const int LowestPriority = 128 /*0x80*/;

  public static void Set2DSoundPriority(AudioSource audio) => audio.priority = 16 /*0x10*/;

  public static void SetSoundGroupInitialPriority(AudioSource audio)
  {
    audio.priority = 128 /*0x80*/;
  }

  public static void SetPreviewPriority(AudioSource audio) => audio.priority = 0;

  public static void Set3DPriority(SoundGroupVariation variation, bool useClipAgePriority)
  {
    if ((Object) DarkTonic.MasterAudio.MasterAudio.ListenerTrans == (Object) null)
      return;
    AudioSource varAudio = variation.VarAudio;
    if ((double) varAudio.spatialBlend == 0.0)
    {
      AudioPrioritizer.Set2DSoundPriority(variation.VarAudio);
    }
    else
    {
      float num = Vector3.Distance(varAudio.transform.position, DarkTonic.MasterAudio.MasterAudio.ListenerTrans.position);
      float a;
      switch (varAudio.rolloffMode)
      {
        case AudioRolloffMode.Logarithmic:
          a = varAudio.volume / Mathf.Max(varAudio.minDistance, num - varAudio.minDistance);
          break;
        case AudioRolloffMode.Linear:
          a = Mathf.Lerp(varAudio.volume, 0.0f, Mathf.Max(0.0f, num - varAudio.minDistance) / (varAudio.maxDistance - varAudio.minDistance));
          break;
        default:
          a = Mathf.Lerp(varAudio.volume, 0.0f, Mathf.Max(0.0f, num - varAudio.minDistance) / (varAudio.maxDistance - varAudio.minDistance));
          break;
      }
      if (useClipAgePriority && !varAudio.loop)
        a = Mathf.Lerp(a, a * 0.1f, AudioUtil.GetAudioPlayedPercentage(varAudio) * 0.01f);
      varAudio.priority = (int) Mathf.Lerp(16f, 128f, Mathf.InverseLerp(1f, 0.0f, a));
    }
  }
}
