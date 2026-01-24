// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.ControllerElementGlyphBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

public abstract class ControllerElementGlyphBase : MonoBehaviour
{
  [Tooltip("If set, when glyph/text objects are created, they will be instantiated from this prefab. If left blank, the global default prefab will be used.")]
  [SerializeField]
  public GameObject _glyphOrTextPrefab;
  [Tooltip("Determines what types of objects are allowed.")]
  [SerializeField]
  public ControllerElementGlyphBase.AllowedTypes _allowedTypes;
  [NonSerialized]
  public List<ControllerElementGlyphBase.GlyphOrTextObject> _entries = new List<ControllerElementGlyphBase.GlyphOrTextObject>();
  [NonSerialized]
  public List<object> _tempGlyphs = new List<object>();
  [NonSerialized]
  public GameObject _lastGlyphOrTextPrefab;

  public virtual GameObject glyphOrTextPrefab
  {
    get => this._glyphOrTextPrefab;
    set
    {
      this._glyphOrTextPrefab = value;
      this.RequireRebuild();
    }
  }

  public virtual ControllerElementGlyphBase.AllowedTypes allowedTypes
  {
    get => this._allowedTypes;
    set => this._allowedTypes = value;
  }

  public List<ControllerElementGlyphBase.GlyphOrTextObject> entries => this._entries;

  public virtual void Awake()
  {
  }

  public virtual void Start()
  {
  }

  public virtual void OnDestroy()
  {
  }

  public virtual void OnEnable()
  {
  }

  public virtual void OnDisable()
  {
  }

  public virtual void Update()
  {
    if (!ReInput.isReady || !((UnityEngine.Object) this._lastGlyphOrTextPrefab != (UnityEngine.Object) this.GetGlyphOrTextPrefabOrDefault()))
      return;
    this._lastGlyphOrTextPrefab = this.GetGlyphOrTextPrefabOrDefault();
    this.RequireRebuild();
  }

  public virtual void RequireRebuild() => this.ClearObjects();

  public virtual void ClearObjects()
  {
    for (int index = 0; index < this._entries.Count; ++index)
    {
      if (this._entries[index] != null)
        this._entries[index].Destroy();
    }
    this._entries.Clear();
    this.Hide();
  }

  public virtual void EvaluateObjectVisibility()
  {
    for (int index = 0; index < this._entries.Count; ++index)
    {
      if (this._entries[index] != null)
        this._entries[index].HideIfIdle();
    }
  }

  public virtual void EvaluateObjectVisibility(Transform transform)
  {
    this.EvaluateObjectVisibility(transform, this._entries);
  }

  public virtual void EvaluateObjectVisibility(
    Transform transform,
    List<ControllerElementGlyphBase.GlyphOrTextObject> entries)
  {
    if ((UnityEngine.Object) transform == (UnityEngine.Object) this.transform)
      return;
    bool flag = false;
    for (int index = 0; index < entries.Count; ++index)
    {
      if (entries[index].isVisible)
        flag = true;
    }
    if (transform.gameObject.activeSelf == flag)
      return;
    transform.gameObject.SetActive(flag);
  }

