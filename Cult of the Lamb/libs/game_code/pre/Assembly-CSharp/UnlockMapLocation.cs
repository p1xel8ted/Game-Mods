// Decompiled with JetBrains decompiler
// Type: UnlockMapLocation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class UnlockMapLocation : BaseMonoBehaviour
{
  public FollowerLocation Location;
  public bool ReReveal;
  public UnityEvent Callback;

  public void Play()
  {
    if (!this.ReReveal && !DataManager.Instance.DiscoverLocation(this.Location))
      return;
    UIWorldMapMenuController mapMenuController = MonoSingleton<UIManager>.Instance.ShowWorldMap();
    mapMenuController.Show(this.Location, this.ReReveal);
    mapMenuController.OnHidden = mapMenuController.OnHidden + (System.Action) (() =>
    {
      Debug.Log((object) ("AAAA " + (object) this.Callback.GetPersistentEventCount()));
      this.Callback?.Invoke();
    });
  }
}
