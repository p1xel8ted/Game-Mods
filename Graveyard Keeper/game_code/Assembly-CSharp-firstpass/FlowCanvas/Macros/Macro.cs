// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Macros.Macro
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Macros;

public class Macro : FlowGraph
{
  [SerializeField]
  public List<DynamicPortDefinition> inputDefinitions = new List<DynamicPortDefinition>();
  [SerializeField]
  public List<DynamicPortDefinition> outputDefinitions = new List<DynamicPortDefinition>();
  [NonSerialized]
  public Dictionary<string, FlowHandler> entryActionMap = new Dictionary<string, FlowHandler>((IEqualityComparer<string>) StringComparer.Ordinal);
  [NonSerialized]
  public Dictionary<string, FlowHandler> exitActionMap = new Dictionary<string, FlowHandler>((IEqualityComparer<string>) StringComparer.Ordinal);
  [NonSerialized]
  public Dictionary<string, ValueHandlerObject> entryFunctionMap = new Dictionary<string, ValueHandlerObject>((IEqualityComparer<string>) StringComparer.Ordinal);
  [NonSerialized]
  public Dictionary<string, ValueHandlerObject> exitFunctionMap = new Dictionary<string, ValueHandlerObject>((IEqualityComparer<string>) StringComparer.Ordinal);
  public MacroInputNode _entry;
  public MacroOutputNode _exit;

  public override object OnDerivedDataSerialization()
  {
    return (object) new Macro.DerivedSerializationData()
    {
      inputDefinitions = this.inputDefinitions,
      outputDefinitions = this.outputDefinitions
    };
  }

  public override void OnDerivedDataDeserialization(object data)
  {
    if (!(data is Macro.DerivedSerializationData))
      return;
    this.inputDefinitions = ((Macro.DerivedSerializationData) data).inputDefinitions;
    this.outputDefinitions = ((Macro.DerivedSerializationData) data).outputDefinitions;
  }

  public override bool useLocalBlackboard => true;

  public MacroInputNode entry
  {
    get
    {
      if (this._entry == null)
      {
        this._entry = this.allNodes.OfType<MacroInputNode>().FirstOrDefault<MacroInputNode>();
        if (this._entry == null)
          this._entry = this.AddNode<MacroInputNode>(new Vector2((float) (-(double) this.translation.x + 200.0), (float) (-(double) this.translation.y + 200.0)));
      }
      return this._entry;
    }
  }

  public MacroOutputNode exit
  {
    get
    {
      if (this._exit == null)
      {
        this._exit = this.allNodes.OfType<MacroOutputNode>().FirstOrDefault<MacroOutputNode>();
        if (this._exit == null)
          this._exit = this.AddNode<MacroOutputNode>(new Vector2((float) (-(double) this.translation.x + 600.0), (float) (-(double) this.translation.y + 200.0)));
      }
      return this._exit;
    }
  }

  public override void OnGraphValidate()
  {
    base.OnGraphValidate();
    this._entry = (MacroInputNode) null;
    this._exit = (MacroOutputNode) null;
    this._entry = this.entry;
    this._exit = this.exit;
    if (this.inputDefinitions.Count != 0 || this.outputDefinitions.Count != 0)
      return;
    DynamicPortDefinition dynamicPortDefinition1 = new DynamicPortDefinition("In", typeof (Flow));
    DynamicPortDefinition dynamicPortDefinition2 = new DynamicPortDefinition("Out", typeof (Flow));
    this.inputDefinitions.Add(dynamicPortDefinition1);
    this.outputDefinitions.Add(dynamicPortDefinition2);
    this.entry.GatherPorts();
    this.exit.GatherPorts();
    BinderConnection.Create(this.entry.GetOutputPort(dynamicPortDefinition1.ID), this.exit.GetInputPort(dynamicPortDefinition2.ID));
  }

  public bool AddInputDefinition(DynamicPortDefinition def)
  {
    if (this.inputDefinitions.Find((Predicate<DynamicPortDefinition>) (d => d.ID == def.ID)) != null)
      return false;
    this.inputDefinitions.Add(def);
    return true;
  }

  public bool AddOutputDefinition(DynamicPortDefinition def)
  {
    if (this.outputDefinitions.Find((Predicate<DynamicPortDefinition>) (d => d.ID == def.ID)) != null)
      return false;
    this.outputDefinitions.Add(def);
    return true;
  }

  [Serializable]
  public struct DerivedSerializationData
  {
    public List<DynamicPortDefinition> inputDefinitions;
    public List<DynamicPortDefinition> outputDefinitions;
  }
}
