// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.RelayValueOutput`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class RelayValueOutput<T> : RelayValueOutputBase
{
  [SerializeField]
  public string _sourceInputUID;
  public object _sourceInput;

  public string sourceInputUID
  {
    get => this._sourceInputUID;
    set => this._sourceInputUID = value;
  }

  public RelayValueInput<T> sourceInput
  {
    get
    {
      if (this._sourceInput == null)
      {
        this._sourceInput = (object) this.graph.GetAllNodesOfType<RelayValueInput<T>>().FirstOrDefault<RelayValueInput<T>>((Func<RelayValueInput<T>, bool>) (i => i.UID == this.sourceInputUID));
        if (this._sourceInput == null)
          this._sourceInput = new object();
      }
      return this._sourceInput as RelayValueInput<T>;
    }
    set => this._sourceInput = (object) value;
  }

  public override string name
  {
    get
    {
      return $"{(this.sourceInput != null ? (object) this.sourceInput.ToString() : (object) "@ NONE")}";
    }
  }

  public override void SetSource(RelayValueInputBase source)
  {
    this._sourceInputUID = source?.UID;
    this._sourceInput = source != null ? (object) source : (object) (RelayValueInputBase) null;
    this.GatherPorts();
  }

  public override void RegisterPorts()
  {
    this.AddValueOutput<T>("Value", (ValueHandler<T>) (() => this.sourceInput == null ? default (T) : this.sourceInput.port.value));
  }

  [CompilerGenerated]
  public bool \u003Cget_sourceInput\u003Eb__6_0(RelayValueInput<T> i)
  {
    return i.UID == this.sourceInputUID;
  }

  [CompilerGenerated]
  public T \u003CRegisterPorts\u003Eb__11_0()
  {
    return this.sourceInput == null ? default (T) : this.sourceInput.port.value;
  }
}
