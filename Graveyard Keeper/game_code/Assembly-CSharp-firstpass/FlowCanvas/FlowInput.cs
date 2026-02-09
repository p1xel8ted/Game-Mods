// Decompiled with JetBrains decompiler
// Type: FlowCanvas.FlowInput
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas;

public class FlowInput : Port
{
  [CompilerGenerated]
  public FlowHandler \u003Cpointer\u003Ek__BackingField;

  public FlowInput(FlowNode parent, string name, string ID, FlowHandler pointer)
    : base(parent, name, ID)
  {
    this.pointer = pointer;
  }

  public FlowHandler pointer
  {
    get => this.\u003Cpointer\u003Ek__BackingField;
    set => this.\u003Cpointer\u003Ek__BackingField = value;
  }

  public override Type type => typeof (Flow);
}
