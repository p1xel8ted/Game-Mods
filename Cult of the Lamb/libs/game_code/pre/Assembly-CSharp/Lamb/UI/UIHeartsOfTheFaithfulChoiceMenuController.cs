// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHeartsOfTheFaithfulChoiceMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIHeartsOfTheFaithfulChoiceMenuController : 
  UIChoiceMenuBase<UIHeartsOfFaithfulChoiceInfoCard, UIHeartsOfTheFaithfulChoiceMenuController.Types>
{
  public Action<UIHeartsOfTheFaithfulChoiceMenuController.Types> OnChoiceMade;

  protected override void Configure()
  {
    this._infoBox1.Configure(UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts, new Vector2(300f, 0.0f), new Vector2(-200f, 0.0f));
    this._infoBox2.Configure(UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength, new Vector2(-300f, 0.0f), new Vector2(200f, 0.0f));
  }

  protected override void OnLeftChoice()
  {
    base.OnLeftChoice();
    UIManager.PlayAudio("event:/hearts_of_the_faithful/select_hearts");
  }

  protected override void OnRightChoice()
  {
    base.OnRightChoice();
    UIManager.PlayAudio("event:/hearts_of_the_faithful/select_swords");
  }

  protected override void MadeChoice(
    UIChoiceInfoCard<UIHeartsOfTheFaithfulChoiceMenuController.Types> infoCard)
  {
    Action<UIHeartsOfTheFaithfulChoiceMenuController.Types> onChoiceMade = this.OnChoiceMade;
    if (onChoiceMade == null)
      return;
    onChoiceMade(infoCard.Info);
  }

  protected override void OnHideCompleted()
  {
    UIManager.PlayAudio("event:/hearts_of_the_faithful/increase_stat_text_appear");
  }

  public enum Types
  {
    Hearts,
    Strength,
  }
}
