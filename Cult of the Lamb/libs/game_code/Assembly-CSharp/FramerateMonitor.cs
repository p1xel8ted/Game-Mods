// Decompiled with JetBrains decompiler
// Type: FramerateMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FramerateMonitor : UserReportingMonitor
{
  public float duration;
  public float MaximumDurationInSeconds;
  public float MinimumFramerate;

  public FramerateMonitor()
  {
    this.MaximumDurationInSeconds = 10f;
    this.MinimumFramerate = 15f;
  }

  public void Update()
  {
    float deltaTime = Time.deltaTime;
    if (1.0 / (double) deltaTime < (double) this.MinimumFramerate)
      this.duration += deltaTime;
    else
      this.duration = 0.0f;
    if ((double) this.duration <= (double) this.MaximumDurationInSeconds)
      return;
    this.duration = 0.0f;
    this.Trigger();
  }
}
