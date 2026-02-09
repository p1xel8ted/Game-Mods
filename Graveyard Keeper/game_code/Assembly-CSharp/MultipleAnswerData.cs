// Decompiled with JetBrains decompiler
// Type: MultipleAnswerData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class MultipleAnswerData : AnswerData
{
  public MultipleAnswerData()
  {
  }

  public MultipleAnswerData(SmartRes reward, List<AnswerData> datas)
  {
    this.d_reward = reward;
    this.datas = datas;
  }

  public void FillVisualData(ref MultipleAnswerVisualData vis_data, WorldGameObject linked_wgo = null)
  {
    bool flag1 = true;
    bool flag2 = true;
    if (this.datas != null)
    {
      for (int index = 0; index < this.datas.Count; ++index)
      {
        AnswerVisualData answerVisualData = new AnswerVisualData();
        vis_data.answer_visual_datas.Add(answerVisualData);
        this.datas[index].d_lock.FillVisualData(out answerVisualData.icon_lock, out answerVisualData.n_lock, out answerVisualData.icon_lock_quality, linked_wgo);
        this.datas[index].d_price.FillVisualData(out answerVisualData.icon_price, out answerVisualData.n_price, out answerVisualData.icon_price_quality, linked_wgo);
        answerVisualData.link_to_answer_data = this.datas[index];
        if (!MainGame.me.player.IsEnough(this.datas[index].d_lock))
          flag2 = false;
        else if (!MainGame.me.player.IsEnough(this.datas[index].d_price))
          flag1 = false;
      }
    }
    if (this.d_reward != null)
      this.d_reward.FillVisualData(out vis_data.icon_reward, out vis_data.n_reward, out vis_data.icon_reward_quality, linked_wgo);
    if (!flag2 || !flag1)
    {
      vis_data.inside_price_is_red = true;
      vis_data.can_be_picked = false;
      if (this.d_price != null && this.d_price.res_type == SmartRes.ResType.Item)
        vis_data.price_txt = $"{MainGame.me.player.data.GetTotalCount(this.d_price.item.id).ToString()}/{vis_data.n_price.ToString()}";
    }
    vis_data.link_to_answer_data = (AnswerData) this;
  }
}
