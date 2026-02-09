// Decompiled with JetBrains decompiler
// Type: Flow_IsDungeonLvlCompleted
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using FlowCanvas;
using FlowCanvas.Nodes;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
[Category("Game Actions")]
[Name("Is Dungeon LVL Completed", 0)]
public class Flow_IsDungeonLvlCompleted : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput yes;
  public FlowOutput no;
  public ValueInput<int> dungeon_lvl_in;
  public ValueOutput<bool> dungeon_lvl_completed_out;
  public bool dungeon_lvl_completed_out_value;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.IsDungeonLvlCompleted));
    this.yes = this.AddFlowOutput("Yes");
    this.no = this.AddFlowOutput("No");
    this.dungeon_lvl_in = this.AddValueInput<int>("lvl_number");
    this.dungeon_lvl_completed_out = this.AddValueOutput<bool>("is_completed", (ValueHandler<bool>) (() => this.dungeon_lvl_completed_out_value));
  }

  public void IsDungeonLvlCompleted(Flow flow)
  {
    SavedDungeon savedDungeon = MainGame.me.save.dungeons.GetSavedDungeon(this.dungeon_lvl_in.value);
    MainGame.me.dungeon_root.UpdateDungeonState();
    if (savedDungeon != null)
    {
      this.dungeon_lvl_completed_out_value = savedDungeon.is_completed;
      if (savedDungeon.is_completed)
        this.yes.Call(flow);
      else
        this.no.Call(flow);
    }
    else
    {
      Debug.LogError((object) ("No found saved dungeon for number " + this.dungeon_lvl_in.value.ToString()));
      this.no.Call(flow);
    }
  }

  [CompilerGenerated]
  public bool \u003CRegisterPorts\u003Eb__6_0() => this.dungeon_lvl_completed_out_value;
}
