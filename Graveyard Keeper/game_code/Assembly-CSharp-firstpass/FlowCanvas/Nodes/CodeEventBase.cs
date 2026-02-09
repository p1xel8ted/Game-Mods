// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CodeEventBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Subscribes to a C# System.Action Event and is called when the event is raised")]
[Category("Events/Script")]
[Obsolete]
public abstract class CodeEventBase : EventNode<Transform>
{
  [SerializeField]
  public string eventName;
  [SerializeField]
  public System.Type targetType;
  public Component targetComponent;

  public EventInfo eventInfo
  {
    get
    {
      return !System.Type.op_Inequality(this.targetType, (System.Type) null) ? (EventInfo) null : this.targetType.RTGetEvent(this.eventName);
    }
  }

  public void SetEvent(EventInfo e, object instace = null)
  {
    this.targetType = e.RTReflectedType();
    this.eventName = e.Name;
    this.GatherPorts();
  }

  public override void OnGraphStarted()
  {
    this.ResolveSelf();
    if (string.IsNullOrEmpty(this.eventName))
    {
      Debug.LogError((object) "No Event Selected for CodeEvent, or target is NULL");
    }
    else
    {
      this.targetComponent = this.target.value.GetComponent(this.targetType);
      if ((UnityEngine.Object) this.targetComponent == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Target is null");
      }
      else
      {
        if (!EventInfo.op_Equality(this.eventInfo, (EventInfo) null))
          return;
        Debug.LogError((object) $"Event {this.eventName} is not found");
      }
    }
  }
}
