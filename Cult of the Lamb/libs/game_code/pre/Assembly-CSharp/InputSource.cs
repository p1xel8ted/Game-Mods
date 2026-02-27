// Decompiled with JetBrains decompiler
// Type: InputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Rewired;
using System;
using Unify;

#nullable disable
public abstract class InputSource
{
  private UnifyManager unifyManager;

  protected Player _rewiredPlayer
  {
    get
    {
      return ReInput.isReady && ReInput.players != null ? ReInput.players.GetPlayer(0) : (Player) null;
    }
  }

  protected InputSource()
  {
    this.unifyManager = UnifyManager.instance;
    this.unifyManager.OnUserControllerConnected += new UnifyManager.UserControllerConnected(this.OnUserControllerConnected);
    UserHelper.OnPlayerGamePadChanged += new UserHelper.PlayerGamePadChangedDelegate(this.HandlePlayerGamePadChanged);
    ReInput.ControllerConnectedEvent += (Action<ControllerStatusChangedEventArgs>) new Action<ControllerStatusChangedEventArgs>(this.OnControllerConnected);
  }

  private void OnControllerConnected(ControllerStatusChangedEventArgs args)
  {
    this._rewiredPlayer.controllers.AddController((Controller) ReInput.controllers.GetJoystick(args.controllerId), true);
  }

  ~InputSource()
  {
    this.unifyManager.OnUserControllerConnected -= new UnifyManager.UserControllerConnected(this.OnUserControllerConnected);
  }

  private void HandlePlayerGamePadChanged(int playerNo, User user) => this.UpdateRewiredPlayer();

  public void OnUserControllerConnected(int playerNo, User user, bool connected)
  {
    this.UpdateRewiredPlayer();
  }

  public void OnPlayerUserChanged(int playerNo, User was, User now) => this.UpdateRewiredPlayer();

  protected virtual void UpdateRewiredPlayer()
  {
    this.OnLastActiveControllerChanged();
    this._rewiredPlayer.controllers.AddLastActiveControllerChangedDelegate(new PlayerActiveControllerChangedDelegate(this.OnLastActiveControllerChanged));
  }

  protected virtual void OnLastActiveControllerChanged(Player player, Controller controller)
  {
  }

  protected virtual void OnLastActiveControllerChanged()
  {
  }

  protected bool GetButtonDown(int button)
  {
    return this._rewiredPlayer != null && this._rewiredPlayer.GetButtonDown(button);
  }

  protected bool GetButtonHeld(int button)
  {
    return this._rewiredPlayer != null && this._rewiredPlayer.GetButton(button);
  }

  protected bool GetButtonUp(int button)
  {
    return this._rewiredPlayer != null && this._rewiredPlayer.GetButtonUp(button);
  }

  protected float GetAxis(int axis)
  {
    return this._rewiredPlayer != null ? this._rewiredPlayer.GetAxis(axis) : 0.0f;
  }
}
