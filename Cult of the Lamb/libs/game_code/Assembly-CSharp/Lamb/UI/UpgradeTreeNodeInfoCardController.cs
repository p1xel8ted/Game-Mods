// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNodeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
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
