// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UIUpgradeShopMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public List<UIUpgradeShopMenuController.AvailableUpgrades> _upgrades = new List<UIUpgradeShopMenuController.AvailableUpgrades>();
  [SerializeField]
  public UpgradeShopItem _upgradeItemTemplate;
  [SerializeField]
  public Transform _contentContainer;
  [SerializeField]
  public TextMeshProUGUI _noAvailableUpgradesText;
  public List<UpgradeShopItem> _items = new List<UpgradeShopItem>();

  public void OnEnable()
  {
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnChangeSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  public new void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnChangeSelection);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  public override void OnShowStarted()
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

  public override void OnHideStarted()
  {
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_close");
  }

  public override void OnHideCompleted() => UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);

  public void OnChangeSelection(Selectable currentSelectable, Selectable previousSelectable)
  {
    this.OnSelection(currentSelectable);
  }

  public void OnSelection(Selectable selectable)
  {
    if (!((UnityEngine.Object) selectable != (UnityEngine.Object) null))
      return;
    UIManager.PlayAudio("event:/upgrade_statue/upgrade_statue_scroll");
  }

  public void OnSelect(UpgradeSystem.Type type)
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
