// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ImplementedAction
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

[Description("Calls a function that has signature of 'public Status NAME()' or 'public Status NAME(T)'. You should return Status.Success, Failure or Running within that function.")]
[Category("✫ Script Control/Standalone Only")]
public class ImplementedAction : ActionTask, ISubParametersContainer
{
  [SerializeField]
  public ReflectedFunctionWrapper functionWrapper;
  public NodeCanvas.Status actionStatus = NodeCanvas.Status.Resting;

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
        return "No Action Selected";
      return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"<color=#ff6457>* {this.functionWrapper.GetMethodString()} *</color>" : $"[ {this.agentInfo}.{this.targetMethod.Name}({(this.functionWrapper.GetVariables().Length == 2 ? (object) this.functionWrapper.GetVariables()[1].ToString() : (object) "")}) ]";
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
      return "ImplementedAction Error";
    }
  }

  public override void OnExecute() => this.Forward();

  public override void OnUpdate() => this.Forward();

  public void Forward()
  {
    if (this.functionWrapper == null)
    {
      this.EndAction(false);
    }
    else
    {
      this.actionStatus = (NodeCanvas.Status) this.functionWrapper.Call();
      if (this.actionStatus == NodeCanvas.Status.Success)
      {
        this.EndAction(true);
      }
      else
      {
        if (this.actionStatus != NodeCanvas.Status.Failure)
          return;
        this.EndAction(false);
      }
    }
  }

  public override void OnStop() => this.actionStatus = NodeCanvas.Status.Resting;

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.functionWrapper = ReflectedFunctionWrapper.Create(method, this.blackboard);
  }
}
