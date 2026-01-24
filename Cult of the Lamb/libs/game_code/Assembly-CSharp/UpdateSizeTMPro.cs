// Decompiled with JetBrains decompiler
// Type: UpdateSizeTMPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[ExecuteAlways]
public class UpdateSizeTMPro : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;
  public bool UpdateHeight = true;
  public bool UpdateWidth = true;
  public string _text;

  public string text
  {
    set
    {
      if (this._text != value)
        this.Text.rectTransform.sizeDelta = new Vector2(this.UpdateWidth ? this.Text.preferredWidth : this.Text.rectTransform.sizeDelta.x, this.UpdateHeight ? this.Text.preferredHeight : this.Text.rectTransform.sizeDelta.y);
      this._text = value;
    }
  }

  public void Start() => this.Text = this.GetComponent<TextMeshProUGUI>();

  public void LateUpdate() => this.text = this.Text.text;
}
