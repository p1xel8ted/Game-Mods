// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.RelayValueInputBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Can be used to set an internal variable, to later be retrieved with a 'Get Internal Var' node.")]
[FlowNode.ContextDefinedInputs(new Type[] {typeof (Wild)})]
[Name("Set Internal Var", 0)]
[Category("Variables/Internal")]
[ExposeAsDefinition]
public abstract class RelayValueInputBase : FlowNode
{
  public abstract Type relayType { get; }
}
