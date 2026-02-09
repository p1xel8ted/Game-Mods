// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.DefaultControllerElementGlyphSettingsBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

public abstract class DefaultControllerElementGlyphSettingsBase : MonoBehaviour
{
  [Tooltip("The Controller element glyph options.")]
  [SerializeField]
  public ControllerElementGlyphSelectorOptions _options;
  [Tooltip("The prefab used for each glyph or text object.")]
  [SerializeField]
  public GameObject _glyphOrTextPrefab;

  public ControllerElementGlyphSelectorOptions options
  {
    get => this._options;
    set
    {
      this._options = value;
      this.SetDefaults();
    }
  }

  public GameObject glyphOrTextPrefab
  {
    get => this._glyphOrTextPrefab;
    set
    {
      this._glyphOrTextPrefab = value;
      this.SetDefaults();
    }
  }

  public virtual void OnEnable() => this.SetDefaults();

  public virtual void OnDisable()
  {
  }

  public virtual void SetDefaults()
  {
    this.SetDefaultOptions();
    this.SetDefaultGlyphOrTextPrefab();
  }

  public virtual void SetDefaultOptions()
  {
    ControllerElementGlyphSelectorOptions.defaultOptions = this.options;
  }

  public abstract void SetDefaultGlyphOrTextPrefab();
}
