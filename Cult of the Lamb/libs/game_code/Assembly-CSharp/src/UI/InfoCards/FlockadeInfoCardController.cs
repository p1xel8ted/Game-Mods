// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.FlockadeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using Lamb.UI;
using src.UI.Items;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class FlockadeInfoCardController : 
  UIInfoCardController<FlockadePieceInfoCard, FlockadeGamePieceConfiguration>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out FlockadeGamePieceConfiguration showParam)
  {
    FlockadePieceItem component;
    if (selectable.TryGetComponent<FlockadePieceItem>(out component) && !component.Locked)
    {
      showParam = component.Data;
      return true;
    }
    showParam = (FlockadeGamePieceConfiguration) null;
    return false;
  }

  public override FlockadeGamePieceConfiguration DefaultShowParam()
  {
    return ScriptableObject.CreateInstance<FlockadeGamePieceConfiguration>();
  }
}
