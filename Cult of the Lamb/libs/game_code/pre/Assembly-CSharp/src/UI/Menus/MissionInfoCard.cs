// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.MissionInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace src.UI.Menus;

public class MissionInfoCard : UIInfoCardBase<FollowerInfo>
{
  public Action<InventoryItem.ITEM_TYPE> OnMissionSelected;
  [Header("Prison Info Card")]
  [SerializeField]
  private TextMeshProUGUI _followerNameText;
  [SerializeField]
  private SkeletonGraphic _followerSpine;
  [SerializeField]
  private TextMeshProUGUI _reasonForOddsText;
  [SerializeField]
  private MissionButton[] _missionButtons;
  private FollowerInfo _followerInfo;

  public SkeletonGraphic FollowerSpine => this._followerSpine;

  public MissionButton[] MissionButtons => this._missionButtons;

  private void Start()
  {
    foreach (MissionButton missionButton in this._missionButtons)
      missionButton.OnMissionSelected += (Action<InventoryItem.ITEM_TYPE>) (itemType =>
      {
        Action<InventoryItem.ITEM_TYPE> onMissionSelected = this.OnMissionSelected;
        if (onMissionSelected == null)
          return;
        onMissionSelected(itemType);
      });
  }

  public override void Configure(FollowerInfo config)
  {
    if (this._followerInfo == config)
      return;
    this._followerInfo = config;
    this._followerNameText.text = $"{config.Name} {config.XPLevel.ToNumeral()}";
    this._followerSpine.ConfigureFollower(config);
    this._reasonForOddsText.text = "";
    if (config.XPLevel == 1)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_BadTrait\"><sprite name=\"icon_BadTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/NotLevelled"), (object) config.Name)}\n";
    }
    else if (config.XPLevel <= 2)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_BadTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/LowLevel"), (object) config.Name)}\n";
    }
    else if (config.XPLevel <= 4)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/MediumLevel"), (object) config.Name)}\n";
    }
    else if (config.XPLevel > 4)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"><sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/HighLevel"), (object) config.Name)}\n";
    }
    if (FollowerBrain.GetOrCreateBrain(config).CurrentState.Type == FollowerStateType.Exhausted)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_BadTrait\"><sprite name=\"icon_BadTrait\"><sprite name=\"icon_BadTrait\"><sprite name=\"icon_BadTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/Exhausted"), (object) config.Name)}\n";
    }
    if (FollowerBrainStats.BrainWashed)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"><sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/Brainwashed"), (object) config.Name)}\n";
    }
    int num = 1;
    if (StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count > 0)
    {
      switch (StructureManager.GetAllStructuresOfType<Structures_Missionary>()[0].Data.Type)
      {
        case StructureBrain.TYPES.MISSIONARY_II:
          num = 2;
          break;
        case StructureBrain.TYPES.MISSIONARY_III:
          num = 3;
          break;
      }
    }
    foreach (MissionButton missionButton in this._missionButtons)
    {
      if (missionButton.Type == InventoryItem.ITEM_TYPE.FOLLOWERS && num < 2 || missionButton.Type == InventoryItem.ITEM_TYPE.BONE && num < 2 || missionButton.Type == InventoryItem.ITEM_TYPE.SEEDS && num < 2 || missionButton.Type == InventoryItem.ITEM_TYPE.LOG_REFINED && num < 3 || missionButton.Type == InventoryItem.ITEM_TYPE.STONE_REFINED && num < 3)
        missionButton.gameObject.SetActive(false);
      missionButton.Configure(config);
    }
  }

  public MMButton FirstAvailableButton()
  {
    foreach (MissionButton missionButton in this._missionButtons)
    {
      if (missionButton.gameObject.activeInHierarchy)
        return missionButton.Button;
    }
    return (MMButton) null;
  }
}
