// Decompiled with JetBrains decompiler
// Type: Lamb.UI.AltarMenu.UIAltarMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using Lamb.UI.Alerts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.AltarMenu;

public class UIAltarMenuController : UIMenuBase
{
  private static int _defaultIndex;
  public System.Action OnSermonSelected;
  public System.Action OnRitualsSelected;
  public System.Action OnDoctrineSelected;
  public System.Action OnPlayerUpgradesSelected;
  [Header("Buttons")]
  [SerializeField]
  private MMButton _sermonButton;
  [SerializeField]
  private MMButton _ritualsButton;
  [SerializeField]
  private MMButton _playerUpgradesButton;
  [SerializeField]
  private MMButton _doctrineButton;
  [Header("Alerts")]
  [SerializeField]
  private GameObject _sermonAlert;
  [SerializeField]
  private GameObject _ugpradeAlert;
  [SerializeField]
  private RitualAlert _ritualAlert;
  [Header("Misc")]
  [SerializeField]
  private TextMeshProUGUI _description;
  private bool _didCancel;

  public void OnEnable()
  {
    this._playerUpgradesButton.gameObject.SetActive(DataManager.Instance.FirstDoctrineStone);
    this._doctrineButton.gameObject.SetActive(DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Afterlife) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Food) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.Possession) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.LawAndOrder) > 0 || DoctrineUpgradeSystem.GetLevelBySermon(SermonCategory.WorkAndWorship) > 0);
    this._ritualsButton.gameObject.SetActive(UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_FirePit) || UpgradeSystem.GetUnlocked(UpgradeSystem.Type.Ritual_Brainwashing));
    this._ugpradeAlert.SetActive(false);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.MONSTER_HEART) >= 1)
      this._ugpradeAlert.SetActive(true);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.TALISMAN) >= 1)
      this._ugpradeAlert.SetActive(true);
    if (Inventory.GetItemQuantity(InventoryItem.ITEM_TYPE.DOCTRINE_STONE) >= 1 && (double) UpgradeSystem.GetCoolDownNormalised(UpgradeSystem.PrimaryRitual1) <= 0.0 && DoctrineUpgradeSystem.TrySermonsStillAvailable())
      this._ugpradeAlert.SetActive(true);
    this._sermonAlert.SetActive(true);
    if (DataManager.Instance.PreviousSermonDayIndex >= TimeManager.CurrentDay)
      this._sermonAlert.SetActive(false);
    if (DataManager.Instance.Followers.Count <= 0)
      this._sermonAlert.SetActive(false);
    if (Ritual.FollowersAvailableToAttendSermon() > 0)
      return;
    this._sermonAlert.SetActive(false);
  }

  public void Start()
  {
    this._sermonButton.onClick.AddListener(new UnityAction(this.OnSermonButtonClicked));
    this._ritualsButton.onClick.AddListener(new UnityAction(this.OnRitualsButtonClicked));
    this._playerUpgradesButton.onClick.AddListener(new UnityAction(this.OnPlayerUpgradesClicked));
    this._doctrineButton.onClick.AddListener(new UnityAction(this.OnDoctrineButtonCLicked));
    this._sermonButton.OnSelected += new System.Action(this.OnSermonButtonSelected);
    this._ritualsButton.OnSelected += new System.Action(this.OnRitualsButtonSelected);
    this._playerUpgradesButton.OnSelected += new System.Action(this.OnPlayerUpgradesButtonSelected);
    this._doctrineButton.OnSelected += new System.Action(this.OnDoctrineButtonSelected);
  }

  private void OnSermonButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Sermon;
  }

  private void OnSermonButtonClicked()
  {
    this.Hide();
    System.Action onSermonSelected = this.OnSermonSelected;
    if (onSermonSelected != null)
      onSermonSelected();
    UIAltarMenuController._defaultIndex = 0;
  }

  private void OnRitualsButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Rituals;
  }

  private void OnRitualsButtonClicked()
  {
    this.Hide();
    System.Action onRitualsSelected = this.OnRitualsSelected;
    if (onRitualsSelected != null)
      onRitualsSelected();
    UIAltarMenuController._defaultIndex = 2;
  }

  private void OnPlayerUpgradesButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Crown;
  }

  private void OnPlayerUpgradesClicked()
  {
    this.Hide();
    System.Action upgradesSelected = this.OnPlayerUpgradesSelected;
    if (upgradesSelected != null)
      upgradesSelected();
    UIAltarMenuController._defaultIndex = 1;
  }

  private void OnDoctrineButtonSelected()
  {
    this._description.text = ScriptLocalization.UI_Altar.Doctrine;
  }

  private void OnDoctrineButtonCLicked()
  {
    this.Hide();
    System.Action doctrineSelected = this.OnDoctrineSelected;
    if (doctrineSelected != null)
      doctrineSelected();
    UIAltarMenuController._defaultIndex = 3;
  }

  protected override void OnShowStarted()
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
    this.ActivateNavigation();
  }

  protected override void OnHideStarted()
  {
    base.OnHideStarted();
    UIManager.PlayAudio("event:/ui/close_menu");
  }

  protected override void OnHideCompleted()
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
