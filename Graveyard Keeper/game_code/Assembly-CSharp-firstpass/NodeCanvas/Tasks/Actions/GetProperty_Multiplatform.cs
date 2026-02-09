// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetProperty_Multiplatform
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

[Category("✫ Script Control/Multiplatform")]
[Description("Get a property of a script and save it to the blackboard")]
[Name("Get Property (mp)", 0)]
public class GetProperty_Multiplatform : ActionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  [BlackboardOnly]
  public BBObjectParameter returnValue;

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
      return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"<color=#ff6457>* {this.method.GetMethodString()} *</color>" : $"{this.returnValue.ToString()} = {this.agentInfo}.{this.targetMethod.Name}";
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
      return "No Property selected";
    return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"Missing Property '{this.method.GetMethodString()}'" : (string) null;
  }

  public override void OnExecute()
  {
    this.returnValue.value = this.targetMethod.Invoke((object) this.agent, (object[]) null);
    this.EndAction();
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.method = new SerializedMethodInfo(method);
    this.returnValue.SetType(method.ReturnType);
  }
}
