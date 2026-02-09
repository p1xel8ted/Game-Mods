// Decompiled with JetBrains decompiler
// Type: NodeCanvas.Tasks.Actions.GetLinecastInfo2D
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

#nullable disable
namespace NodeCanvas.Tasks.Actions;

[Category("Physics")]
public class GetLinecastInfo2D : ActionTask<Transform>
{
  [RequiredField]
  public BBParameter<GameObject> target;
  public LayerMask mask = (LayerMask) -1;
  [BlackboardOnly]
  public BBParameter<GameObject> saveHitGameObjectAs;
  [BlackboardOnly]
  public BBParameter<float> saveDistanceAs;
  [BlackboardOnly]
  public BBParameter<Vector3> savePointAs;
  [BlackboardOnly]
  public BBParameter<Vector3> saveNormalAs;
  public RaycastHit2D hit;

  public override void OnExecute()
  {
    this.hit = Physics2D.Linecast((Vector2) this.agent.position, (Vector2) this.target.value.transform.position, (int) this.mask);
    if ((Object) this.hit.collider != (Object) null)
    {
      this.saveHitGameObjectAs.value = this.hit.collider.gameObject;
      this.saveDistanceAs.value = this.hit.fraction;
      this.savePointAs.value = (Vector3) this.hit.point;
      this.saveNormalAs.value = (Vector3) this.hit.normal;
      this.EndAction(true);
    }
    else
      this.EndAction(false);
  }

  public override void OnDrawGizmosSelected()
  {
    if (!(bool) (Object) this.agent || !(bool) (Object) this.target.value)
      return;
    Gizmos.DrawLine(this.agent.position, this.target.value.transform.position);
  }
}
