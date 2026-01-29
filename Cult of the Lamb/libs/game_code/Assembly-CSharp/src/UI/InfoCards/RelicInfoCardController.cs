// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.RelicInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using src.UI.Items;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class RelicInfoCardController : UIInfoCardController<RelicInfoCard, RelicData>
{
  public override bool IsSelectionValid(Selectable selectable, out RelicData showParam)
  {
    RelicItem component1;
    if (selectable.TryGetComponent<RelicItem>(out component1) && !component1.Locked)
    {
      showParam = component1.Data;
      return true;
    }
    RelicPlayerMenuItem component2;
    if (selectable.TryGetComponent<RelicPlayerMenuItem>(out component2))
    {
      showParam = component2.Data;
      return true;
    }
    ActiveRelicItem component3;
    if (selectable.TryGetComponent<ActiveRelicItem>(out component3))
    {
      showParam = component3.RelicData;
      return true;
    }
    showParam = (RelicData) null;
    return false;
  }

  public override RelicData DefaultShowParam() => ScriptableObject.CreateInstance<RelicData>();
}
