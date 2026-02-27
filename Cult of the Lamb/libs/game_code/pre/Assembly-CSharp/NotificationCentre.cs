// Decompiled with JetBrains decompiler
// Type: NotificationCentre
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class NotificationCentre : BaseMonoBehaviour
{
  private const float kShowDuration = 0.5f;
  private const float kHideDuration = 0.5f;
  public static bool NotificationsEnabled = true;
  public static NotificationCentre Instance;
  [SerializeField]
  private RectTransform _rectTransform;
  [SerializeField]
  private RectTransform _notificationContainer;
  [Header("Templates - Standard")]
  [SerializeField]
  private NotificationGeneric _genericNotificationTemplate;
  [SerializeField]
  private NotificationTwitch _twitchNotificationTemplate;
  [SerializeField]
  private NotificationItem _itemNotificationTemplate;
  [SerializeField]
  private NotificationFaith _faithNotificationTemplate;
  [SerializeField]
  private NotificationFollower _followerNotificationTemplate;
  [SerializeField]
  private NotificationRelationship _relationshipNotificationTemplate;
  [SerializeField]
  private NotificationHelpHinder _helpHinderNotificationTemplate;
  public static List<NotificationBase> Notifications = new List<NotificationBase>();
  private List<string> notificationsThisFrame = new List<string>();
  private Vector2 _onScreenPosition;
  private Vector2 _offScreenPosition;

  private void Awake()
  {
    this._onScreenPosition = this._offScreenPosition = new Vector2(0.0f, -30f);
    this._offScreenPosition.x = (float) (-(double) this._rectTransform.sizeDelta.x - 200.0);
    TwitchManager.OnNotificationReceived += new TwitchManager.NotificationResponse(this.OnTwitchNotificationReceived);
  }

  private void OnDestroy()
  {
    TwitchManager.OnNotificationReceived -= new TwitchManager.NotificationResponse(this.OnTwitchNotificationReceived);
  }

  private void OnEnable()
  {
    NotificationCentre.Instance = this;
    Inventory.OnItemAddedToInventory += new Inventory.ItemAddedToInventory(this.ItemAddedToInventory);
    Inventory.OnItemRemovedFromInventory += new Inventory.ItemAddedToInventory(this.ItemAddedToInventory);
    FollowerBrainStats.OnLevelUp += new FollowerBrainStats.StatChangedEvent(this.OnFollowerLevelUp);
    FollowerBrainInfo.OnReadyToLevelUp += new FollowerBrainStats.StatChangedEvent(this.OnReadyToLevelUp);
    FollowerTask_EatMeal.OnEatRottenFood += new Action<int>(this.OnEatRottenFood);
    FollowerTask_Chat.OnChangeRelationship += new Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState>(this.OnChangeRelationship);
    PlayerWeapon.WeaponBroken += new System.Action(this.WeaponBroken);
    PlayerWeapon.WeaponDamaged += new System.Action(this.WeaponDamaged);
    PlayerSpells.CurseDamaged += new System.Action(this.CurseBroken);
    PlayerSpells.CurseBroken += new System.Action(this.CurseDamaged);
  }

  private void OnDisable()
  {
    if ((UnityEngine.Object) NotificationCentre.Instance == (UnityEngine.Object) this)
      NotificationCentre.Instance = (NotificationCentre) null;
    Inventory.OnItemAddedToInventory -= new Inventory.ItemAddedToInventory(this.ItemAddedToInventory);
    Inventory.OnItemRemovedFromInventory -= new Inventory.ItemAddedToInventory(this.ItemAddedToInventory);
    FollowerBrainStats.OnLevelUp -= new FollowerBrainStats.StatChangedEvent(this.OnFollowerLevelUp);
    FollowerBrainInfo.OnReadyToLevelUp -= new FollowerBrainStats.StatChangedEvent(this.OnReadyToLevelUp);
    FollowerTask_EatMeal.OnEatRottenFood -= new Action<int>(this.OnEatRottenFood);
    FollowerTask_Chat.OnChangeRelationship -= new Action<FollowerInfo, FollowerInfo, IDAndRelationship.RelationshipState>(this.OnChangeRelationship);
    PlayerWeapon.WeaponBroken -= new System.Action(this.WeaponBroken);
    PlayerWeapon.WeaponDamaged -= new System.Action(this.WeaponDamaged);
    PlayerSpells.CurseDamaged -= new System.Action(this.CurseBroken);
    PlayerSpells.CurseBroken -= new System.Action(this.CurseDamaged);
  }

  public void Show(bool instant = false)
  {
    this._rectTransform.DOKill();
    this.StopAllCoroutines();
    if (instant)
      this._rectTransform.anchoredPosition = this._onScreenPosition;
    else
      this.StartCoroutine((IEnumerator) this.DoShow());
  }

  private IEnumerator DoShow()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._onScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void Hide(bool instant = false)
  {
    this._rectTransform.DOKill();
    this.StopAllCoroutines();
    if (instant)
      this._rectTransform.anchoredPosition = this._offScreenPosition;
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  private IEnumerator DoHide()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._offScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  private void CurseBroken()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.CurseDestroyed);
  }

  private void CurseDamaged()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.CurseDamaged);
  }

  private void WeaponBroken()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.WeaponDestroyed);
  }

  private void WeaponDamaged()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.WeaponDamaged);
  }

  private void OnReadyToLevelUp(int brainID, float newValue, float oldValue, float change)
  {
    this.PlayFollowerNotification(NotificationCentre.NotificationType.ReadyToLevelUp, FollowerBrain.FindBrainByID(brainID).Info, NotificationFollower.Animation.Happy);
  }

  private void OnFollowerLevelUp(int brainID, float newValue, float oldValue, float change)
  {
    this.PlayFollowerNotification(NotificationCentre.NotificationType.LevellingUp, FollowerBrain.FindBrainByID(brainID).Info, NotificationFollower.Animation.Happy);
  }

  private void ItemAddedToInventory(InventoryItem.ITEM_TYPE itemType, int delta)
  {
    if (!this.CheckToShowHUD(itemType))
      return;
    foreach (NotificationBase notification in NotificationCentre.Notifications)
    {
      if (notification is NotificationItem notificationItem && notificationItem.ItemType == itemType)
      {
        notificationItem.UpdateDelta(delta);
        return;
      }
    }
    this.PlayItemNotification(itemType, delta);
  }

  public bool CheckToShowHUD(InventoryItem.ITEM_TYPE type)
  {
    return type != InventoryItem.ITEM_TYPE.SEEDS && type != InventoryItem.ITEM_TYPE.INGREDIENTS;
  }

  private void OnRestStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (newState != FollowerStatState.Urgent)
      return;
    this.PlayFollowerNotification(NotificationCentre.NotificationType.Exhausted, brainById.Info, NotificationFollower.Animation.Tired);
  }

  private void OnChangeRelationship(
    FollowerInfo f1,
    FollowerInfo f2,
    IDAndRelationship.RelationshipState state)
  {
    this.PlayRelationshipNotification(NotificationCentre.NotificationType.RelationshipFriend, f1, NotificationFollower.Animation.Happy, f2, NotificationFollower.Animation.Happy);
  }

  private void OnEatRottenFood(int brainID)
  {
    this.PlayFollowerNotification(NotificationCentre.NotificationType.AteRottenFood, FollowerBrain.FindBrainByID(brainID).Info, NotificationFollower.Animation.Sick);
  }

  public void PlayItemNotification(InventoryItem.ITEM_TYPE itemType, int delta)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = itemType.ToString();
    if (this.notificationsThisFrame.Contains(str))
      return;
    NotificationItem notificationItem = this._itemNotificationTemplate.Instantiate<NotificationItem>((Transform) this._notificationContainer, false);
    notificationItem.Configure(itemType, delta);
    NotificationCentre.Notifications.Add((NotificationBase) notificationItem);
    this.notificationsThisFrame.Add(str);
  }

  public void PlayGenericNotification(string locKey)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    NotificationGeneric notificationGeneric = this._genericNotificationTemplate.Instantiate<NotificationGeneric>((Transform) this._notificationContainer, false);
    notificationGeneric.Configure(locKey);
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    DataManager.Instance.AddToNotificationHistory(new FinalizedNotification()
    {
      LocKey = locKey
    });
    this.notificationsThisFrame.Add(locKey);
  }

  public void PlayGenericNotification(NotificationCentre.NotificationType type)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = type.ToString();
    if (this.notificationsThisFrame.Contains(str))
      return;
    NotificationGeneric notificationGeneric = this._genericNotificationTemplate.Instantiate<NotificationGeneric>((Transform) this._notificationContainer, false);
    notificationGeneric.Configure(type, NotificationCentre.GetFlair(type));
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    DataManager.Instance.AddToNotificationHistory(new FinalizedNotification()
    {
      LocKey = NotificationCentre.GetLocKey(type)
    });
    this.notificationsThisFrame.Add(str);
  }

  public void PlayGenericNotificationNonLocalizedParams(
    string locKey,
    params string[] nonLocalizedParameters)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    NotificationGeneric notificationGeneric = this._genericNotificationTemplate.Instantiate<NotificationGeneric>((Transform) this._notificationContainer, false);
    notificationGeneric.ConfigureNonLocalizedParams(locKey, nonLocalizedParameters);
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    DataManager.Instance.AddToNotificationHistory(new FinalizedNotification()
    {
      LocKey = locKey,
      NonLocalisedParameters = nonLocalizedParameters
    });
    this.notificationsThisFrame.Add(locKey);
  }

  public void PlayGenericNotificationLocalizedParams(
    string locKey,
    params string[] localizedParameters)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    NotificationGeneric notificationGeneric = this._genericNotificationTemplate.Instantiate<NotificationGeneric>((Transform) this._notificationContainer, false);
    notificationGeneric.ConfigureLocalizedParams(locKey, localizedParameters);
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    DataManager.Instance.AddToNotificationHistory(new FinalizedNotification()
    {
      LocKey = locKey,
      LocalisedParameters = localizedParameters
    });
    this.notificationsThisFrame.Add(locKey);
  }

  public void PlayTwitchNotification(string locKey, params string[] localizedParameters)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    NotificationTwitch notificationTwitch = this._twitchNotificationTemplate.Instantiate<NotificationTwitch>((Transform) this._notificationContainer, false);
    notificationTwitch.ConfigureLocalizedParams(locKey, localizedParameters);
    NotificationCentre.Notifications.Add((NotificationBase) notificationTwitch);
    DataManager.Instance.AddToNotificationHistory(new FinalizedNotification()
    {
      LocKey = locKey,
      LocalisedParameters = localizedParameters
    });
    this.notificationsThisFrame.Add(locKey);
  }

  public void PlayHelpHinderNotification(string locKey)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    NotificationHelpHinder notificationHelpHinder = this._helpHinderNotificationTemplate.Instantiate<NotificationHelpHinder>((Transform) this._notificationContainer, false);
    notificationHelpHinder.ConfigureLocalizedParams(locKey);
    NotificationCentre.Notifications.Add((NotificationBase) notificationHelpHinder);
    DataManager.Instance.AddToNotificationHistory(new FinalizedNotification()
    {
      LocKey = locKey
    });
    this.notificationsThisFrame.Add(locKey);
  }

  public void PlayFaithNotification(
    string locKey,
    float faithDelta,
    NotificationBase.Flair flair,
    int followerID = -1,
    params string[] args)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = $"{locKey}{faithDelta}{followerID}";
    if (this.notificationsThisFrame.Contains(str))
      return;
    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID, true);
    NotificationFaith notificationFaith = this._faithNotificationTemplate.Instantiate<NotificationFaith>((Transform) this._notificationContainer, false);
    notificationFaith.Configure(locKey, faithDelta, infoById, true, flair, args);
    NotificationCentre.Notifications.Add((NotificationBase) notificationFaith);
    FinalizedFaithNotification faithNotification = new FinalizedFaithNotification();
    faithNotification.LocKey = locKey;
    faithNotification.FaithDelta = faithDelta;
    faithNotification.followerInfoSnapshot = infoById != null ? new FollowerInfoSnapshot(infoById) : (FollowerInfoSnapshot) null;
    faithNotification.LocalisedParameters = args;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) faithNotification);
    this.notificationsThisFrame.Add(str);
  }

  public void PlayFollowerNotification(
    NotificationCentre.NotificationType type,
    FollowerBrainInfo info,
    NotificationFollower.Animation followerAnimation)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = string.Format(type.ToString(), (object) info.GetHashCode());
    if (this.notificationsThisFrame.Contains(str))
      return;
    FollowerInfo infoById = FollowerInfo.GetInfoByID(info.ID);
    NotificationFollower notificationFollower = this._followerNotificationTemplate.Instantiate<NotificationFollower>((Transform) this._notificationContainer, false);
    notificationFollower.Configure(type, FollowerInfo.GetInfoByID(info.ID), followerAnimation, NotificationCentre.GetFlair(type));
    NotificationCentre.Notifications.Add((NotificationBase) notificationFollower);
    FinalizedFollowerNotification followerNotification = new FinalizedFollowerNotification();
    followerNotification.LocKey = NotificationCentre.GetLocKey(type);
    followerNotification.followerInfoSnapshot = new FollowerInfoSnapshot(infoById);
    followerNotification.Animation = followerAnimation;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) followerNotification);
    this.notificationsThisFrame.Add(str);
  }

  public void PlayRelationshipNotification(
    NotificationCentre.NotificationType type,
    FollowerInfo followerInfo,
    NotificationFollower.Animation followerAnimation,
    FollowerInfo otherFollowerInfo,
    NotificationFollower.Animation otherAnimation)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = followerInfo.GetHashCode().ToString() + otherFollowerInfo.GetHashCode().ToString();
    if (this.notificationsThisFrame.Contains(str))
      return;
    NotificationRelationship notificationRelationship = this._relationshipNotificationTemplate.Instantiate<NotificationRelationship>((Transform) this._notificationContainer, false);
    notificationRelationship.Configure(type, followerInfo, otherFollowerInfo, followerAnimation, otherAnimation, NotificationCentre.GetFlair(type));
    NotificationCentre.Notifications.Add((NotificationBase) notificationRelationship);
    FinalizedRelationshipNotification relationshipNotification = new FinalizedRelationshipNotification();
    relationshipNotification.LocKey = $"Notifications/Relationship/{type}";
    relationshipNotification.followerInfoSnapshotA = new FollowerInfoSnapshot(followerInfo);
    relationshipNotification.followerInfoSnapshotB = new FollowerInfoSnapshot(otherFollowerInfo);
    relationshipNotification.FollowerAnimationA = followerAnimation;
    relationshipNotification.FollowerAnimationB = otherAnimation;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) relationshipNotification);
    this.notificationsThisFrame.Add(str);
  }

  public static string GetLocKey(
    NotificationCentre.NotificationType notificationType)
  {
    switch (notificationType)
    {
      case NotificationCentre.NotificationType.Died:
        return "Notifications/Follower/Died";
      case NotificationCentre.NotificationType.Starving:
        return "Notifications/Follower/Starving";
      case NotificationCentre.NotificationType.DiedFromStarvation:
        return "Notifications/Follower/DiedFromStarvation";
      case NotificationCentre.NotificationType.DiedFromIllness:
        return "Notifications/Follower/DiedFromIllness";
      case NotificationCentre.NotificationType.BecomeDissenter:
        return "Notifications/Follower/BecomeDissenter";
      case NotificationCentre.NotificationType.BecomeIll:
        return "Notifications/Follower/BecomeIll";
      case NotificationCentre.NotificationType.NoLongerIll:
        return "Notifications/Follower/NoLongerIll";
      case NotificationCentre.NotificationType.NoLongerStarving:
        return "Notifications/Follower/NoLongerStarving";
      case NotificationCentre.NotificationType.ResearchComplete:
        return "Notifications/NewBuildingUnlocked";
      case NotificationCentre.NotificationType.BuildComplete:
        return "Notifications/BuildComplete";
      case NotificationCentre.NotificationType.AteRottenFood:
        return "Notifications/Follower/AteRottenFood";
      case NotificationCentre.NotificationType.BecomeUnwell:
        return "Notifications/Follower/BecomeUnwell";
      case NotificationCentre.NotificationType.NewUpgradePoint:
        return "Notifications/NewUpgradePoint";
      case NotificationCentre.NotificationType.NewRecruit:
        return "Notifications/NewRecruit";
      case NotificationCentre.NotificationType.FoodBecomeRotten:
        return "Notifications/FoodBecomeRotten";
      case NotificationCentre.NotificationType.LowFaithDonation:
        return "Notifications/LowFaithDonation";
      case NotificationCentre.NotificationType.BecomeOld:
        return "Notifications/Follower/BecomeOld";
      case NotificationCentre.NotificationType.DiedFromOldAge:
        return "Notifications/Follower/DiedFromOldAge";
      case NotificationCentre.NotificationType.QuestComplete:
        return "Notifications/QuestComplete";
      case NotificationCentre.NotificationType.HolidayComplete:
        return "Notifications/HolidayComplete";
      case NotificationCentre.NotificationType.KilledInAFightPit:
        return "Notifications/Follower/KilledInAFightPit";
      case NotificationCentre.NotificationType.QuestFailed:
        return "Notifications/QuestFailed";
      case NotificationCentre.NotificationType.LostRespect:
        return "Notifications/LostRespect";
      case NotificationCentre.NotificationType.DemonConverted:
        return "Notifications/Follower/DemonConverted";
      case NotificationCentre.NotificationType.DemonPreserved:
        return "Notifications/Follower/DemonPreserved";
      case NotificationCentre.NotificationType.FaithEnforcerAssigned:
        return "Notifications/Follower/FaithEnforcerAssigned";
      case NotificationCentre.NotificationType.TaxEnforcerAssigned:
        return "Notifications/Follower/TaxEnforcerAssigned";
      case NotificationCentre.NotificationType.SacrificedAwayFromCult:
        return "Notifications/Follower/SacrificedAwayFromCult";
      case NotificationCentre.NotificationType.UpgradeRitualReady:
        return "Notifications/UpgradeRitualReady";
      default:
        return "MISSING LOCALISATION: " + (object) notificationType;
    }
  }

  public static NotificationBase.Flair GetFlair(NotificationCentre.NotificationType type)
  {
    switch (type)
    {
      case NotificationCentre.NotificationType.Died:
      case NotificationCentre.NotificationType.DiedFromStarvation:
      case NotificationCentre.NotificationType.DiedFromIllness:
      case NotificationCentre.NotificationType.DiedFromOldAge:
      case NotificationCentre.NotificationType.DiedFromDeadlyMeal:
        return NotificationBase.Flair.Negative;
      default:
        return NotificationBase.Flair.None;
    }
  }

  private void LateUpdate() => this.notificationsThisFrame.Clear();

  private void OnTwitchNotificationReceived(string viewerDisplayName, string notificationType)
  {
    if (!(notificationType == "TOTEM_CONTRIBUTION") || TwitchTotem.Deactivated)
      return;
    this.PlayTwitchNotification(string.Format(LocalizationManager.GetTranslation("Notifications/Twitch/TotemContribution"), (object) viewerDisplayName));
  }

  public enum NotificationType
  {
    Died,
    Starving,
    DiedFromStarvation,
    LeaveCult,
    DiedFromIllness,
    RelationshipFriend,
    RelationshipLover,
    RelationshipEnemy,
    LevellingUp,
    RelationshipWasKilledBy,
    BecomeDissenter,
    StopBeingDissenter,
    Hungry,
    BecomeIll,
    NoLongerIll,
    NoLongerStarving,
    Brainwashed,
    NoLongerBrainwashed,
    ReadyToLevelUp,
    LeaveCultUnhappy,
    Exhausted,
    Tired,
    Dynamic_Starving,
    Dynamic_Homeless,
    ResearchComplete,
    BuildComplete,
    AteRottenFood,
    Homeless,
    BecomeUnwell,
    BloodMoon3,
    BloodMoon2,
    BloodMoon1,
    NewUpgradePoint,
    SacrificeFollower,
    NewRecruit,
    TraitAdded,
    TraitRemoved,
    WeaponDamaged,
    WeaponDestroyed,
    CurseDamaged,
    CurseDestroyed,
    NoWorkers,
    MurderedByYou,
    FastComplete,
    FoodBecomeRotten,
    LowFaithDonation,
    BecomeOld,
    DiedFromOldAge,
    LevelUp,
    GainedHeart,
    GainedStrength,
    QuestComplete,
    Ascended,
    HolidayComplete,
    WorkThroughNightRitualComplete,
    ConstructionRitualComplete,
    EnlightenmentRitualComplete,
    FishingRitualComplete,
    KilledInAFightPit,
    QuestFailed,
    LostRespect,
    FirePitBegan,
    FeastTableBegan,
    LevelUpCentreScreen,
    DemonConverted,
    DemonPreserved,
    ConsumeFollower,
    ZombieKilledFollower,
    ZombieSpawned,
    FaithEnforcerAssigned,
    TaxEnforcerAssigned,
    SacrificedAwayFromCult,
    Dynamic_Sick,
    Disciple,
    UpgradeRitualReady,
    FaithUp,
    FaithUpDoubleArrow,
    FaithDown,
    FaithDownDoubleArrow,
    None,
    DiedFromDeadlyMeal,
    RitualHoliday,
    RitualWorkThroughNight,
    RitualFasterBuilding,
    RitualFast,
    RitualFishing,
    RitualBrainwashing,
    RitualEnlightenment,
    EnemiesStronger,
    Dynamic_Dissenter,
    RitualHalloween,
  }
}
