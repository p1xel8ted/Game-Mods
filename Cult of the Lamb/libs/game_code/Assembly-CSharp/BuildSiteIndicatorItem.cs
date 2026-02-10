// Decompiled with JetBrains decompiler
// Type: BuildSiteIndicatorItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
