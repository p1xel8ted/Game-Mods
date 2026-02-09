// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_SetWeather
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Set Weather OLD", 0)]
[Category("Game Actions")]
[Description("OLD FUNCTION, DO NOT USE")]
[Color("FF0000")]
public class Flow_SetWeather : MyFlowNode
{
  public override void RegisterPorts()
  {
    this.AddValueInput<float>("Rain");
    this.AddValueInput<float>("Wind");
    this.AddValueInput<float>("Fog");
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      Debug.LogError((object) "Calling outdated flow block: Flow_SetWeather");
      flow_out.Call(f);
    }));
  }
}
