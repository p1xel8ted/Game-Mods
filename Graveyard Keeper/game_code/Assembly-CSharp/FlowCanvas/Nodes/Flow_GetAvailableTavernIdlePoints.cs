// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetAvailableTavernIdlePoints
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Available Tavern Idle Points", 0)]
[Category("Game Functions")]
[Color("eed9a7")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (List<GDPoint>)})]
public class Flow_GetAvailableTavernIdlePoints : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<string> in_event = this.AddValueInput<string>("event");
    this.AddValueOutput<List<GDPoint>>("List", "GD point", (ValueHandler<List<GDPoint>>) (() =>
    {
      TavernEventDefinition dataOrNull = GameBalance.me.GetDataOrNull<TavernEventDefinition>(in_event.value);
      List<GDPoint> gdPointList = new List<GDPoint>();
      foreach (string availableIdlePoint in MainGame.me.save.players_tavern_engine.GetAvailableIdlePoints(dataOrNull))
      {
        GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(availableIdlePoint);
        if ((UnityEngine.Object) gdPointByGdTag != (UnityEngine.Object) null)
          gdPointList.Add(gdPointByGdTag);
      }
      return gdPointList;
    }));
  }
}
