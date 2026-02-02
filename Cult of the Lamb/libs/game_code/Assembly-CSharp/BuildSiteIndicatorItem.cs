// Decompiled with JetBrains decompiler
// Type: BuildSiteIndicatorItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
public class BuildSiteIndicatorItem : BaseMonoBehaviour
{
  public InventoryItemDisplay item;
  public Text Text;

  public void Init(InventoryItem.ITEM_TYPE Type, int Quantity)
  {
    this.item.SetImage(Type);
    this.Text.text = Quantity.ToString();
  }
}
