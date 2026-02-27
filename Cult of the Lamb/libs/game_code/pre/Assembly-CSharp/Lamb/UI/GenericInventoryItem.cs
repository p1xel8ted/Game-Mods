// Decompiled with JetBrains decompiler
// Type: Lamb.UI.GenericInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI.Alerts;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace Lamb.UI;

public class GenericInventoryItem : UIInventoryItem, ISelectHandler, IEventSystemHandler
{
  [SerializeField]
  protected InventoryAlert _alert;

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
