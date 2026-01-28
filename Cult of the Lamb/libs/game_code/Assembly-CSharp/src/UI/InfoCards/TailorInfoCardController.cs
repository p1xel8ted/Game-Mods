// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.TailorInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class TailorInfoCardController : UIInfoCardController<TailorInfoCard, ClothingData>
{
  public override bool IsSelectionValid(Selectable selectable, out ClothingData showParam)
  {
    showParam = (ClothingData) null;
    TailorItem component1;
    if (selectable.TryGetComponent<TailorItem>(out component1))
    {
      showParam = component1.ClothingData;
      return true;
    }
    TailorActionButton component2;
    if (!selectable.TryGetComponent<TailorActionButton>(out component2))
      return false;
    showParam = component2.ClothingData;
    return true;
  }
}
