// Decompiled with JetBrains decompiler
// Type: PhotoModeCamera
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.ReadWrite;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using src.Extensions;
using src.Managers;
using src.UI;
using src.UINavigator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
public class PhotoModeCamera : MonoBehaviour
{
  public static PhotoModeCamera Instance;
  public float minHeight = -1f;
  public const float maxHeight = -11f;
  public Quaternion restoreCameraRotationOnDisable;
  public Vector3 restoreCameraPositionOnDisable;
  [SerializeField]
  public CameraFollowTarget cameraFollowerTarget;
  [SerializeField]
  public float maxMoveSpeed;
  [SerializeField]
  public float acceleration;
  [SerializeField]
  public float deceleration;
  public UITakePhotoOverlayController takePhotoOverlay;
  public List<KeyValuePair<SkeletonAnimation, bool>> spines = new List<KeyValuePair<SkeletonAnimation, bool>>();
  [CompilerGenerated]
  public Camera \u003CCamera\u003Ek__BackingField;
  public Vector3 speed;
  public HideUI hideUI;
  public RaycastHit hit;
  public bool isErrorShown;

  public Camera Camera
  {
    get => this.\u003CCamera\u003Ek__BackingField;
    set => this.\u003CCamera\u003Ek__BackingField = value;
  }

  public bool IsErrorShown
  {
    get => this.isErrorShown;
    set => this.StartCoroutine((IEnumerator) this.SetErrorShownFlagNextFrame(value));
  }

  public IEnumerator SetErrorShownFlagNextFrame(bool value)
  {
    yield return (object) new WaitForEndOfFrame();
    this.isErrorShown = value;
  }

  public void Awake()
  {
    PhotoModeCamera.Instance = this;
    this.Camera = this.GetComponent<Camera>();
  }

  public void Start()
  {
    PhotoModeManager.OnPhotoModeEnabled += new PhotoModeManager.PhotoEvent(this.OnPhotoModeEnabled);
    PhotoModeManager.OnPhotoModeDisabled += new PhotoModeManager.PhotoEvent(this.OnPhotoModeDisabled);
  }

  public void OnDestroy()
  {
    PhotoModeCamera.Instance = (PhotoModeCamera) null;
    PhotoModeManager.OnPhotoModeEnabled -= new PhotoModeManager.PhotoEvent(this.OnPhotoModeEnabled);
    PhotoModeManager.OnPhotoModeDisabled -= new PhotoModeManager.PhotoEvent(this.OnPhotoModeDisabled);
  }

