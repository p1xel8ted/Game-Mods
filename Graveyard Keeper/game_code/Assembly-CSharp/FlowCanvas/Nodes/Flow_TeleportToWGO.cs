// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TeleportToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Teleport With Fade", 0)]
public class Flow_TeleportToWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_who = this.AddValueInput<WorldGameObject>("Who");
    ValueInput<string> in_destination = this.AddValueInput<string>("Destination Tag");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    FlowOutput flow_middle = this.AddFlowOutput("Middle");
    FlowOutput flow_finished = this.AddFlowOutput("On Finished");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = in_who.value ?? MainGame.me.player;
      if ((Object) worldGameObject == (Object) null)
      {
        Debug.LogError((object) "TeleportToWGO error: WGO is null");
      }
      else
      {
        if (!worldGameObject.is_player)
          worldGameObject.RedrawBubble();
        WorldGameObject objectByCustomTag = WorldMap.GetWorldGameObjectByCustomTag(in_destination.value);
        if ((Object) objectByCustomTag == (Object) null)
        {
          Debug.LogError((object) ("Can't find destination tag: " + in_destination.value));
        }
        else
        {
          Debug.Log((object) ("Teleporting to target: " + objectByCustomTag.name), (Object) objectByCustomTag.gameObject);
          string[] strArray = in_destination.value.Split('_');
          if (strArray.Length >= 2 && strArray[0] == "tp")
            MainGame.me.save.quests.CheckKeyQuests("tp_" + strArray[1]);
          worldGameObject.cur_gd_point = string.Empty;
          worldGameObject.components.character.TeleportWithFade(objectByCustomTag, (GJCommons.VoidDelegate) (() => flow_middle.Call(f)), (GJCommons.VoidDelegate) (() =>
          {
            GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
            flow_finished.Call(f);
          }));
          flow_out.Call(f);
        }
      }
    }));
  }

  public override string name
  {
    get
    {
      return (Object) this.GetInputValuePort<WorldGameObject>("Who").value == (Object) null ? "Player Teleport To WGO" : base.name;
    }
    set => base.name = value;
  }
}
