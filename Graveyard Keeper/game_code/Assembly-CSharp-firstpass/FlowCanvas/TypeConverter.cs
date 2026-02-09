// Decompiled with JetBrains decompiler
// Type: FlowCanvas.TypeConverter
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

#nullable disable
namespace FlowCanvas;

public static class TypeConverter
{
  [SpoofAOT]
  public static ValueHandler<T> GetConverterFuncFromTo<T>(
    System.Type sourceType,
    System.Type targetType,
    ValueHandler<object> func)
  {
    if (targetType.RTIsAssignableFrom(sourceType) || targetType.RTIsSubclassOf(sourceType))
      return (ValueHandler<T>) (() => (T) func());
    if (typeof (IConvertible).RTIsAssignableFrom(targetType) && typeof (IConvertible).RTIsAssignableFrom(sourceType))
      return (ValueHandler<T>) (() => (T) Convert.ChangeType(func(), targetType));
    UnaryExpression exp = (UnaryExpression) null;
    if (ReflectionTools.CanConvert(sourceType, targetType, out exp))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) exp.Method.Invoke((object) null, new object[1]
          {
            func()
          });
        }
        catch
        {
          return default (T);
        }
      });
    if (System.Type.op_Equality(targetType, typeof (string)))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) func().ToString();
        }
        catch
        {
          return default (T);
        }
      });
    if (System.Type.op_Equality(targetType, typeof (System.Type)))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) func().GetType();
        }
        catch
        {
          return default (T);
        }
      });
    if (System.Type.op_Equality(targetType, typeof (Vector3)) && typeof (IConvertible).RTIsAssignableFrom(sourceType))
      return (ValueHandler<T>) (() =>
      {
        double num = (double) (float) Convert.ChangeType(func(), typeof (float));
        return (T) (ValueType) new Vector3((float) num, (float) num, (float) num);
      });
    if (System.Type.op_Equality(targetType, typeof (Vector3)) && typeof (Component).RTIsAssignableFrom(sourceType))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) (ValueType) (func() as Component).transform.position;
        }
        catch
        {
          return default (T);
        }
      });
    if (System.Type.op_Equality(targetType, typeof (Vector3)) && System.Type.op_Equality(sourceType, typeof (GameObject)))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) (ValueType) (func() as GameObject).transform.position;
        }
        catch
        {
          return default (T);
        }
      });
    if (typeof (Component).RTIsAssignableFrom(targetType) && typeof (Component).RTIsAssignableFrom(sourceType))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) (func() as Component).GetComponent(targetType);
        }
        catch
        {
          return default (T);
        }
      });
    if (typeof (Component).RTIsAssignableFrom(targetType) && System.Type.op_Equality(sourceType, typeof (GameObject)))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) (func() as GameObject).GetComponent(targetType);
        }
        catch
        {
          return default (T);
        }
      });
    if (System.Type.op_Equality(targetType, typeof (GameObject)) && typeof (Component).RTIsAssignableFrom(sourceType))
      return (ValueHandler<T>) (() =>
      {
        try
        {
          return (T) (func() as Component).gameObject;
        }
        catch
        {
          return default (T);
        }
      });
    if (typeof (IEnumerable).RTIsAssignableFrom(sourceType))
    {
      if (typeof (IEnumerable).RTIsAssignableFrom(targetType))
      {
        try
        {
          System.Type second = sourceType.RTIsArray() ? sourceType.GetElementType() : ((IEnumerable<System.Type>) sourceType.GetGenericArguments()).Single<System.Type>();
          System.Type type = targetType.RTIsArray() ? targetType.GetElementType() : ((IEnumerable<System.Type>) targetType.GetGenericArguments()).Single<System.Type>();
          if (type.RTIsAssignableFrom(second))
          {
            System.Type listType = typeof (List<>).RTMakeGenericType(type);
            return (ValueHandler<T>) (() =>
            {
              IList instance = (IList) Activator.CreateInstance(listType);
              foreach (object obj in (IEnumerable) func())
                instance.Add(obj);
              return (T) instance;
            });
          }
        }
        catch
        {
          return (ValueHandler<T>) null;
        }
      }
    }
    return (ValueHandler<T>) null;
  }

  public static bool HasConvertion(System.Type sourceType, System.Type targetType)
  {
    return (!System.Type.op_Equality(sourceType, typeof (Flow)) || !System.Type.op_Inequality(sourceType, targetType)) && TypeConverter.GetConverterFuncFromTo<object>(sourceType, targetType, (ValueHandler<object>) null) != null;
  }

  public static T QuickConvert<T>(object obj) => (T) TypeConverter.QuickConvert(obj, typeof (T));

  public static object QuickConvert(object obj, System.Type type)
  {
    return obj == null || System.Type.op_Equality(type, (System.Type) null) ? (object) null : TypeConverter.GetConverterFuncFromTo<object>(obj.GetType(), type, (ValueHandler<object>) (() => obj))();
  }
}