  public void OnPhotoModeEnabled()
  {
    this.restoreCameraRotationOnDisable = this.transform.rotation;
    this.restoreCameraPositionOnDisable = this.transform.position;
    AudioManager.Instance.PauseActiveLoopsAndSFX();
    this.cameraFollowerTarget.enabled = false;
    this.hideUI = this.gameObject.AddComponent<HideUI>();
    BiomeConstants.Instance.DepthOfFieldTween(0.15f, 8.7f, 26f, 1f, 200f);
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.interactor.CurrentInteraction != (UnityEngine.Object) null)
        player.interactor.CurrentInteraction.EndIndicateHighlighted(player);
    }
    this.StartCoroutine((IEnumerator) this.FrameDelay((System.Action) (() =>
    {
      this.takePhotoOverlay = MonoSingleton<UIManager>.Instance.TakePhotoOverlayTemplate.Instantiate<UITakePhotoOverlayController>();
      this.takePhotoOverlay.gameObject.SetActive(true);
      this.takePhotoOverlay.Show();
      UITakePhotoOverlayController takePhotoOverlay = this.takePhotoOverlay;
      takePhotoOverlay.OnHidden = takePhotoOverlay.OnHidden + (System.Action) (() =>
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.takePhotoOverlay.gameObject);
        this.takePhotoOverlay = (UITakePhotoOverlayController) null;
        PhotoModeManager.DisablePhotoMode();
      });
    })));
    foreach (SkeletonAnimation key in UnityEngine.Object.FindObjectsOfType<SkeletonAnimation>())
    {
      this.spines.Add(new KeyValuePair<SkeletonAnimation, bool>(key, key.UseDeltaTime));
      key.UseDeltaTime = true;
    }
  }

  public void OnPhotoModeDisabled()
  {
    BiomeConstants.Instance.DepthOfFieldTween(0.0f, 8.7f, 26f, 1f, 200f);
    this.cameraFollowerTarget.enabled = true;
    this.transform.eulerAngles = new Vector3(-45f, 0.0f, 0.0f);
    this.hideUI.ShowUI();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.hideUI);
    this.hideUI = (HideUI) null;
    foreach (PlayerFarming player in PlayerFarming.players)
    {
      if ((UnityEngine.Object) player.interactor.CurrentInteraction != (UnityEngine.Object) null)
        player.interactor.CurrentInteraction.IndicateHighlighted(player);
    }
    foreach (KeyValuePair<SkeletonAnimation, bool> spine in this.spines)
      spine.Key.UseDeltaTime = spine.Value;
    this.transform.rotation = this.restoreCameraRotationOnDisable;
    this.transform.position = this.restoreCameraPositionOnDisable;
    AudioManager.Instance.ResumePausedLoopsAndSFX();
  }

  public void OnFocusSliderChanged(float focusAxis)
  {
    focusAxis *= Time.unscaledDeltaTime * this.takePhotoOverlay.FocusSlider.maxValue;
    this.takePhotoOverlay.FocusSlider.value += focusAxis;
    BiomeConstants.Instance.DepthOfFieldTween(0.0f, Mathf.Lerp(0.0f, 8.7f, (float) (1.0 - (double) this.takePhotoOverlay.FocusSlider.value / 100.0)), 26f, 1f, 200f);
  }

  public void OnTiltSliderChanged(float tiltAxis, bool mouse)
  {
    if (SettingsManager.Settings.Accessibility.InvertMovement)
      tiltAxis = -tiltAxis;
    if (!mouse)
      tiltAxis *= Time.unscaledDeltaTime * this.takePhotoOverlay.TiltSlider.maxValue;
    this.takePhotoOverlay.TiltSlider.value += tiltAxis;
    this.transform.eulerAngles = new Vector3(Mathf.Lerp(-70f, -20f, (float) (1.0 - (double) this.takePhotoOverlay.TiltSlider.value / 100.0)), 0.0f, 0.0f);
  }

  public void Update()
  {
    if ((PhotoModeManager.PhotoModeActive || PhotoModeManager.TakeScreenShotActive) && !this.isErrorShown)
      Time.timeScale = 0.0f;
    if (!PhotoModeManager.PhotoModeActive || this.isErrorShown || PhotoModeManager.TakeScreenShotActive || PhotoModeManager.CurrentPhotoState != PhotoModeManager.PhotoState.TakePhoto || PhotoModeManager.TakeScreenShotActive || (UnityEngine.Object) this.takePhotoOverlay == (UnityEngine.Object) null)
      return;
    Vector2 vector2 = new Vector2(InputManager.Gameplay.GetHorizontalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), InputManager.Gameplay.GetVerticalAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
    if ((double) Mathf.Abs(vector2.x) > 0.0)
    {
      this.speed.x = Mathf.Lerp(this.speed.x, vector2.x * this.maxMoveSpeed, this.acceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectX.color = StaticColors.GreenColor;
    }
    else
    {
      this.speed.x = Mathf.Lerp(this.speed.x, 0.0f, this.deceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectX.color = StaticColors.GreyColor;
    }
    this.minHeight = !Physics.Raycast(this.transform.position + Vector3.back * 20f, Vector3.forward, out this.hit, float.PositiveInfinity) ? -1f : this.hit.point.z - 1f;
    if ((double) this.transform.position.z > (double) this.minHeight)
      this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.minHeight);
    if ((double) Mathf.Abs(vector2.y) > 0.0)
    {
      this.speed.y = Mathf.Lerp(this.speed.y, vector2.y * this.maxMoveSpeed, this.acceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectY.color = StaticColors.GreenColor;
    }
    else
    {
      this.speed.y = Mathf.Lerp(this.speed.y, 0.0f, this.deceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectY.color = StaticColors.GreyColor;
    }
    if ((double) InputManager.PhotoMode.GetCameraHeightAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) < 0.0 && (double) this.transform.position.z + (double) this.speed.z * (double) Time.unscaledDeltaTime < (double) this.minHeight)
    {
      this.speed.z = Mathf.Lerp(this.speed.z, 1f * this.maxMoveSpeed, this.acceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectZ.color = StaticColors.GreenColor;
    }
    else if ((InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) ? ((double) InputManager.PhotoMode.GetCameraHeightAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) > 0.0 ? 1 : 0) : (InputManager.UI.GetAcceptButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer) ? 1 : 0)) != 0 && (double) this.transform.position.z + (double) this.speed.z * (double) Time.unscaledDeltaTime > -11.0)
    {
      this.speed.z = Mathf.Lerp(this.speed.z, -1f * this.maxMoveSpeed, this.acceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectZ.color = StaticColors.GreenColor;
    }
    else
    {
      this.speed.z = Mathf.Lerp(this.speed.z, 0.0f, this.deceleration * Time.unscaledDeltaTime);
      this.takePhotoOverlay.TransformObjectZ.color = StaticColors.GreyColor;
    }
    if (InputManager.General.InputIsController(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
    {
      this.OnTiltSliderChanged(InputManager.PhotoMode.GetCameraTiltAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), false);
      this.OnFocusSliderChanged(InputManager.PhotoMode.GetFocusAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
    }
    else
    {
      this.OnFocusSliderChanged(InputManager.PhotoMode.GetFocusAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer));
      if (InputManager.General.MouseInputActive)
      {
        if (InputManager.Gameplay.GetInteract2ButtonHeld(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
          this.OnTiltSliderChanged(InputManager.PhotoMode.GetCameraTiltAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), true);
      }
      else
        this.OnTiltSliderChanged(InputManager.PhotoMode.GetCameraTiltAxis(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer), false);
    }
    if (InputManager.PhotoMode.GetTakePhotoButtonDown(MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer))
      this.TakeScreenshot();
    this.transform.position += this.speed * Time.unscaledDeltaTime;
    Vector3 position = this.transform.position;
    position.x = Mathf.Clamp(position.x, this.GetBoundsForLocation().x, this.GetBoundsForLocation().y);
    position.y = Mathf.Clamp(position.y, this.GetBoundsForLocation().z, this.GetBoundsForLocation().w);
    this.transform.position = position;
  }

  public IEnumerator FrameDelay(System.Action callback)
  {
    yield return (object) new WaitForEndOfFrame();
    System.Action action = callback;
    if (action != null)
      action();
  }

  public void TakeScreenshot()
  {
    PhotoModeManager.TakeScreenShotActive = true;
    this.StartCoroutine((IEnumerator) this.TakeScreenshotRoutine());
  }

  public IEnumerator TakeScreenshotRoutine()
  {
    Debug.Log((object) "Screenshot (PC): AfterImageEffects (no UI)");
    int w = Screen.width;
    int h = Screen.height;
    Camera cam = this.Camera;
    RenderTexture captureRT = RenderTexture.GetTemporary(w, h, 0, RenderTextureFormat.ARGB32);
    captureRT.antiAliasing = 1;
    captureRT.Create();
    CommandBuffer cb = new CommandBuffer()
    {
      name = "Photo Capture After Post (No UI)"
    };
    cb.Blit((RenderTargetIdentifier) BuiltinRenderTextureType.CurrentActive, (RenderTargetIdentifier) (Texture) captureRT);
    cam.AddCommandBuffer(CameraEvent.AfterImageEffects, cb);
    yield return (object) new WaitForEndOfFrame();
    cam.RemoveCommandBuffer(CameraEvent.AfterImageEffects, cb);
    cb.Release();
    int num = SystemInfo.graphicsUVStartsAtTop ? 1 : 0;
    RenderTexture src = captureRT;
    RenderTexture flipRT = (RenderTexture) null;
    if (num != 0)
    {
      flipRT = RenderTexture.GetTemporary(w, h, 0, captureRT.format);
      Graphics.Blit((Texture) captureRT, flipRT, new Vector2(1f, -1f), new Vector2(0.0f, 1f));
      src = flipRT;
    }
    bool done = false;
    bool err = false;
    byte[] bytes = (byte[]) null;
    AsyncGPUReadback.Request((Texture) src, 0, TextureFormat.RGB24, (Action<AsyncGPUReadbackRequest>) (req =>
    {
      if (req.hasError)
        err = true;
      else
        bytes = req.GetData<byte>().ToArray();
      if ((bool) (UnityEngine.Object) flipRT)
        RenderTexture.ReleaseTemporary(flipRT);
      RenderTexture.ReleaseTemporary(captureRT);
      done = true;
    }));
    while (!done)
      yield return (object) null;
    if (!err && bytes != null)
    {
      Texture2D screenshot = new Texture2D(w, h, TextureFormat.RGB24, false);
      PhotoModeManager.PhotoData photoData = this.GetScreenShotData();
      screenshot.name = photoData.PhotoName;
      screenshot.LoadRawTextureData(bytes);
      screenshot.Apply(false, false);
      PhotoModeCamera.PhotoReadWriteResult result = PhotoModeCamera.PhotoReadWriteResult.None;
      COTLImageReadWriter imageReadWriter1 = PhotoModeManager.ImageReadWriter;
      imageReadWriter1.OnWriteError = imageReadWriter1.OnWriteError + (Action<MMReadWriteError>) (_ => result = PhotoModeCamera.PhotoReadWriteResult.Error);
      COTLImageReadWriter imageReadWriter2 = PhotoModeManager.ImageReadWriter;
      imageReadWriter2.OnWriteCompleted = imageReadWriter2.OnWriteCompleted + (System.Action) (() => result = PhotoModeCamera.PhotoReadWriteResult.Success);
      PhotoModeManager.ImageReadWriter.Write(screenshot, photoData.PhotoName);
      Action<string> onPhotoSaved = PhotoModeManager.OnPhotoSaved;
      if (onPhotoSaved != null)
        onPhotoSaved(photoData.PhotoName);
      while (result == PhotoModeCamera.PhotoReadWriteResult.None)
        yield return (object) null;
      COTLImageReadWriter imageReadWriter3 = PhotoModeManager.ImageReadWriter;
      imageReadWriter3.OnWriteError = imageReadWriter3.OnWriteError - (Action<MMReadWriteError>) (_ => result = PhotoModeCamera.PhotoReadWriteResult.Error);
      COTLImageReadWriter imageReadWriter4 = PhotoModeManager.ImageReadWriter;
      imageReadWriter4.OnWriteCompleted = imageReadWriter4.OnWriteCompleted - (System.Action) (() => result = PhotoModeCamera.PhotoReadWriteResult.Success);
      if (result == PhotoModeCamera.PhotoReadWriteResult.Error)
      {
        this.IsErrorShown = true;
        PhotoModeManager.ImageReadWriter.Delete(photoData.PhotoName);
        UIMenuConfirmationWindow menu = UIMenuBase.ActiveMenus.LastElement<UIMenuBase>().Push<UIMenuConfirmationWindow>(MonoSingleton<UIManager>.Instance.ConfirmationWindowTemplate);
        menu.Configure(ScriptLocalization.UI_PhotoMode_Error_Write.Heading, ScriptLocalization.UI_PhotoMode_Error_Write.Heading, true);
        UIMenuConfirmationWindow confirmationWindow = menu;
        confirmationWindow.OnHide = confirmationWindow.OnHide + (System.Action) (() =>
        {
          this.IsErrorShown = false;
          MonoSingleton<UINavigatorNew>.Instance.Clear();
          UnityEngine.Object.Destroy((UnityEngine.Object) screenshot);
          screenshot = (Texture2D) null;
        });
        yield return (object) menu.YieldUntilHidden();
        PhotoModeManager.TakeScreenShotActive = false;
      }
      else
        PhotoModeManager.PhotoTaken(screenshot);
    }
  }

  public PhotoModeManager.PhotoData GetScreenShotData()
  {
    string filename;
    do
    {
      ++PersistenceManager.PersistentData.PhotoModePictureIndex;
      string input = $"Photo_{DataManager.Instance.CultName}_{PersistenceManager.PersistentData.PhotoModePictureIndex.ToString()}";
      filename = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()))}]").Replace(input, "");
    }
    while (PhotoModeManager.ImageReadWriter.FileExists(filename));
    return new PhotoModeManager.PhotoData()
    {
      PhotoName = filename,
      PhotoTexture = (Texture2D) null
    };
  }

  public Texture2D FlipTexture(Texture2D original, bool flipX, bool flipY)
  {
    Texture2D texture2D = new Texture2D(original.width, original.height);
    for (int x1 = 0; x1 < original.width; ++x1)
    {
      for (int y1 = 0; y1 < original.height; ++y1)
      {
        int x2 = flipX ? original.width - 1 - x1 : x1;
        int y2 = flipY ? original.height - 1 - y1 : y1;
        texture2D.SetPixel(x2, y2, original.GetPixel(x1, y1));
      }
    }
    texture2D.Apply();
    return texture2D;
  }

  public Vector4 GetBoundsForLocation()
  {
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Church:
        return new Vector4(-10f, 10f, -20f, -5f);
      case FollowerLocation.Base:
        return new Vector4(PlacementRegion.X_Constraints.x, PlacementRegion.X_Constraints.y, PlacementRegion.Y_Constraints.x, PlacementRegion.Y_Constraints.y + 5f);
      case FollowerLocation.HubShore:
        return new Vector4(-30f, 30f, SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter ? -110f : -30f, 15f);
      case FollowerLocation.DoorRoom:
        return new Vector4(-25f, 25f, 20f, 50f);
      case FollowerLocation.DLC_ShrineRoom:
        return new Vector4(-50f, 50f, 5f, 50f);
      default:
        return new Vector4(-30f, 30f, -30f, 30f);
    }
  }

  [CompilerGenerated]
  public void \u003COnPhotoModeEnabled\u003Eb__27_0()
  {
    this.takePhotoOverlay = MonoSingleton<UIManager>.Instance.TakePhotoOverlayTemplate.Instantiate<UITakePhotoOverlayController>();
    this.takePhotoOverlay.gameObject.SetActive(true);
    this.takePhotoOverlay.Show();
    UITakePhotoOverlayController takePhotoOverlay = this.takePhotoOverlay;
    takePhotoOverlay.OnHidden = takePhotoOverlay.OnHidden + (System.Action) (() =>
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) this.takePhotoOverlay.gameObject);
      this.takePhotoOverlay = (UITakePhotoOverlayController) null;
      PhotoModeManager.DisablePhotoMode();
    });
  }

  [CompilerGenerated]
  public void \u003COnPhotoModeEnabled\u003Eb__27_1()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.takePhotoOverlay.gameObject);
    this.takePhotoOverlay = (UITakePhotoOverlayController) null;
    PhotoModeManager.DisablePhotoMode();
  }

  public enum PhotoReadWriteResult
  {
    None,
    Success,
    Error,
  }
}
