// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.Legacy.ReflectedMethodNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes.Legacy;

public abstract class ReflectedMethodNode
{
  public static ReflectedMethodNode Create(MethodInfo method)
  {
    ParameterInfo[] parameters = method.GetParameters();
    if (!method.DeclaringType.RTIsValueType())
    {
      if (!((IEnumerable<ParameterInfo>) parameters).Any<ParameterInfo>((Func<ParameterInfo, bool>) (p => p.ParameterType.IsByRef || p.IsParams(parameters))))
      {
        try
        {
          return ReflectedMethodNode.TryCreateJit(method);
        }
        catch
        {
          return (ReflectedMethodNode) new PureReflectedMethodNode();
        }
      }
    }
    return (ReflectedMethodNode) new PureReflectedMethodNode();
  }

  public static ReflectedMethodNode TryCreateJit(MethodInfo method)
  {
    if (System.Type.op_Equality(method.ReturnType, typeof (void)))
    {
      System.Type type = (System.Type) null;
      List<System.Type> typeList = new List<System.Type>();
      ParameterInfo[] parameters = method.GetParameters();
      int length = parameters.Length;
      if (!method.IsStatic)
      {
        ++length;
        typeList.Add(method.DeclaringType);
      }
      if (length == 0)
        type = typeof (ReflectedActionNode);
      if (length == 1)
        type = typeof (ReflectedActionNode<>);
      if (length == 2)
        type = typeof (ReflectedActionNode<,>);
      if (length == 3)
        type = typeof (ReflectedActionNode<,,>);
      if (length == 4)
        type = typeof (ReflectedActionNode<,,,>);
      if (length == 5)
        type = typeof (ReflectedActionNode<,,,,>);
      if (length == 6)
        type = typeof (ReflectedActionNode<,,,,,>);
      if (length == 7)
        type = typeof (ReflectedActionNode<,,,,,,>);
      if (length == 8)
        type = typeof (ReflectedActionNode<,,,,,,,>);
      if (length >= 9)
      {
        Debug.LogError((object) "ReflectedActionNode currently supports up to 8 parameters");
        return (ReflectedMethodNode) null;
      }
      typeList.AddRange(((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType)));
      return (ReflectedMethodNode) Activator.CreateInstance(typeList.Count > 0 ? type.RTMakeGenericType(typeList.ToArray()) : type);
    }
    System.Type type1 = (System.Type) null;
    List<System.Type> typeList1 = new List<System.Type>();
    ParameterInfo[] parameters1 = method.GetParameters();
    int length1 = parameters1.Length;
    if (!method.IsStatic)
    {
      ++length1;
      typeList1.Add(method.DeclaringType);
    }
    if (length1 == 0)
      type1 = typeof (ReflectedFunctionNode<>);
    if (length1 == 1)
      type1 = typeof (ReflectedFunctionNode<,>);
    if (length1 == 2)
      type1 = typeof (ReflectedFunctionNode<,,>);
    if (length1 == 3)
      type1 = typeof (ReflectedFunctionNode<,,,>);
    if (length1 == 4)
      type1 = typeof (ReflectedFunctionNode<,,,,>);
    if (length1 == 5)
      type1 = typeof (ReflectedFunctionNode<,,,,,>);
    if (length1 == 6)
      type1 = typeof (ReflectedFunctionNode<,,,,,,>);
    if (length1 == 7)
      type1 = typeof (ReflectedFunctionNode<,,,,,,,>);
    if (length1 == 8)
      type1 = typeof (ReflectedFunctionNode<,,,,,,,,>);
    if (length1 >= 9)
    {
      Debug.LogError((object) "ReflectedFunctionNode currently supports up to 8 parameters");
      return (ReflectedMethodNode) null;
    }
    typeList1.AddRange(((IEnumerable<ParameterInfo>) parameters1).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType)));
    typeList1.Add(method.ReturnType);
    return (ReflectedMethodNode) Activator.CreateInstance(type1.RTMakeGenericType(typeList1.ToArray()));
  }

  public string GetName(MethodInfo method, int i)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
      return (string) null;
    ParameterInfo[] parameters = method.GetParameters();
    if (method.IsStatic)
      return parameters[i].Name;
    string name1 = method.DeclaringType.FriendlyName();
    if (i == 0)
      return name1;
    string name2 = parameters[i - 1].Name;
    return !(name2 != name1) ? name2 + " " : name2;
  }

  public abstract void RegisterPorts(
    FlowNode node,
    MethodInfo method,
    ReflectedMethodRegistrationOptions options);

  public delegate void ActionCall();

  public delegate void ActionCall<T1>(T1 a);

  public delegate void ActionCall<T1, T2>(T1 a, T2 b);

  public delegate void ActionCall<T1, T2, T3>(T1 a, T2 b, T3 c);

  public delegate void ActionCall<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d);

  public delegate void ActionCall<T1, T2, T3, T4, T5>(T1 a, T2 b, T3 c, T4 d, T5 e);

  public delegate void ActionCall<T1, T2, T3, T4, T5, T6>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f);

  public delegate void ActionCall<T1, T2, T3, T4, T5, T6, T7>(
    T1 a,
    T2 b,
    T3 c,
    T4 d,
    T5 e,
    T6 f,
    T7 g);

  public delegate void ActionCall<T1, T2, T3, T4, T5, T6, T7, T8>(
    T1 a,
    T2 b,
    T3 c,
    T4 d,
    T5 e,
    T6 f,
    T7 g,
    T8 h);

  public delegate TResult FunctionCall<TResult>();

  public delegate TResult FunctionCall<T1, TResult>(T1 a);

  public delegate TResult FunctionCall<T1, T2, TResult>(T1 a, T2 b);

  public delegate TResult FunctionCall<T1, T2, T3, TResult>(T1 a, T2 b, T3 c);

  public delegate TResult FunctionCall<T1, T2, T3, T4, TResult>(T1 a, T2 b, T3 c, T4 d);

  public delegate TResult FunctionCall<T1, T2, T3, T4, T5, TResult>(T1 a, T2 b, T3 c, T4 d, T5 e);

  public delegate TResult FunctionCall<T1, T2, T3, T4, T5, T6, TResult>(
    T1 a,
    T2 b,
    T3 c,
    T4 d,
    T5 e,
    T6 f);

  public delegate TResult FunctionCall<T1, T2, T3, T4, T5, T6, T7, TResult>(
    T1 a,
    T2 b,
    T3 c,
    T4 d,
    T5 e,
    T6 f,
    T7 g);

  public delegate TResult FunctionCall<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
    T1 a,
    T2 b,
    T3 c,
    T4 d,
    T5 e,
    T6 f,
    T7 g,
    T8 h);
}
