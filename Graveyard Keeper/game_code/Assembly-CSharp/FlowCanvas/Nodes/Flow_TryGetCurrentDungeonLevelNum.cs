// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TryGetCurrentDungeonLevelNum
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Try Get Current Dungeon Level Num")]
[Icon("CubeArrowCube", false, "")]
[Category("Game Actions")]
[Name("Try Get Current Dungeon Level Num", 0)]
public class Flow_TryGetCurrentDungeonLevelNum : MyFlowNode
{
  public int dungeon_level;

  public override void RegisterPorts()
  {
    this.AddValueOutput<int>("dungeon level num", (ValueHandler<int>) (() => this.dungeon_level));
    FlowOutput flow_yes = this.AddFlowOutput("Dungeon is loaded");
    FlowOutput flow_no = this.AddFlowOutput("Dungeon is NOT loaded");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.dungeon_level = 0;
      if (!MainGame.me.dungeon_root.dungeon_is_loaded_now)
      {
        flow_no.Call(f);
      }
      else
      {
        this.dungeon_level = MainGame.me.dungeon_root.cur_dungeon_preset.dungeon_level;
        flow_yes.Call(f);
      }
    }));
  }
}
