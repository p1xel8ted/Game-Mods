// Decompiled with JetBrains decompiler
// Type: Rewired.Platforms.Switch.NintendoSwitchInputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired.Utils.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Rewired.Platforms.Switch;

[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
[RequireComponent(typeof (InputManager))]
public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
{
  [SerializeField]
  private NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

  object IExternalInputManager.Initialize(Platform platform, object configVars) => (object) null;

  void IExternalInputManager.Deinitialize()
  {
  }

  [Serializable]
  private class UserData : IKeyedData<int>
  {
    [SerializeField]
    private int _allowedNpadStyles = -1;
    [SerializeField]
    private int _joyConGripStyle = 1;
    [SerializeField]
    private bool _adjustIMUsForGripStyle = true;
    [SerializeField]
    private int _handheldActivationMode;
    [SerializeField]
    private bool _assignJoysticksByNpadId = true;
    [SerializeField]
    private bool _useVibrationThread = true;
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);
    [SerializeField]
    private NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);
    [SerializeField]
    private NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);
    private Dictionary<int, object[]> __delegates;

    public int allowedNpadStyles
    {
      get => this._allowedNpadStyles;
      set => this._allowedNpadStyles = value;
    }

    public int joyConGripStyle
    {
      get => this._joyConGripStyle;
      set => this._joyConGripStyle = value;
    }

    public bool adjustIMUsForGripStyle
    {
      get => this._adjustIMUsForGripStyle;
      set => this._adjustIMUsForGripStyle = value;
    }

    public int handheldActivationMode
    {
      get => this._handheldActivationMode;
      set => this._handheldActivationMode = value;
    }

    public bool assignJoysticksByNpadId
    {
      get => this._assignJoysticksByNpadId;
      set => this._assignJoysticksByNpadId = value;
    }

    public bool useVibrationThread
    {
      get => this._useVibrationThread;
      set => this._useVibrationThread = value;
    }

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo1 => this._npadNo1;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo2 => this._npadNo2;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo3 => this._npadNo3;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo4 => this._npadNo4;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo5 => this._npadNo5;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo6 => this._npadNo6;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo7 => this._npadNo7;

    private NintendoSwitchInputManager.NpadSettings_Internal npadNo8 => this._npadNo8;

    private NintendoSwitchInputManager.NpadSettings_Internal npadHandheld => this._npadHandheld;

    public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad => this._debugPad;

    private Dictionary<int, object[]> delegates
    {
      get
      {
        if (this.__delegates != null)
          return this.__delegates;
        return this.__delegates = new Dictionary<int, object[]>()
        {
          {
            0,
            new object[2]
            {
              (object) (Func<int>) (() => this.allowedNpadStyles),
              (object) (Action<int>) (x => this.allowedNpadStyles = x)
            }
          },
          {
            1,
            new object[2]
            {
              (object) (Func<int>) (() => this.joyConGripStyle),
              (object) (Action<int>) (x => this.joyConGripStyle = x)
            }
          },
          {
            2,
            new object[2]
            {
              (object) (Func<bool>) (() => this.adjustIMUsForGripStyle),
              (object) (Action<bool>) (x => this.adjustIMUsForGripStyle = x)
            }
          },
          {
            3,
            new object[2]
            {
              (object) (Func<int>) (() => this.handheldActivationMode),
              (object) (Action<int>) (x => this.handheldActivationMode = x)
            }
          },
          {
            4,
            new object[2]
            {
              (object) (Func<bool>) (() => this.assignJoysticksByNpadId),
              (object) (Action<bool>) (x => this.assignJoysticksByNpadId = x)
            }
          },
          {
            5,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo1),
              null
            }
          },
          {
            6,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo2),
              null
            }
          },
          {
            7,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo3),
              null
            }
          },
          {
            8,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo4),
              null
            }
          },
          {
            9,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo5),
              null
            }
          },
          {
            10,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo6),
              null
            }
          },
          {
            11,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo7),
              null
            }
          },
          {
            12,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadNo8),
              null
            }
          },
          {
            13,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.npadHandheld),
              null
            }
          },
          {
            14,
            new object[2]
            {
              (object) (Func<object>) (() => (object) this.debugPad),
              null
            }
          },
          {
            15,
            new object[2]
            {
              (object) (Func<bool>) (() => this.useVibrationThread),
              (object) (Action<bool>) (x => this.useVibrationThread = x)
            }
          }
        };
      }
    }

    bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray))
      {
        value = default (T);
        return false;
      }
      if (!(objArray[0] is Func<T> func))
      {
        value = default (T);
        return false;
      }
      value = func();
      return true;
    }

    bool IKeyedData<int>.TrySetValue<T>(int key, T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray) || !(objArray[1] is Action<T> action))
        return false;
      action(value);
      return true;
    }
  }

  [Serializable]
  private sealed class NpadSettings_Internal : IKeyedData<int>
  {
    [Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
    [SerializeField]
    private bool _isAllowed = true;
    [Tooltip("The Rewired Player Id assigned to this Npad id.")]
    [SerializeField]
    private int _rewiredPlayerId;
    [Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
    [SerializeField]
    private int _joyConAssignmentMode = -1;
    private Dictionary<int, object[]> __delegates;

    private bool isAllowed
    {
      get => this._isAllowed;
      set => this._isAllowed = value;
    }

    private int rewiredPlayerId
    {
      get => this._rewiredPlayerId;
      set => this._rewiredPlayerId = value;
    }

    private int joyConAssignmentMode
    {
      get => this._joyConAssignmentMode;
      set => this._joyConAssignmentMode = value;
    }

    internal NpadSettings_Internal(int playerId) => this._rewiredPlayerId = playerId;

    private Dictionary<int, object[]> delegates
    {
      get
      {
        if (this.__delegates != null)
          return this.__delegates;
        return this.__delegates = new Dictionary<int, object[]>()
        {
          {
            0,
            new object[2]
            {
              (object) (Func<bool>) (() => this.isAllowed),
              (object) (Action<bool>) (x => this.isAllowed = x)
            }
          },
          {
            1,
            new object[2]
            {
              (object) (Func<int>) (() => this.rewiredPlayerId),
              (object) (Action<int>) (x => this.rewiredPlayerId = x)
            }
          },
          {
            2,
            new object[2]
            {
              (object) (Func<int>) (() => this.joyConAssignmentMode),
              (object) (Action<int>) (x => this.joyConAssignmentMode = x)
            }
          }
        };
      }
    }

    bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray))
      {
        value = default (T);
        return false;
      }
      if (!(objArray[0] is Func<T> func))
      {
        value = default (T);
        return false;
      }
      value = func();
      return true;
    }

    bool IKeyedData<int>.TrySetValue<T>(int key, T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray) || !(objArray[1] is Action<T> action))
        return false;
      action(value);
      return true;
    }
  }

  [Serializable]
  private sealed class DebugPadSettings_Internal : IKeyedData<int>
  {
    [Tooltip("Determines whether the Debug Pad will be enabled.")]
    [SerializeField]
    private bool _enabled;
    [Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
    [SerializeField]
    private int _rewiredPlayerId;
    private Dictionary<int, object[]> __delegates;

    private int rewiredPlayerId
    {
      get => this._rewiredPlayerId;
      set => this._rewiredPlayerId = value;
    }

    private bool enabled
    {
      get => this._enabled;
      set => this._enabled = value;
    }

    internal DebugPadSettings_Internal(int playerId) => this._rewiredPlayerId = playerId;

    private Dictionary<int, object[]> delegates
    {
      get
      {
        if (this.__delegates != null)
          return this.__delegates;
        return this.__delegates = new Dictionary<int, object[]>()
        {
          {
            0,
            new object[2]
            {
              (object) (Func<bool>) (() => this.enabled),
              (object) (Action<bool>) (x => this.enabled = x)
            }
          },
          {
            1,
            new object[2]
            {
              (object) (Func<int>) (() => this.rewiredPlayerId),
              (object) (Action<int>) (x => this.rewiredPlayerId = x)
            }
          }
        };
      }
    }

    bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray))
      {
        value = default (T);
        return false;
      }
      if (!(objArray[0] is Func<T> func))
      {
        value = default (T);
        return false;
      }
      value = func();
      return true;
    }

    bool IKeyedData<int>.TrySetValue<T>(int key, T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray) || !(objArray[1] is Action<T> action))
        return false;
      action(value);
      return true;
    }
  }
}
