// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.FinishNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Category("Control")]
[Description("End the dialogue in Success or Failure.\nNote: A Dialogue will anyway End in Succcess if it has reached a node without child connections. Thus this node is mostly useful if you want to end a Dialogue in Failure.")]
[ParadoxNotion.Design.Icon("Halt", false, "")]
[Color("00b9e8")]
[Name("FINISH", 0)]
public class FinishNode : DTNode
{
  public CompactStatus finishState = CompactStatus.Success;

  public override int maxOutConnections => 0;

  public override bool requireActorSelection => false;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    this.status = (NodeCanvas.Status) this.finishState;
    this.DLGTree.Stop(this.finishState == CompactStatus.Success);
    return this.status;
  }
}
