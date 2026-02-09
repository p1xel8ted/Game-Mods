// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.UIButtonEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("UI Button", 0)]
[Description("Called when the target UI Button is clicked")]
[Category("Events/Object/UI")]
public class UIButtonEvent : EventNode<Button>
{
  public FlowOutput o;

  public override void OnGraphStarted()
  {
    this.ResolveSelf();
    if (this.target.isNull)
      return;
    this.target.value.onClick.AddListener(new UnityAction(this.OnClick));
  }

  public override void OnGraphStoped()
  {
    if (this.target.isNull)
      return;
    this.target.value.onClick.RemoveListener(new UnityAction(this.OnClick));
  }

  public override void RegisterPorts()
  {
    this.o = this.AddFlowOutput("Clicked");
    this.AddValueOutput<Button>("This", (ValueHandler<Button>) (() => this.target.value));
  }

  public void OnClick() => this.o.Call(new Flow());

  [CompilerGenerated]
  public Button \u003CRegisterPorts\u003Eb__3_0() => this.target.value;
}
