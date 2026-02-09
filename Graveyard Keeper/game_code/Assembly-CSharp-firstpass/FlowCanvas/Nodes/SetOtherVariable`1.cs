// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SetOtherVariable`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Variables/Blackboard")]
[Description("Use this to set a variable value of blackboards other than the one this flowscript is using")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (Blackboard), typeof (Wild)})]
[Name("Set Other Of Type", 0)]
public class SetOtherVariable<T> : FlowNode
{
  public OperationMethod operation;
  public ValueInput<string> varName;

  public override string name
  {
    get => $"${this.varName.value}{OperationTools.GetOperationString(this.operation)}Value";
  }

  public override void RegisterPorts()
  {
    ValueInput<Blackboard> bb = this.AddValueInput<Blackboard>("Blackboard");
    this.varName = this.AddValueInput<string>("Variable");
    ValueInput<T> v = this.AddValueInput<T>("Value");
    FlowOutput o = this.AddFlowOutput("Out");
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => bb.value.GetValue<T>(this.varName.value)));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.DoSet(bb.value, this.varName.value, v.value);
      o.Call(f);
    }));
  }

  public void DoSet(Blackboard bb, string name, T value)
  {
    Variable<T> variable = bb.GetVariable<T>(name) ?? (Variable<T>) bb.AddVariable(this.varName.value, typeof (T));
    if (this.operation != OperationMethod.Set)
    {
      if (System.Type.op_Equality(typeof (T), typeof (float)))
        variable.value = (T) (ValueType) OperationTools.Operate((float) (object) variable.value, (float) (object) value, this.operation);
      else if (System.Type.op_Equality(typeof (T), typeof (int)))
        variable.value = (T) (ValueType) OperationTools.Operate((int) (object) variable.value, (int) (object) value, this.operation);
      else if (System.Type.op_Equality(typeof (T), typeof (Vector3)))
        variable.value = (T) (ValueType) OperationTools.Operate((Vector3) (object) variable.value, (Vector3) (object) value, this.operation);
      else
        variable.value = value;
    }
    else
      variable.value = value;
  }
}
