// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioUtil
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class AudioUtil
{
  public const float DefaultMinOcclusionCutoffFrequency = 22000f;
  public const float DefaultMaxOcclusionCutoffFrequency = 0.0f;
  public const float SemitonePitchChangeAmt = 1.0594635f;

  public static float CutoffRange(SoundGroupVariationUpdater updater)
  {
    return updater.MinOcclusionFreq - updater.MaxOcclusionFreq;
  }

  public static float MaxCutoffFreq(SoundGroupVariationUpdater updater) => updater.MaxOcclusionFreq;

  public static float MinCutoffFreq(SoundGroupVariationUpdater updater) => updater.MinOcclusionFreq;

  public static float FixedDeltaTime => UnityEngine.Time.fixedDeltaTime;

  public static float FrameTime => UnityEngine.Time.unscaledDeltaTime;

  public static float Time => UnityEngine.Time.unscaledTime;

  public static int FrameCount => UnityEngine.Time.frameCount;

  public static float GetOcclusionCutoffFrequencyByDistanceRatio(
    float distRatio,
    SoundGroupVariationUpdater updater)
  {
    return AudioUtil.MaxCutoffFreq(updater) + distRatio * AudioUtil.CutoffRange(updater);
  }

  public static float GetSemitonesFromPitch(float pitch)
  {
    return (double) pitch >= 1.0 || (double) pitch <= 0.0 ? Mathf.Log(pitch, 1.0594635f) : Mathf.Log(1f / pitch, 1.0594635f) * -1f;
  }

  public static float GetPitchFromSemitones(float semitones)
  {
    return (double) semitones >= 0.0 ? Mathf.Pow(1.0594635f, semitones) : 1f / Mathf.Pow(1.0594635f, Mathf.Abs(semitones));
  }

  public static float GetDbFromFloatVolume(float vol) => Mathf.Log10(vol) * 20f;

  public static float GetFloatVolumeFromDb(float db) => Mathf.Pow(10f, db / 20f);

  public static float GetAudioPlayedPercentage(AudioSource source)
  {
    return (Object) source.clip == (Object) null || (double) source.time == 0.0 ? 0.0f : (float) ((double) source.time / (double) source.clip.length * 100.0);
  }

  public static bool IsAudioPaused(AudioSource source)
  {
    return !source.isPlaying && (double) AudioUtil.GetAudioPlayedPercentage(source) > 0.0;
  }

  public static void ClipPlayed(AudioClip clip, GameObject actor)
  {
    if (AudioUtil.AudioClipWillPreload(clip))
      return;
    AudioLoaderOptimizer.AddNonPreloadedPlayingClip(clip, actor);
  }

  public static void UnloadNonPreloadedAudioData(AudioClip clip, GameObject actor)
  {
    if ((Object) clip == (Object) null || AudioUtil.AudioClipWillPreload(clip))
      return;
    AudioLoaderOptimizer.RemoveNonPreloadedPlayingClip(clip, actor);
    if (AudioLoaderOptimizer.IsAnyOfNonPreloadedClipPlaying(clip))
      return;
    clip.UnloadAudioData();
  }

  public static bool AudioClipWillPreload(AudioClip clip)
  {
    return !((Object) clip == (Object) null) && clip.preloadAudioData;
  }

  public static bool IsClipReadyToPlay(this AudioClip clip)
  {
    return (Object) clip != (Object) null && clip.loadType != AudioClipLoadType.Streaming;
  }

  public static float GetPositiveUsablePitch(AudioSource source)
  {
    return AudioUtil.GetPositiveUsablePitch(source.pitch);
  }

  public static float GetPositiveUsablePitch(float pitch) => (double) pitch <= 0.0 ? 1f : pitch;

  public static float AdjustAudioClipDurationForPitch(float duration, AudioSource sourceWithPitch)
  {
    return AudioUtil.AdjustAudioClipDurationForPitch(duration, sourceWithPitch.pitch);
  }

  public static float AdjustAudioClipDurationForPitch(float duration, float pitch)
  {
    return duration / AudioUtil.GetPositiveUsablePitch(pitch);
  }

  public static float AdjustEndLeadTimeForPitch(float duration, AudioSource sourceWithPitch)
  {
    return duration * AudioUtil.GetPositiveUsablePitch(sourceWithPitch);
  }
}
