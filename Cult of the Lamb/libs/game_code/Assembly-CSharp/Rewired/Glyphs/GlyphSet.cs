// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

[Serializable]
public abstract class GlyphSet : ScriptableObject
{
  [Tooltip("A list of base keys. Final keys will be composed of base key + glyph key. Setting multiple base keys allows one glyph set to apply to multiple controllers, for example.")]
  [SerializeField]
  public string[] _baseKeys;

  public string[] baseKeys
  {
    get => this._baseKeys;
    set => this._baseKeys = value;
  }

  public abstract int glyphCount { get; }

  public abstract GlyphSet.EntryBase GetEntry(int index);

  [Serializable]
  public abstract class EntryBase
  {
    [SerializeField]
    public string _key;

    public string key
    {
      get => this._key;
      set => this._key = value;
    }

    public abstract object GetValue();
  }

  [Serializable]
  public abstract class EntryBase<TValue> : GlyphSet.EntryBase
  {
    [SerializeField]
    public TValue _value;

    public TValue value
    {
      get => this._value;
      set => this._value = value;
    }

    public override object GetValue() => (object) this._value;
  }
}
