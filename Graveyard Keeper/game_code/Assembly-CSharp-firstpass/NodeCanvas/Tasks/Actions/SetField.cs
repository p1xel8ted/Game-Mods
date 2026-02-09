// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SetField
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

[Category("✫ Script Control/Common")]
[Description("Set a variable on a script")]
public class SetField : ActionTask
{
  [SerializeField]
  public BBObjectParameter setValue;
  [SerializeField]
  public System.Type targetType;
  [SerializeField]
  public string fieldName;
  public FieldInfo field;

  public override System.Type agentType => this.targetType ?? typeof (Transform);

  public override string info
  {
    get
    {
      return string.IsNullOrEmpty(this.fieldName) ? "No Field Selected" : $"{this.agentInfo}.{this.fieldName} = {this.setValue}";
    }
  }

  public override string OnInit()
  {
    this.field = this.agentType.RTGetField(this.fieldName);
    return FieldInfo.op_Equality(this.field, (FieldInfo) null) ? "Missing Field: " + this.fieldName : (string) null;
  }

  public override void OnExecute()
  {
    this.field.SetValue((object) this.agent, this.setValue.value);
    this.EndAction();
  }
}
