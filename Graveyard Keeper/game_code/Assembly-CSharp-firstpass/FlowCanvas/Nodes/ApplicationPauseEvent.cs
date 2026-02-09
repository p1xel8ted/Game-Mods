// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ApplicationPauseEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("On Application Pause", 0)]
[Category("Events/Application")]
[Description("Called when the Application is paused or resumed")]
public class ApplicationPauseEvent : EventNode
{
  public FlowOutput pause;
  public bool isPause;

  public override void OnGraphStarted()
  {
    MonoManager.current.onApplicationPause += new Action<bool>(this.ApplicationPause);
  }

  public override void OnGraphStoped()
  {
    MonoManager.current.onApplicationPause -= new Action<bool>(this.ApplicationPause);
  }

  public void ApplicationPause(bool isPause)
  {
    this.isPause = isPause;
    this.pause.Call(new Flow());
  }

  public override void RegisterPorts()
  {
    this.pause = this.AddFlowOutput("Out");
    this.AddValueOutput<bool>("Is Pause", (ValueHandler<bool>) (() => this.isPause));
  }

  [CompilerGenerated]
  public bool \u003CRegisterPorts\u003Eb__5_0() => this.isPause;
}
