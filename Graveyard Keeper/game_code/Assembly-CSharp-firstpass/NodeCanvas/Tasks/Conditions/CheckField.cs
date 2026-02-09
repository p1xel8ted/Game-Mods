// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckField
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Script Control/Common")]
[Description("Check a field on a script and return if it's equal or not to a value")]
public class CheckField : ConditionTask
{
  [SerializeField]
  public BBParameter checkValue;
  [SerializeField]
  public System.Type targetType;
  [SerializeField]
  public string fieldName;
  [SerializeField]
  public CompareMethod comparison;
  public FieldInfo field;

  public override System.Type agentType
  {
    get => !System.Type.op_Inequality(this.targetType, (System.Type) null) ? typeof (Transform) : this.targetType;
  }

  public override string info
  {
    get
    {
      return string.IsNullOrEmpty(this.fieldName) ? "No Field Selected" : $"{this.agentInfo}.{this.fieldName}{(System.Type.op_Equality(this.checkValue.varType, typeof (bool)) ? (object) "" : (object) (OperationTools.GetCompareString(this.comparison) + this.checkValue.ToString()))}";
    }
  }

  public override string OnInit()
  {
    this.field = this.agentType.RTGetField(this.fieldName);
    return FieldInfo.op_Equality(this.field, (FieldInfo) null) ? "Missing Field Info" : (string) null;
  }

  public override bool OnCheck()
  {
    if (System.Type.op_Equality(this.checkValue.varType, typeof (float)))
      return OperationTools.Compare((float) this.field.GetValue((object) this.agent), (float) this.checkValue.value, this.comparison, 0.05f);
    return System.Type.op_Equality(this.checkValue.varType, typeof (int)) ? OperationTools.Compare((int) this.field.GetValue((object) this.agent), (int) this.checkValue.value, this.comparison) : object.Equals(this.field.GetValue((object) this.agent), this.checkValue.value);
  }
}
