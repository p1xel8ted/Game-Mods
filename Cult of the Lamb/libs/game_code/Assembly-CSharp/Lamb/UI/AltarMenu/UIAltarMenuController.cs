// Decompiled with JetBrains decompiler
// Type: Lamb.UI.AltarMenu.UIAltarMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI.Alerts;
using src.UINavigator;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.AltarMenu;

public class UIAltarMenuController : UIMenuBase
{
  public static int _defaultIndex;
  public System.Action OnSermonSelected;
  public System.Action OnRitualsSelected;
  public System.Action OnDoctrineSelected;
  public System.Action OnPlayerUpgradesSelected;
  public System.Action OnCultUpgradeSelected;
  [Header("Buttons")]
  [SerializeField]
  public MMButton _sermonButton;
  [SerializeField]
  public MMButton _ritualsButton;
  [SerializeField]
  public MMButton _playerUpgradesButton;
  [SerializeField]
  public MMButton _doctrineButton;
  [SerializeField]
  public MMButton _cultUpgradeButton;
  [Header("Alerts")]
  [SerializeField]
  public GameObject _sermonAlert;
  [SerializeField]
  public GameObject _ugpradeAlert;
  [SerializeField]
  public RitualAlert _ritualAlert;
  [SerializeField]
  public GameObject _cultUpgradeAlert;
  [Header("Misc")]
  [SerializeField]
  public TextMeshProUGUI _description;
  [SerializeField]
  public RectTransform _container;
  [SerializeField]
  public TextMeshProUGUI _SermonSinText;
  public bool _didCancel;

