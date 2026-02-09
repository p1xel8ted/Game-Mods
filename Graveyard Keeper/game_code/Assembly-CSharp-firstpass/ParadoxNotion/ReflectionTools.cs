// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.ReflectionTools
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace ParadoxNotion;

public static class ReflectionTools
{
  public const BindingFlags flagsEverything = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
  public static Assembly[] _loadedAssemblies;
  public static Dictionary<string, System.Type> typeMap = new Dictionary<string, System.Type>();
  public static System.Type[] _allTypes;
  public static Dictionary<string, string> op_FriendlyNamesLong = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    {
      "op_Equality",
      "Equal"
    },
    {
      "op_Inequality",
      "Not Equal"
    },
    {
      "op_GreaterThan",
      "Greater"
    },
    {
      "op_LessThan",
      "Less"
    },
    {
      "op_GreaterThanOrEqual",
      "Greater Or Equal"
    },
    {
      "op_LessThanOrEqual",
      "Less Or Equal"
    },
    {
      "op_Addition",
      "Add"
    },
    {
      "op_Subtraction",
      "Subtract"
    },
    {
      "op_Division",
      "Divide"
    },
    {
      "op_Multiply",
      "Multiply"
    },
    {
      "op_UnaryNegation",
      "Negate"
    },
    {
      "op_UnaryPlus",
      "Positive"
    },
    {
      "op_Increment",
      "Increment"
    },
    {
      "op_Decrement",
      "Decrement"
    },
    {
      "op_LogicalNot",
      "NOT"
    },
    {
      "op_OnesComplement",
      "Complements"
    },
    {
      "op_False",
      "FALSE"
    },
    {
      "op_True",
      "TRUE"
    },
    {
      "op_Modulus",
      "MOD"
    },
    {
      "op_BitwiseAnd",
      "AND"
    },
    {
      "op_BitwiseOR",
      "OR"
    },
    {
      "op_LeftShift",
      "Shift Left"
    },
    {
      "op_RightShift",
      "Shift Right"
    },
    {
      "op_ExclusiveOr",
      "XOR"
    },
    {
      "op_Implicit",
      "Convert"
    },
    {
      "op_Explicit",
      "Convert"
    }
  };
  public static Dictionary<string, string> op_FriendlyNamesShort = new Dictionary<string, string>((IEqualityComparer<string>) StringComparer.OrdinalIgnoreCase)
  {
    {
      "op_Equality",
      "="
    },
    {
      "op_Inequality",
      "≠"
    },
    {
      "op_GreaterThan",
      ">"
    },
    {
      "op_LessThan",
      "<"
    },
    {
      "op_GreaterThanOrEqual",
      "≥"
    },
    {
      "op_LessThanOrEqual",
      "≤"
    },
    {
      "op_Addition",
      "+"
    },
    {
      "op_Subtraction",
      "-"
    },
    {
      "op_Division",
      "÷"
    },
    {
      "op_Multiply",
      "×"
    },
    {
      "op_UnaryNegation",
      "Negate"
    },
    {
      "op_UnaryPlus",
      "Positive"
    },
    {
      "op_Increment",
      "++"
    },
    {
      "op_Decrement",
      "--"
    },
    {
      "op_LogicalNot",
      "NOT"
    },
    {
      "op_OnesComplement",
      "~"
    },
    {
      "op_False",
      "FALSE"
    },
    {
      "op_True",
      "TRUE"
    },
    {
      "op_Modulus",
      "MOD"
    },
    {
      "op_BitwiseAnd",
      "AND"
    },
    {
      "op_BitwiseOR",
      "OR"
    },
    {
      "op_LeftShift",
      "<<"
    },
    {
      "op_RightShift",
      ">>"
    },
    {
      "op_ExclusiveOr",
      "XOR"
    },
    {
      "op_Implicit",
      "Convert"
    },
    {
      "op_Explicit",
      "Convert"
    }
  };
  public const string METHOD_SPECIAL_NAME_GET = "get_";
  public const string METHOD_SPECIAL_NAME_SET = "set_";
  public const string METHOD_SPECIAL_NAME_ADD = "add_";
  public const string METHOD_SPECIAL_NAME_REMOVE = "remove_";
  public const string METHOD_SPECIAL_NAME_OP = "op_";
  public static Dictionary<MethodBase, string> cacheSignatures = new Dictionary<MethodBase, string>();
  public static Dictionary<System.Type, FieldInfo[]> _typeFields = new Dictionary<System.Type, FieldInfo[]>();
  public static Dictionary<System.Type, PropertyInfo[]> _typeProperties = new Dictionary<System.Type, PropertyInfo[]>();
  public static Dictionary<System.Type, MethodInfo[]> _typeMethods = new Dictionary<System.Type, MethodInfo[]>();
  public static Dictionary<System.Type, ConstructorInfo[]> _typeConstructors = new Dictionary<System.Type, ConstructorInfo[]>();
  public static Dictionary<System.Type, EventInfo[]> _typeEvents = new Dictionary<System.Type, EventInfo[]>();
  public static Dictionary<System.Type, List<MethodInfo>> _typeExtensions = new Dictionary<System.Type, List<MethodInfo>>();

  public static Assembly[] loadedAssemblies
  {
    get
    {
      if (ReflectionTools._loadedAssemblies == null)
        ReflectionTools._loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
      return ReflectionTools._loadedAssemblies;
    }
  }

  public static System.Type GetType(
    string typeFullName,
    bool fallbackNoNamespace = false,
    System.Type fallbackAssignable = null)
  {
    if (string.IsNullOrEmpty(typeFullName))
      return (System.Type) null;
    System.Type type = (System.Type) null;
    if (ReflectionTools.typeMap.TryGetValue(typeFullName, out type))
      return type;
    type = ReflectionTools.GetTypeDirect(typeFullName);
    if (System.Type.op_Inequality(type, (System.Type) null))
      return ReflectionTools.typeMap[typeFullName] = type;
    type = ReflectionTools.TryResolveGenericType(typeFullName, fallbackNoNamespace, fallbackAssignable);
    if (System.Type.op_Inequality(type, (System.Type) null))
      return ReflectionTools.typeMap[typeFullName] = type;
    type = ReflectionTools.TryResolveDeserializeFromAttribute(typeFullName);
    if (System.Type.op_Inequality(type, (System.Type) null))
      return ReflectionTools.typeMap[typeFullName] = type;
    if (fallbackNoNamespace)
    {
      type = ReflectionTools.TryResolveWithoutNamespace(typeFullName, fallbackAssignable);
      if (System.Type.op_Inequality(type, (System.Type) null))
        return ReflectionTools.typeMap[type.FullName] = type;
    }
    ParadoxNotion.Services.Logger.LogError((object) $"Type with name '{typeFullName}' could not be resolved.", "Type Request");
    return ReflectionTools.typeMap[typeFullName] = (System.Type) null;
  }

  public static System.Type GetTypeDirect(string typeFullName)
  {
    System.Type type1 = System.Type.GetType(typeFullName);
    if (System.Type.op_Inequality(type1, (System.Type) null))
      return type1;
    for (int index = 0; index < ReflectionTools.loadedAssemblies.Length; ++index)
    {
      Assembly loadedAssembly = ReflectionTools.loadedAssemblies[index];
      System.Type type2;
      try
      {
        type2 = loadedAssembly.GetType(typeFullName);
      }
      catch
      {
        continue;
      }
      if (System.Type.op_Inequality(type2, (System.Type) null))
        return type2;
    }
    return (System.Type) null;
  }

  public static System.Type TryResolveGenericType(
    string typeFullName,
    bool fallbackNoNamespace = false,
    System.Type fallbackAssignable = null)
  {
    if (typeFullName.Contains<char>('`'))
    {
      if (typeFullName.Contains<char>('['))
      {
        try
        {
          int num1 = typeFullName.IndexOf('`');
          System.Type type1 = ReflectionTools.GetType(typeFullName.Substring(0, num1 + 2), fallbackNoNamespace, fallbackAssignable);
          if (System.Type.op_Equality(type1, (System.Type) null))
            return (System.Type) null;
          int int32 = Convert.ToInt32(typeFullName.Substring(num1 + 1, 1));
          string[] array;
          if (typeFullName.Substring(num1 + 2, typeFullName.Length - num1 - 2).StartsWith("[["))
          {
            int startIndex = typeFullName.IndexOf("[[") + 2;
            int num2 = typeFullName.LastIndexOf("]]");
            array = ((IEnumerable<string>) typeFullName.Substring(startIndex, num2 - startIndex).Split(new string[1]
            {
              "],["
            }, int32, StringSplitOptions.RemoveEmptyEntries)).ToArray<string>();
          }
          else
          {
            int startIndex = typeFullName.IndexOf('[') + 1;
            int num3 = typeFullName.LastIndexOf(']');
            array = ((IEnumerable<string>) typeFullName.Substring(startIndex, num3 - startIndex).Split(new char[1]
            {
              ','
            }, int32, StringSplitOptions.RemoveEmptyEntries)).ToArray<string>();
          }
          System.Type[] typeArray = new System.Type[int32];
          for (int index = 0; index < array.Length; ++index)
          {
            string str = array[index];
            if (!str.Contains<char>('`') && str.Contains<char>(','))
              str = str.Substring(0, str.IndexOf(','));
            System.Type fallbackAssignable1 = (System.Type) null;
            if (fallbackNoNamespace)
            {
              System.Type[] parameterConstraints = type1.RTGetGenericArguments()[index].GetGenericParameterConstraints();
              fallbackAssignable1 = parameterConstraints.Length == 0 ? typeof (object) : parameterConstraints[0];
            }
            System.Type type2 = ReflectionTools.GetType(str, fallbackNoNamespace, fallbackAssignable1);
            if (System.Type.op_Equality(type2, (System.Type) null))
              return (System.Type) null;
            typeArray[index] = type2;
          }
          return type1.RTMakeGenericType(typeArray);
        }
        catch (Exception ex)
        {
          ParadoxNotion.Services.Logger.LogException(ex, "Type Request Bug");
          return (System.Type) null;
        }
      }
    }
    return (System.Type) null;
  }

  public static System.Type TryResolveDeserializeFromAttribute(string typeName)
  {
    foreach (System.Type allType in ReflectionTools.GetAllTypes())
    {
      DeserializeFromAttribute attribute = ReflectionTools.RTGetAttribute<DeserializeFromAttribute>(allType, false);
      if (attribute != null && ((IEnumerable<string>) attribute.previousTypeNames).Any<string>((Func<string, bool>) (n => n == typeName)))
        return allType;
    }
    return (System.Type) null;
  }

  public static System.Type TryResolveWithoutNamespace(string typeName, System.Type fallbackAssignable = null)
  {
    if (typeName.Contains<char>('`') && typeName.Contains<char>('['))
      return (System.Type) null;
    if (typeName.Contains<char>(','))
      typeName = typeName.Substring(0, typeName.IndexOf(','));
    if (typeName.Contains<char>('.'))
    {
      int startIndex = typeName.LastIndexOf('.') + 1;
      typeName = typeName.Substring(startIndex, typeName.Length - startIndex);
    }
    foreach (System.Type allType in ReflectionTools.GetAllTypes())
    {
      if (allType.Name == typeName && (System.Type.op_Equality(fallbackAssignable, (System.Type) null) || fallbackAssignable.RTIsAssignableFrom(allType)))
        return allType;
    }
    return (System.Type) null;
  }

  public static System.Type[] GetAllTypes()
  {
    if (ReflectionTools._allTypes != null)
      return ReflectionTools._allTypes;
    List<System.Type> typeList = new List<System.Type>();
    for (int index = 0; index < ReflectionTools.loadedAssemblies.Length; ++index)
    {
      Assembly loadedAssembly = ReflectionTools.loadedAssemblies[index];
      try
      {
        typeList.AddRange((IEnumerable<System.Type>) loadedAssembly.RTGetExportedTypes());
      }
      catch
      {
      }
    }
    return ReflectionTools._allTypes = typeList.ToArray();
  }

  public static System.Type[] RTGetExportedTypes(this Assembly asm) => asm.GetExportedTypes();

  public static string FriendlyName(this System.Type t, bool compileSafe = false)
  {
    if (System.Type.op_Equality(t, (System.Type) null))
      return (string) null;
    if (!compileSafe && t.IsByRef)
      t = t.GetElementType();
    if (!compileSafe && System.Type.op_Equality(t, typeof (UnityEngine.Object)))
      return "UnityObject";
    string str1 = compileSafe ? t.FullName : t.Name;
    if (!compileSafe)
    {
      if (str1 == "Single")
        str1 = "Float";
      if (str1 == "Single[]")
        str1 = "Float[]";
      if (str1 == "Int32")
        str1 = "Integer";
      if (str1 == "Int32[]")
        str1 = "Integer[]";
    }
    if (t.RTIsGenericParameter())
      str1 = "T";
    if (t.RTIsGenericType())
    {
      str1 = compileSafe ? $"{t.Namespace}.{t.Name}" : t.Name;
      System.Type[] genericArguments = t.RTGetGenericArguments();
      if (genericArguments.Length != 0)
      {
        string str2 = str1.Replace("`" + genericArguments.Length.ToString(), "") + (compileSafe ? "<" : " (");
        for (int index = 0; index < genericArguments.Length; ++index)
          str2 = str2 + (index == 0 ? "" : ", ") + genericArguments[index].FriendlyName(compileSafe);
        str1 = str2 + (compileSafe ? ">" : ")");
      }
    }
    return str1;
  }

  public static ReflectionTools.MethodType GetMethodSpecialType(this MethodBase method)
  {
    string name = method.Name;
    if (method.IsSpecialName)
    {
      if (name.StartsWith("get_") || name.StartsWith("set_"))
        return ReflectionTools.MethodType.PropertyAccessor;
      if (name.StartsWith("add_") || name.StartsWith("remove_"))
        return ReflectionTools.MethodType.Event;
      if (name.StartsWith("op_"))
        return ReflectionTools.MethodType.Operator;
    }
    return ReflectionTools.MethodType.Normal;
  }

  public static string FriendlyName(this MethodBase method)
  {
    ReflectionTools.MethodType specialNameType = ReflectionTools.MethodType.Normal;
    return method.FriendlyName(out specialNameType);
  }

  public static string FriendlyName(
    this MethodBase method,
    out ReflectionTools.MethodType specialNameType)
  {
    specialNameType = ReflectionTools.MethodType.Normal;
    string name = method.Name;
    if (method.IsSpecialName)
    {
      if (name.StartsWith("get_"))
      {
        string str = "Get " + name.Substring("get_".Length);
        specialNameType = ReflectionTools.MethodType.PropertyAccessor;
        return str;
      }
      if (name.StartsWith("set_"))
      {
        string str = "Set " + name.Substring("set_".Length);
        specialNameType = ReflectionTools.MethodType.PropertyAccessor;
        return str;
      }
      if (name.StartsWith("add_"))
      {
        string str = name.Substring("add_".Length) + " +=";
        specialNameType = ReflectionTools.MethodType.Event;
        return str;
      }
      if (name.StartsWith("remove_"))
      {
        string str = name.Substring("remove_".Length) + " -=";
        specialNameType = ReflectionTools.MethodType.Event;
        return str;
      }
      if (name.StartsWith("op_"))
      {
        ReflectionTools.op_FriendlyNamesLong.TryGetValue(name, out name);
        specialNameType = ReflectionTools.MethodType.Operator;
        return name;
      }
    }
    return name;
  }

  public static string SignatureName(this MethodBase method)
  {
    string str1;
    if (ReflectionTools.cacheSignatures.TryGetValue(method, out str1))
      return str1;
    ReflectionTools.MethodType specialNameType = ReflectionTools.MethodType.Normal;
    string str2 = method.FriendlyName(out specialNameType);
    ParameterInfo[] parameters = method.GetParameters();
    string str3 = !(method is ConstructorInfo) ? $"{(!method.IsStatic || specialNameType == ReflectionTools.MethodType.Operator ? (object) "" : (object) "static ")}{str2} (" : $"new {method.DeclaringType.FriendlyName()} (";
    for (int index = 0; index < parameters.Length; ++index)
    {
      ParameterInfo parameter = parameters[index];
      if (parameter.IsParams(parameters))
        str3 += "params ";
      str3 = str3 + (parameter.ParameterType.IsByRef ? (parameter.IsOut ? "out " : "ref ") : "") + parameter.ParameterType.FriendlyName() + (index < parameters.Length - 1 ? ", " : "");
    }
    string str4 = !(method is MethodInfo) ? str3 + ")" : $"{str3}) : {(method as MethodInfo).ReturnType.FriendlyName()}";
    return ReflectionTools.cacheSignatures[method] = str4;
  }

  public static System.Type RTReflectedType(this MemberInfo member)
  {
    return !System.Type.op_Inequality(member.ReflectedType, (System.Type) null) ? member.DeclaringType : member.ReflectedType;
  }

  public static bool RTIsAssignableFrom(this System.Type type, System.Type second)
  {
    return type.IsAssignableFrom(second);
  }

  public static bool RTIsAbstract(this System.Type type) => type.IsAbstract;

  public static bool RTIsValueType(this System.Type type) => type.IsValueType;

  public static bool RTIsArray(this System.Type type) => type.IsArray;

  public static bool RTIsInterface(this System.Type type) => type.IsInterface;

  public static bool RTIsSubclassOf(this System.Type type, System.Type other)
  {
    return type.IsSubclassOf(other);
  }

  public static bool RTIsGenericParameter(this System.Type type) => type.IsGenericParameter;

  public static bool RTIsGenericType(this System.Type type) => type.IsGenericType;

  public static MethodInfo RTGetGetMethod(this PropertyInfo prop) => prop.GetGetMethod();

  public static MethodInfo RTGetSetMethod(this PropertyInfo prop) => prop.GetSetMethod();

  public static FieldInfo RTGetField(this System.Type type, string name)
  {
    return type.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
  }

  public static PropertyInfo RTGetProperty(this System.Type type, string name)
  {
    return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
  }

  public static ConstructorInfo RTGetConstructor(this System.Type type)
  {
    return ((IEnumerable<ConstructorInfo>) type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (info => info.GetParameters().Length == 0));
  }

  public static ConstructorInfo RTGetConstructor(this System.Type type, System.Type[] paramTypes)
  {
    return type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, (Binder) null, paramTypes, (ParameterModifier[]) null);
  }

  public static MethodInfo RTGetMethod(this System.Type type, string name)
  {
    return type.GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
  }

  public static MethodInfo RTGetMethod(this System.Type type, string name, System.Type[] paramTypes)
  {
    return type.GetMethod(name, paramTypes);
  }

  public static EventInfo RTGetEvent(this System.Type type, string name)
  {
    return type.GetEvent(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
  }

  public static MethodInfo RTGetDelegateMethodInfo(this Delegate del) => del.Method;

  public static ParameterInfo[] RTGetDelegateTypeParameters(this System.Type delegateType)
  {
    return System.Type.op_Equality(delegateType, (System.Type) null) || !delegateType.RTIsSubclassOf(typeof (Delegate)) ? new ParameterInfo[0] : delegateType.RTGetMethod("Invoke").GetParameters();
  }

  public static FieldInfo[] RTGetFields(this System.Type type)
  {
    FieldInfo[] fields;
    if (!ReflectionTools._typeFields.TryGetValue(type, out fields))
    {
      fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      ReflectionTools._typeFields[type] = fields;
    }
    return fields;
  }

  public static PropertyInfo[] RTGetProperties(this System.Type type)
  {
    PropertyInfo[] properties;
    if (!ReflectionTools._typeProperties.TryGetValue(type, out properties))
    {
      properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      ReflectionTools._typeProperties[type] = properties;
    }
    return properties;
  }

  public static MethodInfo[] RTGetMethods(this System.Type type)
  {
    MethodInfo[] methods;
    if (!ReflectionTools._typeMethods.TryGetValue(type, out methods))
    {
      methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      ReflectionTools._typeMethods[type] = methods;
    }
    return methods;
  }

  public static ConstructorInfo[] RTGetConstructors(this System.Type type)
  {
    ConstructorInfo[] constructors;
    if (!ReflectionTools._typeConstructors.TryGetValue(type, out constructors))
    {
      constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      ReflectionTools._typeConstructors[type] = constructors;
    }
    return constructors;
  }

  public static EventInfo[] RTGetEvents(this System.Type type)
  {
    EventInfo[] events;
    if (!ReflectionTools._typeEvents.TryGetValue(type, out events))
    {
      events = type.GetEvents(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
      ReflectionTools._typeEvents[type] = events;
    }
    return events;
  }

  public static bool RTIsDefined<T>(this System.Type type, bool inherited) where T : Attribute
  {
    return type.IsDefined(typeof (T), inherited);
  }

  public static bool RTIsDefined<T>(this MemberInfo member, bool inherited) where T : Attribute
  {
    return member.IsDefined(typeof (T), inherited);
  }

  public static T RTGetAttribute<T>(this System.Type type, bool inherited) where T : Attribute
  {
    return (T) ((IEnumerable<object>) type.GetCustomAttributes(typeof (T), inherited)).FirstOrDefault<object>();
  }

  public static T RTGetAttribute<T>(this MemberInfo member, bool inherited) where T : Attribute
  {
    return (T) ((IEnumerable<object>) member.GetCustomAttributes(typeof (T), inherited)).FirstOrDefault<object>();
  }

  public static T[] RTGetAttributesRecursive<T>(this System.Type type) where T : Attribute
  {
    List<T> objList = new List<T>();
    for (System.Type type1 = type; System.Type.op_Inequality(type1, (System.Type) null); type1 = type1.BaseType)
    {
      T attribute = ReflectionTools.RTGetAttribute<T>(type1, false);
      if ((object) attribute != null)
        objList.Add(attribute);
    }
    return objList.ToArray();
  }

  public static T[] RTGetAttributes<T>(this System.Type type, bool inherited) where T : Attribute
  {
    return (T[]) type.GetCustomAttributes(typeof (T), inherited);
  }

  public static T[] RTGetAttributes<T>(this MemberInfo member, bool inherited) where T : Attribute
  {
    return (T[]) member.GetCustomAttributes(typeof (T), inherited);
  }

  public static System.Type RTMakeGenericType(this System.Type type, params System.Type[] typeArgs)
  {
    return type.MakeGenericType(typeArgs);
  }

  public static System.Type[] RTGetGenericArguments(this System.Type type)
  {
    return type.GetGenericArguments();
  }

  public static System.Type[] RTGetEmptyTypes() => System.Type.EmptyTypes;

  public static System.Type RTGetElementType(this System.Type type)
  {
    return System.Type.op_Equality(type, (System.Type) null) ? (System.Type) null : type.GetElementType();
  }

  public static bool RTIsByRef(this System.Type type)
  {
    return !System.Type.op_Equality(type, (System.Type) null) && type.IsByRef;
  }

  public static T RTCreateDelegate<T>(this MethodInfo method, object instance)
  {
    return (T) method.RTCreateDelegate(typeof (T), instance);
  }

  public static Delegate RTCreateDelegate(this MethodInfo method, System.Type type, object instance)
  {
    if (instance != null)
    {
      System.Type type1 = instance.GetType();
      if (System.Type.op_Inequality(method.DeclaringType, type1))
        method = instance.GetType().RTGetMethod(method.Name, ((IEnumerable<ParameterInfo>) method.GetParameters()).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType)).ToArray<System.Type>());
    }
    return Delegate.CreateDelegate(type, instance, method);
  }

  public static Delegate ConvertDelegate(Delegate originalDelegate, System.Type targetDelegateType)
  {
    return Delegate.CreateDelegate(targetDelegateType, originalDelegate.Target, originalDelegate.Method);
  }

  public static bool IsObsolete(this MemberInfo member, bool inherited = true)
  {
    if (member is MethodInfo)
    {
      MethodInfo method = (MethodInfo) member;
      if (method.IsPropertyAccessor())
        member = (MemberInfo) method.GetAccessorProperty();
    }
    return member.RTIsDefined<ObsoleteAttribute>(inherited);
  }

  public static bool IsReadOnly(this FieldInfo field) => field.IsInitOnly || field.IsLiteral;

  public static bool IsConstant(this FieldInfo field) => field.IsReadOnly() && field.IsStatic;

  public static bool IsStatic(this EventInfo info)
  {
    MethodInfo addMethod = info.GetAddMethod();
    return MethodInfo.op_Inequality(addMethod, (MethodInfo) null) && addMethod.IsStatic;
  }

  public static bool IsParams(this ParameterInfo parameter, ParameterInfo[] parameters)
  {
    return parameter.Position == parameters.Length - 1 && parameter.IsDefined(typeof (ParamArrayAttribute), false);
  }

  public static PropertyInfo GetBaseDefinition(this PropertyInfo propertyInfo)
  {
    MethodInfo methodInfo = ((IEnumerable<MethodInfo>) propertyInfo.GetAccessors(true)).FirstOrDefault<MethodInfo>();
    if (MethodInfo.op_Equality(methodInfo, (MethodInfo) null))
      return (PropertyInfo) null;
    MethodInfo baseDefinition = methodInfo.GetBaseDefinition();
    if (MethodInfo.op_Equality(baseDefinition, methodInfo))
      return propertyInfo;
    System.Type[] array = ((IEnumerable<ParameterInfo>) propertyInfo.GetIndexParameters()).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType)).ToArray<System.Type>();
    return baseDefinition.DeclaringType.GetProperty(propertyInfo.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy, (Binder) null, propertyInfo.PropertyType, array, (ParameterModifier[]) null);
  }

  public static FieldInfo GetBaseDefinition(this FieldInfo fieldInfo)
  {
    return fieldInfo.DeclaringType.RTGetField(fieldInfo.Name);
  }

  public static MethodInfo[] GetExtensionMethods(this System.Type targetType)
  {
    List<MethodInfo> methodInfoList1 = (List<MethodInfo>) null;
    if (ReflectionTools._typeExtensions.TryGetValue(targetType, out methodInfoList1))
      return methodInfoList1.ToArray();
    List<MethodInfo> methodInfoList2 = new List<MethodInfo>();
    foreach (System.Type allType in ReflectionTools.GetAllTypes())
    {
      if (allType.IsSealed && !allType.IsGenericType && ReflectionTools.RTIsDefined<ExtensionAttribute>(allType, false))
      {
        foreach (MethodInfo method in allType.RTGetMethods())
        {
          if (method.IsExtensionMethod() && method.GetParameters()[0].ParameterType.RTIsAssignableFrom(targetType))
            methodInfoList2.Add(method);
        }
      }
    }
    ReflectionTools._typeExtensions[targetType] = methodInfoList2;
    return methodInfoList2.ToArray();
  }

  public static bool IsExtensionMethod(this MethodInfo method)
  {
    return method.RTIsDefined<ExtensionAttribute>(false);
  }

  public static bool IsPropertyAccessor(this MethodInfo method)
  {
    return method.GetMethodSpecialType() == ReflectionTools.MethodType.PropertyAccessor;
  }

  public static bool IsIndexerProperty(this PropertyInfo property)
  {
    return property.GetIndexParameters().Length != 0;
  }

  public static PropertyInfo GetAccessorProperty(this MethodInfo method)
  {
    return method.IsPropertyAccessor() ? method.RTReflectedType().RTGetProperty(method.Name.Substring(4)) : (PropertyInfo) null;
  }

  public static bool IsEnumerableCollection(this System.Type type)
  {
    if (System.Type.op_Equality(type, (System.Type) null) || !typeof (IEnumerable).RTIsAssignableFrom(type))
      return false;
    return type.RTIsGenericType() || type.RTIsArray();
  }

  public static System.Type GetEnumerableElementType(this System.Type type)
  {
    if (System.Type.op_Equality(type, (System.Type) null))
      return (System.Type) null;
    if (!typeof (IEnumerable).RTIsAssignableFrom(type))
      return (System.Type) null;
    if (type.RTIsArray())
      return type.GetElementType();
    if (type.RTIsGenericType())
    {
      System.Type[] genericArguments = type.RTGetGenericArguments();
      if (genericArguments.Length == 1)
        return genericArguments[0];
      if (genericArguments.Length == 2)
        return genericArguments[1];
    }
    return (System.Type) null;
  }

  public static System.Type GetFirstGenericParameterConstraintType(this System.Type type)
  {
    if (System.Type.op_Equality(type, (System.Type) null) || !type.RTIsGenericType())
      return (System.Type) null;
    type = type.GetGenericTypeDefinition();
    System.Type type1 = ((IEnumerable<System.Type>) ((IEnumerable<System.Type>) type.GetGenericArguments()).First<System.Type>().GetGenericParameterConstraints()).FirstOrDefault<System.Type>();
    return !System.Type.op_Inequality(type1, (System.Type) null) ? typeof (object) : type1;
  }

  public static System.Type GetFirstGenericParameterConstraintType(this MethodInfo method)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null) || !method.IsGenericMethod)
      return (System.Type) null;
    method = method.GetGenericMethodDefinition();
    System.Type type = ((IEnumerable<System.Type>) ((IEnumerable<System.Type>) method.GetGenericArguments()).First<System.Type>().GetGenericParameterConstraints()).FirstOrDefault<System.Type>();
    return !System.Type.op_Inequality(type, (System.Type) null) ? typeof (object) : type;
  }

  public static bool CanBeMadeGenericWith(this MethodInfo def, System.Type type)
  {
    return !MethodInfo.op_Equality(def, (MethodInfo) null) && def.IsGenericMethod && type.IsAllowedByGenericArgument(((IEnumerable<System.Type>) def.GetGenericMethodDefinition().GetGenericArguments()).FirstOrDefault<System.Type>());
  }

  public static bool CanBeMadeGenericWith(this System.Type def, System.Type type)
  {
    return !System.Type.op_Equality(def, (System.Type) null) && def.RTIsGenericType() && type.IsAllowedByGenericArgument(((IEnumerable<System.Type>) def.GetGenericTypeDefinition().GetGenericArguments()).FirstOrDefault<System.Type>());
  }

  public static bool IsAllowedByGenericArgument(this System.Type type, System.Type genericArgument)
  {
    if (System.Type.op_Equality(type, (System.Type) null) || System.Type.op_Equality(genericArgument, (System.Type) null))
      return false;
    System.Type[] parameterConstraints = genericArgument.GetGenericParameterConstraints();
    GenericParameterAttributes parameterAttributes = genericArgument.GenericParameterAttributes;
    if (parameterConstraints.Length == 0 || parameterConstraints.Length == 1 && ((IEnumerable<System.Type>) parameterConstraints).First<System.Type>().RTIsAssignableFrom(type))
      return true;
    bool flag = true;
    for (int index = 0; index <= parameterConstraints.Length - 1; ++index)
    {
      System.Type type1 = parameterConstraints[index];
      if (!System.Type.op_Equality(type1, typeof (ValueType)))
      {
        if (flag)
          flag &= type1.RTIsAssignableFrom(type);
        else
          break;
      }
    }
    if (flag && (parameterAttributes & GenericParameterAttributes.DefaultConstructorConstraint) == GenericParameterAttributes.DefaultConstructorConstraint && (parameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != GenericParameterAttributes.NotNullableValueTypeConstraint && ConstructorInfo.op_Equality(((IEnumerable<ConstructorInfo>) type.RTGetConstructors()).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (info => info.IsPublic && info.GetParameters().Length == 0)), (ConstructorInfo) null))
      flag = false;
    if (flag && (parameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) == GenericParameterAttributes.ReferenceTypeConstraint && type.RTIsValueType())
      flag = false;
    if (flag && (parameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) == GenericParameterAttributes.NotNullableValueTypeConstraint && !type.RTIsValueType())
      flag = false;
    return flag;
  }

  public static bool CanConvert(System.Type fromType, System.Type toType, out UnaryExpression exp)
  {
    try
    {
      exp = Expression.Convert((Expression) Expression.Parameter(fromType, (string) null), toType);
      return true;
    }
    catch
    {
      exp = (UnaryExpression) null;
      return false;
    }
  }

  public enum MethodType
  {
    Normal,
    PropertyAccessor,
    Event,
    Operator,
  }
}
