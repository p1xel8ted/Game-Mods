// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.StartDialogueTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.DialogueTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Runtime.CompilerServices;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Starts the Dialogue Tree assigned on a Dialogue Tree Controller object with specified agent used for 'Instigator'.")]
[Icon("Dialogue", false, "")]
[Category("Dialogue")]
public class StartDialogueTree : ActionTask<IDialogueActor>
{
  [RequiredField]
  public BBParameter<DialogueTreeController> dialogueTreeController;
  public bool waitActionFinish = true;

  public override string info => $"Start Dialogue {this.dialogueTreeController}";

  public override void OnExecute()
  {
    if (this.waitActionFinish)
    {
      this.dialogueTreeController.value.StartDialogue(this.agent, (Action<bool>) (success => this.EndAction(success)));
    }
    else
    {
      this.dialogueTreeController.value.StartDialogue(this.agent);
      this.EndAction();
    }
  }

  [CompilerGenerated]
  public void \u003COnExecute\u003Eb__4_0(bool success) => this.EndAction(success);
}
