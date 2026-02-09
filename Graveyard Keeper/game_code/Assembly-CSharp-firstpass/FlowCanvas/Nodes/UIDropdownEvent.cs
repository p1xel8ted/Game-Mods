// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UIDropdownEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Called when the target UI Dropdown value changed.")]
[Category("Events/Object/UI")]
[Name("UI Dropdown", 0)]
public class UIDropdownEvent : EventNode<Dropdown>
{
  public FlowOutput o;
  public int value;

  public override void OnGraphStarted()
  {
    this.ResolveSelf();
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.AddListener(new UnityAction<int>(this.OnValueChanged));
  }

  public override void OnGraphStoped()
  {
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.RemoveListener(new UnityAction<int>(this.OnValueChanged));
  }

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Value Changed");
    this.AddValueOutput<Dropdown>("This", (ValueHandler<Dropdown>) (() => this.target.value));
    this.AddValueOutput<int>("Value", (ValueHandler<int>) (() => this.value));
  }

  public void OnValueChanged(int value)
  {
    this.value = value;
    this.o.Call(new Flow());
  }

  [CompilerGenerated]
  public Dropdown \u003CRegisterPorts\u003Eb__4_0() => this.target.value;

  [CompilerGenerated]
  public int \u003CRegisterPorts\u003Eb__4_1() => this.value;
}
