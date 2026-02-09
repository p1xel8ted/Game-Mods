// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_LoadDungeon
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Load Dungeon", 0)]
[Description("Load Dungeon")]
public class Flow_LoadDungeon : MyFlowNode
{
  public Texture lut;

  public override void RegisterPorts()
  {
    ValueInput<int> in_dungeon_level = this.AddValueInput<int>("Dunge level");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      MainGame.me.TeleportToDungeonLevel(in_dungeon_level.value);
      flow_out.Call(f);
    }));
  }
}
