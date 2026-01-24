// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeaponInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class WeaponInfoCardController : UIInfoCardController<WeaponInfoCard, EquipmentType>
{
  public override bool IsSelectionValid(Selectable selectable, out EquipmentType showParam)
  {
    showParam = EquipmentType.Invalid;
    WeaponItem component;
    if (!selectable.TryGetComponent<WeaponItem>(out component))
      return false;
    showParam = component.Type;
    return true;
  }

  public override EquipmentType DefaultShowParam() => EquipmentType.Invalid;
}
