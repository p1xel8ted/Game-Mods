// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_TavernWGOsClear
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Clear interfering WGOs under Tavern", 0)]
[Category("Game Actions")]
public class Flow_TavernWGOsClear : MyFlowNode
{
  public Flow_TavernWGOsClear.WGOCoords[] wgos_to_remove = new Flow_TavernWGOsClear.WGOCoords[8]
  {
    new Flow_TavernWGOsClear.WGOCoords(new string[4]
    {
      "tree_3_1",
      "tree_3_1_stump",
      "tree_spawner",
      "tree_micro"
    }, new Vector2(21024f, -1056f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[1]
    {
      "bush_2"
    }, new Vector2(21216f, -1152f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[1]
    {
      "tree_tiny_2"
    }, new Vector2(21348f, -1056f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[4]
    {
      "tree_1_4",
      "tree_1_4_stump",
      "tree_spawner",
      "tree_micro"
    }, new Vector2(21600f, -1056f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[1]
    {
      "tree_tiny_1"
    }, new Vector2(21816f, -936f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[4]
    {
      "flower_small_1",
      "flower_small_4",
      "flower_small_7",
      "flower_spawner"
    }, new Vector2(21072f, -1296f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[4]
    {
      "flower_small_1",
      "flower_small_4",
      "flower_small_7",
      "flower_spawner"
    }, new Vector2(21636f, -1164f)),
    new Flow_TavernWGOsClear.WGOCoords(new string[4]
    {
      "flower_small_2",
      "flower_small_5",
      "flower_small_8",
      "flower_spawner"
    }, new Vector2(21600f, -1308f))
  };

  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      foreach (Flow_TavernWGOsClear.WGOCoords wgoCoords in this.wgos_to_remove)
      {
        if (!WorldMap.DeleteWGO(wgoCoords.ids, wgoCoords.coords, string.Empty))
          Debug.LogError((object) $"FATAL ERROR: Can not delete WGO \"{wgoCoords.ids[0]}\" on coords ({wgoCoords.coords.ToString()})");
      }
      flow_out.Call(f);
    }));
  }

  public struct WGOCoords(string[] ids, Vector2 coords)
  {
    public string[] ids = ids;
    public Vector2 coords = coords;
  }
}
