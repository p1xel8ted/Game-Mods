// Decompiled with JetBrains decompiler
// Type: Unity.Cloud.UserReporting.Plugin.SimpleJson.Reflection.ReflectionUtils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Unity.Cloud.UserReporting.Plugin.SimpleJson.Reflection;

[GeneratedCode("reflection-utils", "1.0.0")]
public class ReflectionUtils
{
  public static object[] EmptyObjects = new object[0];

  public static Type GetTypeInfo(Type type) => type;

  public static Attribute GetAttribute(MemberInfo info, Type type)
  {
    return info == (MemberInfo) null || type == (Type) null || !Attribute.IsDefined(info, type) ? (Attribute) null : Attribute.GetCustomAttribute(info, type);
  }

  public static Type GetGenericListElementType(Type type)
  {
    foreach (Type type1 in (IEnumerable<Type>) type.GetInterfaces())
    {
      if (ReflectionUtils.IsTypeGeneric(type1) && type1.GetGenericTypeDefinition() == typeof (IList<>))
        return ReflectionUtils.GetGenericTypeArguments(type1)[0];
    }
    return ReflectionUtils.GetGenericTypeArguments(type)[0];
  }

  public static Attribute GetAttribute(Type objectType, Type attributeType)
  {
    return objectType == (Type) null || attributeType == (Type) null || !Attribute.IsDefined((MemberInfo) objectType, attributeType) ? (Attribute) null : Attribute.GetCustomAttribute((MemberInfo) objectType, attributeType);
  }

  public static Type[] GetGenericTypeArguments(Type type) => type.GetGenericArguments();

  public static bool IsTypeGeneric(Type type) => ReflectionUtils.GetTypeInfo(type).IsGenericType;

  public static bool IsTypeGenericeCollectionInterface(Type type)
  {
    if (!ReflectionUtils.IsTypeGeneric(type))
      return false;
    Type genericTypeDefinition = type.GetGenericTypeDefinition();
    return genericTypeDefinition == typeof (IList<>) || genericTypeDefinition == typeof (ICollection<>) || genericTypeDefinition == typeof (IEnumerable<>);
  }

  public static bool IsAssignableFrom(Type type1, Type type2)
  {
    return ReflectionUtils.GetTypeInfo(type1).IsAssignableFrom(ReflectionUtils.GetTypeInfo(type2));
  }

  public static bool IsTypeDictionary(Type type)
  {
    if (typeof (IDictionary).IsAssignableFrom(type))
      return true;
    return ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof (IDictionary<,>);
  }

