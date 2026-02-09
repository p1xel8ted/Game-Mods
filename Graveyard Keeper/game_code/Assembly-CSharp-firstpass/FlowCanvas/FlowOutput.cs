// Decompiled with JetBrains decompiler
// Type: FlowCanvas.FlowOutput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas;

public class FlowOutput(FlowNode parent, string name, string ID) : Port(parent, name, ID)
{
  [CompilerGenerated]
  public FlowHandler \u003Cpointer\u003Ek__BackingField;

  public FlowHandler pointer
  {
    get => this.\u003Cpointer\u003Ek__BackingField;
    set => this.\u003Cpointer\u003Ek__BackingField = value;
  }

  public override Type type => typeof (Flow);

  public void Call(Flow f)
  {
    if (this.pointer == null || this.parent.graph.isPaused || !Graph.globally_enabled)
      return;
    ++f.ticks;
    this.pointer(f);
  }

  public void BindTo(FlowInput target) => this.pointer = target.pointer;

  public void BindTo(FlowHandler call) => this.pointer = call;

  public void UnBind() => this.pointer = (FlowHandler) null;

  public void Append(FlowHandler action) => this.pointer += action;
}
