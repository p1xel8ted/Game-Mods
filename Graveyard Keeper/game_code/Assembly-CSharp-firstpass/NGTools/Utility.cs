// Decompiled with JetBrains decompiler
// Type: NGTools.Utility
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

#nullable disable
namespace NGTools;

public static class Utility
{
  public const BindingFlags ExposedBindingFlags = BindingFlags.Instance | BindingFlags.Public;
  public static ByteBuffer sharedBBuffer = new ByteBuffer(128 /*0x80*/);
  public static StringBuilder sharedBuffer = new StringBuilder(128 /*0x80*/);
  public static Dictionary<Assembly, System.Type[]> assemblyTypes = new Dictionary<Assembly, System.Type[]>();
  public static List<ICollectionModifier> cachedCollectionModifiers = new List<ICollectionModifier>();

  public static IEnumerable<System.Type> EachAssignableFrom(System.Type baseType, Func<System.Type, bool> match = null)
  {
    System.Type[] types;
    if (!Utility.assemblyTypes.TryGetValue(baseType.Assembly(), out types))
    {
      types = baseType.Assembly().GetTypes();
      Utility.assemblyTypes[baseType.Assembly()] = types;
    }
    for (int i = 0; i < types.Length; ++i)
    {
      if (baseType.IsAssignableFrom(types[i]) && System.Type.op_Inequality(types[i], baseType) && (match == null || match(types[i])))
        yield return types[i];
    }
  }

  public static IEnumerable<System.Type> EachSubClassesOf(System.Type baseType, Func<System.Type, bool> match = null)
  {
    System.Type[] types;
    if (!Utility.assemblyTypes.TryGetValue(baseType.Assembly(), out types))
    {
      types = baseType.Assembly().GetTypes();
      Utility.assemblyTypes[baseType.Assembly()] = types;
    }
    for (int i = 0; i < types.Length; ++i)
    {
      if (types[i].IsSubclassOf(baseType) && (match == null || match(types[i])))
        yield return types[i];
    }
  }

  public static bool CanExposeTypeInInspector(System.Type type)
  {
    if (!type.IsInterface() && (type.IsPrimitive() || System.Type.op_Equality(type, typeof (string)) || type.IsEnum() || System.Type.op_Equality(type, typeof (Rect)) || System.Type.op_Equality(type, typeof (Vector3)) || System.Type.op_Equality(type, typeof (Color)) || typeof (UnityEngine.Object).IsAssignableFrom(type) || System.Type.op_Equality(type, typeof (UnityEngine.Object)) || System.Type.op_Equality(type, typeof (Vector2)) || System.Type.op_Equality(type, typeof (Vector4)) || System.Type.op_Equality(type, typeof (Quaternion)) || System.Type.op_Equality(type, typeof (AnimationCurve))))
      return true;
    if (System.Type.op_Inequality(type.GetInterface(typeof (IList<>).Name), (System.Type) null))
    {
      System.Type genericArgument = type.GetInterface(typeof (IList<>).Name).GetGenericArguments()[0];
      return System.Type.op_Equality(genericArgument.GetInterface(typeof (IList<>).Name), (System.Type) null) && Utility.CanExposeTypeInInspector(genericArgument);
    }
    return !typeof (IList).IsAssignableFrom(type) && (!type.IsGenericType() || !System.Type.op_Equality(type.GetGenericTypeDefinition(), typeof (Dictionary<,>))) && System.Type.op_Inequality(type, typeof (Decimal)) && (type.IsClass() || type.IsStruct()) && type.GetCustomAttributes(typeof (SerializableAttribute), true).Length != 0;
  }

  public static System.Type GetArraySubType(System.Type arrayType)
  {
    if (arrayType.IsArray)
      return arrayType.GetElementType();
    System.Type type = arrayType.GetInterface(typeof (IList<>).Name);
    return System.Type.op_Inequality(type, (System.Type) null) ? type.GetGenericArguments()[0] : (System.Type) null;
  }

  public static List<FieldInfo> GetFieldsHierarchyOrdered(
    System.Type t,
    System.Type stopType,
    BindingFlags flags)
  {
    Stack<System.Type> typeStack = new Stack<System.Type>();
    List<FieldInfo> hierarchyOrdered = new List<FieldInfo>();
    typeStack.Push(t);
    for (; System.Type.op_Inequality(t.BaseType(), stopType); t = t.BaseType())
      typeStack.Push(t.BaseType());
    foreach (System.Type type in typeStack)
      hierarchyOrdered.AddRange((IEnumerable<FieldInfo>) type.GetFields(flags | BindingFlags.DeclaredOnly));
    return hierarchyOrdered;
  }

