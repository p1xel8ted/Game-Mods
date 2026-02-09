// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckStaticCSharpEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Script Control/Common")]
[Description("Will subscribe to a public event of Action type and return true when the event is raised.\n(eg public static event System.Action [name])")]
public class CheckStaticCSharpEvent : ConditionTask
{
  [SerializeField]
  public System.Type targetType;
  [SerializeField]
  public string eventName;

  public override string info
  {
    get
    {
      return string.IsNullOrEmpty(this.eventName) ? "No Event Selected" : $"'{this.eventName}' Raised";
    }
  }

  public override string OnInit()
  {
    if (this.eventName == null)
      return "No Event Selected";
    EventInfo eventInfo = this.targetType.RTGetEvent(this.eventName);
    if (EventInfo.op_Equality(eventInfo, (EventInfo) null))
      return "Event was not found";
    Action handler = (Action) (() => this.Raised());
    eventInfo.AddEventHandler((object) null, (Delegate) handler);
    return (string) null;
  }

  public void Raised() => this.YieldReturn(true);

  public override bool OnCheck() => false;

  [CompilerGenerated]
  public void \u003COnInit\u003Eb__4_0() => this.Raised();
}
