// Decompiled with JetBrains decompiler
// Type: BuildSiteIndicatorItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
