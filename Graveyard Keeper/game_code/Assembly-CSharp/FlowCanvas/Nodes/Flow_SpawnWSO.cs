// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SpawnWSO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Spawn WSO", 0)]
[Category("Game Actions")]
[Description("If WSO is null, place nothing")]
[ParadoxNotion.Design.Icon("CubePlus", false, "")]
[Color("#2a2070")]
public class Flow_SpawnWSO : MyFlowNode
{
  public WorldSimpleObject o_wso;

  public override void RegisterPorts()
  {
    ValueInput<GameObject> par_go = this.AddValueInput<GameObject>("Point");
    ValueInput<string> par_obj_id = this.AddValueInput<string>("WSO id");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.o_wso = Resources.Load<WorldSimpleObject>("objects/WorldSimpleObjects/" + par_obj_id.value);
      if ((UnityEngine.Object) this.o_wso == (UnityEngine.Object) null)
      {
        Debug.LogError((object) $"Couldn't spawn: {par_obj_id.value} at {par_go.value?.ToString()}");
      }
      else
      {
        WorldSimpleObject worldSimpleObject = (WorldSimpleObject) null;
        try
        {
          worldSimpleObject = UnityEngine.Object.Instantiate<WorldSimpleObject>(this.o_wso, MainGame.me.world_root, false);
        }
        catch (Exception ex)
        {
          Debug.LogError((object) $"Flow_SpawnWSO exception[{par_obj_id.value}]: {ex?.ToString()}");
        }
        if ((UnityEngine.Object) worldSimpleObject != (UnityEngine.Object) null)
          worldSimpleObject.transform.position = par_go.value.transform.position;
        else
          Debug.LogError((object) "Flow_SpawnWSO error: new_wso_obj is NULL! Call Artur!");
      }
      flow_out.Call(f);
    }));
  }
}
