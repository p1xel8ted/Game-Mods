// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.RelayValueOutputBase
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Get Internal Var", 0)]
[Description("Returns the selected and previously set Internal Variable's input value.")]
[ExposeAsDefinition]
[FlowNode.ContextDefinedOutputs(new Type[] {typeof (Wild)})]
[Category("Variables/Internal")]
[DoNotList]
public abstract class RelayValueOutputBase : FlowNode
{
  public abstract void SetSource(RelayValueInputBase source);
}
