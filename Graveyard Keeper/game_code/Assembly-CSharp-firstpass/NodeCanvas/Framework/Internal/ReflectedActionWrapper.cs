// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedActionWrapper
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

public abstract class ReflectedActionWrapper : ReflectedWrapper
{
  public static ReflectedActionWrapper Create(MethodInfo method, IBlackboard bb)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
      return (ReflectedActionWrapper) null;
    Type type = (Type) null;
    ParameterInfo[] parameters = method.GetParameters();
    if (parameters.Length == 0)
      type = typeof (ReflectedAction);
    if (parameters.Length == 1)
      type = typeof (ReflectedAction<>);
    if (parameters.Length == 2)
      type = typeof (ReflectedAction<,>);
    if (parameters.Length == 3)
      type = typeof (ReflectedAction<,,>);
    if (parameters.Length == 4)
      type = typeof (ReflectedAction<,,,>);
    if (parameters.Length == 5)
      type = typeof (ReflectedAction<,,,,>);
    if (parameters.Length == 6)
      type = typeof (ReflectedAction<,,,,,>);
    Type[] array = ((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).ToArray<Type>();
    ReflectedActionWrapper instance = (ReflectedActionWrapper) Activator.CreateInstance(array.Length != 0 ? type.RTMakeGenericType(array) : type);
    instance._targetMethod = new SerializedMethodInfo(method);
    BBParameter.SetBBFields((object) instance, bb);
    BBParameter[] variables = instance.GetVariables();
    for (int index = 0; index < parameters.Length; ++index)
    {
      ParameterInfo parameterInfo = parameters[index];
      if (parameterInfo.IsOptional)
        variables[index].value = parameterInfo.DefaultValue;
    }
    return instance;
  }

  public abstract void Call();
}
