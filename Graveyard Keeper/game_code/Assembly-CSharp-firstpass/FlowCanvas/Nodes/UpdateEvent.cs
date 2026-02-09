// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UpdateEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Update", 6)]
[Description("Called per-frame. Update Interval optionally determines the period in seconds every which update is called.\nLeave at 0 to call update per-frame as normal.")]
[Category("Events/Graph")]
public class UpdateEvent : EventNode, IUpdatable
{
  public BBParameter<float> updateInterval = (BBParameter<float>) 0.0f;
  public FlowOutput update;
  public float lastUpdatedTime;

  public override void RegisterPorts() => this.update = this.AddFlowOutput("Out");

  public override void OnGraphStarted() => this.lastUpdatedTime = -1f;

  public void Update()
  {
    if ((double) this.updateInterval.value <= 0.0)
    {
      this.update.Call(new Flow());
    }
    else
    {
      float realtimeSinceStartup = Time.realtimeSinceStartup;
      if ((double) realtimeSinceStartup <= (double) this.updateInterval.value + (double) this.lastUpdatedTime)
        return;
      this.update.Call(new Flow());
      this.lastUpdatedTime = realtimeSinceStartup;
    }
  }
}
