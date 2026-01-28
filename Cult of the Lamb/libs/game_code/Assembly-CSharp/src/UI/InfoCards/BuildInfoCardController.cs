// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.BuildInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using Lamb.UI.BuildMenu;
using UnityEngine.UI;

#nullable disable
namespace src.UI.InfoCards;

public class BuildInfoCardController : UIInfoCardController<BuildInfoCard, StructureBrain.TYPES>
{
  public override bool IsSelectionValid(Selectable selectable, out StructureBrain.TYPES showParam)
  {
    showParam = StructureBrain.TYPES.NONE;
    BuildMenuItem component;
    if (!selectable.TryGetComponent<BuildMenuItem>(out component) || component.Locked)
      return false;
    showParam = component.Structure;
    return true;
  }

  public override StructureBrain.TYPES DefaultShowParam() => StructureBrain.TYPES.NONE;
}
