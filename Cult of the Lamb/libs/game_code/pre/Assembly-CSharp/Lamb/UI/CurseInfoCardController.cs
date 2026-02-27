// Decompiled with JetBrains decompiler
// Type: Lamb.UI.CurseInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class CurseInfoCardController : UIInfoCardController<CurseInfoCard, EquipmentType>
{
  protected override bool IsSelectionValid(Selectable selectable, out EquipmentType showParam)
  {
    showParam = EquipmentType.Invalid;
    CurseItem component;
    if (!selectable.TryGetComponent<CurseItem>(out component))
      return false;
    showParam = component.Type;
    return true;
  }

  protected override EquipmentType DefaultShowParam() => EquipmentType.Invalid;
}
