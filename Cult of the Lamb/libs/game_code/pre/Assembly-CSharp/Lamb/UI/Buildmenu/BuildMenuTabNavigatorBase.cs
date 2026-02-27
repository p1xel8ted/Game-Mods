// Decompiled with JetBrains decompiler
// Type: Lamb.UI.BuildMenu.BuildMenuTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
namespace Lamb.UI.BuildMenu;

public class BuildMenuTabNavigatorBase : MMTabNavigatorBase<BuildMenuTab>
{
  public override void ShowDefault()
  {
    if (ObjectiveManager.GroupExists("Objectives/GroupTitles/RepairTheShrine") || ObjectiveManager.GroupExists("Objectives/GroupTitles/Temple"))
      this.SetDefaultTab(this._tabs[1]);
    else
      base.ShowDefault();
  }

  public void RemoveAllAlerts()
  {
    foreach (BuildMenuTab tab in this._tabs)
      tab.Alert.gameObject.SetActive(false);
  }
}
