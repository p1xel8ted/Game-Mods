// Decompiled with JetBrains decompiler
// Type: TraitMenuTabNavigatorBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;

#nullable disable
public class TraitMenuTabNavigatorBase : MMTabNavigatorBase<TraitManipulatorTab>
{
  public static int _persistentTabIndex = -1;

  public override void ShowDefault()
  {
    if (TraitMenuTabNavigatorBase._persistentTabIndex != -1)
      this._defaultTabIndex = TraitMenuTabNavigatorBase._persistentTabIndex;
    base.ShowDefault();
  }

  public void RemoveAllAlerts()
  {
    foreach (TraitManipulatorTab tab in this._tabs)
      tab.Alert.gameObject.SetActive(false);
  }

  public override void OnMenuHide()
  {
    base.OnMenuHide();
    TraitMenuTabNavigatorBase._persistentTabIndex = this.CurrentMenuIndex;
  }

  public void ClearPersistentTab() => TraitMenuTabNavigatorBase._persistentTabIndex = -1;

  public void SetInteractable(bool interactable)
  {
    for (int index = 0; index < this._tabs.Length; ++index)
      this._tabs[index].TrySetInteractable(interactable);
  }
}
