// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.BuildInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.BuildMenu;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class BuildInfoCardController : UIInfoCardController<BuildInfoCard, StructureBrain.TYPES>
{
  protected override bool IsSelectionValid(
    Selectable selectable,
    out StructureBrain.TYPES showParam)
  {
    showParam = StructureBrain.TYPES.NONE;
    BuildMenuItem component;
    if (!selectable.TryGetComponent<BuildMenuItem>(out component) || component.Locked)
      return false;
    showParam = component.Structure;
    return true;
  }

  protected override StructureBrain.TYPES DefaultShowParam() => StructureBrain.TYPES.NONE;
}
