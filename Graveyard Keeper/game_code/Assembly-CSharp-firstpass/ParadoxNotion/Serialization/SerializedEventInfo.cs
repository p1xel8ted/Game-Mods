// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.SerializedEventInfo
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
public class SerializedEventInfo : ISerializationCallbackReceiver
{
  [SerializeField]
  public string _baseInfo;
  [NonSerialized]
  public EventInfo _event;

  void ISerializationCallbackReceiver.OnBeforeSerialize()
  {
    if (!EventInfo.op_Inequality(this._event, (EventInfo) null))
      return;
    this._baseInfo = $"{this._event.DeclaringType.FullName}|{this._event.Name}";
  }

  void ISerializationCallbackReceiver.OnAfterDeserialize()
  {
    if (this._baseInfo == null)
      return;
    string[] strArray = this._baseInfo.Split('|');
    System.Type type = fsTypeCache.GetType(strArray[0], (System.Type) null);
    if (System.Type.op_Equality(type, (System.Type) null))
    {
      this._event = (EventInfo) null;
    }
    else
    {
      string name = strArray[1];
      this._event = type.RTGetEvent(name);
    }
  }

  public SerializedEventInfo()
  {
  }

  public SerializedEventInfo(EventInfo info) => this._event = info;

  public EventInfo Get() => this._event;
}
