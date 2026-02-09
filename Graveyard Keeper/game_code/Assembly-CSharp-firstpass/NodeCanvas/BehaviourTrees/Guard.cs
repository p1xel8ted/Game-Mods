// Decompiled with JetBrains decompiler
// Type: NodeCanvas.BehaviourTrees.Guard
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.BehaviourTrees;

[Name("Guard", 0)]
[Category("Decorators")]
[ParadoxNotion.Design.Icon("Shield", false, "")]
[Description("Protect the decorated child from running if another Guard with the same token is already guarding (Running) that token.\nGuarding is global for all of the agent's Behaviour Trees.")]
public class Guard : BTDecorator
{
  public BBParameter<string> token;
  public Guard.GuardMode ifGuarded;
  public bool isGuarding;
  public static Dictionary<GameObject, List<Guard>> guards = new Dictionary<GameObject, List<Guard>>();

  public static List<Guard> AgentGuards(Component agent) => Guard.guards[agent.gameObject];

  public override void OnGraphStarted() => this.SetGuards(this.graphAgent);

  public override NodeCanvas.Status OnExecute(Component agent, IBlackboard blackboard)
  {
    if (this.decoratedConnection == null)
      return NodeCanvas.Status.Failure;
    if ((Object) agent != (Object) this.graphAgent)
      this.SetGuards(agent);
    for (int index = 0; index < Guard.AgentGuards(agent).Count; ++index)
    {
      Guard agentGuard = Guard.AgentGuards(agent)[index];
      if (agentGuard != this && agentGuard.isGuarding && agentGuard.token.value == this.token.value)
        return this.ifGuarded != Guard.GuardMode.ReturnFailure ? NodeCanvas.Status.Running : NodeCanvas.Status.Failure;
    }
    this.status = this.decoratedConnection.Execute(agent, blackboard);
    if (this.status == NodeCanvas.Status.Running)
    {
      this.isGuarding = true;
      return NodeCanvas.Status.Running;
    }
    this.isGuarding = false;
    return this.status;
  }

  public override void OnReset() => this.isGuarding = false;

  public void SetGuards(Component guardAgent)
  {
    if (!Guard.guards.ContainsKey(guardAgent.gameObject))
      Guard.guards[guardAgent.gameObject] = new List<Guard>();
    if (Guard.AgentGuards(guardAgent).Contains(this) || string.IsNullOrEmpty(this.token.value))
      return;
    Guard.AgentGuards(guardAgent).Add(this);
  }

  public enum GuardMode
  {
    ReturnFailure,
    WaitUntilReleased,
  }
}
