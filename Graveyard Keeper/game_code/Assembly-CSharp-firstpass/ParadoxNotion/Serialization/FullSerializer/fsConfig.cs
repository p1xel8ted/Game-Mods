// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsConfig
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public class fsConfig
{
  public System.Type[] SerializeAttributes = new System.Type[2]
  {
    typeof (SerializeField),
    typeof (fsPropertyAttribute)
  };
  public System.Type[] IgnoreSerializeAttributes = new System.Type[2]
  {
    typeof (NonSerializedAttribute),
    typeof (fsIgnoreAttribute)
  };
  public fsMemberSerialization DefaultMemberSerialization = fsMemberSerialization.Default;
  public Func<string, MemberInfo, string> GetJsonNameFromMemberName = (Func<string, MemberInfo, string>) ((name, info) => name);
  public bool SerializeNonAutoProperties;
  public bool SerializeNonPublicSetProperties;
  public string CustomDateTimeFormatString;
  public bool Serialize64BitIntegerAsString;
  public bool SerializeEnumsAsInteger;
}
