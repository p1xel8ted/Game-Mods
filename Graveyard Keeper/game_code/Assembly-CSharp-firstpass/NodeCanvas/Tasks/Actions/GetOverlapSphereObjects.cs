// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetOverlapSphereObjects
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using LinqTools;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Description("Gets a lists of game objects that are in the physics overlap sphere at the position of the agent, excluding the agent")]
[Category("Physics")]
public class GetOverlapSphereObjects : ActionTask<Transform>
{
  public LayerMask layerMask = (LayerMask) -1;
  public BBParameter<float> radius = (BBParameter<float>) 2f;
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveObjectsAs;

  public override void OnExecute()
  {
    this.saveObjectsAs.value = ((IEnumerable<Collider>) Physics.OverlapSphere(this.agent.position, this.radius.value, (int) this.layerMask)).Select<Collider, GameObject>((Func<Collider, GameObject>) (c => c.gameObject)).ToList<GameObject>();
    this.saveObjectsAs.value.Remove(this.agent.gameObject);
    if (this.saveObjectsAs.value.Count == 0)
      this.EndAction(false);
    else
      this.EndAction(true);
  }

  public override void OnDrawGizmosSelected()
  {
    if (!((UnityEngine.Object) this.agent != (UnityEngine.Object) null))
      return;
    Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
    Gizmos.DrawSphere(this.agent.position, this.radius.value);
  }
}
