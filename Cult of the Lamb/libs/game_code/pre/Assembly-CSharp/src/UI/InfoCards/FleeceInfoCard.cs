// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.FleeceInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.InfoCards;

public class FleeceInfoCard : UIInfoCardBase<int>
{
  [SerializeField]
  private TextMeshProUGUI _itemHeader;
  [SerializeField]
  private TextMeshProUGUI _itemDescription;
  [SerializeField]
  private FleeceItem _fleeceItem;
  [SerializeField]
  private GameObject _costHeader;
  [SerializeField]
  private GameObject _costContainer;
  [SerializeField]
  private TextMeshProUGUI _costText;
  public GameObject _redOutline;

  public override void Configure(int fleece)
  {
    this._redOutline.SetActive(false);
    this._fleeceItem.Configure(fleece);
    this._itemHeader.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{fleece}/Name");
    this._itemDescription.text = LocalizationManager.GetTranslation($"TarotCards/Fleece{fleece}/Description");
    if (!DataManager.Instance.UnlockedFleeces.Contains(fleece))
    {
      this._costText.text = new StructuresData.ItemCost(InventoryItem.ITEM_TYPE.TALISMAN, 1).ToStringShowQuantity();
      this._costHeader.SetActive(true);
      this._costContainer.SetActive(true);
    }
    else
    {
      this._costHeader.SetActive(false);
      this._costContainer.SetActive(false);
    }
  }
}
