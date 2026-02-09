// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedNodesHelper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

#nullable disable
namespace FlowCanvas.Nodes;

public static class ReflectedNodesHelper
{
  public const string RETURN_VALUE_NAME = "Value";

  public static ParamDef GetGetterDefFromInfo(MemberInfo info)
  {
    ParamDef getterDefFromInfo = new ParamDef()
    {
      presentedInfo = info,
      paramMode = ParamMode.Undefined
    };
    if (MemberInfo.op_Inequality(info, (MemberInfo) null))
    {
      getterDefFromInfo.paramMode = ParamMode.Out;
      MethodInfo methodInfo = info as MethodInfo;
      if (MethodInfo.op_Inequality(methodInfo, (MethodInfo) null))
      {
        string str = methodInfo.Name;
        if (str.StartsWith("get_"))
          str = str.Substring("get_".Length);
        getterDefFromInfo.portName = str;
        getterDefFromInfo.paramType = methodInfo.ReturnType;
      }
      FieldInfo fieldInfo = info as FieldInfo;
      if (FieldInfo.op_Inequality(fieldInfo, (FieldInfo) null))
      {
        getterDefFromInfo.portName = fieldInfo.Name;
        getterDefFromInfo.paramType = fieldInfo.FieldType;
      }
    }
    return getterDefFromInfo;
  }

  public static ParamDef GetDefFromInfo(ParameterInfo info, bool last)
  {
    ParamDef defFromInfo = new ParamDef();
    if (info != null)
    {
      Type parameterType = info.ParameterType;
      bool flag = false;
      if (last && parameterType.RTIsArray())
        flag = info.IsDefined(typeof (ParamArrayAttribute), false);
      Type elementType = parameterType.RTGetElementType();
      if (flag)
        defFromInfo.arrayType = parameterType.GetEnumerableElementType();
      defFromInfo.isParamsArray = flag;
      Type type = !parameterType.RTIsByRef() || !Type.op_Inequality(elementType, (Type) null) ? parameterType : elementType;
      defFromInfo.paramType = type;
      defFromInfo.paramMode = !info.IsOut || !parameterType.RTIsByRef() ? (info.IsOut || !info.ParameterType.RTIsByRef() ? ParamMode.In : ParamMode.Ref) : ParamMode.Out;
      defFromInfo.portName = info.Name;
    }
    return defFromInfo;
  }

  public static string GetGeneratedKey(MemberInfo memberInfo)
  {
    if (!MemberInfo.op_Inequality(memberInfo, (MemberInfo) null))
      return "";
    return $"{memberInfo.DeclaringType.FullName} {memberInfo.MemberType.ToString()} {memberInfo?.ToString()}";
  }

  public static bool InitParams(
    Type targetType,
    bool isStatic,
    MemberInfo[] infos,
    out ParametresDef parametres)
  {
    parametres = new ParametresDef();
    if (Type.op_Equality(targetType, (Type) null))
      return false;
    parametres = new ParametresDef()
    {
      paramDefinitions = new List<ParamDef>()
    };
    if (!isStatic)
    {
      parametres.resultDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      };
      parametres.instanceDef = new ParamDef()
      {
        paramType = targetType,
        portName = targetType.FriendlyName(),
        portId = "Instance",
        paramMode = ParamMode.Instance
      };
    }
    for (int index = 0; index <= infos.Length - 1; ++index)
    {
      ParamDef getterDefFromInfo = ReflectedNodesHelper.GetGetterDefFromInfo(infos[index]);
      if (getterDefFromInfo.paramMode != ParamMode.Undefined)
        parametres.paramDefinitions.Add(getterDefFromInfo);
    }
    return true;
  }

  public static bool InitParams(
    ParameterInfo[] prms,
    Type returnType,
    ref ParametresDef parametres)
  {
    bool flag = false;
    for (int index = 0; index <= prms.Length - 1; ++index)
    {
      ParamDef defFromInfo = ReflectedNodesHelper.GetDefFromInfo(prms[index], index == prms.Length - 1);
      if (defFromInfo.portName == "Value" && !flag)
        flag = true;
      if (parametres.instanceDef.paramMode != ParamMode.Undefined && defFromInfo.portName == parametres.instanceDef.portName && (defFromInfo.paramMode == ParamMode.In || defFromInfo.paramMode == ParamMode.Ref || defFromInfo.paramMode == ParamMode.Out))
        defFromInfo.portId = defFromInfo.portName + " ";
      parametres.paramDefinitions.Add(defFromInfo);
    }
    if (Type.op_Inequality(returnType, typeof (void)))
    {
      parametres.resultDef.paramType = returnType;
      parametres.resultDef.portName = "Value";
      parametres.resultDef.portId = flag ? "*Value" : (string) null;
      parametres.resultDef.paramMode = ParamMode.Result;
    }
    return true;
  }

  public static bool InitParams(ConstructorInfo constructor, out ParametresDef parametres)
  {
    parametres = new ParametresDef()
    {
      paramDefinitions = new List<ParamDef>(),
      instanceDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      },
      resultDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      }
    };
    return !ConstructorInfo.op_Equality(constructor, (ConstructorInfo) null) && !constructor.ContainsGenericParameters && !constructor.IsGenericMethodDefinition && ReflectedNodesHelper.InitParams(constructor.GetParameters(), constructor.RTReflectedType(), ref parametres);
  }

  public static bool InitParams(MethodInfo method, out ParametresDef parametres)
  {
    parametres = new ParametresDef()
    {
      paramDefinitions = new List<ParamDef>(),
      instanceDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      },
      resultDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      }
    };
    if (MethodInfo.op_Equality(method, (MethodInfo) null) || method.ContainsGenericParameters || method.IsGenericMethodDefinition)
      return false;
    ParameterInfo[] parameters = method.GetParameters();
    Type returnType1 = method.ReturnType;
    if (!method.IsStatic)
    {
      parametres.instanceDef.paramType = method.DeclaringType;
      parametres.instanceDef.portName = method.DeclaringType.FriendlyName();
      parametres.instanceDef.paramMode = ParamMode.Instance;
    }
    Type returnType2 = returnType1;
    ref ParametresDef local = ref parametres;
    return ReflectedNodesHelper.InitParams(parameters, returnType2, ref local);
  }

  public static bool InitParams(FieldInfo field, out ParametresDef parametres)
  {
    parametres = new ParametresDef()
    {
      paramDefinitions = (List<ParamDef>) null,
      instanceDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      },
      resultDef = new ParamDef()
      {
        paramMode = ParamMode.Undefined
      }
    };
    if (FieldInfo.op_Equality(field, (FieldInfo) null) || field.FieldType.ContainsGenericParameters || field.FieldType.IsGenericTypeDefinition)
      return false;
    if (!field.IsStatic)
    {
      parametres.instanceDef.paramMode = ParamMode.Instance;
      parametres.instanceDef.paramType = field.DeclaringType;
      parametres.instanceDef.portName = field.DeclaringType.FriendlyName();
    }
    parametres.resultDef.paramMode = ParamMode.Result;
    parametres.resultDef.paramType = field.FieldType;
    parametres.resultDef.portName = "Value";
    return true;
  }

  public static object CreateObject(this Type type)
  {
    return Type.op_Equality(type, (Type) null) ? (object) null : FormatterServices.GetUninitializedObject(type);
  }
}
