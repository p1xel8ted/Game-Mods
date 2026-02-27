// Decompiled with JetBrains decompiler
// Type: InputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using RewiredConsts;
using Steamworks;

#nullable disable
public class InputManager : Singleton<InputManager>
{
  public GeneralInputSource _general;
  public RewiredUIInputSource _ui;
  public RewiredGameplayInputSource _gameplay;
  public PhotoModeInputSource _photoMode;

  public static GeneralInputSource General => Singleton<InputManager>.Instance._general;

  public static RewiredUIInputSource UI => Singleton<InputManager>.Instance._ui;

  public static RewiredGameplayInputSource Gameplay => Singleton<InputManager>.Instance._gameplay;

  public static PhotoModeInputSource PhotoMode => Singleton<InputManager>.Instance._photoMode;

  public InputManager()
  {
    SteamInput.Init(false);
    this._ui = new RewiredUIInputSource();
    this._gameplay = new RewiredGameplayInputSource();
    this._photoMode = new PhotoModeInputSource();
    this._general = new GeneralInputSource();
  }
}
