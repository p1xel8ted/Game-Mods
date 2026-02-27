// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Interfaces;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

public class GlyphProvider : MonoBehaviour, IGlyphProvider
{
  [SerializeField]
  [Tooltip("Determines if glyphs should be fetched immediately in bulk when available. If false, glyphs will be fetched when queried.")]
  public bool _prefetch;
  [SerializeField]
  [Tooltip("A list of glyph set collections. At least one collection must be assigned.")]
  public List<GlyphSetCollection> _glyphSetCollections;
  [NonSerialized]
  public Dictionary<string, object> _glyphs = new Dictionary<string, object>();
  [NonSerialized]
  public bool _initialized;

  public bool prefetch
  {
    get => this._prefetch;
    set
    {
      this._prefetch = value;
      if (!this.isActiveAndEnabled || !ReInput.isReady || ReInput.glyphs.glyphProvider != this)
        return;
      ReInput.glyphs.prefetch = value;
    }
  }

  public List<GlyphSetCollection> glyphSetCollections
  {
    get => this._glyphSetCollections;
    set
    {
      this._glyphSetCollections = value;
      this.Reload();
    }
  }

  public Dictionary<string, object> glyphs => this._glyphs;

  public virtual void OnEnable()
  {
    if (!this._initialized)
      this.Initialize();
    this.TrySetGlyphProvider();
  }

  public virtual void OnDisable()
  {
    if (ReInput.isReady && ReInput.glyphs.glyphProvider == this)
      ReInput.glyphs.glyphProvider = (IGlyphProvider) null;
    ReInput.InitializedEvent -= new Action(this.TrySetGlyphProvider);
  }

  public virtual void Update()
  {
  }

  public virtual void TrySetGlyphProvider()
  {
    ReInput.InitializedEvent -= new Action(this.TrySetGlyphProvider);
    ReInput.InitializedEvent += new Action(this.TrySetGlyphProvider);
    if (!ReInput.isReady)
      return;
    if (!UnityTools.IsNullOrDestroyed<IGlyphProvider>(ReInput.glyphs.glyphProvider))
    {
      Debug.LogWarning((object) "Rewired: A glyph provider is already set. Only one glyph provider can exist at a time.");
    }
    else
    {
      ReInput.glyphs.glyphProvider = (IGlyphProvider) this;
      ReInput.glyphs.prefetch = this._prefetch;
    }
  }

  public virtual bool Initialize()
  {
    this._initialized = false;
    if (this._glyphSetCollections == null)
      return false;
    this._glyphs.Clear();
    StringBuilder stringBuilder = new StringBuilder();
    for (int index1 = 0; index1 < this._glyphSetCollections.Count; ++index1)
    {
      GlyphSetCollection glyphSetCollection = this._glyphSetCollections[index1];
      if (!((UnityEngine.Object) glyphSetCollection == (UnityEngine.Object) null))
      {
        foreach (GlyphSet glyphSet in glyphSetCollection.IterateSetsRecursively())
        {
          if (!((UnityEngine.Object) glyphSet == (UnityEngine.Object) null) && glyphSet.baseKeys != null)
          {
            int length = glyphSet.baseKeys.Length;
            for (int index2 = 0; index2 < length; ++index2)
            {
              if (!string.IsNullOrEmpty(glyphSet.baseKeys[index2]))
              {
                int glyphCount = glyphSet.glyphCount;
                for (int index3 = 0; index3 < glyphCount; ++index3)
                {
                  GlyphSet.EntryBase entry = glyphSet.GetEntry(index3);
                  if (entry != null && !string.IsNullOrEmpty(entry.key) && entry.GetValue() != null)
                  {
                    stringBuilder.Append(glyphSet.baseKeys[index2]);
                    stringBuilder.Append('/');
                    stringBuilder.Append(entry.key);
                    string key = stringBuilder.ToString();
                    stringBuilder.Length = 0;
                    if (this._glyphs.ContainsKey(key))
                      Debug.LogError((object) ("Rewired: Duplicate glyph key found: " + key));
                    else
                      this._glyphs.Add(key, entry.GetValue());
                  }
                }
              }
            }
          }
        }
      }
    }
    this._initialized = true;
    return true;
  }

  public void Reload()
  {
    this.Initialize();
    if (!this.isActiveAndEnabled || !ReInput.isReady || ReInput.glyphs.glyphProvider != this)
      return;
    ReInput.glyphs.Reload();
  }

  bool IGlyphProvider.TryGetGlyph(string key, out object result)
  {
    if (this._initialized)
      return this._glyphs.TryGetValue(key, out result);
    result = (object) null;
    return false;
  }
}
