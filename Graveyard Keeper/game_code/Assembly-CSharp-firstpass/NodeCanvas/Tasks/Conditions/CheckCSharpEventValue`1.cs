// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckCSharpEventValue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Script Control/Common")]
[Description("Will subscribe to a public event of Action<T> type and return true when the event is raised and it's value is equal to provided value as well.\n(eg public event System.Action<T> [name])")]
public class CheckCSharpEventValue<T> : ConditionTask
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
    EventInfo eventInfo = this.agentType.RTGetEvent(this.eventName);
    if (EventInfo.op_Equality(eventInfo, (EventInfo) null))
      return "Event was not found";
    Delegate handler = this.GetType().RTGetMethod("Raised").RTCreateDelegate(eventInfo.EventHandlerType, (object) this);
    eventInfo.AddEventHandler((object) this.agent, handler);
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
