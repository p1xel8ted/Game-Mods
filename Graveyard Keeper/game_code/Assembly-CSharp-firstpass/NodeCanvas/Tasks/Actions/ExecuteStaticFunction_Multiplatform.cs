// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ExecuteStaticFunction_Multiplatform
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

[Description("Execute a static function and optionaly save the return value")]
[Category("✫ Script Control/Multiplatform")]
[Name("Execute Static Function (mp)", 0)]
public class ExecuteStaticFunction_Multiplatform : ActionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  public List<BBObjectParameter> parameters = new List<BBObjectParameter>();
  [SerializeField]
  [BlackboardOnly]
  public BBObjectParameter returnValue;

  public MethodInfo targetMethod => this.method == null ? (MethodInfo) null : this.method.Get();

  public override string info
  {
    get
    {
      if (this.method == null)
        return "No Method Selected";
      if (MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null))
        return $"<color=#ff6457>* {this.method.GetMethodString()} *</color>";
      string str1 = System.Type.op_Equality(this.targetMethod.ReturnType, typeof (void)) ? "" : this.returnValue.ToString() + " = ";
      string str2 = "";
      for (int index = 0; index < this.parameters.Count; ++index)
        str2 = str2 + (index != 0 ? ", " : "") + this.parameters[index].ToString();
      return $"{str1}{this.targetMethod.DeclaringType.FriendlyName()}.{this.targetMethod.Name} ({str2})";
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
      return "No methMethodd selected";
    return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"Missing Method '{this.method.GetMethodString()}'" : (string) null;
  }

  public override void OnExecute()
  {
    this.returnValue.value = this.targetMethod.Invoke((object) this.agent, this.parameters.Select<BBObjectParameter, object>((Func<BBObjectParameter, object>) (p => p.value)).ToArray<object>());
    this.EndAction();
  }

  public void SetMethod(MethodInfo method)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
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
    if (System.Type.op_Inequality(method.ReturnType, typeof (void)))
    {
      BBObjectParameter bbObjectParameter = new BBObjectParameter(method.ReturnType);
      bbObjectParameter.bb = this.blackboard;
      this.returnValue = bbObjectParameter;
    }
    else
      this.returnValue = (BBObjectParameter) null;
  }
}
