// Decompiled with JetBrains decompiler
// Type: DarkTonic.MasterAudio.AudioTransformExtensions
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace DarkTonic.MasterAudio;

public static class AudioTransformExtensions
{
  public static void FadeOutSoundGroupOfTransform(
    this Transform sourceTrans,
    string sType,
    float fadeTime)
  {
    DarkTonic.MasterAudio.MasterAudio.FadeOutSoundGroupOfTransform(sourceTrans, sType, fadeTime);
  }

  public static List<SoundGroupVariation> GetAllPlayingVariationsOfTransform(
    this Transform sourceTrans)
  {
    return DarkTonic.MasterAudio.MasterAudio.GetAllPlayingVariationsOfTransform(sourceTrans);
  }

  public static bool PlaySound3DAtTransformAndForget(
    this Transform sourceTrans,
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null)
  {
    return DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndForget(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName);
  }

  public static PlaySoundResult PlaySound3DAtTransform(
    this Transform sourceTrans,
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null)
  {
    return DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransform(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName);
  }

  public static bool PlaySound3DFollowTransformAndForget(
    this Transform sourceTrans,
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null)
  {
    return DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndForget(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName);
  }

  public static PlaySoundResult PlaySound3DFollowTransform(
    this Transform sourceTrans,
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null)
  {
    return DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransform(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName);
  }

  public static IEnumerator PlaySound3DAtTransformAndWaitUntilFinished(
    this Transform sourceTrans,
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    Action completedAction = null)
  {
    return DarkTonic.MasterAudio.MasterAudio.PlaySound3DAtTransformAndWaitUntilFinished(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName, timeToSchedulePlay, completedAction);
  }

  public static IEnumerator PlaySound3DFollowTransformAndWaitUntilFinished(
    this Transform sourceTrans,
    string sType,
    float volumePercentage = 1f,
    float? pitch = null,
    float delaySoundTime = 0.0f,
    string variationName = null,
    double? timeToSchedulePlay = null,
    Action completedAction = null)
  {
    return DarkTonic.MasterAudio.MasterAudio.PlaySound3DFollowTransformAndWaitUntilFinished(sType, sourceTrans, volumePercentage, pitch, delaySoundTime, variationName, timeToSchedulePlay, completedAction);
  }

  public static void PauseAllSoundsOfTransform(Transform sourceTrans)
  {
    DarkTonic.MasterAudio.MasterAudio.PauseAllSoundsOfTransform(sourceTrans);
  }

  public static void PauseBusOfTransform(this Transform sourceTrans, string busName)
  {
    DarkTonic.MasterAudio.MasterAudio.PauseBusOfTransform(sourceTrans, busName);
  }

  public static void PauseSoundGroupOfTransform(this Transform sourceTrans, string sType)
  {
    DarkTonic.MasterAudio.MasterAudio.PauseSoundGroupOfTransform(sourceTrans, sType);
  }

  public static void StopAllSoundsOfTransform(this Transform sourceTrans)
  {
    DarkTonic.MasterAudio.MasterAudio.StopAllSoundsOfTransform(sourceTrans);
  }

  public static void StopBusOfTransform(this Transform sourceTrans, string busName)
  {
    DarkTonic.MasterAudio.MasterAudio.StopBusOfTransform(sourceTrans, busName);
  }

  public static void StopSoundGroupOfTransform(this Transform sourceTrans, string sType)
  {
    DarkTonic.MasterAudio.MasterAudio.StopSoundGroupOfTransform(sourceTrans, sType);
  }

  public static void UnpauseAllSoundsOfTransform(this Transform sourceTrans)
  {
    DarkTonic.MasterAudio.MasterAudio.UnpauseAllSoundsOfTransform(sourceTrans);
  }

  public static void UnpauseBusOfTransform(this Transform sourceTrans, string busName)
  {
    DarkTonic.MasterAudio.MasterAudio.UnpauseBusOfTransform(sourceTrans, busName);
  }

  public static void UnpauseSoundGroupOfTransform(this Transform sourceTrans, string sType)
  {
    DarkTonic.MasterAudio.MasterAudio.UnpauseSoundGroupOfTransform(sourceTrans, sType);
  }

  public static bool IsTransformPlayingSoundGroup(this Transform sourceTrans, string sType)
  {
    return DarkTonic.MasterAudio.MasterAudio.IsTransformPlayingSoundGroup(sType, sourceTrans);
  }
}
