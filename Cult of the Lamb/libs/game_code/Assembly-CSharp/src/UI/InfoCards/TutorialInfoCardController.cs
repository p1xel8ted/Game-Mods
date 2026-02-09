// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TutorialInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
