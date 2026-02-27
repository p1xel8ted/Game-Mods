// Decompiled with JetBrains decompiler
// Type: SampleGradient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class SampleGradient : BaseMonoBehaviour
{
  public Gradient dayColors;
  public Gradient nightColors;
  public Image sceneryImage;
  public Image playerImage;
  public Image timeOfDay;
  public string MaterialColorName;

  private void Start()
  {
  }

  public void setDayColorFloat(float sampleAt)
  {
    if ((Object) this.sceneryImage != (Object) null)
      this.sceneryImage.material.SetColor(this.MaterialColorName, this.Sample(sampleAt));
    if ((Object) this.playerImage != (Object) null)
      this.playerImage.material.SetColor(this.MaterialColorName, this.Sample(sampleAt));
    if (!((Object) this.timeOfDay != (Object) null))
      return;
    this.timeOfDay.color = this.Sample(sampleAt);
  }

  public Color Sample(float sampleAt) => this.dayColors.Evaluate(sampleAt);

  public Color NightSample(float sampleAt) => this.nightColors.Evaluate(sampleAt);
}
