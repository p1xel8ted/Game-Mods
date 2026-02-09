// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckUnityEventValue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Will subscribe to a public UnityEvent<T> and return true when that event is raised and it's value is equal to provided value as well.")]
[Category("✫ Script Control/Common")]
public class CheckUnityEventValue<T> : ConditionTask
{
  [SerializeField]
  public System.Type targetType;
  [SerializeField]
  public string eventName;
  [SerializeField]
  public BBParameter<T> checkValue;

  public override System.Type agentType => this.targetType ?? typeof (Transform);

  public override string info
  {
    get
    {
      return string.IsNullOrEmpty(this.eventName) ? "No Event Selected" : $"'{this.eventName}' Raised && Value == {this.checkValue}";
    }
  }

  public override string OnInit()
  {
    if (this.eventName == null)
      return "No Event Selected";
    FieldInfo field = this.agentType.RTGetField(this.eventName);
    if (FieldInfo.op_Equality(field, (FieldInfo) null))
      return "Event was not found";
    ((UnityEvent<T>) field.GetValue((object) this.agent)).AddListener(new UnityAction<T>(this.Raised));
    return (string) null;
  }

  public void Raised(T eventValue)
  {
    if (!object.Equals((object) this.checkValue.value, (object) eventValue))
      return;
    this.YieldReturn(true);
  }

  public override bool OnCheck() => false;
}
