// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetProperty_Multiplatform
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Design;
using ParadoxNotion.Serialization;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Set Property (mp)", 0)]
[Category("✫ Script Control/Multiplatform")]
[Description("Set a property on a script")]
public class SetProperty_Multiplatform : ActionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  public BBObjectParameter parameter;

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
        return "No Property Selected";
      return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"<color=#ff6457>* {this.method.GetMethodString()} *</color>" : $"{this.agentInfo}.{this.targetMethod.Name} = {this.parameter.ToString()}";
    }
  }

  public override void OnValidate(ITaskSystem ownerSystem)
  {
    if (this.method != null && this.method.HasChanged())
      this.SetMethod(this.method.Get());
    if (this.method == null || !MethodInfo.op_Equality(this.method.Get(), (MethodInfo) null))
      return;
    this.Error($"Missing Property '{this.method.GetMethodString()}'");
  }

  public override string OnInit()
  {
    if (this.method == null)
      return "No property selected";
    return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"Missing property '{this.method.GetMethodString()}'" : (string) null;
  }

  public override void OnExecute()
  {
    this.targetMethod.Invoke((object) this.agent, new object[1]
    {
      this.parameter.value
    });
    this.EndAction();
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.method = new SerializedMethodInfo(method);
    this.parameter.SetType(method.GetParameters()[0].ParameterType);
  }
}
