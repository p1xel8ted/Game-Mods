// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedFunctionWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

#nullable disable
namespace NodeCanvas.Framework.Internal;

public abstract class ReflectedFunctionWrapper : ReflectedWrapper
{
  public static ReflectedFunctionWrapper Create(MethodInfo method, IBlackboard bb)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
      return (ReflectedFunctionWrapper) null;
    Type type = (Type) null;
    List<Type> typeList = new List<Type>()
    {
      method.ReturnType
    };
    ParameterInfo[] parameters = method.GetParameters();
    if (parameters.Length == 0)
      type = typeof (ReflectedFunction<>);
    if (parameters.Length == 1)
      type = typeof (ReflectedFunction<,>);
    if (parameters.Length == 2)
      type = typeof (ReflectedFunction<,,>);
    if (parameters.Length == 3)
      type = typeof (ReflectedFunction<,,,>);
    if (parameters.Length == 4)
      type = typeof (ReflectedFunction<,,,,>);
    if (parameters.Length == 5)
      type = typeof (ReflectedFunction<,,,,,>);
    if (parameters.Length == 6)
      type = typeof (ReflectedFunction<,,,,,,>);
    typeList.AddRange(((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)));
    ReflectedFunctionWrapper instance = (ReflectedFunctionWrapper) Activator.CreateInstance(type.RTMakeGenericType(typeList.ToArray()));
    instance._targetMethod = new SerializedMethodInfo(method);
    BBParameter.SetBBFields((object) instance, bb);
    BBParameter[] variables = instance.GetVariables();
    for (int index = 0; index < parameters.Length; ++index)
    {
      ParameterInfo parameterInfo = parameters[index];
      if (parameterInfo.IsOptional)
        variables[index + 1].value = parameterInfo.DefaultValue;
    }
    return instance;
  }

  public abstract object Call();
}
