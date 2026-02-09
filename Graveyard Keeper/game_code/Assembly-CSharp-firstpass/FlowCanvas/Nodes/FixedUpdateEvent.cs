// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.FixedUpdateEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Graph")]
[Name("On Fixed Update", 4)]
[Description("Called every fixed framerate frame, which should be used when dealing with Physics")]
public class FixedUpdateEvent : EventNode
{
  public FlowOutput fixedUpdate;

  public override void RegisterPorts() => this.fixedUpdate = this.AddFlowOutput("Out");

  public override void OnGraphStarted()
  {
    MonoManager.current.onFixedUpdate += new Action(this.FixedUpdate);
  }

  public override void OnGraphStoped()
  {
    MonoManager.current.onFixedUpdate -= new Action(this.FixedUpdate);
  }

  public void FixedUpdate() => this.fixedUpdate.Call(new Flow());
}
