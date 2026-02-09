// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Macros.MacroNodeWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Macros;

[DoNotList]
public class MacroNodeWrapper : FlowNode, IGraphAssignable, IUpdatable
{
  [SerializeField]
  public Macro _macro;
  public bool instantiated;

  public override string name
  {
    get
    {
      return $"<color=#CCFFFF>{((UnityEngine.Object) this.macro != (UnityEngine.Object) null ? (object) this.macro.name : (object) "No Macro")}</color>";
    }
  }

  public override string description
  {
    get
    {
      return !((UnityEngine.Object) this._macro != (UnityEngine.Object) null) || string.IsNullOrEmpty(this._macro.graphComments) ? base.description : this._macro.graphComments;
    }
  }

  public Macro macro
  {
    get => this._macro;
    set
    {
      if (!((UnityEngine.Object) this._macro != (UnityEngine.Object) value))
        return;
      this._macro = value;
      if (!((UnityEngine.Object) value != (UnityEngine.Object) null))
        return;
      this.GatherPorts();
    }
  }

  Graph IGraphAssignable.nestedGraph
  {
    get => (Graph) this.macro;
    set => this.macro = (Macro) value;
  }

  Graph[] IGraphAssignable.GetInstances()
  {
    if (!this.instantiated)
      return new Graph[0];
    return new Graph[1]{ (Graph) this._macro };
  }

  public void CheckInstance()
  {
    if ((UnityEngine.Object) this.macro == (UnityEngine.Object) null || this.instantiated)
      return;
    this.instantiated = true;
    this.macro = Graph.Clone<Macro>(this.macro);
  }

  void IUpdatable.Update()
  {
    if ((UnityEngine.Object) this.macro == (UnityEngine.Object) null || !this.instantiated)
      return;
    this.macro.UpdateGraph();
  }

  public override void RegisterPorts()
  {
    if ((UnityEngine.Object) this.macro == (UnityEngine.Object) null)
      return;
    for (int index = 0; index < this.macro.inputDefinitions.Count; ++index)
    {
      DynamicPortDefinition defIn = this.macro.inputDefinitions[index];
      if (System.Type.op_Equality(defIn.type, typeof (Flow)))
        this.AddFlowInput(defIn.name, (FlowHandler) (f => this.macro.entryActionMap[defIn.ID](f)), defIn.ID);
      else
        this.macro.entryFunctionMap[defIn.ID] = new ValueHandlerObject(this.AddValueInput(defIn.name, defIn.type, defIn.ID).GetValue);
    }
    for (int index = 0; index < this.macro.outputDefinitions.Count; ++index)
    {
      DynamicPortDefinition defOut = this.macro.outputDefinitions[index];
      if (System.Type.op_Equality(defOut.type, typeof (Flow)))
        this.macro.exitActionMap[defOut.ID] = new FlowHandler(this.AddFlowOutput(defOut.name, defOut.ID).Call);
      else
        this.AddValueOutput(defOut.name, defOut.type, (ValueHandlerObject) (() => this.macro.exitFunctionMap[defOut.ID]()), defOut.ID);
    }
  }
}
