// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RelicInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RelicInfoCard : UIInfoCardBase<RelicData>
{
  [Header("Relic Info")]
  [SerializeField]
  public RectTransform _iconContainer;
  [SerializeField]
  public CanvasGroup _iconContainerCanvasgroup;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public GameObject _lock;
  [SerializeField]
  public TextMeshProUGUI _itemHeader;
  [SerializeField]
  public TextMeshProUGUI _itemLore;
  [SerializeField]
  public TextMeshProUGUI _itemDescription;
  [Header("Stats")]
  [SerializeField]
  public GameObject _statsContainer;
  [SerializeField]
  public GameObject _fragileIcon;
  [SerializeField]
  public TextMeshProUGUI _charge;
  public PlayerFarming playerFarming;

  public RectTransform IconContainer => this._iconContainer;

  public CanvasGroup IconCanvasGroup => this._iconContainerCanvasgroup;

  public void Configure(RelicData config, PlayerFarming playerFarming)
  {
    this.playerFarming = playerFarming;
    this.Configure(config);
  }

  public override void Configure(RelicData config)
  {
    if ((Object) config != (Object) null)
    {
      this._icon.sprite = config.Sprite;
      this._icon.gameObject.SetActive(true);
      this._lock.SetActive(false);
      this._itemHeader.text = RelicData.GetTitleLocalisation(config.RelicType);
      this._itemLore.text = LocalizationManager.GetTranslation($"Relics/{config.RelicType}/Lore");
      this._itemDescription.text = RelicData.GetDescriptionLocalisation(config.RelicType);
      this._fragileIcon.SetActive(config.InteractionType == RelicInteractionType.Fragile || (Object) this.playerFarming != (Object) null && TrinketManager.AreRelicsFragile(this.playerFarming));
      this._statsContainer.SetActive(true);
      RelicChargeCategory chargeCategory = RelicData.GetChargeCategory(config);
      if (config.InteractionType == RelicInteractionType.Fragile || (Object) this.playerFarming != (Object) null && TrinketManager.AreRelicsFragile(this.playerFarming))
        this._charge.text = LocalizationManager.GetTranslation("UI/Fragile");
      else
        this._charge.text = $"{LocalizationManager.GetTranslation("UI/Charge")} <b>{RelicData.GetChargeCategoryColor(chargeCategory)}{LocalizationManager.GetTranslation($"UI/{chargeCategory}")}";
    }
    else
    {
      this._icon.gameObject.SetActive(false);
      this._lock.SetActive(true);
      this._itemHeader.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/NoRelic");
      this._itemLore.gameObject.SetActive(false);
      this._itemDescription.text = LocalizationManager.GetTranslation("UI/PauseScreen/Player/NoRelic/Description");
      this._fragileIcon.SetActive(false);
      this._statsContainer.SetActive(false);
    }
  }
}
