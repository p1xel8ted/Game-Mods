// Decompiled with JetBrains decompiler
// Type: UpdateSizeTMPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
