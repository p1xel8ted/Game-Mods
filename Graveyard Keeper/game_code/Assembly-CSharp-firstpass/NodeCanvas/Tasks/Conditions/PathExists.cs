// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.PathExists
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#nullable disable
namespace NodeCanvas.Tasks.Conditions;

[Description("Check if a path exists for the agent and optionaly save the resulting path positions")]
[Category("Movement")]
public class PathExists : ConditionTask<NavMeshAgent>
{
  public BBParameter<Vector3> targetPosition;
  [BlackboardOnly]
  public BBParameter<List<Vector3>> savePathAs;

  public override bool OnCheck()
  {
    NavMeshPath path = new NavMeshPath();
    this.agent.CalculatePath(this.targetPosition.value, path);
    this.savePathAs.value = ((IEnumerable<Vector3>) path.corners).ToList<Vector3>();
    return path.status == NavMeshPathStatus.PathComplete;
  }
}
