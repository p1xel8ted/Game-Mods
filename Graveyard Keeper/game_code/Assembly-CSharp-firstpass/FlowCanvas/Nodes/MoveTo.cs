// Decompiled with JetBrains decompiler
// Type: FlowCanvas.Nodes.MoveTo
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using ParadoxNotion.Design;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

#nullable disable
namespace FlowCanvas.Nodes;

[Category("Unity")]
[Description("Moves a NavMeshAgent object with pathfinding to target destination")]
public class MoveTo : LatentActionNode<NavMeshAgent, Vector3, float, float>
{
  public NavMeshAgent agent;

  public override IEnumerator Invoke(
    NavMeshAgent agent,
    Vector3 destination,
    float speed,
    float stoppingDistance)
  {
    this.agent = agent;
    agent.speed = speed;
    agent.stoppingDistance = stoppingDistance;
    if ((double) agent.speed > 0.0)
      agent.SetDestination(destination);
    else
      agent.Warp(destination);
    while (agent.pathPending || (double) agent.remainingDistance > (double) stoppingDistance)
      yield return (object) null;
  }

  public override void OnBreak() => this.agent.ResetPath();
}
