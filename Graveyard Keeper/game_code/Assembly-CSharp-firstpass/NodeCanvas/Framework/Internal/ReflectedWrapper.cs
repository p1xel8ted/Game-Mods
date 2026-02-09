// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedWrapper
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Serialization;
using System.Reflection;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Framework.Internal;

public abstract class ReflectedWrapper
{
  [SerializeField]
  public SerializedMethodInfo _targetMethod;

  public static ReflectedWrapper Create(MethodInfo method, IBlackboard bb)
  {
    if (MethodInfo.op_Equality(method, (MethodInfo) null))
      return (ReflectedWrapper) null;
    return System.Type.op_Equality(method.ReturnType, typeof (void)) ? (ReflectedWrapper) ReflectedActionWrapper.Create(method, bb) : (ReflectedWrapper) ReflectedFunctionWrapper.Create(method, bb);
  }

  public void SetVariablesBB(IBlackboard bb)
  {
    foreach (BBParameter variable in this.GetVariables())
      variable.bb = bb;
  }

  public bool HasChanged() => this._targetMethod != null && this._targetMethod.HasChanged();

  public MethodInfo GetMethod()
  {
    return this._targetMethod == null ? (MethodInfo) null : this._targetMethod.Get();
  }

  public string GetMethodString()
  {
    return this._targetMethod == null ? (string) null : this._targetMethod.GetMethodString();
  }

  public abstract BBParameter[] GetVariables();

  public abstract void Init(object instance);

  public delegate void ActionCall();

  public delegate void ActionCall<T1>(T1 a);

  public delegate void ActionCall<T1, T2>(T1 a, T2 b);

  public delegate void ActionCall<T1, T2, T3>(T1 a, T2 b, T3 c);

  public delegate void ActionCall<T1, T2, T3, T4>(T1 a, T2 b, T3 c, T4 d);

  public delegate void ActionCall<T1, T2, T3, T4, T5>(T1 a, T2 b, T3 c, T4 d, T5 e);

  public delegate void ActionCall<T1, T2, T3, T4, T5, T6>(T1 a, T2 b, T3 c, T4 d, T5 e, T6 f);

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
}
