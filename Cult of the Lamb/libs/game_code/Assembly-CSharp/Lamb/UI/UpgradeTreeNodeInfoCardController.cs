// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNodeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
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
