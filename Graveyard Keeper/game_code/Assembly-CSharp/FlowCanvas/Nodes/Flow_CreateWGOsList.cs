// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CreateWGOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Create WGOs list", 0)]
[Category("Game Actions")]
public class Flow_CreateWGOsList : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    List<WorldGameObject> out_list_var = new List<WorldGameObject>();
    this.AddValueOutput<List<WorldGameObject>>("list", (ValueHandler<List<WorldGameObject>>) (() => out_list_var));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      out_list_var = new List<WorldGameObject>();
      flow_out.Call(f);
    }));
  }
}
