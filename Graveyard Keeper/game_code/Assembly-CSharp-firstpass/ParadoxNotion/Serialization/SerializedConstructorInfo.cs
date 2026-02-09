// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.SerializedConstructorInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Serialization.FullSerializer.Internal;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace ParadoxNotion.Serialization;

[Serializable]
public class SerializedConstructorInfo : SerializedMethodBaseInfo
{
  [SerializeField]
  public string _baseInfo;
  [SerializeField]
  public string _paramsInfo;
  [NonSerialized]
  public ConstructorInfo _constructor;
  [NonSerialized]
  public bool _hasChanged;

  public override void OnBeforeSerialize()
  {
    this._hasChanged = false;
    if (!ConstructorInfo.op_Inequality(this._constructor, (ConstructorInfo) null))
      return;
    this._baseInfo = this._constructor.DeclaringType.FullName + "|$Constructor";
    this._paramsInfo = string.Join("|", ((IEnumerable<ParameterInfo>) this._constructor.GetParameters()).Select<ParameterInfo, string>((Func<ParameterInfo, string>) (p => p.ParameterType.FullName)).ToArray<string>());
  }

  public override void OnAfterDeserialize()
  {
    this._hasChanged = false;
    System.Type type = fsTypeCache.GetType(this._baseInfo.Split('|')[0], (System.Type) null);
    if (System.Type.op_Equality(type, (System.Type) null))
    {
      this._constructor = (ConstructorInfo) null;
    }
    else
    {
      string[] strArray;
      if (!string.IsNullOrEmpty(this._paramsInfo))
        strArray = this._paramsInfo.Split('|');
      else
        strArray = (string[]) null;
      string[] source = strArray;
      System.Type[] typeArray = source == null ? new System.Type[0] : ((IEnumerable<string>) source).Select<string, System.Type>((Func<string, System.Type>) (n => fsTypeCache.GetType(n, (System.Type) null))).ToArray<System.Type>();
      if (((IEnumerable<System.Type>) typeArray).All<System.Type>((Func<System.Type, bool>) (t => System.Type.op_Inequality(t, (System.Type) null))))
        this._constructor = type.RTGetConstructor(typeArray);
      if (!ConstructorInfo.op_Equality(this._constructor, (ConstructorInfo) null))
        return;
      this._hasChanged = true;
      this._constructor = ((IEnumerable<ConstructorInfo>) type.RTGetConstructors()).FirstOrDefault<ConstructorInfo>();
    }
  }

  public SerializedConstructorInfo()
  {
  }

  public SerializedConstructorInfo(ConstructorInfo constructor)
  {
    this._hasChanged = false;
    this._constructor = constructor;
  }

  public ConstructorInfo Get() => this._constructor;

  public override MethodBase GetBase() => (MethodBase) this.Get();

  public override bool HasChanged() => this._hasChanged;

  public override string GetMethodString()
  {
    return $"{this._baseInfo.Replace("|", ".")} ({this._paramsInfo.Replace("|", ", ")})";
  }
}
