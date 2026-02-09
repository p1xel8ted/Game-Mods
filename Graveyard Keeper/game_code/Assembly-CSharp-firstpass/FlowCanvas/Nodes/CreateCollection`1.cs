// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CreateCollection`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Description("Create a collection of <T> objects")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (Wild)})]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (List<>)})]
public class CreateCollection<T> : VariableNode, IMultiPortNode
{
  [SerializeField]
  public int _portCount = 4;

  public int portCount
  {
    get => this._portCount;
    set => this._portCount = value;
  }

  public override void SetVariable(object o)
  {
  }

  public override void RegisterPorts()
  {
    List<ValueInput<T>> ins = new List<ValueInput<T>>();
    for (int index = 0; index < this.portCount; ++index)
      ins.Add(this.AddValueInput<T>("Element" + index.ToString()));
    this.AddValueOutput<T[]>("Collection", (ValueHandler<T[]>) (() => ins.Select<ValueInput<T>, T>((Func<ValueInput<T>, T>) (p => p.value)).ToArray<T>()));
  }
}
