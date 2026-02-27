// Decompiled with JetBrains decompiler
// Type: KeyboardLightingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

#nullable disable
public class KeyboardLightingManager : MonoSingleton<KeyboardLightingManager>
{
  private static KeyboardLightingManager.InitializationState _initializationState = KeyboardLightingManager.InitializationState.None;
  public bool DEBUGGING;
  public Color DEBUG_COLOR;
  private const uint DEVTYPE_KEYBOARD = 524288 /*0x080000*/;
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
  private Color[] TimeOfDayColors = new Color[5]
  {
    new Color(1f, 0.5f, 0.0f, 1f),
    new Color(1f, 0.5f, 0.0f, 1f),
    new Color(1f, 0.5f, 0.0f, 1f),
    new Color(1f, 0.5f, 0.5f, 1f),
    new Color(0.5f, 0.0f, 1f, 1f)
  };
  private List<KeyboardLightingManager.CustomKey> customKeys = new List<KeyboardLightingManager.CustomKey>();
  private VideoPlayer videoPlayer;
  private KeyboardLightingManager.EffectType currentEffectType;
  private Array keycodes = Enum.GetValues(typeof (KeyCode));
  private float timestamp;
  private float previousFaith;
  private float previousHP;
  private TweenerCore<float, float, FloatOptions> allKeysTween;

