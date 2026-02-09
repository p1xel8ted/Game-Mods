// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.AnimatorEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Calls Animator based events. Note that using this node will override root motion as usual, but you can call 'Apply Builtin Root Motion' to get it back.")]
[Category("Events/Object")]
[Name("Animator", 0)]
public class AnimatorEvents : MessageEventNode<Animator>
{
  public FlowOutput onAnimatorMove;
  public FlowOutput onAnimatorIK;
  public Animator receiver;
  public int layerIndex;

  public override string[] GetTargetMessageEvents()
  {
    return new string[2]{ "OnAnimatorIK", "OnAnimatorMove" };
  }

  public override void RegisterPorts()
  {
    this.onAnimatorMove = this.AddFlowOutput("On Animator Move");
    this.onAnimatorIK = this.AddFlowOutput("On Animator IK");
    this.AddValueOutput<Animator>("Receiver", (ValueHandler<Animator>) (() => this.receiver));
    this.AddValueOutput<int>("Layer Index", (ValueHandler<int>) (() => this.layerIndex));
  }

  public void OnAnimatorMove(MessageRouter.MessageData msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.onAnimatorMove.Call(new Flow());
  }

  public void OnAnimatorIK(MessageRouter.MessageData<int> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver);
    this.layerIndex = msg.value;
    this.onAnimatorIK.Call(new Flow());
  }

  [CompilerGenerated]
  public Animator \u003CRegisterPorts\u003Eb__5_0() => this.receiver;

  [CompilerGenerated]
  public int \u003CRegisterPorts\u003Eb__5_1() => this.layerIndex;
}
