// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_IsCraftUnlocked
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Is Craft Unlocked", 0)]
[Category("Game Actions")]
public class Flow_IsCraftUnlocked : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> craft_id = this.AddValueInput<string>("craft id");
    FlowOutput flow_no = this.AddFlowOutput("Not unlocked");
    FlowOutput flow_yes = this.AddFlowOutput("Unlocked");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(craft_id.value))
        flow_no.Call(f);
      else if (MainGame.me.save.unlocked_crafts.Contains(craft_id.value))
        flow_yes.Call(f);
      else
        flow_no.Call(f);
    }));
  }
}
