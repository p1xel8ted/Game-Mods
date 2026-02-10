// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TutorialInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.Items;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class TutorialInfoCardController : UIInfoCardController<TutorialInfoCard, TutorialTopic>
{
  public override bool IsSelectionValid(Selectable selectable, out TutorialTopic showParam)
  {
    showParam = TutorialTopic.None;
    TutorialMenuItem component;
    if (!selectable.TryGetComponent<TutorialMenuItem>(out component))
      return false;
    showParam = component.Topic;
    return true;
  }

  public override TutorialTopic DefaultShowParam() => TutorialTopic.None;
}
