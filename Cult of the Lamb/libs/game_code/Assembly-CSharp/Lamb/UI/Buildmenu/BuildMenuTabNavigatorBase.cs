// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.BuildMenu;

public class BuildMenuTabNavigatorBase : MMTabNavigatorBase<BuildMenuTab>
{
  public static int _persistentTabIndex = -1;

  public override void ShowDefault()
  {
    if (ObjectiveManager.GroupExists("Objectives/GroupTitles/RepairTheShrine") || ObjectiveManager.GroupExists("Objectives/GroupTitles/Temple"))
    {
      this.SetDefaultTab(this._tabs[1]);
    }
    else
    {
      if (BuildMenuTabNavigatorBase._persistentTabIndex != -1)
        this._defaultTabIndex = BuildMenuTabNavigatorBase._persistentTabIndex;
      base.ShowDefault();
    }
  }

  public void RemoveAllAlerts()
  {
    foreach (BuildMenuTab tab in this._tabs)
      tab.Alert.gameObject.SetActive(false);
  }

  public override void OnMenuHide()
  {
    base.OnMenuHide();
    BuildMenuTabNavigatorBase._persistentTabIndex = this.CurrentMenuIndex;
  }

  public void ClearPersistentTab() => BuildMenuTabNavigatorBase._persistentTabIndex = -1;
}
