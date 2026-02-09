// Decompiled with JetBrains decompiler
// Type: MainMenuLogoController
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MainMenuLogoController : MonoBehaviour
{
  [SerializeField]
  public GameLogo game_logo;
  [SerializeField]
  public DLCLogoController dlc_logo_controller;
  [SerializeField]
  public SimpleUITable _ui_table;

  public void ShowLogos()
  {
    this.ShowGameLogo();
    this.dlc_logo_controller.Show();
    this._ui_table.Reposition();
  }

  public void ShowGameLogo()
  {
    int num = DLCEngine.DLCAvailableCount();
    if (num == 0)
      this.game_logo.Show(GameLogo.GameLogoType.Vanilla);
    else if ((double) Screen.width / (double) Screen.height > 1.5)
    {
      if (Screen.width <= 1280 /*0x0500*/)
      {
        if (num > 2)
          this.game_logo.Show(GameLogo.GameLogoType.DLC_Texted);
        else
          this.game_logo.Show(GameLogo.GameLogoType.DLC);
      }
      else if (Screen.width <= 1440)
      {
        if (num == 4)
          this.game_logo.Show(GameLogo.GameLogoType.DLC_Texted);
        else
          this.game_logo.Show(GameLogo.GameLogoType.DLC);
      }
      else
        this.game_logo.Show(GameLogo.GameLogoType.DLC);
    }
    else
      this.game_logo.Show(GameLogo.GameLogoType.DLC);
  }
}
