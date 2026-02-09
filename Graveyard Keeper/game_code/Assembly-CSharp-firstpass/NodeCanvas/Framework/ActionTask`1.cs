// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Framework.ActionTask`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace NodeCanvas.Framework;

public abstract class ActionTask<T> : ActionTask where T : class
{
  public sealed override Type agentType => typeof (T);

  public T agent => base.agent as T;
}
