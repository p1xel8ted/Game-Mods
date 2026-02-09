// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedAction`1
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
public class ReflectedAction<T1> : ReflectedActionWrapper
{
  public ReflectedWrapper.ActionCall<T1> call;
  public BBParameter<T1> p1 = new BBParameter<T1>();

  public override BBParameter[] GetVariables()
  {
    return new BBParameter[1]{ (BBParameter) this.p1 };
  }

  public override void Init(object instance)
  {
    this.call = this.GetMethod().RTCreateDelegate<ReflectedWrapper.ActionCall<T1>>(instance);
  }

  public override void Call() => this.call(this.p1.value);
}
