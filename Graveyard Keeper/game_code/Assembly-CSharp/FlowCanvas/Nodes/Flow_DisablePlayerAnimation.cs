// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DisablePlayerAnimation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Set global_state = 999")]
[Category("Game Actions")]
[Name("Disable Player Animation", 0)]
public class Flow_DisablePlayerAnimation : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> in_disable = this.AddValueInput<bool>("disable");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_disable.value)
      {
        MainGame.me.player_char.TryDropOverheadItem();
        MainGame.me.player_char.SetAnimationState(CharAnimState.Disabled);
        Debug.Log((object) "Player anim state: Disabled");
      }
      else
      {
        MainGame.me.player_char.SetAnimationState(CharAnimState.Idle);
        Debug.Log((object) "Player anim state: Idle");
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("disable").value ? "Disable Player animations" : "Enable Player animations";
    }
    set => base.name = value;
  }
}
