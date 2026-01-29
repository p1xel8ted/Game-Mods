// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UpgradeTreeTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.UpgradeMenu;

public class UpgradeTreeTabNavigatorBase : MMTabNavigatorBase<UpgradeTreeTab>
{
  public static int _persistentTabIndex = -1;

  public override void ShowDefault() => this.SetDefaultTab(this._tabs[this._defaultTabIndex]);

  public void RemoveAllAlerts()
  {
    foreach (UpgradeTreeTab tab in this._tabs)
      tab.Alert.gameObject.SetActive(false);
  }

  public override void OnMenuHide()
  {
    base.OnMenuHide();
    UpgradeTreeTabNavigatorBase._persistentTabIndex = this.CurrentMenuIndex;
  }

  public void ClearPersistentTab() => UpgradeTreeTabNavigatorBase._persistentTabIndex = -1;
}
