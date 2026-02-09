// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TryLoadNextDungeonLevel
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Description("Try Load Next Dungeon Level. CALL ANLY FROM ALREADY LOADED DUNGEON!!!")]
[ParadoxNotion.Design.Icon("CubeArrowCube", false, "")]
[Name("Try Load Next Dungeon Level", 0)]
public class Flow_TryLoadNextDungeonLevel : MyFlowNode
{
  public Texture lut;

  public override void RegisterPorts()
  {
    FlowOutput flow_yes = this.AddFlowOutput("Yes");
    FlowOutput flow_no = this.AddFlowOutput("No");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (!MainGame.me.dungeon_root.dungeon_is_loaded_now)
      {
        Debug.LogError((object) "Calling \"Flow_TryLoadNextDungeonLevel\" not from dungeon!");
      }
      else
      {
        MainGame.me.dungeon_root.UpdateDungeonState();
        if (!MainGame.me.dungeon_root.cur_saved_dungeon.is_completed)
        {
          Debug.Log((object) "Can not load next dungeon level: current is not completed.");
          flow_no.Call(f);
        }
        else
        {
          GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() =>
          {
            GUIElements.ChangeBubblesVisibility(false);
            CameraTools.Fade((GJCommons.VoidDelegate) (() =>
            {
              MainGame.me.TeleportToDungeonLevel(MainGame.me.dungeon_root.cur_dungeon_preset.dungeon_level + 1);
              CameraTools.UnFade((GJCommons.VoidDelegate) (() => GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled)), new float?(0.5f));
            }), new float?(0.5f));
          }));
          flow_yes.Call(f);
        }
      }
    }));
  }
}
