// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedDelegateEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace FlowCanvas.Nodes;

[SpoofAOT]
public class ReflectedDelegateEvent
{
  public Delegate theDelegate;

  public event ReflectedDelegateEvent.DelegateEventCallback onCallback;

  public ReflectedDelegateEvent(System.Type delegateType)
  {
    this.theDelegate = this.GetMethodForDelegateType(delegateType).RTCreateDelegate(delegateType, (object) this);
  }

  public void Add(
    ReflectedDelegateEvent.DelegateEventCallback callback)
  {
    this.onCallback += callback;
  }

  public void Remove(
    ReflectedDelegateEvent.DelegateEventCallback callback)
  {
    this.onCallback -= callback;
  }

  public Delegate AsDelegate() => this.theDelegate;

  public MethodInfo GetMethodForDelegateType(System.Type delegateType)
  {
    ParameterInfo[] parameters = delegateType.GetMethod("Invoke").GetParameters();
    System.Type type = this.GetType();
    MethodInfo methodForDelegateType = (MethodInfo) null;
    if (parameters.Length == 0)
      methodForDelegateType = type.GetMethod("Callback0");
    else if (parameters.Length == 1)
      methodForDelegateType = type.GetMethod("Callback1");
    else if (parameters.Length == 2)
      methodForDelegateType = type.GetMethod("Callback2");
    else if (parameters.Length == 3)
      methodForDelegateType = type.GetMethod("Callback3");
    else if (parameters.Length == 4)
      methodForDelegateType = type.GetMethod("Callback4");
    else if (parameters.Length == 5)
      methodForDelegateType = type.GetMethod("Callback5");
    else if (parameters.Length == 6)
      methodForDelegateType = type.GetMethod("Callback6");
    else if (parameters.Length == 7)
      methodForDelegateType = type.GetMethod("Callback7");
    else if (parameters.Length == 8)
      methodForDelegateType = type.GetMethod("Callback8");
    else if (parameters.Length == 9)
      methodForDelegateType = type.GetMethod("Callback9");
    else if (parameters.Length == 10)
      methodForDelegateType = type.GetMethod("Callback10");
    try
    {
      if (methodForDelegateType.IsGenericMethodDefinition)
        methodForDelegateType = methodForDelegateType.MakeGenericMethod(((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, System.Type>((Func<ParameterInfo, System.Type>) (p => p.ParameterType)).ToArray<System.Type>());
    }
    catch (Exception ex)
    {
      Debug.LogException(ex);
      return (MethodInfo) null;
    }
    return methodForDelegateType;
  }

  public void Callback0() => this.onCallback();

  public void Callback1<T0>(T0 arg0) => this.onCallback((object) arg0);

  public void Callback2<T0, T1>(T0 arg0, T1 arg1) => this.onCallback((object) arg0, (object) arg1);

  public void Callback3<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2);
  }

  public void Callback4<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3);
  }

  public void Callback5<T0, T1, T2, T3, T4>(T0 arg0, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3, (object) arg4);
  }

  public void Callback6<T0, T1, T2, T3, T4, T5>(
    T0 arg0,
    T1 arg1,
    T2 arg2,
    T3 arg3,
    T4 arg4,
    T5 arg5)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3, (object) arg4, (object) arg5);
  }

  public void Callback7<T0, T1, T2, T3, T4, T5, T6>(
    T0 arg0,
    T1 arg1,
    T2 arg2,
    T3 arg3,
    T4 arg4,
    T5 arg5,
    T6 arg6)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3, (object) arg4, (object) arg5, (object) arg6);
  }

  public void Callback8<T0, T1, T2, T3, T4, T5, T6, T7>(
    T0 arg0,
    T1 arg1,
    T2 arg2,
    T3 arg3,
    T4 arg4,
    T5 arg5,
    T6 arg6,
    T7 arg7)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3, (object) arg4, (object) arg5, (object) arg6, (object) arg7);
  }

  public void Callback9<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
    T0 arg0,
    T1 arg1,
    T2 arg2,
    T3 arg3,
    T4 arg4,
    T5 arg5,
    T6 arg6,
    T7 arg7,
    T8 arg8)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3, (object) arg4, (object) arg5, (object) arg6, (object) arg7, (object) arg8);
  }

  public void Callback10<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
    T0 arg0,
    T1 arg1,
    T2 arg2,
    T3 arg3,
    T4 arg4,
    T5 arg5,
    T6 arg6,
    T7 arg7,
    T8 arg8,
    T9 arg9)
  {
    this.onCallback((object) arg0, (object) arg1, (object) arg2, (object) arg3, (object) arg4, (object) arg5, (object) arg6, (object) arg7, (object) arg8, (object) arg9);
  }

  public static explicit operator Delegate(ReflectedDelegateEvent that) => that.theDelegate;

  public delegate void DelegateEventCallback(params object[] args);
}
