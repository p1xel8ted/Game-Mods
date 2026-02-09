// Decompiled with JetBrains decompiler
// Type: SleepGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class SleepGUI : BaseGUI
{
  public static string[] _params = new string[2]
  {
    "hp",
    "energy"
  };
  public const float ENERGY_K = 0.75f;
  public const float SANITY_K = 0.75f;
  public const float HP_K = 0.5f;
  public float anim_time = 1f;
  public Transform bubble_pos_tf;
  public UI2DSprite bed_sprite;
  public UIPanel _panel;
  public GJCommons.VoidDelegate _on_wake_up;
  public GJCommons.VoidDelegate _on_sleeped;
  public GJCommons.VoidDelegate _on_after_save;
  public SleepGUI.State _state;
  public GameRes _added_res = new GameRes();
  public float _normal_fixed_timestep = 0.008333334f;

  public Vector3 bubble_pos
  {
    get
    {
      return MainGame.me.world_cam.ScreenToWorldPoint(MainGame.me.gui_cam.WorldToScreenPoint(this.bubble_pos_tf.position));
    }
  }

  public override void Init()
  {
    this._panel = this.GetComponent<UIPanel>();
    base.Init();
  }

  public void Open(
    GJCommons.VoidDelegate on_appeared = null,
    GJCommons.VoidDelegate on_wake_up = null,
    GJCommons.VoidDelegate on_doesnt_need_sleep = null,
    GJCommons.VoidDelegate on_after_save = null)
  {
    if (this.is_shown)
      return;
    this.bed_sprite.sprite2D = WorldMap.GetWorldGameObjectByCustomTag("hero_bed").GetComponentInChildren<SpriteRenderer>().sprite;
    if (MainGame.me.player.energy.EqualsTo((float) MainGame.me.save.max_energy) && MainGame.me.player.sanity.EqualsTo((float) MainGame.me.save.max_sanity))
    {
      on_doesnt_need_sleep.TryInvoke();
    }
    else
    {
      this._panel.alpha = 0.0f;
      this.Open();
      GUIElements.me.hud.Open();
      this._added_res.Clear();
      this._added_res.durability = 0.0f;
      this._on_wake_up = on_appeared;
      this._on_sleeped = on_wake_up;
      this._on_after_save = on_after_save;
      this._state = SleepGUI.State.PlayingAppearing;
      this._panel.ChangeAlpha(this._panel.alpha, 1f, this.anim_time, (GJCommons.VoidDelegate) (() =>
      {
        this._state = SleepGUI.State.Sleeping;
        double timeScale = (double) Time.timeScale;
        this._on_wake_up.TryInvoke();
        Time.timeScale = 10f;
        Time.fixedDeltaTime = 0.0833333358f;
        this.button_tips.Print(new GameKeyTip(GameKey.Interaction, "wake up", gamepad_only: false));
        GC.Collect();
        Resources.UnloadUnusedAssets();
      }));
      this.button_tips.Clear();
      MainGame.me.player.ClearTiredness();
      BuffsLogics.RemoveBuff("buff_tired");
    }
  }

  public override void Update()
  {
    base.Update();
    if (this._state != SleepGUI.State.Sleeping)
      return;
    WorldGameObject player = MainGame.me.player;
    GameSave save = MainGame.me.save;
    float num = 1f + MainGame.me.player.GetParam("sleep_k_add");
    if ((double) player.energy < (double) save.max_energy)
    {
      float energy = player.energy;
      player.energy += Time.deltaTime * 0.75f * num;
      if ((double) player.energy > (double) save.max_energy)
        player.energy = (float) save.max_energy;
      this._added_res.Add("energy", player.energy - energy);
    }
    if ((double) player.hp < (double) save.max_hp)
    {
      float hp = player.hp;
      player.hp += Time.deltaTime * 0.5f * num;
      if ((double) player.hp > (double) save.max_hp)
        player.hp = (float) save.max_hp;
      this._added_res.Add("hp", player.hp - hp);
    }
    if (player.energy.EqualsOrMore((float) save.max_energy) && player.hp.EqualsOrMore((float) save.max_hp) || LazyInput.GetKeyDown(GameKey.Interaction))
    {
      this.WakeUp();
    }
    else
    {
      foreach (string str in SleepGUI._params)
      {
        if (this._added_res.GetInt(str) >= 1)
        {
          EffectBubblesManager.ShowStacked(player, new GameRes(str, this._added_res.Get(str)));
          this._added_res.Set(str, 0.0f);
        }
      }
    }
  }

  public override bool OnPressedBack()
  {
    this.WakeUp();
    return true;
  }

  public void WakeUp()
  {
    if (this._state != SleepGUI.State.Sleeping)
      return;
    this._state = SleepGUI.State.PlayingDisappearing;
    Time.timeScale = 1f;
    Time.fixedDeltaTime = 0.0166666675f;
    this.button_tips.Clear();
    EffectBubblesManager.RemoveAllBubbles();
    PlatformSpecific.SaveGame(MainGame.me.save_slot, MainGame.me.save, (PlatformSpecific.OnSaveCompleteDelegate) (slot =>
    {
      Debug.Log((object) "Autosave: done");
      this._on_after_save.TryInvoke();
      this._panel.ChangeAlpha(this._panel.alpha, 0.0f, this.anim_time, (GJCommons.VoidDelegate) (() =>
      {
        this.Hide(false);
        MainGame.me.save.quests.CheckKeyQuests("wake_up");
        this._on_sleeped.TryInvoke();
      }));
    }));
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__18_0()
  {
    this._state = SleepGUI.State.Sleeping;
    double timeScale = (double) Time.timeScale;
    this._on_wake_up.TryInvoke();
    Time.timeScale = 10f;
    Time.fixedDeltaTime = 0.0833333358f;
    this.button_tips.Print(new GameKeyTip(GameKey.Interaction, "wake up", gamepad_only: false));
    GC.Collect();
    Resources.UnloadUnusedAssets();
  }

  [CompilerGenerated]
  public void \u003CWakeUp\u003Eb__21_0(SaveSlotData slot)
  {
    Debug.Log((object) "Autosave: done");
    this._on_after_save.TryInvoke();
    this._panel.ChangeAlpha(this._panel.alpha, 0.0f, this.anim_time, (GJCommons.VoidDelegate) (() =>
    {
      this.Hide(false);
      MainGame.me.save.quests.CheckKeyQuests("wake_up");
      this._on_sleeped.TryInvoke();
    }));
  }

  [CompilerGenerated]
  public void \u003CWakeUp\u003Eb__21_1()
  {
    this.Hide(false);
    MainGame.me.save.quests.CheckKeyQuests("wake_up");
    this._on_sleeped.TryInvoke();
  }

  public enum State
  {
    Sleeping,
    PlayingAppearing,
    PlayingDisappearing,
  }
}
