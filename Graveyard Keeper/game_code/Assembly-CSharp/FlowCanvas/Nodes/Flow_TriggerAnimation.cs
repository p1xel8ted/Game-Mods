// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TriggerAnimation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Trigger Animation", 0)]
public class Flow_TriggerAnimation : MyFlowNode
{
  public override void RegisterPorts()
  {
    WorldGameObject out_wgo = (WorldGameObject) null;
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> par_anim = this.AddValueInput<string>("Anim");
    FlowOutput flow_out = this.AddFlowOutput("Out", "Out");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => out_wgo));
    FlowOutput flow_finished = this.AddFlowOutput("Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      out_wgo = this.WGOParamOrSelf(par_wgo);
      out_wgo.TriggerSmartAnimation(par_anim.value, (System.Action) (() => flow_finished.Call(f)));
      flow_out.Call(f);
    }));
  }
}
