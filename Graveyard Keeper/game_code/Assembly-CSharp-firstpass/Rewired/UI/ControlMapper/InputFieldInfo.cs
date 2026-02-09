// Decompiled with JetBrains decompiler
// Type: Rewired.UI.ControlMapper.InputFieldInfo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Rewired.UI.ControlMapper;

[AddComponentMenu("")]
public class InputFieldInfo : UIElementInfo
{
  [CompilerGenerated]
  public int \u003CactionId\u003Ek__BackingField;
  [CompilerGenerated]
  public AxisRange \u003CaxisRange\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CactionElementMapId\u003Ek__BackingField;
  [CompilerGenerated]
  public ControllerType \u003CcontrollerType\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CcontrollerId\u003Ek__BackingField;

  public int actionId
  {
    get => this.\u003CactionId\u003Ek__BackingField;
    set => this.\u003CactionId\u003Ek__BackingField = value;
  }

  public AxisRange axisRange
  {
    get => this.\u003CaxisRange\u003Ek__BackingField;
    set => this.\u003CaxisRange\u003Ek__BackingField = value;
  }

  public int actionElementMapId
  {
    get => this.\u003CactionElementMapId\u003Ek__BackingField;
    set => this.\u003CactionElementMapId\u003Ek__BackingField = value;
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
