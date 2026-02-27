// Decompiled with JetBrains decompiler
// Type: Lamb.UI.SacrificeFollowerInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class SacrificeFollowerInfoCardController : 
  UIInfoCardController<SacrificeFollowerInfoCard, FollowerInfo>
{
  protected override bool IsSelectionValid(Selectable selectable, out FollowerInfo showParam)
  {
    showParam = (FollowerInfo) null;
    FollowerInformationBox component;
    if (!selectable.TryGetComponent<FollowerInformationBox>(out component))
      return false;
    showParam = component.FollowerInfo;
    return true;
  }
}
