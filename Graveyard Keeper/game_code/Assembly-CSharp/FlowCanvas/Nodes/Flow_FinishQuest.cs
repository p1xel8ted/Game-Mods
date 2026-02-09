// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_FinishQuest
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Finish Quest", 0)]
public class Flow_FinishQuest : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_quest_id = this.AddValueInput<string>("Quest ID");
    ValueInput<bool> in_success = this.AddValueInput<bool>("T-Success\nF-Fail");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.save.quests.ForceQuestEnd(in_quest_id.value, in_success.value);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get => $"{base.name}\n{this.GetInputValuePort<string>("Quest ID").value}";
    set => base.name = value;
  }
}
