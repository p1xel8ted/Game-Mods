// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_AddScriptToWGO
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Add Script To WGO", 0)]
[Description("Add Script To WGO")]
[Category("Game Actions")]
public class Flow_AddScriptToWGO : MyFlowNode
{
  public override void RegisterPorts()
  {
    ValueInput<WorldGameObject> in_wgo = this.AddValueInput<WorldGameObject>("WGO");
    ValueInput<string> in_script = this.AddValueInput<string>("Script");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((Object) in_wgo.value == (Object) null)
      {
        Debug.LogError((object) "WGO is null");
      }
      else
      {
        in_wgo.value.AttachFlowScript(in_script.value);
        flow_out.Call(f);
      }
    }));
  }
}
