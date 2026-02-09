// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.OwnerVariable
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Name("Self", 100)]
[Description("Returns the Owner GameObject")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (GameObject)})]
public class OwnerVariable : VariableNode
{
  public override string name => "<size=20>SELF</size>";

  public override void RegisterPorts()
  {
    this.AddValueOutput<GameObject>("Value", (ValueHandler<GameObject>) (() => !(bool) (UnityEngine.Object) this.graphAgent ? (GameObject) null : this.graphAgent.gameObject));
  }

  public override void SetVariable(object o)
  {
  }

  [CompilerGenerated]
  public GameObject \u003CRegisterPorts\u003Eb__2_0()
  {
    return !(bool) (UnityEngine.Object) this.graphAgent ? (GameObject) null : this.graphAgent.gameObject;
  }
}
