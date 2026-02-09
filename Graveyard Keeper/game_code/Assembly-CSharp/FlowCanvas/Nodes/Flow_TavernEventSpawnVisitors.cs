// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernEventSpawnVisitors
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Spawn tavern event visitors", 0)]
[Category("Game Actions")]
[Description("")]
[ParadoxNotion.Design.Icon("CubePlus", false, "")]
public class Flow_TavernEventSpawnVisitors : MyFlowNode
{
  public List<WorldGameObject> visitors;
  public List<WorldGameObject> viewers;

  public override void RegisterPorts()
  {
    ValueInput<string> in_event_name = this.AddValueInput<string>("event name");
    this.AddValueOutput<List<WorldGameObject>>("visitors", (ValueHandler<List<WorldGameObject>>) (() => this.visitors));
    this.AddValueOutput<List<WorldGameObject>>("viewers", (ValueHandler<List<WorldGameObject>>) (() => this.viewers));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (string.IsNullOrEmpty(in_event_name.value))
      {
        Debug.LogError((object) "Flow_TavernEventSpawnVisitors error: event name is empty!");
        flow_out.Call(f);
      }
      else
      {
        TavernEventDefinition data = GameBalance.me.GetData<TavernEventDefinition>(in_event_name.value);
        if (data == null)
        {
          Debug.LogError((object) "Flow_TavernEventSpawnVisitors error: event definition is null!");
          flow_out.Call(f);
        }
        else
        {
          int num1 = Mathf.FloorToInt(data.visitors_count.EvaluateFloat(character: MainGame.me.player));
          int num2 = Mathf.FloorToInt(data.viewers_count.EvaluateFloat(character: MainGame.me.player));
          List<string> availableIdlePoints = MainGame.me.save.players_tavern_engine.GetAvailableIdlePoints(data);
          List<GDPoint> collection1 = new List<GDPoint>();
          List<GDPoint> gdPointList1 = new List<GDPoint>();
          foreach (string tag in availableIdlePoints)
          {
            GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(tag);
            if ((Object) gdPointByGdTag != (Object) null)
              collection1.Add(gdPointByGdTag);
          }
          if (num1 >= availableIdlePoints.Count)
          {
            gdPointList1.AddRange((IEnumerable<GDPoint>) collection1);
          }
          else
          {
            do
            {
              int index = 0;
              if (!data.idle_points_whitelist.Contains(collection1[0].gd_tag))
                index = UnityEngine.Random.Range(0, collection1.Count);
              gdPointList1.Add(collection1[index]);
              collection1.RemoveAt(index);
            }
            while (num1-- > 0);
          }
          this.visitors = new List<WorldGameObject>();
          int length = MainGame.me.save.players_tavern_engine.TAVERN_VISITORS.Length;
          foreach (GDPoint p in gdPointList1)
          {
            WorldGameObject worldGameObject = GS.Spawn(MainGame.me.save.players_tavern_engine.TAVERN_VISITORS[UnityEngine.Random.Range(0, length)], p.transform, "npc_event_visitor");
            worldGameObject.OnCameToGDPoint(p);
            worldGameObject.SetParam("is_in_event", 1f);
            this.visitors.Add(worldGameObject);
          }
          this.viewers = new List<WorldGameObject>();
          List<string> collection2 = new List<string>();
          collection2.AddRange((IEnumerable<string>) data.viewers_points);
          List<string> stringList = new List<string>();
          if (num2 > 0 && collection2.Count > 0)
          {
            if (num2 >= collection2.Count)
            {
              stringList.AddRange((IEnumerable<string>) collection2);
            }
            else
            {
              do
              {
                int index = UnityEngine.Random.Range(0, collection2.Count);
                stringList.Add(collection2[index]);
                collection2.RemoveAt(index);
              }
              while (num2-- > 0);
            }
            List<GDPoint> gdPointList2 = new List<GDPoint>();
            foreach (string tag in stringList)
            {
              GDPoint gdPointByGdTag = WorldMap.GetGDPointByGDTag(tag);
              if ((Object) gdPointByGdTag != (Object) null)
                gdPointList2.Add(gdPointByGdTag);
            }
            foreach (GDPoint p in gdPointList2)
            {
              WorldGameObject worldGameObject = GS.Spawn(MainGame.me.save.players_tavern_engine.TAVERN_VISITORS[UnityEngine.Random.Range(0, length)], p.transform, "npc_event_visitor");
              worldGameObject.OnCameToGDPoint(p);
              worldGameObject.SetParam("is_in_event", 1f);
              worldGameObject.SetParam("is_custom_anim", 1f);
              this.viewers.Add(worldGameObject);
            }
          }
          Debug.Log((object) $"Tavern Engine: Spawned visitors: {this.visitors.Count}, viewers: {this.viewers.Count}");
          flow_out.Call(f);
        }
      }
    }));
  }
}
