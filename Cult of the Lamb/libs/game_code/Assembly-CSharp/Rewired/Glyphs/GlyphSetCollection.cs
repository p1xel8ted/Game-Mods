// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.GlyphSetCollection
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

[Serializable]
public class GlyphSetCollection : ScriptableObject
{
  [Tooltip("The list of glyph sets.")]
  [SerializeField]
  public List<GlyphSet> _sets;
  [Tooltip("The list of glyph set collections.")]
  [SerializeField]
  public List<GlyphSetCollection> _collections;

  public List<GlyphSet> sets
  {
    get => this._sets;
    set => this._sets = value;
  }

  public List<GlyphSetCollection> collections
  {
    get => this._collections;
    set
    {
      if (value != null && value.Contains(this))
      {
        GlyphSetCollection.LogCircularDependency();
        Debug.LogWarning((object) "Rewired: Set collections aborted due to circular dependency.");
      }
      else
        this._collections = value;
    }
  }

  public virtual IEnumerable<GlyphSet> IterateSetsRecursively()
  {
    return this.IterateSetsRecursively(new List<GlyphSetCollection>()
    {
      this
    });
  }

  public virtual IEnumerable<GlyphSet> IterateSetsRecursively(
    List<GlyphSetCollection> processedCollections)
  {
    if (processedCollections == null)
      throw new ArgumentNullException(nameof (processedCollections));
    int i;
    if (this._sets != null)
    {
      int setCount = this._sets.Count;
      for (i = 0; i < setCount; ++i)
      {
        if (!((UnityEngine.Object) this._sets[i] == (UnityEngine.Object) null))
          yield return this.sets[i];
      }
    }
    if (this._collections != null)
    {
      int collectionCount = this._collections.Count;
      for (i = 0; i < collectionCount; ++i)
      {
        if (!((UnityEngine.Object) this._collections[i] == (UnityEngine.Object) null))
        {
          if (processedCollections.Contains(this._collections[i]))
          {
            GlyphSetCollection.LogCircularDependency();
          }
          else
          {
            processedCollections.Add(this._collections[i]);
            foreach (GlyphSet glyphSet in this._collections[i].IterateSetsRecursively(processedCollections))
              yield return glyphSet;
          }
        }
      }
    }
  }

  public static void LogCircularDependency()
  {
    Debug.LogError((object) "Rewired: Circular dependency detected. This collection is referenced in a child collection. This is not allowed.");
  }
}
