// Decompiled with JetBrains decompiler
// Type: Lamb.UI.UpgradeTreeNodeInfoCardController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
