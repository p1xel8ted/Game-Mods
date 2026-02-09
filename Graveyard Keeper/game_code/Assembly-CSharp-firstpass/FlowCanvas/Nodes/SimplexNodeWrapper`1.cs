// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.SimplexNodeWrapper`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[DoNotList]
[ParadoxNotion.Design.Icon("", false, "GetRuntimeIconType")]
public class SimplexNodeWrapper<T> : FlowNode where T : SimplexNode
{
  [SerializeField]
  public T _simplexNode;

  public System.Type GetRuntimeIconType() => typeof (T);

  public T simplexNode
  {
    get
    {
      if ((object) this._simplexNode == null)
      {
        this._simplexNode = (T) Activator.CreateInstance(typeof (T));
        if ((object) this._simplexNode != null)
          this.GatherPorts();
      }
      return this._simplexNode;
    }
  }

  public override string name => (object) this.simplexNode == null ? "NULL" : this.simplexNode.name;

  public override string description
  {
    get => (object) this.simplexNode == null ? "NULL" : this.simplexNode.description;
  }

  public override System.Type GetNodeWildDefinitionType()
  {
    return typeof (T).GetFirstGenericParameterConstraintType();
  }

  public override void OnCreate(Graph assignedGraph)
  {
    if ((object) this.simplexNode == null)
      return;
    this.simplexNode.SetDefaultParameters((FlowNode) this);
  }

  public override void OnGraphStarted()
  {
    if ((object) this.simplexNode == null)
      return;
    this.simplexNode.OnGraphStarted();
  }

  public override void OnGraphPaused()
  {
    if ((object) this.simplexNode == null)
      return;
    this.simplexNode.OnGraphPaused();
  }

  public override void OnGraphUnpaused()
  {
    if ((object) this.simplexNode == null)
      return;
    this.simplexNode.OnGraphUnpaused();
  }

  public override void OnGraphStoped()
  {
    if ((object) this.simplexNode == null)
      return;
    this.simplexNode.OnGraphStoped();
  }

  public override void RegisterPorts()
  {
    if ((object) this.simplexNode == null)
      return;
    this.simplexNode.RegisterPorts((FlowNode) this);
  }
}
