// Decompiled with JetBrains decompiler
// Type: Flockade.FlockadeConstants
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;

#nullable disable
namespace Flockade;

public static class FlockadeConstants
{
  public const float INTRO_OUTRO_DURATION = 1f;
  public const Ease INTRO_OUTRO_EASING = Ease.Linear;
  public const float MENU_TRANSITION_DURATION = 0.5f;
  public const Ease MENU_ENTRY_EASING = Ease.InCubic;
  public const Ease MENU_EXIT_EASING = Ease.OutCubic;
  public const float INTRO_ELEMENTS_ENTRY_DURATION = 0.8333333f;
  public const Ease INTRO_ELEMENTS_ENTRY_EASING = Ease.OutCubic;
  public const float TURN_BOUND_ELEMENTS_ENTRY_EXIT_DURATION = 0.5833333f;
  public const Ease TURN_BOUND_ELEMENTS_ENTRY_EASING = Ease.OutQuad;
  public const Ease TURN_BOUND_ELEMENTS_EXIT_EASING = Ease.InQuad;
  public const float DELAY_BETWEEN_GAME_BOARD_TILES_DUELLING = 0.05f;
  public const string CLOSE_MENU_SOUND = "event:/ui/close_menu";
  public const string GAME_PIECE_MOVEMENT_SOUND = "event:/dlc/ui/flockade_minigame/piece_move";
  public const string GAME_PIECE_REPLACEMENT_SOUND = "event:/dlc/ui/flockade_minigame/piece_transform";
  public const string OPEN_MENU_SOUND = "event:/ui/open_menu";
}
