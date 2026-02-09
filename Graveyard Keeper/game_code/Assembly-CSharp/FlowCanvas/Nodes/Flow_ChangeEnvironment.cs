// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_ChangeEnvironment
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Change Environment", 0)]
[Category("Game Actions")]
public class Flow_ChangeEnvironment : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> teleport_tag = this.AddValueInput<string>("teleport tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      EnvironmentEngine.me.ChangeEnvironment(teleport_tag.value);
      flow_out.Call(f);
    }));
  }
}
