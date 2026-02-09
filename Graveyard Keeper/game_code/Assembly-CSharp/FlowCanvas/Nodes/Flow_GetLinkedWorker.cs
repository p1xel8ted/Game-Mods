// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetLinkedWorker
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Get Linked Worker WGO", 0)]
public class Flow_GetLinkedWorker : MyFlowNode
{
  public WorldGameObject o_wgo;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.o_wgo));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.o_wgo = par_wgo.value.linked_worker;
      flow_out.Call(f);
    }));
  }
}
