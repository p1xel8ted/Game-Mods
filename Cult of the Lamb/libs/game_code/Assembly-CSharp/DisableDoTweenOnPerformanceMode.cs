// Decompiled with JetBrains decompiler
// Type: DisableDoTweenOnPerformanceMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
