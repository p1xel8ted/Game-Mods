// Decompiled with JetBrains decompiler
// Type: TraitManipulatorOptionInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using UnityEngine.UI;

#nullable disable
public class TraitManipulatorOptionInfoCardController : 
  UIInfoCardController<TraitManipulatorOptionInfoCard, UITraitManipulatorMenuController.Type>
{
  public override bool IsSelectionValid(
    Selectable selectable,
    out UITraitManipulatorMenuController.Type showParam)
  {
    showParam = UITraitManipulatorMenuController.Type.Shuffle;
    selectable.GetComponentInParent<UITraitManipulatorMenuController>();
    return false;
  }
}
