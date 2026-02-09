// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWGOScale
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set WGO Scale", 0)]
[Category("Game Actions")]
public class Flow_SetWGOScale : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<Vector3> in_scale = this.AddValueInput<Vector3>("Scale Vector");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      WorldGameObject worldGameObject = this.WGOParamOrSelf(in_wgo);
      if ((UnityEngine.Object) worldGameObject == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Flow_SetWGOScale error: WGO is null");
      }
      else
      {
        Transform transform = (Transform) null;
        try
        {
          transform = worldGameObject.GetComponent<Transform>();
        }
        catch (Exception ex)
        {
          Debug.LogError((object) ("Flow_SetWGOScale exception: " + ex?.ToString()));
        }
        if ((UnityEngine.Object) transform != (UnityEngine.Object) null)
        {
          transform.localScale = in_scale.value;
          Debug.Log((object) $"Flow_SetWGOScale: set scale {in_scale.value} to WGO name={worldGameObject.name}, obj_id={worldGameObject.obj_id}");
        }
      }
      flow_out.Call(f);
    }));
  }
}
