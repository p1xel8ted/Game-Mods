// Decompiled with JetBrains decompiler
// Type: CameraManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using EZCameraShake;
using Lamb.UI;
using System;
using System.Collections;
using UnityEngine;

#nullable disable
public class CameraManager : BaseMonoBehaviour
{
  public static float shakeAmount;
  public static float ShakeSpeedX;
  public static float ShakeX;
  public static float ShakeSpeedY;
  public static float ShakeY;
  public static float ShakeZ;
  public static float settings_ScreenShakeIntensity;
  public const float overlayMuteMultiplier = 0.05f;
  public static CameraManager instance;
  [SerializeField]
  public Camera cameraRef;
  public float TestShakeAmount = 0.4f;
  public float Modifier = 0.1f;
  public float DropOff = 5f;
  public CameraManager.ShakeMode MyShakeMode;
  public float Lerp;
  public float EZModifier = 3f;
  public float EZRoughness = 4f;
  public float EZDuration = 0.4f;
  public float prevEZShakeIntensity;
  public float prevEZShakeTimestamp;
  public bool IsShakingForDuration;

  public Camera CameraRef => !(bool) (UnityEngine.Object) this.cameraRef ? Camera.main : this.cameraRef;

  public void OnEnable() => CameraManager.instance = this;

  public void Awake()
  {
  }

  public void Start() => CameraManager.shakeAmount = 0.0f;

  public void Update() => this.DoShake();

  public void TestShake() => CameraManager.shakeCamera(this.TestShakeAmount);

  public void DoShake()
  {
    switch (this.MyShakeMode)
    {
      case CameraManager.ShakeMode.Shake:
        if ((double) CameraManager.ShakeX > 0.0)
        {
          CameraManager.ShakeX -= Time.unscaledDeltaTime * this.DropOff;
          CameraManager.ShakeY -= Time.unscaledDeltaTime * this.DropOff;
          CameraManager.ShakeZ -= Time.unscaledDeltaTime * this.DropOff;
          if ((double) CameraManager.ShakeX <= 0.0)
          {
            double num;
            CameraManager.ShakeZ = (float) (num = 0.0);
            CameraManager.ShakeY = (float) num;
            CameraManager.ShakeX = (float) num;
          }
        }
        this.transform.position = UnityEngine.Random.insideUnitSphere * CameraManager.ShakeX * Time.deltaTime;
        break;
      case CameraManager.ShakeMode.Wobble:
        CameraManager.ShakeSpeedX += (float) ((0.0 - (double) CameraManager.ShakeX) * 0.30000001192092896);
        CameraManager.ShakeX += (CameraManager.ShakeSpeedX *= 0.6f);
        CameraManager.ShakeSpeedY += (float) ((0.0 - (double) CameraManager.ShakeY) * 0.30000001192092896);
        CameraManager.ShakeY += (CameraManager.ShakeSpeedY *= 0.6f);
        this.transform.position = new Vector3(CameraManager.ShakeX, CameraManager.ShakeY) * CameraManager.getScreenshakeSettings() * Time.deltaTime;
        break;
    }
  }

  public static float getScreenshakeSettings()
  {
    return SettingsManager.Settings.Accessibility.ScreenShake;
  }

  public void shakeCamera1(float intensity, float direction)
  {
    CameraManager.shakeCamera(intensity, direction);
  }

  public static void shakeCamera(float intensity, bool stackShakes = true)
  {
    CameraManager.shakeCamera(intensity, (float) UnityEngine.Random.Range(0, 360), stackShakes);
  }

