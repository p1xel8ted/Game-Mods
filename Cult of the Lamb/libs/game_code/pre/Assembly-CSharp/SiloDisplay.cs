// Decompiled with JetBrains decompiler
// Type: SiloDisplay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
public class SiloDisplay : BaseMonoBehaviour
{
  public TextMeshPro Text;
  private Structure structure;

  private void Start() => this.structure = this.GetComponent<Structure>();

  private void Update()
  {
    if (!((Object) this.Text != (Object) null))
      return;
    this.Text.text = this.structure.Inventory.Count.ToString();
  }
}
