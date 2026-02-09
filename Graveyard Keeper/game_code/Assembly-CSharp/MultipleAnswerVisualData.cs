// Decompiled with JetBrains decompiler
// Type: MultipleAnswerVisualData
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

#nullable disable
public class MultipleAnswerVisualData : AnswerVisualData
{
  public override bool IsDetailed()
  {
    bool flag = false;
    for (int index = 0; index < this.answer_visual_datas.Count; ++index)
    {
      if (!string.IsNullOrEmpty(this.answer_visual_datas[index].icon_price) || !string.IsNullOrEmpty(this.answer_visual_datas[index].icon_lock) || !string.IsNullOrEmpty(this.answer_visual_datas[index].icon_reward))
      {
        flag = true;
        break;
      }
    }
    return flag;
  }
}
