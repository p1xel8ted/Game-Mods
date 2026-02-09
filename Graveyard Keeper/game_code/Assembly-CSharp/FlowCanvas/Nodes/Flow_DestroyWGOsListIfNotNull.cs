// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_DestroyWGOsListIfNotNull
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Destroy WGOs List If Not Null", 0)]
[Category("Game Actions")]
[ParadoxNotion.Design.Icon("Cross", false, "")]
[Description("Destroy WGOs List If Not Null")]
public class Flow_DestroyWGOsListIfNotNull : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<List<WorldGameObject>> par_wgo = this.AddValueInput<List<WorldGameObject>>("WGOs List");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      List<WorldGameObject> worldGameObjectList = par_wgo.value;
      if (worldGameObjectList == null)
      {
        flow_out.Call(f);
      }
      else
      {
        foreach (WorldGameObject worldGameObject in worldGameObjectList)
        {
          if (!((Object) worldGameObject == (Object) null))
            worldGameObject.DestroyMe();
        }
        flow_out.Call(f);
      }
    }));
  }
}
