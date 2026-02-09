// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.InputAxisEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Calls out when Horizontal or Vertical Input Axis is not zero")]
[Category("Events/Input")]
[Name("Input Axis (Preset)", 0)]
public class InputAxisEvent : EventNode, IUpdatable
{
  public FlowOutput o;
  public float horizontal;
  public float vertical;
  public bool calledLastFrame;

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Out");
    this.AddValueOutput<float>("Horizontal", (ValueHandler<float>) (() => this.horizontal));
    this.AddValueOutput<float>("Vertical", (ValueHandler<float>) (() => this.vertical));
  }

  public void Update()
  {
    this.horizontal = Input.GetAxis("Horizontal");
    this.vertical = Input.GetAxis("Vertical");
    if ((double) this.horizontal != 0.0 || (double) this.vertical != 0.0)
    {
      this.o.Call(new Flow());
      this.calledLastFrame = true;
    }
    if ((double) this.horizontal != 0.0 || (double) this.vertical != 0.0 || !this.calledLastFrame)
      return;
    this.o.Call(new Flow());
    this.calledLastFrame = false;
  }

  [CompilerGenerated]
  public float \u003CRegisterPorts\u003Eb__4_0() => this.horizontal;

  [CompilerGenerated]
  public float \u003CRegisterPorts\u003Eb__4_1() => this.vertical;
}
