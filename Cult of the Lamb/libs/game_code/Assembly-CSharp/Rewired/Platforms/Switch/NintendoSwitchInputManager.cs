// Decompiled with JetBrains decompiler
// Type: Rewired.Platforms.Switch.NintendoSwitchInputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired.Utils.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
namespace Rewired.Platforms.Switch;

[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
[RequireComponent(typeof (InputManager))]
public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
{
  [SerializeField]
  public NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

  object IExternalInputManager.Initialize(Platform platform, object configVars) => (object) null;

  void IExternalInputManager.Deinitialize()
  {
  }

  [Serializable]
  public class UserData : IKeyedData<int>
  {
    [SerializeField]
    public int _allowedNpadStyles = -1;
    [SerializeField]
    public int _joyConGripStyle = 1;
    [SerializeField]
    public bool _adjustIMUsForGripStyle = true;
    [SerializeField]
    public int _handheldActivationMode;
    [SerializeField]
    public bool _assignJoysticksByNpadId = true;
    [SerializeField]
    public bool _useVibrationThread = true;
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);
    [SerializeField]
    public NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);
    [SerializeField]
    public NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);
    public Dictionary<int, object[]> __delegates;

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

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo1 => this._npadNo1;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo2 => this._npadNo2;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo3 => this._npadNo3;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo4 => this._npadNo4;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo5 => this._npadNo5;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo6 => this._npadNo6;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo7 => this._npadNo7;

    public NintendoSwitchInputManager.NpadSettings_Internal npadNo8 => this._npadNo8;

    public NintendoSwitchInputManager.NpadSettings_Internal npadHandheld => this._npadHandheld;

    public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad => this._debugPad;

    public Dictionary<int, object[]> delegates
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

    public bool Rewired\u002EUtils\u002EInterfaces\u002EIKeyedData\u003CSystem\u002EInt32\u003E\u002ETryGetValue<T>(
      int key,
      out T value)
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

    public bool Rewired\u002EUtils\u002EInterfaces\u002EIKeyedData\u003CSystem\u002EInt32\u003E\u002ETrySetValue<T>(
      int key,
      T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray) || !(objArray[1] is Action<T> action))
        return false;
      action(value);
      return true;
    }

    [CompilerGenerated]
    public int \u003Cget_delegates\u003Eb__56_0() => this.allowedNpadStyles;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__56_1(int x) => this.allowedNpadStyles = x;

    [CompilerGenerated]
    public int \u003Cget_delegates\u003Eb__56_2() => this.joyConGripStyle;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__56_3(int x) => this.joyConGripStyle = x;

    [CompilerGenerated]
    public bool \u003Cget_delegates\u003Eb__56_4() => this.adjustIMUsForGripStyle;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__56_5(bool x) => this.adjustIMUsForGripStyle = x;

    [CompilerGenerated]
    public int \u003Cget_delegates\u003Eb__56_6() => this.handheldActivationMode;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__56_7(int x) => this.handheldActivationMode = x;

    [CompilerGenerated]
    public bool \u003Cget_delegates\u003Eb__56_8() => this.assignJoysticksByNpadId;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__56_9(bool x) => this.assignJoysticksByNpadId = x;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_10() => (object) this.npadNo1;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_11() => (object) this.npadNo2;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_12() => (object) this.npadNo3;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_13() => (object) this.npadNo4;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_14() => (object) this.npadNo5;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_15() => (object) this.npadNo6;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_16() => (object) this.npadNo7;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_17() => (object) this.npadNo8;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_18() => (object) this.npadHandheld;

    [CompilerGenerated]
    public object \u003Cget_delegates\u003Eb__56_19() => (object) this.debugPad;

    [CompilerGenerated]
    public bool \u003Cget_delegates\u003Eb__56_20() => this.useVibrationThread;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__56_21(bool x) => this.useVibrationThread = x;
  }

  [Serializable]
  public sealed class NpadSettings_Internal : IKeyedData<int>
  {
    [Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
    [SerializeField]
    public bool _isAllowed = true;
    [Tooltip("The Rewired Player Id assigned to this Npad id.")]
    [SerializeField]
    public int _rewiredPlayerId;
    [Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
    [SerializeField]
    public int _joyConAssignmentMode = -1;
    public Dictionary<int, object[]> __delegates;

    public bool isAllowed
    {
      get => this._isAllowed;
      set => this._isAllowed = value;
    }

    public int rewiredPlayerId
    {
      get => this._rewiredPlayerId;
      set => this._rewiredPlayerId = value;
    }

    public int joyConAssignmentMode
    {
      get => this._joyConAssignmentMode;
      set => this._joyConAssignmentMode = value;
    }

    public NpadSettings_Internal(int playerId) => this._rewiredPlayerId = playerId;

    public Dictionary<int, object[]> delegates
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

    public bool Rewired\u002EUtils\u002EInterfaces\u002EIKeyedData\u003CSystem\u002EInt32\u003E\u002ETryGetValue<T>(
      int key,
      out T value)
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

    public bool Rewired\u002EUtils\u002EInterfaces\u002EIKeyedData\u003CSystem\u002EInt32\u003E\u002ETrySetValue<T>(
      int key,
      T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray) || !(objArray[1] is Action<T> action))
        return false;
      action(value);
      return true;
    }

    [CompilerGenerated]
    public bool \u003Cget_delegates\u003Eb__15_0() => this.isAllowed;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__15_1(bool x) => this.isAllowed = x;

    [CompilerGenerated]
    public int \u003Cget_delegates\u003Eb__15_2() => this.rewiredPlayerId;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__15_3(int x) => this.rewiredPlayerId = x;

    [CompilerGenerated]
    public int \u003Cget_delegates\u003Eb__15_4() => this.joyConAssignmentMode;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__15_5(int x) => this.joyConAssignmentMode = x;
  }

  [Serializable]
  public sealed class DebugPadSettings_Internal : IKeyedData<int>
  {
    [Tooltip("Determines whether the Debug Pad will be enabled.")]
    [SerializeField]
    public bool _enabled;
    [Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
    [SerializeField]
    public int _rewiredPlayerId;
    public Dictionary<int, object[]> __delegates;

    public int rewiredPlayerId
    {
      get => this._rewiredPlayerId;
      set => this._rewiredPlayerId = value;
    }

    public bool enabled
    {
      get => this._enabled;
      set => this._enabled = value;
    }

    public DebugPadSettings_Internal(int playerId) => this._rewiredPlayerId = playerId;

    public Dictionary<int, object[]> delegates
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

    public bool Rewired\u002EUtils\u002EInterfaces\u002EIKeyedData\u003CSystem\u002EInt32\u003E\u002ETryGetValue<T>(
      int key,
      out T value)
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

    public bool Rewired\u002EUtils\u002EInterfaces\u002EIKeyedData\u003CSystem\u002EInt32\u003E\u002ETrySetValue<T>(
      int key,
      T value)
    {
      object[] objArray;
      if (!this.delegates.TryGetValue(key, out objArray) || !(objArray[1] is Action<T> action))
        return false;
      action(value);
      return true;
    }

    [CompilerGenerated]
    public bool \u003Cget_delegates\u003Eb__11_0() => this.enabled;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__11_1(bool x) => this.enabled = x;

    [CompilerGenerated]
    public int \u003Cget_delegates\u003Eb__11_2() => this.rewiredPlayerId;

    [CompilerGenerated]
    public void \u003Cget_delegates\u003Eb__11_3(int x) => this.rewiredPlayerId = x;
  }
}
