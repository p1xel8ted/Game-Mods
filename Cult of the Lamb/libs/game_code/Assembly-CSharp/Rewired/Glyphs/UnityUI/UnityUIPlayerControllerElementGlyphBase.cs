// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.UnityUI.UnityUIPlayerControllerElementGlyphBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs.UnityUI;

public abstract class UnityUIPlayerControllerElementGlyphBase : UnityUIControllerElementGlyphBase
{
  [Tooltip("Optional reference to an object that defines options. If blank, the global default options will be used.")]
  [SerializeField]
  public ControllerElementGlyphSelectorOptionsSOBase _options;
  [Tooltip("The range of the Action for which to show glyphs / text. This determines whether to show the glyph for an axis-type Action (ex: Move Horizontal), or the positive/negative pole of an Action (ex: Move Right). For button-type Actions, Full and Positive are equivalent.")]
  [SerializeField]
  public AxisRange _actionRange;
  [Tooltip("Optional parent Transform of the first group of instantiated glyph / text objects. If an axis-type Action is bound to multiple elements, the glyphs bound to the negative pole of the Action will be instantiated under this Transform. This allows you to separate negative and positive groups in order to stack glyph groups horizontally or vertically, for example. If an Action is only bound to one element, the glyph will be instantiated under this transform. If blank, objects will be created as children of this object's Transform.")]
  [SerializeField]
  public Transform _group1;
  [Tooltip("Optional parent Transform of the second group of instantiated glyph / text objects. If an axis-type Action is bound to multiple elements, the glyphs bound to the positive pole of the Action will be instantiated under this Transform. This allows you to separate negative and positive groups in order to stack glyph groups horizontally or vertically, for example. If an Action is only bound to one element, the glyph will be instantiated under group1 instead. If blank, objects will be created as children of either group1 if set or the object's Transform.")]
  [SerializeField]
  public Transform _group2;
  [NonSerialized]
  public List<ActionElementMap> _tempAems = new List<ActionElementMap>();
  [NonSerialized]
  public List<ActionElementMap> _tempCombinedElementAems = new List<ActionElementMap>();
  [NonSerialized]
  public List<ControllerElementGlyphBase.GlyphOrTextObject> _group1Objects = new List<ControllerElementGlyphBase.GlyphOrTextObject>();
  [NonSerialized]
  public List<ControllerElementGlyphBase.GlyphOrTextObject> _group2Objects = new List<ControllerElementGlyphBase.GlyphOrTextObject>();

  public virtual ControllerElementGlyphSelectorOptionsSOBase options
  {
    get => this._options;
    set
    {
      this._options = value;
      this.RequireRebuild();
    }
  }

  public abstract int playerId { get; set; }

  public abstract int actionId { get; set; }

  public virtual AxisRange actionRange
  {
    get => this._actionRange;
    set => this._actionRange = value;
  }

  public virtual Transform group1
  {
    get => this._group1;
    set
    {
      this._group1 = value;
      this.RequireRebuild();
    }
  }

  public virtual Transform group2
  {
    get => this._group2;
    set
    {
      this._group2 = value;
      this.RequireRebuild();
    }
  }

  public virtual bool isMousePrioritizedOverKeyboard
  {
    get
    {
      ControllerType controllerType;
      for (int index = 0; this.TryGetControllerTypeOrder(index, out controllerType); ++index)
      {
        if (controllerType == ControllerType.Mouse)
          return true;
        if (controllerType == ControllerType.Keyboard)
          return false;
      }
      return false;
    }
  }

  public virtual bool TryGetControllerTypeOrder(int index, out ControllerType controllerType)
  {
    return this.GetOptionsOrDefault().TryGetControllerTypeOrder(index, out controllerType);
  }

  public override void Update()
  {
    base.Update();
    if (!ReInput.isReady)
      return;
    ActionElementMap aemResult1;
    ActionElementMap aemResult2;
    if (!GlyphTools.TryGetActionElementMaps(this.playerId, this.actionId, this.actionRange, this.GetOptionsOrDefault(), this._tempAems, out aemResult1, out aemResult2))
      this.Hide();
    else if (aemResult1 != null && aemResult2 != null)
      this.ShowSplitAxisBindings(aemResult1, aemResult2);
    else if (aemResult1 != null)
    {
      this.ShowBinding(aemResult1);
    }
    else
    {
      if (aemResult2 == null)
        return;
      this.ShowBinding(aemResult2);
    }
  }

  public override void ClearObjects()
  {
    this._group1Objects.Clear();
    this._group2Objects.Clear();
    base.ClearObjects();
  }

