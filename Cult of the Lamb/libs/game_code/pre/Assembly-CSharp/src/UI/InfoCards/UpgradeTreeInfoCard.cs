// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.UpgradeTreeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Lamb.UI.Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class UpgradeTreeInfoCard : UIInfoCardBase<UpgradeTreeNode>
{
  [Header("Copy")]
  [SerializeField]
  private TextMeshProUGUI _headerText;
  [SerializeField]
  private TextMeshProUGUI _descriptionText;
  [SerializeField]
  private TextMeshProUGUI _requirementsText;
  [Header("Upgrade Category")]
  [SerializeField]
  private TextMeshProUGUI _categoryText;
  [SerializeField]
  private UpgradeCategoryIconMapping _categoryIconMapping;
  [Header("Costs")]
  [Header("Graphics")]
  [SerializeField]
  private UpgradeTypeMapping _upgradeTypeMapping;
  [SerializeField]
  private Image _icon;

  public override void Configure(UpgradeTreeNode node)
  {
    UpgradeSystem.Type upgrade = node.Upgrade;
    this._icon.sprite = this._upgradeTypeMapping.GetItem(upgrade).Sprite;
    this._headerText.text = UpgradeSystem.GetLocalizedName(upgrade);
    this._descriptionText.text = UpgradeSystem.GetLocalizedDescription(upgrade);
    if ((Object) this._upgradeTypeMapping != (Object) null)
    {
      UpgradeTypeMapping.SpriteItem spriteItem = this._upgradeTypeMapping.GetItem(node.Upgrade);
      if ((Object) this._categoryText != (Object) null && (Object) this._categoryIconMapping != (Object) null)
      {
        this._categoryText.text = UpgradeCategoryIconMapping.GetIcon(spriteItem.Category);
        this._categoryText.color = this._categoryIconMapping.GetColor(spriteItem.Category);
      }
    }
    if (node.State == UpgradeTreeNode.NodeState.Locked)
    {
      if (DataManager.Instance.CurrentUpgradeTreeTier <= node.NodeTier || !node.TierConfig.RequiresCentralTier || node.TierConfig.CentralNode != node.Upgrade || node.TreeConfig.NumUnlockedUpgrades() < node.TreeConfig.NumRequiredNodesForTier(node.NodeTier))
        this._requirementsText.text = string.Format(LocalizationManager.GetTranslation("UI/UpgradeTree/RequiredTier"), (object) (int) (node.NodeTier + 1)).Colour(Color.red);
      else if (node.RequiresBuiltStructure != StructureBrain.TYPES.NONE)
        this._requirementsText.text = string.Format(LocalizationManager.GetTranslation("UI/UpgradeTree/Requires"), (object) StructuresData.LocalizedName(node.RequiresBuiltStructure)).Colour(Color.red);
      else
        this._requirementsText.gameObject.SetActive(false);
    }
    else if (node.State == UpgradeTreeNode.NodeState.Unavailable)
    {
      string str = "";
      foreach (UpgradeTreeNode prerequisiteNode in node.PrerequisiteNodes)
        str = $"{str}\n{UpgradeSystem.GetLocalizedName(prerequisiteNode.Upgrade)}";
      this._requirementsText.text = string.Format(LocalizationManager.GetTranslation("UI/UpgradeTree/Requires"), (object) str).Colour(Color.red);
    }
    else
      this._requirementsText.gameObject.SetActive(false);
  }
}
