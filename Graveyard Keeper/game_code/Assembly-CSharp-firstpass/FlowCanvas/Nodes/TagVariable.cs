// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.TagVariable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
[Name("Tag", 0)]
[Description("An easy way to get a Tag name")]
public class TagVariable : VariableNode
{
  [TagField]
  public BBParameter<string> tagName = (BBParameter<string>) "Untagged";

  public override string name => this.tagName.value;

  public override void RegisterPorts()
  {
    this.AddValueOutput<string>("Tag", (ValueHandler<string>) (() => this.tagName.value));
  }

  public override void SetVariable(object o)
  {
    if (!(o is string))
      return;
    this.tagName.value = (string) o;
  }

  [CompilerGenerated]
  public string \u003CRegisterPorts\u003Eb__3_0() => this.tagName.value;
}
