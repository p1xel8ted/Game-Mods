// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetCopyOfWGOParams
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Get copy of WGO's GameRes", 0)]
public class Flow_GetCopyOfWGOParams : MyFlowNode
{
  public GameRes copy;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    this.AddValueOutput<GameRes>("GameRes", (ValueHandler<GameRes>) (() => this.copy));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.copy = in_wgo.value.data.GetParams().Clone();
      flow_out.Call(f);
    }));
  }
}
