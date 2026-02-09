// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.IsNotNull
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Logic Operators")]
[Description("Returns if the object is not null")]
[Name("Is Valid", 0)]
public class IsNotNull : PureFunctionNode<bool, object>
{
  public override bool Invoke(object OBJECT) => OBJECT != null;
}
