// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputFieldInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Glyphs.UnityUI;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class InputFieldInfo : UIElementInfo
{
  public int _actionElementMapId;
  public AxisRange _axisRange;
  [CompilerGenerated]
  public UnityUIControllerElementGlyph \u003CglyphOrText\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CactionId\u003Ek__BackingField;
  [CompilerGenerated]
  public ControllerType \u003CcontrollerType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CcontrollerId\u003Ek__BackingField;

  public UnityUIControllerElementGlyph glyphOrText
  {
    get => this.\u003CglyphOrText\u003Ek__BackingField;
    set => this.\u003CglyphOrText\u003Ek__BackingField = value;
  }

  public int actionId
  {
    get => this.\u003CactionId\u003Ek__BackingField;
    set => this.\u003CactionId\u003Ek__BackingField = value;
  }

  public AxisRange axisRange
  {
    get => this._axisRange;
    set
    {
      this._axisRange = value;
      if (!((Object) this.glyphOrText != (Object) null))
        return;
      this.glyphOrText.axisRange = value;
    }
  }

  public int actionElementMapId
  {
    get => this._actionElementMapId;
    set
    {
      this._actionElementMapId = value;
      if (!((Object) this.glyphOrText != (Object) null))
        return;
      this.glyphOrText.actionElementMap = ReInput.mapping.GetActionElementMap(value);
    }
  }

  public ControllerType controllerType
  {
    get => this.\u003CcontrollerType\u003Ek__BackingField;
    set => this.\u003CcontrollerType\u003Ek__BackingField = value;
  }

  public int controllerId
  {
    get => this.\u003CcontrollerId\u003Ek__BackingField;
    set => this.\u003CcontrollerId\u003Ek__BackingField = value;
  }
}
