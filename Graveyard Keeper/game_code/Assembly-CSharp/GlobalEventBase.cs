// Decompiled with JetBrains decompiler
// Type: GlobalEventBase
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class GlobalEventBase
{
  public DateTime event_start_time;
  public TimeSpan event_duration;
  public int period;
  public GlobalEventBase.GlobalEventPeriodType period_type;
  public IHasExecute on_start_script;
  public IHasExecute on_finish_script;
  public string game_res;

  public GlobalEventBase(
    string game_res,
    DateTime event_start_time,
    TimeSpan event_duration,
    int period = 1,
    GlobalEventBase.GlobalEventPeriodType period_type = GlobalEventBase.GlobalEventPeriodType.Years)
  {
    this.game_res = game_res;
    this.event_start_time = event_start_time;
    this.event_duration = event_duration;
    this.period_type = period_type;
    this.period = period;
  }

  public GlobalEventBase(
    string game_res,
    DateTime event_start_time,
    int event_duration,
    int period = 1,
    GlobalEventBase.GlobalEventPeriodType period_type = GlobalEventBase.GlobalEventPeriodType.Years)
    : this(game_res, event_start_time, new TimeSpan((long) event_duration), period, period_type)
  {
  }

  public void Process()
  {
    if (!this.IsProperTimeForEvent())
    {
      if (MainGame.me.player.GetParamInt(this.game_res) != 1)
        return;
      this.OnFinishEvent();
    }
    else
    {
      if (MainGame.me.player.GetParamInt(this.game_res) != 0)
        return;
      this.OnStartEvent();
    }
  }

  public bool IsProperTimeForEvent()
  {
    DateTime now = DateTime.Now;
    DateTime dateTime1 = this.event_start_time;
    DateTime dateTime2 = this.event_start_time.Add(this.event_duration);
    if (this.period_type != GlobalEventBase.GlobalEventPeriodType.Years)
      throw new ArgumentOutOfRangeException();
    int year = now.Year;
    dateTime1 = dateTime1.AddYears(year - dateTime1.Year);
    dateTime2 = dateTime2.AddYears(year - dateTime2.Year);
    return dateTime1 < now && now < dateTime2;
  }

  public void OnStartEvent()
  {
    this.on_start_script?.Execute();
    MainGame.me.player.SetParam(this.game_res, 1f);
    Debug.Log((object) $"#gevent# Started global event \"{this.game_res}\"");
  }

  public void OnFinishEvent()
  {
    this.on_finish_script?.Execute();
    MainGame.me.player.SetParam(this.game_res, 0.0f);
    Debug.Log((object) $"#gevent# Finished global event \"{this.game_res}\"");
  }

  public enum GlobalEventPeriodType
  {
    Years = 1,
  }
}
