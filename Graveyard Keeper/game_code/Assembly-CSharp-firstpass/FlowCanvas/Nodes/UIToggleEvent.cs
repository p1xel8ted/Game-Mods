// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UIToggleEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("UI Toggle", 0)]
[Category("Events/Object/UI")]
[Description("Called when the target UI Toggle value changed.")]
public class UIToggleEvent : EventNode<UnityEngine.UI.Toggle>
{
  public FlowOutput o;
  public bool state;

  public override void OnGraphStarted()
  {
    this.ResolveSelf();
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.AddListener(new UnityAction<bool>(this.OnValueChanged));
  }

  public override void OnGraphStoped()
  {
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.RemoveListener(new UnityAction<bool>(this.OnValueChanged));
  }

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Value Changed");
    this.AddValueOutput<UnityEngine.UI.Toggle>("This", (ValueHandler<UnityEngine.UI.Toggle>) (() => this.target.value));
    this.AddValueOutput<bool>("Value", (ValueHandler<bool>) (() => this.state));
  }

  public void OnValueChanged(bool state)
  {
    this.state = state;
    this.o.Call(new Flow());
  }

  [CompilerGenerated]
  public UnityEngine.UI.Toggle \u003CRegisterPorts\u003Eb__4_0() => this.target.value;

  [CompilerGenerated]
  public bool \u003CRegisterPorts\u003Eb__4_1() => this.state;
}
