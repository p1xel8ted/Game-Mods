// Decompiled with JetBrains decompiler
// Type: CodeStage.AdvancedFPSCounter.Utils.AFPSRenderRecorder
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace CodeStage.AdvancedFPSCounter.Utils;

[DisallowMultipleComponent]
public class AFPSRenderRecorder : MonoBehaviour
{
  public bool recording;
  public float renderTime;

  public void OnPreCull()
  {
    if (this.recording || (Object) AFPSCounter.Instance == (Object) null)
      return;
    this.recording = true;
    this.renderTime = Time.realtimeSinceStartup;
  }

  public void OnPostRender()
  {
    if (!this.recording || (Object) AFPSCounter.Instance == (Object) null)
      return;
    this.recording = false;
    this.renderTime = Time.realtimeSinceStartup - this.renderTime;
    AFPSCounter.Instance.fpsCounter.AddRenderTime(this.renderTime * 1000f);
  }
}
