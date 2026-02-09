// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetProperty
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
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Script Control/Standalone Only")]
[Description("Get a property of a script and save it to the blackboard")]
public class GetProperty : ActionTask, ISubParametersContainer
{
  [SerializeField]
  public ReflectedFunctionWrapper functionWrapper;

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
        return "No Property Selected";
      return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"<color=#ff6457>* {this.functionWrapper.GetMethodString()} *</color>" : $"{this.functionWrapper.GetVariables()[0]} = {this.agentInfo}.{this.targetMethod.Name}";
    }
  }

  public override void OnValidate(ITaskSystem ownerSystem)
  {
    if (this.functionWrapper != null && this.functionWrapper.HasChanged())
      this.SetMethod(this.functionWrapper.GetMethod());
    if (this.functionWrapper == null || !MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return;
    this.Error($"Missing Property '{this.functionWrapper.GetMethodString()}'");
  }

  public override string OnInit()
  {
    if (this.functionWrapper == null)
      return "No Property selected";
    if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
      return $"Missing Property '{this.functionWrapper.GetMethodString()}'";
    try
    {
      this.functionWrapper.Init((object) this.agent);
      return (string) null;
    }
    catch
    {
      return "GetProperty Error";
    }
  }

  public override void OnExecute()
  {
    if (this.functionWrapper == null)
    {
      this.EndAction(false);
    }
    else
    {
      this.functionWrapper.Call();
      this.EndAction();
    }
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.functionWrapper = ReflectedFunctionWrapper.Create(method, this.blackboard);
  }
}
