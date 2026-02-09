// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Check if an event is received and return true for one frame")]
[Task.EventReceiver(new string[] {"OnCustomEvent"})]
[Category("✫ Utility")]
public class CheckEvent : ConditionTask<GraphOwner>
{
  [RequiredField]
  public BBParameter<string> eventName;

  public override string info => $"[{this.eventName.ToString()}]";

  public override bool OnCheck() => false;

  public void OnCustomEvent(EventData receivedEvent)
  {
    if (!this.isActive || !(receivedEvent.name.ToUpper() == this.eventName.value.ToUpper()))
      return;
    this.YieldReturn(true);
  }
}
