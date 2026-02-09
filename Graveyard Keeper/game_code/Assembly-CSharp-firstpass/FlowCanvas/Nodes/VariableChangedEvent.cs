// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.VariableChangedEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Variable Change", 0)]
[Description("Called when the target variable change. (Not whenever it is set).")]
[Category("Events/Other")]
public class VariableChangedEvent : EventNode
{
  [BlackboardOnly]
  public BBParameter<object> targetVariable;
  public FlowOutput fOut;
  public object newValue;

  public override string name => $"{base.name} [{this.targetVariable}]";

  public override void OnGraphStarted()
  {
    if (this.targetVariable.varRef == null)
      return;
    this.targetVariable.varRef.onValueChanged += new Action<string, object>(this.OnChanged);
  }

  public override void OnGraphStoped()
  {
    if (this.targetVariable.varRef == null)
      return;
    this.targetVariable.varRef.onValueChanged -= new Action<string, object>(this.OnChanged);
  }

  public override void RegisterPorts()
  {
    if (this.targetVariable.varRef == null)
      return;
    this.fOut = this.AddFlowOutput("Out");
    this.AddValueOutput("Value", this.targetVariable.refType, (ValueHandlerObject) (() => this.newValue));
  }

  public void OnChanged(string name, object value)
  {
    this.newValue = value;
    this.fOut.Call(new Flow());
  }

  public void OnVariableRefChange(Variable newVarRef) => this.GatherPorts();

  [CompilerGenerated]
  public object \u003CRegisterPorts\u003Eb__7_0() => this.newValue;
}
