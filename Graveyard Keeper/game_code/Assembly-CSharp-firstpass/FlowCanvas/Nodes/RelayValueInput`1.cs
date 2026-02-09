// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.RelayValueInput`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

public class RelayValueInput<T> : RelayValueInputBase, IEditorMenuCallbackReceiver
{
  [Tooltip("The identifier name of the internal var")]
  public string identifier = "MyInternalVarName";
  [CompilerGenerated]
  public ValueInput<T> \u003Cport\u003Ek__BackingField;

  [HideInInspector]
  public ValueInput<T> port
  {
    get => this.\u003Cport\u003Ek__BackingField;
    set => this.\u003Cport\u003Ek__BackingField = value;
  }

  public override System.Type relayType => typeof (T);

  public override string name => $"@ {this.identifier}";

  public override void RegisterPorts() => this.port = this.AddValueInput<T>("Value");
}
