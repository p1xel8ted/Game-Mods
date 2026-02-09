// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_EnableGlobalCraftControl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Enable Global Craft Control", 0)]
[Category("Game Actions")]
public class Flow_EnableGlobalCraftControl : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> is_enabled = this.AddValueInput<bool>("Enabled");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.save.has_global_craft_control = is_enabled.value;
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return !this.GetInputValuePort<bool>("Enabled").value ? "Disable Global Craft Control" : base.name;
    }
    set => base.name = value;
  }
}
