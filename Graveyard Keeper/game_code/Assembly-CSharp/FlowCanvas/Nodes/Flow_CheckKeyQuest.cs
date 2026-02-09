// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_CheckKeyQuest
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Check Key Quest", 0)]
[Category("Game Actions")]
public class Flow_CheckKeyQuest : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_quest_key = this.AddValueInput<string>("Quest Key");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.save.quests.CheckKeyQuests(in_quest_key.value);
      flow_out.Call(f);
    }));
  }
}