  public void EZShake(float intensity, bool stackShakes = true)
  {
    if ((UnityEngine.Object) GameManager.GetInstance() == (UnityEngine.Object) null)
      return;
    if ((double) this.prevEZShakeIntensity > 0.0 && (double) GameManager.GetInstance().TimeSince(this.prevEZShakeTimestamp) >= (double) this.EZDuration)
      this.prevEZShakeIntensity = 0.0f;
    if (!stackShakes && (double) intensity <= (double) this.prevEZShakeIntensity)
      return;
    this.prevEZShakeIntensity = intensity;
    this.prevEZShakeTimestamp = !((UnityEngine.Object) GameManager.GetInstance() != (UnityEngine.Object) null) ? Time.time : GameManager.GetInstance().CurrentTime;
    if ((UnityEngine.Object) CameraShaker.Instance == (UnityEngine.Object) null)
    {
      CameraShaker cameraShaker = this.gameObject.AddComponent<CameraShaker>();
      cameraShaker.DefaultPosInfluence = Vector3.one * 0.35f;
      cameraShaker.DefaultRotInfluence = Vector3.one * 0.35f;
    }
    CameraShaker.Instance.ShakeOnce(intensity * this.EZModifier * CameraManager.getScreenshakeSettings(), this.EZRoughness, 0.1f, this.EZDuration);
  }

  public static void shakeCamera(float intensity, float direction, bool stackShakes = true)
  {
    if (UIItemSelectorOverlayController.SelectorOverlays.Count > 0)
      intensity *= 0.05f;
    if (!((UnityEngine.Object) CameraManager.instance != (UnityEngine.Object) null))
      return;
    switch (CameraManager.instance.MyShakeMode)
    {
      case CameraManager.ShakeMode.EZCamShake:
        CameraManager.instance.EZShake(intensity, stackShakes);
        break;
      case CameraManager.ShakeMode.Shake:
        CameraManager.ShakeX = intensity * CameraManager.getScreenshakeSettings();
        CameraManager.ShakeY = intensity * CameraManager.getScreenshakeSettings();
        CameraManager.ShakeZ = intensity * CameraManager.getScreenshakeSettings();
        break;
      case CameraManager.ShakeMode.Wobble:
        CameraManager.ShakeSpeedX = intensity * Mathf.Cos(direction * ((float) Math.PI / 180f)) * CameraManager.getScreenshakeSettings();
        CameraManager.ShakeSpeedY = intensity * Mathf.Sin(direction * ((float) Math.PI / 180f)) * CameraManager.getScreenshakeSettings();
        break;
    }
  }

  public void ShakeCameraForDuration(
    float intensityMin,
    float intensityMax,
    float duration,
    bool StackShakes = true)
  {
    if (!((UnityEngine.Object) this != (UnityEngine.Object) null))
      return;
    this.StartCoroutine((IEnumerator) this.DoShakeCameraForDuration(intensityMin, intensityMax, duration, StackShakes));
  }

  public void Stopshake()
  {
    this.StopAllCoroutines();
    CameraManager.ShakeX = 0.0f;
    CameraManager.ShakeY = 0.0f;
    CameraManager.ShakeZ = 0.0f;
    CameraManager.ShakeSpeedX = 0.0f;
    CameraManager.ShakeSpeedY = 0.0f;
  }

  public IEnumerator DoShakeCameraForDuration(
    float intensityMin,
    float intensityMax,
    float duration,
    bool StackShakes = true)
  {
    while ((double) (duration -= Time.deltaTime) > 0.0)
    {
      if ((double) Time.deltaTime > 0.0)
        CameraManager.shakeCamera(UnityEngine.Random.Range(intensityMin, intensityMax), (float) UnityEngine.Random.Range(0, 360), StackShakes);
      yield return (object) null;
    }
  }

  public static void HitStop() => CameraManager.instance.StartCoroutine("HitstopCoroutine");

  public IEnumerator HitstopCoroutine()
  {
    Time.timeScale = 0.0f;
    float RealTimeOfTimestopStart = Time.realtimeSinceStartup;
    float lengthOfTimestop = 0.25f;
    while ((double) Time.realtimeSinceStartup < (double) RealTimeOfTimestopStart + (double) lengthOfTimestop)
      yield return (object) null;
    Time.timeScale = 1f;
  }

  public enum ShakeMode
  {
    EZCamShake,
    Shake,
    Wobble,
  }
}
