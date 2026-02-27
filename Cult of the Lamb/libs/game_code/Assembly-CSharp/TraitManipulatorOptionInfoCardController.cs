// Decompiled with JetBrains decompiler
// Type: TraitManipulatorOptionInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
