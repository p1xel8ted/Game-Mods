// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedFunction`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[SpoofAOT]
[Serializable]
public class ReflectedFunction<TResult> : ReflectedFunctionWrapper
{
  public ReflectedWrapper.FunctionCall<TResult> call;
  [BlackboardOnly]
  public BBParameter<TResult> result = new BBParameter<TResult>();

  public override BBParameter[] GetVariables()
  {
    return new BBParameter[1]{ (BBParameter) this.result };
  }

  public override void Init(object instance)
  {
    this.call = this.GetMethod().RTCreateDelegate<ReflectedWrapper.FunctionCall<TResult>>(instance);
  }

  public override object Call() => (object) (this.result.value = this.call());
}
