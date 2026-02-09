// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DebugLogWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Functions")]
[Color("000000")]
[Name("Debug Log WGO", 0)]
public class Flow_DebugLogWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddValueInput<string>("Description");
    this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f => flow_out.Call(f)));
  }
}
