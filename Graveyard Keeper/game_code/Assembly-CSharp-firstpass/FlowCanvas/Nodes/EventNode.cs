// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.EventNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Color("ff5c5c")]
[Category("Events")]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (Flow)})]
public abstract class EventNode : FlowNode
{
  public override string name => $"➥ {base.name.ToUpper()}";
}
