// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.SerializedTypeInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Serialization.FullSerializer.Internal;
using System;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization;

[Serializable]
public class SerializedTypeInfo : ISerializationCallbackReceiver
{
  [SerializeField]
  public string _baseInfo;
  [NonSerialized]
  public System.Type _type;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    if (!System.Type.op_Inequality(this._type, (System.Type) null))
      return;
    this._baseInfo = this._type.FullName;
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    if (this._baseInfo == null)
      return;
    this._type = fsTypeCache.GetType(this._baseInfo, (System.Type) null);
  }

  public SerializedTypeInfo()
  {
  }

  public SerializedTypeInfo(System.Type info) => this._type = info;

  public System.Type Get() => this._type;
}
