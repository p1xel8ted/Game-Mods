// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIDynamicNotificationCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MMBiomeGeneration;
using src.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unify;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIDynamicNotificationCenter : BaseMonoBehaviour
{
  public const float kShowDuration = 0.5f;
  public const float kHideDuration = 0.5f;
  public static UIDynamicNotificationCenter Instance;
  [SerializeField]
  public RectTransform _rectTransform;
  [SerializeField]
  public RectTransform _dynamicNotificationContainer;
  [Header("Templates")]
  [SerializeField]
  public NotificationDynamicGeneric _genericDynamicNotificationTemplate;
  [SerializeField]
  public NotificationDynamicCursed _cursedDynamicNotificationTemplate;
  [SerializeField]
  public NotificationDynamicRitual _ritualDynamicNotificationTemplate;
  [SerializeField]
  public NotificationDynamicWeather _weatherDynamicNotificationTemplate;
  public static List<NotificationDynamicBase> NotificationsDynamic = new List<NotificationDynamicBase>();
  [CompilerGenerated]
  public static List<DynamicNotificationData> \u003CDynamicNotifications\u003Ek__BackingField = new List<DynamicNotificationData>();
  public static DynamicNotification_StarvingFollower StarvingFollowerData;
  public static DynamicNotification_HomelessFollower HomelessFollowerData;
  public static DynamicNotification_SickFolllower SickFollowerData;
  public static DynamicNotification_ExhaustedFolllower ExhaustedFollowerData;
  public static DynamicNotification_DrunkFolllower DrunkFollowerData;
  public static DynamicNotification_DissentingFolllower DissentingFollowerData;
  public static DynamicNotification_InjuredFolllower InjuredFollowerData;
  public static DynamicNotification_FreezingFolllower FreezingFollowerData;
  public static DynamicNotification_RitualActive HolidayRitualData;
  public static DynamicNotification_RitualActive WorkThroughNightRitualData;
  public static DynamicNotification_RitualActive FastRitualData;
  public static DynamicNotification_RitualActive FishingRitualData;
  public static DynamicNotification_RitualActive BrainwashingRitualData;
  public static DynamicNotification_RitualActive EnlightenmentRitualData;
  public static DynamicNotification_RitualActive HalloweenRitualData;
  public static DynamicNotification_RitualActive PurgeRitualData;
  public static DynamicNotification_RitualActive NudistRitualData;
  public static DynamicNotification_RitualActive RanchHarvestRitualData;
  public static DynamicNotification_RitualActive RanchMeatRitualData;
  public static DynamicNotification_WeatherActive BlizzardData;
  public Vector2 _onScreenPosition;
  public Vector2 _offScreenPosition;
  public bool _initialized;

  public static List<DynamicNotificationData> DynamicNotifications
  {
    set => UIDynamicNotificationCenter.\u003CDynamicNotifications\u003Ek__BackingField = value;
    get => UIDynamicNotificationCenter.\u003CDynamicNotifications\u003Ek__BackingField;
  }

  public void Awake()
  {
    this._onScreenPosition = this._offScreenPosition = this._rectTransform.anchoredPosition;
    this._offScreenPosition.x = (float) (-(double) this._rectTransform.sizeDelta.x - 50.0);
    if (!((UnityEngine.Object) BiomeGenerator.Instance != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public static void Reset()
  {
    UIDynamicNotificationCenter.NotificationsDynamic.Clear();
    UIDynamicNotificationCenter.DynamicNotifications.Clear();
    UIDynamicNotificationCenter.StarvingFollowerData = (DynamicNotification_StarvingFollower) null;
    UIDynamicNotificationCenter.HomelessFollowerData = (DynamicNotification_HomelessFollower) null;
    UIDynamicNotificationCenter.SickFollowerData = (DynamicNotification_SickFolllower) null;
    UIDynamicNotificationCenter.ExhaustedFollowerData = (DynamicNotification_ExhaustedFolllower) null;
    UIDynamicNotificationCenter.DissentingFollowerData = (DynamicNotification_DissentingFolllower) null;
    UIDynamicNotificationCenter.DrunkFollowerData = (DynamicNotification_DrunkFolllower) null;
    UIDynamicNotificationCenter.InjuredFollowerData = (DynamicNotification_InjuredFolllower) null;
    UIDynamicNotificationCenter.FreezingFollowerData = (DynamicNotification_FreezingFolllower) null;
    UIDynamicNotificationCenter.HolidayRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.WorkThroughNightRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.FastRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.FishingRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.BrainwashingRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.EnlightenmentRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.HalloweenRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.PurgeRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.NudistRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.RanchMeatRitualData = (DynamicNotification_RitualActive) null;
    UIDynamicNotificationCenter.RanchHarvestRitualData = (DynamicNotification_RitualActive) null;
  }

  public void Start()
  {
    FollowerBrainStats.OnExhaustionStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnExhaustedStateChanged);
    FollowerBrainStats.OnIllnessStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    FollowerBrainStats.OnStarvationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationStateChanged);
    FollowerBrainStats.OnReeducationStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnReeducationStateChanged);
    FollowerBrainStats.OnInjuredStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnInjuredStateChanged);
    FollowerBrainStats.OnFreezingStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnFreezingStateChanged);
    FollowerBrainStats.OnDrunkStateChanged += new FollowerBrainStats.StatStateChangedEvent(this.OnDrunkStateChanged);
    FollowerBrain.OnBrainAdded += new Action<int>(this.OnBrainAdded);
    FollowerBrain.OnBrainRemoved += new Action<int>(this.OnBrainRemoved);
    FollowerManager.OnFollowerAdded += new FollowerManager.FollowerChanged(this.OnFollowerAdded);
    FollowerManager.OnFollowerRemoved += new FollowerManager.FollowerChanged(this.OnFollowerRemoved);
    FollowerManager.OnFollowerDie += new FollowerManager.FollowerGoneEvent(this.OnFollowerDie);
    FollowerManager.OnFollowerLeave += new FollowerManager.FollowerGoneEvent(this.OnFollowerDie);
    LocationManager.OnFollowersSpawned += new System.Action(this.OnFollowersSpawned);
    TimeManager.OnNewPhaseStarted += new System.Action(this.OnNewPhaseStarted);
    StructureManager.OnStructureAdded += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureUpgraded += new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    Structures_Bed.OnBedCollapsedStatic += new Structures_Bed.BedEvent(this.OnBedModified);
    Structures_Bed.OnBedRebuiltStatic += new Structures_Bed.BedEvent(this.OnBedModified);
    if (SaveAndLoad.Loaded)
      this.InitializeData();
    else
      SaveAndLoad.OnLoadComplete += new System.Action(this.InitializeData);
    RitualWorkHoliday.OnHolidayBegan += new System.Action(this.OnHolidayRitualBegan);
    RitualWorkThroughNight.OnWorkThroughNightBegan += new System.Action(this.OnWorkThroughNightRitualBegan);
    RitualFast.OnRitualFastingBegan += new System.Action(this.OnFastRitualBegan);
    RitualFishing.OnFishingRitualBegan += new System.Action(this.OnFishingRitualBegan);
    RitualBrainwashing.OnBrainwashingRitualBegan += new System.Action(this.OnBrainwashingRitualBegan);
    RitualElightenment.OnEnlightenmentRitualBegan += new System.Action(this.OnEnlightenmentRitualBegan);
    RitualHalloween.OnHalloweenRitualBegan += new System.Action(this.OnHalloweenRitualBegan);
    RitualPurge.OnPurgeBegan += new System.Action(this.OnPurgeRitualBegan);
    RitualNudism.OnNudismBegan += new System.Action(this.OnNudismRitualBegan);
    Ritual_RanchHarvest.OnRitualBegun += new System.Action(this.OnRanchHarvestRitualBegan);
    Ritual_RanchMeat.OnRitualBegun += new System.Action(this.OnRanchMeatRitualBegan);
  }

  public void OnDestroy()
  {
    FollowerBrainStats.OnExhaustionStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnExhaustedStateChanged);
    FollowerBrainStats.OnIllnessStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnIllnessStateChanged);
    FollowerBrainStats.OnStarvationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnStarvationStateChanged);
    FollowerBrainStats.OnReeducationStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnReeducationStateChanged);
    FollowerBrainStats.OnInjuredStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnInjuredStateChanged);
    FollowerBrainStats.OnDrunkStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnInjuredStateChanged);
    FollowerBrainStats.OnFreezingStateChanged -= new FollowerBrainStats.StatStateChangedEvent(this.OnFreezingStateChanged);
    FollowerBrain.OnBrainAdded -= new Action<int>(this.OnBrainAdded);
    FollowerBrain.OnBrainRemoved -= new Action<int>(this.OnBrainRemoved);
    FollowerManager.OnFollowerDie -= new FollowerManager.FollowerGoneEvent(this.OnFollowerDie);
    FollowerManager.OnFollowerAdded -= new FollowerManager.FollowerChanged(this.OnFollowerAdded);
    FollowerManager.OnFollowerRemoved -= new FollowerManager.FollowerChanged(this.OnFollowerRemoved);
    FollowerManager.OnFollowerLeave -= new FollowerManager.FollowerGoneEvent(this.OnFollowerDie);
    LocationManager.OnFollowersSpawned -= new System.Action(this.OnFollowersSpawned);
    TimeManager.OnNewPhaseStarted -= new System.Action(this.OnNewPhaseStarted);
    StructureManager.OnStructureAdded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructureUpgraded -= new StructureManager.StructureChanged(this.OnStructureAdded);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    Structures_Bed.OnBedCollapsedStatic -= new Structures_Bed.BedEvent(this.OnBedModified);
    Structures_Bed.OnBedRebuiltStatic -= new Structures_Bed.BedEvent(this.OnBedModified);
    RitualWorkHoliday.OnHolidayBegan -= new System.Action(this.OnHolidayRitualBegan);
    RitualWorkThroughNight.OnWorkThroughNightBegan -= new System.Action(this.OnWorkThroughNightRitualBegan);
    RitualFast.OnRitualFastingBegan -= new System.Action(this.OnFastRitualBegan);
    RitualFishing.OnFishingRitualBegan -= new System.Action(this.OnFishingRitualBegan);
    RitualBrainwashing.OnBrainwashingRitualBegan -= new System.Action(this.OnBrainwashingRitualBegan);
    RitualElightenment.OnEnlightenmentRitualBegan -= new System.Action(this.OnEnlightenmentRitualBegan);
    RitualHalloween.OnHalloweenRitualBegan -= new System.Action(this.OnHalloweenRitualBegan);
    RitualPurge.OnPurgeBegan -= new System.Action(this.OnPurgeRitualBegan);
    RitualNudism.OnNudismBegan -= new System.Action(this.OnNudismRitualBegan);
    Ritual_RanchHarvest.OnRitualBegun -= new System.Action(this.OnRanchHarvestRitualBegan);
    Ritual_RanchMeat.OnRitualBegun -= new System.Action(this.OnRanchMeatRitualBegan);
    SaveAndLoad.OnLoadComplete -= new System.Action(this.InitializeData);
  }

  public void OnEnable()
  {
    UIDynamicNotificationCenter.Instance = this;
    if (UIDynamicNotificationCenter.StarvingFollowerData == null)
      UIDynamicNotificationCenter.StarvingFollowerData = this.AddDynamicData<DynamicNotification_StarvingFollower>(new DynamicNotification_StarvingFollower());
    this.OnStarvingFollowersNotification();
    UIDynamicNotificationCenter.StarvingFollowerData.UpdateFollowers();
    if (UIDynamicNotificationCenter.HomelessFollowerData == null)
      UIDynamicNotificationCenter.HomelessFollowerData = this.AddDynamicData<DynamicNotification_HomelessFollower>(new DynamicNotification_HomelessFollower());
    if (UIDynamicNotificationCenter.SickFollowerData == null)
      UIDynamicNotificationCenter.SickFollowerData = this.AddDynamicData<DynamicNotification_SickFolllower>(new DynamicNotification_SickFolllower());
    this.OnSickFollowersNotification();
    UIDynamicNotificationCenter.SickFollowerData.UpdateFollowers();
    if (UIDynamicNotificationCenter.DissentingFollowerData == null)
      UIDynamicNotificationCenter.DissentingFollowerData = this.AddDynamicData<DynamicNotification_DissentingFolllower>(new DynamicNotification_DissentingFolllower());
    this.OnDissentingFollowersNotification();
    UIDynamicNotificationCenter.DissentingFollowerData.UpdateFollowers();
    if (UIDynamicNotificationCenter.ExhaustedFollowerData == null)
      UIDynamicNotificationCenter.ExhaustedFollowerData = this.AddDynamicData<DynamicNotification_ExhaustedFolllower>(new DynamicNotification_ExhaustedFolllower());
    this.OnExhaustedFollowersNotification();
    UIDynamicNotificationCenter.ExhaustedFollowerData.UpdateFollowers();
    if (UIDynamicNotificationCenter.DrunkFollowerData == null)
      UIDynamicNotificationCenter.DrunkFollowerData = this.AddDynamicData<DynamicNotification_DrunkFolllower>(new DynamicNotification_DrunkFolllower());
    this.OnDrunkFollowersNotification();
    UIDynamicNotificationCenter.DrunkFollowerData.UpdateFollowers();
    if (UIDynamicNotificationCenter.InjuredFollowerData == null)
      UIDynamicNotificationCenter.InjuredFollowerData = this.AddDynamicData<DynamicNotification_InjuredFolllower>(new DynamicNotification_InjuredFolllower());
    this.OnInjuredFollowersNotification();
    UIDynamicNotificationCenter.InjuredFollowerData.UpdateFollowers();
    if (UIDynamicNotificationCenter.FreezingFollowerData == null)
      UIDynamicNotificationCenter.FreezingFollowerData = this.AddDynamicData<DynamicNotification_FreezingFolllower>(new DynamicNotification_FreezingFolllower());
    this.OnFreezingFollowersNotification();
    UIDynamicNotificationCenter.FreezingFollowerData.UpdateFollowers();
    if (!this._initialized)
    {
      foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
        this.CheckNewBrain(allBrain);
      this._initialized = true;
    }
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    UIDynamicNotificationCenter.HomelessFollowerData.OnStructuresPlaced();
    this.OnHomelessFollowersNotification();
  }

  public void OnDisable()
  {
    if (!((UnityEngine.Object) UIDynamicNotificationCenter.Instance == (UnityEngine.Object) this))
      return;
    UIDynamicNotificationCenter.Instance = (UIDynamicNotificationCenter) null;
  }

  public void Show(bool instant = false)
  {
    this.StopAllCoroutines();
    if (instant)
      this._rectTransform.anchoredPosition = this._onScreenPosition;
    else
      this.StartCoroutine((IEnumerator) this.DoShow());
  }

  public IEnumerator DoShow()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._onScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void Hide(bool instant = false)
  {
    this.StopAllCoroutines();
    if (instant)
      this._rectTransform.anchoredPosition = this._offScreenPosition;
    else
      this.StartCoroutine((IEnumerator) this.DoHide());
  }

  public IEnumerator DoHide()
  {
    this._rectTransform.DOKill();
    this._rectTransform.DOAnchorPos(this._offScreenPosition, 0.5f).SetEase<TweenerCore<Vector2, Vector2, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector2, Vector2, VectorOptions>>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
  }

  public void InitializeWeatherData()
  {
  }

  public void InitializeData()
  {
    if (UIDynamicNotificationCenter.HolidayRitualData == null)
      UIDynamicNotificationCenter.HolidayRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Holiday));
    this.OnHolidayRitualBegan();
    if (UIDynamicNotificationCenter.WorkThroughNightRitualData == null)
      UIDynamicNotificationCenter.WorkThroughNightRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_WorkThroughNight));
    this.OnWorkThroughNightRitualBegan();
    if (UIDynamicNotificationCenter.FastRitualData == null)
      UIDynamicNotificationCenter.FastRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Fast));
    this.OnFastRitualBegan();
    if (UIDynamicNotificationCenter.FishingRitualData == null)
      UIDynamicNotificationCenter.FishingRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_FishingRitual));
    this.OnFishingRitualBegan();
    if (UIDynamicNotificationCenter.BrainwashingRitualData == null)
      UIDynamicNotificationCenter.BrainwashingRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Brainwashing));
    this.OnBrainwashingRitualBegan();
    if (UIDynamicNotificationCenter.EnlightenmentRitualData == null)
      UIDynamicNotificationCenter.EnlightenmentRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Enlightenment));
    this.OnEnlightenmentRitualBegan();
    if (UIDynamicNotificationCenter.HalloweenRitualData == null)
      UIDynamicNotificationCenter.HalloweenRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Halloween));
    this.OnHalloweenRitualBegan();
    if (UIDynamicNotificationCenter.PurgeRitualData == null)
      UIDynamicNotificationCenter.PurgeRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Purge));
    this.OnPurgeRitualBegan();
    if (UIDynamicNotificationCenter.NudistRitualData == null)
      UIDynamicNotificationCenter.NudistRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_Nudism));
    this.OnNudismRitualBegan();
    if (UIDynamicNotificationCenter.RanchMeatRitualData == null)
      UIDynamicNotificationCenter.RanchMeatRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_RanchMeat));
    this.OnRanchMeatRitualBegan();
    if (UIDynamicNotificationCenter.RanchHarvestRitualData == null)
      UIDynamicNotificationCenter.RanchHarvestRitualData = this.AddDynamicData<DynamicNotification_RitualActive>(new DynamicNotification_RitualActive(UpgradeSystem.Type.Ritual_RanchHarvest));
    this.OnRanchHarvestRitualBegan();
  }

  public T AddDynamicData<T>(T data) where T : DynamicNotificationData
  {
    if (!UIDynamicNotificationCenter.DynamicNotifications.Contains((DynamicNotificationData) data))
      UIDynamicNotificationCenter.DynamicNotifications.Add((DynamicNotificationData) data);
    return data;
  }

  public void OnHolidayRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HolidayRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnWorkThroughNightRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.WorkThroughNightRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnFastRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.FastRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnFishingRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.FishingRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnBrainwashingRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.BrainwashingRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnEnlightenmentRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.EnlightenmentRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnHalloweenRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HalloweenRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnPurgeRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.PurgeRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnNudismRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.NudistRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnRanchHarvestRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.RanchHarvestRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnRanchMeatRitualBegan()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.RanchMeatRitualData, (NotificationDynamicBase) this._ritualDynamicNotificationTemplate);
  }

  public void OnHomelessFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnDissentingFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.DissentingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnInjuredFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.InjuredFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnFreezingFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.FreezingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnStarvingFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.StarvingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnSickFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.SickFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnExhaustedFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.ExhaustedFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnDrunkFollowersNotification()
  {
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.DrunkFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }

  public void OnFollowersSpawned()
  {
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnNewPhaseStarted()
  {
    if (DataManager.Instance.OnboardedHomelessAtNight && TimeManager.IsNight)
    {
      DataManager.Instance.OnboardedHomeless = true;
      DataManager.Instance.OnboardedHomelessAtNight = false;
    }
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnFollowerRemoved(int ID)
  {
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnFollowerAdded(int ID)
  {
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnStructureAdded(StructuresData structure)
  {
    UIDynamicNotificationCenter.HomelessFollowerData.OnStructureAdded(structure);
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnStructuresPlaced()
  {
    UIDynamicNotificationCenter.HomelessFollowerData.OnStructuresPlaced();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnBedModified()
  {
    UIDynamicNotificationCenter.HomelessFollowerData.OnStructuresPlaced();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount <= 0.0)
      return;
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
  }

  public void OnStarvationStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.StarvingFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.StarvingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.StarvingFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnExhaustedStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.ExhaustedFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.ExhaustedFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.ExhaustedFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnIllnessStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.SickFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.SickFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.SickFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnReeducationStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.DissentingFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.DissentingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.DissentingFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnInjuredStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.InjuredFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.InjuredFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.InjuredFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnFreezingStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.FreezingFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.FreezingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.FreezingFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnDrunkStateChanged(
    int brainID,
    FollowerStatState newState,
    FollowerStatState oldState)
  {
    FollowerBrain brainById = FollowerBrain.FindBrainByID(brainID);
    if (oldState == FollowerStatState.Off && newState == FollowerStatState.On)
    {
      UIDynamicNotificationCenter.DrunkFollowerData.AddFollower(brainById);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.DrunkFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    else
    {
      if (oldState != FollowerStatState.On || newState != FollowerStatState.Off)
        return;
      UIDynamicNotificationCenter.DrunkFollowerData.RemoveFollower(brainById.Info.ID);
    }
  }

  public void OnFollowerDie(
    int brainID,
    NotificationCentre.NotificationType deathNotificationType)
  {
    AchievementsWrapper.UnlockAchievement(Achievements.Instance.Lookup("FIRST_DEATH"));
    FollowerBrain.FindBrainByID(brainID);
    UIDynamicNotificationCenter.SickFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    UIDynamicNotificationCenter.StarvingFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.ExhaustedFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.DissentingFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.InjuredFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.DrunkFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.FreezingFollowerData.RemoveFollower(brainID);
    Debug.Log((object) "Notifications - OnFollowerDie");
  }

  public void OnBrainAdded(int brainID) => this.CheckNewBrain(FollowerBrain.FindBrainByID(brainID));

  public void OnBrainRemoved(int brainID)
  {
    UIDynamicNotificationCenter.StarvingFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount > 0.0)
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    UIDynamicNotificationCenter.DissentingFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.SickFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.InjuredFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.DrunkFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.FreezingFollowerData.RemoveFollower(brainID);
    UIDynamicNotificationCenter.ExhaustedFollowerData.RemoveFollower(brainID);
  }

  public void PlayDynamic(DynamicNotificationData data, NotificationDynamicBase prefab)
  {
    if (!NotificationCentre.NotificationsEnabled || data.IsEmpty)
      return;
    foreach (NotificationDynamicBase notificationDynamicBase in UIDynamicNotificationCenter.NotificationsDynamic)
    {
      if (!notificationDynamicBase.Closing && notificationDynamicBase.Data.Type == data.Type)
        return;
    }
    NotificationDynamicBase notificationDynamicBase1 = prefab.Instantiate<NotificationDynamicBase>((Transform) this._dynamicNotificationContainer);
    notificationDynamicBase1.Configure(data);
    UIDynamicNotificationCenter.NotificationsDynamic.Add(notificationDynamicBase1);
  }

  public void CheckNewBrain(FollowerBrain brain)
  {
    if (DataManager.Instance.Followers_Dead_IDs.Contains(brain.Info.ID))
      return;
    if (brain.Info.CursedState == Thought.BecomeStarving)
    {
      UIDynamicNotificationCenter.StarvingFollowerData.AddFollower(brain);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.StarvingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    if (brain.Info.CursedState == Thought.Ill)
    {
      UIDynamicNotificationCenter.SickFollowerData.AddFollower(brain);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.SickFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
    }
    if ((double) brain.Stats.Exhaustion > 0.0)
    {
      UIDynamicNotificationCenter.ExhaustedFollowerData.AddFollower(brain);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.ExhaustedFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    }
    if ((double) brain.Stats.Drunk > 0.0)
    {
      UIDynamicNotificationCenter.DrunkFollowerData.AddFollower(brain);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.DrunkFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    }
    if ((double) brain.Stats.Injured > 0.0)
    {
      UIDynamicNotificationCenter.InjuredFollowerData.AddFollower(brain);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.InjuredFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    }
    if ((double) brain.Stats.Freezing > 0.0)
    {
      UIDynamicNotificationCenter.FreezingFollowerData.AddFollower(brain);
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.FreezingFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    }
    UIDynamicNotificationCenter.HomelessFollowerData.CheckFollowerCount();
    if ((double) UIDynamicNotificationCenter.HomelessFollowerData.TotalCount > 0.0)
      this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.HomelessFollowerData, (NotificationDynamicBase) this._genericDynamicNotificationTemplate);
    if (brain.Info.CursedState != Thought.Dissenter)
      return;
    UIDynamicNotificationCenter.DissentingFollowerData.AddFollower(brain);
    this.PlayDynamic((DynamicNotificationData) UIDynamicNotificationCenter.DissentingFollowerData, (NotificationDynamicBase) this._cursedDynamicNotificationTemplate);
  }
}
