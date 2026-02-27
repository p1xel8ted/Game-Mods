// Decompiled with JetBrains decompiler
// Type: UIMissionCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIMissionCell : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  [SerializeField]
  private Image missionIcon;
  [SerializeField]
  private TMP_Text missionName;
  [SerializeField]
  private Image[] starSlots;
  [SerializeField]
  private Sprite starIcon;
  [Space]
  [SerializeField]
  private Image buttonSprite;
  [SerializeField]
  private Sprite unselectedSprite;
  [SerializeField]
  private Sprite selectedSprite;
  [Space]
  [SerializeField]
  private TMP_Text rewardsText;
  [SerializeField]
  private TMP_Text expiryText;
  [Space]
  [SerializeField]
  private GameObject normalBorder;
  [SerializeField]
  private GameObject goldenBorder;
  [Space]
  [SerializeField]
  private Image decorationImage;
  [SerializeField]
  private GameObject skinObject;
  [SerializeField]
  private SkeletonGraphic followerSkinGraphic;

  public MissionManager.Mission Mission { get; private set; }

  public void Play(MissionManager.Mission mission, Sprite icon, int difficulty)
  {
    this.gameObject.SetActive(true);
    this.Mission = mission;
    this.missionIcon.sprite = icon;
    this.missionName.text = MissionManager.GetMissionName(mission);
    this.rewardsText.text = this.GetRewardsText();
    this.expiryText.text = MissionManager.GetExpiryFormatted(mission);
    for (int index = 0; index < this.starSlots.Length; ++index)
    {
      if (index <= difficulty - 1)
        this.starSlots[index].sprite = this.starIcon;
    }
    this.normalBorder.SetActive(!mission.GoldenMission);
    this.goldenBorder.SetActive(mission.GoldenMission);
  }

  public void OnDeselect(BaseEventData eventData)
  {
    this.buttonSprite.sprite = this.unselectedSprite;
    this.transform.DOScale(1f, 0.25f);
  }

  public void OnSelect(BaseEventData eventData)
  {
    this.buttonSprite.sprite = this.selectedSprite;
    this.transform.DOScale(1.25f, 0.25f);
  }

  private string GetRewardsText()
  {
    string rewardsText = "";
    for (int index = 0; index < this.Mission.Rewards.Length; ++index)
    {
      int quantity = this.Mission.Rewards[index].quantity;
      if (quantity > 0)
        rewardsText = $"{rewardsText + FontImageNames.GetIconByType(this.Mission.Rewards[index].itemToBuy)}{quantity.ToString()} ";
    }
    if (this.Mission.GoldenMission)
    {
      rewardsText += "\n+\t\t\n\n";
      if (this.Mission.Rewards[this.Mission.Rewards.Length - 1].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION)
      {
        this.decorationImage.gameObject.SetActive(true);
        this.decorationImage.sprite = TypeAndPlacementObjects.GetByType(this.Mission.Decoration).IconImage;
      }
      else if (this.Mission.Rewards[this.Mission.Rewards.Length - 1].itemToBuy == InventoryItem.ITEM_TYPE.FOUND_ITEM_FOLLOWERSKIN)
      {
        this.skinObject.SetActive(true);
        this.followerSkinGraphic.Skeleton.SetSkin(this.Mission.FollowerSkin);
      }
    }
    return rewardsText;
  }
}
