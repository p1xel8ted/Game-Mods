// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TeleportWGOToGDPoint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Teleport WGO To GD Point", 0)]
public class Flow_TeleportWGOToGDPoint : MyFlowNode
{
  public bool dont_move_camera_while_tp;

  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_who = this.AddValueInput<WorldGameObject>("Who");
    ValueInput<string> in_gd_point_tag = this.AddValueInput<string>("GD Point Tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject player = in_who.value;
      if ((Object) player == (Object) null)
        player = MainGame.me.player;
      else
        player.RedrawBubble();
      player.TeleportToGDPoint(in_gd_point_tag.value, this.dont_move_camera_while_tp);
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return (Object) this.GetInputValuePort<WorldGameObject>("Who").value == (Object) null && !this.GetInputValuePort<WorldGameObject>("Who").isConnected ? "Player Teleport To GD point" : base.name;
    }
    set => base.name = value;
  }
}
