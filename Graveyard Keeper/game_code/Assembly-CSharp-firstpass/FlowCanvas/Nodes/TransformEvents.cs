// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.TransformEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Object")]
[Description("Events relevant to transform changes")]
public class TransformEvents : MessageEventNode<Transform>
{
  public FlowOutput onParentChanged;
  public FlowOutput onChildrenChanged;
  public Transform receiver;
  public Transform parent;
  public IEnumerable<Transform> children;

  public override string[] GetTargetMessageEvents()
  {
    return new string[2]
    {
      "OnTransformParentChanged",
      "OnTransformChildrenChanged"
    };
  }

  public override void RegisterPorts()
  {
    this.onParentChanged = this.AddFlowOutput("On Transform Parent Changed");
    this.onChildrenChanged = this.AddFlowOutput("On Transform Children Changed");
    this.AddValueOutput<Transform>("Receiver", (ValueHandler<Transform>) (() => this.receiver));
    this.AddValueOutput<Transform>("Parent", (ValueHandler<Transform>) (() => this.parent));
    this.AddValueOutput<IEnumerable<Transform>>("Children", (ValueHandler<IEnumerable<Transform>>) (() => this.children));
  }

  public void OnTransformParentChanged(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.parent = this.receiver.parent;
    this.children = this.receiver.Cast<Transform>();
    this.onParentChanged.Call(new Flow());
  }

  public void OnTransformChildrenChanged(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.parent = this.receiver.parent;
    this.children = this.receiver.Cast<Transform>();
    this.onChildrenChanged.Call(new Flow());
  }

  [CompilerGenerated]
  public Transform \u003CRegisterPorts\u003Eb__6_0() => this.receiver;

  [CompilerGenerated]
  public Transform \u003CRegisterPorts\u003Eb__6_1() => this.parent;

  [CompilerGenerated]
  public IEnumerable<Transform> \u003CRegisterPorts\u003Eb__6_2() => this.children;
}
