// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Flee
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
[Description("Flees away from the target")]
public class Flee : ActionTask<NavMeshAgent>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public BBParameter<float> speed = (BBParameter<float>) 4f;
  public BBParameter<float> fledDistance = (BBParameter<float>) 10f;
  public BBParameter<float> lookAhead = (BBParameter<float>) 2f;

  public override string info => $"Flee from {this.target}";

  public override void OnExecute()
  {
    this.agent.speed = this.speed.value;
    if ((double) (this.agent.transform.position - this.target.value.transform.position).magnitude < (double) this.fledDistance.value)
      return;
    this.EndAction(true);
  }

  public override void OnUpdate()
  {
    Vector3 position = this.target.value.transform.position;
    if ((double) (this.agent.transform.position - position).magnitude >= (double) this.fledDistance.value)
    {
      this.EndAction(true);
    }
    else
    {
      if (this.agent.SetDestination(position + (this.agent.transform.position - position).normalized * (this.fledDistance.value + this.lookAhead.value + this.agent.stoppingDistance)))
        return;
      this.EndAction(false);
    }
  }

  public override void OnPause() => this.OnStop();

  public override void OnStop()
  {
    if (!this.agent.gameObject.activeSelf)
      return;
    this.agent.ResetPath();
  }
}
