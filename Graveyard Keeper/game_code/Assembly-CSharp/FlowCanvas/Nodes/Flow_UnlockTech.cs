// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_UnlockTech
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Unlock Tech", 0)]
public class Flow_UnlockTech : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<string> tech_id = this.AddValueInput<string>("tech id");
    ValueInput<bool> silent = this.AddValueInput<bool>("silent?");
    ValueInput<bool> show_tech = this.AddValueInput<bool>("show tech");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (silent.value)
      {
        MainGame.me.save.UnlockTech(tech_id.value);
        flow_out.Call(f);
      }
      else
        GUIElements.me.tech_dialog.Open(GameBalance.me.GetData<TechDefinition>(tech_id.value), (GJCommons.VoidDelegate) (() => flow_out.Call(f)), true, show_tech_tree_after: show_tech.value);
    }));
  }
}
