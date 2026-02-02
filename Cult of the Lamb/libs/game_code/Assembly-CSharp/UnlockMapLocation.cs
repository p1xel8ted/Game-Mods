// Decompiled with JetBrains decompiler
// Type: UnlockMapLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UnlockMapLocation : BaseMonoBehaviour
{
  public FollowerLocation Location;
  public bool ReReveal;
  public UnityEvent Callback;
  public bool isLoadingAssets;

  public void Play()
  {
    if (this.isLoadingAssets || !this.ReReveal && !DataManager.Instance.DiscoverLocation(this.Location))
      return;
    this.isLoadingAssets = true;
    this.StartCoroutine((IEnumerator) UIManager.LoadAssets(MonoSingleton<UIManager>.Instance.LoadWorldMapAssets(), (System.Action) (() =>
    {
      this.isLoadingAssets = false;
      UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
      mapMenuController.Show(this.Location, this.ReReveal);
      mapMenuController.OnHidden = mapMenuController.OnHidden + (System.Action) (() =>
      {
        Debug.Log((object) ("AAAA " + this.Callback.GetPersistentEventCount().ToString()));
        this.Callback?.Invoke();
      });
    })));
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__4_0()
  {
    this.isLoadingAssets = false;
    UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
    mapMenuController.Show(this.Location, this.ReReveal);
    mapMenuController.OnHidden = mapMenuController.OnHidden + (System.Action) (() =>
    {
      Debug.Log((object) ("AAAA " + this.Callback.GetPersistentEventCount().ToString()));
      this.Callback?.Invoke();
    });
  }

  [CompilerGenerated]
  public void \u003CPlay\u003Eb__4_1()
  {
    Debug.Log((object) ("AAAA " + this.Callback.GetPersistentEventCount().ToString()));
    this.Callback?.Invoke();
  }
}
