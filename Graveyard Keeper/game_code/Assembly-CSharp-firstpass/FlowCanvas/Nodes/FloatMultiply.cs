// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.FloatMultiply
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("×", 0)]
[Category("Logic Operators/Floats")]
public class FloatMultiply : PureFunctionNode<float, float, float>
{
  public override float Invoke(float a, float b) => a * b;
}
