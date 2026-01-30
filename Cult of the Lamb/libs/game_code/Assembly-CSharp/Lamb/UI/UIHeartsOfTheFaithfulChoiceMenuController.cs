// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UIHeartsOfTheFaithfulChoiceMenuController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public class UIHeartsOfTheFaithfulChoiceMenuController : 
  UIChoiceMenuBase<UIHeartsOfFaithfulChoiceInfoCard, UIHeartsOfTheFaithfulChoiceMenuController.Types>
{
  public Action<UIHeartsOfTheFaithfulChoiceMenuController.Types> OnChoiceMade;

  public override void Configure()
  {
    this._infoBox1.Configure(UIHeartsOfTheFaithfulChoiceMenuController.Types.Hearts, new Vector2(300f, 0.0f), new Vector2(-200f, 0.0f));
    this._infoBox2.Configure(UIHeartsOfTheFaithfulChoiceMenuController.Types.Strength, new Vector2(-300f, 0.0f), new Vector2(200f, 0.0f));
  }

  public override void OnLeftChoice()
  {
    base.OnLeftChoice();
    UIManager.PlayAudio("event:/hearts_of_the_faithful/select_hearts");
  }

  public override void OnRightChoice()
  {
    base.OnRightChoice();
    UIManager.PlayAudio("event:/hearts_of_the_faithful/select_swords");
  }

  public override void MadeChoice(
    UIChoiceInfoCard<UIHeartsOfTheFaithfulChoiceMenuController.Types> infoCard)
  {
    Action<UIHeartsOfTheFaithfulChoiceMenuController.Types> onChoiceMade = this.OnChoiceMade;
    if (onChoiceMade == null)
      return;
    onChoiceMade(infoCard.Info);
  }

  public override void OnHideCompleted()
  {
    UIManager.PlayAudio("event:/hearts_of_the_faithful/increase_stat_text_appear");
  }

  public enum Types
  {
    Hearts,
    Strength,
  }
}
