// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SayRandom
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("A random statement will be chosen each time for the actor to say")]
[ParadoxNotion.Design.Icon("Dialogue", false, "")]
[Category("Dialogue")]
public class SayRandom : ActionTask<IDialogueActor>
{
  public List<Statement> statements = new List<Statement>();

  public override void OnExecute()
  {
    DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(this.agent, (IStatement) this.statements[UnityEngine.Random.Range(0, this.statements.Count)].BlackboardReplace(this.blackboard), new Action(((ActionTask) this).EndAction)));
  }
}
