// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SwitchFSM
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("✫ Utility")]
[Description("Switch the entire FSM of FSMTreeOwner")]
public class SwitchFSM : ActionTask<FSMOwner>
{
  [RequiredField]
  public BBParameter<FSM> fsm;

  public override string info => $"Switch FSM {this.fsm}";

  public override void OnExecute()
  {
    this.agent.SwitchBehaviour(this.fsm.value);
    this.EndAction();
  }
}
