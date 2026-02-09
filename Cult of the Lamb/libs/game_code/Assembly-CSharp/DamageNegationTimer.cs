// Decompiled with JetBrains decompiler
// Type: DamageNegationTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
