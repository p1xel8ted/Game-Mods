// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SetVariable`1
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
[Description("Set a Blackboard variable value")]
[Name("Set Of Type", 10)]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (Wild)})]
public class SetVariable<T> : FlowNode
{
  [BlackboardOnly]
  public BBParameter<T> targetVariable;
  [HideInInspector]
  public OperationMethod operation;
  [HideInInspector]
  public bool perSecond;

  public override string name
  {
    get
    {
      return $"{this.targetVariable.ToString()}{OperationTools.GetOperationString(this.operation)}{"Value"}";
    }
  }

  public override void RegisterPorts()
  {
    FlowOutput o = this.AddFlowOutput("Out");
    ValueInput<T> v = this.AddValueInput<T>("Value");
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => this.targetVariable.value));
    this.AddFlowInput("In", (FlowHandler) (f =>
    {
      this.DoSet(v.value);
      o.Call(f);
    }));
  }

  public void DoSet(T value)
  {
    if (this.operation != OperationMethod.Set)
    {
      if (System.Type.op_Equality(typeof (T), typeof (float)))
        this.targetVariable.value = (T) (ValueType) OperationTools.Operate((float) (object) this.targetVariable.value, (float) (object) value, this.operation);
      else if (System.Type.op_Equality(typeof (T), typeof (int)))
        this.targetVariable.value = (T) (ValueType) OperationTools.Operate((int) (object) this.targetVariable.value, (int) (object) value, this.operation);
      else if (System.Type.op_Equality(typeof (T), typeof (Vector3)))
        this.targetVariable.value = (T) (ValueType) OperationTools.Operate((Vector3) (object) this.targetVariable.value, (Vector3) (object) value, this.operation);
      else
        this.targetVariable.value = value;
    }
    else
      this.targetVariable.value = value;
  }

  public void SetTargetVariableName(string name) => this.targetVariable.name = name;
}
