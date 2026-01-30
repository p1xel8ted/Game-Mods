// Decompiled with JetBrains decompiler
// Type: DamageNegationTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class DamageNegationTimer : MonoBehaviour
{
  [SerializeField]
  public Image wheel;

  public void UpdateCharging(float normalised) => this.wheel.fillAmount = normalised;
}
