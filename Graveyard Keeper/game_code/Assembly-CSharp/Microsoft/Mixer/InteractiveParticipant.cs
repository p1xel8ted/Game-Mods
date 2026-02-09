// Decompiled with JetBrains decompiler
// Type: Microsoft.Mixer.InteractiveParticipant
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Mixer;

[Serializable]
public class InteractiveParticipant
{
  [CompilerGenerated]
  public uint \u003CLevel\u003Ek__BackingField;
  [CompilerGenerated]
  public uint \u003CUserID\u003Ek__BackingField;
  [CompilerGenerated]
  public string \u003CUserName\u003Ek__BackingField;
  [CompilerGenerated]
  public DateTime \u003CConnectedAt\u003Ek__BackingField;
  [CompilerGenerated]
  public DateTime \u003CLastInputAt\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CInputDisabled\u003Ek__BackingField;
  [CompilerGenerated]
  public InteractiveParticipantState \u003CState\u003Ek__BackingField;
  public string _etag;
  public string _groupID;
  public string _sessionID;
  public List<string> channelGroups;

  public uint Level
  {
    get => this.\u003CLevel\u003Ek__BackingField;
    set => this.\u003CLevel\u003Ek__BackingField = value;
  }

  public uint UserID
  {
    get => this.\u003CUserID\u003Ek__BackingField;
    set => this.\u003CUserID\u003Ek__BackingField = value;
  }

  public string UserName
  {
    get => this.\u003CUserName\u003Ek__BackingField;
    set => this.\u003CUserName\u003Ek__BackingField = value;
  }

  public DateTime ConnectedAt
  {
    get => this.\u003CConnectedAt\u003Ek__BackingField;
    set => this.\u003CConnectedAt\u003Ek__BackingField = value;
  }

  public InteractiveGroup Group
  {
    get
    {
      foreach (InteractiveGroup group in InteractivityManager.SingletonInstance.Groups as List<InteractiveGroup>)
      {
        if (group.GroupID == this._groupID)
          return group;
      }
      return new InteractiveGroup("default");
    }
    set
    {
      if (value == null)
      {
        InteractivityManager.SingletonInstance._LogError("Error: You cannot assign 'null' as the group value.");
      }
      else
      {
        this._groupID = value.GroupID;
        InteractivityManager.SingletonInstance._SendUpdateParticipantsMessage(this);
      }
    }
  }

  public DateTime LastInputAt
  {
    get => this.\u003CLastInputAt\u003Ek__BackingField;
    set => this.\u003CLastInputAt\u003Ek__BackingField = value;
  }

  public bool IsBroadcaster => this.channelGroups.Contains("Owner");

  public bool InputDisabled
  {
    get => this.\u003CInputDisabled\u003Ek__BackingField;
    set => this.\u003CInputDisabled\u003Ek__BackingField = value;
  }

  public IList<InteractiveButtonControl> Buttons
  {
    get
    {
      List<InteractiveButtonControl> buttons = new List<InteractiveButtonControl>();
      Dictionary<string, _InternalButtonState> dictionary;
      if (InteractivityManager._buttonStatesByParticipant.TryGetValue(this.UserID, out dictionary))
      {
        foreach (string key in dictionary.Keys)
        {
          foreach (InteractiveButtonControl button in (IEnumerable<InteractiveButtonControl>) InteractivityManager.SingletonInstance.Buttons)
          {
            if (key == button.ControlID)
            {
              buttons.Add(button);
              break;
            }
          }
        }
      }
      return (IList<InteractiveButtonControl>) buttons;
    }
  }

  public IList<InteractiveJoystickControl> Joysticks
  {
    get
    {
      List<InteractiveJoystickControl> joysticks = new List<InteractiveJoystickControl>();
      Dictionary<string, _InternalJoystickState> dictionary;
      if (InteractivityManager._joystickStatesByParticipant.TryGetValue(this.UserID, out dictionary))
      {
        foreach (string key in dictionary.Keys)
        {
          foreach (InteractiveJoystickControl joystick in (IEnumerable<InteractiveJoystickControl>) InteractivityManager.SingletonInstance.Joysticks)
          {
            if (key == joystick.ControlID)
            {
              joysticks.Add(joystick);
              break;
            }
          }
        }
      }
      return (IList<InteractiveJoystickControl>) joysticks;
    }
  }

  public InteractiveParticipantState State
  {
    get => this.\u003CState\u003Ek__BackingField;
    set => this.\u003CState\u003Ek__BackingField = value;
  }

  public InteractiveParticipant(
    string newSessionID,
    string newEtag,
    uint userID,
    string newGroupID,
    string userName,
    List<string> newChannelGroups,
    uint level,
    DateTime lastInputAt,
    DateTime connectedAt,
    bool inputDisabled,
    InteractiveParticipantState state)
  {
    this._sessionID = newSessionID;
    this.UserID = userID;
    this.UserName = userName;
    this.channelGroups = newChannelGroups;
    this.Level = level;
    this.LastInputAt = lastInputAt;
    this.ConnectedAt = connectedAt;
    this.InputDisabled = inputDisabled;
    this.State = state;
    this._groupID = newGroupID;
    this._etag = newEtag;
  }
}
