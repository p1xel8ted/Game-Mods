// Decompiled with JetBrains decompiler
// Type: GameLogo
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class GameLogo : MonoBehaviour
{
  [SerializeField]
  public GameObject logo_vanilla;
  [SerializeField]
  public GameObject logo_dlc;
  [SerializeField]
  public GameObject logo_dlc_texted;

  public void Show(GameLogo.GameLogoType game_logo_type)
  {
    this.logo_vanilla.SetActive(false);
    this.logo_dlc.SetActive(false);
    this.logo_dlc_texted.SetActive(false);
    switch (game_logo_type)
    {
      case GameLogo.GameLogoType.Vanilla:
        this.logo_vanilla.SetActive(true);
        break;
      case GameLogo.GameLogoType.DLC:
        this.logo_dlc.SetActive(true);
        break;
      case GameLogo.GameLogoType.DLC_Texted:
        this.logo_dlc_texted.SetActive(true);
        break;
    }
  }

  public enum GameLogoType
  {
    Vanilla,
    DLC,
    DLC_Texted,
  }
}
