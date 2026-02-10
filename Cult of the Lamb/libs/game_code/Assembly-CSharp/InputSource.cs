// Decompiled with JetBrains decompiler
// Type: InputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;
using src.UINavigator;
using System;
using Unify;
using Unify.Input;
using UnityEngine;

#nullable disable
public abstract class InputSource
{
  public UnifyManager unifyManager;

  public Player _rewiredPlayer
  {
    get
    {
      return ReInput.isReady && ReInput.players != null && (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null && (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer != (UnityEngine.Object) null ? MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer.rewiredPlayer : ReInput.players.GetPlayer(0);
    }
  }

  public InputSource()
  {
    this.unifyManager = UnifyManager.instance;
    this.unifyManager.OnUserControllerConnected += new UnifyManager.UserControllerConnected(this.OnUserControllerConnected);
    UserHelper.OnPlayerGamePadChanged += new UserHelper.PlayerGamePadChangedDelegate(this.HandlePlayerGamePadChanged);
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
  }

  public void OnControllerConnected(ControllerStatusChangedEventArgs args)
  {
    Debug.Log((object) ("Controller connected " + args.controllerId.ToString()));
  }

  void object.Finalize()
  {
    try
    {
      this.unifyManager.OnUserControllerConnected -= new UnifyManager.UserControllerConnected(this.OnUserControllerConnected);
    }
    finally
    {
      // ISSUE: explicit finalizer call
      base.Finalize();
    }
  }

  public void HandlePlayerGamePadChanged(int playerNo, User user) => this.UpdateRewiredPlayer();

  public void OnUserControllerConnected(int playerNo, User user, bool connected)
  {
    this.UpdateRewiredPlayer();
  }

  public void OnPlayerUserChanged(int playerNo, User was, User now) => this.UpdateRewiredPlayer();

  public virtual void UpdateRewiredPlayer()
  {
  }

  public virtual void OnLastActiveControllerChanged(Player player, Controller controller)
  {
  }

  public virtual void OnLastActiveControllerChanged()
  {
  }

  public bool GetButtonDown(int button, PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
    {
      for (int playerNo = 0; playerNo < 4; ++playerNo)
      {
        Player player = RewiredInputManager.GetPlayer(playerNo);
        if (player != null && player.GetButtonDown(button))
          return true;
      }
      return false;
    }
    return (UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.rewiredPlayer != null && playerFarming.rewiredPlayer.GetButtonDown(button);
  }

  public bool GetButtonDown(int button, Player player) => player.GetButtonDown(button);

  public bool GetButtonDown(int button, Joystick joystick) => joystick.GetButtonDown(button);

  public bool GetButtonHeld(int button, PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
    {
      for (int playerNo = 0; playerNo < 4; ++playerNo)
      {
        Player player = RewiredInputManager.GetPlayer(playerNo);
        if (player != null && player.GetButton(button))
          return true;
      }
      return false;
    }
    return (bool) (UnityEngine.Object) playerFarming && playerFarming.rewiredPlayer != null && playerFarming.rewiredPlayer.GetButton(button);
  }

  public bool GetButtonUp(int button, PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
    {
      for (int playerNo = 0; playerNo < 4; ++playerNo)
      {
        Player player = RewiredInputManager.GetPlayer(playerNo);
        if (player != null && player.GetButtonUp(button))
          return true;
      }
      return false;
    }
    bool buttonUp = (UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.rewiredPlayer != null && playerFarming.rewiredPlayer.GetButtonUp(button);
    if (buttonUp && button == 9 && button == 9)
      Debug.Log((object) $"Break here 2 {Time.realtimeSinceStartup.ToString()} {playerFarming.rewiredPlayer.id.ToString()} {playerFarming.gameObject.name}");
    return buttonUp;
  }

  public float GetAxis(int axis, PlayerFarming playerFarming = null)
  {
    if ((UnityEngine.Object) playerFarming == (UnityEngine.Object) null)
    {
      for (int playerNo = 0; playerNo < 4; ++playerNo)
      {
        Player player = RewiredInputManager.GetPlayer(playerNo);
        if (player != null)
        {
          float axis1 = player.GetAxis(axis);
          if ((double) axis1 != 0.0)
            return axis1;
        }
      }
      return 0.0f;
    }
    return (UnityEngine.Object) playerFarming != (UnityEngine.Object) null && playerFarming.rewiredPlayer != null ? playerFarming.rewiredPlayer.GetAxis(axis) : 0.0f;
  }
}
