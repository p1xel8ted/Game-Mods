// Decompiled with JetBrains decompiler
// Type: LoadingGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class LoadingGUI : MonoBehaviour
{
  public float anim_time = 0.3f;
  public static LoadingGUI _me;
  public static UIWidget _widget;
  public static bool _shown;
  public GameObject progress_bar_go;
  public UI2DSprite progress_bar;
  public static LocalizedLabel _localized_label;
  public bool _inited;
  public AsyncOperation _async;

  public void Init()
  {
    if (this._inited)
      return;
    this._inited = true;
    LoadingGUI._widget = this.GetComponentInChildren<UIWidget>();
    LoadingGUI._localized_label = this.GetComponentInChildren<LocalizedLabel>();
    LoadingGUI._me = this;
    LoadingGUI._me.gameObject.SetActive(false);
  }

  public static void Show(GJCommons.VoidDelegate on_anim_played = null)
  {
    Debug.Log((object) "Loading GUI: Show");
    if (LoadingGUI._shown)
    {
      Debug.LogWarning((object) "loading gui already shown");
    }
    else
    {
      if ((Object) LoadingGUI._me == (Object) null)
      {
        LoadingGUI._me = GUIElements.me.loading;
        LoadingGUI._me.Init();
      }
      LoadingGUI._me.progress_bar_go.SetActive(false);
      LoadingGUI._shown = true;
      LoadingGUI._localized_label.Localize();
      LoadingGUI._me.gameObject.SetActive(true);
      LoadingGUI._me.GetComponent<UIPanel>().ChangeAlpha(0.0f, 1f, LoadingGUI._me.anim_time, (GJCommons.VoidDelegate) (() =>
      {
        TitleScreen.Hide();
        if (on_anim_played == null)
          return;
        on_anim_played();
      }));
    }
  }

  public static void ShowWithProgressBar()
  {
    LoadingGUI.Show();
    LoadingGUI._me._async = (AsyncOperation) null;
    LoadingGUI.ShowProgressBar();
  }

  public static void ShowProgressBar()
  {
    LoadingGUI._me.progress_bar_go.SetActive(true);
    LoadingGUI.SetProgressBar(0.0f);
  }

  public static void SetProgressBar(float progress)
  {
    Debug.Log((object) ("Loading: SetProgressBar = " + progress.ToString()));
    LoadingGUI._me.progress_bar.fillAmount = progress;
    Preloader.SetProgressBar(progress);
  }

  public static void IncreaseProgressBar(float amount = 0.05f)
  {
    LoadingGUI.SetProgressBar(LoadingGUI._me.progress_bar.fillAmount + amount);
  }

  public static void LinkAsyncProcess(AsyncOperation async) => LoadingGUI._me._async = async;

  public void Update()
  {
    if (!this.progress_bar_go.activeSelf || this._async == null)
      return;
    float progress = this._async.progress;
    this.progress_bar.fillAmount = progress;
    Preloader.SetProgressBar(progress);
  }

  public static void Hide(GJCommons.VoidDelegate on_anim_played = null)
  {
    Debug.Log((object) "Loading GUI: Hide");
    if (!LoadingGUI._shown)
      Debug.LogError((object) "loading gui is not shown");
    else if (Preloader.is_shown)
    {
      LoadingGUI.HideImmediate();
      on_anim_played.TryInvoke();
    }
    else
      GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() => LoadingGUI._me.GetComponent<UIPanel>().ChangeAlpha(LoadingGUI._me.GetComponent<UIPanel>().alpha, 0.0f, LoadingGUI._me.anim_time, (GJCommons.VoidDelegate) (() =>
      {
        LoadingGUI._shown = false;
        on_anim_played.TryInvoke();
        LoadingGUI._me.gameObject.SetActive(false);
      }))));
  }

  public static void HideImmediate()
  {
    LoadingGUI._shown = false;
    LoadingGUI._me.gameObject.SetActive(false);
  }

  public static void ShowBlackBackground(bool vis, bool animated = false)
  {
    if (!animated)
    {
      GUIElements.me.black_background.gameObject.SetActive(vis);
      if (!vis)
        return;
      GUIElements.me.black_background.alpha = 1f;
    }
    else
    {
      GUIElements.me.black_background.gameObject.SetActive(true);
      GUIElements.me.black_background.alpha = vis ? 0.0f : 1f;
      GUIElements.me.black_background.ChangeAlpha(GUIElements.me.black_background.alpha, vis ? 1f : 0.0f, LoadingGUI._me.anim_time, (GJCommons.VoidDelegate) (() =>
      {
        if (vis)
          return;
        GUIElements.me.black_background.gameObject.SetActive(false);
      }));
    }
  }

  public static bool is_shown => LoadingGUI._shown;
}
