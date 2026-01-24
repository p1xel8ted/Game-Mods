// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.ControllerElementGlyph
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;

#nullable disable
namespace Rewired.Glyphs;

public abstract class ControllerElementGlyph : ControllerElementGlyphBase
{
  [NonSerialized]
  public ActionElementMap _actionElementMap;
  [NonSerialized]
  public ControllerElementIdentifier _controllerElementIdentifier;
  [NonSerialized]
  public AxisRange _axisRange;

  public ActionElementMap actionElementMap
  {
    get => this._actionElementMap;
    set => this._actionElementMap = value;
  }

  public ControllerElementIdentifier controllerElementIdentifier
  {
    get => this._controllerElementIdentifier;
    set => this._controllerElementIdentifier = value;
  }

  public AxisRange axisRange
  {
    get => this._axisRange;
    set => this._axisRange = value;
  }

  public override void Update()
  {
    base.Update();
    if (!ReInput.isReady)
      return;
    if (this._actionElementMap == null && this.controllerElementIdentifier == null)
    {
      this.Hide();
    }
    else
    {
      if (this.actionElementMap != null)
        this.ShowGlyphsOrText(this._actionElementMap);
      else if (this.controllerElementIdentifier != null)
        this.ShowGlyphsOrText(this._controllerElementIdentifier, this.axisRange);
      this.EvaluateObjectVisibility();
    }
  }
}
