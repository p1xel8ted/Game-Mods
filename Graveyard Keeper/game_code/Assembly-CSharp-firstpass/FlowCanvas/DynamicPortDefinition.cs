// Decompiled with JetBrains decompiler
// Type: FlowCanvas.DynamicPortDefinition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using UnityEngine;

#nullable disable
namespace FlowCanvas;

[Serializable]
public class DynamicPortDefinition : ISerializationCallbackReceiver
{
  [SerializeField]
  public string _ID;
  [SerializeField]
  public string _name;
  [SerializeField]
  public string _type;
  [NonSerialized]
  public System.Type resolvedType;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    this._type = System.Type.op_Inequality(this.resolvedType, (System.Type) null) ? this.resolvedType.FullName : (string) null;
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    this.resolvedType = ReflectionTools.GetType(this._type, true);
  }

  public string ID
  {
    get
    {
      if (string.IsNullOrEmpty(this._ID))
        this._ID = this.name;
      return this._ID;
    }
    set => this._ID = value;
  }

  public string name
  {
    get => this._name;
    set => this._name = value;
  }

  public System.Type type
  {
    get => this.resolvedType;
    set => this.resolvedType = value;
  }

  public DynamicPortDefinition(string name, System.Type type)
  {
    this.ID = Guid.NewGuid().ToString();
    this.name = name;
    this.type = type;
  }
}
