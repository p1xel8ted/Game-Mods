// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.DebugEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Events/Other")]
[Description("Use to debug send a Flow Signal in PlayMode Only")]
public class DebugEvent : EventNode, IUpdatable
{
  public override void RegisterPorts() => this.AddFlowOutput("Out");

  public void Update()
  {
  }
}
