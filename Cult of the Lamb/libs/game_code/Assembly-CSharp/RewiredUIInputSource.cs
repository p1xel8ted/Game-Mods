// Decompiled with JetBrains decompiler
// Type: RewiredUIInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Rewired;

#nullable disable
public class RewiredUIInputSource : CategoryInputSource
{
  public override int Category => 1;

  public static int[] AllBindings
  {
    get => new int[7]{ 34, 35, 43, 44, 38, 39, 56 };
  }

  public float GetHorizontalAxis(PlayerFarming playerFarming = null)
  {
    return this.GetAxis(35, playerFarming);
  }

  public float GetVerticalAxis(PlayerFarming playerFarming = null)
  {
    return this.GetAxis(34, playerFarming);
  }

  public bool GetAcceptButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(38, playerFarming);
  }

  public bool GetAcceptButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(38, playerFarming);
  }

  public bool GetAcceptButtonUp(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(38, playerFarming);
  }

  public bool GetCancelButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(39, playerFarming);
  }

  public bool GetCancelButtonHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(39, playerFarming);
  }

  public bool GetCancelButtonUp(PlayerFarming playerFarming = null)
  {
    return this.GetButtonUp(39, playerFarming);
  }

  public bool GetPageNavigateLeftDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(43, playerFarming);
  }

  public bool GetPageNavigateLeftDown(Player player) => this.GetButtonDown(43, player);

  public bool GetPageNavigateLeftDown(Joystick joystick) => this.GetButtonDown(43, joystick);

  public bool GetPageNavigateLeftHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(43, playerFarming);
  }

  public bool GetPageNavigateRightHeld(PlayerFarming playerFarming = null)
  {
    return this.GetButtonHeld(44, playerFarming);
  }

  public bool GetPageNavigateRightDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(44, playerFarming);
  }

  public bool GetPageNavigateRightDown(Player player) => this.GetButtonDown(44, player);

  public bool GetPageNavigateRightDown(Joystick joystick) => this.GetButtonDown(44, joystick);

  public bool GetResetAllSettingsButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(49, playerFarming);
  }

  public bool GetAccountPickerButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(52, playerFarming);
  }

  public bool GetEditBuildingsButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(51, playerFarming);
  }

  public bool GetCookButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(56, playerFarming);
  }

  public bool GetCancelBindingButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(61, playerFarming);
  }

  public bool GetResetBindingButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(60, playerFarming);
  }

  public bool GetUnbindButtonDown(PlayerFarming playerFarming = null)
  {
    return this.GetButtonDown(65, playerFarming);
  }
}
