// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DestroyWGOOrStages
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[ParadoxNotion.Design.Icon("Cross", false, "")]
[Name("Destroy WGO or Stages", 0)]
[Category("Game/Actions")]
[Description("Destroys WGO by obj_id or by its' possible stages on given world position")]
public class Flow_DestroyWGOOrStages : MyFlowNode
{
  public FlowInput @in;
  public FlowOutput @out;
  public ValueInput<string> par_wgo_name;
  public ValueInput<Vector2> par_coord;

  public override void RegisterPorts()
  {
    this.@in = this.AddFlowInput("In", new FlowHandler(this.DestroyOrFindStages));
    this.@out = this.AddFlowOutput("Out");
    this.par_wgo_name = this.AddValueInput<string>("obj id");
    this.par_coord = this.AddValueInput<Vector2>("world position");
  }

  public void DestroyOrFindStages(Flow flow)
  {
    List<WorldGameObject> wgOs1 = WorldMap.FindWGOs(new string[1]
    {
      this.par_wgo_name.value
    }, this.par_coord.value, string.Empty);
    if (wgOs1 != null && wgOs1.Count > 0)
    {
      wgOs1[0].DestroyMe();
      Debug.Log((object) $"Destroy WGO with ID {wgOs1[0].obj_id} at coords {this.par_coord.value.ToString()}");
    }
    else
    {
      string[] stagesOfWgo = this.par_wgo_name.value.GetStagesOfWGO();
      if (stagesOfWgo != null && stagesOfWgo.Length != 0)
      {
        List<WorldGameObject> wgOs2 = WorldMap.FindWGOs(stagesOfWgo, this.par_coord.value, string.Empty);
        if (wgOs2 != null && wgOs2.Count > 0)
        {
          for (int index = 0; index < wgOs2.Count; ++index)
          {
            Debug.Log((object) $"Destroy stage {wgOs2[index].obj_id} for WGO {this.par_wgo_name.value} at coords {this.par_coord.value.ToString()}");
            wgOs2[index].DestroyMe();
          }
        }
        else
          Debug.LogError((object) $"Not found stages for {this.par_wgo_name.value} at coords {this.par_coord.value.ToString()}");
      }
      else
        Debug.LogError((object) $"Couldn't find stages for WGO with obj_id{this.par_wgo_name.value} at coords {this.par_coord.value.ToString()}");
    }
    this.@out.Call(flow);
  }
}
