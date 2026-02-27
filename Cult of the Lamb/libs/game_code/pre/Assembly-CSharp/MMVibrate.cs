// Decompiled with JetBrains decompiler
// Type: MMVibrate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using Unify.Input;
using UnityEngine;

#nullable disable
public static class MMVibrate
{
  public static long LightDuration = 20;
  public static long MediumDuration = 40;
  public static long HeavyDuration = 80 /*0x50*/;
  public static long RigidDuration = 20;
  public static long SoftDuration = 80 /*0x50*/;
  public static int LightAmplitude = 40;
  public static int MediumAmplitude = 120;
  public static int HeavyAmplitude = (int) byte.MaxValue;
  public static int RigidAmplitude = (int) byte.MaxValue;
  public static int SoftAmplitude = 40;
  private static bool _vibrationsActive = true;
  private static bool _debugLogActive = false;
  private static bool _hapticsPlayedOnce = false;
  private static float _vibrationIntensity = 1f;
  private static long[] _rigidImpactPattern = new long[2]
  {
    0L,
    MMVibrate.RigidDuration
  };
  private static int[] _rigidImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.RigidAmplitude
  };
  private static long[] _softImpactPattern = new long[2]
  {
    0L,
    MMVibrate.SoftDuration
  };
  private static int[] _softImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.SoftAmplitude
  };
  private static long[] _lightImpactPattern = new long[2]
  {
    0L,
    MMVibrate.LightDuration
  };
  private static int[] _lightImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.LightAmplitude
  };
  private static long[] _mediumImpactPattern = new long[2]
  {
    0L,
    MMVibrate.MediumDuration
  };
  private static int[] _mediumImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.MediumAmplitude
  };
  private static long[] _HeavyImpactPattern = new long[2]
  {
    0L,
    MMVibrate.HeavyDuration
  };
  private static int[] _HeavyImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.HeavyAmplitude
  };
  private static long[] _successPattern = new long[4]
  {
    0L,
    MMVibrate.LightDuration,
    MMVibrate.LightDuration,
    MMVibrate.HeavyDuration
  };
  private static int[] _successPatternAmplitude = new int[4]
  {
    0,
    MMVibrate.LightAmplitude,
    0,
    MMVibrate.HeavyAmplitude
  };
  private static long[] _warningPattern = new long[4]
  {
    0L,
    MMVibrate.HeavyDuration,
    MMVibrate.LightDuration,
    MMVibrate.MediumDuration
  };
  private static int[] _warningPatternAmplitude = new int[4]
  {
    0,
    MMVibrate.HeavyAmplitude,
    0,
    MMVibrate.MediumAmplitude
  };
  private static long[] _failurePattern = new long[8]
  {
    0L,
    MMVibrate.MediumDuration,
    MMVibrate.LightDuration,
    MMVibrate.MediumDuration,
    MMVibrate.LightDuration,
    MMVibrate.HeavyDuration,
    MMVibrate.LightDuration,
    MMVibrate.LightDuration
  };
  private static int[] _failurePatternAmplitude = new int[8]
  {
    0,
    MMVibrate.MediumAmplitude,
    0,
    MMVibrate.MediumAmplitude,
    0,
    MMVibrate.HeavyAmplitude,
    0,
    MMVibrate.LightAmplitude
  };
  private static Vector3 _rumbleRigid = new Vector3(0.5f, 1f, 0.08f);
  private static Vector3 _rumbleSoft = new Vector3(1f, 0.03f, 0.1f);
  private static Vector3 _rumbleLight = new Vector3(0.5f, 0.5f, 0.02f);
  private static Vector3 _rumbleMedium = new Vector3(0.8f, 0.8f, 0.04f);
  private static Vector3 _rumbleHeavy = new Vector3(1f, 1f, 0.08f);
  private static Vector3 _rumbleSuccess = new Vector3(1f, 1f, 1f);
  private static Vector3 _rumbleWarning = new Vector3(1f, 1f, 1f);
  private static Vector3 _rumbleFailure = new Vector3(1f, 1f, 1f);
  private static Vector3 _rumbleSelection = new Vector3(1f, 1f, 1f);
  public static bool Rumbling = false;
  public static bool RumblingContinuous = false;

  public static Player player
  {
    get => RewiredInputManager.MainPlayer;
    set => MMVibrate.player = value;
  }

  public static void SetHapticsActive(bool status)
  {
    Debug.Log((object) ("[MMVibrate] Set haptics active : " + status.ToString()));
    MMVibrate._vibrationsActive = status;
    if (status || MMVibrate.player == null)
      return;
    foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
      joystick.StopVibration();
  }

  public static void SetHapticsIntensity(float Intensity)
  {
    Debug.Log((object) ("[MMVibrate] Intensity Set to : " + (object) Intensity));
    MMVibrate._vibrationIntensity = Intensity;
    MMVibrate.SetHapticsActive((double) Intensity > 0.0);
  }

  public static void Haptic(
    MMVibrate.HapticTypes type,
    bool defaultToRegularVibrate = false,
    bool alsoRumble = false,
    MonoBehaviour coroutineSupport = null,
    int controllerID = -1)
  {
    if (!MMVibrate._vibrationsActive || !InputManager.General.InputIsController() || !alsoRumble || !((Object) coroutineSupport != (Object) null))
      return;
    switch (type)
    {
      case MMVibrate.HapticTypes.Selection:
        MMVibrate.Rumble(MMVibrate._rumbleLight.x, MMVibrate._rumbleMedium.y, MMVibrate._rumbleLight.z, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.Success:
        MMVibrate.Rumble(MMVibrate._successPattern, MMVibrate._successPatternAmplitude, -1, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.Warning:
        MMVibrate.Rumble(MMVibrate._warningPattern, MMVibrate._warningPatternAmplitude, -1, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.Failure:
        MMVibrate.Rumble(MMVibrate._failurePattern, MMVibrate._failurePatternAmplitude, -1, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.LightImpact:
        MMVibrate.Rumble(MMVibrate._rumbleLight.x, MMVibrate._rumbleLight.y, MMVibrate._rumbleLight.z, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.MediumImpact:
        MMVibrate.Rumble(MMVibrate._rumbleMedium.x, MMVibrate._rumbleMedium.y, MMVibrate._rumbleMedium.z, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.HeavyImpact:
        MMVibrate.Rumble(MMVibrate._rumbleHeavy.x, MMVibrate._rumbleHeavy.y, MMVibrate._rumbleHeavy.z, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.RigidImpact:
        MMVibrate.Rumble(MMVibrate._rumbleRigid.x, MMVibrate._rumbleRigid.y, MMVibrate._rumbleRigid.z, coroutineSupport, controllerID);
        break;
      case MMVibrate.HapticTypes.SoftImpact:
        MMVibrate.Rumble(MMVibrate._rumbleSoft.x, MMVibrate._rumbleSoft.y, MMVibrate._rumbleSoft.z, coroutineSupport, controllerID);
        break;
    }
  }

  public static void Rumble(
    float lowFrequency,
    float highFrequency,
    float duration,
    MonoBehaviour coroutineSupport,
    int controllerID = -1)
  {
    if (!InputManager.General.InputIsController() || !coroutineSupport.gameObject.activeInHierarchy)
      return;
    coroutineSupport.StartCoroutine((IEnumerator) MMVibrate.RumbleCoroutine(lowFrequency, highFrequency, duration, controllerID));
  }

  private static IEnumerator RumbleCoroutine(
    float lowFrequency,
    float highFrequency,
    float duration,
    int controllerID = -1)
  {
    if (MMVibrate.player != null)
    {
      MMVibrate.Rumbling = true;
      float num = 1f;
      foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
      {
        if (MMVibrate.player.controllers.GetLastActiveController() == joystick && joystick.supportsVibration)
          joystick.SetVibration(lowFrequency * MMVibrate._vibrationIntensity * num, highFrequency * MMVibrate._vibrationIntensity * num);
      }
      float startedAt = Time.unscaledTime;
      while ((double) Time.unscaledTime - (double) startedAt < (double) duration)
        yield return (object) null;
      foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
        joystick.StopVibration();
      MMVibrate.Rumbling = false;
    }
  }

  public static void Rumble(
    long[] pattern,
    int[] amplitudes,
    int repeat,
    MonoBehaviour coroutineSupport,
    int controllerID = -1)
  {
    if (!InputManager.General.InputIsController() || pattern == null || amplitudes == null)
      return;
    coroutineSupport.StartCoroutine((IEnumerator) MMVibrate.RumblePatternCoroutine(pattern, amplitudes, amplitudes, repeat, coroutineSupport, controllerID));
  }

  public static void Rumble(
    long[] pattern,
    int[] lowFreqAmplitudes,
    int[] highFreqAmplitudes,
    int repeat,
    MonoBehaviour coroutineSupport,
    int controllerID = -1)
  {
    if (pattern == null || lowFreqAmplitudes == null || highFreqAmplitudes == null)
      return;
    coroutineSupport.StartCoroutine((IEnumerator) MMVibrate.RumblePatternCoroutine(pattern, lowFreqAmplitudes, highFreqAmplitudes, repeat, coroutineSupport, controllerID));
  }

  private static IEnumerator RumblePatternCoroutine(
    long[] pattern,
    int[] lowFreqAmplitudes,
    int[] highFreqAmplitudes,
    int repeat,
    MonoBehaviour coroutineSupport,
    int controllerID = -1)
  {
    float currentTime = Time.unscaledTime;
    int currentIndex = 0;
label_16:
    while (currentIndex < pattern.Length)
    {
      if (MMVibrate.player == null)
        yield break;
      int num1 = 0;
      float num2 = 0.0f;
      float num3 = 0.0f;
      do
      {
        float num4 = (float) pattern[currentIndex];
        float num5 = lowFreqAmplitudes.Length > currentIndex ? (float) lowFreqAmplitudes[currentIndex] / (float) byte.MaxValue : 0.0f;
        num2 += num5;
        float num6 = highFreqAmplitudes.Length > currentIndex ? (float) highFreqAmplitudes[currentIndex] / (float) byte.MaxValue : 0.0f;
        num3 += num6;
        currentTime += num4 / 1000f;
        ++num1;
        ++currentIndex;
      }
      while ((double) currentTime < (double) Time.unscaledTime && currentIndex < pattern.Length);
      float num7 = 1f;
      foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
      {
        if (MMVibrate.player.controllers.GetLastActiveController() == joystick && joystick.supportsVibration)
          joystick.SetVibration(num2 / (float) num1 * MMVibrate._vibrationIntensity * num7, num3 / (float) num1 * MMVibrate._vibrationIntensity * num7);
      }
      while (true)
      {
        if ((double) currentTime > (double) Time.unscaledTime && currentIndex < pattern.Length)
          yield return (object) null;
        else
          goto label_16;
      }
    }
    foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
    {
      if (joystick.supportsVibration)
        joystick.SetVibration(0.0f, 0.0f);
    }
  }

  public static void RumbleContinuous(float lowFrequency, float highFrequency, int controllerID = -1)
  {
    if (MonoSingleton<UIManager>.Instance.IsPaused || !InputManager.General.InputIsController() || MMVibrate.player == null)
      return;
    float num = 1f;
    MMVibrate.Rumbling = true;
    MMVibrate.RumblingContinuous = true;
    foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
    {
      if (MMVibrate.player.controllers.GetLastActiveController() == joystick && joystick.supportsVibration)
        joystick.SetVibration(lowFrequency * MMVibrate._vibrationIntensity * num, highFrequency * MMVibrate._vibrationIntensity * num);
    }
  }

  public static void StopRumble()
  {
    MMVibrate.Rumbling = false;
    MMVibrate.RumblingContinuous = false;
    if (MMVibrate.player == null)
      return;
    foreach (Joystick joystick in (IEnumerable<Joystick>) MMVibrate.player.controllers.Joysticks)
      joystick.StopVibration();
  }

  public enum HapticTypes
  {
    Selection,
    Success,
    Warning,
    Failure,
    LightImpact,
    MediumImpact,
    HeavyImpact,
    RigidImpact,
    SoftImpact,
    None,
  }
}
