// Decompiled with JetBrains decompiler
// Type: WaitingGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class WaitingGUI : BaseGUI
{
  public static string[] _params = new string[2]
  {
    "hp",
    "energy"
  };
  public const float ENERGY_K = 0.25f;
  public const float HP_K = 0.25f;
  public float anim_time = 1f;
  public UIPanel _panel;
  public WaitingGUI.State _state;
  public GameRes _added_res = new GameRes();
  public GJCommons.VoidDelegate _on_started_waiting;
  public GJCommons.VoidDelegate _on_ended_waiting;

  public override void Init()
  {
    this._panel = this.GetComponent<UIPanel>();
    base.Init();
  }

  public void Open(
    GJCommons.VoidDelegate on_started_waiting = null,
    GJCommons.VoidDelegate on_ended_waiting = null,
    GJCommons.VoidDelegate on_after_save = null)
  {
    if (this.is_shown)
      return;
    this._panel.alpha = 0.0f;
    this.Open();
    GUIElements.me.hud.Open();
    this._added_res.Clear();
    this._added_res.durability = 0.0f;
    this._on_started_waiting = on_started_waiting;
    this._on_ended_waiting = on_ended_waiting;
    this._state = WaitingGUI.State.PlayingAppearing;
    this._panel.ChangeAlpha(this._panel.alpha, 1f, this.anim_time, (GJCommons.VoidDelegate) (() =>
    {
      this._state = WaitingGUI.State.Waiting;
      this._on_started_waiting.TryInvoke();
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

  public override void Update()
  {
    base.Update();
    if (this._state != WaitingGUI.State.Waiting)
      return;
    WorldGameObject player = MainGame.me.player;
    GameSave save = MainGame.me.save;
    float num = 1f + MainGame.me.player.GetParam("sleep_k_add");
    if ((double) player.energy < (double) save.max_energy)
    {
      float energy = player.energy;
      player.energy += Time.deltaTime * 0.25f * num;
      if ((double) player.energy > (double) save.max_energy)
        player.energy = (float) save.max_energy;
      this._added_res.Add("energy", player.energy - energy);
    }
    if ((double) player.hp < (double) save.max_hp)
    {
      float hp = player.hp;
      player.hp += Time.deltaTime * 0.25f * num;
      if ((double) player.hp > (double) save.max_hp)
        player.hp = (float) save.max_hp;
      this._added_res.Add("hp", player.hp - hp);
    }
    if (!player.energy.EqualsOrMore((float) save.max_energy))
    {
      string str = WaitingGUI._params[1];
      if (this._added_res.GetInt(str) >= 1)
      {
        EffectBubblesManager.ShowStacked(player, new GameRes(str, this._added_res.Get(str)));
        this._added_res.Set(str, 0.0f);
      }
    }
    if (!player.hp.EqualsOrMore((float) save.max_hp))
    {
      string str = WaitingGUI._params[0];
      if (this._added_res.GetInt(str) >= 1)
      {
        EffectBubblesManager.ShowStacked(player, new GameRes(str, this._added_res.Get(str)));
        this._added_res.Set(str, 0.0f);
      }
    }
    if (!LazyInput.GetKeyDown(GameKey.Interaction))
      return;
    this.StopWaiting();
  }

  public override bool OnPressedBack()
  {
    this.StopWaiting();
    return true;
  }

  public void StopWaiting()
  {
    if (this._state != WaitingGUI.State.Waiting)
      return;
    this._state = WaitingGUI.State.PlayingDisappearing;
    Time.timeScale = 1f;
    Time.fixedDeltaTime = 0.0166666675f;
    this.button_tips.Clear();
    EffectBubblesManager.RemoveAllBubbles();
    this._panel.ChangeAlpha(this._panel.alpha, 0.0f, this.anim_time, (GJCommons.VoidDelegate) (() =>
    {
      this.Hide(false);
      MainGame.me.save.quests.CheckKeyQuests("stop_waiting");
      this._on_ended_waiting.TryInvoke();
    }));
  }

  [CompilerGenerated]
  public void \u003COpen\u003Eb__11_0()
  {
    this._state = WaitingGUI.State.Waiting;
    this._on_started_waiting.TryInvoke();
    Time.timeScale = 10f;
    Time.fixedDeltaTime = 0.0833333358f;
    this.button_tips.Print(new GameKeyTip(GameKey.Interaction, "wake up", gamepad_only: false));
    GC.Collect();
    Resources.UnloadUnusedAssets();
  }

  [CompilerGenerated]
  public void \u003CStopWaiting\u003Eb__14_0()
  {
    this.Hide(false);
    MainGame.me.save.quests.CheckKeyQuests("stop_waiting");
    this._on_ended_waiting.TryInvoke();
  }

  public enum State
  {
    Waiting,
    PlayingAppearing,
    PlayingDisappearing,
  }
}
