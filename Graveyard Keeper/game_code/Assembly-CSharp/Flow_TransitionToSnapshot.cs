// Decompiled with JetBrains decompiler
// Type: Flow_TransitionToSnapshot
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;

#nullable disable
[Category("Game Actions")]
[Name("Transition To Snapshot", 0)]
public class Flow_TransitionToSnapshot : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<string> snapshot_name;
  public ValueInput<float> transition_time;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.TransitionToSnapshot));
    this.@out = this.AddFlowOutput("Out");
    this.snapshot_name = this.AddValueInput<string>("Snapshot name");
    this.transition_time = this.AddValueInput<float>("Transition time");
  }

  public void TransitionToSnapshot(Flow flow)
  {
    SmartAudioEngine.me.TransitionToSnapshot(this.snapshot_name.value, this.transition_time.value);
    this.@out.Call(flow);
  }
}
