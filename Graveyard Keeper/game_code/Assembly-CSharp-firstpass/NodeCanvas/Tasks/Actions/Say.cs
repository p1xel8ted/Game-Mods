// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Say
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Dialogue")]
[Description("You can use a variable inline with the text by using brackets likeso: [myVarName] or [Global/myVarName].\nThe bracket will be replaced with the variable value ToString")]
[Icon("Dialogue", false, "")]
public class Say : ActionTask<IDialogueActor>
{
  public Statement statement = new Statement("This is a dialogue text...");

  public override string info
  {
    get
    {
      return $"<i>' {(this.statement.text.Length > 30 ? (object) (this.statement.text.Substring(0, 30) + "...") : (object) this.statement.text)} '</i>";
    }
  }

  public override void OnExecute()
  {
    DialogueTree.RequestSubtitles(new SubtitlesRequestInfo(this.agent, (IStatement) this.statement.BlackboardReplace(this.blackboard), new Action(((ActionTask) this).EndAction)));
  }
}