  public virtual bool ShowBinding(ActionElementMap actionElementMap)
  {
    if (actionElementMap == null)
      return false;
    int num = this.ShowGlyphsOrText(actionElementMap, this.GetObjectGroupTransform(0), this._group1Objects);
    this.EvaluateObjectVisibility();
    return num > 0;
  }

  public virtual bool ShowSplitAxisBindings(
    ActionElementMap negativeAem,
    ActionElementMap positiveAem)
  {
    if (negativeAem == null && positiveAem == null)
      return false;
    int num = 0;
    if (negativeAem != null && positiveAem != null)
    {
      this._tempCombinedElementAems.Clear();
      this._tempCombinedElementAems.Add(negativeAem);
      this._tempCombinedElementAems.Add(positiveAem);
      num = this.ShowGlyphsOrText((IList<ActionElementMap>) this._tempCombinedElementAems, this.GetObjectGroupTransform(0), this._group1Objects);
    }
    if (num == 0)
      num = num + this.ShowGlyphsOrText(negativeAem, this.GetObjectGroupTransform(0), this._group1Objects) + this.ShowGlyphsOrText(positiveAem, this.GetObjectGroupTransform(1), this._group2Objects);
    this.EvaluateObjectVisibility();
    return num > 0;
  }

  public override void EvaluateObjectVisibility()
  {
    base.EvaluateObjectVisibility();
    Transform objectGroupTransform1 = this.GetObjectGroupTransform(0);
    Transform objectGroupTransform2 = this.GetObjectGroupTransform(1);
    if ((UnityEngine.Object) objectGroupTransform1 == (UnityEngine.Object) objectGroupTransform2)
    {
      this.EvaluateObjectVisibility(objectGroupTransform1);
    }
    else
    {
      this.EvaluateObjectVisibility(objectGroupTransform1, this._group1Objects);
      this.EvaluateObjectVisibility(objectGroupTransform2, this._group2Objects);
    }
  }

  public virtual int ShowGlyphsOrText(
    IList<ActionElementMap> bindings,
    Transform parent,
    List<ControllerElementGlyphBase.GlyphOrTextObject> objects)
  {
    if (bindings == null)
      return 0;
    object result1;
    if (this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Glyphs) && ActionElementMap.TryGetCombinedElementIdentifierGlyph((IList<ActionElementMap>) bindings, out result1))
    {
      if (!this.CreateObjectsAsNeeded(parent, objects, 1))
        return 0;
      objects[0].ShowGlyph(result1);
      return 1;
    }
    string result2;
    if (!this.IsAllowed(ControllerElementGlyphBase.AllowedTypes.Text) || !ActionElementMap.TryGetCombinedElementIdentifierName((IList<ActionElementMap>) bindings, out result2) || !this.CreateObjectsAsNeeded(parent, objects, 1))
      return 0;
    objects[0].ShowText(result2);
    return 1;
  }

  public override void Hide()
  {
    base.Hide();
    if ((UnityEngine.Object) this._group1 != (UnityEngine.Object) null && (UnityEngine.Object) this._group1 != (UnityEngine.Object) this.transform)
      this._group1.gameObject.SetActive(false);
    if (!((UnityEngine.Object) this._group2 != (UnityEngine.Object) null) || !((UnityEngine.Object) this._group2 != (UnityEngine.Object) this.transform))
      return;
    this._group2.gameObject.SetActive(false);
  }

  public virtual Transform GetObjectGroupTransform(int groupIndex)
  {
    switch (groupIndex)
    {
      case 0:
        return !((UnityEngine.Object) this._group1 != (UnityEngine.Object) null) ? this.transform : this._group1;
      case 1:
        if ((UnityEngine.Object) this._group1 == (UnityEngine.Object) null)
          return this.transform;
        if ((UnityEngine.Object) this._group2 != (UnityEngine.Object) null)
          return this._group2;
        return (UnityEngine.Object) this._group1 != (UnityEngine.Object) null ? this._group1 : this.transform;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public virtual ControllerElementGlyphSelectorOptions GetOptionsOrDefault()
  {
    if ((UnityEngine.Object) this._options != (UnityEngine.Object) null && this._options.options == null)
    {
      Debug.LogError((object) $"Rewired: Options missing on {typeof (ControllerElementGlyphSelectorOptions).Name}. Global default options will be used instead.");
      return ControllerElementGlyphSelectorOptions.defaultOptions;
    }
    return !((UnityEngine.Object) this._options != (UnityEngine.Object) null) ? ControllerElementGlyphSelectorOptions.defaultOptions : this._options.options;
  }
}
