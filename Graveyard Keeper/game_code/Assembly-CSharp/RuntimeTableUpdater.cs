// Decompiled with JetBrains decompiler
// Type: RuntimeTableUpdater
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class RuntimeTableUpdater : MonoBehaviour
{
  public void OnDisable() => this.UpdateTable();

  public void UpdateTable()
  {
    foreach (UITable uiTable in this.GetComponentsInParent<UITable>())
      uiTable.Reposition();
    foreach (SimpleUITable simpleUiTable in this.GetComponentsInParent<SimpleUITable>())
      simpleUiTable.Reposition();
    foreach (UIGrid uiGrid in this.GetComponentsInParent<UIGrid>())
      uiGrid.Reposition();
  }
}
