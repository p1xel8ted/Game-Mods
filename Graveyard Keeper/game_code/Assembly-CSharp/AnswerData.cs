// Decompiled with JetBrains decompiler
// Type: AnswerData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

#nullable disable
[Serializable]
public class AnswerData
{
  public SmartRes d_lock;
  public SmartRes d_price;
  public SmartRes d_reward;
  public List<AnswerData> datas;

  public virtual void FillVisualData(ref AnswerVisualData vis_data, WorldGameObject linked_wgo = null)
  {
    if (this.d_price != null)
      this.d_price.FillVisualData(out vis_data.icon_price, out vis_data.n_price, out vis_data.icon_price_quality, linked_wgo);
    if (this.d_lock != null)
      this.d_lock.FillVisualData(out vis_data.icon_lock, out vis_data.n_lock, out vis_data.icon_lock_quality, linked_wgo);
    if (this.d_reward != null)
      this.d_reward.FillVisualData(out vis_data.icon_reward, out vis_data.n_reward, out vis_data.icon_reward_quality, linked_wgo);
    SmartRes dPrice = this.d_price;
    if (!MainGame.me.player.IsEnough(this.d_price) || !MainGame.me.player.IsEnough(this.d_lock))
    {
      vis_data.inside_price_is_red = true;
      vis_data.can_be_picked = false;
      if (this.d_price != null && this.d_price.res_type == SmartRes.ResType.Item)
        vis_data.price_txt = $"{MainGame.me.player.data.GetTotalCount(this.d_price.item.id).ToString()}/{vis_data.n_price.ToString()}";
    }
    vis_data.link_to_answer_data = this;
  }

  public virtual bool HasAnyReward()
  {
    return this.d_reward != null && this.d_reward.res_type != SmartRes.ResType.Empty;
  }
}
