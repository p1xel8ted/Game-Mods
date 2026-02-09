// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsWGONotNull
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is WGO not null", 0)]
[Category("Game Actions")]
[Description("WGO is not null")]
public class Flow_IsWGONotNull : MyFlowNode
{
  public WorldGameObject o;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.o));
    FlowOutput flow_yes = this.AddFlowOutput("WGO Is Not NULL");
    FlowOutput flow_no = this.AddFlowOutput("WGO Is NULL");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) par_wgo.value != (Object) null)
      {
        this.o = par_wgo.value;
        flow_yes.Call(f);
      }
      else
        flow_no.Call(f);
    }));
  }
}
