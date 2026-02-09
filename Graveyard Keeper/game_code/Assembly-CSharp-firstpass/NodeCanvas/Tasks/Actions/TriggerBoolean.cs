// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.TriggerBoolean
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Triggers a boolean variable for 1 frame to True then back to False")]
[Category("✫ Blackboard")]
public class TriggerBoolean : ActionTask
{
  [RequiredField]
  [BlackboardOnly]
  public BBParameter<bool> variable;

  public override string info => $"Trigger {this.variable}";

  public override void OnExecute()
  {
    if (!this.variable.value)
    {
      this.variable.value = true;
      this.StartCoroutine(this.Flip());
    }
    this.EndAction();
  }

  public IEnumerator Flip()
  {
    yield return (object) null;
    this.variable.value = false;
  }
}
