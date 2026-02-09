// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckEventValue`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Category("✫ Utility")]
[Description("Check if an event is received and it's value is equal to specified value, then return true for one frame")]
[Task.EventReceiver(new string[] {"OnCustomEvent"})]
public class CheckEventValue<T> : ConditionTask<GraphOwner>
{
  [RequiredField]
  public BBParameter<string> eventName;
  [Name("Compare Value To", 0)]
  public BBParameter<T> value;

  public override string info => $"Event [{this.eventName}].value == {this.value}";

  public override bool OnCheck() => false;

  public void OnCustomEvent(EventData receivedEvent)
  {
    if (!(receivedEvent is EventData<T>) || !this.isActive || !(receivedEvent.name.ToUpper() == this.eventName.value.ToUpper()))
      return;
    T obj = ((EventData<T>) receivedEvent).value;
    if ((object) obj == null || !obj.Equals((object) this.value.value))
      return;
    this.YieldReturn(true);
  }
}
