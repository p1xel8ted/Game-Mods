// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.MoveToPosition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Seek (Vector3)", 0)]
[Category("Movement/Pathfinding")]
public class MoveToPosition : ActionTask<NavMeshAgent>
{
  public BBParameter<Vector3> targetPosition;
  public BBParameter<float> speed = (BBParameter<float>) 4f;
  public float keepDistance = 0.1f;
  public Vector3? lastRequest;

  public override string info => "Seek " + this.targetPosition?.ToString();

  public override void OnExecute()
  {
    this.agent.speed = this.speed.value;
    if ((double) Vector3.Distance(this.agent.transform.position, this.targetPosition.value) >= (double) this.agent.stoppingDistance + (double) this.keepDistance)
      return;
    this.EndAction(true);
  }

  public override void OnUpdate()
  {
    Vector3? lastRequest = this.lastRequest;
    Vector3 vector3 = this.targetPosition.value;
    if ((lastRequest.HasValue ? (lastRequest.HasValue ? (lastRequest.GetValueOrDefault() != vector3 ? 1 : 0) : 0) : 1) != 0 && !this.agent.SetDestination(this.targetPosition.value))
    {
      this.EndAction(false);
    }
    else
    {
      this.lastRequest = new Vector3?(this.targetPosition.value);
      if (this.agent.pathPending || (double) this.agent.remainingDistance > (double) this.agent.stoppingDistance + (double) this.keepDistance)
        return;
      this.EndAction(true);
    }
  }

  public override void OnStop()
  {
    if (this.lastRequest.HasValue && this.agent.gameObject.activeSelf)
      this.agent.ResetPath();
    this.lastRequest = new Vector3?();
  }

  public override void OnPause() => this.OnStop();
}
