// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SendGlobalEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Send a Global Event to all graphs")]
[Category("Utility")]
public class SendGlobalEvent : CallableActionNode<string>
{
  public override void Invoke(string eventName) => Graph.SendGlobalEvent(new EventData(eventName));
}
