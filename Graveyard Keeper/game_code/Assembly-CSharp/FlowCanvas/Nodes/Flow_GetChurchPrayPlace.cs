// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_GetChurchPrayPlace
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Church Pray Place", 0)]
[Category("Game Actions")]
public class Flow_GetChurchPrayPlace : MyFlowNode
{
  public override void RegisterPorts()
  {
    FlowOutput flow_out = this.AddFlowOutput("Out");
    GameObject _go = (GameObject) null;
    this.AddValueOutput<GameObject>("GO", (ValueHandler<GameObject>) (() => _go));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      _go = PrayLogics.GetPrayPlace();
      flow_out.Call(f);
    }));
  }
}
