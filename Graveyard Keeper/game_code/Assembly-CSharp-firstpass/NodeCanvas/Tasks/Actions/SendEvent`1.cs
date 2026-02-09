// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendEvent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Send a graph event with T value. If global is true, all graph owners in scene will receive this event. Use along with the 'Check Event' Condition")]
[Category("✫ Utility")]
public class SendEvent<T> : ActionTask<GraphOwner>
{
  [RequiredField]
  public BBParameter<string> eventName;
  public BBParameter<T> eventValue;
  public BBParameter<float> delay;
  public bool sendGlobal;

  public override string info
  {
    get
    {
      return $"{(this.sendGlobal ? (object) "Global " : (object) "")} Event [{this.eventName}] ({this.eventValue}){((double) this.delay.value > 0.0 ? (object) $" after {this.delay?.ToString()} sec." : (object) "")}";
    }
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.delay.value)
      return;
    EventData<T> eventData = new EventData<T>(this.eventName.value, this.eventValue.value);
    if (this.sendGlobal)
      Graph.SendGlobalEvent((EventData) eventData);
    else
      this.agent.SendEvent((EventData) eventData);
    this.EndAction();
  }
}
