// Decompiled with JetBrains decompiler
// Type: AnswerVisualData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;

#nullable disable
public class AnswerVisualData
{
  public string id = "";
  public string icon_price;
  public string icon_lock;
  public string icon_reward;
  public string translation;
  public bool can_be_picked = true;
  public bool inside_price_is_red;
  public string price_txt = "";
  public string icon_price_quality;
  public string icon_reward_quality;
  public string icon_lock_quality;
  public int n_price;
  public int n_reward;
  public int n_lock;
  public AnswerData link_to_answer_data;
  public List<AnswerVisualData> answer_visual_datas = new List<AnswerVisualData>();

  public virtual bool IsDetailed()
  {
    return !string.IsNullOrEmpty(this.icon_price) || !string.IsNullOrEmpty(this.icon_lock) || !string.IsNullOrEmpty(this.icon_reward);
  }
}
