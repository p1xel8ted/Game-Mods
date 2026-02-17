// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.FollowerSelect;
using src.Extensions;
using src.UINavigator;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CultMenu : UISubmenuBase
{
  [Header("Cult Stats")]
  [SerializeField]
  public TextMeshProUGUI _cultNameText;
  [SerializeField]
  public TextMeshProUGUI _followersCount;
  [SerializeField]
  public TextMeshProUGUI _structuresCount;
  [SerializeField]
  public TextMeshProUGUI _deadFollowersCount;
  [SerializeField]
  public GameObject _divider;
  [SerializeField]
  public GameObject _faithContainer;
  [SerializeField]
  public Image _faithBar;
  [SerializeField]
  public GameObject _hungerContainer;
  [SerializeField]
  public Image _hungerBar;
  [SerializeField]
  public GameObject _sicknessContainer;
  [SerializeField]
  public Image _sicknessBar;
  [SerializeField]
  public GameObject _warmthContainer;
  [SerializeField]
  public Image _warmthBar;
  [SerializeField]
  public MMButton _followerButton;
  [SerializeField]
  public UINavigatorFollowElement _buttonBackground;
  [Header("Content")]
  [SerializeField]
  public GameObject _cultHeader;
  [SerializeField]
  public GameObject _statsheader;
  [SerializeField]
  public GameObject _cultContent;
  [SerializeField]
  public GameObject _followerContent;
  [SerializeField]
  public GameObject _notificationsHeader;
  [SerializeField]
  public RectTransform _notificationContent;
  [SerializeField]
  public RectTransform _notificationHistoryContent;
  [SerializeField]
  public GameObject _traitsHeader;
  [SerializeField]
  public GridLayoutGroup _cultTraitLayoutGroup;
  [SerializeField]
  public RectTransform _cultTraitContent;
  [SerializeField]
  public MMScrollRect _scrollRect;
  [SerializeField]
  public GameObject _noFollowerMessage;
  [Header("Templates")]
  [SerializeField]
  public NotificationDynamicGeneric _notificationDynamicTemplate;
  [SerializeField]
  public NotificationDynamicRitual _ritualNotificationDynamicTemplate;
  [SerializeField]
  public NotificationDynamicWeather _weatherNotificationDynamicTemplate;
  [SerializeField]
  public IndoctrinationTraitItem _traitItemTemplate;
  [SerializeField]
  public HistoricalNotificationGeneric _historicalNotificationGeneric;
  [SerializeField]
  public HistoricalNotificationFollower _historicalNotificationFollower;
  [SerializeField]
  public HistoricalNotificationFaith _historicalNotificationFaith;
  [SerializeField]
  public HistoricalNotificationItem _historicalNotificationItem;
  [SerializeField]
  public HistoricalNotificationRelationship _historicalNotificationRelationship;
  public List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  public List<NotificationDynamicBase> _notificationItems = new List<NotificationDynamicBase>();
  public List<UIHistoricalNotification> _notificationHistoryItems = new List<UIHistoricalNotification>();

  public override void Awake()
  {
    base.Awake();
    this._followerButton.onClick.AddListener(new UnityAction(this.OnCultButtonPressed));
  }

  public override void OnShowStarted()
  {
    this._scrollRect.normalizedPosition = Vector2.one;
    this._scrollRect.enabled = false;
    if (DataManager.Instance.Followers.Count + DataManager.Instance.Followers_Dead.Count == 0)
    {
      this._followerButton.Interactable = false;
      MonoSingleton<UINavigatorNew>.Instance.Clear();
      this._cultNameText.gameObject.SetActive(false);
      this._statsheader.SetActive(false);
      this._cultHeader.SetActive(false);
      this._cultContent.SetActive(false);
      this._notificationsHeader.SetActive(false);
      this._notificationContent.gameObject.SetActive(false);
      this._notificationHistoryContent.gameObject.SetActive(false);
      this._traitsHeader.gameObject.SetActive(false);
      this._cultTraitContent.gameObject.SetActive(false);
      this._followerContent.SetActive(false);
    }
    else
    {
      this._noFollowerMessage.gameObject.SetActive(false);
      if (string.IsNullOrEmpty(DataManager.Instance.CultName))
        this._cultNameText.text = ScriptLocalization.NAMES_Place.Cult;
      else
        this._cultNameText.text = LocalizeIntegration.Arabic_ReverseNonRTL(DataManager.Instance.CultName);
      this._followersCount.text = DataManager.Instance.Followers.Count.ToString();
      TextMeshProUGUI structuresCount = this._structuresCount;
      int num1 = StructureManager.GetTotalHomesCount();
      string str1 = num1.ToString();
      structuresCount.text = str1;
      TextMeshProUGUI deadFollowersCount = this._deadFollowersCount;
      num1 = DataManager.Instance.Followers_Dead.Count;
      string str2 = num1.ToString();
      deadFollowersCount.text = str2;
      this._divider.SetActive(DataManager.Instance.ShowCultFaith || DataManager.Instance.ShowCultIllness || DataManager.Instance.ShowCultHunger);
      this._faithContainer.SetActive(DataManager.Instance.ShowCultFaith);
      this._faithBar.fillAmount = CultFaithManager.CultFaithNormalised;
      this._faithBar.color = StaticColors.ColorForThreshold(this._faithBar.fillAmount);
      this._warmthContainer.SetActive(DataManager.Instance.ShowCultWarmth && (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter || DataManager.Instance.HasWeatherVane && (double) SeasonsManager.SEASON_NORMALISED_PROGRESS >= 0.89999997615814209));
      this._warmthBar.fillAmount = WarmthBar.WarmthNormalized;
      this._warmthBar.color = StaticColors.ColorForThreshold(this._warmthBar.fillAmount);
      this._sicknessContainer.SetActive(DataManager.Instance.ShowCultIllness);
      this._sicknessBar.fillAmount = IllnessBar.IllnessNormalized;
      this._sicknessBar.color = StaticColors.ColorForThreshold(this._sicknessBar.fillAmount);
      this._hungerContainer.SetActive(DataManager.Instance.ShowCultHunger);
      this._hungerBar.fillAmount = HungerBar.HungerNormalized;
      this._hungerBar.color = StaticColors.ColorForThreshold(this._hungerBar.fillAmount);
      if (this._traitItems.Count == 0)
      {
        foreach (FollowerTrait.TraitType cultTrait in DataManager.Instance.CultTraits)
        {
          IndoctrinationTraitItem indoctrinationTraitItem = this._traitItemTemplate.Instantiate<IndoctrinationTraitItem>((Transform) this._cultTraitContent);
          indoctrinationTraitItem.Configure(cultTrait);
          this._traitItems.Add(indoctrinationTraitItem);
        }
        this._traitsHeader.SetActive(this._traitItems.Count > 0);
        this._cultTraitContent.gameObject.SetActive(this._traitItems.Count > 0);
      }
      if (this._notificationItems.Count == 0)
      {
        foreach (DynamicNotificationData dynamicNotification in UIDynamicNotificationCenter.DynamicNotifications)
        {
          if (!dynamicNotification.IsEmpty)
          {
            NotificationDynamicBase notificationDynamicBase;
            switch (dynamicNotification)
            {
              case DynamicNotification_RitualActive _:
                notificationDynamicBase = (NotificationDynamicBase) this._ritualNotificationDynamicTemplate.Instantiate<NotificationDynamicRitual>((Transform) this._notificationContent);
                break;
              case DynamicNotification_WeatherActive _:
                notificationDynamicBase = (NotificationDynamicBase) this._weatherNotificationDynamicTemplate.Instantiate<NotificationDynamicWeather>((Transform) this._notificationContent);
                break;
              default:
                notificationDynamicBase = (NotificationDynamicBase) this._notificationDynamicTemplate.Instantiate<NotificationDynamicGeneric>((Transform) this._notificationContent);
                break;
            }
            notificationDynamicBase.Configure(dynamicNotification);
            notificationDynamicBase.StopAllCoroutines();
            notificationDynamicBase.Container.localScale = Vector3.one;
            this._notificationItems.Add(notificationDynamicBase);
          }
        }
        this._notificationContent.gameObject.SetActive(this._notificationItems.Count > 0);
      }
      if (this._notificationHistoryItems.Count == 0)
      {
        foreach (FinalizedNotification finalizedNotification1 in DataManager.Instance.NotificationHistory)
        {
          switch (finalizedNotification1)
          {
            case null:
              continue;
            case FinalizedFaithNotification finalizedNotification2:
              HistoricalNotificationFaith notificationFaith = this._historicalNotificationFaith.Instantiate<HistoricalNotificationFaith>((Transform) this._notificationHistoryContent);
              notificationFaith.Configure(finalizedNotification2);
              this._notificationHistoryItems.Add((UIHistoricalNotification) notificationFaith);
              continue;
            case FinalizedItemNotification finalizedNotification3:
              HistoricalNotificationItem notificationItem = this._historicalNotificationItem.Instantiate<HistoricalNotificationItem>((Transform) this._notificationHistoryContent);
              notificationItem.Configure(finalizedNotification3);
              this._notificationHistoryItems.Add((UIHistoricalNotification) notificationItem);
              continue;
            case FinalizedFollowerNotification finalizedNotification4:
              HistoricalNotificationFollower notificationFollower = this._historicalNotificationFollower.Instantiate<HistoricalNotificationFollower>((Transform) this._notificationHistoryContent);
              notificationFollower.Configure(finalizedNotification4);
              this._notificationHistoryItems.Add((UIHistoricalNotification) notificationFollower);
              continue;
            case FinalizedRelationshipNotification finalizedNotification5:
              HistoricalNotificationRelationship notificationRelationship = this._historicalNotificationRelationship.Instantiate<HistoricalNotificationRelationship>((Transform) this._notificationHistoryContent);
              notificationRelationship.Configure(finalizedNotification5);
              this._notificationHistoryItems.Add((UIHistoricalNotification) notificationRelationship);
              continue;
            default:
              HistoricalNotificationGeneric notificationGeneric = this._historicalNotificationGeneric.Instantiate<HistoricalNotificationGeneric>((Transform) this._notificationHistoryContent);
              notificationGeneric.Configure(finalizedNotification1);
              this._notificationHistoryItems.Add((UIHistoricalNotification) notificationGeneric);
              continue;
          }
        }
      }
      Navigation navigation1 = this._followerButton.navigation with
      {
        mode = Navigation.Mode.Explicit
      };
      if (this._traitItems.Count > 0)
        navigation1.selectOnDown = (Selectable) this._traitItems[0].Selectable;
      else if (this._notificationItems.Count > 0)
        navigation1.selectOnDown = (Selectable) this._notificationItems[0].Selectable;
      else if (this._notificationHistoryItems.Count > 0)
        navigation1.selectOnDown = (Selectable) this._notificationHistoryItems[0].Selectable;
      this._followerButton.navigation = navigation1;
      if (this._traitItems.Count > 0)
      {
        int constraintCount = this._cultTraitLayoutGroup.constraintCount;
        int num2 = Mathf.CeilToInt((float) this._traitItems.Count / (float) constraintCount);
        for (int index1 = 0; index1 < this._traitItems.Count; ++index1)
        {
          Navigation navigation2 = this._traitItems[index1].Selectable.navigation with
          {
            mode = Navigation.Mode.Explicit
          };
          int num3 = Mathf.FloorToInt((float) index1 / (float) constraintCount);
          int index2 = index1 - num3 * constraintCount;
          if (num3 == 0)
          {
            navigation2.selectOnUp = (Selectable) this._followerButton;
            if (index1 + constraintCount < this._traitItems.Count)
              navigation2.selectOnDown = (Selectable) this._traitItems[index1 + constraintCount].Selectable;
            else if (this._notificationItems.Count > 0)
              navigation2.selectOnDown = (Selectable) this._notificationItems[0].Selectable;
            else if (this._notificationHistoryItems.Count > 0)
              navigation2.selectOnDown = (Selectable) this._notificationHistoryItems[0].Selectable;
          }
          else if (num3 == num2 - 1)
          {
            navigation2.selectOnDown = this._notificationItems.Count <= 0 ? (this._notificationHistoryItems.Count <= 0 ? (index2 >= this._notificationItems.Count ? (Selectable) this._notificationItems.LastElement<NotificationDynamicBase>().Selectable : (Selectable) this._notificationItems[index2].Selectable) : (Selectable) this._notificationHistoryItems[0].Selectable) : (Selectable) this._notificationItems[0].Selectable;
            navigation2.selectOnUp = (Selectable) this._traitItems[index1 - constraintCount].Selectable;
          }
          else
          {
            navigation2.selectOnUp = (Selectable) this._traitItems[index1 - constraintCount].Selectable;
            navigation2.selectOnDown = index1 + constraintCount >= this._traitItems.Count ? (Selectable) this._traitItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._traitItems[index1 + constraintCount].Selectable;
          }
          if (index2 > 0)
            navigation2.selectOnLeft = (Selectable) this._traitItems[index1 - 1].Selectable;
          if (index2 < constraintCount && index1 + 1 < this._traitItems.Count)
            navigation2.selectOnRight = (Selectable) this._traitItems[index1 + 1].Selectable;
          this._traitItems[index1].Selectable.navigation = navigation2;
        }
      }
      if (this._notificationItems.Count > 0)
      {
        for (int index = 0; index < this._notificationItems.Count; ++index)
        {
          Navigation navigation3 = this._notificationItems[index].Selectable.navigation with
          {
            mode = Navigation.Mode.Explicit,
            selectOnUp = this._traitItems.Count != 0 ? (index >= this._traitItems.Count ? (Selectable) this._traitItems.LastElement<IndoctrinationTraitItem>().Selectable : (Selectable) this._traitItems[index].Selectable) : (Selectable) this._followerButton
          };
          if (this._notificationHistoryItems.Count > 0)
            navigation3.selectOnDown = (Selectable) this._notificationHistoryItems[0].Selectable;
          if (index > 0)
            navigation3.selectOnLeft = (Selectable) this._notificationItems[index - 1].Selectable;
          if (index < this._notificationItems.Count - 1)
            navigation3.selectOnRight = (Selectable) this._notificationItems[index + 1].Selectable;
          this._notificationItems[index].Selectable.navigation = navigation3;
        }
      }
      if (this._notificationHistoryItems.Count > 0)
      {
        Navigation navigation4 = this._notificationHistoryItems[0].Selectable.navigation with
        {
          mode = Navigation.Mode.Explicit,
          selectOnUp = this._notificationItems.Count <= 0 ? (this._traitItems.Count <= 0 ? (Selectable) this._followerButton : (Selectable) this._traitItems[0].Selectable) : (Selectable) this._notificationItems[0].Selectable
        };
        if (this._notificationHistoryItems.Count > 1)
          navigation4.selectOnDown = (Selectable) this._notificationHistoryItems[1].Selectable;
        this._notificationHistoryItems[0].Selectable.navigation = navigation4;
      }
      this._notificationsHeader.SetActive(this._notificationItems.Count + this._notificationHistoryItems.Count > 0);
      this._scrollRect.enabled = true;
    }
  }

  public void OnCultButtonPressed()
  {
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
      followerSelectEntries.Add(new FollowerSelectEntry(follower));
    UIFollowerSelectMenuController followerSelectMenuController = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    followerSelectMenuController.AllowsVoting = false;
    followerSelectMenuController.Show(followerSelectEntries, false, UpgradeSystem.Type.Count, false, true, false, false, true);
    UIFollowerSelectMenuController selectMenuController1 = followerSelectMenuController;
    selectMenuController1.OnShow = selectMenuController1.OnShow + (System.Action) (() =>
    {
      this._buttonBackground.enabled = false;
      if (!DataManager.Instance.PleasureEnabled)
        return;
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenuController.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(0, false);
    });
    UIFollowerSelectMenuController selectMenuController2 = followerSelectMenuController;
    selectMenuController2.OnShownCompleted = selectMenuController2.OnShownCompleted + (System.Action) (() =>
    {
      if (!DataManager.Instance.PleasureEnabled)
        return;
      foreach (FollowerInformationBox followerInfoBox in followerSelectMenuController.FollowerInfoBoxes)
        followerInfoBox.ShowPleasure(0, false);
    });
    UIFollowerSelectMenuController selectMenuController3 = followerSelectMenuController;
    selectMenuController3.OnHidden = selectMenuController3.OnHidden + (System.Action) (() => this._buttonBackground.enabled = true);
    this.PushInstance<UIFollowerSelectMenuController>(followerSelectMenuController);
  }
}