  public override void Awake() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public override void Start()
  {
    base.Start();
    this.Initialise();
    this.videoPlayer = this.GetComponentInChildren<VideoPlayer>();
    HealthPlayer.OnDamaged += new HealthPlayer.HPUpdated(this.UpdateHealth);
    HealthPlayer.OnHPUpdated += new HealthPlayer.HPUpdated(this.OnHeal);
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnLocationSet);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
  }

  private void OnDestroy()
  {
    HealthPlayer.OnDamaged -= new HealthPlayer.HPUpdated(this.UpdateHealth);
    HealthPlayer.OnHPUpdated -= new HealthPlayer.HPUpdated(this.OnHeal);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnLocationSet);
    CultFaithManager.OnPulse -= new System.Action(this.PulseLowFaithBar);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
  }

  private void Initialise()
  {
  }

  private void Update()
  {
  }

  private void OnLocationSet()
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
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.OnLocationSet();
  }

  public void SetDungeonLayout()
  {
    KeyboardLightingManager.StopAll();
    CultFaithManager.OnPulse -= new System.Action(this.PulseLowFaithBar);
    BiomeGenerator.OnBiomeChangeRoom += new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
    this.UpdateHealth((HealthPlayer) null);
    this.StartCoroutine((IEnumerator) this.WaitForSeconds(0.5f, (System.Action) (() => this.UpdateHealth((HealthPlayer) null))));
  }

  public void SetBaseLayout()
  {
    KeyboardLightingManager.StopAll();
    this.currentEffectType = KeyboardLightingManager.EffectType.None;
    this.OnNewPhaseStarted();
    this.UpdateFaith();
    this.StartCoroutine((IEnumerator) this.UpdateXPBar());
    CultFaithManager.OnPulse += new System.Action(this.PulseLowFaithBar);
    BiomeGenerator.OnBiomeChangeRoom -= new BiomeGenerator.BiomeAction(this.OnBiomeChangeRoom);
  }

  public static void ForceBaseLayout()
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.SetBaseLayout();
  }

  public void SetTempleLayout()
  {
    KeyboardLightingManager.StopAll();
    CultFaithManager.OnPulse -= new System.Action(this.PulseLowFaithBar);
  }

  public static void ForceTempleLayout()
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.SetTempleLayout();
  }

  public void SetIntroLayout()
  {
    KeyboardLightingManager.StopAll();
    this.SetCustomKey(KeyCode.W, Color.black, Color.red, 5f);
    this.SetCustomKey(KeyCode.A, Color.black, Color.red, 5f);
    this.SetCustomKey(KeyCode.S, Color.black, Color.red, 5f);
    this.SetCustomKey(KeyCode.D, Color.black, Color.red, 5f);
  }

  private void UpdateHealth(HealthPlayer Target)
  {
    int length = KeyboardLightingManager.F_KEYS.Length;
    int totalHp = (int) PlayerFarming.Instance.health.totalHP;
    float t = PlayerFarming.Instance.health.HP / (float) totalHp;
    int num = Mathf.CeilToInt(Mathf.Lerp(0.0f, (float) length, t));
    for (int index = 0; index < length; ++index)
      this.SetKeyColor(KeyboardLightingManager.F_KEYS[index], index <= num ? Color.red : Color.black);
  }

  private void OnHeal(HealthPlayer Target)
  {
    if ((double) this.previousHP < (double) Target.HP)
    {
      KeyboardLightingManager.StopAll();
      this.StartCoroutine((IEnumerator) this.WaitForEndOfFrame((System.Action) (() => KeyboardLightingManager.UpdateLocation())));
    }
    this.previousHP = Target.HP;
  }

  private void OnBiomeChangeRoom()
  {
    if ((bool) (UnityEngine.Object) BiomeGenerator.Instance.GetComponentInChildren<MiniBossController>())
    {
      this.currentEffectType = KeyboardLightingManager.EffectType.Boss;
      KeyboardLightingManager.TransitionAllKeys(Color.black, Color.black, 0.0f, KeyboardLightingManager.F_KEYS);
    }
    else
    {
      if (this.currentEffectType != KeyboardLightingManager.EffectType.Boss)
        return;
      this.currentEffectType = KeyboardLightingManager.EffectType.None;
    }
  }

  private void UpdateFaith()
  {
    if (!DataManager.Instance.ShowCultFaith)
      return;
    for (int index = 0; index < KeyboardLightingManager.NUMPAD_KEYS.Length; ++index)
      this.SetKeyColor(KeyboardLightingManager.NUMPAD_KEYS[index], Color.black);
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

  private void PulseLowFaithBar()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location))
      return;
    foreach (KeyCode keycode in KeyboardLightingManager.NUMPAD_KEYS)
    {
      if (!this.GetCustomKey(keycode).IsTransitioning)
        this.SetCustomKey(keycode, Color.red, new Color(0.1f, 0.0f, 0.0f, 1f), 0.5f, Ease.OutBounce);
    }
  }

  private void OnNewPhaseStarted()
  {
    if (GameManager.IsDungeon(PlayerFarming.Location) || this.currentEffectType != KeyboardLightingManager.EffectType.None)
      return;
    List<KeyCode> keyCodeList = new List<KeyCode>();
    if (DataManager.Instance.ShowCultFaith)
      keyCodeList.AddRange((IEnumerable<KeyCode>) KeyboardLightingManager.NUMPAD_KEYS);
    if (DataManager.Instance.HasBuiltShrine1)
      keyCodeList.AddRange((IEnumerable<KeyCode>) KeyboardLightingManager.F_KEYS);
    KeyboardLightingManager.TransitionAllKeys(this.TimeOfDayColors[(int) Mathf.Repeat((float) (TimeManager.CurrentPhase - 1), 5f)], this.TimeOfDayColors[(int) TimeManager.CurrentPhase], 1f, keyCodeList.ToArray());
  }

  private IEnumerator UpdateXPBar()
  {
    if (DataManager.Instance.HasBuiltShrine1)
    {
      while (true)
      {
        if (UpgradeSystem.AbilityPoints > 0)
        {
          for (int i = 0; i < KeyboardLightingManager.F_KEYS.Length; ++i)
          {
            if (this.GetCustomKey(KeyboardLightingManager.F_KEYS[i]).PreviousColor == Color.red)
              this.SetCustomKey(KeyboardLightingManager.F_KEYS[i], Color.white, Color.red, 0.2f);
            else
              this.SetCustomKey(KeyboardLightingManager.F_KEYS[i], Color.red, Color.white, 0.2f);
            yield return (object) new WaitForSecondsRealtime(0.04f);
          }
        }
        else
        {
          float t = (float) DataManager.Instance.XP / (float) DataManager.TargetXP[Mathf.Min(DataManager.Instance.Level, Mathf.Max(DataManager.TargetXP.Count - 1, 0))];
          int num = Mathf.CeilToInt(Mathf.Lerp(0.0f, (float) KeyboardLightingManager.F_KEYS.Length, t));
          for (int index = 0; index < KeyboardLightingManager.F_KEYS.Length; ++index)
            this.SetKeyColor(KeyboardLightingManager.F_KEYS[index], index <= num ? Color.white : Color.black);
        }
        yield return (object) null;
      }
    }
  }

  public void SetRain() => this.currentEffectType = KeyboardLightingManager.EffectType.Rain;

  private void UpdateRainEffect()
  {
    if ((double) Time.unscaledTime <= (double) this.timestamp)
      return;
    this.timestamp = Time.unscaledTime + UnityEngine.Random.Range(0.00025f, 0.0005f);
    KeyCode keycode;
    do
    {
      keycode = (KeyCode) UnityEngine.Random.Range(0, this.keycodes.Length / 2);
    }
    while (KeyboardLightingManager.NUMPAD_KEYS.Contains<KeyCode>(keycode) || KeyboardLightingManager.F_KEYS.Contains<KeyCode>(keycode) || this.GetCustomKey(keycode).IsTransitioning);
    this.SetCustomKey(keycode, Color.blue, Color.grey, 0.5f);
  }

  public static void TransitionAllKeys(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    Ease ease = Ease.Linear)
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    KeyboardLightingManager.TransitionAllKeys(previousColor, targetColor, transitionDuration, new KeyCode[0], ease);
  }

  public static void TransitionAllKeys(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    KeyCode[] excludedKeys,
    Ease ease = Ease.Linear)
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null)
      return;
    int initializationState = (int) KeyboardLightingManager._initializationState;
  }

  public static void PulseAllKeys(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    KeyCode[] excludedKeys)
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.StartCoroutine((IEnumerator) MonoSingleton<KeyboardLightingManager>.Instance.PulseAllKeysIE(previousColor, targetColor, transitionDuration, excludedKeys));
  }

  private IEnumerator PulseAllKeysIE(
    Color previousColor,
    Color targetColor,
    float transitionDuration,
    KeyCode[] excludedKeys)
  {
    Color currentColor = previousColor;
    while (true)
    {
      if (this.allKeysTween == null || !this.allKeysTween.IsPlaying())
      {
        if (currentColor == previousColor)
        {
          KeyboardLightingManager.TransitionAllKeys(targetColor, previousColor, transitionDuration, excludedKeys);
          currentColor = targetColor;
        }
        else
        {
          KeyboardLightingManager.TransitionAllKeys(previousColor, targetColor, transitionDuration, excludedKeys);
          currentColor = previousColor;
        }
      }
      yield return (object) null;
    }
  }

  public static void StopAll()
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    TweenerCore<float, float, FloatOptions> allKeysTween = MonoSingleton<KeyboardLightingManager>.Instance.allKeysTween;
    if (allKeysTween != null)
      allKeysTween.Kill();
    KeyboardLightingManager.ClearAllCustomKeys();
    MonoSingleton<KeyboardLightingManager>.Instance.StopAllCoroutines();
  }

  private void SetKeyColor(KeyCode keycode, Color color) => this.GetCustomKey(keycode);

  private KeyboardLightingManager.CustomKey GetCustomKey(KeyCode keycode)
  {
    foreach (KeyboardLightingManager.CustomKey customKey in this.customKeys)
    {
      if (customKey.KeyCode == keycode)
        return customKey;
    }
    KeyboardLightingManager.CustomKey customKey1 = new KeyboardLightingManager.CustomKey()
    {
      KeyCode = keycode
    };
    this.customKeys.Add(customKey1);
    return customKey1;
  }

  private void SetCustomKey(
    KeyCode keycode,
    Color targetColor,
    float transitionDuration,
    Ease ease = Ease.Linear)
  {
    KeyboardLightingManager.CustomKey key = this.GetCustomKey(keycode);
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

  private void SetCustomKey(
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
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    foreach (KeyboardLightingManager.CustomKey customKey in MonoSingleton<KeyboardLightingManager>.Instance.customKeys)
      customKey.tween.Kill();
  }

  public static void PlayVideo(string fileName)
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.videoPlayer.clip = Resources.Load<VideoClip>("Keyboard Lighting Assets/" + fileName);
    MonoSingleton<KeyboardLightingManager>.Instance.videoPlayer.Play();
    MonoSingleton<KeyboardLightingManager>.Instance.currentEffectType = KeyboardLightingManager.EffectType.Video;
  }

  public static void PlayVideo()
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.currentEffectType = KeyboardLightingManager.EffectType.Video;
  }

  public static void StopVideo()
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    MonoSingleton<KeyboardLightingManager>.Instance.videoPlayer.Stop();
    MonoSingleton<KeyboardLightingManager>.Instance.currentEffectType = KeyboardLightingManager.EffectType.None;
  }

  private void UpdateVideo()
  {
    this.StartCoroutine((IEnumerator) this.WaitForEndOfFrame((System.Action) (() =>
    {
      Texture2D texture2D = new Texture2D(210, 70, TextureFormat.RGB24, false);
      RenderTexture.active = this.videoPlayer.isPlaying ? this.videoPlayer.targetTexture : RenderTexture.active;
      texture2D.ReadPixels(new Rect(0.0f, 0.0f, 210f, 70f), 0, 0, false);
      texture2D.Apply();
      UnityEngine.Object.Destroy((UnityEngine.Object) texture2D);
    })));
  }

  private void UpdateTrickleRedEffect(KeyCode[] excludedKeys)
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

  private void UpdateTrickleRedEffect() => this.UpdateTrickleRedEffect(new KeyCode[0]);

  private IEnumerator WaitForEndOfFrame(System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  private IEnumerator WaitForSeconds(float seconds, System.Action callback)
  {
    yield return (object) new UnityEngine.WaitForSeconds(seconds);
    System.Action action = callback;
    if (action != null)
      action();
  }

  private void OnApplicationQuit()
  {
  }

  public static void Reset()
  {
    if ((UnityEngine.Object) MonoSingleton<KeyboardLightingManager>.Instance == (UnityEngine.Object) null || KeyboardLightingManager._initializationState <= KeyboardLightingManager.InitializationState.Failed)
      return;
    KeyboardLightingManager.StopAll();
    MonoSingleton<KeyboardLightingManager>.Instance.currentEffectType = KeyboardLightingManager.EffectType.None;
  }

  private enum InitializationState
  {
    None,
    Failed,
    Success,
  }

  private class CustomKey
  {
    public KeyCode KeyCode;
    public Color TargetColor;
    public Color PreviousColor;
    public TweenerCore<float, float, FloatOptions> tween;

    public bool IsTransitioning => this.tween != null && this.tween.IsPlaying();
  }

  private enum EffectType
  {
    None,
    Rain,
    Video,
    Boss,
  }
}
