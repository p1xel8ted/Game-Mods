// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckFunction_Multiplatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Name("Check Function (mp)", 0)]
[Category("✫ Script Control/Multiplatform")]
[Description("Call a function on a component and return whether or not the return value is equal to the check value")]
public class CheckFunction_Multiplatform : ConditionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  public List<BBObjectParameter> parameters = new List<BBObjectParameter>();
  [SerializeField]
  [BlackboardOnly]
  public BBObjectParameter checkValue;
  [SerializeField]
  public CompareMethod comparison;
  public object[] args;

  public MethodInfo targetMethod => this.method == null ? (MethodInfo) null : this.method.Get();

  public override System.Type agentType
  {
    get
    {
      return !MethodInfo.op_Inequality(this.targetMethod, (MethodInfo) null) ? typeof (Transform) : this.targetMethod.RTReflectedType();
    }
  }

  public override string info
  {
    get
    {
      if (this.method == null)
        return "No Method Selected";
      if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
        return $"<color=#ff6457>* {this.method.GetMethodString()} *</color>";
      string str = "";
      for (int index = 0; index < this.parameters.Count; ++index)
        str = str + (index != 0 ? ", " : "") + this.parameters[index].ToString();
      return $"{this.agentInfo}.{this.targetMethod.Name}({str}){OperationTools.GetCompareString(this.comparison) + this.checkValue?.ToString()}";
    }
  }

  public override void OnValidate(ITaskSystem ownerSystem)
  {
    if (this.method != null && this.method.HasChanged())
      this.SetMethod(this.method.Get());
    if (this.method == null || !MethodInfo.op_Equality(this.method.Get(), (MethodInfo) null))
      return;
    this.Error($"Missing Method '{this.method.GetMethodString()}'");
  }

  public override string OnInit()
  {
    if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return "CheckFunction Error";
    if (this.args == null)
      this.args = new object[this.parameters.Count];
    return (string) null;
  }

  public override bool OnCheck()
  {
    for (int index = 0; index < this.parameters.Count; ++index)
      this.args[index] = this.parameters[index].value;
    if (System.Type.op_Equality(this.checkValue.varType, typeof (float)))
      return OperationTools.Compare((float) this.targetMethod.Invoke((object) this.agent, this.args), (float) this.checkValue.value, this.comparison, 0.05f);
    return System.Type.op_Equality(this.checkValue.varType, typeof (int)) ? OperationTools.Compare((int) this.targetMethod.Invoke((object) this.agent, this.args), (int) this.checkValue.value, this.comparison) : object.Equals(this.targetMethod.Invoke((object) this.agent, this.args), this.checkValue.value);
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.method = new SerializedMethodInfo(method);
    this.parameters.Clear();
    foreach (ParameterInfo parameter in method.GetParameters())
    {
      BBObjectParameter bbObjectParameter1 = new BBObjectParameter(parameter.ParameterType);
      bbObjectParameter1.bb = this.blackboard;
      BBObjectParameter bbObjectParameter2 = bbObjectParameter1;
      if (parameter.IsOptional)
        bbObjectParameter2.value = parameter.DefaultValue;
      this.parameters.Add(bbObjectParameter2);
    }
    BBObjectParameter bbObjectParameter = new BBObjectParameter(method.ReturnType);
    bbObjectParameter.bb = this.blackboard;
    this.checkValue = bbObjectParameter;
    this.comparison = CompareMethod.EqualTo;
  }
}
