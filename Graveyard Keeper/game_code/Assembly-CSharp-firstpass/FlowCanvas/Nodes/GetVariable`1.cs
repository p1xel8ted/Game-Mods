// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.GetVariable`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Variables")]
[Description("Returns a constant or linked variable value.\nYou can alter between constant or linked at any time using the radio button.")]
[FlowNode.ContextDefinedOutputs(new System.Type[] {typeof (Wild)})]
[Name("Graph Variable", 99)]
public class GetVariable<T> : VariableNode
{
  public BBParameter<T> value;

  public override void RegisterPorts()
  {
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => this.value.value));
  }

  public void SetTargetVariableName(string name) => this.value.name = name;

  public override void SetVariable(object o)
  {
    switch (o)
    {
      case Variable<T> _:
        this.value.name = (o as Variable<T>).name;
        break;
      case T obj:
        this.value.value = obj;
        break;
      default:
        Debug.LogError((object) "Set Variable Error");
        break;
    }
  }

  [CompilerGenerated]
  public T \u003CRegisterPorts\u003Eb__1_0() => this.value.value;
}
