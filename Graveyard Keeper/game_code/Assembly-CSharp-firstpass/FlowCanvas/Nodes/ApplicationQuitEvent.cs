// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ApplicationQuitEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using ParadoxNotion.Services;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Application")]
[Description("Called when the Application quit")]
[Name("On Application Quit", 0)]
public class ApplicationQuitEvent : EventNode
{
  public FlowOutput quit;

  public override void OnGraphStarted()
  {
    MonoManager.current.onApplicationQuit += new Action(this.ApplicationQuit);
  }

  public override void OnGraphStoped()
  {
    MonoManager.current.onApplicationQuit -= new Action(this.ApplicationQuit);
  }

  public void ApplicationQuit() => this.quit.Call(new Flow());

  public override void RegisterPorts() => this.quit = this.AddFlowOutput("Out");
}
