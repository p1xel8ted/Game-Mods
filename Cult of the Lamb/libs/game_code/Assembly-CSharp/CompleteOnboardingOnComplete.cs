// Decompiled with JetBrains decompiler
// Type: CompleteOnboardingOnComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Lamb.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
public class CompleteOnboardingOnComplete : DungeonWorldMapIconEventListener
{
  public override async System.Threading.Tasks.Task AfterStateChange(
    UIDLCMapMenuController mapController,
    DungeonWorldMapIcon node,
    DungeonWorldMapIcon.IconState previousState)
  {
    await this.\u003C\u003En__0(mapController, node, previousState);
    if (node.CurrentState != DungeonWorldMapIcon.IconState.Completed)
      return;
    DataManager.Instance.DLCDungeonNodesCompleted.Clear();
    await mapController.RunDoorwayTransitionAsync(node, mapController.HomeLocation, UIDLCMapMenuController.DLCDungeonSide.OutsideMountain);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public System.Threading.Tasks.Task \u003C\u003En__0(
    UIDLCMapMenuController mapController,
    DungeonWorldMapIcon node,
    DungeonWorldMapIcon.IconState previousState)
  {
    return base.AfterStateChange(mapController, node, previousState);
  }
}
