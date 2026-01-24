// Decompiled with JetBrains decompiler
// Type: SampleGradient
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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

  public void Start()
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
