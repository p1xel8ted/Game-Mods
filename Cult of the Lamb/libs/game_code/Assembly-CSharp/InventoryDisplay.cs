// Decompiled with JetBrains decompiler
// Type: InventoryDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class InventoryDisplay : BaseMonoBehaviour
{
  public TextMeshPro Text;

  public void Update()
  {
    if (!((Object) this.Text != (Object) null))
      return;
    this.Text.text = Inventory.TotalItems().ToString();
  }
}
