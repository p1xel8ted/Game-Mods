// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetSkinToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Setting and applying WGO skin")]
[Name("Set WGO skin", 0)]
public class Flow_SetSkinToWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_skin_name = this.AddValueInput<string>("skin_name");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.WGOParamOrSelf(in_wgo).ApplySkin(in_skin_name.value);
      flow_out.Call(f);
    }));
  }
}
