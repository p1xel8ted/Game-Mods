// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.WaitForSecondsUnscaled
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter;

public class WaitForSecondsUnscaled : CustomYieldInstruction
{
  public float waitTime;
  public float runUntil;

  public override bool keepWaiting => (double) Time.unscaledTime < (double) this.runUntil;

  public new void Reset() => this.runUntil = Time.unscaledTime + this.waitTime;

  public WaitForSecondsUnscaled(float time) => this.waitTime = time;
}
