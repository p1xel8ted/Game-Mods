// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_OpenOrganEnhancer
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Open Organ Enhancer GUI", 0)]
public class Flow_OpenOrganEnhancer : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    WorldGameObject _wgo = (WorldGameObject) null;
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      _wgo = this.WGOParamOrSelf(par_wgo);
      GUIElements.me.organ_enhancer_gui.Open(_wgo);
      flow_out.Call(f);
    }));
  }
}
