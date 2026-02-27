// Decompiled with JetBrains decompiler
// Type: SetTextMaterialPreset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (TextMeshProUGUI))]
public class SetTextMaterialPreset : MonoBehaviour
{
  [SerializeField]
  private Material materialPreset;
  private TextMeshProUGUI text;

  private void OnEnable()
  {
    this.text = this.GetComponent<TextMeshProUGUI>();
    if (!((Object) this.materialPreset != (Object) null))
      return;
    this.text.fontSharedMaterial = this.materialPreset;
  }

  private void SetMaterial() => this.OnEnable();
}
