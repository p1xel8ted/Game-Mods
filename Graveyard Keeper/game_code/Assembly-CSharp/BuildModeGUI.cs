// Decompiled with JetBrains decompiler
// Type: BuildModeGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class BuildModeGUI : BaseGUI
{
  public override void Open()
  {
    base.Open();
    InteractionBubbleGUI.ShowAllRemoveBubbles();
    MainGame.me.build_mode_logics.cur_build_zone.RedrawQualities(new bool?(true), true);
  }

  public void RedrawPlacing(bool can_place, bool can_be_rotated)
  {
    if (!this.gameObject.activeSelf)
      return;
    List<GameKeyTip> tips = new List<GameKeyTip>();
    if (can_be_rotated)
    {
      if (BaseGUI.for_gamepad)
      {
        tips.Add(new GameKeyTip(GameKey.RotateLeft, "rotate left"));
        tips.Add(new GameKeyTip(GameKey.RotateRight, "rotate right"));
      }
      else
        tips.Add(new GameKeyTip(GameKey.RotateRight, "rotate", gamepad_only: false));
    }
    tips.Add(GameKeyTip.Select("place", can_place));
    tips.Add(GameKeyTip.Back(gamepad_only: false));
    this.button_tips.Print(tips);
  }

  public void RedrawRemoving(bool waiting_for_removing, bool can_be_removed)
  {
    if (!this.gameObject.activeSelf)
      return;
    this.button_tips.Print(waiting_for_removing ? GameKeyTip.Select("cancel removing") : GameKeyTip.Select("remove", can_be_removed), GameKeyTip.Back(gamepad_only: false));
  }

  public void RedrawScriptMode(bool has_variation)
  {
    if (!this.gameObject.activeSelf)
      return;
    List<GameKeyTip> tips = new List<GameKeyTip>();
    if (has_variation)
    {
      if (BaseGUI.for_gamepad)
      {
        tips.Add(new GameKeyTip(GameKey.RotateLeft, "next"));
        tips.Add(new GameKeyTip(GameKey.RotateRight, "previous"));
      }
      else
        tips.Add(new GameKeyTip(GameKey.RotateRight, "next", gamepad_only: false));
    }
    if (BaseGUI.for_gamepad)
      tips.Add(GameKeyTip.Select("apply"));
    else
      tips.Add(new GameKeyTip(GameKey.LeftClick, "apply", gamepad_only: false));
    tips.Add(GameKeyTip.Back(gamepad_only: false));
    this.button_tips.Print(tips);
  }
}
