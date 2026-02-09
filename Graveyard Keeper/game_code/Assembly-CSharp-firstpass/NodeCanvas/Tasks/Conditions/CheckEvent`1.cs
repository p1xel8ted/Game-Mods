// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckEvent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Task.EventReceiver(new string[] {"OnCustomEvent"})]
[Category("✫ Utility")]
[Description("Check if an event is received and return true for one frame. Optionaly save the received event's value")]
public class CheckEvent<T> : ConditionTask<GraphOwner>
{
  [RequiredField]
  public BBParameter<string> eventName;
  [BlackboardOnly]
  public BBParameter<T> saveEventValue;

  public override string info => $"Event [{this.eventName}]\n{this.saveEventValue} = EventValue";

  public override bool OnCheck() => false;

  public void OnCustomEvent(EventData receivedEvent)
  {
    if (!this.isActive || !(receivedEvent.name.ToUpper() == this.eventName.value.ToUpper()))
      return;
    if (receivedEvent.value is T)
      this.saveEventValue.value = (T) receivedEvent.value;
    this.YieldReturn(true);
  }
}
