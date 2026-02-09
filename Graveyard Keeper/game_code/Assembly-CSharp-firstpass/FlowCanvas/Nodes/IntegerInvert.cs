// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.IntegerInvert
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Logic Operators/Integers")]
[Name("Invert", 0)]
[Description("Inverts the input ( value = value * -1 )")]
public class IntegerInvert : PureFunctionNode<int, int>
{
  public override int Invoke(int value) => value * -1;
}
