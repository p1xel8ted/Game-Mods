// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveButtonControl
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveButtonControl : InteractiveControl
{
  [CompilerGenerated]
  public string \u003CButtonText\u003Ek__BackingField;
  [CompilerGenerated]
  public uint \u003CCost\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CProgress\u003Ek__BackingField;
  public long _cooldownExpirationTime;

  public string ButtonText
  {
    get => this.\u003CButtonText\u003Ek__BackingField;
    set => this.\u003CButtonText\u003Ek__BackingField = value;
  }

  public uint Cost
  {
    get => this.\u003CCost\u003Ek__BackingField;
    set => this.\u003CCost\u003Ek__BackingField = value;
  }

  public int RemainingCooldown
  {
    get
    {
      DateTime dateTime = DateTime.UtcNow;
      dateTime = dateTime.ToUniversalTime();
      int remainingCooldown = (int) (this._cooldownExpirationTime - (long) dateTime.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds);
      if (remainingCooldown < 0)
        remainingCooldown = 0;
      return remainingCooldown;
    }
  }

  public float Progress
  {
    get => this.\u003CProgress\u003Ek__BackingField;
    set => this.\u003CProgress\u003Ek__BackingField = value;
  }

  public bool ButtonDown
  {
    get
    {
      bool buttonDown = false;
      _InternalButtonCountState buttonCountState;
      if (this.ControlID != null && InteractivityManager._buttonStates.TryGetValue(this.ControlID, out buttonCountState))
        buttonDown = buttonCountState.CountOfButtonDownEvents > 0U;
      return buttonDown;
    }
  }

  public bool ButtonPressed
  {
    get
    {
      bool buttonPressed = false;
      _InternalButtonCountState buttonCountState;
      if (this.ControlID != null && InteractivityManager._buttonStates.TryGetValue(this.ControlID, out buttonCountState))
        buttonPressed = buttonCountState.CountOfButtonPressEvents > 0U;
      return buttonPressed;
    }
  }

  public bool ButtonUp
  {
    get
    {
      bool buttonUp = false;
      _InternalButtonCountState buttonCountState;
      if (this.ControlID != null && InteractivityManager._buttonStates.TryGetValue(this.ControlID, out buttonCountState))
        buttonUp = buttonCountState.CountOfButtonUpEvents > 0U;
      return buttonUp;
    }
  }

  public uint CountOfButtonDowns
  {
    get
    {
      uint countOfButtonDowns = 0;
      _InternalButtonCountState buttonCountState;
      if (this.ControlID != null && InteractivityManager._buttonStates.TryGetValue(this.ControlID, out buttonCountState))
        countOfButtonDowns = buttonCountState.CountOfButtonDownEvents;
      return countOfButtonDowns;
    }
  }

  public uint CountOfButtonPresses
  {
    get
    {
      uint countOfButtonPresses = 0;
      _InternalButtonCountState buttonCountState;
      if (this.ControlID != null && InteractivityManager._buttonStates.TryGetValue(this.ControlID, out buttonCountState))
        countOfButtonPresses = buttonCountState.CountOfButtonPressEvents;
      return countOfButtonPresses;
    }
  }

  public uint CountOfButtonUps
  {
    get
    {
      uint countOfButtonUps = 0;
      _InternalButtonCountState buttonCountState;
      if (this.ControlID != null && InteractivityManager._buttonStates.TryGetValue(this.ControlID, out buttonCountState))
        countOfButtonUps = buttonCountState.CountOfButtonUpEvents;
      return countOfButtonUps;
    }
  }

  public void SetProgress(float progress)
  {
    InteractivityManager.SingletonInstance._SendSetButtonControlProperties(this.ControlID, nameof (progress), false, progress, string.Empty, 0U);
  }

  public void SetText(string text)
  {
    InteractivityManager.SingletonInstance._SendSetButtonControlProperties(this.ControlID, nameof (text), false, 0.0f, text, 0U);
  }

  public void SetCost(uint cost)
  {
    InteractivityManager.SingletonInstance._SendSetButtonControlProperties(this.ControlID, nameof (cost), false, 0.0f, string.Empty, cost);
  }

  public bool GetButtonDown(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetButtonDown(this.ControlID, userID);
  }

  public bool GetButtonPressed(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetButtonPressed(this.ControlID, userID);
  }

  public bool GetButtonUp(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetButtonUp(this.ControlID, userID);
  }

  public uint GetCountOfButtonDowns(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetCountOfButtonDowns(this.ControlID, userID);
  }

  public uint GetCountOfButtonPresses(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetCountOfButtonPresses(this.ControlID, userID);
  }

  public uint GetCountOfButtonUps(uint userID)
  {
    return InteractivityManager.SingletonInstance._GetCountOfButtonUps(this.ControlID, userID);
  }

  public void TriggerCooldown(int cooldown)
  {
    InteractivityManager.SingletonInstance.TriggerCooldown(this.ControlID, cooldown);
  }

  public InteractiveButtonControl(
    string controlID,
    InteractiveEventType type,
    bool disabled,
    string helpText,
    uint cost,
    string eTag,
    string sceneID,
    Dictionary<string, object> metaproperties)
    : base(controlID, "button", type, disabled, helpText, eTag, sceneID, metaproperties)
  {
    this.Cost = cost;
  }
}
