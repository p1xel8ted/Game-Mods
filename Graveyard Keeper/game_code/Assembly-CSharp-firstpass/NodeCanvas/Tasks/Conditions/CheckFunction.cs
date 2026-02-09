// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Call a function with none or up to 6 parameters on a component and return whether or not the return value is equal to the check value")]
[Category("✫ Script Control/Standalone Only")]
public class CheckFunction : ConditionTask, ISubParametersContainer
{
  [SerializeField]
  public ReflectedFunctionWrapper functionWrapper;
  [SerializeField]
  public BBParameter checkValue;
  [SerializeField]
  public CompareMethod comparison;

  BBParameter[] ISubParametersContainer.GetSubParameters()
  {
    return this.functionWrapper == null ? (BBParameter[]) null : this.functionWrapper.GetVariables();
  }

  public MethodInfo targetMethod
  {
    get => this.functionWrapper == null ? (MethodInfo) null : this.functionWrapper.GetMethod();
  }

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
      if (this.functionWrapper == null)
        return "No Method Selected";
      if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
        return $"<color=#ff6457>* {this.functionWrapper.GetMethodString()} *</color>";
      BBParameter[] variables = this.functionWrapper.GetVariables();
      string str = "";
      for (int index = 1; index < variables.Length; ++index)
        str = str + (index != 1 ? ", " : "") + variables[index].ToString();
      return $"{this.agentInfo}.{this.targetMethod.Name}({str}){OperationTools.GetCompareString(this.comparison) + this.checkValue?.ToString()}";
    }
  }

  public override void OnValidate(ITaskSystem ownerSystem)
  {
    if (this.functionWrapper != null && this.functionWrapper.HasChanged())
      this.SetMethod(this.functionWrapper.GetMethod());
    if (this.functionWrapper == null || !MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return;
    this.Error($"Missing Method '{this.functionWrapper.GetMethodString()}'");
  }

  public override string OnInit()
  {
    if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return "CheckFunction Error";
    try
    {
      this.functionWrapper.Init((object) this.agent);
      return (string) null;
    }
    catch
    {
      return "CheckFunction Error";
    }
  }

  public override bool OnCheck()
  {
    if (this.functionWrapper == null)
      return true;
    if (System.Type.op_Equality(this.checkValue.varType, typeof (float)))
      return OperationTools.Compare((float) this.functionWrapper.Call(), (float) this.checkValue.value, this.comparison, 0.05f);
    return System.Type.op_Equality(this.checkValue.varType, typeof (int)) ? OperationTools.Compare((int) this.functionWrapper.Call(), (int) this.checkValue.value, this.comparison) : object.Equals(this.functionWrapper.Call(), this.checkValue.value);
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.functionWrapper = ReflectedFunctionWrapper.Create(method, this.blackboard);
    this.checkValue = BBParameter.CreateInstance(method.ReturnType, this.blackboard);
    this.comparison = CompareMethod.EqualTo;
  }
}
