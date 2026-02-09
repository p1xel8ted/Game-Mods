// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.ForceFinishGraph
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Description("Force Finish the current graph this Task is assigned to.")]
public class ForceFinishGraph : ActionTask
{
  public CompactStatus finishStatus = CompactStatus.Success;

  public override void OnExecute()
  {
    Graph ownerSystem = this.ownerSystem as Graph;
    if ((Object) ownerSystem != (Object) null)
      ownerSystem.Stop(this.finishStatus == CompactStatus.Success);
    this.EndAction((Object) ownerSystem != (Object) null);
  }
}
