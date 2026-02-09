// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Conditions.CheckLOS2D
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
namespace NodeCanvas.Tasks.Conditions;

[Category("GameObject")]
[Description("Check of agent is in line of sight with target by doing a linecast and optionaly save the distance")]
[Name("Target In Line Of Sight 2D", 0)]
public class CheckLOS2D : ConditionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> LOSTarget;
  public BBParameter<LayerMask> layerMask = (BBParameter<LayerMask>) (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [Task.GetFromAgent]
  public Collider2D agentCollider;
  public RaycastHit2D[] hits;

  public override string info => "LOS with " + this.LOSTarget.ToString();

  public override bool OnCheck()
  {
    this.hits = Physics2D.LinecastAll((Vector2) this.agent.position, (Vector2) this.LOSTarget.value.transform.position, (int) this.layerMask.value);
    foreach (Collider2D collider2D in ((IEnumerable<RaycastHit2D>) this.hits).Select<RaycastHit2D, Collider2D>((Func<RaycastHit2D, Collider2D>) (h => h.collider)))
    {
      if ((UnityEngine.Object) collider2D != (UnityEngine.Object) this.agentCollider && (UnityEngine.Object) collider2D != (UnityEngine.Object) this.LOSTarget.value.GetComponent<Collider2D>())
        return false;
    }
    return true;
  }

  public override void OnDrawGizmosSelected()
  {
    if (!(bool) (UnityEngine.Object) this.agent || !(bool) (UnityEngine.Object) this.LOSTarget.value)
      return;
    Gizmos.DrawLine(this.agent.position, this.LOSTarget.value.transform.position);
  }
}
