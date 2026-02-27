// Decompiled with JetBrains decompiler
// Type: src.UI.InfoCards.BuildInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
