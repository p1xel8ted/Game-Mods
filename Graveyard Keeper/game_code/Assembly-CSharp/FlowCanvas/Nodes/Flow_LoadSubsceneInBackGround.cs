// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LoadSubsceneInBackGround
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Load scene additive to the current scene in the background")]
[Icon("CubePlus", false, "")]
[Category("Game Actions")]
[Name("Just Load Subscene", 0)]
public class Flow_LoadSubsceneInBackGround : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> par_custom_tag = this.AddValueInput<string>("Scene name");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_out_on_scene_loaded = this.AddFlowOutput("On loaded");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      SubsceneLoadManager.Load(par_custom_tag.value, (System.Action) (() => flow_out_on_scene_loaded.Call(f)));
      flow_out.Call(f);
    }));
  }
}
