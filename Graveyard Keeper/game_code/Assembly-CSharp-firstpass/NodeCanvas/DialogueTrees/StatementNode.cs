// Decompiled with JetBrains decompiler
// Type: NodeCanvas.DialogueTrees.StatementNode
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

#nullable disable
namespace NodeCanvas.DialogueTrees;

[Name("Say", 0)]
[Description("Make the selected Dialogue Actor talk. You can make the text more dynamic by using variable names in square brackets\ne.g. [myVarName] or [Global/myVarName]")]
public class StatementNode : DTNode
{
  [SerializeField]
  public Statement statement = new Statement("This is a dialogue text");

  public override bool requireActorSelection => true;

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard bb)
  {
    DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(this.finalActor, (IStatement) this.statement.BlackboardReplace(bb), new Action(this.OnStatementFinish)));
    return NodeCanvas.Status.Running;
  }

  public void OnStatementFinish()
  {
    this.status = NodeCanvas.Status.Success;
    this.DLGTree.Continue();
  }
}
