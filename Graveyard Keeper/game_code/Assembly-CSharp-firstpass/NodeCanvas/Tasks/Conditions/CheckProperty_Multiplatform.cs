// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckProperty_Multiplatform
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
namespace NodeCanvas.Tasks.Conditions;

[Name("Check Property (mp)", 0)]
[Category("✫ Script Control/Multiplatform")]
[Description("Check a property on a script and return if it's equal or not to the check value")]
public class CheckProperty_Multiplatform : ConditionTask
{
  [SerializeField]
  public SerializedMethodInfo method;
  [SerializeField]
  public BBObjectParameter checkValue;
  [SerializeField]
  public CompareMethod comparison;

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
      return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? $"<color=#ff6457>* {this.method.GetMethodString()} *</color>" : $"{this.agentInfo}.{this.targetMethod.Name}{OperationTools.GetCompareString(this.comparison) + this.checkValue.ToString()}";
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
    return MethodInfo.op_Equality(this.targetMethod, (MethodInfo) null) ? "CheckProperty Error" : (string) null;
  }

  public override bool OnCheck()
  {
    if (System.Type.op_Equality(this.checkValue.varType, typeof (float)))
      return OperationTools.Compare((float) this.targetMethod.Invoke((object) this.agent, (object[]) null), (float) this.checkValue.value, this.comparison, 0.05f);
    return System.Type.op_Equality(this.checkValue.varType, typeof (int)) ? OperationTools.Compare((int) this.targetMethod.Invoke((object) this.agent, (object[]) null), (int) this.checkValue.value, this.comparison) : object.Equals(this.targetMethod.Invoke((object) this.agent, (object[]) null), this.checkValue.value);
  }

  public void SetMethod(MethodInfo method)
  {
    if (!MethodInfo.op_Inequality(method, (MethodInfo) null))
      return;
    this.method = new SerializedMethodInfo(method);
    this.checkValue.SetType(method.ReturnType);
    this.comparison = CompareMethod.EqualTo;
  }
}
