// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ObjectStateEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Object State", 0)]
[Category("Events/Object")]
[Description("OnEnable, OnDisable and OnDestroy callback events for target object")]
public class ObjectStateEvents : MessageEventNode<Transform>
{
  public FlowOutput onEnable;
  public FlowOutput onDisable;
  public FlowOutput onDestroy;
  public GameObject receiver;

  public override string[] GetTargetMessageEvents()
  {
    return new string[3]
    {
      "OnEnable",
      "OnDisable",
      "OnDestroy"
    };
  }

  public override void RegisterPorts()
  {
    this.onEnable = this.AddFlowOutput("On Enable");
    this.onDisable = this.AddFlowOutput("On Disable");
    this.onDestroy = this.AddFlowOutput("On Destroy");
    this.AddValueOutput<GameObject>("Receiver", (ValueHandler<GameObject>) (() => this.receiver));
  }

  public void OnEnable(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.onEnable.Call(new Flow());
  }

  public void OnDisable(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.onDisable.Call(new Flow());
  }

  public void OnDestroy(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.onDestroy.Call(new Flow());
  }

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__5_0() => this.receiver;
}
