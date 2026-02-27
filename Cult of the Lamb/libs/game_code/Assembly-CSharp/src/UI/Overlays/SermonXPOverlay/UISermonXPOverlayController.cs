// Decompiled with JetBrains decompiler
// Type: src.UI.Overlays.SermonXPOverlay.UISermonXPOverlayController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.Extensions;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace src.UI.Overlays.SermonXPOverlay;

public class UISermonXPOverlayController : UIMenuBase
{
  [SerializeField]
  public RectTransform _xpItemsContainer;
  [SerializeField]
  public SermonXPItem _sermonXPItemTemplate;
  public List<SermonXPItem> _sermonXPItems = new List<SermonXPItem>();

  public void Show(List<FollowerBrain> followers, bool instant = false)
  {
    foreach (FollowerBrain follower in followers)
    {
      SermonXPItem sermonXpItem = this._sermonXPItemTemplate.Instantiate<SermonXPItem>();
      sermonXpItem.Configure(follower, this._xpItemsContainer);
      sermonXpItem.RectTransform.SetParent((Transform) this._xpItemsContainer);
      this._sermonXPItems.Add(sermonXpItem);
    }
    this.Show(instant);
  }

  public override void UpdateSortingOrder()
  {
    this._canvas.sortingOrder = HUD_Manager.Instance.GetComponentInParent<Canvas>().sortingOrder - 1;
  }

  public void UpdateXPItem(FollowerBrain followerBrain, int level)
  {
    foreach (SermonXPItem sermonXpItem in this._sermonXPItems)
    {
      if (sermonXpItem.FollowerBrain == followerBrain)
      {
        sermonXpItem.UpdateCount(level);
        break;
      }
    }
  }

  public List<SermonXPItem> GetAllItems()
  {
    return new List<SermonXPItem>((IEnumerable<SermonXPItem>) this._sermonXPItems);
  }

  public override void OnHideCompleted() => Object.Destroy((Object) this.gameObject);
}
