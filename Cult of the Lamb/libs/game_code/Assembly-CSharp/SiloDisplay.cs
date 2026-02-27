// Decompiled with JetBrains decompiler
// Type: SiloDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class SiloDisplay : BaseMonoBehaviour
{
  public TextMeshPro Text;
  public Structure structure;

  public void Start() => this.structure = this.GetComponent<Structure>();

  public void Update()
  {
    if (!((Object) this.Text != (Object) null))
      return;
    this.Text.text = this.structure.Inventory.Count.ToString();
  }
}
