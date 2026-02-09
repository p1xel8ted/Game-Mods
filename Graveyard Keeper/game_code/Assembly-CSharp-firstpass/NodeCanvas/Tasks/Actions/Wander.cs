// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Wander
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Movement/Pathfinding")]
[Description("Makes the agent wander randomly within the navigation map")]
public class Wander : ActionTask<NavMeshAgent>
{
  public BBParameter<float> speed = (BBParameter<float>) 4f;
  public BBParameter<float> keepDistance = (BBParameter<float>) 0.1f;
  public BBParameter<float> minWanderDistance = (BBParameter<float>) 5f;
  public BBParameter<float> maxWanderDistance = (BBParameter<float>) 20f;
  public bool repeat = true;

  public override void OnExecute()
  {
    this.agent.speed = this.speed.value;
    this.DoWander();
  }

  public override void OnUpdate()
  {
    if (this.agent.pathPending || (double) this.agent.remainingDistance > (double) this.agent.stoppingDistance + (double) this.keepDistance.value)
      return;
    if (this.repeat)
      this.DoWander();
    else
      this.EndAction();
  }

  public void DoWander()
  {
    float num1 = this.minWanderDistance.value;
    float max = this.maxWanderDistance.value;
    float min = Mathf.Clamp(num1, 0.01f, max);
    float num2 = Mathf.Clamp(max, min, max);
    Vector3 target = this.agent.transform.position;
    while ((double) (target - this.agent.transform.position).sqrMagnitude < (double) min)
      target = Random.insideUnitSphere * num2 + this.agent.transform.position;
    if (this.agent.SetDestination(target))
      return;
    this.DoWander();
  }

  public override void OnPause() => this.OnStop();

  public override void OnStop()
  {
    if (!this.agent.gameObject.activeSelf)
      return;
    this.agent.ResetPath();
  }
}
