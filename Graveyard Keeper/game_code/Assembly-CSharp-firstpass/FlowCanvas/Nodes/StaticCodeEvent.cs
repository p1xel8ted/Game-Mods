// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.StaticCodeEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

[Obsolete]
public class StaticCodeEvent : StaticCodeEventBase
{
  public FlowOutput o;
  public Action pointer;

  public override void OnGraphStarted()
  {
    base.OnGraphStarted();
    this.pointer = new Action(this.Call);
    this.eventInfo.AddEventHandler((object) null, (Delegate) this.pointer);
  }

  public override void OnGraphStoped()
  {
    if (string.IsNullOrEmpty(this.eventName) || !EventInfo.op_Inequality(this.eventInfo, (EventInfo) null))
      return;
    this.eventInfo.RemoveEventHandler((object) null, (Delegate) this.pointer);
  }

  public void Call() => this.o.Call(new Flow());

  public override void RegisterPorts()
  {
    if (string.IsNullOrEmpty(this.eventName))
      return;
    this.o = this.AddFlowOutput(this.eventName);
  }
}
