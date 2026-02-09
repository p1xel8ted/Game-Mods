// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddScriptToWGOsList
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Add Script To WGOs List")]
[Category("Game Actions")]
[Name("Add Script To WGOs List", 0)]
public class Flow_AddScriptToWGOsList : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> in_wgos = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    ValueInput<string> in_script = this.AddValueInput<string>("Script");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if (in_wgos.value == null)
      {
        Debug.LogError((object) "WGO is null");
      }
      else
      {
        foreach (WorldGameObject worldGameObject in in_wgos.value)
        {
          if (!((Object) worldGameObject == (Object) null))
            worldGameObject.AttachFlowScript(in_script.value);
        }
        flow_out.Call(f);
      }
    }));
  }
}
