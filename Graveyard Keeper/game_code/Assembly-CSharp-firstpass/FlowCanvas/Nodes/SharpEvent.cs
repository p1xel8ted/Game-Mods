// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SharpEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System.Reflection;

#nullable disable
namespace FlowCanvas.Nodes;

public abstract class SharpEvent
{
  public object instance;
  public EventInfo eventInfo;

  public static SharpEvent Create(EventInfo eventInfo)
  {
    if (EventInfo.op_Equality(eventInfo, (EventInfo) null))
      return (SharpEvent) null;
    SharpEvent sharpEvent = (SharpEvent) typeof (SharpEvent<>).RTMakeGenericType(eventInfo.EventHandlerType).CreateObject();
    sharpEvent.eventInfo = eventInfo;
    return sharpEvent;
  }

  public void StartListening(
    ReflectedDelegateEvent reflectedEvent,
    ReflectedDelegateEvent.DelegateEventCallback callback)
  {
    if (reflectedEvent == null || callback == null)
      return;
    reflectedEvent.Add(callback);
    this.eventInfo.AddEventHandler(this.instance, reflectedEvent.AsDelegate());
  }

  public void StopListening(
    ReflectedDelegateEvent reflectedEvent,
    ReflectedDelegateEvent.DelegateEventCallback callback)
  {
    if (reflectedEvent == null || callback == null)
      return;
    reflectedEvent.Remove(callback);
    this.eventInfo.RemoveEventHandler(this.instance, reflectedEvent.AsDelegate());
  }
}
