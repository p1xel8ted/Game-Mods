// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.Internal.ReflectedAction
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion;
using System;

#nullable disable
namespace NodeCanvas.Framework.Internal;

[Serializable]
public class ReflectedAction : ReflectedActionWrapper
{
  public ReflectedWrapper.ActionCall call;

  public override BBParameter[] GetVariables() => new BBParameter[0];

  public override void Init(object instance)
  {
    this.call = this.GetMethod().RTCreateDelegate<ReflectedWrapper.ActionCall>(instance);
  }

  public override void Call() => this.call();
}
