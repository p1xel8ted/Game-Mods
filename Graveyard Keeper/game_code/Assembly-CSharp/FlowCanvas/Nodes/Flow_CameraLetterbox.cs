// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CameraLetterbox
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Camera Letterbox", 0)]
[Category("Game Actions")]
public class Flow_CameraLetterbox : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> par_show = this.AddValueInput<bool>("Show");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      CameraTools.TweenLetterbox(par_show.value);
      GJTimer.AddTimer(1f, (GJTimer.VoidDelegate) (() => flow_finished.Call(f)));
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => base.name + (this.GetInputValuePort<bool>("Show").value ? " (on)" : " (off)");
    set => base.name = value;
  }
}
