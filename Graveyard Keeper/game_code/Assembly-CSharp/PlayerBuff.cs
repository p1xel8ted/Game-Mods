// Decompiled with JetBrains decompiler
// Type: PlayerBuff
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
[Serializable]
public class PlayerBuff
{
  public string buff_id;
  public float end_time;
  [SerializeField]
  public float _tick_time;

  public BuffDefinition definition => GameBalance.me.GetData<BuffDefinition>(this.buff_id);

  public string GetTimerText()
  {
    float num1 = this.end_time - MainGame.game_time;
    if ((double) num1 < 0.0)
      return "0:00";
    float num2 = num1 * 450f;
    int num3 = Mathf.FloorToInt(num2 / 60f);
    int num4 = Mathf.FloorToInt(num2 - (float) (num3 * 60));
    int num5 = Mathf.FloorToInt((float) num3 / 60f);
    int num6 = num3 - num5 * 60;
    return string.Format(num5 == 0 ? "{0:0}:{1:00}" : "{2:0}:{0:00}:{1:00}", (object) num6, (object) num4, (object) num5);
  }

  public void CustomUpdate(float delta_time)
  {
    if (EnvironmentEngine.me.IsTimeStopped())
      return;
    this._tick_time += delta_time;
    BuffDefinition definition = this.definition;
    if (definition.tick_period.EqualsTo(0.0f) || (double) this._tick_time <= (double) definition.tick_period)
      return;
    this._tick_time -= definition.tick_period;
    GameRes gameRes = MainGame.me.player.data.GetParams().Clone();
    definition.se_tick.Evaluate();
    GameRes res = MainGame.me.player.data.GetParams() - gameRes;
    res.RemoveAllBut(new List<string>()
    {
      "hp",
      "energy",
      "money"
    });
    if (res.IsEmpty() || MainGame.me.player.is_dead)
      return;
    EffectBubblesManager.ShowImmediately(MainGame.me.player.bubble_pos, res);
  }
}
