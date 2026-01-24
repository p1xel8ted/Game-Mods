// Decompiled with JetBrains decompiler
// Type: ActivateOnPerformanceMode
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class ActivateOnPerformanceMode : MonoBehaviour
{
  public bool ActivateOnPerformance = true;

  public void Awake()
  {
    PerformanceModeManager.OnPerformanceModeChanged += new Action<bool>(this.OnModeChanged);
  }

  public void Start() => this.CheckState(SettingsManager.Settings.Game.PerformanceMode);

  public void OnDestroy()
  {
    PerformanceModeManager.OnPerformanceModeChanged -= new Action<bool>(this.OnModeChanged);
  }

  public void OnModeChanged(bool enabled) => this.CheckState(enabled);

  public void CheckState(bool performanceMode)
  {
    if (performanceMode)
    {
      if (this.ActivateOnPerformance)
        this.gameObject.SetActive(true);
      else
        this.gameObject.SetActive(false);
    }
    else if (this.ActivateOnPerformance)
      this.gameObject.SetActive(false);
    else
      this.gameObject.SetActive(true);
  }
}
