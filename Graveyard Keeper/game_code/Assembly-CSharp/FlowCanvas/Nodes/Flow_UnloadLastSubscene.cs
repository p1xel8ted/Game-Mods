// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_UnloadLastSubscene
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Removes last additive added subscene")]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubeArrowStraight", false, "")]
[Name("Unload Last Subscene", 0)]
public class Flow_UnloadLastSubscene : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out_fade = this.AddFlowOutput("On fade");
    FlowOutput flow_out = this.AddFlowOutput("On unload started");
    FlowOutput flow_out_unfade = this.AddFlowOutput("On unfaded");
    ValueInput<GameObject> game_object_to_restore = this.AddValueInput<GameObject>("Camera pos");
    ValueInput<bool> restore_camera_to_player = this.AddValueInput<bool>("Camera to player");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      GameObject gameObject = game_object_to_restore.value;
      bool flag_restore_camera_to_player = restore_camera_to_player.value;
      float unfade_camera_time = 1f;
      if ((Object) gameObject != (Object) null)
      {
        Transform target_transform = gameObject.transform;
        if ((Object) target_transform != (Object) null)
        {
          CameraTools.Fade((GJCommons.VoidDelegate) (() =>
          {
            flow_out_fade.Call(f);
            if (!flag_restore_camera_to_player)
              CameraTools.CameraFlyTo(target_transform, (GJCommons.VoidDelegate) (() => CameraTools.UnFade((GJCommons.VoidDelegate) (() => flow_out_unfade.Call(f)), new float?(unfade_camera_time))));
            else
              CameraTools.CameraFlyBack((GJCommons.VoidDelegate) (() =>
              {
                MainGame.me.player.GetComponent<ChunkedGameObject>().active_now_because_of_events = true;
                CameraTools.UnFade((GJCommons.VoidDelegate) (() => flow_out_unfade.Call(f)), new float?(unfade_camera_time));
              }), 0.0f);
            SubsceneLoadManager.UnloadLastScene();
            flow_out.Call(f);
          }), new float?(2f));
        }
        else
        {
          Debug.LogError((object) "Target transform is NULL");
          flow_out_unfade.Call(f);
          flow_out.Call(f);
        }
      }
      else
      {
        Debug.LogError((object) "Target GO is NULL");
        flow_out_fade.Call(f);
        flow_out_unfade.Call(f);
        flow_out.Call(f);
      }
    }));
  }
}
