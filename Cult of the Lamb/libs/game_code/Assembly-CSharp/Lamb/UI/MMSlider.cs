// Decompiled with JetBrains decompiler
// Type: Lamb.UI.MMSlider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class MMSlider : Slider
{
  [SerializeField]
  public int _increment = 1;
  [SerializeField]
  public MMSlider.ValueDisplayFormat _valueDisplayFormat;
  [SerializeField]
  public TextMeshProUGUI _valueText;
  public Func<float, string> GetCustomDisplayFormat;

  public override float value
  {
    get => this.wholeNumbers ? Mathf.Round(this.m_Value) : this.m_Value;
    set => this.Set(Mathf.Round(value / (float) this._increment) * (float) this._increment);
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this._valueText == (UnityEngine.Object) null)
      return;
    if (this._valueDisplayFormat == MMSlider.ValueDisplayFormat.Percentage)
      this._valueText.text = $"{this.value}%";
    else if (this._valueDisplayFormat == MMSlider.ValueDisplayFormat.RawValue)
      this._valueText.text = this.value.ToString();
    else
      this._valueText.text = this.GetCustomDisplayFormat(this.value);
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
    Custom,
  }
}
