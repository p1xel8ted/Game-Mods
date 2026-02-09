// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.CountersData.UpdatebleCounterData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.CountersData;

public abstract class UpdatebleCounterData : BaseCounterData
{
  public Coroutine updateCoroutine;
  public WaitForSecondsUnscaled cachedWaitForSecondsUnscaled;
  [Range(0.1f, 10f)]
  [Tooltip("Update interval in seconds.")]
  [SerializeField]
  public float updateInterval = 0.5f;

  public float UpdateInterval
  {
    get => this.updateInterval;
    set
    {
      if ((double) Math.Abs(this.updateInterval - value) < 1.0 / 1000.0 || !Application.isPlaying)
        return;
      this.updateInterval = value;
      this.CacheWaitForSeconds();
    }
  }

  public override void PerformInitActions()
  {
    base.PerformInitActions();
    this.StartUpdateCoroutine();
  }

  public override void PerformActivationActions()
  {
    base.PerformActivationActions();
    this.CacheWaitForSeconds();
  }

  public override void PerformDeActivationActions()
  {
    base.PerformDeActivationActions();
    this.StoptUpdateCoroutine();
  }

  public abstract IEnumerator UpdateCounter();

  public void StartUpdateCoroutine()
  {
    this.updateCoroutine = this.main.StartCoroutine(this.UpdateCounter());
  }

  public void StoptUpdateCoroutine() => this.main.StopCoroutine(this.updateCoroutine);

  public void CacheWaitForSeconds()
  {
    this.cachedWaitForSecondsUnscaled = new WaitForSecondsUnscaled(this.updateInterval);
  }
}
