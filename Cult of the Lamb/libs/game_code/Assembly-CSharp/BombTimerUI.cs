// Decompiled with JetBrains decompiler
// Type: BombTimerUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class BombTimerUI : MonoBehaviour
{
  [Header("UI Elements")]
  public Image BG;
  public Image Wheel;
  public float timerValue = 1f;

  public void SetTimerValue(float value)
  {
    this.timerValue = Mathf.Clamp01(value);
    if ((Object) this.BG != (Object) null)
      this.BG.fillAmount = this.timerValue;
    if (!((Object) this.Wheel != (Object) null))
      return;
    this.Wheel.fillAmount = this.timerValue;
  }

  public float GetTimerValue() => this.timerValue;
}
