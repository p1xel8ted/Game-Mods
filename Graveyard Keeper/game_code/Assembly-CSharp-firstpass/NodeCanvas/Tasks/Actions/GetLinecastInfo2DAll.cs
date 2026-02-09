// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetLinecastInfo2DAll
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

[Category("Physics")]
[Description("Get hit info for ALL objects in the linecast, in Lists")]
public class GetLinecastInfo2DAll : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public LayerMask mask = (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<List<GameObject>> saveHitGameObjectsAs;
  [BlackboardOnly]
  public BBParameter<List<float>> saveDistancesAs;
  [BlackboardOnly]
  public BBParameter<List<Vector3>> savePointsAs;
  [BlackboardOnly]
  public BBParameter<List<Vector3>> saveNormalsAs;
  public RaycastHit2D[] hits;

  public override void OnExecute()
  {
    this.hits = Physics2D.LinecastAll((Vector2) this.agent.position, (Vector2) this.target.value.transform.position, (int) this.mask);
    if (this.hits.Length != 0)
    {
      this.saveHitGameObjectsAs.value = ((IEnumerable<RaycastHit2D>) this.hits).Select<RaycastHit2D, GameObject>((Func<RaycastHit2D, GameObject>) (h => h.collider.gameObject)).ToList<GameObject>();
      this.saveDistancesAs.value = ((IEnumerable<RaycastHit2D>) this.hits).Select<RaycastHit2D, float>((Func<RaycastHit2D, float>) (h => h.fraction)).ToList<float>();
      this.savePointsAs.value = ((IEnumerable<RaycastHit2D>) this.hits).Select<RaycastHit2D, Vector2>((Func<RaycastHit2D, Vector2>) (h => h.point)).Cast<Vector3>().ToList<Vector3>();
      this.saveNormalsAs.value = ((IEnumerable<RaycastHit2D>) this.hits).Select<RaycastHit2D, Vector2>((Func<RaycastHit2D, Vector2>) (h => h.normal)).Cast<Vector3>().ToList<Vector3>();
      this.EndAction(true);
    }
    else
      this.EndAction(false);
  }

  public override void OnDrawGizmosSelected()
  {
    if (!(bool) (UnityEngine.Object) this.agent || !(bool) (UnityEngine.Object) this.target.value)
      return;
    Gizmos.DrawLine(this.agent.position, this.target.value.transform.position);
  }
}
