// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_MoveWithCustomAnimation
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Move with custom animation", 0)]
[Category("Game Actions")]
[Description("If WGO is null, then self")]
[ParadoxNotion.Design.Icon("CubeArrowStraight", false, "")]
public class Flow_MoveWithCustomAnimation : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> par_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<GameObject> par_target = this.AddValueInput<GameObject>("target");
    ValueInput<string> par_anim = this.AddValueInput<string>("animation_trigger");
    ValueInput<float> par_speed = this.AddValueInput<float>("speed");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_came = this.AddFlowOutput("Came to dest");
    WorldGameObject _wgo;
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(par_wgo);
      _wgo = worldGameObject;
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "WGO GoTo error: WGO #1 is null");
        flow_out.Call(f);
      }
      else
      {
        float num = par_speed.value;
        if ((double) num <= 0.0)
          num = _wgo.data.GetParam("speed");
        _wgo.TriggerSmartAnimation(par_anim.value);
        float dist = Vector2.Distance((Vector2) par_target.value.transform.position, (Vector2) _wgo.transform.position) / 96f;
        _wgo.components.timer.Play(dist / num);
        if ((Object) _wgo == (Object) MainGame.me.player)
          MainGame.me.player.components.character.player_controlled_by_script = true;
        _wgo.components.character.CurveMoveTo(par_target.value.transform, (AnimationCurve) null, dist, (GJCommons.VoidDelegate) (() =>
        {
          if ((Object) _wgo == (Object) MainGame.me.player)
            MainGame.me.player.components.character.player_controlled_by_script = false;
          flow_came.Call(f);
        }), false, true);
        flow_out.Call(f);
      }
    }));
  }
}
