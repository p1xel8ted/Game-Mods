// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsTechUnlocked
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is Tech Unlocked", 0)]
[Category("Game Actions")]
public class Flow_IsTechUnlocked : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> tech_id = this.AddValueInput<string>("tech id");
    FlowOutput flow_no = this.AddFlowOutput("Not unlocked");
    FlowOutput flow_yes = this.AddFlowOutput("Unlocked");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(tech_id.value))
        flow_no.Call(f);
      else if (MainGame.me.save.unlocked_techs.Contains(tech_id.value))
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
