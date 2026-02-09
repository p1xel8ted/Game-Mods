// Decompiled with JetBrains decompiler
// Type: BuffsLogics
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public static class BuffsLogics
{
  public static float _time;

  public static void UpdateEveryFrame()
  {
    BuffsLogics._time += Time.deltaTime;
    if ((double) BuffsLogics._time <= 1.0)
      return;
    BuffsLogics._time = 0.0f;
    BuffsLogics.CheckBuffsGiveConditions();
  }

  public static void CheckBuffsGiveConditions()
  {
    if (!MainGame.game_started)
      return;
    foreach (BuffDefinition buff in GameBalance.me.buffs_data)
    {
      if (!buff.condition_player_res.IsEmpty())
      {
        bool flag = true;
        foreach (GameResAtom atom in buff.condition_player_res.ToAtomList())
        {
          flag = (double) MainGame.me.player.GetParam(atom.type) >= (double) atom.value;
          if (!flag)
            break;
        }
        if (flag)
          BuffsLogics.GiveBuffIfNotExists(buff);
      }
    }
  }

  public static void GiveBuffIfNotExists(BuffDefinition buff)
  {
    if (BuffsLogics.FindBuffByID(buff.id) != null)
      return;
    BuffsLogics.AddBuff(buff.id);
  }

  public static void AddBuff(string buff_id, float? length = null)
  {
    Debug.Log((object) ("AddBuff: " + buff_id));
    BuffDefinition data = GameBalance.me.GetData<BuffDefinition>(buff_id);
    if (data == null)
    {
      Debug.LogError((object) ("BuffDefinition not found, id = " + buff_id));
    }
    else
    {
      PlayerBuff buffById = BuffsLogics.FindBuffByID(buff_id);
      float num = (float) (((double) length ?? (double) data.length.EvaluateFloat()) / 450.0 * 60.0);
      if (buffById == null)
      {
        MainGame.me.save.buffs.Add(new PlayerBuff()
        {
          buff_id = buff_id,
          end_time = MainGame.game_time + num
        });
        MainGame.me.player.data.AddToParams(data.res);
        data.se_start.Evaluate();
      }
      else
      {
        switch (data.overlay_type)
        {
          case BuffDefinition.BuffOverlayType.Set:
            buffById.end_time = MainGame.game_time + num;
            break;
          case BuffDefinition.BuffOverlayType.Add:
            buffById.end_time += num;
            break;
        }
      }
      GUIElements.me.buffs.Redraw();
    }
  }

  public static void RemoveBuff(string buff_id)
  {
    Debug.Log((object) ("RemoveBuff: " + buff_id));
    PlayerBuff buffById = BuffsLogics.FindBuffByID(buff_id);
    if (buffById == null)
      return;
    MainGame.me.save.buffs.Remove(buffById);
    BuffDefinition data = GameBalance.me.GetData<BuffDefinition>(buff_id);
    MainGame.me.player.data.SubFromParams(data.res);
    data.se_finish.Evaluate();
    GUIElements.me.buffs.Redraw();
  }

  public static void RecalculateBuffs()
  {
    float deltaTime = Time.deltaTime;
    bool flag = false;
    for (int index = 0; index < MainGame.me.save.buffs.Count; ++index)
    {
      PlayerBuff buff = MainGame.me.save.buffs[index];
      buff.CustomUpdate(deltaTime);
      if ((double) buff.end_time <= (double) MainGame.game_time)
      {
        flag = true;
        BuffsLogics.RemoveBuff(buff.buff_id);
        --index;
      }
    }
    if (!flag)
      return;
    GUIElements.me.buffs.Redraw();
  }

  public static PlayerBuff FindBuffByID(string buff_id)
  {
    foreach (PlayerBuff buff in MainGame.me.save.buffs)
    {
      if (buff.buff_id == buff_id)
        return buff;
    }
    return (PlayerBuff) null;
  }
}
