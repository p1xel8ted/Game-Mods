// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeMenu.UpgradeTreeTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
