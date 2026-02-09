// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveJoystickControl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveJoystickControl(
  string controlID,
  InteractiveEventType type,
  bool enabled,
  string helpText,
  string eTag,
  string sceneID,
  Dictionary<string, object> metaproperties) : InteractiveControl(controlID, "joystick", type, enabled, helpText, eTag, sceneID, metaproperties)
{
  [CompilerGenerated]
  public double \u003CIntensity\u003Ek__BackingField;
  public uint _userID;

  public double X
  {
    get
    {
      double x = 0.0;
      _InternalJoystickState internalJoystickState;
      if (this.ControlID != null && InteractivityManager._joystickStates.TryGetValue(this.ControlID, out internalJoystickState))
        x = internalJoystickState.X;
      return x;
    }
  }

  public double Y
  {
    get
    {
      double y = 0.0;
      _InternalJoystickState internalJoystickState;
      if (this.ControlID != null && InteractivityManager._joystickStates.TryGetValue(this.ControlID, out internalJoystickState))
        y = internalJoystickState.Y;
      return y;
    }
  }

  public double Intensity
  {
    get => this.\u003CIntensity\u003Ek__BackingField;
    set => this.\u003CIntensity\u003Ek__BackingField = value;
  }

  public double GetX(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetJoystickX(this.ControlID, userID);
  }

  public double GetY(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetJoystickY(this.ControlID, userID);
  }

  public bool TryGetJoystickStateByParticipant(
    uint userID,
    string controlID,
    out _InternalJoystickState joystickState)
  {
    joystickState = new _InternalJoystickState();
    bool stateByParticipant = false;
    Dictionary<string, _InternalJoystickState> dictionary;
    if (InteractivityManager._joystickStatesByParticipant.TryGetValue(userID, out dictionary) && dictionary.TryGetValue(controlID, out joystickState))
      stateByParticipant = true;
    return stateByParticipant;
  }
}
