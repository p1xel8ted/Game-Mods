// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddBodyToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Add Item To WGO")]
[Category("Game Actions")]
[Name("Add Body To WGO", 0)]
public class Flow_AddBodyToWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddValueInput<WorldGameObject>("WGO");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      Debug.LogException(new Exception("Deprecated method. Don't use it!"));
      flow_out.Call(f);
    }));
  }
}
