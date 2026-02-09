// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_StopAnyMovement
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Stop any movement", 0)]
public class Flow_StopAnyMovement : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<bool> in_is_player = this.AddValueInput<bool>("is player");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_is_player.value)
      {
        MainGame.me.player_char.StopMovement();
        MainGame.me.player_char.player_controlled_by_script = false;
      }
      else
      {
        WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
        if ((Object) worldGameObject == (Object) null)
        {
          Debug.LogError((object) "WGO is null!");
          flow_out.Call(f);
          return;
        }
        if (!worldGameObject.obj_def.IsCharacter())
        {
          Debug.LogError((object) "WGO is not character!");
          flow_out.Call(f);
          return;
        }
        BaseCharacterComponent character = worldGameObject.components.character;
        if (character == null)
        {
          Debug.LogError((object) "BaseCharacterComponent is null!");
          flow_out.Call(f);
          return;
        }
        character.StopMovement();
        if (worldGameObject.is_player)
          character.player_controlled_by_script = false;
      }
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("is player").value ? "<color=#FFFF50>Stop player's movement </color>" : base.name;
    }
    set => base.name = value;
  }
}
