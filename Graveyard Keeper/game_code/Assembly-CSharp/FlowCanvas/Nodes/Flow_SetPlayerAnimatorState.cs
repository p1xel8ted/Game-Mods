// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetPlayerAnimatorState
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Be careful and return state to idle/walk")]
[Name("Set Player Animator State", 0)]
[Category("Game Actions")]
public class Flow_SetPlayerAnimatorState : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<int> state = this.AddValueInput<int>("State");
    FlowOutput flow_out = this.AddFlowOutput("Out", "Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.player_char.TryDropOverheadItem();
      MainGame.me.player.components.character.SetGlobalState(state.value);
      flow_out.Call(f);
    }));
  }
}
