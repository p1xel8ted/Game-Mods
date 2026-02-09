// Decompiled with JetBrains decompiler
// Type: ParadoxNotion.Serialization.FullSerializer.Internal.fsOption`1
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System;

#nullable disable
namespace ParadoxNotion.Serialization.FullSerializer.Internal;

public struct fsOption<T>(T value)
{
  public bool _hasValue = true;
  public T _value = value;
  public static fsOption<T> Empty;

  public bool HasValue => this._hasValue;

  public bool IsEmpty => !this._hasValue;

  public T Value
  {
    get
    {
      if (this.IsEmpty)
        throw new InvalidOperationException("fsOption is empty");
      return this._value;
    }
  }
}
