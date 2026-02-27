// Decompiled with JetBrains decompiler
// Type: DeviceLightingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class DeviceLightingManager : MonoSingleton<DeviceLightingManager>
{
  public bool DEBUGGING;
  public Color DEBUG_COLOR;
  public const uint DEVTYPE_KEYBOARD = 524288 /*0x080000*/;
  public static KeyCode[] F_KEYS = new KeyCode[12]
  {
    KeyCode.F1,
    KeyCode.F2,
    KeyCode.F3,
    KeyCode.F4,
    KeyCode.F5,
    KeyCode.F6,
    KeyCode.F7,
    KeyCode.F8,
    KeyCode.F9,
    KeyCode.F10,
    KeyCode.F11,
    KeyCode.F12
  };
  public static KeyCode[] NUMPAD_KEYS = new KeyCode[17]
  {
    KeyCode.Keypad0,
    KeyCode.KeypadPeriod,
    KeyCode.Keypad1,
    KeyCode.Keypad2,
    KeyCode.Keypad3,
    KeyCode.KeypadEnter,
    KeyCode.Keypad4,
    KeyCode.Keypad5,
    KeyCode.Keypad6,
    KeyCode.Keypad7,
    KeyCode.Keypad8,
    KeyCode.Keypad9,
    KeyCode.KeypadPlus,
    KeyCode.Numlock,
    KeyCode.KeypadDivide,
    KeyCode.KeypadMultiply,
    KeyCode.KeypadMinus
  };
  public static Color[] TimeOfDayColors = new Color[5]
  {
    new Color(1f, 0.5f, 0.0f, 1f),
    new Color(1f, 0.5f, 0.0f, 1f),
    new Color(1f, 0.5f, 0.0f, 1f),
    new Color(1f, 0.5f, 0.5f, 1f),
    new Color(0.5f, 0.0f, 1f, 1f)
  };
  public List<DeviceLightingManager.CustomKey> customKeys = new List<DeviceLightingManager.CustomKey>();
  public VideoPlayer videoPlayer;
  public DeviceLightingManager.EffectType currentEffectType;
  public Array keycodes = Enum.GetValues(typeof (KeyCode));
  public float timestamp;
  public float previousFaith;
  public float previousHP;
  public TweenerCore<float, float, FloatOptions> allKeysTween;

  public override void Start()
  {
    base.Start();
    this.Initialise();
    this.videoPlayer = this.GetComponentInChildren<VideoPlayer>();
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.health.OnDamaged += new HealthPlayer.HPUpdated(this.UpdateHealth);
      player.health.OnHPUpdated += new HealthPlayer.HPUpdated(this.OnHeal);
    }
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnLocationSet);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  public void OnDestroy()
  {
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      player.health.OnDamaged -= new HealthPlayer.HPUpdated(this.UpdateHealth);
      player.health.OnHPUpdated -= new HealthPlayer.HPUpdated(this.OnHeal);
    }
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnLocationSet);
    CultFaithManager.OnPulse -= new System.Action(this.PulseLowFaithBar);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  public void Initialise()
  {
  }

  public void Update()
  {
    if (this.currentEffectType == DeviceLightingManager.EffectType.Video)
      this.UpdateVideo();
    if (PlayerFarming.Location == FollowerLocation.None)
      return;
    if (GameManager.IsDungeon(PlayerFarming.Location))
    {
      if (this.currentEffectType != DeviceLightingManager.EffectType.Boss)
        return;
      this.UpdateTrickleRedEffect(DeviceLightingManager.F_KEYS);
    }
    else if (PlayerFarming.Location == FollowerLocation.Church)
    {
      this.UpdateTrickleRedEffect();
    }
    else
    {
      if (WeatherSystemController.Instance.IsRaining && this.currentEffectType == DeviceLightingManager.EffectType.None)
      {
        List<KeyCode> keyCodeList = new List<KeyCode>();
        keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.F_KEYS);
        keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.NUMPAD_KEYS);
        DeviceLightingManager.TransitionLighting(DeviceLightingManager.TimeOfDayColors[(int) TimeManager.CurrentPhase], Color.grey, 1f, keyCodeList.ToArray());
        this.currentEffectType = DeviceLightingManager.EffectType.Rain;
      }
      else if (!WeatherSystemController.Instance.IsRaining && this.currentEffectType == DeviceLightingManager.EffectType.Rain)
      {
        List<KeyCode> keyCodeList = new List<KeyCode>();
        keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.F_KEYS);
        keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.NUMPAD_KEYS);
        DeviceLightingManager.TransitionLighting(Color.grey, DeviceLightingManager.TimeOfDayColors[(int) TimeManager.CurrentPhase], 1f, keyCodeList.ToArray());
        this.currentEffectType = DeviceLightingManager.EffectType.None;
      }
      if (this.currentEffectType == DeviceLightingManager.EffectType.Rain)
        this.UpdateRainEffect();
      if ((double) Mathf.Abs(CultFaithManager.CultFaithNormalised - this.previousFaith) <= 0.05000000074505806 || (double) CultFaithManager.CultFaithNormalised <= 0.10000000149011612)
        return;
      this.UpdateFaith();
    }
  }

  public void OnLocationSet()
  {
    if (PlayerFarming.Location == FollowerLocation.IntroDungeon)
      this.SetIntroLayout();
    else if (GameManager.IsDungeon(PlayerFarming.Location))
      this.SetDungeonLayout();
    else if (PlayerFarming.Location == FollowerLocation.Church)
      this.SetTempleLayout();
    else
      this.SetBaseLayout();
  }

  public static void UpdateLocation()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.OnLocationSet();
  }

  public void SetDungeonLayout()
  {
    DeviceLightingManager.StopAll();
    if (this.currentEffectType != DeviceLightingManager.EffectType.Boss)
      this.currentEffectType = DeviceLightingManager.EffectType.None;
    CultFaithManager.OnPulse -= new System.Action(this.PulseLowFaithBar);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
    this.UpdateHealth((HealthPlayer) null);
    this.StartCoroutine(this.WaitForSeconds(0.5f, (System.Action) (() => this.UpdateHealth((HealthPlayer) null))));
  }

  public void SetBaseLayout()
  {
    DeviceLightingManager.StopAll();
    this.currentEffectType = DeviceLightingManager.EffectType.None;
    this.OnNewPhaseStarted();
    this.UpdateFaith();
    this.StartCoroutine(this.UpdateXPBar());
    CultFaithManager.OnPulse += new System.Action(this.PulseLowFaithBar);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public static void ForceBaseLayout()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.SetBaseLayout();
  }

  public void SetTempleLayout()
  {
    DeviceLightingManager.StopAll();
    CultFaithManager.OnPulse -= new System.Action(this.PulseLowFaithBar);
  }

  public static void ForceTempleLayout()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.SetTempleLayout();
  }

  public void SetIntroLayout()
  {
    DeviceLightingManager.StopAll();
    this.SetCustomKey(KeyCode.W, Color.black, Color.red, 5f);
    this.SetCustomKey(KeyCode.A, Color.black, Color.red, 5f);
    this.SetCustomKey(KeyCode.S, Color.black, Color.red, 5f);
    this.SetCustomKey(KeyCode.D, Color.black, Color.red, 5f);
  }

  public void UpdateHealth(HealthPlayer Target)
  {
    if ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      return;
    int length = DeviceLightingManager.F_KEYS.Length;
    int totalHp = (int) PlayerFarming.Instance.health.totalHP;
    float t = PlayerFarming.Instance.health.HP / (float) totalHp;
    int num = Mathf.CeilToInt(Mathf.Lerp(0.0f, (float) length, t));
    for (int index = 0; index < length; ++index)
      this.SetKeyColor(DeviceLightingManager.F_KEYS[index], index <= num ? Color.red : Color.black);
  }

  public void OnHeal(HealthPlayer Target)
  {
    if ((double) this.previousHP < (double) Target.HP)
    {
      DeviceLightingManager.StopAll();
      this.StartCoroutine(this.WaitForEndOfFrame((System.Action) (() => DeviceLightingManager.UpdateLocation())));
    }
    this.previousHP = Target.HP;
  }

  public void OnBiomeChangeRoom()
  {
    if ((bool) (UnityEngine.Object) BiomeGenerator.Instance.GetComponentInChildren<MiniBossController>())
    {
      this.currentEffectType = DeviceLightingManager.EffectType.Boss;
      DeviceLightingManager.TransitionLighting(Color.black, Color.black, 0.0f, DeviceLightingManager.F_KEYS);
    }
    else
    {
      if (this.currentEffectType != DeviceLightingManager.EffectType.Boss)
        return;
      this.currentEffectType = DeviceLightingManager.EffectType.None;
    }
  }

  public void UpdateFaith()
  {
    if (!DataManager.Instance.ShowCultFaith)
      return;
    for (int index = 0; index < DeviceLightingManager.NUMPAD_KEYS.Length; ++index)
      this.SetKeyColor(DeviceLightingManager.NUMPAD_KEYS[index], Color.black);
    float cultFaithNormalised = CultFaithManager.CultFaithNormalised;
    Color color = StaticColors.ColorForThreshold(cultFaithNormalised);
    if ((double) cultFaithNormalised >= 0.10000000149011612)
    {
      if ((double) this.previousFaith < 0.10000000149011612)
      {
        this.SetCustomKey(KeyCode.Keypad0, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.KeypadPeriod, Color.white, color, 2f);
      }
      else
      {
        this.SetKeyColor(KeyCode.Keypad0, color);
        this.SetKeyColor(KeyCode.KeypadPeriod, color);
      }
    }
    if ((double) cultFaithNormalised >= 0.30000001192092896)
    {
      if ((double) this.previousFaith < 0.30000001192092896)
      {
        this.SetCustomKey(KeyCode.Keypad1, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.Keypad2, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.Keypad3, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.KeypadEnter, Color.white, color, 2f);
      }
      else
      {
        this.SetKeyColor(KeyCode.Keypad1, color);
        this.SetKeyColor(KeyCode.Keypad2, color);
        this.SetKeyColor(KeyCode.Keypad3, color);
        this.SetKeyColor(KeyCode.KeypadEnter, color);
      }
    }
    if ((double) cultFaithNormalised >= 0.5)
    {
      if ((double) this.previousFaith < 0.5)
      {
        this.SetCustomKey(KeyCode.Keypad4, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.Keypad5, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.Keypad6, Color.white, color, 2f);
      }
      else
      {
        this.SetKeyColor(KeyCode.Keypad4, color);
        this.SetKeyColor(KeyCode.Keypad5, color);
        this.SetKeyColor(KeyCode.Keypad6, color);
      }
    }
    if ((double) cultFaithNormalised >= 0.699999988079071)
    {
      if ((double) this.previousFaith < 0.699999988079071)
      {
        this.SetCustomKey(KeyCode.Keypad7, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.Keypad8, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.Keypad9, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.KeypadPlus, Color.white, color, 2f);
      }
      else
      {
        this.SetKeyColor(KeyCode.Keypad7, color);
        this.SetKeyColor(KeyCode.Keypad8, color);
        this.SetKeyColor(KeyCode.Keypad9, color);
        this.SetKeyColor(KeyCode.KeypadPlus, color);
      }
    }
    if ((double) cultFaithNormalised >= 0.949999988079071)
    {
      if ((double) this.previousFaith < 0.949999988079071)
      {
        this.SetCustomKey(KeyCode.Numlock, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.KeypadDivide, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.KeypadMultiply, Color.white, color, 2f);
        this.SetCustomKey(KeyCode.KeypadMinus, Color.white, color, 2f);
      }
      else
      {
        this.SetKeyColor(KeyCode.Numlock, color);
        this.SetKeyColor(KeyCode.KeypadDivide, color);
        this.SetKeyColor(KeyCode.KeypadMultiply, color);
        this.SetKeyColor(KeyCode.KeypadMinus, color);
      }
    }
    this.previousFaith = cultFaithNormalised;
  }

  public void PulseLowFaithBar()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    foreach (KeyCode keycode in DeviceLightingManager.NUMPAD_KEYS)
    {
      if (!this.GetCustomKey(keycode).IsTransitioning)
        this.SetCustomKey(keycode, Color.red, new Color(0.1f, 0.0f, 0.0f, 1f), 0.5f, Ease.OutBounce);
    }
  }

  public void OnNewPhaseStarted()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location) || this.currentEffectType != DeviceLightingManager.EffectType.None)
      return;
    List<KeyCode> keyCodeList = new List<KeyCode>();
    if (DataManager.Instance.ShowCultFaith)
      keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.NUMPAD_KEYS);
    if (DataManager.Instance.HasBuiltShrine1)
      keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.F_KEYS);
    DeviceLightingManager.TransitionLighting(DeviceLightingManager.TimeOfDayColors[(int) Utils.Repeat((float) (TimeManager.CurrentPhase - 1), 5f)], DeviceLightingManager.TimeOfDayColors[(int) TimeManager.CurrentPhase], 1f, keyCodeList.ToArray());
  }

  public IEnumerator UpdateXPBar()
  {
    if (DataManager.Instance.HasBuiltShrine1)
    {
      while (true)
      {
        if (UpgradeSystem.AbilityPoints > 0)
        {
          for (int i = 0; i < DeviceLightingManager.F_KEYS.Length; ++i)
          {
            if (this.GetCustomKey(DeviceLightingManager.F_KEYS[i]).PreviousColor == Color.red)
              this.SetCustomKey(DeviceLightingManager.F_KEYS[i], Color.white, Color.red, 0.2f);
            else
              this.SetCustomKey(DeviceLightingManager.F_KEYS[i], Color.red, Color.white, 0.2f);
            yield return (object) new WaitForSecondsRealtime(0.04f);
          }
        }
        else
        {
          float t = (float) DataManager.Instance.XP / (float) DataManager.GetTargetXP(Mathf.Min(DataManager.Instance.Level, Mathf.Max(DataManager.TargetXP.Count - 1, 0)));
          int num = Mathf.CeilToInt(Mathf.Lerp(0.0f, (float) DeviceLightingManager.F_KEYS.Length, t));
          for (int index = 0; index < DeviceLightingManager.F_KEYS.Length; ++index)
            this.SetKeyColor(DeviceLightingManager.F_KEYS[index], index <= num ? Color.white : Color.black);
        }
        yield return (object) null;
      }
    }
  }

  public void SetRain() => this.currentEffectType = DeviceLightingManager.EffectType.Rain;

  public void UpdateRainEffect()
  {
    if ((double) Time.unscaledTime <= (double) this.timestamp)
      return;
    this.timestamp = Time.unscaledTime + UnityEngine.Random.Range(0.00025f, 0.0005f);
    KeyCode keycode;
    do
    {
      keycode = (KeyCode) UnityEngine.Random.Range(0, this.keycodes.Length / 2);
    }
    while (DeviceLightingManager.NUMPAD_KEYS.Contains<KeyCode>(keycode) || DeviceLightingManager.F_KEYS.Contains<KeyCode>(keycode) || this.GetCustomKey(keycode).IsTransitioning);
    this.SetCustomKey(keycode, Color.blue, Color.grey, 0.5f);
  }

  public static void TransitionLighting(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    Ease ease = Ease.Linear)
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    DeviceLightingManager.TransitionLighting(previousColor, targetColor, transitionDuration, new KeyCode[0], ease);
  }

  public static void TransitionLighting(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    KeyCode[] excludedKeys,
    Ease ease = Ease.Linear)
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    if (MonoSingleton<DeviceLightingManager>.Instance.allKeysTween != null && MonoSingleton<DeviceLightingManager>.Instance.allKeysTween.active)
      MonoSingleton<DeviceLightingManager>.Instance.allKeysTween.Kill();
    float time = 0.0f;
    MonoSingleton<DeviceLightingManager>.Instance.allKeysTween = DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, transitionDuration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => Color.Lerp(previousColor, targetColor, time))).SetEase<TweenerCore<float, float, FloatOptions>>(ease).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
  }

  public static void PulseAllLighting(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    KeyCode[] excludedKeys)
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.StartCoroutine(MonoSingleton<DeviceLightingManager>.Instance.PulseAllLightingIE(previousColor, targetColor, transitionDuration, excludedKeys));
  }

  public IEnumerator PulseAllLightingIE(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    KeyCode[] excludedKeys)
  {
    Color currentColor = previousColor;
    while (true)
    {
      if (this.allKeysTween == null || !this.allKeysTween.active || !this.allKeysTween.IsPlaying())
      {
        if (currentColor == previousColor)
        {
          DeviceLightingManager.TransitionLighting(targetColor, previousColor, transitionDuration, excludedKeys);
          currentColor = targetColor;
        }
        else
        {
          DeviceLightingManager.TransitionLighting(previousColor, targetColor, transitionDuration, excludedKeys);
          currentColor = previousColor;
        }
      }
      yield return (object) null;
    }
  }

  public static void StopAll()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    if (MonoSingleton<DeviceLightingManager>.Instance.allKeysTween != null && MonoSingleton<DeviceLightingManager>.Instance.allKeysTween.active)
    {
      TweenerCore<float, float, FloatOptions> allKeysTween = MonoSingleton<DeviceLightingManager>.Instance.allKeysTween;
      if (allKeysTween != null)
        allKeysTween.Kill();
    }
    DeviceLightingManager.ClearAllCustomKeys();
    MonoSingleton<DeviceLightingManager>.Instance.StopAllCoroutines();
  }

  public void SetKeyColor(KeyCode keycode, Color color) => this.GetCustomKey(keycode);

  public DeviceLightingManager.CustomKey GetCustomKey(KeyCode keycode)
  {
    foreach (DeviceLightingManager.CustomKey customKey in this.customKeys)
    {
      if (customKey.KeyCode == keycode)
        return customKey;
    }
    DeviceLightingManager.CustomKey customKey1 = new DeviceLightingManager.CustomKey()
    {
      KeyCode = keycode
    };
    this.customKeys.Add(customKey1);
    return customKey1;
  }

  public void SetCustomKey(
    KeyCode keycode,
    Color targetColor,
    float transitionDuration,
    Ease ease = Ease.Linear)
  {
    DeviceLightingManager.CustomKey key = this.GetCustomKey(keycode);
    key.TargetColor = targetColor;
    if ((double) transitionDuration == 0.0)
    {
      this.SetKeyColor(key.KeyCode, key.TargetColor);
    }
    else
    {
      float time = 0.0f;
      key.tween = DOTween.To((DOGetter<float>) (() => time), (DOSetter<float>) (x => time = x), 1f, transitionDuration).OnUpdate<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() => this.SetKeyColor(key.KeyCode, Color.Lerp(key.PreviousColor, key.TargetColor, time)))).SetEase<TweenerCore<float, float, FloatOptions>>(ease).SetUpdate<TweenerCore<float, float, FloatOptions>>(true);
    }
  }

  public void SetCustomKey(
    KeyCode keycode,
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    Ease ease = Ease.Linear)
  {
    this.GetCustomKey(keycode).PreviousColor = previousColor;
    this.SetCustomKey(keycode, targetColor, transitionDuration, ease);
  }

  public static void ClearAllCustomKeys()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    foreach (DeviceLightingManager.CustomKey customKey in MonoSingleton<DeviceLightingManager>.Instance.customKeys)
    {
      if (customKey.tween != null && customKey.tween.active)
        customKey.tween.Kill();
    }
  }

  public static void PlayVideo(string fileName)
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null || (UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance.videoPlayer == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.videoPlayer.clip = Resources.Load<VideoClip>("Keyboard Lighting Assets/" + fileName);
    MonoSingleton<DeviceLightingManager>.Instance.videoPlayer.Play();
    MonoSingleton<DeviceLightingManager>.Instance.currentEffectType = DeviceLightingManager.EffectType.Video;
  }

  public static void PlayVideo()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null || (UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance.videoPlayer == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.currentEffectType = DeviceLightingManager.EffectType.Video;
  }

  public static void StopVideo()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    MonoSingleton<DeviceLightingManager>.Instance.videoPlayer.Stop();
    MonoSingleton<DeviceLightingManager>.Instance.currentEffectType = DeviceLightingManager.EffectType.None;
  }

  public void UpdateVideo()
  {
    this.StartCoroutine(this.WaitForEndOfFrame((System.Action) (() =>
    {
      Texture2D texture2D = new Texture2D(210, 70, TextureFormat.RGB24, false);
      RenderTexture.active = this.videoPlayer.isPlaying ? this.videoPlayer.targetTexture : RenderTexture.active;
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, 210f, 70f), 0, 0, false);
      texture2D.Apply(false, true);
      UnityEngine.Object.Destroy((UnityEngine.Object) texture2D);
    })));
  }

  public static void FlashColor(Color color, float fadeOut = 0.5f)
  {
    if (GameManager.IsDungeon(PlayerFarming.Location))
      DeviceLightingManager.TransitionLighting(color, Color.black, fadeOut, DeviceLightingManager.F_KEYS);
    else if (PlayerFarming.Location == FollowerLocation.Church)
    {
      DeviceLightingManager.TransitionLighting(color, Color.black, fadeOut);
    }
    else
    {
      List<KeyCode> keyCodeList = new List<KeyCode>();
      if (DataManager.Instance.ShowCultFaith)
        keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.NUMPAD_KEYS);
      if (DataManager.Instance.HasBuiltShrine1)
        keyCodeList.AddRange((IEnumerable<KeyCode>) DeviceLightingManager.F_KEYS);
      DeviceLightingManager.TransitionLighting(color, DeviceLightingManager.TimeOfDayColors[(int) TimeManager.CurrentPhase], fadeOut, keyCodeList.ToArray());
    }
  }

  public void UpdateTrickleRedEffect(KeyCode[] excludedKeys)
  {
    if ((double) Time.unscaledTime <= (double) this.timestamp)
      return;
    this.timestamp = Time.unscaledTime + UnityEngine.Random.Range(2.5E-05f, 5E-05f);
    KeyCode keycode;
    do
    {
      keycode = (KeyCode) UnityEngine.Random.Range(0, this.keycodes.Length);
    }
    while (excludedKeys.Contains<KeyCode>(keycode) || this.GetCustomKey(keycode).IsTransitioning);
    this.SetCustomKey(keycode, Color.red, Color.black, 0.5f);
  }

  public void UpdateTrickleRedEffect() => this.UpdateTrickleRedEffect(new KeyCode[0]);

  public IEnumerator WaitForEndOfFrame(System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public IEnumerator WaitForSeconds(float seconds, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(seconds);
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void OnApplicationQuit()
  {
  }

  public static void Reset()
  {
    if ((UnityEngine.Object) MonoSingleton<DeviceLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    DeviceLightingManager.StopAll();
    MonoSingleton<DeviceLightingManager>.Instance.currentEffectType = DeviceLightingManager.EffectType.None;
  }

  [CompilerGenerated]
  public void \u003CSetDungeonLayout\u003Eb__22_0() => this.UpdateHealth((HealthPlayer) null);

  [CompilerGenerated]
  public void \u003CUpdateVideo\u003Eb__50_0()
  {
    Texture2D texture2D = new Texture2D(210, 70, TextureFormat.RGB24, false);
    RenderTexture.active = this.videoPlayer.isPlaying ? this.videoPlayer.targetTexture : RenderTexture.active;
    texture2D.ReadPixels(new Rect(0.0f, 0.0f, 210f, 70f), 0, 0, false);
    texture2D.Apply(false, true);
    UnityEngine.Object.Destroy((UnityEngine.Object) texture2D);
  }

  public class CustomKey
  {
    public KeyCode KeyCode;
    public Color TargetColor;
    public Color PreviousColor;
    public TweenerCore<float, float, FloatOptions> tween;

    public bool IsTransitioning
    {
      get => this.tween != null && this.tween.active && this.tween.IsPlaying();
    }
  }

  public enum EffectType
  {
    None,
    Rain,
    Video,
    Boss,
  }
}
