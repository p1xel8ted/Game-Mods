// Decompiled with JetBrains decompiler
// Type: Rewired.Glyphs.ControllerElementGlyphSelectorOptions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Rewired.Glyphs;

[Serializable]
public class ControllerElementGlyphSelectorOptions
{
  [Tooltip("Determines if the Player's last active controller is used for glyph selection.")]
  [SerializeField]
  public bool _useLastActiveController = true;
  [Tooltip("List of controller type priority. First in list corresponds to highest priority. This determines which controller types take precedence when displaying glyphs. If use last active controller is enabled, the active controller will always take priority, however, if there is no last active controller, selection will fall back based on this priority. In addition, keyboard and mouse are treated as a single controller for the purposes of glyph handling, so to prioritze keyboard over mouse or vice versa, the one that is lower in the list will take precedence.")]
  [SerializeField]
  public ControllerType[] _controllerTypeOrder = new ControllerType[4]
  {
    ControllerType.Joystick,
    ControllerType.Custom,
    ControllerType.Mouse,
    ControllerType.Keyboard
  };
  public static ControllerElementGlyphSelectorOptions s_defaultOptions;

  public bool useLastActiveController
  {
    get => this._useLastActiveController;
    set => this._useLastActiveController = value;
  }

  public ControllerType[] controllerTypeOrder
  {
    get => this._controllerTypeOrder;
    set => this._controllerTypeOrder = value;
  }

  public virtual bool TryGetControllerTypeOrder(int index, out ControllerType controllerType)
  {
    if ((uint) index >= (uint) this._controllerTypeOrder.Length)
    {
      controllerType = ControllerType.Keyboard;
      return false;
    }
    controllerType = this._controllerTypeOrder[index];
    return true;
  }

  public static ControllerElementGlyphSelectorOptions defaultOptions
  {
    get
    {
      return ControllerElementGlyphSelectorOptions.s_defaultOptions == null ? (ControllerElementGlyphSelectorOptions.s_defaultOptions = new ControllerElementGlyphSelectorOptions()) : ControllerElementGlyphSelectorOptions.s_defaultOptions;
    }
    set => ControllerElementGlyphSelectorOptions.s_defaultOptions = value;
  }
}
