// Decompiled with JetBrains decompiler
// Type: FramerateMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class FramerateMonitor : UserReportingMonitor
{
  private float duration;
  public float MaximumDurationInSeconds;
  public float MinimumFramerate;

  public FramerateMonitor()
  {
    this.MaximumDurationInSeconds = 10f;
    this.MinimumFramerate = 15f;
  }

  private void Update()
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
