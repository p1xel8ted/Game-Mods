// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CultMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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
  private TextMeshProUGUI _cultNameText;
  [SerializeField]
  private TextMeshProUGUI _followersCount;
  [SerializeField]
  private TextMeshProUGUI _structuresCount;
  [SerializeField]
  private TextMeshProUGUI _deadFollowersCount;
  [SerializeField]
  private GameObject _divider;
  [SerializeField]
  private GameObject _faithContainer;
  [SerializeField]
  private Image _faithBar;
  [SerializeField]
  private GameObject _hungerContainer;
  [SerializeField]
  private Image _hungerBar;
  [SerializeField]
  private GameObject _sicknessContainer;
  [SerializeField]
  private Image _sicknessBar;
  [SerializeField]
  private MMButton _followerButton;
  [Header("Content")]
  [SerializeField]
  private GameObject _cultHeader;
  [SerializeField]
  private GameObject _statsheader;
  [SerializeField]
  private GameObject _cultContent;
  [SerializeField]
  private GameObject _followerContent;
  [SerializeField]
  private GameObject _notificationsHeader;
  [SerializeField]
  private RectTransform _notificationContent;
  [SerializeField]
  private RectTransform _notificationHistoryContent;
  [SerializeField]
  private GameObject _traitsHeader;
  [SerializeField]
  private RectTransform _cultTraitContent;
  [SerializeField]
  private MMScrollRect _scrollRect;
  [SerializeField]
  private GameObject _noFollowerMessage;
  [Header("Templates")]
  [SerializeField]
  private NotificationDynamicGeneric _notificationDynamicTemplate;
  [SerializeField]
  private NotificationDynamicRitual _ritualNotificationDynamicTemplate;
  [SerializeField]
  private IndoctrinationTraitItem _traitItemTemplate;
  [SerializeField]
  private HistoricalNotificationGeneric _historicalNotificationGeneric;
  [SerializeField]
  private HistoricalNotificationFollower _historicalNotificationFollower;
  [SerializeField]
  private HistoricalNotificationFaith _historicalNotificationFaith;
  [SerializeField]
  private HistoricalNotificationItem _historicalNotificationItem;
  [SerializeField]
  private HistoricalNotificationRelationship _historicalNotificationRelationship;
  private List<IndoctrinationTraitItem> _traitItems = new List<IndoctrinationTraitItem>();
  private List<NotificationDynamicBase> _notificationItems = new List<NotificationDynamicBase>();
  private List<UIHistoricalNotification> _notificationHistoryItems = new List<UIHistoricalNotification>();

  public override void Awake()
  {
    base.Awake();
    this._followerButton.onClick.AddListener(new UnityAction(this.OnCultButtonPressed));
  }

  protected override void OnShowStarted()
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
        this._cultNameText.text = DataManager.Instance.CultName;
      this._followersCount.text = DataManager.Instance.Followers.Count.ToString();
      TextMeshProUGUI structuresCount = this._structuresCount;
      int num = StructureManager.GetTotalHomesCount();
      string str1 = num.ToString();
      structuresCount.text = str1;
      TextMeshProUGUI deadFollowersCount = this._deadFollowersCount;
      num = DataManager.Instance.Followers_Dead.Count;
      string str2 = num.ToString();
      deadFollowersCount.text = str2;
      this._divider.SetActive(DataManager.Instance.ShowCultFaith || DataManager.Instance.ShowCultIllness || DataManager.Instance.ShowCultHunger);
      this._faithContainer.SetActive(DataManager.Instance.ShowCultFaith);
      this._faithBar.fillAmount = CultFaithManager.CultFaithNormalised;
      this._faithBar.color = StaticColors.ColorForThreshold(this._faithBar.fillAmount);
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
            DynamicNotificationData notificationData = dynamicNotification;
            NotificationDynamicBase notificationDynamicBase = notificationData == null || !(notificationData is DynamicNotification_RitualActive _) ? (NotificationDynamicBase) this._notificationDynamicTemplate.Instantiate<NotificationDynamicGeneric>((Transform) this._notificationContent) : (NotificationDynamicBase) this._ritualNotificationDynamicTemplate.Instantiate<NotificationDynamicRitual>((Transform) this._notificationContent);
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
        for (int index = 0; index < this._traitItems.Count; ++index)
        {
          Navigation navigation2 = this._traitItems[index].Selectable.navigation with
          {
            mode = Navigation.Mode.Explicit,
            selectOnUp = (Selectable) this._followerButton,
            selectOnDown = this._notificationItems.Count != 0 || this._notificationHistoryItems.Count <= 0 ? (index >= this._notificationItems.Count ? (Selectable) this._notificationItems.LastElement<NotificationDynamicBase>().Selectable : (Selectable) this._notificationItems[index].Selectable) : (Selectable) this._notificationHistoryItems[0].Selectable
          };
          if (index > 0)
            navigation2.selectOnLeft = (Selectable) this._traitItems[index - 1].Selectable;
          if (index < this._traitItems.Count - 1)
            navigation2.selectOnRight = (Selectable) this._traitItems[index + 1].Selectable;
          this._traitItems[index].Selectable.navigation = navigation2;
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

  private void OnCultButtonPressed()
  {
    UIFollowerSelectMenuController menu = MonoSingleton<UIManager>.Instance.FollowerSelectMenuTemplate.Instantiate<UIFollowerSelectMenuController>();
    menu.Show(DataManager.Instance.Followers, (List<FollowerInfo>) null, false, UpgradeSystem.Type.Count, false, true, false);
    this.PushInstance<UIFollowerSelectMenuController>(menu);
  }
}
