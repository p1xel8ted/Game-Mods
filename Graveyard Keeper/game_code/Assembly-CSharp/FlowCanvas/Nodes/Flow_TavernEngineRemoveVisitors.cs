// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEngineRemoveVisitors
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Game Actions")]
[Name("Tavern Engine remove visitors for event", 0)]
public class Flow_TavernEngineRemoveVisitors : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<bool> in_place_back = this.AddValueInput<bool>("place back");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_place_back.value)
        MainGame.me.save.players_tavern_engine.PlaceVisitorsBackAfterEvent();
      else
        MainGame.me.save.players_tavern_engine.TemporarilyRemoveVisitors();
      flow_out.Call(f);
    }));
  }

  public override string name
  {
    get
    {
      return this.GetInputValuePort<bool>("place back").value ? "Place visitors back after event" : "Remove visitors for event";
    }
    set => base.name = value;
  }
}
