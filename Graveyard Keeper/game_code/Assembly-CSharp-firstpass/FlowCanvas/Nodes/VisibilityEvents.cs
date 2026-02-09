// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.VisibilityEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Visibility", 0)]
[Category("Events/Object")]
[Description("Calls events based on object's render visibility")]
public class VisibilityEvents : MessageEventNode<Transform>
{
  public FlowOutput onVisible;
  public FlowOutput onInvisible;
  public GameObject receiver;

  public override string[] GetTargetMessageEvents()
  {
    return new string[2]
    {
      "OnBecameVisible",
      "OnBecameInvisible"
    };
  }

  public override void RegisterPorts()
  {
    this.onVisible = this.AddFlowOutput("Became Visible");
    this.onInvisible = this.AddFlowOutput("Became Invisible");
    this.AddValueOutput<GameObject>("Receiver", (ValueHandler<GameObject>) (() => this.receiver));
  }

  public void OnBecameVisible(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.onVisible.Call(new Flow());
  }

  public void OnBecameInvisible(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.onInvisible.Call(new Flow());
  }

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__4_0() => this.receiver;
}
