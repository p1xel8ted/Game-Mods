// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.CreateDictionary`1
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

[Description("Create a Dictionary of <string, T> objects")]
[FlowNode.ContextDefinedInputs(new System.Type[] {typeof (string), typeof (Wild)})]
public class CreateDictionary<T> : VariableNode, IMultiPortNode
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
    List<ValueInput<string>> keys = new List<ValueInput<string>>();
    List<ValueInput<T>> values = new List<ValueInput<T>>();
    for (int index = 0; index < this.portCount; ++index)
    {
      keys.Add(this.AddValueInput<string>("Key" + index.ToString()));
      values.Add(this.AddValueInput<T>("Value" + index.ToString()));
    }
    this.AddValueOutput<IDictionary<string, T>>("Dictionary", (ValueHandler<IDictionary<string, T>>) (() =>
    {
      List<string> k = keys.Select<ValueInput<string>, string>((Func<ValueInput<string>, string>) (x => x.value)).ToList<string>();
      List<T> v = values.Select<ValueInput<T>, T>((Func<ValueInput<T>, T>) (x => x.value)).ToList<T>();
      return (IDictionary<string, T>) k.ToDictionary<string, string, T>((Func<string, string>) (x => x), (Func<string, T>) (x => v[k.IndexOf(x)]));
    }));
  }
}
