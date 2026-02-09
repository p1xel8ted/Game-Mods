// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.SerializedMethodInfo
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
public class SerializedMethodInfo : SerializedMethodBaseInfo
{
  [SerializeField]
  public string _baseInfo;
  [SerializeField]
  public string _paramsInfo;
  [SerializeField]
  public string _genericArgumentsInfo;
  [NonSerialized]
  public MethodInfo _method;
  [NonSerialized]
  public bool _hasChanged;

  public override void OnBeforeSerialize()
  {
    this._hasChanged = false;
    if (!MethodInfo.op_Inequality(this._method, (MethodInfo) null))
      return;
    this._baseInfo = $"{this._method.DeclaringType.FullName}|{this._method.Name}|{this._method.ReturnType.FullName}";
    this._paramsInfo = string.Join("|", ((IEnumerable<ParameterInfo>) this._method.GetParameters()).Select<ParameterInfo, string>((Func<ParameterInfo, string>) (p => p.ParameterType.FullName)).ToArray<string>());
    this._genericArgumentsInfo = this._method.IsGenericMethod ? string.Join("|", ((IEnumerable<System.Type>) this._method.GetGenericArguments()).Select<System.Type, string>((Func<System.Type, string>) (a => a.FullName)).ToArray<string>()) : (string) null;
  }

  public override void OnAfterDeserialize()
  {
    this._hasChanged = false;
    if (this._baseInfo == null)
      return;
    string[] strArray1 = this._baseInfo.Split('|');
    System.Type type1 = fsTypeCache.GetType(strArray1[0], (System.Type) null);
    if (System.Type.op_Equality(type1, (System.Type) null))
    {
      this._method = (MethodInfo) null;
    }
    else
    {
      string name = strArray1[1];
      string[] strArray2;
      if (!string.IsNullOrEmpty(this._paramsInfo))
        strArray2 = this._paramsInfo.Split('|');
      else
        strArray2 = (string[]) null;
      string[] source = strArray2;
      System.Type[] parameterTypes = source == null ? new System.Type[0] : ((IEnumerable<string>) source).Select<string, System.Type>((Func<string, System.Type>) (n => fsTypeCache.GetType(n, (System.Type) null))).ToArray<System.Type>();
      if (((IEnumerable<System.Type>) parameterTypes).All<System.Type>((Func<System.Type, bool>) (t => System.Type.op_Inequality(t, (System.Type) null))))
      {
        if (!string.IsNullOrEmpty(this._genericArgumentsInfo))
        {
          System.Type[] genericArgumentTypes = ((IEnumerable<string>) this._genericArgumentsInfo.Split('|')).Select<string, System.Type>((Func<string, System.Type>) (x => fsTypeCache.GetType(x, (System.Type) null))).ToArray<System.Type>();
          this._method = ((IEnumerable<MethodInfo>) type1.RTGetMethods()).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => m.IsGenericMethod && m.Name == name && m.GetParameters().Length == parameterTypes.Length && ((IEnumerable<ParameterInfo>) m.MakeGenericMethod(genericArgumentTypes).GetParameters()).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType)).SequenceEqual<System.Type>((IEnumerable<System.Type>) parameterTypes)));
          if (MethodInfo.op_Inequality(this._method, (MethodInfo) null))
            this._method = this._method.MakeGenericMethod(genericArgumentTypes);
        }
        else
        {
          this._method = type1.RTGetMethod(name, parameterTypes);
          if (strArray1.Length >= 3)
          {
            System.Type type2 = fsTypeCache.GetType(strArray1[2], (System.Type) null);
            if (MethodInfo.op_Inequality(this._method, (MethodInfo) null) && System.Type.op_Inequality(type2, this._method.ReturnType))
              this._method = (MethodInfo) null;
          }
        }
      }
      if (!MethodInfo.op_Equality(this._method, (MethodInfo) null))
        return;
      this._hasChanged = true;
      this._method = ((IEnumerable<MethodInfo>) type1.RTGetMethods()).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => m.Name == name));
      if (MethodInfo.op_Equality(this._method, (MethodInfo) null))
      {
        Debug.LogError((object) ("method is null for " + name));
      }
      else
      {
        if (!this._method.IsGenericMethodDefinition)
          return;
        System.Type type3 = ((IEnumerable<System.Type>) ((IEnumerable<System.Type>) this._method.GetGenericArguments()).First<System.Type>().GetGenericParameterConstraints()).FirstOrDefault<System.Type>();
        this._method = this._method.MakeGenericMethod(System.Type.op_Inequality(type3, (System.Type) null) ? type3 : typeof (object));
      }
    }
  }

  public SerializedMethodInfo()
  {
  }

  public SerializedMethodInfo(MethodInfo method)
  {
    this._hasChanged = false;
    this._method = method;
  }

  public MethodInfo Get() => this._method;

  public override MethodBase GetBase() => (MethodBase) this.Get();

  public override bool HasChanged() => this._hasChanged;

  public override string GetMethodString()
  {
    return $"{this._baseInfo.Replace("|", ".")} ({this._paramsInfo.Replace("|", ", ")})";
  }
}
