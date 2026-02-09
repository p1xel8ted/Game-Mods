// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsPortableReflection
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public static class fsPortableReflection
{
  public static Type[] EmptyTypes = new Type[0];
  public static IDictionary<fsPortableReflection.AttributeQuery, Attribute> _cachedAttributeQueries = (IDictionary<fsPortableReflection.AttributeQuery, Attribute>) new Dictionary<fsPortableReflection.AttributeQuery, Attribute>((IEqualityComparer<fsPortableReflection.AttributeQuery>) new fsPortableReflection.AttributeQueryComparator());
  public static BindingFlags DeclaredFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

  public static bool HasAttribute(MemberInfo element, Type attributeType)
  {
    return fsPortableReflection.GetAttribute(element, attributeType, true) != null;
  }

  public static bool HasAttribute<TAttribute>(MemberInfo element)
  {
    return fsPortableReflection.HasAttribute(element, typeof (TAttribute));
  }

  public static Attribute GetAttribute(MemberInfo element, Type attributeType, bool shouldCache)
  {
    fsPortableReflection.AttributeQuery key = new fsPortableReflection.AttributeQuery()
    {
      MemberInfo = element,
      AttributeType = attributeType
    };
    Attribute attribute;
    if (!fsPortableReflection._cachedAttributeQueries.TryGetValue(key, out attribute))
    {
      attribute = (Attribute) ((IEnumerable<object>) element.GetCustomAttributes(attributeType, true)).FirstOrDefault<object>();
      if (shouldCache)
        fsPortableReflection._cachedAttributeQueries[key] = attribute;
    }
    return attribute;
  }

  public static TAttribute GetAttribute<TAttribute>(MemberInfo element, bool shouldCache) where TAttribute : Attribute
  {
    return (TAttribute) fsPortableReflection.GetAttribute(element, typeof (TAttribute), shouldCache);
  }

  public static TAttribute GetAttribute<TAttribute>(MemberInfo element) where TAttribute : Attribute
  {
    return fsPortableReflection.GetAttribute<TAttribute>(element, true);
  }

  public static PropertyInfo GetDeclaredProperty(this Type type, string propertyName)
  {
    PropertyInfo[] declaredProperties = type.GetDeclaredProperties();
    for (int index = 0; index < declaredProperties.Length; ++index)
    {
      if (declaredProperties[index].Name == propertyName)
        return declaredProperties[index];
    }
    return (PropertyInfo) null;
  }

  public static MethodInfo GetDeclaredMethod(this Type type, string methodName)
  {
    MethodInfo[] declaredMethods = type.GetDeclaredMethods();
    for (int index = 0; index < declaredMethods.Length; ++index)
    {
      if (declaredMethods[index].Name == methodName)
        return declaredMethods[index];
    }
    return (MethodInfo) null;
  }

  public static ConstructorInfo GetDeclaredConstructor(this Type type, Type[] parameters)
  {
    foreach (ConstructorInfo declaredConstructor in type.GetDeclaredConstructors())
    {
      ParameterInfo[] parameters1 = declaredConstructor.GetParameters();
      if (parameters.Length == parameters1.Length)
      {
        for (int index = 0; index < parameters1.Length; ++index)
          Type.op_Inequality(parameters1[index].ParameterType, parameters[index]);
        return declaredConstructor;
      }
    }
    return (ConstructorInfo) null;
  }

  public static ConstructorInfo[] GetDeclaredConstructors(this Type type)
  {
    return type.GetConstructors(fsPortableReflection.DeclaredFlags);
  }

  public static MemberInfo[] GetFlattenedMember(this Type type, string memberName)
  {
    List<MemberInfo> memberInfoList = new List<MemberInfo>();
    for (; Type.op_Inequality(type, (Type) null); type = type.Resolve().BaseType)
    {
      MemberInfo[] declaredMembers = type.GetDeclaredMembers();
      for (int index = 0; index < declaredMembers.Length; ++index)
      {
        if (declaredMembers[index].Name == memberName)
          memberInfoList.Add(declaredMembers[index]);
      }
    }
    return memberInfoList.ToArray();
  }

  public static MethodInfo GetFlattenedMethod(this Type type, string methodName)
  {
    for (; Type.op_Inequality(type, (Type) null); type = type.Resolve().BaseType)
    {
      MethodInfo[] declaredMethods = type.GetDeclaredMethods();
      for (int index = 0; index < declaredMethods.Length; ++index)
      {
        if (declaredMethods[index].Name == methodName)
          return declaredMethods[index];
      }
    }
    return (MethodInfo) null;
  }

  public static IEnumerable<MethodInfo> GetFlattenedMethods(this Type type, string methodName)
  {
    while (Type.op_Inequality(type, (Type) null))
    {
      MethodInfo[] methods = type.GetDeclaredMethods();
      for (int i = 0; i < methods.Length; ++i)
      {
        if (methods[i].Name == methodName)
          yield return methods[i];
      }
      type = type.Resolve().BaseType;
      methods = (MethodInfo[]) null;
    }
  }

  public static PropertyInfo GetFlattenedProperty(this Type type, string propertyName)
  {
    for (; Type.op_Inequality(type, (Type) null); type = type.Resolve().BaseType)
    {
      PropertyInfo[] declaredProperties = type.GetDeclaredProperties();
      for (int index = 0; index < declaredProperties.Length; ++index)
      {
        if (declaredProperties[index].Name == propertyName)
          return declaredProperties[index];
      }
    }
    return (PropertyInfo) null;
  }

  public static MemberInfo GetDeclaredMember(this Type type, string memberName)
  {
    MemberInfo[] declaredMembers = type.GetDeclaredMembers();
    for (int index = 0; index < declaredMembers.Length; ++index)
    {
      if (declaredMembers[index].Name == memberName)
        return declaredMembers[index];
    }
    return (MemberInfo) null;
  }

  public static MethodInfo[] GetDeclaredMethods(this Type type)
  {
    return type.GetMethods(fsPortableReflection.DeclaredFlags);
  }

  public static PropertyInfo[] GetDeclaredProperties(this Type type)
  {
    return type.GetProperties(fsPortableReflection.DeclaredFlags);
  }

  public static FieldInfo[] GetDeclaredFields(this Type type)
  {
    return type.GetFields(fsPortableReflection.DeclaredFlags);
  }

  public static MemberInfo[] GetDeclaredMembers(this Type type)
  {
    return type.GetMembers(fsPortableReflection.DeclaredFlags);
  }

  public static MemberInfo AsMemberInfo(Type type) => (MemberInfo) type;

  public static bool IsType(MemberInfo member) => member is Type;

  public static Type AsType(MemberInfo member) => (Type) member;

  public static Type Resolve(this Type type) => type;

  public struct AttributeQuery
  {
    public MemberInfo MemberInfo;
    public Type AttributeType;
  }

  public class AttributeQueryComparator : IEqualityComparer<fsPortableReflection.AttributeQuery>
  {
    public bool Equals(fsPortableReflection.AttributeQuery x, fsPortableReflection.AttributeQuery y)
    {
      return MemberInfo.op_Equality(x.MemberInfo, y.MemberInfo) && Type.op_Equality(x.AttributeType, y.AttributeType);
    }

    public int GetHashCode(fsPortableReflection.AttributeQuery obj)
    {
      return obj.MemberInfo.GetHashCode() + 17 * obj.AttributeType.GetHashCode();
    }
  }
}
