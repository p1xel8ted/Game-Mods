// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.fsContext
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer;

public sealed class fsContext
{
  public Dictionary<Type, object> _contextObjects = new Dictionary<Type, object>();

  public void Reset() => this._contextObjects.Clear();

  public void Set<T>(T obj) => this._contextObjects[typeof (T)] = (object) obj;

  public bool Has<T>() => this._contextObjects.ContainsKey(typeof (T));

  public T Get<T>()
  {
    object obj;
    if (this._contextObjects.TryGetValue(typeof (T), out obj))
      return (T) obj;
    throw new InvalidOperationException("There is no context object of type " + typeof (T)?.ToString());
  }
}