  public static bool IsNullableType(Type type)
  {
    return ReflectionUtils.GetTypeInfo(type).IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>);
  }

  public static object ToNullableType(object obj, Type nullableType)
  {
    return obj != null ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), (IFormatProvider) CultureInfo.InvariantCulture) : (object) null;
  }

  public static bool IsValueType(Type type) => ReflectionUtils.GetTypeInfo(type).IsValueType;

  public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
  {
    return (IEnumerable<ConstructorInfo>) type.GetConstructors();
  }

  public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
  {
    foreach (ConstructorInfo constructor in ReflectionUtils.GetConstructors(type))
    {
      ParameterInfo[] parameters = constructor.GetParameters();
      if (argsType.Length == parameters.Length)
      {
        int index = 0;
        bool flag = true;
        foreach (ParameterInfo parameter in constructor.GetParameters())
        {
          if (parameter.ParameterType != argsType[index])
          {
            flag = false;
            break;
          }
        }
        if (flag)
          return constructor;
      }
    }
    return (ConstructorInfo) null;
  }

  public static IEnumerable<PropertyInfo> GetProperties(Type type)
  {
    return (IEnumerable<PropertyInfo>) type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
  }

  public static IEnumerable<FieldInfo> GetFields(Type type)
  {
    return (IEnumerable<FieldInfo>) type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
  }

  public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
  {
    return propertyInfo.GetGetMethod(true);
  }

  public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
  {
    return propertyInfo.GetSetMethod(true);
  }

  public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
  {
    return ReflectionUtils.GetConstructorByReflection(constructorInfo);
  }

  public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
  {
    return ReflectionUtils.GetConstructorByReflection(type, argsType);
  }

  public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(
    ConstructorInfo constructorInfo)
  {
    return (ReflectionUtils.ConstructorDelegate) (args => constructorInfo.Invoke(args));
  }

  public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(
    Type type,
    params Type[] argsType)
  {
    ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
    return !(constructorInfo == (ConstructorInfo) null) ? ReflectionUtils.GetConstructorByReflection(constructorInfo) : (ReflectionUtils.ConstructorDelegate) null;
  }

  public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
  {
    return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
  }

  public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
  {
    return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
  }

  public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
  {
    MethodInfo methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
    return (ReflectionUtils.GetDelegate) (source => methodInfo.Invoke(source, ReflectionUtils.EmptyObjects));
  }

  public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
  {
    return (ReflectionUtils.GetDelegate) (source => fieldInfo.GetValue(source));
  }

  public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
  {
    return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
  }

  public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
  {
    return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
  }

  public static ReflectionUtils.SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
  {
    MethodInfo methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
    return (ReflectionUtils.SetDelegate) ((source, value) => methodInfo.Invoke(source, new object[1]
    {
      value
    }));
  }

  public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
  {
    return (ReflectionUtils.SetDelegate) ((source, value) => fieldInfo.SetValue(source, value));
  }

  public delegate object GetDelegate(object source);

  public delegate void SetDelegate(object source, object value);

  public delegate object ConstructorDelegate(params object[] args);

  public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

  public sealed class ThreadSafeDictionary<TKey, TValue> : 
    IDictionary<TKey, TValue>,
    ICollection<KeyValuePair<TKey, TValue>>,
    IEnumerable<KeyValuePair<TKey, TValue>>,
    IEnumerable
  {
    public Dictionary<TKey, TValue> _dictionary;
    public object _lock = new object();
    public ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;

    public ThreadSafeDictionary(
      ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
    {
      this._valueFactory = valueFactory;
    }

    public int Count => this._dictionary.Count;

    public bool IsReadOnly => throw new NotImplementedException();

    public TValue this[TKey key]
    {
      get => this.Get(key);
      set => throw new NotImplementedException();
    }

    public ICollection<TKey> Keys => (ICollection<TKey>) this._dictionary.Keys;

    public ICollection<TValue> Values => (ICollection<TValue>) this._dictionary.Values;

    public void Add(TKey key, TValue value) => throw new NotImplementedException();

    public void Add(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

    public TValue AddValue(TKey key)
    {
      TValue obj1 = this._valueFactory(key);
      lock (this._lock)
      {
        if (this._dictionary == null)
        {
          this._dictionary = new Dictionary<TKey, TValue>();
          this._dictionary[key] = obj1;
        }
        else
        {
          TValue obj2;
          if (this._dictionary.TryGetValue(key, out obj2))
            return obj2;
          this._dictionary = new Dictionary<TKey, TValue>((IDictionary<TKey, TValue>) this._dictionary)
          {
            [key] = obj1
          };
        }
      }
      return obj1;
    }

    public void Clear() => throw new NotImplementedException();

    public bool Contains(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

    public bool ContainsKey(TKey key) => this._dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
      throw new NotImplementedException();
    }

    public TValue Get(TKey key)
    {
      TValue obj;
      return this._dictionary == null || !this._dictionary.TryGetValue(key, out obj) ? this.AddValue(key) : obj;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
      return (IEnumerator<KeyValuePair<TKey, TValue>>) this._dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._dictionary.GetEnumerator();

    public bool Remove(TKey key) => throw new NotImplementedException();

    public bool Remove(KeyValuePair<TKey, TValue> item) => throw new NotImplementedException();

    public bool TryGetValue(TKey key, out TValue value)
    {
      value = this[key];
      return true;
    }
  }
}
