// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_RandomIdlePointsGroup
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Random Idle Points Group")]
[Category("Game Actions")]
[Name("Random Idle Points Group", 0)]
public class Flow_RandomIdlePointsGroup : MyFlowNode
{
  public override void RegisterPorts()
  {
    GDPoint.IdlePointPrefix random_idle_prefix = GDPoint.IdlePointPrefix.None;
    this.AddValueOutput<GDPoint.IdlePointPrefix>("random prefix", (ValueHandler<GDPoint.IdlePointPrefix>) (() => random_idle_prefix));
    FlowOutput flow_out = this.AddFlowOutput("Out");
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      random_idle_prefix = (GDPoint.IdlePointPrefix) UnityEngine.Random.Range(0, (int) (Enum.GetValues(typeof (GDPoint.IdlePointPrefix)).Cast<GDPoint.IdlePointPrefix>().Max<GDPoint.IdlePointPrefix>() - 1));
      flow_out.Call(f);
    }));
  }
}