  public static string ClipBoard
  {
    get => GUIUtility.systemCopyBuffer;
    set => GUIUtility.systemCopyBuffer = value;
  }

  public static float RelativeAngle(Vector3 fwd, Vector3 targetDir, Vector3 upDir)
  {
    float num = Vector3.Angle(fwd, targetDir);
    return (double) Utility.AngleDirection(fwd, targetDir, upDir) == -1.0 ? -num : num;
  }

  public static float RelativeAngle(Vector2 fwd, Vector2 targetDir, Vector3 upDir)
  {
    float num = Vector2.Angle(fwd, targetDir);
    return (double) Utility.AngleDirection(fwd, targetDir, upDir) == -1.0 ? -num : num;
  }

  public static float AngleDirection(Vector3 fwd, Vector3 targetDir, Vector3 up)
  {
    float num = Vector3.Dot(Vector3.Cross(fwd, targetDir), up);
    if ((double) num > 0.0)
      return 1f;
    return (double) num < 0.0 ? -1f : 0.0f;
  }

  public static float AngleDirection(Vector2 fwd, Vector2 targetDir, Vector3 up)
  {
    float num = Vector3.Dot(Vector3.Cross(new Vector3(fwd.x, 0.0f, fwd.y), new Vector3(targetDir.x, 0.0f, targetDir.y)), up);
    if ((double) num > 0.0)
      return 1f;
    return (double) num < 0.0 ? -1f : 0.0f;
  }

  public static bool IsUnityArray(this System.Type t)
  {
    return t.IsArray || typeof (IList).IsAssignableFrom(t);
  }

  public static bool IsStruct(this System.Type t)
  {
    return t.IsValueType() && !t.IsPrimitive() && !t.IsEnum() && System.Type.op_Inequality(t, typeof (Decimal));
  }

  public static string GetShortAssemblyType(this System.Type t)
  {
    return !t.Assembly().FullName.StartsWith("mscorlib") ? $"{t.FullName},{t.Assembly().FullName.Substring(0, t.Assembly().FullName.IndexOf(','))}" : t.FullName;
  }

  public static IFieldModifier GetFieldInfo(System.Type type, string name)
  {
    FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    if (FieldInfo.op_Inequality(field, (FieldInfo) null))
      return (IFieldModifier) new FieldModifier(field);
    PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
    return PropertyInfo.op_Inequality(property, (PropertyInfo) null) ? (IFieldModifier) new PropertyModifier(property) : throw new MissingFieldException($"Field or property \"{name}\" was not found in type \"{type.Name}\".");
  }

  public static ICollectionModifier GetCollectionModifier(object rawArray)
  {
    switch (rawArray)
    {
      case Array array:
        for (int index = 0; index < Utility.cachedCollectionModifiers.Count; ++index)
        {
          if (Utility.cachedCollectionModifiers[index] is ArrayModifier collectionModifier)
          {
            collectionModifier.array = array;
            Utility.cachedCollectionModifiers.RemoveAt(index);
            return (ICollectionModifier) collectionModifier;
          }
        }
        return (ICollectionModifier) new ArrayModifier(array);
      case IList list:
        for (int index = 0; index < Utility.cachedCollectionModifiers.Count; ++index)
        {
          if (Utility.cachedCollectionModifiers[index] is ListModifier collectionModifier)
          {
            collectionModifier.list = list;
            Utility.cachedCollectionModifiers.RemoveAt(index);
            return (ICollectionModifier) collectionModifier;
          }
        }
        return (ICollectionModifier) new ListModifier(list);
      default:
        throw new Exception($"Collection of type \"{rawArray.GetType()?.ToString()}\" is not supported.");
    }
  }

  public static void ReturnCollectionModifier(ICollectionModifier modifier)
  {
    Utility.cachedCollectionModifiers.Add(modifier);
  }

  public static bool IsComponentEnableable(Component component)
  {
    System.Type type = component.GetType();
    return component is Behaviour ? MethodInfo.op_Inequality(type.GetMethod("Start", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (MethodInfo) null) || MethodInfo.op_Inequality(type.GetMethod("Update", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (MethodInfo) null) || MethodInfo.op_Inequality(type.GetMethod("FixedUpdate", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (MethodInfo) null) || MethodInfo.op_Inequality(type.GetMethod("OnGUI", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (MethodInfo) null) : PropertyInfo.op_Inequality(type.GetProperty("enabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (PropertyInfo) null);
  }
}
