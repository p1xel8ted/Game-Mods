// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UIUpgradeShopMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.Extensions;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI.UpgradeMenu;

public class UIUpgradeShopMenuController : UIMenuBase
{
  public Action<UpgradeSystem.Type> OnUpgradeChosen;
  [SerializeField]
  private List<UIUpgradeShopMenuController.AvailableUpgrades> _upgrades = new List<UIUpgradeShopMenuController.AvailableUpgrades>();
  [SerializeField]
  private UpgradeShopItem _upgradeItemTemplate;
  [SerializeField]
  private Transform _contentContainer;
  [SerializeField]
  private TextMeshProUGUI _noAvailableUpgradesText;
  private List<UpgradeShopItem> _items = new List<UpgradeShopItem>();

  private void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnChangeSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnChangeSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  protected override void OnShowStarted()
  {
    float num = -0.03f;
    foreach (UIUpgradeShopMenuController.AvailableUpgrades upgrade in this._upgrades)
    {
      switch (upgrade.CheckUnlocked)
      {
        case UIUpgradeShopMenuController.CheckUnlockedType.HideIfUnlocked:
          if (!UpgradeSystem.GetUnlocked(upgrade.Type))
            break;
          continue;
        case UIUpgradeShopMenuController.CheckUnlockedType.ShowIfUnlocked:
          if (!UpgradeSystem.GetUnlocked(upgrade.Type))
            continue;
          break;
      }
      if (!upgrade.RequireUnlocked || UpgradeSystem.GetUnlocked(upgrade.RequireUnlockedType))
      {
        DataManager.Instance.Alerts.Upgrades.AddOnce(upgrade.Type);
        UpgradeShopItem upgradeShopItem = this._upgradeItemTemplate.Instantiate<UpgradeShopItem>(this._contentContainer);
        upgradeShopItem.Configure(upgrade.Type, num += 0.03f);
        upgradeShopItem.OnUpgradeSelected += new Action<UpgradeSystem.Type>(this.OnSelect);
        this._items.Add(upgradeShopItem);
      }
    }
    if (this._items.Count > 0)
    {
      this.OverrideDefaultOnce((Selectable) this._items[0].Button);
      this.ActivateNavigation();
    }
    this._noAvailableUpgradesText.gameObject.SetActive(this._items.Count == 0);
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.Hide();
  }

  protected override void OnHideStarted()
  {
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_close");
  }

  protected override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  private void OnChangeSelection(Selectable currentSelectable, Selectable previousSelectable)
  {
    this.OnSelection(currentSelectable);
  }

  private void OnSelection(Selectable selectable)
  {
    if (!((UnityEngine.Object) selectable != (UnityEngine.Object) null))
      return;
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_scroll");
  }

  private void OnSelect(UpgradeSystem.Type type)
  {
    if ((double) UpgradeSystem.GetCoolDownNormalised(type) > 0.0)
      return;
    foreach (StructuresData.ItemCost itemCost in UpgradeSystem.GetCost(type))
      Inventory.ChangeItemQuantity((int) itemCost.CostItem, -itemCost.CostValue);
    Action<UpgradeSystem.Type> onUpgradeChosen = this.OnUpgradeChosen;
    if (onUpgradeChosen != null)
      onUpgradeChosen(type);
    this.OnCancelButtonInput();
  }

  public enum CheckUnlockedType
  {
    None,
    HideIfUnlocked,
    ShowIfUnlocked,
  }

  [Serializable]
  public class AvailableUpgrades
  {
    public UpgradeSystem.Type Type;
    public UIUpgradeShopMenuController.CheckUnlockedType CheckUnlocked;
    public bool RequireUnlocked;
    public UpgradeSystem.Type RequireUnlockedType;
  }
}
