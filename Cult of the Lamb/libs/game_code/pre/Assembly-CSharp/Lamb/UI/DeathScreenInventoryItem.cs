// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DeathScreenInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class DeathScreenInventoryItem : UIInventoryItem
{
  [SerializeField]
  private TextMeshProUGUI _quantityDelta;

  public TextMeshProUGUI AmountText => this._amountText;

  public TextMeshProUGUI DeltaText => this._quantityDelta;

  public override void Configure(InventoryItem item, bool showQuantity = true)
  {
    base.Configure(item, showQuantity);
    this._quantityDelta.text = "";
  }

  public void ShowDelta(int delta)
  {
    this._quantityDelta.text = delta.ToString();
    if (delta > 0)
    {
      this._quantityDelta.text = "+" + this._quantityDelta.text;
      this._quantityDelta.color = StaticColors.GreenColor;
    }
    else if (delta < 0)
      this._quantityDelta.color = StaticColors.RedColor;
    else
      this._quantityDelta.text = "";
  }
}
