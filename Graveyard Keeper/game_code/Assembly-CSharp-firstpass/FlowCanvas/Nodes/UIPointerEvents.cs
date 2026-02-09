// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UIPointerEvents
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("UI Pointer", 0)]
[Category("Events/Object/UI")]
[Description("Calls UI Pointer based events on target. The Unity Event system has to be set through 'GameObject/UI/Event System'")]
public class UIPointerEvents : MessageEventNode<Transform>
{
  public FlowOutput onPointerDown;
  public FlowOutput onPointerPressed;
  public FlowOutput onPointerUp;
  public FlowOutput onPointerEnter;
  public FlowOutput onPointerExit;
  public FlowOutput onPointerClick;
  public GameObject receiver;
  public PointerEventData eventData;
  public bool updatePressed;

  public override string[] GetTargetMessageEvents()
  {
    return new string[5]
    {
      "OnPointerEnter",
      "OnPointerExit",
      "OnPointerDown",
      "OnPointerUp",
      "OnPointerClick"
    };
  }

  public override void RegisterPorts()
  {
    this.onPointerClick = this.AddFlowOutput("Click");
    this.onPointerDown = this.AddFlowOutput("Down");
    this.onPointerPressed = this.AddFlowOutput("Pressed");
    this.onPointerUp = this.AddFlowOutput("Up");
    this.onPointerEnter = this.AddFlowOutput("Enter");
    this.onPointerExit = this.AddFlowOutput("Exit");
    this.AddValueOutput<GameObject>("Receiver", (ValueHandler<GameObject>) (() => this.receiver));
    this.AddValueOutput<PointerEventData>("Event Data", (ValueHandler<PointerEventData>) (() => this.eventData));
  }

  public void OnPointerDown(MessageRouter.MessageData<PointerEventData> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.eventData = msg.value;
    this.onPointerDown.Call(new Flow());
    this.updatePressed = true;
    this.StartCoroutine(this.UpdatePressed());
  }

  public void OnPointerUp(MessageRouter.MessageData<PointerEventData> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.eventData = msg.value;
    this.onPointerUp.Call(new Flow());
    this.updatePressed = false;
  }

  public IEnumerator UpdatePressed()
  {
    while (this.updatePressed)
    {
      this.onPointerPressed.Call(new Flow());
      yield return (object) null;
    }
  }

  public void OnPointerEnter(MessageRouter.MessageData<PointerEventData> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.eventData = msg.value;
    this.onPointerEnter.Call(new Flow());
  }

  public void OnPointerExit(MessageRouter.MessageData<PointerEventData> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.eventData = msg.value;
    this.onPointerExit.Call(new Flow());
  }

  public void OnPointerClick(MessageRouter.MessageData<PointerEventData> msg)
  {
    this.receiver = this.ResolveReceiver(msg.receiver).gameObject;
    this.eventData = msg.value;
    this.onPointerClick.Call(new Flow());
  }

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__10_0() => this.receiver;

  [CompilerGenerated]
  public PointerEventData \u003CRegisterPorts\u003Eb__10_1() => this.eventData;
}
