// Decompiled with JetBrains decompiler
// Type: SiloDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
