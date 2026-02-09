// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CodeEvent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[Obsolete]
public class CodeEvent<T> : CodeEventBase
{
  public FlowOutput o;
  public Action<T> pointer;
  public T eventValue;

  public override void OnGraphStarted()
  {
    base.OnGraphStarted();
    this.pointer = new Action<T>(this.Call);
    this.eventInfo.AddEventHandler((object) this.targetComponent, (Delegate) this.pointer);
  }

  public override void OnGraphStoped()
  {
    if (string.IsNullOrEmpty(this.eventName) || !EventInfo.op_Inequality(this.eventInfo, (EventInfo) null))
      return;
    this.eventInfo.RemoveEventHandler((object) this.target.value.GetComponent(this.targetType), (Delegate) this.pointer);
  }

  public void Call(T eventValue)
  {
    this.eventValue = eventValue;
    this.o.Call(new Flow());
  }

  public override void RegisterPorts()
  {
    if (string.IsNullOrEmpty(this.eventName))
      return;
    this.o = this.AddFlowOutput(this.eventName);
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => this.eventValue));
  }

  [CompilerGenerated]
  public T \u003CRegisterPorts\u003Eb__6_0() => this.eventValue;
}
