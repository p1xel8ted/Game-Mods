// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SendEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Utility")]
[Description("Send a Local Event to specified graph")]
public class SendEvent : CallableActionNode<GraphOwner, string>
{
  public override void Invoke(GraphOwner target, string eventName)
  {
    target.SendEvent(new EventData(eventName));
  }
}
