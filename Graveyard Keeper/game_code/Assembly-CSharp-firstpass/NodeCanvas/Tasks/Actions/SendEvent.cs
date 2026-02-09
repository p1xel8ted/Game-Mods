// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SendEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Description("Send a graph event. If global is true, all graph owners in scene will receive this event. Use along with the 'Check Event' Condition")]
public class SendEvent : ActionTask<GraphOwner>
{
  [RequiredField]
  public BBParameter<string> eventName;
  public BBParameter<float> delay;
  public bool sendGlobal;

  public override string info
  {
    get
    {
      return $"{(this.sendGlobal ? "Global " : "")}Send Event [{this.eventName?.ToString()}]{((double) this.delay.value > 0.0 ? $" after {this.delay?.ToString()} sec." : "")}";
    }
  }

  public override void OnUpdate()
  {
    if ((double) this.elapsedTime < (double) this.delay.value)
      return;
    EventData eventData = new EventData(this.eventName.value);
    if (this.sendGlobal)
      Graph.SendGlobalEvent(eventData);
    else
      this.agent.SendEvent(eventData);
    this.EndAction();
  }
}
