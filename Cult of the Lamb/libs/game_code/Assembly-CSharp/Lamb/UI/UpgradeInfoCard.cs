// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeInfoCard : UIInfoCardBase<UpgradeSystem.Type>
{
  [Header("Copy")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [Header("Costs")]
  [SerializeField]
  public GameObject _costHeader;
  [SerializeField]
  public TextMeshProUGUI _costText;
  [Header("Graphics")]
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public SpriteAtlas _atlas;

  public override void Configure(UpgradeSystem.Type config)
  {
    this._icon.sprite = this._atlas.GetSprite(config.ToString());
    this._headerText.text = UpgradeSystem.GetLocalizedName(config);
    this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(config);
    if (UpgradeSystem.GetUnlocked(config))
    {
      this._costHeader.SetActive(false);
      this._costText.text = "";
    }
    else
      this._costText.text = StructuresData.ItemCost.GetCostStringWithQuantity(UpgradeSystem.GetCost(config));
  }
}
