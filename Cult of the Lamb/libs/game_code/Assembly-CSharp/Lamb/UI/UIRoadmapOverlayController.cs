// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIRoadmapOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Beffio.Dithering;
using LightShaft.Scripts;
using src.UINavigator;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIRoadmapOverlayController : UIMenuBase
{
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public CanvasGroup _subCanvasGroup;
  [SerializeField]
  public GameObject[] _trailers;
  [CompilerGenerated]
  public YoutubePlayer \u003CYouTubePlayer\u003Ek__BackingField;

  public CanvasGroup SubCanvasGroup => this._subCanvasGroup;

  public YoutubePlayer YouTubePlayer
  {
    get => this.\u003CYouTubePlayer\u003Ek__BackingField;
    set => this.\u003CYouTubePlayer\u003Ek__BackingField = value;
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    MonoSingleton<UINavigatorNew>.Instance.Clear();
    UIManager.PlayAudio("event:/ui/open_menu");
    this._scrollRect.normalizedPosition = (Vector2) Vector3.one;
    this.ShowTrailers();
  }

  public void ShowTrailers()
  {
    foreach (GameObject trailer in this._trailers)
      trailer.gameObject.SetActive(false);
    foreach (GameObject trailer in this._trailers)
      trailer.gameObject.SetActive(Application.internetReachability != 0);
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
    this._scrollRect.enabled = false;
  }

  public override void OnCancelButtonInput()
  {
    if ((Object) this.YouTubePlayer != (Object) null)
    {
      Camera.main.GetComponent<Stylizer>().enabled = true;
      this.YouTubePlayer.ToogleFullsScreenMode();
      this.YouTubePlayer.Stop();
      this.YouTubePlayer = (YoutubePlayer) null;
      this.ShowTrailers();
    }
    else
    {
      if (!this._canvasGroup.interactable)
        return;
      AudioManager.Instance.PlayMusic("event:/music/menu/menu_title");
      this.Hide();
    }
  }

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
