// Decompiled with JetBrains decompiler
// Type: DamageNegationTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
