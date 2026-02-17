// Decompiled with JetBrains decompiler
// Type: Lamb.UI.WeaponInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
