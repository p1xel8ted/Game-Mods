// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.ReflectedUnityEvent
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using ParadoxNotion;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;

#nullable disable
namespace FlowCanvas.Nodes;

[SpoofAOT]
public class ReflectedUnityEvent
{
  public Type _eventType;
  public MethodInfo _addListenerMethod;
  public MethodInfo _removeListenerMethod;
  public ParameterInfo[] _parameters;
  public MethodInfo _callMethod;

  public event ReflectedUnityEvent.UnityEventCallback _callback;

  public ParameterInfo[] parameters => this._parameters;

  public Type eventType => this._eventType;

  public ReflectedUnityEvent()
  {
  }

  public ReflectedUnityEvent(Type eventType) => this.InitForEventType(eventType);

  public void InitForEventType(Type eventType)
  {
    this._eventType = eventType;
    if (Type.op_Equality(this._eventType, (Type) null) || !this._eventType.RTIsSubclassOf(typeof (UnityEventBase)))
      return;
    this._parameters = this._eventType.GetMethod("Invoke", BindingFlags.Instance | BindingFlags.Public).GetParameters();
    Type type = this.GetType();
    if (this._parameters.Length == 0)
      this._callMethod = type.RTGetMethod("CallbackMethod0");
    if (this._parameters.Length == 1)
      this._callMethod = type.RTGetMethod("CallbackMethod1");
    if (this._parameters.Length == 2)
      this._callMethod = type.RTGetMethod("CallbackMethod2");
    if (this._parameters.Length == 3)
      this._callMethod = type.RTGetMethod("CallbackMethod3");
    if (this._parameters.Length == 4)
      this._callMethod = type.RTGetMethod("CallbackMethod4");
    if (this._callMethod.IsGenericMethodDefinition)
      this._callMethod = this._callMethod.MakeGenericMethod(((IEnumerable<ParameterInfo>) this._parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).ToArray<Type>());
    Type[] types = new Type[2]
    {
      typeof (object),
      typeof (MethodInfo)
    };
    BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic;
    this._addListenerMethod = typeof (UnityEventBase).GetMethod("AddListener", bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
    this._removeListenerMethod = typeof (UnityEventBase).GetMethod("RemoveListener", bindingAttr, (Binder) null, types, (ParameterModifier[]) null);
  }

  public void StartListening(
    UnityEventBase targetEvent,
    ReflectedUnityEvent.UnityEventCallback callback)
  {
    this._callback += callback;
    this._addListenerMethod.Invoke((object) targetEvent, new object[2]
    {
      (object) this,
      (object) this._callMethod
    });
  }

  public void StopListening(
    UnityEventBase targetEvent,
    ReflectedUnityEvent.UnityEventCallback callback)
  {
    this._callback -= callback;
    this._removeListenerMethod.Invoke((object) targetEvent, new object[2]
    {
      (object) this,
      (object) this._callMethod
    });
  }

  public void CallbackMethod0() => this._callback();

  public void CallbackMethod1<T0>(T0 arg0) => this._callback((object) arg0);

  public void CallbackMethod2<T0, T1>(T0 arg0, T1 arg1)
  {
    this._callback((object) arg0, (object) arg1);
  }

  public void CallbackMethod3<T0, T1, T2>(T0 arg0, T1 arg1, T2 arg2)
  {
    this._callback((object) arg0, (object) arg1, (object) arg2);
  }

  public void CallbackMethod4<T0, T1, T2, T3>(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
  {
    this._callback((object) arg0, (object) arg1, (object) arg2, (object) arg3);
  }

  public delegate void UnityEventCallback(params object[] args);
}
