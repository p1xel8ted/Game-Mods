// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SendEvent`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[ExposeAsDefinition]
[Description("Send a Local Event with 1 argument to specified graph")]
[Category("Utility")]
public class SendEvent<T> : CallableActionNode<GraphOwner, string, T>
{
  public override void Invoke(GraphOwner target, string eventName, T eventValue)
  {
    target.SendEvent((EventData) new EventData<T>(eventName, eventValue));
  }
}
