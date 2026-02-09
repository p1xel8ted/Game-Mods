// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedFunction`2
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[Serializable]
public class ReflectedFunction<TResult, T1> : ReflectedFunctionWrapper
{
  public ReflectedWrapper.FunctionCall<T1, TResult> call;
  public BBParameter<T1> p1 = new BBParameter<T1>();
  [BlackboardOnly]
  public BBParameter<TResult> result = new BBParameter<TResult>();

  public override BBParameter[] GetVariables()
  {
    return new BBParameter[2]
    {
      (BBParameter) this.result,
      (BBParameter) this.p1
    };
  }

  public override void Init(object instance)
  {
    this.call = this.GetMethod().RTCreateDelegate<ReflectedWrapper.FunctionCall<T1, TResult>>(instance);
  }

  public override object Call() => (object) (this.result.value = this.call(this.p1.value));
}
