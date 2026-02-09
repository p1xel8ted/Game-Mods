// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedAction`3
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[Serializable]
public class ReflectedAction<T1, T2, T3> : ReflectedActionWrapper
{
  public ReflectedWrapper.ActionCall<T1, T2, T3> call;
  public BBParameter<T1> p1 = new BBParameter<T1>();
  public BBParameter<T2> p2 = new BBParameter<T2>();
  public BBParameter<T3> p3 = new BBParameter<T3>();

  public override BBParameter[] GetVariables()
  {
    return new BBParameter[3]
    {
      (BBParameter) this.p1,
      (BBParameter) this.p2,
      (BBParameter) this.p3
    };
  }

  public override void Init(object instance)
  {
    this.call = this.GetMethod().RTCreateDelegate<ReflectedWrapper.ActionCall<T1, T2, T3>>(instance);
  }

  public override void Call() => this.call(this.p1.value, this.p2.value, this.p3.value);
}
