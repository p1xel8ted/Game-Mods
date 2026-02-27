// Decompiled with JetBrains decompiler
// Type: UpdateSizeTMPro
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;

#nullable disable
[ExecuteAlways]
public class UpdateSizeTMPro : BaseMonoBehaviour
{
  private TextMeshProUGUI Text;
  public bool UpdateHeight = true;
  public bool UpdateWidth = true;
  private string _text;

  private string text
  {
    set
    {
      if (this._text != value)
        this.Text.rectTransform.sizeDelta = new Vector2(this.UpdateWidth ? this.Text.preferredWidth : this.Text.rectTransform.sizeDelta.x, this.UpdateHeight ? this.Text.preferredHeight : this.Text.rectTransform.sizeDelta.y);
      this._text = value;
    }
  }

  private void Start() => this.Text = this.GetComponent<TextMeshProUGUI>();

  private void LateUpdate() => this.text = this.Text.text;
}
