// Decompiled with JetBrains decompiler
// Type: TraitManipulatorOptionInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
