// Decompiled with JetBrains decompiler
// Type: UIRitualScreenListObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public float Shaking;
  public float ShakeSpeed;

  public void Init(SermonsAndRituals.SermonRitualType Type)
  {
    this.Text.text = this.SelectedText.text = SermonsAndRituals.LocalisedName(Type);
    this.Icon.sprite = this.SelectedIcon.sprite = SermonsAndRituals.Sprite(Type);
  }

  public void Shake() => this.ShakeSpeed = (float) (25 * (Random.Range(0, 2) < 1 ? 1 : -1));

  public void Update()
  {
    this.ShakeSpeed += (float) ((0.0 - (double) this.Shaking) * 0.40000000596046448);
    this.Shaking += (this.ShakeSpeed *= 0.8f);
    this.ShakeRectTransform.localPosition = Vector3.left * this.Shaking;
  }
}
