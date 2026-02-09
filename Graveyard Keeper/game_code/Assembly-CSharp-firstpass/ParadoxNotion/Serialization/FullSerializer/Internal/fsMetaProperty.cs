// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsMetaProperty
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public class fsMetaProperty
{
  public MemberInfo _memberInfo;
  [CompilerGenerated]
  public Type \u003CStorageType\u003Ek__BackingField;
  [CompilerGenerated]
  public Type \u003COverrideConverterType\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCanRead\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCanWrite\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CJsonName\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CMemberName\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CIsPublic\u003Ek__BackingField;

  public fsMetaProperty(fsConfig config, FieldInfo field)
  {
    this._memberInfo = (MemberInfo) field;
    this.StorageType = field.FieldType;
    this.MemberName = field.Name;
    this.IsPublic = field.IsPublic;
    this.CanRead = true;
    this.CanWrite = true;
    this.CommonInitialize(config);
  }

  public fsMetaProperty(fsConfig config, PropertyInfo property)
  {
    this._memberInfo = (MemberInfo) property;
    this.StorageType = property.PropertyType;
    this.MemberName = property.Name;
    this.IsPublic = MethodInfo.op_Inequality(property.GetGetMethod(), (MethodInfo) null) && property.GetGetMethod().IsPublic && MethodInfo.op_Inequality(property.GetSetMethod(), (MethodInfo) null) && property.GetSetMethod().IsPublic;
    this.CanRead = property.CanRead;
    this.CanWrite = property.CanWrite;
    this.CommonInitialize(config);
  }

  public void CommonInitialize(fsConfig config)
  {
    fsPropertyAttribute attribute = fsPortableReflection.GetAttribute<fsPropertyAttribute>(this._memberInfo);
    if (attribute != null)
    {
      this.JsonName = attribute.Name;
      this.OverrideConverterType = attribute.Converter;
    }
    if (!string.IsNullOrEmpty(this.JsonName))
      return;
    this.JsonName = config.GetJsonNameFromMemberName(this.MemberName, this._memberInfo);
  }

  public Type StorageType
  {
    get => this.\u003CStorageType\u003Ek__BackingField;
    set => this.\u003CStorageType\u003Ek__BackingField = value;
  }

  public Type OverrideConverterType
  {
    get => this.\u003COverrideConverterType\u003Ek__BackingField;
    set => this.\u003COverrideConverterType\u003Ek__BackingField = value;
  }

  public bool CanRead
  {
    get => this.\u003CCanRead\u003Ek__BackingField;
    set => this.\u003CCanRead\u003Ek__BackingField = value;
  }

  public bool CanWrite
  {
    get => this.\u003CCanWrite\u003Ek__BackingField;
    set => this.\u003CCanWrite\u003Ek__BackingField = value;
  }

  public string JsonName
  {
    get => this.\u003CJsonName\u003Ek__BackingField;
    set => this.\u003CJsonName\u003Ek__BackingField = value;
  }

  public string MemberName
  {
    get => this.\u003CMemberName\u003Ek__BackingField;
    set => this.\u003CMemberName\u003Ek__BackingField = value;
  }

  public bool IsPublic
  {
    get => this.\u003CIsPublic\u003Ek__BackingField;
    set => this.\u003CIsPublic\u003Ek__BackingField = value;
  }

  public void Write(object context, object value)
  {
    FieldInfo memberInfo1 = this._memberInfo as FieldInfo;
    PropertyInfo memberInfo2 = this._memberInfo as PropertyInfo;
    if (FieldInfo.op_Inequality(memberInfo1, (FieldInfo) null))
    {
      memberInfo1.SetValue(context, value);
    }
    else
    {
      if (!PropertyInfo.op_Inequality(memberInfo2, (PropertyInfo) null))
        return;
      MethodInfo setMethod = memberInfo2.GetSetMethod(true);
      if (!MethodInfo.op_Inequality(setMethod, (MethodInfo) null))
        return;
      setMethod.Invoke(context, new object[1]{ value });
    }
  }

  public object Read(object context)
  {
    return this._memberInfo is PropertyInfo ? ((PropertyInfo) this._memberInfo).GetValue(context, new object[0]) : ((FieldInfo) this._memberInfo).GetValue(context);
  }
}
