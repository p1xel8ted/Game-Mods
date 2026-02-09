// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpawnNPC
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("CubePlus", false, "")]
[Description("If WGO is null, then self")]
[Category("Game Actions")]
[Name("Spawn NPC", 0)]
public class Flow_SpawnNPC : MyFlowNode
{
  public WorldGameObject wgo;

  public override void RegisterPorts()
  {
    ValueInput<GameObject> par_go = this.AddValueInput<GameObject>("Point");
    ValueInput<string> par_obj_id = this.AddValueInput<string>("Object id");
    ValueInput<string> par_custom_tag = this.AddValueInput<string>("Custom tag");
    ValueInput<WorldGameObject> par_direction_WGO = this.AddValueInput<WorldGameObject>("Direction to WGO");
    ValueInput<Direction> par_direction_enum = this.AddValueInput<Direction>("Direction Enum");
    ValueInput<Vector2> par_direction_vector2 = this.AddValueInput<Vector2>("Direction Vector2");
    ValueInput<CharAnimState> par_anim_state = this.AddValueInput<CharAnimState>("Animation State");
    ValueInput<string> par_skin_id = this.AddValueInput<string>("Skin id");
    ValueInput<bool> par_is_a_copy = this.AddValueInput<bool>(" Is it a copy?");
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => this.wgo));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.wgo = GS.Spawn(par_obj_id.value, par_go.value.transform, par_custom_tag.value);
      if ((Object) this.wgo == (Object) null)
      {
        Debug.LogError((object) $"Couldn't spawn: {par_obj_id.value} at {par_go.value?.ToString()}");
        flow_out.Call(f);
      }
      else
      {
        this.wgo.components.StartComponents();
        BaseCharacterComponent char_comp = this.wgo.components.character;
        if (char_comp == null)
        {
          Debug.LogError((object) "Character Component is null!");
          flow_out.Call(f);
        }
        else
        {
          if (!par_skin_id.isDefaultValue)
            this.wgo.ApplySkin(par_skin_id.value);
          if (par_is_a_copy.value)
            this.wgo.SetParam("it_is_a_copy", 1f);
          GJTimer.AddTimer(0.1f, (GJTimer.VoidDelegate) (() =>
          {
            if (!par_direction_WGO.isDefaultValue)
              char_comp.LookAt(par_direction_WGO.value);
            else if (par_direction_enum.value != Direction.None)
              char_comp.LookAt(par_direction_enum.value);
            else if (!par_direction_vector2.isDefaultValue)
              char_comp.LookAt(par_direction_vector2.value);
            char_comp.SetAnimationState(!par_anim_state.isDefaultValue ? par_anim_state.value : CharAnimState.Idle);
          }));
          flow_out.Call(f);
        }
      }
    }));
  }
}
