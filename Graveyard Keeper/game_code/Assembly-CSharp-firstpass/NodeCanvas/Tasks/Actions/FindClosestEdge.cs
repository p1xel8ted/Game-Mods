// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.FindClosestEdge
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using UnityEngine.AI;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Name("Find Closest NavMesh Edge", 0)]
[Category("Movement/Pathfinding")]
[Description("Find the closes Navigation Mesh position to the target position")]
public class FindClosestEdge : ActionTask
{
  public BBParameter<Vector3> targetPosition;
  [BlackboardOnly]
  public BBParameter<Vector3> saveFoundPosition;
  public NavMeshHit hit;

  public override void OnExecute()
  {
    if (NavMesh.FindClosestEdge(this.targetPosition.value, out this.hit, -1))
    {
      this.saveFoundPosition.value = this.hit.position;
      this.EndAction(true);
    }
    this.EndAction(false);
  }
}
