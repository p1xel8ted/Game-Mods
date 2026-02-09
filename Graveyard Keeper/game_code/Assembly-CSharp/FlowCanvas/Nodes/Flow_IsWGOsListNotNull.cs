// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsWGOsListNotNull
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is WGOs List not null", 0)]
[Category("Game Actions")]
[Description("WGOs List is not null")]
public class Flow_IsWGOsListNotNull : MyFlowNode
{
  public List<WorldGameObject> o;

  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> par_wgo = this.AddValueInput<List<WorldGameObject>>("WGO List");
    this.AddValueOutput<List<WorldGameObject>>("WGO List", (ValueHandler<List<WorldGameObject>>) (() => this.o));
    FlowOutput flow_yes = this.AddFlowOutput("WGOs List Is Not NULL");
    FlowOutput flow_no = this.AddFlowOutput("WGOs List Is NULL");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (par_wgo.value != null)
      {
        this.o = par_wgo.value;
        flow_yes.Call(f);
      }
      else
        flow_no.Call(f);
    }));
  }
}
