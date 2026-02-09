// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GoTo
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("If WGO is null, then self")]
[ParadoxNotion.Design.Icon("CubeArrowStraight", false, "")]
[Name("GoTo", 0)]
public class Flow_GoTo : MyFlowNode
{
  public override void RegisterPorts()
  {
    string cur_gd_point = string.Empty;
    WorldGameObject _wgo = (WorldGameObject) null;
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<GameObject> par_target = this.AddValueInput<GameObject>("target");
    ValueInput<string> par_event = this.AddValueInput<string>("finished_event");
    ValueInput<float> par_speed = this.AddValueInput<float>("speed");
    ValueInput<MovementComponent.GoToMethod> par_goto_method = this.AddValueInput<MovementComponent.GoToMethod>("method");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_came = this.AddFlowOutput("Came to dest");
    this.AddValueOutput<string>("Current GDPoint", (ValueHandler<string>) (() => cur_gd_point));
    this.AddValueOutput<WorldGameObject>("WGO", (ValueHandler<WorldGameObject>) (() => _wgo));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject o = this.WGOParamOrSelf(par_wgo);
      _wgo = o;
      if ((UnityEngine.Object) o == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "WGO GoTo error: WGO #1 is null");
        flow_out.Call(f);
      }
      else
      {
        GameObject gameObject = par_target.value;
        if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
        {
          Debug.LogError((object) "GoTo: target is null");
          flow_out.Call(f);
        }
        else
        {
          BaseCharacterComponent character = o.components.character;
          if (character == null)
          {
            Debug.LogError((object) "Can't move a non-character WGO");
            flow_out.Call(f);
          }
          else
          {
            ChunkedGameObject chunk = o.gameObject.GetComponent<ChunkedGameObject>();
            bool do_chunk_lock = (UnityEngine.Object) chunk != (UnityEngine.Object) null && !chunk.always_active;
            if (do_chunk_lock)
              chunk.active_now_because_of_movement = true;
            uint? filter_astar_area = new uint?();
            GDPoint component = gameObject.GetComponent<GDPoint>();
            if (par_goto_method.value == MovementComponent.GoToMethod.GDGraph)
            {
              if ((UnityEngine.Object) component != (UnityEngine.Object) null)
              {
                try
                {
                  filter_astar_area = new uint?(component.node.Area);
                }
                catch (Exception ex)
                {
                  Debug.LogError((object) ("Error with GDPoint " + component.gd_tag), (UnityEngine.Object) component);
                  throw;
                }
              }
              else
                Debug.LogError((object) $"[{o.obj_id}] Trying to walk by GDGraph, but target is not GDPoint: {gameObject.name}", (UnityEngine.Object) gameObject);
            }
            o.cur_zone = string.Empty;
            o.cur_gd_point = string.Empty;
            character.GoTo(gameObject, on_complete: (GJCommons.VoidDelegate) (() =>
            {
              if (do_chunk_lock && (UnityEngine.Object) chunk != (UnityEngine.Object) null)
                chunk.active_now_because_of_movement = false;
              _wgo = o;
              flow_came.Call(f);
            }), goto_method: par_goto_method.value, event_on_complete: par_event.value, filter_astar_area: filter_astar_area, from_script: true, target_gdp: component);
            if ((UnityEngine.Object) this.wgo == (UnityEngine.Object) null || !this.wgo.is_player)
              character.SetSpeed(par_speed.HasValue<float>() ? par_speed.value : 1.2f);
            _wgo = o;
            flow_out.Call(f);
          }
        }
      }
    }));
  }

  public override void OnNodeInspectorGUI()
  {
    base.OnNodeInspectorGUI();
    this.MakeStringNullIfEmpty("finished_event");
  }
}
