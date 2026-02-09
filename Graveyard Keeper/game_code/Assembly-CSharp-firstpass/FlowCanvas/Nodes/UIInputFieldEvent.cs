// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UIInputFieldEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Object/UI")]
[Description("Called when the target UI Dropdown value changed.")]
[Name("UI Field Input", 0)]
public class UIInputFieldEvent : EventNode<InputField>
{
  public FlowOutput onValueChanged;
  public FlowOutput onEndEdit;
  public string value;

  public override void OnGraphStarted()
  {
    this.ResolveSelf();
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
    this.target.value.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
  }

  public override void OnGraphStoped()
  {
    if (this.target.isNull)
      return;
    this.target.value.onValueChanged.RemoveListener(new UnityAction<string>(this.OnValueChanged));
    this.target.value.onEndEdit.RemoveListener(new UnityAction<string>(this.OnEndEdit));
  }

  public override void RegisterPorts()
  {
    this.onValueChanged = this.AddFlowOutput("Value Changed");
    this.onEndEdit = this.AddFlowOutput("End Edit");
    this.AddValueOutput<InputField>("This", (ValueHandler<InputField>) (() => this.target.value));
    this.AddValueOutput<string>("Value", (ValueHandler<string>) (() => this.value));
  }

  public void OnValueChanged(string value)
  {
    this.value = value;
    this.onValueChanged.Call(new Flow());
  }

  public void OnEndEdit(string value)
  {
    this.value = value;
    this.onEndEdit.Call(new Flow());
  }

  [CompilerGenerated]
  public InputField \u003CRegisterPorts\u003Eb__5_0() => this.target.value;

  [CompilerGenerated]
  public string \u003CRegisterPorts\u003Eb__5_1() => this.value;
}
