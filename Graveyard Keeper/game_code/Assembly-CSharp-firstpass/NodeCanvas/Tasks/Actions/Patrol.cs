// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.Patrol
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Move Randomly or Progressively between various game object positions taken from the list provided")]
[Category("Movement/Pathfinding")]
public class Patrol : ActionTask<NavMeshAgent>
{
  [RequiredField]
  public BBParameter<List<GameObject>> targetList;
  public BBParameter<Patrol.PatrolMode> patrolMode = (BBParameter<Patrol.PatrolMode>) Patrol.PatrolMode.Random;
  public BBParameter<float> speed = (BBParameter<float>) 4f;
  public float keepDistance = 0.1f;
  public int index = -1;
  public Vector3? lastRequest;

  public override string info => $"{this.patrolMode} Patrol {this.targetList}";

  public override void OnExecute()
  {
    if (this.targetList.value.Count == 0)
    {
      this.EndAction(false);
    }
    else
    {
      if (this.targetList.value.Count == 1)
      {
        this.index = 0;
      }
      else
      {
        if (this.patrolMode.value == Patrol.PatrolMode.Random)
        {
          int num = this.index;
          while (num == this.index)
            num = Random.Range(0, this.targetList.value.Count);
          this.index = num;
        }
        if (this.patrolMode.value == Patrol.PatrolMode.Progressive)
          this.index = (int) Mathf.Repeat((float) (this.index + 1), (float) this.targetList.value.Count);
      }
      GameObject gameObject = this.targetList.value[this.index];
      if ((Object) gameObject == (Object) null)
      {
        Debug.LogWarning((object) "List's game object is null on MoveToFromList Action");
        this.EndAction(false);
      }
      else
      {
        Vector3 position = gameObject.transform.position;
        this.agent.speed = this.speed.value;
        if ((double) (this.agent.transform.position - position).magnitude >= (double) this.agent.stoppingDistance + (double) this.keepDistance)
          return;
        this.EndAction(true);
      }
    }
  }

  public override void OnUpdate()
  {
    Vector3 position = this.targetList.value[this.index].transform.position;
    Vector3? lastRequest = this.lastRequest;
    Vector3 vector3 = position;
    if ((lastRequest.HasValue ? (lastRequest.HasValue ? (lastRequest.GetValueOrDefault() != vector3 ? 1 : 0) : 0) : 1) != 0 && !this.agent.SetDestination(position))
    {
      this.EndAction(false);
    }
    else
    {
      this.lastRequest = new Vector3?(position);
      if (this.agent.pathPending || (double) this.agent.remainingDistance > (double) this.agent.stoppingDistance + (double) this.keepDistance)
        return;
      this.EndAction(true);
    }
  }

  public override void OnPause() => this.OnStop();

  public override void OnStop()
  {
    if (this.lastRequest.HasValue && this.agent.gameObject.activeSelf)
      this.agent.ResetPath();
    this.lastRequest = new Vector3?();
  }

  public override void OnDrawGizmosSelected()
  {
    if (!(bool) (Object) this.agent || this.targetList.value == null)
      return;
    foreach (GameObject gameObject in this.targetList.value)
    {
      if ((Object) gameObject != (Object) null)
        Gizmos.DrawSphere(gameObject.transform.position, 0.1f);
    }
  }

  public enum PatrolMode
  {
    Progressive,
    Random,
  }
}
