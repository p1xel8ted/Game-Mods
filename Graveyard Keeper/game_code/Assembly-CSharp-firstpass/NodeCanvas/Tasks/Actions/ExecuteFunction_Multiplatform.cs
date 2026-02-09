// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ExecuteFunction_Multiplatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Execute Function (mp)", 0)]
[Description("Execute a function on a script and save the return if any. If function is an IEnumerator it will execute as a coroutine.")]
[Category("✫ Script Control/Multiplatform")]
public class ExecuteFunction_Multiplatform : ActionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  public List<BBObjectParameter> parameters = new List<BBObjectParameter>();
  [SerializeField]
  public List<bool> parameterIsByRef = new List<bool>();
  [SerializeField]
  [BlackboardOnly]
  public BBObjectParameter returnValue;
  public object[] args;
  public bool routineRunning;

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
      string str1 = System.Type.op_Equality(this.targetMethod.ReturnType, typeof (void)) || System.Type.op_Equality(this.targetMethod.ReturnType, typeof (IEnumerator)) ? "" : this.returnValue.ToString() + " = ";
      string str2 = "";
      for (int index = 0; index < this.parameters.Count; ++index)
        str2 = str2 + (index != 0 ? ", " : "") + this.parameters[index].ToString();
      return $"{str1}{this.agentInfo}.{this.targetMethod.Name}({str2})";
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
    if (this.method == null)
      return "No Method selected";
    if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return $"Missing Method '{this.method.GetMethodString()}'";
    if (this.args == null)
      this.args = new object[this.parameters.Count];
    if (this.parameterIsByRef.Count != this.parameters.Count)
      this.parameterIsByRef = this.parameters.Select<BBObjectParameter, bool>((Func<BBObjectParameter, bool>) (p => false)).ToList<bool>();
    return (string) null;
  }

  public override void OnExecute()
  {
    for (int index = 0; index < this.parameters.Count; ++index)
      this.args[index] = this.parameters[index].value;
    if (System.Type.op_Equality(this.targetMethod.ReturnType, typeof (IEnumerator)))
    {
      this.StartCoroutine(this.InternalCoroutine((IEnumerator) this.targetMethod.Invoke((object) this.agent, this.args)));
    }
    else
    {
      this.returnValue.value = this.targetMethod.Invoke((object) this.agent, this.args);
      for (int index = 0; index < this.parameters.Count; ++index)
      {
        if (this.parameterIsByRef[index])
          this.parameters[index].value = this.args[index];
      }
      this.EndAction();
    }
  }

  public override void OnStop() => this.routineRunning = false;

  public IEnumerator InternalCoroutine(IEnumerator routine)
  {
    ExecuteFunction_Multiplatform functionMultiplatform = this;
    functionMultiplatform.routineRunning = true;
    while (functionMultiplatform.routineRunning && routine.MoveNext())
    {
      if (!functionMultiplatform.routineRunning)
        yield break;
      yield return routine.Current;
    }
    if (functionMultiplatform.routineRunning)
      functionMultiplatform.EndAction();
  }

  public void SetMethod(MethodInfo method)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
      return;
    this.method = new SerializedMethodInfo(method);
    this.parameters.Clear();
    foreach (ParameterInfo parameter in method.GetParameters())
    {
      System.Type parameterType = parameter.ParameterType;
      BBObjectParameter bbObjectParameter1 = new BBObjectParameter(parameterType.IsByRef ? parameterType.GetElementType() : parameterType);
      bbObjectParameter1.bb = this.blackboard;
      BBObjectParameter bbObjectParameter2 = bbObjectParameter1;
      if (parameter.IsOptional)
        bbObjectParameter2.value = parameter.DefaultValue;
      this.parameters.Add(bbObjectParameter2);
      this.parameterIsByRef.Add(parameterType.IsByRef);
    }
    if (System.Type.op_Inequality(method.ReturnType, typeof (void)) && System.Type.op_Inequality(this.targetMethod.ReturnType, typeof (IEnumerator)))
    {
      BBObjectParameter bbObjectParameter = new BBObjectParameter(method.ReturnType);
      bbObjectParameter.bb = this.blackboard;
      this.returnValue = bbObjectParameter;
    }
    else
      this.returnValue = (BBObjectParameter) null;
  }
}
