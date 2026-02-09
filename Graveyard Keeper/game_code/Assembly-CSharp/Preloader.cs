// Decompiled with JetBrains decompiler
// Type: Preloader
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#nullable disable
public class Preloader : MonoBehaviour
{
  public TitleScreenCamera camera;
  public static Preloader me;
  public static bool is_shown;
  public UI2DSprite progress_bar;
  public static bool awake_was_done;
  public GameObject error_go;
  public UILabel error_txt;

  public void Awake()
  {
    PlatformSpecific.FixScreenModeAfterStart();
    Debug.Log((object) ("****************************************\n*** Starting Graveyard Keeper, ver. " + LazyConsts.VERSION.ToString()));
    Debug.Log((object) $"Init: device name : {SystemInfo.graphicsDeviceName}, type: {SystemInfo.deviceType.ToString()}, model: {SystemInfo.deviceModel}, ver: {SystemInfo.graphicsDeviceVersion}, screen: {Screen.width.ToString()}x{Screen.height.ToString()}", (UnityEngine.Object) this);
    Debug.Log((object) ("Preloader.Awake, time = " + Time.time.ToString()), (UnityEngine.Object) this);
    Debug.Log((object) ("Current system language: " + Application.systemLanguage.ToString()));
    Debug.Log((object) $"Stranger Sins DLC is available: {DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Stories)}");
    Debug.Log((object) $"Game of Crone DLC is available: {DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Refugees)}");
    Debug.Log((object) $"Better Save Soul DLC is available: {DLCEngine.IsDLCAvailable(DLCEngine.DLCVersion.Souls)}");
    if ((UnityEngine.Object) this.error_go != (UnityEngine.Object) null)
      this.error_go.SetActive(false);
    if (GameSettings.me == null)
    {
      Debug.LogError((object) "Error loading game settings");
    }
    else
    {
      ResolutionConfig.InitResolutions();
      GameSettings.me.ApplyScreenMode();
      Stats.Init();
      Stats.DesignEvent("Initialize", LazyConsts.VERSION);
      Preloader.me = this;
      Preloader.is_shown = true;
      Preloader.awake_was_done = true;
      Preloader.SetProgressBar(0.0f);
      new GameObject("Resolution Helper").AddComponent<ResolutionHelper>();
      new GameObject("MouseCursorAutoHide").AddComponent<MouseCursorAutoHide>();
      UIRoot reloader_root = this.gameObject.GetComponentInChildren<UIRoot>(true);
      reloader_root.gameObject.SetActive(false);
      int num = 1 & (this.CheckTimezoneBug() ? 1 : 0) & (this.CheckTurkishBug() ? 1 : 0);
      PlatformSpecific.SetDefaultCultureInfo();
      if (num == 0)
        reloader_root.gameObject.SetActive(true);
      else
        GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() =>
        {
          SceneManager.LoadSceneAsync("scene_main", LoadSceneMode.Additive);
          SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(Preloader.OnSceneLoaded);
          GJTimer.AddTimer(0.0f, (GJTimer.VoidDelegate) (() => reloader_root.gameObject.SetActive(true)));
        }));
    }
  }

  public bool CheckTurkishBug()
  {
    Debug.Log((object) ("CheckTurkishBug... " + "i".ToUpper()));
    return true;
  }

  public bool CheckTimezoneBug()
  {
    Debug.Log((object) "CheckTimezoneBug...");
    try
    {
      Debug.Log((object) ("time now = " + DateTime.Now.ToString()));
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("Timezone bug detected: " + ex?.ToString()));
      this.error_go.SetActive(true);
      this.error_txt.text = "Error initializing TimeZone!\n\nPlease, check your current TIME ZONE in your OS settings -- seems that something is wrong with it.\n\nRestart the game after that.";
      return false;
    }
    return true;
  }

  public static void OnSceneLoaded(Scene s, LoadSceneMode mode)
  {
    Debug.Log((object) $"Preloader.OnSceneLoaded: {s.name}, time = {Time.time.ToString()}");
    if (!(s.name == "scene_main"))
      return;
    SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(Preloader.OnSceneLoaded);
  }

  public static void OnMainGameLoaded()
  {
    int num = (UnityEngine.Object) Preloader.me == (UnityEngine.Object) null ? 1 : 0;
  }

  public static void Hide()
  {
    if ((UnityEngine.Object) Preloader.me == (UnityEngine.Object) null)
      return;
    Preloader.is_shown = false;
    NGUITools.Destroy((UnityEngine.Object) Preloader.me.gameObject);
  }

  public static void SetProgressBar(float progress)
  {
    if (!Preloader.is_shown || (UnityEngine.Object) Preloader.me == (UnityEngine.Object) null)
      return;
    Preloader.me.progress_bar.fillAmount = progress;
  }
}
