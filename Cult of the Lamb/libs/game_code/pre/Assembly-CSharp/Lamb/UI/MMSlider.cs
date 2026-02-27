// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMSlider : Slider
{
  [SerializeField]
  private int _increment = 1;
  [SerializeField]
  private MMSlider.ValueDisplayFormat _valueDisplayFormat;
  [SerializeField]
  private TextMeshProUGUI _valueText;

  public override float value
  {
    get => this.wholeNumbers ? Mathf.Round(this.m_Value) : this.m_Value;
    set => this.Set(Mathf.Round(value / (float) this._increment) * (float) this._increment);
  }

  protected override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this._valueText == (UnityEngine.Object) null)
      return;
    if (this._valueDisplayFormat == MMSlider.ValueDisplayFormat.Percentage)
      this._valueText.text = $"{this.value}%";
    else
      this._valueText.text = this.value.ToString();
  }

  public void IncrementValue()
  {
    this.value = (float) Math.Round((double) this.value + (double) this._increment, 2);
  }

  public void DecrementValue()
  {
    this.value = (float) Math.Round((double) this.value - (double) this._increment, 2);
  }

  public enum ValueDisplayFormat
  {
    Percentage,
    RawValue,
  }
}
