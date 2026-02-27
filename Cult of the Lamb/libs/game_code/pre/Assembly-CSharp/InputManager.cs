// Decompiled with JetBrains decompiler
// Type: InputManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Steamworks;

#nullable disable
public class InputManager : Singleton<InputManager>
{
  private GeneralInputSource _general;
  private RewiredUIInputSource _ui;
  private RewiredGameplayInputSource _gameplay;

  public static GeneralInputSource General => Singleton<InputManager>.Instance._general;

  public static RewiredUIInputSource UI => Singleton<InputManager>.Instance._ui;

  public static RewiredGameplayInputSource Gameplay => Singleton<InputManager>.Instance._gameplay;

  public InputManager()
  {
    SteamInput.Init(false);
    this._ui = new RewiredUIInputSource();
    this._gameplay = new RewiredGameplayInputSource();
    this._general = new GeneralInputSource();
  }
}