  public virtual int ShowGlyphsOrText(
    ActionElementMap actionElementMap,
    Transform parent,
    List<ControllerElementGlyphBase.GlyphOrTextObject> entries)
  {
    this._tempGlyphs.Clear();
    int num = 0;
    if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Glyphs) && ControllerElementGlyphBase.GetGlyphs(actionElementMap, this._tempGlyphs) > 0)
    {
      if (!this.CreateObjectsAsNeeded(parent, entries, this._tempGlyphs.Count))
        return 0;
      for (int index = 0; index < this._tempGlyphs.Count; ++index)
        entries[index].ShowGlyph(this._tempGlyphs[index]);
      num += this._tempGlyphs.Count;
    }
    else if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Text) && actionElementMap != null)
    {
      if (!this.CreateObjectsAsNeeded(parent, entries, 1))
        return 0;
      entries[0].ShowText(actionElementMap.elementIdentifierName);
      ++num;
    }
    return num;
  }

  public virtual int ShowGlyphsOrText(ActionElementMap actionElementMap)
  {
    return this.ShowGlyphsOrText(actionElementMap, this.transform, this._entries);
  }

  public virtual int ShowGlyphsOrText(
    ControllerElementIdentifier elementIdentifier,
    AxisRange axisRange,
    Transform parent,
    List<ControllerElementGlyphBase.GlyphOrTextObject> entries)
  {
    if (elementIdentifier == null)
      return 0;
    int num = 0;
    object glyph;
    if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Glyphs) && (glyph = elementIdentifier.GetGlyph(axisRange)) != null)
    {
      if (!this.CreateObjectsAsNeeded(parent, entries, 1))
        return 0;
      entries[0].ShowGlyph(glyph);
      ++num;
    }
    else if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Text))
    {
      if (!this.CreateObjectsAsNeeded(parent, entries, 1))
        return 0;
      entries[0].ShowText(elementIdentifier.GetDisplayName(axisRange));
      ++num;
    }
    return num;
  }

  public virtual int ShowGlyphsOrText(
    ControllerElementIdentifier elementIdentifier,
    AxisRange axisRange)
  {
    return this.ShowGlyphsOrText(elementIdentifier, axisRange, this.transform, this._entries);
  }

  public virtual void Hide()
  {
    for (int index = 0; index < this._entries.Count; ++index)
    {
      if (this._entries[index] != null)
        this._entries[index].Hide();
    }
  }

  public virtual GameObject GetGlyphOrTextPrefabOrDefault()
  {
    return !((UnityEngine.Object) this._glyphOrTextPrefab != (UnityEngine.Object) null) ? this.GetDefaultGlyphOrTextPrefab() : this._glyphOrTextPrefab;
  }

  public abstract GameObject GetDefaultGlyphOrTextPrefab();

  public virtual bool CreateObjectsAsNeeded(
    Transform parent,
    List<ControllerElementGlyphBase.GlyphOrTextObject> entries,
    int count)
  {
    if (count <= 0)
      return false;
    GameObject textPrefabOrDefault = this.GetGlyphOrTextPrefabOrDefault();
    if ((UnityEngine.Object) textPrefabOrDefault == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Rewired: Default prefab is null.");
      return false;
    }
    if (entries == null)
      return false;
    for (int count1 = entries.Count; count1 < count; ++count1)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(textPrefabOrDefault);
      gameObject.name = "Object";
      gameObject.hideFlags = HideFlags.DontSave;
      gameObject.transform.SetParent(parent, false);
      GlyphOrTextBase component = gameObject.GetComponent<GlyphOrTextBase>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      {
        Debug.LogError((object) $"Rewired: Prefab does not contain a {typeof (GlyphOrTextBase)?.ToString()} component.");
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
      }
      else
      {
        ControllerElementGlyphBase.GlyphOrTextObject glyphOrTextObject = new ControllerElementGlyphBase.GlyphOrTextObject(component);
        entries.Add(glyphOrTextObject);
        if (entries != this._entries)
          this._entries.Add(glyphOrTextObject);
      }
    }
    return true;
  }

  public virtual bool IsAllowed(
    ControllerElementGlyphBase.AllowedTypes allowedType)
  {
    return this._allowedTypes == ControllerElementGlyphBase.AllowedTypes.All || allowedType == this._allowedTypes;
  }

  public static int GetGlyphs(ActionElementMap actionElementMap, List<object> results)
  {
    if (actionElementMap == null)
      return 0;
    int glyphs = 1;
    if (actionElementMap.hasModifiers)
    {
      if (actionElementMap.modifierKey1 != ModifierKey.None)
        ++glyphs;
      if (actionElementMap.modifierKey2 != ModifierKey.None)
        ++glyphs;
      if (actionElementMap.modifierKey3 != ModifierKey.None)
        ++glyphs;
    }
    if (actionElementMap.elementIdentifierGlyphCount != glyphs)
      return 0;
    actionElementMap.GetElementIdentifierGlyphs((ICollection<object>) results);
    return glyphs;
  }

  public class GlyphOrTextObject
  {
    public GlyphOrTextBase _glyphOrText;
    public int _frame;
    public bool _isVisible;

    public virtual bool isVisible
    {
      get => this._isVisible;
      set => this._isVisible = value;
    }

    public GlyphOrTextBase glyphOrText
    {
      get => this._glyphOrText;
      set => this._glyphOrText = value;
    }

    public GlyphOrTextObject(GlyphOrTextBase glyphOrText) => this._glyphOrText = glyphOrText;

    public virtual void ShowGlyph(object glyph)
    {
      if ((UnityEngine.Object) this._glyphOrText == (UnityEngine.Object) null)
        return;
      this._glyphOrText.ShowGlyph(glyph);
      this._frame = Time.frameCount;
      this._isVisible = true;
    }

    public virtual void ShowText(string text)
    {
      if ((UnityEngine.Object) this._glyphOrText == (UnityEngine.Object) null)
        return;
      this._glyphOrText.ShowText(text);
      this._frame = Time.frameCount;
      this._isVisible = true;
    }

    public virtual void Hide()
    {
      if ((UnityEngine.Object) this._glyphOrText == (UnityEngine.Object) null || !this._isVisible)
        return;
      this._glyphOrText.Hide();
      this._isVisible = false;
    }

    public virtual void HideIfIdle()
    {
      if (this._frame == Time.frameCount)
        return;
      this.Hide();
    }

    public virtual void Destroy()
    {
      if ((UnityEngine.Object) this._glyphOrText == (UnityEngine.Object) null)
        return;
      UnityEngine.Object.Destroy((UnityEngine.Object) this._glyphOrText.gameObject);
      this._glyphOrText = (GlyphOrTextBase) null;
      this._isVisible = false;
    }
  }

  public enum AllowedTypes
  {
    All,
    Glyphs,
    Text,
  }
}
