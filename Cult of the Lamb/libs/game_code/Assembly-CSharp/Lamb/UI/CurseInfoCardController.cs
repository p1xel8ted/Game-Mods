// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CurseInfoCardController : UIInfoCardController<CurseInfoCard, EquipmentType>
{
  public override bool IsSelectionValid(Selectable selectable, out EquipmentType showParam)
  {
    showParam = EquipmentType.Invalid;
    CurseItem component;
    if (!selectable.TryGetComponent<CurseItem>(out component))
      return false;
    showParam = component.Type;
    return true;
  }

  public override EquipmentType DefaultShowParam() => EquipmentType.Invalid;
}
