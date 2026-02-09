// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetGameResToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Set GameRes to WGO", 0)]
public class Flow_SetGameResToWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<GameRes> in_res = this.AddValueInput<GameRes>("GameRes");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      for (int index = 0; index < in_res.value.Types.Count; ++index)
        in_wgo.value.data.SetParam(in_res.value.Types[index], in_res.value.Get(in_res.value.Types[index]));
      flow_out.Call(f);
    }));
  }
}
