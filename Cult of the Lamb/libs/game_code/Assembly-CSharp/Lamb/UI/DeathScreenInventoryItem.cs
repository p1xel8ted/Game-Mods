// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DeathScreenInventoryItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class DeathScreenInventoryItem : UIInventoryItem
{
  [SerializeField]
  public TextMeshProUGUI _quantityDelta;
  public InventoryItem Item;

  public TextMeshProUGUI AmountText => this._amountText;

  public TextMeshProUGUI DeltaText => this._quantityDelta;

  public override void Configure(InventoryItem item, bool showQuantity = true)
  {
    this.IgnoreSingles = true;
    base.Configure(item, showQuantity);
    this._quantityDelta.text = "";
    this.Item = item;
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
    this.StartCoroutine(this.FadeNumber());
  }

  public IEnumerator FadeNumber()
  {
    yield return (object) new WaitForSecondsRealtime(3f);
    ShortcutExtensionsTMPText.DOFade(this._quantityDelta, 0.0f, 1f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
  }
}
