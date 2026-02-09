// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.GoToNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Obsolete("Use Jumpers instead")]
[Description("Jump to another Dialogue node. Usefull if that other node is far away to connect, but otherwise it's exactly the same.\n\nPlease enable 'Show Node IDs' in Editor Prefs for convenience")]
[ParadoxNotion.Design.Icon("Set", false, "")]
[Category("Control")]
[Name("GO TO", 0)]
[Color("00b9e8")]
public class GoToNode : DTNode
{
  [SerializeField]
  public DTNode _targetNode;

  public override int maxOutConnections => 0;

  public override bool requireActorSelection => false;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    if (this._targetNode == null)
      return this.Error("Target node of GOTO node is null");
    this.DLGTree.EnterNode(this._targetNode);
    return NodeCanvas.Status.Success;
  }
}
