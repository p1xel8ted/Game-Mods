// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.OscillatorEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("OSC Pulse", 0)]
[Category("Events/Other")]
[Description("Calls Hi when curve value is greater than 0, else calls Low.\nThe curve is evaluated over time and it's evaluated value is exposed")]
public class OscillatorEvent : EventNode, IUpdatable
{
  public BBParameter<AnimationCurve> curve;
  public float time;
  public float value;
  public FlowOutput hi;
  public FlowOutput low;

  public OscillatorEvent()
  {
    this.curve = (BBParameter<AnimationCurve>) new AnimationCurve(new Keyframe[4]
    {
      new Keyframe(0.0f, 1f),
      new Keyframe(0.5f, 1f),
      new Keyframe(0.5f, -1f),
      new Keyframe(1f, -1f)
    });
  }

  public override void RegisterPorts()
  {
    this.hi = this.AddFlowOutput("Hi");
    this.low = this.AddFlowOutput("Low");
    this.AddValueOutput<float>("Value", (ValueHandler<float>) (() => this.value));
  }

  public override void OnGraphStarted() => this.time = 0.0f;

  public void Update()
  {
    this.value = this.curve.value.Evaluate(this.time);
    this.time += Time.deltaTime;
    this.time = Mathf.Repeat(this.time, 1f);
    this.Call((double) this.value >= 0.0 ? this.hi : this.low, new Flow());
  }

  [CompilerGenerated]
  public float \u003CRegisterPorts\u003Eb__6_0() => this.value;
}
