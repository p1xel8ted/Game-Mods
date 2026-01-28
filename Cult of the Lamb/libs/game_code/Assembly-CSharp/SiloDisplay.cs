// Decompiled with JetBrains decompiler
// Type: SiloDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
