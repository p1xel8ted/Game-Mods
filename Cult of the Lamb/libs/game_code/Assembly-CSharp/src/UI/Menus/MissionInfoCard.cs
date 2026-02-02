// Decompiled with JetBrains decompiler
// Type: src.UI.Menus.MissionInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.Menus;

public class MissionInfoCard : UIInfoCardBase<FollowerInfo>
{
  public Action<InventoryItem.ITEM_TYPE> OnMissionSelected;
  [Header("Mission Info Card")]
  [SerializeField]
  public TextMeshProUGUI _followerNameText;
  [SerializeField]
  public SkeletonGraphic _followerSpine;
  [SerializeField]
  public TextMeshProUGUI _reasonForOddsText;
  [SerializeField]
  public MissionButton[] _missionButtons;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public LayoutElement _scrollRectLayoutGroup;
  public FollowerInfo _followerInfo;

  public SkeletonGraphic FollowerSpine => this._followerSpine;

  public MissionButton[] MissionButtons => this._missionButtons;

  public void SetScrollActive(bool state)
  {
    this._scrollRect.verticalScrollbar.interactable = state;
  }

  public void Start()
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

  public override void DoShow(bool instant)
  {
    base.DoShow(instant);
    this._scrollRect.content.anchoredPosition = Vector2.zero;
  }

  public override void Configure(FollowerInfo config)
  {
    this._scrollRect.verticalScrollbar.value = 1f;
    this.SetScrollActive(false);
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
    if (FollowerBrain.GetOrCreateBrain(config).Info.IsDrunk)
      this._reasonForOddsText.text += (string.Format(LocalizationManager.GetTranslation("UI/MatingTent/Drunk"), (object) config.Name) + "\n").Replace("<color=#FFD201>", "").Replace("</color>", "");
    if (FollowerBrain.GetOrCreateBrain(config).Info.HasTrait(FollowerTrait.TraitType.MissionaryTerrified))
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_BadTrait\"> <sprite name=\"icon_BadTrait\"> <sprite name=\"icon_BadTrait\"> {LocalizationManager.GetTranslation("Traits/MissionaryTerrified")}\n";
    }
    if (FollowerBrain.GetOrCreateBrain(config).Info.HasTrait(FollowerTrait.TraitType.MissionaryInspired))
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"> {LocalizationManager.GetTranslation("Traits/MissionaryInspired")}\n";
    }
    if (FollowerBrain.GetOrCreateBrain(config).Info.HasTrait(FollowerTrait.TraitType.MissionaryExcited))
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"> {LocalizationManager.GetTranslation("Traits/MissionaryExcited")}\n";
    }
    if (FollowerBrainStats.BrainWashed || config.SozoBrainshed)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"> <sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/Brainwashed"), (object) config.Name)}\n";
    }
    if (config.Necklace == InventoryItem.ITEM_TYPE.Necklace_Missionary)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation("UI/MissionaryScreen/ReasonsForProbability/MissionaryNecklace"), (object) config.Name)}\n";
    }
    if (DataManager.Instance.NextMissionarySuccessful)
    {
      TextMeshProUGUI reasonForOddsText = this._reasonForOddsText;
      reasonForOddsText.text = $"{reasonForOddsText.text}<sprite name=\"icon_GoodTrait\"> <sprite name=\"icon_GoodTrait\"> <sprite name=\"icon_GoodTrait\"> {string.Format(LocalizationManager.GetTranslation($"Manipulations/{(Enum) WorldManipulatorManager.Manipulations.MissionarySuccessful}/Notification"), (object) LocalizationManager.GetTranslation("UI/Twitch/ThanksTwitchChat"))}";
    }
    int num1 = 1;
    if (StructureManager.GetAllStructuresOfType<Structures_Missionary>().Count > 0)
    {
      switch (StructureManager.GetAllStructuresOfType<Structures_Missionary>()[0].Data.Type)
      {
        case StructureBrain.TYPES.MISSIONARY_II:
          num1 = 2;
          break;
        case StructureBrain.TYPES.MISSIONARY_III:
          num1 = 3;
          break;
      }
    }
    foreach (MissionButton missionButton in this._missionButtons)
    {
      if (missionButton.Type == InventoryItem.ITEM_TYPE.FOLLOWERS && num1 < 2 || missionButton.Type == InventoryItem.ITEM_TYPE.BONE && num1 < 2 || missionButton.Type == InventoryItem.ITEM_TYPE.SEEDS && num1 < 2 || missionButton.Type == InventoryItem.ITEM_TYPE.LOG_REFINED && num1 < 3 || missionButton.Type == InventoryItem.ITEM_TYPE.STONE_REFINED && num1 < 3)
        missionButton.gameObject.SetActive(false);
      if (!DataManager.Instance.OnboardedRotstone && missionButton.Type == InventoryItem.ITEM_TYPE.MAGMA_STONE)
        missionButton.gameObject.SetActive(false);
      if (!SeasonsManager.Active && missionButton.Type == InventoryItem.ITEM_TYPE.LIGHTNING_SHARD)
        missionButton.gameObject.SetActive(false);
      if (!DataManager.Instance.OnboardedWool && missionButton.Type == InventoryItem.ITEM_TYPE.WOOL)
        missionButton.gameObject.SetActive(false);
      if (!DataManager.Instance.CollectedYewMutated && missionButton.Type == InventoryItem.ITEM_TYPE.YEW_CURSED)
        missionButton.gameObject.SetActive(false);
      missionButton.Configure(config);
    }
    int num2 = 0;
    for (int index = 0; index < this._scrollRect.content.childCount; ++index)
    {
      if (this._scrollRect.content.GetChild(index).gameObject.activeSelf)
        ++num2;
    }
    this._scrollRectLayoutGroup.preferredHeight = (float) Mathf.Min(450, 20 + 70 * num2);
    this._scrollRect.verticalScrollbar.gameObject.SetActive(num2 > 4);
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

  [CompilerGenerated]
  public void \u003CStart\u003Eb__13_0(InventoryItem.ITEM_TYPE itemType)
  {
    Action<InventoryItem.ITEM_TYPE> onMissionSelected = this.OnMissionSelected;
    if (onMissionSelected == null)
      return;
    onMissionSelected(itemType);
  }
}
