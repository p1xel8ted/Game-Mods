// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNodeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class UpgradeTreeNodeInfoCardController : 
  UIInfoCardController<UpgradeTreeNodeInfoCard, UpgradeTreeNode>
{
  public override bool IsSelectionValid(Selectable selectable, out UpgradeTreeNode showParam)
  {
    return selectable.TryGetComponent<UpgradeTreeNode>(out showParam);
  }
}
