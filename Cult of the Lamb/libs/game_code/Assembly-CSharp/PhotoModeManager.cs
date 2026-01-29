// Decompiled with JetBrains decompiler
// Type: PhotoModeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Data.ReadWrite;
using Lamb.UI;
using System;
using UnityEngine;

#nullable disable
public static class PhotoModeManager
{
  public static bool PhotoModeActive = false;
  public static PhotoModeManager.PhotoState CurrentPhotoState = PhotoModeManager.PhotoState.None;
  public static Action<string> OnPhotoSaved;
  public static Action<string> OnPhotoDeleted;
  public static int ResolutionX = Screen.width;
  public static int ResolutionY = Screen.height;
  public static int ConsoleFileLimit = 14680064 /*0xE00000*/;
  public static int ConsolePhotoLimit = 10;
  public const string StickerPath = "Data/Sticker Data";
  public static COTLImageReadWriter ImageReadWriter = new COTLImageReadWriter();
  public static bool depthOfFieldSetting = false;
  public static bool TakeScreenShotActive = false;

  public static event PhotoModeManager.PhotoEvent OnPhotoModeEnabled;

  public static event PhotoModeManager.PhotoEvent OnPhotoModeDisabled;

  public static event PhotoModeManager.PhotoTakenEvent OnPhotoTaken;

  public static void EnablePhotoMode()
  {
    PhotoModeManager.depthOfFieldSetting = SettingsManager.Settings.Graphics.DepthOfField;
    PhotoModeManager.PhotoModeActive = true;
    PhotoModeManager.PhotoEvent photoModeEnabled = PhotoModeManager.OnPhotoModeEnabled;
    if (photoModeEnabled != null)
      photoModeEnabled();
    Time.timeScale = 0.0f;
    GameManager.GetInstance().WaitForSecondsRealtime(0.1f, (System.Action) (() =>
    {
      AudioManager.Instance.PlayOneShot("event:/ui/photo_mode/camera_focus");
      AudioManager.Instance.ToggleFilter(SoundParams.Filter, true);
      PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.TakePhoto;
    }));
    SettingsManager.Settings.Graphics.DepthOfField = true;
    GraphicsSettingsUtilities.UpdatePostProcessing();
  }

  public static void DisablePhotoMode()
  {
    AudioManager.Instance.ToggleFilter(SoundParams.Filter, false);
    PhotoModeManager.PhotoModeActive = false;
    PhotoModeManager.CurrentPhotoState = PhotoModeManager.PhotoState.None;
    PhotoModeManager.PhotoEvent photoModeDisabled = PhotoModeManager.OnPhotoModeDisabled;
    if (photoModeDisabled != null)
      photoModeDisabled();
    Time.timeScale = 1f;
    SettingsManager.Settings.Graphics.DepthOfField = PhotoModeManager.depthOfFieldSetting;
    GraphicsSettingsUtilities.UpdatePostProcessing();
    SettingsManager.Settings.Graphics.DepthOfField = PhotoModeManager.depthOfFieldSetting;
    GraphicsSettingsUtilities.UpdatePostProcessing();
    MonoSingleton<UIManager>.Instance.UnloadPhotomodeAssets();
  }

  public static void PhotoTaken(Texture2D texture)
  {
    DeviceLightingManager.FlashColor(Color.white);
    AudioManager.Instance.PlayOneShot("event:/ui/photo_mode/camera_click");
    MMVibrate.Haptic(MMVibrate.HapticTypes.Success);
    PhotoModeManager.PhotoTakenEvent onPhotoTaken = PhotoModeManager.OnPhotoTaken;
    if (onPhotoTaken == null)
      return;
    onPhotoTaken(texture);
  }

  public static void DeletePhoto(string filePath)
  {
    PhotoModeManager.ImageReadWriter.Delete(filePath);
    Action<string> onPhotoDeleted = PhotoModeManager.OnPhotoDeleted;
    if (onPhotoDeleted == null)
      return;
    onPhotoDeleted(filePath);
  }

  public static StickerData[] GetAllStickers()
  {
    return Resources.LoadAll<StickerData>("Data/Sticker Data");
  }

  public enum PhotoState
  {
    None,
    TakePhoto,
    Gallery,
    EditPhoto,
  }

  public class PhotoData
  {
    public string PhotoName;
    public Texture2D PhotoTexture;
  }

  public delegate void PhotoEvent();

  public delegate void PhotoTakenEvent(Texture2D photo);
}
