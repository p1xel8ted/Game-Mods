// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Cooldown
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (float)})]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (float)})]
[Description("Filters OUT so that it can't be called very frequently")]
[Category("Flow Controllers/Filters")]
public class Cooldown : FlowControlNode
{
  public float current;
  public Coroutine coroutine;

  public override string name => base.name + $" [{this.current.ToString("0.0")}]";

  public override void OnGraphStarted()
  {
    this.current = 0.0f;
    this.coroutine = (Coroutine) null;
  }

  public override void OnGraphStoped()
  {
    if (this.coroutine == null)
      return;
    this.StopCoroutine(this.coroutine);
    this.coroutine = (Coroutine) null;
    this.current = 0.0f;
  }

  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    FlowOutput ready = this.AddFlowOutput("Ready");
    ValueInput<float> time = this.AddValueInput<float>("Time");
    this.AddValueOutput<float>("Current", (ValueHandler<float>) (() => Mathf.Max(this.current, 0.0f)));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      if ((double) this.current > 0.0 || this.coroutine != null)
        return;
      this.current = time.value;
      this.coroutine = this.StartCoroutine(this.CountDown(ready, f));
      o.Call(f);
    }));
    this.AddFlowInput("Cancel", (FlowHandler) (f =>
    {
      if (this.coroutine == null)
        return;
      this.StopCoroutine(this.coroutine);
      this.coroutine = (Coroutine) null;
      this.current = 0.0f;
    }));
  }

  public IEnumerator CountDown(FlowOutput ready, Flow f)
  {
    Cooldown cooldown = this;
    while ((double) cooldown.current > 0.0)
    {
      while (cooldown.graph.isPaused)
        yield return (object) null;
      cooldown.current -= Time.deltaTime;
      yield return (object) null;
    }
    cooldown.coroutine = (Coroutine) null;
    ready.Call(f);
  }
}
