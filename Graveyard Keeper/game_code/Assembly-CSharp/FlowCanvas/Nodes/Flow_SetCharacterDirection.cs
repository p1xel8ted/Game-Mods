// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetCharacterDirection
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Character Direction", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
public class Flow_SetCharacterDirection : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_me_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<WorldGameObject> par_direction_WGO = this.AddValueInput<WorldGameObject>("Direction to WGO");
    ValueInput<Direction> par_direction_enum = this.AddValueInput<Direction>("Direction Enum");
    ValueInput<Vector2> par_direction_vector2 = this.AddValueInput<Vector2>("Direction Vector2");
    ValueInput<GameObject> par_direction_game_object = this.AddValueInput<GameObject>("Direction to GameObject");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_me_wgo);
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO is null !");
        flow_out.Call(f);
      }
      else
      {
        BaseCharacterComponent character = worldGameObject.components.character;
        if (character == null)
        {
          Debug.LogError((object) "Character Component is null!");
          flow_out.Call(f);
        }
        else
        {
          if (!par_direction_WGO.isDefaultValue || (Object) par_direction_WGO.value != (Object) null)
            character.LookAt(par_direction_WGO.value);
          else if (par_direction_enum.value != Direction.None)
            character.LookAt(par_direction_enum.value);
          else if (!par_direction_vector2.isDefaultValue)
            character.LookAt(par_direction_vector2.value);
          else if (!par_direction_game_object.isDefaultValue)
            character.LookAt(par_direction_game_object.value);
          flow_out.Call(f);
        }
      }
    }));
  }
}
