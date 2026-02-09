// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.EventNode`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (Wild)})]
public abstract class EventNode<T> : EventNode where T : Component
{
  public BBParameter<T> target;

  public override string name
  {
    get
    {
      return $"{base.name.ToUpper()} [{(!this.target.isNull || this.target.useBlackboard ? (object) this.target.ToString() : (object) "Self")}]";
    }
  }

  public override void OnGraphStarted() => this.ResolveSelf();

  public void ResolveSelf()
  {
    if (!this.target.isNull || this.target.useBlackboard)
      return;
    this.target.value = this.graphAgent.GetComponent<T>();
  }
}
