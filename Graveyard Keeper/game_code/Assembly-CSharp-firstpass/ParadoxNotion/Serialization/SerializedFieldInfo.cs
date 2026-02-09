// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.SerializedFieldInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Serialization.FullSerializer.Internal;
using System;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization;

[Serializable]
public class SerializedFieldInfo : ISerializationCallbackReceiver
{
  [SerializeField]
  public string _baseInfo;
  [NonSerialized]
  public FieldInfo _field;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    if (!FieldInfo.op_Inequality(this._field, (FieldInfo) null))
      return;
    this._baseInfo = $"{this._field.DeclaringType.FullName}|{this._field.Name}";
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    if (this._baseInfo == null)
      return;
    string[] strArray = this._baseInfo.Split('|');
    System.Type type = fsTypeCache.GetType(strArray[0], (System.Type) null);
    if (System.Type.op_Equality(type, (System.Type) null))
    {
      this._field = (FieldInfo) null;
    }
    else
    {
      string name = strArray[1];
      this._field = type.RTGetField(name);
    }
  }

  public SerializedFieldInfo()
  {
  }

  public SerializedFieldInfo(FieldInfo info) => this._field = info;

  public FieldInfo Get() => this._field;
}
