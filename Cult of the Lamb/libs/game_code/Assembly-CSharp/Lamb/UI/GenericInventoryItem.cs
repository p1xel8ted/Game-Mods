// Decompiled with JetBrains decompiler
// Type: Lamb.UI.GenericInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class GenericInventoryItem : UIInventoryItem, ISelectHandler, IEventSystemHandler
{
  [SerializeField]
  public InventoryAlert _alert;

  public override void Configure(InventoryItem item, bool showQuantity = true)
  {
    base.Configure(item, showQuantity);
    if (!((Object) this._alert != (Object) null))
      return;
    this._alert.Configure(this.Type);
  }

  public void OnSelect(BaseEventData eventData)
  {
    if (!((Object) this._alert != (Object) null))
      return;
    this._alert.TryRemoveAlert();
  }
}
