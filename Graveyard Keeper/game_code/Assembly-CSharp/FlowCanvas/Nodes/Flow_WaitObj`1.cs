// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Flow_WaitObj`1
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Wait Obj", 0)]
public class Flow_WaitObj<T> : MyFlowNode
{
  public override void RegisterPorts()
  {
    T o = default (T);
    ValueInput<float> par_time = this.AddValueInput<float>("time");
    ValueInput<T> par_o = this.AddValueInput<T>("obj");
    FlowOutput flow_out = this.AddFlowOutput("Immediate");
    FlowOutput flow_done = this.AddFlowOutput("Finished");
    this.AddValueOutput<T>("obj", (ValueHandler<T>) (() => o));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      T v = par_o.value;
      o = v;
      GJTimer.AddTimer(par_time.value, (GJTimer.VoidDelegate) (() =>
      {
        o = v;
        flow_done.Call(f);
      }));
      flow_out.Call(f);
    }));
  }
}
