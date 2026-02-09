// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.AND
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Logic Operators/Boolean")]
public class AND : PureFunctionNode<bool, bool, bool>
{
  public override bool Invoke(bool a, bool b) => a & b;
}
