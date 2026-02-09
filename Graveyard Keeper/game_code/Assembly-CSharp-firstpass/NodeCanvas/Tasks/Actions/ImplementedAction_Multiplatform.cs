// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ImplementedAction_Multiplatform
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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Implemented Action (mp)", 0)]
[Category("✫ Script Control/Multiplatform")]
[Description("Calls a function that has signature of 'public Status NAME()' or 'public Status NAME(T)'. You should return Status.Success, Failure or Running within that function.")]
public class ImplementedAction_Multiplatform : ActionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  public List<BBObjectParameter> parameters = new List<BBObjectParameter>();
  public NodeCanvas.Status actionStatus = NodeCanvas.Status.Resting;

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
        return "No Action Selected";
      return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"<color=#ff6457>* {this.method.GetMethodString()} *</color>" : $"[ {this.agentInfo}.{this.targetMethod.Name}({(this.parameters.Count == 1 ? (object) this.parameters[0].ToString() : (object) "")}) ]";
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
      return "No method selected";
    return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"Missing method '{this.method.GetMethodString()}'" : (string) null;
  }

  public override void OnExecute() => this.Forward();

  public override void OnUpdate() => this.Forward();

  public void Forward()
  {
    this.actionStatus = (NodeCanvas.Status) this.targetMethod.Invoke((object) this.agent, this.parameters.Select<BBObjectParameter, object>((Func<BBObjectParameter, object>) (p => p.value)).ToArray<object>());
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

  public override void OnStop() => this.actionStatus = NodeCanvas.Status.Resting;

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.method = new SerializedMethodInfo(method);
    this.parameters.Clear();
    foreach (ParameterInfo parameter in method.GetParameters())
    {
      BBObjectParameter bbObjectParameter = new BBObjectParameter(parameter.ParameterType);
      bbObjectParameter.bb = this.blackboard;
      this.parameters.Add(bbObjectParameter);
    }
  }
}
