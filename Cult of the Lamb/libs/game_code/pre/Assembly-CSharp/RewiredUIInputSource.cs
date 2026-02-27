// Decompiled with JetBrains decompiler
// Type: RewiredUIInputSource
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class RewiredUIInputSource : CategoryInputSource
{
  protected override int Category => 1;

  public float GetHorizontalAxis() => this.GetAxis(35);

  public float GetVerticalAxis() => this.GetAxis(34);

  public bool GetAcceptButtonDown() => this.GetButtonDown(38);

  public bool GetAcceptButtonHeld() => this.GetButtonHeld(38);

  public bool GetAcceptButtonUp() => this.GetButtonUp(38);

  public bool GetCancelButtonDown() => this.GetButtonDown(39);

  public bool GetCancelButtonHeld() => this.GetButtonHeld(39);

  public bool GetCancelButtonUp() => this.GetButtonUp(39);

  public bool GetPageNavigateLeftDown() => this.GetButtonDown(43);

  public bool GetPageNavigateLeftHeld() => this.GetButtonHeld(43);

  public bool GetPageNavigateRightHeld() => this.GetButtonHeld(44);

  public bool GetPageNavigateRightDown() => this.GetButtonDown(44);

  public bool GetResetAllSettingsButtonDown() => this.GetButtonDown(49);

  public bool GetAccountPickerButtonDown() => this.GetButtonDown(52);

  public bool GetEditBuildingsButtonDown() => this.GetButtonDown(51);

  public bool GetCookButtonDown() => this.GetButtonDown(56);

  public bool GetCancelBindingButtonDown() => this.GetButtonDown(61);

  public bool GetResetBindingButtonDown() => this.GetButtonDown(60);

  public bool GetUnbindButtonDown() => this.GetButtonDown(65);
}
