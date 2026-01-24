// Decompiled with JetBrains decompiler
// Type: DisableDoTweenOnPerformanceMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public class DisableDoTweenOnPerformanceMode : MonoBehaviour
{
  public void Awake()
  {
    if (!SettingsManager.Settings.Game.PerformanceMode)
      return;
    foreach (Object componentsInChild in this.GetComponentsInChildren<DOTweenAnimation>())
      Object.Destroy(componentsInChild);
  }
}
