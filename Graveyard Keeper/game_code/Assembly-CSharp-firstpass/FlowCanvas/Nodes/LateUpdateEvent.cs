// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.LateUpdateEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Late Update", 5)]
[Category("Events/Graph")]
[Description("Called per-frame, but after normal Update")]
public class LateUpdateEvent : EventNode
{
  public FlowOutput lateUpdate;

  public override void RegisterPorts() => this.lateUpdate = this.AddFlowOutput("Out");

  public override void OnGraphStarted()
  {
    MonoManager.current.onLateUpdate += new Action(this.LateUpdate);
  }

  public override void OnGraphStoped()
  {
    MonoManager.current.onLateUpdate -= new Action(this.LateUpdate);
  }

  public void LateUpdate() => this.lateUpdate.Call(new Flow());
}
