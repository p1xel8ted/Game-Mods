// Decompiled with JetBrains decompiler
// Type: Lamb.UI.JobBoardInfoCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Flockade;
using I2.Loc;
using Lamb.UI.Assets;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class JobBoardInfoCard : UIInfoCardBase<JobBoardMenuItem>
{
  [SerializeField]
  public TMP_Text groupTitle;
  [SerializeField]
  public TMP_Text objectiveText;
  [SerializeField]
  public TMP_Text hintText;
  [SerializeField]
  public GameObject untrackedObjective;
  [SerializeField]
  public GameObject trackedObjective;
  [SerializeField]
  public TMP_Text trackedText;
  [SerializeField]
  public TMP_Text rewardText;
  [Header("Decoration Reward")]
  [SerializeField]
  public Image decoImage;
  [SerializeField]
  public Transform decoContainer;
  [Header("Misc Item Reward")]
  [SerializeField]
  public Image iconImage;
  [SerializeField]
  public Transform miscItemContainer;
  [Header("Flockade Reward")]
  [SerializeField]
  public Image flockadePieceImage;
  [SerializeField]
  public Transform flockadePieceContainer;
  [Header("Tarot Reward")]
  [SerializeField]
  public SkeletonGraphic tarot;
  [SerializeField]
  public Transform tarotContainer;
  [Header("Ranch Animal Reward")]
  [SerializeField]
  public SkeletonGraphic spine;
  [SerializeField]
  public Transform spineContainer;
  [Header("Button")]
  [SerializeField]
  public Image buttonImage;
  [SerializeField]
  public Sprite greenButton;
  [SerializeField]
  public GameObject trackedParent;
  public MMButtonClick trackButton;
  public UnityAction<JobBoardMenuItem> OnTrackButtonClicked;
  [Header("Outline")]
  [SerializeField]
  public Image outlineImageBox;
  [SerializeField]
  public Image outlineImageDiamond;
  [SerializeField]
  public Material completedMaterial;
  [SerializeField]
  public Material incompleteMaterial;
  [Header("Misc")]
  [SerializeField]
  public GameObject disabledContainer;
  [SerializeField]
  public GameObject trackContainer;
  [SerializeField]
  public GameObject trackOutline;
  [SerializeField]
  public Transform trackTickContainer;
  [SerializeField]
  public InventoryIconMapping iconMapping;
  public JobBoardMenuItem config;
  public bool _oldTrackState;

  public JobBoardMenuItem Config => this.config;

  public void OnClick()
  {
    Debug.Log((object) ("Clicked on track button for job: " + this.config.Objective.GroupId));
  }

  public override void Configure(JobBoardMenuItem config)
  {
    this.config = config;
    this.trackButton.OnClick.AddListener(new UnityAction(this.OnClick));
    this.groupTitle.text = LocalizationManager.GetTranslation(config.Objective.GroupId);
    this.objectiveText.text = config.Objective.Text;
    this.decoContainer.gameObject.SetActive(false);
    this.miscItemContainer.gameObject.SetActive(false);
    this.flockadePieceContainer.gameObject.SetActive(false);
    this.tarotContainer.gameObject.SetActive(false);
    this.spineContainer.gameObject.SetActive(false);
    this.hintText.gameObject.SetActive(false);
    if (config.Completed)
      this.outlineImageBox.material = this.completedMaterial;
    else
      this.outlineImageBox.material = this.incompleteMaterial;
    if (config.IsDisabled)
      this.disabledContainer.gameObject.SetActive(true);
    else
      this.disabledContainer.gameObject.SetActive(false);
    if (config.JobData.IsRewardItem)
    {
      if (InventoryItem.AllAnimals.Contains(config.JobData.RewardItem))
      {
        this.spineContainer.gameObject.SetActive(true);
        this.spine.ConfigureAnimal(new StructuresData.Ranchable_Animal()
        {
          Type = config.JobData.RewardItem,
          Age = 10,
          Level = 3,
          GrowthStage = 10
        });
        if (!config.Completed)
          this.spine.color = Color.black;
        else
          this.spine.color = Color.white;
        if (config.JobData.RewardItem == InventoryItem.ITEM_TYPE.ANIMAL_LLAMA)
          this.spine.transform.localScale = Vector3.one;
        else
          this.spine.transform.localScale = Vector3.one * 1.5f;
        this.rewardText.text = string.Format(LocalizationManager.GetTranslation("UI/JobBoard/Reward/Animal"), (object) InventoryItem.LocalizedName(config.JobData.RewardItem));
      }
      else
      {
        this.miscItemContainer.gameObject.SetActive(true);
        this.iconMapping.GetImage(config.JobData.RewardItem, this.iconImage);
        this.rewardText.text = InventoryItem.LocalizedName(config.JobData.RewardItem);
        if (config.JobData.RewardItem == InventoryItem.ITEM_TYPE.LORE_STONE)
        {
          this.rewardText.text = LocalizationManager.GetTranslation("Inventory/LAMB_LORE");
          TMP_Text rewardText = this.rewardText;
          rewardText.text = $"{rewardText.text}<br>{LocalizationManager.GetTranslation($"UI/JobBoard/Lore/Description/{config.Index}")}";
        }
        else if (config.JobData.RewardItem == InventoryItem.ITEM_TYPE.WOOL)
        {
          EquipmentData equipmentData = EquipmentManager.GetEquipmentData((config.Objective as Objectives_LegendaryWeaponRun).LegendaryWeapon);
          this.iconImage.sprite = equipmentData.WorldSprite;
          this.iconImage.color = Color.black;
          this.rewardText.text = string.Format(LocalizationManager.GetTranslation("Notifications/LegendaryWeaponUpgrade"), (object) equipmentData.GetLocalisedTitle());
        }
      }
    }
    else if (config.JobData.IsRewardFlockade)
    {
      this.flockadePieceContainer.gameObject.SetActive(true);
      FlockadeGamePieceConfiguration piece = FlockadePieceManager.GetPiecesPool().GetPiece(config.JobData.RewardFlockade);
      this.flockadePieceImage.sprite = piece.Image;
      this.rewardText.text = LocalizationManager.GetTranslation(piece.Name);
      TMP_Text rewardText = this.rewardText;
      rewardText.text = $"{rewardText.text}<br>{LocalizationManager.GetTranslation(piece.Description)}";
    }
    else if (config.JobData.IsRewardDeco)
    {
      this.decoContainer.gameObject.SetActive(true);
      this.decoImage.sprite = TypeAndPlacementObjects.GetByType(config.JobData.RewardDeco).IconImage;
      this.rewardText.text = StructuresData.LocalizedName(config.JobData.RewardDeco);
    }
    else if (config.JobData.IsRewardWeapon)
    {
      EquipmentData weaponData = (EquipmentData) EquipmentManager.GetWeaponData(config.JobData.RewardWeapon);
      this.miscItemContainer.gameObject.SetActive(true);
      this.iconImage.sprite = weaponData.WorldSprite;
      this.rewardText.text = weaponData.GetLocalisedTitle();
      TMP_Text rewardText = this.rewardText;
      rewardText.text = $"{rewardText.text}<br>{LocalizationManager.GetTranslation("UI/LegendaryUpgrade")}";
      if (config.IsDisabled)
        this.iconImage.color = Color.black;
    }
    else if (config.JobData.IsRewardTarot)
    {
      this.tarotContainer.gameObject.SetActive(true);
      this.tarot.Skeleton.SetSkin(TarotCards.Skin(config.JobData.RewardTarot));
      this.rewardText.text = TarotCards.LocalisedName(config.JobData.RewardTarot);
      TMP_Text rewardText = this.rewardText;
      rewardText.text = $"{rewardText.text}<br>{TarotCards.LocalisedDescription(config.JobData.RewardTarot, (PlayerFarming) null)}";
      this.hintText.gameObject.SetActive(true);
      this.hintText.text = LocalizationManager.GetTranslation($"UI/JobBoard/Hint/Tarot/{config.Index}");
    }
    ObjectivesData objective = (ObjectivesData) null;
    this.UpdateTracked(config.JobBoardMenuController.TrackingObjective(config, out objective, true, true));
    if (!config.CompletedClaimedReward && !config.Completed)
      return;
    if (config.Objective is Objectives_WinFlockadeBet)
    {
      if (config.JobData.Objective.MinimumWoolBet == 10)
        this.objectiveText.text = this.objectiveText.text.Replace("10/", config.JobData.Objective.MinimumWoolBet.ToString() + "/");
      else
        this.objectiveText.text = this.objectiveText.text.Replace("0/", config.JobData.Objective.MinimumWoolBet.ToString() + "/");
    }
    else
    {
      if (!(config.Objective is Objectives_PlaceStructure))
        return;
      this.objectiveText.text = this.objectiveText.text.Replace("0", config.JobData.Objective.DecorationCount.ToString());
    }
  }

  public void UpdateTracked(bool tracked)
  {
    if (tracked != this._oldTrackState)
    {
      this.trackTickContainer.DOComplete();
      this.trackTickContainer.DOPunchPosition(new Vector3(0.0f, -10f, 0.0f), 0.25f).SetUpdate<Tweener>(true);
      this.trackTickContainer.DOPunchRotation(new Vector3(0.0f, 0.0f, 6f), 0.25f).SetUpdate<Tweener>(true);
    }
    if (this.config.Completed)
    {
      this.trackedObjective.gameObject.SetActive(false);
      this.untrackedObjective.gameObject.SetActive(false);
      this.trackedText.text = LocalizationManager.GetTranslation("UI/ClaimReward");
      this.buttonImage.sprite = this.greenButton;
    }
    else
    {
      this.trackedObjective.gameObject.SetActive(tracked);
      this.trackedText.text = tracked ? LocalizationManager.GetTranslation("UI/PauseScreen/Quests/UntrackQuest") : LocalizationManager.GetTranslation("UI/PauseScreen/Quests/TrackQuest");
    }
  }
}
