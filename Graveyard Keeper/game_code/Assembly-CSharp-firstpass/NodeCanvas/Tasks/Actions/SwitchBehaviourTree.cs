// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.SwitchBehaviourTree
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Switch the entire Behaviour Tree of BehaviourTreeOwner")]
[Category("✫ Utility")]
public class SwitchBehaviourTree : ActionTask<BehaviourTreeOwner>
{
  [RequiredField]
  public BBParameter<BehaviourTree> behaviourTree;

  public override string info => $"Switch Behaviour {this.behaviourTree}";

  public override void OnExecute()
  {
    this.agent.SwitchBehaviour(this.behaviourTree.value);
    this.EndAction();
  }
}
