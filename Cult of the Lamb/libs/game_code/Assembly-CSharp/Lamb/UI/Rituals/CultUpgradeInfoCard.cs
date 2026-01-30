// Decompiled with JetBrains decompiler
// Type: Lamb.UI.Rituals.CultUpgradeInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Assets;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.Rituals;

public class CultUpgradeInfoCard : UIInfoCardBase<CultUpgradeData.TYPE>
{
  [Header("Copy")]
  [SerializeField]
  public TextMeshProUGUI _headerText;
  [SerializeField]
  public TextMeshProUGUI _descriptionText;
  [SerializeField]
  public Image _icon;
  [SerializeField]
  public Image _locked;
  [SerializeField]
  public Image _iconExample;
  [SerializeField]
  public CultUpgradeIconMapping _iconMapping;
  [SerializeField]
  public CultUpgradeIconMapping _iconExampleMapping;
  [SerializeField]
  public GameObject _equippedThemeNotifier;
  [Header("Costs")]
  [SerializeField]
  public TextMeshProUGUI[] _costTexts;
  [SerializeField]
  public GameObject _costContainer;
  public GameObject _redOutline;

  public override void Configure(CultUpgradeData.TYPE config)
  {
    bool flag1 = CultUpgradeData.IsUnlocked(config);
    bool flag2 = CultUpgradeData.IsBorder(config);
    string yellowColorHex = StaticColors.YellowColorHex;
    if (!flag1 & flag2)
    {
      this._headerText.text = CultUpgradeData.GetLockedText();
      this._descriptionText.text = CultUpgradeData.GetLockedText();
      if (config == CultUpgradeData.TYPE.Border5 && !DataManager.Instance.BossesCompleted.Contains(FollowerLocation.Dungeon1_5))
        this._descriptionText.text = LocalizationManager.GetTranslation("UI/CultUpgrade/Dungeon1_5/Locked");
      else if (config == CultUpgradeData.TYPE.Border6 && !DataManager.Instance.BeatenExecutioner)
        this._descriptionText.text = LocalizationManager.GetTranslation("UI/CultUpgrade/Dungeon1_6/Locked");
      TextMeshProUGUI descriptionText = this._descriptionText;
      descriptionText.text = $"{descriptionText.text}<br><color={yellowColorHex}>{ScriptLocalization.UI_CultUpgrade.UnlocksNewTempleAesthetic}";
    }
    else
    {
      this._headerText.text = CultUpgradeData.GetLocalizedName(config);
      this._descriptionText.text = CultUpgradeData.GetLocalizedDescription(config);
      TextMeshProUGUI descriptionText = this._descriptionText;
      descriptionText.text = $"{descriptionText.text}<br><color={yellowColorHex}>{ScriptLocalization.UI_CultUpgrade.AddsNewTempleDecorations}";
    }
    this._locked.enabled = !flag1;
    this._icon.enabled = flag1;
    this._icon.sprite = this._iconMapping.GetImage(config);
    Sprite image = this._iconExampleMapping.GetImage(config);
    if ((Object) image == (Object) null)
    {
      this._iconExample.gameObject.SetActive(false);
    }
    else
    {
      this._iconExample.gameObject.SetActive(true);
      this._iconExample.sprite = image;
    }
    this._equippedThemeNotifier.gameObject.SetActive((CultUpgradeData.TYPE) DataManager.Instance.TempleBorder == config);
    this._costContainer.SetActive(!flag2);
    List<StructuresData.ItemCost> cost = CultUpgradeData.GetCost(config);
    for (int index = 0; index < this._costTexts.Length; ++index)
    {
      if (index >= cost.Count)
      {
        this._costTexts[index].gameObject.SetActive(false);
      }
      else
      {
        this._costTexts[index].text = cost[index].ToStringShowQuantity();
        this._costTexts[index].gameObject.SetActive(true);
      }
    }
  }
}
