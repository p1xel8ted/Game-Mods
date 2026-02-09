// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SaveAndExitDungeon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Save and Exit Dungeon", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("CubeArrowCube", false, "")]
[Description("Save Dungeon And Exit.")]
public class Flow_SaveAndExitDungeon : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_target_wgo = this.AddValueInput<WorldGameObject>("Destination WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_target_wgo.value == (Object) null)
      {
        Debug.LogError((object) "Destination WGO is null!!!");
      }
      else
      {
        GUIElements.ChangeBubblesVisibility(false);
        GJTimer.AddTimer(0.01f, (GJTimer.VoidDelegate) (() => MainGame.me.player.components.character.TeleportWithFade(in_target_wgo.value, (GJCommons.VoidDelegate) (() =>
        {
          if (MainGame.me.dungeon_root.TrySaveDungeon())
            Debug.Log((object) "Successfully saved dungeon.");
          MainGame.me.dungeon_root.DestroyTiles();
          MainGame.me.OnExitDungeon();
        }), (GJCommons.VoidDelegate) (() =>
        {
          GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
          MainGame.me.save.quests.CheckKeyQuests("dungeon_exit");
        }))));
        flow_out.Call(f);
      }
    }));
  }
}