  public void OnEnable()
  {
    this._playerUpgradesButton.gameObject.SetActive(DataManager.Instance.FirstDoctrineStone);
    this._doctrineButton.gameObject.SetActive(DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship) > 0 || DataManager.Instance.PleasureDoctrineEnabled && DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Pleasure) > 0 || DataManager.Instance.WinterDoctrineEnabled && DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Winter) > 0);
    this._ritualsButton.gameObject.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit) || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Brainwashing));
    this._ugpradeAlert.SetActive(false);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MONSTER_HEART) >= 1)
    {
      if ((!UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Eat) || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_BlackHeart) || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_Resurrection) || !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_TeleportHome)) && !DataManager.Instance.SurvivalModeActive)
        this._ugpradeAlert.SetActive(true);
      if (DataManager.Instance.BeatenYngya && !UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ability_WinterChoice))
        this._ugpradeAlert.SetActive(true);
    }
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN) >= 1)
      this._ugpradeAlert.SetActive(true);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.DOCTRINE_STONE) >= 1 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.PrimaryRitual1) <= 0.0 && DoctrineUpgradeSystem.TrySermonsStillAvailable())
      this._ugpradeAlert.SetActive(true);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.CRYSTAL_DOCTRINE_STONE) >= 1 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.Type.Ritual_CrystalDoctrine) <= 0.0 && DoctrineUpgradeSystem.GetAllRemainingDoctrines().Count > 0)
      this._ugpradeAlert.SetActive(true);
    if (!DataManager.Instance.PostGameFleecesOnboarded && DataManager.Instance.DeathCatBeaten)
      this._ugpradeAlert.SetActive(true);
    if (!DataManager.Instance.CowboyFleeceOnboarded && DataManager.Instance.WeaponPool.Contains(EquipmentType.Blunderbuss) && DataManager.Instance.PleasureEnabled)
      this._ugpradeAlert.SetActive(true);
    if (!DataManager.Instance.GoatFleeceOnboarded)
      this._ugpradeAlert.SetActive(true);
    this._sermonAlert.SetActive(true);
    if (DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay)
      this._sermonAlert.SetActive(false);
    if (DataManager.Instance.Followers.Count <= 0)
      this._sermonAlert.SetActive(false);
    if (Ritual.FollowersAvailableToAttendSermon() <= 0)
      this._sermonAlert.SetActive(false);
    LocalizationManager.OnLocalizeEvent += new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
    this.UpdateLocalisation();
    if (this.CanSinSermon && this.HasDoneNormalSermon)
    {
      if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.PLEASURE_POINT) >= 3)
        this._sermonAlert.SetActive(true);
      else
        this._SermonSinText.color = this._SermonSinText.color with
        {
          a = 0.7f
        };
    }
    if (DataManager.Instance.PleasureEnabled && DataManager.Instance.SinSermonEnabled && DataManager.Instance.PreviousSinSermonDayIndex >= TimeManager.CurrentDay && this.HasDoneNormalSermon)
      this._SermonSinText.color = this._SermonSinText.color with
      {
        a = 0.7f
      };
    this._cultUpgradeButton.gameObject.SetActive(DataManager.Instance.HasOnboardedCultLevel);
    this._cultUpgradeAlert.SetActive(!CultUpgradeData.IsUpgradeMaxed() && CultUpgradeData.UserCanAffordUpgrade((CultUpgradeData.TYPE) (1 + Mathf.Max(0, DataManager.Instance.TempleLevel))));
  }

  public bool HasDoneNormalSermon
  {
    get => DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay;
  }

  public bool CanSinSermon
  {
    get
    {
      return DataManager.Instance.PleasureEnabled && DataManager.Instance.SinSermonEnabled && DataManager.Instance.PreviousSinSermonDayIndex < TimeManager.CurrentDay;
    }
  }

  public string SinString
  {
    get
    {
      return this.HasDoneNormalSermon && DataManager.Instance.PleasureEnabled && DataManager.Instance.SinSermonEnabled ? "<size=25><sprite name=\"icon_SinDoctrine\">" + 3.ToString() : "";
    }
  }

  public void UpdateLocalisation()
  {
    this._SermonSinText.text = ScriptLocalization.Interactions.Sermon + this.SinString;
  }

  public new void OnDisable()
  {
    LocalizationManager.OnLocalizeEvent -= new LocalizationManager.OnLocalizeCallback(this.UpdateLocalisation);
  }

  public IEnumerator ShowCultLevelUpgrade()
  {
    UIAltarMenuController altarMenuController = this;
    altarMenuController.SetActiveStateForMenu(false);
    EventInstance loopingSound = AudioManager.Instance.CreateLoop("event:/hearts_of_the_faithful/draw_power_loop", true);
    Vector3 pos = altarMenuController._container.transform.localPosition;
    float time = 0.0f;
    while ((double) time < 2.0)
    {
      time += Time.unscaledDeltaTime;
      altarMenuController._container.transform.localPosition = pos + (Vector3) UnityEngine.Random.insideUnitCircle * time * 3f;
      int num = (int) loopingSound.setParameterByName("power", time / 2f);
      yield return (object) null;
    }
    UIManager.PlayAudio("event:/hearts_of_the_faithful/draw_power_end");
    AudioManager.Instance.StopLoop(loopingSound);
    altarMenuController._container.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f).SetUpdate<Tweener>(true);
    altarMenuController._cultUpgradeButton.gameObject.SetActive(true);
    altarMenuController._cultUpgradeAlert.gameObject.SetActive(false);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    altarMenuController._cultUpgradeAlert.gameObject.SetActive(true);
    altarMenuController._cultUpgradeAlert.transform.DOPunchScale(Vector3.one * 0.25f, 0.2f).SetUpdate<Tweener>(true);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    DataManager.Instance.HasOnboardedCultLevel = true;
    altarMenuController.SetActiveStateForMenu(true);
    MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) altarMenuController._cultUpgradeButton);
  }

  public void Start()
  {
    this._sermonButton.onClick.AddListener(new UnityAction(this.OnSermonButtonClicked));
    this._ritualsButton.onClick.AddListener(new UnityAction(this.OnRitualsButtonClicked));
    this._playerUpgradesButton.onClick.AddListener(new UnityAction(this.OnPlayerUpgradesClicked));
    this._doctrineButton.onClick.AddListener(new UnityAction(this.OnDoctrineButtonCLicked));
    this._cultUpgradeButton.onClick.AddListener(new UnityAction(this.OnCultUpgradeClicked));
    this._sermonButton.OnSelected += new System.Action(this.OnSermonButtonSelected);
    this._ritualsButton.OnSelected += new System.Action(this.OnRitualsButtonSelected);
    this._playerUpgradesButton.OnSelected += new System.Action(this.OnPlayerUpgradesButtonSelected);
    this._doctrineButton.OnSelected += new System.Action(this.OnDoctrineButtonSelected);
    this._cultUpgradeButton.OnSelected += new System.Action(this.OnCultUpgradeButtonSelected);
  }

  public void OnSermonButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Sermon;
  }

  public void OnSermonButtonClicked()
  {
    if (this.HasDoneNormalSermon && DataManager.Instance.PreviousSinSermonDayIndex < TimeManager.CurrentDay && DataManager.Instance.PleasureEnabled && DataManager.Instance.SinSermonEnabled)
    {
      if (Inventory.GetItemQuantities(InventoryItem.ITEM_TYPE.PLEASURE_POINT) < 3)
      {
        this._sermonButton._confirmSFX = "event:/ui/negative_feedback";
        return;
      }
    }
    this._sermonButton.Confirmable = true;
    this.Hide();
    System.Action onSermonSelected = this.OnSermonSelected;
    if (onSermonSelected != null)
      onSermonSelected();
    UIAltarMenuController._defaultIndex = 0;
  }

  public void OnRitualsButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Rituals;
  }

  public void OnRitualsButtonClicked()
  {
    this.Hide();
    System.Action onRitualsSelected = this.OnRitualsSelected;
    if (onRitualsSelected != null)
      onRitualsSelected();
    UIAltarMenuController._defaultIndex = 2;
  }

  public void OnPlayerUpgradesButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Crown;
  }

  public void OnPlayerUpgradesClicked()
  {
    this.Hide();
    System.Action upgradesSelected = this.OnPlayerUpgradesSelected;
    if (upgradesSelected != null)
      upgradesSelected();
    UIAltarMenuController._defaultIndex = 1;
  }

  public void OnDoctrineButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Doctrine;
  }

  public void OnDoctrineButtonCLicked()
  {
    this.Hide();
    System.Action doctrineSelected = this.OnDoctrineSelected;
    if (doctrineSelected != null)
      doctrineSelected();
    UIAltarMenuController._defaultIndex = 3;
  }

  public void OnCultUpgradeButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_UpgradeMenu.Description;
  }

  public void OnCultUpgradeClicked()
  {
    System.Action cultUpgradeSelected = this.OnCultUpgradeSelected;
    if (cultUpgradeSelected != null)
      cultUpgradeSelected();
    UIAltarMenuController._defaultIndex = 4;
    this.Hide(true);
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    UIManager.PlayAudio("event:/ui/open_menu");
    switch (UIAltarMenuController._defaultIndex)
    {
      case 0:
        this.OverrideDefault((Selectable) this._sermonButton);
        break;
      case 1:
        this.OverrideDefault((Selectable) this._playerUpgradesButton);
        break;
      case 2:
        this.OverrideDefault((Selectable) this._ritualsButton);
        break;
      case 3:
        this.OverrideDefault((Selectable) this._doctrineButton);
        break;
    }
    if (UICultUpgradeProgress.showCultUpgradeProgressSequence)
      this.OnCultUpgradeClicked();
    else if (DataManager.Instance.pleasurePointsRedeemed >= 1 && !DataManager.Instance.HasOnboardedCultLevel)
      this.SetActiveStateForMenu(false);
    else
      this.ActivateNavigation();
  }

  public override void OnShowCompleted()
  {
    base.OnShowCompleted();
    if (UICultUpgradeProgress.showCultUpgradeProgressSequence)
    {
      this.OnCultUpgradeClicked();
    }
    else
    {
      if (DataManager.Instance.pleasurePointsRedeemed < 1 || DataManager.Instance.HasOnboardedCultLevel)
        return;
      this.StartCoroutine((IEnumerator) this.ShowCultLevelUpgrade());
    }
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  public override void OnHideCompleted()
  {
    if (this._didCancel)
    {
      System.Action onCancel = this.OnCancel;
      if (onCancel != null)
        onCancel();
    }
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    UIAltarMenuController._defaultIndex = 0;
    this._didCancel = true;
    this.Hide();
  }
}
