// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ExecuteFunction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Script Control/Standalone Only")]
[Description("Execute a function on a script, of up to 6 parameters and save the return if any. If function is an IEnumerator it will execute as a coroutine.")]
public class ExecuteFunction : ActionTask, ISubParametersContainer
{
  [SerializeField]
  public ReflectedWrapper functionWrapper;
  public bool routineRunning;

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
      string str1 = "";
      string str2 = "";
      if (System.Type.op_Equality(this.targetMethod.ReturnType, typeof (void)))
      {
        for (int index = 0; index < variables.Length; ++index)
          str2 = str2 + (index != 0 ? ", " : "") + variables[index].ToString();
      }
      else
      {
        str1 = System.Type.op_Equality(this.targetMethod.ReturnType, typeof (void)) || System.Type.op_Equality(this.targetMethod.ReturnType, typeof (IEnumerator)) || variables[0].isNone ? "" : variables[0]?.ToString() + " = ";
        for (int index = 1; index < variables.Length; ++index)
          str2 = str2 + (index != 1 ? ", " : "") + variables[index].ToString();
      }
      return $"{str1}{this.agentInfo}.{this.targetMethod.Name}({str2})";
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
    if (this.functionWrapper == null)
      return "No Method selected";
    if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return $"Missing Method '{this.functionWrapper.GetMethodString()}'";
    try
    {
      this.functionWrapper.Init((object) this.agent);
      return (string) null;
    }
    catch
    {
      return "ExecuteFunction Error";
    }
  }

  public override void OnExecute()
  {
    if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
    {
      this.EndAction(false);
    }
    else
    {
      try
      {
        if (System.Type.op_Equality(this.targetMethod.ReturnType, typeof (IEnumerator)))
        {
          this.StartCoroutine(this.InternalCoroutine((IEnumerator) ((ReflectedFunctionWrapper) this.functionWrapper).Call()));
        }
        else
        {
          if (System.Type.op_Equality(this.targetMethod.ReturnType, typeof (void)))
            ((ReflectedActionWrapper) this.functionWrapper).Call();
          else
            ((ReflectedFunctionWrapper) this.functionWrapper).Call();
          this.EndAction(true);
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) $"{ex.Message}\n{ex.StackTrace}");
        this.EndAction(false);
      }
    }
  }

  public override void OnStop() => this.routineRunning = false;

  public IEnumerator InternalCoroutine(IEnumerator routine)
  {
    ExecuteFunction executeFunction = this;
    executeFunction.routineRunning = true;
    while (executeFunction.routineRunning && routine.MoveNext())
    {
      if (!executeFunction.routineRunning)
        yield break;
      yield return routine.Current;
    }
    if (executeFunction.routineRunning)
      executeFunction.EndAction();
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.functionWrapper = ReflectedWrapper.Create(method, this.blackboard);
  }
}
