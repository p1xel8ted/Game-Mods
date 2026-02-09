// Decompiled with JetBrains decompiler
// Type: DungeonLevelGUIItem
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using DungeonGenerator;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DungeonLevelGUIItem : MonoBehaviour
{
  public UILabel label;
  public UI2DSprite bg_sprite;
  public UIButton button;
  public GamepadNavigationItem gamepad_navigation;
  public int dungeon_level;
  public DungeonLevelGUIItem.ButtonState state = DungeonLevelGUIItem.ButtonState.Inactive;

  public void SetDungeonLevel(int t_level)
  {
    if (t_level < 1)
    {
      Debug.LogError((object) "Wrong dungeon level!");
    }
    else
    {
      this.dungeon_level = t_level;
      SavedDungeon savedDungeon = MainGame.me.save.dungeons.GetSavedDungeon(this.dungeon_level);
      this.label.text = this.dungeon_level.ToString();
      if (MainGame.me.dungeon_root.dungeon_is_loaded_now && MainGame.me.dungeon_root.cur_saved_dungeon == savedDungeon)
      {
        this.SetState(DungeonLevelGUIItem.ButtonState.Selected);
      }
      else
      {
        bool flag = false;
        if (this.dungeon_level != 1 && !savedDungeon.is_completed)
        {
          if (MainGame.me.save.dungeons.GetSavedDungeon(this.dungeon_level - 1).is_completed)
          {
            for (int index = 0; index < DungeonWindowGUI.NEED_TO_BE_UNLOCKED.Length; ++index)
            {
              if (DungeonWindowGUI.NEED_TO_BE_UNLOCKED[index] == this.dungeon_level)
              {
                if (MainGame.me.player.GetParamInt("dungeon_unlocked_" + this.dungeon_level.ToString()) == 0)
                {
                  flag = true;
                  break;
                }
                break;
              }
            }
          }
          else
            flag = true;
        }
        this.SetState(flag ? DungeonLevelGUIItem.ButtonState.Inactive : DungeonLevelGUIItem.ButtonState.Active);
      }
      this.gamepad_navigation.SetCallbacks((GJCommons.VoidDelegate) null, (GJCommons.VoidDelegate) null, new GJCommons.VoidDelegate(this.OnPressed));
    }
  }

  public void SetState(DungeonLevelGUIItem.ButtonState t_state)
  {
    this.state = t_state;
    this.bg_sprite.alpha = this.state == DungeonLevelGUIItem.ButtonState.Inactive ? 0.5f : 1f;
    this.button.enabled = this.state == DungeonLevelGUIItem.ButtonState.Active;
  }

  public void OnPressed()
  {
    if (this.state != DungeonLevelGUIItem.ButtonState.Active)
      return;
    this.OnEnterToDungeon();
  }

  public void OnEnterToDungeon()
  {
    Debug.Log((object) ("Enter to dungeon #" + this.dungeon_level.ToString()));
    GUIElements.ChangeBubblesVisibility(false);
    MainGame.me.player.components.character.control_enabled = false;
    GUIElements.me.dungeon_window.OnClosePressed();
    CameraTools.Fade((GJCommons.VoidDelegate) (() =>
    {
      MainGame.me.TeleportToDungeonLevel(this.dungeon_level);
      CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
      {
        MainGame.me.player.components.character.control_enabled = true;
        GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
      }), new float?(0.5f));
    }), new float?(0.5f));
  }

  [CompilerGenerated]
  public void \u003COnEnterToDungeon\u003Eb__9_0()
  {
    MainGame.me.TeleportToDungeonLevel(this.dungeon_level);
    CameraTools.UnFade((GJCommons.VoidDelegate) (() =>
    {
      MainGame.me.player.components.character.control_enabled = true;
      GUIElements.ChangeBubblesVisibility(MainGame.me.player_char.control_enabled);
    }), new float?(0.5f));
  }

  public enum ButtonState
  {
    Active,
    Selected,
    Inactive,
  }
}
