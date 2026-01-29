// Decompiled with JetBrains decompiler
// Type: NotificationCentre
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class NotificationCentre : BaseMonoBehaviour
{
  public const float kShowDuration = 0.5f;
  public const float kHideDuration = 0.5f;
  public static bool NotificationsEnabled = true;
  public static NotificationCentre Instance;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _notificationContainer;
  [Header("Templates - Standard")]
  [SerializeField]
  public NotificationGeneric _genericNotificationTemplate;
  [SerializeField]
  public NotificationTwitch _twitchNotificationTemplate;
  [SerializeField]
  public NotificationItem _itemNotificationTemplate;
  [SerializeField]
  public NotificationFaith _faithNotificationTemplate;
  [SerializeField]
  public NotificationFaith _warmthNotificationTemplate;
  [SerializeField]
  public NotificationFaith _sinNotificationTemplate;
  [SerializeField]
  public NotificationFollower _followerNotificationTemplate;
  [SerializeField]
  public NotificationRelationship _relationshipNotificationTemplate;
  [SerializeField]
  public NotificationHelpHinder _helpHinderNotificationTemplate;
  public static List<NotificationBase> Notifications = new List<NotificationBase>();
  public List<string> notificationsThisFrame = new List<string>();
  public Vector2 _onScreenPosition;
  public Vector2 _offScreenPosition;
  public Vector2 _onScreenPosCoopOff = new Vector2(-200f, -30f);
  public Vector2 _onScreenPosCoopOn = new Vector2(-200f, -120f);
  public bool isNotificationsShown;
  public Coroutine showCoroutine;
  public Coroutine hideCoroutine;

  public void Awake()
  {
    this._onScreenPosition = this._offScreenPosition = new Vector2(-200f, -30f);
    this._offScreenPosition.x = (float) (-(double) this._rectTransform.sizeDelta.x - 900.0);
    TwitchManager.OnNotificationReceived += new TwitchManager.NotificationResponse(this.OnTwitchNotificationReceived);
    this._genericNotificationTemplate.CreatePoolUI<NotificationGeneric>(2, false);
    this._itemNotificationTemplate.CreatePoolUI<NotificationItem>(5, false);
    this._faithNotificationTemplate.CreatePoolUI<NotificationFaith>(2, false);
    this._sinNotificationTemplate.CreatePoolUI<NotificationFaith>(2, false);
    this._followerNotificationTemplate.CreatePoolUI<NotificationFollower>(2, false);
    this._relationshipNotificationTemplate.CreatePoolUI<NotificationRelationship>(2, false);
  }

  public void SetCoopPositions(bool state)
  {
    this._onScreenPosition = this._offScreenPosition = state ? this._onScreenPosCoopOn : this._onScreenPosCoopOff;
    this._offScreenPosition.x = (float) (-(double) this._rectTransform.sizeDelta.x - 900.0);
  }

  public void OnDestroy()
  {
    TwitchManager.OnNotificationReceived -= new TwitchManager.NotificationResponse(this.OnTwitchNotificationReceived);
  }

  public void OnEnable()
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

  public void OnDisable()
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
    this.StopCoroutines();
    if (instant)
    {
      this._rectTransform.anchoredPosition = this._onScreenPosition;
      this.isNotificationsShown = true;
    }
    else
      this.showCoroutine = this.StartCoroutine((IEnumerator) this.DoShow());
  }

  public IEnumerator DoShow()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._onScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.isNotificationsShown = true;
  }

  public void Hide(bool instant = false)
  {
    this._rectTransform.DOKill();
    this.StopCoroutines();
    if (instant)
    {
      this._rectTransform.anchoredPosition = this._offScreenPosition;
      this.isNotificationsShown = false;
    }
    else
      this.hideCoroutine = this.StartCoroutine((IEnumerator) this.DoHide());
  }

  public IEnumerator DoHide()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._offScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    this.isNotificationsShown = false;
  }

  public void StopCoroutines()
  {
    if (this.showCoroutine != null)
    {
      this.StopCoroutine(this.showCoroutine);
      this.showCoroutine = (Coroutine) null;
    }
    if (this.hideCoroutine == null)
      return;
    this.StopCoroutine(this.hideCoroutine);
    this.hideCoroutine = (Coroutine) null;
  }

  public void CurseBroken()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.CurseDestroyed);
  }

  public void CurseDamaged()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.CurseDamaged);
  }

  public void WeaponBroken()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.WeaponDestroyed);
  }

  public void WeaponDamaged()
  {
    NotificationCentreScreen.Play(NotificationCentre.NotificationType.WeaponDamaged);
  }

  public void OnReadyToLevelUp(int brainID, float newValue, float oldValue, float change)
  {
    this.PlayFollowerNotification(NotificationCentre.NotificationType.ReadyToLevelUp, FollowerBrain.FindBrainByID(brainID).Info, NotificationFollower.Animation.Happy);
  }

  public void OnFollowerLevelUp(int brainID, float newValue, float oldValue, float change)
  {
    this.PlayFollowerNotification(NotificationCentre.NotificationType.LevellingUp, FollowerBrain.FindBrainByID(brainID).Info, NotificationFollower.Animation.Happy);
  }

  public void ItemAddedToInventory(InventoryItem.ITEM_TYPE itemType, int delta)
  {
    if (!this.CheckToShowHUD(itemType))
      return;
    this.PlayItemNotification(itemType, delta);
  }

  public bool CheckToShowHUD(InventoryItem.ITEM_TYPE type)
  {
    return type != InventoryItem.ITEM_TYPE.SEEDS && type != InventoryItem.ITEM_TYPE.INGREDIENTS;
  }

  public void OnRestStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (newState != FollowerStatState.Urgent)
      return;
    this.PlayFollowerNotification(NotificationCentre.NotificationType.Exhausted, brainById.Info, NotificationFollower.Animation.Tired);
  }

  public void OnChangeRelationship(
    FollowerInfo f1,
    FollowerInfo f2,
    IDAndRelationship.RelationshipState state)
  {
    NotificationFollower.Animation animation = NotificationFollower.Animation.Happy;
    NotificationCentre.NotificationType type = NotificationCentre.NotificationType.RelationshipFriend;
    switch (state)
    {
      case IDAndRelationship.RelationshipState.Enemies:
        type = NotificationCentre.NotificationType.RelationshipEnemy;
        animation = NotificationFollower.Animation.Angry;
        break;
      case IDAndRelationship.RelationshipState.Lovers:
        type = NotificationCentre.NotificationType.RelationshipLover;
        animation = NotificationFollower.Animation.Happy;
        break;
    }
    this.PlayRelationshipNotification(type, f1, animation, f2, animation);
  }

  public void OnEatRottenFood(int brainID)
  {
    this.PlayFollowerNotification(NotificationCentre.NotificationType.AteRottenFood, FollowerBrain.FindBrainByID(brainID).Info, NotificationFollower.Animation.Sick);
  }

  public void PlayItemNotification(InventoryItem.ITEM_TYPE itemType, int delta)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    this.StartCoroutine((IEnumerator) this.PlayItemNotificationIE(itemType, delta));
  }

  public IEnumerator PlayItemNotificationIE(InventoryItem.ITEM_TYPE itemType, int delta)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayItemNotificationIE\u003Eb__47_0));
    foreach (NotificationBase notification in NotificationCentre.Notifications)
    {
      if (notification is NotificationItem notificationItem && notificationItem.ItemType == itemType)
      {
        notificationItem.UpdateDelta(delta);
        yield break;
      }
    }
    string str = itemType.ToString();
    if (!notificationCentre.notificationsThisFrame.Contains(str))
    {
      notificationCentre.notificationsThisFrame.Add(str);
      NotificationItem notificationItem = notificationCentre._itemNotificationTemplate.SpawnUI<NotificationItem>((Transform) notificationCentre._notificationContainer, false);
      notificationItem.Configure(itemType, delta);
      NotificationCentre.Notifications.Add((NotificationBase) notificationItem);
    }
  }

  public void PlayGenericNotification(string locKey, NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    this.notificationsThisFrame.Add(locKey);
    this.StartCoroutine((IEnumerator) this.PlayGenericNotificationIE(locKey, flair));
  }

  public IEnumerator PlayGenericNotificationIE(string locKey, NotificationBase.Flair flair = NotificationBase.Flair.None)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayGenericNotificationIE\u003Eb__49_0));
    NotificationGeneric notificationGeneric = notificationCentre._genericNotificationTemplate.SpawnUI<NotificationGeneric>((Transform) notificationCentre._notificationContainer, false);
    notificationGeneric.Configure(locKey, flair);
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    if (flair == NotificationBase.Flair.Winter)
      notificationGeneric.SetOverrideShowDuration(10f);
    FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
    notificationSimple.LocKey = locKey;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) notificationSimple);
  }

  public void PlayGenericNotification(NotificationCentre.NotificationType type)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = type.ToString();
    if (this.notificationsThisFrame.Contains(str))
      return;
    this.notificationsThisFrame.Add(str);
    this.StartCoroutine((IEnumerator) this.PlayGenericNotificationIE(type));
  }

  public IEnumerator PlayGenericNotificationIE(NotificationCentre.NotificationType type)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayGenericNotificationIE\u003Eb__51_0));
    NotificationGeneric notificationGeneric = notificationCentre._genericNotificationTemplate.SpawnUI<NotificationGeneric>((Transform) notificationCentre._notificationContainer, false);
    notificationGeneric.Configure(type, NotificationCentre.GetFlair(type));
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
    notificationSimple.LocKey = NotificationCentre.GetLocKey(type);
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) notificationSimple);
  }

  public void PlayGenericNotificationNonLocalizedParams(
    string locKey,
    params string[] nonLocalizedParameters)
  {
    if (!NotificationCentre.NotificationsEnabled || this.ContainsNotificationThisFrameWithParams(locKey, nonLocalizedParameters))
      return;
    this.notificationsThisFrame.Add(locKey + (nonLocalizedParameters.Length != 0 ? nonLocalizedParameters[0] : ""));
    this.StartCoroutine((IEnumerator) this.PlayGenericNotificationNonLocalizedParamsIE(locKey, nonLocalizedParameters));
  }

  public IEnumerator PlayGenericNotificationNonLocalizedParamsIE(
    string locKey,
    params string[] nonLocalizedParameters)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayGenericNotificationNonLocalizedParamsIE\u003Eb__53_0));
    NotificationGeneric notificationGeneric = notificationCentre._genericNotificationTemplate.SpawnUI<NotificationGeneric>((Transform) notificationCentre._notificationContainer, false);
    notificationGeneric.ConfigureNonLocalizedParams(locKey, nonLocalizedParameters);
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
    notificationSimple.LocKey = locKey;
    notificationSimple.NonLocalisedParameters = nonLocalizedParameters;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) notificationSimple);
  }

  public void PlayGenericNotificationLocalizedParams(
    string locKey,
    params string[] localizedParameters)
  {
    if (!NotificationCentre.NotificationsEnabled || this.ContainsNotificationThisFrameWithParams(locKey, localizedParameters))
      return;
    this.notificationsThisFrame.Add(locKey + (localizedParameters.Length != 0 ? localizedParameters[0] : ""));
    this.StartCoroutine((IEnumerator) this.PlayGenericNotificationLocalizedParamsIE(locKey, localizedParameters));
  }

  public IEnumerator PlayGenericNotificationLocalizedParamsIE(
    string locKey,
    params string[] localizedParameters)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayGenericNotificationLocalizedParamsIE\u003Eb__55_0));
    NotificationGeneric notificationGeneric = notificationCentre._genericNotificationTemplate.SpawnUI<NotificationGeneric>((Transform) notificationCentre._notificationContainer, false);
    notificationGeneric.ConfigureLocalizedParams(locKey, localizedParameters);
    NotificationCentre.Notifications.Add((NotificationBase) notificationGeneric);
    FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
    notificationSimple.LocKey = locKey;
    notificationSimple.LocalisedParameters = localizedParameters;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) notificationSimple);
  }

  public void PlayTwitchNotification(string locKey, params string[] localizedParameters)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    this.notificationsThisFrame.Add(locKey);
    this.StartCoroutine((IEnumerator) this.PlayTwitchNotificationIE(locKey, localizedParameters));
  }

  public IEnumerator PlayTwitchNotificationIE(string locKey, params string[] localizedParameters)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayTwitchNotificationIE\u003Eb__57_0));
    NotificationTwitch notificationTwitch = notificationCentre._twitchNotificationTemplate.SpawnUI<NotificationTwitch>((Transform) notificationCentre._notificationContainer, false);
    notificationTwitch.ConfigureLocalizedParams(locKey, localizedParameters);
    NotificationCentre.Notifications.Add((NotificationBase) notificationTwitch);
    FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
    notificationSimple.LocKey = locKey;
    notificationSimple.LocalisedParameters = localizedParameters;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) notificationSimple);
  }

  public void PlayHelpHinderNotification(string locKey)
  {
    if (!NotificationCentre.NotificationsEnabled || this.notificationsThisFrame.Contains(locKey))
      return;
    this.notificationsThisFrame.Add(locKey);
    this.StartCoroutine((IEnumerator) this.PlayHelpHinderNotificationIE(locKey));
  }

  public IEnumerator PlayHelpHinderNotificationIE(string locKey)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayHelpHinderNotificationIE\u003Eb__59_0));
    NotificationHelpHinder notificationHelpHinder = notificationCentre._helpHinderNotificationTemplate.SpawnUI<NotificationHelpHinder>((Transform) notificationCentre._notificationContainer, false);
    notificationHelpHinder.ConfigureLocalizedParams(locKey);
    NotificationCentre.Notifications.Add((NotificationBase) notificationHelpHinder);
    FinalizedNotificationSimple notificationSimple = new FinalizedNotificationSimple();
    notificationSimple.LocKey = locKey;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) notificationSimple);
  }

  public void PlaySinNotification(
    string locKey,
    float sinDelta,
    NotificationBase.Flair flair,
    int followerID = -1,
    params string[] args)
  {
    if (!NotificationCentre.NotificationsEnabled)
      return;
    string str = $"{locKey}{sinDelta}{followerID}";
    if (this.notificationsThisFrame.Contains(str))
      return;
    this.notificationsThisFrame.Add(str);
    this.StartCoroutine((IEnumerator) this.PlaySinNotificationIE(locKey, sinDelta, flair, followerID, args));
  }

  public IEnumerator PlaySinNotificationIE(
    string locKey,
    float sinDelta,
    NotificationBase.Flair flair,
    int followerID = -1,
    params string[] args)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlaySinNotificationIE\u003Eb__61_0));
    FollowerInfo infoById = FollowerInfo.GetInfoByID(followerID, true);
    if (infoById == null || !infoById.IsSnowman)
    {
      NotificationFaith notificationFaith = notificationCentre._sinNotificationTemplate.SpawnUI<NotificationFaith>((Transform) notificationCentre._notificationContainer, false);
      notificationFaith.Configure(locKey, sinDelta, infoById, true, flair, false, args);
      NotificationCentre.Notifications.Add((NotificationBase) notificationFaith);
      FinalizedFaithNotification faithNotification = new FinalizedFaithNotification();
      faithNotification.LocKey = locKey;
      faithNotification.FaithDelta = sinDelta;
      faithNotification.followerInfoSnapshot = infoById != null ? new FollowerInfoSnapshot(infoById) : (FollowerInfoSnapshot) null;
      faithNotification.LocalisedParameters = args;
      DataManager.Instance.AddToNotificationHistory((FinalizedNotification) faithNotification);
    }
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
    if (infoById == null && followerID != -1 || infoById != null && infoById.IsSnowman)
      return;
    this.notificationsThisFrame.Add(str);
    if ((bool) (UnityEngine.Object) GameManager.GetInstance())
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlayFaithNotificationIE(locKey, faithDelta, flair, infoById, followerID, args));
    else
      this.StartCoroutine((IEnumerator) this.PlayFaithNotificationIE(locKey, faithDelta, flair, infoById, followerID, args));
  }

  public IEnumerator PlayFaithNotificationIE(
    string locKey,
    float faithDelta,
    NotificationBase.Flair flair,
    FollowerInfo followerInfo,
    int followerID = -1,
    params string[] args)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayFaithNotificationIE\u003Eb__63_0));
    NotificationFaith notificationFaith = notificationCentre._faithNotificationTemplate.SpawnUI<NotificationFaith>((Transform) notificationCentre._notificationContainer, false);
    notificationFaith.Configure(locKey, faithDelta, followerInfo, true, flair, false, args);
    NotificationCentre.Notifications.Add((NotificationBase) notificationFaith);
    FinalizedFaithNotification faithNotification = new FinalizedFaithNotification();
    faithNotification.LocKey = locKey;
    faithNotification.FaithDelta = faithDelta;
    faithNotification.followerInfoSnapshot = followerInfo != null ? new FollowerInfoSnapshot(followerInfo) : (FollowerInfoSnapshot) null;
    faithNotification.LocalisedParameters = args;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) faithNotification);
  }

  public void PlayWarmthNotification(
    string locKey,
    float faithDelta,
    NotificationBase.Flair flair,
    params string[] args)
  {
    if (!NotificationCentre.NotificationsEnabled || FollowerBrainStats.LockedWarmth)
      return;
    string str = $"{locKey}{faithDelta}";
    if (this.notificationsThisFrame.Contains(str))
      return;
    this.notificationsThisFrame.Add(str);
    if ((bool) (UnityEngine.Object) GameManager.GetInstance())
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.PlayWarmthNotificationIE(locKey, faithDelta, flair, args));
    else
      this.StartCoroutine((IEnumerator) this.PlayWarmthNotificationIE(locKey, faithDelta, flair, args));
  }

  public IEnumerator PlayWarmthNotificationIE(
    string locKey,
    float faithDelta,
    NotificationBase.Flair flair,
    params string[] args)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayWarmthNotificationIE\u003Eb__65_0));
    foreach (NotificationBase notification in NotificationCentre.Notifications)
    {
      if (notification is NotificationFaith notificationFaith && notificationFaith.LocKey == locKey)
      {
        notificationFaith.UpdateDelta(faithDelta, true, true);
        yield break;
      }
    }
    NotificationFaith notificationFaith1 = notificationCentre._warmthNotificationTemplate.SpawnUI<NotificationFaith>((Transform) notificationCentre._notificationContainer, false);
    notificationFaith1.Configure(locKey, faithDelta, (FollowerInfo) null, true, flair, true, args);
    NotificationCentre.Notifications.Add((NotificationBase) notificationFaith1);
    FinalizedFaithNotification faithNotification = new FinalizedFaithNotification();
    faithNotification.LocKey = locKey;
    faithNotification.FaithDelta = faithDelta;
    faithNotification.followerInfoSnapshot = (FollowerInfoSnapshot) null;
    faithNotification.LocalisedParameters = args;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) faithNotification);
  }

  public void PlayFollowerNotification(
    NotificationCentre.NotificationType type,
    FollowerBrainInfo info,
    NotificationFollower.Animation followerAnimation)
  {
    if (!NotificationCentre.NotificationsEnabled || info != null && info.IsSnowman)
      return;
    string str = string.Format(type.ToString(), (object) info.GetHashCode());
    if (this.notificationsThisFrame.Contains(str))
      return;
    this.notificationsThisFrame.Add(str);
    this.StartCoroutine((IEnumerator) this.PlayFollowerNotificationIE(type, info, followerAnimation));
  }

  public IEnumerator PlayFollowerNotificationIE(
    NotificationCentre.NotificationType type,
    FollowerBrainInfo info,
    NotificationFollower.Animation followerAnimation)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayFollowerNotificationIE\u003Eb__67_0));
    FollowerInfo infoById = FollowerInfo.GetInfoByID(info.ID);
    NotificationFollower notificationFollower = notificationCentre._followerNotificationTemplate.SpawnUI<NotificationFollower>((Transform) notificationCentre._notificationContainer, false);
    notificationFollower.Configure(type, FollowerInfo.GetInfoByID(info.ID), followerAnimation, NotificationCentre.GetFlair(type));
    NotificationCentre.Notifications.Add((NotificationBase) notificationFollower);
    if (infoById != null)
    {
      FinalizedFollowerNotification followerNotification = new FinalizedFollowerNotification();
      followerNotification.LocKey = NotificationCentre.GetLocKey(type);
      followerNotification.followerInfoSnapshot = new FollowerInfoSnapshot(infoById);
      followerNotification.Animation = followerAnimation;
      DataManager.Instance.AddToNotificationHistory((FinalizedNotification) followerNotification);
    }
  }

  public void PlayRelationshipNotification(
    NotificationCentre.NotificationType type,
    FollowerInfo followerInfo,
    NotificationFollower.Animation followerAnimation,
    FollowerInfo otherFollowerInfo,
    NotificationFollower.Animation otherAnimation)
  {
    if (!NotificationCentre.NotificationsEnabled || followerInfo != null && followerInfo.IsSnowman)
      return;
    string str = followerInfo.GetHashCode().ToString() + otherFollowerInfo.GetHashCode().ToString();
    if (this.notificationsThisFrame.Contains(str))
      return;
    this.notificationsThisFrame.Add(str);
    this.StartCoroutine((IEnumerator) this.PlayRelationshipNotificationIE(type, followerInfo, followerAnimation, otherFollowerInfo, otherAnimation));
  }

  public IEnumerator PlayRelationshipNotificationIE(
    NotificationCentre.NotificationType type,
    FollowerInfo followerInfo,
    NotificationFollower.Animation followerAnimation,
    FollowerInfo otherFollowerInfo,
    NotificationFollower.Animation otherAnimation)
  {
    NotificationCentre notificationCentre = this;
    if (!notificationCentre.isNotificationsShown)
      yield return (object) new WaitUntil((Func<bool>) new Func<bool>(notificationCentre.\u003CPlayRelationshipNotificationIE\u003Eb__69_0));
    NotificationRelationship notificationRelationship = notificationCentre._relationshipNotificationTemplate.SpawnUI<NotificationRelationship>((Transform) notificationCentre._notificationContainer, false);
    notificationRelationship.Configure(type, followerInfo, otherFollowerInfo, followerAnimation, otherAnimation, NotificationCentre.GetFlair(type));
    NotificationCentre.Notifications.Add((NotificationBase) notificationRelationship);
    FinalizedRelationshipNotification relationshipNotification = new FinalizedRelationshipNotification();
    relationshipNotification.LocKey = $"Notifications/Relationship/{type}";
    relationshipNotification.followerInfoSnapshotA = new FollowerInfoSnapshot(followerInfo);
    relationshipNotification.followerInfoSnapshotB = new FollowerInfoSnapshot(otherFollowerInfo);
    relationshipNotification.FollowerAnimationA = followerAnimation;
    relationshipNotification.FollowerAnimationB = otherAnimation;
    DataManager.Instance.AddToNotificationHistory((FinalizedNotification) relationshipNotification);
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
      case NotificationCentre.NotificationType.StopBeingDissenter:
        return "Notifications/Cult_CureDissenter/Notification/On";
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
      case NotificationCentre.NotificationType.BecomeDrunk:
        return "Notifications/Follower/BecomeDrunk";
      case NotificationCentre.NotificationType.NewPleasurePoint:
        return "Notifications/NewPleasurePoint";
      case NotificationCentre.NotificationType.NoLongerInjured:
        return "Notifications/Follower/NoLongerInjured";
      case NotificationCentre.NotificationType.NoLongerOld:
        return "Notifications/NoLongerOld";
      case NotificationCentre.NotificationType.InjuredFromFightSingle:
        return "Notifications/InjuredFromFightSingle";
      case NotificationCentre.NotificationType.ReachedAdulthood:
        return "Notifications/ReachedAdulthood";
      case NotificationCentre.NotificationType.Freezing:
        return "Notifications/FollowerFreezing";
      case NotificationCentre.NotificationType.Soaking:
        return "Notifications/FollowerSoaking";
      case NotificationCentre.NotificationType.Overheating:
        return "Notifications/FollowerOverheating";
      case NotificationCentre.NotificationType.Aflame:
        return "Notifications/FollowerAflame";
      case NotificationCentre.NotificationType.NoLongerFreezing:
        return "Notifications/Follower/NoLongerFreezing";
      case NotificationCentre.NotificationType.KilledByBlizzardMonster:
        return "Notifications/KilledByBlizzardMonster";
      case NotificationCentre.NotificationType.RanchOvercrowded:
        return "Notifications/RanchOvercrowded";
      case NotificationCentre.NotificationType.ResurrectedFromNecklace:
        return "Notifications/ResurrectedFromNecklace";
      case NotificationCentre.NotificationType.RanchAnimalStinky:
        return "Notifications/RanchAnimalStinky";
      case NotificationCentre.NotificationType.RanchAnimalFeral:
        return "Notifications/RanchAnimalFeral";
      case NotificationCentre.NotificationType.RanchNotOvercrowded:
        return "Notifications/RanchNotOvercrowded";
      case NotificationCentre.NotificationType.UnfrozeFollower:
        return "Notifications/UnfrozeFollower";
      case NotificationCentre.NotificationType.ClearedBlizzard:
        return "Notifications/ClearedBlizzard";
      case NotificationCentre.NotificationType.Furnace_Low:
        return "Notifications/Furnace_Low";
      case NotificationCentre.NotificationType.Furnace_Empty:
        return "Notifications/Furnace_Empty";
      case NotificationCentre.NotificationType.Furnace_On:
        return "Notifications/Furnace_On";
      case NotificationCentre.NotificationType.RotCantFreeze:
        return "Notifications/RotCantFreeze";
      case NotificationCentre.NotificationType.RotCantIll:
        return "Notifications/RotCantIll";
      case NotificationCentre.NotificationType.RotCantStarve:
        return "Notifications/RotCantStarve";
      case NotificationCentre.NotificationType.RotCantDissent:
        return "Notifications/RotCantDissent";
      default:
        return "MISSING LOCALISATION: " + notificationType.ToString();
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
      case NotificationCentre.NotificationType.Furnace_Low:
      case NotificationCentre.NotificationType.Furnace_Empty:
      case NotificationCentre.NotificationType.Furnace_On:
        return NotificationBase.Flair.Winter;
      default:
        return NotificationBase.Flair.None;
    }
  }

  public void ClearNotifications()
  {
    this.StopAllCoroutines();
    for (int index = this._notificationContainer.childCount - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this._notificationContainer.transform.GetChild(index).gameObject);
    this.notificationsThisFrame.Clear();
  }

  public bool ContainsNotificationThisFrameWithParams(string locKey, string[] parameters)
  {
    string str = locKey;
    foreach (string parameter in parameters)
      str += parameter;
    return this.notificationsThisFrame.Contains(str);
  }

  public void LateUpdate() => this.notificationsThisFrame.Clear();

  public void OnTwitchNotificationReceived(string viewerDisplayName, string notificationType)
  {
    if (!(notificationType == "TOTEM_CONTRIBUTION") || TwitchTotem.Deactivated)
      return;
    this.PlayTwitchNotification(string.Format(LocalizationManager.GetTranslation("Notifications/Twitch/TotemContribution"), (object) viewerDisplayName));
  }

  [CompilerGenerated]
  public bool \u003CPlayItemNotificationIE\u003Eb__47_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayGenericNotificationIE\u003Eb__49_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayGenericNotificationIE\u003Eb__51_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayGenericNotificationNonLocalizedParamsIE\u003Eb__53_0()
  {
    return this.isNotificationsShown;
  }

  [CompilerGenerated]
  public bool \u003CPlayGenericNotificationLocalizedParamsIE\u003Eb__55_0()
  {
    return this.isNotificationsShown;
  }

  [CompilerGenerated]
  public bool \u003CPlayTwitchNotificationIE\u003Eb__57_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayHelpHinderNotificationIE\u003Eb__59_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlaySinNotificationIE\u003Eb__61_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayFaithNotificationIE\u003Eb__63_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayWarmthNotificationIE\u003Eb__65_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayFollowerNotificationIE\u003Eb__67_0() => this.isNotificationsShown;

  [CompilerGenerated]
  public bool \u003CPlayRelationshipNotificationIE\u003Eb__69_0() => this.isNotificationsShown;

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
    StoleGold,
    MurderedByFollower,
    DiedOfInjury,
    Dynamic_Injured,
    RitualPurge,
    RitualNudism,
    Dynamic_Drunk,
    BecomeDrunk,
    NewPleasurePoint,
    NoLongerInjured,
    DiedFromBeingEatenBySozo,
    DiedOnMissionary,
    NoLongerOld,
    BecamePossessed,
    InjuredFromFightSingle,
    ReachedAdulthood,
    LeaveCultSpy,
    DiedFromBeingEaten,
    Freezing,
    FrozeToDeath,
    StruckByLightning,
    Soaking,
    Overheating,
    DiedFromOverheating,
    BurntToDeath,
    Aflame,
    Blizzard,
    Typhoon,
    Heatwave,
    Dynamic_Freezing,
    MeltedToDeath,
    NoLongerFreezing,
    FreezingCult,
    WarmCult,
    KilledByBlizzardMonster,
    RitualRanch_Meat,
    RitualRanch_Harvest,
    DiedFromRot,
    RanchOvercrowded,
    ResurrectedFromNecklace,
    RanchAnimalStinky,
    RanchAnimalFeral,
    RanchNotOvercrowded,
    UnfrozeFollower,
    ClearedBlizzard,
    Furnace_Low,
    Furnace_Empty,
    Furnace_On,
    RotCantFreeze,
    RotCantIll,
    RotCantStarve,
    RotCantDissent,
  }
}
