// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UIScrollbarEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when the target UI Scrollbar value changed.")]
[Category("Events/Object/UI")]
[Name("UI Scrollbar", 0)]
public class UIScrollbarEvent : EventNode<Scrollbar>
{
  public FlowOutput o;
  public float value;

  public override void OnGraphStarted()
  {
    this.ResolveSelf();
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
  }

  public override void OnGraphStoped()
  {
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.RemoveListener(new UnityAction<float>(this.OnValueChanged));
  }

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Value Changed");
    this.AddValueOutput<Scrollbar>("This", (ValueHandler<Scrollbar>) (() => this.target.value));
    this.AddValueOutput<float>("Value", (ValueHandler<float>) (() => this.value));
  }

  public void OnValueChanged(float value)
  {
    this.value = value;
    this.o.Call(new Flow());
  }

  [CompilerGenerated]
  public Scrollbar \u003CRegisterPorts\u003Eb__4_0() => this.target.value;

  [CompilerGenerated]
  public float \u003CRegisterPorts\u003Eb__4_1() => this.value;
}
