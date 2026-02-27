// Decompiled with JetBrains decompiler
// Type: UIRitualScreenListObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UIRitualScreenListObject : BaseMonoBehaviour
{
  public TextMeshProUGUI Text;
  public TextMeshProUGUI SelectedText;
  public Image Icon;
  public Image SelectedIcon;
  public RectTransform ShakeRectTransform;
  private float Shaking;
  private float ShakeSpeed;

  public void Init(SermonsAndRituals.SermonRitualType Type)
  {
    this.Text.text = this.SelectedText.text = SermonsAndRituals.LocalisedName(Type);
    this.Icon.sprite = this.SelectedIcon.sprite = SermonsAndRituals.Sprite(Type);
  }

  public void Shake() => this.ShakeSpeed = (float) (25 * (Random.Range(0, 2) < 1 ? 1 : -1));

  private void Update()
  {
    this.ShakeSpeed += (float) ((0.0 - (double) this.Shaking) * 0.40000000596046448);
    this.Shaking += (this.ShakeSpeed *= 0.8f);
    this.ShakeRectTransform.localPosition = Vector3.left * this.Shaking;
  }
}
