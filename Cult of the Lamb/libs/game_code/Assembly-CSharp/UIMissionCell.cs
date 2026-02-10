// Decompiled with JetBrains decompiler
// Type: UIMissionCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using Spine.Unity;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UIMissionCell : BaseMonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
  [SerializeField]
  public Image missionIcon;
  [SerializeField]
  public TMP_Text missionName;
  [SerializeField]
  public Image[] starSlots;
  [SerializeField]
  public Sprite starIcon;
  [Space]
  [SerializeField]
  public Image buttonSprite;
  [SerializeField]
  public Sprite unselectedSprite;
  [SerializeField]
  public Sprite selectedSprite;
  [Space]
  [SerializeField]
  public TMP_Text rewardsText;
  [SerializeField]
  public TMP_Text expiryText;
  [Space]
  [SerializeField]
  public GameObject normalBorder;
  [SerializeField]
  public GameObject goldenBorder;
  [Space]
  [SerializeField]
  public Image decorationImage;
  [SerializeField]
  public GameObject skinObject;
  [SerializeField]
  public SkeletonGraphic followerSkinGraphic;
  [CompilerGenerated]
  public MissionManager.Mission \u003CMission\u003Ek__BackingField;

  public MissionManager.Mission Mission
  {
    get => this.\u003CMission\u003Ek__BackingField;
    set => this.\u003CMission\u003Ek__BackingField = value;
  }

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

  public string GetRewardsText()
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
