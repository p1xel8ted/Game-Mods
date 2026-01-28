// Decompiled with JetBrains decompiler
// Type: MMVibrate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using FMOD.Studio;
using Lamb.UI;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using Unify.Input;
using UnityEngine;

#nullable disable
public static class MMVibrate
{
  public static EventInstance rumbleEventInstance;
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
  public static bool _vibrationsActive = true;
  public static bool _debugLogActive = false;
  public static bool _hapticsPlayedOnce = false;
  public static float _vibrationIntensity = 1f;
  public static long[] _rigidImpactPattern = new long[2]
  {
    0L,
    MMVibrate.RigidDuration
  };
  public static int[] _rigidImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.RigidAmplitude
  };
  public static long[] _softImpactPattern = new long[2]
  {
    0L,
    MMVibrate.SoftDuration
  };
  public static int[] _softImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.SoftAmplitude
  };
  public static long[] _lightImpactPattern = new long[2]
  {
    0L,
    MMVibrate.LightDuration
  };
  public static int[] _lightImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.LightAmplitude
  };
  public static long[] _mediumImpactPattern = new long[2]
  {
    0L,
    MMVibrate.MediumDuration
  };
  public static int[] _mediumImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.MediumAmplitude
  };
  public static long[] _HeavyImpactPattern = new long[2]
  {
    0L,
    MMVibrate.HeavyDuration
  };
  public static int[] _HeavyImpactPatternAmplitude = new int[2]
  {
    0,
    MMVibrate.HeavyAmplitude
  };
  public static long[] _successPattern = new long[4]
  {
    0L,
    MMVibrate.LightDuration,
    MMVibrate.LightDuration,
    MMVibrate.HeavyDuration
  };
  public static int[] _successPatternAmplitude = new int[4]
  {
    0,
    MMVibrate.LightAmplitude,
    0,
    MMVibrate.HeavyAmplitude
  };
  public static long[] _warningPattern = new long[4]
  {
    0L,
    MMVibrate.HeavyDuration,
    MMVibrate.LightDuration,
    MMVibrate.MediumDuration
  };
  public static int[] _warningPatternAmplitude = new int[4]
  {
    0,
    MMVibrate.HeavyAmplitude,
    0,
    MMVibrate.MediumAmplitude
  };
  public static long[] _failurePattern = new long[8]
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
  public static int[] _failurePatternAmplitude = new int[8]
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
  public static Vector3 _rumbleRigid = new Vector3(0.5f, 1f, 0.08f);
  public static Vector3 _rumbleSoft = new Vector3(1f, 0.03f, 0.1f);
  public static Vector3 _rumbleLight = new Vector3(0.5f, 0.5f, 0.02f);
  public static Vector3 _rumbleMedium = new Vector3(0.8f, 0.8f, 0.04f);
  public static Vector3 _rumbleHeavy = new Vector3(1f, 1f, 0.08f);
  public static Vector3 _rumbleSuccess = new Vector3(1f, 1f, 1f);
  public static Vector3 _rumbleWarning = new Vector3(1f, 1f, 1f);
  public static Vector3 _rumbleFailure = new Vector3(1f, 1f, 1f);
  public static Vector3 _rumbleSelection = new Vector3(1f, 1f, 1f);
  public static bool Rumbling = false;
  public static bool RumblingContinuous = false;

  public static void SetHapticsActive(bool status, PlayerFarming playerFarming)
  {
    Debug.Log((object) ("[MMVibrate] Set haptics active : " + status.ToString()));
    MMVibrate._vibrationsActive = status;
    if (status || !((Object) playerFarming != (Object) null))
      return;
    foreach (Joystick joystick in (IEnumerable<Joystick>) playerFarming.rewiredPlayer.controllers.Joysticks)
      joystick.StopVibration();
  }

  public static void SetHapticsIntensity(float Intensity, PlayerFarming playerFarming = null)
  {
    if ((Object) playerFarming == (Object) null)
      playerFarming = PlayerFarming.Instance;
    Debug.Log((object) ("[MMVibrate] Intensity Set to : " + Intensity.ToString()));
    MMVibrate._vibrationIntensity = Intensity;
    MMVibrate.SetHapticsActive((double) Intensity > 0.0, playerFarming);
  }

  public static void HapticAllPlayers(
    MMVibrate.HapticTypes type,
    bool defaultToRegularVibrate = false,
    bool alsoRumble = true,
    MonoBehaviour coroutineSupport = null)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      MMVibrate.Haptic(type, player, defaultToRegularVibrate, alsoRumble, coroutineSupport);
  }

  public static void Haptic(
    MMVibrate.HapticTypes type,
    PlayerFarming playerFarming = null,
    bool defaultToRegularVibrate = false,
    bool alsoRumble = true,
    MonoBehaviour coroutineSupport = null,
    int controllerID = -1)
  {
    if (!MMVibrate._vibrationsActive)
      return;
    if ((Object) playerFarming == (Object) null)
    {
      if (PlayerFarming.playersCount > 1)
        return;
      playerFarming = PlayerFarming.Instance;
    }
    if ((Object) coroutineSupport == (Object) null)
      coroutineSupport = (MonoBehaviour) GameManager.GetInstance();
    if (!((Object) coroutineSupport != (Object) null))
      return;
    switch (type)
    {
      case MMVibrate.HapticTypes.Selection:
        MMVibrate.Rumble(MMVibrate._rumbleLight.x, MMVibrate._rumbleMedium.y, MMVibrate._rumbleLight.z, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.Success:
        MMVibrate.Rumble(MMVibrate._successPattern, MMVibrate._successPatternAmplitude, -1, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.Warning:
        MMVibrate.Rumble(MMVibrate._warningPattern, MMVibrate._warningPatternAmplitude, -1, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.Failure:
        MMVibrate.Rumble(MMVibrate._failurePattern, MMVibrate._failurePatternAmplitude, -1, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.LightImpact:
        MMVibrate.Rumble(MMVibrate._rumbleLight.x, MMVibrate._rumbleLight.y, MMVibrate._rumbleLight.z, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.MediumImpact:
        MMVibrate.Rumble(MMVibrate._rumbleMedium.x, MMVibrate._rumbleMedium.y, MMVibrate._rumbleMedium.z, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.HeavyImpact:
        MMVibrate.Rumble(MMVibrate._rumbleHeavy.x, MMVibrate._rumbleHeavy.y, MMVibrate._rumbleHeavy.z, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.RigidImpact:
        MMVibrate.Rumble(MMVibrate._rumbleRigid.x, MMVibrate._rumbleRigid.y, MMVibrate._rumbleRigid.z, coroutineSupport, playerFarming, controllerID);
        break;
      case MMVibrate.HapticTypes.SoftImpact:
        MMVibrate.Rumble(MMVibrate._rumbleSoft.x, MMVibrate._rumbleSoft.y, MMVibrate._rumbleSoft.z, coroutineSupport, playerFarming, controllerID);
        break;
    }
  }

  public static void Rumble(
    float lowFrequency,
    float highFrequency,
    float duration,
    MonoBehaviour coroutineSupport,
    PlayerFarming playerFarming = null,
    int controllerID = -1)
  {
    if ((Object) playerFarming != (Object) null && !InputManager.General.InputIsController(playerFarming) || !coroutineSupport.gameObject.activeInHierarchy)
      return;
    coroutineSupport.StartCoroutine((IEnumerator) MMVibrate.RumbleCoroutine(lowFrequency, highFrequency, duration, playerFarming, controllerID));
  }

  public static void RumbleForAllPlayers(
    float lowFrequency,
    float highFrequency,
    float duration,
    MonoBehaviour coroutineSupport = null)
  {
    if ((Object) coroutineSupport == (Object) null)
      coroutineSupport = (MonoBehaviour) GameManager.GetInstance();
    foreach (PlayerFarming player in PlayerFarming.players)
      MMVibrate.Rumble(lowFrequency, highFrequency, duration, coroutineSupport, player);
  }

  public static IEnumerator RumbleCoroutine(
    float lowFrequency,
    float highFrequency,
    float duration,
    PlayerFarming playerFarming,
    int controllerID = -1)
  {
    Player rewiredPlayer = !(bool) (Object) playerFarming ? RewiredInputManager.GetPlayer(0) : playerFarming.rewiredPlayer;
    MMVibrate.Rumbling = true;
    float num = 1f;
    foreach (Joystick joystick in (IEnumerable<Joystick>) rewiredPlayer.controllers.Joysticks)
    {
      if (rewiredPlayer.controllers.GetLastActiveController() == joystick && joystick.supportsVibration)
        joystick.SetVibration(lowFrequency * MMVibrate._vibrationIntensity * num, highFrequency * MMVibrate._vibrationIntensity * num);
    }
    float startedAt = Time.unscaledTime;
    while ((double) Time.unscaledTime - (double) startedAt < (double) duration)
      yield return (object) null;
    foreach (Joystick joystick in (IEnumerable<Joystick>) rewiredPlayer.controllers.Joysticks)
      joystick.StopVibration();
    MMVibrate.Rumbling = false;
  }

  public static void Rumble(
    long[] pattern,
    int[] amplitudes,
    int repeat,
    MonoBehaviour coroutineSupport,
    PlayerFarming playerFarming,
    int controllerID = -1)
  {
    if (!InputManager.General.InputIsController(playerFarming) || pattern == null || amplitudes == null)
      return;
    coroutineSupport.StartCoroutine((IEnumerator) MMVibrate.RumblePatternCoroutine(pattern, amplitudes, amplitudes, repeat, coroutineSupport, playerFarming, controllerID));
  }

  public static void Rumble(
    long[] pattern,
    int[] lowFreqAmplitudes,
    int[] highFreqAmplitudes,
    int repeat,
    MonoBehaviour coroutineSupport,
    PlayerFarming playerFarming,
    int controllerID = -1)
  {
    if (pattern == null || lowFreqAmplitudes == null || highFreqAmplitudes == null)
      return;
    coroutineSupport.StartCoroutine((IEnumerator) MMVibrate.RumblePatternCoroutine(pattern, lowFreqAmplitudes, highFreqAmplitudes, repeat, coroutineSupport, playerFarming, controllerID));
  }

  public static void RumblePad(Joystick Pad, int motorIndex, int motorLevel, float duration)
  {
    if (!MMVibrate._vibrationsActive)
      return;
    float num = 1f;
    Pad.SetVibration(motorIndex, (float) motorLevel * MMVibrate._vibrationIntensity * num, duration);
  }

  public static IEnumerator RumblePatternCoroutine(
    long[] pattern,
    int[] lowFreqAmplitudes,
    int[] highFreqAmplitudes,
    int repeat,
    MonoBehaviour coroutineSupport,
    PlayerFarming playerFarming,
    int controllerID = -1)
  {
    float currentTime = Time.unscaledTime;
    int currentIndex = 0;
label_16:
    while (currentIndex < pattern.Length)
    {
      if ((Object) playerFarming == (Object) null)
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
      foreach (Joystick joystick in (IEnumerable<Joystick>) playerFarming.rewiredPlayer.controllers.Joysticks)
      {
        if (playerFarming.rewiredPlayer.controllers.GetLastActiveController() == joystick && joystick.supportsVibration)
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
    foreach (Joystick joystick in (IEnumerable<Joystick>) playerFarming.rewiredPlayer.controllers.Joysticks)
    {
      if (joystick.supportsVibration)
        joystick.SetVibration(0.0f, 0.0f);
    }
  }

  public static void RumbleContinuousForAllPlayers(float lowFrequency, float highFrequency)
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      MMVibrate.RumbleContinuous(lowFrequency, highFrequency, player);
  }

  public static void StopRumbleForAllPlayers()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
      MMVibrate.StopRumble(player);
  }

  public static void RumbleContinuous(
    float lowFrequency,
    float highFrequency,
    PlayerFarming playerFarming = null)
  {
    if (MonoSingleton<UIManager>.Instance.IsPaused)
      return;
    Player player = !(bool) (Object) playerFarming ? RewiredInputManager.GetPlayer(0) : playerFarming.rewiredPlayer;
    float num = 1f;
    MMVibrate.Rumbling = true;
    MMVibrate.RumblingContinuous = true;
    foreach (Joystick joystick in (IEnumerable<Joystick>) player.controllers.Joysticks)
    {
      if (player.controllers.GetLastActiveController() == joystick && joystick.supportsVibration)
        joystick.SetVibration(lowFrequency * MMVibrate._vibrationIntensity * num, highFrequency * MMVibrate._vibrationIntensity * num);
    }
  }

  public static void StopRumble(PlayerFarming playerFarming = null)
  {
    MMVibrate.Rumbling = false;
    MMVibrate.RumblingContinuous = false;
    if ((bool) (Object) playerFarming)
    {
      foreach (Joystick joystick in (IEnumerable<Joystick>) playerFarming.rewiredPlayer.controllers.Joysticks)
        joystick.StopVibration();
    }
    else
    {
      for (int playerNo = 0; playerNo < 4; ++playerNo)
      {
        Player player = RewiredInputManager.GetPlayer(playerNo);
        if (player != null)
        {
          foreach (Joystick joystick in (IEnumerable<Joystick>) player.controllers.Joysticks)
            joystick.StopVibration();
        }
      }
    }
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
